<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="ERP.OMS.Reports.management_ContactDetailsIframe" Codebehind="ContactDetailsIframe.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Contact Details</title>
        <script language="javascript" type="text/javascript">
          function height()
        {
            if(document.body.scrollHeight>=350)
            {
                window.frameElement.height = document.body.scrollHeight;
            }
            else
            {
                window.frameElement.height = '350px';
            }
            window.frameElement.width = document.body.scrollwidth;
        }
        </script>
</head>
<body >
    <form id="form1" runat="server">
        <div>
            <table width="100%">
                <tr>
                 <td align="center" style="width: 100px; vertical-align: middle; background-color: #FFFFFF;">
                                                      <a href="javascript:window.print()">  <img src="/assests/images/logo.jpg" width="261" height="61" alt="Click To Print" /></a></td>
                    
                </tr>
                <tr>
                    <td>
                        <div id="MainContainer" runat="server" style="font-family: verdana; font-size: 12px;">
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
