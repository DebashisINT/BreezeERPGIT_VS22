<%@ Page Language="C#" AutoEventWireup="true" Inherits="ERP.OMS.management_ajax_list" CodeBehind="ajax_list.aspx.cs" %>

<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
    </div>
</asp:Content>--%>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script type="text/javascript">
        function HideShow(obj, HS) {
            if (HS == "H")
                document.getElementById(obj).style.display = "None";
            else
                document.getElementById(obj).style.display = "inline";
        }
        function GetObjectID(obj) {
            return document.getElementById(obj);
        }
        FieldName = '';
        window.history.forward();
        function noBack() { window.history.forward(); }

        function AddFavourite() {
            document.getElementById('BtnFavourite').click();
        }

        function OnMoreInfoClick(url, HeaderText, Width, Height, anyKey) //AnyKey will help to call back event to child page, if u have to fire more that one function 
        {
            editwin = dhtmlmodal.open("Editbox", "iframe", url, HeaderText, "width=" + Width + ",height=" + Height + ",center=1,resize=1,scrolling=2,top=500", "recal")
            editwin.onclose = function () {
                if (anyKey == 'Y') {
                    //document.getElementById('IFRAME_ForAllPages').contentWindow.callback();
                }
            }
        }

    </script>
    <style type="text/css">
        /* Big box with list of options */
        #ajax_listOfOptions {
            position: absolute; /* Never change this one */
            width: 50px; /* Width of box */
            height: auto; /* Height of box */
            overflow: auto; /* Scrolling features */
            border: 1px solid Blue; /* Blue border */
            background-color: #FFF; 
            text-align: left;
            font-size: 0.9em;
            z-index: 100;
        }

            #ajax_listOfOptions div { /* General rule for both .optionDiv and .optionDivSelected */
                margin: 1px;
                padding: 1px;
                cursor: pointer;
                font-size: 0.9em;
            }

            #ajax_listOfOptions .optionDiv { /* Div for each item in list */
            }

            #ajax_listOfOptions .optionDivSelected { /* Selected item in the list */
                background-color: #DDECFE;
                color: Blue;
                z-index: 900;
            }

        #ajax_listOfOptions_iframe {
            background-color: #F00;
            position: absolute;
            z-index: 10;
        }

        form {
            display: inline;
        }
    </style>
</head>
<body>
    
</body>
</html>
