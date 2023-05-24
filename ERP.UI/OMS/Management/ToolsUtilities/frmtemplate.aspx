<%--================================================== Revision History =============================================
1.0   Pallab    V2.0.38      23-05-2023          0026202: Email Template module design modification & check in small device
====================================================== Revision History =============================================--%>

<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" EnableEventValidation="false"
    Inherits="ERP.OMS.Management.ToolsUtilities.management_utilities_frmtemplate" CodeBehind="frmtemplate.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        #TrReceip {
            display: inline-grid !important;
        }

        
    </style>

    <link href="../../../ckeditor/contents.css" rel="stylesheet" />
    <script src="../../../ckeditor/jquery-1.10.2.min.js"></script>
    <%--  <script src="~/Scripts/jquery.validate.min.js"></script>--%>
    <script src="../../../ckeditor/ckeditor.js"></script>

    <script language="javascript" type="text/javascript">
        var KeyWordIndex = 0;
        $(function () {
            // debugger;
            //var editor = CKEDITOR.instances['text_id'];
            //if (editor) { editor.destroy(true); }

            //CKEDITOR.config.toolbar_Basic = [['Bold', 'Italic', 'Underline',
            //'-', 'JustifyLeft', 'JustifyCenter', 'JustifyRight', '-', 'Undo', 'Redo']];
            //CKEDITOR.config.toolbar = 'Basic';
            //CKEDITOR.config.width = 400;
            //CKEDITOR.config.height = 300;






            /// CKEDITOR.replace('text_id', CKEDITOR.config);
            var Id1 = $("#drpSenderType").val();
            $.ajax({
                type: "POST",
                url: "frmtemplate.aspx/FetchEmailTagsByStage",
                data: JSON.stringify({ 'Id': Id1 }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",

                success: function (data) {

                    //  alert(data.d);
                    //var editor1 = CKEDITOR.instances['text_id'];
                    //if (editor1) { editor1.destroy(true); }
                    GenerateKeyWords(data.d);

                },
                failure: function (response) {
                    jAlert("Error");
                }
            });

            var KeyWordIndex = 0;
            $("#drpSenderType").change(function () {
                //  debugger;
                //   alert($("#drpSenderType").val());
                var Id1 = $("#drpSenderType").val();


                var editor = CKEDITOR.instances['text_id'];
                if (editor) { editor.destroy(true); }
                CKEDITOR.replace('text_id');

                //CKEDITOR.instances.TemplateFormat.setData('', function () {
                //    //this.checkDirty();  // true
                //});


                $.ajax({
                    type: "POST",
                    url: "frmtemplate.aspx/FetchEmailTagsByStage",
                    data: JSON.stringify({ 'Id': Id1 }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",

                    success: function (data) {

                        //  alert(data.d);

                        GenerateKeyWords(data.d);

                    },
                    failure: function (response) {
                        jAlert("Error");
                    }
                });


                //  CKEDITOR.replace('text_id', CKEDITOR.config);
            });





        });




        function GenerateKeyWords(EmailTags) {
            // debugger;
            // alert(EmailTags);

            var config = {};
            var KeyWordName = 'keywords' + (KeyWordIndex++);
            config.extraPlugins = KeyWordName;



            var KeyWordName = 'keywords' + (KeyWordIndex++);

            CKEDITOR.plugins.add(KeyWordName,
 {
     requires: ['richcombo'],
     init: function (editor) {
         var config = editor.config,
            lang = editor.lang.format;

         // Gets the list of tags from the settings.
         var tags = []; //new Array();
         //this.add('value', 'drop_text', 'drop_label');
         tags[0] = ["[contact_name]", "Name", "Name"];
         tags[1] = ["[contact_email]", "email", "email"];
         tags[2] = ["[contact_user_name]", "User name", "User name"];



         var strings = [];
         $.each(JSON.parse(EmailTags), function (index, value) {
             // alert(value.EmailTags);
             strings.push([value.EmailTags]);

         });
         // Create style objects for all defined styles.

         editor.ui.addRichCombo('keywords',
            {
                label: "Insert Field Code",
                title: "Insert Field Code",
                voiceLabel: "Insert Field Code",
                className: 'cke_format',
                multiSelect: false,

                panel:
                {
                    css: [editor.config.contentsCss, CKEDITOR.skin.getPath('editor')],
                    voiceLabel: lang.panelVoiceLabel
                },

                init: function () {
                    this.startGroup("Insert Field Code");
                    //this.add('value', 'drop_text', 'drop_label');

                    for (var i in strings) {
                        this.add(strings[i]);
                    }


                    //for (var this_tag in tags) {
                    //    this.add(tags[this_tag][0], tags[this_tag][1], tags[this_tag][2]);
                    //}
                },

                onClick: function (value) {
                    editor.focus();
                    editor.fire('saveSnapshot');
                    editor.insertHtml(value);
                    editor.fire('saveSnapshot');
                }
            });
     }
 });




            var editor1 = CKEDITOR.instances['text_id'];
            if (editor1) { editor1.destroy(true); }

            //    CKEDITOR.replace('text_id', CKEDITOR.config);
            CKEDITOR.replace('text_id', {

                extraPlugins: KeyWordName
            }
);
        }



        function OnGridFocusedRowChanged() {
            // Query the server for the Row ID "Rid" fields from the focused row 
            // The values will be returned to the OnGetRowValues() function     
            grid1.GetRowValues(grid1.GetFocusedRowIndex(), 'UID', OnGetRowValues1);
            //alert();
        }
        //            //Value array contains Row ID "Rid" field values returned from the server 
        function OnGetRowValues1(values) {
            RowID = values;
            //GridForR = document.getElementById("GridReminder");
        }
        function OnGridFocusedRowChanged1() {
            // Query the server for the Row ID "Rid" fields from the focused row 
            // The values will be returned to the OnGetRowValues() function     
            grid.GetRowValues(grid.GetFocusedRowIndex(), 'tem_id', OnGetRowValues);
        }
        //            //Value array contains Row ID "Rid" field values returned from the server 
        function OnGetRowValues(values) {
            RowID1 = values;
            //GridForR = document.getElementById("GridReminder");
        }
        function frmOpenNewWindow_custom(location, v_height, v_weight, top, left) {
            // window.open(location, "Search_Conformation_Box", "height=" + v_height + ",width=" + v_weight + ",top=" + top + ",left=" + left + ",location=no,directories=no,menubar=no,toolbar=no,status=no,scrollbars=yes,resizable=no,dependent=no'");

            // Change By Sudip on 16122016 - Pop up change to Dev ex Popup

            popup.SetContentUrl(location);
            popup.Show();
            popup.SetHeaderText("Reserved Word");
        }
        function frmOpenNewWindow() {
            $("#myDiv").show();
        }

        //function PostReservedWord(obj) {
        //    document.getElementById("txt_msg").value = document.getElementById("txt_msg").value + '< ' + obj + '>'
        //}

        //            function ReservedWord_Click()
        //            {
        //                frmOpenNewWindow_custom('frmreservedword.aspx?type=receipent&control=window.opener.document.aspnetForm.txt_msg.value','200','400','375','500')
        //            }
        FieldName = document.getElementById("btnAdd");
        function Page_Load() {
            document.getElementById("TrGrdTemplate").style.display = '';
            document.getElementById("TrInsert").style.display = 'none';
            document.getElementById("TrMessage").style.display = 'none';
            document.getElementById("TrButton").style.display = 'none';
        }
        function drpChange() {
            var cmb = document.getElementById("drp_templatetype");
            var val = cmb.value;
            if (val == '2') {
                document.getElementById("TrReceip").style.display = 'none';
                document.getElementById("TrAccess").style.display = 'none';
                document.getElementById("TrReserveWord").style.display = '';
            }
            else {
                document.getElementById("TrReceip").style.display = '';
                document.getElementById("TrAccess").style.display = '';
                document.getElementById("TrReserveWord").style.display = 'none';
            }
        }
    </script>

    <script type="text/ecmascript">

        function btn_adduser_Click() {
            var data = 'AddUser';
            var cmb = document.getElementById("txt_ajax");
            var cmb1 = document.getElementById("txt_ajax_hidden");
            //alert(cmb.Value);
            // data+='~'+cmb.value + '~' + cmb1.Value;
            data += '~' + cmb.value;
            data += '~' + cmb1.value;
            CallServer(data, "");
            grid1.PerformCallback();
            var cmb = document.getElementById("txt_ajax");
            cmb.value = '';
            document.getElementById("Trgrd").style.display = 'inline';
        }
        function btn_remove_Click() {
            CallServer('Remove' + '~' + RowID, "");
            grid1.PerformCallback();
        }
        function btnDelete_Click(keyValue) {
            doIt = confirm('Confirm delete?');
            if (doIt) {
                CallServer('Delete' + '~' + keyValue, "");
                grid.PerformCallback();
            }
        }
        function btnAdd_Click() {
            debugger;
            $("#myDiv").hide();
            document.getElementById("hndRecipients_hidden").value = '';
            document.getElementById("TrGrdTemplate").style.display = 'none';
            document.getElementById("btnAdd").style.display = 'none';
            document.getElementById("TrInsert").style.display = '';
            document.getElementById("TrMessage").style.display = '';
            document.getElementById("TrButton").style.display = '';
            document.getElementById("Trgrd").style.display = 'none';
            var cmb = document.getElementById("drp_templatetype");
            if (cmb.value == 1) {
                document.getElementById("TrReceip").style.display = '';
                document.getElementById("TrAccess").style.display = '';
                document.getElementById("TrReserveWord").style.display = 'none';
            }
            else {
                document.getElementById("TrReceip").style.display = 'none';
                document.getElementById("TrAccess").style.display = 'none';
                document.getElementById("TrReserveWord").style.display = '';
            }
            var cmb = document.getElementById("txt_description");
            cmb.value = '';

            //Commented by:Subhabrata
            //cmb = document.getElementById("txt_msg");
            //cmb.value = '';
            //end

            //Subhabrata:To set the ckeditor value blank on Add button click
            // CKEDITOR.instances["CKEditor1"].setData("");
            //End

            cmb = document.getElementById("txt_ajax");
            cmb.value = '';
            cmb = document.getElementById("drpSenderType");
            cmb.SelectedIndex = 1;
            cmb = document.getElementById("hdID");
            cmb.value = '';
            CallServer('Add', "");
            grid1.PerformCallback();

            ListBind();
            //Added by :Subhabrata
            //$('#drpSenderType').trigger("chosen:updated");
            //End
            BindRecipients();
            //setWithFromBind();
        }
        function btnDiscard_Click() {
            document.getElementById("TrGrdTemplate").style.display = 'none';
            document.getElementById("btnAdd").style.display = '';
            document.getElementById("TrInsert").style.display = '';
            document.getElementById("TrMessage").style.display = '';
            document.getElementById("TrButton").style.display = '';
            var cmb = document.getElementById("drp_templatetype");
            if (cmb.value == 1) {
                document.getElementById("TrReceip").style.display = '';
                document.getElementById("TrAccess").style.display = '';
                document.getElementById("TrReserveWord").style.display = 'none';
            }
            else {
                document.getElementById("TrReceip").style.display = 'none';
                document.getElementById("TrAccess").style.display = 'none';
                document.getElementById("TrReserveWord").style.display = '';
            }
            var cmb = document.getElementById("txt_description");
            cmb.value = '';
            cmb = document.getElementById("txt_msg");
            cmb.value = '';
            cmb = document.getElementById("txt_ajax");
            cmb.value = '';
            cmb = document.getElementById("drpSenderType");
            cmb.SelectedIndex = 1;
            CallServer('Add', "");
            document.getElementById("TrGrdTemplate").style.display = '';
            document.getElementById("TrInsert").style.display = 'none';
            document.getElementById("TrMessage").style.display = 'none';
            document.getElementById("TrButton").style.display = 'none';
        }
        function btnUpdate_Click(keyValue) {
            // debugger;

            $("#myDiv").hide();
            CallServer('Update~' + keyValue, "");

            grid1.PerformCallback();
            document.getElementById("TrGrdTemplate").style.display = 'none';
            document.getElementById("btnAdd").style.display = 'none';
            document.getElementById("TrInsert").style.display = '';
            document.getElementById("TrMessage").style.display = '';
            document.getElementById("TrButton").style.display = '';







        }

        //Added by:Subhabrata
        function OneMoreIsDefaultAccessClick(keyValue) {
            grid.PerformCallback('Access~' + keyValue);

        }
        //End

        function btnSave_Click() {

            // var value = CKEDITOR.instances['text_id'].getData();
            //  alert(value);

            //debugger;
            var data = 'Save';
            var cmb = document.getElementById("txt_description");
            data += '~' + cmb.value;
            //cmb = document.getElementById("txt_msg");
            //data += '~' + cmb.value;

            //subhabrata:To get the data from CkEditor
            cmb = CKEDITOR.instances['text_id'].getData();

            //End
            data += '~' + cmb;
            cmb = document.getElementById("drp_templatetype");
            data += '~' + cmb.value;
            cmb = document.getElementById("drp_accesslevel");
            data += '~' + cmb.value;
            cmb = document.getElementById("hdID");
            data += '~' + cmb.value;
            cmb = document.getElementById("drpSenderType");
            data += '~' + cmb.value;
            cmb = document.getElementById("hndRecipients_hidden");
            data += '~' + cmb.value;
            //Added By:Subhabrata on 21-01-2017
            var chk_default = document.getElementById("IsDefault_Chk");
            //End

            //Added By:Subhabrata on 21-01-2017
            if (chk_default.checked) {
                var sender_Type = document.getElementById("drpSenderType");

                jConfirm('Do you want to make this template as Default?', 'Confirmation Dialog', function (r) {
                    debugger;
                    //Added By:Subhabrata on 21-01-2017
                    if (r == true) {
                        $.ajax({
                            type: "POST",
                            url: "frmtemplate.aspx/AddEmailTemplateDefaultInfo",
                            data: JSON.stringify({ "SenderType": sender_Type.value }),
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            global: false,
                            async: false,
                            success: function (msg) {
                                if (msg.d) {
                                    debugger;
                                    data += '~' + '1';

                                    CallServer(data, "");
                                    grid.PerformCallback();
                                    Page_Load();
                                }
                            },
                            error: function (response) {
                                console.log(response);
                            }
                        });
                    }
                    else {
                        data += '~' + '0';
                        CallServer(data, "");
                        grid.PerformCallback();
                        Page_Load();
                    }
                });

            }//End
            else {
                data += '~' + '0';
                CallServer(data, "");
                grid.PerformCallback();
                Page_Load();
            }
            //End

            //Commented by:Subhabrata
            //CallServer(data, "");
            //grid.PerformCallback();
            //Page_Load();
            //End
        }


        function ReceiveServerData(rValue) {
            //    debugger;
            var DATA = rValue.split('~');
            if (DATA[0] == "Edit") {

                var Id1 = DATA[8];

                $.ajax({
                    type: "POST",
                    url: "frmtemplate.aspx/FetchEmailTagsByStage",
                    data: JSON.stringify({ 'Id': Id1 }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",

                    success: function (data) {

                        //  alert(data.d);
                        //var editor1 = CKEDITOR.instances['text_id'];
                        //if (editor1) { editor1.destroy(true); }
                        GenerateKeyWords(data.d);

                        var cmb = document.getElementById("txt_description");
                        cmb.value = DATA[1];
                        //Commented by:Subhabrata
                        //cmb = document.getElementById("txt_msg");
                        //cmb.value = DATA[2];
                        //End

                        //Subhabrata:To set the ckeditor value
                        CKEDITOR.instances["text_id"].setData(DATA[2]);
                        //End
                        cmb = document.getElementById("hndRecipients_hidden");
                        cmb.value = DATA[3];
                        cmb = document.getElementById("drp_templatetype");
                        cmb.value = DATA[4];
                        cmb = document.getElementById("drp_accesslevel");
                        if (DATA[5] == 1) {
                            cmb.SelectedIndex = 1;
                        }
                        else {
                            cmb.SelectedIndex = 2;
                        }
                        cmb = document.getElementById("hdID");
                        cmb.value = DATA[6];
                        var rcount = DATA[7];
                        if (DATA[4] == 1) {
                            document.getElementById("TrReceip").style.display = '';
                            document.getElementById("TrAccess").style.display = '';
                            document.getElementById("TrReserveWord").style.display = 'none';
                        }
                        if (DATA[4] == 2) {
                            document.getElementById("TrReceip").style.display = 'none';
                            document.getElementById("TrAccess").style.display = 'none';
                            document.getElementById("TrReserveWord").style.display = '';
                        }
                        if (rcount != '0') {
                            document.getElementById("Trgrd").style.display = 'inline';
                        }
                        else {
                            document.getElementById("Trgrd").style.display = 'none';
                        }
                        //Done by:Subhabrata
                        cmb = document.getElementById("drpSenderType");
                        //if (DATA[8] == 1) {
                        //    cmb.value = 1;
                        //}
                        //else if (DATA[8] == 2) {
                        //    cmb.value = 2;
                        //}
                        //else if (DATA[8] == 3) {
                        //    cmb.value = 3;
                        //}
                        //else if (DATA[8] == 4) {
                        //    cmb.value = 4;
                        //}
                        //else if (DATA[8] == 5) {
                        //    cmb.value = 5;
                        //}
                        //else if (DATA[8] == 6) {
                        //    cmb.value = 6;
                        //}
                        //else if (DATA[8] == 7) {
                        //    cmb.value = 7;
                        //}
                        //else if (DATA[8] == 8) {
                        //    cmb.value = 8;
                        //}
                        //else if (DATA[8] == 9) {
                        //    cmb.value = 9;
                        //}
                        //else if (DATA[8] == 10) {
                        //    cmb.value = 10;
                        //}
                        //else if (DATA[8] == 11) {
                        //    cmb.value = 11;
                        //}
                        //else if (DATA[8] == 13) {
                        cmb.value = DATA[8];
                        //}
                        //End

                        //Added By:Subhabrata
                        var Chk_Box = document.getElementById("IsDefault_Chk");
                        if (DATA[9] == "True") {
                            Chk_Box.checked = true;
                        }
                        else {
                            Chk_Box.checked = false;
                        }


                        //End

                        //Done By:Subhabrata
              <%--  if (DATA[8] == 1) {
                    $("<%=drpSenderType.ClientID%>").val(DATA[8]);
                }
                else if (DATA[8] == 2) {
                    $("<%=drpSenderType.ClientID%>").val(DATA[8]);
                }
                else {
                    cmb.SelectedIndex = 3;
                }--%>
                        //End
                        BindRecipients();
                        //setWithFromBind();

                    },
                    failure: function (response) {
                        jAlert("Error");
                    }
                });







                // var Id1 = $("#drpSenderType").val();


                //var editor = CKEDITOR.instances['text_id'];
                //if (editor) { editor.destroy(true); }
                //CKEDITOR.replace('text_id');







            }
            //                if(DATA[0]=="Delete")
            //                {          
            //                    if(DATA[1]="Y")
            //                    alert('Deleted Successfully!');
            //                }   
        }
    </script>

    <script type="text/javascript" src="/assests/js/init.js"></script>
    <script type="text/javascript" src="/assests/js/ajax-dynamic-list.js"></script>
    <script src="/assests/pluggins/choosen/choosen.min.js"></script>
    <%--Choosen List by Sudip 19-12-2016--%>

    <script type="text/javascript">
        //$(function () {
        //    ListBind();
        //    BindRecipients();
        //});





        function BindRecipients() {
            //  debugger;
            var lBox = $('select[id$=lstItems]');
            var listItems = [];

            lBox.empty();
            var firstParam = '';

            $.ajax({
                type: "POST",
                url: 'frmtemplate.aspx/GetRecipients',
                data: "{Type:\"" + firstParam + "\"}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var list = msg.d;

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

                        $(lBox).append(listItems.join(''));


                        setWithFromBind();
                        //ListBind();
                        ListAssignTo();
                        $('#lstItems').trigger("chosen:updated");

                        $('#lstItems').prop('disabled', false).trigger("chosen:updated");
                    }
                    else {
                        lBox.empty();
                        $('#lstItems').trigger("chosen:updated");
                        $('#lstItems').prop('disabled', true).trigger("chosen:updated");

                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });
        }

        function ListBind() {
            var config = {
                '.chsn': {},
                '.chsn-deselect': { allow_single_deselect: true },
                '.chsn-no-single': { disable_search_threshold: 10 },
                '.chsn-no-results': { no_results_text: 'Oops, nothing found!' },
                '.chsn-width': { width: "100%" }
            }
            for (var selector in config) {
                $(selector).chosen(config[selector]);
            }
        }

        function ListAssignTo() {
            $('#lstItems').chosen();
            $('#lstItems').fadeIn();
        }

        function changeFunc() {
            var ListValue = '';

            var techGroups = document.getElementById("lstItems");
            for (var i = 0; i < techGroups.options.length; i++) {
                if (techGroups.options[i].selected == true) {
                    if (ListValue == '') {
                        ListValue = techGroups.options[i].value;
                    }
                    else {
                        ListValue = ListValue + ',' + techGroups.options[i].value;
                    }
                }
            }

            document.getElementById("hndRecipients_hidden").value = ListValue;
        }

        function setWithFromBind() {
            var techGroups = document.getElementById("lstItems");
            var listValue = document.getElementById("hndRecipients_hidden").value;
            var str_array = listValue.split(',');

            for (var i = 0; i < techGroups.options.length; i++) {
                var selectedValue = techGroups.options[i].value;

                if (str_array.indexOf(selectedValue) > -1) {
                    techGroups.options[i].selected = true;
                }
                else {
                    techGroups.options[i].selected = false;
                }
            }
        }
    </script>
    <style type="text/css">
        .chosen-container.chosen-container-multi {
            width: 100% !important;
        }

        .chosen-choices {
            width: 100% !important;
        }
    </style>
    <style>
        #myDiv input, #Div input {
            margin-top: 20px;
            background: #0176c5;
            color: #fff !important;
            border: 1px solid #094b77 !important;
            margin-right: 2px;
        }

            #myDiv input:hover, #myDiv input:focus,
            #Div input:hover, #Div input:focus {
                background: #0a619c;
                box-shadow: none;
            }

        .pullleftClass {
            position: absolute;
            right: -5px;
            top: 28px;
        }
    </style>

    <%--Rev 1.0--%>
    <link href="/assests/css/custom/newcustomstyle.css" rel="stylesheet" />
    
    <style>
        select
        {
            z-index: 1;
        }

        #gridAdvanceAdj {
            max-width: 99% !important;
        }
        #FormDate, #toDate, #dtTDate, #dt_PLQuote, #dt_PlQuoteExpiry {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        select
        {
            -webkit-appearance: none;
        }

        .calendar-icon
        {
            right: 20px;
        }

        .panel-title h3
        {
            padding-top: 0px !important;
        }

        .fakeInput
        {
                min-height: 30px;
    border-radius: 4px;
        }
        
    </style>
    <%--Rev end 1.0--%>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main clearfix">
        <div class="panel-heading">
        <div class="panel-title">
            <h3>Email Template</h3>

        </div>
    </div>
        <div class="form_main" style="border: 1px solid #ccc; padding: 10px 15px;">
        <div class="row mb-10">
            <div class="col-md-12">
                <% if (rights.CanAdd)
                   { %><input id="btnAdd" type="button" value="Add" class="btn btn-primary" onclick="btnAdd_Click()" /><%} %>

                <%-- Add, Edit & Delete Design Change By Sudip on 16122016--%>

                <%--  <input id="btnUpdate" type="button" value="Modify" class="btn btn-info" onclick="btnUpdate_Click()" />
                <input id="btnDelete" type="button" value="Delete" onclick="btnDelete_Click()" class="btn btn-danger" />--%>
            </div>
        </div>

        <table class="TableMain100">
            <%--            <tr>
                <td class="EHEADER" style="text-align: center;">
                    <strong><span style="color: #000099;">Message Template</span></strong></td>
            </tr>--%>
        </table>
        <table class="TableMain100" style="min-width: 183px;">

            <tr>
                <td width="90%" style="text-align: left">
                    <table width="100%">
                        <tr id="TrGrdTemplate">
                            <td colspan="2" valign="top">
                                <dxe:ASPxGridView ID="GrdTemplate" ClientInstanceName="grid" runat="server" KeyFieldName="tem_id"
                                    Width="100%" OnCustomCallback="GrdTemplate_CustomCallback" AutoGenerateColumns="False">
                                    <Styles>
                                        <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                        </Header>
                                        <LoadingPanel ImageSpacing="10px">
                                        </LoadingPanel>
                                    </Styles>
                                    <Columns>
                                        <dxe:GridViewDataTextColumn FieldName="tem_id" Visible="False" VisibleIndex="0">
                                            <CellStyle CssClass="gridcellleft">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn FieldName="Title" VisibleIndex="0" Caption="Title">
                                            <CellStyle CssClass="gridcellleft">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn FieldName="IsDefault" VisibleIndex="0" Caption="IsDefault">
                                            <CellStyle CssClass="gridcellleft" Wrap="True">
                                            </CellStyle>
                                            <EditFormSettings Visible="False"></EditFormSettings>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn VisibleIndex="1" Width="100px">
                                            <CellStyle CssClass="gridcellleft">
                                            </CellStyle>
                                            <DataItemTemplate>
                                                <% if (rights.CanEdit)
                                                   { %><a href="javascript:void(0);" onclick="btnUpdate_Click('<%# Container.KeyValue %>')">
                                                       <img src="../../../assests/images/Edit.png" /></a><%} %>&nbsp;|&nbsp;
                                                <%--<% if (rights.CanEdit)
                                                   { %><a href="javascript:void(0);" onclick="OnMoreAccessCick('<%# Container.KeyValue %>')" class="pad"><img src="../../../assests/images/activity.png" /></a><%} %>--%>
                                                <% if (rights.CanDelete)
                                                   { %><a href="javascript:void(0);" onclick="btnDelete_Click('<%# Container.KeyValue %>')"><img src="../../../assests/images/Delete.png" /></a><%} %>
                                            </DataItemTemplate>
                                            <HeaderTemplate>
                                                <%-- <a href="javascript:void(0);" onclick="javascript:OnAddButtonClick();"><span style="color: #000099; text-decoration: underline">Add New</span> </a>--%>
                                            </HeaderTemplate>
                                            <EditFormSettings Visible="False"></EditFormSettings>
                                        </dxe:GridViewDataTextColumn>
                                    </Columns>
                                    <SettingsSearchPanel Visible="True" />
                                    <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowFilterRow="true" ShowFilterRowMenu="True" />
                                    <SettingsPager NumericButtonCount="20" PageSize="20" ShowSeparators="True" AlwaysShowPager="True">
                                        <FirstPageButton Visible="True">
                                        </FirstPageButton>
                                        <LastPageButton Visible="True">
                                        </LastPageButton>
                                    </SettingsPager>
                                    <StylesEditors>
                                        <ProgressBar Height="25px">
                                        </ProgressBar>
                                    </StylesEditors>
                                    <%-- <SettingsBehavior AllowFocusedRow="false" AllowSort="False" />--%>
                                    <SettingsSearchPanel Visible="True" />
                                    <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />
                                    <%--  <ClientSideEvents FocusedRowChanged="function(s, e) { OnGridFocusedRowChanged1(); }" />--%>
                                    <%--<SettingsPager>
                                        <FirstPageButton Visible="True">
                                        </FirstPageButton>
                                        <LastPageButton Visible="True">
                                        </LastPageButton>
                                    </SettingsPager>--%>
                                </dxe:ASPxGridView>
                            </td>
                        </tr>
                        <tr id="TrInsert">
                            <td style="vertical-align: top; width: 47px;">
                                <div class="crossBtn"><a href="javascript:window.location.reload(true)"><i class="fa fa-times"></i></a></div>
                                <div>
                                    <div class="col-md-3" style="display: none;">
                                        <label>
                                            <span class="Ecoheadtxt">Template For :</span>
                                        </label>
                                        <div>
                                            <asp:DropDownList ID="drp_templatetype" runat="server" Width="100%" Enabled="False">
                                                <asp:ListItem Value="1">Message</asp:ListItem>
                                                <asp:ListItem Value="2">Email</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <label>
                                            <%-- <span class="Ecoheadtxt">Short Description :</span>--%>
                                            <span class="Ecoheadtxt">Subject :</span>
                                        </label>
                                        <div>
                                            <asp:TextBox ID="txt_description" runat="server" MaxLength="3000" Width="100%"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-3" style="display: none">
                                        <label>
                                            <span class="Ecoheadtxt">Recipients :</span>
                                        </label>
                                        <div>
                                            <asp:TextBox ID="txt_ajax" runat="server"></asp:TextBox>
                                            <asp:TextBox ID="txt_ajax_hidden" runat="server"></asp:TextBox>
                                            <input id="btn_adduser" type="button" value="Add" class="btn btn-success" style="margin-bottom: 5px;"
                                                onclick="btn_adduser_Click()" />
                                        </div>
                                    </div>
                                    <div class="col-md-3" id="TrReceip" style="display: grid">
                                        <label>
                                            <span class="Ecoheadtxt">Recipients :</span>
                                        </label>
                                        <div>
                                            <asp:ListBox ID="lstItems" runat="server" SelectionMode="Multiple" Font-Size="12px" Height="90px" Width="253px" CssClass="mb0 chsn hide" data-placeholder="Select ..." onchange="changeFunc();"></asp:ListBox>
                                            <asp:HiddenField ID="hndRecipients_hidden" runat="server" />
                                        </div>
                                    </div>

                                    <div class="col-md-3" id="TrAccess">
                                        <label>
                                            <span class="Ecoheadtxt">Access Level :</span>
                                        </label>
                                        <div>
                                            <asp:DropDownList ID="drp_accesslevel" runat="server" Width="100%">
                                                <asp:ListItem Value="1">Public</asp:ListItem>
                                                <asp:ListItem Value="0">Private</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <%--Rev 1.0: "simple-select" class add --%>
                                    <div class="col-md-3 simple-select" id="TrReserveWord">
                                        <label>Sender Type :</label>
                                        <div class="relative">
                                            <%--<asp:ListBox ID="drpSenderType" CssClass="chsn" runat="server" Width="100%"   
                                        data-placeholder="Select..." ></asp:ListBox>--%>
                                            <%--Commented by:Subhabrata as it will be fetched from Database--%>
                                            <asp:DropDownList ID="drpSenderType" runat="server" Width="100%">
                                            </asp:DropDownList>
                                            <%--End--%>

                                            <%--<asp:DropDownList ID="drpSenderType" runat="server" Width="100%" Font-Size="11px" TabIndex="2">
                                            </asp:DropDownList>--%>

                                            <%--Commented by:Subhabrata--%>
                                            <%--<span style="font-weight: bold; color: Blue; position: absolute; right: -82px; top: 4px;">(Sender Type)</span>--%>
                                            <%--End--%>

                                            <%--<input id="Button1" type="button" value="Reserved Word" class="btnUpdate" style="height: 20px" visible="false" onclick="ReservedWord_Click()" />--%>
                                        </div>
                                    </div>
                                    <div class="col-md-3" id="TrReserveWord">
                                        <label>IsDefault :</label>
                                        <div class="relative">
                                            <asp:CheckBox ID="IsDefault_Chk" runat="server" />
                                            <%--<span style="font-weight: bold; color: Blue; position: absolute; right: -82px; top: 4px;">(IsDefault)</span>--%>
                                        </div>
                                    </div>

                                </div>


                                <asp:HiddenField ID="HiddenField1" runat="server" />
                                <div style="text-align: left" id="Trgrd">
                                    <table style="width: 336px; margin-left: 162px; display: none">
                                        <tr>
                                            <td>
                                                <dxe:ASPxGridView ID="grduser" runat="server" AutoGenerateColumns="true" ClientInstanceName="grid1"
                                                    KeyFieldName="UID" Width="86%" OnCustomCallback="grduser_CustomCallback">
                                                    <Styles>
                                                        <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                        </Header>
                                                        <LoadingPanel ImageSpacing="10px">
                                                        </LoadingPanel>
                                                    </Styles>
                                                    <Columns>
                                                        <dxe:GridViewDataTextColumn FieldName="UID" Visible="false" VisibleIndex="0">
                                                            <CellStyle CssClass="gridcellleft">
                                                            </CellStyle>
                                                        </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewDataTextColumn FieldName="UNAME" Caption="Recipient Name" Visible="true">
                                                            <CellStyle CssClass="gridcellleft">
                                                            </CellStyle>
                                                        </dxe:GridViewDataTextColumn>
                                                    </Columns>
                                                    <StylesEditors>
                                                        <ProgressBar Height="25px">
                                                        </ProgressBar>
                                                    </StylesEditors>
                                                    <SettingsBehavior AllowFocusedRow="True" AllowSort="False" />
                                                    <ClientSideEvents FocusedRowChanged="function(s, e) { OnGridFocusedRowChanged(); }" />
                                                </dxe:ASPxGridView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <input id="btn_remove" type="button" value="Remove" class="btn btn-danger" onclick="btn_remove_Click()" />&nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </div>

                                <div class="clear"></div>
                                <div class="col-md-9" id="TrMessage">
                                    <%--<label class="Ecoheadtxt">Message :</label>--%>
                                    <label class="Ecoheadtxt">Body :</label>
                                    <div>
                                        <%--<asp:TextBox ID="txt_msg" runat="server" Height="160px" TextMode="MultiLine" Width="100%" MaxLength="3000"></asp:TextBox>--%>
                                        <%--  <CKEditor:CKEditorControl ID="CKEditor1" BasePath="/ckeditor/" runat="server" ClientIDMode="Static">
                                        </CKEditor:CKEditorControl>--%>

                                        <asp:TextBox ID="text_id" runat="server"></asp:TextBox>
                                    </div>
                                    <div style="text-align: right; display: none;">
                                        <%--  <a href="#" onclick="frmOpenNewWindow_custom('frmreservedword.aspx?type=receipent,sender&control=window.opener.document.aspnetForm.txt_msg.value','200','400','200','300')">
                                            <span style="color: #000099; text-decoration: underline">Use Reserved Word</span></a>--%>
                                        <a href="#" onclick="frmOpenNewWindow()">
                                            <span style="color: #000099; text-decoration: underline">Use Reserved Word</span></a>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div id="myDiv" runat="server">
                                    </div>
                                </div>
                                <div class="clear"></div>
                                <br />
                                <div class="col-md-12" id="TrButton">

                                    <input id="btnSave" type="button" value="Save" class="btn btn-primary" onclick="btnSave_Click()"
                                        style="width: 64px" />
                                    <input id="btnDiscard" type="button" value="Discard" class="btn btn-danger" onclick="btnDiscard_Click()" />
                                    <input id="hdID" type="hidden" style="width: 151px; height: 7px" />

                                </div>
                            </td>
                        </tr>


                    </table>
                </td>
            </tr>
        </table>
    </div>
    </div>
    <dxe:ASPxPopupControl ID="ASPXPopupControl_Bulk" runat="server" ContentUrl="frmtemplate.aspx"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popup"
        Width="700px" Height="200px" HeaderText="Add Document" AllowResize="true" ResizingMode="Postponed">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="Blue" Font-Bold="True" ForeColor="White" />
    </dxe:ASPxPopupControl>
</asp:Content>
