<%@ Page Title="Resignation" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="Resignation.aspx.cs" Inherits="ERP.OMS.Management.Master.Resignation" %>

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
        .dxgvControl_PlasticBlue a {
            color: #fff !important;
        }

        #gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EC, #gridHistory_DXPEForm_efnew_DXEFL_DXEditor11_EC,
        #gridHistory_DXPEForm_efnew_DXEFL_DXEditor6_EC {
            position: absolute;
        }

        #gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EC {
            padding-right: 10px;
        }
        #gridHistory_DXPEForm_tcefnew {
            overflow:visible !important;
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
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        function setvalue()
        {
            
        }
    </script>
      <script>
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
                //alert(name);
                document.location.href = "Resignation.aspx";
            }
        }
  </script>
     <div class="panel-title">
        <h3>Resignation</h3>
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

        </table>

      </div>
     <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="17" ClientInstanceName="page"
                        Width="100%">

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
                              
                                     <%--   <table width="100%">
                                            <tr>
                                                <td>
                                                    <table style="width: 100%; z-index: 101">
                                                        <tr>
                                                             <td class="Ecoheadtxt" style="width: 137px">RESIGNATION SUBMITTED ON:
                                                            </td>
                                                            <td class="Ecoheadtxt" style="text-align: left; width: 214px;">
                                                                <dxe:ASPxDateEdit ID="txtresignationsubmitdate" runat="server" DateOnError="Today" EditFormat="Custom"
                                                                    TabIndex="12">
                                                                </dxe:ASPxDateEdit>
                                                                </td>
                                                             <td class="Ecoheadtxt" style="width: 137px">NOTICE / EXPECTED LEAVING DATE:
                                                            </td>
                                                            <td class="Ecoheadtxt" style="text-align: left; width: 214px;">
                                                                <dxe:ASPxDateEdit ID="txtexpectedleavingdate" runat="server" DateOnError="Today" EditFormat="Custom"
                                                                    TabIndex="12">
                                                                </dxe:ASPxDateEdit>
                                                                </td>
                                                             <td class="Ecoheadtxt" style="width: 137px">REMARKS:
                                                            </td>
                                                            <td class="Ecoheadtxt" style="text-align: left; width: 214px;">
                                                                <dxe:ASPxTextBox ID="txtremarks" runat="server" Width="170px" TabIndex="2" MaxLength="150">
                                         
                                                                </dxe:ASPxTextBox>
                                                                </td>
                                                            </tr>
                                                            </table>
                                                    </td>
                                                </tr>
                                            </table>--%>
                                      <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                        <table style="width: 100%">
                                            <tr>
                                                <td align="left">
                                                    <% if (rights.CanAdd)
                                                       { %>
                                                    <a href="javascript:void(0);" onclick="grid.AddNewRow();" class="btn btn-primary"><span>Add New</span> </a><%} %>
                                                                              
                                                </td>
                                            </tr>
                                        </table>
                                        <div style="display: none">
                                            <div class="row">
                                                <div class="col-md-2">
                                                    <label>Date Of Joining</label>
                                                    <dxe:ASPxDateEdit Width="100%" ID="cmbDOJ" runat="server" EditFormat="Custom" UseMaskBehavior="True" EditFormatString="dd-MM-yyyy">
                                                    </dxe:ASPxDateEdit>
                                                </div>
                                                <div class="col-md-2">
                                                    <label>Date of leaving</label>
                                                    <dxe:ASPxDateEdit Width="100%" ID="cmbDOL" ClientInstanceName="date" runat="server"
                                                        EditFormat="Custom" UseMaskBehavior="True" EditFormatString="dd-MM-yyyy">
                                                        <ClientSideEvents DateChanged="function(s, e) {
                                                                        var date=s.GetDate();
	                                                                    ShowReplacement(date.getMonth()+1 + '/'+date.getDate()+'/'+date.getYear());
                                                                    }" />
                                                    </dxe:ASPxDateEdit>
                                                </div>
                                                <div class="col-md-2">
                                                    <label>Reason of Leaving</label>
                                                    <asp:TextBox ID="txtROLeaving" runat="server" Width="100%" TextMode="MultiLine"></asp:TextBox>
                                                </div>
                                                <div class="col-md-2">
                                                    <label>Next Employer Name</label>
                                                    <asp:TextBox Width="100%" ID="txtNextEmployeerName" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="col-md-2">
                                                    <label>Next Employer Addres /Phone</label>
                                                    <asp:TextBox ID="txtNextEmployeerAddress" runat="server" Width="100%" TextMode="MultiLine"></asp:TextBox>
                                                </div>
                                                <div class="col-md-2">
                                                    <label>DIN</label>
                                                    <asp:TextBox ID="txtDIN" runat="server" Width="100%"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-2">
                                                    <div class="checkbox">
                                                        <label>
                                                            <asp:CheckBox ID="chkReplacement" runat="server" />
                                                            Replacement
                                                        </label>
                                                    </div>
                                                </div>
                                                <div class="col-md-2" style="vertical-align: top; display: none" id="tdReplacement">
                                                    <asp:TextBox ID="txtReplacement" runat="server" Width="100%"></asp:TextBox>
                                                    <asp:HiddenField ID="txtReplacement_hidden" runat="server" />
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-6">
                                                    <dxe:ASPxButton ID="ASPxButton1" CssClass="btn btn-primary" runat="server" Text="Save" OnClick="btnSave_Click">
                                                    </dxe:ASPxButton>
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:TextBox ID="emp_id" runat="server" Visible="False"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <table width="100%">
                                            <tr>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <dxe:ASPxGridView ID="gridHistory" runat="server" Width="100%" ClientInstanceName="grid"
                                                        DataSourceID="sqlHistory" AutoGenerateColumns="False" KeyFieldName="emp_resignation_id" Font-Size="12px" OnStartRowEditing="gridHistory_StartRowEditing" OnCommandButtonInitialize="gridHistory_CommandButtonInitialize">
                                                        <Columns>
                                                           <%-- <dxe:GridViewDataTextColumn FieldName="emp_id" ReadOnly="True" VisibleIndex="0"
                                                                Visible="False">
                                                                <EditFormSettings Visible="False"></EditFormSettings>
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataTextColumn FieldName="emp_InternalId" VisibleIndex="1" Visible="False">
                                                                <EditFormSettings Visible="False" />
                                                            </dxe:GridViewDataTextColumn>--%>


                                                            <%--<dxe:GridViewDataTextColumn FieldName="emp_employerName" VisibleIndex="4" Caption="Name" PropertiesTextEdit-MaxLength="50">
                                                                <PropertiesTextEdit MaxLength="50">

                                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="" ErrorTextPosition="right"
                                                                        SetFocusOnError="True" ValidateOnLeave="False">


                                                                        <RequiredField ErrorText="Mandatory" IsRequired="true" />


                                                                    </ValidationSettings>

                                                                </PropertiesTextEdit>
                                                                <EditFormSettings Caption="Employer Name" Visible="True" VisibleIndex="0" />
                                                                <EditFormCaptionStyle Wrap="False">
                                                                </EditFormCaptionStyle>
                                                            </dxe:GridViewDataTextColumn>--%>
                                                           <%-- <dxe:GridViewDataTextColumn FieldName="emp_employerAddress" VisibleIndex="5" Caption="Address"
                                                                Visible="False" PropertiesTextEdit-MaxLength="200">
                                                                <EditFormSettings Caption="Employer Address" Visible="True" VisibleIndex="1" />
                                                                <EditFormCaptionStyle Wrap="False">
                                                                </EditFormCaptionStyle>
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataTextColumn FieldName="emp_employerPhone" VisibleIndex="6" Caption="Phone"
                                                                Visible="False" PropertiesTextEdit-MaxLength="50">

                                                                <EditFormSettings Caption="Employer Phone" Visible="True" VisibleIndex="2" />
                                                                <EditFormCaptionStyle Wrap="False">
                                                                </EditFormCaptionStyle>
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataTextColumn FieldName="emp_employerFax" VisibleIndex="7" Caption="Fax"
                                                                Visible="False" PropertiesTextEdit-MaxLength="50">

                                                                <EditFormSettings Caption="Employer Fax" Visible="True" VisibleIndex="3" />
                                                                <EditFormCaptionStyle Wrap="False">
                                                                </EditFormCaptionStyle>
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataTextColumn FieldName="emp_employerEmail" VisibleIndex="8" Caption="Email"
                                                                Visible="False" PropertiesTextEdit-MaxLength="50">

                                                                <EditFormSettings Caption="Employer Email" Visible="True" VisibleIndex="4" />
                                                                <EditFormCaptionStyle Wrap="False">
                                                                </EditFormCaptionStyle>
                                                                <PropertiesTextEdit>

                                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="" ErrorTextPosition="right" SetFocusOnError="True" ValidateOnLeave="False">
                                                                   
                                                                        <RegularExpression ErrorText="Invalid Email entered." ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />

                                                                    </ValidationSettings>

                                                                </PropertiesTextEdit>
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataTextColumn FieldName="emp_employerPan" VisibleIndex="9" Caption="PAN No"
                                                                Visible="False" PropertiesTextEdit-MaxLength="50">
                                                                <EditFormSettings Caption="PAN No" Visible="True" VisibleIndex="5" />
                                                                <EditFormCaptionStyle Wrap="False">
                                                                </EditFormCaptionStyle>
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataComboBoxColumn Caption="Job Responsibility" FieldName="emp_jobResponsibility"
                                                                VisibleIndex="10">
                                                                <PropertiesComboBox DataSourceID="Sqljobresponsibility" TextField="job_responsibility"
                                                                    ValueField="job_id" ValueType="System.String" EnableIncrementalFiltering="True"
                                                                    EnableSynchronization="False">
                                                                </PropertiesComboBox>
                                                                <EditFormSettings Visible="True" VisibleIndex="8" />
                                                                <EditFormCaptionStyle Wrap="False">
                                                                </EditFormCaptionStyle>
                                                            </dxe:GridViewDataComboBoxColumn>
                                                            <dxe:GridViewDataComboBoxColumn Caption="Designation" FieldName="emp_designation"
                                                                VisibleIndex="11" Visible="False">
                                                                <PropertiesComboBox DataSourceID="Sqldesignation" ValueField="deg_id" TextField="deg_designation"
                                                                    ValueType="System.String" EnableIncrementalFiltering="True" EnableSynchronization="False">
                                                                </PropertiesComboBox>
                                                                <EditFormSettings Visible="True" VisibleIndex="9" />
                                                                <EditFormCaptionStyle Wrap="False">
                                                                </EditFormCaptionStyle>
                                                            </dxe:GridViewDataComboBoxColumn>

                                                            <dxe:GridViewDataComboBoxColumn Caption="Department" FieldName="emp_department"
                                                                VisibleIndex="12">
                                                                <PropertiesComboBox DataSourceID="sqlDept" TextField="cost_description"
                                                                    ValueField="cost_id" ValueType="System.String" EnableIncrementalFiltering="True"
                                                                    EnableSynchronization="False">
                                                                </PropertiesComboBox>
                                                                <EditFormSettings Visible="True" VisibleIndex="10" />
                                                                <EditFormCaptionStyle Wrap="False">
                                                                </EditFormCaptionStyle>
                                                            </dxe:GridViewDataComboBoxColumn>

                                                            <dxe:GridViewDataTextColumn FieldName="emp_ctc" VisibleIndex="13" Caption="CTC" PropertiesTextEdit-MaxLength="10">
                                                                <EditFormSettings Caption="Current CTC" Visible="True" VisibleIndex="11" />
                                                                <EditFormCaptionStyle Wrap="False">
                                                                </EditFormCaptionStyle>
                                                                <PropertiesTextEdit>

                                                                    <ValidationSettings SetFocusOnError="True" ErrorTextPosition="right" ErrorDisplayMode="ImageWithTooltip">

                                                                        <RegularExpression ErrorText="Please Enter Number" ValidationExpression="[0-9]+" />

                                                                    </ValidationSettings>

                                                                </PropertiesTextEdit>
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataTextColumn FieldName="emp_taxIncome" VisibleIndex="17" Caption="TaxIncome"
                                                                Visible="False" PropertiesTextEdit-MaxLength="50">
                                                                <EditFormSettings Caption="Taxable Income" Visible="True" VisibleIndex="12" />
                                                                <EditFormCaptionStyle Wrap="False">
                                                                </EditFormCaptionStyle>
                                                                <PropertiesTextEdit>

                                                                    <ValidationSettings SetFocusOnError="True" ErrorTextPosition="Bottom" ErrorDisplayMode="ImageWithText">

                                                                        <RegularExpression ErrorText="Please Enter Number" ValidationExpression="[0-9]+" />

                                                                    </ValidationSettings>

                                                                </PropertiesTextEdit>
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataTextColumn FieldName="emp_tds" VisibleIndex="18" Caption="TDS"
                                                                Visible="False" PropertiesTextEdit-MaxLength="50">
                                                                <EditFormSettings Caption="TDS" Visible="True" VisibleIndex="13" />
                                                                <EditFormCaptionStyle Wrap="False">
                                                                </EditFormCaptionStyle>
                                                                <PropertiesTextEdit>

                                                                    <ValidationSettings SetFocusOnError="True" ErrorTextPosition="Bottom" ErrorDisplayMode="ImageWithText">

                                                                        <RegularExpression ErrorText="Please Enter Number" ValidationExpression="[0-9]+" />

                                                                    </ValidationSettings>

                                                                </PropertiesTextEdit>
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataTextColumn FieldName="CreateUser" VisibleIndex="19" Visible="False">
                                                                <EditFormSettings Visible="False" />
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataDateColumn FieldName="CreateDate" VisibleIndex="20" Visible="False">
                                                                <EditFormSettings Visible="False" />
                                                            </dxe:GridViewDataDateColumn>
                                                            <dxe:GridViewDataTextColumn FieldName="LastModifyDate" VisibleIndex="21" Visible="False">
                                                                <EditFormSettings Visible="False" />
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataDateColumn FieldName="LastModifyUser" VisibleIndex="22" Visible="False">
                                                                <EditFormSettings Visible="False" />
                                                            </dxe:GridViewDataDateColumn>
                                                            <dxe:GridViewCommandColumn Width="10%" VisibleIndex="14" ShowEditButton="true" ShowDeleteButton="true" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">

                                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                                                <CellStyle HorizontalAlign="Center"></CellStyle>

                                                                <HeaderTemplate>
                                                                    Actions
                                                                       
                                                                </HeaderTemplate>
                                                            </dxe:GridViewCommandColumn>--%>
                                                          <%--  <dxe:GridViewDataTextColumn Caption="From" FieldName="emp_employerFrm1" ShowInCustomizationForm="True" VisibleIndex="2">
                                                                <EditFormSettings Visible="False" />
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataTextColumn Caption="To" FieldName="emp_employerTo1" ShowInCustomizationForm="True" VisibleIndex="3">
                                                                <EditFormSettings Visible="False" />
                                                            </dxe:GridViewDataTextColumn>--%>




                                                             <dxe:GridViewDataTextColumn FieldName="emp_resignation_id" ReadOnly="True" VisibleIndex="0"
                                                                Visible="False">
                                                                <EditFormSettings Visible="False"></EditFormSettings>
                                                            </dxe:GridViewDataTextColumn>
                                                           <%-- <dxe:GridViewDataTextColumn FieldName="emp_InternalId" VisibleIndex="1" Visible="False">
                                                                <EditFormSettings Visible="False" />
                                                            </dxe:GridViewDataTextColumn>--%>




                                                               <dxe:GridViewDataTextColumn FieldName="Remarks" VisibleIndex="3" Caption="Remarks" PropertiesTextEdit-MaxLength="3">
                                                                <PropertiesTextEdit MaxLength="50">

                                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="" ErrorTextPosition="right"
                                                                        SetFocusOnError="True" ValidateOnLeave="False">


                                                                        <RequiredField ErrorText="Mandatory" IsRequired="true" />


                                                                    </ValidationSettings>

                                                                </PropertiesTextEdit>
                                                                   
                                                                <EditFormSettings Caption="Remarks" Visible="True" VisibleIndex="3" />
                                                                <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right" VerticalAlign="Top">
                                                                </EditFormCaptionStyle>
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataDateColumn Caption="Resignation Submitted On" FieldName="emp_dateofresignation" ShowInCustomizationForm="False" Visible="True" VisibleIndex="1">
                                                                <PropertiesDateEdit DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="true">
                                                                
                                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="" ErrorTextPosition="right"
                                                                        SetFocusOnError="True" ValidateOnLeave="False">


                                                                        <RequiredField ErrorText="Mandatory" IsRequired="true" />


                                                                    </ValidationSettings>
                                                                </PropertiesDateEdit>
                                                                <EditFormSettings Visible="True" VisibleIndex="2" />
                                                                <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right" VerticalAlign="Top">
                                                                </EditFormCaptionStyle>
                                                               
                                                            </dxe:GridViewDataDateColumn>
                                                            <dxe:GridViewDataDateColumn Caption=" Notice / Expected Leaving Date:" FieldName="emp_dateofleaving" ShowInCustomizationForm="False" Visible="True" VisibleIndex="2">
                                                                <PropertiesDateEdit DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="true">
                                                                    
                                                                
                                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="" ErrorTextPosition="right"
                                                                        SetFocusOnError="True" ValidateOnLeave="False">


                                                                        <RequiredField ErrorText="Mandatory" IsRequired="true" />


                                                                    </ValidationSettings>
                                                   
                                                                </PropertiesDateEdit>
                                                                <EditFormSettings Visible="True" VisibleIndex="2" />
                                                                <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right" VerticalAlign="Top">
                                                                </EditFormCaptionStyle>
                                            
                                                            </dxe:GridViewDataDateColumn>
                                                         
                                                             <dxe:GridViewCommandColumn Width="10%" VisibleIndex="14" ShowEditButton="true" ShowDeleteButton="true" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">

                                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                                                <CellStyle HorizontalAlign="Center"></CellStyle>

                                                                <HeaderTemplate>
                                                                    Actions
                                                                       
                                                                </HeaderTemplate>
                                                            </dxe:GridViewCommandColumn>
                                                        </Columns>
                                                        <SettingsCommandButton>
                                                            <EditButton Image-Url="../../../assests/images/Edit.png" ButtonType="Image" Image-AlternateText="Edit" Styles-Style-CssClass="pad">
                                                                <Image AlternateText="Edit" Url="../../../assests/images/Edit.png"></Image>

                                                                <Styles>
                                                                    <Style CssClass="pad"></Style>
                                                                </Styles>
                                                            </EditButton>
                                                            <DeleteButton Image-Url="../../../assests/images/Delete.png" ButtonType="Image" Image-AlternateText="Delete" Styles-Style-CssClass="pad">
                                                                <Image AlternateText="Delete" Url="../../../assests/images/Delete.png"></Image>

                                                                <Styles>
                                                                    <Style CssClass="pad"></Style>
                                                                </Styles>
                                                            </DeleteButton>

                                                            <UpdateButton Text="Save" ButtonType="Button" Styles-Style-CssClass="btn btn-primary pull-left">
                                                                <Styles>
                                                                    <Style CssClass="btn btn-primary pull-left"></Style>
                                                                </Styles>
                                                            </UpdateButton>
                                                            <CancelButton Text="Cancel" ButtonType="Button" Styles-Style-CssClass="btn btn-danger pull-left">
                                                                <Styles>
                                                                    <Style CssClass="btn btn-danger pull-left"></Style>
                                                                </Styles>
                                                            </CancelButton>
                                                        </SettingsCommandButton>
                                                        <StylesEditors>
                                                            <ProgressBar Height="25px">
                                                            </ProgressBar>
                                                        </StylesEditors>
                                                        <Settings ShowFooter="True" ShowStatusBar="Visible" ShowTitlePanel="True" ShowGroupButtons="False" />
                                                        <SettingsEditing Mode="PopupEditForm" PopupEditFormHorizontalAlign="WindowCenter"
                                                            PopupEditFormModal="True" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="750px" />
                                                        <Styles>
                                                            <LoadingPanel ImageSpacing="10px">
                                                            </LoadingPanel>
                                                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                            </Header>
                                                        </Styles>
                                                        <SettingsText PopupEditFormCaption="Add Employee Resignation Details" ConfirmDelete="Confirm Delete?"
                                                            Title="Add Address" />
                                                        <SettingsPager NumericButtonCount="20" PageSize="20">
                                                        </SettingsPager>
                                                        <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True" />
                                                        <Templates>
                                                            <EditForm>
                                                                <table style="width: 100%">
                                                                    <tr>
                                                                        <td style="width: 100%">
                                                                            <controls>
                                                               <dxe:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormEditors" ColumnID="" ID="Editors">
                                                               </dxe:ASPxGridViewTemplateReplacement>                                                           
                                                             </controls>
                                                                            <div style="text-align: right; padding: 2px 2px 2px 115px">
                                                                                <dxe:ASPxGridViewTemplateReplacement ID="UpdateButton" ReplacementType="EditFormUpdateButton"
                                                                                    runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                                                <dxe:ASPxGridViewTemplateReplacement ID="CancelButton" ReplacementType="EditFormCancelButton"
                                                                                    runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </EditForm>
                                                            <TitlePanel>
                                                                <%--<table style="width: 100%">
                                                                        <tr>
                                                                            <td align="left">
                                                                                    <%if (Session["PageAccess"].ToString().Trim() == "All" || Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "DelAdd")
                                                                          { %>
                                                                        <a href="javascript:void(0);" onclick="grid.AddNewRow();"  class="btn btn-primary"><span >Add New</span> </a>
                                                                        <%} %>
                                                                              
                                                                            </td>
                                                                        </tr>
                                                                    </table>--%>
                                                            </TitlePanel>
                                                        </Templates>
                                                    </dxe:ASPxGridView>
                                                    <asp:SqlDataSource ID="Sqljobresponsibility" runat="server"
                                                        SelectCommand="SELECT [job_id], [job_responsibility] FROM [tbl_master_jobResponsibility]"></asp:SqlDataSource>


                                                 <%--   <asp:SqlDataSource ID="sqlDept" runat="server"
                                                        SelectCommand="select cost_id,cost_description from tbl_master_costCenter  where cost_costCenterType='Department'"></asp:SqlDataSource>
                                                    <asp:SqlDataSource ID="Sqldesignation" runat="server"
                                                        SelectCommand="SELECT [deg_id], [deg_designation] FROM [tbl_master_designation]"></asp:SqlDataSource>--%>





                                                      <%-- DeleteCommand="INSERT INTO tbl_master_EmploymentHistory_Log(emp_id, emp_InternalId, emp_employerName, emp_employerAddress, emp_employerPhone, emp_employerFax, emp_employerEmail, emp_employerPan, emp_employerFrm, emp_employerTo, emp_jobResponsibility, emp_designation, emp_department, emp_ctc, emp_taxIncome, emp_tds, CreateUser, CreateDate, LastModifyDate, LastModifyUser, LogModifyDate, LogModifyUser, LogStatus) SELECT *,getdate(),@User,'D' FROM tbl_master_EmploymentHistory WHERE [emp_id] = @emp_id DELETE FROM [tbl_master_EmploymentHistory] WHERE [emp_id] = @emp_id"--%>
                                                      <%--InsertCommand="INSERT INTO [tbl_master_EmploymentHistory] ([emp_InternalId], [emp_employerName], [emp_employerAddress], [emp_employerPhone], [emp_employerFax], [emp_employerEmail], [emp_employerPan], [emp_employerFrm], [emp_employerTo], [emp_jobResponsibility], [emp_designation], [emp_department], [emp_ctc], [emp_taxIncome], [emp_tds], [CreateUser], [CreateDate]) VALUES (@emp_InternalId, @emp_employerName, @emp_employerAddress, @emp_employerPhone, @emp_employerFax, @emp_employerEmail, @emp_employerPan, @emp_employerFrm, @emp_employerTo, @emp_jobResponsibility, @emp_designation, @emp_department, @emp_ctc, @emp_taxIncome, @emp_tds, @CreateUser, getdate())"--%>
                                                    <%-- SelectCommand="SELECT *,convert(varchar(11),emp_employerFrm,113) as emp_employerFrm1,convert(varchar(11),emp_employerTo,113) as emp_employerTo1 FROM [tbl_master_EmploymentHistory] where emp_InternalId=@emp_InternalId"--%>
                                                      <%-- UpdateCommand="UPDATE [tbl_master_EmploymentHistory] SET  [emp_employerName] = @emp_employerName, [emp_employerAddress] = @emp_employerAddress, [emp_employerPhone] = @emp_employerPhone, [emp_employerFax] = @emp_employerFax, [emp_employerEmail] = @emp_employerEmail, [emp_employerPan] = @emp_employerPan, [emp_employerFrm] = @emp_employerFrm, [emp_employerTo] = @emp_employerTo, [emp_jobResponsibility] = @emp_jobResponsibility, [emp_designation] = @emp_designation, [emp_department] = @emp_department, [emp_ctc] = @emp_ctc, [emp_taxIncome] = @emp_taxIncome, [emp_tds] = @emp_tds,  [LastModifyDate] = getdate(), [LastModifyUser] = @LastModifyUser WHERE [emp_id] = @emp_id">--%>
                                                 







                                                    <asp:SqlDataSource ID="sqlHistory" runat="server"
                                                           DeleteCommand="DELETE FROM tbl_master_resignation WHERE emp_resignation_id=@emp_resignation_id and  emp_contactId=@emp_InternalId"

                                                          InsertCommand="INSERT INTO tbl_master_resignation(emp_contactId,emp_dateofresignation,emp_dateofleaving,Remarks,CreateDate,CreateUser)
                                                        VALUES (@emp_InternalId,@emp_dateofresignation,@emp_dateofleaving,@Remarks,GETDATE(),@CreateUser)"
                                                            SelectCommand="select * from tbl_master_resignation where emp_contactId=@emp_InternalId"
                                                            UpdateCommand="UPDATE tbl_master_resignation SET emp_dateofresignation=@emp_dateofresignation,emp_dateofleaving=@emp_dateofleaving,LastModifyDate=getdate(),LastModifyUser=@LastModifyUser,Remarks=@Remarks where emp_resignation_id=@emp_resignation_id and  emp_contactId=@emp_InternalId">
                                                        <DeleteParameters>
                                                           <%-- <asp:Parameter Name="emp_id" Type="Int32" />--%>
                                                             <asp:Parameter Name="emp_resignation_id" Type="Int32" />
                                                            <asp:SessionParameter Name="User" SessionField="userid" Type="Int32" />
                                                            <asp:SessionParameter Name="emp_InternalId" SessionField="KeyVal_InternalID" Type="String" />

                                                        </DeleteParameters>
                                                        <SelectParameters>
                                                            <asp:SessionParameter Name="emp_InternalId" SessionField="KeyVal_InternalID" Type="String" />
                                                        </SelectParameters>
                                                        <InsertParameters>
                                                            <asp:SessionParameter Name="emp_InternalId" SessionField="KeyVal_InternalID" Type="String" />

                                                             <asp:Parameter Type="DateTime" Name="emp_dateofresignation" />
                                                             <asp:Parameter Type="DateTime" Name="emp_dateofleaving" />
                                                            <asp:Parameter Type="String" Name="Remarks" />
                                                            <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="Int32" />
                                                            <asp:Parameter Name="emp_resignation_id" Type="Int32" />
                                                           <%-- <asp:Parameter Name="emp_employerName" Type="String" />
                                                            <asp:Parameter Name="emp_employerAddress" Type="String" />
                                                            <asp:Parameter Name="emp_employerPhone" Type="String" />
                                                            <asp:Parameter Name="emp_employerFax" Type="String" />
                                                            <asp:Parameter Name="emp_employerEmail" Type="String" />
                                                            <asp:Parameter Name="emp_employerPan" Type="String" />
                                                            <asp:Parameter Type="DateTime" Name="emp_employerFrm" />
                                                            <asp:Parameter Type="DateTime" Name="emp_employerTo" />
                                                            <asp:Parameter Name="emp_jobResponsibility" Type="String" />
                                                            <asp:Parameter Name="emp_designation" Type="String" />
                                                            <asp:Parameter Name="emp_department" Type="String" />
                                                            <asp:Parameter Name="emp_ctc" Type="String" />
                                                            <asp:Parameter Name="emp_taxIncome" Type="String" />
                                                            <asp:Parameter Name="emp_tds" Type="String" />
                                                            <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="Int32" />--%>
                                                        </InsertParameters>
                                                        <UpdateParameters>
                                                            <asp:Parameter Type="DateTime" Name="emp_dateofresignation" />
                                                            <asp:Parameter Type="DateTime" Name="emp_dateofleaving" />
                                                            <asp:Parameter Type="String" Name="Remarks" />
                                                             <asp:SessionParameter Name="LastModifyUser" SessionField="userid" Type="Int32" />
                                                              <asp:SessionParameter Name="emp_InternalId" SessionField="KeyVal_InternalID" Type="String" />
                                                            <%--<asp:Parameter Name="emp_employerName" Type="String" />
                                                            <asp:Parameter Name="emp_employerAddress" Type="String" />
                                                            <asp:Parameter Name="emp_employerPhone" Type="String" />
                                                            <asp:Parameter Name="emp_employerFax" Type="String" />
                                                            <asp:Parameter Name="emp_employerEmail" Type="String" />
                                                            <asp:Parameter Name="emp_employerPan" Type="String" />
                                                            <asp:Parameter Type="DateTime" Name="emp_employerFrm" />
                                                            <asp:Parameter Type="DateTime" Name="emp_employerTo" />
                                                            <asp:Parameter Name="emp_jobResponsibility" Type="String" />
                                                            <asp:Parameter Name="emp_designation" Type="String" />
                                                            <asp:Parameter Name="emp_department" Type="String" />
                                                            <asp:Parameter Name="emp_ctc" Type="String" />
                                                            <asp:Parameter Name="emp_taxIncome" Type="String" />
                                                            <asp:Parameter Name="emp_tds" Type="String" />
                                                            <asp:SessionParameter Name="LastModifyUser" SessionField="userid" Type="Int32" />
                                                            <asp:Parameter Name="emp_id" Type="Int32" />--%>
                                                        </UpdateParameters>
                                                    </asp:SqlDataSource>
                                                    <asp:SqlDataSource ID="Replacement" runat="server" ConflictDetection="CompareAllValues"
                        >
                                                        <SelectParameters>
                                                            <asp:SessionParameter Name="ID" Type="String" SessionField="KeyVal_InternalID" />
                                                        </SelectParameters>
                                                    </asp:SqlDataSource>
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:TextBox Width="170px" ID="txtEmployeeID" runat="server" Visible="False"></asp:TextBox>
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


   <%-- <table style="width: 100%;">
                        <tr>
                            <td align="left" style="width: 843px">
                                <asp:HiddenField ID="hdReferenceBy" runat="server" />
                                <table style="margin-top: 10px;">
                                    <tr>
                                        <td>
                                            <dxe:ASPxButton ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save"  ValidationGroup="a"
                                                TabIndex="26" CssClass="btn btn-primary">
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
                    </table>--%>
    <div>
        <asp:HiddenField ID="hdninternalid" runat="server" />
    </div>
</asp:Content>
