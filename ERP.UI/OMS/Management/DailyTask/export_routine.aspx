<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.DailyTask.management_DailyTask_export_routine" CodeBehind="export_routine.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<%@ Register Assembly="DevExpress.Web.v15.1" Namespace="DevExpress.Web"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1" Namespace="DevExpress.Web"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1" Namespace="DevExpress.Web"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe000001" %>--%>
    

    

    <script type="text/javascript" src="/assests/js/init.js"></script>

    <script type="text/javascript" src="/assests/js/ajaxList.js"></script>

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
        var FieldName = 'a';
        function SignOff() {
            window.parent.SignOff();
        }
        function PageLoad() {
            dtexec1.SetDate(new Date());
        }

        function height() {
            //alert('svsafvgase');
            if (document.body.scrollHeight >= 500) {
                window.frameElement.height = document.body.scrollHeight;
            }
            else {
                window.frameElement.height = '500px';
            }
            window.frameElement.width = document.body.scrollWidth;
        }
        function BatchNo() {
            var textboxvalue = document.getElementById("txtbatch").value;
            if (textboxvalue != '') {
                document.getElementById("txtbatch").value = '';
            }

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div align="center">
        <table border="1">
            <tr>
                <td style="width: 100px">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <table border="1">
                                <tr>
                                    <td align="left" colspan="3" style="font-weight: bold" valign="top">
                                        <div class="EHEADER" style="text-align: center">
                                            <strong><span id="Span1" style="color: #000099">Batch Generate</span></strong>&nbsp;
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" style="vertical-align: top; width: 20%; height: 11px; background-color: #b7ceec; text-align: left;" valign="top">Transaction Type</td>
                                    <td valign="top" style="text-align: left">&nbsp;<asp:DropDownList ID="DropDownList1" runat="server" Width="192px">
                                        <asp:ListItem>Transaction</asp:ListItem>
                                        <asp:ListItem>Inter-Depository</asp:ListItem>
                                        <asp:ListItem>Early Pay-in</asp:ListItem>
                                        <asp:ListItem>BO Confirmation</asp:ListItem>
                                    </asp:DropDownList></td>
                                    <td style="width: 100px"></td>
                                </tr>
                                <tr>
                                    <td align="left" style="height: 17px; vertical-align: top; width: 20%; background-color: #b7ceec; text-align: left;" valign="top">Transaction Date</td>
                                    <td valign="top" style="height: 17px; text-align: left;">
                                        <dxe:ASPxDateEdit ID="txtExecDate" runat="server" AllowNull="False" ClientInstanceName="dtexec1"
                                            EditFormat="Custom" EditFormatString="dd-MM-yyyy" UseMaskBehavior="True"
                                            Width="190px">
                                            <ButtonStyle Width="13px"></ButtonStyle>
                                        </dxe:ASPxDateEdit>
                                    </td>
                                    <td valign="top" style="height: 17px">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td align="left" colspan="3">

                                        <asp:GridView ID="gridSummary" runat="server" AutoGenerateColumns="False" BackColor="PeachPuff"
                                            BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" EmptyDataText="No Record Found."
                                            Width="644px">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Select Batch To Generate">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chb1" runat="server" Checked="True" />
                                                    </ItemTemplate>
                                                    <ControlStyle Width="10%" />
                                                    <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Batchnumber" HeaderText="Batch Number" />
                                                <asp:BoundField DataField="TransactionType" HeaderText="Transactions Type" />
                                                <asp:BoundField DataField="count" HeaderText="Number of Instructions" />
                                                <asp:BoundField DataField="BatchDateTime" HeaderText="Batch Export DateTime" />
                                            </Columns>
                                        </asp:GridView>

                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" style="vertical-align: top; width: 20%; height: 17px; background-color: #b7ceec; text-align: left;">Batch No.</td>
                                    <td valign="top" style="text-align: left">
                                        <asp:TextBox ID="txtbatch" runat="server" Width="181px" MaxLength="5"></asp:TextBox></td>
                                    <td valign="top"></td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Button ID="btnshow" runat="server" CssClass="btnUpdate" Height="20px" OnClick="btnshow_Click"
                                            Text="Show" Width="100px" /></td>
                                    <td align="left">
                                        <asp:Button ID="btncreate" runat="server" CssClass="btnUpdate" OnClick="btncreate_Click" Text="Create" ValidationGroup="a" Height="20px" Width="101px" /></td>
                                    <td style="text-align: left"></td>
                                </tr>
                            </table>
                            &nbsp;
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btncreate" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnshow" EventName="Click" />


                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td style="width: 100px">
                    <asp:Button ID="Download" runat="server" CssClass="btnUpdate" Height="20px" OnClick="Download_Click"
                        Text="Download File" Width="100px" /></td>
            </tr>
        </table>
        &nbsp; &nbsp;&nbsp;
    </div>
</asp:Content>
