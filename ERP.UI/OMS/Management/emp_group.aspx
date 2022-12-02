<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.Reports_emp_group" CodeBehind="emp_group.aspx.cs" %>

<%--<%@ Register Assembly="DevExpress.Web.v15.1" Namespace="DevExpress.Web"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1" Namespace="DevExpress.Web"
    TagPrefix="dxe000001" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="/assests/js/loaddata1.js"></script>

    <script type="text/javascript" src="/assests/js/init.js"></script>

    <script type="text/javascript" src="/assests/js/ajax-dynamic-list_wofolder.js"></script>

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


    <script type="text/javascript" language="javascript">


        function height() {
            if (document.body.scrollHeight >= 500) {
                window.frameElement.height = document.body.scrollHeight;
            }
            else
                window.frameElement.height = '500px';
            window.frameElement.Width = document.body.scrollWidth;
        }
        function SignOff() {
            window.parent.SignOff();
        }
        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }
        function OnMoreInfoClick(keyValue) {
            var url = 'employee_general.aspx?id=' + keyValue;
            OnMoreInfoClick(url, "Modify Employee Details", '980px', '550px', "Y");
        }
        function OnAddButtonClick() {
            var url = 'employee_general.aspx?id=' + 'ADD';
            OnMoreInfoClick(url, "Add Employee Details", '980px', '550px', "Y");


        }
        function callback() {
            grid.PerformCallback();
        }
        function OnContactInfoClick(keyValue, CompName) {
            var url = 'insurance_contactPerson.aspx?id=' + keyValue;
            OnMoreInfoClick(url, "Employee Name : " + CompName + "", '980px', '550px', "Y");
        }

        FieldName = 'lstSuscriptions';


    </script>

    <script language="javascript" type="text/javascript">
        function PageLoad() {

            document.getElementById("Trfilter").style.display = 'none';
            height();
        }

        function showOptions(obj1, obj2, obj3) {

            var cmb = document.getElementById("cmbsearchOption");
            var obj4 = cmb.value;
            if (cmb.value == 'Clients') {
                if (document.getElementById("rdbbSelected").checked == true) {
                    if (document.getElementById("hdBranch").value != '')
                        obj4 += '~' + document.getElementById("hdBranch").value;
                    else
                        document.getElementById("rdbbAll").checked = true;
                }
            }
            else if (cmb.value == 'Products') {
                if (document.getElementById("rdbInsuCompS").checked == true) {
                    if (document.getElementById("hdCompany").value != '')
                        obj4 += '~' + document.getElementById("hdCompany").value;
                    else
                        document.getElementById("rdbInsuCompA").checked = true;
                }
            }
            //            else if(cmb.value=='Company')
            //            {
            //                if(document.getElementById("cmbReportType").value=='L')
            //                    obj4+='~'+'Insurance-Life';
            //                else if(document.getElementById("cmbReportType").value=='G')
            //                    obj4+='~'+'Insurance-General';
            //                else if(document.getElementById("cmbReportType").value=='B')
            //                    obj4+='~'+'Insurance-Both';
            //            }

            ajax_showOptions(obj1, obj2, obj3, obj4);
        }
        function ShowBankName(obj1, obj2, obj3) {
            ajax_showOptions(obj1, obj2, obj3);
        }
        function height() {
            if (document.body.scrollHeight >= 350) {
                window.frameElement.height = document.body.scrollHeight;
            }
            else {
                window.frameElement.height = '350px';
            }
            //window.frameElement.width = document.body.scrollWidth;
        }
        function btnAddsubscriptionlist_click() {
            var userid = document.getElementById('txtsubscriptionID');
            if (userid.value != '') {
                var ids = document.getElementById('txtsubscriptionID_hidden');
                var listBox = document.getElementById('lstSuscriptions');
                var tLength = listBox.length;
                //alert(tLength);

                var no = new Option();
                no.value = ids.value;
                no.text = userid.value;
                listBox[tLength] = no;
                var recipient = document.getElementById('txtsubscriptionID');
                recipient.value = '';
            }
            else
                alert('Please search name and then Add!');
            FocusFiter();
        }
        function FocusFiter() {
            var s = document.getElementById('txtsubscriptionID');
            s.focus();
            s.select();
        }
        function Focus(obj) {
            var s = document.getElementById(obj);
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

        function SegAll(obj) {
            document.getElementById('showFilter').style.display = 'none';
        }
        function SegSelected(obj) {
            document.getElementById('showFilter').style.display = 'inline';
            document.getElementById('cmbsearchOption').value = obj;
            FocusFiter();
        }
        function ShowGrid(obj) {
            if (obj > 0) {
                document.getElementById("TrAll").style.display = 'none';
                document.getElementById("Trfilter").style.display = 'inline';
            }
            else
                alert('No data Found!');
            height();

        }
        function filter() {
            document.getElementById("Trfilter").style.display = 'none';
            document.getElementById("TrAll").style.display = 'inline';
            height();
        }


        function clientselectionfinal() {
            var listBoxSubs = document.getElementById('lstSuscriptions');
            var cmb = document.getElementById('cmbsearchOption');
            var listIDs = '';
            var i;
            var DataToHidden = '';
            if (listBoxSubs.length > 0) {
                for (i = 0; i < listBoxSubs.length; i++) {
                    if (listIDs == '') {
                        listIDs = listBoxSubs.options[i].value + ';' + listBoxSubs.options[i].text;
                        DataToHidden = '\'' + listBoxSubs.options[i].value + '\'';
                    }
                    else {
                        listIDs += ',' + listBoxSubs.options[i].value + ';' + listBoxSubs.options[i].text;
                        DataToHidden += ',\'' + listBoxSubs.options[i].value + '\'';
                    }
                }
                //                if(cmb.value=='Company')
                //                {
                //                    document.getElementById("hdCompany").value=DataToHidden;
                //                }
                //                if(cmb.value=='Branch')
                //                {
                //                    document.getElementById("hdBranch").value=DataToHidden;
                //                }
                var sendData = cmb.value + '~' + listIDs;
                CallServer(sendData, "");
            }
            var i;
            for (i = listBoxSubs.options.length - 1; i >= 0; i--) {
                listBoxSubs.remove(i);
            }
            document.getElementById('showFilter').style.display = 'none';
            document.getElementById('Button1').disabled = false;
        }
        function FillValues() {
            var btn = document.getElementById('Button1');
            btn.click();
        }
        //         document.body.style.cursor = 'pointer'; 
        var oldColor = '';
        function ChangeRowColor(rowID, length) {
            //alert(rowID);
            var gridview = document.getElementById('grdCashBankBook');
            var rCount = gridview.rows.length;
            var rowIndex = 1;
            var len;
            if (length > 25)
                len = 3;
            else
                len = 2;
            for (rowIndex; rowIndex <= rCount - len; rowIndex++) {
                var rowElement = gridview.rows[rowIndex];
                rowElement.style.backgroundColor = '#FFFFFF'
            }
            var color = document.getElementById(rowID).style.backgroundColor;
            if (color != '#ffe1ac') {
                oldColor = color;
            }
            if (color == '#ffe1ac') {
                document.getElementById(rowID).style.backgroundColor = oldColor;
            }
            else
                document.getElementById(rowID).style.backgroundColor = '#ffe1ac';

        }
        FieldName = 'lstSuscriptions';
    </script>

    <script type="text/javascript">
        function ReceiveServerData(rValue) {
            var Data = rValue.split('~');

            if (Data[0] == 'Branch') {
                var combo = document.getElementById('litBranch');
                var NoItems = Data[1].split(',');
                var i;
                var val = '';
                for (i = 0; i < NoItems.length; i++) {
                    var items = NoItems[i].split(';');
                    //var items1=items[1].split('-');
                    if (val == '') {
                        val = '(' + items[1];
                    }
                    else {
                        val += ',' + items[1];
                    }
                }
                val = val + ')';
                combo.innerText = val;
                //                Focus('rdbClientA');
            }
            //            if(Data[0]=='Clients')
            //            {
            //                var combo = document.getElementById('spnClient');
            //                var NoItems=Data[1].split(',');
            //                var i;
            //                var val='';
            //                for(i=0;i<NoItems.length;i++)
            //                {
            //                    var items = NoItems[i].split(';');
            //                    if(val=='')
            //                    {
            //                        val=items[1];
            //                    }
            //                    else
            //                    {
            //                        val+=','+items[1];
            //                    }
            //                }
            //                combo.innerText=val;
            //                Focus('rdbInsuCompA');
            //            }
            if (Data[0] == 'InsuComp') {
                var combo = document.getElementById('spnInsuComp');
                var NoItems = Data[1].split(',');
                var i;
                var val = '';
                for (i = 0; i < NoItems.length; i++) {
                    var items = NoItems[i].split(';');
                    if (val == '') {
                        val = items[1];
                    }
                    else {
                        val += ',' + items[1];
                    }
                }
                combo.innerText = val;
                //                Focus('rdbProductA');
            }
            //            if(Data[0]=='InsuComp')
            //            {
            //                var combo = document.getElementById('spnInsuComp');
            //                var NoItems=Data[1].split(',');
            //                var i;
            //                var val='';
            //                for(i=0;i<NoItems.length;i++)
            //                {
            //                    var items = NoItems[i].split(';');
            //                    if(val=='')
            //                    {
            //                        val=items[1];
            //                    }
            //                    else
            //                    {
            //                        val+=','+items[1];
            //                    }
            //                }
            //                combo.innerText=val;
            //                Focus('rdbProductA');
            //            }
            //            if(Data[0]=='Products')
            //            {
            //                var combo = document.getElementById('spnProduct');
            //                var NoItems=Data[1].split(',');
            //                var i;
            //                var val='';
            //                for(i=0;i<NoItems.length;i++)
            //                {
            //                    var items = NoItems[i].split(';');
            //                    if(val=='')
            //                    {
            //                        val=items[1];
            //                    }
            //                    else
            //                    {
            //                        val+=','+items[1];
            //                    }
            //                }
            //                combo.innerText=val;
            //                Focus('rdbTelecallerA');
            //            }
            if (Data[0] == 'ReportTo') {
                var combo = document.getElementById('spnTeleCaller');
                var NoItems = Data[1].split(',');
                var i;
                var val = '';
                for (i = 0; i < NoItems.length; i++) {
                    var items = NoItems[i].split(';');
                    if (val == '') {
                        val = items[1];
                    }
                    else {
                        val += ',' + items[1];
                    }
                }
                combo.innerText = val;
                Focus('rdbSalesRepresentativeA');
            }
            if (Data[0] == 'SaleRep') {
                var combo = document.getElementById('spnSalesRepresentative');
                var NoItems = Data[1].split(',');
                var i;
                var val = '';
                for (i = 0; i < NoItems.length; i++) {
                    var items = NoItems[i].split(';');
                    if (val == '') {
                        val = items[1];
                    }
                    else {
                        val += ',' + items[1];
                    }
                }
                combo.innerText = val;
                //                Focus('rdbAssociateA');
            }
            //            if(Data[0]=='Associate')
            //            {
            //                var combo = document.getElementById('spnAssociate');
            //                var NoItems=Data[1].split(',');
            //                var i;
            //                var val='';
            //                for(i=0;i<NoItems.length;i++)
            //                {
            //                    var items = NoItems[i].split(';');
            //                    if(val=='')
            //                    {
            //                        val=items[1];
            //                    }
            //                    else
            //                    {
            //                        val+=','+items[1];
            //                    }
            //                }
            //                combo.innerText=val;
            //                Focus('rdbSubBroakerA');
            //            }
            //            if(Data[0]=='SubBroker')
            //            {
            //                var combo = document.getElementById('spnSubBroaker');
            //                var NoItems=Data[1].split(',');
            //                var i;
            //                var val='';
            //                for(i=0;i<NoItems.length;i++)
            //                {
            //                    var items = NoItems[i].split(';');
            //                    if(val=='')
            //                    {
            //                        val=items[1];
            //                    }
            //                    else
            //                    {
            //                        val+=','+items[1];
            //                    }
            //                }
            //                combo.innerText=val;
            //                Focus('drpPolicyStatus');
            //            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Branchwise Employee Details</h3>
        </div>

    </div> 
 <div class="form_main">
        <table class="TableMain100">
           
            <tr>
                <td>
                    <table>
                        <tr id="TrAll">
                            <td style="text-align: left; vertical-align: top">
                                <table>
                                    <%--                        <tr>
                            <td class="gridcellleft">
                                <strong><span >Report Type:</span></strong>
                            </td>
                            <td class="gridcellleft" colspan="2">
                                <asp:DropDownList ID="cmbReportType" runat="server" Font-Size="12px" TabIndex="1">
                                    <asp:ListItem Selected="True" Value="L">LI</asp:ListItem>
                                    <asp:ListItem Value="G">GI</asp:ListItem>
                                    <asp:ListItem Value="B">Both</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>--%>
                                    <tr>
                                        <td style="width:100px">
                                            <strong><span >Joining Date:</span></strong>
                                        </td>
                                        <td colspan="2" class="gridcellleft">
                                            <table cellspacing="0px">
                                                <tr>
                                                    <td style="padding-right:20px">
                                                        <dxe:ASPxDateEdit ID="dtDate" ClientInstanceName="dtDate" runat="server" EditFormat="Custom"
                                                            UseMaskBehavior="True" TabIndex="1" Width="135px">
                                                            <DropDownButton Text="From ">
                                                            </DropDownButton>
                                                        </dxe:ASPxDateEdit>
                                                    </td>
                                                    <td class="gridcellleft">
                                                        <dxe:ASPxDateEdit ID="dtToDate" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                            TabIndex="2" Width="135px">
                                                            <DropDownButton Text="To">
                                                            </DropDownButton>
                                                        </dxe:ASPxDateEdit>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="gridcellleft">
                                            <strong><span >Company:</span></strong>
                                        </td>
                                        <td style="text-align: left;" colspan="2">
                                            <table width="150px">
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="rdbInsuCompA" runat="server" Checked="True" GroupName="i" TabIndex="5" />
                                                    </td>
                                                    <td>All
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rdbInsuCompS" runat="server" GroupName="i" />
                                                    </td>
                                                    <td>Selected
                                                    </td>
                                                    <td>
                                                        <span id="spnInsuComp" runat="server" style="color: Maroon"></span>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="gridcellleft">
                                            <strong><span >Branch:</span></strong>
                                        </td>
                                        <td style="text-align: left;" colspan="2">
                                            <table width="150px">
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="rdbbAll" runat="server" Checked="True" GroupName="b" TabIndex="3" />
                                                    </td>
                                                    <td>All
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rdbbSelected" runat="server" GroupName="b" />
                                                    </td>
                                                    <td>Selected
                                                    </td>
                                                    <td>
                                                        <span id="litBranch" runat="server" style="color: Maroon"></span>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <%--                        <tr>
                            <td class="gridcellleft">
                                <strong><span >Client:</span></strong>
                            </td>
                            <td style="text-align: left;" colspan="2">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="rdbClientA" runat="server" Checked="True" GroupName="c" TabIndex="4" />
                                        </td>
                                        <td>
                                            All
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdbClientS" runat="server" GroupName="c" />
                                        </td>
                                        <td>
                                            Selected
                                        </td>
                                        <td>
                                            <span id="spnClient" runat="server" style="color: Maroon"></span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>--%>

                                    <%--                        <tr>
                            <td class="gridcellleft">
                                <strong><span >Product:</span></strong>
                            </td>
                            <td style="text-align: left;" colspan="2">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="rdbProductA" runat="server" Checked="True" GroupName="p" TabIndex="6" />
                                        </td>
                                        <td>
                                            All
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdbProductS" runat="server" GroupName="p" />
                                        </td>
                                        <td>
                                            Selected
                                        </td>
                                        <td>
                                            <span id="spnProduct" runat="server" style="color: Maroon"></span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>--%>
                                    <tr>
                                        <td class="gridcellleft">
                                            <strong><span >Report To:</span></strong>
                                        </td>
                                        <td style="text-align: left;" colspan="2">
                                            <table width="150px">
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="rdbTelecallerA" runat="server" Checked="True" GroupName="t"
                                                            TabIndex="7" />
                                                    </td>
                                                    <td>All
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rdbTelecallerS" runat="server" GroupName="t" />
                                                    </td>
                                                    <td>Selected
                                                    </td>
                                                    <td>
                                                        <span id="spnTeleCaller" runat="server" style="color: Maroon"></span>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="gridcellleft">
                                            <strong><span >Employee.:</span></strong>
                                        </td>
                                        <td style="text-align: left;" colspan="2">
                                            <table width="150px">
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="rdbSalesRepresentativeA" runat="server" Checked="True" GroupName="s"
                                                            TabIndex="8" />
                                                    </td>
                                                    <td>All
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rdbSalesRepresentativeS" runat="server" GroupName="s" />
                                                    </td>
                                                    <td>Selected
                                                    </td>
                                                    <td>
                                                        <span id="spnSalesRepresentative" runat="server" style="color: Maroon"></span>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <%--                        <tr>
                            <td class="gridcellleft">
                                <strong><span >Associate:</span></strong>
                            </td>
                            <td style="text-align: left;" colspan="2">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="rdbAssociateA" runat="server" Checked="True" GroupName="a" TabIndex="9" />
                                        </td>
                                        <td>
                                            All
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdbAssociateS" runat="server" GroupName="a" />
                                        </td>
                                        <td>
                                            Selected
                                        </td>
                                        <td>
                                            <span id="spnAssociate" runat="server" style="color: Maroon"></span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                       <tr>
                            <td class="gridcellleft">
                                <strong><span >Sub Broaker:</span></strong>
                            </td>
                            <td style="text-align: left;" colspan="2">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="rdbSubBroakerA" runat="server" Checked="True" GroupName="sb"
                                                TabIndex="10" />
                                        </td>
                                        <td>
                                            All
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdbSubBroakerS" runat="server" GroupName="sb" />
                                        </td>
                                        <td>
                                            Selected
                                        </td>
                                        <td>
                                            <span id="spnSubBroaker" runat="server" style="color: Maroon"></span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <strong><span >Policy Status:</span></strong>
                            </td>
                            <td class="gridcellleft">
                                <asp:DropDownList ID="drpPolicyStatus" runat="server" Width="179px" Font-Size="12px"
                                    TabIndex="11">
                                    <asp:ListItem Value="A">All</asp:ListItem>
                                    <asp:ListItem Value="0">Business in Hand</asp:ListItem>
                                    <asp:ListItem Value="1">Cancelled</asp:ListItem>
                                    <asp:ListItem Value="2">Canclled from inception</asp:ListItem>
                                    <asp:ListItem Value="3">Cheque Bounced</asp:ListItem>
                                    <asp:ListItem Value="4">Issued</asp:ListItem>
                                    <asp:ListItem Value="5">Lapsed</asp:ListItem>
                                    <asp:ListItem Value="6">Login</asp:ListItem>
                                </asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="Button1" runat="server" Text="Show" OnClick="Button1_Click" CssClass="btnUpdate"
                                    Height="19px" Width="101px" TabIndex="12" />
                            </td>
                        </tr>--%>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <asp:Button ID="Button1" runat="server" Text="Show" OnClick="Button1_Click" CssClass="btn btn-primary"
                                              TabIndex="12" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="text-align: right; vertical-align: top;">
                                <table width="100%" id="showFilter" style="display: none;">
                                    <tr>
                                        <td style="text-align: left; vertical-align: top">
                                            <table cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td style="vertical-align: top;">
                                                        <table cellpadding="0px" cellspacing="0px">
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="txtsubscriptionID" runat="server" Font-Size="12px" Width="150px"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbsearchOption" runat="server" Font-Size="11px" Width="85px"
                                                                        Enabled="false">
                                                                        <asp:ListItem>Branch</asp:ListItem>
                                                                        <asp:ListItem>Clients</asp:ListItem>
                                                                        <asp:ListItem>Company</asp:ListItem>
                                                                        <asp:ListItem>Products</asp:ListItem>
                                                                        <asp:ListItem>ReportTo</asp:ListItem>
                                                                        <asp:ListItem>Employee</asp:ListItem>
                                                                        <asp:ListItem>Associate</asp:ListItem>
                                                                        <asp:ListItem>Sub Broker</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <a id="A4" href="javascript:void(0);" onclick="btnAddsubscriptionlist_click()"><span
                                                                        style="color: #009900; text-decoration: underline; font-size: 8pt;">Add to List</span></a><span
                                                                            style="color: #009900; font-size: 8pt;"> </span>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <br />
                                                        <asp:ListBox ID="lstSuscriptions" runat="server" Font-Size="12px" Height="90px" Width="253px"></asp:ListBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: center">
                                                        <table cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td>
                                                                    <a id="A2" href="javascript:void(0);" onclick="clientselectionfinal()"><span style="color: #000099; text-decoration: underline; font-size: 10pt;">Done</span></a>&nbsp;&nbsp;
                                                                </td>
                                                                <td>
                                                                    <a id="A1" href="javascript:void(0);" onclick="btnRemovefromsubscriptionlist_click()">
                                                                        <span style="color: #cc3300; text-decoration: underline; font-size: 8pt;">Remove</span></a>
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
                            </td>
                        </tr>

                    </table>
                </td>
            </tr>




            <tr>
                <td>
                    <table width="100%">

                        <tr>
                            <td style="text-align: left; vertical-align: top">
                                <table>
                                    <tr>
                                        <td id="TDS">
                                            <a href="javascript:ShowHideFilter('s');" class="btn btn-success"><span >Show Filter</span></a>
                                        </td>
                                        <td id="TDA">
                                            <a href="javascript:ShowHideFilter('All');" class="btn btn-primary"><span >All Records</span></a>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td></td>
                            <td class="gridcellright pull-right">
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
                                    <ItemStyle >
                                        <HoverStyle >
                                        </HoverStyle>
                                    </ItemStyle>
                                    <Border BorderColor="black" />
                                    <DropDownButton Text="Export">
                                    </DropDownButton>
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Panel ID="Panel1" runat="server" Height="100%">
                                <dxe:ASPxGridView ID="EmployeeGrid" runat="server" KeyFieldName="cnt_id" AutoGenerateColumns="False"
                                    DataSourceID="EmployeeDataSource" Width="100%" ClientInstanceName="grid" OnCustomCallback="EmployeeGrid_CustomCallback">
                                    <ClientSideEvents ColumnResizing="function(s, e) {
	                                                        Callheight();
                                                        }" />
                                    <SettingsBehavior AllowFocusedRow="True" ConfirmDelete="True" ColumnResizeMode="NextColumn" />
                                    <Styles>
                                        <Header SortingImageSpacing="5px" ImageSpacing="5px"></Header>

                                        <LoadingPanel ImageSpacing="10px"></LoadingPanel>

                                        <Row Wrap="False"></Row>
                                    </Styles>
                                    <SettingsPager ShowSeparators="True" AlwaysShowPager="True" NumericButtonCount="20"
                                        PageSize="20">
                                        <FirstPageButton Visible="True"></FirstPageButton>

                                        <LastPageButton Visible="True"></LastPageButton>
                                    </SettingsPager>
                                    <Columns>
                                        <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="0" FieldName="Name">
                                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>

                                            <EditFormCaptionStyle HorizontalAlign="Right"></EditFormCaptionStyle>

                                            <EditFormSettings Visible="False"></EditFormSettings>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="cnt_shortName" Width="6%" Caption="Code">
                                            <CellStyle Wrap="False" HorizontalAlign="Left"></CellStyle>

                                            <EditFormSettings Visible="False"></EditFormSettings>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="BranchName">
                                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>

                                            <EditFormCaptionStyle HorizontalAlign="Right"></EditFormCaptionStyle>

                                            <EditFormSettings Visible="False"></EditFormSettings>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="CompName" Caption="Company Name">
                                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>

                                            <EditFormCaptionStyle HorizontalAlign="Right"></EditFormCaptionStyle>

                                            <EditFormSettings Visible="False"></EditFormSettings>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="4" FieldName="cost_description" Caption="Department">
                                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>

                                            <EditFormCaptionStyle HorizontalAlign="Right"></EditFormCaptionStyle>

                                            <EditFormSettings Visible="False"></EditFormSettings>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="deg_designation" Caption="Designation">
                                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>

                                            <EditFormCaptionStyle HorizontalAlign="Right"></EditFormCaptionStyle>

                                            <EditFormSettings Visible="False"></EditFormSettings>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="5" FieldName="phone" Caption="Mobile">
                                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>

                                            <EditFormCaptionStyle HorizontalAlign="Right"></EditFormCaptionStyle>

                                            <EditFormSettings Visible="False"></EditFormSettings>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="6" FieldName="ReportTo" Caption="Report To">
                                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>

                                            <EditFormCaptionStyle HorizontalAlign="Right"></EditFormCaptionStyle>

                                            <EditFormSettings Visible="False"></EditFormSettings>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataDateColumn ReadOnly="True" VisibleIndex="7" ToolTip="DD-MM-YYYY" FieldName="DOJ" Caption="DOJ">
                                            <PropertiesDateEdit DisplayFormatString="dd-MM-yyyy" EditFormat="Custom"></PropertiesDateEdit>

                                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>

                                            <EditFormCaptionStyle HorizontalAlign="Right"></EditFormCaptionStyle>

                                            <EditFormSettings Visible="False"></EditFormSettings>
                                        </dxe:GridViewDataDateColumn>
                                        <dxe:GridViewDataTextColumn VisibleIndex="8" Caption="Details">
                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                            <DataItemTemplate>
                                                <a href="javascript:void(0);" onclick="OnMoreInfoClick('<%# Container.KeyValue %>')">More Info...</a>

                                            </DataItemTemplate>

                                            <CellStyle Wrap="False"></CellStyle>
                                            <HeaderTemplate>
                                                <%--  <%if (Session["PageAccess"].ToString().Trim() == "All" || Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "DelAdd")
                                      { %>
                                    <a href="javascript:void(0);" onclick="OnAddButtonClick()"><span style="color: #000099;
                                        text-decoration: underline">Add New</span> </a>
                                    <%} %>--%>
                                            </HeaderTemplate>
                                              
                                            <EditFormSettings Visible="False"></EditFormSettings>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewCommandColumn Visible="False" ShowDeleteButton="true">
                                          <%--  <DeleteButton Visible="True" Text="Delete"></DeleteButton>--%>
                                        </dxe:GridViewCommandColumn>
                                        <dxe:GridViewDataTextColumn VisibleIndex="9" Caption="Cont.Person">
                                            <DataItemTemplate>
                                                <a href="javascript:void(0);" onclick="OnContactInfoClick('<%#Eval("Id") %>','<%#Eval("Name") %>')">Show</a>

                                            </DataItemTemplate>

                                            <CellStyle Wrap="False" HorizontalAlign="Left"></CellStyle>

                                            <EditFormSettings Visible="False"></EditFormSettings>
                                        </dxe:GridViewDataTextColumn>
                                    </Columns>
                                    <SettingsCommandButton>

                                        <DeleteButton  Text="Delete"></DeleteButton>
                                    </SettingsCommandButton>
                                    <SettingsEditing Mode="PopupEditForm" PopupEditFormHorizontalAlign="Center" PopupEditFormModal="True"
                                        PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="900px" EditFormColumnCount="3" />
                                    <SettingsText PopupEditFormCaption="Add/ Modify Employee" ConfirmDelete="Are you sure to delete?" />
                                    <Settings ShowGroupPanel="True" ShowStatusBar="Visible" />
                                </dxe:ASPxGridView>
                                <br />
                                <br />
                                <br />
                                <br />
                                <br />

                            </asp:Panel>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click"></asp:AsyncPostBackTrigger>
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
        <asp:SqlDataSource ID="EmployeeDataSource" runat="server"
            DeleteCommandType="StoredProcedure" DeleteCommand="EmployeeDelete">
            <DeleteParameters>
                <asp:Parameter Name="cnt_id" Type="Int32" />
                <asp:SessionParameter Name="userId" SessionField="userId" Type="Int32" />
            </DeleteParameters>
        </asp:SqlDataSource>
        <br />
        <dxe:ASPxGridViewExporter ID="exporter" runat="server">
        </dxe:ASPxGridViewExporter>
    </div>

</asp:Content>
