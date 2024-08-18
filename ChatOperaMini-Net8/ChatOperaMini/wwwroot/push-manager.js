if ('serviceWorker' in navigator && 'PushManager' in window) {
    navigator.serviceWorker.register('/service-worker.js')
        .then(function (registration) {
            console.log('Service Worker registered with scope:', registration.scope);
            return navigator.serviceWorker.ready;
        })
        .then(function (registration) {
            console.log('Service Worker is ready:', registration);

            return registration.pushManager.getSubscription()
                .then(function (subscription) {
                    if (subscription) {
                        console.log('User is already subscribed:', subscription);
                         //Optional: Send the subscription to the server if you need to update it
                         //This can be skipped if no updates are needed.
                        const transformedSubscription = {
                            Endpoint: subscription.endpoint,
                            P256DH: arrayBufferToBase64(subscription.getKey('p256dh')),
                            Auth: arrayBufferToBase64(subscription.getKey('auth')),
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
                    } else {
                        return registration.pushManager.subscribe({
                            userVisibleOnly: true,
                            applicationServerKey: urlBase64ToUint8Array(vapidPublicKey)
                        })
                            .then(function (newSubscription) {
                                console.log('User is subscribed:', newSubscription);

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
                            });
                    }
                });
        })
        .catch(function (err) {
            console.log('Failed to subscribe the user: ', err);
        });
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