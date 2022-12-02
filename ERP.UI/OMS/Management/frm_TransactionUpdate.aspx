<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true"
    Inherits="ERP.OMS.Management.management_frm_TransactionUpdate" CodeBehind="frm_TransactionUpdate.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../CSS/style.css" rel="stylesheet" type="text/css" />

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
            z-index: 32767;
        }

            #ajax_listOfOptions div { /* General rule for both .optionDiv and 

.optionDivSelected */
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
        FieldName = 'BtnSave';
        function InsurerCompany(obj1, obj2, obj3, obj4) {
            ajax_showOptions(obj1, obj2, obj3, obj4, 'Main');
        }
        function SignOff() {
            window.parent.SignOff();
        }
        function height() {
            if (document.body.scrollHeight >= 500)
                window.frameElement.height = document.body.scrollHeight;
            else
                window.frameElement.height = '500px';
            window.frameElement.Width = document.body.scrollWidth;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div align="center">
        <table class="TableMain100">
            <tr>
                <td class="EHEADER">
                    <span style="color: blue"><strong>Transaction IMPORT</strong></span></td>
            </tr>
        </table>
        <asp:Panel ID="Panelmain" runat="server" Visible="true" HorizontalAlign="Center">
            <table id="tbl_main" class="login" cellspacing="0" cellpadding="0" width="510" height="300">
                <tbody>
                    <tr>
                        <td class="lt">
                            <table class="width100per" cellspacing="12" cellpadding="0">
                                <tbody>
                                    <tr>
                                        <td align="left">Insurance Company:
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtInsurerCompany" runat="server" Width="325px" CssClass="EcoheadCon"
                                                TabIndex="0"></asp:TextBox>
                                            <asp:HiddenField ID="txtInsurerCompany_hidden" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Upload CSV File Only:
                                        </td>
                                        <td align="right" style="width: 278px" colspan="2">
                                            <asp:FileUpload ID="NCDEXSelectFile" runat="server" Width="325px" Height="21px" TabIndex="1" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px">
                                            <asp:Label ID="lblMsgAccCode" Width="120px" ForeColor="Red" runat="server"></asp:Label></td>
                                        <td align="left" valign="middle">
                                            <table cellspacing="0" cellpadding="0">
                                                <tbody>
                                                    <tr>
                                                        <td valign="top" align="left" style="height: 19px">
                                                            <asp:Button ID="BtnSave" runat="server" Text="Import File" CssClass="btn" Width="84px"
                                                                OnClick="BtnSave_Click" TabIndex="2" />
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                </tbody>
            </table>
        </asp:Panel>
    </div>
</asp:Content>
