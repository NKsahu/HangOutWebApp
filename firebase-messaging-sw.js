importScripts('https://www.gstatic.com/firebasejs/7.8.1/firebase-app.js');
importScripts('https://www.gstatic.com/firebasejs/7.8.1/firebase-messaging.js');
firebase.initializeApp({
    'messagingSenderId': '996349517183',
    'projectId': "tracking-1557219983795",
    apiKey: "AIzaSyAOXRvYFi2TFHzoYzWJEJ2d4KwgJj2W_bo",
    appId: "1:996349517183:web:0c5a7fa0aefc49ff1d4e4a"
});

const messaging = firebase.messaging();
console.log("aaya");
//messaging.setBackgroundMessageHandler((payload) => {
//    // Parses data received and sets accordingly
//    var title = 'Yay a message.';
//    var body = 'We have received a push message.';
//    var icon = '~/Image/app_icon.png';
//    var tag = 'simple-push-demo-notification-tag';


//    return self.registration.showNotification(title, {
//        body: body,
//        icon: icon,
//        tag: tag
//    });
//});

self.addEventListener('push', (event) => {
    //console.log('Received a push message', event);

    //var title = 'Yay a message.';
    //var body = 'We have received a push message.';
    //var icon = '~/Image/app_icon.png';
    //var tag = 'simple-push-demo-notification-tag';
    //event.waitUntil(
    //    self.registration.showNotification(title, {
    //        body: body,
    //        icon: icon,
    //        tag: tag
    //    })
    //);
});

self.addEventListener('notificationclick', (event) => {
    event.notification.close();
    var urlToOpen = new URL('/', self.location.origin).href;
    event.waitUntil(
        clients.matchAll({
            type: 'window',
            includeUncontrolled: true,
        })
            .then((windowClients) => {
                var matchingClient = null;
                for (var i = 0; i < windowClients.length; i++) {
                    var windowClient = windowClients[i];
                    if (windowClient.url === urlToOpen) {
                        matchingClient = windowClient;
                        break;
                    }
                }
                if (matchingClient) {
                    return matchingClient.focus();
                } else {
                    return clients.openWindow(urlToOpen);
                }
            })
    );
});