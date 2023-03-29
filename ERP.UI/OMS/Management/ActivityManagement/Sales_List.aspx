<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                29-03-2023        2.0.36           Pallab              25733 : Master pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Title="Sales Activity" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" EnableEventValidation="false"  AutoEventWireup="true" CodeBehind="Sales_List.aspx.cs" Inherits="ERP.OMS.Management.Activities.Sales_List" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
         <script src="/assests/pluggins/choosen/choosen.min.js"></script>
    <script type="text/javascript" language="Javascript">
        //function ShowDetail(ProductTypePath) {
        //    document.getElementById('GridDiv').style.display = 'none';
        //    document.getElementById('FrameDiv').style.display = 'inline';
        //    //alert(ShowDetails);
        //    ShowDetails.location = ProductTypePath;
        //}

        $(document).ready(function () {
           BindAssignSupervisor();
           ListAssignTo();

            $("#lstAssignTo").chosen().change(function () {
                var assignId = $(this).val();
                $('#<%=hdnAssign.ClientID %>').val(assignId);
            })


            $("#lstSupervisorAssignTo").chosen().change(function () {
                debugger;
                var assignId = $(this).val();
                $('#<%=hdnSupervisorAssign.ClientID %>').val(assignId);
            })

            $('#lblSalesActivity').text('Assign Sales Activity');
        })

     


        function OnViewClick(keyValue) {

            cproductpopup.Show();
            popproductPanel.PerformCallback('ShowSupervisorHistory~' + keyValue);
        }
        function CancelReassignSupervisor_save() {

            $('#<%=hdnSupervisorAssign.ClientID %>').val('');
            $('#MandatorySupervisorAssign').attr('style', 'display:none;position: absolute; right: -20px; top: 8px;');
            cPopup_ReassignSupervisor.Hide();
            $("#lstSupervisorAssignTo").find("option").attr("selected", false);
        }
        function CallSupervisor_save() {
            var suid = $('#<%=hdnSupervisorAssign.ClientID %>').val();
            var Actid = $('#<%=hdnAct.ClientID %>').val();

            var Osuid = $('#<%=hdnOldSupervisorAssign.ClientID %>').val();
            if (suid == "" || suid == null) {
                $('#MandatorySupervisorAssign').attr('style', 'display:block;position: absolute; right: -20px; top: 8px;');
                return false;
            }
            else {
                $('#MandatorySupervisorAssign').attr('style', 'display:none;position: absolute; right: -20px; top: 8px;');
                Sgrid.PerformCallback('Reassign~' + suid + '~' + Actid + '~' + Osuid);
            }
            $('#<%=hdnSupervisorAssign.ClientID %>').val('');
            $("#lstSupervisorAssignTo").find("option").attr("selected", false);
            // $('#lstSupervisorAssignTo').
        }

        function BindNewSupervisor(activityid, supervisorId, assignedToId) {

       
          //  BindSupervisorList(activityid, supervisorId, assignedToId);

            $('#<%=hdnAct.ClientID %>').val(activityid);

            $('#<%=hdnOldSupervisorAssign.ClientID %>').val(supervisorId);
            // ListAssignTo();
            // reassignvar = slsid;
            var lstSupervisorAssignToTemp = $('select[id$=lstSupervisorAssignTo]');
            lstSupervisorAssignToTemp.empty();
            $('#lstSupervisorAssignTo').trigger("chosen:updated");


            BindAssignSupervisor();
            cPopup_ReassignSupervisor.Show();

            return false;
        }
        function AfterSave() {
            document.getElementById('GridDiv').style.display = 'inline';
            document.getElementById('FrameDiv').style.display = 'none';
            //height();
        }

        function disp_prompt(name) {
            if (name == "tab0") {
                $('#divDocumentdetails').attr('style', 'display:none');
                $('#divFuturedetails').attr('style', 'display:none');
                $('#divCloseddetails').attr('style', 'display:none');
                $('#divdetails').attr('style', 'display:block');
                //divdetails
                //divDocumentdetails
                //divFuturedetails
                //divCloseddetails
                //document.location.href="crm_sales.aspx"; 
            }
            if (name == "tab1") {
                //   alert('2');
                $('#divDocumentdetails').attr('style', 'display:block');
                $('#divFuturedetails').attr('style', 'display:none');
                $('#divCloseddetails').attr('style', 'display:none');
                $('#divdetails').attr('style', 'display:none');
                gridDocument.PerformCallback();
                // document.location.href = "../Activities/frmDocument.aspx";
            }
            else if (name == "tab2") {
                $('#divDocumentdetails').attr('style', 'display:none');
                $('#divFuturedetails').attr('style', 'display:block');
                $('#divCloseddetails').attr('style', 'display:none');
                $('#divdetails').attr('style', 'display:none');
                gridFuture.PerformCallback();
            }
            else if (name == "tab3") {
                $('#divDocumentdetails').attr('style', 'display:none');
                $('#divFuturedetails').attr('style', 'display:none');
                $('#divCloseddetails').attr('style', 'display:block');
                $('#divdetails').attr('style', 'display:none');
                gridClosed.PerformCallback();
            }

        }


        function CallForms(val) {

            if (val == "Expences") {
                var frm = "sales_Sconveyence.aspx";
                var Title = "Expences";
            }
            else if (val == "Address") {
                var frm = "Contact_Correspondence.aspx?type=modify&requesttype=lead&formtype=lead";
                var Title = "Address/Phone";

            }
            else if (val == "Contact") {
                var frm = "Lead_general.aspx?type=modify&requesttype=lead&formtype=lead123";
                var Title = "Contact";
            }
            else if (val == "Bank") {
                var frm = "Contact_BankDetails.aspx?type=modify&requesttype=lead&formtype=lead123";
                Title = "Bank Details";
            }
            else if (val == "Registration") {
                var frm = "Contact_Registration.aspx?type=modify&requesttype=lead&formtype=lead123";
                var Title = "Registration";
            }
            else if (val == "Document") {
                var frm = "SalesDocument.aspx?type=modify&requesttype=lead&formtype=lead";
                var Title = "Document Details";
            }
            else if (val == "History") {
                var frm = "../ShowHistory_Phonecall.aspx";
                var Title = "History";
            }
            OnMoreInfoClick(frm, Title, '950px', '450px', 'Y');
        }
        function callback() {
            document.getElementById("ShowDetails").contentWindow.ClosingDHTML();
        }
        function ShowDetail(keyValue, AssignedID) {
            //$('#SaleListID').attr('style', 'display:none');
            //$('#SaleDetailsListID').attr('style', 'display:none');



            //$('#divadd').attr('style', 'display:none');
            //$('#btncross').attr('style', 'display:block');
            //$('#divdetails').attr('style', 'display:block');

            //grid.PerformCallback('Details~' + keyValue + '~' + AssignedID);

            var url = "ActivityNewSales.aspx?id1=" + keyValue + "&Aid=" + AssignedID;
            document.location.href = url;

            //  $('#lblSalesActivity').text('Sales Activity Assign');
        }

        function ShowCreateActivity(LeadId, sls_id, sls_activity_id, act_assignedTo, act_activityNo, act_assign_task) {
            grid.PerformCallback('CreateActivity~' + LeadId + '~' + sls_id + '~' + sls_activity_id + '~' + act_assignedTo + '~' + act_activityNo + '~' + act_assign_task);
        }

        function ShowHistory(sls_id) {
            var url = "../Master/ShowHistory_Phonecall.aspx?id1=" + sls_id;
            document.location.href = url;
        }
        function DeleteRow(keyValue) {

            jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    Sgrid.PerformCallback('Delete~' + keyValue);
                }
            });
        }

        function LastCall(obj) {
            if (Sgrid.cpDelmsg != null) {

                if (Sgrid.cpDelmsg.trim() != '') {
                    jAlert(Sgrid.cpDelmsg);
                    Sgrid.cpDelmsg = null;
                    Sgrid.cpSupmsg = null;
                    Sgrid.cpSManmsg = null;

                }
            }

            if (Sgrid.cpSupmsg != null) {
                cPopup_ReassignSupervisor.Hide();
                if (Sgrid.cpSupmsg.trim() != '') {

                    jAlert(Sgrid.cpSupmsg);
                    Sgrid.cpDelmsg = null;
                    Sgrid.cpSupmsg = null;
                    Sgrid.cpSManmsg = null;

                }
            }

            if (Sgrid.cpSManmsg != null) {
                cPopup_Reassign.Hide();
                if (Sgrid.cpSManmsg.trim() != '') {

                    jAlert(Sgrid.cpSManmsg);
                    Sgrid.cpDelmsg = null;
                    Sgrid.cpSupmsg = null;
                    Sgrid.cpSManmsg = null;

                }
            }
        }
        function AddButtonClick() {

            document.location.href = "Sales_Activity.aspx";
        }


        function BindAssignSupervisor() {

            debugger;
            var lstSupervisorAssignTo = $('select[id$=lstSupervisorAssignTo]');
            var lstAssignTo = $('select[id$=lstAssignTo]');

            $.ajax({
                type: "POST",
                url: 'Sales_List.aspx/GetAllUserListBeforeSelect',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var list = msg.d;
                    var listItems = [];
                    if (list.length > 0) {

                        for (var i = 0; i < list.length; i++) {

                            var id = '';
                            var name = '';
                            id = list[i].split('|')[1];
                            name = list[i].split('|')[0];

                            listItems.push('<option value="' +
                            id + '">' + name
                            + '</option>');
                        }

                        $(lstSupervisorAssignTo).append(listItems.join(''));


                        ListSupervisorAssignTo();
                        $('#lstSupervisorAssignTo').trigger("chosen:updated");
                        
                        
                        
                        $('#lstAssignTo').append(listItems.join(''));


                        ListAssignTo();
                        $('#lstAssignTo').trigger("chosen:updated");


                    }
                    else {
                        $('#lstSupervisorAssignTo').trigger("chosen:updated");
                        $('#lstSupervisorAssignTo').prop('disabled', true).trigger("chosen:updated");


                        $('#lstAssignTo').trigger("chosen:updated");
                        $('#lstAssignTo').prop('disabled', true).trigger("chosen:updated");
                        // alert("No records found");
                    }
                    //else {
                    //    alert("No records found");
                    //}
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });


        }



        function BindSupervisorList(activityid, supervisorId, assignedToId) {

            var lAssignTo = $('select[id$=lstSupervisorAssignTo]');


            $.ajax({
                type: "POST",
                url: 'Sales_List.aspx/GetAllNewSupervisorList',
                data: JSON.stringify({ activityid: activityid, supervisorId: supervisorId, assignedToId: assignedToId }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var list = msg.d;
                    var listItems = [];
                    if (list.length > 0) {
                       // alert(list.length)
                        for (var i = 0; i < list.length; i++) {

                            var id = '';
                            var name = '';
                            id = list[i].split('|')[1];
                            name = list[i].split('|')[0];

                            listItems.push('<option value="' +
                            id + '">' + name
                            + '</option>');
                        }
                       // alert(listItems);
                        $(lAssignTo).append(listItems.join(''));


                        ListSupervisorAssignTo();
                        $('#lstSupervisorAssignTo').trigger("chosen:updated");


                    }
                    else {
                        $('#lstSupervisorAssignTo').trigger("chosen:updated");
                        $('#lstSupervisorAssignTo').prop('disabled', true).trigger("chosen:updated");
                        // alert("No records found");
                    }
                    //else {
                    //    alert("No records found");
                    //}
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });


        }
        function LastActivityDetailsCall(obj) {
            if (grid.cpSave != null) {
                if (grid.cpSave == 'Y') {
                    grid.cpSave = '';
                    jAlert("Reassign Successfully");
                    cPopup_Reassign.Hide();

                    $("#lstAssignTo").empty();
                    $('#lstAssignTo').trigger("chosen:updated");
                }
                else {
                    grid.cpSave = '';
                    jAlert("Activity already done ,cannot reassign.");
                    cPopup_Reassign.Hide();
                }
            }

            if (grid.cpredirect != null) {
                var rurl = grid.cpredirect;
                grid.cpredirect = null;

                document.location.href = rurl;

            }
        }
        function ListAssignTo() {

            $('#lstAssignTo').chosen();
            $('#lstAssignTo').fadeIn();

            var config = {
                '.chsnProduct': {},
                '.chsnProduct-deselect': { allow_single_deselect: true },
                '.chsnProduct-no-single': { disable_search_threshold: 10 },
                '.chsnProduct-no-results': { no_results_text: 'Oops, nothing found!' },
                '.chsnProduct-width': { width: "100%" }
            }
            for (var selector in config) {
                $(selector).chosen(config[selector]);
            }
        }

        function ListSupervisorAssignTo() {

            $('#lstSupervisorAssignTo').chosen();
            $('#lstSupervisorAssignTo').fadeIn();

            var config = {
                '.chsnProduct': {},
                '.chsnProduct-deselect': { allow_single_deselect: true },
                '.chsnProduct-no-single': { disable_search_threshold: 10 },
                '.chsnProduct-no-results': { no_results_text: 'Oops, nothing found!' },
                '.chsnProduct-width': { width: "100%" }
            }
            for (var selector in config) {
                $(selector).chosen(config[selector]);
            }
        }
        //sales activity details
        function salesView(obj) {

            //var URL = 'Contact_Document.aspx?idbldng=' + obj;
            var URL = 'Sales_Activity.aspx?idslsact=' + obj;
            //editwin = dhtmlmodal.open("Editbox", "iframe", URL, "Document", "width=1000px,height=400px,center=0,resize=1,top=-1", "recal");
            window.location.href = URL;



        }

        function Budget_open() {
            var SMid = '';
            var url = '/OMS/Management/Activities/SalesmanBudget.aspx?tid=2';
            popupbudget.SetContentUrl(url);
            popupbudget.Show();

            return false;
            //return true;
        }
        function BudgetAfterHide(s, e) {
            popupbudget.Hide();
        }

        function CancelReassign_save() {
            txtRemarks.SetValue();
            $('#chkMail').prop('checked', false);
            $('#<%=hdnAssign.ClientID %>').val('');
            cPopup_Reassign.Hide();
        }
   


        function Call_save() {
            //    debugger;
            var flag = true;
            var uid = $('#<%=hdnAssign.ClientID %>').val();



               //  debugger;
            var Remarks = txtRemarks.GetValue();

            if (uid == "" || uid == null) {
                $('#MandatoryAssign').attr('style', 'display:block;position: absolute; right: -20px; top: 8px;');
                flag = false;
            }
            else {

                $('#MandatoryAssign').attr('style', 'display:none;position: absolute; right: -20px; top: 8px;');
                flag = true;
            }

            if (Remarks == "" || Remarks == null) {
                $('#MandatoryRemarks').attr('style', 'display:block;position: absolute; right: -20px; top: 8px;');
                flag = false;
            }
            else {
                $('#MandatoryRemarks').attr('style', 'display:none;position: absolute; right: -20px; top: 8px;');

                Sgrid.PerformCallback('SalesmanReassign~' + reassignsalesmanvar + '~' + uid + '~' + Remarks);
            }
            return flag;

        }
        function btn_ShowRecordsClick() {
            //CallServer(data, "");
            Sgrid.PerformCallback('');
        }
        // Mantis Issue 24211
        function btn_CreateOpportunities()
        {
            Sgrid.PerformCallback('CreateOpportunities');
        }
        // End of Mantis Issue 24211


        function ShowDetailReassign(act_id, assignedToId) {
            //debugger;
            assignto = assignedToId;
          //  BindAssignSalesman(assignto);
            $('#MandatoryRemarks').attr('style', 'display:none;position: absolute; right: -20px; top: 8px;');
            $('#MandatoryAssign').attr('style', 'display:none;position: absolute; right: -20px; top: 8px;');
            reassignsalesmanvar = act_id;
            var lstAssignToTemp = $('select[id$=lstAssignTo]');
             lstAssignToTemp.empty();
             $('#lstAssignTo').trigger("chosen:updated");
            BindAssignSupervisor();
            ListAssignTo();
            cPopup_Reassign.Show();
            txtRemarks.SetValue();
            $('#chkMail').prop('checked', false);

            return false;
        }



        function BindAssignSalesman(UserId) {

            var lAssignTo = $('select[id$=lstAssignTo]');
            lAssignTo.empty();

            $.ajax({
                type: "POST",
                url: 'Sales_List.aspx/GetAllSalesmanListBeforeSelect',
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ UserId: UserId }),
                dataType: "json",

                success: function (msg) {
                    var list = msg.d;
                    var listItems = [];
                    if (list.length > 0) {

                        for (var i = 0; i < list.length; i++) {

                            var id = '';
                            var name = '';
                            id = list[i].split('|')[1];
                            name = list[i].split('|')[0];

                            listItems.push('<option value="' +
                            id + '">' + name
                            + '</option>');
                        }

                        $(lAssignTo).append(listItems.join(''));


                        ListAssignTo();
                        $('#lstAssignTo').trigger("chosen:updated");


                    }
                    else {
                        $('#lstAssignTo').trigger("chosen:updated");
                        $('#lstAssignTo').prop('disabled', true).trigger("chosen:updated");
                        // alert("No records found");
                    }
                    //else {
                    //    alert("No records found");
                    //}
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });


        }

        function gridRowclick(s, e) {
            $('#gridAdvanceAdj').find('tr').removeClass('rowActive');
            $('.floatedBtnArea').removeClass('insideGrid');
            //$('.floatedBtnArea a .ico').css({ 'opacity': '0' });
            $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea').addClass('insideGrid');
            $(s.GetRow(e.visibleIndex)).addClass('rowActive');
            setTimeout(function () {
                //alert('delay');
                var lists = $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a');
                //$(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a .ico').css({'opacity': '1'});
                //$(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a').each(function (e) {
                //    setTimeout(function () {
                //        $(this).fadeIn();
                //    }, 100);
                //});    
                $.each(lists, function (index, value) {
                    //console.log(index);
                    //console.log(value);
                    setTimeout(function () {
                        $(value).css({ 'opacity': '1' });
                    }, 100);
                });
            }, 200);
        }

    </script>

      <style>
       .dxtcSys.dxtc-init > .dxtc-stripContainer {
     visibility: visible !important; 
}
        .chosen-container.chosen-container-multi,
        .chosen-container.chosen-container-single {
            width:100% !important;
        }
        .chosen-choices {
            width:100% !important;
        }
        #lstAssignTo {
            width:200px;
        }
        .hide {
            display:none;
        }
       .dxtcSys.dxtc-init > .dxtc-content {
           border-color:#bbbbbb !important;
       }
       #ASPxPageControl1_TC li {
           height:25px;
       }
       .pull-right{
           float:right !important;
       }
       .man {
               position: absolute;
    right: -18px;
    top: 5px;
       }
    </style>

    <style>
        /*Rev 1.0*/

        /*select
        {
            height: 30px !important;
            border-radius: 4px;
            -webkit-appearance: none;
            position: relative;
            z-index: 1;
            background-color: transparent;
            padding-left: 10px !important;
            padding-right: 22px !important;
        }*/

        .dxeButtonEditSys.dxeButtonEdit_PlasticBlue , .dxeTextBox_PlasticBlue
        {
            height: 30px;
            border-radius: 4px;
        }

        .dxeButtonEditButton_PlasticBlue
        {
            background: #094e8c !important;
            border-radius: 4px !important;
            padding: 0 4px !important;
        }

        .dxeDisabled_PlasticBlue
        {
            z-index: 0 !important;
        }

        .dxtcLite_PlasticBlue > .dxtc-stripContainer .dxtc-activeTab, .dxgvFooter_PlasticBlue
        {
            background: #1b5ea4 !important;
        }

        .simple-select::after {
            /*content: '<';*/
            content: url(../../../assests/images/left-arw.png);
            position: absolute;
            top: 26px;
            right: 13px;
            font-size: 16px;
            transform: rotate(269deg);
            font-weight: 500;
            background: #094e8c;
            color: #fff;
            height: 18px;
            display: block;
            width: 26px;
            /* padding: 10px 0; */
            border-radius: 4px;
            text-align: center;
            line-height: 19px;
            z-index: 0;
        }
        .simple-select {
            position: relative;
        }
        .simple-select:disabled::after
        {
            background: #1111113b;
        }
        select.btn
        {
            padding-right: 10px !important;
        }

        /*select.btn
        {
            padding-right: 10px !important;
        }*/

        /*.panel-group .panel
        {
            box-shadow: 1px 1px 8px #1111113b;
            border-radius: 8px;
        }*/

        .dxpLite_PlasticBlue .dxp-current
        {
            background-color: #1b5ea4;
            padding: 3px 5px;
            border-radius: 2px;
        }

        #accordion {
            margin-bottom: 20px;
            margin-top: 10px;
        }

        .dxgvHeader_PlasticBlue {
    background: #1b5ea4 !important;
    color: #fff !important;
}
        #ShowGrid
        {
            margin-top: 10px;
        }

        .pt-25{
                padding-top: 25px !important;
        }

        .dxgvEditFormDisplayRow_PlasticBlue td.dxgv, .dxgvDataRow_PlasticBlue td.dxgv, .dxgvDataRowAlt_PlasticBlue td.dxgv, .dxgvSelectedRow_PlasticBlue td.dxgv, .dxgvFocusedRow_PlasticBlue td.dxgv
        {
            padding: 6px 6px 6px !important;
        }

        #lookupCardBank_DDD_PW-1
        {
                left: -182px !important;
        }
        .plhead a>i
        {
                top: 9px;
        }

        .clsTo
        {
            display: flex;
    align-items: flex-start;
        }

        input[type="radio"], input[type="checkbox"]
        {
            margin-right: 5px;
        }
        .dxeCalendarDay_PlasticBlue
        {
                padding: 6px 6px;
        }

        .modal-dialog
        {
            width: 50%;
        }

        .modal-header
        {
            padding: 8px 4px 8px 10px;
            background: #094e8c !important;
        }

        /*.TableMain100 #ShowGrid , .TableMain100 #ShowGridList , .TableMain100 #ShowGridRet , .TableMain100 #EmployeeGrid , .TableMain100 #GrdEmployee,
        .TableMain100 #SalesDetailsGrid
        {
            max-width: 96% !important;
        }*/

        /*div.dxtcSys > .dxtc-content > div, div.dxtcSys > .dxtc-content > div > div
        {
            width: 95% !important;
        }*/

        .btn-info
        {
                background-color: #1da8d1 !important;
                background-image: none;
        }

        .for-cust-icon {
            position: relative;
            z-index: 1;
        }

        .dxeDisabled_PlasticBlue, .aspNetDisabled
        {
            background: #f3f3f3 !important;
        }

        .dxeButtonDisabled_PlasticBlue
        {
            background: #b5b5b5 !important;
            border-color: #b5b5b5 !important;
        }

        #ddlValTech
        {
            width: 100% !important;
            margin-bottom: 0 !important;
        }

        .dis-flex
        {
            display: flex;
            align-items: baseline;
        }

        input + label
        {
            line-height: 1;
                margin-top: 7px;
        }

        .dxtlHeader_PlasticBlue
        {
            background: #094e8c !important;
        }

        .dxeBase_PlasticBlue .dxichCellSys
        {
            padding-top: 2px !important;
        }

        .pBackDiv
        {
            border-radius: 10px;
            box-shadow: 1px 1px 10px #1111112e;
        }
        .HeaderStyle th
        {
            padding: 5px;
        }

        .dxtcLite_PlasticBlue.dxtc-top > .dxtc-stripContainer
        {
            padding-top: 15px;
        }

        .pt-2
        {
            padding-top: 5px;
        }
        .pt-10
        {
            padding-top: 10px;
        }

        .pt-15
        {
            padding-top: 15px;
        }

        .pb-10
        {
            padding-bottom: 10px;
        }

        .pTop10 {
    padding-top: 20px;
}
        .custom-padd
        {
            padding-top: 4px;
    padding-bottom: 10px;
        }

        input + label
        {
                margin-right: 10px;
        }

        .btn
        {
            margin-bottom: 0;
        }

        .pl-10
        {
            padding-left: 10px;
        }

        .col-md-3>label, .col-md-3>span
        {
            margin-top: 0 !important;
        }

        .devCheck
        {
            margin-top: 5px;
        }

        .mtc-5
        {
            margin-top: 5px;
        }

        .mtc-10
        {
            margin-top: 10px;
        }

        /*select.btn
        {
           position: relative;
           z-index: 0;
        }*/

        select
        {
            margin-bottom: 0;
        }

        .form-control
        {
            background-color: transparent;
        }

        select.btn-radius {
    padding: 4px 8px 6px 11px !important;
}
        .mt-30{
            margin-top: 30px;
        }

        .panel-title h3
        {
            padding-top: 0;
            padding-bottom: 0;
        }

        .btn-radius
        {
            padding: 4px 11px !important;
            border-radius: 4px !important;
        }

        .crossBtn
        {
             right: 30px;
             top: 25px;
        }

        .mb-10
        {
            margin-bottom: 10px;
        }

        .btn-cust
        {
            background-color: #108b47 !important;
            color: #fff;
        }

        .btn-cust:hover
        {
            background-color: #097439 !important;
            color: #fff;
        }

        .gHesder
        {
            background: #1b5ca0 !important;
            color: #ffffff !important;
            padding: 6px 0 6px !important;
        }

        .close
        {
             color: #fff;
             opacity: .5;
             font-weight: 400;
        }

        .mt-27
        {
            margin-top: 27px !important;
        }

        .col-md-3 , .col-md-2
        {
            margin-top: 8px;
        }

        #upldBigLogo , #upldSmallLogo
        {
            width: 100%;
        }

        #DivSetAsDefault
        {
            margin-top: 25px;
        }

        .btn.btn-xs
        {
                font-size: 14px !important;
        }

        .dxpc-content table
        {
             width: 100%;
        }

        input[type="text"], input[type="password"], textarea
        {
            margin-bottom: 0 !important;
        }
        #FromDate , #ToDate , #ASPxFromDate , #ASPxToDate
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        #FromDate_B-1 , #ToDate_B-1 , #ASPxFromDate_B-1 , #ASPxToDate_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #FromDate_B-1 #FromDate_B-1Img , #ToDate_B-1 #ToDate_B-1Img , #ASPxFromDate_B-1 #ASPxFromDate_B-1Img , #ASPxToDate_B-1 #ASPxToDate_B-1Img
        {
            display: none;
        }

        #lblToDate
        {
            padding-left: 10px;
        }

        .dxtc-activeTab:after {
            content: '';
            width: 0;
            height: 0;
            border-left: 8px solid transparent;
            border-right: 8px solid transparent;
            border-top: 9px solid #3e5395;
            position: absolute;
            /* left: 50%; */
            z-index: 3;
            /* bottom: -15px; */
            margin-left: -9px;
        }
        /*Rev end 1.0*/
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main">
        <div class="panel-heading">
        <div class="panel-title">
            <h3><%--Assign Sales Activity--%>
              
                <asp:Label id="lblSalesActivity" runat="server" Text="" />
                
            </h3>
            
         <div id="btncross" class="crossBtn" style="display:none;margin-left:50px;"><a href="Sales_List.aspx"><i class="fa fa-times"></i></a></div>
           
        </div>
    </div>
        <div class="form_main">
           <div class="" id="divadd">
               
                    <div class="clearfix">
                        <div style=" padding-right: 5px; padding-bottom: 10px"> 
                           


                            <table class="pull-left" >
                        <tr>
                            <td>
                                <% if (rights.CanAdd) 
                                   { %>          
                            <a href="javascript:void(0);" onclick="AddButtonClick()" class="btn btn-primary"><span>Add New</span> </a> 
                             <% } %>
                            <a href="javascript:void(0);" onclick="Budget_open()" title="Budget"  class="btn btn-primary">
                               Target Sale Of Product</a>
                            </td>
                            <td>
                                <div style="color: #b5285f; font-weight: bold;" class="clsFrom">
                                    <asp:Label ID="lblFromDate" runat="Server" Text="Assigned From : " CssClass="mylabel1"
                                        Width="110px"></asp:Label>
                                </div>
                            </td>
                            <%--Rev 1.0 : "for-cust-icon" class add--%>
                            <td class="for-cust-icon">
                                <dxe:ASPxDateEdit ID="ASPxFromDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                                    UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeFromDate">
                                    <ButtonStyle Width="13px">
                                    </ButtonStyle>
                                </dxe:ASPxDateEdit>
                                <%--Rev 1.0--%>
                                <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                                <%--Rev end 1.0--%>
                            </td>
                            <td style="padding-left: 15px">
                                <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                                    <asp:Label ID="lblToDate" runat="Server" Text="To  : " CssClass="mylabel1"
                                        Width="50px"></asp:Label>
                                </div>
                            </td>
                            <%--Rev 1.0 : "for-cust-icon" class add--%>
                            <td class="for-cust-icon">
                                <dxe:ASPxDateEdit ID="ASPxToDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                                    UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeToDate">
                                    <ButtonStyle Width="13px">
                                    </ButtonStyle>
                                    <%-- <ClientSideEvents DateChanged="cxdeToDate_OnChaged"></ClientSideEvents>--%>
                                </dxe:ASPxDateEdit>
                                <%--Rev 1.0--%>
                                <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                                <%--Rev end 1.0--%>
                            </td>
                            <td style="padding-left: 10px; padding-top: 0px">
                                <button class="btn btn-success" onclick="btn_ShowRecordsClick()" type="button">Show</button>
                             
                            </td>
                            <%--Mantis Issue 24211--%>
                            <%  if (rights.CanCreateOpportunities) { %>
                                <td style="padding-left: 10px; padding-top: 0px">
                                    <button runat="server" class="btn btn-info" onclick="btn_CreateOpportunities()" type="button">Create Opportunities</button>
                             
                                </td>
                            <% }  %>
                            <%--End of Mantis Issue 24211--%>
                        </tr>

                    </table>

                            <% if (rights.CanExport) 
                                   { %>    
                        <asp:DropDownList ID="drdSalesActivity" runat="server" CssClass="btn btn-sm btn-primary expad" OnSelectedIndexChanged="drdSalesActivity_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Value="0">Export to</asp:ListItem>
                            <asp:ListItem Value="1">PDF</asp:ListItem>
                            <asp:ListItem Value="2">XLS</asp:ListItem>
                            <asp:ListItem Value="3">RTF</asp:ListItem>
                            <asp:ListItem Value="4">CSV</asp:ListItem>
                        </asp:DropDownList>  
                             <% } %>
                        </div>
                    </div>
                </div>


         <div class="" id="divdetails" style="display:none">
                    <div class="clearfix">
                        <div style="float: left; padding-right: 5px;">               
                       
                              <asp:DropDownList ID="drdSalesActivityDetails" runat="server" CssClass="btn btn-sm btn-primary expad" OnSelectedIndexChanged="drdSalesActivityDetails_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Value="0">Export to</asp:ListItem>
                            <asp:ListItem Value="1">PDF</asp:ListItem>
                            <asp:ListItem Value="2">XLS</asp:ListItem>
                            <asp:ListItem Value="3">RTF</asp:ListItem>
                            <asp:ListItem Value="4">CSV</asp:ListItem>
                        </asp:DropDownList>  

                              <asp:Button ID="btnActivity"   Text="Create Activity" runat="server"  CssClass="btn btn-primary fa fa-save" style="padding: 5px 10px;display:none"  OnClick="btnActivity_Click" />                             
                        </div>
                    </div>
                </div>


        <div class="" id="divDocumentdetails" style="display:none">
                    <div class="clearfix">
                        <div style="float: left; padding-right: 5px;">               
                       
                              <asp:DropDownList ID="drdSalesDocumentDetails" runat="server" CssClass="btn btn-sm btn-primary expad" OnSelectedIndexChanged="drdSalesDocumentDetails_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Value="0">Export to</asp:ListItem>
                            <asp:ListItem Value="1">PDF</asp:ListItem>
                            <asp:ListItem Value="2">XLS</asp:ListItem>
                            <asp:ListItem Value="3">RTF</asp:ListItem>
                            <asp:ListItem Value="4">CSV</asp:ListItem>
                        </asp:DropDownList>  

                              
                        </div>
                    </div>
                </div>


        <div class="" id="divFuturedetails" style="display:none">
                    <div class="clearfix">
                        <div style="float: left; padding-right: 5px;">               
                       
                              <asp:DropDownList ID="drdSalesFutureDetails" runat="server" CssClass="btn btn-sm btn-primary expad" OnSelectedIndexChanged="drdSalesFutureDetails_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Value="0">Export to</asp:ListItem>
                            <asp:ListItem Value="1">PDF</asp:ListItem>
                            <asp:ListItem Value="2">XLS</asp:ListItem>
                            <asp:ListItem Value="3">RTF</asp:ListItem>
                            <asp:ListItem Value="4">CSV</asp:ListItem>
                        </asp:DropDownList>  

                              
                        </div>
                    </div>
                </div>
        <div class="" id="divCloseddetails" style="display:none">
                    <div class="clearfix">
                        <div style="float: left; padding-right: 5px;">               
                       
                              <asp:DropDownList ID="drdsalesClosedDetails" runat="server" CssClass="btn btn-sm btn-primary expad" OnSelectedIndexChanged="drdsalesClosedDetails_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Value="0">Export to</asp:ListItem>
                            <asp:ListItem Value="1">PDF</asp:ListItem>
                            <asp:ListItem Value="2">XLS</asp:ListItem>
                            <asp:ListItem Value="3">RTF</asp:ListItem>
                            <asp:ListItem Value="4">CSV</asp:ListItem>
                        </asp:DropDownList>  

                              
                        </div>
                    </div>
                </div>

         <div class="" id="divBudgetdetails">
                    <div class="clearfix">
                        <div style="float: left; padding-right: 5px;"> 
                              
                            
                               </div>
                    </div>
                </div>              
                       
        <div>
        <table class="TableMain100">
            <tr>
                <td>
                   <%-- DataSourceID="SalesDataSource" --%>
                                        <div id="GridDiv">
                                            <table width="100%">
                                                <tr>
                                                    <td id="SaleListID">

                                                        <dxe:ASPxGridView ID="SaleGrid" KeyFieldName="act_id" runat="server" AutoGenerateColumns="False" ClientInstanceName="Sgrid"                                                            
