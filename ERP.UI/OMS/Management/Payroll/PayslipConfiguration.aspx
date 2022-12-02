<%@ Page Title="Payslip Configuration" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="PayslipConfiguration.aspx.cs" Inherits="ERP.OMS.Management.Payroll.PayslipConfiguration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/SearchPopup.css" rel="stylesheet" />
    <script src="JS/SearchPopup.js"></script>

    <style>
        .panel-body {
            border: 1px solid #ccc;
            border-top: 3px solid #3D5294;
        }
    </style>

    <script type="text/javascript">
        function txtPayStructure_KeyDown(s, e) {
            if (e.htmlEvent.key == "Enter") {
                s.OnButtonClick(0);
            }
        }

        function txtPayStructure_Click(s, e) {
            document.getElementById('hdnInputType').value = "PayStructure";
            document.getElementById("txtSearchBox").value = "";

            var txt = "<table border='1' width=\"100%\" class=\"dynamicPopupTbl\"><tr class=\"HeaderStyle\"><th class=\"hide\">id</th><th>>Pay Structure Name</th></tr><table>";
            document.getElementById("TableStructure").innerHTML = txt;

            BindModalGrid('');
            setTimeout(function () { $("#txtSearchBox").focus(); }, 500);
            $('#ModelPopup').modal('show');
        }

        
        function txtSearchBoxkeydown(e) {
            if (e.code == "Enter" || e.code == "NumpadEnter") {
                if ($("#txtSearchBox").val() != '') {
                    BindModalGrid($("#txtSearchBox").val());
                }
            }
            else if (e.code == "ArrowDown") {
                if ($("input[searchIndex=0]"))
                    $("input[searchIndex=0]").focus();
            }
        }
        
        //function PayStructure_TextChange() {
        //    document.getElementById('hdnInputType').value = "EmployeeTable";
           
        //    BindModalGrid('EmployeeTable');
            
        //}
        
        function Gridlookup_DesignName_EndCallBack() {
            cGridlookup_DesignName.Refresh();
        }

        function Gridlookup_DesignName_gotFocus() {
            //cGridlookup_DesignName.gridView.Refresh();
            //var startDate = new Date();
            //startDate = tDate.GetDate().format('yyyy-MM-dd');
            cGridlookup_DesignName.ShowDropDown();
        }
        function Gridlookup_DesignName_LostFocus() {
            //var text = cGridlookup_DesignName();
            //$("#divDesignName").attr("title", text);
            //$("#divDesignName").attr("data-original-title", text);
            //$("#divDesignName").tooltip({
            //    customClass: 'tooltip-custom',
              //  template: '<div class="tooltip CUSTOM-CLASS" role="tooltip"><div class="arrow"></div><div class="tooltip-inner"></div></div>'
           // });
        }

        function onDelete(payConfig_ID) {

            cViewGrid.PerformCallback('Delete' + '|' + payConfig_ID );
        }

        function onEdit(payConfig_ID) {
            $("#hdnPayConfig_ID").val(payConfig_ID);
            
            cViewGrid.PerformCallback('Edit' + '|' + payConfig_ID);
        }

    </script>
    <script>
        function BindModalGrid(SearchKey) {
            var SearchType = document.getElementById('hdnInputType').value;

            if (SearchType == "PayStructure") {
                var OtherDetails = {}
                OtherDetails.SearchKey = SearchKey;

                var HeaderCaption = [];
                HeaderCaption.push("Pay Structure Name");

                callonServerScroll("Services/Payroll_Master.asmx/GetPayStructureList", OtherDetails, "TableStructure", HeaderCaption, "searchIndex", "SetDropdownValue");
            }
            
        }

        function SetDropdownValue(Id, Name, e) {
            var SearchType = document.getElementById('hdnInputType').value;

            if (SearchType == "PayStructure") {
                var PayStructureID = Id;
                var PayStructureName = Name;

                if (PayStructureID != "") {
                    $('#ModelPopup').modal('hide');

                    txtPayStructure.SetText(PayStructureName);
                    document.getElementById('hdnPayStructureID').value = Id;
                    //txtEmployeeNmae.SetFocus(true);


                    //txtEmployeeNmae.SetText("");
                    //txtPeriod.SetText("");
                    //document.getElementById('hdnEmployeeID').value = "";
                    //document.getElementById('hdnPeriod').value = "";
                    
                  //  PayStructure_TextChange();
                    
                }
            }
          }

        function ValueSelected(e, indexName) {
            if (e.code == "Enter") {
                var Id = e.target.parentElement.parentElement.cells[0].innerText;
                var name = e.target.parentElement.parentElement.cells[1].children[0].value;
                if (Id) {
                    if (indexName == "searchIndex") {
                        SetDropdownValue(Id, name, e.target.parentElement);
                    }
                }
            }

            else if (e.code == "ArrowDown") {
                thisindex = parseFloat(e.target.getAttribute(indexName));
                thisindex++;
                //if (thisindex < 10)
                $("input[" + indexName + "=" + thisindex + "]").focus();
            }
            else if (e.code == "ArrowUp") {
                thisindex = parseFloat(e.target.getAttribute(indexName));
                thisindex--;
                if (thisindex > -1)
                    $("input[" + indexName + "=" + thisindex + "]").focus();
                else {
                    if (indexName == "searchIndex")
                        $('#txtSearchBox').focus();
                }
            }
        }
    </script>
    <script>
        function btnPayslipConfigAdd_Click() {
            var PayStructureID = document.getElementById('hdnPayStructureID').value;
            var DefaultDesign = cCmbDefaultDesignName.GetValue() ;
            var hdnPayConfig_ID = document.getElementById('hdnPayConfig_ID').value;
           
            if (hdnPayConfig_ID != '') {
                cViewGrid.PerformCallback('Update' + '|' + PayStructureID + '|' + DefaultDesign + '|' + hdnPayConfig_ID);

            }
            else {
                cViewGrid.PerformCallback('Add' + '|' + PayStructureID + '|' + DefaultDesign);
            }
            
        }
        function Gridlookup_DesignName_onEndcallback(s, e) {
            // test
        }

        function ViewGridonEndcallback(s, e)
        {
            if (s.cpInsertSuccessOrFail == 'duplicate') {
                jAlert("Configuration already exist with this Pay Structure.");
            }
            else if (s.cpInsertSuccessOrFail == 'successInsert' || s.cpInsertSuccessOrFail == 'successDelete') {

                txtPayStructure.SetText('');
                $("#hdnPayStructureID").val('');
                cCmbDefaultDesignName.SetSelectedIndex(0);

                //cGridlookup_DesignName.gridView.PerformCallback('Add')
                cPanellookup_DesignName.PerformCallback('Add')

                cViewGrid.Refresh();
                
            }
            else if (s.cpInsertSuccessOrFail == 'failInsert' || s.cpInsertSuccessOrFail == 'failDelete') {
                jAlert("Please try after sometimes");
            }
            else if (s.cpInsertSuccessOrFail == 'Edit') {
                txtPayStructure.SetText(s.cpPayStructure);
                $("#hdnPayStructureID").val(s.cpHdnPayStructureID);
                cCmbDefaultDesignName.SetText(s.cpDefaultDesignName);
                txtPayStructure.SetEnabled(false);

                // cGridlookup_DesignName.gridView.PerformCallback('Edit' + '|' + s.cpSelectedDesignName)
                cPanellookup_DesignName.PerformCallback('Edit' + '|' + s.cpSelectedDesignName)
            }
            else if (s.cpInsertSuccessOrFail == 'successUpdate' || s.cpInsertSuccessOrFail == 'failUpdate') {
                $("#hdnPayConfig_ID").val('');
                txtPayStructure.SetText('');
                $("#hdnPayStructureID").val('');
                cCmbDefaultDesignName.SetSelectedIndex(0);
                txtPayStructure.SetEnabled(true);
                
                cPanellookup_DesignName.PerformCallback('Add');

                cViewGrid.Refresh();
            }
            else if (s.cpInsertSuccessOrFail == 'emptyPayStructure') {
                jAlert("Please select a Pay Structure");
            }
            else if (s.cpInsertSuccessOrFail == 'emptyDefaultDesign') {
                jAlert("Please select a Default Design");
            }
            else if (s.cpInsertSuccessOrFail == 'noSelectedDesign') {
                jAlert("Please select at least one Design");
            }

            s.cpInsertSuccessOrFail = '';
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title clearfix">
            <h3 class="pull-left">Payslip Configuration</h3>
        </div>
    </div>
    <div class="panel-body">
        <div class="row">
            <div class="col-md-3">
                <label>Pay Structure</label>
                 <span style="color: red">*</span>
                <dxe:ASPxButtonEdit ID="txtPayStructure" runat="server" ReadOnly="true" ClientInstanceName="txtPayStructure" Width="100%" AutoPostBack="false" >
                    <Buttons>
                        <dxe:EditButton>
                        </dxe:EditButton>
                    </Buttons>
                    <ClientSideEvents ButtonClick="function(s,e){txtPayStructure_Click();}" KeyDown="function(s,e){txtPayStructure_KeyDown(s,e);}" />
                </dxe:ASPxButtonEdit>
            </div>
            
            <div class="col-md-3" id="divDesignName" runat="server">
                <label id="Label27" runat="server">Select Design</label>
                 <span style="color: red">*</span>
                <dxe:ASPxCallbackPanel runat="server" ID="Panellookup_DesignName" ClientInstanceName="cPanellookup_DesignName" OnCallback="Panellookup_DesignName_Callback1" >
                    <PanelCollection>
                        <dxe:PanelContent runat="server">
                            <asp:HiddenField runat="server" ID="OldSelectedKeyvalue" />
                            <dxe:ASPxGridLookup ID="Gridlookup_DesignName" SelectionMode="Multiple" runat="server" ClientInstanceName="cGridlookup_DesignName"
                                OnDataBinding="Gridlookup_DesignName_DataBinding"  
                                ClientSideEvents-EndCallback="Gridlookup_DesignName_onEndcallback"
                                KeyFieldName="DesignName" Width="500" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", " >
                                <Columns>
                                    <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="100" Caption=" " />
                                    <dxe:GridViewDataColumn FieldName="DesignName" Visible="true" VisibleIndex="1" Caption="Design Name" Settings-AutoFilterCondition="Contains" Width="400" />
                                     <dxe:GridViewDataTextColumn FieldName="FullDesignName" Caption="hidden Field Id" Width="0" HeaderStyle-CssClass="hide">
                                       </dxe:GridViewDataTextColumn>   
                                </Columns>
                                <GridViewProperties Settings-VerticalScrollBarMode="Auto">
                                    <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" />
                                    <SettingsPager Mode="ShowPager" PageSize="10" Visible="true">
                                        <PageSizeItemSettings Items="10,20,30"></PageSizeItemSettings>
                                    </SettingsPager>
                                    <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                </GridViewProperties>
                                <ClientSideEvents GotFocus="function(s, e) { Gridlookup_DesignName_gotFocus();}" LostFocus="function(s, e) { Gridlookup_DesignName_LostFocus();}" />
                            </dxe:ASPxGridLookup>
                        </dxe:PanelContent>
                    </PanelCollection>
                    <ClientSideEvents EndCallback="Gridlookup_DesignName_EndCallBack" />
                </dxe:ASPxCallbackPanel>
            </div>

            <div class="col-md-3">
                <label>Select Default Design</label>
                 <span style="color: red">*</span>
                <dxe:ASPxComboBox ID="CmbDefaultDesignName" ClientInstanceName="cCmbDefaultDesignName" runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True" >
                </dxe:ASPxComboBox>
            </div>

            <div class="col-md-3">
                <label> </label>
                <button type="button" id="btnAdd" onclick="btnPayslipConfigAdd_Click()" class="btn btn-primary">Update</button>
                
            </div>

            <div class="clearfix"></div>
            <div class="row">
                
                <div class="col-md-12" style="padding-top: 27px;">
                    <dxe:ASPxGridView ID="ViewGrid" ClientInstanceName="cViewGrid"  runat="server" KeyFieldName="payConfig_ID" AutoGenerateColumns="False"
                        
                        OnCustomCallback="ViewGrid_CustomCallback" OnDataBinding="ViewGrid_DataBinding"  ClientSideEvents-EndCallback="ViewGridonEndcallback"
                        Width="100%" 
                        Settings-VerticalScrollableHeight="280" Settings-VerticalScrollBarMode="Auto"
                        SettingsBehavior-AllowFocusedRow="true" Settings-HorizontalScrollBarMode="Visible"
                        SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" 
                        >
                        
                        <SettingsSearchPanel Visible="True" Delay="5000" />
                        <Settings ShowFilterRow="true" ShowGroupPanel="true" ShowFilterRowMenu="true" />
                        
                        <Columns>
                            <dxe:GridViewDataTextColumn Visible="True" ReadOnly="True" VisibleIndex="0" FieldName="StructureName" Caption="Pay Structure" Width="25%">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Visible="True" ReadOnly="True" VisibleIndex="1" FieldName="SelectedDesign" Caption="Design" Width="50%">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Visible="True" ReadOnly="True" VisibleIndex="2" FieldName="DefaultDesign" Caption="Default Design" Width="25%">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="9" Width="15%">
                                <DataItemTemplate>
                                    <a href="javascript:void(0);" onclick="onEdit('<%# Container.KeyValue %>')" class="pad" title="Edit">
                                    <img src="../../../assests/images/Edit.png" />
                                    </a>
                                    <a href="javascript:void(0);" onclick="onDelete('<%# Container.KeyValue %>')" class="pad" title="Delete">
                                    <img src="../../../assests/images/Delete.png" />
                                    </a>
                                </DataItemTemplate>
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                <HeaderTemplate><span>Actions</span></HeaderTemplate>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="FullDefaultDesign" Caption="hidden Field Id" Width="0" HeaderStyle-CssClass="hide">
                                       </dxe:GridViewDataTextColumn> 
                            <dxe:GridViewDataTextColumn FieldName="FullSelectedDesign" Caption="hidden Field Id" Width="0" HeaderStyle-CssClass="hide">
                                       </dxe:GridViewDataTextColumn> 
                        </Columns>
                        
                    </dxe:ASPxGridView>

                </div>
                
            </div>
        </div>
    </div>
    </div>
    <!-- Modal PopUp -->
    <div class="modal fade" id="ModelPopup" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Pay structure</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="txtSearchBoxkeydown(event)" id="txtSearchBox" autofocus width="100%" placeholder="Search By Pay Structure Name or Short Name" />
                    <div id="TableStructure">
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>
    <!-- Modal PopUp -->

    <div>
        <asp:HiddenField ID="hdnInputType" runat="server" />
        <asp:HiddenField ID="hdnPayStructureID" runat="server" />
        <asp:HiddenField ID="hdnEmployeeID" runat="server" />
        <asp:HiddenField ID="hdnPeriod" runat="server" />
        <asp:HiddenField ID="hdnPayConfig_ID" runat="server" />
    </div>


</asp:Content>
