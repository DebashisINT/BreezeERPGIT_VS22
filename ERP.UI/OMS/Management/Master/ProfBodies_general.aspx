<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.Master.management_master_ProfBodies_general" CodeBehind="ProfBodies_general.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script language="javascript" type="text/javascript">
        function disp_prompt(name) {
            if (name == "tab0") {
                //alert(name);
                //document.location.href="ProfBodies_general.aspx"; 
            }
            if (name == "tab1") {
                //alert(name);
                document.location.href = "ProfBodies_correspondence.aspx";
            }
        }
    </script>
    <style>
        #RequiredFieldValidator2 {
            position: absolute;
            right: -20px;
            top: 6px;
        }

        #RequiredFieldValidator1 {
            position: absolute;
            right: -20px;
            top: 6px;
        }

        .dxtc-content {
            overflow: visible !important;
        }
    </style>
    <script type="text/javascript">
        function fn_ctxtPro_Name_TextChanged() {
            var ShortName = document.getElementById("txtShortName").value;

            $.ajax({
                type: "POST",
                url: "ProfBodies_general.aspx/CheckUniqueName",
                data: JSON.stringify({ ShortName: ShortName }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var data = msg.d;

                    if (data == true) {
                        jAlert("Please enter unique name");
                        document.getElementById("txtShortName").value='';
                        document.getElementById("txtShortName").focus();
                        return false;
                    }
                }
            });
        }
   </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Add Professional/Technical Bodies</h3>
            <div class="crossBtn"><a href="ProfBodies.aspx"><i class="fa fa-times"></i></a></div>
        </div>
    </div>
    <div class="form_main">
        <table class="TableMain100">

            <tr>
                <td>
                    <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" ClientInstanceName="page"
                        Font-Size="12px" Width="100%">
                        <TabPages>
                            <dxe:TabPage Text="General" Name="General">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                        <div>
                                            <div class="col-md-3">
                                                <label class="Ecoheadtxt">
                                                    Name :  <span class="iconRed" runat="server">*</span>
                                                </label>

                                                <div style="position: relative">
                                                    <asp:TextBox ID="txtName" runat="server" MaxLength="100" Width="100%"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtName" ValidationGroup="prof" SetFocusOnError="true" ToolTip="Mandatory." class="pullrightClass fa fa-exclamation-circle abs" ErrorMessage="" ForeColor="red"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label class="Ecoheadtxt">Short Name :  <span class="iconRed" runat="server">*</span></label>
                                                <div style="position: relative;">
                                                    <asp:TextBox ID="txtShortName" runat="server" Width="100%" MaxLength="50" onchange="fn_ctxtPro_Name_TextChanged(this);"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtShortName" ValidationGroup="prof" SetFocusOnError="true" ToolTip="Mandatory." class="pullrightClass fa fa-exclamation-circle abs" ErrorMessage="" ForeColor="red"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label class="Ecoheadtxt" style="">
                                                    Registration No :
                                                </label>
                                                <div style="">
                                                    <asp:TextBox ID="txtRegnNumber" runat="server" MaxLength="50" Width="100%"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label class="Ecoheadtxt">
                                                    Registration Date :
                                                </label>
                                                <div>
                                                    <dxe:ASPxDateEdit ID="txtRegndate" runat="server" EditFormat="Custom" UseMaskBehavior="true" Width="100%">
                                                        <ButtonStyle Width="13px">
                                                        </ButtonStyle>
                                                    </dxe:ASPxDateEdit>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label class="Ecoheadtxt">
                                                    Authority Name :
                                                </label>
                                                <div class="Ecoheadtxt">
                                                    <asp:TextBox ID="txtAuthorityName" runat="server" MaxLength="100" Width="100%"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label class="Ecoheadtxt">
                                                    Country Of Registration :
                                                </label>
                                                <div>
                                                    <asp:DropDownList ID="drpcountry" runat="server" Width="100%">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <label class="Ecoheadtxt">
                                                    Description
                                                </label>
                                                <div>
                                                    <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Width="100%" MaxLength="2000"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div style="clear: both"></div>
                                            <div class="col-md-12">
                                                <asp:Button ID="btnSave" runat="server" ValidationGroup="prof" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                                            </div>
                                        </div>

                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Text="Correspondence" Name="CorresPondence">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                        </TabPages>
                        <ClientSideEvents ActiveTabChanged="function(s, e) {
	                                            var activeTab   = page.GetActiveTab();
	                                            var Tab0 = page.GetTab(0);
	                                            var Tab1 = page.GetTab(1);
	                                            
	                                            if(activeTab == Tab0)
	                                            {
	                                                disp_prompt('tab0');
	                                            }
	                                            if(activeTab == Tab1)
	                                            {
	                                                disp_prompt('tab1');
	                                            }
	                                            }"></ClientSideEvents>
                        <ContentStyle>
                            <Border BorderColor="#002D96" BorderStyle="Solid" BorderWidth="1px" />
                        </ContentStyle>
                        <LoadingPanelStyle ImageSpacing="6px">
                        </LoadingPanelStyle>
                    </dxe:ASPxPageControl>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

