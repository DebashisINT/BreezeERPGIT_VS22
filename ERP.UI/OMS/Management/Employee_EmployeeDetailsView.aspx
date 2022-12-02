<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.management_Employee_EmployeeDetailsView" CodeBehind="Employee_EmployeeDetailsView.aspx.cs" %>

<%--<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe000001" %>
<%@ Register Assembly="DevExpress.Web.v15.1" Namespace="DevExpress.Web"
    TagPrefix="dxe" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />

    <script type="text/javascript" src="/assests/js/loaddata1.js"></script>

    <script type="text/javascript" src="/assests/js/init.js"></script>

    <script type="text/javascript" src="/assests/js/ajaxList_wofolder.js"></script>

    <style type="text/css">
        /* Big box with list of options */
        #ajax_listOfOptions {
            position: absolute; /* Never change this one */
            width: 50px; /* Width of box */
            height: auto; /* Height of box */
            overflow: auto; /* Scrolling features */
            border: 1px solid Blue; /* Blue border */
            background-color: #FFF; /* White background color */
            text-align: left;
            font-size: 0.9em;
            z-index: 32767;
        }

            #ajax_listOfOptions div { /* General rule for both .optionDiv and .optionDivSelected */
                margin: 1px;
                padding: 1px;
                cursor: pointer;
                font-size: 0.9em;
            }

            #ajax_listOfOptions .optionDiv { /* Div for each item in list */
            }

            #ajax_listOfOptions .optionDivSelected { /* Selected item in the list */
                background-color: #DDECFE;
                color: Blue;
            }

        #ajax_listOfOptions_iframe {
            background-color: #F00;
            position: absolute;
            z-index: 3000;
        }

        form {
            display: inline;
        }
    </style>

    <script language="javascript" type="text/javascript">
            function height() {
                if (document.body.scrollHeight >= 380) {
                    window.frameElement.height = document.body.scrollHeight;
                }
                else {
                    window.frameElement.height = '380px';
                }
                window.frameElement.width = document.body.scrollWidth;
            }
            function SignOff() {
                window.parent.SignOff();
            }
            function ShowDate(obj) {
                if (document.getElementById('TdSelect').style.display == 'inline') {
                    clientselectionfinal();
                    document.getElementById('<%=RadAll.ClientID%>').checked = true;
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
function ShowGrid() {
    document.getElementById('TdGrid').style.display = 'inline';
    document.getElementById('HeaderGrid').style.display = 'inline';
    document.getElementById('TdExport').style.display = 'inline';
    document.getElementById('TrAll').style.display = 'none';
    document.getElementById('TrButton').style.display = 'none';
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
            for(i=listBoxSubs.options.length-1;i>=0;i--)
            {
                listBoxSubs.remove(i);
            }           	        
        }
        function FunClientScrip(objID,objListFun,objEvent)
        {
            var cmbVal;            
            if(document.getElementById('cmbsearchOption').value=="Employee")
            {
                cmbVal=document.getElementById('cmbsearchOption').value;       
                cmbVal=cmbVal+'~E';   
            }
            else if(document.getElementById('cmbsearchOption').value=="Company")
            {
                cmbVal=document.getElementById('cmbsearchOption').value;       
                cmbVal=cmbVal+'~C';            
            }
            else if(document.getElementById('cmbsearchOption').value=="Branch")
            {
                cmbVal=document.getElementById('cmbsearchOption').value;       
                cmbVal=cmbVal+'~B';            
            }
            else if(document.getElementById('cmbsearchOption').value=="ReportTo")
            {
                cmbVal=document.getElementById('cmbsearchOption').value;       
                cmbVal=cmbVal+'~R';            
            }
            else if(document.getElementById('cmbsearchOption').value=="Type")
            {
                cmbVal=document.getElementById('cmbsearchOption').value;       
                cmbVal=cmbVal+'~T';            
            }
            else
            {
                cmbVal=document.getElementById('cmbsearchOption').value;       
                cmbVal=cmbVal+'~N';
            }
            ajax_showOptions(objID,objListFun,objEvent,cmbVal);
        }	    
        function ReceiveServerData(rValue)
        {
            var Data=rValue.split('~');

            if(Data[0]=='Employee')
            {
                Employeevalue=Data[1];
                document.getElementById('HDNEmployee').value=Data[1];
            } 
            if(Data[0]=='Branch')
            {           
                Branchvalue=Data[1];                
                document.getElementById('HDNBranch').value=Data[1];
            }    
            if(Data[0]=='Company')
            {           
                Companyvalue=Data[1];                
                document.getElementById('HDNCompany').value=Data[1];
            }      
            if(Data[0]=='ReportTo')
            {           
                ReportTovalue=Data[1];                
                document.getElementById('HDNReportTo').value=Data[1];
            }                        
            if(Data[0]=='Type')
            {           
                Typevalue=Data[1];                
                document.getElementById('HDNType').value=Data[1];
            }
        }      
        function ShowHideFilter(obj)
        {
            grid.PerformCallback("Filter~"+obj);
        }    
        FieldName='lstSuscriptions';
        </script>

    <script type="text/javascript">
            function OnLeftNav_Click()
            {
                var i=document.getElementById("A1").innerText;
                if(parseInt(i)>1)
                {   
                    grid.PerformCallback("SearchByNavigation~"+document.getElementById("A1").innerText+"~LeftNav");
                }
                else
                {
                    alert('No More Pages.');
                }
            }
            function OnRightNav_Click()
            {
                var TestEnd=document.getElementById("A10").innerText;
                var TotalPage=document.getElementById("B_TotalPage").innerText;
                if(TestEnd=="" || TestEnd==TotalPage)
                {
                    alert('No More Records.');
                    return;
                }
                var i=document.getElementById("A1").innerText;
                if(parseInt(i)<TotalPage)
                {   
                    grid.PerformCallback("SearchByNavigation~"+document.getElementById("A1").innerText+"~RightNav");
                }
                else
                {
                    alert('You are at the End');
                }
            }
            function OnPageNo_Click(obj)
            {
                var i=document.getElementById(obj).innerText;
                grid.PerformCallback("SearchByNavigation~"+i+"~PageNav");
        
            }
            function EmployeeGrid_EndCallBack()
            {                 
                if(grid.cpRefreshNavPanel!=undefined)
                {
                    document.getElementById("B_PageNo").innerText='';
                    document.getElementById("B_TotalPage").innerText='';
                    document.getElementById("B_TotalRows").innerText='';
            
                    var NavDirection=grid.cpRefreshNavPanel.split('~')[0];
                    var PageNum=grid.cpRefreshNavPanel.split('~')[1];
                    var TotalPage=grid.cpRefreshNavPanel.split('~')[2];
                    var TotalRows=grid.cpRefreshNavPanel.split('~')[3];
            
                    if(NavDirection=="RightNav")
                    {
                        PageNum=parseInt(PageNum)+10;
                        document.getElementById("B_PageNo").innerText=PageNum;
                        document.getElementById("B_TotalPage").innerText=TotalPage;
                        document.getElementById("B_TotalRows").innerText=TotalRows;
                        var n=parseInt(TotalPage)-parseInt(PageNum)>10?parseInt(11):parseInt(TotalPage)-parseInt(PageNum)+2;
                        for(r=1;r<n;r++)
                        {
                            var obj="A"+r;
                            document.getElementById(obj).innerText=PageNum++;
                        }
                        for(r=n;r<11;r++)
                        {
                            var obj="A"+r;
                            document.getElementById(obj).innerText="";
                        }
                    }
                    if(NavDirection=="LeftNav")
                    {
                        if(parseInt(PageNum)>1)
                        {   
                            PageNum=parseInt(PageNum)-10;
                            document.getElementById("B_PageNo").innerText=PageNum;
                            document.getElementById("B_TotalPage").innerText=TotalPage;
                            document.getElementById("B_TotalRows").innerText=TotalRows;
                            for(l=1;l<11;l++)
                            {
                                var obj="A"+l;
                                document.getElementById(obj).innerText=PageNum++;
                            }
                        }
                        else
                        {
                            alert('No More Pages.');
                        }
                    }
                    if(NavDirection=="PageNav")
                    {
                        document.getElementById("B_PageNo").innerText=PageNum;
                        document.getElementById("B_TotalPage").innerText=TotalPage;
                        document.getElementById("B_TotalRows").innerText=TotalRows;
                    }
                    if(NavDirection=="ShowBtnClick")
                    {
                        document.getElementById("B_PageNo").innerText=PageNum;
                        document.getElementById("B_TotalPage").innerText=TotalPage;
                        document.getElementById("B_TotalRows").innerText=TotalRows;
                        for(l=1;l<11;l++)
                        {
                            var obj="A"+l;
                            document.getElementById(obj).innerText=PageNum++;
                        }
                    }
                }
                if (grid.cpIsEmptyDsSearch=="NoRecord")
                {               
                    alert('No Record Found');
                    Filter();
                }       
                height();
            }
            function btnShowOnclick()
            {
                ShowGrid();
                grid.PerformCallback("Show~"+document.getElementById("A1").innerText);
            }
            function NORECORD(obj)
            {        
                if(obj=='Excel')
                {
                    alert('No Record Found !! ');               
                }
                height();
            }
            function ddlExport_OnChange()
            {              
                var ddlExprtValue = document.getElementById("<%=ddlExport.ClientID%>").value;
         document.getElementById("hdnExportValue").value= ddlExprtValue;                
         if(ddlExprtValue!="Ex") 
         {
             document.getElementById('btnExport').click();                     
         }
     }   
     </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table class="TableMain100">
            <tr>
                <td class="EHEADER" style="text-align: center;">
                    <div style="float: right" id="TdExport">
                        <asp:DropDownList ID="ddlExport" runat="server" Font-Size="10" Height="19px" onchange="ddlExport_OnChange()">
                            <asp:ListItem Value="Ex" Selected="True">Export</asp:ListItem>
                            <asp:ListItem Value="E">Excel</asp:ListItem>
                            <%-- <asp:ListItem Value="P">PDF</asp:ListItem>--%>
                        </asp:DropDownList>
                        <a href="javascript:void(0);" onclick="Filter()"><span style="color: #009900; text-decoration: underline; font-size: 10pt;">Filter</span></a>
                    </div>
                    <strong><span style="color: #000099">Employee Details</span></strong>
                </td>
            </tr>
            <tr>
                <td>
                    <table>
                        <tr id="TrAll">
                            <td>
                                <table class="TableMain100">
                                    <tr>
                                        <td>
                                            <table cellspacing="1" cellpadding="2" style="background-color: #B7CEEC; border: solid 1px  #ffffff"
                                                border="1">
                                                <tr id="trEmployee">
                                                    <td class="gridcellleft">Select Employee
                                                    </td>
                                                    <td align="left">
                                                        <table>
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
                                                    <td class="gridcellleft">Employee Status :
                                                    </td>
                                                    <td class="gridcellleft">
                                                        <table cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td>
                                                                    <asp:RadioButton ID="RadAll" runat="server" GroupName="k1" Checked="true" onclick="ShowDate('a')" /></td>
                                                                <td class="gridcellleft">Active Employee
                                                                </td>
                                                                <td>
                                                                    <asp:RadioButton ID="RadActive" runat="server" GroupName="k1" onclick="ShowDate('b')" /></td>
                                                                <td class="gridcellleft">Ex Employee
                                                                </td>
                                                                <td>
                                                                    <asp:RadioButton ID="RadClosed" runat="server" GroupName="k1" onclick="ShowDate('c')" /></td>
                                                                <td class="gridcellleft">Both
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr id="TrDate">
                                                    <td class="gridcellleft">
                                                        <div id="divAsOnDate">
                                                        </div>
                                                    </td>
                                                    <td align="left">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <table cellpadding="0" cellspacing="0">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:RadioButton ID="RadDateRangeA" runat="server" GroupName="G1" Checked="true"
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
                                                                <td id="trDateRange">
                                                                    <table>
                                                                        <tr>
                                                                            <td id="TdFrom">
                                                                                <dxe:ASPxDateEdit ID="dtFrom" runat="server" EditFormat="Custom" ClientInstanceName="dtFrom"
                                                                                    UseMaskBehavior="True" Width="108px">
                                                                                    <DropDownButton Text="From">
                                                                                    </DropDownButton>
                                                                                    <%-- <clientsideevents datechanged="function(s,e){DateChangeForFrom();}" />--%>
                                                                                </dxe:ASPxDateEdit>
                                                                            </td>
                                                                            <td id="TdTo">
                                                                                <dxe:ASPxDateEdit ID="dtTo" runat="server" EditFormat="Custom" ClientInstanceName="dtToDate"
                                                                                    UseMaskBehavior="True" Width="108px">
                                                                                    <DropDownButton Text="To">
                                                                                    </DropDownButton>
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
                                                    <td class="gridcellleft">Select Company
                                                    </td>
                                                    <td align="left">
                                                        <table>
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
                                                    <td class="gridcellleft">Select Branch
                                                    </td>
                                                    <td align="left">
                                                        <table>
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
                                                    <td class="gridcellleft">Select Report To
                                                    </td>
                                                    <td align="left">
                                                        <table>
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
                                                    <td class="gridcellleft">Select Employee Type
                                                    </td>
                                                    <td align="left">
                                                        <table>
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
                                            </table>
                                        </td>
                                        <td style="vertical-align: top">
                                            <table>
                                                <tr>
                                                    <td class="gridcellleft" style="vertical-align: top; text-align: left; width: 313px;">
                                                        <table cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td id="TdFilter1" style="height: 23px">
                                                                    <asp:DropDownList ID="cmbsearchOption" runat="server" Width="85px" Font-Size="11px"
                                                                        Enabled="false">
                                                                        <asp:ListItem>Employee</asp:ListItem>
                                                                        <asp:ListItem>Company</asp:ListItem>
                                                                        <asp:ListItem>Branch</asp:ListItem>
                                                                        <asp:ListItem>ReportTo</asp:ListItem>
                                                                        <asp:ListItem>Type</asp:ListItem>
                                                                    </asp:DropDownList></td>
                                                            </tr>
                                                            <tr>
                                                                <td id="TdFilter" style="height: 23px">
                                                                    <asp:TextBox ID="txtsubscriptionID" runat="server" Font-Size="12px" Width="253" onkeyup="FunClientScrip(this,'ShowEmployeeByFilter',event)"></asp:TextBox><a
                                                                        href="javascript:void(0);" onclick="btnAddsubscriptionlist_click()"><span style="color: #009900; text-decoration: underline; font-size: 8pt;">Add to List</span></a><span style="color: #009900; font-size: 8pt;">&nbsp;</span>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: left; vertical-align: top; width: 313px;">
                                                        <table cellpadding="0" cellspacing="0" id="TdSelect">
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
                                                                                <a href="javascript:void(0);" onclick="clientselectionfinal()"><span style="color: #000099; text-decoration: underline; font-size: 10pt;">Done</span></a>&nbsp;&nbsp;
                                                                            </td>
                                                                            <td>
                                                                                <a href="javascript:void(0);" onclick="btnRemovefromsubscriptionlist_click()"><span
                                                                                    style="color: #cc3300; text-decoration: underline; font-size: 8pt;">Remove</span></a>
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
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table>
                        <tr id="TrButton">
                            <td id="TrBtn">
                                <dxe:ASPxButton ID="btnShow" runat="server" AutoPostBack="False" ClientInstanceName="cbtnAdd"
                                    Text="Show" Width="85px">
                                    <ClientSideEvents Click="function(s,e){btnShowOnclick();}" />
                                </dxe:ASPxButton>
                            </td>
                            <td style="display: none;">
                                <asp:Button ID="btnPrint" Text="Print" runat="server" CssClass="btnUpdate" OnClick="btnPrint_Click"
                                    TabIndex="6" />
                            </td>
                            <td style="display: none">
                                <asp:TextBox ID="txtsubscriptionID_hidden" runat="server" Font-Size="12px" Width="1px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="TdGrid" style="background-color: #DDECFE;">
                <td style="background-color: #DDECFE;">
                    <table style="background-color: #DDECFE;" class="TableMain100">
                        <tr>
                            <td id="HeaderGrid" style="height: 10px; font-size: 12px; font-weight: bold;">
                                <asp:Label ID="lblReportHeader" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; vertical-align: top">
                                <table>
                                    <tr>
                                        <td id="ShowFilter">
                                            <a href="javascript:ShowHideFilter('s');"><span style="color: #000099; text-decoration: underline">Show Filter</span></a>
                                        </td>
                                        <td id="Td1">
                                            <a href="javascript:ShowHideFilter('All');"><span style="color: #000099; text-decoration: underline">All Records</span></a>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table style="width: 60%" border="1">
                                    <tr>
                                        <td valign="top" style="vertical-align: top; width: 34px; height: 11px; background-color: #b7ceec; text-align: left">Page</td>
                                        <td valign="top" style="width: 4px">
                                            <b style="text-align: right" id="B_PageNo" runat="server"></b>
                                        </td>
                                        <td valign="top" style="vertical-align: top; height: 11px; background-color: #b7ceec; text-align: left;">Of
                                        </td>
                                        <td valign="top">
                                            <b style="text-align: right" id="B_TotalPage" runat="server"></b>
                                        </td>
                                        <td valign="top" style="vertical-align: top; height: 11px; background-color: #b7ceec; text-align: left">( <b style="text-align: right" id="B_TotalRows" runat="server"></b>&nbsp;items )
                                        </td>
                                        <td valign="top">
                                            <table width="100%">
                                                <tr>
                                                    <td valign="top" style="vertical-align: top; height: 11px; background-color: #b7ceec; text-align: left">
                                                        <a id="A_LeftNav" runat="server" href="javascript:void(0);" onclick="OnLeftNav_Click()">
                                                            <img src="/assests/images/LeftNav.gif" width="10" />
                                                        </a>
                                                    </td>
                                                    <td valign="top" style="vertical-align: top; height: 11px; background-color: #b7ceec; text-align: left">
                                                        <a id="A1" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A1')">1</a>
                                                    </td>
                                                    <td valign="top" style="vertical-align: top; height: 11px; background-color: #b7ceec; text-align: left">
                                                        <a id="A2" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A2')">2</a>
                                                    </td>
                                                    <td valign="top" style="vertical-align: top; height: 11px; background-color: #b7ceec; text-align: left">
                                                        <a id="A3" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A3')">3</a>
                                                    </td>
                                                    <td valign="top" style="vertical-align: top; height: 11px; background-color: #b7ceec; text-align: left">
                                                        <a id="A4" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A4')">4</a>
                                                    </td>
                                                    <td valign="top" style="vertical-align: top; height: 11px; background-color: #b7ceec; text-align: left">
                                                        <a id="A5" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A5')">5</a>
                                                    </td>
                                                    <td valign="top" style="vertical-align: top; height: 11px; background-color: #b7ceec; text-align: left">
                                                        <a id="A6" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A6')">6</a>
                                                    </td>
                                                    <td valign="top" style="vertical-align: top; height: 11px; background-color: #b7ceec; text-align: left">
                                                        <a id="A7" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A7')">7</a>
                                                    </td>
                                                    <td valign="top" style="vertical-align: top; height: 11px; background-color: #b7ceec; text-align: left">
                                                        <a id="A8" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A8')">8</a>
                                                    </td>
                                                    <td valign="top" style="vertical-align: top; height: 11px; background-color: #b7ceec; text-align: left">
                                                        <a id="A9" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A9')">9</a>
                                                    </td>
                                                    <td valign="top" style="vertical-align: top; height: 11px; background-color: #b7ceec; text-align: left">
                                                        <a id="A10" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A10')">10</a>
                                                    </td>
                                                    <td style="text-align: right; vertical-align: top; height: 11px; background-color: #b7ceec;"
                                                        valign="top">
                                                        <a id="A_RightNav" runat="server" href="javascript:void(0);" onclick="OnRightNav_Click()">
                                                            <img src="../images/RightNav.gif" width="10" />
                                                        </a>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <div style="width: 970px; overflow: scroll;">
                                    <dxe:ASPxGridView ID="EmployeeGrid" runat="server" KeyFieldName="cnt_shortName"
                                        AutoGenerateColumns="False" Width="100%" ClientInstanceName="grid" OnCustomCallback="EmployeeGrid_CustomCallback">
                                        <ClientSideEvents EndCallback="function(s, e) {EmployeeGrid_EndCallBack();}" />
                                        <SettingsBehavior AllowFocusedRow="True" ConfirmDelete="True" />
                                        <Styles>
                                            <Header SortingImageSpacing="5px" ImageSpacing="5px">
                                            </Header>
                                            <FocusedRow BackColor="#FFC0C0" Cursor="auto">
                                            </FocusedRow>
                                            <LoadingPanel ImageSpacing="10px">
                                            </LoadingPanel>
                                            <Row Wrap="False">
                                            </Row>
                                            <FocusedGroupRow BackColor="#FFC0C0">
                                            </FocusedGroupRow>
                                        </Styles>
                                        <SettingsPager AlwaysShowPager="False" PageSize="10" ShowSeparators="True">
                                        </SettingsPager>
                                        <Columns>
                                            <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="0" FieldName="Name" Width="250px">
                                                <CellStyle Wrap="False" CssClass="gridcellleft">
                                                </CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="cnt_shortName" Width="75px"
                                                Caption="Employee Code">
                                                <CellStyle Wrap="False" CssClass="gridcellleft">
                                                </CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="CompName" Width="150px"
                                                Caption="Company Name">
                                                <CellStyle Wrap="False" HorizontalAlign="Left">
                                                </CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="BranchName" Width="150px"
                                                Caption="Branch">
                                                <CellStyle Wrap="False" CssClass="gridcellleft">
                                                </CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="4" FieldName="DOJ" Width="75px"
                                                Caption="Date Of Joining">
                                                <CellStyle Wrap="False" CssClass="gridcellleft">
                                                </CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="DateOfLeaving" Width="150px"
                                                Caption="Date Of Leaving">
                                                <CellStyle Wrap="False" CssClass="gridcellleft">
                                                </CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="cost_description" Width="150px"
                                                Caption="Department">
                                                <CellStyle Wrap="False" CssClass="gridcellleft">
                                                </CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="7" FieldName="deg_designation"
                                                Width="75px" Caption="Designation">
                                                <CellStyle Wrap="False" CssClass="gridcellleft">
                                                </CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn VisibleIndex="8" FieldName="EmpTypeName" Width="150px"
                                                Caption="Employee Type">
                                                <CellStyle Wrap="False" CssClass="gridcellleft">
                                                </CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn VisibleIndex="9" FieldName="ReportTo" Width="150px"
                                                Caption="Report To">
                                                <CellStyle Wrap="False" CssClass="gridcellleft">
                                                </CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="10" FieldName="emp_currentCTC"
                                                Width="80px" Caption="Current CTC">
                                                <CellStyle Wrap="False" CssClass="gridcellleft">
                                                </CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="11" FieldName="phone"
                                                Width="80px" Caption="Phone">
                                                <CellStyle Wrap="False" CssClass="gridcellleft">
                                                </CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="12" FieldName="Email"
                                                Width="80px" Caption="Email">
                                                <CellStyle Wrap="False" CssClass="gridcellleft">
                                                </CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="13" FieldName="PanCard"
                                                Width="80px" Caption="PAN">
                                                <CellStyle Wrap="False" CssClass="gridcellleft">
                                                </CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="14" FieldName="ContactAddress"
                                                Width="250px" Caption="Address">
                                                <CellStyle Wrap="False" CssClass="gridcellleft">
                                                </CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataDateColumn ReadOnly="True" VisibleIndex="15" FieldName="FatherName"
                                                Width="150px" Caption="Father Name">
                                                <CellStyle Wrap="False" CssClass="gridcellleft">
                                                </CellStyle>
                                            </dxe:GridViewDataDateColumn>
                                            <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="16" FieldName="DOB" Width="150px"
                                                Caption="Date Of Birth">
                                                <CellStyle Wrap="False" CssClass="gridcellleft">
                                                </CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="17" FieldName="BankName"
                                                Width="75px" Caption="Bank Name">
                                                <CellStyle Wrap="False" CssClass="gridcellleft">
                                                </CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="18" FieldName="BranchAdres"
                                                Width="75px" Caption="Branch Address">
                                                <CellStyle Wrap="False" CssClass="gridcellleft">
                                                </CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="19" FieldName="AccountType"
                                                Width="75px" Caption="Account type">
                                                <CellStyle Wrap="False" CssClass="gridcellleft">
                                                </CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="20" FieldName="AccountNumber"
                                                Width="75px" Caption="Account Number">
                                                <CellStyle Wrap="False" CssClass="gridcellleft">
                                                </CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="21" FieldName="CREATEDBY"
                                                Width="75px" Caption="Created By">
                                                <CellStyle Wrap="False" CssClass="gridcellleft">
                                                </CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="22" FieldName="CMPID"
                                                Width="75px" Caption="Company ID">
                                                <CellStyle Wrap="False" CssClass="gridcellleft">
                                                </CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                        </Columns>
                                    </dxe:ASPxGridView>
                                    <dxe:ASPxGridViewExporter ID="exporter" runat="server">
                                    </dxe:ASPxGridViewExporter>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="display: none;">
                    <asp:HiddenField ID="HDNEmployee" runat="server" />
                    <asp:HiddenField ID="HDNBranch" runat="server" />
                    <asp:HiddenField ID="HDNCompany" runat="server" />
                    <asp:HiddenField ID="HDNReportTo" runat="server" />
                    <asp:HiddenField ID="HDNType" runat="server" />
                    <asp:HiddenField ID="hdnExportValue" runat="server" />
                    <asp:Button ID="btnExport" runat="server" BackColor="#DDECFE" BorderStyle="None"
                        OnClick="btnExport_Click" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
