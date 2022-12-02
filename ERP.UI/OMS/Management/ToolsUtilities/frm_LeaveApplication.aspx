<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.ToolsUtilities.management_utilities_frm_LeaveApplication" CodeBehind="frm_LeaveApplication.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


    <script type="text/javascript">
        function getday(date2) {
            //var date1=convertToDateTime(date2,"MM/DD/YYYY");
            alert(date2);
        }
        function OnChangeDate(date) {

            //GridApplicationLeave.GetEditor("la_joinDateTime").PerformCallback(ASPxDateEdit.GetDate().toString(),);
        }
        function ShowHideFilter(obj) {
            GridApplicationLeave.PerformCallback(obj);
        }
        //function SignOff()
        //{
        //    window.parent.SignOff();
        //}
        //function height()
        //{
        //    if(document.body.scrollHeight>=500)
        //        window.frameElement.height = document.body.scrollHeight;
        //    else
        //        window.frameElement.height = '500px';
        //    window.frameElement.Width = document.body.scrollWidth;
        //}
        function LastCall(obj) {
            //height();
        }

      

        function OnEmployeeChanged(cmbEmployee) {
            //alert(cmbEmployee.GetValue().toString());
            var url = 'frm_LeaveApplication_popup.aspx?ID=' + cmbEmployee.GetValue().toString();
            //window.location.href = url;
            window.open(url, '50', 'resizable=1,height=250px,width=400px,top=200x,left=500x');




        }
    </script>
