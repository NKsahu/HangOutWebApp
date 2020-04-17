    var ReloadSeatingSts = false;
    var timer = null;
    var OrgId = 0;
    var LoginId = 0;
    var OrgType = 0;
    var CartList = [];
    var TablesList = null;
    var ItemList = null;
    var ChargeAmt = 0.00;
    var CurrentOrder = 0;// which means SeatingId  Table/Seat/TakeAway
    var CurrOID = 0; //current Order Id;
    var finalAmt = 0.00;
    //===printer setting
    var PrintInvoice = 0;//false
    var NoOfCopy = 0;
    var OrderDisplay = 1;//default Mobile
    var KotPrintType = 0;
    var KotNoOfCopy = 0;
    var ParcelCharge = 0.00;
    //=======
        var ItemHtmlFirst = "<div class='col-md-3  sp-grid-cell ItemsClass'";
    var ItemHtmlFirstPrefix = "><div class='sp-grid-cell-contents highlight-color-4DB6AC' > <span class='sp-grid-cell-text'>";
        var ItemHtmlSecond = " <span><small class='text-muted'>";
        var ItemHtmlLast = "</small> </span> </span></div></div>";
var first = "</td ><td><div style='width:100%'><div class='row' style='border: 1px solid;border-radius: 5px;'>";
var Minushtml = "<span class='col-md-4' style='color:green;font-size:medium;'";
var CountHtml = ">−</span><span class='col-md-4' style=' font-size:medium;'";
var PlusHtml = "</span><span class='col-md-4' style='color:green;font-size:medium;'";
var Carthtmfirst = ">+</span></div ></div ></td > <td style='font-size:medium;' class='sp-product-price'>";
Carthtmfirst += "<span class='sp-padright-10'></span>"
var Carthtmlsecond = "<span class='sp-price'>";
var cartlasthtml = "</span ></td >";
function plus(itemid, ItmUUID) {
    var CartValue = parseInt($("#C" + ItmUUID).text());
    AddToCart(itemid, ItmUUID, CartValue + 1);
    //$("#" + add).text(CartValue+1);
}
function minus(itemid, ItmUUID) {
    var CartValue = parseInt($("#C" + ItmUUID).text());
    if (CartValue > 0) {
        AddToCart(itemid, ItmUUID, CartValue - 1);
    }

}
function AddByItemClick(itemid) {

    //check item apply addons
    var ObjItem = ItemList.find(x => {
        return x.IID=== itemid
    });
    if ("Addons" in ObjItem) {

        console.log("adddons" + ObjItem.Addons);
        OpenAddons(ObjItem.Addons, itemid);
        return;
    }
    var UUID = ObjItem.UUID;
    if (UUID != null && UUID != "") {
        var carts = CartList.filter(function (x) {
            return x.ItemId.toString() == itemid;
        });
        if (carts.length > 0) {
            for (var i = 0; i < carts.length; i++) {
                if (carts[i].IsParcel == 0) {
                    UUID = carts[i].ItemUUID;
                    break;
                }
                if (i == (carts.length - 1)) {
                    UUID = "";
                }
            }
        }
    }
    if ($("#C" + UUID).length > 0) {
        var CartValue = parseInt($("#C" + UUID).text());
        AddToCart(itemid, UUID, CartValue + 1);
    }
    else {
        AddToCart(itemid, UUID, 1);
    }
}
function OpenAddons(AddonList, ItemId) {


    var html = "";
    var strhtml = '<div class="card">';
    strhtml += '<div class="card-body">';
    strhtml += '<div class="AddOnnCls row">';
    for (var i = 0; i < AddonList.length; i++) {
        // title
        strhtml += '<div class="row" style="border:1px solid black" >';
        strhtml += '<div class="col-md-8">';
        strhtml += '<label></label>';
        strhtml += '<input type="text" id="Addon' + AddonList[i].TitleId + '" readonly value="' + AddonList[i].AddOnTitle + '" class="form-control" />';
        strhtml += '</div>';
        //min 
        strhtml += '<div class="col-md-2">';
        strhtml += '<label>Min</label>';
        strhtml += ' <input type="text" id="Min' + AddonList[i].TitleId + '" readonly value="' + AddonList[i].Min + '" class="form-control" />';
        strhtml += '</div>';

        //max
        strhtml += '<div class="col-md-2">';
        strhtml += '<label>Max</label>';
        strhtml += ' <input type="text" readonly id="Max' + AddonList[i].TitleId + '" value="' + AddonList[i].Max + '" class="form-control" />';
        strhtml += '</div>';
        //=====items =====serving size
        strhtml += '<div class="col-md-12" id="Items" >'
        for (var j = 0; j < AddonList[i].AddOnItemList.length; j++) {
            strhtml += '<div class="row">';
            //item 
            strhtml += '<div class="col-md-6">';
            // strhtml += '<label></label>';
            strhtml += '<input type="text" readonly  value="' + AddonList[i].AddOnItemList[j].Title + '" class="form-control" />';
            strhtml += '</div>';
            //amt
            strhtml += '<div class="col-md-3">';
            //  strhtml += '  <label></label>';
            strhtml += '<input type="text" id="Price' + AddonList[i].AddOnItemList[j].AddOnItemId + '" readonly value="' + AddonList[i].AddOnItemList[j].Price.toFixed(2) + '" class="form-control" />';
            strhtml += '</div>';
            //tax
            strhtml += '<div class="col-md-3">';
            // strhtml += '  <label></label>';
            strhtml += '<input type="checkbox" TitileId="' + AddonList[i].TitleId + '" id="' + AddonList[i].AddOnItemList[j].AddOnItemId + '" class="form-control AddonCheckBox ' + AddonList[i].TitleId + '" />';
            strhtml += '</div>';
            strhtml += ' </div>';

        }
        strhtml += ' </div>';
        strhtml += ' </div>';
    }


    //submit and cancel button
    strhtml += '<div class="col-md-3"> <button onclick="CancelAddon()" class="btn btn-md btn-danger">Cancel</button> </div>';
    strhtml += '<div class="col-md-6"></div>'
    strhtml += '<div class="col-md-3"><button onclick="SubmitAddon(' + ItemId + ')" class="btn btn-md btn-success">Submit</button> </div>';

    strhtml += ' </div>';
    strhtml += ' </div>';
    strhtml += ' </div>';
    makedpt("ShowAddon", 300, 400);
    $("#ShowAddon > .modal-dialog > .modal-content > .modal-body").html(strhtml);
    showdpt("ShowAddon");

}
function CancelAddon() {
    hidedpt("ShowAddon");
}
function SubmitAddon(itemid) {
    var ObjItem = ItemList.find(x => {
        return x.IID === itemid
    });

    console.log("item sum");
    var ObjItmAddon = {};
    var AddonItemSelected = document.getElementsByClassName("AddonCheckBox");
    var AddonItemIdCsv = "";
    var ItemPrice = parseFloat(ObjItem.ItemPrice);
    var AddonItemId = [];
    var AddonAmts = [];
    var NotSelectedIds = [];
    var ArrayAddonsId = [];
    for (var i = 0; i < AddonItemSelected.length; i++) {
        var ObjCheckBx = $(AddonItemSelected[i]);
        if (ObjCheckBx.is(':checked')) {
            console.log("Aaya=" + AddonItemSelected[i]);
            if (i == (AddonItemSelected.length - 1)) {
                AddonItemIdCsv += ObjCheckBx.attr('id');
            }
            else {
                AddonItemIdCsv += ObjCheckBx.attr('id') + ",";
            }

            AddonItemId.push(ObjCheckBx.attr('id'));
            // sumOfItem += parseFloat($("#Price" + ObjCheckBx.attr('id')).val());
            var addonamt = parseFloat($("#Price" + ObjCheckBx.attr('id')).val());
            AddonAmts.push(addonamt);
            var titleId = ObjCheckBx.attr('TitileId');
            ArrayAddonsId.push(titleId);
            var Min = parseInt($("#Min" + titleId).val());
            var Max = parseInt($("#Max" + titleId).val());
            var AddonAppliedArray = ArrayAddonsId.filter(function (titid) {
                return titid == titleId;
            });
            var Items = document.getElementsByClassName(titleId);
            if (Items.length == AddonAppliedArray.length) {
                var result = AddonValidation(titleId);
                if (result != null) {
                    return;
                }
            }
        }
        else {
            var titleId = ObjCheckBx.attr('TitileId');
            ArrayAddonsId.push(titleId);
            var Items = document.getElementsByClassName(titleId);
            var AddonAppliedArray = ArrayAddonsId.filter(function (titid) {
                return titid == titleId;
            });
            if (Items.length == AddonAppliedArray.length) {
                var result = AddonValidation(titleId);
                if (result != null) {
                    return;
                }
            }

        }


    }
    ObjItmAddon.AddonItemIdCsv = AddonItemIdCsv;
    ObjItmAddon.AddonItemId = AddonItemId;
    ObjItmAddon.AddonAmts = AddonAmts;
    $("#waiting").show();
    //==
    var ObjCart = {};
    ObjCart.CID = LoginId;
    ObjCart.ItemId = itemid;
    ObjCart.TableorSheatOrTaleAwayId = CurrentOrder;
    ObjCart.itemAddons = ObjItmAddon;
    ObjCart.ItemPrice = ItemPrice;
    ObjCart.OrgId = OrgId;
    ObjCart.IsAddon = 1;
    ObjCart.ParcelCharge = 0.00;
    ObjCart.AddonAmts = AddonAmts;
    ResetOrder(CurrentOrder, 3);

    var CurrItemIndex = CartList.findIndex(x => {
        return (x.ItemId == ObjCart.ItemId && x.itemAddons != null && x.itemAddons.AddonItemIdCsv == AddonItemIdCsv && x.IsParcel == 0 && x.TableorSheatOrTaleAwayId == CurrentOrder)
    });
    var ItemUUID = "";
    var Cnt = 1;
    var IsParcel = 0;
    var ItemPrice = 0.00;
    if (CurrItemIndex >= 0 && CartList[CurrItemIndex].IsParcel == 0) {
        Cnt = CartList[CurrItemIndex].Count + 1
        CartList[CurrItemIndex].Count = Cnt;
        ItemUUID = CartList[CurrItemIndex].ItemUUID;
        ItemPrice = CartList[CurrItemIndex].ItemPrice;
        IsParcel = CartList[CurrItemIndex].IsParcel;
    }
    else {
        ObjCart.Count = Cnt;
        ObjCart.ItemUUID = GetUUID();
        ObjCart.IsParcel = IsParcel;
        ItemUUID = ObjCart.ItemUUID;
        ItemPrice = ObjCart.ItemPrice;
        CartList.push(ObjCart);
    }
    if (ItemAlreadyAdded(Object.UUID)) {
        $("#tr" + Object.UUID).remove();
    }
    ObjItem.ItemArray = [];
    ObjItem.ItemCartValue = 1;
    if (ItemAlreadyAdded(ItemUUID)) {
        $("#tr" + ItemUUID).remove();
    }
    var ObjItmAdd = {};
    ObjItmAdd.UUID = ItemUUID;
    ObjItmAdd.IsAddon = 1;
    ObjItmAdd.Price = ItemPrice;
    ObjItmAdd.Cnt = Cnt;
    ObjItmAdd.ParcelCharge = 0.00;
    ObjItmAdd.AddonAmts = AddonAmts;
    ObjItmAdd.IsParcel = IsParcel;
    ObjItem.ItemArray.push(ObjItmAdd);
    AddItemToQue(ObjItem);
    //ItemArray
    hidedpt("ShowAddon");
    //update final amt
    var FinlAmt = 0.00;
    var CurrSeatingItem = CartList.filter(function (x) {
        return x.TableorSheatOrTaleAwayId == CurrentOrder;
    });
    for (var i = 0; i < CurrSeatingItem.length; i++) {
        FinlAmt += CurrSeatingItem[i].ItemPrice * CurrSeatingItem[i].Count;
        FinlAmt += CurrSeatingItem[i].ParcelCharge * CurrSeatingItem[i].Count;
        if (CurrSeatingItem[i].IsAddon == 1) {
            for (var j = 0; j < CurrSeatingItem[i].AddonAmts.length; j++) {
                FinlAmt += CurrSeatingItem[i].AddonAmts[j] * CurrSeatingItem[i].Count;
            }
        }
    }
    ShowSeatingAmt(FinlAmt);
    $("#waiting").hide();
}
function AddToCartOffline(itemId, ItemUUID, Cnt) {

    var ObjItem = ItemList.find(x => {
        return x.IID.toString() == itemId;
    });
    var SingleItemPrice = 0.00;
    var AddonAmts = 0.00;
    var IsParcel = 0;
    var ParcelCharge = 0.00;
    if (CartList.length > 0) {
        var Cart = CartList.find(x => {
            return x.ItemUUID == ItemUUID;
        });
        if (Cart != null) {
            for (var i = 0; i < CartList.length; i++) {
                if (CartList[i].ItemUUID == ItemUUID) {
                    CartList[i].Count = Cnt;
                    SingleItemPrice = CartList[i].ItemPrice;
                    IsParcel = CartList[i].IsParcel;
                    ParcelCharge = CartList[i].ParcelCharge;
                    if (CartList[i].IsAddon == 1) {
                        for (var j = 0; j < CartList[i].AddonAmts.length; j++) {
                            AddonAmts += CartList[i].AddonAmts[j] * CartList[i].Count;
                        }
                    }
                    if (Cnt <= 0) {
                        CartList.splice(i, 1);
                    }
                    break;
                }
            }
            console.log("Update Cart Count=" + CartList.length);
        }
        else {
            var Cart = {};
            Cart.CID = LoginId;
            Cart.ItemId = itemId;
            Cart.Count = Cnt;
            Cart.TableorSheatOrTaleAwayId = CurrentOrder;
            Cart.OrgId = OrgId;
            Cart.ItemPrice = ObjItem.ItemPrice;
            SingleItemPrice = Cart.ItemPrice;
            Cart.ItemUUID = GetUUID();
            Cart.IsParcel = IsParcel;
            Cart.ParcelCharge = 0;;
            ItemUUID = Cart.ItemUUID;
            CartList.push(Cart);

        }
    }
    else {

        var Cart = {};
        Cart.CID = LoginId;
        Cart.ItemId = itemId;
        Cart.Count = Cnt;
        Cart.TableorSheatOrTaleAwayId = CurrentOrder;
        Cart.OrgId = OrgId;
        if (ItemUUID == null || ItemUUID == "" || ItemUUID === 'undefined') {
            Cart.ItemUUID = GetUUID();
        }
        else {
            Cart.ItemUUID = ItemUUID;
        }
        Cart.IsParcel = IsParcel;
        Cart.ItemPrice = ObjItem.ItemPrice;
        Cart.ParcelCharge = 0;
        SingleItemPrice = Cart.ItemPrice;
        ItemUUID = Cart.ItemUUID;
        CartList.push(Cart);
        //  ItemPrice { get; set; }
        //   IsAddon { get; set; } //0: no , 1: yes
    }
    
    var FinlAmt = 0.00;
    var CurrSeatingItem = CartList.filter(function (x) {
        return x.TableorSheatOrTaleAwayId == CurrentOrder;
    });
    console.log("Cuuseatingcnt=" + CurrSeatingItem.length + "Finalamt=" + FinlAmt);
    for (var i = 0; i < CurrSeatingItem.length; i++) {
        FinlAmt += CurrSeatingItem[i].ItemPrice * CurrSeatingItem[i].Count;
        FinlAmt += CurrSeatingItem[i].ParcelCharge * CurrSeatingItem[i].Count;
        if (CurrSeatingItem[i].IsAddon == 1) {
            for (var j = 0; j < CurrSeatingItem[i].AddonAmts.length; j++) {
                FinlAmt += CurrSeatingItem[i].AddonAmts[j] * CurrSeatingItem[i].Count;
            }
        }
    }
    //==============
    for (i = 0; i < ItemList.length; i++) {
        if (ItemList[i].IID === itemId) {
            ItemList[i].UUID = ItemUUID;
            break;
        }
    }
    if (!ItemAlreadyAdded(ItemUUID)) {
        $("#tr" + ItemUUID).remove();
        ObjItem.ItemCartValue = 1;
        ObjItem.ItemArray = [];
        var ObjItmAdd = {};
        ObjItmAdd.UUID = ItemUUID;
        ObjItmAdd.IsAddon = 0;
        ObjItmAdd.Price = SingleItemPrice;
        ObjItmAdd.IsParcel = IsParcel;
        ObjItmAdd.ParcelCharge = ParcelCharge;
        ObjItmAdd.Cnt = Cnt;
        ObjItem.ItemArray.push(ObjItmAdd);
        if (Cnt > 0) {
            AddItemToQue(ObjItem);
            ResetOrder(CurrentOrder, 3);
        }
    }
    else {
        $("#C" + ItemUUID).text(Cnt);
        var newamt = SingleItemPrice * Cnt;
        newamt += ParcelCharge * Cnt;
        newamt += AddonAmts;
        $("#P" + ItemUUID).text(newamt.toFixed(2));
    }
    //remove items
    if (Cnt == 0) {
        $("#tr" + ItemUUID).remove();
    }
    // show Final Order Price Floor_or_ScreenId
    var ObjSeating = TablesList.find(x => {
        return x.Table_or_RowID.toString() == CurrentOrder;
    });

    if (CurrSeatingItem.length == 0 && ObjSeating.CurrOID != null && ObjSeating.CurrOID == 0) {
        ResetOrder(CurrentOrder, 1);
    }
    ShowSeatingAmt(FinlAmt);
}
function ShowSeatingAmt(SeatAmt) {
    $("#FinalAmt").html(SeatAmt.toFixed(2));
    var finalChargeToShow = ChargeAmt + SeatAmt;
    $("#FinalAmtCharge").text('CHARGE ' + finalChargeToShow.toFixed(2) + ' /RS');
}
function GetUUID() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}
function AddonValidation(titileid) {
    var Items = document.getElementsByClassName(titileid);
    var checkedcnt = 0;
    var unchecked = 0;
    for (var j = 0; j < Items.length; j++) {
        var obj = $(Items[j]);
        var titleId = obj.attr('TitileId');
        var Min = parseInt($("#Min" + titleId).val());
        var Max = parseInt($("#Max" + titleId).val());
        var title = $("#Addon" + titleId).val();
        console.log("aaya");
        if (obj.is(':checked')) {
            checkedcnt += 1;
        }
        else {
            unchecked += 1;
        }
    }
    if (Items.length == unchecked + checkedcnt) {
        condition = true;
    }
    if (checkedcnt > Min && condition && checkedcnt > Max) {
        alert("Qty can't more than " + Max + " of " + title);
        return 1;
    }
    if (checkedcnt < Min && condition) {
        alert("Select Minimum " + Min + " Qty of " + title);
        return 1;
    }
    if (checkedcnt > Max && condition) {
        alert("Qty can't more than " + Max + " of " + title);
        return 1;
    }
    if (unchecked == Items.length && Min > 0) {

        alert('Select atleast ' + Min + ' Item from ' + title);
        return 1;
    }
    else {
        return null;
    }
}
function AddToCart(ItemId, ItmUUID, Cnt) {
    AddToCartOffline(ItemId, ItmUUID, Cnt);
}
function AddItemToQue(ObjItem) {
    if (ObjItem.ItemCartValue != '0') {
        var Itemid = ObjItem.IID;
        var ItemUUIDS = ObjItem.ItemArray;
        for (var i = 0; i < ItemUUIDS.length; i++) {
            // console.log("==" + ItemUUIDS[i]);
            var ObjUUID = ItemUUIDS[i];
            var ItmUUID = ItemUUIDS[i].UUID;
            var IsAddon = ItemUUIDS[i].IsAddon;
            var IsParcel = ItemUUIDS[i].IsParcel;
            var info = "";
            var ItemPrice = parseFloat(ObjUUID.Price) * parseInt(ObjUUID.Cnt);
            ItemPrice += ItemUUIDS[i].ParcelCharge * parseInt(ObjUUID.Cnt);
            if (IsAddon == 1) {
                info += " <i class='fa fa-info-circle' " + "onclick='AddonInfo(\"" + ItmUUID + "\",\"" + Itemid + "\")' style='font-size:small;'" + "aria-hidden='true'></i>";
                for (j = 0; j < ItemUUIDS[i].AddonAmts.length; j++) {
                    ItemPrice += parseFloat(ItemUUIDS[i].AddonAmts[j]) * parseInt(ObjUUID.Cnt);
                }
            }
            var ParcelHtml = "<td><i UUID=" + ItmUUID + " title='make parcel' onclick='Makeparcel(this)' style='font-size:medium;' class='fas fa-shopping-bag'></i></td></tr>";
            if (IsParcel == 1) {
                ParcelHtml = "<td><i style='color:red;font-size:medium;'title='remove parcel' UUID=" + ItmUUID + " onclick='Makeparcel(this)'  class='fas fa-shopping-bag'></i></td></tr>";
            }
            $("#AddItem").append("<tr class='ItemsTr' id='tr" + ItmUUID + "'><td style='font-size:medium;'>" + ObjItem.ItemName + first + Minushtml + "onclick='minus(\"" + Itemid + "\",\"" + ItmUUID + "\")'" + CountHtml + "id='C" + ItmUUID + "' >" + ObjUUID.Cnt + PlusHtml + "onclick='plus(\"" + Itemid + "\",\"" + ItmUUID + "\")'" + Carthtmfirst + Carthtmlsecond +"<Span id='P" + ItmUUID + "'>" + ItemPrice.toFixed(2) + "</span>" + info + cartlasthtml + ParcelHtml);
        }

    }

}
function ItemAlreadyAdded(Itemid) {
    if ($('#tr' + Itemid).length) {
        return true;
    }
    else {
        return false;
    }
}
function Makeparcel(event) {
    var itemuuid = $(event).attr('UUID');
    // console.log("aaya parcel");
    var CountAmt = 0.00;
    for (var i = 0; i < CartList.length; i++) {
        if (CartList[i].ItemUUID == itemuuid) {
            // SingleItemPrice = CartList[i].ItemPrice;
            if (CartList[i].IsParcel == 0) {
                $(event).css('color', 'red');
                $(event).attr('title', 'remove parcel');
                CartList[i].IsParcel = 1;
                CartList[i].ParcelCharge = ParcelCharge;

            }
            else {
                $(event).css('color', 'black');
                CartList[i].IsParcel = 0;
                $(event).attr('title', 'make parcel');
                CartList[i].ParcelCharge = 0;
            }
            CountAmt = CartList[i].ItemPrice * CartList[i].Count;
            CountAmt += CartList[i].ParcelCharge * CartList[i].Count;
            if (CartList[i].IsAddon == 1) {
                for (var j = 0; j < CartList[i].AddonAmts.length; j++) {
                    CountAmt += CartList[i].AddonAmts[j] * CartList[i].Count;
                }
            }
            break;
        }
    }
    var CurrSeatingItem = CartList.filter(function (x) {
        return x.TableorSheatOrTaleAwayId == CurrentOrder;
    });
    $("#P" + itemuuid).text(CountAmt.toFixed(2));
    var FinlAmt = 0.00;
    for (var i = 0; i < CurrSeatingItem.length; i++) {
        FinlAmt += CurrSeatingItem[i].ItemPrice * CurrSeatingItem[i].Count;
        FinlAmt += CurrSeatingItem[i].ParcelCharge * CurrSeatingItem[i].Count;
        if (CurrSeatingItem[i].IsAddon == 1) {
            for (var j = 0; j < CurrSeatingItem[i].AddonAmts.length; j++) {
                FinlAmt += CurrSeatingItem[i].AddonAmts[j] * CurrSeatingItem[i].Count;
            }
        }
    }
    ShowSeatingAmt(FinlAmt);
}
function SearchItem(SearchItem, hideClsName) {
    var input = document.getElementById(SearchItem);
    var filter = input.value.toLowerCase();
    var nodes = document.getElementsByClassName(hideClsName);
    for (i = 0; i < nodes.length; i++) {
        if (nodes[i].innerText.toLowerCase().includes(filter)) {
            nodes[i].style.display = "block";
        } else {
            nodes[i].style.display = "none";
        }
    }
}

