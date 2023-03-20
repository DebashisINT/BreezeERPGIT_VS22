<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                20-03-2023        2.0.36           Pallab              25733 : Master pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Title="Countries" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.Master.management_master_Country" CodeBehind="Country.aspx.cs" %>

<%--<%@ Register Assembly="DevExpress.Web.v10.2.Export, Version=10.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.Export" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe000001" %>
<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxHiddenField" TagPrefix="dxhf" %>--%>
<%--<%@ Register assembly="DevExpress.Web.v10.2" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v10.2" namespace="DevExpress.Web" tagprefix="dx" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <style type="text/css">
        .stateDiv {
            height: 25px;
            width: 68px;
            float: left;
        }

        .dxpc-headerContent {
            color: white;
        }
    
        .dxgvHeader {
            border: 1px solid #2c4182 !important;
            background-color: #415698 !important;
        }

            .dxgvHeader, .dxgvHeader table {
                color: #fff !important;
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

        /*select
        {
            height: 30px !important;
            border-radius: 4px;
            -webkit-appearance: none;
            position: relative;
            z-index: 1;
            background-color: transparent;
            padding-left: 10px !important;
            padding-right: 22px !important;
        }*/

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
            top: 26px;
            right: 13px;
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
        .TableMain100 #GrdHolidays , #GridCountry
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
                margin-top: 3px;
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
    <script type="text/javascript" src="../../CentralData/JSScript/GenericJScript.js"></script>

    <script language="javascript" type="text/javascript">
        //function SignOff() {
        //    window.parent.SignOff()
        //}
        //function height() {
        //    if (document.body.scrollHeight <= 500)
        //        window.frameElement.height = '500px';
        //    else
        //        window.frameElement.height = document.body.scrollHeight;
        //    window.frameElement.widht = document.body.scrollWidht;
        //}
        function gridRowclick(s, e) {
            //alert('hi');
            $('#gridcrmCampaign').find('tr').removeClass('rowActive');
            $('.floatedBtnArea').removeClass('insideGrid');
            //$('.floatedBtnArea a .ico').css({ 'opacity': '0' });
            $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea').addClass('insideGrid');
            $(s.GetRow(e.visibleIndex)).addClass('rowActive');
            setTimeout(function () {
                //alert('delay');
                var lists = $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a');
                //$(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a .ico').css({'opacity': '1'});
                //$(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a').each(function (e) {
                //    setTimeout(function () {
                //        $(this).fadeIn();
                //    }, 100);
                //});    
                $.each(lists, function (index, value) {
                    //console.log(index);
                    //console.log(value);
                    setTimeout(function () {
                        console.log(value);
                        $(value).css({ 'opacity': '1' });
                    }, 100);
                });
            }, 200);
        }
    </script>

    <script type="text/javascript">
        $(document).ready(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                grid.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                grid.SetWidth(cntWidth);
            }
            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    grid.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    grid.SetWidth(cntWidth);
                }

            });
        });
    </script>

   

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main">
        <div class="panel-heading">
        <div class="panel-title">
            <h3>Countries</h3>
        </div>
    </div>
        <div class="form_main">
        <div class="Main">
            <%--<div class="TitleArea">
                <strong><span style="color: #000099">Country List</span></strong>
            </div>--%>

            <div class="SearchArea clearfix">
                <div class="FilterSide">
                    <div style="float: left; padding-right: 5px;">
                        <% if (rights.CanAdd)
                           { %>
                        <a href="javascript:void(0);" onclick="fn_PopUpOpen()" class="btn btn-success btn-radius"><span class="btn-icon"><i class="fa fa-plus" ></i></span>Add New </a>
                        <% } %>
                        <%--<a href="javascript:ShowHideFilter('s');" class="btn btn-primary"><span>Show Filter</span></a>--%>
                        <% if (rights.CanExport)
                                               { %>
                        <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-radius" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                            <asp:ListItem Value="0">Export to</asp:ListItem>
                            <asp:ListItem Value="1">PDF</asp:ListItem>
                            <asp:ListItem Value="2">XLS</asp:ListItem>
                            <asp:ListItem Value="3">RTF</asp:ListItem>
                            <asp:ListItem Value="4">CSV</asp:ListItem>
                        </asp:DropDownList>
                        <% } %>
                    </div>
                    <%--...................................Code Commented By Sam on 28092016................................. --%>

                    <%-- <div class="pull-left">
                        <a href="javascript:ShowHideFilter('All');" class="btn btn-primary"><span>All Records</span></a>
                    </div>--%>
                    <%-- ...................................Code Above Commented By Sam on 28092016.................................--%>

                    <%--<div class="ExportSide pull-right">
                        <div>
                            <dxe:ASPxComboBox ID="cmbExport" runat="server" AutoPostBack="true"
                                Font-Bold="False" ForeColor="black" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged"
                                ValueType="System.Int32" Width="130px">
                                <Items>
                                    <dxe:ListEditItem Text="Select" Value="0" />
                                    <dxe:ListEditItem Text="PDF" Value="1" />
                                    <dxe:ListEditItem Text="XLS" Value="2" />
                                    <dxe:ListEditItem Text="RTF" Value="3" />
                                    <dxe:ListEditItem Text="CSV" Value="4" />
                                </Items>
                                <ButtonStyle>
                                </ButtonStyle>
                                <ItemStyle>
                                    <HoverStyle>
                                    </HoverStyle>
                                </ItemStyle>
                                <Border BorderColor="black" />
                                <DropDownButton Text="Export">
                                </DropDownButton>
                            </dxe:ASPxComboBox>
                        </div>
                    </div>--%>
                </div>

            </div>

            <div class="GridViewArea relative">
                <dxe:ASPxGridView ID="GridCountry" runat="server" AutoGenerateColumns="False" ClientInstanceName="grid"
                    KeyFieldName="cou_id" Width="100%" OnHtmlEditFormCreated="GridCountry_HtmlEditFormCreated" Settings-VerticalScrollableHeight="280" Settings-VerticalScrollBarMode="Auto"
                    OnCustomCallback="GridCountry_CustomCallback" SettingsBehavior-AllowFocusedRow="true" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" >
                    <%--OnHtmlRowCreated="GridCountry_HtmlRowCreated"--%>
                    <SettingsSearchPanel Visible="True" Delay="5000" />
                    <Columns>
                        <dxe:GridViewDataTextColumn Caption="Country ID" FieldName="cou_id" ReadOnly="True"
                            Visible="False" VisibleIndex="0">
                            <EditCellStyle HorizontalAlign="Left">
                            </EditCellStyle>
                            <EditFormSettings Visible="False" VisibleIndex="1" />
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="Country Name" FieldName="cou_country" VisibleIndex="1"
                            Width="100%">
                            <EditFormSettings Visible="True" />
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>


                        <%--                        <dxe:GridViewDataTextColumn Caption="NSE Code" FieldName="Country_NSECode" VisibleIndex="2"
                            Width="8%" Visible="false">
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="BSE Code" FieldName="Country_BSECode" VisibleIndex="3"
                            Width="8%" Visible="false">
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="MCX Code" FieldName="Country_MCXCode" VisibleIndex="4"
                            Width="8%" Visible="false">
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="MCXSX Code" FieldName="Country_MCXSXCode"
                            VisibleIndex="5" Width="8%" Visible="false">
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="NCDEX Code" FieldName="Country_NCDEXCode"
                            VisibleIndex="6" Width="8%" Visible="false">
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="CDSL Code" FieldName="Country_CdslID" VisibleIndex="7"
                            Width="8%" Visible="false">
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="NSDL Code" FieldName="Country_NsdlID" VisibleIndex="8"
                            Width="8%" Visible="false">
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="NDML Code" FieldName="Country_NDMLId" VisibleIndex="9"
                            Width="8%" Visible="false">
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="DOTEX Code" FieldName="Country_DotExID" VisibleIndex="10"
                            Width="8%" Visible="false">
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="CVLID Code" FieldName="Country_CVLID" VisibleIndex="11"
                            Width="8%" Visible="false">
                        </dxe:GridViewDataTextColumn>--%>
                        <dxe:GridViewDataTextColumn CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" Width="1px">
                            <HeaderTemplate>
                                <span></span>
                            </HeaderTemplate>
                            <DataItemTemplate>
                                <div class='floatedBtnArea'>
                                <% if (rights.CanEdit)
                                   { %>
                                <a href="javascript:void(0);" onclick="fn_EditCountry('<%#Container.KeyValue %>')" class="" title="">
                                    <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Edit</span></a>
                                <% } %>
                                <% if (rights.CanDelete)
                                   { %>
                                <a href="javascript:void(0);" onclick="fn_DeleteCountry('<%#Container.KeyValue %>')" title="">
                                    <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>Delete</span></a>
                                <% } %>
                                </div>
                            </DataItemTemplate>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                    </Columns>
                    <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                    <SettingsSearchPanel Visible="True" />
                    <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />
                    <%--<SettingsPager NumericButtonCount="20" PageSize="20" ShowSeparators="True" AlwaysShowPager="True">
                        <FirstPageButton Visible="True">
                        </FirstPageButton>
                        <LastPageButton Visible="True">
                        </LastPageButton>
                    </SettingsPager>
                    <SettingsBehavior ColumnResizeMode="NextColumn" />--%>




                    <ClientSideEvents EndCallback="function (s, e) {grid_EndCallBack();}" RowClick="gridRowclick" />
                </dxe:ASPxGridView>
            </div>

            <div class="PopUpArea">
                <dxe:ASPxPopupControl ID="PopupCountry" runat="server" ClientInstanceName="cPopupCountry"
                    Width="400px" Height="100px" HeaderText="Add/Modify Country" PopupHorizontalAlign="Windowcenter"
                    PopupVerticalAlign="WindowCenter" CloseAction="closeButton" Modal="true">
                    <ContentCollection>
                        <dxe:PopupControlContentControl ID="countryPopup" runat="server">
                            <div class="Top clearfix">
                                <div style="padding-top: 5px;" class="col-md-12">
                                    <div class="stateDiv" style="padding-top: 5px;">Country:<span style="color: red;">*</span></div>
                                    <div style="padding-top: 5px;">
                                        <dxe:ASPxTextBox ID="txtCountryName" ClientInstanceName="ctxtCountryName" ClientEnabled="true"
                                            runat="server" Width="236px" MaxLength="50">
                                        </dxe:ASPxTextBox>
                                        <div id="valid" style="display: none; position: absolute; right: -4px; top: 30px;">
                                            <img id="grid_DXPEForm_DXEFL_DXEditor2_EI" title="Mandatory" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-YRohc" alt="Required" /></div>
                                    </div>
                                </div>
                                <%--<div style="padding-top: 5px; display: none;" class="col-md-4">
                                    <div style="height: 20px; background-color: Gray; text-align: center">
                                        <h5>Static Code</h5>
                                    </div>
                                    <div style="background-color: Gray; overflow: hidden">
                                        <div style="height: 20px; width: 130px; float: left; margin-left: 70px;">Exchange</div>
                                        <div style="height: 20px; width: 200px; text-align: left; margin-left: 50px;">
                                            Value
                                        </div>
                                    </div>
                                    <div class="stateDiv" style="padding-top: 5px;">
                                        NSE Code
                                    </div>
                                    <div style="padding-top: 5px;">
                                        <dxe:ASPxTextBox ID="txtNseCode" ClientInstanceName="ctxtNseCode" ClientEnabled="true"
                                            runat="server" CssClass="StateTextbox">
                                        </dxe:ASPxTextBox>
                                    </div>
                                </div>--%>
                                <%--<div style="padding-top: 5px; display: none" class="col-md-4">
                                    <div class="stateDiv" style="padding-top: 5px;">
                                        BSE Code
                                    </div>
                                    <div style="padding-top: 5px;">
                                        <dxe:ASPxTextBox ID="txtBseCode" ClientInstanceName="ctxtBseCode" ClientEnabled="true"
                                            runat="server" CssClass="StateTextbox">
                                        </dxe:ASPxTextBox>
                                    </div>
                                </div>--%>
                                <%-- <br style="clear: both;" />
                                <div style="padding-top: 5px; display: none" class="col-md-4">
                                    <div class="stateDiv" style="padding-top: 5px;">
                                        MCX Code
                                    </div>
                                    <div style="padding-top: 5px;">
                                        <dxe:ASPxTextBox ID="txtMcxCode" ClientInstanceName="ctxtMcxCode" ClientEnabled="true"
                                            runat="server" CssClass="StateTextbox">
                                        </dxe:ASPxTextBox>
                                    </div>
                                </div>--%>
                                <%--<div style="padding-top: 5px; display: none" class="col-md-4">
                                    <div class="stateDiv" style="padding-top: 5px;">
                                        MCXSX Code
                                    </div>
                                    <div style="padding-top: 5px;">
                                        <dxe:ASPxTextBox ID="txtMcsxCode" ClientInstanceName="ctxtMcsxCode" ClientEnabled="true"
                                            runat="server" CssClass="StateTextbox">
                                        </dxe:ASPxTextBox>
                                    </div>
                                </div>--%>
                                <%--<div style="padding-top: 5px; display: none" class="col-md-4">
                                    <div class="stateDiv" style="padding-top: 5px;">
                                        NCDEX Code
                                    </div>
                                    <div style="padding-top: 5px;">
                                        <dxe:ASPxTextBox ID="txtNcdexCode" ClientInstanceName="ctxtNcdexCode" ClientEnabled="true"
                                            runat="server" CssClass="StateTextbox">
                                        </dxe:ASPxTextBox>
                                    </div>
                                </div>--%>
                                <%-- <br style="clear: both;" />
                                <div style="padding-top: 5px; display: none" class="col-md-4">
                                    <div class="stateDiv" style="padding-top: 5px;">
                                        CDSL Code
                                    </div>
                                    <div style="padding-top: 5px;">
                                        <dxe:ASPxTextBox ID="txtCdslCode" ClientInstanceName="ctxtCdslCode" ClientEnabled="true"
                                            runat="server" CssClass="StateTextbox">
                                        </dxe:ASPxTextBox>
                                    </div>
                                </div>
                                <div style="padding-top: 5px; display: none" class="col-md-4">
                                    <div class="stateDiv" style="padding-top: 5px;">
                                        NSDL Code
                                    </div>
                                    <div style="padding-top: 5px;">
                                        <dxe:ASPxTextBox ID="txtNsdlCode" ClientInstanceName="ctxtNsdlCode" ClientEnabled="true"
                                            runat="server" CssClass="StateTextbox">
                                        </dxe:ASPxTextBox>
                                    </div>
                                </div>
                                <div style="padding-top: 5px; display: none" class="col-md-4">
                                    <div class="stateDiv" style="padding-top: 5px;">
                                        NDML Code
                                    </div>
                                    <div style="padding-top: 5px;">
                                        <dxe:ASPxTextBox ID="txtNdmlCode" ClientInstanceName="ctxtNdmlCode" ClientEnabled="true"
                                            runat="server" CssClass="StateTextbox">
                                        </dxe:ASPxTextBox>
                                    </div>
                                </div>
                                <br style="clear: both;" />
                                <div style="padding-top: 5px; display: none" class="col-md-4">
                                    <div class="stateDiv" style="padding-top: 5px;">
                                        DOTEXID Code
                                    </div>
                                    <div style="padding-top: 5px;">
                                        <dxe:ASPxTextBox ID="txtDotexidCode" ClientInstanceName="ctxtDotexidCode" ClientEnabled="true"
                                            runat="server" CssClass="StateTextbox">
                                        </dxe:ASPxTextBox>
                                    </div>
                                </div>
                                <div style="padding-top: 5px; display: none" class="col-md-4">
                                    <div class="stateDiv" style="padding-top: 5px;">
                                        CVLID Code
                                    </div>
                                    <div style="padding-top: 5px;">
                                        <dxe:ASPxTextBox ID="txtCvlidCode" ClientInstanceName="ctxtCvlidCode" ClientEnabled="true"
                                            runat="server" CssClass="StateTextbox">
                                        </dxe:ASPxTextBox>
                                    </div>
                                </div>
                                <br style="clear: both;" />--%>
                            </div>
                            <div class="ContentDiv">
                                <div class="ScrollDiv"></div>
                                <br style="clear: both;" />
                                <div class="Footer" style="padding-left: 84px;">
                                    <div style="float: left;">
                                        <dxe:ASPxButton ID="btnSave_Country" ClientInstanceName="cbtnSave_States" runat="server"
                                            AutoPostBack="False" Text="Save" CssClass="btn btn-primary">
                                            <ClientSideEvents Click="function (s, e) {btnSave_Country();}" />
                                        </dxe:ASPxButton>
                                    </div>
                                    <div style="">
                                        <dxe:ASPxButton ID="btnCancel_Country" runat="server" AutoPostBack="False" Text="Cancel" CssClass="btn btn-danger">
                                            <ClientSideEvents Click="function (s, e) {fn_btnCancel();}" />
                                        </dxe:ASPxButton>
                                    </div>
                                    <br style="clear: both;" />
                                </div>
                                <br style="clear: both;" />
                            </div>
                        </dxe:PopupControlContentControl>
                    </ContentCollection>
                    <HeaderStyle BackColor="LightGray" ForeColor="Black" />
                </dxe:ASPxPopupControl>
                <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
            </div>

            <div class="HiddenFieldArea" style="display: none;">
                <dxe:ASPxHiddenField runat="server" ClientInstanceName="chfID" ID="hfID">
                </dxe:ASPxHiddenField>
            </div>
        </div>
    </div>
    </div>
    <script type="text/javascript">
        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }
        function fn_PopUpOpen() {
            $('#valid').attr('style', 'display:none;');
            chfID.Set("hfID", '');
            ctxtCountryName.SetText('');
            cPopupCountry.SetHeaderText('Add Country');
            //ctxtNseCode.SetText('');
            //ctxtBseCode.SetText('');
            //ctxtMcxCode.SetText('');
            //ctxtMcsxCode.SetText('');
            //ctxtNcdexCode.SetText('');
            //ctxtCdslCode.SetText('');
            //ctxtNsdlCode.SetText('');
            //ctxtNdmlCode.SetText('');
            //ctxtDotexidCode.SetText('');
            //ctxtCvlidCode.SetText('');
            cPopupCountry.Show();

        }
        function fn_EditCountry(keyValue) {
            grid.PerformCallback('Edit~' + keyValue);
        }
        function fn_DeleteCountry(keyValue) {
            //var result=confirm('Confirm delete?');
            //if(result)
            //{
            //    grid.PerformCallback('Delete~' + keyValue);
            //}
            jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    grid.PerformCallback('Delete~' + keyValue);
                }
                else {
                    return false;
                }
            });


        }
        function fn_btnCancel() {
            cPopupCountry.Hide();
        }
        function btnSave_Country() {
            var countrynm = ctxtCountryName.GetText();
            if (countrynm.trim() == '')
                //if (ctxtCountryName.GetText() == '')
            {
                $('#valid').attr('style', 'display:block;position: absolute;right: 32px;top: 17px;');
                // alert('Please Enter Country Name');
                ctxtCountryName.Focus();
            }
            else {
                var id = chfID.Get('hfID');
                if (id == '')

                    //grid.PerformCallback('savecountry~' + ctxtCountryName.GetText() + '~' + ctxtNseCode.GetText() + '~' + ctxtBseCode.GetText() + '~' + ctxtMcxCode.GetText() + '~' + ctxtMcsxCode.GetText() + '~' + ctxtNcdexCode.GetText() + '~' + ctxtCdslCode.GetText() + '~' + ctxtNsdlCode.GetText() + '~' + ctxtNdmlCode.GetText() + '~' + ctxtDotexidCode.GetText() + '~' + ctxtCvlidCode.GetText());
                    grid.PerformCallback('savecountry~' + ctxtCountryName.GetText());
                else
                    grid.PerformCallback('updatecountry~' + chfID.Get('hfID'));
            }
        }


        function grid_EndCallBack() {
            if (grid.cpEdit != null) {
                ctxtCountryName.SetText(grid.cpEdit.split('~')[0]);
                //ctxtNseCode.SetValue(grid.cpEdit.split('~')[2]);
                //ctxtBseCode.SetValue(grid.cpEdit.split('~')[3]);
                //ctxtMcxCode.SetValue(grid.cpEdit.split('~')[4]);
                //ctxtMcsxCode.SetValue(grid.cpEdit.split('~')[5]);
                //ctxtNcdexCode.SetValue(grid.cpEdit.split('~')[6]);
                //ctxtCdslCode.SetValue(grid.cpEdit.split('~')[7]);
                //ctxtNsdlCode.SetValue(grid.cpEdit.split('~')[8]);
                //ctxtNdmlCode.SetValue(grid.cpEdit.split('~')[9]);
                //ctxtDotexidCode.SetValue(grid.cpEdit.split('~')[10]);
                //ctxtCvlidCode.SetValue(grid.cpEdit.split('~')[11]);
                var hfid = grid.cpEdit.split('~')[1];
                cPopupCountry.SetHeaderText('Modify Country');
                chfID.Set("hfID", hfid);
                cPopupCountry.Show();
            }

            if (grid.cpinsert != null) {
                if (grid.cpinsert == 'Success') {
                    jAlert('Saved successfully');
                    cPopupCountry.Hide();
                }
                else {
                    jAlert("Error On Insertion\n'Please Try Again!!'");
                }
            }

            if (grid.cpExists != null) {
                if (grid.cpExists == 'Exists') {
                    jAlert('Duplicate value');
                    cPopupCountry.Hide();
                }

            }

            if (grid.cpUpdate != null) {
                if (grid.cpUpdate == 'Success') {
                    jAlert('Updated successfully');
                    grid.cpUpdate = null;
                    cPopupCountry.Hide();
                }
                else{
                    jAlert("Error on Updation\n'Please Try again!!'")
                    grid.cpUpdate = null;
                }
            }


            if (grid.cpDelete != null) {
                if (grid.cpDelete == 'Success') {
                    jAlert(grid.cpDelete);
                    grid.cpDelete = null;
                    grid.PerformCallback();
                }
                else {
                    jAlert(grid.cpDelete)
                    grid.cpDelete = null;
                    grid.PerformCallback();
                }
            }
        }
    </script>
</asp:Content>

