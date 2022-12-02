<%@ Page Title="Education" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true"
    Inherits="ERP.OMS.Management.Master.management_master_Employee_Education" CodeBehind="Employee_Education.aspx.cs" %>

<%--<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxTabControl" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxClasses" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe000001" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxTabControl" TagPrefix="dxe" %>
<%@ Register assembly="DevExpress.Web.v10.2" namespace="DevExpress.Web.ASPxTabControl" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v10.2" namespace="DevExpress.Web.ASPxClasses" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v10.2" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v10.2" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<title>Education</title>--%>
    <style>
        .dxgvControl_PlasticBlue a {
            color: #fff !important;
        }

        #gridEducation_DXPEForm_efnew_DXEFL_DXEditor2_EC, #gridEducation_DXPEForm_efnew_DXEFL_DXEditor3_EC,
        #gridEducation_DXPEForm_efnew_DXEFL_DXEditor7_EC {
            position: absolute;
        }

        .dxeErrorFrameSys.dxeErrorCellSys {
            position: absolute;
        }
    </style>
    <script language="javascript" type="text/javascript">
       
        function ValidateEditorValue(s, e) {
            var istur = false;

            if (GetEditorValue("edu_courseuntil") < GetEditorValue("edu_courseFrom")) {
                istur = true;
            }
            else {
                istur = false;

            }
            return istur;
        }
        function disp_prompt(name) {

            //var ID = document.getElementById(txtID);
            if (name == "tab0") {
                //alert(name);
                document.location.href = "Employee_general.aspx?id=" + $("#hdKeyVal").val();
            }
            if (name == "tab1") {
                //alert(name);
                document.location.href = "Employee_Correspondence.aspx";
            }
            else if (name == "tab2") {
                //alert(name);
                //document.location.href = "Employee_BankDetails.aspx";
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
                document.location.href = "Employee_EmployeeCTC.aspx";
                //alert(name);
                //  document.location.href = "Employee_GroupMember.aspx";

            }
            else if (name == "tab8") {
                document.location.href = "Employee_BankDetails.aspx";
                //alert(name);
                // document.location.href = "Employee_Employee.aspx";

            }
            else if (name == "tab9") {
                //alert(name);
                document.location.href = "Employee_Remarks.aspx";
            }
            else if (name == "tab10") {
                //alert(name);
                //  document.location.href = "Employee_BankDetails.aspx";
            }
            else if (name == "tab11") {
                ////  document.location.href = "Employee_Remarks.aspx";
                //alert(name);
                //document.location.href="Employee_Education.aspx"; 
            }
            else if (name == "tab12") {
                //alert(name);
                //  document.location.href="Employee_Subscription.aspx";
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
        function OnCountryChanged(cmbCountry) {
            grid.GetEditor("edu_state").PerformCallback(cmbCountry.GetValue().toString());
            grid.GetEditor("edu_city").PerformCallback(0);
        }
        function OnStateChanged(cmbState) {
            grid.GetEditor("edu_city").PerformCallback(cmbState.GetValue().toString());
        }


    </script>

    <%-- <script type="text/javascript">
        function ValidateEditorValue(s, e) {
            alert('jjjjj');
            var flag = true;
            if(GetEditorValue("edu_courseuntil") - GetEditorValue("edu_courseFrom")>0)
            {

                alert('Start Date Less Than End Date');
                flag=false;
            }
            //alert(GetEditorValue("edu_courseuntil") - GetEditorValue("edu_courseFrom"));
            //return GetEditorValue("edu_courseuntil") - GetEditorValue("edu_courseFrom");
            return flag;
        }

        function GetEditorValue(fieldName) {
            var editor = grid.GetEditor(fieldName);
           
            return editor.GetDate();
        }

      
       
    </script> --%>
    <script type="text/javascript">
        function ValidateEditorValue() {

            var startDate = GetEditorValue("edu_courseFrom");
            var endDate = GetEditorValue("edu_courseuntil");
            if (startDate != null && endDate != null) {
                if (dates.compare(endDate, startDate) == -1 || dates.compare(endDate, startDate) == 0) {
                    return false;
                }
                else { return true; }
            } else {
                return true;


            }
        }

        function GetEditorValue(fieldName) {
            var editor = grid.GetEditor(fieldName);
            return editor.GetValue();
        }
        // Source: http://stackoverflow.com/questions/497790
        var dates = {
            convert: function (d) {
                // Converts the date in d to a date-object. The input can be:
                //   a date object: returned without modification
                //  an array      : Interpreted as [year,month,day]. NOTE: month is 0-11.
                //   a number     : Interpreted as number of milliseconds
                //                  since 1 Jan 1970 (a timestamp) 
                //   a string     : Any format supported by the javascript engine, like
                //                  "YYYY/MM/DD", "MM/DD/YYYY", "Jan 31 2009" etc.
                //  an object     : Interpreted as an object with year, month and date
                //                  attributes.  **NOTE** month is 0-11.
                return (
                    d.constructor === Date ? d :
                    d.constructor === Array ? new Date(d[0], d[1], d[2]) :
                    d.constructor === Number ? new Date(d) :
                    d.constructor === String ? new Date(d) :
                    typeof d === "object" ? new Date(d.year, d.month, d.date) :
                    NaN
                );
            },
            compare: function (a, b) {
                // Compare two dates (could be of any type supported by the convert
                // function above) and returns:
                //  -1 : if a < b
                //   0 : if a = b
                //   1 : if a > b
                // NaN : if a or b is an illegal date
                // NOTE: The code inside isFinite does an assignment (=).
                return (
                    isFinite(a = this.convert(a).valueOf()) &&
                    isFinite(b = this.convert(b).valueOf()) ?
                    (a > b) - (a < b) :
                    NaN
                );
            },
            inRange: function (d, start, end) {
                // Checks if date in d is between dates in start and end.
                // Returns a boolean or NaN:
                //    true  : if d is between start and end (inclusive)
                //    false : if d is before start or after end
                //    NaN   : if one or more of the dates is illegal.
                // NOTE: The code inside isFinite does an assignment (=).
                return (
                     isFinite(d = this.convert(d).valueOf()) &&
                     isFinite(start = this.convert(start).valueOf()) &&
                     isFinite(end = this.convert(end).valueOf()) ?
                     start <= d && d <= end :
                     NaN
                 );
            }
        }
    </script>
    <style>
        #gridEducation_DXPEForm_efnew_DXEFL_DXEditor3_CC, #gridEducation_DXPEForm_efnew_DXEFL_DXEditor7_CC,
        #gridEducation_DXPEForm_efnew_DXEFL_DXEditor8_CC, #gridEducation_DXPEForm_efnew_DXEFL_DXEditor10_CC {
            padding: 0;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-title">
        <h3>Employee Education</h3>
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
                    <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="2" ClientInstanceName="page">
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
                                        <dxe:ASPxGridView ID="gridEducation" runat="server" ClientInstanceName="grid" AutoGenerateColumns="False"
                                            DataSourceID="sqleducation" KeyFieldName="edu_id" Width="100%" OnCellEditorInitialize="gridEducation_CellEditorInitialize1" OnCommandButtonInitialize="gridEducation_CommandButtonInitialize"
                                            OnInitNewRow="gridEducation_InitNewRow" OnStartRowEditing="gridEducation_StartRowEditing" EnableRowsCache="False">
                                            <Columns>
                                                <dxe:GridViewDataTextColumn FieldName="edu_id" ReadOnly="True" VisibleIndex="0"
                                                    Visible="False">
                                                    <EditFormSettings Visible="False" />
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="edu_internalId" VisibleIndex="0" Visible="False">
                                                    <EditFormSettings Visible="False" />
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataComboBoxColumn Caption="Degree" FieldName="edu_degree" VisibleIndex="0">
                                                    <PropertiesComboBox DataSourceID="sqlDegree" ValueField="edu_id" TextField="edu_education"
                                                        EnableIncrementalFiltering="True" ValueType="System.String">
                                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="right" SetFocusOnError="True">
                                                            <RequiredField ErrorText="Mandatory" IsRequired="True" />
                                                        </ValidationSettings>
                                                    </PropertiesComboBox>
                                                    <EditFormSettings Visible="True" VisibleIndex="1" />
                                                    <CellStyle CssClass="gridcellleft">
                                                    </CellStyle>
                                                    <EditFormCaptionStyle Wrap="False">
                                                    </EditFormCaptionStyle>
                                                </dxe:GridViewDataComboBoxColumn>
                                                <dxe:GridViewDataTextColumn FieldName="edu_instName" VisibleIndex="1" Caption="Institute Name">
                                                    <EditFormSettings Visible="True" Caption="Institute Name" VisibleIndex="2" />
                                                    <CellStyle CssClass="gridcellleft">
                                                    </CellStyle>
                                                    <EditFormCaptionStyle Wrap="False">
                                                    </EditFormCaptionStyle>
                                                    <PropertiesTextEdit MaxLength="50">
                                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" ErrorImage-ToolTip="Mandatory" ErrorTextPosition="right" SetFocusOnError="True">


                                                            <RequiredField ErrorText="Mandatory" IsRequired="True" />

                                                            <%--<RegularExpression ErrorText="Enter Valid PinCode" ValidationExpression="[0-9]{6}" />--%>
                                                        </ValidationSettings>
                                                    </PropertiesTextEdit>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataComboBoxColumn Caption="Country" FieldName="edu_country" VisibleIndex="2"
                                                    Visible="false">
                                                    <PropertiesComboBox DataSourceID="SqlCountry" ValueField="cou_id" TextField="cou_country"
                                                        ValueType="System.String" EnableIncrementalFiltering="True" EnableSynchronization="False">
                                                        <ClientSideEvents SelectedIndexChanged="function(s, e) { OnCountryChanged(s); }"></ClientSideEvents>
                                                    </PropertiesComboBox>
                                                    <EditFormSettings Visible="True" VisibleIndex="3" />
                                                    <EditFormCaptionStyle Wrap="False">
                                                    </EditFormCaptionStyle>
                                                </dxe:GridViewDataComboBoxColumn>
                                                <dxe:GridViewDataComboBoxColumn Caption="State" FieldName="edu_state" VisibleIndex="3"
                                                    Visible="false">
                                                    <PropertiesComboBox DataSourceID="StateSelect" ValueField="ID" TextField="State"
                                                        ValueType="System.String" EnableIncrementalFiltering="True" EnableSynchronization="False">
                                                        <ClientSideEvents SelectedIndexChanged="function(s, e) { OnStateChanged(s); }"></ClientSideEvents>
                                                    </PropertiesComboBox>
                                                    <EditFormSettings Visible="True" VisibleIndex="4" />
                                                    <EditFormCaptionStyle Wrap="False">
                                                    </EditFormCaptionStyle>
                                                </dxe:GridViewDataComboBoxColumn>
                                                <dxe:GridViewDataComboBoxColumn Caption="City" FieldName="edu_city" VisibleIndex="4">
                                                    <PropertiesComboBox DataSourceID="SelectCity" ValueField="CityId" TextField="City"
                                                        ValueType="System.String" EnableIncrementalFiltering="True" EnableSynchronization="False">
                                                    </PropertiesComboBox>
                                                    <EditFormSettings Visible="True" VisibleIndex="5" />
                                                    <EditFormCaptionStyle Wrap="False">
                                                    </EditFormCaptionStyle>
                                                </dxe:GridViewDataComboBoxColumn>
                                                <dxe:GridViewDataDateColumn Caption="From" FieldName="edu_courseFrom" Visible="false"
                                                    VisibleIndex="5">
                                                    <PropertiesDateEdit EditFormat="Custom" EditFormatString="dd-MM-yyyy" UseMaskBehavior="true">
                                                        <%-- <ValidationSettings Display="Dynamic" ErrorDisplayMode="Text" SetFocusOnError="True">
                                                            <RequiredField ErrorText="Required" IsRequired="false" />
                                                        </ValidationSettings>--%>

                                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" ErrorImage-ToolTip="Mandatory" ErrorTextPosition="right" SetFocusOnError="True">
                                                            <RequiredField ErrorText="Mandatory" IsRequired="false" />
                                                        </ValidationSettings>
                                                        <ClientSideEvents Validation="function(s, e) { e.isValid =ValidateEditorValue(s, e); e.errorText = 'From Date could not be greater than To Date';}"></ClientSideEvents>
                                                        <%--<ClientSideEvents Validation="function(s,e) {{ e.isValid = ValidateEditorValue(); e.errorText = 'From Date could not be greater than To Date';}}" />--%>
                                                    </PropertiesDateEdit>
                                                    <EditFormSettings Visible="True" VisibleIndex="6" />
                                                    <EditFormCaptionStyle Wrap="False">
                                                    </EditFormCaptionStyle>
                                                </dxe:GridViewDataDateColumn>
                                                <dxe:GridViewDataDateColumn Caption="To" FieldName="edu_courseuntil" Visible="false"
                                                    VisibleIndex="6">
                                                    <PropertiesDateEdit EditFormat="Custom" EditFormatString="dd-MM-yyyy" UseMaskBehavior="true">
                                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True" ErrorImage-ToolTip="Mandatory" ErrorTextPosition="right">
                                                            <RequiredField ErrorText="Mandatory" IsRequired="false" />

                                                        </ValidationSettings>
                                                        <ClientSideEvents Validation="function(s, e) { e.isValid =ValidateEditorValue(s, e); e.errorText = 'From Date could not be greater than To Date';}"></ClientSideEvents>
                                                        <%--<ClientSideEvents Validation="function(s,e) {{ e.isValid = ValidateEditorValue(); e.errorText = 'From Date could not be greater than To Date';}}" />--%>
                                                    </PropertiesDateEdit>

                                                    <%--   <PropertiesDateEdit EditFormat="Custom" EditFormatString="dd-MM-yyyy" UseMaskBehavior="true">
                                <ValidationSettings  Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" ></ValidationSettings>
                                <ClientSideEvents Validation="function(s,e) {{ e.isValid = ValidateEditorValue(); e.errorText = 'xvcv';}}" />
                                </PropertiesDateEdit>--%>



                                                    <EditFormSettings Visible="True" VisibleIndex="7" />
                                                    <EditFormCaptionStyle Wrap="False">
                                                    </EditFormCaptionStyle>
                                                </dxe:GridViewDataDateColumn>

                                                <dxe:GridViewDataComboBoxColumn Caption="Result" FieldName="edu_courseResult" VisibleIndex="7">
                                                    <PropertiesComboBox ValueType="System.String" EnableIncrementalFiltering="True">
                                                        <Items>
                                                            <dxe:ListEditItem Text="Passed" Value="Passed"></dxe:ListEditItem>
                                                            <dxe:ListEditItem Text="Failed" Value="Failed"></dxe:ListEditItem>
                                                            <dxe:ListEditItem Text="Discontinued" Value="Discontinued"></dxe:ListEditItem>
                                                            <dxe:ListEditItem Text="Passed with distinction" Value="Passed with distinction"></dxe:ListEditItem>
                                                            <dxe:ListEditItem Text="pursuing" Value="pursuing"></dxe:ListEditItem>
                                                            <dxe:ListEditItem Text="N/A" Value="N/A"></dxe:ListEditItem>
                                                        </Items>
                                                    </PropertiesComboBox>
                                                    <EditFormSettings Visible="True" VisibleIndex="8" />
                                                    <CellStyle CssClass="gridcellleft">
                                                    </CellStyle>
                                                    <EditFormCaptionStyle Wrap="False">
                                                    </EditFormCaptionStyle>
                                                </dxe:GridViewDataComboBoxColumn>
                                                <dxe:GridViewDataTextColumn FieldName="edu_percentage" VisibleIndex="8" Caption="Percentage">
                                                    <EditFormSettings Visible="true" Caption="Percentage" VisibleIndex="9" />
                                                    <CellStyle CssClass="gridcellleft">
                                                    </CellStyle>
                                                    <EditFormCaptionStyle Wrap="False">
                                                    </EditFormCaptionStyle>
                                                    <PropertiesTextEdit>
                                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="Text" SetFocusOnError="True">
                                                            <RegularExpression ErrorText="Valid Percentage" ValidationExpression="^(\d{0,13}\.\d{0,5}|\d{0,13})$" />
                                                            <RequiredField ErrorText="Required" IsRequired="false" />
                                                        </ValidationSettings>
                                                    </PropertiesTextEdit>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="edu_grade" VisibleIndex="9" Caption="Grade">
                                                    <EditFormSettings Visible="True" Caption="Grade" VisibleIndex="10" />
                                                    <CellStyle CssClass="gridcellleft">
                                                    </CellStyle>
                                                    <EditFormCaptionStyle Wrap="False">
                                                    </EditFormCaptionStyle>
                                                    <PropertiesTextEdit MaxLength="50"></PropertiesTextEdit>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataDateColumn Caption="Passing Month & Year" Visible="false" FieldName="edu_month_year"
                                                    VisibleIndex="10">
                                                    <PropertiesDateEdit DisplayFormatString="" EditFormat="Custom" EditFormatString="MMMM yyyy"
                                                        UseMaskBehavior="true">
                                                    </PropertiesDateEdit>
                                                    <EditFormSettings Visible="True" VisibleIndex="10" />
                                                    <EditFormCaptionStyle Wrap="False">
                                                    </EditFormCaptionStyle>
                                                </dxe:GridViewDataDateColumn>
                                                <dxe:GridViewDataTextColumn FieldName="createuser" VisibleIndex="11" Visible="False">
                                                    <EditFormSettings Visible="False" />
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataDateColumn FieldName="createdate" VisibleIndex="11" Visible="False">
                                                    <EditFormSettings Visible="False" />
                                                </dxe:GridViewDataDateColumn>
                                                <dxe:GridViewDataTextColumn FieldName="lastmodifyuser" VisibleIndex="11" Visible="False">
                                                    <EditFormSettings Visible="False" />
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataDateColumn FieldName="lastmodifydate" VisibleIndex="11" Visible="False">
                                                    <EditFormSettings Visible="False" />
                                                </dxe:GridViewDataDateColumn>
                                                <dxe:GridViewDataTextColumn FieldName="edu_courseFrom1" VisibleIndex="5" Caption="From">
                                                    <EditFormSettings Visible="false" />
                                                    <CellStyle CssClass="gridcellleft">
                                                    </CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="edu_courseuntil1" VisibleIndex="6" Caption="To">
                                                    <EditFormSettings Visible="false" />
                                                    <CellStyle CssClass="gridcellleft">
                                                    </CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="edu_country1" VisibleIndex="2" Caption="Country">
                                                    <EditFormSettings Visible="false" />
                                                    <CellStyle CssClass="gridcellleft">
                                                    </CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="edu_state1" VisibleIndex="3" Caption="State">
                                                    <EditFormSettings Visible="false" />
                                                    <CellStyle CssClass="gridcellleft">
                                                    </CellStyle>
                                                </dxe:GridViewDataTextColumn>

                                                <dxe:GridViewDataTextColumn FieldName="edu_degree1" VisibleIndex="2" Caption="Degree" Visible="false">
                                                    <EditFormSettings Visible="false" />
                                                    <CellStyle CssClass="gridcellleft">
                                                    </CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="edu_city1" VisibleIndex="3" Caption="City" Visible="false">
                                                    <EditFormSettings Visible="false" />
                                                    <CellStyle CssClass="gridcellleft">
                                                    </CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="edu_month_year1" VisibleIndex="10" Caption="Passing Month & Year" Width="12%">
                                                    <EditFormSettings Visible="false" />
                                                    <CellStyle CssClass="gridcellleft">
                                                    </CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewCommandColumn VisibleIndex="11" ShowEditButton="true" ShowDeleteButton="true" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                    <%-- <EditButton Visible="True">
                                                        </EditButton>
                                                        <DeleteButton Visible="True">
                                                        </DeleteButton>--%>
                                                    <HeaderTemplate>
                                                        Actions
                                                          <%--  <a href="javascript:void(0);" onclick="grid.AddNewRow();"><span >Add New</span> </a>--%>
                                                    </HeaderTemplate>
                                                </dxe:GridViewCommandColumn>
                                            </Columns>
                                            <SettingsCommandButton>



                                                <EditButton Image-Url="../../../assests/images/Edit.png" ButtonType="Image" Image-AlternateText="Edit" Styles-Style-CssClass="pad">
                                                </EditButton>
                                                <DeleteButton Image-Url="../../../assests/images/Delete.png" ButtonType="Image" Image-AlternateText="Delete">
                                                </DeleteButton>

                                                <UpdateButton Text="Save" ButtonType="Button" Styles-Style-CssClass="btn btn-primary"></UpdateButton>
                                                <CancelButton Text="Cancel" ButtonType="Button" Styles-Style-CssClass="btn btn-danger"></CancelButton>
                                            </SettingsCommandButton>
                                            <StylesEditors>
                                                <ProgressBar Height="25px">
                                                </ProgressBar>
                                            </StylesEditors>
                                            <Settings ShowFooter="True" ShowStatusBar="Visible" ShowTitlePanel="True" ShowGroupButtons="False" />
                                            <SettingsEditing Mode="PopupEditForm" PopupEditFormHorizontalAlign="WindowCenter"
                                                PopupEditFormModal="True" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="500px"
                                                EditFormColumnCount="1" />
                                            <Styles>
                                                <LoadingPanel ImageSpacing="10px">
                                                </LoadingPanel>
                                                <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                </Header>
                                            </Styles>
                                            <SettingsText PopupEditFormCaption="Add Education" ConfirmDelete="Confirm Delete?"
                                                Title="Add Address" />
                                            <SettingsPager NumericButtonCount="20" PageSize="20">
                                            </SettingsPager>
                                            <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True" />
                                            <Templates>
                                                <EditForm>
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td style="width: 5%"></td>
                                                            <td style="width: 90%">
                                                                <controls>
                                                       <dxe:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormEditors" ColumnID="" ID="Editors">
                                                       </dxe:ASPxGridViewTemplateReplacement>                                                           
                                                     </controls>
                                                                <div style="padding: 2px 2px 2px 140px">
                                                                    <dxe:ASPxGridViewTemplateReplacement ID="UpdateButton" ReplacementType="EditFormUpdateButton"
                                                                        runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                                    <dxe:ASPxGridViewTemplateReplacement ID="CancelButton" ReplacementType="EditFormCancelButton"
                                                                        runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                                </div>
                                                            </td>
                                                            <td style="width: 5%"></td>
                                                        </tr>
                                                    </table>
                                                </EditForm>
                                                <TitlePanel>
                                                    <table style="width: 100%">
                                                        <tr>

                                                            <td align="left">
                                                                <% if (rights.CanAdd)
                                                                   { %>
                                                                <a href="javascript:void(0);" onclick="grid.AddNewRow();" class="btn btn-primary"><span>Add New</span> </a><%} %>
                                                                <%-- <span class="Ecoheadtxt">Add/Modify Education.</span>--%>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </TitlePanel>
                                            </Templates>
                                        </dxe:ASPxGridView>
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

                              <dxe:TabPage Name="Language"  Text="Language" >
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
                        <TabStyle Font-Size="12px">
                        </TabStyle>
                    </dxe:ASPxPageControl>
                </td>
            </tr>
            <tr>
                <td></td>
            </tr>
        </table>
        <asp:SqlDataSource ID="sqleducation" runat="server" 
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
        <asp:SqlDataSource ID="sqlDegree" runat="server"
            SelectCommand="SELECT [edu_id], [edu_education] FROM [tbl_master_education] order by edu_education"></asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlCountry" runat="server" 
            SelectCommand="SELECT [cou_id], [cou_country] FROM [tbl_master_country] order by cou_country"></asp:SqlDataSource>
        <asp:SqlDataSource ID="StateSelect" runat="server" 
            SelectCommand="SELECT s.id as ID,s.state as State from tbl_master_state s where (s.countryId = @State) ORDER BY s.state">
            <SelectParameters>
                <asp:Parameter Name="State" Type="string" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="SelectCity" runat="server" 
            SelectCommand="SELECT c.city_id AS CityId, c.city_name AS City FROM tbl_master_city c where c.state_id=@City order by c.city_name">
            <SelectParameters>
                <asp:Parameter Name="City" Type="string" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
     <asp:HiddenField ID="hdKeyVal" runat="server" />
</asp:Content>

