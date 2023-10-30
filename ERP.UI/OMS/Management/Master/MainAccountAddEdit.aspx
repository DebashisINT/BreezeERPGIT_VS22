<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                22-03-2023        V2.0.36           Pallab              25733 : Master pages design modification
2.0                30-10-2023        V2.0.40           Priti               0026948 : In Accounts Head creation screen "Asset Type" will be mandatory and Default selection value will be "Blank".

====================================================== Revision History =============================================--%>

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
            /*border: 1px solid #ccc;
            background: #f9f9f9;*/
            padding: 15px 0;
        }

        /*Rev 1.0*/
        .outer-div-main {
            background: #ffffff;
            padding: 10px;
            border-radius: 10px;
            box-shadow: 1px 1px 10px #11111154;
        }

        /*.form_main {
            overflow: hidden;
        }*/

        label , .mylabel1, .clsTo, .dxeBase_PlasticBlue
        {
            color: #141414 !important;
            font-size: 14px !important;
                font-weight: 500 !important;
                margin-bottom: 0 !important;
                    line-height: 20px;
        }

        #GrpSelLbl .dxeBase_PlasticBlue
        {
                line-height: 20px !important;
        }

        select
        {
            height: 30px !important;
            border-radius: 4px;
            -webkit-appearance: none;
            position: relative;
            z-index: 1;
            background-color: transparent;
            padding-left: 10px !important;
            padding-right: 22px !important;
        }

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

        .calendar-icon {
            position: absolute;
            bottom: 6px;
            right: 5px;
            z-index: 0;
            cursor: pointer;
        }

        #ASPxFromDate , #ASPxToDate , #ASPxASondate , #ASPxAsOnDate , #txtDOB , #txtAnniversary , #txtcstVdate , #txtLocalVdate ,
        #txtCINVdate , #txtincorporateDate , #txtErpValidFrom , #txtErpValidUpto , #txtESICValidFrom , #txtESICValidUpto , #FormDate , #toDate
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        .dxeDisabled_PlasticBlue
        {
            z-index: 0 !important;
        }

        #ASPxFromDate_B-1 , #ASPxToDate_B-1 , #ASPxASondate_B-1 , #ASPxAsOnDate_B-1 , #txtDOB_B-1 , #txtAnniversary_B-1 , #txtcstVdate_B-1 ,
        #txtLocalVdate_B-1 , #txtCINVdate_B-1 , #txtincorporateDate_B-1 , #txtErpValidFrom_B-1 , #txtErpValidUpto_B-1 , #txtESICValidFrom_B-1 ,
        #txtESICValidUpto_B-1 , #FormDate_B-1 , #toDate_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #ASPxFromDate_B-1 #ASPxFromDate_B-1Img , #ASPxToDate_B-1 #ASPxToDate_B-1Img , #ASPxASondate_B-1 #ASPxASondate_B-1Img , #ASPxAsOnDate_B-1 #ASPxAsOnDate_B-1Img ,
        #txtDOB_B-1 #txtDOB_B-1Img ,
        #txtAnniversary_B-1 #txtAnniversary_B-1Img ,
        #txtcstVdate_B-1 #txtcstVdate_B-1Img ,
        #txtLocalVdate_B-1 #txtLocalVdate_B-1Img , #txtCINVdate_B-1 #txtCINVdate_B-1Img , #txtincorporateDate_B-1 #txtincorporateDate_B-1Img ,
        #txtErpValidFrom_B-1 #txtErpValidFrom_B-1Img , #txtErpValidUpto_B-1 #txtErpValidUpto_B-1Img , #txtESICValidFrom_B-1 #txtESICValidFrom_B-1Img ,
        #txtESICValidUpto_B-1 #txtESICValidUpto_B-1Img , #FormDate_B-1 #FormDate_B-1Img , #toDate_B-1 #toDate_B-1Img
        {
            display: none;
        }

        .dxtcLite_PlasticBlue > .dxtc-stripContainer .dxtc-activeTab, .dxgvFooter_PlasticBlue
        {
            background: #1b5ea4 !important;
        }

        .simple-select::after {
            /*content: '<';*/
            content: url(../../../assests/images/left-arw.png);
            position: absolute;
            top: 6px;
            right: -2px;
            font-size: 16px;
            transform: rotate(269deg);
            font-weight: 500;
            background: #094e8c;
            color: #fff;
            height: 18px;
            display: block;
            width: 26px;
            /* padding: 10px 0; */
            border-radius: 4px;
            text-align: center;
            line-height: 19px;
            z-index: 0;
        }
        .simple-select {
            position: relative;
        }
        .simple-select:disabled::after
        {
            background: #1111113b;
        }
        /*select.btn
        {
            padding-right: 10px !important;
        }*/

        .panel-group .panel
        {
            box-shadow: 1px 1px 8px #1111113b;
            border-radius: 8px;
        }

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

        .styled-checkbox {
        position: absolute;
        opacity: 0;
        z-index: 1;
    }

        .styled-checkbox + label {
            position: relative;
            /*cursor: pointer;*/
            padding: 0;
            margin-bottom: 0 !important;
        }

            .styled-checkbox + label:before {
                content: "";
                margin-right: 6px;
                display: inline-block;
                vertical-align: text-top;
                width: 16px;
                height: 16px;
                /*background: #d7d7d7;*/
                margin-top: 2px;
                border-radius: 2px;
                border: 1px solid #c5c5c5;
            }

        .styled-checkbox:hover + label:before {
            background: #094e8c;
        }


        .styled-checkbox:checked + label:before {
            background: #094e8c;
        }

        .styled-checkbox:disabled + label {
            color: #b8b8b8;
            cursor: auto;
        }

            .styled-checkbox:disabled + label:before {
                box-shadow: none;
                background: #ddd;
            }

        .styled-checkbox:checked + label:after {
            content: "";
            position: absolute;
            left: 3px;
            top: 9px;
            background: white;
            width: 2px;
            height: 2px;
            box-shadow: 2px 0 0 white, 4px 0 0 white, 4px -2px 0 white, 4px -4px 0 white, 4px -6px 0 white, 4px -8px 0 white;
            transform: rotate(45deg);
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
        .TableMain100 #GrdHolidays , #AccountGroup
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

        .for-cust-icon {
            position: relative;
            z-index: 1;
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

        .mt-24
        {
            margin-top: 24px;
        }

        .col-md-3
        {
            margin-top: 8px;
        }

        #upldBigLogo , #upldSmallLogo
        {
            width: 100%;
        }

        .chosen-container-single .chosen-single span
        {
            min-height: 30px;
            line-height: 30px;
        }

        .chosen-container-single .chosen-single div {
        background: #094e8c;
        color: #fff;
        border-radius: 4px;
        height: 26px;
        top: 1px;
        right: 1px;
        /*position:relative;*/
    }

        .chosen-container-single .chosen-single div b {
            display: none;
        }

        .chosen-container-single .chosen-single div::after {
            /*content: '<';*/
            content: url(../../../assests/images/left-arw.png);
            position: absolute;
            top: 2px;
            right: 5px;
            font-size: 18px;
            transform: rotate(269deg);
            font-weight: 500;
        }

    .chosen-container-active.chosen-with-drop .chosen-single div {
        background: #094e8c;
        color: #fff;
    }

        .chosen-container-active.chosen-with-drop .chosen-single div::after {
            transform: rotate(90deg);
            right: 5px;
        }

        #DivSetAsDefault
        {
            margin-top: 25px;
        }

        /*.dxeDisabled_PlasticBlue, .aspNetDisabled {
            opacity: 0.4 !important;
            color: #ffffff !important;
        }*/
                /*.padTopbutton {
            padding-top: 27px;
        }*/
        /*#lookup_project
        {
            max-width: 100% !important;
        }*/
        /*Rev end 1.0*/
    </style>
    <script src="Js/MainAccountAddEdit.js?v=2.1"></script>

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
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main">
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
                                        <%--Rev 2.0--%>
                                        <dxe:ListEditItem Text="Select" Value="0" />
                                        <%--Rev 2.0 End--%>
                                        <dxe:ListEditItem Text="Bank" Value="Bank" />
                                        <dxe:ListEditItem Text="Cash" Value="Cash" />

                                        <dxe:ListEditItem Text="Other" Value="Other" />
                                        <dxe:ListEditItem Text="Fixed Asset" Value="Fixed Asset" />
                                    </Items>
                                </dxe:ASPxComboBox>
                                <span id="spn_asset_type" style="display: none;" class="errorField">
                                    <img id="mandetoryAssetType" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory" />
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
