<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="MainAccountAddEdit.aspx.cs" Inherits="ERP.OMS.Management.Master.MainAccountAddEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <script type="text/javascript" src="/assests/js/init.js"></script>
    <script type="text/javascript" src="/assests/js/ajax-dynamic-list.js"></script>
    <script src="/assests/pluggins/choosen/choosen.min.js"></script>
    <script src="/assests/pluggins/bootstrap-multiselect/bootstrap-multiselect.min.js"></script>
    <style>
        .errorField {
            position: absolute;
            right: -18px;
            top: 4px;
        }
        select#ddlModuleSet {
            display: none;
        }

        .maskInput table > tbody > tr > td.dxeErrorCell_PlasticBlue {
            display: none;
        }
        /*.totalWrap {
        margin-top: 12px;
        background: #c3d8d8;
        border: 1px solid #b6d0d0;
        padding: 9px 0;
    }*/
        .totalWrap {
            border: 1px solid #ccc;
            background: #f9f9f9;
            padding: 15px 0;
        }
    </style>
    <script src="Js/MainAccountAddEdit.js?v=2.0"></script>

    <script>
        $(document).ready(function () {

            var ModuleID = $("#hdnModuleMAPID").val();
                
            $.ajax({
                url: "MainAccountAddEdit.aspx/PopulateAllModule",
                contentType: "application/json; charset=utf-8",
                datatype: "JSON",
                type: "POST",
                data: JSON.stringify({ ModuleMAPID: ModuleID }),
                success: function (data) {

                    $("#ddlModuleSet").empty();
                    var grpdetl = data.d;

                    var opts = "";
                    for (i in grpdetl) {
                        if (grpdetl[i].IsChecked == "True") {
                            opts += "<option selected  value='" + grpdetl[i].Module_Value + "'>" + grpdetl[i].Module_Name + "</option>";
                        }
                        else {
                            opts += "<option   value='" + grpdetl[i].Module_Value + "'>" + grpdetl[i].Module_Name + "</option>";
                        }
                    }

                    $("#ddlModuleSet").empty().append(opts);

                    setTimeout(function () {                        
                        $('#ddlModuleSet').multiselect('rebuild');
                    }, 200)
                },
                error: function (data) {
                    jAlert("Please try again later");
                }
            });

            $('#ddlModuleSet').multiselect({
                includeSelectAllOption: true,
                buttonWidth: '100%',
                enableFiltering: true,
                maxHeight: 200,
                //dropUp: true,
                enableCaseInsensitiveFiltering: true,
                //onDropdownHide: function (event) {
                //    //console.log(event)
                //},
                onChange: function () {
                    var selected = this.$select.val();
                    $('#hdnModuleSet').val(selected);                    
                },
                buttonText: function (options, select) {
                    if (options.length === 0) {
                        return 'No option selected ...';
                    }
                    else if (options.length > 2) {
                        return 'More than 2 options selected!';
                    } else {
                        var labels = [];
                        options.each(function () {
                            if ($(this).attr('label') !== undefined) {
                                labels.push($(this).attr('label'));
                            }
                            else {
                                labels.push($(this).html());
                            }
                        });
                        return labels.join(', ') + '';
                    }
                }
            }).multiselect('selectAll', false).multiselect('updateButtonText');
        })

        function alertify(msg) {
            if (msg == "true") {
                jAlert("Saved Successfully", "Alert", function () {
                    window.location.href = 'MainAccountHead.aspx';
                });

            }
            else if (msg == "Update Successfully.") {

               <%-- if ("<%=Convert.ToString(Session["EditId"])%>" != "ADD") {
                    "<%=Convert.ToString(Session["EditId"])%>" = "";
                }--%>
                jAlert("Updated Successfully", "Alert", function () {
                    window.location.href = 'MainAccountHead.aspx';
                });
            }

            else {
                if ("<%=Convert.ToString(Session["EditId"])%>" != "ADD") {
                    $("#hdMainActId").val("<%=Convert.ToString(Session["EditId"])%>");
                }
                jAlert(msg);
            }
    }

    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
        <ClientSideEvents ControlsInitialized="AllControlInitilize" />
    </dxe:ASPxGlobalEvents>
    <div class="panel-heading">
        <div class="panel-title clearfix">
            <h3 class="pull-left">Add/Edit Main Account</h3>

            <div class="crossBtn"><a href="MainAccountHead.aspx"><i class="fa fa-times"></i></a></div>
        </div>
    </div>
    <div class="form_main">

        <div class="clearfix">
        </div>

        <table style="width: 100%">
            <tr>
                <td style="width: 100%">

                    <div class="totalWrap row">
                        <div class="col-md-3">

                            <label>Account Name<span style="color: red">*</span></label>
                            <div class="relative">

                                <dxe:ASPxTextBox ID="txt_acnt_nm" runat="server" class="form-control" Width="100%" ClientInstanceName="ctxt_acnt_nm">
                                </dxe:ASPxTextBox>
                                <span id="acnt_nm" style="display: none;" class="errorField">
                                    <img id="mandetorydelivery" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory" />
                                </span>
                            </div>

                        </div>
                        <div class="col-md-3">
                            <label>Short Name<span style="color: red">*</span></label>

                            <div class="relative">

                                <dxe:ASPxTextBox ID="txt_short_nm" runat="server" class="form-control" Width="100%" ClientInstanceName="ctxt_short_nm"></dxe:ASPxTextBox>
                                <span id="short_nm" style="display: none;" class="errorField">
                                    <img id="mandetorydelivery" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory" />
                                </span>
                            </div>


                        </div>

                        <div class="col-md-3">
                            <label>Account Type<span style="color: red">*</span></label>


                            <div class="relative">
                                <dxe:ASPxComboBox ID="drp_acnt_type" runat="server" Width="100%" ValueType="System.String" class="form-control" EnableIncrementalFiltering="true" ClientInstanceName="cdrp_asset_type">
                                    <ClientSideEvents SelectedIndexChanged="getaccounttype" />
                                </dxe:ASPxComboBox>
                                <span id="spn_acnt_type" style="display: none;" class="errorField">
                                    <img id="mandetorydelivery" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory" />
                                </span>
                            </div>
                        </div>

                        <div class="col-md-3">
                            <label>Account Group</label>
                            <div class="relative">
                                <dxe:ASPxComboBox ID="drp_acnt_grp" Width="100%" runat="server" ValueType="System.String" OnCallback="drp_acnt_grp_Callback" ClientInstanceName="cdrp_acnt_grp" EnableIncrementalFiltering="true">
                                    <ClientSideEvents EndCallback="drp_acnt_grpendcallback" />
                                    <Columns>
                                        <dxe:ListBoxColumn FieldName="CategoryArrange" Caption="Account Group" Width="500" />

                                    </Columns>
                                </dxe:ASPxComboBox>
                            </div>
                        </div>


                        <div class="col-md-3" id="asset_type">
                            <label id="AssLiaType">Asset Type<span style="color: red">*</span></label>

                            <div class="relative">
                                <dxe:ASPxComboBox ID="drp_asset_type" runat="server" Width="100%" ValueType="System.String" class="form-control" SelectedIndex="0" EnableIncrementalFiltering="true" ClientInstanceName="cdrpassettype">
                                    <ClientSideEvents SelectedIndexChanged="getasset_type" />
                                    <Items>
                                        <dxe:ListEditItem Text="Bank" Value="Bank" />
                                        <dxe:ListEditItem Text="Cash" Value="Cash" />

                                        <dxe:ListEditItem Text="Other" Value="Other" />
                                        <dxe:ListEditItem Text="Fixed Asset" Value="Fixed Asset" />
                                    </Items>
                                </dxe:ASPxComboBox>
                                <span id="spn_asset_type" style="display: none;" class="errorField">
                                    <img id="mandetorydelivery" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory" />
                                </span>
                            </div>
                        </div>

                        <div class="col-md-3" id="bnk_acnt">
                            <label>Bank Account Number</label>

                            <div class="relative">

                                <dxe:ASPxTextBox ID="txt_bnk_acnt_nmbr" Width="100%" runat="server" class="form-control" MaxLength="20"></dxe:ASPxTextBox>
                            </div>

                        </div>

                        <div class="col-md-3">
                            <label>Company Name<span style="color: red">*</span></label>

                            <div class="relative">
                                <dxe:ASPxComboBox ID="drp_cmp_nm" Width="100%" runat="server" ValueType="System.String" class="form-control" EnableIncrementalFiltering="true" ClientInstanceName="cdrp_cmp_nm"></dxe:ASPxComboBox>
                                <span id="cmp_nm" style="display: none;" class="errorField">
                                    <img id="mandetorydelivery" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory" />
                                </span>
                            </div>
                        </div>

                        <div class="col-md-3">
                            <label>Branch<span style="color: red">*</span></label>


                            <div class="relative">
                                <dxe:ASPxGridLookup ID="BranchGridLookup" runat="server" SelectionMode="Multiple" ClientInstanceName="cBranchGridLookup" KeyFieldName="branch_id" TextFormatString="{1}" MultiTextSeparator=", "
                                    DataSourceID="BranchdataSource" Width="100%">
                                    <Columns>
                                        <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="60" Caption=" " SelectAllCheckboxMode="Page">
                                        </dxe:GridViewCommandColumn>

                                        <dxe:GridViewDataColumn FieldName="branch_code" Caption="Branch Code" Width="150">
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
                                                            <dxe:ASPxButton ID="btn_select_all" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll" UseSubmitBehavior="false" />
                                                            <dxe:ASPxButton ID="btn_deselect_all" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAll" UseSubmitBehavior="false" />
                                                            <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridLookup" UseSubmitBehavior="false" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </StatusBar>
                                        </Templates>
                                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />

                                    </GridViewProperties>
                                </dxe:ASPxGridLookup>

                                <span id="branch" style="display: none;" class="errorField">
                                    <img id="mandetorydelivery" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory" /></span>
                            </div>

                        </div>

                        <div class="col-md-3" id="posting_type">
                            <label>Select Posting Type</label>

                            <div class="relative">
                                <dxe:ASPxComboBox ID="cPaymenttype" ClientInstanceName="cPaymenttype" runat="server"
                                    ValueType="System.String" EnableIncrementalFiltering="true"
                                    Width="100%" SelectedIndex="0">
                                    <Items>
                                        <dxe:ListEditItem Text="None" Value="None" Selected="true" />
                                        <dxe:ListEditItem Text="Card" Value="Card" />
                                        <dxe:ListEditItem Text="Coupon" Value="Coupon" />
                                        <dxe:ListEditItem Text="Etransfer" Value="Etransfer" />

                                    </Items>


                                </dxe:ASPxComboBox>
                            </div>
                        </div>



                        <div class="col-md-3" style="display: none" id="sub_ledger">
                            <label>Sub-Ledger Type</label>
                            <div class="relative">
                                <dxe:ASPxComboBox ID="drp_sub_ledger_type" Width="100%" runat="server" ClientInstanceName="cdrp_sub_ledger_type" ValueType="System.String" class="form-control" EnableIncrementalFiltering="true">
                                    <ClientSideEvents SelectedIndexChanged="get_SubLedger_type" />
                                    <Items>
                                        <dxe:ListEditItem Text="None" Value="None" Selected="true"></dxe:ListEditItem>
                                        <dxe:ListEditItem Text="Customers" Value="Customers "></dxe:ListEditItem>
                                        <dxe:ListEditItem Text="Employees" Value="Employees"></dxe:ListEditItem>
                                        <dxe:ListEditItem Text="Vendors" Value="Vendors "></dxe:ListEditItem>
                                        <dxe:ListEditItem Text="Agents" Value="Agents"></dxe:ListEditItem>
                                        <dxe:ListEditItem Text="Custom" Value="Custom"></dxe:ListEditItem>
                                        <dxe:ListEditItem Text="Driver/Transporter" Value="DriverTransporter"></dxe:ListEditItem>
                                        <dxe:ListEditItem Text="Influencer" Value="Influencer"></dxe:ListEditItem>
                                        <dxe:ListEditItem Text="Lead" Value="Lead"></dxe:ListEditItem>
                                    </Items>

                                </dxe:ASPxComboBox>
                            </div>

                        </div>

                        <%-- <div class="col-md-3" id="roi">
                            <label>Rate Of Interest(p/a)</label>
                            <div>
                                <dxe:ASPxTextBox ID="txtRateofIntrest" ClientInstanceName="CtxtRateofIntrest" runat="server"
                                    Width="100%">
                                    <MaskSettings Mask="<0..9999g>.<00..99>" ErrorText="None" IncludeLiterals="DecimalSymbol" />
                                </dxe:ASPxTextBox>


                            </div>

                        </div>--%>


                        <div class="col-md-3" id="depr" style="display: none">
                            <label>Depreciation</label>
                            <div class="relative maskInput">
                                <dxe:ASPxTextBox ID="txtDepreciation" ClientInstanceName="txtDepreciation" runat="server"
                                    Width="100%" MaskSettings-Mask="<0..9999g>.<00..99>"
                                    MaskSettings-IncludeLiterals="DecimalSymbol">
                                </dxe:ASPxTextBox>


                            </div>

                        </div>


                        <div class="col-md-3" runat="server" id="tds" style="display: none">
                            <label>TDS Section</label>

                            <div class="relative">
                                <dxe:ASPxComboBox ID="drp_tds_section" Width="100%" runat="server" ValueType="System.String" class="form-control" EnableIncrementalFiltering="true">
                                </dxe:ASPxComboBox>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <label>Old Unit Ledger</label>
                            <div class="relative">

                                <dxe:ASPxComboBox ID="drp_old_unit_ledger" runat="server" Width="100%" ValueType="System.String" class="form-control" EnableIncrementalFiltering="true">
                                    <Items>
                                        <dxe:ListEditItem Text="Yes" Value="1" />
                                        <dxe:ListEditItem Text="No" Value="0" Selected="true" />


                                    </Items>
                                </dxe:ASPxComboBox>
                            </div>

                        </div>




                        <div class="col-md-3">
                            <label>Reverse Applicable</label>

                            <div class="relative">
                                <dxe:ASPxComboBox ID="drp_revrs_applicabl" runat="server" Width="100%" ValueType="System.String" class="form-control" EnableIncrementalFiltering="true">
                                    <Items>
                                        <dxe:ListEditItem Text="Yes" Value="1" />
                                        <dxe:ListEditItem Text="No" Value="0" Selected="true" />


                                    </Items>
                                </dxe:ASPxComboBox>
                            </div>
                        </div>

                        <div class="clear"></div>
                        <div class="col-md-3" id="div_BalLimit">
                            <asp:Label ID="Label23" runat="server" Text="Cash/Bank Balance "></asp:Label>
                            <div>
                                <dxe:ASPxTextBox ID="txtBalanceLimit" ClientInstanceName="ctxtBalanceLimit" MaxLength="18" HorizontalAlign="Right" DisplayFormatString="{0:0.00}"
                                    runat="server" Width="100%">

                                    <%-- <MaskSettings Mask="&lt;0..9999999999g&gt;.&lt;00..99&gt;" />--%>
                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                </dxe:ASPxTextBox>
                            </div>
                        </div>
                        <div class="col-md-3" id="div_NegStock">

                            <asp:Label ID="Label6" runat="server" Text="Below Cash Bank Balance" CssClass="newLbl"></asp:Label>

                            <div class="Left_Content">
                                <dxe:ASPxComboBox ID="cmbNegativeStk" ClientInstanceName="ccmbNegativeStk" runat="server"
                                    ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0">
                                    <Items>
                                        <dxe:ListEditItem Text="Select" Value="" />
                                        <dxe:ListEditItem Text="Block" Value="B" />
                                        <dxe:ListEditItem Text="Warn" Value="W" />
                                        <dxe:ListEditItem Text="Ignore" Value="I" />
                                    </Items>
                                </dxe:ASPxComboBox>
                            </div>
                        </div>

                        <div class="col-md-3" id="div_DailyLimit">
                            <asp:Label ID="Label1" runat="server" Text="Daily Limit"></asp:Label>
                            <div>
                                <dxe:ASPxTextBox ID="txtDailyLimit" ClientInstanceName="ctxtDailyLimit" MaxLength="18" HorizontalAlign="Right" DisplayFormatString="{0:0.00}"
                                    runat="server" Width="100%">
                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                </dxe:ASPxTextBox>
                            </div>
                        </div>
                        <div class="col-md-3" id="div_DailyLimitExceed">
                            <asp:Label ID="Label2" runat="server" Text="Daily Limit Exceed" CssClass="newLbl"></asp:Label>

                            <div class="Left_Content">
                                <dxe:ASPxComboBox ID="cmbDailyLimitExceed" ClientInstanceName="ccmbDailyLimitExceed" runat="server"
                                    ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0">
                                    <Items>
                                        <dxe:ListEditItem Text="Select" Value="" />
                                        <dxe:ListEditItem Text="Block" Value="B" />
                                        <dxe:ListEditItem Text="Warn" Value="W" />
                                        <dxe:ListEditItem Text="Ignore" Value="I" />

                                    </Items>
                                </dxe:ASPxComboBox>
                            </div>
                        </div>

                        <div class="clear"></div>


                        <div class="col-md-3" id="dvDeducteestat" runat="server">
                            <asp:Label ID="Label4" runat="server" Text="Deductee Status" CssClass="newLbl"></asp:Label>
                            <div class="Left_Content">
                                <dxe:ASPxComboBox ID="cmbDeducteestat" ClientInstanceName="ccmbDeducteestat" runat="server"
                                    ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0">
                                    <Items>
                                        <dxe:ListEditItem Text="Select" Value="0" Selected="true" />
                                        <dxe:ListEditItem Text="Company" Value="01" />
                                        <dxe:ListEditItem Text="Other than Company" Value="02" />
                                    </Items>
                                </dxe:ASPxComboBox>
                            </div>
                        </div>

                        <div class="col-md-3" id="dvTaxDeducteeType" runat="server">
                            <label class="labelt">
                                <dxe:ASPxLabel ID="ASPxLabel37" runat="server" Text="Tax Entity Type">
                                </dxe:ASPxLabel>
                            </label>
                            <div class="visF">
                                <dxe:ASPxComboBox ID="cmbTaxdeducteedType" ClientInstanceName="ccmbTaxdeducteedType" runat="server" Width="200px">
                                    <Items>
                                        <dxe:ListEditItem Selected="true" Text="Select" Value="0" />
                                        <dxe:ListEditItem Text="Government" Value="G" />
                                        <dxe:ListEditItem Text="Non-government" Value="NG" />
                                        <dxe:ListEditItem Text="Others" Value="O" />
                                    </Items>
                                </dxe:ASPxComboBox>
                            </div>
                        </div>
                        <div class="col-md-3" id="DivSetAsDefault" runat="server">
                            <label class="labelt">
                                <dxe:ASPxLabel ID="ASPxLabel1" runat="server" Text="Set As Default">
                                </dxe:ASPxLabel>
                            </label>
                            <select class="form-control " id="ddlModuleSet" multiple>
                            <%--    <option value="None" selected>None</option>
                                <option value="CashBankVoucher">Cash/Bank Voucher</option>
                                 <option value="CustomerVendorVoucher">Customer/Vendor Voucher</option>--%>
                            </select>
                        </div>
                        <div class="clear"></div>

                        <div class="col-md-4" id="dvIsparty" runat="server" style="display: none">

                            <div class="Left_Content">
                                <dxe:ASPxCheckBox ID="Isparty" ClientInstanceName="cIsparty" Checked="false" Text="Consider as Party" TextAlign="Right" runat="server">
                                    <ClientSideEvents GotFocus="PartyOnFocus" />
                                </dxe:ASPxCheckBox>
                            </div>

                        </div>

                        <div class="clear"></div>

                        <div class="col-md-12" style="padding-top: 10px;">

                            <dxe:ASPxButton ID="btn_save" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btn_save_Click" ClientSideEvents-Click="submitvalidate" UseSubmitBehavior="false"></dxe:ASPxButton>
                            <dxe:ASPxButton ID="ASPxButton1" runat="server" Text="Cancel" AutoPostBack="false" CssClass="btn btn-danger" ClientSideEvents-Click="cancel_click" UseSubmitBehavior="false"></dxe:ASPxButton>
                            <dxe:ASPxButton ID="ASPxButton2" runat="server" Text="Udf" CssClass="btn btn-primary" AutoPostBack="false" UseSubmitBehavior="false"
                                ClientSideEvents-Click="UdfPopupClick">
                            </dxe:ASPxButton>

                        </div>
                    </div>

                </td>

            </tr>
        </table>
    </div>


    <dxe:ASPxPopupControl ID="ASPXPopupControl" runat="server"
        CloseAction="CloseButton" Top="120" Left="300" ClientInstanceName="popup" Height="680"
        Width="700" HeaderText="Asset Details" AllowResize="false" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ContentStyle-Wrap="True" ResizingMode="Live" Modal="true">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="Blue" Font-Bold="True" ForeColor="White" />
    </dxe:ASPxPopupControl>

    <asp:HiddenField runat="server" ID="Keyval_internalId" />
    <asp:HiddenField runat="server" ID="hdMainActId" />
    <asp:HiddenField runat="server" ID="hdnSubledgerCashBankType" />
    <asp:HiddenField ID="hdnModuleMAPID" runat="server" />
    <asp:HiddenField ID="hdnModuleSet" runat="server" />

    
    <asp:SqlDataSource ID="BranchdataSource" runat="server"
        SelectCommand="select br.branch_id,branch_code,branch_description from tbl_master_branch br 
                            inner join tbl_master_ledgerBranch_map map on map.branch_id = br.branch_id
                            where MainAccount_id=@id union all  
                            select branch_id,branch_code,branch_description from tbl_master_branch br where branch_id not in
                            (select branch_id from tbl_master_ledgerBranch_map where MainAccount_id=@id)">
        <SelectParameters>
            <asp:ControlParameter Name="id" DefaultValue="0" ControlID="hdMainActId" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>
