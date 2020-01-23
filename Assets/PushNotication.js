
var firebaseConfig = {
    apiKey: "AIzaSyAOXRvYFi2TFHzoYzWJEJ2d4KwgJj2W_bo",
    authDomain: "tracking-1557219983795.firebaseapp.com",
    databaseURL: "https://tracking-1557219983795.firebaseio.com",
    projectId: "tracking-1557219983795",
    storageBucket: "tracking-1557219983795.appspot.com",
    messagingSenderId: "996349517183",
    appId: "1:996349517183:web:0c5a7fa0aefc49ff1d4e4a"
};
// Initialize Firebase
firebase.initializeApp(firebaseConfig);
const messaging = firebase.messaging();
messaging.requestPermission()
    .then(function () {
        //MsgElem.innerHTML = "Notification permission granted."
        console.log("Notification permission granted.");

        // get the token in the form of promise
        return messaging.getToken();
    })
    .then(function (token) {
        console.log("Tockeen Is=  " + token);
       // TokenElem.innerHTML = "token is : " + token
    })
    .catch(function (err) {
        //ErrElem.innerHTML = ErrElem.innerHTML + "; " + err
        console.log("Unable to get permission to notify.", err);
    });

messaging.onMessage(function (payload) {
    console.log("Message received. ", payload);
   // NotisElem.innerHTML = NotisElem.innerHTML + JSON.stringify(payload)
});


