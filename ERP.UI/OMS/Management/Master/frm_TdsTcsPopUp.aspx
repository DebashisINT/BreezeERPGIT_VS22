<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                22-03-2023        2.0.36           Pallab              25733 : Master pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Title="TDS/TCS" Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/Erp.Master" EnableEventValidation="false"
    Inherits="ERP.OMS.Management.Master.management_master_frm_TdsTcsPopUp" CodeBehind="frm_TdsTcsPopUp.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/assests/pluggins/choosen/choosen.min.js"></script>
    <%--    <script src="https://cdnjs.cloudflare.com/ajax/libs/angular.js/1.5.8/angular.min.js"></script>--%>
    <%--chosen.css--%>

    <style type="text/css">
        .chosen-container.chosen-container-single {
            width: 100% !important;
        }

        .chosen-choices {
            width: 100% !important;
        }

        /*Rev Mantis Issue 24161 [#lstAdvanceLedger added] */
        #lstMainAccount, #lstSubAccount, #lstInterestLedger, #lstLateFeeLedger, #lstOthersLedger, #lstAdvanceLedger {
            width: 200px;
        }

        .mandtry {
            position: absolute;
            right: -19px;
            top: 7px;
        }

        .dxflHARSys > table, .dxflHARSys > div {
            margin-left: 0px !important;
            margin-right: 0px !important;
            padding-left: 74px !important;
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

        #divNewTDS
        {
            margin-top: 15px;
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

    <script src="Js/frm_TdsTcsPopUp.js?V=1.33"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main">
        <div class="panel-heading">
        <div class="panel-title">
            <h3>Add TDS / TCS</h3>
        </div>
        <div class="crossBtn"><a href="IframeTdsTcs.aspx"><i class="fa fa-times"></i></a></div>
    </div>
        <div class="form_main">
        <div class="row">
            <div class="col-md-3">
                <label>Type </label>
                <div class=" simple-select">
                    <asp:DropDownList ID="ddlType" runat="server" CssClass="form-control" onchange="myFunction();">
                        <asp:ListItem Text="TDS" Value="0"> </asp:ListItem>
                        <asp:ListItem Text="TCS" Value="1"></asp:ListItem>
                        <%--<asp:ListItem>TDS

                        </asp:ListItem>
                        <asp:ListItem>TCS</asp:ListItem>--%>
                    </asp:DropDownList>
                </div>
            </div>



            <div class="col-md-3">
                <label id="lblTdsTcsSection">TDS Section: </label>
                <em>*</em>

                <div class="relative">
                    <asp:TextBox ID="txtCode" runat="server" Style="display: none" CssClass="form-control" MaxLength="80"></asp:TextBox>


                    <dxe:ASPxComboBox ID="TdsSection" ClientInstanceName="cTdsSection" runat="server" SelectedIndex="0"
                        ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True">
                        <ClientSideEvents SelectedIndexChanged="TdsSectionChanged" />
                    </dxe:ASPxComboBox>

                    <span id="tdsshortname" style="display: none" class="mandtry">
                        <img id="gridHistory1_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory" /></span>
                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtCode" ValidationGroup="contact" SetFocusOnError="true" ErrorMessage="Required" ForeColor="red"></asp:RequiredFieldValidator>--%>
                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtCode" ValidationGroup="contact" SetFocusOnError="true" ToolTip="Mandatory" class="pullrightClass fa fa-exclamation-circle abs iconRed" ErrorMessage=""></asp:RequiredFieldValidator>--%>
                </div>
            </div>

            <div class="col-md-3" style="display: none" id="divTCSPayable">
                <label id="lblTCSPayable">
                    <%--<asp:Label ID="Label1" runat="server" Text=" :"></asp:Label>--%>
                    TCS Payable
                </label>
                <em>*</em>


                <div class="relative">
                    <asp:ListBox ID="lstTCSPayable" CssClass="hide" runat="server" Font-Size="12px" ClientIDMode="Static" Width="253px"></asp:ListBox>
                    <span id="TCSPayablename" style="display: none" class="mandtry">
                        <img id="gridHistory1_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory" /></span>
                    <%--<asp:TextBox ID="txtSubAccount" runat="server" CssClass="form-control"></asp:TextBox>--%>
                </div>
                <span class="EcoheadCon_" id="tdTCSPayable" style="display: none">&nbsp;</span>
            </div>

            <div class="col-md-3">
                <label id="lblPostingLedger">TDS Posting Ledger: </label>
                <em>*</em>
                <div class="relative">
                    <%-- <asp:TextBox ID="txtMainAccount" runat="server" CssClass="form-control" OnTextChanged="txtMainAccount_TextChanged"></asp:TextBox>--%>
                    <asp:ListBox ID="lstMainAccount" runat="server" Font-Size="12px" ClientIDMode="Static" Width="253px"></asp:ListBox>

                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="lstMainAccount" InitialValue="0" ValidationGroup="contact" SetFocusOnError="true" ErrorMessage="Required" ForeColor="red"></asp:RequiredFieldValidator>--%>
                    <span id="mainacname" style="display: none" class="mandtry">
                        <img id="gridHistory1_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory" /></span>
                </div>
            </div>







            <div class="clear"></div>
            <div class="col-md-3" id="divPurchase">
                <label id="lblpurchaseAccount">
                    <%--<asp:Label ID="Label1" runat="server" Text=" :"></asp:Label>--%>
                    Purchase Account
                </label>


                <div class="relative">
                    <asp:ListBox ID="lstPurchase" CssClass="hide" runat="server" Font-Size="12px" ClientIDMode="Static" Width="253px"></asp:ListBox>
                    <span id="puracname" style="display: none" class="mandtry">
                        <img id="gridHistory1_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory" /></span>

                    <%--<asp:TextBox ID="txtSubAccount" runat="server" CssClass="form-control"></asp:TextBox>--%>
                </div>
            </div>

             <div class="col-md-3" id="divSales">
                <label id="lblSalesAccount">
                    <%--<asp:Label ID="Label1" runat="server" Text=" :"></asp:Label>--%>
                    Sales Account
                </label>
                <div class="relative">
                    <asp:ListBox ID="lstSales" CssClass="hide" runat="server" Font-Size="12px" ClientIDMode="Static" Width="253px"></asp:ListBox>
                    <span id="salesacname" style="display: none" class="mandtry">
                        <img id="gridHistory1_DXPEForm_efnew_DXEFL_DXEditor2_EIsales" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory" /></span>
                    <%--<asp:TextBox ID="txtSubAccount" runat="server" CssClass="form-control"></asp:TextBox>--%>
                </div>
            </div>

            <div class="col-md-3" style="display: none" id="divsubAC">
                <label id="lblSubAccount">
                    <%--<asp:Label ID="Label1" runat="server" Text=" :"></asp:Label>--%>
                    Sub Account
                </label>


                <div class="relative">
                    <asp:ListBox ID="lstSubAccount" CssClass="hide" runat="server" Font-Size="12px" ClientIDMode="Static" Width="253px"></asp:ListBox>
                    <span id="subacname" style="display: none" class="mandtry">
                        <img id="gridHistory1_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory" /></span>
                    <%--<asp:TextBox ID="txtSubAccount" runat="server" CssClass="form-control"></asp:TextBox>--%>
                </div>
                <span class="EcoheadCon_" id="tdSub1" style="display: none">&nbsp;</span>
            </div>
            <div class="col-md-3" style="display: block" id="divInterestLedger">
                <label id="lblInterestLedger">Interest Ledger: </label>
                <div class="relative">
                    <asp:ListBox ID="lstInterestLedger" runat="server" Font-Size="12px" ClientIDMode="Static" Width="253px"></asp:ListBox>
                    <span id="InterestLedger" style="display: none" class="mandtry">
                        <img id="gridHistory1_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory" /></span>
                </div>
            </div>

            <div class="col-md-3" style="display: block" id="divLateFeeLedger">
                <label id="lblLateFeeLedger">Late Fee Ledger: </label>
                <div class="relative">
                    <asp:ListBox ID="lstLateFeeLedger" runat="server" Font-Size="12px" ClientIDMode="Static" Width="253px"></asp:ListBox>
                    <span id="LateFeeLedger" style="display: none" class="mandtry">
                        <img id="gridHistory1_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory" /></span>
                </div>
            </div>

            <div class="col-md-3" style="display: block" id="divOthersLedger">
                <label id="lblOthersLedger">Others Ledger: </label>
                <div class="relative">
                    <asp:ListBox ID="lstOthersLedger" runat="server" Font-Size="12px" ClientIDMode="Static" Width="253px"></asp:ListBox>
                    <span id="OthersLedger" style="display: none" class="mandtry">
                        <img id="gridHistory1_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory" /></span>
                </div>
            </div>
            <div class="col-md-3" style="display: none" id="divCalculationBasedOn">
                <label>Calculation Based On </label>
                <div class=" simple-select">
                    <asp:DropDownList ID="ddlCalculationBasedOn" runat="server" CssClass="form-control">
                        <asp:ListItem Text="Select" Value="0"> </asp:ListItem>
                        <asp:ListItem Text="Inclusive GST" Value="I"> </asp:ListItem>
                        <asp:ListItem Text="Exclusive GST" Value="E"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <%--Rev Mantis Issue 24161--%>
            <div class="col-md-3" style="display: block" id="divAdvanceLedger">
                <label id="lblAdvanceLedger">Advance Ledger </label>
                <div class="relative">
                    <asp:ListBox ID="lstAdvanceLedger" runat="server" Font-Size="12px" ClientIDMode="Static" Width="253px"></asp:ListBox>
                    <span id="AdvanceLedger" style="display: none" class="mandtry">
                        <img id="gridHistory1_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory" /></span>
                </div>
            </div>
            <%--End of Rev Mantis Issue 24161--%>
            <div class="clear"></div>
            <div class="col-md-6">
                <label class="EcoheadCon_">
                    Description 
                </label>
                <div>
                    <%-- <asp:TextBox ID="txtDescription12" TextMode="MultiLine" runat="server"  CssClass="form-control" Height="62px" onkeypress="return CheckLength();"></asp:TextBox>--%>
                    <dxe:ASPxMemo ID="txtDescription" runat="server" Height="60px" CssClass="form-control" MaxLength="150"></dxe:ASPxMemo>

                </div>
            </div>

            <div class="clear"></div>
            <div class="col-md-12" style="margin-top: 12px">

                <input id="btnSave" type="button" value="Save" onclick="btnSave_Click()" class="btnUpdate btn btn-primary" />
                <input id="btnCancel" type="button" value="Cancel" class="btnUpdate btn btn-danger" onclick="Back('IframeTdsTcs.aspx')" />

            </div>
        </div>

        <div runat="server" id="divOldTds">
            <table>
                <tr>
                    <td class="EHEADER" colspan="2" style="text-align: left">
                        <h4>Add/Modify Rates</h4>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <%if (Session["PageAccess"].ToString().Trim() == "All" || Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "DelAdd")
                          { %>
                        <a class="btn btn-primary mb-10" href="javascript:void(0);" onclick="OnAddButtonClick()"><span>Add New</span> </a>
                        <%} %>
                        <% if (rights.CanExport)
                           { %>
                        <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnChange="if(!AvailableExportOption()){return false;}" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Value="0">Export to</asp:ListItem>
                            <asp:ListItem Value="1">PDF</asp:ListItem>
                            <asp:ListItem Value="2">XLS</asp:ListItem>
                            <asp:ListItem Value="3">RTF</asp:ListItem>
                            <asp:ListItem Value="4">CSV</asp:ListItem>

                        </asp:DropDownList>
                        <% } %>

                        <dxe:ASPxGridView ID="gridTdsTcs" runat="server" AutoGenerateColumns="False" Width="100%"
                            ClientInstanceName="gridTdsTcs" DataSourceID="SqlTscTcsRate" KeyFieldName="TDSTCSRates_ID"
                            OnRowInserting="gridTdsTcs_RowInserting" OnCustomJSProperties="gridTdsTcs_CustomJSProperties"
                            OnStartRowEditing="gridTdsTcs_StartRowEditing" OnInitNewRow="gridTdsTcs_InitNewRow" OnCellEditorInitialize="gridTdsTcs_CellEditorInitialize">
                            <Styles>
                                <LoadingPanel ImageSpacing="10px">
                                </LoadingPanel>
                                <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                </Header>
                            </Styles>
                            <SettingsSearchPanel Visible="True" />
                            <Settings ShowTitlePanel="True" ShowStatusBar="Visible" ShowGroupPanel="True" ShowFilterRow="true" ShowFilterRowMenu="true" />
                            <SettingsBehavior AllowFocusedRow="false" ConfirmDelete="True" />
                            <SettingsText PopupEditFormCaption="Add/Modify Rates" ConfirmDelete="Are You Want To Delete This Record?" />
                            <SettingsEditing Mode="PopupEditForm" PopupEditFormHeight="250px" PopupEditFormHorizontalAlign="Center"
                                PopupEditFormModal="True" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="600px" />
                            <SettingsPager ShowSeparators="True" AlwaysShowPager="True" NumericButtonCount="8"
                                PageSize="20">
                                <FirstPageButton Visible="True">
                                </FirstPageButton>
                                <LastPageButton Visible="True">
                                </LastPageButton>
                            </SettingsPager>
                            <Columns>
                                <dxe:GridViewDataTextColumn FieldName="TDSTCSRates_ID" Visible="False">
                                    <EditFormSettings Visible="False" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Short Name" FieldName="TDSTCSRates_Code" VisibleIndex="0">
                                    <EditFormSettings Visible="False" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataDateColumn Caption="From" FieldName="TDSTCSRates_DateFrom" VisibleIndex="1" ReadOnly="false">
                                    <EditFormSettings Visible="True" />
                                    <PropertiesDateEdit EditFormatString="dd-MM-yyyy" DisplayFormatString="dd MMM yyyy">
                                        <ValidationSettings ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom" SetFocusOnError="True">
                                            <RequiredField IsRequired="true" ErrorText="*" />
                                        </ValidationSettings>
                                    </PropertiesDateEdit>
                                </dxe:GridViewDataDateColumn>
                                <dxe:GridViewDataDateColumn Caption="To" FieldName="TDSTCSRates_DateTo"
                                    VisibleIndex="2">
                                    <PropertiesDateEdit EditFormatString="dd-MM-yyyy" DisplayFormatString="dd MMM yyyy"></PropertiesDateEdit>

                                    <EditFormSettings Visible="False" />
                                </dxe:GridViewDataDateColumn>
                                <dxe:GridViewDataTextColumn Caption="Rate" FieldName="TDSTCSRates_Rate" VisibleIndex="3" ReadOnly="false">
                                    <EditFormSettings Visible="True" />
                                    <PropertiesTextEdit MaskSettings-Mask="&lt;0..99g&gt;.&lt;00..999&gt;" MaskSettings-IncludeLiterals="DecimalSymbol"
                                        ValidationSettings-ErrorDisplayMode="None">
                                        <MaskSettings Mask="&lt;0..99g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol"></MaskSettings>

                                        <ValidationSettings ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom" SetFocusOnError="True">
                                            <RequiredField IsRequired="true" ErrorText="*" />
                                        </ValidationSettings>
                                    </PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Surcharge" Visible="true" FieldName="TDSTCSRates_Surcharge"
                                    VisibleIndex="4" ReadOnly="false">
                                    <EditFormSettings Visible="True" />
                                    <PropertiesTextEdit MaskSettings-Mask="&lt;0..99g&gt;.&lt;00..99&gt;" MaskSettings-IncludeLiterals="DecimalSymbol"
                                        ValidationSettings-ErrorDisplayMode="None">
                                        <MaskSettings Mask="&lt;0..99g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol"></MaskSettings>

                                        <ValidationSettings ErrorDisplayMode="None"></ValidationSettings>
                                    </PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="EduCess" Visible="true" FieldName="TDSTCSRates_EduCess"
                                    VisibleIndex="5">
                                    <EditFormSettings Visible="True" />
                                    <PropertiesTextEdit MaskSettings-Mask="&lt;0..99g&gt;.&lt;00..99&gt;" MaskSettings-IncludeLiterals="DecimalSymbol"
                                        ValidationSettings-ErrorDisplayMode="None">
                                        <MaskSettings Mask="&lt;0..99g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol"></MaskSettings>

                                        <ValidationSettings ErrorDisplayMode="None"></ValidationSettings>
                                    </PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="HgrEduCess" Visible="true" FieldName="TDSTCSRates_HgrEduCess"
                                    VisibleIndex="6">
                                    <EditFormSettings Visible="True" />
                                    <PropertiesTextEdit MaskSettings-Mask="&lt;0..99g&gt;.&lt;00..99&gt;" MaskSettings-IncludeLiterals="DecimalSymbol"
                                        ValidationSettings-ErrorDisplayMode="None">
                                        <MaskSettings Mask="&lt;0..99g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol"></MaskSettings>

                                        <ValidationSettings ErrorDisplayMode="None"></ValidationSettings>
                                    </PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Applicable Amount" Visible="true" FieldName="TDSTCSRates_ApplicableAmount"
                                    VisibleIndex="7">
                                    <EditFormSettings Visible="True" />
                                    <PropertiesTextEdit MaskSettings-Mask="&lt;0..99999999999g&gt;.&lt;00..99&gt;" MaskSettings-IncludeLiterals="DecimalSymbol"
                                        ValidationSettings-ErrorDisplayMode="None">
                                        <MaskSettings Mask="&lt;0..99999999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol"></MaskSettings>

                                        <ValidationSettings ErrorDisplayMode="None"></ValidationSettings>
                                    </PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewCommandColumn VisibleIndex="8" ShowDeleteButton="true" ShowEditButton="true" ShowUpdateButton="true" ShowCancelButton="true" Width="60px">
                                    <%-- <DeleteButton Visible="True">
                                    </DeleteButton>
                                    <EditButton Visible="True">
                                    </EditButton>--%>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <HeaderTemplate>
                                        <span style="width: auto">Actions</span>
                                    </HeaderTemplate>
                                </dxe:GridViewCommandColumn>
                            </Columns>
                            <SettingsCommandButton>

                                <EditButton ButtonType="Image" Image-Url="/assests/images/Edit.png">
                                    <Image Url="/assests/images/Edit.png"></Image>
                                </EditButton>
                                <DeleteButton ButtonType="Image" Image-Url="/assests/images/Delete.png">
                                    <Image Url="/assests/images/Delete.png"></Image>
                                </DeleteButton>
                                <UpdateButton ButtonType="Button" Text="Update" Styles-Style-CssClass="btn btn-primary"></UpdateButton>
                                <CancelButton ButtonType="Button" Text="Cancel" Styles-Style-CssClass="btn btn-danger"></CancelButton>
                            </SettingsCommandButton>
                            <ClientSideEvents EndCallback="function(s,e){ShowDateEn(s.cpInsertEna);}" />
                        </dxe:ASPxGridView>



                        <asp:SqlDataSource ID="SqlTscTcsRate" runat="server"
                            SelectCommand="SELECT * FROM [Config_TDSTCSRates] where TDSTCSRates_Code=@TDSTCSRates_Code"
                            DeleteCommand="DELETE FROM [Config_TDSTCSRates] WHERE [TDSTCSRates_ID] = @TDSTCSRates_ID"
                            InsertCommand="TdsTcsRate_Insert" InsertCommandType="StoredProcedure" UpdateCommand="UPDATE [Config_TDSTCSRates] SET  [TDSTCSRates_DateFrom] = @TDSTCSRates_DateFrom, [TDSTCSRates_Rate] = @TDSTCSRates_Rate, [TDSTCSRates_Surcharge] = @TDSTCSRates_Surcharge, [TDSTCSRates_EduCess] = @TDSTCSRates_EduCess, [TDSTCSRates_HgrEduCess] = @TDSTCSRates_HgrEduCess, [TDSTCSRates_ModifyUser] = @TDSTCSRates_ModifyUser, [TDSTCSRates_ModifyDateTime] = getdate(), [TDSTCSRates_ApplicableAmount] = @TDSTCSRates_ApplicableAmount WHERE [TDSTCSRates_ID] = @TDSTCSRates_ID">
                            <SelectParameters>
                                <asp:SessionParameter Name="TDSTCSRates_Code" SessionField="KeyVal" Type="string" />
                            </SelectParameters>
                            <DeleteParameters>
                                <asp:Parameter Name="TDSTCSRates_ID" Type="Int64" />
                            </DeleteParameters>
                            <UpdateParameters>
                                <asp:Parameter Name="TDSTCSRates_DateFrom" Type="DateTime" />
                                <asp:Parameter Name="TDSTCSRates_Rate" Type="Decimal" />
                                <asp:Parameter Name="TDSTCSRates_Surcharge" Type="Decimal" />
                                <asp:Parameter Name="TDSTCSRates_EduCess" Type="Decimal" />
                                <asp:Parameter Name="TDSTCSRates_HgrEduCess" Type="Decimal" />
                                <asp:SessionParameter Name="TDSTCSRates_ModifyUser" SessionField="userid" Type="Int32" />
                                <asp:Parameter Name="TDSTCSRates_ApplicableAmount" Type="Decimal" />
                                <asp:Parameter Name="TDSTCSRates_ID" Type="Int64" />
                            </UpdateParameters>
                            <InsertParameters>
                                <asp:Parameter Name="TDSTCSRates_Code" Type="String" />
                                <asp:Parameter Name="TDSTCSRates_DateFrom" Type="DateTime" />
                                <asp:Parameter Name="TDSTCSRates_Rate" Type="Decimal" />
                                <asp:Parameter Name="TDSTCSRates_Surcharge" Type="Decimal" />
                                <asp:Parameter Name="TDSTCSRates_EduCess" Type="Decimal" />
                                <asp:Parameter Name="TDSTCSRates_HgrEduCess" Type="Decimal" />
                                <asp:Parameter Name="TDSTCSRates_ApplicableAmount" Type="Decimal" />
                                <asp:SessionParameter Name="TDSTCSRates_CreateUser" SessionField="userid" Type="Int32" />
                            </InsertParameters>
                        </asp:SqlDataSource>
                    </td>
                </tr>
                <tr style="display: none">
                    <td style="height: 102px">
                        <asp:HiddenField ID="txtMainAccount_hidden" runat="server" />
                        <asp:HiddenField ID="txtSubAccount_hidden" runat="server" />
                        <dxe:ASPxComboBox ID="comboInsert" runat="server" ClientInstanceName="comboInsert"
                            ValueType="System.String" OnCallback="ASPxComboBox1_Callback" OnCustomJSProperties="ASPxComboBox1_CustomJSProperties">
                            <ClientSideEvents EndCallback="function(s,e) {ShowInsert(comboInsert.cpInsert);}" />
                        </dxe:ASPxComboBox>
                        <asp:HiddenField ID="hddnVal" runat="server" />
                        <asp:HiddenField ID="hdnSubACCode" runat="server" />

                        <asp:HiddenField ID="hdnOldTDS" runat="server" />


                        <asp:HiddenField ID="hdnInterestLedgerID" runat="server" />
                        <asp:HiddenField ID="hdnLateFeeLedgerID" runat="server" />
                        <asp:HiddenField ID="hdnOthersLedgerID" runat="server" />
                        <%--Rev Mantis Issue 24161--%>
                        <asp:HiddenField ID="hdnAdvanceLedgerID" runat="server" />
                        <%--End of Rev Mantis Issue 24161--%>
                    </td>
                </tr>
            </table>
        </div>

        <div runat="server" id="divNewTDS">
            <table>
                <tr>
                    <td class="EHEADER" colspan="2" style="text-align: left">
                        <h4>Add/Modify Rates</h4>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <%if (Session["PageAccess"].ToString().Trim() == "All" || Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "DelAdd")
                          { %>
                        <a class="btn btn-primary mb-10" href="javascript:void(0);" onclick="OnAddButtonNewClick()"><span>Add New</span> </a>
                        <%} %>
                        <% if (rights.CanExport)
                           { %>
                        <asp:DropDownList ID="DropDownList1" runat="server" CssClass="btn btn-sm btn-primary" OnChange="if(!AvailableExportOption()){return false;}" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Value="0">Export to</asp:ListItem>
                            <asp:ListItem Value="1">PDF</asp:ListItem>
                            <asp:ListItem Value="2">XLS</asp:ListItem>
                            <asp:ListItem Value="3">RTF</asp:ListItem>
                            <asp:ListItem Value="4">CSV</asp:ListItem>

                        </asp:DropDownList>
                        <% } %>

                        <dxe:ASPxGridView ID="ASPxTdsTcsNew" runat="server" AutoGenerateColumns="False" Width="100%"
                            ClientInstanceName="gridTdsTcsNew" KeyFieldName="TDSTCSRates_ID" DataSourceID="LinqServerModeTdsTcsNew" OnAfterPerformCallback="ASPxTdsTcsNew_AfterPerformCallback"
                            OnStartRowEditing="gridTdsTcs_StartRowEditing" OnInitNewRow="gridTdsTcs_InitNewRow" OnCellEditorInitialize="gridTdsTcs_CellEditorInitialize">
                            <Styles>
                                <LoadingPanel ImageSpacing="10px">
                                </LoadingPanel>
                                <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                </Header>
                            </Styles>
                            <SettingsSearchPanel Visible="True" />
                            <Settings ShowTitlePanel="True" ShowStatusBar="Visible" ShowGroupPanel="True" ShowFilterRow="true" ShowFilterRowMenu="true" />
                            <SettingsBehavior AllowFocusedRow="false" ConfirmDelete="True" />
                            <SettingsText PopupEditFormCaption="Add/Modify Rates" ConfirmDelete="Are You Want To Delete This Record?" />
                            <SettingsEditing Mode="PopupEditForm" PopupEditFormHeight="250px" PopupEditFormHorizontalAlign="Center"
                                PopupEditFormModal="True" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="600px" />
                            <SettingsPager ShowSeparators="True" AlwaysShowPager="True" NumericButtonCount="8"
                                PageSize="20">
                                <FirstPageButton Visible="True">
                                </FirstPageButton>
                                <LastPageButton Visible="True">
                                </LastPageButton>
                            </SettingsPager>
                            <Columns>
                                <dxe:GridViewDataTextColumn FieldName="TDSTCSRates_ID" Visible="False">
                                    <EditFormSettings Visible="False" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Short Name" FieldName="TDSTCSRates_Code" VisibleIndex="0">
                                    <EditFormSettings Visible="False" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataDateColumn Caption="From" FieldName="TDSTCSRates_DateFrom" VisibleIndex="1" ReadOnly="false">
                                    <EditFormSettings Visible="False" />
                                    <PropertiesDateEdit EditFormatString="dd-MM-yyyy" DisplayFormatString="dd MMM yyyy">
                                        <ValidationSettings ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom" SetFocusOnError="True">
                                            <RequiredField IsRequired="true" ErrorText="*" />
                                        </ValidationSettings>
                                    </PropertiesDateEdit>
                                </dxe:GridViewDataDateColumn>
                                <dxe:GridViewDataDateColumn Caption="To" FieldName="TDSTCSRates_DateTo"
                                    VisibleIndex="2">
                                    <PropertiesDateEdit EditFormatString="dd-MM-yyyy" DisplayFormatString="dd MMM yyyy"></PropertiesDateEdit>

                                    <EditFormSettings Visible="False" />
                                </dxe:GridViewDataDateColumn>
                                <dxe:GridViewDataTextColumn Caption="Rate" FieldName="TDSTCSRates_Rate" VisibleIndex="3" ReadOnly="false">
                                    <EditFormSettings Visible="True" />
                                    <PropertiesTextEdit MaskSettings-Mask="&lt;0..99g&gt;.&lt;00..999&gt;" MaskSettings-IncludeLiterals="DecimalSymbol"
                                        ValidationSettings-ErrorDisplayMode="None">
                                        <MaskSettings Mask="&lt;0..99g&gt;.&lt;00..999&gt;" IncludeLiterals="DecimalSymbol"></MaskSettings>

                                        <ValidationSettings ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom" SetFocusOnError="True">
                                            <RequiredField IsRequired="true" ErrorText="*" />
                                        </ValidationSettings>
                                    </PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Surcharge" Visible="true" FieldName="TDSTCSRates_Surcharge"
                                    VisibleIndex="4" ReadOnly="false">
                                    <EditFormSettings Visible="True" />
                                    <PropertiesTextEdit MaskSettings-Mask="&lt;0..99g&gt;.&lt;00..99&gt;" MaskSettings-IncludeLiterals="DecimalSymbol"
                                        ValidationSettings-ErrorDisplayMode="None">
                                        <MaskSettings Mask="&lt;0..99g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol"></MaskSettings>

                                        <ValidationSettings ErrorDisplayMode="None"></ValidationSettings>
                                    </PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Applicable for" FieldName="TDSTCSRates_Applicable" VisibleIndex="5">
                                    <EditFormSettings Visible="True" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="EduCess" Visible="true" FieldName="TDSTCSRates_EduCess"
                                    VisibleIndex="5">
                                    <EditFormSettings Visible="True" />
                                    <PropertiesTextEdit MaskSettings-Mask="&lt;0..99g&gt;.&lt;00..99&gt;" MaskSettings-IncludeLiterals="DecimalSymbol"
                                        ValidationSettings-ErrorDisplayMode="None">
                                        <MaskSettings Mask="&lt;0..99g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol"></MaskSettings>

                                        <ValidationSettings ErrorDisplayMode="None"></ValidationSettings>
                                    </PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="HgrEduCess" Visible="true" FieldName="TDSTCSRates_HgrEduCess"
                                    VisibleIndex="6">
                                    <EditFormSettings Visible="True" />
                                    <PropertiesTextEdit MaskSettings-Mask="&lt;0..99g&gt;.&lt;00..99&gt;" MaskSettings-IncludeLiterals="DecimalSymbol"
                                        ValidationSettings-ErrorDisplayMode="None">
                                        <MaskSettings Mask="&lt;0..99g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol"></MaskSettings>

                                        <ValidationSettings ErrorDisplayMode="None"></ValidationSettings>
                                    </PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Applicable Amount" Visible="true" FieldName="TDSTCSRates_ApplicableAmount"
                                    VisibleIndex="7">
                                    <EditFormSettings Visible="True" />
                                    <PropertiesTextEdit MaskSettings-Mask="&lt;0..99999999999g&gt;.&lt;00..99&gt;" MaskSettings-IncludeLiterals="DecimalSymbol"
                                        ValidationSettings-ErrorDisplayMode="None">
                                        <MaskSettings Mask="&lt;0..99999999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol"></MaskSettings>

                                        <ValidationSettings ErrorDisplayMode="None"></ValidationSettings>
                                    </PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <%--    <dxe:GridViewCommandColumn VisibleIndex="8" ShowDeleteButton="true" ShowEditButton="true" ShowUpdateButton="true" ShowCancelButton="true" Width="60px">
                                  <a href="javascript:void(0);" onclick="ClickOnEdit('<%# Container.KeyValue %>')" id="a_editInvoice" class="pad" title="Edit">
                                <img src="../../../assests/images/info.png" /></a>

                            <a href="javascript:void(0);" onclick="OnClickDelete('<%# Container.KeyValue %>')" class="pad" title="Delete" id="a_delete">
                                <img src="../../../assests/images/Delete.png" /></a>
                                <HeaderStyle HorizontalAlign="Center" />
                                <HeaderTemplate>
                                    <span style="width:auto">Actions</span>
                                </HeaderTemplate>
                            </dxe:GridViewCommandColumn>--%>
                                <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="7" Width="240px">
                                    <DataItemTemplate>

                                        <a href="javascript:void(0);" onclick="ClickOnEdit('<%# Container.KeyValue %>')" id="a_editInvoice" class="pad" title="Edit">
                                            <img src="../../../assests/images/info.png" /></a>

                                        <a href="javascript:void(0);" onclick="OnClickDelete('<%# Container.KeyValue %>')" class="pad" title="Delete" id="a_delete">
                                            <img src="../../../assests/images/Delete.png" /></a>

                                        <%-- <% if (rights.CanView)
                                       { %>
                                    <a href="javascript:void(0);" onclick="OnclickView('<%# Container.KeyValue %>')" class="pad" title="View Attachment">
                                        <img src="../../../assests/images/viewIcon.png" />
                                    </a><%} %>--%>
                                    </DataItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <CellStyle HorizontalAlign="Center"></CellStyle>
                                    <HeaderTemplate><span>Actions</span></HeaderTemplate>
                                    <EditFormSettings Visible="False"></EditFormSettings>
                                    <Settings AllowAutoFilterTextInputTimer="False" />
                                </dxe:GridViewDataTextColumn>
                            </Columns>
                            <ClientSideEvents EndCallback="function(s,e){ShowDateEn(s.cpInsertEna);}" />
                        </dxe:ASPxGridView>

                        <dx:LinqServerModeDataSource ID="LinqServerModeTdsTcsNew" runat="server" OnSelecting="LinqServerModeTdsTcsNew_Selecting"
                            ContextTypeName="ERPDataClassesDataContext" TableName="V_TDSTCSNewList" />

                    </td>
                </tr>

            </table>
        </div>
    </div>
    </div>
    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>
    <%--</span></span></span>--%>

    <div class="modal fade pmsModal w50" id="HoliDayDetail" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document" style="max-width: 50%;">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel">Add/Modify Rates </h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="pmsForm">
                        <div class="form-group row">
                            <label for="" class="col-sm-2 col-form-label">From : <span class="red">*</span></label>
                            <div class="col-sm-4">
                                <div class=" relative">
                                    <dxe:ASPxDateEdit ID="FormDetlDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFrmDateDetl"
                                        Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True" CssClass="dateEditInput">
                                        <ButtonStyle Width="13px">
                                        </ButtonStyle>
                                    </dxe:ASPxDateEdit>
                                </div>
                            </div>
                            <label for="" class="col-sm-2 col-form-label">Rate : <span class="red">*</span></label>
                            <div class="col-sm-4">
                                <dxe:ASPxTextBox ID="txt_Rate" runat="server" Width="100%" ClientInstanceName="ctxtRate" CssClass="number" DisplayFormatString="0.000">
                                    <%--<ClientSideEvents LostFocus="ReBindGrid_Currency" GotFocus="GetPreviousCurrency" />--%>
                                    <MaskSettings Mask="&lt;0..99&gt;.&lt;00..999&gt;" IncludeLiterals="DecimalSymbol" />
                                </dxe:ASPxTextBox>
                            </div>
                        </div>

                        <div class="formLine"></div>
                        <div class="form-group row">
                            <label for="" class="col-sm-2 col-form-label">Surcharge : </label>
                            <div class="col-sm-4">
                                <div class=" relative">
                                    <dxe:ASPxTextBox ID="txt_Surcharge" runat="server" Width="100%" ClientInstanceName="ctxtSurcharge" CssClass="number" DisplayFormatString="0.00">
                                        <MaskSettings Mask="&lt;0..99&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                    </dxe:ASPxTextBox>
                                </div>
                            </div>
                            <label for="" class="col-sm-2 col-form-label">Applicable for : </label>
                            <div class="col-sm-4">
                                <dxe:ASPxComboBox ID="aspxDeducteesNew" ClientInstanceName="caspxDeducteesNew" runat="server" SelectedIndex="0"
                                    ValueType="System.String" Width="100%" EnableSynchronization="True">
                                </dxe:ASPxComboBox>
                            </div>
                        </div>
                        <div class="formLine"></div>
                        <div class="form-group row">
                            <label for="" class="col-sm-2 col-form-label">EduCess :</label>
                            <div class="col-sm-4">
                                <div class=" relative">
                                    <dxe:ASPxTextBox ID="txt_EduCess" runat="server" Width="100%" ClientInstanceName="ctxtEduCess" CssClass="number" DisplayFormatString="0.00">
                                        <MaskSettings Mask="&lt;0..99&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                    </dxe:ASPxTextBox>
                                </div>
                            </div>
                            <label for="" class="col-sm-2 col-form-label">HgrEduCess :</label>
                            <div class="col-sm-4">
                                <dxe:ASPxTextBox ID="txt_HgrEduCess" runat="server" Width="100%" ClientInstanceName="ctxtHgrEduCess" CssClass="number" DisplayFormatString="0.00">
                                    <MaskSettings Mask="&lt;0..99&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                </dxe:ASPxTextBox>
                            </div>
                        </div>
                        <div class="formLine"></div>
                        <div class="form-group row">
                            <label for="" class="col-sm-2 col-form-label">Applicable Amount : </label>
                            <div class="col-sm-4">
                                <dxe:ASPxTextBox ID="txt_ApplicableAmount" runat="server" Width="100%" ClientInstanceName="ctxtApplicableAmount" CssClass="number" DisplayFormatString="0.00">
                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                </dxe:ASPxTextBox>
                            </div>
                            <label for="" class="col-sm-2 col-form-label">Round Off Upto : </label>
                            <div class="col-sm-4">
                                <dxe:ASPxComboBox ID="cmbRouding" ClientInstanceName="cCmbRouding" runat="server" SelectedIndex="0"
                                    ValueType="System.String" Width="100%" EnableSynchronization="True">
                                    <Items>
                                        <dxe:ListEditItem Text="None" Value="0" />
                                        <dxe:ListEditItem Text="1 Rupee" Value="1" />
                                        <dxe:ListEditItem Text="5 Rupee" Value="2" />
                                        <dxe:ListEditItem Text="10 Rupee" Value="3" />

                                    </Items>
                                </dxe:ASPxComboBox>
                            </div>
                        </div>
                        <div class="formLine"></div>

                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger btn-radius" data-dismiss="modal" onclick="Close()">Close</button>
                    <button type="button" class="btn btn-success btn-radius" id="btnSaveNew" onclick="AddRate()">
                        <span class="btn-icon"><i class="fa fa-plus"></i></span>Save</button>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="TDSTCSRates_ID" />
    <asp:HiddenField runat="server" ID="TDSTCSRates_Code" />
    <asp:HiddenField runat="server" ID="ltsPurchase_code" />


    <asp:HiddenField runat="server" ID="ltsSales_code" />

</asp:Content>
