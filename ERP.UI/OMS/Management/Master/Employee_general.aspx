<%@ Page Title="Employee" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true"
    Inherits="ERP.OMS.Management.Master.management_master_Employee_general" CodeBehind="Employee_general.aspx.cs" EnableEventValidation="false" %>

<%--<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxTabControl" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxClasses" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe000001" %>
<%@ Register assembly="DevExpress.Web.v10.2" namespace="DevExpress.Web.ASPxTabControl" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v10.2" namespace="DevExpress.Web.ASPxClasses" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v10.2" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <!--___________________These files are for List Items__________________________-->


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <style>
        a img {
            border: none;
        }

        ol li {
            list-style: decimal outside;
        }

        div#container {
            width: 780px;
            margin: 0 auto;
            padding: 1em 0;
        }

        div.side-by-side {
            width: 100%;
            margin-bottom: 1em;
        }

            div.side-by-side > div {
                float: left;
                width: 50%;
            }

                div.side-by-side > div > em {
                    margin-bottom: 10px;
                    display: block;
                }

        .clearfix:after {
            content: "\0020";
            display: block;
            height: 0;
            clear: both;
            overflow: hidden;
            visibility: hidden;
        }

        .chosen-container-active.chosen-with-drop .chosen-single div,
        .chosen-container-single .chosen-single div {
            display: none !important;
        }

        .chosen-container-single .chosen-single {
            border-radius: 0 !important;
            background: transparent !important;
        }
    </style>
    <style>
        .chosen-container.chosen-container-single {
            width: 100% !important;
        }

        .chosen-choices {
            width: 100% !important;
        }

        #lstReferedBy {
            width: 400px;
        }

        #lstReferedBy {
            display: none !important;
        }

        .dxtcLite_PlasticBlue > .dxtc-content {
            overflow: visible !important;
        }

        #lstReferedBy_chosen {
            width: 39% !important;
        }
    </style>
    <link href="../../css/choosen.min.css" rel="stylesheet" />
    <script src="/assests/pluggins/choosen/choosen.min.js"></script>

    <script language="javascript" type="text/javascript">

        //Code for UDF Control 
        function OpenUdf() {
            if (document.getElementById('IsUdfpresent').value == '0') {
                jAlert("UDF not define.");
            }
            else {
                var url = 'frm_BranchUdfPopUp.aspx?Type=Em';
                popup.SetContentUrl(url);
                popup.Show();
            }
        }
        // End Udf Code

        $(document).ready(function () {
            $(".chzn-select").chosen();
            $(".chzn-select-deselect").chosen({ allow_single_deselect: true });
            ListBind();
            ChangeSource();

        });
        function Changeselectedvalue() {
            var lstReferedBy = document.getElementById("lstReferedBy");
            if (document.getElementById("hdReferenceBy").value != '') {
                for (var i = 0; i < lstReferedBy.options.length; i++) {
                    if (lstReferedBy.options[i].value == document.getElementById("hdReferenceBy").value) {
                        lstReferedBy.options[i].selected = true;
                    }
                }
                $('#lstReferedBy').trigger("chosen:updated");
            }

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
        function lstReferedBy() {

            // $('#lstReferedBy').chosen();
            $('#lstReferedBy').fadeIn();
        }
        function setvalue() {
            document.getElementById("txtReferedBy_hidden").value = document.getElementById("lstReferedBy").value;
        }
        function ChangeSource() {

            var InterId = "";
            var fname = "%";
            var lReferBy = $('select[id$=lstReferedBy]');
            lReferBy.empty();
            $.ajax({
                type: "POST",
                url: "Employee_general.aspx/GetReferredBy",
                data: JSON.stringify({ firstname: fname }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var list = msg.d;
                    var listItems = [];
                    if (list.length > 0) {

                        for (var i = 0; i < list.length; i++) {
                            var id = '';
                            var name = '';
                            cnt_internalId = list[i].split('|')[1];
                            cnt_firstName = list[i].split('|')[0];
                            $('#lstReferedBy').append($('<option>').text(cnt_firstName).val(cnt_internalId));
                        }

                        $(lReferBy).append(listItems.join(''));

                        lstReferedBy();
                        $('#lstReferedBy').trigger("chosen:updated");
                        Changeselectedvalue();
                    }
                    else {
                        //alert("No records found");
                        lstReportTo();
                        $('#lstReferedBy').trigger("chosen:updated");
                    }
                }
            });

        }

        function ClickOnSpeak(keyValue) {
            var url = 'frmLanguageProfi.aspx?id=' + keyValue + '&status=speak';
            window.location.href = url;
            //window.open(url, 'popupwindow', 'left=120,top=200,height=500,width=500,scrollbars=no,toolbar=no,location=center,menubar=no');
        }

        function ClickOnWrite(keyValue) {
            var url = 'frmLanguageProfi.aspx?id=' + keyValue + '&status=write';
            window.location.href = url;
            //window.open(url, 'popupwindow', 'left=120,top=200,height=500,width=500,scrollbars=no,toolbar=no,location=center,menubar=no');

        }
        function heightB() {
            //   alert("123");
        }

        function disp_prompt(name) {
            if (name == "tab0") {
                //alert(name);
                document.location.href = "Employee_general.aspx";
            }
            if (name == "tab1") {
                //alert(name);
                document.location.href = "Employee_Correspondence.aspx";
            }
            else if (name == "tab2") {
                //alert(name);
                document.location.href = "Employee_Education.aspx";
            }
            else if (name == "tab3") {
                //alert(name);
                document.location.href = "Employee_Employee.aspx";
            }
            else if (name == "tab4") {
                //alert(name);
                document.location.href = "Employee_Document.aspx";
            }
            else if (name == "tab5") {
                //alert(name);
                document.location.href = "Employee_FamilyMembers.aspx";
            }
            else if (name == "tab6") {
                //alert(name);
                document.location.href = "Employee_GroupMember.aspx";
            }
            else if (name == "tab7") {
                //alert(name);
                document.location.href = "Employee_EmployeeCTC.aspx";
            }
            else if (name == "tab8") {
                //alert(name);
                document.location.href = "Employee_BankDetails.aspx";
            }
            else if (name == "tab9") {
                //alert(name);
                document.location.href = "Employee_Remarks.aspx";
            }
            else if (name == "tab10") {
                //alert(name);
                //document.location.href = "Employee_Remarks.aspx";
            }
            else if (name == "tab11") {
                //alert(name);
                // document.location.href = "Employee_Education.aspx";
            }
            else if (name == "tab12") {
                //alert(name);
                //   document.location.href="Employee_Subscription.aspx"; 
            }

            else if (name == "tab13") {
                //alert(name);
                var keyValue = $("#hdnlanguagespeak").val();
                document.location.href = 'frmLanguageProfi.aspx?id=' + keyValue + '&status=speak';
                //   document.location.href="Employee_Subscription.aspx"; 
            }
            else if (name == "tab14") {
                //alert(name);
                document.location.href = "StatutoryDocumnentDtls.aspx";
            }
            else if (name == "tab15") {
                //alert(name);
                document.location.href = "PFESIDtls.aspx";
            }

            else if (name == "tab16") {
                //alert(name);
                document.location.href = "OtherDetails.aspx";
            }
            else if (name == "tab17") {
                document.location.href = "Resignation.aspx";
            }


        }
        function FillValues(chk) {
            var sel = document.getElementById('ASPxPageControl1_LitSpokenLanguage');
            sel.value = chk;
        }
        function FillValues1(chk) {
            var sel = document.getElementById('ASPxPageControl1_LitWrittenLanguage');
            sel.value = chk;
        }

        function CallList(obj1, obj2, obj3) {
            if (obj1.value == "") {
                obj1.value = "%";
            }
            //alert('rrr');
            FieldName = 'ASPxPageControl1_cmbGender';


            //var obj4 = document.getElementById("ASPxPageControl1_cmbSource");
            var obj4 = document.getElementById('<%=cmbSource.ClientID %>');
            var obj5 = obj4.value;
            //alert(obj5);
            if (obj5 != '18') {
                ajax_showOptionsTEST(obj1, obj2, obj3, obj5);
                if (obj1.value == "%") {
                    obj1.value = "";
                }
            }
        }
        function AtTheTimePageLoad() {
            FieldName = 'ASPxPageControl1_cmbLegalStatus';
            //alert('22');
            document.getElementById("txtReferedBy_hidden").style.display = 'none';
        }
        function FreeHiddenField() {
            var hddn = document.getElementById("txtReferedBy_hidden");
            //  alert(hddn.value);
            hddn.value = '';
            //  alert(hddn.value);
        }
        FieldName = 'ASPxPageControl1_cmbLegalStatus';
    </script>
    <style>
        .noleftpad {
            padding-left: 0 !important;
            margin-left: 0 !important;
        }

        .pos22 {
            position: absolute;
            right: 9px;
            top: 10px;
        }

        #lstReferedBy_chosen {
            width: 170px !important;
        }

        .dxbButton_PlasticBlue div.dxb {
            padding: 0 !important;
        }

        #fileupload {
            opacity: 0;
        }

        #upload-label {
            position: absolute;
            top: 50%;
            left: 1rem;
            transform: translateY(-50%);
        }

        .image-area {
            border: 1px dashed rgba(86, 83, 83, 0.7);
            padding: 3px;
            position: relative;
            width: 80px;
            height: 80px;
            /* border-radius: 50%; */
            overflow: hidden;
        }

            .image-area::before {
                content: 'Image';
                color: #3e3a3a;
                font-weight: bold;
                text-transform: uppercase;
                position: absolute;
                top: 50%;
                left: 50%;
                transform: translate(-50%, -50%);
                font-size: 1.2rem;
                z-index: 1;
            }

        .pback {
            background: #f5faff;
            padding: 10px 15px;
            border-radius: 15px;
            box-shadow: 0 4px 5px rgba(0,0,0,0.15);
            display: inline-block;
        }

        .image-area img {
            z-index: 2;
            position: relative;
            max-width: 100%;
        }

        #profileUpload {
            padding: 10px 15px;
            border: 1px solid #f3f2f2;
            border-radius: 7px;
            margin-bottom: 10px;
        }

        .shadow-sm {
            box-shadow: 0 .125rem .25rem rgba(0,0,0,.075) !important;
        }

        .rounded-pill {
            border-radius: 50rem !important;
        }

        .bg-white {
            background-color: #fff !important;
        }

        #profileUpload .input-group {
            position: relative;
            display: -ms-flexbox;
            display: flex;
            -ms-flex-wrap: wrap;
            flex-wrap: wrap;
            -ms-flex-align: stretch;
            align-items: stretch;
            width: 100%;
            margin-bottom: 13px;
        }

            #profileUpload .input-group > .custom-file, #profileUpload .input-group > .custom-select, #profileUpload .input-group > .form-control, #profileUpload .input-group > .form-control-plaintext {
                position: relative;
                -ms-flex: 1 1 auto;
                flex: 1 1 auto;
                width: 1%;
                margin-bottom: 0;
            }

        #upload-label {
            position: absolute;
            top: 50%;
            left: 1rem;
            transform: translateY(-50%);
        }

        #profileUpload .btn-light {
            color: #212529;
            background-color: #f8f9fa;
            border-color: #f8f9fa;
        }

        #profileUpload .font-weight-bold {
            font-weight: 700 !important;
        }

        #profileUpload {
            padding: 10px 15px;
            border: 1px solid #e4dddd;
        }

            #profileUpload .btn-light:hover {
                background-color: #3575c5;
                border-color: #3575c5;
                color: #fff;
            }

                #profileUpload .btn-light:hover .text-muted {
                    color: #fff;
                }

        .image-area > img {
            max-width: 100%;
        }
    </style>
    <script type="text/javascript">
        /*  ==========================================
            SHOW UPLOADED IMAGE
        * ========================================== */
        function readURL(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();

                reader.onload = function (e) {
                    console.log(e)
                    $('#imageResult')
                        .attr('src', e.target.result);
                };
                reader.readAsDataURL(input.files[0]);
            }
        }

        $(function () {
            $('#fileupload').on('change', function () {
                readURL(input);
            });
        });

        /*  ==========================================
            SHOW UPLOADED IMAGE NAME
        * ========================================== */
        var input = document.getElementById('fileupload');
        var infoArea = document.getElementById('upload-label');


        //$('#fileupload').on('change', function (e) {
        //    console.log(this)
        //});

        function showFileName(event) {
            var input = event.srcElement;
            var fileName = input.files[0].name;
            infoArea.textContent = 'File name: ' + fileName;
        }
    </script>
    <style>
        .pActive, .pOut {
            display: inline-block;
            width: 10px;
            height: 10px;
            border-radius: 50%;
        }

        .pActive {
            background: green;
        }

        .pOut {
            background: red;
        }

        .profileInformation > div {
            padding: 0;
            margin-bottom: 4px;
            font-size: 14px;
        }

        .pNameT {
            font-size: 14px;
        }

        .pback table > tbody > tr > td:first-child {
            padding-right: 25px;
        }

        .rplTbl > tbody > tr > td {
            padding: 8px 10px;
        }

            .rplTbl > tbody > tr > td:first-child {
                padding-right: 35px;
            }
    </style>
    <div class="panel-title">
        <h3>Employee</h3>
        <div class="crossBtn">
            <a href="Employee.aspx" id="goBackCrossBtn"><i class="fa fa-times"></i></a>
            <%--  <asp:HyperLink
                ID="goBackCrossBtn"
                NavigateUrl="#"
                runat="server">
        <i class="fa fa-times"></i>
            </asp:HyperLink>--%>
        </div>
    </div>


    <div class="form_main">
        <table class="TableMain100">
            <tr>
                <td style="text-align: center">
                    <asp:Label ID="lblHeader" runat="server" Font-Bold="True" Font-Size="15px" ForeColor="Navy"
                        Width="819px" Height="18px"></asp:Label>
                    <%--debjyoti 22-12-2016--%>
                    <dxe:ASPxPopupControl ID="ASPXPopupControl" runat="server"
                        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popup" Height="630px"
                        Width="600px" HeaderText="Add/Modify UDF" Modal="true" AllowResize="true" ResizingMode="Postponed">
                        <ContentCollection>
                            <dxe:PopupControlContentControl runat="server">
                            </dxe:PopupControlContentControl>
                        </ContentCollection>
                    </dxe:ASPxPopupControl>

                    <asp:HiddenField runat="server" ID="IsUdfpresent" />
                    <%--End debjyoti 22-12-2016--%>

                </td>
            </tr>
            <tr>
                <td>
                    <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" ClientInstanceName="page"
                        Width="100%">
                        <TabPages>
                            <dxe:TabPage Text="General" Name="General">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                        <div class="clearfix">
                                            <div class="col-lg-12 mx-auto clearfix" id="profileUpload">
                                                <div class="col-md-3" style="padding-left: 0">
                                                    <div style="font-size: 15px; margin-bottom: 10px;">
                                                        <label>Upload Profile Photo</label></div>
                                                    <!-- Upload image input-->
                                                    <div class="input-group rounded-pill bg-white shadow-sm">
                                                        <%-- <input id="upload" type="file" onchange="readURL(this);" class="form-control border-0">--%>
                                                        <%-- <label id="upload-label" for="upload" class="font-weight-light text-muted">Choose file</label>--%>
                                                        <asp:FileUpload ID="fileupload" runat="server" CssClass="form-control border-0 " onchange="readURL(this);" />

                                                        <asp:Label ID="lblfilesize" Visible="false" Text="Please choose file upto 1 MB" ForeColor="Red" runat="server"></asp:Label>
                                                        <label id="upload-label" for="fileupload" class="font-weight-light text-muted">Choose file</label>
                                                        <div class="input-group-append">
                                                            <label for="fileupload" class="btn btn-light m-0 rounded-pill px-4"><i class="fa fa-cloud-upload mr-2 text-muted"></i><small class="text-uppercase font-weight-bold text-muted">Choose file</small></label>

                                                            <asp:HiddenField ID="hdnimagesrc" runat="server" />
                                                        </div>
                                                    </div>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                                        ControlToValidate="fileupload" ErrorMessage="Only .jpg,.png,.jpeg,.gif Files are allowed" Font-Bold="True" Font-Size="Small" ForeColor="Red"
                                                        ValidationExpression="(.*?)\.(jpg|jpeg|png|gif|JPG|JPEG|PNG|GIF)$"></asp:RegularExpressionValidator>
                                                    <!-- Uploaded image area-->

                                                </div>
                                                <div class="col-md-6">
                                                    <div class="pback">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <div class="image-area mt-4">
                                                                        <img id="imageResult" runat="server" src="#" alt="" class="img-fluid rounded shadow-sm mx-auto d-block">
                                                                    </div>
                                                                </td>
                                                                <td>
                                                                    <div class="profileInformation">
                                                                        <table class="rplTbl">
                                                                            <tr>
                                                                                <td>
                                                                                    <div class="pNameT">
                                                                                        <asp:Label ID="lblname" runat="server"></asp:Label></div>
                                                                                </td>
                                                                                <td>

                                                                                    <div>
                                                                                        <span><i class="fa fa-building-o"></i></span><span>
                                                                                            <asp:Label ID="lblprofession" runat="server"></asp:Label></span>
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <div class="pNameT">
                                                                                        <asp:Label ID="lblsupervisor" runat="server"></asp:Label></div>
                                                                                </td>
                                                                                <td>
                                                                                    <div>
                                                                                        <span><i class="fa fa-map-marker"></i></span><span>
                                                                                            <asp:Label ID="lbllocation" runat="server"></asp:Label></span>
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <div>
                                                                                        <span class="pActive"></span>
                                                                                        <span id="active" runat="server">Active</span>
                                                                                    </div>
                                                                                    <div id="inactive" runat="server">
                                                                                        <span class="pOut"></span>
                                                                                        <%--   <span>Logout</span>--%>
                                                                                    </div>
                                                                                </td>
                                                                                <td></td>
                                                                            </tr>
                                                                        </table>





                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>

                                                </div>
                                                <div class="clear"></div>

                                            </div>
                                        </div>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <table style="width: 100%; z-index: 101">
                                                        <tr>
                                                            <td class="Ecoheadtxt" style="width: 151px">Salutation
                                                            </td>
                                                            <td class="Ecoheadtxt" style="text-align: left; width: 214px;">
                                                                <asp:DropDownList ID="CmbSalutation" runat="server" Width="170px" TabIndex="1">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td class="Ecoheadtxt" style="width: 100px">First Name<span style="color: red">*</span>
                                                            </td>
                                                            <td class="Ecoheadtxt" style="text-align: left; width: 197px; position: relative">
                                                                <dxe:ASPxTextBox ID="txtFirstNmae" runat="server" Width="170px" TabIndex="2" MaxLength="150">
                                                                    <ValidationSettings ValidationGroup="a">
                                                                    </ValidationSettings>
                                                                </dxe:ASPxTextBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtFirstNmae"
                                                                    Display="Dynamic" ErrorMessage="" SetFocusOnError="True" CssClass="pullleftClass fa fa-exclamation-circle iconRed pos22"
                                                                    ValidationGroup="a"></asp:RequiredFieldValidator>
                                                            </td>
                                                            <td class="Ecoheadtxt" style="width: 137px">Middle Name
                                                            </td>
                                                            <td class="Ecoheadtxt" style="text-align: left; width: 214px;">
                                                                <dxe:ASPxTextBox ID="txtMiddleName" runat="server" Width="170px" TabIndex="3" MaxLength="50">
                                                                </dxe:ASPxTextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4" style="text-align: right; height: 1px"></td>
                                                            <td colspan="2" style="text-align: right; height: 1px"></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="Ecoheadtxt" style="width: 151px">Last Name
                                                            </td>
                                                            <td class="Ecoheadtxt" style="text-align: left; width: 214px;">
                                                                <dxe:ASPxTextBox ID="txtLastName" runat="server" Width="170px" TabIndex="4" MaxLength="50">
                                                                </dxe:ASPxTextBox>
                                                            </td>
                                                            <td class="Ecoheadtxt" style="width: 100px">Employee ID
                                                            </td>
                                                            <td class="Ecoheadtxt" style="text-align: left; width: 197px;">



                                                                <%--  <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                                         <ContentTemplate>--%>
                                                                <dxe:ASPxTextBox ID="txtAliasName" runat="server" Width="170px" TabIndex="5" ReadOnly="true" ForeColor="black">
                                                                </dxe:ASPxTextBox>
                                                                <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click" Style="color: #cc3300; text-decoration: underline; font-size: 8pt; display: none">Make System Employee Code</asp:LinkButton>
                                                                <br />
                                                                <asp:Label ID="lblErr" Text="" runat="server"></asp:Label>
                                                                <%--  </ContentTemplate>
                                                                    </asp:UpdatePanel>--%>
                                                                <%--  <asp:UpdatePanel ID="UpdatePanelId" runat="server">
                                                                    <ContentTemplate>
                                                                  <dxe:ASPxTextBox ID="txtAliasName" runat="server" Width="170px" TabIndex="5">
                                                                    </dxe:ASPxTextBox>
                                                                      <asp:LinkButton ID="LinkButton1" runat="server"  OnClick="LinkButton1_Click"  style="color: #cc3300; text-decoration: underline; font-size: 8pt;">Make System Employee Code</asp:LinkButton>
                                                                    <br /><asp:Label ID="lblErr" Text="" runat="server"></asp:Label>
                                                                                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>--%>
                                                            </td>
                                                            <td class="Ecoheadtxt" style="width: 137px">Branch
                                                            </td>
                                                            <td class="Ecoheadtxt" style="text-align: left; width: 214px;">
                                                                <asp:DropDownList ID="cmbBranch" runat="server" Width="170px" TabIndex="6">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="Ecoheadtxt" style="width: 100px">Source
                                                            </td>
                                                            <td class="Ecoheadtxt" style="text-align: left; width: 214px;">
                                                                <asp:DropDownList ID="cmbSource" runat="server" Width="170px" TabIndex="7">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td class="Ecoheadtxt" style="width: 143px">Referred By
                                                            </td>
                                                            <td class="Ecoheadtxt" style="text-align: left; width: 197px;">
                                                                <asp:DropDownList data-placeholder="Select or type here" runat="server" Visible="false" ID="ddlReferedBy" TabIndex="8" class="chzn-select" Style="width: 169px;">
                                                                    <asp:ListItem Text="select referer" Value=""></asp:ListItem>

                                                                </asp:DropDownList>
                                                                <asp:ListBox ID="lstReferedBy" CssClass="chsn" runat="server" Font-Size="12px" Width="150px" data-placeholder="Select..."></asp:ListBox>
                                                                <%--  <asp:TextBox ID="txtReferedBy" runat="server" Width="165px" Visible="true" TabIndex="8"></asp:TextBox>--%>
                                                                <asp:TextBox ID="txtReferedBy_hidden" runat="server"></asp:TextBox>
                                                            </td>
                                                            <td class="Ecoheadtxt" style="width: 137px">Marital Status
                                                            </td>
                                                            <td class="Ecoheadtxt" style="text-align: left; width: 214px;">
                                                                <asp:DropDownList ID="cmbMaritalStatus" runat="server" Width="170px" TabIndex="9">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="Ecoheadtxt" style="width: 100px">Gender
                                                            </td>
                                                            <td class="Ecoheadtxt" style="text-align: left; width: 214px;">
                                                                <asp:DropDownList ID="cmbGender" runat="server" Width="170px" TabIndex="10">
                                                                    <asp:ListItem Value="1">Male</asp:ListItem>
                                                                    <asp:ListItem Value="0">Female</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td class="Ecoheadtxt" style="width: 143px">D.O.B.
                                                            </td>
                                                            <td class="Ecoheadtxt" style="text-align: left; width: 197px;">
                                                                <dxe:ASPxDateEdit ID="DOBDate" runat="server" DateOnError="Today" EditFormat="Custom"
                                                                    TabIndex="11">
                                                                </dxe:ASPxDateEdit>
                                                            </td>
                                                            <td class="Ecoheadtxt" style="width: 137px">Anniversary Date
                                                            </td>
                                                            <td class="Ecoheadtxt" style="text-align: left; width: 214px;">
                                                                <dxe:ASPxDateEdit ID="AnniversaryDate" runat="server" DateOnError="Today" EditFormat="Custom"
                                                                    TabIndex="12">
                                                                </dxe:ASPxDateEdit>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="Ecoheadtxt" style="width: 100px">Legal Status
                                                            </td>
                                                            <td class="Ecoheadtxt" style="text-align: left; width: 214px;">
                                                                <asp:DropDownList ID="cmbLegalStatus" runat="server" Width="170px" TabIndex="13">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td style="width: 143px" class="Ecoheadtxt">Education
                                                            </td>
                                                            <td class="Ecoheadtxt" style="text-align: left; width: 197px;">
                                                                <asp:DropDownList ID="cmbEducation" runat="server" Width="170px" TabIndex="14">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td id="Td1" class="Ecoheadtxt" runat="server" style="width: 137px">Profession
                                                            </td>
                                                            <td id="Td2" class="Ecoheadtxt" style="text-align: left; width: 214px;" runat="server">
                                                                <asp:DropDownList ID="cmbProfession" runat="server" Width="170px" TabIndex="15">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr id="hide1" runat="server" visible="False">
                                                            <td id="Td3" class="Ecoheadtxt" runat="server" style="width: 100px">Organization
                                                            </td>
                                                            <td id="Td4" class="Ecoheadtxt" style="text-align: left; width: 214px;" runat="server">
                                                                <dxe:ASPxTextBox ID="txtOrganization" runat="server" Width="170px" TabIndex="16">
                                                                </dxe:ASPxTextBox>
                                                            </td>
                                                            <td id="Td5" class="Ecoheadtxt" runat="server" style="width: 143px">Job Responsibility
                                                            </td>
                                                            <td id="Td6" class="Ecoheadtxt" style="text-align: left; width: 197px;" runat="server">
                                                                <asp:DropDownList ID="cmbJobResponsibility" runat="server" Width="170px" TabIndex="17">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td id="Td7" class="Ecoheadtxt" runat="server" style="width: 137px">Designation
                                                            </td>
                                                            <td id="Td8" class="Ecoheadtxt" style="text-align: left; width: 214px;" runat="server">
                                                                <asp:DropDownList ID="cmbDesignation" runat="server" Width="169px" TabIndex="18">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr id="hide2" runat="server" visible="False">
                                                            <td id="Td9" class="Ecoheadtxt" runat="server" style="width: 100px">Industry/Sector
                                                            </td>
                                                            <td id="Td10" class="Ecoheadtxt" runat="server" style="width: 214px; text-align: left;">
                                                                <asp:DropDownList ID="cmbIndustry" runat="server" Width="170px" TabIndex="19">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td id="Td11" class="Ecoheadtxt" runat="server" style="width: 143px">Rating
                                                            </td>
                                                            <td id="Td12" class="Ecoheadtxt" style="text-align: left; width: 197px;" runat="server">
                                                                <asp:DropDownList ID="cmbRating" runat="server" Width="170px" TabIndex="20">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr id="hide3" runat="server" visible="False">
                                                            <td id="Td13" class="Ecoheadtxt" runat="server" style="width: 151px">Contact Status
                                                            </td>
                                                            <td id="Td14" class="Ecoheadtxt" style="text-align: left; width: 214px;" runat="server">
                                                                <asp:DropDownList ID="cmbContactStatus" runat="server" Width="170px" TabIndex="21">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td id="Td16" class="Ecoheadtxt" runat="server" style="width: 137px">Blood Group
                                                            </td>
                                                            <td id="Td17" class="Ecoheadtxt" style="text-align: left; width: 214px;" runat="server">
                                                                <asp:DropDownList ID="cmbBloodgroup" runat="server" Width="75px" TabIndex="22">
                                                                    <asp:ListItem Value="N/A">N/A</asp:ListItem>
                                                                    <asp:ListItem Value="A+">A+</asp:ListItem>
                                                                    <asp:ListItem Value="A-">A-</asp:ListItem>
                                                                    <asp:ListItem Value="B+">B+</asp:ListItem>
                                                                    <asp:ListItem Value="B-">B-</asp:ListItem>
                                                                    <asp:ListItem Value="AB+">AB+</asp:ListItem>
                                                                    <asp:ListItem Value="AB-">AB-</asp:ListItem>
                                                                    <asp:ListItem Value="O+">O+</asp:ListItem>
                                                                    <asp:ListItem Value="O-">O-</asp:ListItem>
                                                                </asp:DropDownList>
                                                                &nbsp;&nbsp; <%--Allow Web Login--%>
                                                                <asp:CheckBox ID="chkAllow" Visible="false" TextAlign="Left" runat="server" Width="17px" TabIndex="23" />
                                                            </td>
                                                            <%-- Rev Rajdip --%>
                                                            <td class="Ecoheadtxt" style="width: 137px">Date Of Joinning
                                                            </td>
                                                            <td class="Ecoheadtxt" style="text-align: left; width: 214px;">
                                                                <dxe:ASPxDateEdit ID="txtjoinningdate" runat="server" DateOnError="Today" EditFormat="Custom"
                                                                    TabIndex="12">
                                                                </dxe:ASPxDateEdit>
                                                            </td>
                                                            <td class="Ecoheadtxt" style="width: 137px">Date Of Confirmation
                                                            </td>
                                                            <td class="Ecoheadtxt" style="text-align: left; width: 214px;">
                                                                <dxe:ASPxDateEdit ID="txtconfirmationdate" runat="server" DateOnError="Today" EditFormat="Custom"
                                                                    TabIndex="12">
                                                                </dxe:ASPxDateEdit>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <%--End Rev Rajdip --%>
                                                            <td>Can Update Attendence<br />
                                                                for Last</td>
                                                            <td>

                                                                <table>
                                                                    <tr>
                                                                        <td style="width: 40%;">
                                                                            <dxe:ASPxTextBox ID="txtUpdateDays" ClientInstanceName="ctxtUpdateDays" runat="server" Width="100%" ValidationSettings-Display="Dynamic">
                                                                                <MaskSettings Mask="&lt;0..365&gt;" AllowMouseWheel="false" />
                                                                            </dxe:ASPxTextBox>
                                                                        </td>
                                                                        <td style="width: 60%; padding-left: 5px;">Day(s)
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td id="Td18" class="Ecoheadtxt" runat="server" style="width: 137px">Show Subordinates Only<br />
                                                                (Attendance Cycle) 
                                                            </td>
                                                            <td>
                                                                <dxe:ASPxCheckBox ID="chkUsehierchy" runat="server" ClientInstanceName="cchkUsehierchy" ToolTip="Select this option to show only Subordinates data in Attendance cycle. De-Select this option to see all Employees.">
                                                                </dxe:ASPxCheckBox>
                                                            </td>

                                                            <td id="Td19" class="Ecoheadtxt" runat="server" style="width: 137px">Inactive? 
                                                            </td>
                                                            <td>
                                                                <dxe:ASPxCheckBox ID="chkInactive" runat="server" ClientInstanceName="cchkInactive">
                                                                </dxe:ASPxCheckBox>
                                                            </td>

                                                        </tr>
                                                        <tr>

                                                            <td id="Td20" class="Ecoheadtxt" runat="server" style="width: 137px">Tds Deductee Type
                                                            </td>

                                                            <td id="Td21" class="Ecoheadtxt" style="text-align: left; width: 400px;" runat="server">
                                                                <dxe:ASPxComboBox ID="cmbTDS" ClientInstanceName="ccmbTDS" runat="server" Width="171px">
                                                                    <%-- <Items>
                                                                        <dxe:ListEditItem  Text="Select" Value="0" />
                                                                        <dxe:ListEditItem Text="Individual/HUF" Value="1" />
                                                                        <dxe:ListEditItem Text="Others" Value="2" />
                                                                    </Items>--%>
                                                                </dxe:ASPxComboBox>
                                                            </td>


                                                            <td id="lblchkIsDirector" class="Ecoheadtxt" runat="server" style="width: 137px">Is Director 
                                                            </td>
                                                            <td class="Ecoheadtxt" id="DivchkIsDirector" runat="server">
                                                                <dxe:ASPxCheckBox ID="chkIsDirector" runat="server" ClientInstanceName="cchkIsDirector">
                                                                </dxe:ASPxCheckBox>
                                                            </td>


                                                        </tr>
                                                        <tr>

                                                            <td colspan="4"></td>
                                                        </tr>
                                                    </table>

                                                </td>
                                            </tr>
                                            <tr id="TrLang" runat="server">
                                                <td id="Td15" runat="server" style="width: 845px">
                                                    <asp:Panel ID="Panel2" runat="server" Width="100%" BorderColor="White" BorderWidth="1px">
                                                        <table width="100%">
                                                            <tr>
                                                                <td>
                                                                    <table style="width: 100%; display: none;">
                                                                        <tr>
                                                                            <td colspan="2" style="text-align: center; padding: 8px 10px; background: #ccc;">
                                                                                <span>Language Proficiencies </span>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 50%; vertical-align: top; display: none;">
                                                                                <asp:Panel ID="PnlSpLAng" runat="server" Width="100%" BorderColor="White" BorderWidth="1px">
                                                                                    <table width="80%">
                                                                                        <tr>
                                                                                            <td style="text-align: left; color: Blue;">Can Speak
                                                                                            </td>

                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td style="vertical-align: top" class="gridcellleft">
                                                                                                <asp:TextBox ID="LitSpokenLanguage" runat="server" ForeColor="Maroon" BackColor="Transparent"
                                                                                                    BorderStyle="None" Width="100%" ReadOnly="True" CssClass="noleftpad"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td style="text-align: left; vertical-align: top; font-size: x-small; color: Red; display: none;">
                                                                                                <%-- <a href="frmLanguages.aspx?id='<%=SpLanguage %>'&status=speak" onclick="window.open(this.href,'popupwindow','left=120,top=170,height=400,width=200,scrollbars=no,toolbar=no,location=center,menubar=no'); return false;"
                                                                                                        style="font-size: x-small; color: Red;" tabindex="24">click to add</a>--%>
                                                                                                <a href="javascript:void(0);" onclick="ClickOnSpeak('<%# Eval("id") %>')" title="Map" class="btn btn-primary" tabindex="24">Select Language(s)</a>
                                                                                                <asp:HiddenField ID="hdnlanguagespeak" runat="server" Value='<%# Eval("id") %>' />

                                                                                            </td>

                                                                                        </tr>
                                                                                    </table>
                                                                                </asp:Panel>
                                                                            </td>
                                                                            <td style="width: 50%; vertical-align: top; display: none;">
                                                                                <asp:Panel ID="PnlWrLang" runat="server" Width="100%" BorderColor="White" BorderWidth="1px">
                                                                                    <table width="80%">
                                                                                        <tr>
                                                                                            <td style="text-align: left; color: Blue;">Can Write
                                                                                            </td>

                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:TextBox ID="LitWrittenLanguage" runat="server" ForeColor="Maroon" BackColor="Transparent"
                                                                                                    BorderStyle="None" Width="100%" ReadOnly="True" CssClass="noleftpad"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td style="text-align: left; vertical-align: top; font-size: x-small; color: Red;">
                                                                                                <%--<a href="frmLanguages.aspx?id='<%=WLanguage %>'&status=write" onclick="window.open(this.href,'popupwindow','left=120,top=170,height=400,width=200,scrollbars=no,toolbar=no,location=center,menubar=no'); return false;"
                                                                                                        style="color: Red; font-size: x-small;" tabindex="25">click to add</a>--%>
                                                                                                <a href="javascript:void(0);" onclick="ClickOnWrite('<%# Eval("id") %>')" title="Map" class="btn btn-primary" tabindex="24">Select Language(s)</a>
                                                                                                <asp:HiddenField ID="hddnwrite" runat="server" Value='<%# Eval("id") %>' />

                                                                                            </td>
                                                                                            <td></td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </asp:Panel>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                    <asp:Label ID="lblmessage" runat="server" ForeColor="Red"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Text="Correspondence" Name="CorresPondence">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Education" Text="Education">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Employee" Text="Employment">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Documents" Text="Documents">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="FamilyMembers" Text="Family">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="GroupMember" Text="Group">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>

                            <dxe:TabPage Name="EmployeeCTC" Text="CTC">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="BankDetails" Text="Bank">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Remarks" Text="UDF" Visible="false">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="DPDetails" Text="DP" Visible="false">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>


                            <dxe:TabPage Name="Registration" Text="Registration" Visible="false">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>




                            <dxe:TabPage Name="Subscription" Text="Subscription" Visible="false">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>

                            <dxe:TabPage Name="Language" Text="Language">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="StautoryDocumentDetails" Text="Statutory">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="PFESI" Text="PF/ESI">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="othrdtls" Text="Other Details">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="RESIGNATION" Text="Resignation">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
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
	                                            var Tab4 = page.GetTab(4);
	                                            var Tab5 = page.GetTab(5);
	                                            var Tab6 = page.GetTab(6);
	                                            var Tab7 = page.GetTab(7);
	                                            var Tab8 = page.GetTab(8);
	                                            var Tab9 = page.GetTab(9);
	                                            var Tab10 = page.GetTab(10);
	                                            var Tab11 = page.GetTab(11);
	                                            var Tab12 = page.GetTab(12);
                                                var Tab13 = page.GetTab(13);
                                                 var Tab14 = page.GetTab(14);
                                                 var Tab15 = page.GetTab(15);
                                                var Tab16 = page.GetTab(16);
                                                 var Tab17 = page.GetTab(17);

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
	                                            else if(activeTab == Tab4)
	                                            {
	                                                disp_prompt('tab4');
	                                            }
	                                            else if(activeTab == Tab5)
	                                            {
	                                                disp_prompt('tab5');
	                                            }
	                                            else if(activeTab == Tab6)
	                                            {
	                                                disp_prompt('tab6');
	                                            }
	                                            else if(activeTab == Tab7)
	                                            {
	                                                disp_prompt('tab7');
	                                            }
	                                            else if(activeTab == Tab8)
	                                            {
	                                                disp_prompt('tab8');
	                                            }
	                                            else if(activeTab == Tab9)
	                                            {
	                                                disp_prompt('tab9');
	                                            }
	                                            else if(activeTab == Tab10)
	                                            {
	                                                disp_prompt('tab10');
	                                            }
	                                            else if(activeTab == Tab11)
	                                            {
	                                                disp_prompt('tab11');
	                                            }
	                                            else if(activeTab == Tab12)
	                                            {
	                                                disp_prompt('tab12');
	                                            }
                                                else if(activeTab == Tab13)
	                                            {
	                                                disp_prompt('tab13');
	                                            }
                                                else if(activeTab == Tab14)
	                                            {
	                                                disp_prompt('tab14');
	                                            }
                                                else if(activeTab == Tab15)
	                                            {
	                                                disp_prompt('tab15');
	                                            }
                                             else if(activeTab == Tab16)
	                                            {
	                                                disp_prompt('tab16');
	                                            }
                                            else if(activeTab == Tab17)
	                                            {
	                                                disp_prompt('tab17');
	                                            }
	                                            }"></ClientSideEvents>
                        <ContentStyle>
                            <Border BorderColor="#002D96" BorderStyle="Solid" BorderWidth="1px" />
                        </ContentStyle>
                        <LoadingPanelStyle ImageSpacing="6px">
                        </LoadingPanelStyle>
                    </dxe:ASPxPageControl>
                </td>
            </tr>
            <tr>
                <td style="height: 8px">
                    <table style="width: 100%;">
                        <tr>
                            <td align="left" style="width: 843px">
                                <asp:HiddenField ID="hdReferenceBy" runat="server" />
                                <table style="margin-top: 10px;">
                                    <tr>
                                        <td>
                                            <dxe:ASPxButton ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" ValidationGroup="a"
                                                TabIndex="26" CssClass="btn btn-primary">
                                                <ClientSideEvents Click="function(s,e){
                                                    setvalue()}" />
                                            </dxe:ASPxButton>
                                        </td>
                                        <td>
                                            <button type="button" value="UDF" class="btn btn-primary dxbButton" onclick="OpenUdf()">UDF</button>
                                            <a href="employee.aspx" class="btn btn-danger">Cancel</a>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <style>
        a img {
            border: none;
        }

        ol li {
            list-style: decimal outside;
        }

        div#container {
            width: 780px;
            margin: 0 auto;
            padding: 1em 0;
        }

        div.side-by-side {
            width: 100%;
            margin-bottom: 1em;
        }

            div.side-by-side > div {
                float: left;
                width: 50%;
            }

                div.side-by-side > div > em {
                    margin-bottom: 10px;
                    display: block;
                }

        .clearfix:after {
            content: "\0020";
            display: block;
            height: 0;
            clear: both;
            overflow: hidden;
            visibility: hidden;
        }

        .chosen-container-active.chosen-with-drop .chosen-single div,
        .chosen-container-single .chosen-single div {
            display: none !important;
        }

        .chosen-container-single .chosen-single {
            border-radius: 0 !important;
            background: transparent !important;
        }
    </style>

</asp:Content>

