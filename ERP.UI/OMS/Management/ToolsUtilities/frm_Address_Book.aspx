<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.ToolsUtilities.management_utilities_frm_Address_Book" EnableEventValidation="false" CodeBehind="frm_Address_Book.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--    
    <script type="text/javascript" src="/assests/js/init.js"></script>
    <script type="text/javascript" src="/assests/js/jquery-1.3.2.js"></script>
    
    <script type="text/javascript" src="/assests/js/ajax-dynamic-list.js"></script>--%>

    <script language="javascript" type="text/javascript">
        FieldName = 'txtClientID';
        function ShowTab_ShowFilter(obj) {
            if (obj == "y") {
                Hide('Tab_showFilter');
            }
            else if (obj == "x") {
                //alert('abcd');
                //if(document.getElementById('ddlGroupBy').value=='Branch')
                document.getElementById('cmbsearchOption').value = 'Branch';
                //alert('cmbsearchOption');
                Show('Tab_showFilter');
            }
        }

        var contType = '';
        var addType = '';
        function Hide(obj) {
            document.getElementById(obj).style.display = 'none';
        }
        function Show(obj) {
            document.getElementById(obj).style.display = 'inline';
        }


        function FnddlGroupBy(obj) {

            if (obj == 'Group') {
                Hide('Td_OtherGroupBy');
                Show('Td_Group');

                document.getElementById('BtnGroup').click();
            }
            else {
                Show('Td_OtherGroupBy');
                Hide('Td_Group');

            }
        }
        function FnOtherGroupBy(obj) {
            Hide('Td_Group');
            if (obj == "a")
                Hide('Tab_showFilter');

            else {
                if (document.getElementById('ddlGroupBy').value == 'Clients')
                    document.getElementById('cmbsearchOption').value = 'Clients';
                if (document.getElementById('ddlGroupBy').value == 'Branch')
                    document.getElementById('cmbsearchOption').value = 'Branch';
                Show('Tab_showFilter');
            }

        }
        function FnGroup(obj) {
            Show('Td_Group');
            if (obj == "a")
                Hide('Tab_showFilter');
            else {
                document.getElementById('cmbsearchOption').value = 'Group';
                Show('Tab_showFilter');
            }

        }
        function selecttion() {
            var combo = document.getElementById('cmbExport');
            combo.value = '0';
        }
        //function SignOff() {
        //    window.parent.SignOff();
        //}

        //function height() {

        //    if (document.body.scrollHeight >= 600)
        //        window.frameElement.height = document.body.scrollHeight;
        //    else
        //        window.frameElement.height = '700px';
        //    window.frameElement.Width = document.body.scrollWidth;
        //}
        function ValidatePage() {

            selecttion();

        }



        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }


        function SearchOpt(obj) {

            var cmbt = document.getElementById('cmbDuplicate');
            //var ID=document.getElementById('ddlcountry');

            if (cmbt.value == 'None') {
                //alert('1');
                Show("Td_KeyWord");
                document.getElementById("Td_Specific").style.display = "none";
                document.getElementById("td_add1").style.display = "none";
                document.getElementById("td_print").style.display = "none";
                document.getElementById("tr_grp").style.display = "none";
                document.getElementById("Tab_showFilter").style.display = "none";
                document.getElementById("Td_Group").style.display = "none";
                document.getElementById("tblBulk").style.display = "none";
                document.getElementById("td_type").style.display = "none";
                //alert('2');
                document.getElementById("Td_branch").style.display = "none";
                //alert('3');
            }

            else if (cmbt.value == 'PANEXEMPT') {
                document.getElementById("Td_KeyWord").style.display = "none";
                document.getElementById("Td_Specific").style.display = "inline";
                document.getElementById("td_add1").style.display = "inline";
                document.getElementById("td_print").style.display = "none";
                document.getElementById("tr_grp").style.display = "none";
                document.getElementById("Tab_showFilter").style.display = "none";
                document.getElementById("Td_Group").style.display = "none";
                // document.getElementById("tr_cou").style.display="none";
                document.getElementById("tblBulk").style.display = "none";
                document.getElementById("td_type").style.display = "none";
                document.getElementById("Td_branch").style.display = "none";
            }

            else {

                document.getElementById("Td_KeyWord").style.display = "none";
                document.getElementById("Td_Specific").style.display = "none";
                document.getElementById("td_add1").style.display = "inline";
                document.getElementById("td_print").style.display = "inline";
                document.getElementById("tr_grp").style.display = "none";
                document.getElementById("Td_Group").style.display = "inline";
                // document.getElementById("tr_cou").style.display="inline";
                document.getElementById("tblBulk").style.display = "none";
                document.getElementById("td_type").style.display = "table-row";
                document.getElementById("Td_branch").style.display = "none";
                //alert(ID);

            }

        }

        function SearchOpt1(obj) {
            var ID = document.getElementById('ddlcountry').value;

            if (ID == '1') {
                //alert(ID);
                document.getElementById("tr_grp").style.display = "inline";
                document.getElementById("Td_Group").style.display = "none";
                //document.getElementById("Td_branch").style.display="none";
                //document.getElementById("td_add1").style.display="inline";

            }
            else {
                //alert('4');
                document.getElementById("tr_grp").style.display = "none";
                document.getElementById("Td_Group").style.display = "none";
                ////////////////////////////////////////document.getElementById("Td_branch").style.display="inline";
                // document.getElementById("td_add1").style.display="none";
            }

        }

        function btnSearch_click() {
            document.getElementById('TrFilter').style.display = "none";
            grid.PerformCallback('s');
        }
        function abcd(obj1, Obj2, obj3) {
            var strQuery_Table = '';
            var strQuery_FieldName = '';
            var strQuery_WhereClause = '';
            var strQuery_OrderBy = '';
            var strQuery_GroupBy = '';
            var CombinedQuery = '';

            if (document.getElementById('cmbsearchOption').value == "Clients") {

                // alert('456');
                strQuery_Table = "tbl_master_contact,tbl_master_contacttype,tbl_master_branch";
                strQuery_FieldName = "distinct top 10  isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_shortname),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(CNT_UCC),'')+'] ['+isnull(rtrim( BRANCH_DESCRIPTION),'')+'] ['+isnull(rtrim( cnttpy_contacttype),'')+']' ,cnt_internalID";
                strQuery_WhereClause = "  branch_id=cnt_branchid and cnt_prefix=cnt_contacttype and (CNT_UCC Like (\'%RequestLetter%') or CNT_FIRSTNAME like (\'%RequestLetter%') OR CNT_SHORTNAME like (\'%RequestLetter%')) and cnt_branchid in (<%=Session["userbranchHierarchy"]%>)";
                //}

            }

            else if (document.getElementById('cmbsearchOption').value == "Branch") {
                //alert ('1234');
                strQuery_Table = "tbl_master_branch";
                strQuery_FieldName = "top 10 branch_description+'-'+branch_code,branch_id";
                strQuery_WhereClause = " (branch_description Like (\'%RequestLetter%') or branch_code like (\'%RequestLetter%')) and branch_id in (<%=Session["userbranchHierarchy"]%>)";
            }

            else if (document.getElementById('cmbsearchOption').value == "Group") {
                //alert ('123');
                strQuery_Table = "tbl_master_groupmaster";
                strQuery_FieldName = "top 10 gpm_description+'-'+gpm_code ,gpm_id";
                strQuery_WhereClause = " (gpm_description Like (\'%RequestLetter%') or gpm_code like (\'%RequestLetter%')) and gpm_type='" + document.getElementById('ddlGroup').value + "'";
            }


            //          else if(obj3=='asd')
            //                    {
            //                    //alert('123456');
            //       strQuery_Table = "tbl_master_contact,tbl_master_contacttype,tbl_master_branch";
            //       strQuery_FieldName = "distinct top 10  isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_shortname),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(CNT_UCC),'')+'] ['+isnull(rtrim( BRANCH_DESCRIPTION),'')+'] ['+isnull(rtrim( cnttpy_contacttype),'')+']' ,cnt_internalID";
            //       strQuery_WhereClause = "  branch_id=cnt_branchid and cnt_prefix=cnt_contacttype and (CNT_UCC Like (\'%RequestLetter%') or CNT_FIRSTNAME like (\'%RequestLetter%') OR CNT_SHORTNAME like (\'%RequestLetter%')) and cnt_branchid in (<%=Session["userbranchHierarchy"]%>)";
            //       }
            CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);
            //alert(CombinedQuery);
            //ajax_showOptions(obj1,'GenericAjaxList',obj3,replaceChars(CombinedQuery));
            ajax_showOptions(obj1, 'GenericAjaxList', obj3, replaceChars(CombinedQuery));
            //alert(ajax_showOptions);
        }
        function replaceChars(entry) {
            out = "+";
            add = "--";
            temp = "" + entry;

            while (temp.indexOf(out) > -1) {
                pos = temp.indexOf(out);
                temp = "" + (temp.substring(0, pos) + add +
                temp.substring((pos + out.length), temp.length));
            }
            return temp;
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

            Hide('Tab_showFilter');
            document.getElementById('BtnGenerate').disabled = false;
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
        function btnShow_Click() {
            var cmbt = document.getElementById('cmbDuplicate');
            if (cmbt.value == 'Print')
                document.getElementById("tblBulk").style.display = "inline";
        }
    </script>

    <script type="text/ecmascript">
        function ReceiveServerData(rValue) {
            var j = rValue.split('~');

            if (j[0] == 'Branch')
                document.getElementById('HiddenField_Branch').value = j[1];
            if (j[0] == 'Group')
                document.getElementById('HiddenField_Group').value = j[1];
            if (j[0] == 'Clients')
                document.getElementById('HiddenField_Client').value = j[1];


        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Address Book/Label Printing</h3>
        </div>
    </div>
    <div class="form_main">

        <script language="javascript" type="text/javascript">
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_initializeRequest(InitializeRequest);
            prm.add_endRequest(EndRequest);
            var postBackElement;
            function InitializeRequest(sender, args) {
                if (prm.get_isInAsyncPostBack())

                    args.set_cancel(true);
                postBackElement = args.get_postBackElement();
                $get('UpdateProgress1').style.display = 'block';

            }
            function EndRequest(sender, args) {
                $get('UpdateProgress1').style.display = 'none';
            }
        </script>

        <table class="TableMain100">
            <tr>
                <td style="vertical-align: top; padding-left: 30px; padding-bottom: 15px;">
                    <div class="crossBtn"><a href="../ProjectMainPage.aspx"><i class="fa fa-times"></i></a></div>
                </td>
            </tr>

            <tr>
                <td class="gridcellleft">
                    <table cellpadding="1" cellspacing="1">
                        <tr>
                            <td class="gridcellleft">Find By
                            </td>
                            <td class="gridcellleft">
                                <asp:DropDownList ID="cmbDuplicate" Width="150px" runat="server" onchange="SearchOpt(this.value)">
                                    <asp:ListItem Text="Keyword" Value="None"></asp:ListItem>
                                    <asp:ListItem Text="Specific Contact" Value="PANEXEMPT"></asp:ListItem>
                                    <asp:ListItem Text="Label Print" Value="Print"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <table>
                                    <tr valign="top">
                                        <td id="Td_KeyWord">
                                            <asp:TextBox ID="txtClientID" runat="server" Width="300px" TabIndex="1"></asp:TextBox></td>
                                        <td id="Td_Specific">
                                            <asp:TextBox ID="txtClientID1" runat="server" Width="300px" TabIndex="1" onkeyup="abcd(this,event,'asd')"></asp:TextBox></td>
                                    </tr>
                                </table>
                            </td>
                            <td class="gridcellright">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnSave" runat="server" Text="Show" CssClass="btnUpdate btn btn-primary"
                                                OnClick="btnSave_Click" OnClientClick="btnShow_Click();" />
                                        </td>
                                        <td>
                                            <asp:Button ID="btnExport" runat="server" Text="Export to Excel" CssClass="btnUpdate btn btn-primary"
                                                OnClick="btnExport_Click" />
                                        </td>
                                        <td id="tblBulk" runat="server">
                                            <asp:Button ID="btnPrint" runat="server" Text="Print" Width="150px" CssClass="btnUpdate btn btn-primary" OnClick="btnPrint_Click1" />
                                        </td>
                                        <td id="td_print" runat="server"></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="td_type">
                            <td colspan="4">
                                <div class="col-md-3"><label>Contact Type</label>
                                    <asp:DropDownList ID="ddlcountry" Font-Size="12px" Width="150px" runat="server" onchange="SearchOpt1(this.value)">
                                </asp:DropDownList>
                                </div>
                            </td>
                            
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="Td_branch" runat="server">
                <td >
                    <div></div>
                    <table border="10" cellpadding="1" cellspacing="1">
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Branch</td>
                                        <td class="gridcellleft">
                                            <table border="10" cellpadding="1" cellspacing="1">
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="Rdbranchall" runat="server" GroupName="a" onclick="ShowTab_ShowFilter('y')" Checked="True" />All
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="Rdbranchselected" runat="server" GroupName="a" onclick="ShowTab_ShowFilter('x')" />Selected
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
                <td class="gridcellleft">
                    <table border="10" cellpadding="1" cellspacing="1">
                        <tr id="tr_grp" runat="server">
                            <td class="gridcellleft" bgcolor="#B7CEEC">Group By</td>
                            <td>
                                <asp:DropDownList ID="ddlGroupBy" runat="server" Width="180px" Font-Size="12px" onchange="FnddlGroupBy(this.value)">
                                    <asp:ListItem Value="Select">Select</asp:ListItem>
                                    <asp:ListItem Value="Clients">Customers</asp:ListItem>
                                    <asp:ListItem Value="Branch">Branch</asp:ListItem>
                                    <asp:ListItem Value="Group">Group</asp:ListItem>

                                </asp:DropDownList>
                            </td>
                            <td class="gridcellleft" id="Td_OtherGroupBy">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="RadioBtnOtherGroupByAll" runat="server" Checked="True" GroupName="a"
                                                onclick="FnOtherGroupBy('a')" />
                                            All
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="RadioBtnOtherGroupBySelected" runat="server" GroupName="a" onclick="FnOtherGroupBy('b')" />Selected
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="RadioBtnOtherGroupByallbutSelected" runat="server" Style="display: none" GroupName="a"
                                                onclick="FnOtherGroupBy('c')" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="gridcellleft" id="Td_Group">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddlGroup" runat="server" Font-Size="12px">
                                                    </asp:DropDownList>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="BtnGroup" EventName="Click"></asp:AsyncPostBackTrigger>
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="RadioBtnGroupAll" runat="server" Checked="True" GroupName="b"
                                                onclick="FnGroup('a')" />
                                            All
                                                <asp:RadioButton ID="RadioBtnGroupSelected" runat="server" GroupName="b" onclick="FnGroup('b')" />Selected
                                                <asp:RadioButton ID="RadioBtnGroupallbutSelected" runat="server" Style="display: none" GroupName="b" onclick="FnGroup('c')" />

                                        </td>
                                    </tr>
                                </table>
                            </td>

                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td valign="top">
                    <table id="Tab_showFilter">
                        <tr>
                            <td valign="top">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtSelectionID" runat="server" Font-Size="12px" Width="300px" onkeyup="abcd(this,event,'Other')"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="cmbsearchOption" runat="server" Font-Size="11px" Width="70px"
                                                Enabled="false">
                                                <asp:ListItem>Clients</asp:ListItem>
                                                <asp:ListItem>Branch</asp:ListItem>
                                                <asp:ListItem>Group</asp:ListItem>


                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <a id="A3" href="javascript:void(0);" onclick="btnAddsubscriptionlist_click()"><span
                                                style="color: #2554C7; text-decoration: underline; font-size: 8pt;"><b>Add to List</b></span></a><span
                                                    style="color: #009900; font-size: 8pt;"> </span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:ListBox ID="lstSlection" runat="server" Font-Size="12px" Height="100px" Width="400px"></asp:ListBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center">
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <a id="A5" href="javascript:void(0);" onclick="clientselectionfinal()"><span style="color: #000099; text-decoration: underline; font-size: 10pt;">Done</span></a>&nbsp;&nbsp;
                                        </td>
                                        <td>
                                            <a id="A6" href="javascript:void(0);" onclick="btnRemovefromsubscriptionlist_click()">
                                                <span style="color: #cc3300; text-decoration: underline; font-size: 8pt;">Remove</span></a>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="td_add1" runat="server">

                <td class="gridcellleft">
                    <table border="10" cellpadding="1" cellspacing="1">
                        <tr>
                            <td class="">Address Type
                            </td>
                            <td class="">
                                <asp:DropDownList ID="cmbadd" Width="150px" runat="server">
                                    <asp:ListItem Text="All" Value="All"></asp:ListItem>
                                    <asp:ListItem Text="No Preference" Value="No Preference"></asp:ListItem>
                                    <asp:ListItem Text="Registered" Value="Registered"></asp:ListItem>
                                    <asp:ListItem Text="Residence" Value="Residence"></asp:ListItem>
                                    <asp:ListItem Text="Correspondence" Value="Correspondence"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>

                    </table>

                </td>
            </tr>





            <tr>
                <td>
                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                        <ProgressTemplate>
                            <div id='Div1' style='position: absolute; font-family: arial; font-size: 30; left: 50%; top: 8; background-color: white; layer-background-color: white; height: 80; width: 150;'>
                                <table width='100' height='35' border='1' cellpadding='0' cellspacing='0' bordercolor='#C0D6E4'>
                                    <tr>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td height='25' align='center' bgcolor='#FFFFFF'>
                                                        <img src='/assests/images/progress.gif' width='18' height='18'></td>
                                                    <td height='10' width='100%' align='center' bgcolor='#FFFFFF'>
                                                        <font size='2' face='Tahoma'><strong align='center'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Loading..</strong></font></td>
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
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table class="TableMain100">
                                <tr>
                                    <td>
                                        <dxe:ASPxGridView ID="gridContract" ClientInstanceName="grid" Width="100%" KeyFieldName="CustomerID"
                                            runat="server" AutoGenerateColumns="False" OnCustomCallback="gridContract_CustomCallback">
                                            <SettingsBehavior AllowFocusedRow="True" ConfirmDelete="True"  />
                                            <Styles>
                                                <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                </Header>
                                                <LoadingPanel ImageSpacing="10px">
                                                </LoadingPanel>
                                                <FocusedRow BackColor="#FEC6AB">
                                                </FocusedRow>
                                            </Styles>
                                            <Columns>
                                                <dxe:GridViewDataTextColumn Visible="False" FieldName="CustomerID" Caption="ID">
                                                    <CellStyle CssClass="gridcellleft">
                                                    </CellStyle>
                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="ClientName" Caption="Name">
                                                    <CellStyle Wrap="True" CssClass="gridcellleft">
                                                    </CellStyle>
                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="Ucc" Caption="Ucc">
                                                    <CellStyle Wrap="True" CssClass="gridcellleft">
                                                    </CellStyle>
                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="AddRess1" Caption="Address1">
                                                    <CellStyle Wrap="True" CssClass="gridcellleft">
                                                    </CellStyle>
                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="AddRess2" Caption="Address2">
                                                    <CellStyle Wrap="True" CssClass="gridcellleft">
                                                    </CellStyle>
                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="AddRess3" Caption="Address3">
                                                    <CellStyle Wrap="True" CssClass="gridcellleft">
                                                    </CellStyle>
                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="landmark" Caption="Landmark">
                                                    <CellStyle Wrap="True" CssClass="gridcellleft">
                                                    </CellStyle>
                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn VisibleIndex="8" FieldName="CountryName" Caption="Country">
                                                    <CellStyle Wrap="True" CssClass="gridcellleft">
                                                    </CellStyle>
                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="StateName" Caption="State">
                                                    <CellStyle Wrap="True" CssClass="gridcellleft">
                                                    </CellStyle>
                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="CityName" Caption="City">
                                                    <CellStyle Wrap="True" CssClass="gridcellleft">
                                                    </CellStyle>
                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn VisibleIndex="8" FieldName="AreaName" Caption="Area">
                                                    <CellStyle Wrap="True" CssClass="gridcellleft">
                                                    </CellStyle>
                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn VisibleIndex="8" FieldName="pin" Caption="Pin">
                                                    <CellStyle Wrap="True" CssClass="gridcellleft">
                                                    </CellStyle>
                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn VisibleIndex="8" FieldName="type1" Caption="CntType">
                                                    <CellStyle Wrap="True" CssClass="gridcellleft">
                                                    </CellStyle>
                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn VisibleIndex="8" FieldName="AddType" Caption="Addresstype">
                                                    <CellStyle Wrap="True" CssClass="gridcellleft">
                                                    </CellStyle>
                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                </dxe:GridViewDataTextColumn>
                                            </Columns>
                                            <StylesEditors>
                                                <ProgressBar Height="25px">
                                                </ProgressBar>
                                            </StylesEditors>
                                            <SettingsBehavior AllowFocusedRow="True" AllowSort="False" AllowMultiSelection="True" />
                                            <SettingsPager AlwaysShowPager="True" NumericButtonCount="20" ShowSeparators="True"
                                                PageSize="15">
                                                <FirstPageButton Visible="True">
                                                </FirstPageButton>
                                                <LastPageButton Visible="True">
                                                </LastPageButton>
                                            </SettingsPager>
                                            <SettingsText Title="Template" />
                                            <SettingsSearchPanel Visible="True" />
                                            <Settings ShowGroupedColumns="True" ShowGroupPanel="True" ShowFilterRow="true" ShowFilterRowMenu ="true" />
                                        </dxe:ASPxGridView>
                                        <dxe:ASPxGridViewExporter ID="exporter" runat="server">
                                        </dxe:ASPxGridViewExporter>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
        <table>
            <tr style="display: none;">
                <td>
                    <asp:Button ID="BtnGroup" runat="server" Text="BtnGroup" OnClick="BtnGroup_Click" />
                    <asp:TextBox ID="txtSelectionID_hidden" runat="server" Font-Size="12px" Width="150px"></asp:TextBox>
                    <asp:TextBox ID="txtClientID1_hidden" runat="server" Font-Size="12px" Width="150px"></asp:TextBox>
                    <asp:HiddenField ID="HiddenField_Group" runat="server" />
                    <asp:HiddenField ID="HiddenField_Branch" runat="server" />
                    <asp:HiddenField ID="HiddenField_Client" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <table>

                        <%-- <td>
                                <table class="TableMain100" id="tblBulk">
                                    <tr>
                                        <td>
                                            <table class="TableMain">
                                                <tr>
                                                    <td>
                                                        <dxe:ASPxComboBox runat="server" ValueType="System.String" Width="120px" Font-Size="12px"
                                                            ID="cmbContact" EnableIncrementalFiltering="True">
                                                            <DropDownButton Text="ContactType" Width="50px">
                                                            </DropDownButton>
                                                            <ButtonStyle Width="13px">
                                                            </ButtonStyle>
                                                        </dxe:ASPxComboBox>
                                                    </td>
                                                    <td>
                                                        <dxe:ASPxComboBox runat="server" ValueType="System.String" Width="150px" Font-Size="12px"
                                                            ID="cmbclient" EnableIncrementalFiltering="True">
                                                            <DropDownButton Text="Single client" Width="50px">
                                                            </DropDownButton>
                                                            <ButtonStyle Width="13px">
                                                            </ButtonStyle>
                                                        </dxe:ASPxComboBox>
                                                    </td>
                                                    <td>
                                                        <dxe:ASPxComboBox ID="cmbState" DataSourceID="SDSState" TextField="state" ValueField="id"
                                                            runat="server" Font-Size="12px" ValueType="System.String" Width="120px" EnableCallbackMode="True"
                                                            EnableIncrementalFiltering="True">
                                                            <ButtonStyle Width="13px">
                                                            </ButtonStyle>
                                                            <DropDownButton Text="State" Width="50px">
                                                            </DropDownButton>
                                                            <ClientSideEvents SelectedIndexChanged="function(s, e) { OnStateChanged(s); }" />
                                                        </dxe:ASPxComboBox>
                                                    </td>
                                                    <td>
                                                        <dxe:ASPxComboBox runat="server" ValueType="System.String" DataSourceID="SDSCity"
                                                            TextField="city_name" ValueField="city_id" Width="120px" Font-Size="12px" ID="cmbCity"
                                                            OnCallback="cmbCity_Callback" DropDownStyle="DropDown" EnableCallbackMode="True"
                                                            EnableIncrementalFiltering="True">
                                                            <DropDownButton Text="City" Width="50px">
                                                            </DropDownButton>
                                                            <ButtonStyle Width="20px">
                                                            </ButtonStyle>
                                                            <ClientSideEvents SelectedIndexChanged="function(s, e) { docprint1(s); }" />
                                                        </dxe:ASPxComboBox>
                                                    </td>
                                                    <td>
                                                        <dxe:ASPxComboBox ID="cmbArea" DataSourceID="SDSArea" TextField="area_name" ValueField="area_id"
                                                            runat="server" Font-Size="12px" ValueType="System.String" Width="120px" EnableCallbackMode="True"
                                                            OnCallback="cmbArea_Callback" EnableIncrementalFiltering="True">
                                                            <DropDownButton Text="Area" Width="50px">
                                                            </DropDownButton>
                                                            <ButtonStyle Width="13px">
                                                            </ButtonStyle>
                                                        </dxe:ASPxComboBox>
                                                    </td>
                                                    <td>
                                                        <dxe:ASPxComboBox ID="cmbAddressType" runat="server" Font-Size="12px" ValueType="System.String"
                                                            Width="120px" EnableIncrementalFiltering="True">
                                                            <DropDownButton Text="AddressType" Width="50px">
                                                            </DropDownButton>
                                                            <ButtonStyle Width="13px">
                                                            </ButtonStyle>
                                                            <Items>
                                                                <dxe:ListEditItem Text="All" Value="All" />
                                                                <dxe:ListEditItem Text="Residence" Value="Regidence" />
                                                                <dxe:ListEditItem Text="Correspondence" Value="Correspondence" />
                                                                <dxe:ListEditItem Text="Registered" Value="Office" />
                                                            </Items>
                                                        </dxe:ASPxComboBox>
                                                    </td>
                                                    <td>
                                                        <dxe:ASPxComboBox runat="server" ValueType="System.String" Width="120px" Font-Size="12px"
                                                            ID="cmbBranch" EnableIncrementalFiltering="True">
                                                            <DropDownButton Text="Branch" Width="50px">
                                                            </DropDownButton>
                                                            <ButtonStyle Width="13px">
                                                            </ButtonStyle>
                                                        </dxe:ASPxComboBox>
                                                    </td>
                                                    <td>
                                                        <dxe:ASPxButton ID="btnPrint" runat="server" Text="Label Print" AutoPostBack="False"
                                                            Width="80px">
                                                            <ClientSideEvents Click="function(s,e){ goPrintPage();}" />
                                                        </dxe:ASPxButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>--%>
                    </table>
                </td>
            </tr>
        </table>


    </div>
    <asp:SqlDataSource ID="SDSAddMaster" runat="server"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SDSAddDetails" runat="server" ></asp:SqlDataSource>
    <asp:SqlDataSource ID="SDSState" runat="server" ></asp:SqlDataSource>
    <asp:SqlDataSource ID="SDSCity" runat="server"
        SelectCommand="SELECT c.city_id AS CityId, c.city_name AS City FROM tbl_master_city c where c.state_id=@City order by c.city_name">
        <SelectParameters>
            <asp:Parameter Name="City" Type="string" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SDSArea" runat="server"
        SelectCommand="SELECT area_id as areaId,area_name as Area from tbl_master_area as tma where tma.city_id = @Area order by tma.area_name">
        <SelectParameters>
            <asp:Parameter Name="Area" Type="string" />
        </SelectParameters>
    </asp:SqlDataSource>

    <script language="ecmascript" type="text/ecmascript">
        function OnStateChanged(cmbState) {
            cmbCity.PerformCallback(cmbState.GetValue().toString());
        }
        function OnCityChanged(cmbCity) {
            cmbArea.PerformCallback(cmbCity.GetValue().toString());
        }
        function FnHideShow(cmbSearch) {
            if (cmbSearch.GetValue().toString() == "1") {
                document.getElementById("tblSingle").style.display = 'inline';
                document.getElementById("tblBulk").style.display = 'none';
                document.getElementById("tblMasterAddress").style.display = 'none';
            }
            else {
                document.getElementById("tblBulk").style.display = 'inline';
                document.getElementById("tblSingle").style.display = 'none';
                document.getElementById("tblMasterAddress").style.display = 'none';
            }
        }
    </script>
</asp:Content>
