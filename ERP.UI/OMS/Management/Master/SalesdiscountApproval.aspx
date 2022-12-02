<%@ Page Title="Sales Discount Approver's" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="SalesdiscountApproval.aspx.cs" Inherits="ERP.OMS.Management.Master.SalesdiscountApproval" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .dxgvEditFormDisplayRow_PlasticBlue td.dxgv, .dxgvDataRow_PlasticBlue td.dxgv, .dxgvDataRowAlt_PlasticBlue td.dxgv, .dxgvSelectedRow_PlasticBlue td.dxgv, .dxgvFocusedRow_PlasticBlue td.dxgv {
            overflow: visible !important;
        }

        .dxgvControl_PlasticBlue td.dxgvBatchEditModifiedCell_PlasticBlue {
            background: #fff !important;
        }

        #SalesapprovalGrid_DXEditor4_I {
            display: none !important;
        }
        #SalesapprovalGrid_DXMainTable>tbody>tr>td:last-child {
            display:none !important;
        }
    </style>
    <script type="text/javascript">
        function callMobile() {
            window.location.href = "Mobileaccessconfiguration.aspx";
        }

        $(document).ready(function () {
            debugger;
            cSalesapprovalGrid.GetEditor('branch_description').SetEnabled(false);
            gl.SetEnabled(false);
            //glm.SetEnabled(false);
            //glf.SetEnabled(false);
        })

        //var globalRowIndex;
        //function GetVisibleIndex(s, e) {
        //    globalRowIndex = e.visibleIndex;
        //}
        //RowClick = "GetVisibleIndex"


        function OnCmblevel1SelectedIndexChanged(level1id) { }

        function OnAllowEditChanged(s, e) {
            debugger;
            if (s.GetValue() == true) {
                                            
               //cSalesapprovalGrid.GetEditor('branch_description').SetEnabled(true);
                gl.SetEnabled(true);
                //gl.SetFocus;
               //glm.SetEnabled(true);
                //glf.SetEnabled(true);

                //gl.SetFocusedRowIndex.SetEnabled(true);

            } else if (s.GetValue() == false) {

                //cSalesapprovalGrid.GetEditor('branch_description').SetEnabled(false);
                gl.SetEnabled(false);
                //glm.SetEnabled(false);
                //glf.SetEnabled(false);
            }
            else {
                //cSalesapprovalGrid.GetEditor('branch_description').SetEnabled(false);
                gl.SetEnabled(false);
                //glm.SetEnabled(false);
                //glf.SetEnabled(false);
            }
        }
        function saveandupdate() {
            debugger;
            cSalesapprovalGrid.UpdateEdit();
            //cSalesapprovalGrid.PerformCallback();
        }
        function callnewform() {
            window.location.href = 'newformtest.aspx';
        }
        function callnewform2() {
            window.location.href = 'newformtest2.aspx';
        }

        function ShowError(obj) {
            //IntializeGlobalVariables(s);
            if (cSalesapprovalGrid.cperroremssg != null) {
                jAlert(cSalesapprovalGrid.cperroremssg);
                cSalesapprovalGrid.cperroremssg = null;
                //location.reload();
            }
            if (cSalesapprovalGrid.cpupdatemssg != null) {
                jAlert(cSalesapprovalGrid.cpupdatemssg);
                cSalesapprovalGrid.cpupdatemssg = null;
                location.reload();
            }
            if (cSalesapprovalGrid.cpupdatemssg != null) {
                jAlert(cSalesapprovalGrid.cpupdatemssg);
                cSalesapprovalGrid.cpupdatemssg = null;
            }
            if (cSalesapprovalGrid.cpDelete != null) {
                if (cSalesapprovalGrid.cpDelete == 'Success') {
                    jAlert('Deleted Successfully');
                    cSalesapprovalGrid.cpDelete = null;
                }
                else {
                    jAlert('Used in other module.Can not delete');
                    cSalesapprovalGrid.cpDelete = null;
                }

            }
        }

        function OnCustomButtonClick(s, e) {
            //alert("Out--CustomDelete");
            if (e.buttonID == 'CustomDelete') {
                //alert("CustomDelete");
                cSalesapprovalGrid.batchEditApi.EndEdit();
                cSalesapprovalGrid.DeleteRow(e.visibleIndex);
            }
        }
      
        //function AddBatchNew(s, e) {
        //    var keyCode = ASPxClientUtils.GetKeyCode(e.htmlEvent);
        //    if (keyCode === 13) {

        //        var mainAccountValue = (cSalesapprovalGrid.GetEditor('branched').GetValue() != null) ? cSalesapprovalGrid.GetEditor('branched').GetValue() : "";
        //        if (mainAccountValue != "") {

        //            gl.GetGridView().UnselectAllRowsOnPage();
        //            cSalesapprovalGrid.AddNewRow();
        //            cSalesapprovalGrid.SetFocusedRowIndex();
        //        } else {
        //            var selct = cSalesapprovalGrid.GetEditor('active').GetValue();
        //            if (selct.GetValue() == true) {


        //                //gl.SetFocusedRowIndex.SetEnabled(true);
        //                cSalesapprovalGrid.GetEditor('active').SetValue(true);

        //            } else if (selct.GetValue() == false) {

        //                cSalesapprovalGrid.GetEditor('active').SetValue(false);
        //            }
        //            else {
        //                cSalesapprovalGrid.GetEditor('active').SetValue(false);
        //            }
        //            gl.GetGridView().UnselectAllRowsOnPage();
        //        }
        //    }
        //    else if (keyCode === 9) {
        //        // document.getElementById("txtNarration").focus();
        //    }
        //}
        var prevColumnIndex;
        var prevColumnNameS;
        var prevMColumnIndex;
        var prevColumnNameM;
        var prevFColumnIndex;
        var prevColumnNameF;
      
        function test(s, e) { 
            if (e.column.fieldName == "Salesman") {
                prevColumnNameS = "Salesman";
                prevColumnIndex = e.column.index;
                //s.GetRowValues(1, 'Salesman', OnGetRowValues);
                //s.SetEditValue('userID', gl.GetGridView().GetSelectedKeysOnPage())
                //s.SetEditValue('userName', glm.GetText())
             
            }
          
            if (e.column.fieldName == "Manager") {
                prevColumnNameM = "Manager";
                prevMColumnIndex = e.column.index;
            }
        }
      
        function onBatchEditRowValidating(s, e) { 
            gl.GetGridView().UnselectAllRowsOnPage();
            return true;
        }
      
        function OnStartEditing(s, e) { 
            if (e.focusedColumn.index == 2 || e.focusedColumn.index == 3 || e.focusedColumn.index == 4) {

            
            if (e.rowValues[e.focusedColumn.index].text!="")
            {
                //gl.SetText = e.rowValues[e.focusedColumn.index].text;
                //gl.SetValue = e.rowValues[e.focusedColumn.index].value;
                gl.gridView.SelectItemsByKey(e.rowValues[e.focusedColumn.index].value,true)

            }
            }
            //if (e.focusedColumn.fieldName == "Salesman") {
            //    //cSalesapprovalGrid.GetEditor('Approvers').SetEnabled(false);
            //    prevColumnIndex = e.focusedColumn.index;
            //    gl.GetGridView().UnselectAllRowsOnPage();
            //    gl.SetValue(e.rowValues[e.focusedColumn.index].value);
            //    prevColumnIndex = e.focusedColumn.index;

            //}

            //if (e.focusedColumn.fieldName == "Manager") {

            //    //cSalesapprovalGrid.GetEditor('Approvers').SetEnabled(false);
            //    prevColumnIndex = e.focusedColumn.index;
            //    glm.GetGridView().UnselectAllRowsOnPage();
            //    glm.SetValue(e.rowValues[e.focusedColumn.index].value);
            //    prevColumnIndex = e.focusedColumn.index;

            //}
            //if (e.focusedColumn.fieldName == "Financer") {

            //    //cSalesapprovalGrid.GetEditor('Approvers').SetEnabled(false);
            //    prevColumnIndex = e.focusedColumn.index;
            //    glf.GetGridView().UnselectAllRowsOnPage();
            //    glf.SetValue(e.rowValues[e.focusedColumn.index].value);
            //    prevColumnIndex = e.focusedColumn.index;

            //}
        }

        function OnEndEditing(s, e) { 
            if (prevColumnIndex == null && prevMColumnIndex == null && prevFColumnIndex == null) {
                return;
            }
            else {

                if (prevColumnIndex != null) {
                    if (gl.GetText() != "") {
                        e.rowValues[prevColumnIndex].value = gl.GetGridView().GetSelectedKeysOnPage();
                        e.rowValues[prevColumnIndex].text = gl.GetText();
                        //gl.GetGridView().UnselectAllRowsOnPage();

                        //gl.SetValue('');
                    }
                }
                if (prevMColumnIndex != null) {
                    e.rowValues[prevMColumnIndex].value = glm.GetGridView().GetSelectedKeysOnPage();
                    e.rowValues[prevMColumnIndex].text = glm.GetText();
                    glm.GetGridView().UnselectAllRowsOnPage();
                    glm.SetValue('');
                }
                //if (prevFColumnIndex == null) return;
                //e.rowValues[prevFColumnIndex].value = glf.GetGridView().GetSelectedKeysOnPage();
                //e.rowValues[prevFColumnIndex].text = glf.GetText();
                prevColumnIndex = null;
                prevMColumnIndex = null;

            }


            // if (prevColumnIndex == null) return;
            // e.rowValues[prevColumnIndex].value = gl.GetGridView().GetSelectedKeysOnPage();
            // e.rowValues[prevColumnIndex].text = gl.GetText();
            // gl.GetGridView().UnselectAllRowsOnPage();
            // gl.SetValue('');

            //if (prevMColumnIndex == null) return;
            // e.rowValues[prevMColumnIndex].value = glm.GetGridView().GetSelectedKeysOnPage();
            // e.rowValues[prevMColumnIndex].text = glm.GetText();
            // glm.GetGridView().UnselectAllRowsOnPage();
            // glm.SetValue('');

            //if (prevFColumnIndex == null) return;
            //e.rowValues[prevFColumnIndex].value = glf.GetGridView().GetSelectedKeysOnPage();
            //e.rowValues[prevFColumnIndex].text = glf.GetText();

            prevColumnIndex = null;
            prevMColumnIndex = null;
            //prevFColumnIndex = null;
        }

        function CheckBoxListClient_Init(s, e) { }

        function CloseGridLookup() {
            gl.ConfirmCurrentSelection();
            gl.HideDropDown();
            gl.Focus();

        }
        function CloseMGridLookup() {
            glm.ConfirmCurrentSelection();
            glm.HideDropDown();
            glm.Focus();

        }
        function CloseFGridLookup() {
            glf.ConfirmCurrentSelection();
            glf.HideDropDown();
            glf.Focus();

        }
        function Onlookupclick(s, e) {
            //debugger;
            //var gv = s.GetGridView();
            //var curentbranches = cSalesapprovalGrid.GetEditor('branched').GetValue();
            //if (curentbranches != null || curentbranches != "") {
            //    s.SetEnabled(true);

            //} else { s.SetEnabled(false); }
            

        }

     
        function OnEndCallbacks(s, e) { 
            var gv = s.GetGridView();
            if (gv.cpValueChanged) {

                cSalesapprovalGrid.GetEditor("Textdata").SetValue(gv.cpuserID);

            }
        }

        function OnMlookupclick(s, e) {
            //alert();
            var gv = s.GetGridView();
            var curentbranches = cSalesapprovalGrid.GetEditor('branched').GetValue();
            if (curentbranches != null || curentbranches != "") {
                s.SetEnabled(true);

            } else { s.SetEnabled(false); }


        }

        function OnMEndCallbacks(s, e) {
            var gv = s.GetGridView();
            if (gv.cpValueChanged) {

                cSalesapprovalGrid.GetEditor("Textdatamanager").SetValue(gv.cpuserID);

            }
        }

        function OnFlookupclick(s, e) {
            //alert();
            var gv = s.GetGridView();
            var curentbranches = cSalesapprovalGrid.GetEditor('branched').GetValue();
            if (curentbranches != null || curentbranches != "") {
                s.SetEnabled(true);

            } else { s.SetEnabled(false); }


        }

        function OnFEndCallbacks(s, e) {
            var gv = s.GetGridView();
            if (gv.cpValueChanged) {

                cSalesapprovalGrid.GetEditor("Textdatafinancer").SetValue(gv.cpuserID);

            }
        }

        function Oninit(s, e) {

            //if (s.GetValue() == true) {

            //    // cSalesapprovalGrid.GetEditor('branched').SetEnabled(true);
            //    //gl.SetFocusedRowIndex.SetEnabled(true);
            //    s.SetValue(true);

            //} else if (s.GetValue() == false) {

            //    s.SetValue(false);
            //}
            //else {
            //    s.SetValue(false);
            //}
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title" id="td_contact1" runat="server">

            <h3>
                <asp:Label ID="lblHeadTitle" runat="server" Text="Mobile Access Configuration"></asp:Label>
            </h3>
        </div>

    </div>
    <div class="form_main">
        <div class="SearchArea">
            <div class="FilterSide">

                <div class="pull-left">
                  
                    <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnChange="if(!AvailableExportOption()){return false;}" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Value="0">Export to</asp:ListItem>
                        <asp:ListItem Value="1">PDF</asp:ListItem>
                        <asp:ListItem Value="2">XLS</asp:ListItem>
                        <asp:ListItem Value="3">RTF</asp:ListItem>
                        <asp:ListItem Value="4">CSV</asp:ListItem>

                    </asp:DropDownList>
                     
                </div>
            </div>

        </div>
        <dxe:ASPxGridView ID="SalesapprovalGrid" runat="server" KeyFieldName="id" AutoGenerateColumns="False" OnParseValue="Grid_ParseValue" OnCustomColumnDisplayText="Grid_CustomColumnDisplayText"
            Width="100%" ClientInstanceName="cSalesapprovalGrid" OnRowInserting="SalesapprovalGrid_RowInserting" OnRowUpdating="SalesapprovalGrid_RowUpdating" OnRowDeleting="SalesapprovalGrid_RowDeleting"
            OnCustomCallback="SalesapprovalGrid_CustomCallback" OnDataBinding="SalesapprovalGrid_DataBinding" OnBatchUpdate="SalesapprovalGrid_BatchUpdate"
            ClientSideEvents-BatchEditTemplateCellFocused="test"  ClientSideEvents-BatchEditRowValidating="onBatchEditRowValidating">


            <SettingsEditing Mode="Batch" NewItemRowPosition="Bottom">
                <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="row" />
            </SettingsEditing>
            <Settings ShowGroupPanel="false" ShowStatusBar="Hidden" ShowTitlePanel="false" />
            <SettingsDataSecurity AllowDelete="False" />
            <SettingsPager PageSizeItemSettings-ShowAllItem="true" AlwaysShowPager="false" Mode="ShowAllRecords">
                <PageSizeItemSettings ShowAllItem="True"></PageSizeItemSettings>
            </SettingsPager>
            <Columns>
              

                <dxe:GridViewDataCheckColumn Caption="Active" VisibleIndex="1" FieldName="active" Width="5px">
                    <PropertiesCheckEdit>
                        <ClientSideEvents CheckedChanged="OnAllowEditChanged" GotFocus="OnAllowEditChanged" Init="Oninit" />

                    </PropertiesCheckEdit>

                </dxe:GridViewDataCheckColumn>
          
                   <dxe:GridViewDataTextColumn Caption="Branches" FieldName="branch_description" VisibleIndex="2" Width="220px">
                 
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataComboBoxColumn FieldName="Salesman" VisibleIndex="3" Caption="Salesman">
                    <EditItemTemplate >
                        <dxe:ASPxGridLookup ID="glCategory" runat="server" AutoGenerateColumns="False" DataSourceID="Sqluserlist" ClientInstanceName="gl"
                            KeyFieldName="userID" MultiTextSeparator=", " OnInit="glCategory_Init"
                            TextFormatString="{1}" Width="260px" SelectionMode="Multiple" OnValueChanged="glCategory_ValueChanged">
                            <GridViewProperties>
                                <SettingsBehavior AllowFocusedRow="false" ProcessSelectionChangedOnServer="false" />
                                
                            </GridViewProperties>
                            <ClientSideEvents KeyPress="Onlookupclick" EndCallback="OnEndCallbacks"/>
                            <Columns>
                                <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Caption=" " VisibleIndex="0" Width="50px"/>
                                <dxe:GridViewDataTextColumn FieldName="userID" ReadOnly="True" Width="100px"
                                    Visible="False" VisibleIndex="1">
                                    <EditFormSettings Visible="False" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="userName" VisibleIndex="2" Caption="Salesman's" Width="200px">
                                </dxe:GridViewDataTextColumn>

                            </Columns>
                            <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">

                                <Templates>
                                    <StatusBar>
                                        <table class="OptionsTable" style="float: right">
                                            <tr>
                                                <td>
                                                    <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridLookup" />
                                                </td>
                                            </tr>
                                        </table>
                                    </StatusBar>
                                </Templates>
                                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                            </GridViewProperties>
                        </dxe:ASPxGridLookup>

                    </EditItemTemplate>

                </dxe:GridViewDataComboBoxColumn>

                <%-- <dxe:GridViewDataComboBoxColumn FieldName="Manager" VisibleIndex="4" Caption="Manager">
                      <EditItemTemplate>
                           <dxe:ASPxGridLookup ID="glManager" runat="server" AutoGenerateColumns="False" DataSourceID="Sqluserlist" ClientInstanceName="glm"
                            KeyFieldName="userID" MultiTextSeparator=", " OnInit="glManager_Init"
                            TextFormatString="{1}" Width="260px" SelectionMode="Multiple" OnValueChanged="glManager_ValueChanged">
                            <GridViewProperties>
                                <SettingsBehavior AllowFocusedRow="false" ProcessSelectionChangedOnServer="false" />
                                
                            </GridViewProperties>
                            <ClientSideEvents KeyPress="OnMlookupclick" EndCallback="OnMEndCallbacks"  />
                            <Columns>
                                <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Caption=" " VisibleIndex="0" Width="50px"/>
                                <dxe:GridViewDataTextColumn FieldName="userID" ReadOnly="True" Width="100px"
                                    Visible="False" VisibleIndex="1">
                                    <EditFormSettings Visible="False" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="userName" VisibleIndex="2" Caption="Manager's" Width="200px">
                                </dxe:GridViewDataTextColumn>

                            </Columns>
                            <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">

                                <Templates>
                                    <StatusBar>
                                        <table class="OptionsTable" style="float: right">
                                            <tr>
                                                <td>
                                                    <dxe:ASPxButton ID="Close1" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseMGridLookup" />
                                                </td>
                                            </tr>
                                        </table>
                                    </StatusBar>
                                </Templates>
                                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                            </GridViewProperties>
                        </dxe:ASPxGridLookup>
                      </EditItemTemplate>
               
                </dxe:GridViewDataComboBoxColumn>--%>

               <%--  <dxe:GridViewDataColumn FieldName="Financer" VisibleIndex="5" Caption="Financer">
                    <EditItemTemplate>
                        <dxe:ASPxGridLookup ID="glFinancer" runat="server" AutoGenerateColumns="False" DataSourceID="Sqluserlist" ClientInstanceName="glf"
                            KeyFieldName="userID" MultiTextSeparator=", "
                            TextFormatString="{1}" Width="260px" SelectionMode="Multiple" OnValueChanged="glFinancer_ValueChanged">
                            <GridViewProperties>
                                <SettingsBehavior AllowFocusedRow="false" ProcessSelectionChangedOnServer="false" />
                                
                            </GridViewProperties>
                            <ClientSideEvents KeyPress="OnFlookupclick" EndCallback="OnFEndCallbacks"  />
                            <Columns>
                                <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Caption=" " VisibleIndex="0" Width="50px"/>
                                <dxe:GridViewDataTextColumn FieldName="userID" ReadOnly="True" Width="100px"
                                    Visible="False" VisibleIndex="1">
                                    <EditFormSettings Visible="False" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="userName" VisibleIndex="2" Caption="Approver's" Width="200px">
                                </dxe:GridViewDataTextColumn>

                            </Columns>
                            <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">

                                <Templates>
                                    <StatusBar>
                                        <table class="OptionsTable" style="float: right">
                                            <tr>
                                                <td>
                                                    <dxe:ASPxButton ID="Close2" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseFGridLookup" />
                                                </td>
                                            </tr>
                                        </table>
                                    </StatusBar>
                                </Templates>
                                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                            </GridViewProperties>
                        </dxe:ASPxGridLookup>

                    </EditItemTemplate>

                </dxe:GridViewDataColumn>--%>
                <dxe:GridViewDataColumn FieldName="Textdata" Caption="" CellStyle-CssClass="hide" VisibleIndex="6">
                    <HeaderStyle CssClass="hide" />
                    <CellStyle CssClass="hide"></CellStyle>
                    <EditCellStyle CssClass="hide"></EditCellStyle>
                    <EditFormCaptionStyle CssClass="hide"></EditFormCaptionStyle>
                    <FilterCellStyle CssClass="hide"></FilterCellStyle>
                    <FooterCellStyle CssClass="hide"></FooterCellStyle>
                    <GroupFooterCellStyle CssClass="hide"></GroupFooterCellStyle>

                </dxe:GridViewDataColumn>
                 <dxe:GridViewDataColumn FieldName="Textdatamanager" Caption="" CellStyle-CssClass="hide" VisibleIndex="7">
                    <HeaderStyle CssClass="hide" />
                    <CellStyle CssClass="hide"></CellStyle>
                    <EditCellStyle CssClass="hide"></EditCellStyle>
                    <EditFormCaptionStyle CssClass="hide"></EditFormCaptionStyle>
                    <FilterCellStyle CssClass="hide"></FilterCellStyle>
                    <FooterCellStyle CssClass="hide"></FooterCellStyle>
                    <GroupFooterCellStyle CssClass="hide"></GroupFooterCellStyle>

                </dxe:GridViewDataColumn>
                 <dxe:GridViewDataColumn FieldName="Textdatafinancer" Caption="" CellStyle-CssClass="hide" VisibleIndex="8">
                    <HeaderStyle CssClass="hide" />
                    <CellStyle CssClass="hide"></CellStyle>
                    <EditCellStyle CssClass="hide"></EditCellStyle>
                    <EditFormCaptionStyle CssClass="hide"></EditFormCaptionStyle>
                    <FilterCellStyle CssClass="hide"></FilterCellStyle>
                    <FooterCellStyle CssClass="hide"></FooterCellStyle>
                    <GroupFooterCellStyle CssClass="hide"></GroupFooterCellStyle>
                </dxe:GridViewDataColumn>
                 <dxe:GridViewDataTextColumn Caption="Branches" FieldName="branched" Visible="false" Width="220px">
                   
                 </dxe:GridViewDataTextColumn>
            </Columns>
            <ClientSideEvents  EndCallback="function(s,e) { ShowError(s.cpInsertError);}" CustomButtonClick="OnCustomButtonClick" BatchEditStartEditing="OnStartEditing"  BatchEditEndEditing="OnEndEditing" />
        </dxe:ASPxGridView>


    </div>
    <br />
    <div>
         <% if (rights.CanAdd && rights.CanEdit)
                       { %>
        <dxe:ASPxButton ID="btnSaveRecords" ClientInstanceName="cbtnSaveRecords" runat="server"
            AccessKey="S" AutoPostBack="false" CssClass="btn btn-primary" TabIndex="0" Text="Save"
            UseSubmitBehavior="False">
            <ClientSideEvents Click="function(s, e) {saveandupdate();}" />
        </dxe:ASPxButton>
            <% } %>
    </div>
    <div>
        <dxe:ASPxButton ID="ASPxButton1" ClientInstanceName="cbtnSaveRecords" runat="server"
            AccessKey="S" AutoPostBack="false" CssClass="btn btn-primary" TabIndex="0" Text="Mobile"
            UseSubmitBehavior="False">
            <ClientSideEvents Click="function(s, e) {callMobile();}" />
        </dxe:ASPxButton>
    </div>
    <div>
      
    </div>
    <%--<asp:ObjectDataSource ID="Sqluserlist" runat="server" TypeName="DataProvider"
        SelectMethod="Getusers" />
         <asp:ObjectDataSource ID="LookupDataSource" runat="server" TypeName="DataProvider"
            SelectMethod="Getusers" />--%>
    <asp:SqlDataSource ID="Sqluserlist" runat="server" 
        SelectCommand="select user_id as userID ,user_name as userName from tbl_master_user  order by user_name asc"></asp:SqlDataSource>
    

    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>
</asp:Content>