</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Leave Application</h3>
        </div>

    </div>
    <div class="form_main">
        <table class="TableMain100">

            <tr>
                <td style="vertical-align: top" class="gridcellleft">
                    <table width="100%">
                        <tr>
                            <td style="text-align: left; vertical-align: top">
                                <table>
                                    <tr>
                                        <td id="ShowFilter">
                                            <%--<a href="javascript:ShowHideFilter('s');"><span style="color: #000099; text-decoration: underline">Show Filter</span></a>--%>
                                            <a href="javascript:void(0);" onclick="GridApplicationLeave.AddNewRow();" class="btn btn-primary"><span>Add New</span></a>
                                        </td>
                                        <td id="Td1">
                                            <a href="javascript:ShowHideFilter('All');" class="btn btn-primary"><span>All Records</span></a>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td></td>
                            <td class="gridcellright" style="float: right; vertical-align: top">
                                <dxe:ASPxComboBox ID="cmbExport" runat="server" AutoPostBack="true"
                                    Font-Bold="False" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged"
                                    ValueType="System.Int32" Width="130px">
                                    <Items>
                                        <dxe:ListEditItem Text="Select" Value="0" />
                                        <dxe:ListEditItem Text="PDF" Value="1" />
                                        <dxe:ListEditItem Text="XLS" Value="2" />
                                        <dxe:ListEditItem Text="RTF" Value="3" />
                                        <dxe:ListEditItem Text="CSV" Value="4" />
                                    </Items>
                                    <Border BorderColor="Black" />
                                    <DropDownButton Text="Export">
                                    </DropDownButton>
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <dxe:ASPxGridView ID="GridApplicationLeave" ClientInstanceName="GridApplicationLeave"
                        runat="server" AutoGenerateColumns="False" DataSourceID="DataSourceLeaveApplication" KeyFieldName="la_id"
                        Width="100%" OnRowValidating="GridApplicationLeave_RowValidating" OnCellEditorInitialize="GridApplicationLeave_CellEditorInitialize"
                        OnCustomCallback="GridApplicationLeave_CustomCallback">
                        <SettingsBehavior AllowFocusedRow="True"></SettingsBehavior>
                        <Styles>
                            <Header SortingImageSpacing="5px" ImageSpacing="5px">
                            </Header>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                        </Styles>
                        <SettingsPager PageSize="20" AlwaysShowPager="True" ShowSeparators="True">
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </SettingsPager>

                        <SettingsEditing Mode="PopupEditForm" PopupEditFormHorizontalAlign="WindowCenter"
                            PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="700px" PopupEditFormModal="True">
                        </SettingsEditing>
                        <Columns>
                            <dxe:GridViewDataTextColumn FieldName="la_id" ReadOnly="True" Visible="False" VisibleIndex="0">
                                <EditFormSettings Visible="False" />
                                <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                </EditCellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                </EditFormCaptionStyle>
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataComboBoxColumn FieldName="la_cntId" VisibleIndex="1" Caption="Employee name">
                                <PropertiesComboBox DataSourceID="AllEmployee" TextField="Name" ValueField="ID" ValueType="System.String"
                                    EnableIncrementalFiltering="True">
                                    <ClientSideEvents SelectedIndexChanged="function(s, e) { OnEmployeeChanged(s); }"></ClientSideEvents>
                                </PropertiesComboBox>
                                <EditFormSettings Visible="True" Caption="Employee name" VisibleIndex="1" />
                                <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                </EditCellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                </EditFormCaptionStyle>
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>

                            </dxe:GridViewDataComboBoxColumn>
                            <dxe:GridViewDataTextColumn Caption="Branch" FieldName="branch" ReadOnly="True"
                                VisibleIndex="2">
                                <EditFormSettings Visible="False" />
                                <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                </EditCellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                </EditFormCaptionStyle>
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataComboBoxColumn Caption="Leave Type" FieldName="la_appType" VisibleIndex="9">
                                <PropertiesComboBox EnableIncrementalFiltering="True" ValueType="System.String">
                                    <ClientSideEvents SelectedIndexChanged="function(s, e) {
	var value = s.GetValue();
	
    if(value == &quot;HC&quot; || value == &quot;HS&quot;)
    {
        GridApplicationLeave.GetEditor(&quot;la_joinDateTime&quot;).SetClientVisible(false);
        GridApplicationLeave.GetEditor(&quot;la_endDateAppl&quot;).SetClientVisible(false);
    }
    else
    {
        GridApplicationLeave.GetEditor(&quot;la_joinDateTime&quot;).SetClientVisible(true);
        GridApplicationLeave.GetEditor(&quot;la_endDateAppl&quot;).SetClientVisible(true);
    }
    GridApplicationLeave.GetEditor(&quot;la_startDateAppl&quot;).SetDate();
    }" />
                                    <Items>
                                        <dxe:ListEditItem Text="PL" Value="PL"></dxe:ListEditItem>
                                        <dxe:ListEditItem Text="CL" Value="CL"></dxe:ListEditItem>
                                        <dxe:ListEditItem Text="SL" Value="SL"></dxe:ListEditItem>
                                        <dxe:ListEditItem Text="ML" Value="ML"></dxe:ListEditItem>
                                        <dxe:ListEditItem Text="HC" Value="HC"></dxe:ListEditItem>
                                        <dxe:ListEditItem Text="HS" Value="HS"></dxe:ListEditItem>
                                    </Items>
                                </PropertiesComboBox>
                                <EditFormSettings Visible="True" Caption="Leave Type" VisibleIndex="6" />
                                <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                </EditCellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                </EditFormCaptionStyle>
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                            </dxe:GridViewDataComboBoxColumn>
                            <dxe:GridViewDataComboBoxColumn FieldName="la_Consideration" Visible="False" VisibleIndex="3">
                                <PropertiesComboBox EnableIncrementalFiltering="True" ValueField="la_Consideration"
                                    ValueType="System.Char">
                                    <Items>
                                        <dxe:ListEditItem Text="Leave" Value="L"></dxe:ListEditItem>
                                        <dxe:ListEditItem Text="Cash" Value="C"></dxe:ListEditItem>
                                    </Items>
                                </PropertiesComboBox>
                                <EditFormSettings Visible="True" Caption="Consideration" VisibleIndex="3" />
                                <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                </EditCellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                </EditFormCaptionStyle>
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                            </dxe:GridViewDataComboBoxColumn>
                            <dxe:GridViewDataCheckColumn FieldName="la_ReceivedPhysical" Visible="False" VisibleIndex="4">
                                <PropertiesCheckEdit ValueChecked="Y" ValueType="System.Char" ValueUnchecked="N"
                                    DisplayTextChecked="Yes" DisplayTextUnchecked="No" UseDisplayImages="False">
                                </PropertiesCheckEdit>
                                <EditFormSettings Visible="True" Caption="Application(Physical) Received" VisibleIndex="4" />
                                <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                </EditCellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                </EditFormCaptionStyle>
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                            </dxe:GridViewDataCheckColumn>
                            <dxe:GridViewDataDateColumn FieldName="la_appDate" Visible="False" VisibleIndex="5">
                                <PropertiesDateEdit DateOnError="Null" EditFormat="Custom" EditFormatString="dd MMMM yyyy"
                                    UseMaskBehavior="True">
                                </PropertiesDateEdit>
                                <EditFormSettings Visible="True" Caption="Application Date" VisibleIndex="5" />
                                <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                </EditCellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                </EditFormCaptionStyle>
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                            </dxe:GridViewDataDateColumn>
                            <dxe:GridViewDataDateColumn FieldName="la_startDateAppl" Visible="False" VisibleIndex="6">
                                <PropertiesDateEdit DateOnError="Null" EditFormat="Custom" EditFormatString="dd MMMM yyyy"
                                    UseMaskBehavior="True">
                                    <ClientSideEvents DateChanged="function(s, e) {
	        var date = s.GetDate();
	        var status = GridApplicationLeave.GetEditor(&quot;la_appType&quot;).GetValue();
	        if(status==&quot;HC&quot; || status==&quot;HS&quot;)
	        {
	            GridApplicationLeave.GetEditor(&quot;la_endDateAppl&quot;).SetDate(date);
	            var newDate = new Date(date.getYear(), date.getMonth(), date.getDate()+1, 10, 0, 0);
	            GridApplicationLeave.GetEditor(&quot;la_joinDateTime&quot;).SetDate(newDate);
	        } 
        }" />
                                </PropertiesDateEdit>
                                <EditFormSettings Visible="True" Caption="Leave Start Date (applied)" VisibleIndex="9" />
                                <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                </EditCellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                </EditFormCaptionStyle>
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                            </dxe:GridViewDataDateColumn>
                            <dxe:GridViewDataDateColumn FieldName="la_endDateAppl" Visible="False" VisibleIndex="7">
                                <PropertiesDateEdit DateOnError="Null" EditFormat="Custom" EditFormatString="dd MMMM yyyy"
                                    UseMaskBehavior="True">
                                    <ClientSideEvents DateChanged="function(s, e) {
	var date = s.GetDate();
	var newDate;
	if(date.getDay()== 6)
	    newDate = new Date(date.getYear(), date.getMonth(), date.getDate()+2, 10, 0, 0);
	else
	    newDate = new Date(date.getYear(), date.getMonth(), date.getDate()+1, 10, 0, 0);
	GridApplicationLeave.GetEditor(&quot;la_joinDateTime&quot;).SetDate(newDate);
	}" />
                                </PropertiesDateEdit>
                                <EditFormSettings Visible="True" Caption="Leave End Date (applied)" VisibleIndex="10" />
                                <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                </EditCellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                </EditFormCaptionStyle>
                                <%--<CellStyle CssClass="gridcellleft">
                                </CellStyle>--%>
                            </dxe:GridViewDataDateColumn>
                            <dxe:GridViewDataTextColumn FieldName="la_totaldaysAppl" Visible="False" VisibleIndex="8">
                                <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                </EditCellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                </EditFormCaptionStyle>
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormSettings VisibleIndex="6" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataComboBoxColumn Caption="Application Status" FieldName="la_appStatus"
                                VisibleIndex="14">
                                <PropertiesComboBox EnableIncrementalFiltering="True" ValueType="System.String">
                                    <ClientSideEvents SelectedIndexChanged="function(s, e) {
	var value = s.GetValue();
	if(value == &quot;AM&quot;)
    {
        GridApplicationLeave.GetEditor(&quot;la_startDateApr&quot;).SetClientVisible(true);
        GridApplicationLeave.GetEditor(&quot;la_endDateApr&quot;).SetClientVisible(true);
    }
    else
    {
         GridApplicationLeave.GetEditor(&quot;la_startDateApr&quot;).SetClientVisible(false);
         GridApplicationLeave.GetEditor(&quot;la_endDateApr&quot;).SetClientVisible(false);
    }
    if(value == &quot;DU&quot;)
    {
alert(value);
        $('#GridApplicationLeave_DXPEForm_efnew_DXEFL_DXEditor11_I,#GridApplicationLeave_DXPEForm_efnew_DXEFL_DXEditor12_I').hide();
        GridApplicationLeave.GetEditor(&quot;la_apprRejBy&quot;).SetClientVisible(false);
        GridApplicationLeave.GetEditor(&quot;la_apprRejOn&quot;).SetClientVisible(false);
     

    }
    else
    {
         GridApplicationLeave.GetEditor(&quot;la_apprRejBy&quot;).SetClientVisible(true);
         GridApplicationLeave.GetEditor(&quot;la_apprRejOn&quot;).SetClientVisible(true);
         
       
    }
}" />
                                    <Items>
                                        <dxe:ListEditItem Text="Due" Value="DU"></dxe:ListEditItem>
                                        <dxe:ListEditItem Text="Approved" Value="AP"></dxe:ListEditItem>
                                        <dxe:ListEditItem Text="Approved with modification" Value="AM"></dxe:ListEditItem>
                                        <dxe:ListEditItem Text="Rejected " Value="RJ"></dxe:ListEditItem>
                                    </Items>
                                </PropertiesComboBox>
                                <EditFormSettings Visible="True" Caption="Application Status" VisibleIndex="7" />
                                <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                </EditCellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                </EditFormCaptionStyle>
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                            </dxe:GridViewDataComboBoxColumn>
                            <dxe:GridViewDataComboBoxColumn Caption="Approved/rejected By" FieldName="la_apprRejBy"
                                VisibleIndex="16">
                                <PropertiesComboBox DataSourceID="AllEmployee" EnableIncrementalFiltering="True"
                                    TextField="Name" ValueField="ID" ValueType="System.String">
                                </PropertiesComboBox>
                                <EditFormSettings Visible="True" Caption="Approved/rejected By" VisibleIndex="8" />
                                <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                </EditCellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                </EditFormCaptionStyle>
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                            </dxe:GridViewDataComboBoxColumn>
                            <dxe:GridViewDataDateColumn Caption="Approved/rejected Date" FieldName="la_apprRejOn"
                                Visible="False" VisibleIndex="18">
                                <PropertiesDateEdit EditFormat="Custom" EditFormatString="dd MMMM yyyy" UseMaskBehavior="True">
                                </PropertiesDateEdit>
                                <EditFormSettings Visible="True" Caption="Approved/rejected  On" VisibleIndex="8" />
                                <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                </EditCellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                </EditFormCaptionStyle>
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                            </dxe:GridViewDataDateColumn>
                            <dxe:GridViewDataDateColumn Caption="From" FieldName="la_startDateApr" VisibleIndex="10"
                                Visible="False">
                                <PropertiesDateEdit EditFormat="Custom" EditFormatString="dd MMMM yyyy" UseMaskBehavior="True">
                                </PropertiesDateEdit>
                                <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                </EditCellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                </EditFormCaptionStyle>
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormSettings Caption="Leave Start date (approved)" VisibleIndex="12" Visible="True" />
                            </dxe:GridViewDataDateColumn>
                            <dxe:GridViewDataDateColumn Caption="To" FieldName="la_endDateApr" VisibleIndex="11"
                                Visible="False">
                                <PropertiesDateEdit EditFormat="Custom" EditFormatString="dd MMMM yyyy" UseMaskBehavior="True">
                                    <ClientSideEvents DateChanged="function(s, e){
         var date = s.GetDate();
	var newDate;
	if(date.getDay()== 6)
	    newDate = new Date(date.getYear(), date.getMonth(), date.getDate()+2, 10, 0, 0);
	else
	    newDate = new Date(date.getYear(), date.getMonth(), date.getDate()+1, 10, 0, 0);
	GridApplicationLeave.GetEditor(&quot;la_joinDateTime&quot;).SetDate(newDate);
	}" />
                                </PropertiesDateEdit>
                                <EditFormSettings Visible="True" Caption="Leave End date (approved)" VisibleIndex="13" />
                                <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                </EditCellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                </EditFormCaptionStyle>
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                            </dxe:GridViewDataDateColumn>
                            <dxe:GridViewDataTextColumn FieldName="stdate" VisibleIndex="12" Caption="From">
                                <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                </EditCellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                </EditFormCaptionStyle>
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormSettings Visible="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="endate" VisibleIndex="13" Caption="To">
                                <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                </EditCellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                </EditFormCaptionStyle>
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormSettings Visible="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="la_totaldaysApr" Visible="False" VisibleIndex="15">
                                <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                </EditCellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                </EditFormCaptionStyle>
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataDateColumn Caption="Joining Datetime" FieldName="la_joinDateTime"
                                VisibleIndex="17" Visible="False">
                                <PropertiesDateEdit EditFormat="Custom" EditFormatString="dd MMMM yyyy hh:mm tt"
                                    UseMaskBehavior="True">
                                </PropertiesDateEdit>
                                <EditFormSettings Visible="True" Caption="Joining DateTime" VisibleIndex="14" />
                                <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                </EditCellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                </EditFormCaptionStyle>
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                            </dxe:GridViewDataDateColumn>
                            <dxe:GridViewCommandColumn VisibleIndex="19" ShowEditButton="true" ShowClearFilterButton="true" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                <%--<EditButton Visible="True">
                            </EditButton>--%>
                                <%--  <ClearFilterButton Visible="True">
                            </ClearFilterButton>--%>
