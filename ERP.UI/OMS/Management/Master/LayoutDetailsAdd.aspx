<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="LayoutDetailsAdd.aspx.cs" Inherits="ERP.OMS.Management.Master.LayoutDetailsAdd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .padding {
            width: 100%;
        }

            .padding > tbody > tr > td {
                padding: 5px 0px;
            }

        .cnt {
            width: 70%;
            margin: 0 auto;
        }
    </style>
    <script type="text/javascript">
        var CutedGroupId = 0;
        var IsPasteActive = false;

        function treeView_OnInit(s, e) {
            ProcessNode(s, s.GetRootNode());
        }

        function ProcessNode(tree, node) {
            var htmlElement = node.GetHtmlElement();
            var count = node.GetNodeCount();

            if (htmlElement != null) {
                var handler = function (evt) {
                    popupMenu_ToggleItemsVisibility(count > 0);

                    tree.SetSelectedNode(node);

                    popupMenu.cpClickedNode = node;
                    popupMenu.ShowAtElement(node.GetHtmlElement());

                    ASPxClientUtils.PreventEventAndBubble(evt);
                };
                ASPxClientUtils.AttachEventToElement(htmlElement, "contextmenu", handler);
            }

            for (var i = 0; i < count; i++)
                ProcessNode(tree, node.GetNode(i));
        }
        function getUrlVars() {
            var vars = [], hash;
            var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
            for (var i = 0; i < hashes.length; i++) {
                hash = hashes[i].split('=');
                vars.push(hash[0]);
                vars[hash[0]] = hash[1];
            }
            return vars;
        }

        function popupMenu_ToggleItemsVisibility(isParent) {

            var urlKeys = getUrlVars();
            if (urlKeys.Type == "View") {
                popupMenu.GetItemByName("itmExpandCollapse").SetVisible(false);
                popupMenu.GetItemByName("itmEnableDisable").SetVisible(false);
                //popupMenu.GetItemByName("itmLedgerMap").SetVisible(false);
                popupMenu.GetItemByName("itmDelete").SetVisible(false);
                popupMenu.GetItemByName("itmCut").SetVisible(false);
                popupMenu.GetItemByName("itmEdit").SetVisible(false);
                popupMenu.GetItemByName("itmPaste").SetVisible(false);
            }
            else {
                popupMenu.GetItemByName("itmExpandCollapse").SetVisible(true);
                popupMenu.GetItemByName("itmEnableDisable").SetVisible(true);
                //popupMenu.GetItemByName("itmLedgerMap").SetVisible(true);
                popupMenu.GetItemByName("itmDelete").SetVisible(true);
                popupMenu.GetItemByName("itmCut").SetVisible(true);
                popupMenu.GetItemByName("itmEdit").SetVisible(true);
                popupMenu.GetItemByName("itmExpandCollapse").SetVisible(isParent);
                popupMenu.GetItemByName("itmEnableDisable").SetVisible(!isParent);
                //popupMenu.GetItemByName("itmLedgerMap").SetVisible(!isParent);
                popupMenu.GetItemByName("itmDelete").SetVisible(!isParent);
                popupMenu.GetItemByName("itmCut").SetVisible(!isParent);
            }






        }

        function popupMenu_OnPopUp(s, e) {
            s.GetItemByName("itmExpandCollapse").SetText(s.cpClickedNode.GetExpanded() ? "Collapse" : "Expand");
            s.GetItemByName("itmEnableDisable").SetText(s.cpClickedNode.GetEnabled() ? "Disable" : "Enable");
            if (IsPasteActive == true) {
                // popupMenu.GetItemByName("itmPaste").SetVisible(true);
            }
            else {
                //popupMenu.GetItemByName("itmPaste").SetVisible(false);
            }
        }

        function popupMenu_OnItemClick(s, e) {




            if (e.item.name == "itmExpandCollapse")
                s.cpClickedNode.SetExpanded(!s.cpClickedNode.GetExpanded());

            if (e.item.name == "itmEnableDisable")
                s.cpClickedNode.SetEnabled(!s.cpClickedNode.GetEnabled());

            //if (e.item.name == "itmAddNew") {
            //    cAddEditPopUp.SetHeaderText("Add Sub Group");
            //    var Permisssion = true;
            //    var selectedNode = treeView.GetSelectedNode();
            //    var Parent1 = selectedNode.parent;
            //    if (Parent1 != null) {
            //        var parent2 = Parent1.parent;
            //        if (parent2 != null) {
            //            Permisssion = false;
            //        }
            //    }
            //    if (Permisssion) {
            //        var ID = selectedNode.name;
            //        var NodeCount = selectedNode
            //        $("#CallStatus").val('Add');
            //        $("#GroupId").val(ID);
            //        cGroupLabel.SetValue('Sub ');
            //        $("#ParentId").val("2");
            //        ClearPopUp();
            //        $('.HideDiv').hide();
            //        ctxtAccountShortName.Enabled = true;
            //        ccmbGroup.RemoveItem(3);
            //        ccmbGroup.RemoveItem(2);
            //        ccmbGroup.SetText("Income");
            //        cAddEditPopUp.Show();
            //        ctxtAccountShortName.SetEnabled(true);
            //        ccmbGroupType.SetEnabled(true);

            //    }
            //    else {
            //        jAlert('Cannot Add Account group after this level.');
            //    }
            //}
            if (e.item.name == "itmEdit") {
                var selectedNode = treeView.GetSelectedNode();
                var GroupId = selectedNode.name;
                $("#GroupId").val(GroupId);
                // $("#ParentId").val(lavel);
                ClearPopUp();


                $("#CallStatus").val("Edit");
                ccmbGroup.RemoveItem(3);
                ccmbGroup.RemoveItem(2);
                var Parent1 = selectedNode.parent;
                if (Parent1 == null) {
                    $('.HideDiv').show();
                    cAddEditPopUp.SetHeaderText("Edit Group");
                    if (!ccmbGroup.FindItemByText("Defined Total")) {
                        ccmbGroup.AddItem("Defined Total");
                    }
                    //if (!ccmbGroup.FindItemByText("Text"))
                    //    ccmbGroup.AddItem("Text");
                    if (!ccmbGroup.FindItemByText("Net Total")) {
                        ccmbGroup.AddItem("Net Total");
                    }
                }
                else {
                    $('.HideDiv').hide();
                    cAddEditPopUp.SetHeaderText("Edit Sub Group");
                }
                cAddEditPopUp.Show();

                cSelectPanel.PerformCallback('Edit~' + GroupId);
            }
            if (e.item.name == "itmDelete") {
                var selectedNode = treeView.GetSelectedNode();
                if (selectedNode.GetNodeCount() == 0) {
                    var GroupId = selectedNode.name;
                    $("#GroupId").val(GroupId);
                    ClearPopUp();
                    $("#CallStatus").val("Delete");
                    jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                        if (r == true) {
                            cSelectPanel.PerformCallback('Delete~' + GroupId);
                        }
                    });
                }
                else {
                    jAlert('You Cant Delete This Group. Because it has child node');
                }
            }
            if (e.item.name == "itmCut") {
                var selectedNode = treeView.GetSelectedNode();
                if (selectedNode.GetNodeCount() == 0) {
                    CutedGroupId = selectedNode.name;
                    IsPasteActive = true;
                }
                else {
                    jAlert('You Cant Cut This Group. Because it has child node');
                }
            }
            if (e.item.name == "itmPaste") {
                var selectedNode = treeView.GetSelectedNode();
                var CurrentGroupId = selectedNode.name;
                if (CutedGroupId != 0) {
                    cSelectPanel.PerformCallback('CutPaste~' + CutedGroupId + '~' + CurrentGroupId);
                    IsPasteActive = false;
                    CutedGroupId = 0;
                }
            }
            if (e.item.name == "itmLedgerMap") {
                var selectedNode = treeView.GetSelectedNode();
                var GroupId = selectedNode.name;
                OnMapLedger(GroupId);
            }

        }
        function UniqueCodeCheck() {

            var uniqueid = '0';
            var id = '<%= Convert.ToString(Session["id"]) %>';
            var LayoutIdPass = '0';
            var LayoutId = '<%= Convert.ToInt32(Session["LayoutId"]) %>';

            if (LayoutId != '' && LayoutId != null) {
                var LayoutIdPass = LayoutId;
            }
            var uniqueCode = grid.GetEditor('AccountGroupCode').GetValue();

            if ((id != null) && (id != '')) {
                uniqueid = id;
                '<%=Session["id"]=null %>'
            }
            var CheckUniqueCodee = false;
            $.ajax({
                type: "POST",
                url: "AccountGroup.aspx/CheckUniqueCode",
                data: JSON.stringify({ uniqueCode: uniqueCode, uniqueid: uniqueid, LayoutId: LayoutIdPass }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    CheckUniqueCodee = msg.d;
                    if (CheckUniqueCodee == true) {
                        jAlert('Please enter unique short name');
                        grid.GetEditor('AccountGroupCode').SetValue('');
                        grid.GetEditor('AccountGroupCode').Focus();
                    }
                }
            });
        }
        function ReloadPage() {
            var url = 'AccountGroupPlList.aspx';
            window.location.href = url;
        }
        function ClearPopUp() {
            $("#cmbGroup_I").val('');
            ccmbGroupType.ClearItems();
            ctxtAccountName.SetValue('');
            ctxtAccountShortName.SetValue('');
            ctxtGroupSequence.SetValue('');
            ctxtGroupSchedule.SetValue('');
            //cListAccount.ClearItems();
            //cListFormula.ClearItems();
            // ccmbParentId.ClearItems();
        }
        function CloseWindow() {
            cAddEditPopUp.Hide();
        }


        //function AllControlInitilize() {
        //    if (canCallBack) {
        //        cAddEditPopUp.Hide();
        //    }
        //}

        //$(document).ready(function () {

        //   });



        function OnMapLedger(Id) {

            cLedgerPopUp.Show();
            $("#GroupId").val(Id);
            cLedgerGrid.PerformCallback('ShowDetails~' + Id);

        }
        function SaveMap() {
            var Id = $("#GroupId").val();
            cLedgerGrid.PerformCallback('SaveMap~' + Id);
        }

        function UniqueSequenceCheck() {
            var uniqueid = '0';
            var id = '<%= Convert.ToString(Session["id"]) %>';
            var LayoutIdPass = '0';
            var LayoutId = '<%= Convert.ToInt32(Session["LayoutId"]) %>';

            if (LayoutId != '' && LayoutId != null) {
                LayoutIdPass = LayoutId;
            }
            var uniqueCode = ctxtGroupSequence.GetValue();

            if ((id != null) && (id != '')) {
                uniqueid = id;
                '<%=Session["id"]=null %>'
            }
            var CheckUniqueCodee = false;
            $.ajax({
                type: "POST",
                url: "LayoutDetailsAdd.aspx/CheckUniqueSequence",
                data: JSON.stringify({ uniqueid: uniqueid, Sequence: uniqueCode, LayoutId: LayoutIdPass }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    CheckUniqueCodee = msg.d;
                    if (CheckUniqueCodee == true) {
                        jAlert('Please enter unique Sequence No');
                        ctxtGroupSequence.SetValue(0);
                    }
                }
            });
        }

        function UniqueCodeCheckPopUp() {

            var uniqueid = '0';
            var id = '<%= Convert.ToString(Session["id"]) %>';
            var LayoutIdPass = '0';
            var LayoutId = '<%= Convert.ToInt32(Session["LayoutId"]) %>';

            if (LayoutId != '' && LayoutId != null) {
                LayoutIdPass = LayoutId;
            }
            var uniqueCode = ctxtAccountShortName.GetValue();

            if ((id != null) && (id != '')) {
                uniqueid = id;
                '<%=Session["id"]=null %>'
             }
             var CheckUniqueCodee = false;
             $.ajax({
                 type: "POST",
                 url: "LayoutDetailsAdd.aspx/CheckUniqueCode",
                 data: JSON.stringify({ uniqueCode: uniqueCode, uniqueid: uniqueid, LayoutId: LayoutIdPass }),
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 success: function (msg) {
                     CheckUniqueCodee = msg.d;
                     if (CheckUniqueCodee == true) {
                         jAlert('Please enter unique short name');
                         ctxtAccountShortName.SetValue('');
                     }
                 }
             });
         }

         //function AddAccountType() {

         //    var uniqueid = '0';
         //    ccmbGroupType.ClearItems();

         //    var AccountGroupType = ccmbGroup.GetValue();
         //    //   ccmbGroupType.ClearItems();
         //    //var CheckUniqueCodee = false;
         //    $.ajax({
         //        type: "POST",
         //        url: "AccountGroup.aspx/GetAccountType",
         //        data: JSON.stringify({ AccountGroup: AccountGroupType }),
         //        contentType: "application/json; charset=utf-8",
         //        dataType: "json",
         //        async: false,
         //        success: function (msg) {
         //            var list = msg.d;
         //            var listItems = [];
         //            if (list.length > 0) {

         //                for (var i = 0; i < list.length; i++) {
         //                    var id = '';
         //                    var name = '';
         //                    id = list[i]['TypeId'];
         //                    name = list[i]['TypeName'];

         //                    var type = new Array(name, id);

         //                    ccmbGroupType.AddItem(type);

         //                    //  $('#lstTaxRates_MainAccount').append($('<option>').text(name).val(id));
         //                }
         //            }

         //        }
         //    });


         //}
         function AddAccountType() {
             if (ccmbGroup.GetText() == "Defined Total" || ccmbGroup.GetText() == "Net Total") {
                 cSelectPanel.PerformCallback('BindDefinedList');
             }

             else {
                 ccmbGroupType.PerformCallback();
             }
             //ccmbParentId.PerformCallback();
         }


         function UniqueNameCheck() {

             var uniqueid = '0';
             var id = '<%= Convert.ToString(Session["id"]) %>';

            var uniqueCode = ctxtAccountName.GetValue();

            if ((id != null) && (id != '')) {
                uniqueid = id;
                '<%=Session["id"]=null %>'
            }
            var CheckUniqueCodee = false;
            $.ajax({
                type: "POST",
                url: "LayoutDetailsAdd.aspx/CheckUniqueName",
                data: JSON.stringify({ uniqueName: uniqueCode, uniqueid: uniqueid }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    CheckUniqueCodee = msg.d;
                    if (CheckUniqueCodee == true) {
                        jAlert('Please enter unique name');
                        grid.GetEditor('AccountGroupName').SetValue('');
                        grid.GetEditor('AccountGroupName').Focus();
                    }
                }
            });
        }


        function UniqueNameCheckpopUp() {

            var uniqueid = '0';
            var id = '<%= Convert.ToString(Session["id"]) %>';
            var LayoutIdPass = '0';
            var LayoutId = '<%= Convert.ToInt32(Session["LayoutId"]) %>';

            if (LayoutId != '' && LayoutId != null) {
                LayoutIdPass = LayoutId;
            }
            var uniqueCode = ctxtAccountName.GetValue();

            if ((id != null) && (id != '')) {
                uniqueid = id;
                '<%=Session["id"]=null %>'
            }
            var CheckUniqueCodee = false;
            $.ajax({
                type: "POST",
                url: "LayoutDetailsAdd.aspx/CheckUniqueName",
                data: JSON.stringify({ uniqueName: uniqueCode, uniqueid: uniqueid, LayoutId: LayoutIdPass }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    CheckUniqueCodee = msg.d;
                    if (CheckUniqueCodee == true) {
                        jAlert('Please enter unique name');
                        ctxtAccountName.SetValue('');
                    }
                }
            });
        }




        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }
        function AddNewRow() {
            $("#CallStatus").val('Add');
            $('.HideDiv').show();
            $("#GroupId").val("");
            ClearPopUp();
            $("#ParentId").val("1");
            ctxtAccountShortName.Enabled = true;
            ccmbGroupType.SetEnabled(true);
            //$("#txtAccountShortName_I").prop("disabled", false);
            //$("#txtAccountName_I").prop("disabled", false);

            cAddEditPopUp.Show();
            cGroupLabel.SetValue('');
            $("ctxtAccountShortName").prop("Disabled", false);
        }

        function OnAddClick(ID) {
            $("#CallStatus").val('Add');
            $("#GroupId").val(ID);
            cGroupLabel.SetValue('');
            $("#ParentId").val("2");
            ClearPopUp();
            ctxtAccountShortName.Enabled = true;
            //$("#txtAccountShortName_I").prop("disabled", false);
            //$("#txtAccountName_I").prop("disabled", false);

            cAddEditPopUp.Show();
            ctxtAccountShortName.SetEnabled(true);
        }
        function EndCall(obj) {

            if (grid.cpDelmsg != null)
                jAlert(grid.cpDelmsg);
        }
        function OnCountryChanged(cmbParent) {
            grid.GetEditor("ParentIDWithName").PerformCallback(cmbParent.GetValue().toString());
        }
        function PerformCallToGridBind() {
            $("#ItemList").val("");
            var status = $("#CallStatus").val();
            var FormulaItems = cListFormula.itemsValue;
            $("#ItemList").val(FormulaItems);
            var id = $("#GroupId").val();
            var valid = true;

            if (ccmbGroup.GetText() == "") {
                valid = false;
                jAlert('Please select valid group type to proceed.', 'Error');
                return;
            }
            if (ctxtGroupSequence.GetText() == "" || ctxtGroupSequence.GetText() == "0") {
                valid = false;
                jAlert('Please select valid group sequence to proceed.', 'Error');
                return;
            }
            if (ctxtAccountShortName.GetText() == "") {
                valid = false;
                jAlert('Please select valid short name to proceed.', 'Error');
                return;
            }
            if (ctxtAccountName.GetText() == "") {
                valid = false;
                jAlert('Please select valid  name to proceed.', 'Error');
                return;
            }
            if (valid) {
                cSelectPanel.PerformCallback("Save~" + status + "~" + id);
            }
        }
        function OnEditClick(id, lavel) {
            var GroupId = id;
            $("#GroupId").val(id);
            $("#ParentId").val(lavel);
            ClearPopUp();



            $("#CallStatus").val("Edit");
            cAddEditPopUp.Show();

            cSelectPanel.PerformCallback('Edit~' + GroupId);
            if (lavel != 1) {
                cGroupLabel.SetValue(' ');
            }
            else {
                cGroupLabel.SetValue(' ');
            }
            //$("#txtAccountShortName").prop("disabled", true);
            //$("#txtAccountName").prop("disabled", true);
        }
        function LedgerPopUpClosing() {
            cLedgerGridLookUp.ClearFilter();
        }
        function LdgEnCall() {
            if (cLedgerGrid.cpAutoID != "") {
                if (cLedgerGrid.cpAutoID == "Sucsess") {
                    cLedgerPopUp.Hide();
                    jAlert("Ledger Mapped Successfully");

                }
                else {
                    cLedgerPopUp.Hide();
                    jAlert("Please Try Again");
                }
                cLedgerGridLookUp.ClearFilter();

            }
            var urlKeys = getUrlVars();
            if (urlKeys.Type == "View") {
                cbtnLedgerSave.SetVisible(false);
            }
            else {
                cbtnLedgerSave.SetVisible(true);

            }

        }
        function SelectPanel_EndCallBack() {

            if (ccmbGroup.GetText() != "Defined Total")
                $("#dvFormula").attr("disabled", "disabled").off('click');
            else
                $("#dvFormula").attr("disabled", "enabled").off('click');

            if (cSelectPanel.cpAutoID != "") {
                if (cSelectPanel.cpAutoID == "Edit") {
                    cAddEditPopUp.Hide();
                    ClearPopUp();
                    jAlert("Data Updated Successfully.");
                    // cAccountGroupTree.Refresh();
                    // location.reload();
                }
                else if (cSelectPanel.cpAutoID == "Delete") {
                    jAlert("Data Deleted Successfully.");
                    //cAccountGroupTree.Refresh();
                    //location.reload();
                }
                else if (cSelectPanel.cpAutoID == "Mapped") {
                    jAlert("Selected Group is mapped with ledger. Cannot proceed.");
                    //cAccountGroupTree.Refresh();
                    //location.reload();
                }
                else if (cSelectPanel.cpAutoID == "Add") {
                    jAlert("Data Saved Successfully.");
                    cAddEditPopUp.Hide();
                    ClearPopUp();
                    // cAccountGroupTree.Refresh();
                    //location.reload();
                }
                else if (cSelectPanel.cpAutoID == "ParentNull") {
                    $('.HideDiv').show();
                }
                else if (cSelectPanel.cpAutoID == "HasParent") {
                    $('.HideDiv').hide();
                }
                else {
                    jAlert("Try again Later");
                }
            }
            $("#ItemList").val("");
        }
        function TypeChange() {


        }
        function AddMainGroup() {
            ccmbGroup.SetText("Income");
            cAddEditPopUp.SetHeaderText("Add Group");
            $("#CallStatus").val('Add');
            var selectedNode = treeView.GetSelectedNode();
            $('.HideDiv').show();
            if (selectedNode != null) {
                var Id = selectedNode.name;
                $("#ParentId").val(Id);
            }
            else {
                $("#ParentId").val("0");
            }

            if (selectedNode != null) {
                if (selectedNode.parent != null) {
                    ccmbGroup.RemoveItem(3);
                    ccmbGroup.RemoveItem(2);

                }
                else {
                    if (!ccmbGroup.FindItemByText("Defined Total"))
                        ccmbGroup.AddItem("Defined Total");
                    if (!ccmbGroup.FindItemByText("Net Total"))
                        ccmbGroup.AddItem("Net Total");
                }
            }


            $("#GroupId").val("");
            ClearPopUp();

            ctxtAccountShortName.Enabled = true;
            ctxtAccountShortName.SetEnabled(true);
            ccmbGroupType.SetEnabled(true);
            //$("#txtAccountShortName_I").prop("disabled", false);
            //$("#txtAccountName_I").prop("disabled", false);
            //if (!ccmbGroup.FindItemByText("Defined Total"))
            //    ccmbGroup.AddItem("Defined Total");
            //if (!ccmbGroup.FindItemByText("Text"))
            //    ccmbGroup.AddItem("Text");
            cAddEditPopUp.Show();
            cGroupLabel.SetValue('Parent ');
            $("ctxtAccountShortName").prop("Disabled", false);
        }
        function OnClickDelete(id) {
            var GroupId = id;
            $("#GroupId").val(id);
            ClearPopUp();
            $("#CallStatus").val("Delete");
            jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    cSelectPanel.PerformCallback('Delete~' + GroupId);
                }
            });
        }
        function CloseLedgerWindow() {
            cLedgerPopUp.Hide();

        }
        function AddAccountGroup() {
            if (cListFormula.itemsValue.length == 0) {

                var obj = {};
                var SelectedItem = cListAccount.GetSelectedItem();
                var SelectedText = SelectedItem.text;
                var SelectedValue = SelectedItem.value;
                obj.ItemValue = SelectedValue;
                obj.ItemText = SelectedText;

                //if (ctxtSelectedAcoount.GetText() == "") {
                //    cListFormula.AddItem(SelectedText);
                //    //ctxtSelectedAcoount.SetText("[" + SelectedText + "]");
                //}
                //else {
                cListFormula.AddItem(SelectedText, obj.ItemValue);
                //cListAccount.RemoveItem(cListAccount.GetSelectedIndices(cListAccount.GetSelectedItems().length - 1));

            }
            else {
                if (cListFormula.GetItem(cListFormula.itemsValue.length - 1).value == "+" || cListFormula.GetItem(cListFormula.itemsValue.length - 1).value == "-") {

                    var obj = {};
                    var SelectedItem = cListAccount.GetSelectedItem();
                    var SelectedText = SelectedItem.text;
                    var SelectedValue = SelectedItem.value;
                    obj.ItemValue = SelectedValue;
                    obj.ItemText = SelectedText;
                    //if (ctxtSelectedAcoount.GetText() == "") {
                    //    cListFormula.AddItem(SelectedText);
                    //    //ctxtSelectedAcoount.SetText("[" + SelectedText + "]");
                    //}
                    //else {
                    cListFormula.AddItem(SelectedText, obj.ItemValue);
                    //cListAccount.RemoveItem(cListAccount.GetSelectedIndices(cListAccount.GetSelectedItems().length - 1));
                    //ctxtSelectedAcoount.SetText(ctxtSelectedAcoount.GetText() + "[" + SelectedText + "]");
                    //}
                }
                else {
                    jAlert("Please Select an Operator");
                }
            }
        }
        function PlusAccountGroup(s, e) {
            if (cListFormula.itemsValue.length != 0) {
                if (cListFormula.GetItem(cListFormula.itemsValue.length - 1).value != "+" && cListFormula.GetItem(cListFormula.itemsValue.length - 1).value != "-") {
                    var obj = {};
                    obj.ItemValue = "+";
                    obj.ItemText = "+";
                    cListFormula.AddItem("+", obj.ItemValue);

                    //ctxtSelectedAcoount.SetText(ctxtSelectedAcoount.GetText() + " + ");
                }
                else if (cListFormula.GetItem(cListFormula.itemsValue.length - 1).value.ItemValue == "-") {
                    cListFormula.RemoveItem(cListFormula.itemsValue.length - 1);
                    var obj = {};
                    obj.ItemValue = "+";
                    obj.ItemText = "+";
                    cListFormula.AddItem("+", obj.ItemValue);
                }
            }
            e.processOnServer = false;
        }
        function MinusAccountGroup(s, e) {
            if (cListFormula.itemsValue.length != 0) {
                if (cListFormula.GetItem(cListFormula.itemsValue.length - 1).value != "+" && cListFormula.GetItem(cListFormula.itemsValue.length - 1).value != "-") {
                    var obj = {};
                    obj.ItemValue = "-";
                    obj.ItemText = "-";
                    cListFormula.AddItem("-", obj.ItemValue);
                    //ctxtSelectedAcoount.SetText(ctxtSelectedAcoount.GetText() + " - ");
                }
                else if (cListFormula.GetItem(cListFormula.itemsValue.length - 1).value.ItemValue == "+") {
                    cListFormula.RemoveItem(cListFormula.itemsValue.length - 1);
                    var obj = {};
                    obj.ItemValue = "-";
                    obj.ItemText = "-";
                    cListFormula.AddItem("-", obj.ItemValue);
                }
            }
            e.processOnServer = false;
        }
        function RemoveAccountGroup(s, e) {
            if (cListFormula.GetItem(cListFormula.GetSelectedIndex()).value != "+" && cListFormula.GetItem(cListFormula.GetSelectedIndex()).value != "-") {
                var len = cListFormula.GetSelectedIndex();
                if (len > 0) {
                    cListFormula.RemoveItem(cListFormula.GetSelectedIndex() - 1);
                    cListFormula.RemoveItem(cListFormula.GetSelectedIndex());
                }
                else if (len == 0) {
                    cListFormula.RemoveItem(cListFormula.GetSelectedIndex() + 1);
                    cListFormula.RemoveItem(cListFormula.GetSelectedIndex());
                }

            }
            e.processOnServer = false;

        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="panel-heading">
        <div class="panel-title clearfix">
            <h3 class="clearfix"><span class="pull-left">
                <asp:Label ID="lblHeading" runat="server" Text="Edit Profit and Loss Layout"></asp:Label></span>
            </h3>
            <div id="btncross" class="crossBtn" runat="server" style="display: none; margin-left: 50px;"><a href="javascript:ReloadPage()"></a><i class="fa fa-times"></i></a></div>
            <%-- <div id="btncross" style="display: none; margin-left: 50px;" class="crossBtn CloseBtn">
                <asp:ImageButton ID="imgClose" runat="server" ImageUrl="/assests/images/CrIcon.png" OnClick="imgClose_Click" />
            </div>--%>
        </div>
        <div id="Div1" runat="server" class="crossBtn" style="margin-left: 50px;"><a href="javascript:void(0);" onclick="ReloadPage()"><i class="fa fa-times"></i></a></div>
    </div>

    <div style="background: #f5f4f3; padding: 8px 0; margin-bottom: 0px; border-radius: 4px; border: 1px solid #ccc;" class="clearfix">
        <div class="col-md-3">
            <label>Layout Name</label>
            <div>
                <asp:TextBox ID="txtLayoutName" runat="server" Width="95%" meta:resourcekey="txtLayoutNo" MaxLength="30"></asp:TextBox>

            </div>
        </div>

        <div class="col-md-9 lblmTop8">
            <label>Layout Description</label>
            <div>
                <asp:TextBox ID="txtLayoutDescription" runat="server" Width="95%" meta:resourcekey="txtBillNoResource1" MaxLength="300"></asp:TextBox>

            </div>
        </div>
    </div>



    <div style="background: #f5f4f3; padding: 8px 0; margin-bottom: 0px; border-radius: 4px; border: 1px solid #ccc;" class="clearfix">
        <div class="col-md-3">

            <div>

                <a href="javascript:void(0);" onclick="AddMainGroup()" class="btn btn-primary" style="display:none"><span>Add Group</span> </a>


            </div>
        </div>
    </div>

    <dxe:ASPxCallbackPanel runat="server" ID="SelectPanel" ClientInstanceName="cSelectPanel" OnCallback="SelectPanel_Callback" CssClass="">
        <ClientSideEvents EndCallback="function(s, e) {SelectPanel_EndCallBack();}" />
        <PanelCollection>
            <dxe:PanelContent runat="server">
                <dxe:ASPxTreeView ID="TvLayout" ClientInstanceName="treeView" runat="server" EnableCallBacks="true" OnVirtualModeCreateChildren="TvLayout_VirtualModeCreateChildren" Font-Size="Medium"
                    EnableHotTrack="false" AllowSelectNode="true" EnableAnimation="true" ShowTreeLines="true" EnableClientSideAPI="true" SyncSelectionMode="CurrentPathAndQuery">
                    <Images NodeImage-Width="13px">
                    </Images>
                    <Styles NodeImage-Paddings-PaddingTop="2px">
                    </Styles>
                    <ClientSideEvents Init="treeView_OnInit" />
                </dxe:ASPxTreeView>
                <dxe:ASPxPopupMenu ID="popupMenu" ClientInstanceName="popupMenu" runat="server">
                    <Items>
                        <dxe:MenuItem Name="itmExpandCollapse" Text="Expand">
                        </dxe:MenuItem>
                        <dxe:MenuItem Name="itmEnableDisable" Text="Enable">
                        </dxe:MenuItem>
                        <%--<dxe:MenuItem Name="itmAddNew" Text="Create Subgroup">
                        </dxe:MenuItem>--%>
                        <dxe:MenuItem Name="itmEdit" Text="Edit">
                        </dxe:MenuItem>
                        <dxe:MenuItem Name="itmDelete" Text="Delete">
                        </dxe:MenuItem>
                        <dxe:MenuItem Name="itmCut" Text="Cut">
                        </dxe:MenuItem>
                        <dxe:MenuItem Name="itmPaste" Text="Paste">
                        </dxe:MenuItem>
                        <dxe:MenuItem Name="itmLedgerMap" Text="Ledger Map">
                        </dxe:MenuItem>
                    </Items>
                    <ClientSideEvents PopUp="popupMenu_OnPopUp" ItemClick="popupMenu_OnItemClick" />
                </dxe:ASPxPopupMenu>

                <div class="PopUpArea">
                    <dxe:ASPxPopupControl ID="AddEditPopUp" runat="server" ClientInstanceName="cAddEditPopUp"
                        Width="600px" HeaderText="Add/Edit Group" PopupHorizontalAlign="WindowCenter"
                        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
                        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">

                        <ContentCollection>
                            <dxe:PopupControlContentControl runat="server">


                                <div class="cnt" id="dvAll">
                                    <table class="padding">
                                        <tr Class="HideDiv">
                                            <td>
                                                <div>Group Type</div>
                                            </td>
                                            <td>
                                                <div>
                                                    <dxe:ASPxComboBox ID="cmbGroup" ClientInstanceName="ccmbGroup" runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True">
                                                        <ClientSideEvents SelectedIndexChanged="function(s,e){AddAccountType()}" />
                                                        <Items>
                                                            <%-- <dxe:ListEditItem Value="Income" Text="Income" Selected></dxe:ListEditItem>
                                                            <dxe:ListEditItem Value="Expenses" Text="Expenses"></dxe:ListEditItem>
                                                            <%-- <dxe:ListEditItem Value="PageTitle" Text="Page Title"></dxe:ListEditItem>--%>
                                                            <dxe:ListEditItem Value="DefinedTotal" Text="Defined Total"></dxe:ListEditItem>
                                                            <dxe:ListEditItem Value="Net Total" Text="Net Total"></dxe:ListEditItem>
                                                            <%--<dxe:ListEditItem Value="Income" Text="Income"></dxe:ListEditItem>
                                                        <dxe:ListEditItem Value="Expenses" Text="Expenses"></dxe:ListEditItem>--%>
                                                            <%--#ag18102016 - 0011279--%>
                                                        </Items>

                                                    </dxe:ASPxComboBox>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr style="display:none;">
                                            <td>
                                                <div>Group Type</div>
                                            </td>
                                            <td>
                                                <div>
                                                    <dxe:ASPxComboBox ID="cmbGroupType" ClientInstanceName="ccmbGroupType" runat="server" Width="100%" OnCallback="cmbGroupType_Callback">
                                                        <ClientSideEvents SelectedIndexChanged="TypeChange" />

                                                        <ValidationSettings RequiredField-IsRequired="true" Display="None"></ValidationSettings>
                                                    </dxe:ASPxComboBox>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div>
                                                    <dxe:ASPxLabel runat="server" ID="GroupLabel" ClientInstanceName="cGroupLabel"></dxe:ASPxLabel>
                                                    Group Sequence
                                                </div>
                                            </td>
                                            <td>
                                                <div>
                                                    <dxe:ASPxTextBox ID="txtGroupSequence" ClientInstanceName="ctxtGroupSequence" runat="server" ValueType="System.Int32" Width="100%">
                                                        <ClientSideEvents TextChanged="function(s,e){UniqueSequenceCheck()}" />
                                                        <MaskSettings Mask="&lt;0..999999999&gt;" AllowMouseWheel="false" />
                                                        <ValidationSettings RequiredField-IsRequired="true" Display="None"></ValidationSettings>
                                                    </dxe:ASPxTextBox>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr Class="HideDiv">
                                            <td>
                                                <div>Group Schedule</div>
                                            </td>
                                            <td>
                                                <div>
                                                    <dxe:ASPxTextBox ID="txtGroupSchedule" ClientInstanceName="ctxtGroupSchedule" runat="server" ValueType="System.String" Width="100%">
                                                        
                                                        <ValidationSettings RequiredField-IsRequired="true" Display="None"></ValidationSettings>
                                                    </dxe:ASPxTextBox>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div>Short Name</div>
                                            </td>
                                            <td>
                                                <div>
                                                    <dxe:ASPxTextBox ID="txtAccountShortName" ClientInstanceName="ctxtAccountShortName" runat="server" ValueType="System.String" Width="100%">
                                                        <ClientSideEvents TextChanged="function(s,e){UniqueCodeCheckPopUp()}" />
                                                        <ValidationSettings RequiredField-IsRequired="true" Display="None"></ValidationSettings>
                                                    </dxe:ASPxTextBox>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div>Name</div>
                                            </td>
                                            <td>
                                                <div>
                                                    <dxe:ASPxTextBox ID="txtAccountName" ClientInstanceName="ctxtAccountName" runat="server" ValueType="System.String" Width="100%">
                                                        <ClientSideEvents TextChanged="function(s,e){UniqueNameCheckpopUp()}" />
                                                        <ValidationSettings RequiredField-IsRequired="true" Display="None"></ValidationSettings>
                                                    </dxe:ASPxTextBox>
                                                </div>
                                            </td>
                                        </tr>
                                        <%--                                    <tr>
                                        <td><div>Parent Id</div></td>
                                        <td>
                                            <div>
                                                <dxe:ASPxComboBox ID="cmbParentId" ClientInstanceName="ccmbParentId" runat="server" Width="100%"
                                                    OnCallback="cmbParentId_Callback">
                                                    <ValidationSettings RequiredField-IsRequired="true" Display="None"></ValidationSettings>
                                                </dxe:ASPxComboBox>
                                            </div>
                                        </td>
                                    </tr>--%>
                                        <tr style="display: none;">
                                            <td></td>
                                            <td>
                                                <div>
                                                    <dxe:ASPxButton ID="btnSave" ClientInstanceName="cbtnSave" runat="server" AutoPostBack="False" Text="Save" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                        <ClientSideEvents Click="function(s, e) {return PerformCallToGridBind(); }" />

                                                    </dxe:ASPxButton>
                                                    <dxe:ASPxButton ID="btnCancel" ClientInstanceName="cbtnCancel" runat="server" AutoPostBack="False" Text="Cancel" CssClass="btn btn-danger" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                        <ClientSideEvents Click="function(s, e) {return CloseWindow(); }" />

                                                    </dxe:ASPxButton>
                                                </div>
                                            </td>
                                        </tr>

                                    </table>
                                </div>
                                <div id="dvFormula" >
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="width: 40%">Available Group</td>
                                            <td style="width: 20%"></td>
                                            <td style="width: 40%">Selected Group</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <dxe:ASPxListBox ID="ListAccount" ClientInstanceName="cListAccount" runat="server" ValueType="System.String" Width="100%" Height="200px">
                                                    <ClientSideEvents ItemDoubleClick="AddAccountGroup"  />
                                                </dxe:ASPxListBox>
                                            </td>
                                            <td style="padding: 0 15px; text-align: center">
                                                <dxe:ASPxButton ID="btnAdd" runat="server"  Width="100px" Height="20px" Text="+" UseSubmitBehavior="false">
                                                    <ClientSideEvents Click="function(s, e) {return PlusAccountGroup(s,e);  }" />
                                                </dxe:ASPxButton>
                                                <br />
                                                <br />
                                                <dxe:ASPxButton ID="btnMinus" runat="server" Width="100px" Height="20px" Text="-" UseSubmitBehavior="false">
                                                    <ClientSideEvents Click="function(s, e) {return MinusAccountGroup(s,e); }" />
                                                </dxe:ASPxButton>
                                                <br />
                                                <br />
                                                <dxe:ASPxButton ID="btnRemove" runat="server" Width="100px" Height="20px" Text="Remove" UseSubmitBehavior="false">
                                                    <ClientSideEvents Click="function(s, e) {return RemoveAccountGroup(s,e); }" />
                                                </dxe:ASPxButton>
                                            </td>
                                            <td>
                                                <dxe:ASPxListBox ID="ListFormula" ClientInstanceName="cListFormula" runat="server" ValueType="System.String" Width="100%" Height="200px">
                                                    <ClientSideEvents ItemDoubleClick="AddAccountGroup" />
                                                </dxe:ASPxListBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" style="padding: 0 15px; text-align: center">
                                                <div>
                                                    <dxe:ASPxButton ID="btnAddNew" ClientInstanceName="cbtnSave" runat="server" AutoPostBack="False" Text="Save" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                        <ClientSideEvents Click="function(s, e) {return PerformCallToGridBind(); }" />

                                                    </dxe:ASPxButton>
                                                    <dxe:ASPxButton ID="btnRemoveNew" ClientInstanceName="cbtnCancel" runat="server" AutoPostBack="False" Text="Cancel" CssClass="btn btn-danger" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                        <ClientSideEvents Click="function(s, e) {return CloseWindow(); }" />

                                                    </dxe:ASPxButton>
                                                </div>
                                            </td>

                                        </tr>
                                    </table>
                                </div>

                                <div style="padding-top: 15px;">
                                </div>



                            </dxe:PopupControlContentControl>
                        </ContentCollection>

                    </dxe:ASPxPopupControl>

                </div>
            </dxe:PanelContent>
        </PanelCollection>
    </dxe:ASPxCallbackPanel>
    <div>
        <dxe:ASPxPopupControl ID="LedgerPopUp" runat="server" ClientInstanceName="cLedgerPopUp"
            Width="600px" HeaderText="Ledger Map" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True" Opacity="100">
            <ClientSideEvents Closing="function(s, e) {LedgerPopUpClosing();}" />
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <dxe:ASPxCallbackPanel runat="server" ID="LedgerPanel" ClientInstanceName="cLedgerGrid" OnCallback="LedgerPanel_Callback">
                        <ClientSideEvents EndCallback="function(s, e) {LdgEnCall();}"  />
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <dxe:ASPxGridView ID="LedgerGrid" runat="server" ClientInstanceName="cLedgerGridLookUp"
                                    KeyFieldName="LedgerCode" Width="100%" SelectionMode="Multiple" AutoGenerateColumns="False"
                                    OnDataBinding="LedgerGrid_DataBinding" TextFormatString="{0}" MultiTextSeparator=", ">
                                    <SettingsPager Mode="ShowPager">
                                    </SettingsPager>

                                    <SettingsPager PageSize="10">
                                        <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,20,50,100,150,200" />
                                    </SettingsPager>

                                    <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                    <Columns>
                                        <dxe:GridViewCommandColumn ShowSelectCheckbox="true" VisibleIndex="0" Width="100" Caption="Select">
                                        </dxe:GridViewCommandColumn>
                                        <dxe:GridViewDataColumn FieldName="LedgerCode" Caption="Code" Width="100" Visible="false">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataColumn>
                                        <dxe:GridViewDataColumn FieldName="LedgerCodeName" Caption="Ledger Code" Width="100">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataColumn>
                                        <dxe:GridViewDataColumn FieldName="LedgerName" Caption="Ledger Name" Width="180">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataColumn>

                                        <dxe:GridViewDataColumn FieldName="GroupName" Caption="Group Name" Width="50" Visible="false">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataColumn>
                                        <dxe:GridViewDataColumn FieldName="GroupCode" Visible="false" Caption="GroupCode" Width="80">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataColumn>
                                    </Columns>




                                </dxe:ASPxGridView>

                                <div>
                                    <dxe:ASPxButton ID="LedgerSave" ClientInstanceName="cbtnLedgerSave" runat="server" AutoPostBack="False" Text="Save" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                        <ClientSideEvents Click="function(s, e) {return SaveMap(); }" />

                                    </dxe:ASPxButton>
                                    <dxe:ASPxButton ID="LedgerCancel" ClientInstanceName="cbtnLedgerCancel" runat="server" AutoPostBack="False" Text="Cancel" CssClass="btn btn-danger" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                        <ClientSideEvents Click="function(s, e) {return CloseLedgerWindow(); }" />

                                    </dxe:ASPxButton>
                                </div>
                            </dxe:PanelContent>
                        </PanelCollection>
                    </dxe:ASPxCallbackPanel>
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>
        <asp:HiddenField ID="CallStatus" runat="server" EnableViewState="true" />
        <asp:HiddenField ID="GroupId" runat="server" EnableViewState="true" />
        <asp:HiddenField ID="ParentId" runat="server" EnableViewState="true" />
        <asp:HiddenField ID="ItemList" runat="server" EnableViewState="true" />

    </div>
</asp:Content>
