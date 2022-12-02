function MyTaskClick() {
    ctaskgrid.Refresh();
    cMyTaskPopup.Show();
}


function PopupOpen(id,status){
    console.log(id);
    console.log(status);

        var OtherDetails = {}
        OtherDetails.id = id;
        $.ajax({
            type: "POST",
            url: "MainDasBoard.aspx/MarkComplete",
            data: JSON.stringify(OtherDetails),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var returnObject = msg.d;
                if (returnObject.status == "Ok")
                    ctaskgrid.Refresh();
                else
                    alert(returnObject.status);
            }
        });
}