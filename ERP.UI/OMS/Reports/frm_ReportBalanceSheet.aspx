<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Reports_frm_ReportBalanceSheet" CodeBehind="frm_ReportBalanceSheet.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="/assests/js/init.js"></script>

    <script type="text/javascript" src="/assests/js/ajax-dynamic-list_rootfile.js"></script>

    

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
        
        function ShowClientClick() {
            $('#MandatoryBranch').css({ 'display': 'none' });
            $('#MandatoryDate').css({ 'display': 'none' });

            if (dtfor.GetDate() == null) {
                $('#MandatoryDate').css({ 'display': 'block' });
            }
            else if (gridLookup.GetText().trim() == "") {
                $('#MandatoryBranch').css({ 'display': 'block' });
            }
            else {
                caspxPlBlGrid.PerformCallback();
            }
        }

        function Page_Load()///Call Into Page Load
        {
            Hide('showFilter');
            Hide('td_Rpt');
            document.getElementById('hiddencount').value = 0;
            height();
        }
        function height() {
            if (document.body.scrollHeight >= 350) {
               // window.frameElement.height = document.body.scrollHeight;
            }
            else {
              //  window.frameElement.height = '350px';
            }
          //  window.frameElement.width = document.body.scrollwidth;
        }
        function Hide(obj) {
            document.getElementById(obj).style.display = 'none';
        }
        function Show(obj) {
            document.getElementById(obj).style.display = 'table';
        }



        function fn_Segment(obj) {
            if (obj == "a")
                Hide('showFilter');
            else {
                var cmb = document.getElementById('cmbsearchOption');
                cmb.value = 'Segment';
                Show('showFilter');
            }
            height();
        }
        function fn_Branch(obj) {
            if (obj == "a")
                Hide('showFilter');
            else {
                var cmb = document.getElementById('cmbsearchOption');
                cmb.value = 'Branch';
                Show('showFilter');
            }
            height();
        }

        function btnAddsubscriptionlist_click() {

            var cmb = document.getElementById('cmbsearchOption');
            var userid = document.getElementById('txtSelectionID');
            if (userid.value != '') {
                var ids = document.getElementById('txtSelectionID_hidden');
                var listBox = document.getElementById('lstSlection');
                var tLength = listBox.length;


                var no = new Option();
                no.value = ids.value;
                no.text = userid.value;
                listBox[tLength] = no;
                var recipient = document.getElementById('txtSelectionID');
                recipient.value = '';
            }
            else
                alert('Please search name and then Add!')
            var s = document.getElementById('txtSelectionID');
            s.focus();
            s.select();

        }

        function clientselectionfinal() {
            var listBoxSubs = document.getElementById('lstSlection');

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

            }
            var i;
            for (i = listBoxSubs.options.length - 1; i >= 0; i--) {
                listBoxSubs.remove(i);
            }

            Hide('showFilter');
            document.getElementById('btnShow').disabled = false;
        }


        function btnRemovefromsubscriptionlist_click() {

            var listBox = document.getElementById('lstSlection');
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



        function heightlight(obj) {

            var colorcode = obj.split('&');

            if ((document.getElementById('hiddencount').value) == 0) {
                prevobj = '';
                prevcolor = '';
                document.getElementById('hiddencount').value = 1;

            }
            document.getElementById(obj).style.backgroundColor = '#ffe1ac';

            if (prevobj != '') {
                document.getElementById(prevobj).style.backgroundColor = prevcolor;
            }
            prevobj = obj;
            prevcolor = colorcode[1];

        }

        function ajaxcall(objID, objListFun, objEvent) {
            cmbVal = document.getElementById('cmbsearchOption').value;
            ajax_showOptions(objID, 'ShowClientFORMarginStocks', objEvent, cmbVal);
        }
        function displayresult(obj) {
            if (obj == '1')
                Show('displayAll');
            if (obj == '2') {
                Hide('displayAll');
                alert('No Record Found !!');
            }
            height();
        }

        function fnrptview(obj) {
            if (obj == "2") {
                Show('td_Rpt');
                Hide('btnShow');
            }
            else {
                Hide('td_Rpt');
                Show('btnShow');
            }
        }
        FieldName = 'lstSlection';
    </script>

    <script type="text/ecmascript">
        function ReceiveServerData(rValue) {
            var j = rValue.split('~');
            if (j[0] == 'Branch') {
                document.getElementById('HiddenField_Branch').value = j[1];
            }
            if (j[0] == 'Segment') {
                document.getElementById('HiddenField_Segment').value = j[1];
            }

        }
    </script>
    <script type="text/javascript">
    //    var prm = Sys.WebForms.PageRequestManager.getInstance();
      //  prm.add_initializeRequest(InitializeRequest);
    //    prm.add_endRequest(EndRequest);
        var postBackElement;
        //function InitializeRequest(sender, args) {
        //    if (prm.get_isInAsyncPostBack())
        //        args.set_cancel(true);
        //    postBackElement = args.get_postBackElement();
        //    $get('UpdateProgress1').style.display = 'block';
        //}
        //function EndRequest(sender, args) {
        //    $get('UpdateProgress1').style.display = 'none';

        //}
    </script>
    <style>
        .bacgrnded {
            padding: 13px 7px;
            background: #c5d3da;
            border-radius: 5px;
            margin-bottom: 7px;
        }
        #MandatoryBranch ,#MandatoryDate {
            position:absolute;
            right:-5px;
            top:10px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Balance Sheet</h3>
        </div>
    </div>
    <div class="form_main inner clearfix">
        <%--<table class="TableMain100">
            <tr>
                <td class="EHEADER" colspan="0" style="text-align: center; height: 20px;">
                    <strong><span id="SpanHeader" style="color: #000099">Balance Sheet</span></strong></td>
            </tr>
        </table>--%>
        <table class="TableMain100">
            <tr valign="top">
                <td>
                    <table cellspacing="1" cellpadding="2" 
                        >
                        <tr>
                            <td class="gridcellleft" width="110px">As On Date :</td>
                            <td class="gridcellleft relative"  style="padding-right:15px">
                                <dxe:ASPxDateEdit ID="dtfor" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                    Font-Size="12px" Width="150px" ClientInstanceName="dtfor"> 
                                </dxe:ASPxDateEdit>
                                  <span id="MandatoryDate" class="pullleftClass fa fa-exclamation-circle iconRed " style="color:red;display:none;padding-left: 9px;" title="Mandatory"></span>
                            </td>
                            <td class="gridcellleft " width="110px" style="padding-left:15px;">Branch :</td>
                            <td style="padding:5px 0; padding-right: 15px;" class="relative">
                                <div style="display:none">
                                    <asp:RadioButton ID="rdBanchAll" runat="server" Checked="True" GroupName="a" onclick="fn_Branch('a')" />
                                            All
                                     <asp:RadioButton ID="rdBanchSelected" runat="server" GroupName="a" onclick="fn_Branch('b')" />Selected
                                    </div>
                                <%--Branch LookUp--%>
                                  <dxe:ASPxGridLookup ID="GridLookup" runat="server" SelectionMode="Multiple" ClientInstanceName="gridLookup"
                                                                        KeyFieldName="branch_id" Width="100%" TextFormatString="{0}" MultiTextSeparator=", " OnDataBinding="GridLookup_DataBinding">
                                                                        <Columns>
                                                                            <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="60" Caption=" "/>
                                                                            <dxe:GridViewDataColumn FieldName="branch_code" Caption="Branch Code" Width="150"/>
                                                                            <dxe:GridViewDataColumn FieldName="branch_description" Caption="Branch Description" Width="300"/>
                                                                        </Columns>
                                                                        <GridViewProperties  Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords"   >
                                                                             
                                                                            <Templates>
                                                                                <StatusBar>
                                                                                    <table class="OptionsTable" style="float: right">
                                                                                        <tr>
                                                                                            <td>
                                                                                              <%--  <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridLookup" />--%>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </StatusBar>
                                                                            </Templates>
                                                                            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true"/>
                                                                        </GridViewProperties>
                                                                    </dxe:ASPxGridLookup>
                                  <span id="MandatoryBranch" class="pullleftClass fa fa-exclamation-circle iconRed " style="color:red;display:none;padding-left: 9px;" title="Mandatory"></span>

                            </td>
                             <td class="gridcellleft" style="display:none">Segment :</td>
                            <td  style="display:none">
                                <asp:RadioButton ID="rdSegmentAll" runat="server" Checked="True" GroupName="b" onclick="fn_Segment('a')" />
                                All
                                    <asp:RadioButton ID="rdSegmentSelected" runat="server" GroupName="b" onclick="fn_Segment('b')" />Selected
                            </td>
                            <td class="gridcellleft" style="display:none">Generate Type:</td>
                            <td style="display:none">
                                <asp:DropDownList ID="ddlrptview" runat="server" Font-Size="11px" Width="150px" Enabled="true"
                                    onchange="fnrptview(this.value)">
                                    <asp:ListItem Value="1">Screen</asp:ListItem>
                                    <asp:ListItem Value="2">Export</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td class="gridcellleft" style="padding-left:15px;padding-top:2px">
                                <span  id="td_Show">
                                <asp:Button ID="btnShow" runat="server" CssClass="btnUpdate btn btn-primary"  Text="Show"
                                     OnClick="btnShow_Click" OnClientClick="ShowClientClick();return false;" />
                                </span>
                                <span id="td_Rpt">
                                <asp:Button ID="btnRpt" runat="server" CssClass="btnUpdate" Height="20px" Text="Open To PDF"
                                    Width="101px" OnClick="btnRpt_Click" />
                                </span>
                                <% if (rights.CanExport)
                                               { %>
                                <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary"  OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true">
                                    <asp:ListItem Value="0">Export to</asp:ListItem>
                                    <asp:ListItem Value="1">PDF</asp:ListItem>
                                    <asp:ListItem Value="2">XLS</asp:ListItem>
                                    <asp:ListItem Value="3">RTF</asp:ListItem>
                                    <asp:ListItem Value="4">CSV</asp:ListItem>
                                </asp:DropDownList>
                                 <% } %>
                                <dxe:ASPxGridViewExporter ID="exporter" runat="server">
                                </dxe:ASPxGridViewExporter> 
                            </td>
                            
                        </tr>
                        
                        <tr>
                            <td></td>
                            <td>
                    <table id="showFilter">
                        <tr>
                            
                            <td>
                                <div class="bacgrnded">
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td class="gridcellleft" style="vertical-align: top; text-align: right; height: 21px;"
                                            id="TdFilter">
                                            <asp:TextBox ID="txtSelectionID" runat="server" Font-Size="12px" Width="210px" onkeyup="ajaxcall(this,'chkfn',event)"></asp:TextBox>
                                            
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="cmbsearchOption" runat="server" Font-Size="11px" Width="70px"
                                                Enabled="false">
                                                <asp:ListItem>Segment</asp:ListItem>
                                                <asp:ListItem>Branch</asp:ListItem>
                                            </asp:DropDownList>
                                            <a id="A4" href="javascript:void(0);" onclick="btnAddsubscriptionlist_click()"><span
                                                style="color: #009900; text-decoration: underline; font-size: 8pt;">Add to List</span></a><span
                                                    style="color: #009900; font-size: 8pt;"> </span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="" colspan="2">
                                            <asp:ListBox ID="lstSlection" runat="server" Font-Size="12px" Height="90px" Width="100%"></asp:ListBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: center" colspan="2">
                                            <table cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <a id="A2" href="javascript:void(0);" onclick="clientselectionfinal()" class="btn btn-primary btn-small"><span>Done</span></a>&nbsp;&nbsp;
                                                    </td>
                                                    <td>
                                                        <a id="A1" href="javascript:void(0);" onclick="btnRemovefromsubscriptionlist_click()" class="btn btn-danger btn-small">
                                                            <span>Remove</span></a>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                   
                                </table>
                               </div>
                            </td>
                        </tr>
                    </table>
                            </td>
                        </tr>
                        
                    </table>
                </td>
                
            </tr>
        </table>
        <table>
            <tr>
                <td style="display: none;">
                    <asp:TextBox ID="txtSelectionID_hidden" runat="server" Font-Size="12px" Width="150px"></asp:TextBox>
                    <asp:HiddenField ID="HiddenField_Branch" runat="server" />
                    <asp:HiddenField ID="HiddenField_Segment" runat="server" />
                    <asp:HiddenField ID="hiddencount" runat="server" />
                </td>
                <td>
                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                        <ProgressTemplate>
                            <div id='Div1' style='position: absolute; font-family: arial; font-size: 30; left: 50%; top: 50%; background-color: white; layer-background-color: white; height: 80; width: 150;'>
                                <table width='100' height='35' border='1' cellpadding='0' cellspacing='0' bordercolor='#C0D6E4'>
                                    <tr>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td height='25' align='center' bgcolor='#FFFFFF'>
                                                        <img src='/assests/images/progress.gif' width='18' height='18'></td>
                                                    <td height='10' width='100%' align='center' bgcolor='#FFFFFF'>
                                                        <font size='1' face='Tahoma'><strong align='center'>Please Wait..</strong></font></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </td>
            </tr>
        </table>
        <div id="displayAll" style="display: none;">
            <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                <ContentTemplate>
                    <table width="100%" border="1">
                        <tr>
                            <td>
                                <div id="display" runat="server">
                                </div>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click"></asp:AsyncPostBackTrigger>
                </Triggers>
            </asp:UpdatePanel>
        </div>
        

        <div class="GridViewArea">
            <dxe:ASPxGridView ID="aspxPlBlGrid" runat="server" AutoGenerateColumns="False" ClientInstanceName="caspxPlBlGrid"
                Width="100%" CssClass="pull-left" SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="400" OnHtmlRowPrepared="aspxPlBlGrid_HtmlRowPrepared"
                OnCustomCallback="aspxPlBlGrid_CustomCallback" OnDataBinding="aspxPlBlGrid_DataBinding">

                <settingspager mode="ShowAllRecords"></settingspager>

                <settings showstatusbar="Hidden" showfilterrow="false" showfilterrowmenu="false" />
                <columns>
                      <dxe:GridViewBandColumn Caption="Liabilities">
                           <Columns>
                            <dxe:GridViewDataTextColumn Caption="Main Account"  ReadOnly="True" FieldName="libMainAct"
                                Visible="True" FixedStyle="Left" VisibleIndex="0">
                                <EditFormSettings Visible="True" /> 
                            </dxe:GridViewDataTextColumn>

                             <dxe:GridViewDataTextColumn Caption="Amount"  ReadOnly="True" FieldName="libAmount"
                                Visible="True" FixedStyle="Left" VisibleIndex="1">
                                <EditFormSettings Visible="True" /> 
                            </dxe:GridViewDataTextColumn>
                           </columns>
                     </dxe:GridViewBandColumn>
                     <dxe:GridViewBandColumn Caption="Asset">
                         <Columns>
                             <dxe:GridViewDataTextColumn Caption="Main Account"  ReadOnly="True" FieldName="astMainAct"
                                Visible="True" FixedStyle="Left" VisibleIndex="2">
                                <EditFormSettings Visible="True" /> 
                            </dxe:GridViewDataTextColumn>

                             <dxe:GridViewDataTextColumn Caption="Amount"  ReadOnly="True" FieldName="astAmount"
                                Visible="True" FixedStyle="Left" VisibleIndex="3">
                                <EditFormSettings Visible="True" /> 
                            </dxe:GridViewDataTextColumn>
                        </columns>
                    </dxe:GridViewBandColumn>
                 </columns>

                <settingsbehavior columnresizemode="NextColumn" /> 
            </dxe:ASPxGridView>
        </div>
        </div>
     <asp:SqlDataSource ID="BranchDataSource" runat="server" 
            SelectCommand="">
            
  </asp:SqlDataSource>
    
</asp:Content>