<HeaderStyle HorizontalAlign="Center"></HeaderStyle>

<CellStyle HorizontalAlign="Center"></CellStyle>
                                <HeaderTemplate>
                                    <span>Actions</span>
                                    <%-- <a href="javascript:void(0);" onclick="GridApplicationLeave.AddNewRow()"><span style="color: #000099; text-decoration: underline">Add New</span></a>--%>
                                </HeaderTemplate>
                            </dxe:GridViewCommandColumn>


                            <dxe:GridViewDataMemoColumn Caption="Remarks" Visible="False" VisibleIndex="20" FieldName="la_Remarks">
                                <PropertiesMemoEdit Columns="2" Height="100px" NullText="Enter Remarks Here">
                                </PropertiesMemoEdit>
                                <EditFormSettings Caption="Remarks" ColumnSpan="2" Visible="True" VisibleIndex="15" />
                                <EditCellStyle CssClass="gridcellleft">
                                </EditCellStyle>
                            </dxe:GridViewDataMemoColumn>
                        </Columns>
                        <SettingsCommandButton>
                            <UpdateButton Text="Update" ButtonType="Button" Styles-Style-CssClass="btn btn-primary">
<Styles>
<Style CssClass="btn btn-primary"></Style>
</Styles>
                            </UpdateButton>
                            <CancelButton Text="Cancel" ButtonType="Button" Styles-Style-CssClass="btn btn-danger">