function GeTablesTakeAwya() {
    $.ajax({
        type: 'POST',
        url: "/AdminApi/GetSeating",
        success: function (data) {
            var Seating = JSON.parse(data);
            TablesList = Seating.Seating;
            ShowOrders(TablesList);
            if (TablesList.length > 0) {
                SeatingClick(TablesList[0].Table_or_RowID);
            }
            AddFlrScrFilter(Seating.FlrScrList);
            OrgId =parseInt(Seating.OrgId);
            LoginId = parseInt(Seating.UserCode);
            OrgType = parseInt(Seating.OrgType);
        },
        error: function (jqXhr, textStatus, errorMessage) { // error callback
            $("#waiting").hide();
        }
    });

}
function AddFlrScrFilter(FlrScrList) {
    //FlrScrSrc
    for (var i = 0; i < FlrScrList.length; i++) {
        var option = '<option class="PosSelItem" value="' + FlrScrList[i].Floor_or_ScreenID + '"> ' + FlrScrList[i].Name + '</option>';
        $("#FlrScrSrc").append(option);
    }
}
function FilterTables(event) {
    var ForSid = $(event).val();
    if (ForSid != null && ForSid != "" && ForSid != "0") {
        var ForSidInt = parseInt(ForSid);
        var TakeAwayList = TablesList.filter(function (itm) {
            return itm.Floor_or_ScreenId == ForSidInt;
        });
        ShowOrders(TakeAwayList);
    }
    else {
        ShowOrders(TablesList);
    }
}
function FilterOrder(Status,event) {
    //all
    $(".posTbleBtn").css('background-color', '#000000');
    $(event).css('background-color', '#44cd4a');
    if (Status == '0') {
        ShowOrders(TablesList);
    }
    else if (Status == '3') {
        console.log(TablesList);
        var TakeAwayList = TablesList.filter(function (itm) {
            return itm.Type == '3';
        });
        ShowOrders(TakeAwayList);

    }
    else if (Status == '1') {
        if (OrgType == "1") {
            var TableListOnly = TablesList.filter(function (itm) {
                return itm.Type == '1';
            });
        }
        else {
            var TableListOnly = TablesList.filter(function (itm) {
                return itm.Type == '2';
            });
        }
        ShowOrders(TableListOnly);
        console.log("Table" + TablesList);
    }
    else {
        var TableListOnly = TablesList.filter(function (itm) {
            return itm.Type == '0';
        });
        ShowOrders(TableListOnly);
    }

}
function ShowOrders(list) {
    $("#SeatingTbl").html('');
    var ColSize = 3;
    console.log("dis=" + $('#MenuItems').css('display'));
    //if ($('#CartItemsMenu').css('display') == 'block') {
    //    ColSize = 3;
    //}
    for (i = 0; i < list.length; i++) {
        var TableID = list[i].Table_or_RowID.toString();
        var Status = list[i].Status;//{"1":free,"2":"BOOKED",3:"PROGRESS"}
        var Otp = list[i].Otp;
        var FlrScrName = list[i].ScrnFlr;
        var SeatName = list[i].SeatName;
        var RowSide = list[i].RowSide;
       // var ShowStatus = list[i].SeatingUser == null ? "" : list[i].SeatingUser;
        var html = '<div id="SeatingId'+TableID+'" class="col-md-' + ColSize + ' SeatingNum" onclick="SeatingClick(' + TableID + ');" ondblclick="SeatingDBClick(' + TableID+');">';
        html += '<div class="SeatingBox"><div class="text-center" style="margin-top:12px;">';
        html += '<div style="height:110px;overflow:hidden">';
        html += '<h3>' + FlrScrName + '</h3>';
        html += '<h2 style="font-weight:bold">' + SeatName + '</h2>';
        html += '<h4>' + RowSide + '</h4></div>';
        console.log("Status=" + Status);
        if (Status == 3) {
            html += '<div id="SeatingLine' + TableID+'" class="SeatingLine" style="background-color:red" ></div > ';
            html += ' <span id="OtpBox'+TableID+'" class="OtpBox" style="color:red">' + Otp + '</span></div></div>';
        }
        else {
            html += '<div id="SeatingLine' + TableID +'" class="SeatingLine"></div > ';
            html += ' <span id="OtpBox' + TableID +'" class="OtpBox">' + Otp + '</span></div></div>';
        }
        html += "</div>";
        $("#SeatingTbl").append(html);
    }
    
}
function FreeOrOccupied(Status,event) {
    $(".posTbleBtn").css('background-color', '#000000');
    $(event).css('background-color', '#44cd4a');
    if (Status == '1') {
        var FreeList = TablesList.filter(function (x) {
            return x.Status == 1;
        });
        ShowOrders(FreeList);
        console.log("FreeList" + FreeList.length);

    }
    else if (Status == '3') {
        var OccupiedList = TablesList.filter(function (x) {
            return x.Status == 3;
        });
        ShowOrders(OccupiedList);
        console.log("OccupiedList" + OccupiedList.length);
    }
    else if (Status == '2') {
        var OccupiedList = TablesList.filter(function (x) {
            return x.Status == 2;
        });
        ShowOrders(OccupiedList);
        console.log("Call For Bill" + OccupiedList.length);
    }
}
function ReloadSeating() {
    var RunningOrd = [];
    $.ajax({
        type: 'POST',
        url: "/AdminApi/ReloadSeating",
        success: function (data) {
            var Jobj = JSON.parse(data);
            RunningOrd = Jobj.RunningOrds;
            if (RunningOrd.length > 0) {
                for (var i = 0; i < TablesList.length; i++) {
                    var ObjOrd = RunningOrd.find(x => {
                        return x.Table_or_SheatId == TablesList[i].Table_or_RowID && x.TableOtp == TablesList[i].Otp;
                    });
                    if (ObjOrd != null) {
                        TablesList[i].CurrOID = ObjOrd.OID;
                        TablesList[i].Status = 3;//SeatingAmt
                        if (ObjOrd.PaymentStatus > 0) {
                            TablesList[i].SeatingAmt = 0;
                        }
                        else {
                            TablesList[i].SeatingAmt = parseFloat(ObjOrd.OrdAmt);
                        }
                    }
                    else if (TablesList[i].CurrOID>0) {
                        var CurrSeatingItem = CartList.filter(function (x) {
                            return x.TableorSheatOrTaleAwayId == TablesList[i].Table_or_RowID;
                        });
                        if (CurrSeatingItem.Count == 0) {
                            TablesList[i].CurrOID = 0;
                            TablesList[i].Status = 1;
                            TablesList[i].SeatingAmt = 00;
                        }
                    }
                }
            }
        },
        error: function (jqXhr, textStatus, errorMessage) { // error callback
            $("#waiting").hide();
        }
    });
    
}
function GetItemList(TableOrMWid) {
    CurrentOrder = TableOrMWid;
    GetItemListByCategory(CurrentOrder)
}
function GetItemListByCategory(TableOrMWid) {
    $("#waiting").show();
    $("tr.ItemsTr").remove();
    var ObjOrderName = TablesList.find(x => {
        return x.Table_or_RowID == TableOrMWid;
    });
    var CurrSeatObj = ObjOrderName;
    var CategoryList = CurrSeatObj.MenuItems;
    $("#C_mobile").val('');
    $("#C_name").val('');
    $("#TorTWID").text(ObjOrderName.ScrnFlr + " " + ObjOrderName.SeatName + " " + ObjOrderName.RowSide);
    CurrOID = ObjOrderName.CurrOID;
    
    var TotalRs = 0.00;
    ChargeAmt = 0.00;
    var CurrSeatingItem = CartList.filter(function (x) {
        return x.TableorSheatOrTaleAwayId == CurrentOrder;
    });
    for (i = 0; i < CategoryList.length; i++) {
        var ItemsList = CategoryList[i].MenuItems;
        for (j = 0; j < ItemsList.length; j++) {
           // ShowItems(ItemsList[j]);
            var CartalreadyAdded = CurrSeatingItem.filter(function (x) {
                return x.ItemId == ItemsList[j].IID.toString();
            });
            if (CartalreadyAdded.length > 0) {
                var list = [];
                for (var k = 0; k < CartalreadyAdded.length; k++) {
                    var Jobj = {};
                    Jobj.UUID = CartalreadyAdded[k].ItemUUID;
                    Jobj.Price = CartalreadyAdded[k].ItemPrice;
                    Jobj.Cnt = CartalreadyAdded[k].Count;
                    Jobj.IsAddon = CartalreadyAdded[k].IsAddon;
                    if (Jobj.IsAddon == 1) {
                        Jobj.AddonAmts = CartalreadyAdded[k].AddonAmts;
                    }
                    Jobj.IsParcel = CartalreadyAdded[k].IsParcel;
                    Jobj.ParcelCharge = CartalreadyAdded[k].ParcelCharge;
                    list.push(Jobj);
                }
                ItemsList[j].ItemCartValue = CartalreadyAdded.length;
                ItemsList[j].ItemArray = list;
                AddItemToQue(ItemsList[j]);
            }
        }
    }
    ChargeAmt = parseFloat(CurrSeatObj.SeatingAmt);
    for (var c = 0; c < CurrSeatingItem.length; c++) {
        TotalRs += CurrSeatingItem[c].ItemPrice * CurrSeatingItem[c].Count;
        TotalRs += CurrSeatingItem[c].ParcelCharge * CurrSeatingItem[c].Count;
        if (CurrSeatingItem[c].IsAddon == 1) {
            for (var j = 0; j < CurrSeatingItem[c].AddonAmts.length; j++) {
                TotalRs += CurrSeatingItem[c].AddonAmts[j] * CurrSeatingItem[c].Count;
            }
        }
    }
    $("#waiting").hide();
    ShowSeatingAmt(TotalRs);
    $("#ContactId").val(CategoryList[0].ContactId);
    var MobileNo = CategoryList[0].Mobile;
    var Cname = CategoryList[0].CName;
    $("#C_mobile").val(MobileNo);
    $("#C_name").val(Cname);
    if (CategoryList[0].ContactId == -1) {
        $('#C_mobile,#C_name').prop('readonly', true);
        $("#C_save").attr('disabled', 'disabled');
    }
    
}