SettingsCookies-Enabled="true" SettingsCookies-StorePaging="true" SettingsCookies-StoreFiltering="true" SettingsCookies-StoreGroupingAndSorting="true" SettingsBehavior-AllowFocusedRow="true" 
                                                           
                                                            OnDataBinding="SaleGrid_DataBinding"
                                                            
                                                              SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false"   Width="100%" OnCustomCallback="SGrid_CustomCallback">
                                                           <SettingsSearchPanel Visible="true" Delay="6000" />
                                                             <Columns>

                                                                
                                                                 <dxe:GridViewDataTextColumn FieldName="act_activityName" Caption="Activity Name"  Visible="false" VisibleIndex="0" Width="14%">
                                                                     <Settings AllowAutoFilterTextInputTimer="False" />
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn FieldName="ActivityID" Caption="Activity ID"  VisibleIndex="1" Width="8%" Settings-AllowAutoFilterTextInputTimer="False">
                                                                      <Settings AllowAutoFilterTextInputTimer="False" />
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn FieldName="Assigned To" Caption="Assigned To" VisibleIndex="2" Width="14%" Settings-AllowAutoFilterTextInputTimer="False">
                                                                      <Settings AllowAutoFilterTextInputTimer="False" />
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn FieldName="Assigned Date" Caption="Assigned Date" VisibleIndex="3" Settings-AllowAutoFilterTextInputTimer="False"
                                                                    Width="18%">
                                                                      <Settings AllowAutoFilterTextInputTimer="False" />
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn FieldName="Assigned By" Caption="Assigned By" VisibleIndex="4" Settings-AllowAutoFilterTextInputTimer="False"
                                                                    Width="18%">
                                                                    
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn FieldName="Supervisor"
                                                                    VisibleIndex="5" Settings-AllowAutoFilterTextInputTimer="False" >
                                                                </dxe:GridViewDataTextColumn> 
                                                                <dxe:GridViewDataTextColumn FieldName="ProductClassName"  Caption="Product Class"
                                                                    VisibleIndex="6" Settings-AllowAutoFilterTextInputTimer="False" >
                                                                </dxe:GridViewDataTextColumn>

                                                               <%-- <dxe:GridViewDataTextColumn VisibleIndex="7" Caption="Details" Visible="false">
                                                                    <DataItemTemplate>
                                                                      
                                                                    </DataItemTemplate>
                                                                    <EditFormSettings Visible="False" />
                                                                </dxe:GridViewDataTextColumn>--%>

                                                                 <dxe:GridViewDataTextColumn FieldName="Industry"  VisibleIndex="7"  Caption="Industry">
                                                                       <Settings AllowAutoFilterTextInputTimer="False" />
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn Caption="" VisibleIndex="8" Width="170px">
                                                                    <CellStyle HorizontalAlign="Center">
                                                                    </CellStyle>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <HeaderTemplate>
                                                                        Actions
                                   
                                                                    </HeaderTemplate>
                                                                    <DataItemTemplate>
                                                                          <% if (rights.CanHistory)
                            { %>
                        <a href="javascript:void(0);" onclick="OnViewClick('<%# Eval("ID") %>')" class="pad" title="View History">
                            <img src="../../../assests/images/history.png" /></a>
                        <% } %>
                                                                        <% if (rights.CanReassignSupervisor){ %>
                                                                          <asp:LinkButton  runat="server" ID="lnkReassign"  OnClientClick='<%# string.Format("return BindNewSupervisor(\"{0}\", \"{1}\", \"{2}\")", Eval("ID"), Eval("act_supervisor"),Eval("act_assignedTo")) %>' ><img  src="/assests/images/reassign.png"  title="Reassign Supervisor"></asp:LinkButton>
                                                                        <% } %>

                                                                        <% if (rights.CanReassignSalesman){ %>
                                                                          <asp:LinkButton  runat="server" ID="LinkButton2"  OnClientClick='<%# string.Format(" return ShowDetailReassign(\"{0}\", \"{1}\")", Eval("act_id"),Eval("act_assignedTo")) %>' ><img  src="/assests/images/reassign.png"  title="Reassign Salesman"></asp:LinkButton>
                                                                         <% } %>

                                                                          <% if (rights.CanEdit)
                                                                        { %>
                                                                          <a href="javascript:void(0)" onclick="ShowDetail('<%#Eval("ID") %>','<%#Eval("cnt_id") %>')" title="Details"  class="pad" > <img src="/assests/images/Info.png" /></a></a>
                                                                        <% } %>

                                                                         <% if (rights.CanView)
                                        { %>
                                    <a href="javascript:void(0);" onclick="salesView('<%# Eval("ID") %>')" title="Show assign sales activity" class="pad">
                                        <img src="../../../assests/images/viewIcon.png" /></a><% } %>

                                                                        <% if (rights.CanDelete)
                                                                           { %>
                                                                        <a href="javascript:void(0);" onclick="DeleteRow('<%#Eval("ID") %>')" title="Delete" class="pad">
                                                                            <img src="/assests/images/Delete.png"/ title="Delete"></a>
                                                                        <% } %>
                                                                    </DataItemTemplate>
                                                                      <Settings AllowAutoFilterTextInputTimer="False" />
                                                                </dxe:GridViewDataTextColumn>

                                                            </Columns>

                                                            <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                                                            <SettingsPager PageSize="10">
                                                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                                                            </SettingsPager>
                                                            <SettingsCommandButton>
                                                                <DeleteButton Image-Url="../../../assests/images/Delete.png" ButtonType="Image" Image-AlternateText="Delete">
                                                                </DeleteButton>


                                                            </SettingsCommandButton>
                                                            <Styles>
                                                                <LoadingPanel ImageSpacing="10px">
                                                                </LoadingPanel>
                                                                <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                                </Header>
                                                                <Cell CssClass="gridcellleft">
                                                                </Cell>
                                                            </Styles>
                                                         <%--   <SettingsPager NumericButtonCount="20" ShowSeparators="True">
                                                                <FirstPageButton Visible="True">
                                                                </FirstPageButton>
                                                                <LastPageButton Visible="True">
                                                                </LastPageButton>
                                                            </SettingsPager>--%>
                                                            <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" />

                                                            <ClientSideEvents EndCallback="function(s, e) {
	LastCall(s.cpHeight);
}" />
                                                        </dxe:ASPxGridView>
                                                    </td>


                                                    <td style="display: none" id="SaleDetailsListID">

                                                         <dxe:ASPxPageControl  style="display: none" ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" Width="100%"
                        ClientInstanceName="page">


                        <TabPages><dxe:TabPage Text="New Sales" Name="New Sales">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">

                                                        <dxe:ASPxGridView ID="SalesDetailsGrid" runat="server" AutoGenerateColumns="False" ClientInstanceName="grid"
                                                            Width="100%"  DataSourceID="SalesDetailsGridDataSource" OnCustomCallback="SalesDetailsGrid_CustomCallback">
                                                            <Columns>
                                                              <%--  <dxe:GridViewCommandColumn VisibleIndex="0">
                                                                </dxe:GridViewCommandColumn>--%>
                                                           <%--     <dxe:GridViewDataTextColumn FieldName="Status" Visible="False" VisibleIndex="-1">
                                                                </dxe:GridViewDataTextColumn>--%>

                                                             <dxe:GridViewDataTextColumn Caption="Activity" VisibleIndex="0" FieldName="LeadId" Visible="false">
                                <DataItemTemplate>
                                    <dxe:ASPxCheckBox ID="chkDetail" runat="server">
                                    </dxe:ASPxCheckBox>                              

                                    <dxe:ASPxTextBox ID="lblActNo" runat="server" Width="100%" Visible="false"
                                        NullText="0" Value='<%# Eval("LeadId") %>'>

                                        
                                    </dxe:ASPxTextBox>

                                  

                                </DataItemTemplate>
                            </dxe:GridViewDataTextColumn>


                                                                 <dxe:GridViewDataTextColumn FieldName="Assigned To" VisibleIndex="1" Caption="Assigned To">
                                                                </dxe:GridViewDataTextColumn>

                                                                 <dxe:GridViewDataTextColumn FieldName="Assigned By" VisibleIndex="2"  Caption="Assigned By">
                                                                </dxe:GridViewDataTextColumn>

                                                                <dxe:GridViewDataTextColumn FieldName="Industry"  VisibleIndex="3"  Caption="Industry">
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn FieldName="Name" ReadOnly="True" VisibleIndex="4" Caption="Customer/Lead Name">
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn FieldName="Address" ReadOnly="True" VisibleIndex="5"
                                                                    Width="18%" Visible="false">
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn FieldName="Phone" ReadOnly="True" VisibleIndex="6"
                                                                    Width="8%" Visible="false">
                                                                </dxe:GridViewDataTextColumn>
                                                             <%--   <dxe:GridViewDataTextColumn FieldName="ProductType" ReadOnly="True" Visible="False"
                                                                    VisibleIndex="4"  Width="18%">
                                                                </dxe:GridViewDataTextColumn>--%>
                                                                <dxe:GridViewDataTextColumn FieldName="Id" ReadOnly="True" Visible="False" VisibleIndex="7">
                                                                    <EditFormSettings Visible="False" />
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn FieldName="Amount" Visible="False" VisibleIndex="8">
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn FieldName="Product" ReadOnly="True" Visible="False"
                                                                    VisibleIndex="9">
                                                                </dxe:GridViewDataTextColumn>
                                                               
                                                                <dxe:GridViewDataTextColumn FieldName="ProductType" VisibleIndex="10"  Caption="Product Type" Visible="false">
                                                                </dxe:GridViewDataTextColumn>

                                                                  <dxe:GridViewDataTextColumn FieldName="ProductName" VisibleIndex="11"  Caption="Product Name">
                                                                </dxe:GridViewDataTextColumn>
                                                                 <dxe:GridViewDataTextColumn FieldName="ExpectedTime" VisibleIndex="12"  Caption="Date Of Completion">
                                                                      <CellStyle HorizontalAlign="Center">
                                                                    </CellStyle>
                                                                </dxe:GridViewDataTextColumn>

                                                                 <dxe:GridViewDataTextColumn VisibleIndex="13" Caption="History">
                                                                       <CellStyle HorizontalAlign="Center">
                                                                    </CellStyle>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <DataItemTemplate>
                                                                        <a href="javascript:void(0)" onclick="ShowHistory('<%#Eval("sls_id") %>')">
                                                                            <img  src="/assests/images/history.png" width="16" height="16" title="History"></a>
                                                                    </DataItemTemplate>
                                                                    <EditFormSettings Visible="False" />
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn VisibleIndex="14" Caption="Actions">
                                                                      <CellStyle HorizontalAlign="Center">
                                                                    </CellStyle>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <DataItemTemplate>
                                                                        
                                                                        <a href="javascript:void(0)" onclick="ShowDetailReassign('<%#Eval("sls_id") %>')" class="pad">

                                                                             <img  src="/assests/images/reassign.png"  title="Reassign"></a>
                                                                        </a>
                                                                    </DataItemTemplate>
                                                                    <EditFormSettings Visible="False" />
                                                                </dxe:GridViewDataTextColumn>

                                                                
                                                                  <dxe:GridViewDataTextColumn  VisibleIndex="14" Caption="Create Activity" Visible="false">
                                                                    <DataItemTemplate>
                                                                          
                                                                 
                                                                        <a href="javascript:void(0)" onclick="ShowCreateActivity('<%#Eval("LeadId") %>','<%#Eval("sls_id") %>','<%#Eval("sls_activity_id") %>','<%#Eval("act_assignedTo") %>','<%#Eval("act_activityNo") %>','<%#Eval("act_assign_task") %>')">Create Activity</a>
                                                                    </DataItemTemplate>
                                                                    <EditFormSettings Visible="False" />
                                                                </dxe:GridViewDataTextColumn>
                                                               <%-- <dxe:GridViewDataTextColumn VisibleIndex="7">
                                                                    <DataItemTemplate>
                                                                        <a href="javascript:void(0)" onclick="ShowDetail('<%#Eval("ProductTypePath") %>')">Show</a>
                                                                    </DataItemTemplate>
                                                                    <EditFormSettings Visible="False" />
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn VisibleIndex="8" Caption="History">
                                                                    <DataItemTemplate>
                                                                        <a href="javascript:void(0)" onclick="ShowHistory('<%#Eval("LeadId") %>')">History</a>
                                                                    </DataItemTemplate>
                                                                    <EditFormSettings Visible="False" />
                                                                </dxe:GridViewDataTextColumn>--%>
                                                            </Columns>
                                                            <Styles>
                                                                <LoadingPanel ImageSpacing="10px">
                                                                </LoadingPanel>
                                                                <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                                </Header>
                                                                <Cell CssClass="gridcellleft">
                                                                </Cell>
                                                            </Styles>
                                                            <SettingsPager NumericButtonCount="10" ShowSeparators="True">
                                                                <FirstPageButton Visible="True">
                                                                </FirstPageButton>
                                                                <LastPageButton Visible="True">
                                                                </LastPageButton>
                                                            </SettingsPager>
                                                            <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" />

                                                          
                                                          <clientsideevents endcallback="function(s, e) {
	LastActivityDetailsCall(s.cpHeight);
}" />
                                                        </dxe:ASPxGridView>
                                         </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>

                                                         <dxe:TabPage Text="Document Collection" Name="Document Collection">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">

                                         <dxe:ASPxGridView ID="SalesDocumentDetailsGrid" runat="server" AutoGenerateColumns="False" ClientInstanceName="gridDocument"
                                                            Width="100%"  DataSourceID="SalesDetailsDocumentGridDataSource" OnCustomCallback="SalesDocumentDetailsGrid_CustomCallback">
                                                            <Columns>
                                                              <%--  <dxe:GridViewCommandColumn VisibleIndex="0">
                                                                </dxe:GridViewCommandColumn>--%>
                                                           <%--     <dxe:GridViewDataTextColumn FieldName="Status" Visible="False" VisibleIndex="-1">
                                                                </dxe:GridViewDataTextColumn>--%>

                                                             <dxe:GridViewDataTextColumn Caption="Activity" VisibleIndex="0" FieldName="LeadId" Visible="false">
                                <DataItemTemplate>
                                    <dxe:ASPxCheckBox ID="chkDetail" runat="server">
                                    </dxe:ASPxCheckBox>                              

                                    <dxe:ASPxTextBox ID="lblActNo" runat="server" Width="100%" Visible="false"
                                        NullText="0" Value='<%# Eval("LeadId") %>'>

                                        
                                    </dxe:ASPxTextBox>

                                  

                                </DataItemTemplate>
                            </dxe:GridViewDataTextColumn>


                                                                 <dxe:GridViewDataTextColumn FieldName="Assigned To" VisibleIndex="1" Caption="Assigned To">
                                                                </dxe:GridViewDataTextColumn>

                                                                 <dxe:GridViewDataTextColumn FieldName="Assigned By" VisibleIndex="2"  Caption="Assigned By">
                                                                </dxe:GridViewDataTextColumn>

                                                                <dxe:GridViewDataTextColumn FieldName="Industry"  VisibleIndex="3"  Caption="Industry">
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn FieldName="Name" ReadOnly="True" VisibleIndex="4" Caption="Customer/Lead Name">
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn FieldName="Address" ReadOnly="True" VisibleIndex="5"
                                                                    Width="18%" Visible="false">
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn FieldName="Phone" ReadOnly="True" VisibleIndex="6"
                                                                    Width="8%" Visible="false">
                                                                </dxe:GridViewDataTextColumn>
                                                             <%--   <dxe:GridViewDataTextColumn FieldName="ProductType" ReadOnly="True" Visible="False"
                                                                    VisibleIndex="4"  Width="18%">
                                                                </dxe:GridViewDataTextColumn>--%>
                                                                <dxe:GridViewDataTextColumn FieldName="Id" ReadOnly="True" Visible="False" VisibleIndex="7">
                                                                    <EditFormSettings Visible="False" />
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn FieldName="Amount" Visible="False" VisibleIndex="8">
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn FieldName="Product" ReadOnly="True" Visible="False"
                                                                    VisibleIndex="9">
                                                                </dxe:GridViewDataTextColumn>
                                                               
                                                                <dxe:GridViewDataTextColumn FieldName="ProductType" VisibleIndex="10"  Caption="Product Type" Visible="false">
                                                                </dxe:GridViewDataTextColumn>

                                                                  <dxe:GridViewDataTextColumn FieldName="ProductName" VisibleIndex="11"  Caption="Product Name">
                                                                </dxe:GridViewDataTextColumn>
                                                                 <dxe:GridViewDataTextColumn FieldName="ExpectedTime" VisibleIndex="12"  Caption="Date Of Completion">
                                                                      <CellStyle HorizontalAlign="Center">
                                                                    </CellStyle>
                                                                </dxe:GridViewDataTextColumn>

                                                                 <dxe:GridViewDataTextColumn VisibleIndex="13" Caption="History">
                                                                       <CellStyle HorizontalAlign="Center">
                                                                    </CellStyle>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <DataItemTemplate>
                                                                        <a href="javascript:void(0)" onclick="ShowHistory('<%#Eval("sls_id") %>')">
                                                                            <img  src="/assests/images/history.png" width="16" height="16" title="History"></a>
                                                                    </DataItemTemplate>
                                                                    <EditFormSettings Visible="False" />
                                                                </dxe:GridViewDataTextColumn>
                                                               
                                                            </Columns>
                                                            <Styles>
                                                                <LoadingPanel ImageSpacing="10px">
                                                                </LoadingPanel>
                                                                <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                                </Header>
                                                                <Cell CssClass="gridcellleft">
                                                                </Cell>
                                                            </Styles>
                                                            <SettingsPager NumericButtonCount="10" ShowSeparators="True">
                                                                <FirstPageButton Visible="True">
                                                                </FirstPageButton>
                                                                <LastPageButton Visible="True">
                                                                </LastPageButton>
                                                            </SettingsPager>
                                                            <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" />
                                                            <%-- <clientsideevents endcallback="function(s, e) {
	LastActivityDetailsCall(s.cpHeight);
}" />--%>
                                                        </dxe:ASPxGridView>
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Text="Future Sales" Name="Future Sales">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">

                                        <dxe:ASPxGridView ID="SalesFutureDetailsGrid" runat="server" AutoGenerateColumns="False" ClientInstanceName="gridFuture"
                                                            Width="100%"  DataSourceID="SalesDetailsFutureGridDataSource" OnCustomCallback="SalesFutureDetailsGrid_CustomCallback">
                                                            <Columns>
                                                              <%--  <dxe:GridViewCommandColumn VisibleIndex="0">
                                                                </dxe:GridViewCommandColumn>--%>
                                                           <%--     <dxe:GridViewDataTextColumn FieldName="Status" Visible="False" VisibleIndex="-1">
                                                                </dxe:GridViewDataTextColumn>--%>

                                                             <dxe:GridViewDataTextColumn Caption="Activity" VisibleIndex="0" FieldName="LeadId" Visible="false">
                                <DataItemTemplate>
                                    <dxe:ASPxCheckBox ID="chkDetail" runat="server">
                                    </dxe:ASPxCheckBox>                              

                                    <dxe:ASPxTextBox ID="lblActNo" runat="server" Width="100%" Visible="false"
                                        NullText="0" Value='<%# Eval("LeadId") %>'>

                                        
                                    </dxe:ASPxTextBox>

                                  

                                </DataItemTemplate>
                            </dxe:GridViewDataTextColumn>


                                                                 <dxe:GridViewDataTextColumn FieldName="Assigned To" VisibleIndex="1" Caption="Assigned To">
                                                                </dxe:GridViewDataTextColumn>

                                                                 <dxe:GridViewDataTextColumn FieldName="Assigned By" VisibleIndex="2"  Caption="Assigned By">
                                                                </dxe:GridViewDataTextColumn>

                                                                <dxe:GridViewDataTextColumn FieldName="Industry"  VisibleIndex="3"  Caption="Industry">
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn FieldName="Name" ReadOnly="True" VisibleIndex="4" Caption="Customer/Lead Name">
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn FieldName="Address" ReadOnly="True" VisibleIndex="5"
                                                                    Width="18%" Visible="false">
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn FieldName="Phone" ReadOnly="True" VisibleIndex="6"
                                                                    Width="8%" Visible="false">
                                                                </dxe:GridViewDataTextColumn>
                                                             <%--   <dxe:GridViewDataTextColumn FieldName="ProductType" ReadOnly="True" Visible="False"
                                                                    VisibleIndex="4"  Width="18%">
                                                                </dxe:GridViewDataTextColumn>--%>
                                                                <dxe:GridViewDataTextColumn FieldName="Id" ReadOnly="True" Visible="False" VisibleIndex="7">
                                                                    <EditFormSettings Visible="False" />
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn FieldName="Amount" Visible="False" VisibleIndex="8">
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn FieldName="Product" ReadOnly="True" Visible="False"
                                                                    VisibleIndex="9">
                                                                </dxe:GridViewDataTextColumn>
                                                               
                                                                <dxe:GridViewDataTextColumn FieldName="ProductType" VisibleIndex="10"  Caption="Product Type" Visible="false">
                                                                </dxe:GridViewDataTextColumn>

                                                                  <dxe:GridViewDataTextColumn FieldName="ProductName" VisibleIndex="11"  Caption="Product Name">
                                                                </dxe:GridViewDataTextColumn>
                                                                 <dxe:GridViewDataTextColumn FieldName="ExpectedTime" VisibleIndex="12"  Caption="Date Of Completion">
                                                                      <CellStyle HorizontalAlign="Center">
                                                                    </CellStyle>
                                                                </dxe:GridViewDataTextColumn>

                                                                 <dxe:GridViewDataTextColumn VisibleIndex="13" Caption="History">
                                                                       <CellStyle HorizontalAlign="Center">
                                                                    </CellStyle>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <DataItemTemplate>
                                                                        <a href="javascript:void(0)" onclick="ShowHistory('<%#Eval("sls_id") %>')">
                                                                            <img  src="/assests/images/history.png" width="16" height="16" title="History"></a>
                                                                    </DataItemTemplate>
                                                                    <EditFormSettings Visible="False" />
                                                                </dxe:GridViewDataTextColumn>
                                                               
                                                            </Columns>
                                                            <Styles>
                                                                <LoadingPanel ImageSpacing="10px">
                                                                </LoadingPanel>
                                                                <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                                </Header>
                                                                <Cell CssClass="gridcellleft">
                                                                </Cell>
                                                            </Styles>
                                                            <SettingsPager NumericButtonCount="10" ShowSeparators="True">
                                                                <FirstPageButton Visible="True">
                                                                </FirstPageButton>
                                                                <LastPageButton Visible="True">
                                                                </LastPageButton>
                                                            </SettingsPager>
                                                            <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" />
                                                             <%--<clientsideevents endcallback="function(s, e) {
	LastActivityDetailsCall(s.cpHeight);
}" />--%>
                                                        </dxe:ASPxGridView>
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Text="Closed Sales" Name="Closed Sales">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">

                                        <dxe:ASPxGridView ID="SalesClosedDetailsGrid" runat="server" AutoGenerateColumns="False" ClientInstanceName="gridClosed"
                                                            Width="100%"  DataSourceID="SalesDetailsClosedGridDataSource" OnCustomCallback="SalesClosedDetailsGrid_CustomCallback">
                                                            <Columns>
                                                              <%--  <dxe:GridViewCommandColumn VisibleIndex="0">
                                                                </dxe:GridViewCommandColumn>--%>
                                                           <%--     <dxe:GridViewDataTextColumn FieldName="Status" Visible="False" VisibleIndex="-1">
                                                                </dxe:GridViewDataTextColumn>--%>

                                                             <dxe:GridViewDataTextColumn Caption="Activity" VisibleIndex="0" FieldName="LeadId" Visible="false">
                                <DataItemTemplate>
                                    <dxe:ASPxCheckBox ID="chkDetail" runat="server">
                                    </dxe:ASPxCheckBox>                              

                                    <dxe:ASPxTextBox ID="lblActNo" runat="server" Width="100%" Visible="false"
                                        NullText="0" Value='<%# Eval("LeadId") %>'>

                                        
                                    </dxe:ASPxTextBox>

                                  

                                </DataItemTemplate>
                            </dxe:GridViewDataTextColumn>


                                                                 <dxe:GridViewDataTextColumn FieldName="Assigned To" VisibleIndex="1" Caption="Assigned To">
                                                                </dxe:GridViewDataTextColumn>

                                                                 <dxe:GridViewDataTextColumn FieldName="Assigned By" VisibleIndex="2"  Caption="Assigned By">
                                                                </dxe:GridViewDataTextColumn>

                                                                <dxe:GridViewDataTextColumn FieldName="Industry"  VisibleIndex="3"  Caption="Industry">
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn FieldName="Name" ReadOnly="True" VisibleIndex="4" Caption="Customer/Lead Name">
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn FieldName="Address" ReadOnly="True" VisibleIndex="5"
                                                                    Width="18%" Visible="false">
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn FieldName="Phone" ReadOnly="True" VisibleIndex="6"
                                                                    Width="8%" Visible="false">
                                                                </dxe:GridViewDataTextColumn>
                                                             <%--   <dxe:GridViewDataTextColumn FieldName="ProductType" ReadOnly="True" Visible="False"
                                                                    VisibleIndex="4"  Width="18%">
                                                                </dxe:GridViewDataTextColumn>--%>
                                                                <dxe:GridViewDataTextColumn FieldName="Id" ReadOnly="True" Visible="False" VisibleIndex="7">
                                                                    <EditFormSettings Visible="False" />
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn FieldName="Amount" Visible="False" VisibleIndex="8">
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn FieldName="Product" ReadOnly="True" Visible="False"
                                                                    VisibleIndex="9">
                                                                </dxe:GridViewDataTextColumn>
                                                               
                                                                <dxe:GridViewDataTextColumn FieldName="ProductType" VisibleIndex="10"  Caption="Product Type" Visible="false">
                                                                </dxe:GridViewDataTextColumn>

                                                                  <dxe:GridViewDataTextColumn FieldName="ProductName" VisibleIndex="11"  Caption="Product Name">
                                                                </dxe:GridViewDataTextColumn>
                                                                 <dxe:GridViewDataTextColumn FieldName="ExpectedTime" VisibleIndex="12"  Caption="Date Of Completion">
                                                                      <CellStyle HorizontalAlign="Center">
                                                                    </CellStyle>
                                                                </dxe:GridViewDataTextColumn>

                                                                 <dxe:GridViewDataTextColumn VisibleIndex="13" Caption="History">
                                                                       <CellStyle HorizontalAlign="Center">
                                                                    </CellStyle>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <DataItemTemplate>
                                                                        <a href="javascript:void(0)" onclick="ShowHistory('<%#Eval("sls_id") %>')">
                                                                            <img  src="/assests/images/history.png" width="16" height="16" title="History"></a>
                                                                    </DataItemTemplate>
                                                                    <EditFormSettings Visible="False" />
                                                                </dxe:GridViewDataTextColumn>
                                                               
                                                            </Columns>
                                                            <Styles>
                                                                <LoadingPanel ImageSpacing="10px">
                                                                </LoadingPanel>
                                                                <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                                </Header>
                                                                <Cell CssClass="gridcellleft">
                                                                </Cell>
                                                            </Styles>
                                                            <SettingsPager NumericButtonCount="10" ShowSeparators="True">
                                                                <FirstPageButton Visible="True">
                                                                </FirstPageButton>
                                                                <LastPageButton Visible="True">
                                                                </LastPageButton>
                                                            </SettingsPager>
                                                            <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" />
                                                           <%--  <clientsideevents endcallback="function(s, e) {
	LastActivityDetailsCall(s.cpHeight);
}" />--%>
                                                        </dxe:ASPxGridView>
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>

                          
                        </TabPages>


                        <ClientSideEvents ActiveTabChanged="function(s, e) {
	                                            var activeTab   = page.GetActiveTab();
	                                            var Tab0 = page.GetTab(0);
	                                            var Tab1 = page.GetTab(1);
	                                            var Tab2 = page.GetTab(2);
	                                            var Tab3 = page.GetTab(3);
	                                          
	                                            if(activeTab == Tab0)
	                                            {
	                                                disp_prompt('tab0');
	                                            }
	                                            if(activeTab == Tab1)
	                                            {
	                                                disp_prompt('tab1');
	                                            }
	                                            else if(activeTab == Tab2)
	                                            {
	                                                disp_prompt('tab2');
	                                            }
	                                            else if(activeTab == Tab3)
	                                            {
	                                                disp_prompt('tab3');
	                                            }
                                              
	                                            }"></ClientSideEvents>
                        <ContentStyle>
                            <Border BorderColor="#002D96" BorderStyle="Solid" BorderWidth="1px" />
                        </ContentStyle>
                        <LoadingPanelStyle ImageSpacing="6px">
                        </LoadingPanelStyle>
                        <TabStyle Font-Size="12px">
                        </TabStyle>
                    </dxe:ASPxPageControl>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div id="FrameDiv" style="display: none;">
                                            <iframe width="100%" id="ShowDetails" src="" frameborder="0"></iframe>
                                        </div>
                                 
                </td>
            </tr>
        </table>
            </div>
        <asp:SqlDataSource ID="SalesDataSource" runat="server"  ></asp:SqlDataSource>
          <asp:SqlDataSource ID="SalesDetailsGridDataSource" runat="server" ></asp:SqlDataSource>

          <asp:SqlDataSource ID="SalesDetailsDocumentGridDataSource" runat="server" ></asp:SqlDataSource>
          <asp:SqlDataSource ID="SalesDetailsFutureGridDataSource" runat="server" ></asp:SqlDataSource>
          <asp:SqlDataSource ID="SalesDetailsClosedGridDataSource" runat="server" ></asp:SqlDataSource>

         <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="true" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true" >
        </dxe:ASPxGridViewExporter>

            <dxe:ASPxPopupControl ID="Popup_Reassign" runat="server" ClientInstanceName="cPopup_Reassign"
            Width="400px" HeaderText="Reassign Salesman" PopupHorizontalAlign="WindowCenter"
            BackColor="white" Height="100px" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <%--<div style="Width:400px;background-color:#FFFFFF;margin:0px;border:1px solid red;">--%>
                    <div class="Top clearfix">

                        <table style="width:94%">
                            <tr>
                                <td>Assign To <span style="color: red">*</span>
                                </td>
                                <td class="relative">
                                    <asp:ListBox ID="lstAssignTo" CssClass="hide" runat="server" Font-Size="12px" TabIndex="0" Height="90px" Width="100%" data-placeholder="Select..."></asp:ListBox>

                                    <asp:TextBox ID="txtAssign" runat="server" Width="100%" Style="display: none"></asp:TextBox>
                                    <span id="MandatoryAssign" style="display: none">
                                        <img id="gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                                    <asp:HiddenField ID="hdnAssign" runat="server" />
                                    <asp:HiddenField ID="hdnAssignText" runat="server" />
                                </td>
                            </tr>
                            <tr style="padding-top:30px;"><td>Remarks<span style="color: red">*</span></td>
                                <td class="relative"   style="padding-top:5px;">
                                     <dxe:ASPxMemo ID="txtInstNote" runat="server" Width="100%" Height="50px" ClientInstanceName="txtRemarks"></dxe:ASPxMemo>
                                                                        <span id="MandatoryRemarks" style="display: none">
                                        <img id="gridHistory_DXPEForm_efnew_DXEFL_DXEditor12_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                                </td></tr>

                              <tr>    <td >
                                      <asp:CheckBox ID="chkMail" runat="server"  ClientIDMode="Static"  /> Send Mail
                                     </td> <td colspan="2" style="padding-left: 121px;"></td></tr>
                       
                            <tr style="padding-top:30px;">
                                <td colspan="3" style="padding-left: 121px;">
                                    <input id="btnSave" class="btn btn-primary" onclick="Call_save()" type="button" value="Save" />
                                    <input id="btnCancel" class="btn btn-danger" onclick="CancelReassign_save()" type="button" value="Cancel" />
                                </td>

                            </tr>
                        </table>


                    </div>

                </dxe:PopupControlContentControl>
            </ContentCollection>
            <HeaderStyle BackColor="LightGray" ForeColor="Black" />

          
        </dxe:ASPxPopupControl>


        <dxe:ASPxPopupControl ID="ASPXPopupControl2" runat="server"
                CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popupbudget" Height="500px"
                Width="1310px" HeaderText="Budget" Modal="true" AllowResize="true" ResizingMode="Postponed">
                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">
                    </dxe:PopupControlContentControl>
                </ContentCollection>
                
                 <ClientSideEvents CloseUp="BudgetAfterHide" />
            </dxe:ASPxPopupControl>



          <dxe:ASPxPopupControl ID="Popup_ReassignSupervisor" runat="server" ClientInstanceName="cPopup_ReassignSupervisor"
            Width="400px" HeaderText="Reassign Supervisor" PopupHorizontalAlign="WindowCenter"
            BackColor="white" Height="150px" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <%--<div style="Width:400px;background-color:#FFFFFF;margin:0px;border:1px solid red;">--%>
                    <div class="Top clearfix">

                        <table style="width:94%">
                            <tr>
                                <td>Select Supervisor<span style="color: red">*</span>
                                </td>
                                <td class="relative">
                                    <asp:ListBox ID="lstSupervisorAssignTo" CssClass="hide" runat="server" Font-Size="12px" TabIndex="0" Height="90px" Width="100%" data-placeholder="Select..."></asp:ListBox>

                                    <asp:TextBox ID="txtSupervisorAssign" runat="server" Width="100%" Style="display: none"></asp:TextBox>
                                    <span id="MandatorySupervisorAssign" style="display: none" class="man">
                                        <img id="gridHistory_DXPEForm_efnew_DXEFL_DXEditor1112_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                                    <asp:HiddenField ID="hdnSupervisorAssign" runat="server" />
                                    <asp:HiddenField ID="hdnSupervisorAssignText" runat="server" />
                                     <asp:HiddenField ID="hdnAct" runat="server" />

                                     <asp:HiddenField ID="hdnOldSupervisorAssign" runat="server" />
                                </td>
                            </tr>
                            

                           
                            <tr>
                                <td colspan="3" style="padding-left: 121px;padding-top:30px;">
                                    <input id="btnSupervisorSave" class="btn btn-primary" onclick="CallSupervisor_save()" type="button" value="Save" />
                                    <input id="btnSupervisorCancel" class="btn btn-danger" onclick="CancelReassignSupervisor_save()" type="button" value="Cancel" />
                                </td>

                            </tr>
                        </table>


                    </div>

                </dxe:PopupControlContentControl>
            </ContentCollection>
            <HeaderStyle BackColor="LightGray" ForeColor="Black" />

          
        </dxe:ASPxPopupControl>


        <dxe:ASPxPopupControl ID="productpopup" ClientInstanceName="cproductpopup" runat="server"
