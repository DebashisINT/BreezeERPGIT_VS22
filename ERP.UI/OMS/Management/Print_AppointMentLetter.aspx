<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true"
    Inherits="ERP.OMS.Management.management_Print_AppointMentLetter" CodeBehind="Print_AppointMentLetter.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<%@ Register Assembly="DevExpress.Web.v15.1" Namespace="DevExpress.Web"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1" Namespace="DevExpress.Web"
    TagPrefix="dxe000001" %>
<%@ Register Assembly="DevExpress.Web.v15.1" Namespace="DevExpress.Web"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1" Namespace="DevExpress.Web"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1" Namespace="DevExpress.Web"
    TagPrefix="dxpc" %>

<%@ Register Assembly="DevExpress.Web.v15.1" Namespace="DevExpress.Web"
    TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v15.1" Namespace="DevExpress.Web"
    TagPrefix="dxp" %>--%>
    <link type="text/css" href="../CSS/GenericCss.css" rel="Stylesheet" />
    <!--External Scripts file-->
    <!-- Ajax List Requierd-->
    <link type="text/css" href="../CSS/AjaxStyle.css" rel="Stylesheet" />

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
    </style>

    <script type="text/javascript" language="javascript">
        //Global Variable
        FieldName = '';
        //AjaxList
        var CombinedQuery;
        var WhichTextBox;

        //End
        var _selectNumber = 0;
        var _handle = true;

        function PageLoad() {
            HideShow("Row2_Col2", "H");
            HideShow("Row2_Col3", "H");
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
        function ChkSignature() {
            var checkbox = document.getElementById('chkSignature')
            if (checkbox.checked) {
                HideShow("Row2_Col2", "S");
                HideShow("Row2_Col3", "S");
                document.getElementById('txtSignature').focus();
            }
            else {
                HideShow("Row2_Col2", "H");
                HideShow("Row2_Col3", "H");
                document.getElementById('txtSignature_hidden').value = "";
                document.getElementById('txtSignature').value = "";
            }
        }

        function OnLeftNav_Click() {
            SetValue("hdn_GridBindOrNotBind", "False");
            var i = GetObjectID("A1").innerText;
            GetObjectID('A1').className = "number_box_selected";
            //SetValue("hdn_cGvEmployeeDtl","False"); //To Stop Bind On Page Load
            if (parseInt(i) > 1) {
                SetValue("HDNCheckedEmpCode", "");
                cGvEmployeeDtl.PerformCallback("SearchByNavigation~" + cComboLtrPrint.GetValue() + '~' + cDtFrom.GetText() + '~' + cDtTo.GetText() + "~" + GetObjectID("A1").innerText + "~LeftNav");
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
                cGvEmployeeDtl.PerformCallback("SearchByNavigation~" + cComboLtrPrint.GetValue() + '~' + cDtFrom.GetText() + '~' + cDtTo.GetText() + "~" + GetObjectID("A1").innerText + "~RightNav");
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
            cGvEmployeeDtl.PerformCallback("SearchByNavigation~" + cComboLtrPrint.GetValue() + '~' + cDtFrom.GetText() + '~' + cDtTo.GetText() + "~" + i + "~PageNav");
        }


        function GvEmployeeDtl_EndCallBack() {


            if (cGvEmployeeDtl.cpAuthorizeSlctd == "T") {
                alert('Selected Employees Authorized Successfully');
                SetValue("HDNCheckedEmpCode", "");
                cGvEmployeeDtl.PerformCallback('GridBind~' + cComboLtrPrint.GetValue() + '~' + cDtFrom.GetText() + '~' + cDtTo.GetText());
                cGvEmployeeDtl.cpAuthorizeSlctd = null;
            }
            if (cGvEmployeeDtl.cpAuthorizeAll == "T") {
                alert('All Employees Authorized Successfully');
                SetValue("HDNCheckedEmpCode", "");
                cGvEmployeeDtl.PerformCallback('GridBind~' + cComboLtrPrint.GetValue() + '~' + cDtFrom.GetText() + '~' + cDtTo.GetText());
                cGvEmployeeDtl.cpAuthorizeAll = null;
            }
            if (cGvEmployeeDtl.cpUnAuthorizeSlctd == "T") {
                alert('Selected Employees UnAuthorized Successfully');
                SetValue("HDNCheckedEmpCode", "");
                cGvEmployeeDtl.PerformCallback('GridBind~' + cComboLtrPrint.GetValue() + '~' + cDtFrom.GetText() + '~' + cDtTo.GetText());
                cGvEmployeeDtl.cpUnAuthorizeSlctd = null;
            }
            if (cGvEmployeeDtl.cpUnAuthorizeAll == "T") {
                alert('All Employees UnAuthorized Successfully');
                SetValue("HDNCheckedEmpCode", "");
                cGvEmployeeDtl.PerformCallback('GridBind~' + cComboLtrPrint.GetValue() + '~' + cDtFrom.GetText() + '~' + cDtTo.GetText());
                cGvEmployeeDtl.cpUnAuthorizeAll = null;
            }
            if (cGvEmployeeDtl.cpNoSelection == "T") {
                alert('Please Select Any Row First ');
                cBtnAuthorise.SetEnabled(true);
                cGvEmployeeDtl.cpNoSelection = null;
            }
            if (cGvEmployeeDtl.cpExcelExport == "T") {
                document.getElementById('BtnForExportEvent').click();
                cGvEmployeeDtl.cpExcelExport = null;
            }
            if (cGvEmployeeDtl.cpbtnrptall == "rptall") {
                cBtnAuthoriseAll.SetEnabled(true);

                document.getElementById('BtnForrpt').click();
                cGvEmployeeDtl.cpbtnrptall = null;
            }
            if (cGvEmployeeDtl.cpbtnrptselected == "rptselected") {
                cBtnAuthorise.SetEnabled(true);
                document.getElementById('btnrptselectted').click();
                cGvEmployeeDtl.cpbtnrptselected = null;
            }
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
            if (cGvEmployeeDtl.cpAjaxEmployeeSign != undefined) {
                var Txt_Signature = document.getElementById('<%=txtSignature.ClientID%>');
                CombinedQuery = cGvEmployeeDtl.cpAjaxEmployeeSign;
                WhichTextBox = Txt_Signature;
                Txt_Signature.attachEvent('onkeyup', CallGenericAjaxJS);
            }
            SetValue("hdn_GridBindOrNotBind", "True");
            Height('650', '650');


        }
        function CallGenericAjaxJS(e) {
            CombinedQuery = CombinedQuery.replace("\'", "'");
            ajax_showOptions(WhichTextBox, 'GenericAjaxList', e, replaceChars(CombinedQuery), 'Main');
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
            if (values != null || values != undefined)
                SetValue("HDNCheckedEmpCode", values);
            //alert(GetValue("HDNCheckedEmpCode"));              
        }
        function OnAuthorizationTypeChange() {
            SetValue("HDNShowBy", "");
            SetValue("HDNCheckedEmpCode", "");
            SetValue("hdn_GridBindOrNotBind", "False");

            cGvEmployeeDtl.PerformCallback('GridBind~' + cComboLtrPrint.GetValue() + '~' + cDtFrom.GetText() + '~' + cDtTo.GetText());
        }

        function btnAuthorizeSlctd_Click() {

            cBtnAuthorise.SetEnabled(false);
            var txtbox = '';
            SetValue("hdn_GridBindOrNotBind", "False");
            var checkbox = document.getElementById('chkSignature').checked;
            if (checkbox == true) {
                txtbox = document.getElementById("txtSignature_hidden").value;
                if (txtbox == '') {
                    alert('Please Select Signature');
                    cBtnAuthorise.SetEnabled(true);
                    document.getElementById("txtSignature").focus();
                }
                else {
                    cGvEmployeeDtl.PerformCallback('AuthorizeGridBind~' + GetValue("HDNCheckedEmpCode") + '~' + cDtFrom.GetText() + '~' + cDtTo.GetText());
                }
            }
            else
                cGvEmployeeDtl.PerformCallback('AuthorizeGridBind~' + GetValue("HDNCheckedEmpCode") + '~' + cDtFrom.GetText() + '~' + cDtTo.GetText());
            //        else
            //        {
            //        alert('Please Select Signature');
            //        cBtnAuthorise.SetEnabled(true);
            //        }
        }

        function btnAuthorizeAll_Click() {
            cBtnAuthoriseAll.SetEnabled(false);
            SetValue("hdn_GridBindOrNotBind", "False");
            SetValue("HDNCheckedEmpCode", "All");
            var txtbox = '';
            var checkbox = document.getElementById('chkSignature').checked;
            var checkbox = document.getElementById('chkSignature').checked;
            if (checkbox == true) {
                txtbox = document.getElementById("txtSignature_hidden").value;
                if (txtbox == '') {
                    alert('Please Select Signature');
                    cBtnAuthoriseAll.SetEnabled(true);
                    document.getElementById("txtSignature").focus();
                }
                else {
                    cGvEmployeeDtl.PerformCallback('AuthorizeAllGridBind~' + GetValue("HDNCheckedEmpCode") + '~' + cDtFrom.GetText() + '~' + cDtTo.GetText() + '~' + checkbox);
                }
            }
            else
                cGvEmployeeDtl.PerformCallback('AuthorizeAllGridBind~' + GetValue("HDNCheckedEmpCode") + '~' + cDtFrom.GetText() + '~' + cDtTo.GetText() + '~' + checkbox);
        }


        function btnShow_Click() {
            SetValue("HDNShowBy", "");
            SetValue("HDNCheckedEmpCode", "");
            SetValue("hdn_GridBindOrNotBind", "False");

            cGvEmployeeDtl.PerformCallback('GridBind~' + cComboLtrPrint.GetValue() + '~' + cDtFrom.GetText() + '~' + cDtTo.GetText());
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
                cGvEmployeeDtl.PerformCallback('ExcelReport~' + cComboLtrPrint.GetValue() + '~' + cDtFrom.GetText() + '~' + cDtTo.GetText());
            }
        }

        function OnTypeChange() {
            SetValue("hdn_GridBindOrNotBind", "False");
            SetValue("HDNCheckedEmpCode", "");
            cGvEmployeeDtl.PerformCallback('GridBind~' + cComboLtrPrint.GetValue() + '~' + cDtFrom.GetText() + '~' + cDtTo.GetText());
        }
        function ShowHideFilter(obj) {
            SetValue("HDNCheckedEmpCode", "");
            cGvEmployeeDtl.PerformCallback('GridBindFilter~' + cComboLtrPrint.GetValue() + '~' + cDtFrom.GetText() + '~' + cDtTo.GetText() + '~' + obj);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="Gmain">
        <div id="Div_header" class="Header">
            &nbsp;Print Appointment Letter&nbsp;
        </div>
        <div id="Container1" class="container" style="width: 99%">
            <div id="Row1" class="LFloat_Content paddingContnt" style="width: 99%; margin-bottom: 4px;">
                <div id="Row1_Column1" class="LFloat_Content divHeight" style="width: 20%;">
                    <dxe:ASPxDateEdit ID="DtFrom" runat="server" ClientInstanceName="cDtFrom" DateOnError="Today"
                        EditFormat="Custom" EditFormatString="dd-MM-yyyy" UseMaskBehavior="True" Width="98%">
                        <ButtonStyle Width="3px">
                        </ButtonStyle>
                        <DropDownButton Text="Req.DateFrom">
                        </DropDownButton>
                        <ClientSideEvents DateChanged="function(s,e){SetTradeDate();}" />
                    </dxe:ASPxDateEdit>
                </div>
                <div id="Row1_Column2" class="LFloat_Content divHeight" style="width: 20%;">
                    <dxe:ASPxDateEdit ID="DtTo" runat="server" ClientInstanceName="cDtTo" DateOnError="Today"
                        EditFormat="Custom" EditFormatString="dd-MM-yyyy" UseMaskBehavior="True" Width="98%">
                        <ButtonStyle Width="3px">
                        </ButtonStyle>
                        <DropDownButton Text="Req.DateTo">
                        </DropDownButton>
                        <ClientSideEvents DateChanged="function(s,e){DateCompare(cDtFrom,cDtTo);}" />
                    </dxe:ASPxDateEdit>
                </div>
                <div id="Row1_Column4" class="LFloat_Content divHeight" style="width: 21%">
                    <dxe:ASPxComboBox ID="ComboLtrPrint" runat="server" ClientInstanceName="cComboLtrPrint"
                        EnableIncrementalFiltering="True" Font-Size="Small" SelectedIndex="0" ValueType="System.String"
                        Width="100%">
                        <DropDownButton Text="FilterBy">
                        </DropDownButton>
                        <Items>
                            <dxe:ListEditItem Text="To Be Printed" Value="ToBePrinted" />
                            <dxe:ListEditItem Text="Already Printed" Value="AlrdyPrinted" />
                            <dxe:ListEditItem Text="All" Value="All" />
                        </Items>
                        <ClientSideEvents SelectedIndexChanged="function(s,e){OnAuthorizationTypeChange()}" />
                    </dxe:ASPxComboBox>
                </div>
                <div id="Row1_Column3" class="LFloat_Row BtnLeft divHeight" style="width: 86px;">
                    <dxe:ASPxButton ID="btnShow" runat="server" AutoPostBack="False" ClientInstanceName="cbtnShow"
                        Text="Show" Width="99%">
                        <ClientSideEvents Click="function(s,e){btnShow_Click();}" />
                    </dxe:ASPxButton>
                </div>
            </div>
        </div>
        <div id="lblGridHeader" class="paging textLeft">
            <span class="pagingContent">Showing Record(s) <span id="spnReqType"></span></span>
        </div>
        <div id="Container2" class="paging textLeft">
            <div class="right">
                <div>
                    <div id="Row_callender">
                        <div id="Row_callender_Col1" class="LFloat_Content">
                            Appt.Ltr.Date:
                        </div>
                        <div id="Row_callender_Col2" class="LFloat_Content">
                            <dxe:ASPxDateEdit ID="appointmentdate" runat="server" ClientInstanceName="dtpDate"
                                Width="157px" EditFormat="Custom" UseMaskBehavior="True" EditFormatString="dd-MM-yyyy">
                                <DropDownButton Text="Date">
                                </DropDownButton>
                            </dxe:ASPxDateEdit>
                        </div>
                    </div>
                </div>
                <div>
                    <div id="Row2">
                        <div id="Row2_Col1" class="LFloat_Content">
                            Sign.:
                                <input id="chkSignature" onclick="ChkSignature()" type="checkbox" runat="server" />
                        </div>
                        <div id="Row2_Col2" class="Content">
                            Employee:
                        </div>
                        <div id="Row2_Col3" class="Content">
                            <asp:TextBox ID="txtSignature" runat="server" Width="200px"></asp:TextBox>
                            <asp:TextBox Style="display: none" ID="txtSignature_hidden" TabIndex="11" runat="server"
                                Width="100px"></asp:TextBox>
                        </div>
                    </div>
                    <div>
                        <div id="Div1" class="LFloat_Content">
                            Cmp. Logo:
                                <input id="ChkLogo" type="checkbox" runat="server" />
                        </div>
                        <div id="Div2" class="LFloat_Content">
                            Cmp. Name:
                                <input id="Chkcmpname" type="checkbox" runat="server" />
                        </div>
                        <div id="Div3" class="LFloat_Content">
                            Cmp. Add.:
                                <input id="Chkcmpaddress" type="checkbox" runat="server" />
                        </div>
                    </div>
                </div>
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
                    CssClass="btnRight" Font-Size="8" TabIndex="0" Text="       Print All      ">
                    <Paddings Padding="0px" />
                    <ClientSideEvents Click="function(s,e){btnAuthorizeAll_Click();}" />
                </dxe:ASPxButton>
                <span style="color: #555555; background-color: #ededed"></span>
                <dxe:ASPxButton ID="BtnAuthorise" runat="server" AutoPostBack="False" ClientInstanceName="cBtnAuthorise"
                    CssClass="btnRight" Font-Size="8" TabIndex="0" Text="Print Selected">
                    <Paddings Padding="0px" />
                    <ClientSideEvents Click="function(s,e){btnAuthorizeSlctd_Click();}" />
                </dxe:ASPxButton>




            </div>
            <div>
                <div id="ShowFilter" style="width: 69px; float: left">
                    <a href="javascript:ShowHideFilter('s');"><span style="color: #000099; text-decoration: underline; font-size: 12px">Show Filter</span></a>
                </div>
                <div id="ShowAllRecords" style="width: 72px; float: left">
                    <a href="javascript:ShowHideFilter('All');"><span style="color: #000099; text-decoration: underline; font-size: 12px">All Records</span></a>
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
        <dxe:ASPxGridView ID="GvEmployeeDtl" runat="server" AutoGenerateColumns="False"
            Settings-ShowHorizontalScrollBar="true" ClientInstanceName="cGvEmployeeDtl" KeyFieldName="ContactID"
            OnCustomCallback="GvEmployeeDtl_CustomCallback" Width="985px">
            <ClientSideEvents EndCallback="function(s, e){GvEmployeeDtl_EndCallBack();}" SelectionChanged="OnGridSelectionChanged" />
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
                    Width="120px">
                    <CellStyle CssClass="gridcellleft" Font-Size="11px">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="AuthorizeDateTime1" FieldName="AuthDateTime1"
                    VisibleIndex="9" Width="140px">
                    <CellStyle CssClass="gridcellleft" Font-Size="11px">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="AuthorizeUser2" FieldName="AuthUser2" VisibleIndex="10"
                    Width="120px">
                    <CellStyle CssClass="gridcellleft" Font-Size="11px">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="AuthorizeDateTime2" FieldName="AuthDateTime2"
                    VisibleIndex="11" Width="140px">
                    <CellStyle CssClass="gridcellleft" Font-Size="11px">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="AuthorizeUser3" FieldName="AuthUser3" VisibleIndex="12"
                    Width="120px">
                    <CellStyle CssClass="gridcellleft" Font-Size="11px">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="AuthorizeDateTime3" FieldName="AuthDateTime3"
                    VisibleIndex="13" Width="240px">
                    <CellStyle CssClass="gridcellleft" Font-Size="11px">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Printed By" FieldName="ExportedUser" VisibleIndex="14"
                    Width="240px">
                    <CellStyle CssClass="gridcellleft" Font-Size="11px">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Printed DateTime" FieldName="ExportedDateTime"
                    VisibleIndex="15" Width="240px">
                    <CellStyle CssClass="gridcellleft" Font-Size="11px">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="ContactID" FieldName="ContactID" Visible="False"
                    VisibleIndex="16" Width="75px">
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
    <asp:Button ID="BtnForExportEvent" runat="server" OnClick="cmbExport_SelectedIndexChanged"
        BackColor="#DDECFE" BorderStyle="None" />
    <asp:Button ID="BtnForrpt" runat="server" OnClick="btnrptall_SelectedIndexChanged"
        BackColor="#DDECFE" BorderStyle="None" />
    <asp:Button ID="btnrptselectted" runat="server" OnClick="btnrptselectted_SelectedIndexChanged"
        BackColor="#DDECFE" BorderStyle="None" />
</asp:Content>