function GetOnlyItemsList(SeatingId) {
    ItemList = [];
    $("#MenuFilter").html('');
    $("#SubMenuItems").html('');
    var ObjOrderName = TablesList.find(x => {
        return x.Table_or_RowID == SeatingId;
    });
    CurrOID = ObjOrderName.CurrOID;
    var CurrSeatObj = ObjOrderName;
    var CategoryList = CurrSeatObj.MenuItems;
    var HtmlMenuFilter = '<div class="col-md-2">';
    var categoryCnt = CategoryList.length;
    HtmlMenuFilter += '<button class="btn btn-sm MenuBox form-control " onclick="filterItemByCat(this, 0)" style="background-color:#44cd4a" > <span class="TblBtnText">All</span></button ></div>';
    for (i = 0; i < categoryCnt; i++) {
        var addagain = false;
        if (i < 10) {
            HtmlMenuFilter += '<div class="col-md-2"><button class="btn btn-sm  MenuBox form-control " onclick="filterItemByCat(this,' + CategoryList[i].MenuId + ')" > <span class="TblBtnText">' + CategoryList[i].Name + '</span></button ></div>';
        }
        else if (i == 10 && categoryCnt > 10) {
            HtmlMenuFilter += '<div class="col-md-2" ><button class="btn btn-sm MenuBox form-control" onclick="ShowHidden(this)" > <span class="TblBtnText"  >' + CategoryList[i].Name + '&darr;</span></button ></div>';
            addagain = true;
        }
        else if (i == 10 && categoryCnt == 10) {
            HtmlMenuFilter += '<div class="col-md-2"><button class="btn btn-sm  MenuBox form-control " onclick="filterItemByCat(this,' + CategoryList[i].MenuId + ')" > <span class="TblBtnText" >' + CategoryList[i].Name + '</span></button ></div>';
        }
        else {
            HtmlMenuFilter += '<div class="col-md-2 hiddenmenus" style="display:none;"  ><button class="btn btn-sm  MenuBox form-control " onclick="filterItemByCat(this,' + CategoryList[i].MenuId + ')" > <span class="TblBtnText">' + CategoryList[i].Name + '</span></button ></div>';
        }

        if (addagain) {
            HtmlMenuFilter += '<div class="col-md-2 hiddenmenus" style="display:none;"  ><button class="btn btn-sm  MenuBox form-control " onclick="filterItemByCat(this,' + CategoryList[i].MenuId + ')" > <span class="TblBtnText">' + CategoryList[i].Name + '</span></button ></div>';

        }
        var ItemsList = CategoryList[i].MenuItems;
        for (j = 0; j < ItemsList.length; j++) {
            ItemList.push(ItemsList[j]);
            ShowItems(ItemsList[j]);
        }
    }

    $("#MenuFilter").html(HtmlMenuFilter);
    if (CategoryList.length == 0) {

        $.alert({
            title: 'Alert!',
            content: "Menu Not Applied.",
        });
        return;
    }
}
function FilterItems(list) {
    $("#SubMenuItems").html('');
    for (j = 0; j < list.length; j++) {
        ShowItems(list[j]);
    }
}
function ShowItems(objitem) {
    console.log("item mode");
    var htmlstr = '<div class="col-md-3" onclick="AddByItemClick('+ objitem.IID +')">';
    htmlstr += '<div class="ItemCard">';
    htmlstr += '<h4 class="ItemName">' + objitem.ItemName + '</h4>';
    if (objitem.ItemMode =="1") {
        htmlstr += '<div class="SeatingLine"></div>';
    }
    else {
        htmlstr += '<div class="SeatingLine" style="background-color:red;"></div>';
    }
    htmlstr += '<div>';
    htmlstr += '<h4 class="GreenTxt text-center">Rs. ' + objitem.ItemPrice +'</h4></div></div></div>';
    $("#SubMenuItems").append(htmlstr);
}
function filterItemByCat(event, MenuId) {
    $('.MenuBox').css('background-color', '#000000');
    $(event).css('background-color', '#44cd4a');
    if (MenuId != "0") {
        var FilteredItemList = ItemList.filter(function (x) {
            return x.MenuId == MenuId;
        });
        FilterItems(FilteredItemList);
    } else {

        FilterItems(ItemList);
    }
}
function ResetOrder(SeatingId, status) {
    console.log("aya color");
    if (status == 1) {
        $("#SeatingLine" + SeatingId).css('background-color','#44cd4a');
        $("#OtpBox" + SeatingId).css('color','#44cd4a');
    }
    else if (status == 3) {
        $("#SeatingLine" + SeatingId).css('background-color','red');
        $("#OtpBox" + SeatingId).css('color','red');
    }

    for (var i = 0; i < TablesList.length; i++) {
        if (TablesList[i].Table_or_RowID == SeatingId) {
            TablesList[i].Status = status;
            return;
        }
    }
}
function PostOrder(Obj) {
    var CurrSeatingItems = CartList.filter(function (x) {
        return x.TableorSheatOrTaleAwayId == CurrentOrder;
    });
    Obj.OrderList = CurrSeatingItems;
    $("#waiting").show();
    $.ajax({
        type: 'POST',
        url: "/WebApi/MakeOfflineOrd?Obj=" + JSON.stringify(Obj),
        success: function (data) {
            var JsonObj = JSON.parse(data);
            $("#waiting").hide();
            if (JsonObj.Status == 400) {
                $.alert({
                    title: 'Alert!',
                    content: JsonObj.MSG,
                });
            }
            else {
                
                CartList = CartList.filter(function (x) {
                    return x.TableorSheatOrTaleAwayId != CurrentOrder;
                });
                var JsonMsg = JsonObj.MSG.split(",");
                var PaymentSts = JsonMsg[2];
                var OrderSts = parseInt(JsonObj.OrderSts);
                if (PaymentSts != "0") {
                    ChargeAmt = 0.00;
                    ShowSeatingAmt(ChargeAmt);
                    for (var i = 0; i < TablesList.length; i++) {
                        if (TablesList[i].Table_or_RowID == CurrentOrder) {
                            TablesList[i].SeatingAmt = 0.00;
                            break;
                        }
                    }
                }
                else {
                    var finalAmt = parseFloat($("#FinalAmt").text());
                    finalAmt += parseFloat(JsonObj.DeliveryChrge);
                    ChargeAmt = ChargeAmt + finalAmt;
                    $("#FinalAmtCharge").text('CHARGE ' + ChargeAmt.toFixed(2) + ' /Rs.');
                    for (var i = 0; i < TablesList.length; i++) {
                        if (TablesList[i].Table_or_RowID == CurrentOrder) {
                            TablesList[i].SeatingAmt = ChargeAmt;
                            break;
                        }
                    }
                }
                $("tr.ItemsTr").remove();
                CurrOID = JsonMsg[0];
                if (OrderSts == 1) {
                    for (var i = 0; i < TablesList.length; i++) {
                        if (TablesList[i].Table_or_RowID == CurrentOrder) {
                            TablesList[i].CurrOID = JsonMsg[0];
                            break;
                        }
                    }
                }
                else {
                    ResetOrder(CurrentOrder, 1);
                    for (var i = 0; i < TablesList.length; i++) {
                        if (TablesList[i].Table_or_RowID == CurrentOrder) {
                            TablesList[i].CurrOID = 0;
                            break;
                        }
                    }

                }

                if (PrintInvoice == 1 && Obj.PaymtType != 0) {
                    AskNofCopyPrint(1, JsonMsg[0], JsonMsg[1]);
                }
                else {
                    if (Obj.PaymtType != 0) {
                        DisplyOrderConfirmMsg(1, JsonMsg[0], JsonMsg[1]);
                    }
                    else {
                        ConfirmOrder(JsonMsg[0], JsonMsg[1]);
                    }
                }
                // kot mode on and auto print
                if (OrderDisplay == 2 && KotPrintType == 2 && Obj.Status != 3) {
                    DisplyOrderConfirmMsg(2, JsonMsg[0], JsonMsg[1]);
                }
                else if (OrderDisplay == 2 && KotPrintType == 1 && Obj.Status != 3) {
                    AskNofCopyPrint(2, JsonMsg[0], JsonMsg[1]);
                }
            }
        },
        error: function (jqXhr, textStatus, errorMessage) { // error callback
            $("#waiting").hide();
        }
    });
}
function SUBMIT() {
    $("#SubMitbtn").find('button').css('background-color', '#000000');
    DeliveryCharge(1);
}
function INSTANTCLR() {
    $("#InstantClear").find('button').css('background-color', '#000000');
    DeliveryCharge(3);
}
function DeliveryCharge(Status) {
    var Parmas = {};
    Parmas.CID = LoginId;
    Parmas.OrgId = OrgId;
    Parmas.TSTWID = CurrentOrder;
    $.ajax({
        type: 'POST',
        url: "/WebApi/DeliveryCharge?Obj=" + JSON.stringify(Parmas),
        success: function (data) {
            var Obj = {};
            Obj.CID = LoginId;
            Obj.OrgID = OrgId;
            Obj.TORSID = CurrentOrder;
            Obj.OID = CurrOID;
            Obj.Status = Status;
            Obj.OrdingSts = 1;
            Obj.AppType = 3;
            Obj.PaymtType = 0;
            Obj.ContactId = $("#ContactId").val();
            var Msg = "Send To Chef ?";
            var ChrMsg = "Send To Chef & Charge?";
            if (Status == '3') {
                Msg = "Eleminate Chef ?";
                ChrMsg = "Eleminate Chef & Charge?";
            }
            var titlemsg = '<div class="row">';
            titlemsg += '<div class="col-md-6"><h4 style="text-align:left;">' + Msg + '</h4></div>';
            titlemsg += '<div class="col-md-6"><h4 style="text-align:right;">' + ChrMsg + '</h4></div>';
            titlemsg += '</div>';
            var JsonObj = JSON.parse(data);
            if (parseFloat(JsonObj.ChargeAmt) > 0) {
                var DeliveryType = JsonObj.DeliveryType;
                var incldmsg = '';
                var Postfix = '';
                if (DeliveryType == '1') {
                    incldmsg = 'fixed';
                }
                else {
                    Postfix = 'below Rs ' + JsonObj.MinAmt;
                }
                var DelivryMsg = 'A ' + incldmsg + ' delivery charge of Rs ' + JsonObj.ChargeAmt + ' applicable ' + Postfix;
                $.confirm({
                    title: '<p style="color:red;">' + DelivryMsg + '</p>',
                    content: titlemsg,
                    closeIcon: true,
                    boxWidth: '40%',
                    useBootstrap: false,
                    buttons: {
                        confirm: {
                            text: 'Confirm',
                            btnClass: 'btn-info LeftBtn',
                            columnClass: 'col-md-3',
                            action: function () {
                                PostOrder(Obj);
                            }
                        },
                        cancel: {
                            text: 'Cancel',
                            btnClass: 'btn-red LeftBtn',
                            keys: ['Esc', 'esc'],
                            columnClass: 'col-md-3'
                        },
                        ByWallet: {
                            text: 'Bank/Wallet',
                            btnClass: 'btn-success RightBtn',
                            columnClass: 'col-md-3',
                            action: function () {
                                Obj.PaymtType = 2;
                                PostOrder(Obj);
                            }
                        },
                        Bycash: {
                            text: 'Cash',
                            btnClass: 'btn-info RightBtn',
                            keys: ['enter', 'shift'],
                            columnClass: 'col-md-3',
                            action: function () {
                                Obj.PaymtType = 1;
                                PostOrder(Obj);
                            }
                        }
                    }
                    ,
                    onContentReady: function () {

                    },
                    onOpenBefore: function () {
                        $(".jconfirm-buttons").css('width', '100%');
                    },
                    onDestroy: function () {
                        $(".jconfirm-buttons").removeAttr('width');
                    }
                });
            }
            else {
                $.confirm({
                    title: '<p>Are You Sure</p>',
                    content: titlemsg,
                    closeIcon: true,
                    boxWidth: '40%',
                    useBootstrap: false,
                    buttons: {
                        confirm: {
                            text: 'Confirm',
                            btnClass: 'btn-info LeftBtn',
                            columnClass: 'col-md-3',
                            action: function () {
                                PostOrder(Obj)
                            }
                        },
                        cancel: {
                            text: 'Cancel',
                            btnClass: 'btn-red LeftBtn',
                            columnClass: 'col-md-3'
                        },
                        ByWallet: {
                            text: 'Bank/Wallet',
                            btnClass: 'btn-success RightBtn',
                            columnClass: 'col-md-3',
                            action: function () {
                                Obj.PaymtType = 2;
                                PostOrder(Obj);
                            }
                        },
                        ByCash: {
                            text: 'Cash',
                            btnClass: 'btn-info RightBtn',
                            keys: ['enter', 'shift'],
                            columnClass: 'col-md-3',
                            action: function () {
                                Obj.PaymtType = 1;
                                PostOrder(Obj);
                            }
                        }
                    }
                    ,
                    onContentReady: function () {
                    },
                    onOpenBefore: function () {
                        $(".jconfirm-buttons").css('width', '100%');
                    },
                    onDestroy: function () {
                        $(".jconfirm-buttons").removeAttr('width');
                    }
                });
            }
        },
        error: function (jqXhr, textStatus, errorMessage) { // error callback
            $("#waiting").hide();
        }
    });
}
function Charge() {
    $("#Charge").find('button').css('background-color', '#44cd4a');
    $.ajax({
        type: 'POST',
        url: "/WebApi/ShowOrderItems?TOrSId=" + CurrentOrder,
        success: function (data) {
            var Jobje = JSON.parse(data);
            if ($("tr.ItemsTr").length > 0) {
                $.alert({
                    title: 'Alert !',
                    content: 'please take action on selected item first',
                });
                return;
            }
            else if (Jobje.Status == 400 && Jobje.TorSsts != 1) {
                $.alert({
                    title: 'Alert !',
                    content: 'Already Charged ',
                });
                //Jobje.MSG
                return;
            }
            else if (Jobje.Status == 400 && Jobje.TorSsts == 1) {
                $.alert({
                    title: 'Alert !',
                    content: 'NO TICKETS YET',
                });
                //Jobje.MSG
                return;
            }
            else {
                FinaliZeOrd();
            }

        },
        error: function (jqXhr, textStatus, errorMessage) { // error callback
            $("#waiting").hide();
        }
    });
}
function GenInvoice(oldOtp, OID) {
    if (OID == undefined || OID == null) {
        OID = 0;
    }
    $("#PrintInvoice").html('');
    $("#waiting").show();
    $.ajax({
        type: 'POST',
        url: "/HG_Orders/PrintInvoice?TorSid=" + CurrentOrder + "&OldOtp=" + oldOtp + "&OID=" + OID,
        success: function (data) {
            $("#PrintInvoice").html(data);
            PrintElem();
            $("#waiting").hide();
            //var Jobje = JSON.parse(data);
        },
        error: function (jqXhr, textStatus, errorMessage) { // error callback
            $("#waiting").hide();
        }
    });
}
function ByCash() {
    CompleteOrder(1)// bycash
}
function ByOnline() {
    CompleteOrder(2)// by online
}
function CompleteOrder(PaymentType) {
    $.ajax({
        type: 'POST',
        url: "/WebApi/CompleteOrder?PaymentType=" + PaymentType + "&UpdatedBy=" + LoginId + "&OID=0&TorSid=" + CurrentOrder + "&AppType=3",
        success: function (data) {
            var Jobj = JSON.parse(data);
            if (Jobj.Status == 400) {
                $.alert({
                    title: 'Alert!',
                    content: Jobj.MSG,
                });
            }
            else {
                // CurrOID = 0;
                var OldOtp = 0;
                for (var i = 0; i < TablesList.length; i++) {
                    if (TablesList[i].Table_or_RowID == CurrentOrder) {
                        TablesList[i].CurrOID = 0;
                        OldOtp = TablesList[i].Otp;
                        TablesList[i].Otp = Jobj.MSG;
                        break;
                    }
                }
                $("#OtpBox" + CurrentOrder).text(Jobj.MSG);
               // OtpBox18
                if (Jobj.ChangeOtp > 0) {
                    ResetOrder(CurrentOrder, 1);
                }
                if (PrintInvoice == 1) {
                    AskNofCopyPrint(1, Jobj.OID, 0);
                }
                else {
                    OpenPrinter(1, Jobj.OID, NoOfCopy, 0);

                }
                OrderClick();
            }

        },
        error: function (jqXhr, textStatus, errorMessage) { // error callback
            $("#waiting").hide();
        }
    });
}
function OrdDetail() {
    $("#waiting").show();
    $.ajax({
        type: 'POST',
        url: "/HG_OrderItem/index?OID=" + CurrOID,
        success: function (data) {
            makedpt("CurrOrder");
            $("#CurrOrder > .modal-dialog > .modal-content > .modal-body").html(data);
            showdpt("CurrOrder");
            $("#waiting").hide();
        },
        error: function (jqXhr, textStatus, errorMessage) { // error callback
            $("#waiting").hide();
        }
    });
}
function EditOrder() {
    $("#waiting").show();
    $.ajax({
        type: 'POST',
        url: "/HG_Orders/EditOrder?OID=" + CurrOID,
        success: function (data) {
            makedpt("CurrOrder");
            $("#CurrOrder > .modal-dialog > .modal-content > .modal-body").html(data);
            showdpt("CurrOrder");
            $("#waiting").hide();
        },
        error: function (jqXhr, textStatus, errorMessage) { // error callback
            $("#waiting").hide();
        }
    });
}
function FinaliZeOrd() {
    $.confirm({
        title: 'Alert !',
        content: 'FINALIZE ORDER ?',
        buttons: {
            Bycash: {
                text: 'ByCash',
                btnClass: 'btn-info',
                action: function () {
                    ByCash();
                }
            },
            ByOnline: {
                text: 'Bank/Wallet',
                btnClass: 'btn-success',
                action: function () {
                    ByOnline();
                }
            },
            cancel: function () {
                // $.alert('Canceled!');
            }
        }
    });
}

