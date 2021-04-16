
function ShowDialog(message) {

    BootstrapDialog.show({
        closable: false,
        size: BootstrapDialog.SIZE_SMALL,
        type: BootstrapDialog.TYPE_DANGER,
        title: 'RAM Mobile Operations',
        message: message,
        buttons: {
            danger: {
                label: "Close",
                cssClass: "btn-danger",
                action: function (dialog) {
                    dialog.close();
                }
            }
        }
    });
}