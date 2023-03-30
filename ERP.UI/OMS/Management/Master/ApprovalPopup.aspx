<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                30-03-2023        2.0.36           Pallab              25733 : Master pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="ApprovalPopup.aspx.cs" Inherits="ERP.OMS.Management.Master.ApprovalPopup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        /*Rev 1.0*/

        select
        {
            height: 30px !important;
            border-radius: 4px !important;
            -webkit-appearance: none;
            position: relative;
            z-index: 1;
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

        .simple-select::after {
            /*content: '<';*/
            content: url(../../../assests/images/left-arw.png);
            position: absolute;
            top: 26px;
            right: 13px;
            font-size: 16px;
            transform: rotate(269deg);
            font-weight: 500;
            background: #094e8c;
            color: #fff;
            height: 18px;
            display: block;
            width: 26px;
            /* padding: 10px 0; */
            border-radius: 4px;
            text-align: center;
            line-height: 19px;
            z-index: 0;
        }
        .simple-select {
            position: relative;
        }
        .simple-select:disabled::after
        {
            background: #1111113b;
        }
        select.btn
        {
            padding-right: 10px !important;
        }

        /*select.btn
        {
            padding-right: 10px !important;
        }*/

        /*.panel-group .panel
        {
            box-shadow: 1px 1px 8px #1111113b;
            border-radius: 8px;
        }*/

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
        .TableMain100 #SalesDetailsGrid, #ShowGrid
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

        .col-md-3 , .col-md-2
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

        .dxpc-content table
        {
             width: 100%;
        }

        input[type="text"], input[type="password"], textarea
        {
            margin-bottom: 0 !important;
        }
        #FromDate , #ToDate , #ASPxFromDate , #ASPxToDate
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        #FromDate_B-1 , #ToDate_B-1 , #ASPxFromDate_B-1 , #ASPxToDate_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #FromDate_B-1 #FromDate_B-1Img , #ToDate_B-1 #ToDate_B-1Img , #ASPxFromDate_B-1 #ASPxFromDate_B-1Img , #ASPxToDate_B-1 #ASPxToDate_B-1Img
        {
            display: none;
        }

        #lblToDate
        {
            padding-left: 10px;
        }

        .dxtc-activeTab:after {
            content: '';
            width: 0;
            height: 0;
            border-left: 8px solid transparent;
            border-right: 8px solid transparent;
            border-top: 9px solid #3e5395;
            position: absolute;
            /* left: 50%; */
            z-index: 3;
            /* bottom: -15px; */
            margin-left: -9px;
        }

        .calendar-icon
        {
            right: 14px;
        }
        .pdTop15
        {
            padding-top: 28px !important;
        }
        /*Rev end 1.0*/
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .padCenter {
            width: auto;
            margin: 0 auto;
        }

            .padCenter > tbody > tr > td {
                padding: 10px;
            }
    </style>

    <script>
        //function grid_SelectionChanged(s, e) {
        //    s.GetSelectedFieldValues("EuniqueId", GetSelectedFieldValuesCallback);
        //}

        //var value = "";

        //function GetSelectedFieldValuesCallback(values) {
        //    value = "";
        //    for (var i = 0; i < values.length; i++) {
        //        if (i == 0) {
        //            value = values[i];
        //        }
        //        else {
        //            value = value + ',' + values[i];
        //        }
        //    }
        //    //document.getElementById("selCount").innerHTML = grid.GetSelectedRowCount();
        //}

        function ShowClick() {
          

            grid.PerformCallback('BindGrid~' + $("#ddlModules").val() + '~' + cddlStatus.GetValue());
        }

        function ConfirmClick() {

           
            grid.PerformCallback('SaveData~' + $("#ddlModules").val());

        }
        function grid_endcallback() {
            if (grid.cpStatus == "1") {
                grid.cpStatus = null;
                jAlert('Data Saved Successfully.', 'Alert.');
            }

            else if (grid.cpStatus == "2") {
                grid.cpStatus = null;
                jAlert('You must select atleast one document to save.', 'Alert.');
            }
        }


        function getconfirmtype() {

            cCallbackPanel.PerformCallback(cddlStatus.GetValue());
        }


        function OpenDetails(Uniqueid, type, keyValue) {
            var url = '';
            //debugger;
            if (type == 'Customers') {
                CAspxDirectCustomerViewPopup.SetWidth(window.screen.width - 50);
                CAspxDirectCustomerViewPopup.SetHeight(window.innerHeight - 70);
                CAspxDirectCustomerViewPopup.SetHeaderText("View Customer");
                 url = '/OMS/Management/Master/View/ViewCustomer.html?id=' + keyValue;

                CAspxDirectCustomerViewPopup.SetContentUrl(url);

                CAspxDirectCustomerViewPopup.RefreshContentUrl();
                CAspxDirectCustomerViewPopup.Show();
            }

            if (type == 'Vendor') {
                CAspxDirectCustomerViewPopup.SetWidth(window.screen.width - 50);
                CAspxDirectCustomerViewPopup.SetHeight(window.innerHeight - 70);
                CAspxDirectCustomerViewPopup.SetHeaderText("View Vendor");
                 url = '/OMS/management/master/View/ViewVendor.html?id=' + keyValue;

                CAspxDirectCustomerViewPopup.SetContentUrl(url);

                CAspxDirectCustomerViewPopup.RefreshContentUrl();
                CAspxDirectCustomerViewPopup.Show();
            }

           
            if (type == 'Products') {
                CAspxDirectCustomerViewPopup.SetWidth(window.screen.width - 50);
                CAspxDirectCustomerViewPopup.SetHeight(window.innerHeight - 70);
                CAspxDirectCustomerViewPopup.SetHeaderText("View Product");
                url = '/OMS/Management/Master/View/ViewProduct.html?id=' + keyValue;

                CAspxDirectCustomerViewPopup.SetContentUrl(url);

                CAspxDirectCustomerViewPopup.RefreshContentUrl();
                CAspxDirectCustomerViewPopup.Show();
            }
           
           
        }

       

    </script>

    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main">
        <div class="panel-heading">
        <div class="panel-title">
            <h3>Document Verify</h3>

        </div>
    </div>
        <div class="form_main">
        <div class="clearfix row mb-10">
            <div class="col-md-2 simple-select">
                <label>Modules</label>
                <div>
                    <select id="ddlModules" class="form-control">
                        <option value="AccountGroup">Account Group</option>
                        <option value="AccountHead">Account Head</option>
                        <option value="Products">Products</option>
                        <option value="Customers">Customers</option>
                        <option value="Vendor">Vendors</option>
                        <option value="Employees">Employees</option>

                    </select>
                </div>
            </div>
            <div id="dvHeader" runat="server">
                <div class="col-md-2">
                    <label>From Date</label>
                    <div>
                        <dxe:ASPxDateEdit runat="server" ID="dtFrom" Width="100%" ClientInstanceName="cdtFrom"></dxe:ASPxDateEdit>
                    </div>
                </div>

                <div class="col-md-2">
                    <label>To Date</label>
                    <div>
                        <dxe:ASPxDateEdit runat="server" ID="dtTo" Width="100%" ClientInstanceName="cdtTo"></dxe:ASPxDateEdit>
                    </div>
                </div>
            </div>
            <div class="col-md-2">
                <label>Verification Status</label>
                <div>
                    <dxe:ASPxComboBox  runat="server" SelectedIndex="0" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged" ID="ddlStatus" Width="100%" ClientInstanceName="cddlStatus" DataSourceID="dsStatus" ValueField="STATUS_ID" TextField="STATUS_NAME">
                        <ClientSideEvents SelectedIndexChanged="getconfirmtype" />
                    </dxe:ASPxComboBox>
                </div>
            </div>

            <div class="col-md-4 pdTop15">
                <label>&nbsp;</label>
                <button class="btn btn-primary mBot10" onclick="ShowClick();" type="button">Show</button>
            </div>
            <br />
        </div>
        <div class="clearfix row">
            <div class="col-md-12">
                <dxe:ASPxGridView ID="grid" ClientInstanceName="grid" runat="server" OnCustomCallback="grid_CustomCallback" OnDataBinding="grid_DataBinding" KeyFieldName="EuniqueId" Width="100%"  CssClass="pull-left" SettingsBehavior-AllowFocusedRow="true" SettingsDataSecurity-AllowEdit="false"
                    SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false">
                     
                    <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowFilterRow="true" ShowFilterRowMenu="True" />
                  
                   
                    <Columns>
                        <dxe:GridViewCommandColumn ShowSelectCheckbox="true"  Caption="Select"/>
                        <dxe:GridViewDataColumn FieldName="EuniqueId" Visible="false" />
                        <dxe:GridViewDataColumn FieldName="MODULE_TYPE" Visible="false" />

                        <dxe:GridViewDataColumn FieldName="hdn_code" Visible="false" />
                        <dxe:GridViewDataColumn FieldName="Code" Caption="Code">
                          <Settings AutoFilterCondition="Contains" />   
                            
                         </dxe:GridViewDataColumn>
                        
                        <dxe:GridViewDataColumn FieldName="Description">
                         <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataColumn>   

                        <dxe:GridViewDataColumn FieldName="EnteredBy" Caption="Created By" >
                        <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataColumn>   

                        <dxe:GridViewDataColumn FieldName="EnteredOn" Caption="Created On">
                        <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataColumn>   
                        <dxe:GridViewDataColumn FieldName="ModifiedBy" Caption="Modified By">
                        <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataColumn>   
                        <dxe:GridViewDataColumn FieldName="ModifiedOn" Caption="Modified On">
                        <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataColumn> 
                        
                        
                                <dxe:GridViewDataTextColumn ReadOnly="True" CellStyle-HorizontalAlign="Center" Width="100px">
                            <HeaderStyle HorizontalAlign="Center" />

                            <CellStyle HorizontalAlign="Center"></CellStyle>
                            <HeaderTemplate>
                                Actions
                            </HeaderTemplate>
                            <DataItemTemplate>
                                <% if (rights.CanView)
                                   { %>
                                <a href="javascript:void(0)" onclick="OpenDetails('<%#Eval("EuniqueId") %>','<%#Eval("MODULE_TYPE") %>','<%#Eval("hdn_code") %>')" class="pad">
                                <img src="/assests/images/doc.png" />
                                </a>
                               <%} %>
                            </DataItemTemplate>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                    </Columns>
                    <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                    <ClientSideEvents EndCallback="grid_endcallback" />
                </dxe:ASPxGridView>
            </div>
        </div>
        <div class="clearfix row">
            <div class="col-md-12 text-center">
                <table class="padCenter">
                    <tr>
                        <td>
                            <label>Update verify status as</label>

                        </td>
                        <td>
                            <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
                                <PanelCollection>
                                    <dxe:PanelContent runat="server">
                                        <dxe:ASPxComboBox runat="server" ID="ddlConfirm" Width="100%" ClientInstanceName="cddlConfirm" ValueType="System.String">
                                            
                                        </dxe:ASPxComboBox>
                                       

                                    </dxe:PanelContent>
                                </PanelCollection>
                                <ClientSideEvents />
                            </dxe:ASPxCallbackPanel>




                        </td>
                        <td>
                             <% if (rights.CanAdd)
                           { %>
                            <button type="button" id="btnConfirm" onclick="ConfirmClick();" class="btn btn-success btn-xs">Save</button>
                             <%} %>
                        </td>
                    </tr>
                </table>
            </div>
        </div>


        <asp:SqlDataSource ID="dsStatus" runat="server" SelectCommand="select STATUS_ID,STATUS_NAME from MASTER_APPROVAL_STATUS"></asp:SqlDataSource>

        
    </div>
    </div>

    <dxe:ASPxPopupControl ID="AspxDirectCustomerViewPopup" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="CAspxDirectCustomerViewPopup" Height="650px"
        Width="1020px" HeaderText="Customer View" Modal="true" AllowResize="false">
         
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>

</asp:Content>
