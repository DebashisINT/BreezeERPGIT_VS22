<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="StatutoryDocumnentDtls.aspx.cs" Inherits="ERP.StatutoryDocumnentDtls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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

        #lstReferedBy_chosen {
            width: 170px !important;
        }

        .dxbButton_PlasticBlue div.dxb {
            padding: 0 !important;
        }
        .padTbl>tbody>tr>td {
            padding-right:30px;
        }
    </style>
    <script language="javascript" type="text/javascript">

        $(document).ready(function () {

            if (cASPxTxtPan.GetText() != "") {
                ctxtNameAsPerPan.SetEnabled(true);
            }
            else {
                ctxtNameAsPerPan.SetEnabled(false);
            }


            if (txtpassport.GetValue() == null || txtpassport.GetValue() == "") {
                $('.pullleftClass').hide();
                document.getElementById('error_validupto').style.display = 'none';
                oVal = document.getElementById("<%=RequiredFieldValidator1.ClientID %>");
                oVal.enabled = false;
            }
            else {
                // $('.pullleftClass').show();
                document.getElementById('error_validupto').style.display = 'inline-block';
                oVal = document.getElementById("<%=RequiredFieldValidator1.ClientID %>");
                oVal.enabled = true;
            }
        });

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
            else if (name == "tab15") {
                //alert(name);
                document.location.href = "PFESIDtls.aspx";
            }
            else if (name == "tab16") {
                //alert(name);
                document.location.href = "OtherDetails.aspx";
            }
            else if (name == "tab17") {
                //alert(name);
                document.location.href = "Resignation.aspx";
            }
        }

        function setvalue() {
            //document.getElementById("txtReferedBy_hidden").value = document.getElementById("lstReferedBy").value;
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
    </script>
    <script>


        function NameBasedOnPAN(s,e)
        {
            if (cASPxTxtPan.GetText() != "")
            {
                ctxtNameAsPerPan.SetEnabled(true);
            }
            else
            {
                ctxtNameAsPerPan.SetEnabled(false);
            }
        }

        function HideReqFldValidtrValidUpto()
        {
           

            if (txtpassport.GetValue() == null || txtpassport.GetValue()=="") {
                $('.pullleftClass').hide();
                document.getElementById('error_validupto').style.display = 'none';
                oVal = document.getElementById("<%=RequiredFieldValidator1.ClientID %>");
                oVal.enabled = false;
            }
            else
            {
               // $('.pullleftClass').show();
                document.getElementById('error_validupto').style.display = 'inline-block';
                oVal = document.getElementById("<%=RequiredFieldValidator1.ClientID %>");
                oVal.enabled =true;
            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-title">
        <h3>Statutory Document Details</h3>
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
                    <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="14" ClientInstanceName="page">
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

                            <dxe:TabPage Text="Stautory" Name="StautoryDocumentDetails">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                        <table  class="padTbl">
                                            
                                                        <tr>
                                                            <td class="Ecoheadtxt" >PAN<span style="color: red"></span>
                                                            </td>
                                                            <td class="Ecoheadtxt relative">
                                                                <dxe:ASPxTextBox ID="ASPxTxtPan" runat="server" Width="170px" TabIndex="2" MaxLength="150" ClientInstanceName="cASPxTxtPan">
                                                                    <ValidationSettings ValidationGroup="a">
                                                                    </ValidationSettings>
                                                                    <ClientSideEvents LostFocus="NameBasedOnPAN" />
                                                                </dxe:ASPxTextBox>
                                                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ASPxTxtPan"
                                                                    Display="Dynamic" ErrorMessage="" SetFocusOnError="True" CssClass="pullleftClass fa fa-exclamation-circle iconRed pos22"
                                                                    ValidationGroup="a"></asp:RequiredFieldValidator>--%>
                                                            </td>
                                                            <td class="Ecoheadtxt" >Passport<span style="color: red"></span>
                                                            </td>
                                                            <td class="Ecoheadtxt" style="text-align: left; position: relative">
                                                                <dxe:ASPxTextBox ID="txtpassport" runat="server" Width="170px" TabIndex="2" MaxLength="150" onkeyup="HideReqFldValidtrValidUpto();" ClientInstanceName="txtpassport">
                                                                    <ValidationSettings ValidationGroup="a">
                                                                    </ValidationSettings>
                                                                </dxe:ASPxTextBox>
                                                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtpassport"
                                                                    Display="Dynamic" ErrorMessage="" SetFocusOnError="True" CssClass="pullleftClass fa fa-exclamation-circle iconRed pos22"
                                                                    ValidationGroup="a"></asp:RequiredFieldValidator>--%>
                                                            </td>
                                                            <td class="Ecoheadtxt" >Valid Upto<span id="error_validupto" style="color: red">*</span>
                                                            </td>
                                                            <td class="Ecoheadtxt relative" style="text-align: left;">
                                                                <dxe:ASPxDateEdit ID="ASPxDateEditValid" runat="server" DateOnError="Today" EditFormat="Custom"
                                                                    TabIndex="11" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy">
                                                                </dxe:ASPxDateEdit>
                                                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ASPxDateEditValid"
                                                                    Display="Dynamic" ErrorMessage="" SetFocusOnError="True" CssClass="pullleftClass fa fa-exclamation-circle iconRed pos22"
                                                                    ValidationGroup="a"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4" style="text-align: right; height: 1px"></td>
                                                            <td colspan="2" style="text-align: right; height: 1px"></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="Ecoheadtxt">EPIC<span style="color: red"></span>
                                                            </td>
                                                            <td class="Ecoheadtxt relative" style="text-align: left; ">
                                                                <dxe:ASPxTextBox ID="txtEpic" runat="server" Width="170px" TabIndex="4" MaxLength="50">
                                                                    <ValidationSettings ValidationGroup="a">
                                                                    </ValidationSettings>
                                                                </dxe:ASPxTextBox>
                                                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtEpic"
                                                                    Display="Dynamic" ErrorMessage="" SetFocusOnError="True" CssClass="pullleftClass fa fa-exclamation-circle iconRed pos22"
                                                                    ValidationGroup="a"></asp:RequiredFieldValidator>--%>
                                                            </td>
                                                            <td class="Ecoheadtxt" >Aadhaar<span style="color: red"></span>
                                                            </td>
                                                            <td class="Ecoheadtxt relative" style="text-align: left;">

                                                                <dxe:ASPxTextBox ID="txtaadhaar" runat="server" Width="170px" TabIndex="4" MaxLength="50">
                                                                    <ValidationSettings ValidationGroup="a">
                                                                    </ValidationSettings>
                                                                </dxe:ASPxTextBox>
                                                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtaadhaar"
                                                                    Display="Dynamic" ErrorMessage="" SetFocusOnError="True" CssClass="pullleftClass fa fa-exclamation-circle iconRed pos22"
                                                                    ValidationGroup="a"></asp:RequiredFieldValidator>--%>


                                                            </td>

                                                         </tr>
                                             <tr>
                                                            <td colspan="4" style="text-align: right; height: 1px"></td>
                                                            <td colspan="2" style="text-align: right; height: 1px"></td>
                                               </tr>
                                            <tr>
                                                 <td class="Ecoheadtxt" >Name as per PAN Card<span style="color: red"></span>
                                                  </td>

                                                   <td class="Ecoheadtxt relative" style="text-align: left;">

                                                                 <dxe:ASPxTextBox ID="txtNameAsPerPan"  ClientInstanceName="ctxtNameAsPerPan" MaxLength="250" HorizontalAlign="Left"
                                                                         runat="server" Width="100%">

                                                                  </dxe:ASPxTextBox>


                                                  </td>
                                                     <td class="Ecoheadtxt" >Deductee Status<span style="color: red"></span>
                                                  </td>

                                                   <td class="Ecoheadtxt relative" style="text-align: left;">

                                                           <dxe:ASPxComboBox ID="cmbDeducteestat" ClientInstanceName="ccmbDeducteestat" runat="server"
                                                                  ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0">
                                                               <Items>
                                                                     <dxe:ListEditItem Text="Select" Value="0" Selected="true" />
                                                                     <dxe:ListEditItem Text="Company" Value="01" />
                                                                     <dxe:ListEditItem Text="Other than Company" Value="02" />
                                       

                                                               </Items>
                                                          </dxe:ASPxComboBox>
                                                            

                                                  </td>

                                                    <td class="Ecoheadtxt" ></td>
                                                   <td class="Ecoheadtxt" style="text-align: left;"></td>

                                            </tr>
                                                      
                                          
                                                       
                                          

                                                   
                                        </table>
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
       <asp:HiddenField runat="server" ID="empcode" /> 
     <asp:HiddenField runat="server" ID="empid" />

    </div>
</asp:Content>
