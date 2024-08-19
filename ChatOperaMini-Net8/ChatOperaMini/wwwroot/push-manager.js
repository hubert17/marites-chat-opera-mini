let deferredPrompt;
let isSubscribed = false;

// Check if Service Worker, Push, and Notifications APIs are available
if ('serviceWorker' in navigator && 'PushManager' in window) {
    navigator.serviceWorker.register('/service-worker.js')
        .then(function (registration) {
            console.log('Service Worker registered with scope:', registration.scope);

            return registration.pushManager.getSubscription();
        })
        .then(function (subscription) {
            if (subscription || Notification.permission === 'granted' || Notification.permission === 'denied') {
                console.log('User is already subscribed or notifications are granted:', subscription);
                isSubscribed = true;

                if (isPWAInstalled()) {
                    const topContainer = document.getElementById('top-container');
                    topContainer.style.display = 'none';
                }
            } else {
                showEnableNotificationButton(); // Show button if not subscribed and notifications not granted
            }
        })
        .catch(function (err) {
            console.log('Failed to check subscription status: ', err);
        });

            // Listen for the `beforeinstallprompt` event
    window.addEventListener('beforeinstallprompt', (e) => {
        e.preventDefault();
        deferredPrompt = e;

        if (!isPWAInstalled()) {
            showInstallNotificationPrompt(); // Show the install prompt if PWA is not installed
        }
    });
}

// Show the enable notification button
function showEnableNotificationButton() {
    const enableNotificationsButton = document.getElementById('enable-notifications-button');
    enableNotificationsButton.style.display = 'block';

    const promptMessage = document.getElementById('prompt-message');
    promptMessage.style.display = 'block';

    enableNotificationsButton.addEventListener('click', () => {
        askForNotificationPermission();
    });
}

// Hide the enable notification button
function hideEnableNotificationButton() {
    const enableNotificationsButton = document.getElementById('enable-notifications-button');
    enableNotificationsButton.style.display = 'none';

    const promptMessage = document.getElementById('prompt-message');
    promptMessage.style.display = 'none';
}

// Ask for notification permissions
function askForNotificationPermission() {
    if (!isSubscribed && Notification.permission !== 'granted') {
        Notification.requestPermission().then(permission => {
            if (permission === 'granted') {
                registerForPushNotifications();
            } else {
                console.log('Notifications permission denied.');
            }
        });
    }
}

// Register for push notifications
function registerForPushNotifications() {
    navigator.serviceWorker.ready.then(function (registration) {
        registration.pushManager.subscribe({
            userVisibleOnly: true,
            applicationServerKey: urlBase64ToUint8Array(vapidPublicKey)
        })
            .then(function (newSubscription) {
                console.log('User is subscribed:', newSubscription);
                isSubscribed = true;

                const transformedSubscription = {
                    Endpoint: newSubscription.endpoint,
                    P256DH: arrayBufferToBase64(newSubscription.getKey('p256dh')),
                    Auth: arrayBufferToBase64(newSubscription.getKey('auth')),
                    Sender: senderSelf,
                    ChannelCode: channelCode
                };

                return fetch('/chat/subscribe/', {
                    method: 'POST',
                    body: JSON.stringify(transformedSubscription),
                    headers: {
                        'Content-Type': 'application/json'
                    }
                });
            })
            .catch(function (err) {
                console.error('Failed to subscribe the user:', err);
            });
    });
}

// Show the prompt to install the PWA and enable notifications
function showInstallNotificationPrompt() {
    const installButton = document.getElementById('install-pwa-button');
    installButton.style.display = 'block';

    installButton.addEventListener('click', () => {
        if (deferredPrompt) {
            deferredPrompt.prompt();
            deferredPrompt.userChoice.then((choiceResult) => {
                if (choiceResult.outcome === 'accepted') {
                    console.log('User accepted the install prompt');
                } else {
                    console.log('User dismissed the install prompt');
                }
                deferredPrompt = null;
            });
        }
    });
}

// Check if the PWA is installed
function isPWAInstalled() {
    return window.matchMedia('(display-mode: standalone)').matches || window.navigator.standalone === true;
}


// Utility functions
function arrayBufferToBase64(buffer) {
    let binary = '';
    const bytes = new Uint8Array(buffer);
    const len = bytes.byteLength;
    for (let i = 0; i < len; i++) {
        binary += String.fromCharCode(bytes[i]);
    }
    return window.btoa(binary);
}

function urlBase64ToUint8Array(base64String) {
    const padding = '='.repeat((4 - base64String.length % 4) % 4);
    const base64 = (base64String + padding).replace(/\-/g, '+').replace(/_/g, '/');
    const rawData = window.atob(base64);
    const outputArray = new Uint8Array(rawData.length);

    for (let i = 0; i < rawData.length; ++i) {
        outputArray[i] = rawData.charCodeAt(i);
    }
    return outputArray;
}
