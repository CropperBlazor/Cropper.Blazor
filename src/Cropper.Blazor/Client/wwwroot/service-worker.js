// In development, always fetch from the network and do not enable offline support.
// This is because caching would make development more difficult (changes would not
// be reflected on the first load after each change).

self.addEventListener('fetch', event => event.respondWith(onFetch(event)));
self.addEventListener('install', event => event.waitUntil(onInstall(event)));

async function onInstall(event) {
    console.info('Service worker: Install');
}

async function onFetch(event) {
    let cachedResponse = null;
    const cache = await caches.open('blazor_pwa');
    if (event.request.method === 'GET') {
        const request = event.request;
        cachedResponse = await caches.match(request);
        if (cachedResponse) {
            return cachedResponse;
        }
        var resp = await fetch(event.request)
        cache.put(event.request, resp.clone());
        return resp;
    }

    return fetch(event.request);
}
