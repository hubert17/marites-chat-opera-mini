const CACHE_NAME = 'mariteschat-cache-v1';
const urlsToCache = [
    '/',
    '/images/icons/icon-192x192.png',
    '/images/icons/icon-512x512.png'
];

// Install the service worker and cache assets
self.addEventListener('install', event => {
    event.waitUntil(
        caches.open(CACHE_NAME)
            .then(cache => {
                return cache.addAll(urlsToCache);
            })
    );
});

// Fetch assets from the cache, only cache GET requests
self.addEventListener('fetch', event => {
    if (event.request.method === 'GET') {
        event.respondWith(
            fetch(event.request, { redirect: 'follow' })
                .then(response => {
                    // Check if the response is valid
                    if (!response || response.status !== 200 || response.type !== 'basic') {
                        return response;
                    }

                    // Clone the response because the response is a stream
                    const responseClone = response.clone();

                    // Open the cache and store the response
                    caches.open(CACHE_NAME).then(cache => {
                        cache.put(event.request, responseClone);
                    });

                    return response;
                })
                .catch(() => {
                    return caches.match(event.request);
                })
        );
    } else {
        // For non-GET requests, just fetch from the network
        event.respondWith(
            fetch(event.request)
        );
    }
});

// Handle push events
self.addEventListener('push', function (event) {
    const data = event.data.json();

    //// Assuming the current user's identifier is available in the data
    //if (data.sender.toLowerCase() === senderSelf.toLowerCase()) {
    //    return; // Do nothing if the notification is for the sender
    //}

    const options = {
        body: data.message,
        icon: '/images/icons/icon-512x512.png',
        badge: '/images/icons/icon-192x192.png',
        vibrate: [100, 50, 100],
        data: {
            url: data.url
        }
    };

    event.waitUntil(
        self.registration.showNotification(data.title, options)
    );
});

// Handle notification clicks
self.addEventListener('notificationclick', function (event) {
    event.notification.close();
    event.waitUntil(
        clients.openWindow(event.notification.data.url)
    );
});

// Activate service worker and clean up old caches
self.addEventListener('activate', event => {
    const cacheWhitelist = [CACHE_NAME];
    event.waitUntil(
        caches.keys().then(cacheNames => {
            return Promise.all(
                cacheNames.map(cacheName => {
                    if (!cacheWhitelist.includes(cacheName)) {
                        return caches.delete(cacheName);
                    }
                })
            );
        })
    );
    return self.clients.claim();
});
