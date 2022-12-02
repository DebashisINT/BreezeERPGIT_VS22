<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.DailyTask.management_DailyTask_CarryForwardStocks" CodeBehind="CarryForwardStocks.aspx.cs" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../CSS/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="/assests/js/init.js"></script>
    <script type="text/javascript" src="/assests/js/ajaxList.js"></script>
    <script type="text/javascript" src="/assests/js/jquery-1.3.2.js"></script>
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
            z-index: 32767;
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
            z-index: 3000;
        }

        form {
            display: inline;
        }
    </style>

    <script language="javascript" type="text/javascript">
        function Page_Load()///Call Into Page Load
        {
            FnDdlType("1");
            //height();
        }
        //function height() {
        //    if (document.body.scrollHeight >= 250) {
        //        window.frameElement.height = document.body.scrollHeight;
        //    }
        //    else {
        //        window.frameElement.height = '250px';
        //    }
        //    window.frameElement.width = document.body.scrollWidth;
        //}
        function Hide(obj) {
            document.getElementById(obj).style.display = 'none';
        }
        function Show(obj) {
            document.getElementById(obj).style.display = 'inline';
        }
        function FnDdlType(obj) {
            if (obj == "2") {
                Show('Tr_Client');
            }
            else {
                Hide('Tr_Client');
            }
        }
        function FnGenerate(obj) {

            FnDdlType(document.getElementById('ddlType').value);
            if (obj == "1")
                alert('Generate Successfully !!');
            if (obj == "2")
                alert('Please Select Client !!');
            //height();
        }
        FieldName = 'lstSlection';

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <script language="javascript" type="text/javascript">
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_initializeRequest(InitializeRequest);
            prm.add_endRequest(EndRequest);
            var postBackElement;
            function InitializeRequest(sender, args) {
                if (prm.get_isInAsyncPostBack())
                    args.set_cancel(true);
                postBackElement = args.get_postBackElement();
                $get('UpdateProgress1').style.display = 'block';
            }
            function EndRequest(sender, args) {
                $get('UpdateProgress1').style.display = 'none';

            }
        </script>

        <table class="TableMain100">
            <tr>
                <td class="EHEADER" colspan="0" style="text-align: center; height: 20px;">
                    <strong><span id="SpanHeader" style="color: #000099">Carry Forward of Stocks</span></strong></td>
            </tr>
        </table>
        <table>
            <tr>
                <td class="gridcellleft">
                    <table border="10" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="gridcellleft" bgcolor="#B7CEEC">Type :
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlType" runat="server" Width="150px" Font-Size="12px" onchange="FnDdlType(this.value)">
                                    <asp:ListItem Value="1">Margin/HoldBack Stock</asp:ListItem>
                                    <asp:ListItem Value="2">Own Stock</asp:ListItem>
                                    <asp:ListItem Value="3">Obligation</asp:ListItem>

                                </asp:DropDownList></td>
                        </tr>
                        <tr id="Tr_Client">
                            <td class="gridcellleft" bgcolor="#B7CEEC">Select Client :
                            </td>
                            <td>
                                <asp:TextBox ID="txtClient" runat="server" Width="200px" Font-Size="12px" onkeyup="ajax_showOptions(this,'ShowClientFORMarginStocks',event,'ProClients')"></asp:TextBox></td>

                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="btnshow" runat="server" CssClass="btnUpdate" Height="20px" Text="Generate"
                                            Width="101px" OnClick="btnshow_Click" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                                    <ProgressTemplate>
                                        <div id='Div1' style='position: absolute; font-family: arial; font-size: 30; left: 50%; top: 50%; background-color: white; layer-background-color: white; height: 80; width: 150;'>
                                            <table width='100' height='35' border='1' cellpadding='0' cellspacing='0' bordercolor='#C0D6E4'>
                                                <tr>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td height='25' align='center' bgcolor='#FFFFFF'>
                                                                    <img src='/assests/images/progress.gif' width='18' height='18'></td>
                                                                <td height='10' width='100%' align='center' bgcolor='#FFFFFF'>
                                                                    <font size='1' face='Tahoma'><strong align='center'>Please Wait..</strong></font></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="display: none;">
                                <asp:TextBox ID="txtClient_hidden" runat="server" Width="5px"></asp:TextBox></td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