AllowDragging="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" HeaderText="Supervisor's Assigned  History"
EnableHotTrack="False" BackColor="#DDECFE" Width="660px" CloseAction="CloseButton">
<ContentCollection>
<dxe:PopupControlContentControl runat="server">
<dxe:ASPxCallbackPanel ID="propanel" runat="server" Width="650px" ClientInstanceName="popproductPanel"
    OnCallback="propanel_Callback1" >
    <PanelCollection>
        <dxe:PanelContent runat="server">
                <div>
                    <dxe:ASPxGridView ID="grdproduct" runat="server" KeyFieldName="Id" AutoGenerateColumns="False"
                            Width="100%" ClientInstanceName="cpbproduct">
                            <Columns>
                                <dxe:GridViewDataTextColumn  FieldName="OldSupervisor" Caption="Old Supervisor" HeaderStyle-CssClass="text-center"
                                    VisibleIndex="0" FixedStyle="Left" Width="150px">
                                    <CellStyle CssClass="text-center" Wrap="true" >
                                    </CellStyle>
                                    <Settings AutoFilterCondition="Contains"  />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn  FieldName="NewSupervisor" Caption="ReassignTo Supervisor" HeaderStyle-CssClass="text-center"
                                    VisibleIndex="0" FixedStyle="Left" Width="200px">
                                    <CellStyle CssClass="text-center" Wrap="true" >
                                    </CellStyle>
                                    <Settings AutoFilterCondition="Contains"  />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="CreateDate" Caption="Assigned On" HeaderStyle-CssClass="text-center"
                                    VisibleIndex="0" FixedStyle="Left" Width="150px">
                                    <CellStyle CssClass="text-center" Wrap="true" >
                                    </CellStyle>
                                    <Settings AutoFilterCondition="Contains"  />
                                </dxe:GridViewDataTextColumn>
                             
                                
                                </Columns>
                        </dxe:ASPxGridView>
                </div>
        </dxe:PanelContent>
    </PanelCollection> 
</dxe:ASPxCallbackPanel>
</dxe:PopupControlContentControl>
</ContentCollection>
<HeaderStyle HorizontalAlign="Left">
<Paddings PaddingRight="6px" />
</HeaderStyle>
<SizeGripImage Height="16px" Width="16px" />
<CloseButtonImage Height="12px" Width="13px" />
<ClientSideEvents CloseButtonClick="function(s, e) {
cproductpopup.Hide();
}" />
</dxe:ASPxPopupControl>
    </div>
    </div>
</asp:Content>
