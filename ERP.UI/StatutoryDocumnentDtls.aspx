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
            width:100% !important;
        }
        .chosen-choices {
            width:100% !important;
        }
        #lstReferedBy {
            width:400px;
        }
        #lstReferedBy {
            display:none !important;
            
        }
        
        .dxtcLite_PlasticBlue > .dxtc-content {
            overflow:visible !important
        }
        #lstReferedBy_chosen{
            width:39% !important;
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
                top: 10px;
        }
        #lstReferedBy_chosen {
            width:170px !important;
        }
        .dxbButton_PlasticBlue  div.dxb {
            padding:0 !important;
        }
    </style>
    <script language="javascript" type="text/javascript">
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

                            <dxe:TabPage Text="Stautory Document Details" Name="StautoryDocumentDetails">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <table style="width: 100%; z-index: 101">
                                                        <tr>
                                                            <td class="Ecoheadtxt" style="width: 151px">Pan
                                                            </td>
                                                            <td class="Ecoheadtxt" style="text-align: left; width: 214px;">
                                                                <dxe:ASPxTextBox ID="ASPxTxtPan" runat="server" Width="170px" TabIndex="2" MaxLength="150">
                                                                    <ValidationSettings ValidationGroup="a">
                                                                    </ValidationSettings>
                                                                </dxe:ASPxTextBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ASPxTxtPan"
                                                                    Display="Dynamic" ErrorMessage="" SetFocusOnError="True" CssClass="pullleftClass fa fa-exclamation-circle iconRed pos22"
                                                                    ValidationGroup="a"></asp:RequiredFieldValidator>
                                                            </td>
                                                            <td class="Ecoheadtxt" style="width: 100px">Passport<span style="color: red">*</span>
                                                            </td>
                                                            <td class="Ecoheadtxt" style="text-align: left; width: 197px; position: relative">
                                                                <dxe:ASPxTextBox ID="txtpassport" runat="server" Width="170px" TabIndex="2" MaxLength="150">
                                                                    <ValidationSettings ValidationGroup="a">
                                                                    </ValidationSettings>
                                                                </dxe:ASPxTextBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtpassport"
                                                                    Display="Dynamic" ErrorMessage="" SetFocusOnError="True" CssClass="pullleftClass fa fa-exclamation-circle iconRed pos22"
                                                                    ValidationGroup="a"></asp:RequiredFieldValidator>
                                                            </td>
                                                            <td class="Ecoheadtxt" style="width: 137px">Valid Upto
                                                            </td>
                                                            <td class="Ecoheadtxt" style="text-align: left; width: 214px;">
                                                                <dxe:ASPxDateEdit ID="ASPxDateEditValid" runat="server" DateOnError="Today" EditFormat="Custom"
                                                                    TabIndex="11">
                                                                </dxe:ASPxDateEdit>
                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td class="Ecoheadtxt" style="width: 151px">Epic
                                                            </td>
                                                            <td class="Ecoheadtxt" style="text-align: left; width: 214px;">
                                                                <dxe:ASPxTextBox ID="txtEpic" runat="server" Width="170px" TabIndex="4" MaxLength="50">
                                                                    <ValidationSettings ValidationGroup="a">
                                                                    </ValidationSettings>
                                                                </dxe:ASPxTextBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtEpic"
                                                                    Display="Dynamic" ErrorMessage="" SetFocusOnError="True" CssClass="pullleftClass fa fa-exclamation-circle iconRed pos22"
                                                                    ValidationGroup="a"></asp:RequiredFieldValidator>
                                                            </td>
                                                            <td class="Ecoheadtxt" style="width: 100px">Aadhaar
                                                            </td>
                                                            <td class="Ecoheadtxt" style="text-align: left; width: 197px;">

                                                                <dxe:ASPxTextBox ID="txtaadhaar" runat="server" Width="170px" TabIndex="4" MaxLength="50">
                                                                    <ValidationSettings ValidationGroup="a">
                                                                    </ValidationSettings>
                                                                </dxe:ASPxTextBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtaadhaar"
                                                                    Display="Dynamic" ErrorMessage="" SetFocusOnError="True" CssClass="pullleftClass fa fa-exclamation-circle iconRed pos22"
                                                                    ValidationGroup="a"></asp:RequiredFieldValidator>


                                                            </td>
                                                            <td class="Ecoheadtxt" style="width: 137px"></td>
                                                            <td class="Ecoheadtxt" style="text-align: left; width: 214px;"></td>
                                                        </tr>
                                                        
                                                    </table>
                                                </td>
                                            </tr>

                                        </table>
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
                <td></td>
            </tr>
        </table>
        <asp:SqlDataSource ID="sqleducation" runat="server" ConnectionString="<%$ ConnectionStrings:crmConnectionString %>"
            DeleteCommand="INSERT INTO tbl_master_educationProfessional_Log(edu_id, edu_internalId, edu_degree, edu_instName, edu_country, edu_state, edu_city, edu_courseFrom, edu_courseuntil, edu_courseResult, edu_percentage, edu_grade, edu_month_year, createuser, createdate, lastmodifyuser, lastmodifydate, LogModifyDate, LogModifyUser, LogStatus) SELECT *,getdate(),@User,'D' FROM tbl_master_educationProfessional WHERE [edu_id] = @edu_id DELETE FROM [tbl_master_educationProfessional] WHERE [edu_id] = @edu_id"
            InsertCommand="EmployeeEducationInsert" InsertCommandType="StoredProcedure"
            SelectCommand="select ep.edu_id as edu_id,ep.edu_internalId as edu_internalId,ep.edu_instName as edu_instName,ep.edu_courseFrom as edu_courseFrom,case edu_degree when '' then 'N/A' else(select edu_education from tbl_master_education where ep.edu_degree=edu_id) end as edu_degree1,ep.edu_courseuntil as edu_courseuntil,ep.edu_courseResult as edu_courseResult,ep.edu_percentage as edu_percentage,ep.edu_grade as edu_grade,ep.edu_month_year as edu_month_year,ep.createuser as createuser,ep.createdate as createdate,ep.lastmodifyuser as lastmodifyuser,ep.lastmodifydate as lastmodifydate,case edu_state when  '0' then 'N/A' else (select state from tbl_master_state where ep.edu_state=id) end as edu_state1,case edu_city when null then '0' else(select city_name from tbl_master_city where ep.edu_city=city_id) end as edu_city1,case edu_country when '0' then 'N/A' else(select cou_country from tbl_master_country where ep.edu_country=cou_id) end as edu_country1,convert(varchar(11),ep.edu_courseFrom,113) as edu_courseFrom1,convert(varchar(11),ep.edu_courseuntil,113) as edu_courseuntil1,ep.edu_country as edu_country,ep.edu_state as edu_state,ep.edu_city as edu_city,ep.edu_degree as edu_degree,Right(Convert(VARCHAR(11),ep.edu_month_year,113),8) as  edu_month_year1 from tbl_master_educationProfessional ep where ep.edu_internalId=@edu_internalId"
            UpdateCommand="INSERT INTO tbl_master_educationProfessional_Log(edu_id, edu_internalId, edu_degree, edu_instName, edu_country, edu_state, edu_city, edu_courseFrom, edu_courseuntil, edu_courseResult, edu_percentage, edu_grade, edu_month_year, createuser, createdate, lastmodifyuser, lastmodifydate, LogModifyDate, LogModifyUser, LogStatus) SELECT *,getdate(),@lastmodifyuser,'M' FROM tbl_master_educationProfessional WHERE [edu_id] = @edu_id UPDATE [tbl_master_educationProfessional] SET [edu_degree] = @edu_degree, [edu_instName] = @edu_instName, [edu_country] = @edu_country, [edu_state] = @edu_state, [edu_city] = @edu_city, [edu_courseFrom] = @edu_courseFrom, [edu_courseuntil] = @edu_courseuntil, [edu_courseResult] = @edu_courseResult, [edu_percentage] = @edu_percentage, [edu_grade] = @edu_grade, [edu_month_year] = @edu_month_year,  [lastmodifyuser] = @lastmodifyuser, [lastmodifydate] = getdate() WHERE [edu_id] = @edu_id">
            <SelectParameters>
                <asp:SessionParameter Name="edu_internalId" SessionField="KeyVal_InternalID" Type="String" />
            </SelectParameters>
            <DeleteParameters>
                <asp:Parameter Name="edu_id" Type="Int32" />
                <asp:SessionParameter Name="User" SessionField="userid" Type="Int32" />
            </DeleteParameters>
            <InsertParameters>
                <asp:SessionParameter Name="edu_internalId" SessionField="KeyVal_InternalID" Type="String" />
                <asp:Parameter Name="edu_degree" Type="String" />
                <asp:Parameter Name="edu_instName" Type="String" />
                <asp:Parameter Name="edu_country" Type="Int32" />
                <asp:Parameter Name="edu_state" Type="Int32" />
                <asp:Parameter Name="edu_city" Type="Int32" />
                <asp:Parameter Type="datetime" Name="edu_courseFrom" />
                <asp:Parameter Type="datetime" Name="edu_courseuntil" />
                <asp:Parameter Name="edu_courseResult" Type="String" />
                <asp:Parameter Name="edu_percentage" Type="string" />
                <asp:Parameter Name="edu_grade" Type="String" />
                <asp:Parameter Name="edu_month_year" Type="String" />
                <asp:SessionParameter Name="createuser" SessionField="userid" Type="Int32" />
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="edu_degree" Type="String" />
                <asp:Parameter Name="edu_instName" Type="String" />
                <asp:Parameter Name="edu_country" Type="Int32" />
                <asp:Parameter Name="edu_state" Type="Int32" />
                <asp:Parameter Name="edu_city" Type="Int32" />
                <asp:Parameter Type="datetime" Name="edu_courseFrom" />
                <asp:Parameter Type="datetime" Name="edu_courseuntil" />
                <asp:Parameter Name="edu_courseResult" Type="String" />
                <asp:Parameter Name="edu_percentage" Type="string" />
                <asp:Parameter Name="edu_grade" Type="String" />
                <asp:Parameter Name="edu_month_year" Type="String" />
                <asp:SessionParameter Name="lastmodifyuser" SessionField="userid" Type="Decimal" />
                <asp:Parameter Type="datetime" Name="lastmodifydate" />
                <asp:Parameter Name="edu_id" Type="Int32" />
            </UpdateParameters>
        </asp:SqlDataSource>

    </div>
</asp:Content>
