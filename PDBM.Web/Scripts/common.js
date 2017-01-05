mini_debugger = false;

function ShowSucessMessage(message) {
    mini.showTips({
        content: "<b>" + message + "</b>",
        state: "info",
        x: "center",
        y: "center",
        timeout: 1000
    });
}

$(document).ajaxError(function (evt, xhr, settings) {
    var errorPage = xhr.responseText;
    alert("系统异常：" + $.trim($("#ErrorMsg", errorPage).html()));
});

$(document).ajaxComplete(function (evt, xhr, settings) {
    var jsonResult = xhr.responseText;
    var result = mini.decode(jsonResult);
    if (result.Code != null) {
        if (result.Code == -1) {
            mini.showMessageBox({
                showHeader: true,
                width: 260,
                title: "错误提示",
                buttons: ["ok"],
                message: result.Message,
                iconCls: "mini-messagebox-error"
            });
        }
        else if (result.Code == -2) {
            top["main"].ShowSignOut(result.Message, result.ReturnUrl);
        }
    }
});

AddAntiForgeryToken = function (data) {
    data.__RequestVerificationToken = $('#__AjaxAntiForgeryForm input[name=__RequestVerificationToken]').val();
    return data;
}

function OpenPrintWindow(url) {
    window.open(url, "_blank", "height=500px, width=850px, scrollbars=yes, resizable=yes");
}