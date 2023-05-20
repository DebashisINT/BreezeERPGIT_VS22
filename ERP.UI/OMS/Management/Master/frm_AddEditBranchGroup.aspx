<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                19-05-2023        2.0.38           Pallab              26176: Branch Groups module design modification & check in small device
====================================================== Revision History =============================================--%>

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

    <style>
        /*Rev 1.0*/
        .outer-div-main {
            background: #ffffff;
            padding: 10px;
            border-radius: 10px;
            box-shadow: 1px 1px 10px #11111154;
        }

        /*.form_main {
            overflow: hidden;
        }*/

        label , .mylabel1, .clsTo, .dxeBase_PlasticBlue
        {
            color: #141414 !important;
            font-size: 14px !important;
                font-weight: 500 !important;
                margin-bottom: 0 !important;
                    line-height: 20px;
        }

        #GrpSelLbl .dxeBase_PlasticBlue
        {
                line-height: 20px !important;
        }

        select
        {
            height: 30px !important;
            border-radius: 4px;
            -webkit-appearance: none;
            position: relative;
            z-index: 1;
            background-color: transparent;
            padding-left: 10px !important;
            padding-right: 22px !important;
        }

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

        .calendar-icon {
            position: absolute;
            bottom: 6px;
            right: 20px;
            z-index: 0;
            cursor: pointer;
        }

        #ASPxFromDate , #ASPxToDate , #ASPxASondate , #ASPxAsOnDate , #txtDOB , #txtAnniversary , #txtcstVdate , #txtLocalVdate ,
        #txtCINVdate , #txtincorporateDate , #txtErpValidFrom , #txtErpValidUpto , #txtESICValidFrom , #txtESICValidUpto
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        .dxeDisabled_PlasticBlue
        {
            z-index: 0 !important;
        }

        #ASPxFromDate_B-1 , #ASPxToDate_B-1 , #ASPxASondate_B-1 , #ASPxAsOnDate_B-1 , #txtDOB_B-1 , #txtAnniversary_B-1 , #txtcstVdate_B-1 ,
        #txtLocalVdate_B-1 , #txtCINVdate_B-1 , #txtincorporateDate_B-1 , #txtErpValidFrom_B-1 , #txtErpValidUpto_B-1 , #txtESICValidFrom_B-1 ,
        #txtESICValidUpto_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #ASPxFromDate_B-1 #ASPxFromDate_B-1Img , #ASPxToDate_B-1 #ASPxToDate_B-1Img , #ASPxASondate_B-1 #ASPxASondate_B-1Img , #ASPxAsOnDate_B-1 #ASPxAsOnDate_B-1Img ,
        #txtDOB_B-1 #txtDOB_B-1Img ,
        #txtAnniversary_B-1 #txtAnniversary_B-1Img ,
        #txtcstVdate_B-1 #txtcstVdate_B-1Img ,
        #txtLocalVdate_B-1 #txtLocalVdate_B-1Img , #txtCINVdate_B-1 #txtCINVdate_B-1Img , #txtincorporateDate_B-1 #txtincorporateDate_B-1Img ,
        #txtErpValidFrom_B-1 #txtErpValidFrom_B-1Img , #txtErpValidUpto_B-1 #txtErpValidUpto_B-1Img , #txtESICValidFrom_B-1 #txtESICValidFrom_B-1Img ,
        #txtESICValidUpto_B-1 #txtESICValidUpto_B-1Img
        {
            display: none;
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
            line-height: 18px;
            z-index: 0;
        }
        .simple-select {
            position: relative;
        }
        .simple-select:disabled::after
        {
            background: #1111113b;
        }
        /*select.btn
        {
            padding-right: 10px !important;
        }*/

        .panel-group .panel
        {
            box-shadow: 1px 1px 8px #1111113b;
            border-radius: 8px;
        }

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

        .styled-checkbox {
        position: absolute;
        opacity: 0;
        z-index: 1;
    }

        .styled-checkbox + label {
            position: relative;
            /*cursor: pointer;*/
            padding: 0;
            margin-bottom: 0 !important;
        }

            .styled-checkbox + label:before {
                content: "";
                margin-right: 6px;
                display: inline-block;
                vertical-align: text-top;
                width: 16px;
                height: 16px;
                /*background: #d7d7d7;*/
                margin-top: 2px;
                border-radius: 2px;
                border: 1px solid #c5c5c5;
            }

        .styled-checkbox:hover + label:before {
            background: #094e8c;
        }


        .styled-checkbox:checked + label:before {
            background: #094e8c;
        }

        .styled-checkbox:disabled + label {
            color: #b8b8b8;
            cursor: auto;
        }

            .styled-checkbox:disabled + label:before {
                box-shadow: none;
                background: #ddd;
            }

        .styled-checkbox:checked + label:after {
            content: "";
            position: absolute;
            left: 3px;
            top: 9px;
            background: white;
            width: 2px;
            height: 2px;
            box-shadow: 2px 0 0 white, 4px 0 0 white, 4px -2px 0 white, 4px -4px 0 white, 4px -6px 0 white, 4px -8px 0 white;
            transform: rotate(45deg);
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

        .TableMain100 #ShowGrid , .TableMain100 #ShowGridList , .TableMain100 #ShowGridRet , .TableMain100 #EmployeeGrid , .TableMain100 #RootGrid
        {
            max-width: 98% !important;
        }

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
                margin-top: 3px;
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

        .for-cust-icon {
            position: relative;
            z-index: 1;
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

        .mt-24
        {
            margin-top: 24px;
        }

        .col-md-3
        {
            margin-top: 8px;
        }

        #upldBigLogo , #upldSmallLogo
        {
            width: 100%;
        }

        .chosen-container-single .chosen-single span
        {
            min-height: 30px;
            line-height: 30px;
        }

        .chosen-container-single .chosen-single div {
        background: #094e8c;
        color: #fff;
        border-radius: 4px;
        height: 26px;
        top: 1px;
        right: 1px;
        /*position:relative;*/
    }

        .chosen-container-single .chosen-single div b {
            display: none;
        }

        .chosen-container-single .chosen-single div::after {
            /*content: '<';*/
            content: url(../../../assests/images/left-arw.png);
            position: absolute;
            top: 2px;
            right: 5px;
            font-size: 17px;
            transform: rotate(269deg);
            font-weight: 500;
        }

    .chosen-container-active.chosen-with-drop .chosen-single div {
        background: #094e8c;
        color: #fff;
    }

        .chosen-container-active.chosen-with-drop .chosen-single div::after {
            transform: rotate(90deg);
            right: 5px;
        }

        .panel-title
        {
            padding-bottom: 20px;
        }

        /*.dxeDisabled_PlasticBlue, .aspNetDisabled {
            opacity: 0.4 !important;
            color: #ffffff !important;
        }*/
                /*.padTopbutton {
            padding-top: 27px;
        }*/
        /*#lookup_project
        {
            max-width: 100% !important;
        }*/
        /*Rev end 1.0*/
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main">
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
    </div>
</asp:Content>

