<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/Masterpage/ERP.Master" AutoEventWireup="true"
    CodeBehind="Document-Template.aspx.cs" Inherits="Import.Import.Document_Template" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="../../../ckeditor/contents.css" rel="stylesheet" />
    <script src="../../../ckeditor/ckeditor.js"></script>

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

    <script type="text/javascript">

        var KeyWordIndex = 0;
        $(function () {


            //document.getElementById("TrButton").style.display = 'none';

            $("#TrButton").hide();
            $("#TrInsert").hide();

            $("#hdID").val('');
            var Id1 = "";

            $.ajax({
                type: "POST",
                url: "Document-Template.aspx/FetchEmailTagsByStage",
                data: JSON.stringify({ Id: Id1 }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    GenerateKeyWords(data.d);

                }
            });




        });


        function GenerateKeyWords(ImportTag) {
            // debugger;
            // alert(ImportTag);

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
         $.each(JSON.parse(ImportTag), function (index, value) {
             //alert(value.ImportTag);
             strings.push([value.ImportTag]);

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
            grid1.GetRowValues(grid1.GetFocusedRowIndex(), 'UID', OnGetRowValues1);
        }

        //    //Value array contains Row ID "Rid" field values returned from the server 


        function OnGetRowValues1(values) {
            RowID = values;
        }


        function OnGridFocusedRowChanged1() {
            grid.GetRowValues(grid.GetFocusedRowIndex(), 'tem_id', OnGetRowValues);
        }



        function OnGetRowValues(values) {
            RowID1 = values;
        }


        function frmOpenNewWindow_custom(location, v_height, v_weight, top, left) {
            popup.SetContentUrl(location);
            popup.Show();
            popup.SetHeaderText("Reserved Word");

        }


        function frmOpenNewWindow() {
            $("#myDiv").show();
        }


        FieldName = document.getElementById("btnAdd");
        function Page_Load() {
            //document.getElementById("TrGrdTemplate").style.display = '';
            //document.getElementById("TrInsert").style.display = 'none';
            //document.getElementById("TrMessage").style.display = 'none';
            //document.getElementById("TrButton").style.display = 'none';
        }


        function drpChange() {

            var cmb = document.getElementById("drp_templatetype");

        }

    </script>

    <script type="text/ecmascript">

        $(document).ready(function () {
            document.onkeydown = function (e) {

                if (event.altKey == true) {
                    switch (event.keyCode) {

                        case 115:

                            StopDefaultAction(e);
                            btnSave_Click(0);

                            break;

                        case 83:

                            StopDefaultAction(e);
                            btnSave_Click(0);

                            break;

                        case 88:

                            StopDefaultAction(e);
                            btnSave_Click(1);

                            break;

                        case 120:

                            StopDefaultAction(e);
                            btnSave_Click(1);

                            break;


                        case 97:
                            StopDefaultAction(e);

                            btnAdd_Click();


                            break;
                        case 65:
                            StopDefaultAction(e);

                            btnAdd_Click();

                            break;



                        case 85:
                            StopDefaultAction(e);
                            btnUpdate_Click(1);

                            break;
                        case 117:

                            StopDefaultAction(e);
                            btnUpdate_Click(1);

                            break;

                            //case 87:
                            //    StopDefaultAction(e);
                            //    grid.PerformCallback();

                            //    break;
                            //case 119:

                            //    StopDefaultAction(e);
                            //    grid.PerformCallback();

                            //    break;

                    }

                }
            }
        });

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

        function StopDefaultAction(e) {
            if (e.preventDefault) { e.preventDefault() }
            else { e.stop() };

            e.returnValue = false;
            e.stopPropagation();
        }

        function btn_remove_Click() {
            ///  CallServer('Remove' + '~' + RowID, "");
            grid1.PerformCallback();
        }


        //function btnDelete_Click(keyValue) {
        //    doIt = confirm('Confirm delete?');
        //    if (doIt) {
        //        //   CallServer('Delete' + '~' + keyValue, "");
        //        grid.PerformCallback();
        //    }
        //}


        function btnAdd_Click() {

            $("#myDiv").hide();

            document.getElementById("hndRecipients_hidden").value = '';
            document.getElementById("TrGrdTemplate").style.display = 'none';
            document.getElementById("btnAdd").style.display = 'none';

            if ($("#drdExport").length) {

                document.getElementById("drdExport").style.display = 'none';
            }

            document.getElementById("btn_show").style.display = 'none';
            document.getElementById("TrInsert").style.display = '';
            document.getElementById("TrMessage").style.display = '';
            document.getElementById("TrButton").style.display = '';
            //document.getElementById("Trgrd").style.display = 'none';
            var cmb = document.getElementById("drp_templatetype");


            var cmb = document.getElementById("txt_description");
            cmb.value = '';


            //cmb = document.getElementById("txt_ajax");
            //cmb.value = '';

            cmb = document.getElementById("drpSenderType");
            cmb.SelectedIndex = 1;
            cmb = document.getElementById("hdID");
            cmb.value = '';


            // CallServer('Add', "");
            // grid1.PerformCallback();
            // ListBind();
            // BindRecipients();

        }


        function btnDiscard_Click() {


            document.getElementById("TrGrdTemplate").style.display = 'none';
            document.getElementById("btnAdd").style.display = '';
            if ($("#drdExport").length) {
                document.getElementById("drdExport").style.display = '';
            }
            document.getElementById("TrInsert").style.display = '';
            document.getElementById("TrMessage").style.display = '';
            document.getElementById("TrButton").style.display = '';
            var cmb = document.getElementById("drp_templatetype");


            var cmb = document.getElementById("txt_description");
            cmb.value = '';
            cmb = document.getElementById("txt_msg");
            cmb.value = '';
            //cmb = document.getElementById("txt_ajax");
            //cmb.value = '';
            cmb = document.getElementById("drpSenderType");
            cmb.SelectedIndex = 1;
            /// CallServer('Add', "");
            document.getElementById("TrGrdTemplate").style.display = '';
            document.getElementById("TrInsert").style.display = 'none';
            document.getElementById("TrMessage").style.display = 'none';
            document.getElementById("TrButton").style.display = 'none';


        }

        function btnDelete_Click(keyValue) {

            jConfirm('Do you want to delete?', 'Confirmation Dialog', function (r) {


                if (r == true) {
                    $.ajax({
                        type: "POST",
                        url: "Document-Template.aspx/DeleteTemplate",
                        data: JSON.stringify({ Id: keyValue }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {

                            var s = data.d;
                            if (s.success == true) {
                                jAlert('Deleted Successfully');

                            }
                            ///val(s.IsDefault);
                            grid.PerformCallback();
                        }
                    });

                }


            });
        }


        function btnUpdate_Click(keyValue) {

            $("#myDiv").hide();


            document.getElementById("TrGrdTemplate").style.display = 'none';
            if ($("#btnAdd").length) {
                document.getElementById("btnAdd").style.display = 'none';

            }
            if ($("#drdExport").length) {
                document.getElementById("drdExport").style.display = 'none';
            }
            document.getElementById("btn_show").style.display = 'none';

            document.getElementById("TrInsert").style.display = '';
            document.getElementById("TrMessage").style.display = '';
            document.getElementById("TrButton").style.display = '';

            $("#hdID").val(keyValue);


            $.ajax({
                type: "POST",
                url: "Document-Template.aspx/ModifyListByID",
                data: JSON.stringify({ Id: keyValue }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    var s = data.d;
                    document.getElementById("txt_description").value = s.Template;
                    CKEDITOR.instances['text_id'].setData(s.Body);
                    document.getElementById("txtremarks").value = s.Remarks;
                    document.getElementById("drp_templatetype").value = s.DocType;
                    $("#IsDefault_Chk").prop('checked', s.IsDefault);
                    ///val(s.IsDefault);

                }
            });

        }

        function OnViewClick(keyValue) {

            $("#myDiv").hide();

            $("#lblHeading").val("View Document Template");
            document.getElementById("TrGrdTemplate").style.display = 'none';
            if ($("#btnAdd").length) {
                document.getElementById("btnAdd").style.display = 'none';

            }
            if ($("#drdExport").length) {
                document.getElementById("drdExport").style.display = 'none';
            }
            document.getElementById("btn_show").style.display = 'none';

            document.getElementById("TrInsert").style.display = '';
            document.getElementById("TrMessage").style.display = '';
            document.getElementById("TrButton").style.display = '';

            $("#hdID").val(keyValue);


            $.ajax({
                type: "POST",
                url: "Document-Template.aspx/ModifyListByID",
                data: JSON.stringify({ Id: keyValue }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    var s = data.d;
                    document.getElementById("txt_description").value = s.Template;
                    CKEDITOR.instances['text_id'].setData(s.Body);
                    document.getElementById("txtremarks").value = s.Remarks;
                    document.getElementById("drp_templatetype").value = s.DocType;
                    $("#IsDefault_Chk").prop('checked', s.IsDefault);
                    ///val(s.IsDefault);

                }
            });

            $("#TrButton").hide();

        }
        

        function OneMoreIsDefaultAccessClick(keyValue) {
            grid.PerformCallback('Access~' + keyValue);
        }

        var flag = true;
        function btnSave_Click(typeret) {

            var data = 'Save';

            var templatename = document.getElementById("txt_description").value;
            var bodyhtml = CKEDITOR.instances['text_id'].getData();
            var remrks = document.getElementById("txtremarks").value;
            var Template_Type = document.getElementById("drp_templatetype").value;
            var DEfaultcheck = $("#IsDefault_Chk").is(":checked");
            var Id = $("#hdID").val();
            if (templatename != '' && bodyhtml != '') {
                $.ajax({
                    type: "POST",
                    url: "Document-Template.aspx/AddEmailTemplateInfo",

                    data: JSON.stringify({ "data": data, "templatename": templatename, "bodyhtml": bodyhtml, "remrks": remrks, "type": Template_Type, "Defaultcheck": DEfaultcheck, "Id": Id }),
                    contentType: "application/json; charset=utf-8",

                    dataType: "json",

                    success: function (msg) {

                        if (msg.d) {

                            // alert(msg.d.status);

                            if (msg.d.status == "success")
                                // jAlert('Saved Successfully');

                                if (typeret == '0') {

                                    if (flag) {
                                        flag = false;
                                        jAlert('Saved Successfully', "", function () {

                                            flag = true;
                                        });

                                    }
                                    document.getElementById("txt_description").value = '';
                                    document.getElementById("txtremarks").value = '';
                                    CKEDITOR.instances['text_id'].setData('')
                                    $("#hdID").val('');
                                }

                                else {

                                    if (flag) {
                                        flag = false;

                                        jAlert('Saved Successfully', 'Alert', function () {
                                            flag = true;
                                            window.location.href = "../Import/Document-Template.aspx";
                                        });
                                    }
                                    //  grid.PerformCallback();
                                }



                        }
                    },


                    error: function (response) {
                        console.log(response);
                    }


                });
            }
            else {

                if (flag) {
                    flag = false;

                    jAlert('Template and Content require', "", function () {

                        flag = true;
                    });
                }
            }
        }

        function btnUpdateSave_Click(typeret) {

            var data = 'Update';

            var templatename = document.getElementById("txt_description").value;
            var bodyhtml = CKEDITOR.instances['text_id'].getData();
            var remrks = document.getElementById("txtremarks").value;
            var Template_Type = document.getElementById("drp_templatetype").value;
            var DEfaultcheck = $("#IsDefault_Chk").is(":checked");
            var Id = $("#hdID").val();
            if (templatename != '' && bodyhtml != '') {
                $.ajax({
                    type: "POST",
                    url: "Document-Template.aspx/ModifyTemplateInfo",

                    data: JSON.stringify({ "data": data, "templatename": templatename, "bodyhtml": bodyhtml, "remrks": remrks, "type": Template_Type, "Defaultcheck": DEfaultcheck, "Id": Id }),
                    contentType: "application/json; charset=utf-8",

                    dataType: "json",

                    success: function (msg) {

                        if (msg.d) {

                            // alert(msg.d.status);

                            if (msg.d.status == "success")

                                if (typeret == '0') {

                                    jAlert('Updated  Successfully');

                                }
                                else {

                                    jAlert('Updated Successfully', 'Alert', function () {

                                        window.location.href = "../Import/Document-Template.aspx";
                                    }
                                        );
                                }
                        }
                    },

                    error: function (response) {
                        console.log(response);
                    }


                });
            }

            else {

                jAlert('Template and Content require');

            }

        }


        function btnOpenlink_Click(keyfield) {

            window.open("../Import/Preview-Template.aspx?T=" + keyfield);

        }

        function BindTemplate() {
            grid.PerformCallback();


        }

        function Callback_BeginCallback() {
            $("#drdExport").val(0);
        }


        function Closelink() {
            window.location.href = "../Import/Document-Template.aspx";
        }


    </script>


    <script type="text/javascript" src="/assests/js/init.js"></script>
    <script type="text/javascript" src="/assests/js/ajax-dynamic-list.js"></script>
    <script src="/assests/pluggins/choosen/choosen.min.js"></script>


</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="panel-heading">

        <div class="panel-title">
            <h3>
                 <asp:Label ID="lblHeading" runat="server" Text="Document Template"></asp:Label>
            </h3>
        </div>

    </div>

    <div class="form_main" style="border: 1px solid #ccc; padding: 10px 15px;">


        <div class="row">

            <div class="col-md-12">
                <% if (rights.CanAdd)
                   { %>
                <a href="javascript:void(0);" onclick="btnAdd_Click()" class="btn btn-primary" id="btnAdd"><span><u>A</u>dd Template</span> </a>

                <%} %>


                <% if (rights.CanExport)
                   { %>

                <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary"
                    OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                    <asp:ListItem Value="0">Export to</asp:ListItem>
                    <asp:ListItem Value="1">PDF</asp:ListItem>
                    <asp:ListItem Value="2">XLS</asp:ListItem>
                    <asp:ListItem Value="3">RTF</asp:ListItem>
                    <asp:ListItem Value="4">CSV</asp:ListItem>
                </asp:DropDownList>

                <%} %>

                <span style="float: right;">

                    <input type="button" value="Show" id="btn_show" class="btn btn-primary" onclick="BindTemplate()" />
                </span>
            </div>

        </div>


        <table class="TableMain100" style="min-width: 183px;">

            <tr>
                <td width="90%" style="text-align: left">

                    <table width="100%">


                        <tr id="TrGrdTemplate">
                            <td colspan="2" valign="top">
                                <dxe:ASPxGridView ID="GrdTemplate" ClientInstanceName="grid" runat="server" KeyFieldName="ID"
                                    Width="100%" OnCustomCallback="GrdTemplate_CustomCallback" OnDataBinding="GrdTemplate_DataBinding" ClientSideEvents-BeginCallback="Callback_BeginCallback" AutoGenerateColumns="False">
                                    <styles>
                                        <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                        </Header>
                                        <LoadingPanel ImageSpacing="10px">
                                        </LoadingPanel>
                                    </styles>
                                    <columns>
                                        <dxe:GridViewDataTextColumn FieldName="ID" Visible="False" VisibleIndex="0">
                                            <CellStyle CssClass="gridcellleft">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn FieldName="Template" VisibleIndex="1" Caption="Template Name">
                                            <Settings AllowAutoFilterTextInputTimer="False" />
                                             <Settings AutoFilterCondition="Contains" />
                                            <CellStyle CssClass="gridcellleft">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>

                               <%--          <dxe:GridViewDataTextColumn FieldName="DocType" VisibleIndex="2" Caption="Type">
                                            <CellStyle CssClass="gridcellleft" Wrap="True">
                                            </CellStyle>
                                            <EditFormSettings Visible="False"></EditFormSettings>
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn FieldName="IsDefault" VisibleIndex="3" Caption="IsDefault">
                                            <CellStyle CssClass="gridcellleft" Wrap="True">
                                            </CellStyle>
                                            <EditFormSettings Visible="False"></EditFormSettings>
                                        </dxe:GridViewDataTextColumn>--%>


                                        <dxe:GridViewDataTextColumn VisibleIndex="4" Width="200px" Caption="Action">
                                            <CellStyle CssClass="gridcellleft">
                                            </CellStyle>
                                            <DataItemTemplate>
                                                 <a href="javascript:void(0);" onclick="OnViewClick('<%# Container.KeyValue %>')" class="pad" title="View" >
                                                                                            <img src="../../../assests/images/viewIcon.png" /></a>

                                                <% if (rights.CanEdit)
                                                   { %>
                                             <a href="javascript:void(0);" onclick="btnUpdate_Click('<%# Container.KeyValue %>')"  title="Edit">
                                                       <img src="../../../assests/images/Edit.png" /></a>
                                                
                                                 <%} %>

                                                 <% if (rights.CanDelete)
                                                    { %>

                                               <a href="javascript:void(0);"  title="Delete"  onclick="btnDelete_Click('<%# Container.KeyValue %>')"><img src="../../../assests/images/Delete.png" /></a>
                                                 
                                                <%} %>

                                                <% if (rights.CanView)
                                                   { %>

                                             <a href="javascript:void(0);"  title="Preview" onclick="btnOpenlink_Click('<%# Container.KeyValue %>')"><img src="../../../assests/images/eye.png" /></a>
                                              <%} %>
                                            </DataItemTemplate>
                                             <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                            <CellStyle HorizontalAlign="Center"></CellStyle>
                                            <HeaderTemplate><span>Actions</span></HeaderTemplate>
                                            <EditFormSettings Visible="False"></EditFormSettings>
                                           
                                        </dxe:GridViewDataTextColumn>




                                    </columns>
                                    <settingssearchpanel visible="True" />
                                    <settings showgrouppanel="True" showstatusbar="Hidden" showfilterrow="true" showfilterrowmenu="True" />
                                    <settingspager numericbuttoncount="20" pagesize="20" showseparators="True" alwaysshowpager="True">
                                        <FirstPageButton Visible="True">
                                        </FirstPageButton>
                                        <LastPageButton Visible="True">
                                        </LastPageButton>
                                    </settingspager>
                                    <styleseditors>
                                        <ProgressBar Height="25px">
                                        </ProgressBar>
                                    </styleseditors>

                                    <settingspager pagesize="10" position="Bottom">
                                        <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                                        </settingspager>

                                    <settingssearchpanel visible="True" />
                                    <settings showgrouppanel="True" showstatusbar="Visible" showfilterrow="true" showfilterrowmenu="true" />
                                    <settingssearchpanel visible="True" delay="5000" />
                                </dxe:ASPxGridView>

                            </td>
                        </tr>

                        <tr id="TrInsert" style="display: none;">
                            <td style="vertical-align: top; width: 47px;">
                                <div class="crossBtn"><a onclick="Closelink()"><i class="fa fa-times"></i></a></div>
                                <div>
                                    <div class="col-md-3">
                                        <label>
                                            <span class="Ecoheadtxt">Template For :</span>
                                        </label>
                                        <div>


                                            <asp:DropDownList ID="drp_templatetype" runat="server" Width="100%">

                                                <asp:ListItem Value="DF">Default</asp:ListItem>

                                            </asp:DropDownList>


                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <label>
                                            <span class="Ecoheadtxt">Template Name :</span>
                                            <span style="color: red;">*</span>
                                        </label>
                                        <div>
                                            <asp:TextBox ID="txt_description" runat="server" MaxLength="3000" Width="100%"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-3" style="display: none">
                                        <label>
                                        </label>
                                        <div>
                                        </div>
                                    </div>



                                    <div class="col-md-3" id="TrReceip" style="display: none">
                                        <label>
                                        </label>
                                        <div>
                                            <asp:ListBox ID="lstItems" runat="server" SelectionMode="Multiple" Font-Size="12px" Height="90px" Width="253px" CssClass="mb0 chsn hide" data-placeholder="Select ..."></asp:ListBox>
                                            <asp:HiddenField ID="hndRecipients_hidden" runat="server" />
                                        </div>
                                    </div>

                                    <div class="col-md-3" id="TrAccess" style="display: none">
                                        <label>
                                        </label>
                                        <div>
                                            <asp:DropDownList ID="drp_accesslevel" runat="server" Width="100%">
                                                <asp:ListItem Value="1">Public</asp:ListItem>
                                                <asp:ListItem Value="0">Private</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="col-md-3" id="TrReserveWord" style="display: none">


                                        <div class="relative">

                                            <asp:DropDownList ID="drpSenderType" runat="server" Width="100%">
                                            </asp:DropDownList>

                                        </div>
                                    </div>


                                    <div class="col-md-3" id="Trdefault" style="display: none">
                                        <label>IsDefault :</label>

                                        <div class="relative">
                                            <asp:CheckBox ID="IsDefault_Chk" runat="server" />
                                        </div>


                                    </div>

                                </div>

                                <asp:HiddenField ID="HiddenField1" runat="server" />


                                <div class="clear"></div>
                                <div class="col-md-9" id="TrMessage">

                                    <label class="Ecoheadtxt">Content :</label>
                                    <span style="color: red;">*</span>


                                    <div>
                                        <asp:TextBox ID="text_id" runat="server"></asp:TextBox>
                                    </div>


                                    <div style="text-align: right; display: none;">

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

                                <div class="col-md-3">
                                    <label>
                                        <span class="Ecoheadtxt">Remarks :</span>
                                    </label>
                                    <div>
                                        <asp:TextBox ID="txtremarks" runat="server" Style="width: 879px; height: 50px" TextMode="MultiLine"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="clear"></div>

                                <br />

                                <div class="col-md-12" id="TrButton">

                                    <input id="btnSave" type="button" value="S&#818;ave & New" class="btn btn-primary" onclick="btnSave_Click(0)" />


                                    <input id="btnSaveexit" type="button" value="Save & Ex&#818;it" class="btn btn-primary" onclick="btnSave_Click(1)" />


                                    <%--  <input id="btnupdate" type="button" value="U&#818;pdate" class="btn btn-primary" onclick="btnUpdateSave_Click(1)" />--%>


                                    <%--  <input id="btnupdateexit" type="button" value="Save & Ex&#818;it" class="btn btn-primary" onclick="btnUpdateSave_Click(1)" />--%>

                                    <input id="btnDiscard" type="button" value="Discard" class="btn btn-danger" style="display: none;" onclick="btnDiscard_Click()" />


                                    <input id="hdID" type="hidden" style="width: 151px; height: 7px" />


                                </div>

                            </td>
                        </tr>


                    </table>

                </td>
            </tr>

        </table>


    </div>



    <div style="display: none">
        <dxe:ASPxGridViewExporter ID="exporter" GridViewID="GrdTemplate" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
    </div>


</asp:Content>


