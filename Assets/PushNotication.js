console.log("aaya push notcie me");
var firebaseConfig = {
    apiKey: "AIzaSyAOXRvYFi2TFHzoYzWJEJ2d4KwgJj2W_bo",
    authDomain: "tracking-1557219983795.firebaseapp.com",
    databaseURL: "https://tracking-1557219983795.firebaseio.com",
    projectId: "tracking-1557219983795",
    storageBucket: "tracking-1557219983795.appspot.com",
    messagingSenderId: "996349517183",
    appId: "1:996349517183:web:0c5a7fa0aefc49ff1d4e4a",
    measurementId: "G-XLXDMQMKG5"
};
// Initialize Firebase
firebase.initializeApp(firebaseConfig);
// Retrieve an instance of Firebase Messaging so that it can handle background
// messages.
const messaging = firebase.messaging();
messaging
    .requestPermission()
    .then(function () {
        //MsgElem.innerHTML = "Notification permission granted."
      //  console.log("Notification permission granted.+=" + messaging.getToken());
       // var tocken = messaging.getToken();

        // get the token in the form of promise
        return messaging.getToken()
    })
    .then(function (token) {
        // print the token on the HTML page
        console.log("Notification permission granted.+=" + token);
        // Subscribe(token);
        subscribeTokenToTopic(token, '100');
        // TokenElem.innerHTML = "token is : " + token
    })
    .catch(function (err) {
        // ErrElem.innerHTML = ErrElem.innerHTML + "; " + err
        console.log("Unable to get permission to notify.", err);
    });
messaging.onMessage(function (payload) {
    console.log("Message received. ", payload);
    // ...
});
//navigator.serviceWorker.register('/firebase-messaging-sw.js');
//Notification.requestPermission(function (result) {
//    if (result === 'granted') {
//        navigator.serviceWorker.ready.then(function (registration) {
//            //registration.showNotification('Notification with ServiceWorker');
//        });
//    }
//});
if ('serviceWorker' in navigator) {
    window.addEventListener('load', function () {
        navigator.serviceWorker.register('/firebase-messaging-sw.js').then(function (registration) {
            messaging.useServiceWorker(registration);
            console.log('ServiceWorker registration successful with scope: ', registration.scope);
        }, function (err) {
            console.log('ServiceWorker registration failed: ', err);
        });
    });
}
navigator.serviceWorker.addEventListener('message', function (event) {
    console.log("service worker " + event.data.msg);
    //you can do whatever you want now that you have this data in script.
});

function subscribeTokenToTopic(token, topic) {
    topic = $("#OrgIdHead").val();
    console.log("topic==" + topic);
    fetch('https://iid.googleapis.com/iid/v1/' + token + '/rel/topics/' + topic, {
        method: 'POST',
        headers: new Headers({
            'Authorization': 'key=AAAA5_sPHX8:APA91bHDAXzfpWGrIXMebCCIySxJo7WY-t8ID4mylmgd-ZHRp65Ybbuk_HW0YZ_nOQkPYjUN83Y9OYv1Gh7WY6Kd8GEJ-xK3xaLz8Zt9BHwz59Ba4P6cwHX4XFd1f2krQYOEuV9hSy94'
        })
    }).then(response => {
        if (response.status < 200 || response.status >= 400) {
            throw 'Error subscribing to topic: ' + response.status + ' - ' + response.text();
        }
        console.log('Subscribed to "' + topic + '"');
    }).catch(error => {
        console.error(error);
    })
}