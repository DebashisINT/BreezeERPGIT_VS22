<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="view-version-features.aspx.cs" Inherits="ERP.OMS.Management.Master.view_version_features" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="https://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.12.4.min.js"></script>
    <script type="text/ecmascript">
        $(document).ready(function () {
            btnSave_Click("2");
        });
        function btnSave_Click(flag) {
            $.ajax({
                type: "POST",
                url: "view-version-features.aspx/addversionfeatures",
                data: JSON.stringify({ "VersionNumber": "", "FeaturesMarkup": "", "flag": flag }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                global: false,
                async: false,
                success: function (msg) {
                    if (msg.d) {
                        if (flag == "2") {
                            $("#features").html(msg.d);
                            return false;
                        }
                        

                    }
                },
                error: function (response) {
                    console.log(response);
                }
            });

        }

    </script>
</head>

<body>
    <form id="form1" runat="server">
    <div id="features">
    </div>
    </form>
</body>
</html>
