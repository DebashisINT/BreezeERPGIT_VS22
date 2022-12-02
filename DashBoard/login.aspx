<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="DashBoard.DashBoard.login" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="stylesheet"  href="https://cdnjs.cloudflare.com/ajax/libs/jquery-confirm/3.3.2/jquery-confirm.min.css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.4.1/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.24.0/moment.min.js" integrity="sha256-4iQZ6BVL4qNKlQ27TExEhBN1HFPvAvAMbFavKKosSWQ=" crossorigin="anonymous"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.7/umd/popper.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/mobile-detect@1.4.4/mobile-detect.min.js"></script>
    
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery-confirm/3.3.2/jquery-confirm.min.js"></script>
    <title></title>
    <script type="text/javascript">
        $(document).ready(function () {
            var md = new MobileDetect(window.navigator.userAgent);
            if (md.mobile()) {
                console.log('Mobile Name'+ md.mobile());          
                console.log('Phone' + md.phone());
                console.log('Is Tablet' + md.tablet());
                console.log(md.userAgent());       
                console.log(md.os());             
                console.log('Is iphone' + md.is('iPhone'));
                console.log(md.is('bot'));         
                console.log(md.version('Webkit'));         
                console.log(md.versionStr('Build'));      
                console.log(md.match('playstation|xbox'));

                $.confirm({
                    title: 'What do you want to visit',
                    content: 'Choose one to continue',
                    buttons: {
                        Report: {
                            text: 'Reports only',
                            btnClass: 'btn-purple',
                            keys: ['enter', 'shift'],
                            action: function () {
                                $('#ismobile').val('1');
                            }
                        },
                        Erp: {
                            text: 'ERP Site',
                            btnClass: 'btn-dark',
                            keys: ['enter', 'shift'],
                            action: function () {
                                $('#ismobile').val('0');
                            }
                        }
                    }
                });
               

            } else {
                $('#ismobile').val('0');
            }
           
        })
        
    </script>
</head>
<body> 
    <form id="form1" runat="server">
    <div>
        <asp:TextBox ID="userName" runat="server"></asp:TextBox>
        <asp:TextBox ID="txtPass" runat="server"></asp:TextBox>
        <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click"/>
        <input type="hidden" id="ismobile" runat="server" />
    </div>
    </form>
</body>
</html>
