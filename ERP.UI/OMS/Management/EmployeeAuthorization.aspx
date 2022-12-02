<%@ Page Title="Authorize Requisition" Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.management_EmployeeAuthorisation" CodeBehind="EmployeeAuthorization.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!--External Styles-->
    <link type="text/css" href="/assests/custom/CSS/GenericCss.css" rel="Stylesheet" />
    <!--External Scripts file-->
    <!-- Ajax List Requierd-->
    <link type="text/css" href="/assests/custom/CSS/AjaxStyle.css" rel="Stylesheet" />
    <script type="text/javascript" src="/assests/js/ajaxList_wofolder.js"></script>
    <script type="text/javascript" src="/assests/js/init.js"></script>
    <!--Other Script-->
    
    <script type="text/javascript" src="/assests/js/GenericJScript.js"></script>
    <!--JS Inline Method-->

    <style type="text/css">
        .divHeight {
            height: 26px;
        }

        .btnRight {
            margin-right: 10px;
            float: right;
        }

        .paddingContnt {
            padding-bottom: 4px;
            padding-top: 4px;
        }
        .LFloat_Content {
            border:none !important;
        }
    </style>

    <script type="text/javascript" language="javascript">
        //Global Variable
        FieldName = '';

        //End
        var _selectNumber = 0;
        var _handle = true;

        function PageLoad() {
            cDtFrom.Focus();
            HideShow("BtnUnAuthoriseAll", "H");
            HideShow("BtnUnAuthorise", "H");
            HideShow("BtnAuthoriseAll", "S");
            HideShow("BtnAuthorise", "S");

        }
        function DateCompare(DateobjFrm, DateobjTo) {
            var Msg = "To Date Can Not Be Less Than From Date!!!";
            DevE_CompareDateForMin(DateobjFrm, DateobjTo, Msg);
        }

        function OnLeftNav_Click() {
            SetValue("hdn_GridBindOrNotBind", "False");
            var i = GetObjectID("A1").innerText;
            GetObjectID('A1').className = "number_box_selected";
            //SetValue("hdn_cGvEmployeeDtl","False"); //To Stop Bind On Page Load
            if (parseInt(i) > 1) {
                SetValue("HDNCheckedEmpCode", "");
                cGvEmployeeDtl.PerformCallback("SearchByNavigation~" + cComboAuthorize.GetValue() + '~' + cDtFrom.GetText() + '~' + cDtTo.GetText() + '~' + cComboAuthType.GetValue() + "~" + GetObjectID("A1").innerText + "~LeftNav");
            }
            else {
                alert('No More Pages.');
            }
        }

        function OnRightNav_Click() {
            SetValue("hdn_GridBindOrNotBind", "False");
            var TestEnd = GetObjectID("A10").innerText;
            //SetValue("hdn_cGvEmployeeDtl","False"); //To Stop Bind On Page Load
            var TotalPage = GetObjectID("B_TotalPage").innerText;
            if (TestEnd == "" || TestEnd == TotalPage) {
                alert('No More Records.');
                return;
            }
            var i = GetObjectID("A1").innerText;

            if (parseInt(i) < TotalPage) {
                SetValue("HDNCheckedEmpCode", "");
                cGvEmployeeDtl.PerformCallback("SearchByNavigation~" + cComboAuthorize.GetValue() + '~' + cDtFrom.GetText() + '~' + cDtTo.GetText() + '~' + cComboAuthType.GetValue() + "~" + GetObjectID("A1").innerText + "~RightNav");
            }
            else {
                alert('You are at the End');
            }
        }

        function OnPageNo_Click(obj) {
            SetValue("hdn_GridBindOrNotBind", "False");
            var i = GetObjectID(obj).innerText;
            SetValue("HDNCheckedEmpCode", "");
            // SetValue("hdn_cGvEmployeeDtl","False"); //To Stop Bind On Page Load
            cGvEmployeeDtl.PerformCallback("SearchByNavigation~" + cComboAuthorize.GetValue() + '~' + cDtFrom.GetText() + '~' + cDtTo.GetText() + '~' + cComboAuthType.GetValue() + "~" + i + "~PageNav");
        }


        function GvEmployeeDtl_EndCallBack() {


            if (cGvEmployeeDtl.cpAuthorizeSlctd == "T") {
                alert('Selected Employees Authorized Successfully');
                SetValue("HDNCheckedEmpCode", "");
                cGvEmployeeDtl.PerformCallback('GridBind~' + cComboAuthorize.GetValue() + '~' + cDtFrom.GetText() + '~' + cDtTo.GetText() + '~' + cComboAuthType.GetValue());
                cGvEmployeeDtl.cpAuthorizeSlctd = null;
            }
            if (cGvEmployeeDtl.cpAuthorizeAll == "T") {
                alert('All Employees Authorized Successfully');
                SetValue("HDNCheckedEmpCode", "");
                cGvEmployeeDtl.PerformCallback('GridBind~' + cComboAuthorize.GetValue() + '~' + cDtFrom.GetText() + '~' + cDtTo.GetText() + '~' + cComboAuthType.GetValue());
                cGvEmployeeDtl.cpAuthorizeAll = null;
            }
            if (cGvEmployeeDtl.cpUnAuthorizeSlctd == "T") {
                alert('Selected Employees UnAuthorized Successfully');
                SetValue("HDNCheckedEmpCode", "");
                cGvEmployeeDtl.PerformCallback('GridBind~' + cComboAuthorize.GetValue() + '~' + cDtFrom.GetText() + '~' + cDtTo.GetText() + '~' + cComboAuthType.GetValue());
                cGvEmployeeDtl.cpUnAuthorizeSlctd = null;
            }
            if (cGvEmployeeDtl.cpUnAuthorizeAll == "T") {
                alert('All Employees UnAuthorized Successfully');
                SetValue("HDNCheckedEmpCode", "");
                cGvEmployeeDtl.PerformCallback('GridBind~' + cComboAuthorize.GetValue() + '~' + cDtFrom.GetText() + '~' + cDtTo.GetText() + '~' + cComboAuthType.GetValue());
                cGvEmployeeDtl.cpUnAuthorizeAll = null;
            }
            if (cGvEmployeeDtl.cpNoSelection == "T") {
                alert('Please Select Any Row First ');
                cGvEmployeeDtl.cpNoSelection = null;
            }
            if (cGvEmployeeDtl.cpExcelExport == "T") {
                document.getElementById('BtnForExportEvent').click();
                cGvEmployeeDtl.cpExcelExport = null;
            }
            //       if(cGvEmployeeDtl.cpSelectedRowsOnPage!=undefined)
            //         {
            //             _selectNumber = cGvEmployeeDtl.cpSelectedRowsOnPage;
            //         }
            if (cGvEmployeeDtl.cpRefreshNavPanel != undefined) {
                GetObjectID("B_PageNo").innerText = '';
                GetObjectID("B_TotalPage").innerText = '';
                GetObjectID("B_TotalRows").innerText = '';

                var NavDirection = cGvEmployeeDtl.cpRefreshNavPanel.split('~')[0];
                var PageNum = cGvEmployeeDtl.cpRefreshNavPanel.split('~')[1];
                var TotalPage = cGvEmployeeDtl.cpRefreshNavPanel.split('~')[2];
                var TotalRows = cGvEmployeeDtl.cpRefreshNavPanel.split('~')[3];

                if (NavDirection == "RightNav") {
                    PageNum = parseInt(PageNum) + 10;
                    GetObjectID("B_PageNo").innerText = PageNum;
                    GetObjectID("B_TotalPage").innerText = TotalPage;
                    GetObjectID("B_TotalRows").innerText = TotalRows;
                    var n = parseInt(TotalPage) - parseInt(PageNum) > 10 ? parseInt(11) : parseInt(TotalPage) - parseInt(PageNum) + 2;
                    for (r = 1; r < n; r++) {
                        var obj = "A" + r;
                        GetObjectID(obj).innerText = " " + PageNum++ + " ";
                    }
                    for (r = n; r < 11; r++) {
                        var obj = "A" + r;
                        GetObjectID(obj).innerText = "";
                    }
                }
                if (NavDirection == "LeftNav") {
                    if (parseInt(PageNum) > 1) {
                        PageNum = parseInt(PageNum) - 10;
                        GetObjectID("B_PageNo").innerText = PageNum;
                        GetObjectID("B_TotalPage").innerText = TotalPage;
                        GetObjectID("B_TotalRows").innerText = TotalRows;
                        for (l = 1; l < 11; l++) {
                            var obj = "A" + l;
                            GetObjectID(obj).innerText = " " + PageNum++ + " ";
                        }
                    }
                    else {
                        alert('No More Pages.');
                    }
                }
                if (NavDirection == "PageNav") {
                    GetObjectID("B_PageNo").innerText = PageNum;
                    GetObjectID("B_TotalPage").innerText = TotalPage;
                    GetObjectID("B_TotalRows").innerText = TotalRows;
                }
                if (NavDirection == "ShowBtnClick") {
                    GetObjectID("B_PageNo").innerText = PageNum;
                    GetObjectID("B_TotalPage").innerText = TotalPage;
                    GetObjectID("B_TotalRows").innerText = TotalRows;
                    var n = parseInt(TotalPage) - parseInt(PageNum) > 10 ? parseInt(11) : parseInt(TotalPage) - parseInt(PageNum) + 2;
                    for (r = 1; r < n; r++) {
                        var obj = "A" + r;
                        GetObjectID(obj).innerText = " " + PageNum++ + " ";
                    }
                    for (r = n; r < 11; r++) {
                        var obj = "A" + r;
                        GetObjectID(obj).innerText = "";
                    }
                }
            }
            if (cGvEmployeeDtl.cpSetGlobalFields != undefined) {
                SetValue("Hdn_PageSize", cGvEmployeeDtl.cpSetGlobalFields.split('~')[0]);
                SetValue("Hdn_PageNumber", cGvEmployeeDtl.cpSetGlobalFields.split('~')[1]);
                SetValue("Hdn_DateFrom", cGvEmployeeDtl.cpSetGlobalFields.split('~')[2]);
                SetValue("Hdn_DateTo", cGvEmployeeDtl.cpSetGlobalFields.split('~')[3]);
                SetValue("Hdn_AuthorizeType", cGvEmployeeDtl.cpSetGlobalFields.split('~')[4]);
                SetValue("Hdn_AuthorizeFor", cGvEmployeeDtl.cpSetGlobalFields.split('~')[5]);

            }
            SetValue("hdn_GridBindOrNotBind", "True");
            Height('650', '650');
        }

        function OnAllCheckedChanged(s, e) {
            if (s.GetChecked())
                cGvEmployeeDtl.SelectRows();
            else
                cGvEmployeeDtl.UnselectRows();
        }
        function OnGridSelectionChanged(s, e) {
            cGvEmployeeDtl.GetSelectedFieldValues('ContactID', OnGetRowValues);
            //        cbAll.SetChecked(s.GetSelectedRowCount() == s.cpVisibleRowCount);     
            //         if (e.isChangedOnServer == false) 
            //            {
            //                if (e.isAllRecordsOnPage && e.isSelected)
            //                    _selectNumber = s.GetVisibleRowsOnPage();
            //                else if (e.isAllRecordsOnPage && !e.isSelected)
            //                    _selectNumber = 0;
            //                else if (!e.isAllRecordsOnPage && e.isSelected)
            //                    _selectNumber++;
            //                else if (!e.isAllRecordsOnPage && !e.isSelected)
            //                    _selectNumber--;
            //                  
            //                _handle = true;
            //            }       
        }

        function OnGetRowValues(values) {
            SetValue("HDNCheckedEmpCode", values);
            //alert(GetValue("HDNCheckedEmpCode"));              
        }
        function OnAuthorizationTypeChange() {
            SetValue("HDNShowBy", "");
            SetValue("HDNCheckedEmpCode", "");
            SetValue("hdn_GridBindOrNotBind", "False");

            if (cComboAuthorize.GetValue() == "Authorized") {
                HideShow("BtnUnAuthoriseAll", "S");
                HideShow("BtnUnAuthorise", "S");
                HideShow("BtnAuthoriseAll", "H");
                HideShow("BtnAuthorise", "H");

            }
            if (cComboAuthorize.GetValue() == "UnAuthorized") {
                HideShow("BtnUnAuthoriseAll", "H");
                HideShow("BtnUnAuthorise", "H");
                HideShow("BtnAuthoriseAll", "S");
                HideShow("BtnAuthorise", "S");
            }
            if (cComboAuthorize.GetValue() == "All") {
                HideShow("BtnUnAuthoriseAll", "S");
                HideShow("BtnUnAuthorise", "S");
                HideShow("BtnAuthoriseAll", "S");
                HideShow("BtnAuthorise", "S");
            }
            cGvEmployeeDtl.PerformCallback('GridBind~' + cComboAuthorize.GetValue() + '~' + cDtFrom.GetText() + '~' + cDtTo.GetText() + '~' + cComboAuthType.GetValue());
        }

        function btnAuthorizeSlctd_Click() {
            SetValue("hdn_GridBindOrNotBind", "False");
            cGvEmployeeDtl.PerformCallback('AuthorizeGridBind~' + GetValue("HDNCheckedEmpCode") + '~' + cDtFrom.GetText() + '~' + cDtTo.GetText() + '~' + cComboAuthType.GetValue());
        }

        function btnAuthorizeAll_Click() {
            SetValue("hdn_GridBindOrNotBind", "False");
            SetValue("HDNCheckedEmpCode", "All");
            cGvEmployeeDtl.PerformCallback('AuthorizeAllGridBind~' + GetValue("HDNCheckedEmpCode") + '~' + cDtFrom.GetText() + '~' + cDtTo.GetText() + '~' + cComboAuthType.GetValue());
        }

        function btnUnAuthorizeSlctd_Click() {
            SetValue("hdn_GridBindOrNotBind", "False");
            cGvEmployeeDtl.PerformCallback('UnAuthorizeGridBind~' + GetValue("HDNCheckedEmpCode") + '~' + cDtFrom.GetText() + '~' + cDtTo.GetText() + '~' + cComboAuthType.GetValue());
        }

        function btnUnAuthorizeAll_Click() {
            SetValue("hdn_GridBindOrNotBind", "False");
            SetValue("HDNCheckedEmpCode", "All");
            cGvEmployeeDtl.PerformCallback('UnAuthorizeAllGridBind~' + GetValue("HDNCheckedEmpCode") + '~' + cDtFrom.GetText() + '~' + cDtTo.GetText() + '~' + cComboAuthType.GetValue());
        }

        function btnShow_Click() {
            SetValue("HDNShowBy", "");
            SetValue("HDNCheckedEmpCode", "");
            SetValue("hdn_GridBindOrNotBind", "False");
            if (cComboAuthorize.GetValue() == "Authorized") {
                HideShow("BtnUnAuthoriseAll", "S");
                HideShow("BtnUnAuthorise", "S");
                HideShow("BtnAuthoriseAll", "H");
                HideShow("BtnAuthorise", "H");

            }
            if (cComboAuthorize.GetValue() == "UnAuthorized") {
                HideShow("BtnUnAuthoriseAll", "H");
                HideShow("BtnUnAuthorise", "H");
                HideShow("BtnAuthoriseAll", "S");
                HideShow("BtnAuthorise", "S");
            }
            if (cComboAuthorize.GetValue() == "All") {
                HideShow("BtnUnAuthoriseAll", "S");
                HideShow("BtnUnAuthorise", "S");
                HideShow("BtnAuthoriseAll", "S");
                HideShow("BtnAuthorise", "S");
            }
            cGvEmployeeDtl.PerformCallback('GridBind~' + cComboAuthorize.GetValue() + '~' + cDtFrom.GetText() + '~' + cDtTo.GetText() + '~' + cComboAuthType.GetValue());
        }

        function OnCmbExcelExportChanged() {

            if (cCmbExcelExport.GetText() == "Export") {
                SetValue("HDNShowBy", "Export");
            }
            if (cCmbExcelExport.GetText() == "Excel") {
                SetValue("hdn_GridBindOrNotBind", "False");
                SetValue("HDNShowBy", "Excel");
                cCmbExcelExport.SetText("Export");
                SetValue("HDNCheckedEmpCode", "");
                cGvEmployeeDtl.PerformCallback('ExcelReport~' + cComboAuthorize.GetValue() + '~' + cDtFrom.GetText() + '~' + cDtTo.GetText() + '~' + cComboAuthType.GetValue());
            }
        }

        function OnTypeChange() {
            SetValue("hdn_GridBindOrNotBind", "False");
            SetValue("HDNCheckedEmpCode", "");
            cGvEmployeeDtl.PerformCallback('GridBind~' + cComboAuthorize.GetValue() + '~' + cDtFrom.GetText() + '~' + cDtTo.GetText() + '~' + cComboAuthType.GetValue());
        }
        function ShowHideFilter(obj) {
            SetValue("HDNCheckedEmpCode", "");
            cGvEmployeeDtl.PerformCallback('GridBindFilter~' + cComboAuthorize.GetValue() + '~' + cDtFrom.GetText() + '~' + cDtTo.GetText() + '~' + cComboAuthType.GetValue() + '~' + obj);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Authorize Requisition</h3>
        </div>
    </div>
    <div class="form_main">
        <div id="Gmain" style="width:100%;">
            <div id="Div_header" class="Header">
                &nbsp;Authorize Requisition For Appt. Letter/I-Card/V-Card&nbsp;
            </div>
            <div id="Container1" class="" style="width: 100%">
                <div id="Row1" class="row">
                    <div class="col-md-3">
                        <div id="Row1_Column1">
                            <dxe:ASPxDateEdit ID="DtFrom" runat="server" ClientInstanceName="cDtFrom"
                                DateOnError="Today" EditFormat="Custom" EditFormatString="dd-MM-yyyy" UseMaskBehavior="True"
                                Width="100%">
                                <ButtonStyle Width="3px">
                                </ButtonStyle>
                                <DropDownButton Text="Req.DateFrom">
                                </DropDownButton>
                                <ClientSideEvents DateChanged="function(s,e){SetTradeDate();}" />
                            </dxe:ASPxDateEdit>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div id="Row1_Column2">
                            <dxe:ASPxDateEdit ID="DtTo" runat="server" ClientInstanceName="cDtTo"
                                DateOnError="Today" EditFormat="Custom" EditFormatString="dd-MM-yyyy" UseMaskBehavior="True"
                                Width="100%">
                                <ButtonStyle Width="3px">
                                </ButtonStyle>
                                <DropDownButton Text="Req.DateTo">
                                </DropDownButton>
                                <ClientSideEvents DateChanged="function(s,e){DateCompare(cDtFrom,cDtTo);}" />
                            </dxe:ASPxDateEdit>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div id="Row1_Column4">
                            <dxe:ASPxComboBox ID="ComboAuthorize" runat="server" ClientInstanceName="cComboAuthorize"
                                EnableIncrementalFiltering="True" Font-Size="Small" SelectedIndex="0" ValueType="System.String"
                                Width="100%">
                                <DropDownButton Text="FilterBy">
                                </DropDownButton>
                                <Items>
                                    <dxe:ListEditItem Text="Authorization Pending" Value="UnAuthorized" />
                                    <dxe:ListEditItem Text="Authorized" Value="Authorized" />
                                    <dxe:ListEditItem Text="All" Value="All" />
                                </Items>
                                <ClientSideEvents SelectedIndexChanged="function(s,e){OnAuthorizationTypeChange()}" />
                            </dxe:ASPxComboBox>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div id="Row1_Column5">
                            <dxe:ASPxComboBox ID="ComboAuthType" runat="server" ClientInstanceName="cComboAuthType"
                                EnableIncrementalFiltering="True" Font-Size="Small" SelectedIndex="0" ValueType="System.String"
                                Width="100%">
                                <DropDownButton Text="Type">
                                </DropDownButton>
                                <Items>
                                    <dxe:ListEditItem Text="Appointment Letter" Value="A" />
                                    <dxe:ListEditItem Text="Identity Card" Value="I" />
                                    <dxe:ListEditItem Text="Visiting Card" Value="V" />
                                </Items>
                                <ClientSideEvents SelectedIndexChanged="function(s,e){OnTypeChange()}" />
                            </dxe:ASPxComboBox>
                        </div>
                    </div>
                    <div class="clear"></div>
                    
                    
                    
                    
                    <div id="Row1_Column3" class="col-md-12">
                        <dxe:ASPxButton ID="btnShow" runat="server" AutoPostBack="False" ClientInstanceName="cbtnShow"
                            Text="Show" CssClass="btn btn-primary">
                            <ClientSideEvents Click="function(s,e){btnShow_Click();}" />
                        </dxe:ASPxButton>
                    </div>
                </div>
            </div>
            <div id="lblGridHeader" class="paging textLeft">
                <span class="pagingContent">Showing Record(s) <span id="spnReqType"></span></span>
            </div>
            <div id="Container2" class="paging textLeft clearfix">
                <div class="right">
                    <dxe:ASPxComboBox ID="CmbExcelExport" runat="server" ClientInstanceName="cCmbExcelExport"
                        CssClass="btnRight" Font-Size="8" SelectedIndex="0" TabIndex="0" ValueType="System.String"
                        Width="100px">
                        <Items>
                            <dxe:ListEditItem Text="Export" Value="Ex" />
                            <dxe:ListEditItem Text="Excel" Value="1" />
                        </Items>
                        <ClientSideEvents SelectedIndexChanged="OnCmbExcelExportChanged" />
                        <Paddings Padding="0px" />
                    </dxe:ASPxComboBox>
                    <span style="color: #555555; background-color: #ededed"></span>
                    <dxe:ASPxButton ID="BtnAuthoriseAll" runat="server" AutoPostBack="False" ClientInstanceName="cBtnAuthoriseAll"
                        CssClass="btnRight" Font-Size="8" TabIndex="0"
                        Text="Authorise All">
                        <Paddings Padding="0px" />
                        <ClientSideEvents Click="function(s,e){btnAuthorizeAll_Click();}" />
                    </dxe:ASPxButton>
                    <span style="color: #555555; background-color: #ededed"></span>
                    <dxe:ASPxButton ID="BtnAuthorise" runat="server" AutoPostBack="False" ClientInstanceName="cBtnAuthorise"
                        CssClass="btnRight" Font-Size="8" TabIndex="0"
                        Text="Authorise Selected">
                        <Paddings Padding="0px" />
                        <ClientSideEvents Click="function(s,e){btnAuthorizeSlctd_Click();}" />
                    </dxe:ASPxButton>
                    <dxe:ASPxButton ID="BtnUnAuthoriseAll" runat="server" AutoPostBack="False" ClientInstanceName="cBtnUnAuthoriseAll"
                        CssClass="btnRight" Font-Size="8" TabIndex="0"
                        Text="UnAuthorise All">
                        <Paddings Padding="0px" />
                        <ClientSideEvents Click="function(s,e){btnUnAuthorizeAll_Click();}" />
                    </dxe:ASPxButton>
                    <dxe:ASPxButton ID="BtnUnAuthorise" runat="server" AutoPostBack="False" ClientInstanceName="cBtnUnAuthorise"
                        CssClass="btnRight" Font-Size="8" TabIndex="0"
                        Text="UnAuthorise Selected">
                        <Paddings Padding="0px" />
                        <ClientSideEvents Click="function(s,e){btnUnAuthorizeSlctd_Click();}" />
                    </dxe:ASPxButton>
                    <div id="ShowFilter" style="width: 105px; float: left">
                        <a href="javascript:ShowHideFilter('s');" class="btn btn-success"><span>Show Filter</span></a>
                    </div>

                    <div id="ShowAllRecords" style="width: 105px; float: left">
                        <a href="javascript:ShowHideFilter('All');" class="btn btn-primary"><span>All Records</span></a>
                    </div>

                </div>
                <div class="left pagingContent" style="vertical-align: bottom;">
                    Page <b id="B_PageNo" runat="server"></b>Of <b id="B_TotalPage" runat="server"></b>
                    ( <b id="B_TotalRows" runat="server"></b>items ) <span class="textLeft"><a id="A_LeftNav"
                        runat="server" href="javascript:void(0);" onclick="OnLeftNav_Click()">
                        <img align="middle" alt="" class="paging_nav" src="/assests/images/LeftNav.gif" width="16" />
                    </a></span><a id="A1" runat="server" class="number_box" href="javascript:void(0);"
                        onclick="OnPageNo_Click('A1')">1 </a><a id="A2" runat="server" class="number_box"
                            href="javascript:void(0);" onclick="OnPageNo_Click('A2')">2 </a><a id="A3" runat="server"
                                class="number_box" href="javascript:void(0);" onclick="OnPageNo_Click('A3')">3 </a>
                    <a id="A4" runat="server" class="number_box" href="javascript:void(0);" onclick="OnPageNo_Click('A4')">4 </a><a id="A5" runat="server" class="number_box" href="javascript:void(0);" onclick="OnPageNo_Click('A5')">5 </a><a id="A6" runat="server" class="number_box" href="javascript:void(0);" onclick="OnPageNo_Click('A6')">6 </a><a id="A7" runat="server" class="number_box" href="javascript:void(0);" onclick="OnPageNo_Click('A7')">7 </a><a id="A8" runat="server" class="number_box" href="javascript:void(0);" onclick="OnPageNo_Click('A8')">8 </a><a id="A9" runat="server" class="number_box" href="javascript:void(0);" onclick="OnPageNo_Click('A9')">9 </a><a id="A10" runat="server" class="number_box" href="javascript:void(0);" onclick="OnPageNo_Click('A10')">10 </a><span class="textRight"><a id="A_RightNav" runat="server" href="javascript:void(0);"
                        onclick="OnRightNav_Click()">
                        <img align="middle" alt="" class="paging_nav" src="../images/RightNav.gif" width="16" />
                    </a></span><span class="clear"></span>
                </div>
            </div>
            <%-- OnCustomJSProperties="GvEmployeeDtl_CustomJSProperties" --%>
            <dxe:ASPxGridView ID="GvEmployeeDtl" runat="server" AutoGenerateColumns="False" Settings-ShowHorizontalScrollBar="true"
                ClientInstanceName="cGvEmployeeDtl" KeyFieldName="ContactID" OnCustomCallback="GvEmployeeDtl_CustomCallback"
                Width="100%">

                <ClientSideEvents EndCallback="function(s, e){GvEmployeeDtl_EndCallBack();}"
                    SelectionChanged="OnGridSelectionChanged" />
                <SettingsBehavior AllowFocusedRow="True" AutoFilterRowInputDelay="1200" AllowMultiSelection="True" />
                <Styles>
                    <Header ImageSpacing="5px" SortingImageSpacing="5px">
                    </Header>
                    <LoadingPanel ImageSpacing="10px">
                    </LoadingPanel>
                    <Row Wrap="False">
                    </Row>
                    <FocusedRow BackColor="#FCA977" HorizontalAlign="Left" VerticalAlign="Top">
                    </FocusedRow>
                    <AlternatingRow Enabled="True">
                    </AlternatingRow>
                </Styles>
                <Columns>
                    <dxe:GridViewCommandColumn FixedStyle="Left" ShowSelectCheckbox="True" VisibleIndex="0"
                        Width="15px">
                    </dxe:GridViewCommandColumn>
                    <dxe:GridViewDataTextColumn Caption="Name" FieldName="Name" VisibleIndex="1" Width="130px">
                        <CellStyle CssClass="gridcellleft" Font-Size="11px">
                        </CellStyle>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="EmpCode" FieldName="EmpCode" VisibleIndex="2"
                        Width="90px">
                        <CellStyle CssClass="gridcellleft" Font-Size="11px">
                        </CellStyle>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Department" FieldName="Department" VisibleIndex="3"
                        Width="150px">
                        <CellStyle CssClass="gridcellleft" Font-Size="11px">
                        </CellStyle>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Branch" FieldName="BranchName" VisibleIndex="4"
                        Width="100px">
                        <CellStyle CssClass="gridcellleft" Font-Size="11px">
                        </CellStyle>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="CTC" FieldName="CTC" VisibleIndex="5" Width="80px">
                        <CellStyle CssClass="gridcellleft" Font-Size="11px" HorizontalAlign="Right">
                        </CellStyle>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Designation" FieldName="Designation" VisibleIndex="6"
                        Width="170px">
                        <CellStyle CssClass="gridcellleft" Font-Size="11px">
                        </CellStyle>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Company" FieldName="Company" VisibleIndex="7"
                        Width="160px">
                        <CellStyle CssClass="gridcellleft" Font-Size="11px">
                        </CellStyle>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="AuthorizeUser1" FieldName="AuthUser1" VisibleIndex="8"
                        Width="180px">
                        <CellStyle CssClass="gridcellleft" Font-Size="11px">
                        </CellStyle>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="AuthorizeDateTime1" FieldName="AuthDateTime1" VisibleIndex="9"
                        Width="140px">
                        <CellStyle CssClass="gridcellleft" Font-Size="11px">
                        </CellStyle>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="AuthorizeUser2" FieldName="AuthUser2" VisibleIndex="10"
                        Width="180px">
                        <CellStyle CssClass="gridcellleft" Font-Size="11px">
                        </CellStyle>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="AuthorizeDateTime2" FieldName="AuthDateTime2" VisibleIndex="11"
                        Width="140px">
                        <CellStyle CssClass="gridcellleft" Font-Size="11px">
                        </CellStyle>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="AuthorizeUser3" FieldName="AuthUser3" VisibleIndex="12"
                        Width="180px">
                        <CellStyle CssClass="gridcellleft" Font-Size="11px">
                        </CellStyle>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="AuthorizeDateTime3" FieldName="AuthDateTime3" VisibleIndex="13"
                        Width="240px">
                        <CellStyle CssClass="gridcellleft" Font-Size="11px">
                        </CellStyle>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="ContactID" FieldName="ContactID" Visible="False"
                        VisibleIndex="14" Width="75px">
                        <CellStyle CssClass="gridcellleft" Wrap="False">
                        </CellStyle>
                    </dxe:GridViewDataTextColumn>
                </Columns>

                <Settings ShowGroupPanel="True" ShowHorizontalScrollBar="True" ShowStatusBar="Hidden" />
                <SettingsLoadingPanel Text="Please Wait..." />
                <SettingsPager Visible="False">
                </SettingsPager>
                <StylesPager EnableDefaultAppearance="False">
                </StylesPager>
            </dxe:ASPxGridView>

        </div>
        <%--<asp:HiddenField ID="Hdn_DateFrom" runat="server" />
        <asp:HiddenField ID="Hdn_DateTo" runat="server" />--%>
        <asp:HiddenField ID="HDNCheckedEmpCode" runat="server" />
        <asp:HiddenField ID="HDNShowBy" runat="server" />
        <asp:HiddenField ID="hdn_GridBindOrNotBind" runat="server" />
        <asp:HiddenField ID="Hdn_PageSize" runat="server" />
        <asp:HiddenField ID="Hdn_PageNumber" runat="server" />
        <asp:HiddenField ID="Hdn_DateFrom" runat="server" />
        <asp:HiddenField ID="Hdn_DateTo" runat="server" />
        <asp:HiddenField ID="Hdn_AuthorizeType" runat="server" />
        <asp:HiddenField ID="Hdn_AuthorizeFor" runat="server" />
        <asp:Button ID="BtnForExportEvent" runat="server" OnClick="cmbExport_SelectedIndexChanged" BackColor="#DDECFE" BorderStyle="None" />
    </div>
</asp:Content>
