window.updateAvailable = new Promise<boolean>((resolve, reject) => {
    if (!('serviceWorker' in navigator)) {
        const errorMessage: string = `This browser doesn't support service workers`;
        console.error(errorMessage);
        reject(errorMessage);
        return;
    }

    navigator.serviceWorker.register('/service-worker.min.js')
        .then((registration: ServiceWorkerRegistration) => {
            console.info(`Service worker registration successful (scope: ${registration.scope})`);

            setInterval(() => {
                registration.update();
            }, 60 * 1000); // 60000ms -> check each minute

            registration.onupdatefound = () => {
                const installingServiceWorker = registration.installing;
                installingServiceWorker.onstatechange = () => {
                    if (installingServiceWorker.state === 'installed') {
                        resolve(!!navigator.serviceWorker.controller);
                    }
                };
            };
        })
        .catch((error: Error) => {
            console.error('Service worker registration failed with error:', error);
            reject(error);
        });
});