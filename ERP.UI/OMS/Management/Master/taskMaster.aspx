<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                24-03-2023        2.0.36           Pallab              25733 : Master pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Language="C#" Title="Task Master" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" CodeBehind="taskMaster.aspx.cs" Inherits="ERP.OMS.Management.Master.taskMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
      <style>
        /*.chosen-container.chosen-container-multi,
        .chosen-container.chosen-container-single {
            width:100% !important;
        }
        .chosen-choices {
            width:100% !important;
        }
       #ListBoxActivityType {
            width:200px;
        }
        .hide {
            display:none;
        }
        .dxtc-activeTab .dxtc-link  {
            color:#fff !important;
        }*/
    </style>
    <script type="text/javascript">
    /*$(document).ready(function () {
        BindActivityType();
    });

    function BindActivityType() {
        var lBox = $('select[id$=ListBoxActivityType]');
        var listItems = [];

        lBox.empty();
        $.ajax({
            type: "POST",
            url: 'taskMaster.aspx/GetActivityTypeList',
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
                    ListActivityType();
                    $('#ListBoxActivityType').trigger("chosen:updated");
                    $('#ListBoxActivityType').prop('disabled', false).trigger("chosen:updated");
                }
                else {
                    lBox.empty();
                    $('#ListBoxActivityType').trigger("chosen:updated");
                    $('#ListBoxActivityType').prop('disabled', true).trigger("chosen:updated");
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                console.log(textStatus);
            }
        });
    }

    function ListActivityType() {
        $('#ListBoxActivityType').chosen();
        $('#ListBoxActivityType').fadeIn();
        var config = {
            '.chsnProduct': {},
            '.chsnProduct-deselect': { allow_single_deselect: true },
            '.chsnProduct-no-single': { disable_search_threshold: 10 },
            '.chsnProduct-no-results': { no_results_text: 'Oops, nothing found!' },
            '.chsnProduct-width': { width: "100%" }
        }
        for (var selector in config) {
            $(selector).chosen(config[selector]);
        }
    }*/
        function LastCall() {
            if (grid.cpErrorMsg != null) {
                jAlert(grid.cpErrorMsg);
                grid.cpErrorMsg = null;
            }
        }

        function checkBoxList_Init()
        {
            ccheckBoxList_callBack.PerformCallback('');
           
        }
        function OnUpdateClick(editor) {
            if (ASPxClientEdit.ValidateGroup("editForm"))
                grid.UpdateEdit();
        }
    </script>
    <style>
        .dxeErrorFrameSys.dxeErrorCellSys {
            position:absolute;
        }
        
        .dxgvControl_PlasticBlue a.btn {
            color:#fff !important;
        }
        .dxbButton_PlasticBlue  div.dxb {
            padding:0px;
        }

        /*Rev 1.0*/

        select
        {
            height: 30px !important;
            border-radius: 4px;
            /*-webkit-appearance: none;
            position: relative;
            z-index: 1;*/
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

        .dxeDisabled_PlasticBlue
        {
            z-index: 0 !important;
        }

        .dxtcLite_PlasticBlue > .dxtc-stripContainer .dxtc-activeTab, .dxgvFooter_PlasticBlue
        {
            background: #1b5ea4 !important;
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

        .TableMain100 #ShowGrid , .TableMain100 #ShowGridList , .TableMain100 #ShowGridRet , .TableMain100 #EmployeeGrid , .TableMain100 #GrdEmployee,
        .TableMain100 #GrdHolidays , #cityGrid
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
                margin-top: 7px;
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

        .mt-27
        {
            margin-top: 27px !important;
        }

        .col-md-3
        {
            margin-top: 8px;
        }

        #upldBigLogo , #upldSmallLogo
        {
            width: 100%;
        }

        #DivSetAsDefault
        {
            margin-top: 25px;
        }

        .btn.btn-xs
        {
                font-size: 14px !important;
        }

        /*#ShowFilter
        {
            padding-bottom: 3px !important;
        }*/

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

<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="server">
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main">
        <div class="panel-heading">
        <div class="panel-title">
            <h3>Task Master</h3>
        </div>
    </div>
        <div class="form_main">
        <table class="TableMain100">
            <tr>
                <td>
                    <div class="SearchArea">
                        <%--Rev 1.0: "mb-10" class add --%>
                        <div class="FilterSide pull-left mb-10">
                            <% if (rights.CanAdd) { %>
                            <a href="javascript:void(0);" onclick="grid.AddNewRow();" class="btn btn-success btn-radius"><span class="btn-icon"><i class="fa fa-plus" ></i></span><span style="color: white;">Add New</span></a>
                            <% } %>
                           <% if (rights.CanExport)
                                               { %>
                             <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-radius" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Value="0">Export to</asp:ListItem>
                                <asp:ListItem Value="1">PDF</asp:ListItem>
                                <asp:ListItem Value="2">XLS</asp:ListItem>
                                <asp:ListItem Value="3">RTF</asp:ListItem>
                                <asp:ListItem Value="4">CSV</asp:ListItem>
                            </asp:DropDownList>
                              <% } %>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td class="relative">

                    <dxe:ASPxGridView ID="TaskGridView" runat="server" ClientInstanceName="grid" Width="100%" AutoGenerateColumns="False" DataSourceID="TaskDataSrc" KeyFieldName="task_id" OnRowInserting="TaskGridView_RowInserting" OnRowUpdating="TaskGridView_RowUpdating" OnStartRowEditing="TaskGridView_StartRowEditing" OnRowDeleting="TaskGridView_RowDeleting"   >
                       <SettingsSearchPanel Visible="True" Delay="5000" />
                         <SettingsPager NumericButtonCount="15" PageSize="50" ShowSeparators="True">
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                            <PageSizeItemSettings Items="50, 100, 150, 200" Visible="True">
                            </PageSizeItemSettings>
                        </SettingsPager>
                        <SettingsEditing Mode="PopupEditForm" EditFormColumnCount="1"></SettingsEditing>

                        <Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowGroupPanel="True" ShowStatusBar="Visible" />
                        <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True" />
                        <SettingsCommandButton>
                            <UpdateButton ButtonType="Button" Text="Save">
                                <Styles>
                                    <Style CssClass="btn btn-primary">
                                    </Style>
                                </Styles>
                            </UpdateButton>
                            <CancelButton ButtonType="Button" Text="Cancel">
                                <Styles>
                                    <Style CssClass="btn btn-danger">
                                    </Style>
                                </Styles>
                            </CancelButton>
                            <EditButton ButtonType="Image" Styles-Style-CssClass="pad">
                                <Image AlternateText="Edit" Url="../../../assests/images/Edit.png">
                                </Image>
                            </EditButton>
                            <DeleteButton ButtonType="Image" Styles-Style-CssClass="pad">
                                <Image Url="../../../assests/images/Delete.png">
                                </Image>
                            </DeleteButton>
                        </SettingsCommandButton>
                        <SettingsPopup>
                            <EditForm Width="350px"  HorizontalAlign="Center" VerticalAlign="WindowCenter" />
                        </SettingsPopup>
                        
                        <SettingsText ConfirmDelete="Confirm delete ?" PopupEditFormCaption="Add/Modify Task Details" />

                        <Columns>
                            <dxe:GridViewDataTextColumn FieldName="task_title" VisibleIndex="0" Caption="Task Title">
                                <PropertiesTextEdit>
                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" SetFocusOnError="True">
                                        <RequiredField ErrorText="Mandatory." IsRequired="True" />
                                    </ValidationSettings>
                                </PropertiesTextEdit>
                                <EditFormSettings Caption="Title" Visible="True" />
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="task_id" ReadOnly="True" VisibleIndex="1" Visible="False">
                                <EditFormSettings Visible="False" />
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataColumn FieldName="chk_actvt" ReadOnly="True" VisibleIndex="2" Visible="False">
                                <EditFormSettings Caption="Activities" Visible="True" />
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataColumn>

                            <dxe:GridViewCommandColumn Caption="Actions" VisibleIndex="5" ShowEditButton="true" ShowDeleteButton="true" Width="108px">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <HeaderTemplate>
                                    Actions
                                </HeaderTemplate>
                                <CellStyle HorizontalAlign="Center"></CellStyle>
                            </dxe:GridViewCommandColumn>
                        </Columns>

 
                        <Templates>
                            <EditForm>
                                <div class="">
                                    <div class="col-md-12">
                                        <label style="margin-top: 18px;">Task Title<span style="font-size: 7.5pt; color: red"><strong>*</strong></span></label>
                                        <div>
                                            <dxe:ASPxTextBox ID="ASPxTextBox1" Text='<%#Bind("task_title") %>' runat="server" Width="100%" MaxLength="255">
                                              <%--  <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" SetFocusOnError="True">
                                                    <RequiredField ErrorText="Mandatory." IsRequired="True" />
                                                </ValidationSettings>--%>
                                                
                                                 <ValidationSettings ValidationGroup="editForm" ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right">
                                                  <RequiredField IsRequired="True" ErrorText="Mandatory." />
                                                 </ValidationSettings>
                                            </dxe:ASPxTextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-12 datalis">
                                        <div>
                                            <dxe:ASPxCallbackPanel runat="server" id="checkBoxList_callBack" ClientInstanceName="ccheckBoxList_callBack" OnCallback="checkBoxList_Callback">
                                                  <PanelCollection>
                                                       <dxe:PanelContent runat="server">
                                                            <dxe:ASPxCheckBoxList id="checkBoxList" onCallBack="" ClientInstanceName="CcheckBoxList" runat="server" datasourceid="SqlDataSrcActivity"
                                                                        valuefield="aty_id" textfield="aty_activityType" repeatcolumns="4" repeatlayout="Table" caption="Select Required Activities">
                                                                        <CaptionSettings Position="Top" /> 
                                                             </dxe:ASPxCheckBoxList>
                                                      </dxe:PanelContent>
                                                  </PanelCollection>
                                                <ClientSideEvents Init="checkBoxList_Init" />
                                                </dxe:ASPxCallbackPanel>
                                        </div>
                                    </div>
                                    <div class="clear"></div>
                                    <div class="col-md-12" style="margin-top:15px;margin-bottom:25px;">
                                        <a href="javascript:void(0);" onclick="OnUpdateClick(this)" class="btn btn-primary">Save</a>
                                           <%-- <dxe:aspxgridviewtemplatereplacement id="btnUpdate" runat="server" replacementtype="EditFormUpdateButton"></dxe:aspxgridviewtemplatereplacement>--%>
                                            <dxe:aspxgridviewtemplatereplacement id="btnCancel" runat="server" replacementtype="EditFormCancelButton"></dxe:aspxgridviewtemplatereplacement>
                                    </div>
                                  
                                </div>
                                
                            </EditForm>
                        </Templates>

                        <%--<EditFormLayoutProperties ColCount="1">
                            <Items>
                                <dxe:GridViewColumnLayoutItem ColumnName="task_title">
                                </dxe:GridViewColumnLayoutItem>
                                <dxe:GridViewColumnLayoutItem ColumnName="chk_actvt">
                                    <Template>
                                        <dxe:ASPxCheckBoxList  id="checkBoxList" runat="server" datasourceid="SqlDataSrcActivity"
                                            valuefield="aty_id" textfield="aty_activityType" repeatcolumns="4" repeatlayout="Table" caption="Select Required Activities">
                                            <CaptionSettings Position="Top" />
                                        </dxe:ASPxCheckBoxList>
                                    </Template>
                                </dxe:GridViewColumnLayoutItem>
                                <dxe:EditModeCommandLayoutItem ColSpan="1" />
                            </Items>
                        </EditFormLayoutProperties>--%>
                          <clientsideevents endcallback="function(s, e) {	LastCall( );}" />
                    </dxe:ASPxGridView>

                </td>
            </tr>
        </table>
    </div>
    </div>
    <asp:SqlDataSource ID="TaskDataSrc" runat="server" 
        InsertCommand="SELECT NULL"
        SelectCommand="SELECT task_title, task_id FROM tbl_master_task"  
        UpdateCommand="SELECT NULL" 
        DeleteCommand="SELECT NULL">
        <DeleteParameters>
            <asp:Parameter Name="task_id" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="task_id" />
            <asp:Parameter Name="task_title" />
            <asp:Parameter Name="task_description" />
        </UpdateParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="SqlDataSrcActivity" runat="server"
        SelectCommand="sp_Sales" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:Parameter DefaultValue="GetActivityTypeList" Name="Mode" Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>

    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>
</asp:content>


