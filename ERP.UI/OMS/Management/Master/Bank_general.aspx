<%@ Page Title="Bank Details" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="Bank_general.aspx.cs" Inherits="ERP.OMS.Management.Master.Bank_general" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function disp_prompt(name) {
            if (name == "tab0") {
                document.location.href = "Bank_general.aspx";
            }
            if (name == "tab1") {
                document.location.href = "Bank_Correspondence.aspx";
            }


        }

        function selectAll() {
            cBranchGridLookup.gridView.SelectRows();
        }
        function unselectAll() {
            cBranchGridLookup.gridView.UnselectRows();
        }
        function CloseGridLookup() {
            cBranchGridLookup.ConfirmCurrentSelection();
            cBranchGridLookup.HideDropDown();
        }

        $(document).ready(function () {
            $("#<%=btnCancel.ClientID %>").click(function () {
           var url = 'bank.aspx';
           window.location.href = url;
           return false;
       });
   });

    </script>
    <style>
        .abs {
            position: absolute;
            right: -20px;
            top: 5px;
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">

            <h3>
                <asp:Label ID="lblHeadTitle" runat="server">Add/Edit Bank Details</asp:Label>
            </h3>
            <div class="crossBtn"><a href="bank.aspx"><i class="fa fa-times"></i></a></div>

        </div>

    </div>
    <div class="form_main">
        <table width="100%">
            <tr>
                <td class="EHEADER" style="text-align: center">
                    <asp:Label ID="lblName" runat="server" Font-Size="12px" Font-Bold="True"></asp:Label>
                </td>
            </tr>
        </table>

        <table class="TableMain100">

            <tr>
                <td>
                    <dxe:ASPxPageControl runat="server" ID="pageControl" Width="100%" ActiveTabIndex="0" ClientInstanceName="page">
                        <TabPages>
                            <dxe:TabPage Text="General">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                        <div class="row">
                                            <div>
                                                <asp:HiddenField ID="txtbnk_internalId_hidden" runat="server" />
                                            </div>
                                            <div class="col-md-3">
                                                <label>
                                                    Bank Name   <span style="color: Red;">*</span>
                                                </label>
                                                <div class="relative">
                                                    <asp:TextBox ID="txtBankName" runat="server" TabIndex="1" Width="100%" MaxLength="200"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtBankName" ValidationGroup="bank"
                                                        SetFocusOnError="true" ToolTip="Mandatory" class=" fa fa-exclamation-circle abs iconRed" ErrorMessage="">
                                                    </asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>Bank Branch</label>
                                                <div>
                                                    <asp:TextBox ID="txtBranch" runat="server" TabIndex="2" Width="100%" MaxLength="200"></asp:TextBox></div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>MICR Code <span style="color: Red;">*</span></label>
                                                <div>
                                                    <asp:TextBox ID="txtMICRcode" runat="server" TabIndex="3" Width="100%" MaxLength="11"></asp:TextBox></div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>IFSC Code</label>
                                                <div>
                                                    <asp:TextBox ID="txtIFSCCode" runat="server" TabIndex="4" Width="100%" MaxLength="11"></asp:TextBox></div>
                                            </div>
                                            <div class="clear"></div>
                                            <div class="col-md-3">
                                                <label>NEFT Code</label>
                                                <div>
                                                    <asp:TextBox ID="txtNEFTcode" runat="server" TabIndex="5" Width="100%" MaxLength="11"></asp:TextBox></div>
                                            </div>

                                            <div class="col-md-3">
                                                <label>RTGS Code</label>
                                                <div>
                                                    <asp:TextBox ID="txtRTGScode" runat="server" TabIndex="6" Width="100%" MaxLength="11"></asp:TextBox></div>
                                            </div>

                                            <%--Mantis issue no #16918--%>
                                            <div class="col-md-3">
                                                <label>Account Number</label>
                                                <div>
                                                    <dxe:ASPxTextBox ID="txtAcNo" ClientInstanceName="ctxtAcNo" runat="server" TabIndex="7" Width="100%" MaxLength="20"></dxe:ASPxTextBox>
                                                </div>
                                            </div>

                                            <div class="col-md-3">
                                                <label>Swift Code</label>
                                                <div>
                                                    <dxe:ASPxTextBox ID="txtSftCode" ClientInstanceName="ctxtSftCode" runat="server" TabIndex="8" Width="100%" MaxLength="20"></dxe:ASPxTextBox>
                                                </div>
                                            </div>
                                            <div class="clear"></div>
                                            <div class="col-md-9 lblmTop8">
                                                <label>Remarks</label>
                                                <div>
                                                    <dxe:ASPxTextBox ID="txtRemrks" ClientInstanceName="ctxtRemrks" runat="server" TabIndex="9" Width="100%" MaxLength="8000"></dxe:ASPxTextBox>
                                                </div>
                                            </div>

                                            <%-- End Mantis issue no #16918--%>


                                            <div class="col-md-3">

                                                <label>Branch</label>
                                                <div>
                                                    <dxe:ASPxCallbackPanel runat="server" ID="BranchPanel" ClientInstanceName="cBranchPanel" OnCallback="Component_Callback">
                                                        <PanelCollection>
                                                            <dxe:PanelContent runat="server">
                                                                <dxe:ASPxGridLookup ID="BranchGridLookup" runat="server" SelectionMode="Multiple" ClientInstanceName="cBranchGridLookup"
                                                                    KeyFieldName="branch_id" Width="100%" TextFormatString="{1}" MultiTextSeparator=", " OnDataBinding="grid_DataBinding">
                                                                    <Columns>
                                                                        <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="60" Caption=" " SelectAllCheckboxMode="Page">
                                                                        </dxe:GridViewCommandColumn>

                                                                        <dxe:GridViewDataColumn FieldName="branch_code" Caption="Branch Code" Width="150">
                                                                            <Settings AutoFilterCondition="Contains" />
                                                                        </dxe:GridViewDataColumn>

                                                                        <dxe:GridViewDataColumn FieldName="branch_description" Caption="Branch Name" Width="150">
                                                                            <Settings AutoFilterCondition="Contains" />
                                                                        </dxe:GridViewDataColumn>

                                                                        <dxe:GridViewDataColumn FieldName="branch_type" Caption="Branch Type" Width="150">
                                                                            <Settings AutoFilterCondition="Contains" />
                                                                        </dxe:GridViewDataColumn>

                                                                        <dxe:GridViewDataColumn FieldName="branch_description" Caption="Branch Name" Width="150">
                                                                            <Settings AutoFilterCondition="Contains" />
                                                                        </dxe:GridViewDataColumn>

                                                                    </Columns>
                                                                    <GridViewProperties Settings-VerticalScrollBarMode="Auto">

                                                                        <Templates>
                                                                            <StatusBar>
                                                                                <table class="OptionsTable" style="float: right">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <dxe:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll" />
                                                                                            <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAll" />
                                                                                            <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridLookup" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </StatusBar>
                                                                        </Templates>
                                                                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />

                                                                    </GridViewProperties>
                                                                    <ClientSideEvents GotFocus="LookupGotFocus" />
                                                                </dxe:ASPxGridLookup>


                                                            </dxe:PanelContent>
                                                        </PanelCollection>
                                                    </dxe:ASPxCallbackPanel>
                                                </div>
                                            </div>
                                           <%-- Mantis Issue 0023983--%>
                                            <div class="clear"></div>
                                            <div class="col-md-3">
                                                <label class="darkLabel mTop5"> &nbsp </label>
                                                <div>
                                                    <label class="checkbox-inline">
                                                        <asp:CheckBox ID="chkActive" ClientInstanceName="cchkActive" runat="server" ></asp:CheckBox>
                                                        <span style="margin: -2px 0; display: block">
                                                            <dxe:ASPxLabel ID="lblActive" runat="server" Text="Is Active">
                                                            </dxe:ASPxLabel>
                                                        </span>
                                                    </label>
                                                </div>
                                            </div>
                                            <%--End of Mantis Issue 0023983--%>

                                            <div class="clear"></div>
                                        </div>
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="tabCorrespondence" Text="Correspondance">

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

            <tr>
                <td>
                    <table style="width: 100%;">
                        <tr>
                            <td style="text-align: left; font-size: small; color: Red;">* Denotes Mandatory Field
                            </td>

                        </tr>
                        <tr>

                            <td style="text-align: center;">

                                <dxe:ASPxButton ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" ClientInstanceName="cbtnSave"
                                    ValidationGroup="bank" TabIndex="7" OnClick="btnSave_Click">
                                </dxe:ASPxButton>
                                <dxe:ASPxButton ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-danger"
                                    TabIndex="8">
                                </dxe:ASPxButton>

                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>

    </div>
</asp:Content>
