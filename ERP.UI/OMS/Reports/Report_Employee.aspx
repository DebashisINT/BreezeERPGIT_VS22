<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Reports.reports_report_employee" CodeBehind="Report_Employee.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxTabControl"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dxpc" %>
<%@ Register Assembly="DevExpress.Web.v10.2.Export" Namespace="DevExpress.Web.Export" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxCallbackPanel"
    TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxPanel"
    TagPrefix="dxp" %>--%>
   
    <!--Ajax List Section-->
    <script type="text/javascript" src="/assests/js/ajax-dynamic-list_rootfile.js"></script>
    <script type="text/javascript" src="/assests/js/init.js"></script>
    <link href="../CSS/AjaxStyle.css" rel="stylesheet" type="text/css" />
    <!--End Ajax List Section-->

    <!--dhtml PopUp Section-->
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />
    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <link rel="stylesheet" href="../modalfiles/modal.css" type="text/css" />
    <script type="text/javascript" src="../modalfiles/modal.js"></script>
    <!--End dhtml PopUp Section-->

    <script language="javascript" type="text/javascript">
       
        function ShowDate(obj) {
            if (document.getElementById('TdSelect').style.display == 'inline') {
                clientselectionfinal();
                document.getElementById('<%=RadActiveEmp.ClientID%>').checked = true;
                divAsOnDate.innerText = "Employee Joining Date :";
            }
            else {
                document.getElementById('TdFilter').style.display = 'none';
                document.getElementById('TdFilter1').style.display = 'none';
                document.getElementById('TdSelect').style.display = 'none';
                if (obj == 'b') {
                    divAsOnDate.innerText = "Employee Leaving Date :";
                }
                else {
                    divAsOnDate.innerText = "Employee Joining Date :";
                }
            }
        }
        function ShowDateRange(obj) {
            if (document.getElementById('TdSelect').style.display == 'inline') {
                clientselectionfinal();
                document.getElementById('<%=RadDateRangeA.ClientID %>').checked = true;
                document.getElementById("trDateRange").style.display = "none";
            }
            else {
                document.getElementById('TdFilter').style.display = 'none';
                document.getElementById('TdFilter1').style.display = 'none';
                document.getElementById('TdSelect').style.display = 'none';
                if (obj == 'a') {
                    document.getElementById("trDateRange").style.display = "none";
                }
                else if (obj == 's') {
                    document.getElementById("trDateRange").style.display = "inline";
                }
            }
        }
        function Page_Load() {
            divAsOnDate.innerText = "Employee Joining Date :";
            document.getElementById('TdFilter').style.display = 'none';
            document.getElementById('TdFilter1').style.display = 'none';
            document.getElementById('TdSelect').style.display = 'none';
            document.getElementById('TdGrid').style.display = 'none';
            document.getElementById('TdExport').style.display = 'none';
            document.getElementById("trDateRange").style.display = "none";
            document.getElementById('TrButton').style.display = 'inline';
        }
        function AllSelct(obj, obj1) {
            var FilTer = document.getElementById('cmbsearchOption');
            if (obj != 'a') {
                document.getElementById('<%=txtsubscriptionID.ClientID%>').value = '';
                if (document.getElementById('TdSelect').style.display == 'inline') {
                    if (obj1 == 'C') {
                        document.getElementById('<%=RadCompanyA.ClientID%>').checked = true;
                   }
                   else if (obj1 == 'B') {
                       document.getElementById('<%=RadBranchA.ClientID%>').checked = true;
                   }
                   else if (obj1 == 'E') {
                       document.getElementById('<%=RadEmployeeA.ClientID%>').checked = true;
                    }
                    else if (obj1 == 'R') {
                        document.getElementById('<%=RadReportToA.ClientID%>').checked = true;
                  }
                  else if (obj1 == 'T') {
                      document.getElementById('<%=TadTypeA.ClientID%>').checked = true;
                    }
    clientselectionfinal();
}
else {
    if (obj1 == 'C') {
        FilTer.value = 'Company';
    }
    else if (obj1 == 'B') {
        FilTer.value = 'Branch';
    }
    else if (obj1 == 'E') {
        FilTer.value = 'Employee';
    }
    else if (obj1 == 'R') {
        FilTer.value = 'ReportTo';
    }
    else if (obj1 == 'T') {
        FilTer.value = 'Type';
    }
}
    document.getElementById('TdFilter').style.display = 'inline';
    document.getElementById('TdFilter1').style.display = 'inline';
    document.getElementById('TdSelect').style.display = 'inline';
}
else {
    document.getElementById('TdFilter').style.display = 'none';
    document.getElementById('TdFilter1').style.display = 'none';
    document.getElementById('TdSelect').style.display = 'none';

    document.getElementById('TrEmpStat').style.display = "inline";
    document.getElementById('TrDate').style.display = "inline";
    document.getElementById('trClient').style.display = "inline";
    document.getElementById('trBranch').style.display = "inline";
    document.getElementById('trReportTo').style.display = "inline";
    document.getElementById('TrAccount').style.display = "inline";
}
}
function HideFilter() {
    document.getElementById('TrAll').style.display = 'none';
    document.getElementById('TdShowFilter').style.display = 'inline';
    height();
}
function Filter() {
    document.getElementById('TrAll').style.display = 'inline';
    document.getElementById('TrButton').style.display = 'inline';
    document.getElementById('TdGrid').style.display = 'none';
    document.getElementById('TdExport').style.display = 'none';
    height();
}
function btnAddsubscriptionlist_click() {
    var userid = document.getElementById('txtsubscriptionID');
    if (userid.value != '') {
        var ids = document.getElementById('txtsubscriptionID_hidden');
        var listBox = document.getElementById('lstSuscriptions');
        var tLength = listBox.length;
        var no = new Option();
        no.value = ids.value;
        no.text = userid.value;
        listBox[tLength] = no;
        var recipient = document.getElementById('txtsubscriptionID');
        recipient.value = '';
    }
    else
        alert('Please Search Name And Then Add!')
    var s = document.getElementById('txtsubscriptionID');
    s.focus();
    s.select();
}
function btnRemovefromsubscriptionlist_click() {
    var listBox = document.getElementById('lstSuscriptions');
    var tLength = listBox.length;
    var arrTbox = new Array();
    var arrLookup = new Array();
    var i;
    var j = 0;
    for (i = 0; i < listBox.options.length; i++) {
        if (listBox.options[i].selected && listBox.options[i].value != "") {

        }
        else {
            arrLookup[listBox.options[i].text] = listBox.options[i].value;
            arrTbox[j] = listBox.options[i].text;
            j++;
        }
    }
    listBox.length = 0;
    for (i = 0; i < j; i++) {
        var no = new Option();
        no.value = arrLookup[arrTbox[i]];
        no.text = arrTbox[i];
        listBox[i] = no;
    }
}
function clientselectionfinal() {
    var listBoxSubs = document.getElementById('lstSuscriptions');
    var cmb = document.getElementById('cmbsearchOption');
    var listIDs = '';
    var i;
    if (listBoxSubs.length > 0) {
        for (i = 0; i < listBoxSubs.length; i++) {
            if (listIDs == '')
                listIDs = listBoxSubs.options[i].value + ';' + listBoxSubs.options[i].text;
            else
                listIDs += ',' + listBoxSubs.options[i].value + ';' + listBoxSubs.options[i].text;
        }
        var sendData = cmb.value + '~' + listIDs;
        CallServer(sendData, "");

        document.getElementById('TrAll').style.display = 'inline';
        document.getElementById('TdFilter').style.display = 'none';
        document.getElementById('TdSelect').style.display = 'none';
        document.getElementById('TdFilter1').style.display = 'none';
    }
    else {
        alert("Please Select Atleast One " + cmb.value + " Item!!!");
        document.getElementById('TrAll').style.display = 'inline';
        document.getElementById('TdFilter').style.display = 'inline';
        document.getElementById('TdSelect').style.display = 'inline';
        document.getElementById('TdFilter1').style.display = 'inline';
        document.getElementById('<%=txtsubscriptionID.ClientID%>').focus();
    }
    var i;
    for (i = listBoxSubs.options.length - 1; i >= 0; i--) {
        listBoxSubs.remove(i);
    }
}
function FunClientScrip(objID, objListFun, objEvent) {
    var cmbVal;
    if (document.getElementById('cmbsearchOption').value == "Employee") {
        cmbVal = document.getElementById('cmbsearchOption').value;
        cmbVal = cmbVal + '~E';
    }
    else if (document.getElementById('cmbsearchOption').value == "Company") {
        cmbVal = 'Organization~C';
    }
    else if (document.getElementById('cmbsearchOption').value == "Branch") {
        cmbVal = document.getElementById('cmbsearchOption').value;
        cmbVal = cmbVal + '~B';
    }
    else if (document.getElementById('cmbsearchOption').value == "ReportTo") {
        cmbVal = document.getElementById('cmbsearchOption').value;
        cmbVal = cmbVal + '~R';
    }
    else if (document.getElementById('cmbsearchOption').value == "Type") {
        cmbVal = document.getElementById('cmbsearchOption').value;
        cmbVal = cmbVal + '~T';
    }
    else {
        cmbVal = document.getElementById('cmbsearchOption').value;
        cmbVal = cmbVal + '~N';
    }
    ajax_showOptions(objID, objListFun, objEvent, cmbVal);
}
function ReceiveServerData(rValue) {
    var Data = rValue.split('~');

    if (Data[0] == 'Employee') {
        Employeevalue = Data[1];
        document.getElementById('HDNEmployee').value = Data[1];
    }
    if (Data[0] == 'Branch') {
        Branchvalue = Data[1];
        document.getElementById('HDNBranch').value = Data[1];
    }
    if (Data[0] == 'Company') {
        Companyvalue = Data[1];
        document.getElementById('HDNCompany').value = Data[1];
    }
    if (Data[0] == 'ReportTo') {
        ReportTovalue = Data[1];
        document.getElementById('HDNReportTo').value = Data[1];
    }
    if (Data[0] == 'Type') {
        Typevalue = Data[1];
        document.getElementById('HDNType').value = Data[1];
    }
}
function ShowHideFilter(obj) {
    grid.PerformCallback("Filter~" + obj);
}
FieldName = 'lstSuscriptions';
    </script>

    <!-- Navigation Script-->
    <script language="javascript" type="text/javascript">
        function OnLeftNav_Click() {
            var i = document.getElementById("A1").innerText;
            document.getElementById("hdn_GridBindOrNotBind").value = "False"; //To Stop Bind On Page Load
            if (parseInt(i) > 1) {
                var IsDateRageALL = document.getElementById('<%=RadDateRangeA.ClientID %>');
                if (IsDateRageALL.checked)
                    cGrdEmployee.PerformCallback("SearchByNavigation~" + cDtFrom.GetValue() + '~' + cDtTo.GetValue() + "~" + document.getElementById("A1").innerText + "~LeftNav");
                else
                    cGrdEmployee.PerformCallback("SearchByNavigation~~~" + document.getElementById("A1").innerText + "~LeftNav");
            }
            else {
                alert('No More Pages.');
            }
        }
        function OnRightNav_Click() {
            var TestEnd = document.getElementById("A10").innerText;
            document.getElementById("hdn_GridBindOrNotBind").value = "False"; //To Stop Bind On Page Load
            var TotalPage = document.getElementById("B_TotalPage").innerText;
            if (TestEnd == "" || TestEnd == TotalPage) {
                alert('No More Records.');
                return;
            }
            var i = document.getElementById("A1").innerText;
            if (parseInt(i) < TotalPage) {
                var IsDateRageALL = document.getElementById('<%=RadDateRangeA.ClientID %>');
                if (IsDateRageALL.checked)
                    cGrdEmployee.PerformCallback("SearchByNavigation~" + cDtFrom.GetValue() + '~' + cDtTo.GetValue() + "~" + document.getElementById("A1").innerText + "~RightNav");
                else
                    cGrdEmployee.PerformCallback("SearchByNavigation~~~" + document.getElementById("A1").innerText + "~RightNav");
            }
            else {
                alert('You are at the End');
            }
        }
        function OnPageNo_Click(obj) {
            var i = document.getElementById(obj).innerText;
            document.getElementById("hdn_GridBindOrNotBind").value = "False"; //To Stop Bind On Page Load
            var IsDateRageALL = document.getElementById('<%=RadDateRangeA.ClientID %>');
            if (IsDateRageALL.checked)
                cGrdEmployee.PerformCallback("SearchByNavigation~" + cDtFrom.GetValue() + '~' + cDtTo.GetValue() + "~" + i + "~PageNav");
            else
                cGrdEmployee.PerformCallback("SearchByNavigation~~~" + i + "~PageNav");

        }
        function BtnShow_Click() {
            document.getElementById("hdn_GridBindOrNotBind").value = "False"; //To Stop Bind On Page Load
            var IsDateRageALL = document.getElementById('<%=RadDateRangeA.ClientID %>');
        if (IsDateRageALL.checked)
            cGrdEmployee.PerformCallback("Show~" + cDtFrom.GetValue() + '~' + cDtTo.GetValue());
        else
            cGrdEmployee.PerformCallback("Show~~~");
    }
    function BtnShowFilter_Click() {
        document.getElementById('TrAll').style.display = 'inline';
        document.getElementById('TdShowFilter').style.display = 'none';
        height();
    }

    function GrdEmployee_EndCallBack() {
        if (cGrdEmployee.cpExcelExport != undefined) {
            document.getElementById('BtnForExportEvent').click();
        }
        if (cGrdEmployee.cpRefreshNavPanel != undefined) {
            document.getElementById("B_PageNo").innerText = '';
            document.getElementById("B_TotalPage").innerText = '';
            document.getElementById("B_TotalRows").innerText = '';

            var NavDirection = cGrdEmployee.cpRefreshNavPanel.split('~')[0];
            var PageNum = cGrdEmployee.cpRefreshNavPanel.split('~')[1];
            var TotalPage = cGrdEmployee.cpRefreshNavPanel.split('~')[2];
            var TotalRows = cGrdEmployee.cpRefreshNavPanel.split('~')[3];

            if (NavDirection == "RightNav") {
                PageNum = parseInt(PageNum) + 10;
                document.getElementById("B_PageNo").innerText = PageNum;
                document.getElementById("B_TotalPage").innerText = TotalPage;
                document.getElementById("B_TotalRows").innerText = TotalRows;
                var n = parseInt(TotalPage) - parseInt(PageNum) > 10 ? parseInt(11) : parseInt(TotalPage) - parseInt(PageNum) + 2;
                for (r = 1; r < n; r++) {
                    var obj = "A" + r;
                    document.getElementById(obj).innerText = PageNum++;
                }
                for (r = n; r < 11; r++) {
                    var obj = "A" + r;
                    document.getElementById(obj).innerText = "";
                }
            }
            if (NavDirection == "LeftNav") {
                if (parseInt(PageNum) > 1) {
                    PageNum = parseInt(PageNum) - 10;
                    document.getElementById("B_PageNo").innerText = PageNum;
                    document.getElementById("B_TotalPage").innerText = TotalPage;
                    document.getElementById("B_TotalRows").innerText = TotalRows;
                    for (l = 1; l < 11; l++) {
                        var obj = "A" + l;
                        document.getElementById(obj).innerText = PageNum++;
                    }
                }
                else {
                    alert('No More Pages.');
                }
            }
            if (NavDirection == "PageNav") {
                document.getElementById("B_PageNo").innerText = PageNum;
                document.getElementById("B_TotalPage").innerText = TotalPage;
                document.getElementById("B_TotalRows").innerText = TotalRows;
            }
            if (NavDirection == "ShowBtnClick") {
                document.getElementById("B_PageNo").innerText = PageNum;
                document.getElementById("B_TotalPage").innerText = TotalPage;
                document.getElementById("B_TotalRows").innerText = TotalRows;
                var n = parseInt(TotalPage) - parseInt(PageNum) > 10 ? parseInt(11) : parseInt(TotalPage) - parseInt(PageNum) + 2;

                for (r = 1; r < n; r++) {
                    var obj = "A" + r;
                    document.getElementById(obj).innerText = PageNum++;
                }

                for (r = n; r < 11; r++) {
                    var obj = "A" + r;
                    document.getElementById(obj).innerText = "";
                }

            }
        }
        if (cGrdEmployee.cpCallOtherWhichCallCondition != undefined) {
            if (cGrdEmployee.cpCallOtherWhichCallCondition == "Show") {
                if (crbDOJ_Specific_All.GetValue() == "S")
                    cGrdEmployee.PerformCallback("Show~" + cDtFrom.GetValue() + '~' + cDtTo.GetValue());
                else
                    cGrdEmployee.PerformCallback("Show~~~");
            }
        }
        if (cGrdEmployee.cpSetGlobalFields != undefined) {
            document.getElementById("Hdn_PageSize").value = cGrdEmployee.cpSetGlobalFields.split('~')[0];
            document.getElementById("Hdn_PageNumber").value = cGrdEmployee.cpSetGlobalFields.split('~')[1];
            document.getElementById("Hdn_Emp_ContactID").value = cGrdEmployee.cpSetGlobalFields.split('~')[2];
            document.getElementById("Hdn_Emp_Status").value = cGrdEmployee.cpSetGlobalFields.split('~')[3];
            document.getElementById("Hdn_DateFrom").value = cGrdEmployee.cpSetGlobalFields.split('~')[4];
            document.getElementById("Hdn_DateTo").value = cGrdEmployee.cpSetGlobalFields.split('~')[5];
            document.getElementById("Hdn_Company").value = cGrdEmployee.cpSetGlobalFields.split('~')[6];
            document.getElementById("Hdn_Branch").value = cGrdEmployee.cpSetGlobalFields.split('~')[7];
            document.getElementById("Hdn_ReportTo").value = cGrdEmployee.cpSetGlobalFields.split('~')[8];
            document.getElementById("Hdn_EmployeeType").value = cGrdEmployee.cpSetGlobalFields.split('~')[9];
            document.getElementById("Hdn_DevXFilterOn").value = cGrdEmployee.cpSetGlobalFields.split('~')[10];
            document.getElementById("Hdn_DevXFilterString").value = cGrdEmployee.cpSetGlobalFields.split('~')[11];
            document.getElementById("Hdn_ReportType").value = cGrdEmployee.cpSetGlobalFields.split('~')[12];
            HideFilter();
        }




        //Now Reset GridBindOrNotBind to True for Next Page Load
        document.getElementById("hdn_GridBindOrNotBind").value = "True";
        height();
    }
    function ddlExport_OnChange() {
        document.getElementById("hdn_GridBindOrNotBind").value = "False";
        var ddlExport = document.getElementById("<%=ddlExport.ClientID%>");
        var IsDateRageALL = document.getElementById('<%=RadDateRangeA.ClientID %>');
        if (IsDateRageALL.checked)
            cGrdEmployee.PerformCallback("ExcelExport~" + cDtFrom.GetValue() + '~' + cDtTo.GetValue());
        else
            cGrdEmployee.PerformCallback("ExcelExport~~~");
        ddlExport.options[0].selected = true;
    }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Employee Details</h3>
        </div>

    </div>
    <div class="form_main">
        <table style="width: 98%">
        </table>
        <table>
            <tr id="TrAll">
                <td>
                    <table class="TableMain100">
                        <tr>
                            <td>
                                <table  cellpadding="2" cellspacing="1 " class="spctbl">
                                    <tr id="trEmployee">
                                        <td class="gridcellleft" width="150px">Select Employee
                                        </td>
                                        <td align="left">
                                            <table width="150px">
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="RadEmployeeA" runat="server" Checked="True" GroupName="t1" onclick="AllSelct('a','E')" />
                                                    </td>
                                                    <td>All
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="RadEmployeeS" runat="server" GroupName="t1" onclick="AllSelct('b','E')" />
                                                    </td>
                                                    <td>Specific
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr id="TrEmpStat">
                                        <td class="gridcellleft" >Employee Status :
                                        </td>
                                        <td class="gridcellleft">
                                            <table cellpadding="0" cellspacing="0" width="250px">
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="RadActiveEmp" runat="server" Checked="true" GroupName="k1" onclick="ShowDate('a')" /></td>
                                                    <td class="gridcellleft">Active Employee
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="RadExEmp" runat="server" GroupName="k1" onclick="ShowDate('b')" /></td>
                                                    <td class="gridcellleft">Ex Employee
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="RadActiveExBoth" runat="server" GroupName="k1" onclick="ShowDate('c')" /></td>
                                                    <td class="gridcellleft">Both
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr id="TrDate">
                                        <td class="gridcellleft"  >
                                            <div id="divAsOnDate">
                                            </div>
                                        </td>
                                        <td align="left">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <table cellpadding="0" cellspacing="0" width="200px">
                                                            <tr>
                                                                <td>
                                                                    <asp:RadioButton ID="RadDateRangeA" runat="server" Checked="true" GroupName="G1"
                                                                        onclick="ShowDateRange('a')" /></td>
                                                                <td class="gridcellleft">All
                                                                </td>
                                                                <td>
                                                                    <asp:RadioButton ID="RadDateRangeS" runat="server" GroupName="G1" onclick="ShowDateRange('s')" /></td>
                                                                <td class="gridcellleft">Specific Date Range
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td id="trDateRange" style="display: none">
                                                        <table>
                                                            <tr>
                                                                <td id="TdFrom">
                                                                    <dxe:ASPxDateEdit ID="DtFrom" runat="server" ClientInstanceName="cDtFrom" EditFormatString="dd-MM-yyyy"
                                                                        EditFormat="Custom" UseMaskBehavior="True" Width="108px">
                                                                        <DropDownButton Text="From" />
                                                                        <%-- <clientsideevents datechanged="function(s,e){DateChangeForFrom();}" />--%>
                                                                    </dxe:ASPxDateEdit>
                                                                </td>
                                                                <td id="TdTo">
                                                                    <dxe:ASPxDateEdit ID="DtTo" runat="server" ClientInstanceName="cDtTo" EditFormatString="dd-MM-yyyy"
                                                                        EditFormat="Custom" UseMaskBehavior="True" Width="108px">
                                                                        <DropDownButton Text="To" />
                                                                        <%-- <clientsideevents datechanged="function(s,e){DateChangeForTo();}" />--%>
                                                                    </dxe:ASPxDateEdit>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr id="trClient">
                                        <td class="gridcellleft" >Select Company
                                        </td>
                                        <td align="left">
                                            <table width="150px">
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="RadCompanyA" runat="server" Checked="True" GroupName="p1" onclick="AllSelct('a','C')" />
                                                    </td>
                                                    <td>All
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="RadCompanyS" runat="server" GroupName="p1" onclick="AllSelct('b','C')" />
                                                    </td>
                                                    <td>Specific
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr id="trBranch">
                                        <td class="gridcellleft" >Select Branch
                                        </td>
                                        <td align="left">
                                            <table width="150px">
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="RadBranchA" runat="server" Checked="True" GroupName="s1" onclick="AllSelct('a','B')" />
                                                    </td>
                                                    <td>All
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="RadBranchS" runat="server" GroupName="s1" onclick="AllSelct('b','B')" />
                                                    </td>
                                                    <td>Specific
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr id="trReportTo">
                                        <td class="gridcellleft" >Select Report To
                                        </td>
                                        <td align="left">
                                            <table width="150px">
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="RadReportToA" runat="server" Checked="True" GroupName="h1" onclick="AllSelct('a','R')" />
                                                    </td>
                                                    <td>All
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="RadReportToS" runat="server" GroupName="h1" onclick="AllSelct('b','R')" />
                                                    </td>
                                                    <td>Specific
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr id="TrAccount">
                                        <td class="gridcellleft" style=" ">Select Employee Type
                                        </td>
                                        <td align="left">
                                            <table width="150px">
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="TadTypeA" runat="server" Checked="True" GroupName="b1" onclick="AllSelct('a','T')" />
                                                    </td>
                                                    <td>All
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="TadTypeS" runat="server" GroupName="b1" onclick="AllSelct('b','T')" />
                                                    </td>
                                                    <td>Selected
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class=" ">
                                            <dxe:ASPxButton ID="btnShow" runat="server" AutoPostBack="False" ClientInstanceName="cbtnAdd"
                                                Text="Show" Width="85px" CssClass="btn btn-primary">
                                                <ClientSideEvents Click="function(s,e){BtnShow_Click();}" />
                                            </dxe:ASPxButton>
                                        </td>
                                        <td align="left"></td>
                                    </tr>
                                </table>
                            </td>
                            <td style="vertical-align: top">
                                <table>
                                    <tr>
                                        <td class="gridcellleft" style="vertical-align: top; width: 313px; text-align: left">
                                            <table cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td id="TdFilter1" style="height: 23px">
                                                        <asp:DropDownList ID="cmbsearchOption" runat="server" Enabled="false" Font-Size="11px"
                                                            Width="85px">
                                                            <asp:ListItem>Employee</asp:ListItem>
                                                            <asp:ListItem>Company</asp:ListItem>
                                                            <asp:ListItem>Branch</asp:ListItem>
                                                            <asp:ListItem>ReportTo</asp:ListItem>
                                                            <asp:ListItem>Type</asp:ListItem>
                                                        </asp:DropDownList></td>
                                                </tr>
                                                <tr>
                                                    <td id="TdFilter" style="height: 23px">
                                                        <asp:TextBox ID="txtsubscriptionID" runat="server" Font-Size="12px" onkeyup="FunClientScrip(this,'ShowEmployeeByFilter',event)"
                                                            Width="253"></asp:TextBox><a href="javascript:void(0);" onclick="btnAddsubscriptionlist_click()"><span
                                                                style="font-size: 8pt; color: #009900; text-decoration: underline">Add to List</span></a><span
                                                                    style="font-size: 8pt; color: #009900">&nbsp;</span>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="vertical-align: top; width: 313px; text-align: left">
                                            <table id="TdSelect" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td style="padding-left: 7px">
                                                        <asp:ListBox ID="lstSuscriptions" runat="server" Font-Size="12px" Height="90px" Width="253px"></asp:ListBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: center">
                                                        <table cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td>
                                                                    <a href="javascript:void(0);" onclick="clientselectionfinal()"><span style="font-size: 10pt; color: #000099; text-decoration: underline">Done</span></a> &nbsp;
                                                                </td>
                                                                <td>
                                                                    <a href="javascript:void(0);" onclick="btnRemovefromsubscriptionlist_click()"><span
                                                                        style="font-size: 8pt; color: #cc3300; text-decoration: underline">Remove</span></a>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr style="display: none">
                <td>
                    <asp:TextBox ID="txtsubscriptionID_hidden" runat="server" Font-Size="12px" Width="1px"></asp:TextBox>
                </td>
            </tr>
        </table>
        <br />
        <table border="1" style="width: 60%">
            <tr>        
                <td valign="middle" style="">
                    <table width="100%">
                        <tr>
                            <td>Page Of ( <b id="B_TotalRows" runat="server" style="text-align: right"></b>&nbsp;items )</td>
                            <td style="vertical-align: middle;  text-align: left" valign="top">
                                <a id="A_LeftNav" runat="server" href="javascript:void(0);" onclick="OnLeftNav_Click()">
                                    <img src="/assests/images/LeftNav.gif" alt="" width="10" />
                                </a>
                            </td>
                            <td style="vertical-align: middle; text-align: left" valign="top">
                                <a id="A1" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A1')">1</a>
                            </td>
                            <td style="vertical-align: middle;text-align: left" valign="top">
                                <a id="A2" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A2')">2</a>
                            </td>
                            <td style="vertical-align: middle; text-align: left" valign="top">
                                <a id="A3" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A3')">3</a>
                            </td>
                            <td style="vertical-align: middle;  text-align: left" valign="top">
                                <a id="A4" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A4')">4</a>
                            </td>
                            <td style="vertical-align: middle; text-align: left" valign="top">
                                <a id="A5" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A5')">5</a>
                            </td>
                            <td style="vertical-align: middle;  text-align: left" valign="top">
                                <a id="A6" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A6')">6</a>
                            </td>
                            <td style="vertical-align: middle;  text-align: left" valign="top">
                                <a id="A7" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A7')">7</a>
                            </td>
                            <td style="vertical-align: middle;  text-align: left" valign="top">
                                <a id="A8" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A8')">8</a>
                            </td>
                            <td style="vertical-align: middle;  text-align: left" valign="top">
                                <a id="A9" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A9')">9</a>
                            </td>
                            <td style="vertical-align: middle;  text-align: left" valign="top">
                                <a id="A10" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A10')">10</a>
                            </td>
                            <td style="vertical-align: middle; text-align: right" valign="top">
                                <a id="A_RightNav" runat="server" href="javascript:void(0);" onclick="OnRightNav_Click()">
                                    <img src="../images/RightNav.gif" width="10" alt="" />
                                </a>
                            </td>
                            <td style="vertical-align: middle;  text-align: right" valign="top">
                                <asp:DropDownList ID="ddlExport" runat="server" Font-Size="12px" Onchange="ddlExport_OnChange()"
                                    Width="100px">
                                    <asp:ListItem Selected="True" Value="Ex">Export</asp:ListItem>
                                    <asp:ListItem Value="1">Excel</asp:ListItem>
                                </asp:DropDownList>&nbsp;
                            </td>
                            <td id="TdShowFilter" style="display: none; vertical-align: middle;  text-align: right"
                                valign="top">
                                <dxe:ASPxButton ID="btnShowFilter" runat="server" AutoPostBack="False" ClientInstanceName="cbtnShowFilter"
                                    Text="Show Filter" Width="85px">
                                    <ClientSideEvents Click="function(s,e){BtnShowFilter_Click();}" />
                                </dxe:ASPxButton>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <br />
        <dxe:ASPxGridView ID="GrdEmployee" runat="server" AutoGenerateColumns="False" ClientInstanceName="cGrdEmployee"
            KeyFieldName="cnt_id" OnCustomCallback="GrdEmployee_CustomCallback" OnProcessColumnAutoFilter="GrdEmployee_ProcessColumnAutoFilter"
            Width="100%">
            <ClientSideEvents EndCallback="function(s, e) {GrdEmployee_EndCallBack();}" />
            <SettingsBehavior AllowFocusedRow="True" AutoFilterRowInputDelay="1200"
                ConfirmDelete="True" AllowMultiSelection="True" />
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
                <dxe:GridViewDataTextColumn Caption="Srl." FieldName="SRLNO" FixedStyle="Left"
                    VisibleIndex="1" Width="20px">
                    <CellStyle CssClass="gridcellleft" Wrap="False">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Name" FieldName="Name" FixedStyle="Left" VisibleIndex="2"
                    Width="150px">
                    <CellStyle CssClass="gridcellleft" Wrap="False">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="EmpCode" FieldName="EmpCode" FixedStyle="Left" VisibleIndex="3"
                    Width="150px">
                    <CellStyle CssClass="gridcellleft" Wrap="False">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Father's Name" FieldName="FatherName" VisibleIndex="4"
                    Width="150px">
                    <CellStyle CssClass="gridcellleft" Wrap="False">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="DOB" FieldName="DOB" VisibleIndex="5"
                    Width="75px">
                    <CellStyle CssClass="gridcellleft" Wrap="False">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="DOJ" FieldName="DOJ" VisibleIndex="6" Width="75px">
                    <CellStyle CssClass="gridcellleft" Wrap="False">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="DOL" FieldName="DOL" VisibleIndex="7" Width="75px">
                    <CellStyle CssClass="gridcellleft" Wrap="False">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Department" FieldName="Department" VisibleIndex="8"
                    Width="120px">
                    <CellStyle CssClass="gridcellleft" Wrap="False">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Branch" FieldName="BranchName" VisibleIndex="9"
                    Width="75px">
                    <CellStyle CssClass="gridcellleft" Wrap="False">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="CTC" FieldName="CTC" VisibleIndex="10" Width="75px">
                    <CellStyle CssClass="gridcellleft" HorizontalAlign="Right" Wrap="False">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="ReportTo" FieldName="ReportTo" VisibleIndex="11"
                    Width="150px">
                    <CellStyle CssClass="gridcellleft" Wrap="False">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Designation" FieldName="Designation" VisibleIndex="12"
                    Width="150px">
                    <CellStyle CssClass="gridcellleft" Wrap="False">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Company" FieldName="Company" VisibleIndex="13"
                    Width="150px">
                    <CellStyle CssClass="gridcellleft" Wrap="False">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="EmpType" FieldName="EmpType" VisibleIndex="14"
                    Width="120px">
                    <CellStyle CssClass="gridcellleft" Wrap="False">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="EmailId(s)" FieldName="Email_Ids" VisibleIndex="15"
                    Width="150px">
                    <CellStyle CssClass="gridcellleft" Wrap="False">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Phone/Mobile" FieldName="PhoneMobile_Numbers"
                    VisibleIndex="16" Width="75px">
                    <CellStyle CssClass="gridcellleft" Wrap="False">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="PanCard" FieldName="PanCardNumber" VisibleIndex="17"
                    Width="75px">
                    <CellStyle CssClass="gridcellleft" Wrap="False">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Address" FieldName="Address" VisibleIndex="18"
                    Width="200px">
                    <CellStyle CssClass="gridcellleft" Wrap="False">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Ac[BankName][Branch][AcType]" FieldName="Bank" VisibleIndex="19"
                    Width="200px">
                    <CellStyle CssClass="gridcellleft" Wrap="False">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="CreatedBy" FieldName="CreatedBy" VisibleIndex="20">
                    <CellStyle CssClass="gridcellleft" Wrap="False">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="ContactID" FieldName="ContactID" Visible="False"
                    VisibleIndex="21" Width="75px">
                    <CellStyle CssClass="gridcellleft" Wrap="False">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewCommandColumn Visible="False" ShowDeleteButton="true">
                    <%-- <DeleteButton Text="Delete" Visible="True">
                    </DeleteButton>--%>
                </dxe:GridViewCommandColumn>

            </Columns>
            <SettingsCommandButton>
                <DeleteButton Text="Delete"></DeleteButton>
            </SettingsCommandButton>
            <SettingsEditing EditFormColumnCount="3" Mode="PopupEditForm" PopupEditFormHorizontalAlign="Center"
                PopupEditFormModal="True" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="900px" />
            <SettingsText ConfirmDelete="Are you sure to delete?" PopupEditFormCaption="Add/ Modify Employee" />
            <Settings ShowGroupPanel="True" ShowHorizontalScrollBar="True" ShowStatusBar="Hidden" />
            <SettingsLoadingPanel Text="Please Wait..." />
            <SettingsPager Visible="False">
            </SettingsPager>
            <StylesPager EnableDefaultAppearance="False">
            </StylesPager>
        </dxe:ASPxGridView>
        <asp:HiddenField ID="HDNEmployee" runat="server" />
        <asp:HiddenField ID="HDNBranch" runat="server" />
        <asp:HiddenField ID="HDNCompany" runat="server" />
        <asp:HiddenField ID="HDNReportTo" runat="server" />
        <asp:HiddenField ID="HDNType" runat="server" />
        <asp:HiddenField ID="hdn_GridBindOrNotBind" runat="server" />

        <!--Create HiddenFields For filter Option -->
        <asp:HiddenField ID="Hdn_PageSize" runat="server" />
        <asp:HiddenField ID="Hdn_PageNumber" runat="server" />
        <asp:HiddenField ID="Hdn_Emp_ContactID" runat="server" />
        <asp:HiddenField ID="Hdn_Emp_Status" runat="server" />
        <asp:HiddenField ID="Hdn_DateFrom" runat="server" />
        <asp:HiddenField ID="Hdn_DateTo" runat="server" />
        <asp:HiddenField ID="Hdn_Company" runat="server" />
        <asp:HiddenField ID="Hdn_Branch" runat="server" />
        <asp:HiddenField ID="Hdn_ReportTo" runat="server" />
        <asp:HiddenField ID="Hdn_EmployeeType" runat="server" />
        <asp:HiddenField ID="Hdn_DevXFilterOn" runat="server" />
        <asp:HiddenField ID="Hdn_DevXFilterString" runat="server" />
        <asp:HiddenField ID="Hdn_ReportType" runat="server" />
        <!--End Create HiddenFields For filter Option -->

        <asp:Button ID="BtnForExportEvent" runat="server" OnClick="cmbExport_SelectedIndexChanged" BackColor="#DDECFE" BorderStyle="None" />
    </div>
</asp:Content>

