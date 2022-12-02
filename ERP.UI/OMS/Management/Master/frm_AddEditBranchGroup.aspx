<%@ Page Title="Branch Groups" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.Master.management_master_frm_AddEditBranchGroup" CodeBehind="frm_AddEditBranchGroup.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <%--     <script type="text/javascript" src="/assests/js/init.js"></script>

    <script type="text/javascript" src="/assests/js/ajax-dynamic-list.js"></script>--%>

    <script language="javascript" type="text/javascript">
        FieldName = null;

        //function height() {

        //    if (document.body.scrollHeight >= 500)
        //        window.frameElement.height = document.body.scrollHeight;
        //    else
        //        window.frameElement.height = '850px';
        //    window.frameElement.Width = document.body.scrollWidth;
        //}

        function GetBranches(obj1, obj2, obj3) {
            
            var strQuery_Table = "tbl_master_branch";
            var strQuery_FieldName = "top 10 (isnull(branch_description,\'\')+ \'-[\'+ isnull(branch_code,\'\') + \']\') as Branch,branch_id";
            var strQuery_WhereClause = " (branch_description Like (\'%RequestLetter%\') or branch_code Like (\'%RequestLetter%\')) and branch_id not in (select BranchGroupMembers_BranchID from trans_branchgroupmembers)";
            var strQuery_OrderBy = '';
            var strQuery_GroupBy = '';
            var CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);
            if (obj1.value == "") {
                obj1.value = "%";
            }
            ajax_showOptions(obj1, obj2, obj3, replaceChars(CombinedQuery));
            if (obj1.value == "%") {
                obj1.value = "";
            }
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

        function btnAddBranch() {
            var userid = document.getElementById('txtBranch');
            if (userid.value != '') {
                var ids = document.getElementById('txtBranch_hidden');
                var listBox = document.getElementById('lstBranches');
                var tLength = listBox.length;
                //
                var i;
                if (tLength > 0) {
                    for (i = 0; i < tLength; i++) {

                        if (listBox[i].value == ids.value) {
                            alert('This Branch is Already Added !');

                            return false;
                        }

                    }

                }
                //
                var no = new Option();
                no.value = ids.value;
                no.text = userid.value;
                listBox[tLength] = no;
                var recipient = document.getElementById('txtBranch');
                recipient.value = '';

                var listIDs = '';
                if (listBox.length > 0) {
                    for (i = 0; i < listBox.length; i++) {
                        if (listIDs == '')
                            listIDs = listBox.options[i].value + ';' + listBox.options[i].text;
                        else
                            listIDs += ',' + listBox.options[i].value + ';' + listBox.options[i].text;
                    }

                    // var sendData = cmb.value + '~' + listIDs;
                    CallServer(listIDs, "");

                }

            }
            else
                alert('Please search name and then Add!')
            var s = document.getElementById('txtBranch');
            s.focus();
            s.select();
        }

        function Branchselectionfinal() {

            var listBoxSubs = document.getElementById('lstBranches');
            // var cmb=document.getElementById('cmbsearchOption');
            var listIDs = '';
            var i;

            if (listBoxSubs.length > 0) {
                for (i = 0; i < listBoxSubs.length; i++) {
                    if (listIDs == '')
                        listIDs = listBoxSubs.options[i].value + ';' + listBoxSubs.options[i].text;
                    else
                        listIDs += ',' + listBoxSubs.options[i].value + ';' + listBoxSubs.options[i].text;
                }

                // var sendData = cmb.value + '~' + listIDs;
                CallServer(listIDs, "");

            }
            //	        var i;
            for (i = listBoxSubs.options.length - 1; i >= 0; i--) {
                listBoxSubs.remove(i);
            }

            // document.getElementById('TdFilter').style.visibility='hidden';
            // document.getElementById('TdFilter1').style.visibility='hidden';


        }

        function btnRemoveBranch() {

            var listBox = document.getElementById('lstBranches');
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

            var listIDs = '';
            var k;
            if (listBox.length > 0) {
                for (k = 0; k < listBox.length; k++) {
                    if (listIDs == '')
                        listIDs = listBox.options[k].value + ';' + listBox.options[k].text;
                    else
                        listIDs += ',' + listBox.options[k].value + ';' + listBox.options[k].text;
                }

                // var sendData = cmb.value + '~' + listIDs;
                CallServer(listIDs, "");

            }

        }


        function ReceiveServerData(rValue) {

            var Data = rValue.split('~');
            var NoItems = Data[0].split(';');
            var a = '';
            if (NoItems.length > 1) {

                var NoItemsDis = Data[1].split(',');
                for (i = 0; i < NoItemsDis.length; i++) {
                    if (i == 0) {
                        //                        var a=NoItemsDis[i];
                        var Dis = NoItemsDis[i].split(';');
                        a = Dis[1];
                    }
                    else {
                        var Dis = NoItemsDis[i].split(';');
                        a = a + ',' + Dis[1];
                    }

                }
            }

        }

        //function CheckValid() {
        //    var Nameval = document.getElementById('txtName').value;
        //    var Codeval = document.getElementById('txtCode').value;
        //    var listBox = document.getElementById('lstBranches');
        //    if (Nameval != '')
        //        if (Codeval != '')
        //            return true;
        //        else {
        //            alert('Please Insert Short Name !');
        //            return false;
        //        }
        //    else {
        //        alert('Please Insert BranchGroup Name');
        //        return false;
        //    }

        //}
        //if (/^ *$/.test(your_string)) {
        //    // Only spaces
        //}
        
        function CheckValid() {
             
            var Nameval = document.getElementById('txtName').value;
            var newNameval = /^ *$/.test(Nameval);
            var Codeval = document.getElementById('txtCode').value;
            var newCodeval = /^ *$/.test(Codeval);
            var lstBranches = document.getElementById('lstBranches');
           // var newlstBranches = /^ *$/.test(lstBranches);

            if (!newNameval)
                if(!newCodeval)
                    if (lstBranches.length>0)
                    {
                        return true;
                    }
                    else
                    {
                        alert('Branch name required');
                        return false;
                    }
                else
                {
                    alert('Short name required');
                    return false;
                }
            else
            {
                alert('Branch Group name required');
                return false;
            }
        }


        function Check() {
            //var txtBranch = document.getElementById('txtBranch').value;
            var lstBranches = document.getElementById('lstBranches');
            var Name = document.getElementById('txtName').value;
            var Code = document.getElementById('txtCode').value;
            var tLength = lstBranches.length;
            //var selectedValue = lstBranches.checked;
          
       
            if (Name.trim().length == 0) {
                $('#MandatoryName').css({ 'display': 'block' });
                return false;
            }
            else {
                $('#MandatoryName').css({ 'display': 'none' });
            }

            if (Code.trim().length == 0) {
                $('#MandatoryShortname').css({ 'display': 'block' });
                return false;
            }
            else {
                $('#MandatoryShortname').css({ 'display': 'none' });
            }
            //if (tLength > 0) {
            //    $('#MandatoryFileNo').attr('style', 'display:none;color:red; position:absolute;right:736px;top:240px');
            //}
            //else {
            //    $('#MandatoryFileNo').attr('style', 'display:block;color:red; position:absolute;right:736px;top:240px');
            //    return false;
            //}
            var count = 0;
            for (i = 0; i < tLength; i++) {
                if (lstBranches.options[i].selected== true) {
                    count++;
                }
               
            }
            if (count > 0) {
                $('#MandatoryFileNo').css({ 'display': 'none' });
            }
            else {
                $('#MandatoryFileNo').css({'display' : 'block'});
                return false;
            }
        }
            //alert(newNameval);
            //    var Codeval = document.getElementById('txtCode').value;
            //    var txtBranch = document.getElementById('txtBranch').value;
            //    var lstBranches = document.getElementById('lstBranches');
            //    if (Nameval!= '')
            //        if (Codeval != '')                        
            //            if (txtBranch != '')
            //                return true;
            //            else
            //            {
            //                alert('Please Insert Branch Name !');
            //                return false;
            //            }
            //        else
            //        {
            //            alert('Please Insert Short Name !');
            //            return false;
            //        }

            //    else {
            //        alert('Please Insert BranchGroup Name');
            //        return false;
            //    }

            //}

        function EditBranch(branchtext, branchvalue) {
            //alert(branchtext);
            var listBox = document.getElementById('lstBranches');
            var tLength = listBox.length;
            var no = new Option();
            no.value = branchvalue;
            no.text = branchtext;
            listBox[tLength] = no;

        }
        function ClosePage() {
            parent.editwin.close();

        }
    </script>
    <style type="text/css">
        .auto-style1 {
            height: 85px;
        }
        .mb0 {
            margin-bottom:0px !important;
        }
    </style>
    <script src="/assests/pluggins/choosen/choosen.min.js"></script>
    
    
    
    <script type="text/javascript">
        $(document).ready(function () {
            //$('#lstBranches').chosen();
            var config = {
                '#lstBranches': {},
                '#lstBranches-deselect': { allow_single_deselect: true },
                '#lstBranches-no-single': { disable_search_threshold: 10 },
                '#lstBranches-no-results': { no_results_text: 'Oops, nothing found!' },
                '#lstBranches-width': { width: "95%" }
            }
            for (var selector in config) {
                $(selector).chosen(config[selector]);
            }
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Branch Groups</h3>
            <div class="crossBtn"><a href="frm_BranchGroups.aspx"><i class="fa fa-times"></i></a></div>
        </div>

    </div>
    <div class="form_main" style="border:1px solid #ccc;">
        <table class="TableMain100">
            <%--            <tr>
                <td class="EHEADER" style="text-align: center; height: 20px;">
                    <strong><span style="color: #000099">Add/Edit Branch Groups</span></strong>
                </td>
            </tr>--%>
            <tr>
                <td>
                    <table class="pdtble">
                        <tr>
                            <td align="left" style="">Branch Group Name<span style="color:red">*</span>
                            </td>
                            <td align="left" style="position:relative">
                                <asp:TextBox ID="txtName" ClientIDMode="Static" runat="server" Width="253px" MaxLength="100"></asp:TextBox>
                                <span id="MandatoryName" class="pullleftClass fa fa-exclamation-circle iconRed " style="color:red; position:absolute;right:-12px;top:10px;display:none" title="Mandatory"></span>
                            </td>
                            <td>
                              <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtName" CssClass="pullleftClass fa fa-exclamation-circle" ErrorMessage="" ToolTip="Mandatory" ForeColor="Red" ValidationGroup="va" SetFocusOnError="True"></asp:RequiredFieldValidator>--%>
                                  
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td align="left">Short Name<span style="color:red">*</span>
                            </td>
                            <td align="left" style="position:relative">
                                <asp:TextBox ID="txtCode" ClientIDMode="Static" runat="server" Width="253px" MaxLength="50"></asp:TextBox>
                                <span id="MandatoryShortname" class="pullleftClass fa fa-exclamation-circle iconRed" style="color:red; position:absolute;right:-12px;top:10px;display:none" title="Mandatory"></span>
                            </td>
                            <td>
                              <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="pullleftClass fa fa-exclamation-circle" ErrorMessage="" ToolTip="Mandatory"  ControlToValidate="txtCode"
                                     ForeColor="Red" ValidationGroup="va" SetFocusOnError="True"></asp:RequiredFieldValidator>--%>
                                 
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <%--<td valign="top" align="left" style="padding-top: 7px">Add Branch<span style="color:red">*</span></td>--%>
                            <td valign="top" align="left" style="padding-top: 7px">Add Branch<span style="color:red">*</span></td>
                            <td align="left" colspan="3" style="padding:0">

                                <%--<table>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtBranch" runat="server" ClientIDMode="Static" onclick="GetBranches(this,'GenericAjaxList',event)" Width="253px"></asp:TextBox><asp:HiddenField
                                                ID="txtBranch_hidden" runat="server" />
                                            <a id="A4" href="javascript:void(0);" onclick="btnAddBranch()"><span
                                                style="color: #009900; text-decoration: underline; font-size: 8pt;">Add to List</span></a><span
                                                    style="color: #009900; font-size: 8pt;">&nbsp;</span>
                                        </td>
                                       
                                        <td id="TdFilter1" style="height: 23px; display: none; ">
                                            <asp:DropDownList ID="cmbsearchOption" runat="server" Width="85px" Font-Size="11px"
                                                Enabled="false">
                                                <asp:ListItem>Branch</asp:ListItem>

                                            </asp:DropDownList></td>
                                    </tr>
                                </table>--%>
                                <table cellpadding="0" cellspacing="0" id="TdSelect">
                                    <tr>
                                          
                                        <td class="" style="padding-bottom:0px;position:relative">
                                            <asp:ListBox ID="lstBranches" runat="server" Font-Size="12px" Height="90px" Width="253px" CssClass="mb0" data-placeholder="Select Branches..." SelectionMode="Multiple"></asp:ListBox>
                                            <span id="MandatoryFileNo" class="pullleftClass fa fa-exclamation-circle iconRed" style="color:red; position:absolute;right:-12px;top:10px;display:none" title="Mandatory"></span>
                                        </td>
                                        <td>
                                             
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: center;padding-top:0px" >
                                            <table cellpadding="0" cellspacing="0">
                                                <tr>
                                                    
                                                    <td style="padding-left:0;padding-top:0px">
                                                        <%--<a id="A1" href="javascript:void(0);" onclick="btnRemoveBranch()">
                                                            <span style="color: #cc3300; text-decoration: underline; font-size: 8pt;">Remove</span></a>--%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    </table>
                            </td>

                        </tr>
                        <tr>
                            <td></td>
                            <td align="left" colspan="3">
                              <%--  <asp:Button ID="btnSubmit" runat="Server" CssClass="btn btn-primary" Text="Submit" OnClientClick="return CheckValid()" OnClick="btnSubmit_Click" />--%>
                                  <asp:Button ID="btnSubmit" runat="Server" CssClass="btn btn-primary" Text="Submit" OnClientClick="return Check()" OnClick="btnSubmit_Click" ValidationGroup="va" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

