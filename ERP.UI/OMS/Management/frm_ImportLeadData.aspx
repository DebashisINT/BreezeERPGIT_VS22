<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.management_frm_ImportLeadData" Title="" Codebehind="frm_ImportLeadData.aspx.cs" %>
<%@ Register Src="Headermain.ascx" TagName="Headermain" TagPrefix="uc1" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script language="javascript" type="text/javascript">
    FieldName='ctl00_ContentPlaceHolder3_DDLMiddleName';
    function CallList(obj1,obj2,obj3)
    {
        var obj4=document.getElementById("ctl00_ContentPlaceHolder3_DDLContactSource");
        var obj5=obj4.value;
        //alert(obj5);
        ajax_showOptions(obj1,obj2,obj3,obj5);
        //alert(obj5);
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table class="TableMain100">
    <tr>
        <td>
            <table class="TableMain">
                <tr>
                    <td class="Ecoheadtxt" style="width: 32%">
                        Country :
                    </td>
                    <td style="width: 228px; text-align: left;">
                        <asp:DropDownList ID="DDLCountry" runat="server" Width="165px" CssClass="EcoheadCon" AutoPostBack="True" OnSelectedIndexChanged="DDLCountry_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td class="Ecoheadtxt" style="width: 49%">
                        State :
                    </td>
                    <td style="text-align: left">
                        <asp:DropDownList ID="DDLState" runat="server" Width="165px" CssClass="EcoheadCon" AutoPostBack="True" OnSelectedIndexChanged="DDLState_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td class="Ecoheadtxt"> City</td>
                    <td>
                        <asp:DropDownList ID="DDLResidenceCity" runat="server" Width="165px" CssClass="EcoheadCon">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="Ecoheadtxt" style="width: 32%">
                        Contact Source :
                    </td>
                    <td style="width: 228px; text-align: left;">
                        <asp:DropDownList ID="DDLContactSource" runat="server" Width="165px" CssClass="EcoheadCon">
                        </asp:DropDownList>
                    </td>
                    <td class="Ecoheadtxt" style="width: 49%">
                        Refered By :
                    </td>
                    <td style="text-align: left">
                        <asp:TextBox ID="txtReferedBy" runat="server" TabIndex="14" Width="165px"></asp:TextBox>                        
                    </td>
                    <td colspan="2">
                        <asp:TextBox ID="txtReferedBy_hidden" runat="server" BackColor="Transparent" BorderColor="Transparent" BorderStyle="None" ForeColor="#DDECFE"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr id="TrMain" runat="server">
        <td>
            <table class="TableMain">
                <tr>
                    <td colspan="3" class="Ecoheadtxt" style="text-align: center">
                        Database Item :
                    </td>
                    <td colspan="3" class="Ecoheadtxt" style="text-align: center">
                        Import Data Item
                    </td>
                </tr>
                <tr>
                    <td class="Ecoheadtxt" style="width:19%">
                        First Name :
                    </td>
                    <td style="width:17%">
                        <asp:DropDownList ID="DDLFirstName" runat="server" Width="165px" CssClass="EcoheadCon">
                        </asp:DropDownList>
                    </td>
                    <td class="Ecoheadtxt" style="width:19%">
                        Middle Name :
                    </td>
                    <td style="width:17%">
                        <asp:DropDownList ID="DDLMiddleName" runat="server" Width="165px" CssClass="EcoheadCon">
                        </asp:DropDownList>
                    </td>
                    <td class="Ecoheadtxt" style="width:19%">
                        Last Name :
                    </td>
                    <td style="width:17%">
                        <asp:DropDownList ID="DDLLastName" runat="server" Width="165px" CssClass="EcoheadCon">
                        </asp:DropDownList>
                    </td>
                </tr>                
                <tr>
                    <td style="width:19%" class="Ecoheadtxt">
                        Short Name :
                    </td>
                    <td style="width:17%">
                        <asp:DropDownList ID="DDLShortName" runat="server" Width="165px" CssClass="EcoheadCon">
                        </asp:DropDownList>
                    </td>
                    <td class="Ecoheadtxt" style="width:19%">
                        Dob :
                    </td>
                    <td style="width:17%">
                        <asp:DropDownList ID="DDLDob" runat="server" Width="165px" CssClass="EcoheadCon">
                        </asp:DropDownList>
                    </td>
                    <td class="Ecoheadtxt" style="width:19%">
                        Residence Address1 :
                    </td>
                    <td style="width:17%">
                        <asp:DropDownList ID="DDLResidenceAdd1" runat="server" Width="165px" CssClass="EcoheadCon">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="Ecoheadtxt" style="width:19%">
                        Residence Address2 :
                    </td>
                    <td style="width:17%">
                        <asp:DropDownList ID="DDLResidenceAdd2" runat="server" Width="165px" CssClass="EcoheadCon">
                        </asp:DropDownList>
                    </td>
                    <td class="Ecoheadtxt" style="width:19%">
                        Residence Address3 :
                    </td>
                    <td style="width:17%">
                        <asp:DropDownList ID="DDLResidenceAdd3" runat="server" Width="165px" CssClass="EcoheadCon">
                        </asp:DropDownList>
                    </td>
                    <td class="Ecoheadtxt" style="width:19%">
                        Residence LandMark :
                    </td>
                    <td style="width:17%">
                        <asp:DropDownList ID="DDLResidenceLandmark" runat="server" Width="165px" CssClass="EcoheadCon">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="Ecoheadtxt" style="width:19%">
                        Residence Pin :</td>
                    <td style="width:17%">
                        <asp:DropDownList ID="DDLResidencePin" runat="server" Width="165px" CssClass="EcoheadCon">
                        </asp:DropDownList></td>
                     <td class="Ecoheadtxt" style="width:19%">
                         Office Address1 :
                    </td>
                    <td style="width:17%">
                        <asp:DropDownList ID="DDLOfficeAddress1" runat="server" Width="165px" CssClass="EcoheadCon">
                        </asp:DropDownList>&nbsp;</td>
                    <td class="Ecoheadtxt" style="width:19%">
                        Office Address2 :</td>
                    <td style="width:17%">
                        <asp:DropDownList ID="DDLOfficeAddress2" runat="server" Width="165px" CssClass="EcoheadCon">
                        </asp:DropDownList>&nbsp;</td>
                </tr>
                <tr>
                    <td class="Ecoheadtxt" style="width: 19%">
                        Office Address3 :</td>
                    <td style="width:17%">
                        <asp:DropDownList ID="DDLOfficeAddress3" runat="server" Width="165px" CssClass="EcoheadCon">
                        </asp:DropDownList>&nbsp;</td>
                    <td class="Ecoheadtxt" style="width:19%">
                        Office LandMark :</td>
                    <td style="width:17%">
                        <asp:DropDownList ID="DDLOfficeLandmark" runat="server" Width="165px" CssClass="EcoheadCon">
                        </asp:DropDownList>&nbsp;</td>
                    <td class="Ecoheadtxt" style="width:19%">
                        Office Pin :</td>
                    <td style="width:17%">
                        <asp:DropDownList ID="DDLOfficePin" runat="server" Width="165px" CssClass="EcoheadCon">
                        </asp:DropDownList>&nbsp;</td>
                </tr>
                <tr>
                    <td class="Ecoheadtxt" style="width: 19%">
                        Residence Phone No. :</td>
                    <td style="width:17%">
                        <asp:DropDownList ID="DDLResidencePhoneNo" runat="server" Width="165px" CssClass="EcoheadCon">
                        </asp:DropDownList>&nbsp;</td>
                    <td class="Ecoheadtxt" style="width:19%">
                        Residence FaxNo :</td>
                    <td style="width:17%">
                        <asp:DropDownList ID="DDLResidenceFaxNo" runat="server" Width="165px" CssClass="EcoheadCon">
                        </asp:DropDownList>&nbsp;</td>
                     <td class="Ecoheadtxt" style="width:19%">
                         Office Phone No. :</td>
                    <td style="width:17%">
                        <asp:DropDownList ID="DDLOfficePhoneNo" runat="server" Width="165px" CssClass="EcoheadCon">
                        </asp:DropDownList>&nbsp;</td>
                </tr>
                <tr>
                    <td class="Ecoheadtxt" style="width: 19%">
                        Office FaxNo :
                    </td>
                    <td style="width:17%">
                        <asp:DropDownList ID="DDLOfficeFaxNo" runat="server" Width="165px" CssClass="EcoheadCon">
                        </asp:DropDownList>&nbsp;</td>
                    <td class="Ecoheadtxt" style="width:19%">
                        Mobile No. :</td>
                    <td style="width:17%">
                        <asp:DropDownList ID="DDLMobileNo" runat="server" Width="165px" CssClass="EcoheadCon">
                        </asp:DropDownList>&nbsp;</td>
                    <td class="Ecoheadtxt" style="width:19%">
                        Personal Email :</td>
                    <td style="width:17%">
                        <asp:DropDownList ID="DDLEmailAddress" runat="server" Width="165px" CssClass="EcoheadCon">
                        </asp:DropDownList>&nbsp;</td>
                </tr>
                <tr>
                    <td class="Ecoheadtxt" style="width: 19%">
                        Official Email :</td>
                    <td style="width:17%">
                        <asp:DropDownList ID="DDLCCEmail" runat="server" Width="165px" CssClass="EcoheadCon">
                        </asp:DropDownList>&nbsp;</td>
                    <td class="Ecoheadtxt" style="width:19%">
                        &nbsp;</td>
                    <td style="width:17%">
                        &nbsp;</td>
                    <td class="Ecoheadtxt" style="width:19%">
                        &nbsp;</td>
                    <td style="width:17%">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td colspan="6">
                        <asp:Button ID="BtnSave" runat="server" Text="Submit" CssClass="btnUpdate" OnClick="BtnSave_Click" />
                    </td>
                </tr>
            </table>
                        <asp:DropDownList ID="DDLOfficeCity" runat="server" Width="165px" CssClass="EcoheadCon" Visible="False">
                        </asp:DropDownList></td>
    </tr>
</table>
</asp:Content>

