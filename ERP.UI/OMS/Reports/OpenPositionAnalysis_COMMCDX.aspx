<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Reports_OpenPositionAnalysis_COMMCDX" CodeBehind="OpenPositionAnalysis_COMMCDX.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/assests/js/loaddata1.js"></script>

    <script type="text/javascript" src="/assests/js/ajaxList_rootfile.js"></script>

    <script type="text/javascript" src="/assests/js/init.js"></script>

    <script type="text/javascript" src="/assests/js/GenericJScript.js"></script>

    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />
    <link type="text/css" href="../CSS/AjaxStyle.css" rel="Stylesheet" />
    <style type="text/css">
        form {
            display: inline;
        }
    </style>

    <script type="text/javascript" language="javascript">
        function SignOff() {
            window.parent.SignOff();
        }
        function PageLoad()///Call Into Page Load
        {
            Hide('showFilter');
            cdtPosition.Focus();
            Reset();
            height();
        }
        function Hide(obj) {
            document.getElementById(obj).style.display = 'none';
        }
        function Show(obj) {
            document.getElementById(obj).style.display = 'inline';
        }
        function height() {
            if (document.body.scrollHeight >= 300)
                window.frameElement.height = document.body.scrollHeight;
            else
                window.frameElement.height = '350px';
            window.frameElement.Width = document.body.scrollWidth;
        }
        function ChangeDateFormat_CalToDB(obj) {
            var SelectedDate = new Date(obj);
            var monthnumber = SelectedDate.getMonth() + 1;
            var monthday = SelectedDate.getDate();
            var year = SelectedDate.getYear();
            var changedDateValue = year + '-' + monthnumber + '-' + monthday;
            return changedDateValue;
        }
        function ChangeDateFormat_SetCalenderValue(obj) {
            var SelectedDate = new Date(obj);
            var monthnumber = SelectedDate.getMonth();
            var monthday = SelectedDate.getDate();
            var year = SelectedDate.getYear();
            var changeDateValue = new Date(year, monthnumber, monthday);
            return changeDateValue;
        }
        function replaceChars(entry) {
            out = "+"; // replace this
            add = "--"; // with this
            temp = "" + entry; // temporary holder

            while (temp.indexOf(out) > -1) {
                pos = temp.indexOf(out);
                temp = "" + (temp.substring(0, pos) + add +
                temp.substring((pos + out.length), temp.length));
            }
            return temp;
        }
        function DateChange(positionDate) {
            var FYS = '<%=Session["FinYearStart"]%>';
            var FYE = '<%=Session["FinYearEnd"]%>';
            var LFY = '<%=Session["LastFinYear"]%>';
            DevE_CheckForFinYear(positionDate, FYS, FYE, LFY);
        }
    </script>

    <script type="text/javascript" language="javascript">
        FieldName = "none";
        //=================For Call ajax to fill text boxes after page Load and with ICallbackEventHandler Interface==================
        function CallGenericAjaxJS(e) {
            var AjaxList_TextBox = document.getElementById('<%=txtSelectionID.ClientID%>');
            AjaxList_TextBox.focus();
            AjaxComQuery = AjaxComQuery.replace("\'", "'");
            ajax_showOptions(AjaxList_TextBox, 'GenericAjaxList', e, replaceChars(AjaxComQuery), 'Main');
        }
        //=================For Call ajax to fill text boxes on page Load==================
        function CallAjax(obj1, obj2, obj3, Query) {
            var CombinedQuery = new String(Query);
            ajax_showOptions(obj1, obj2, obj3, replaceChars(CombinedQuery), 'Main');
        }
        function fnddlGroup(obj) {
            document.getElementById('<%=lstSelection.ClientID%>').length = 0;
            if (obj == "0") {
                document.getElementById('rdbranchclientAll').checked = true;
                fn_Branch('a');
                Hide('dvGroup');
                Hide('dvGroupTypeByAllSelect');
                Show('dvBranch');
                CallServer("CallAjax-Branch", "");
            }
            if (obj == "1") {
                Show('dvGroup');
                Hide('dvBranch');
                Hide('showFilter');
                document.getElementById('divExport').style.display = 'inline';
                var btn = document.getElementById('btnhide');
                btn.click();
            }
            if (obj == "2") {
                document.getElementById('rdbranchclientAll').checked = true;
                fn_Clients('a');
                Hide('dvGroup');
                Show('dvBranch');
                Hide('dvGroupTypeByAllSelect');
                CallServer("CallAjax-Client", "");
            }
            height();
        }
        function fn_BranchClient(obj) {
            var grpBy = document.getElementById('<%=ddlGroup.ClientID%>');
           if (grpBy.value == "0") {
               fn_Branch(obj);
           }
           else if (grpBy.value == "2") {
               fn_Clients(obj);
           }
       }
       function fn_Branch(obj) {
           if (obj == "a") {
               Hide('showFilter');
               document.getElementById('divExport').style.display = 'inline';
           }
           else {
               Show('showFilter');
               document.getElementById('divExport').style.display = 'none';
               CallServer("CallAjax-Branch", "");
               document.getElementById('<%=txtSelectionID.ClientID%>').focus();
           }
       }
       function fn_Clients(obj) {
           if (obj == "a") {
               Hide('showFilter');
               document.getElementById('divExport').style.display = 'inline';
           }
           else {
               Show('showFilter');
               document.getElementById('divExport').style.display = 'none';
               CallServer("CallAjax-Client", "");
               document.getElementById('<%=txtSelectionID.ClientID%>').focus();
              }
          }
          function fngrouptype(obj) {
              if (obj == "0") {
                  Hide('dvGroupTypeByAllSelect');
                  alert('Please Select Group Type !');
              }
              else {
                  document.getElementById('rdddlgrouptypeAll').checked = true;
                  Show('dvGroupTypeByAllSelect');
              }
              height();
          }
          function fn_Group(obj) {
              if (obj == "a") {
                  Hide('showFilter');
                  document.getElementById('divExport').style.display = 'inline';
              }
              else {
                  Show('showFilter');
                  document.getElementById('divExport').style.display = 'none';
                  CallServer("CallAjax-Group", "");
                  document.getElementById('<%=txtSelectionID.ClientID%>').focus();
              }
          }
          function ReceiveServerData(rValue) {
              var Data = rValue.split('~');
              var btnHideGroupType = document.getElementById('btnGroupTypehide');
              if (Data[1] != "undefined") {
                  if (Data[0] == 'Client') {
                      document.getElementById('HiddenField_Client').value = Data[1];
                  }
                  if (Data[0] == 'Branch') {
                      document.getElementById('HiddenField_Branch').value = Data[1];
                  }
                  if (Data[0] == 'Group') {
                      document.getElementById('HiddenField_Group').value = Data[1];
                  }
              }
              if (Data[0] == 'AjaxQuery') {
                  AjaxComQuery = Data[1];
                  var AjaxList_TextBox = document.getElementById('<%=txtSelectionID.ClientID%>');
                  AjaxList_TextBox.value = '';
                  AjaxList_TextBox.attachEvent('onkeyup', CallGenericAjaxJS);
              }
          }
          function btnAddsubscriptionlist_click() {
              var txtName = document.getElementById('txtSelectionID');
              if (txtName != '') {
                  var txtId = document.getElementById('txtSelectionID_hidden').value;
                  var listBox = document.getElementById('<%=lstSelection.ClientID%>');
                var listLength = listBox.length;
                var opt = new Option();
                opt.value = txtId;
                opt.text = txtName.value;
                listBox[listLength] = opt;
                txtName.value = '';
            }
            else
                alert('Please search name and then Add!');
            txtName.focus();
            txtName.select();
        }
        function lnkBtnAddFinalSelection() {
            var listBox = document.getElementById('<%=lstSelection.ClientID%>');
            var listID = '';
            var i;
            if (listBox.length > 0) {
                for (i = 0; i < listBox.length; i++) {
                    if (listID == '')
                        listID = listBox.options[i].value + ';' + listBox.options[i].text;
                    else
                        listID += ',' + listBox.options[i].value + ';' + listBox.options[i].text;
                }
                var sendData = '';
                var grpBy = document.getElementById('<%=ddlGroup.ClientID%>').value;
                if (grpBy == "0") {
                    var sendData = 'Branch~' + listID;
                }
                else if (grpBy == "1") {
                    var sendData = 'Group~' + listID;
                }
                else if (grpBy == "2") {
                    var sendData = 'Client~' + listID;
                }
                CallServer(sendData, "");
            }
            var i;
            for (i = listBox.options.length - 1; i >= 0; i--) {
                listBox.remove(i);
            }
            Hide('showFilter');
            document.getElementById('divExport').style.display = 'inline';
        }
        function lnkBtnRemoveFromSelection() {
            var listBox = document.getElementById('<%=lstSelection.ClientID%>');
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
        function NORECORD(obj) {
            Hide('showFilter');
            document.getElementById('divExport').style.display = 'inline';
            var grpBy = document.getElementById('<%=ddlGroup.ClientID%>').value;
            if (obj == '1') {
                if (grpBy == '0' && document.getElementById('HiddenField_Branch').value == '') {
                    alert('You Have To Select Atleast One Branch !!');
                    Reset();
                }
                if (grpBy == '1' && document.getElementById('HiddenField_Group').value == '') {
                    alert('You Have To Select Atleast One Group !!');
                    Reset();
                }
                if (grpBy == '2' && document.getElementById('HiddenField_Client').value == '') {
                    alert('You Have To Select Atleast One Client !!');
                    Reset();
                }
            }
            if (obj == '2') {
                alert('No Record Found !! ');
                Reset();
            }
            height();
        }
        function Reset() {
            Hide('showFilter');
            document.getElementById('divExport').style.display = 'inline';
            document.getElementById('HiddenField_Branch').value == '';
            document.getElementById('HiddenField_Group').value == '';
            document.getElementById('HiddenField_Client').value == '';
            document.getElementById('<%=ddlGroup.ClientID%>').selectedIndex = 0;
          document.getElementById('rdbranchclientAll').checked = true;
          document.getElementById('<%=ddlgrouptype.ClientID%>').selectedIndex = 0;
           document.getElementById('<%=ddlgrouptype.ClientID%>').style.display = 'none';
          document.getElementById('dvGroup').style.display = 'none';
          document.getElementById('dvBranch').style.display = 'inline';
          height();
      }
      function Validate(obj) {
          if (obj == "1") {
              alert("Please Select Position Date.");
              cdtPosition.Focus();
          }
          if (obj == "2") {
              alert("Please Select Report type.");
              ddlRptType.Focus();
          }
          if (obj == "3") {
              alert("Please Select A Group Type.");
              document.getElementById('<%=ddlgrouptype.ClientID%>').focus();
          }
      }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <div class="TableMain100">
            <div class="EHEADER" style="text-align: center;">
                <strong><span style="color: #000099">COMM_CDX Open Position Analysis</span></strong>
            </div>
        </div>
        <br class="clear" />
        <div class="pageContent">
            <div id="divPageFilter" style="width: 995px;">
                <div id="showFilter" class="right" style="width: 452px; background-color: #B7CEEC; border: solid 2px  #ccc; display: none;">
                    <div style="width: 100%">
                        <div class="frmleftContent">
                            <asp:TextBox ID="txtSelectionID" runat="server" Font-Size="12px" Height="20px" Width="350px"
                                TabIndex="0"></asp:TextBox>
                        </div>
                        <div class="frmleftContent">
                            <a id="A4" href="javascript:void(0);" tabindex="11" onclick="btnAddsubscriptionlist_click()">
                                <span style="color: #009900; text-decoration: underline; font-size: 10pt;">Add to List</span></a>
                        </div>
                    </div>
                    <span class="clear" style="background-color: #B7CEEC;"></span>
                    <div class="frmleftContent" style="height: 105px; margin-top: 5px">
                        <asp:ListBox ID="lstSelection" runat="server" Font-Size="12px" Height="100px" Width="440px"
                            TabIndex="0"></asp:ListBox>
                    </div>
                    <span class="clear" style="background-color: #B7CEEC;"></span>
                    <div class="frmleftContent" style="text-align: center">
                        <a id="A2" href="javascript:void(0);" tabindex="13" onclick="lnkBtnAddFinalSelection()">
                            <span style="color: #000099; text-decoration: underline; font-size: 10pt;">Done</span></a>&nbsp;&nbsp;
                            <a id="A1" href="javascript:void(0);" tabindex="14" onclick="lnkBtnRemoveFromSelection()">
                                <span style="color: #cc3300; text-decoration: underline; font-size: 10pt;">Remove</span></a>
                    </div>
                </div>
                <div id="dvMainFilter" class="frmContent" style="width: 528px">
                    <div id="forDate">
                        <div class="frmleftContent" style="width: 110px; line-height: 20px">
                            <asp:Label ID="lblDate" runat="server" Text="Date : "></asp:Label>
                        </div>
                        <div class="left">
                            <div class="frmleftContent">
                                <dxe:ASPxDateEdit ID="dtPosition" runat="server" ClientInstanceName="cdtPosition"
                                    DateOnError="Today" EditFormat="Custom" EditFormatString="dd-MM-yyyy" UseMaskBehavior="True"
                                    Width="125px" Height="25px" Font-Size="11px" TabIndex="0">
                                    <DropDownButton Text="Position">
                                    </DropDownButton>
                                    <ClientSideEvents DateChanged="function(s,e){DateChange(cdtPosition);}"></ClientSideEvents>
                                </dxe:ASPxDateEdit>
                            </div>
                        </div>
                    </div>
                    <span class="clear"></span>
                    <div id="dvGroupBy">
                        <div class="frmleftContent" style="width: 110px; line-height: 20px">
                            <asp:Label ID="lblGroupBy" runat="server" Text="Group By : "></asp:Label>
                        </div>
                        <div class="left">
                            <div class="frmleftContent" style="padding-top: 3px">
                                <asp:DropDownList ID="ddlGroup" runat="server" Width="125px" Font-Size="13px" onchange="fnddlGroup(this.value)"
                                    TabIndex="0">
                                    <asp:ListItem Value="0">Branch</asp:ListItem>
                                    <asp:ListItem Value="1">Group</asp:ListItem>
                                    <asp:ListItem Value="2">Clients</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div id="dvBranch" class="frmleftContent" style="width: 125px; padding-top: 3px; font-size: 12px;">
                                <asp:RadioButton ID="rdbranchclientAll" runat="server" Checked="True" GroupName="a"
                                    onclick="fn_BranchClient('a')" TabIndex="0" />
                                All
                                    <asp:RadioButton ID="rdbranchclientSelected" runat="server" GroupName="a" onclick="fn_BranchClient('b')"
                                        TabIndex="0" />Selected
                            </div>
                            <div id="dvGroup" class="left" style="display: none;">
                                <div class="left">
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <div class="frmleftContent">
                                                <asp:DropDownList ID="ddlgrouptype" AutoPostBack="true" runat="server" Width="125px"
                                                    Font-Size="13px" onchange="fngrouptype(this.value)" TabIndex="0">
                                                </asp:DropDownList>
                                            </div>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnhide" EventName="Click"></asp:AsyncPostBackTrigger>
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                                <div id="dvGroupTypeByAllSelect" class="frmleftContent" style="display: none; width: 125px; font-size: 12px">
                                    <asp:RadioButton ID="rdddlgrouptypeAll" runat="server" Checked="True" GroupName="b"
                                        onclick="fn_Group('a')" TabIndex="0" />
                                    All
                                        <asp:RadioButton ID="rdddlgrouptypeSelected" runat="server" GroupName="b" onclick="fn_Group('b')"
                                            TabIndex="0" />Selected
                                </div>
                            </div>
                        </div>
                    </div>
                    <span class="clear"></span>
                    <div id="divRptType">
                        <div class="frmleftContent" style="width: 110px; line-height: 20px">
                            <asp:Label ID="lblRptType" runat="server" Text="Report Type : "></asp:Label>
                        </div>
                        <div class="frmleftContent">
                            <asp:DropDownList ID="ddlRptType" runat="server" Width="240px" Font-Size="13px" TabIndex="0">
                                <asp:ListItem Value="0" Selected="True">Select Report Type</asp:ListItem>
                                <asp:ListItem Value="Summary">Summary</asp:ListItem>
                                <asp:ListItem Value="Detail">Detail Working</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <span class="clear"></span>
                    <div id="divExport">
                        <div class="frmleftContent" style="width: 110px; line-height: 20px">
                            <asp:Label ID="Label1" runat="server" Text="Report Generate : "></asp:Label>
                        </div>
                        <div class="frmleftContent">
                            <asp:Button ID="btnExport" TabIndex="0" runat="server" Text="Excel Export" OnClick="btnExport_Click"
                                CssClass="frmbtn" />
                        </div>
                    </div>
                </div>
            </div>
            <br class="clear" />
            <div style="display: none">
                <asp:HiddenField ID="hdnDPSessionValue" runat="server" />
                <asp:TextBox ID="txtSelectionID_hidden" runat="server" Font-Size="12px" Width="150px"></asp:TextBox>
                <asp:Button ID="btnhide" runat="server" Text="Button" OnClick="btnhide_Click" />
                <asp:HiddenField ID="HiddenField_Group" runat="server" />
                <asp:HiddenField ID="HiddenField_Branch" runat="server" />
                <asp:HiddenField ID="HiddenField_Client" runat="server" />
            </div>
        </div>
    </div>
</asp:Content>
