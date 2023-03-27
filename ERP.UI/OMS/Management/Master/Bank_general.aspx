<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                24-03-2023        2.0.36           Pallab              25733 : Master pages design modification
====================================================== Revision History =============================================--%>

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

        /*Rev 1.0*/

        .dxeButtonEditSys.dxeButtonEdit_PlasticBlue , .dxeTextBox_PlasticBlue
        {
            height: 30px;
            border-radius: 4px;
        }

        .dxeButtonEditButton_PlasticBlue
        {
            background: #094e8c !important;
            border-radius: 4px !important;
            padding: 0 4px !important;
        }

        .dxeDisabled_PlasticBlue
        {
            z-index: 0 !important;
        }

        .dxtcLite_PlasticBlue > .dxtc-stripContainer .dxtc-activeTab, .dxgvFooter_PlasticBlue
        {
            background: #1b5ea4 !important;
        }

        /*select.btn
        {
            padding-right: 10px !important;
        }*/

        /*.panel-group .panel
        {
            box-shadow: 1px 1px 8px #1111113b;
            border-radius: 8px;
        }*/

        .dxpLite_PlasticBlue .dxp-current
        {
            background-color: #1b5ea4;
            padding: 3px 5px;
            border-radius: 2px;
        }

        #accordion {
            margin-bottom: 20px;
            margin-top: 10px;
        }

        .dxgvHeader_PlasticBlue {
    background: #1b5ea4 !important;
    color: #fff !important;
}
        #ShowGrid
        {
            margin-top: 10px;
        }

        .pt-25{
                padding-top: 25px !important;
        }

        .dxgvEditFormDisplayRow_PlasticBlue td.dxgv, .dxgvDataRow_PlasticBlue td.dxgv, .dxgvDataRowAlt_PlasticBlue td.dxgv, .dxgvSelectedRow_PlasticBlue td.dxgv, .dxgvFocusedRow_PlasticBlue td.dxgv
        {
            padding: 6px 6px 6px !important;
        }

        #lookupCardBank_DDD_PW-1
        {
                left: -182px !important;
        }
        .plhead a>i
        {
                top: 9px;
        }

        .clsTo
        {
            display: flex;
    align-items: flex-start;
        }

        input[type="radio"], input[type="checkbox"]
        {
            margin-right: 5px;
        }
        .dxeCalendarDay_PlasticBlue
        {
                padding: 6px 6px;
        }

        .modal-dialog
        {
            width: 50%;
        }

        .modal-header
        {
            padding: 8px 4px 8px 10px;
            background: #094e8c !important;
        }

        .TableMain100 #ShowGrid , .TableMain100 #ShowGridList , .TableMain100 #ShowGridRet , .TableMain100 #EmployeeGrid , .TableMain100 #GrdEmployee,
        .TableMain100 #GrdHolidays , #PortCode
        {
            max-width: 98% !important;
        }

        /*div.dxtcSys > .dxtc-content > div, div.dxtcSys > .dxtc-content > div > div
        {
            width: 95% !important;
        }*/

        .btn-info
        {
                background-color: #1da8d1 !important;
                background-image: none;
        }

        .for-cust-icon {
            position: relative;
            z-index: 1;
        }

        .dxeDisabled_PlasticBlue, .aspNetDisabled
        {
            background: #f3f3f3 !important;
        }

        .dxeButtonDisabled_PlasticBlue
        {
            background: #b5b5b5 !important;
            border-color: #b5b5b5 !important;
        }

        #ddlValTech
        {
            width: 100% !important;
            margin-bottom: 0 !important;
        }

        .dis-flex
        {
            display: flex;
            align-items: baseline;
        }

        input + label
        {
            line-height: 1;
                margin-top: 7px;
        }

        .dxtlHeader_PlasticBlue
        {
            background: #094e8c !important;
        }

        .dxeBase_PlasticBlue .dxichCellSys
        {
            padding-top: 2px !important;
        }

        .pBackDiv
        {
            border-radius: 10px;
            box-shadow: 1px 1px 10px #1111112e;
        }
        .HeaderStyle th
        {
            padding: 5px;
        }

        .dxtcLite_PlasticBlue.dxtc-top > .dxtc-stripContainer
        {
            padding-top: 15px;
        }

        .pt-2
        {
            padding-top: 5px;
        }
        .pt-10
        {
            padding-top: 10px;
        }

        .pt-15
        {
            padding-top: 15px;
        }

        .pb-10
        {
            padding-bottom: 10px;
        }

        .pTop10 {
    padding-top: 20px;
}
        .custom-padd
        {
            padding-top: 4px;
    padding-bottom: 10px;
        }

        input + label
        {
                margin-right: 10px;
        }

        .btn
        {
            margin-bottom: 0;
        }

        .pl-10
        {
            padding-left: 10px;
        }

        .col-md-3>label, .col-md-3>span
        {
            margin-top: 0 !important;
        }

        .devCheck
        {
            margin-top: 5px;
        }

        .mtc-5
        {
            margin-top: 5px;
        }

        .mtc-10
        {
            margin-top: 10px;
        }

        /*select.btn
        {
           position: relative;
           z-index: 0;
        }*/

        select
        {
            margin-bottom: 0;
        }

        .form-control
        {
            background-color: transparent;
        }

        select.btn-radius {
    padding: 4px 8px 6px 11px !important;
}
        .mt-30{
            margin-top: 30px;
        }

        .panel-title h3
        {
            padding-top: 0;
            padding-bottom: 0;
        }

        .btn-radius
        {
            padding: 4px 11px !important;
            border-radius: 4px !important;
        }

        .crossBtn
        {
             right: 30px;
             top: 25px;
        }

        .mb-10
        {
            margin-bottom: 10px;
        }

        .btn-cust
        {
            background-color: #108b47 !important;
            color: #fff;
        }

        .btn-cust:hover
        {
            background-color: #097439 !important;
            color: #fff;
        }

        .gHesder
        {
            background: #1b5ca0 !important;
            color: #ffffff !important;
            padding: 6px 0 6px !important;
        }

        .close
        {
             color: #fff;
             opacity: .5;
             font-weight: 400;
        }

        .mt-27
        {
            margin-top: 27px !important;
        }

        .col-md-3
        {
            margin-top: 8px;
        }

        #upldBigLogo , #upldSmallLogo
        {
            width: 100%;
        }

        #DivSetAsDefault
        {
            margin-top: 25px;
        }

        .btn.btn-xs
        {
                font-size: 14px !important;
        }
        /*Rev end 1.0*/
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main">
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
    </div>
</asp:Content>