<Styles>
<Style CssClass="btn btn-danger"></Style>
</Styles>
                            </CancelButton>
                            <EditButton Image-Url="/assests/images/Edit.png" Image-AlternateText="Edit" ButtonType="Image">
<Image AlternateText="Edit" Url="/assests/images/Edit.png"></Image>
                            </EditButton>
                            <ClearFilterButton Text="ClearFilter"></ClearFilterButton>
                        </SettingsCommandButton>
                        <SettingsSearchPanel Visible="True" />
                        <Settings ShowGroupPanel="True" ShowFilterRow="true" ShowFilterRowMenu="true"></Settings>
                        <StylesEditors>
                            <ProgressBar Height="25px">
                            </ProgressBar>
                        </StylesEditors>
                        <Templates>
                            <EditForm>
                                <table style="width: 100%">

                                    <tr>
                                        <td>

                                            <dxe:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormEditors" ColumnID="" ID="Editors"></dxe:ASPxGridViewTemplateReplacement>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-left: 15px;">
                                            <dxe:ASPxGridViewTemplateReplacement ID="UpdateButton" ReplacementType="EditFormUpdateButton"
                                                runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                            <dxe:ASPxGridViewTemplateReplacement ID="CancelButton" ReplacementType="EditFormCancelButton"
                                                runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                        </td>
                                    </tr>
                                </table>
                            </EditForm>
                        </Templates>
                    </dxe:ASPxGridView>
                    <asp:SqlDataSource ID="DataSourceLeaveApplication" runat="server" ConflictDetection="CompareAllValues"
                        DeleteCommand="DELETE FROM [tbl_trans_LeaveApplication] WHERE [la_id] = @la_id "
                        InsertCommand="LeaveApplicationInsert" InsertCommandType="StoredProcedure" SelectCommand=""
                        UpdateCommand="update table1 set temp123='123'">
                        <DeleteParameters>
                            <asp:Parameter Name="la_id" Type="Int32" />
                        </DeleteParameters>
                        <UpdateParameters>
                            <asp:Parameter Name="la_cntId" Type="String" />
                            <asp:Parameter Name="la_appType" Type="String" />
                            <asp:Parameter Name="la_Consideration" Type="String" />
                            <asp:Parameter Name="la_ReceivedPhysical" Type="String" />
                            <asp:Parameter Name="la_appDate" Type="DateTime" />
                            <asp:Parameter Name="la_startDateAppl" Type="DateTime" />
                            <asp:Parameter Name="la_endDateAppl" Type="DateTime" />
                            <asp:Parameter Name="la_appStatus" Type="String" />
                            <asp:Parameter Name="la_apprRejBy" Type="String" />
                            <asp:Parameter Name="la_apprRejOn" Type="DateTime" />
                            <asp:Parameter Name="la_startDateApr" Type="DateTime" />
                            <asp:Parameter Name="la_endDateApr" Type="DateTime" />
                            <asp:SessionParameter Name="userId" SessionField="userid" Type="int32" />
                            <asp:Parameter Name="la_id" Type="Int32" />
                            <asp:Parameter Name="la_joinDateTime" Type="DateTime" />
                            <asp:Parameter Name="la_Remarks" Type="string" />
                        </UpdateParameters>
                        <InsertParameters>
                            <asp:Parameter Name="la_cntId" Type="String" />
                            <asp:Parameter Name="la_appType" Type="String" />
                            <asp:Parameter Name="la_Consideration" Type="String" />
                            <asp:Parameter Name="la_ReceivedPhysical" Type="String" />
                            <asp:Parameter Name="la_appDate" Type="DateTime" />
                            <asp:Parameter Name="la_startDateAppl" Type="DateTime" />
                            <asp:Parameter Name="la_endDateAppl" Type="DateTime" />
                            <asp:Parameter Name="la_totaldaysAppl" Type="Int32" />
                            <asp:Parameter Name="la_appStatus" Type="String" />
                            <asp:Parameter Name="la_apprRejBy" Type="String" />
                            <asp:Parameter Name="la_apprRejOn" Type="DateTime" />
                            <asp:Parameter Name="la_startDateApr" Type="DateTime" />
                            <asp:Parameter Name="la_endDateApr" Type="DateTime" />
                            <asp:Parameter Name="la_totaldaysApr" Type="Int32" />
                            <asp:SessionParameter Name="userId" SessionField="userid" Type="String" />
                            <asp:Parameter Name="la_joinDateTime" Type="DateTime" />
                            <asp:Parameter Name="la_Remarks" Type="string" />
                        </InsertParameters>
                    </asp:SqlDataSource>
                    <asp:SqlDataSource ID="AllEmployee" runat="server" ConflictDetection="CompareAllValues"
                         SelectCommand=""></asp:SqlDataSource>
                </td>
            </tr>
        </table>
        <dxe:ASPxGridViewExporter ID="exporter" runat="server">
        </dxe:ASPxGridViewExporter>
    </div>
</asp:Content>
