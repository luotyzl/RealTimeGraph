function StartRamping() {
    var postUrl = WebSiteBaseUrl + "Temperature/StartRamping";
    var targetTemp = document.getElementById("TargetTemp").value;
    var rampRate = document.getElementById("RampRate").value;
    var settlingBand = document.getElementById("SettlingBand").value;
    var settlingTime = document.getElementById("SettlingTime").value;
    var dataToPost = {
        "targetTemp": targetTemp,
        "rampRate": rampRate,
        "settlingBand": settlingBand,
        "settlingTime": settlingTime
    };
    $.ajax({
        url: postUrl,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        processData: false,
        dataType: "json",
        data: JSON.stringify(dataToPost),
        success: function(resp) {
            bootbox.alert(resp);
        },
        error: function(resp) {
            console.WriteLine(resp);
        }
    });
}

function GetCurrentTemperature() {
    jQuery.ajax({
        url: WebSiteBaseUrl + "Temperature/GetTemperature",
        type: "get",
        contenttype: "application/json; charset=utf-8",
        dataType: "json",
        success: function(resp) {
            return resp.Number;
        },
        error: function(resp) {
            console.WriteLine(resp);
        }
    });
}

function CancelRamping() {
    jQuery.ajax({
        url: WebSiteBaseUrl + "Temperature/CancelRamping",
        type: "get",
        contenttype: "application/json; charset=utf-8",
        dataType: "json",
        success: function(resp) {
            bootbox.alert(resp);
        },
        error: function(resp) {
            console.WriteLine(resp);
        }
    });
}

function displayIndicator(current, nextTemp) {
    var fixedTemp = Number(current.toFixed(2));
    var fixedNextTemp = Number(nextTemp.toFixed(2));
    if (fixedNextTemp > fixedTemp) {
        $("#upIndicator").css("text-decoration", "underline");
        $("#downIndicator").css("text-decoration", "none");
        $("#maintainIndicator").css("text-decoration", "none");
    } else if (fixedNextTemp < fixedTemp) {
        $("#upIndicator").css("text-decoration", "none");
        $("#downIndicator").css("text-decoration", "underline");
        $("#maintainIndicator").css("text-decoration", "none");
    } else {
        $("#upIndicator").css("text-decoration", "none");
        $("#downIndicator").css("text-decoration", "none");
        $("#maintainIndicator").css("text-decoration", "underline");
    }
}

function Num(obj) {
    obj.value = obj.value.replace(/[^\d.]/g, "");
    obj.value = obj.value.replace(/^\./g, "");
    obj.value = obj.value.replace(/\.{2,}/g, ".");
    obj.value = obj.value.replace(".", "$#$").replace(/\./g, "").replace("$#$", ".");
    obj.value = obj.value.replace(/^(\-)*(\d+)\.(\d\d).*$/, "$1$2.$3");
}