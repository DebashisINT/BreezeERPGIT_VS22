<%@ Page Title="Cost Centers/Departments" Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.Master.management_master_HrCost" CodeBehind="HrCost.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .pdtble td {
            padding: 2px 0px;
        }
    </style>



    <script language="javascript" type="text/javascript">
        function disp_prompt(name) {
            if (name == "tab0") {
                //alert(name);
                //document.location.href="HrCost.aspx"; 
            }
            if (name == "tab1") {
                //alert(name);
                document.location.href = "HRCostBranch.aspx";
            }
            else if (name == "tab2") {
                //alert(name);
                document.location.href = "HRCostEmployee.aspx";
            }
        }
    </script>
    <style>
        #RequiredFieldValidator1, #revEmailID {
            position: absolute;
            right: 37px;
            top: 13px;
        }
    </style>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Cost Centers/Departments Information</h3>
            <div class="crossBtn"><a href="HRCostCenter.aspx"><i class="fa fa-times"></i></a></div>
        </div>

    </div>
    <div class="form_main">
        <table class="TableMain100 pdtble">
            <tr>
                <td>
                    <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" Width="100%"
                        ClientInstanceName="page" Font-Size="12px">
                        <TabPages>
                            <dxe:TabPage Text="Update" Name="Update">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                        <table>
                                            <tr>
                                                <td style="width: 150px">
                                                    <asp:Label ID="Label1" runat="server" Text="Cost Center/Department Name"></asp:Label><span style="color: red">*</span></td>
                                                <td style="position: relative; width: 260px">
                                                    <asp:TextBox ID="TxtCenter" runat="server" Width="254px"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="" ToolTip="Mandatory" ForeColor="Red" CssClass="pullleftClass fa fa-exclamation-circle spl" ControlToValidate="TxtCenter" Display="Dynamic"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label2" runat="server" Text="Cost Center/Department Type"></asp:Label></td>
                                                <td>
                                                    <asp:DropDownList ID="DDLType" runat="server" Width="254px">
                                                        <asp:ListItem>Department</asp:ListItem>
                                                        <asp:ListItem>Employee</asp:ListItem>
                                                        <asp:ListItem>Branch</asp:ListItem>
                                                    </asp:DropDownList></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label3" runat="server" Text="Parent Cost Center/Department"></asp:Label></td>
                                                <td>
                                                    <asp:DropDownList ID="DDLCostDept" runat="server" Width="254px">
                                                        
                                                    </asp:DropDownList></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label4" runat="server" Text="Head Of Department"></asp:Label></td>
                                                <td>
                                                    <asp:DropDownList ID="DDLHeadDept" runat="server" Width="254px">
                                                    </asp:DropDownList></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label5" runat="server" Text="Email ID"></asp:Label></td>
                                                <td style="position: relative">
                                                    <asp:TextBox ID="TxtEmail" runat="server" Width="254px"></asp:TextBox>

                                                    <asp:RegularExpressionValidator ID="revEmailID" runat="server"
                                                        ControlToValidate="TxtEmail" ToolTip="Invalid Email" ForeColor="Red" CssClass="pullleftClass fa fa-exclamation-circle spl" Display="Dynamic"
                                                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>

                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td></td>
                                            </tr>
                                            <tr style="display: none;">
                                                <td>&nbsp;</td>
                                                <td style="padding: 0">
                                                    <table style="width: 300px">
                                                        <tr>
                                                            <td style="width: 100px">
                                                                <asp:CheckBox ID="ChkFund" runat="server" Text="Mutual Funds" />
                                                            </td>
                                                            <td style="width: 100px">
                                                                <asp:CheckBox ID="ChkBrok" runat="server" Text="Broking " />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 100px">
                                                                <asp:CheckBox ID="ChkInsu" runat="server" Text="Insurance" />
                                                            </td>
                                                            <td style="width: 100px">
                                                                <asp:CheckBox ID="ChkDepos" runat="server" Text="Depository" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;</td>
                                                <td>
                                                    <asp:Label ID="lblmessage" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td>
                                                    <asp:Button ID="BtnSave" runat="server" Text="Save" OnClick="BtnSave_Click" CssClass="btn btn-primary" />
                                                    <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-danger" Text="Cancel" OnClick="btnCancel_Click" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td></td>
                                            </tr>
                                        </table>
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Text="Branch" Name="Branch">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Employee" Text="Employee">
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
	                                            var Tab2 = page.GetTab(2);	                                            
	                                            if(activeTab == Tab0)
	                                            {
	                                                disp_prompt('tab0');
	                                            }
	                                            if(activeTab == Tab1)
	                                            {
	                                                disp_prompt('tab1');
	                                            }
	                                            else if(activeTab == Tab2)
	                                            {
	                                                disp_prompt('tab2');
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
            <tr>
                <td style="height: 8px">
                    <table style="width: 100%;">
                        <tr>
                            <td align="right" style="width: 843px"></td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 843px"></td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