function ChangeContactList(event) {
    var ContctID = $("#ContactId").val();
    console.log("value=" + ContctID);
    if (ContctID >= 0) {
        $("#C_save").removeAttr('disabled');
    }
    if ($(event).attr('id') == "C_mobile") {
        if ($(event).val().length == 10 && $("#C_name").val() == "") {
            GetContactNameByMobile($(event).val());
            $("#C_name").focus();
        }


    }
}
function SaveLocalContact() {
    var Mobile = $("#C_mobile").val();
    var C_name = $("#C_name").val();
    var ContctID = $("#ContactId").val();
    if (Mobile == "") {
        alert("Mobile No required");
        return;
    }
    if (Mobile != "" && (Mobile.length < 10 || Mobile.length > 10)) {
        alert("Mobile Number Must Be 10 Digit");
        return;
    }
    if (ContctID >= 0) {
        $("#waiting").show();
        $.ajax({
            type: 'POST',
            url: "/CommonApi/SaveLocalContact?Mobile=" + Mobile + "&Cname=" + C_name + "&ContctID=" + ContctID,
            success: function (data) {
                var Jobj = JSON.parse(data);
                if (Jobj.Status == 200) {
                    $("#ContactId").val(Jobj.ContactId);
                    $("#C_save").attr('disabled', 'disabled');
                }
                $("#waiting").hide();
            },
            error: function (jqXhr, textStatus, errorMessage) { // error callback
                $("#waiting").hide();
            }
        });
    }

}
function GetContactNameByMobile(MobileNo) {
    $.ajax({
        type: 'POST',
        url: "/CommonApi/GetNameByMobileNo?MobileNo=" + MobileNo,
        success: function (data) {
            var Jobj = JSON.parse(data);
            if (Jobj.Status == 200) {
                $("#C_name").val(Jobj.CName);
            }
        },
        error: function (jqXhr, textStatus, errorMessage) { // error callback
        }
    });
}
function ConfirmOrder(OID, TicketNo) {
    $.confirm({
        title: "Alert",
        content: 'Order No: ' + OID + ' & Ticket No ' + TicketNo,
        buttons: {
            Ok: {
                text: 'OK',
                btnClass: 'btn-info',
                action: function () {

                }
            },

        }
    });
}
function DisplyOrderConfirmMsg(Type, OID, TicketNo) {
    console.log(" DisplyOrderConfirmMsg Type=" + Type)
    var Msg = "";
    if (Type == 2 && OrderDisplay == 2) {
        OpenPrinter(Type, OID, KotNoOfCopy, TicketNo);
        return;
        //Msg = "Print Kot";
    }
    else if (Type == 1) {
        if (PrintInvoice == 2) {
            OpenPrinter(Type, OID, NoOfCopy, TicketNo);
        }
        Msg = "Print Invoice";
    }
    $.confirm({
        title: Msg,
        content: 'Order No: ' + OID + ' & Ticket No ' + TicketNo,
        buttons: {
            Ok: {
                text: 'OK',
                btnClass: 'btn-info',
                action: function () {


                }
            },

        }
    });
}
function AskNofCopyPrint(Type, OID, Ticket) {
    console.log(" AskNofCopyPrint Type=" + Type);
    var Msg = "";
    var ContentMsg = 'Order No: ' + OID;
    if (Ticket > 0) {
        ContentMsg += ' & Ticket No ' + Ticket;
    }
    if (Type == 1) {
        Msg = "Print Invoice";
    }
    else if (Type == 2) {
        Msg = "Print Kot";
    }
    $.confirm({
        title: Msg,
        content: ContentMsg,
        closeIcon: true,
        buttons: {
            Zero: {
                text: '0',
                btnClass: 'btn-info btn-lg PrintBtnWidth',
                keys: ['0'],
                columnClass: 'col-md-3',
                action: function () {

                }
            },
            One: {
                text: '1',
                btnClass: 'btn-info btn-lg PrintBtnWidth',
                columnClass: 'col-md-3',
                keys: ['1'],
                action: function () {
                    OpenPrinter(Type, OID, 1, Ticket);
                }
            },
            Two: {
                text: '2',
                btnClass: 'btn-info btn-lg PrintBtnWidth',
                keys: ['2'],
                columnClass: 'col-md-3',
                action: function () {
                    OpenPrinter(Type, OID, 2, Ticket);
                }
            },
            Three: {
                text: '3',
                btnClass: 'btn-info btn-lg PrintBtnWidth',
                columnClass: 'col-md-3',
                keys: ['3'],
                action: function () {
                    OpenPrinter(Type, OID, 3, Ticket);
                }
            },
        },
        onOpenBefore: function () {
            $(".jconfirm-buttons").css('width', '100%');
        },
        onDestroy: function () {
            $(".jconfirm-buttons").removeAttr('width');
        }
    });
}
function OpenPrinter(Type, OID, NoOfCpy, TicketNo) {
    window.location.href = "foodDo:" + Type + "&" + OID + "&" + NoOfCpy + "&" + TicketNo;
    return;
}
GetPrinterSetting();
function GetPrinterSetting() {
    $.ajax({
        type: 'POST',
        url: "/CommonApi/OrgSettng",
        success: function (data) {
            var Jobj = JSON.parse(data);
            PrintInvoice = Jobj.InvoicePrint;
            NoOfCopy = Jobj.InvoiceNoOfCopy;
            OrderDisplay = Jobj.OrdDisaply;
            KotPrintType = Jobj.KotPrint;
            KotNoOfCopy = Jobj.NoOfCopy;
            ParcelCharge = parseFloat(Jobj.ParcelAmt);
        },
        error: function (jqXhr, textStatus, errorMessage) { // error callback
            $("#waiting").hide();
        }
    });
}
function AddChargeDiscnt(Type) {
  
    $("#waiting").show();
    $.ajax({
        type: 'GET',
        url: "/HG_Orders/DiscntCharges?SeatingId=" + CurrentOrder + "&Type=" + Type,
        success: function (data) {
            makedpt("DiscntCharge", 400, 400);
            $("#DiscntCharge > .modal-dialog > .modal-content > .modal-body").html(data);
            showdpt("DiscntCharge");
            $("#waiting").hide();
        },
        error: function (jqXhr, textStatus, errorMessage) { // error callback
            $("#waiting").hide();
        }
    });
}
function AddonInfo(ItmUUID, ItemId) {
    //$.ajax({
    //    type: 'POST',
    //    url: "/CommonApi/GetAddonsItems?UUID=" + ItmUUID,
    //    contentType: "application/json",
    //    success: function (data) {
    var HashAddinIds = [];
    var Cart = CartList.find(x => {
        return x.ItemUUID == ItmUUID;
    });
    for (var i = 0; i < Cart.itemAddons.AddonItemId.length; i++) {
        HashAddinIds.push(Cart.itemAddons.AddonItemId[i]);
    }
    console.log("addon info=" + HashAddinIds);
    // HashAddinIds = JSON.parse(data);
    var ObjItem = ItemList.find(x => {
        return x.IID.toString() === ItemId
    });
    var addonlist = ObjItem.Addons;
    var AddonItem = [];
    for (var i = 0; i < addonlist.length; i++) {
        AddonItem.push(...addonlist[i].AddOnItemList);
    }
    var onlySelectedAddons = AddonItem.filter(function (itm) {
        return HashAddinIds.indexOf(itm.AddOnItemId.toString()) > -1;
    });
    var strhtml = "";
    strhtml += "<div class='card'>";
    strhtml += "<div class='card-header'> <h3 style='text-align:center'> Addon Items</h3></div>";
    strhtml += '<div class="card-body col-md-12" id="Items" style="margin-top:10px;">'
    for (var j = 0; j < onlySelectedAddons.length; j++) {
        strhtml += '<div class="row" style="border:1px solid black;">';
        //item 
        strhtml += '<div class="col-md-6">';
        // strhtml += '<label></label>';
        strhtml += '<input type="text" readonly value="' + onlySelectedAddons[j].Title + '" class="form-control" />';
        strhtml += '</div>';
        //amt
        strhtml += '<div class="col-md-6">';
        //  strhtml += '  <label></label>';
        strhtml += '<input type="text" id="Price' + onlySelectedAddons[j].AddOnItemId + '" readonly value="' + onlySelectedAddons[j].Price.toFixed(2) + '" class="form-control" />';
        strhtml += '</div>';
        strhtml += ' </div>';
    }
    strhtml += ' </div>';
    strhtml += ' </div>';
    makedpt("ShowAddedItem", 200, 300);
    $("#ShowAddedItem > .modal-dialog > .modal-content > .modal-body").html(strhtml);
    showdpt("ShowAddedItem");
    // },
    //    error: function (jqXhr, textStatus, errorMessage) { // error callback
    //        $("#waiting").hide();
    //    }
    //});

}

 