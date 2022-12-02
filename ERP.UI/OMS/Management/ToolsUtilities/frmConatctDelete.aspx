<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.ToolsUtilities.management_ToolsUtilities_frmConatctDelete" CodeBehind="frmConatctDelete.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <%--   <link href="../../css/style.css" rel="stylesheet" type="text/css" />
    <!--___________________These files are for List Items__________________________-->

    <script type="text/javascript" src="/assests/js/init.js"></script>

    <script type="text/javascript" src="/assests/js/ajax-dynamic-list.js"></script>--%>

    <!--___________________________________________________________________________-->


    <script type="text/javascript">
        FieldName = 'btnSave';

        //function SignOff() {
        //    window.parent.SignOff();
        //}
        //function height() {
        //    if (document.body.scrollHeight >= 600)
        //        window.frameElement.height = document.body.scrollHeight;
        //    else
        //        window.frameElement.height = '600px';
        //    window.frameElement.Width = document.body.scrollWidth;
        //}
        function showOptions(obj1, obj2, obj3) {


            var c = document.getElementById("ddlText").value;
            var d = document.getElementById("ddlValue").value;
            var obj4 = c + '~' + d;
            if (c != '' && d != '') {
                ajax_showOptions(obj1, obj2, obj3, obj4);
            }
            else {
                alert('Please Select Contact Type...!!')
            }
        }
        function Validate(obj) {
            alert(obj);
            return false;

        }

    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Contact Delete</h3>
        </div>
    </div>
    <div class="form_main">
        <table class="TableMain100">
            <tr>
                <td style="vertical-align: top; padding-left: 30px;">
                    <div class="crossBtn"><a href='<%= Page.ResolveUrl("~/OMS/Management/ProjectMainPage.aspx")%>'><i class="fa fa-times"></i></a></div>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <%--  <asp:Panel ID="Panel1" runat="server" Width="99%" Visible="False">--%>
                            <table width="100%">
                                <tr>
                                    <td style="text-align: left; width: 127px;">
                                        <asp:Label ID="Label1" runat="server" Text="Choose By" Font-Bold="True"></asp:Label>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList ID="DDLAddData" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DDLAddData_SelectedIndexChanged"
                                            Width="230px">
                                        </asp:DropDownList>
                                    </td>
                                    <td>Select Contact:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtReportTo" runat="server" Width="300px"></asp:TextBox>
                                        <asp:HiddenField ID="txtReportTo_hidden" runat="server" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnSave" runat="server" Text="Delete Contact" CssClass="btnUpdate btn btn-primary" OnClick="btnSave_Click" />

                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="text-align: left" visible="false">
                                        <asp:HiddenField ID="txtID" runat="server" />
                                        <asp:HiddenField ID="MType" runat="server" />
                                        <asp:HiddenField ID="ddlValue" runat="server" />
                                        <asp:HiddenField ID="ddlText" runat="server" />
                                        <%-- <asp:ListBox ID="LLbAddData" runat="server" Width="100%" Height="266px" SelectionMode="Multiple" Visible="false"></asp:ListBox>--%>
                                        <br />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click"></asp:AsyncPostBackTrigger>
                        </Triggers>
                    </asp:UpdatePanel>

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
        </table>
    </div>
</asp:Content>
