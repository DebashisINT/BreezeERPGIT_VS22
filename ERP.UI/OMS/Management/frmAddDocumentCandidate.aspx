<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.management_frmAddDocumentCandidate" CodeBehind="frmAddDocumentCandidate.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe000001" %>--%>
    <link href="../CSS/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function OnCloseButtonClick(s, e) {
            var parentWindow = window.parent;
            parentWindow.popup.Hide();
        }
        function Validation() {
            var FName = document.getElementById('TxtName');
            if (FName.value == '') {
                alert('File Name Required!');
                return false;
                document.getElementById('TxtName').focus();
            }
            var Upload_Image = document.getElementById('FileUpload1');
            if (Upload_Image.value == '') {
                var fileNo = document.getElementById('TxtfileNo');
                if (fileNo.value == '') {
                    alert('Number Required!');
                    return false;
                }
                else {
                    var cellNo = document.getElementById('TxtCellNo');
                    if (cellNo.value == '') {
                        alert('Cell/Cabinet No Required!');
                        return false;
                    }
                    else {
                        var RoomNo = document.getElementById('TxtRoomNo');
                        if (RoomNo.value == '') {
                            alert('Room No Required!');
                            return false;
                        }
                        else {
                            var FloorNo = document.getElementById('TxtFloorNo');
                            if (FloorNo.value == '') {
                                alert('Floor No Required!');
                                return false;
                            }
                            else {
                                var Building = document.getElementById('Building');
                                if (Building.value == 'Select') {
                                    alert('Building Required!');
                                    return false;
                                }
                                else {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Document Type</h3>
        </div>
        <div class="crossBtn" >
            <asp:HyperLink ID="goBackCrossBtn" NavigateUrl="#" runat="server">        
               <i class="fa fa-times"></i>
            </asp:HyperLink>
        </div>
    </div>
    <div class="form_main">
        <table class="TableMain100">
            <tr>
                <td class="Ecoheadtxt" style="width: 118px">Document Type</td>
                <td style="width: 173px">
                    <asp:DropDownList ID="DTYpe" runat="server" Width="156px">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Select Document Type" ControlToValidate="DTYpe" ValidationGroup="a"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="Ecoheadtxt" style="width: 118px">File Name</td>
                <td style="width: 173px">
                    <asp:TextBox ID="TxtName" runat="server"></asp:TextBox></td>
                <td>
                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="TxtName"
                        ErrorMessage="File Name Required" ValidationGroup="a"></asp:RequiredFieldValidator>--%></td>
            </tr>
            <tr>
                <td class="Ecoheadtxt" style="width: 118px">Select File
                </td>
                <td colspan="2" style="text-align: left">
                    <asp:FileUpload ID="FileUpload1" runat="server" />
                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="FileUpload1"
                        Display="Dynamic" ErrorMessage="File Required" ValidationGroup="a"></asp:RequiredFieldValidator>--%></td>
            </tr>
            <tr>
                <td class="Ecoheadtxt" style="width: 118px">Number</td>
                <td style="width: 173px">
                    <asp:TextBox ID="TxtfileNo" runat="server"></asp:TextBox></td>
                <td></td>
            </tr>
            <tr>
                <td class="Ecoheadtxt" style="width: 118px; height: 26px;">Cell/Cabinet No.</td>
                <td style="width: 173px; height: 26px;">
                    <asp:TextBox ID="TxtCellNo" runat="server"></asp:TextBox></td>
                <td style="height: 26px"></td>
            </tr>
            <tr>
                <td class="Ecoheadtxt" style="width: 118px">Room No.</td>
                <td style="width: 173px">
                    <asp:TextBox ID="TxtRoomNo" runat="server"></asp:TextBox></td>
                <td></td>
            </tr>
            <tr>
                <td class="Ecoheadtxt" style="width: 118px">Floor No.</td>
                <td style="width: 173px">
                    <asp:TextBox ID="TxtFloorNo" runat="server"></asp:TextBox></td>
                <td></td>
            </tr>
            <tr>
                <td class="Ecoheadtxt" style="width: 118px">Building</td>
                <td style="width: 173px">
                    <%-- <dxe:ASPxComboBox ID="Building" DataSourceID="SlectBuilding" TextField="bui_Name" ValueField="bui_id" runat="server" CssFilePath="~/App_Themes/Office2003 Blue/{0}/styles.css" CssPostfix="Office2003_Blue" ImageFolder="~/App_Themes/Office2003 Blue/{0}/" ValueType="System.String">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxComboBox>--%>
                    <asp:DropDownList ID="Building" runat="server" Width="152px">
                    </asp:DropDownList>
                </td>
                <td></td>
            </tr>
            <tr>
                <td style="width: 118px"></td>
                <td style="width: 173px"></td>
                <td></td>
            </tr>
            <tr>
                <td valign="top" style="width: 118px; text-align: right">
                    <asp:Button ID="Button1" runat="server" Text="Add" OnClick="Button1_Click" CssClass="btnUpdate" Width="50px" />
                </td>
                <td style="width: 173px">
                    <dxe:ASPxButton ID="btnDiscard" runat="server" AutoPostBack="false" Text="Discard" CssClass="btnUpdate" VerticalAlign="Bottom" Width="50px" OnClick="btnDiscard_Click">
                    </dxe:ASPxButton>
                </td>
                <td></td>
            </tr>

        </table>
    </div>
    <asp:SqlDataSource ID="SlectBuilding" runat="server"
        SelectCommand="select bui_Name,bui_id from tbl_master_building"></asp:SqlDataSource>
</asp:Content>
