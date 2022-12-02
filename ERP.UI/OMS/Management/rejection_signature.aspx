<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.management_rejection_signature" CodeBehind="rejection_signature.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />

    <script type="text/javascript" src="/assests/js/loaddata1.js"></script>

    <script type="text/javascript" src="/assests/js/init.js"></script>

    <script type="text/javascript" src="/assests/js/ajaxList_wofolder.js"></script>

    <style type="text/css">
        /* Big box with list of options */
        #ajax_listOfOptions {
            position: absolute; /* Never change this one */
            width: 50px; /* Width of box */
            height: auto; /* Height of box */
            overflow: auto; /* Scrolling features */
            border: 1px solid Blue; /* Blue border */
            background-color: #FFF; /* White background color */
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
            }

        #ajax_listOfOptions_iframe {
            background-color: #F00;
            position: absolute;
            z-index: 5;
        }

        form {
            display: inline;
        }
    </style>

    <script language="javascript" type="text/javascript">
        function pageclose() {
            //                var x=window.confirm("Do you want to close the window?");
            //                if(x)
            //                {
            parent.editwin.close();
            //                }
            //                else
            //                {
            //                 document.getElementById('txtisin').focus();
            //                }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div align="center">
        <table>
            <tr>
                <td style="width: 100px">Reject Reason</td>
                <td colspan="2" rowspan="2">
                    <asp:TextBox ID="txtreject" runat="server" Height="62px" TextMode="MultiLine" Width="261px"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="width: 100px"></td>
            </tr>
            <tr>
                <td style="width: 100px"></td>
                <td style="width: 100px">
                    <asp:Button ID="btnreject" runat="server" OnClick="btnreject_Click" Text="Reject" /></td>
                <td style="width: 100px"></td>
            </tr>
        </table>

    </div>
</asp:Content>
