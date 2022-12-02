<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="OtherDetails.aspx.cs" Inherits="ERP.OMS.Management.Master.OtherDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="../Activities/JS/SearchPopup.js"></script>
    <link href="../Activities/CSS/SearchPopup.css" rel="stylesheet" />
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
    <style>
        .noleftpad {
            padding-left: 0 !important;
            margin-left: 0 !important;
        }

        .pos22 {
            position: absolute;
            right: 9px;
            top: 3px;
        }
         .padTbl > tbody > tr > td {
            padding-right: 30px;
            padding-bottom:4px;
        }

        #lstReferedBy_chosen {
            width: 170px !important;
        }

        .dxbButton_PlasticBlue div.dxb {
            padding: 0 !important;
        }
    </style>
    <script language="javascript" type="text/javascript">


        $(document).ready(function () {
            $('#EmpGroupModel').on('shown.bs.modal', function () {
                //clearPopup();
                $('#txtEmpSearch').focus();
            })
        })

  
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
                document.location.href = "PFESIDtls.aspx";
            }
            else if (name == "tab17") {
                //alert(name);
                document.location.href = "Resignation.aspx";
            }
        }
        function setvalue() {
            // document.getElementById("txtReferedBy_hidden").value = document.getElementById("lstReferedBy").value;
        }
        function alertify(msg) {
            if (msg == "Success") {
                jAlert("Saved Successfully", "Alert", function () {
                    window.location.href = 'Employee.aspx';
                });

            }

            else
                jAlert('Please try again later');
        }



        function UserGroupSelect(type) {
            var modaltitle = '';
            if (type == "unit")
            {
                modaltitle = "Employee Unit Search";
            }
            else if (type == "dept")
            {
                modaltitle = "Employee Department Search";
            }
            else if (type == "desig") {
                modaltitle = "Employee Designation Search";
            }
            else if (type == "grade") {
                modaltitle = "Employee Grade Search";
            }
            $('#EmpGroupModel').find('.modal-title').text(modaltitle);
            $('#EmpGroupModel').modal('show');
            $("#emptype").val(type);
            LoadEmpOthrDtls('');

        }
        function UserGroupKeyDown(s, e, type) {
            if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
                UserGroupSelect(type);
            }
        }

        function btnUserGroupkeydown(e) {
            if (e.code == "Enter" || e.code == "NumpadEnter") {
                if ($("#txtEmpSearch").val() != '') {
                    LoadEmpOthrDtls($("#txtEmpSearch").val());
                }
            }
            else if (e.code == "ArrowDown") {
                if ($("input[UserIndex=0]"))
                    $("input[UserIndex=0]").focus();
            }
        }


        function LoadEmpOthrDtls(SearchKey) {

            var caption = '';
            if ($("#emptype").val() == "unit") {
                caption = "Employee Unit";
            }
            else if ($("#emptype").val() == "dept") {
                caption = "Employee Department";
            }
            else if ($("#emptype").val() == "desig") {
                caption = "Employee Designation";
            }
            else if ($("#emptype").val() == "grade") {
                caption = "Employee Grade";
            }
            var _groupdetails = {};
            _groupdetails.SearchKey = SearchKey;
            _groupdetails.Type = $("#emptype").val();

            var HeaderCaption = [];
            HeaderCaption.push(caption);
            callonServer("../Activities/Services/Master.asmx/GetEmployeeOtherDetails", _groupdetails, "UserTable", HeaderCaption, "UserIndex", "SetUser");
            //e.preventDefault();
            //return false;
        }

        function SetUser(Id,name) {
           // alert(Name);

            if (Id != '' || Id != null) {
                $("#EmpGroupModel").modal('hide');
                if ($("#emptype").val() == 'unit') {
                    $('#Hdnunit').val(Id);
                    ctxtunit.SetText(name);
                }
                else if ($("#emptype").val() == 'dept') {
                    $('#Hdndept').val(Id);
                    ctxtdept.SetText(name);
                }
                else if ($("#emptype").val() == 'desig') {
                    $('#Hdndesig').val(Id);
                    ctxtdesig.SetText(name);
                }
                else if ($("#emptype").val() == 'grade') {
                    $('#Hdngrade').val(Id);
                    ctxtgrade.SetText(name);
                }

            }


        }

        function ValueSelected(e, indexName) {
            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var Id = e.target.parentElement.parentElement.cells[0].innerText;
                var name = e.target.parentElement.parentElement.cells[1].children[0].value;
                if (Id) {
                    SetUser(Id, name);
                }

            }

            else if (e.code == "ArrowDown") {
                thisindex = parseFloat(e.target.getAttribute(indexName));
                thisindex++;
                if (thisindex < 10)
                    $("input[" + indexName + "=" + thisindex + "]").focus();
            }
            else if (e.code == "ArrowUp") {
                thisindex = parseFloat(e.target.getAttribute(indexName));
                thisindex--;
                if (thisindex > -1)
                    $("input[" + indexName + "=" + thisindex + "]").focus();
                else {

                    $('#txtEmpSearch').focus();
                }
            }

        }

       </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-title">
        <h3>Other Details</h3>
        <div class="crossBtn">
            <a href="employee.aspx" id="goBackCrossBtn"><i class="fa fa-times"></i></a>

        </div>
    </div>

    <div class="form_main">
        <table class="TableMain100">
            <tr>
                <td style="text-align: center">
                    <asp:Label ID="lblHeader" runat="server" Font-Bold="True" Font-Size="15px" ForeColor="Navy"
                        Width="819px" Height="18px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="16" ClientInstanceName="page">
                        <TabPages>
                            <dxe:TabPage Text="General" Name="General">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
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
                            <dxe:TabPage Name="Family Members" Text="Family">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Group Member" Text="Group">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>

                            <dxe:TabPage Name="Employee CTC" Text="CTC">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Bank Details" Text="Bank">
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

                            <dxe:TabPage Name="DP Details" Text="DP" Visible="false">
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


                            <dxe:TabPage Name="Subscription" Text="Subscriptions" Visible="false">
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

                            <dxe:TabPage Text="Other Details" Name="othrdtls">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                     
                                                    <table class="padTbl">
                                                        <tr>
                                                            <td class="Ecoheadtxt">Unit<span style="color: red">*</span>
                                                            </td>
                                                            <td class="Ecoheadtxt relative">
                                                                <dxe:ASPxButtonEdit ID="txtunit" ReadOnly="true" Width="170px" MaxLength="150" runat="server" ClientInstanceName="ctxtunit">
                                                                    <ValidationSettings ValidationGroup="a">
                                                                    </ValidationSettings>
                                                                    <Buttons>
                                                                        <dxe:EditButton>
                                                                        </dxe:EditButton>
                                                                    </Buttons>
                                                                    <ClientSideEvents ButtonClick="function(s,e){UserGroupSelect('unit');}" KeyDown="function(s,e){UserGroupKeyDown(s,e,'unit');}" />
                                                                </dxe:ASPxButtonEdit>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtunit"
                                                                    Display="Dynamic" ErrorMessage="" SetFocusOnError="True" CssClass="pullleftClass fa fa-exclamation-circle iconRed pos22"
                                                                    ValidationGroup="a"></asp:RequiredFieldValidator>
                                                            </td>
                                                            <td class="Ecoheadtxt">Department<span style="color: red">*</span>
                                                            </td>
                                                            <td class="Ecoheadtxt relative">
                                                                <dxe:ASPxButtonEdit ID="txtdept" ReadOnly="true" Width="170px" MaxLength="150" runat="server" ClientInstanceName="ctxtdept">
                                                                    <ValidationSettings ValidationGroup="a">
                                                                    </ValidationSettings>
                                                                    <Buttons>
                                                                        <dxe:EditButton>
                                                                        </dxe:EditButton>
                                                                    </Buttons>
                                                                    <ClientSideEvents ButtonClick="function(s,e){UserGroupSelect('dept');}" KeyDown="function(s,e){UserGroupKeyDown(s,e,'dept');}" />
                                                                </dxe:ASPxButtonEdit>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtdept"
                                                                    Display="Dynamic" ErrorMessage="" SetFocusOnError="True" CssClass="pullleftClass fa fa-exclamation-circle iconRed pos22"
                                                                    ValidationGroup="a"></asp:RequiredFieldValidator>
                                                            </td>
                                                            <td class="Ecoheadtxt">Designation<span style="color: red">*</span>
                                                            </td>
                                                            <td class="Ecoheadtxt relative">
                                                                <dxe:ASPxButtonEdit ID="txtdesig" ReadOnly="true" Width="170px" MaxLength="150" runat="server" ClientInstanceName="ctxtdesig">
                                                                    <ValidationSettings ValidationGroup="a">
                                                                    </ValidationSettings>
                                                                    <Buttons>
                                                                        <dxe:EditButton>
                                                                        </dxe:EditButton>
                                                                    </Buttons>
                                                                    <ClientSideEvents ButtonClick="function(s,e){UserGroupSelect('desig');}" KeyDown="function(s,e){UserGroupKeyDown(s,e,'desig');}" />
                                                                </dxe:ASPxButtonEdit>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtdesig"
                                                                    Display="Dynamic" ErrorMessage="" SetFocusOnError="True" CssClass="pullleftClass fa fa-exclamation-circle iconRed pos22"
                                                                    ValidationGroup="a"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4" style="text-align: right; height: 1px"></td>
                                                            <td colspan="2" style="text-align: right; height: 1px"></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="Ecoheadtxt">Grade<span style="color: red">*</span>
                                                            </td>
                                                            <td class="Ecoheadtxt relative">
                                                                <dxe:ASPxButtonEdit ID="txtgrade" ReadOnly="true" Width="170px" MaxLength="150" runat="server" ClientInstanceName="ctxtgrade">
                                                                    <ValidationSettings ValidationGroup="a">
                                                                    </ValidationSettings>
                                                                    <Buttons>
                                                                        <dxe:EditButton>
                                                                        </dxe:EditButton>
                                                                    </Buttons>
                                                                    <ClientSideEvents ButtonClick="function(s,e){UserGroupSelect('grade');}" KeyDown="function(s,e){UserGroupKeyDown(s,e,'grade');}" />
                                                                </dxe:ASPxButtonEdit>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtgrade"
                                                                    Display="Dynamic" ErrorMessage="" SetFocusOnError="True" CssClass="pullleftClass fa fa-exclamation-circle iconRed pos22"
                                                                    ValidationGroup="a"></asp:RequiredFieldValidator>
                                                            </td>
                                                            <td class="Ecoheadtxt">Joining Date</td>
                                                            <td class="Ecoheadtxt relative">
                                                                <dxe:ASPxDateEdit ID="ASPxDateEditJoining" runat="server" DateOnError="Today" EditFormat="Custom"
                                                                    TabIndex="11" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy">
                                                                </dxe:ASPxDateEdit>
                                                            </td>
                                                            <td class="Ecoheadtxt">Leaving Date</td>
                                                            <td class="Ecoheadtxt relative">
                                                                <dxe:ASPxDateEdit ID="ASPxDateEditLeaving" runat="server" DateOnError="Today" EditFormat="Custom"
                                                                    TabIndex="11" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy">
                                                                </dxe:ASPxDateEdit>
                                                            </td>
                                                        </tr>

                                                    </table>
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
                        <TabStyle Font-Size="12px">
                        </TabStyle>
                    </dxe:ASPxPageControl>
                </td>
            </tr>
            <tr>
                <td style="height: 8px">
                    <table style="width: 100%;">
                        <tr>
                            <td align="left" style="width: 843px">
                                <table style="margin-top: 10px;">
                                    <tr>
                                        <td>
                                            <dxe:ASPxButton ID="btnSave" runat="server" Text="Save" ValidationGroup="a"
                                                TabIndex="26" CssClass="btn btn-primary" OnClick="btnSave_Click">
                                                <ClientSideEvents Click="function(s,e){
                                                    setvalue()}" />
                                            </dxe:ASPxButton>
                                        </td>
                                        <td>

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

    <!--Unit Modal -->
    <div class="modal fade" id="EmpGroupModel" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">User Group Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="btnUserGroupkeydown(event)" id="txtEmpSearch" autofocus width="100%" autocomplete="off" placeholder="Search By Name" />

                    <div id="UserTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th>Name</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>

        </div>
    </div>





    <asp:HiddenField runat="server" ID="emptype" />
    <asp:HiddenField runat="server" ID="empcode" />
    <asp:HiddenField runat="server" ID="empid" />
    <asp:HiddenField runat="server" ID="Hdnunit" />
    <asp:HiddenField runat="server" ID="Hdndept" />
    <asp:HiddenField runat="server" ID="Hdndesig" />
    <asp:HiddenField runat="server" ID="Hdngrade" />
</asp:Content>
