var db;
(function () {
    'use strict';
    
    if (!('indexedDB' in window)) {
        console.log('This browser doesn\'t support IndexedDB');
        return;
    }
    console.log("index db");
    var request = window.indexedDB.open('test-db4', 1);
        request.onerror = function (event) {
            console.log("error: ");
        };

        request.onsuccess = function (event) {
            db = request.result;
            console.log("success: " + db);
        };

        request.onupgradeneeded = function (event) {
            var db = event.target.result;
            var objectStore = db.createObjectStore("CartTbl", { keyPath: "ItemUUID" });
        }
      
})();

function AddDbCart(Cart) {
    var cartObj = db.transaction('CartTbl', 'readwrite');
    var CartTbl = cartObj.objectStore('CartTbl');
    if (Cart.Count > 0) {
        CartTbl.put(Cart);
    }
    else {
        CartTbl.delete(Cart.ItemUUID);
    }
    
}
function GetCartList() {
    console.log("reading..");
    var CartList = [];
    var cartObj = db.transaction('CartTbl', 'readonly');
    var CartTbl = cartObj.objectStore('CartTbl');
    CartTbl.openCursor().onsuccess = function (event) {
        var cursor = event.target.result;
        if (cursor) {
            CartList.push(cursor.value);
                  cursor.continue();
        } else {
           
        }
    };
    return CartList;
}


