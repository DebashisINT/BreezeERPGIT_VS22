<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                30-03-2023        2.0.36           Pallab              25768: CRM pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="Followup.aspx.cs" Inherits="ERP.OMS.Management.Followup.Followup" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .padtable > tbody > tr > td {
            padding-right: 15px;
            padding-top: 15px;
        }

            .padtable > tbody > tr > td #chkShowAll {
                padding-left: 0px;
            }

        .gridHeader {
            background: #54749D;
        }

        .dxgvFocusedRow_PlasticBlue {
            background-color: #54749D !important;
            color: #efe6e6 !important;
        }

        .dxgvFocusedRow_PlasticBlue a {
            color: #efe6e6 !important;
        }
        .layer {
            position: fixed;
            width: 100%;
            height: 100%;
            background: rgba(0,0,0,0.5);
            z-index: 1072;
            left: 0;
            top: 0;
        }
        .DocumentDetails {
                position: fixed;
    top: 61px;
    right: 0;
    background: rgb(241, 241, 241);
    padding: 26px 25px;
    border: 1px solid #948a068c;
    z-index:1200;
        }
       .DocumentDetails .toggler {
                   position: absolute;
    background: #83f1ff;
    display: inline-block;
    left: -15px;
    width: 15px;
    height: 40px; 
    text-align: center;
    line-height: 39px;
       }
       .tablStyl{
           border: 1px solid #dedcdc;
           background: #ffffffad;
       }
       .tablStyl>thead>tr>th{
              background: #f59d9deb; 
       }
       .tablStyl>thead>tr>th,
       .tablStyl>tbody>tr>td {
           padding: 5px 10px;
           
       }

        .DetailProductRowFont {
          color: #FFFFFF;
        }
        .dateCell {
        font-weight: 700;
        color: maroon;
        }

        .helpspan {
        padding-top: 24px;
    display: inline-block;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                cGrid.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                cGrid.SetWidth(cntWidth);
            }

            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    cGrid.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    cGrid.SetWidth(cntWidth);
                }

            });
        });
    </script>

    <style>
        /*Rev 1.0*/

        select
        {
            height: 30px !important;
            border-radius: 4px !important;
            /*-webkit-appearance: none;
            position: relative;
            z-index: 1;*/
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

        .dxeDisabled_PlasticBlue
        {
            z-index: 0 !important;
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
        select.btn
        {
            padding-right: 10px !important;
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
        .TableMain100 #SalesDetailsGrid, #ShowGrid
        {
            max-width: 99% !important;
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

        .col-md-3 , .col-md-2
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

        .dxpc-content table
        {
             width: 100%;
        }

        input[type="text"], input[type="password"], textarea
        {
            margin-bottom: 0 !important;
        }
        #FromDate , #ToDate , #ASPxFromDate , #ASPxToDate , #FormDate , #toDate
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        #FromDate_B-1 , #ToDate_B-1 , #ASPxFromDate_B-1 , #ASPxToDate_B-1 , #FormDate_B-1 , #toDate_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #FromDate_B-1 #FromDate_B-1Img , #ToDate_B-1 #ToDate_B-1Img , #ASPxFromDate_B-1 #ASPxFromDate_B-1Img , #ASPxToDate_B-1 #ASPxToDate_B-1Img ,
        #FormDate_B-1 #FormDate_B-1Img , #toDate_B-1 #toDate_B-1Img
        {
            display: none;
        }

        #lblToDate
        {
            padding-left: 10px;
        }

        .dxtc-activeTab:after {
            content: '';
            width: 0;
            height: 0;
            border-left: 8px solid transparent;
            border-right: 8px solid transparent;
            border-top: 9px solid #3e5395;
            position: absolute;
            /* left: 50%; */
            z-index: 3;
            /* bottom: -15px; */
            margin-left: -9px;
        }

        table td
        {
            padding-bottom: 10px !important;
        }

        #cmbContactType , #txtUserId
        {
            margin-bottom: 10px !important;
        }

        .calendar-icon
        {
                right: 19px;
        }
        /*Rev end 1.0*/
    </style>
</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script src="../Activities/JS/SearchPopup.js?v=0.04"></script>


    <script src="Js/Followup.js?v=0.05"></script>
    <link href="../Activities/CSS/SearchPopup.css" rel="stylesheet" />
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main">
        <div class="panel-heading">
        <div class="panel-title clearfix">
            <h3 class="pull-left">Payment Follow-up</h3>
        </div>
    </div>
        <div class="form_main">
        <div class="row">
            <div class="col-md-2">
                <label>Main Unit</label>
                <dxe:ASPxComboBox ID="cmbMainUnit" runat="server" ClientInstanceName="ccmbMainUnit" Width="100%" ClientSideEvents-SelectedIndexChanged="cmbMainBranchChange">
                </dxe:ASPxComboBox>
            </div>
            <div class="col-md-2">
                <label>Sub Unit</label>
                <dxe:ASPxComboBox ID="cmbSubunit" runat="server" ClientInstanceName="ccmbSubunit" EnableCallbackMode="false" Width="100%">
                </dxe:ASPxComboBox>
            </div>
            <div class="col-md-2 for-cust-icon">
                <label>From Date</label>
                <dxe:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" AllowNull="false"
                    ClientInstanceName="cFormDate" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                    <ButtonStyle Width="13px">
                    </ButtonStyle>
                </dxe:ASPxDateEdit>
                <%--Rev 1.0--%>
                <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                <%--Rev end 1.0--%>
            </div>
            <div class="col-md-2 for-cust-icon">
                <label>To Date</label>
                <dxe:ASPxDateEdit ID="toDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" AllowNull="false"
                    ClientInstanceName="ctoDate" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                    <ButtonStyle Width="13px">
                    </ButtonStyle>
                </dxe:ASPxDateEdit>
                <%--Rev 1.0--%>
                <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                <%--Rev end 1.0--%>
            </div>

            <div class="col-md-2">
                <label>Customer/Vendor</label>
                <dxe:ASPxButtonEdit ID="txtCustName" runat="server" ReadOnly="true" ClientInstanceName="ctxtCustName" Width="100%">
                    <Buttons>
                        <dxe:EditButton>
                        </dxe:EditButton>

                    </Buttons>
                    <ClientSideEvents ButtonClick="function(s,e){CustomerButnClick();}" KeyDown="function(s,e){CustomerKeyDown(s,e);}" />
                </dxe:ASPxButtonEdit>
            </div>
            <div class="col-md-2 hide">
                 <span data-toggle="tooltip" title="Help" class="helpspan">
                            <span data-toggle="popover" data-placement="bottom" data-html="true" data-content="This module shows the listing of documents for which amount is due. It considers Opening and period Sales Invoice, Transit Sales Invoice, Sale Invoice for Vendor, Debit Note.<br />If we give From and To date, it finds the actual outstanding which considers post dated entries also, for the filtered document within the given date range, so that wrong  follow-up to be stopped and unwanted call to be reduced. This in-turn increase Customer satisfaction also by avoiding unwanted calls.<br /> <br /><strong>Main Unit -</strong> Parent Branch based on which Child branch to be populated if the logged in user is attached with the respective branch.<br/Sub Unit - Child Branch of the selected Parent Branch based to be populated if the logged in user is attached with the respective branch.<br/><strong>From Date -</strong> Enter the from date based on which Sales Invoice(Due Date), Transit Sales Invoice(Due Date), Sale Invoice for Vendor(Due Date), Debit Note(Document Date) to be filtered.All the opening data before from date to be considered in this list.<br/><strong>To Date -</strong> Enter the date upto which Sales Invoice(Due Date), Transit Sales Invoice(Due Date), Debit Note(Document Date) to be filtered.<br /> <strong>Customer/Vendor -</strong> We can select one or multiple Party for which we wish to see the listing of outstanding. Vendor will fetch data from Sale Invoice Vendor.<br/><strong>Show All Customer/Vendor -</strong> Tick this option to see documents of all Party for which we wish to see the listing of outstanding.<br/> <strong>Show Zero Balance :</strong> Tick this option to include data of such party and documents for which outstanding balance is ZERO(0).>"><i class="fa fa-question-circle"></i></span>
                        </span>
            </div>

            <div class="col-md-3">
            </div>
            <div class="col-md-3">
            </div>
        </div>
        <div>
            <table class="padtable">
                <tr>
                    <td>
                        <dxe:ASPxCheckBox ID="chkShowAll" runat="server" ClientInstanceName="cchkShowAll">
                            <ClientSideEvents CheckedChanged="showAllChkChange" />
                        </dxe:ASPxCheckBox>
                        <label>Show All Customer/Vendor</label>
                    </td>
                    <td>
                        <dxe:ASPxCheckBox ID="chkzeroBal" runat="server" ClientInstanceName="cchkzeroBal"></dxe:ASPxCheckBox>
                        <label for="chkzeroBal">Show Zero Balance</label></td>
                    <td>
                        <input type="button" value="Show" class="btn btn-success" onclick="GenerateFollowup()" />

                    </td>
                    <td>
                        <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary pull-right " OnSelectedIndexChanged="drdExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                            <asp:ListItem Value="0">Export to</asp:ListItem>
                            <asp:ListItem Value="1">PDF</asp:ListItem>
                            <asp:ListItem Value="2">XLSX</asp:ListItem>
                            <asp:ListItem Value="3">RTF</asp:ListItem>
                            <asp:ListItem Value="4">CSV</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </div>

    </div>
    
    <dxe:ASPxGridViewExporter ID="exporter" GridViewID="GridFollowup" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>
    <div onkeypress="OnWaitingGridKeyPress(event)">
        <dxe:ASPxGridView ID="GridFollowup" runat="server" ClientInstanceName="cGrid" KeyFieldName="Slno"
            Width="100%" Settings-HorizontalScrollBarMode="Auto" OnCustomCallback="GridFullYear_CustomCallback"
            SettingsBehavior-ColumnResizeMode="Control" DataSourceID="EntityServerModeDataSource"
            Settings-VerticalScrollableHeight="300" SettingsBehavior-AllowSelectByRowClick="true"
            Settings-VerticalScrollBarMode="Auto" Settings-ShowGroupPanel="true" KeyboardSupport="true"
            Settings-ShowGroupFooter="VisibleAlways" SettingsBehavior-AllowFocusedRow="true"
            Settings-ShowFilterRow="true" Settings-ShowFilterRowMenu="true">
            <Columns>


                   <dxe:GridViewDataDateColumn HeaderStyle-CssClass="gridHeader" Caption="Next Follow-up Date" FieldName="NextFollowupDate" Width="25%"
                    PropertiesDateEdit-DisplayFormatString="dd-MM-yyyy" PropertiesDateEdit-EditFormatString="dd-MM-yyyy" FixedStyle="Left" CellStyle-CssClass="dateCell">
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Equals" /> 
                </dxe:GridViewDataDateColumn>


                <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Customer" Width="35%" FieldName="custName">
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                    <DataItemTemplate>
                        <a href="javascript:void(0);" title="Click here to view Customer." onclick="View_Customer('<%# Eval("CustId") %>');"><%#Eval("custName")%> 
                        </a>
                    </DataItemTemplate>
                </dxe:GridViewDataTextColumn>

             

                  <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="CustId" Width="0" FieldName="CustId">
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Equals" />
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Total Amount" Width="10%" FieldName="TotalAmount">
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Equals" />
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Adjusted Amount" Width="10%" FieldName="AdjsutedAmount">
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Equals" />
                </dxe:GridViewDataTextColumn>


                <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Outstanding Amount" Width="10%" FieldName="unpaidAmount">
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Equals" />
                </dxe:GridViewDataTextColumn>


                <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" Width="10%">
                    <DataItemTemplate>

                        <%--   <% if (rights.CanHistory)
                           { %>
                        <a href="javascript:void(0);" onclick="OnView('<%# Eval("CustId") %>')" id="a_editInvoice" class="pad" title="Details">
                            <img src="../../../assests/images/viewIcon.png" /></a>
                        <%} %>--%>

                        <% if (rights.CanAdd)
                           { %>
                        <a href="javascript:void(0);" onclick="OnFolloupAdd('<%# Eval("CustId") %>')" class="pad" title="Follow-up">
                            <img src="../../../assests/images/notification1.png" /></a>
                        <%} %>


                         <a href="javascript:void(0);" onclick="loadDrawer('<%# Container.VisibleIndex %>')" class="pad" title="Follow-up History">
                            <img src="../../../assests/images/viewIcon.png" /></a>
                    </DataItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" CssClass="gridHeader"></HeaderStyle>
                    <CellStyle HorizontalAlign="Center"></CellStyle>
                    <HeaderTemplate><span>Actions</span></HeaderTemplate>
                    <EditFormSettings Visible="False"></EditFormSettings>
                    <Settings AllowAutoFilterTextInputTimer="False" />

                </dxe:GridViewDataTextColumn>

            </Columns>

            <SettingsContextMenu Enabled="true"></SettingsContextMenu>
            <SettingsPager NumericButtonCount="10" PageSize="10" ShowSeparators="True" Mode="ShowPager">
                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                <FirstPageButton Visible="True">
                </FirstPageButton>
                <LastPageButton Visible="True">
                </LastPageButton>
            </SettingsPager>
            <ClientSideEvents RowDblClick="FocusedRowDoubleClick" />
        </dxe:ASPxGridView>
    </div>

    </div>

    <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
        <ClientSideEvents ControlsInitialized="AllControlInitilize" />
    </dxe:ASPxGlobalEvents>

    <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
        ContextTypeName="ERPDataClassesDataContext" TableName="V_custOutstandingDetailss" />



    <asp:HiddenField ID="hdBranchList" runat="server" />
    <asp:HiddenField ID="hdnCustomerId" runat="server" />

    <!--Customer Modal -->
    <div class="modal fade" id="CustModel" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Customer Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Customerkeydown(event)" id="txtCustSearch" autocomplete="off" autofocus width="100%" placeholder="Search By Customer Name or Unique Id" />

                    <div id="CustomerTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Customer Name</th>
                                <th>Unique Id</th>
                                <th>Address</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>

        </div>
    </div>

    <dxe:ASPxPopupControl ID="DetailPopup" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="cDetailPopup" Height="433px"
        Width="1020px" HeaderText="Outstanding Details" Modal="true" AllowResize="true" ResizingMode="Postponed"
        CloseOnEscape="true" PopupAnimationType="Fade" CloseAnimationType="Fade">
        <ClientSideEvents Closing="popupClosing" />

        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>



    <div style="display: none">
        <dxe:ASPxComboBox ID="errorDD" runat="server" ClientInstanceName="cerrorDD">
        </dxe:ASPxComboBox>
    </div>


    <div  class="DocumentDetails" style="display:none" id="MainDivDetails">
        <span class="toggler" onclick="SlideChange()" style="cursor: pointer;"> <i class="fa fa-angle-right"></i> </span>
        <div id="DocumentDetails" style="right:-50px">
        </div>

          <div id="divProductDetails" style="padding-top:10px" > 
              
        </div>

    </div>




</asp:Content>
