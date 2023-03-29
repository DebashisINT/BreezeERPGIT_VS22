<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                29-03-2023        2.0.36           Pallab              25733 : Master pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"  CodeBehind="TodayTask.aspx.cs" Inherits="ERP.OMS.Management.Activities.TodayTask" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    

    <script type="text/javascript">
        function SignOff() {
            window.parent.SignOff()
        }

        function ShowDetail(ProductType) {
            document.getElementById('GridDiv').style.display = 'none';
            document.getElementById('FrameDiv').style.display = '';
            document.getElementById("ASPxPageControl1_ShowDetails").src = ProductType;
        }

        function ShowHistory(LeadId) {
            var width = 800;
            var height = 300;
            var x = (screen.availHeight - height) / 2;
            var y = (screen.availWidth - width) / 2;
            window.open("ShowHistory_Phonecall.aspx?id1=" + LeadId, 'welcome', 'width=' + width + ',height=' + height + ',top=' + x + ',left=' + y + ',menubar=no,status=no,location=no,toolbar=no,scrollbars=yes');
        }


        function Showhistory(slsid) {

            var frmdate = cxdeFromDate.GetText();
            var todate = cxdeToDate.GetText();
            var stract = slsid + "&frmdate=" + frmdate + "&todate=" + todate;
            //  alert(stract)
            //   alert(stract)
            //    window.open("ShowHistory_Phonecall.aspx?id1=" + stract);
            document.location.href = "../Master/ShowHistory_Phonecall.aspx?id1=" + slsid + "&frmdate=" + frmdate + "&todate=" + todate;

        }

        function disp_prompt(name) {
            if (name == "tab0") {
                document.location.href = "crm_sales.aspx";
            }
            if (name == "tab1") {
                document.location.href = "frmDocument.aspx";
            }
            else if (name == "tab2") {
                //document.location.href="futuresale.aspx"; 
            }
            else if (name == "tab3") {
                document.location.href = "ClarificationSales.aspx";
            }
            else if (name == "tab4") {
                document.location.href = "ClosedSales.aspx";
            }
        }

        function ShowDetailProduct(actid) {

            cPopup_Product.Show();
            cAspxProductGrid.PerformCallback(actid);
            // cComponentPanel.PerformCallback(slsid);
        }
        function ShowDetailProductClass(actid) {

            cPopup_Product_Class.Show();
            cAspxProductClassGrid.PerformCallback(actid);
            // cComponentPanel.PerformCallback(slsid);
        }
        function Budget_open() {
            var SMid = '';
            var url = '/OMS/Management/Activities/SalesmanBudget.aspx?tid=1';
            popupbudget.SetContentUrl(url);
            popupbudget.Show();

            return false;
            //return true;
        }
        function BudgetAfterHide(s, e) {
            popupbudget.Hide();
        }
        function btn_ShowRecordsClick() {
            //CallServer(data, "");
            grid.PerformCallback('');
        }

        function ShowClosed(keyValue) {

            jConfirm('Confirm Closed Sales?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    grid.PerformCallback('ClosedStatus~' + keyValue);
                }
            });
        }

        function LastActivityDetailsCall(obj) {


            if (grid.cpDelmsg != null) {
                if (grid.cpDelmsg.trim() != '') {
                    jAlert(grid.cpDelmsg);
                    grid.cpDelmsg = '';

                }


            }
        }
    </script>
    <style>

         .pull-right {
            float:right !important;
        }

          .mtop3 {
            margin-top:3px;
        }
    </style>

    <style>
        /*Rev 1.0*/

        /*select
        {
            height: 30px !important;
            border-radius: 4px;
            -webkit-appearance: none;
            position: relative;
            z-index: 1;
            background-color: transparent;
            padding-left: 10px !important;
            padding-right: 22px !important;
        }*/

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
        .TableMain100 #GridSalesReport
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
        /*Rev end 1.0*/
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main">   
        <div class="panel-heading">
        <div class="panel-title">
            <h3><%--Today's Task- Future Sales--%>


                <asp:Label ID="lblActivity" runat="server" Text=""></asp:Label>
            </h3>
          
              <div id="btncross" class="crossBtn" style="margin-left:50px;">
                <%--  <a href="crm_sales.aspx"><i class="fa fa-times"></i></a>--%>

                  <asp:HyperLink
                ID="goBackCrossBtn"
                NavigateUrl="#"
                runat="server">
        <i class="fa fa-times"></i>
            </asp:HyperLink>
              </div>
          
        </div>
    </div>
        <div class="form_main">
               <div class="" id="divdetails">
            <div class="clearfix">
                <div style="padding-right: 5px; padding-bottom: 10px">
                    

                 
                      
                        <% if (rights.CanExport)
                           { %>
                    
                     <asp:DropDownList ID="drdSalesActivityDetails" runat="server" Height="34px" CssClass="btn btn-sm btn-primary  expad  mtop3" OnSelectedIndexChanged="drdSalesActivityDetails_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Value="0">Export to</asp:ListItem>
                        <asp:ListItem Value="1">PDF</asp:ListItem>
                        <asp:ListItem Value="2">XLS</asp:ListItem>
                        <asp:ListItem Value="3">RTF</asp:ListItem>
                        <asp:ListItem Value="4">CSV</asp:ListItem>
                    </asp:DropDownList>

                        <% } %>
                </div>
            </div>
        </div>
            <table class="TableMain100">
                <tr>
                    <td>
                        <div id="GridDiv">
                                                <table width="100%">
                                                    
                                                    <tr>
                                                        <td>
                                                             <dxe:ASPxGridView ID="SalesDetailsGrid"  KeyFieldName="sls_id"  SettingsCookies-Enabled="true" SettingsCookies-StorePaging="true" SettingsCookies-StoreFiltering="true" SettingsCookies-StoreGroupingAndSorting="true" SettingsBehavior-AllowFocusedRow="true" runat="server" AutoGenerateColumns="False" ClientInstanceName="grid"
                                                            Width="100%" DataSourceID="SalesDetailsGridDataSource" 
                                                                   
                                                                >
                                                            <Columns>
                                                                <%--  <dxe:GridViewCommandColumn VisibleIndex="0">
                                                                </dxe:GridViewCommandColumn>--%>
                                                                <dxe:GridViewDataTextColumn Caption="Activity" VisibleIndex="0"  FieldName="LeadId" Visible="false">
                                                                    <DataItemTemplate>
                                                                        <dxe:ASPxCheckBox ID="chkDetail" runat="server">
                                                                        </dxe:ASPxCheckBox>

                                                                        <dxe:ASPxTextBox ID="lblActNo" runat="server" Width="100%" Visible="false"
                                                                            NullText="0" Value='<%# Eval("LeadId") %>'>
                                                                        </dxe:ASPxTextBox>


                                                                        <dxe:ASPxTextBox ID="lblSalesId" runat="server" Width="100%" Visible="false"
                                                                            NullText="0" Value='<%# Eval("sls_id") %>'>
                                                                        </dxe:ASPxTextBox>

                                                                        <dxe:ASPxTextBox ID="lblact_id" runat="server" Width="100%" Visible="false"
                                                                            NullText="0" Value='<%# Eval("sls_activity_id") %>'>
                                                                        </dxe:ASPxTextBox>
                                                                        <dxe:ASPxTextBox ID="lblAssignedTaskId" runat="server" Width="100%" Visible="false"
                                                                            NullText="0" Value='<%# Eval("act_assignedTo") %>'>
                                                                        </dxe:ASPxTextBox>

                                                                    </DataItemTemplate>
                                                                </dxe:GridViewDataTextColumn>
                                                            <%--      <dxe:GridViewDataTextColumn FieldName="Address" Caption="Address" VisibleIndex="1" Width="18%">
                                                                </dxe:GridViewDataTextColumn>--%>

                                                                   <dxe:GridViewDataTextColumn FieldName="ContactNumber" Caption="Contact Number" VisibleIndex="2" Width="18%">
                                                                </dxe:GridViewDataTextColumn>
                                                                 <dxe:GridViewDataTextColumn FieldName="sls_assignedBy" Caption="Assigned ID" VisibleIndex="0" Visible="false" Width="18%" >
                                                                </dxe:GridViewDataTextColumn>

                                                                <dxe:GridViewDataTextColumn FieldName="Assigned To" VisibleIndex="3" Caption="Salesman" Settings-AllowAutoFilterTextInputTimer="False" >
                                                                </dxe:GridViewDataTextColumn>

                                                                <dxe:GridViewDataTextColumn FieldName="Assigned By" VisibleIndex="4" Caption="Assigned By" Visible="false">
                                                                </dxe:GridViewDataTextColumn>

                                                                <dxe:GridViewDataTextColumn FieldName="Industry" VisibleIndex="5"  Caption="Industry" Settings-AllowAutoFilterTextInputTimer="False">
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn FieldName="Name" ReadOnly="True" VisibleIndex="6"  Caption="Customer/Lead Name" Settings-AllowAutoFilterTextInputTimer="False">
                                                                </dxe:GridViewDataTextColumn>
                                                                
                                                                <%--   <dxe:GridViewDataTextColumn FieldName="ProductType" ReadOnly="True" Visible="False"
                                                                    VisibleIndex="4"  Width="18%">
                                                                </dxe:GridViewDataTextColumn>--%>
                                                                <dxe:GridViewDataTextColumn FieldName="Id" ReadOnly="True" Visible="False" VisibleIndex="7">
                                                                    <EditFormSettings Visible="False" />
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn FieldName="Amount" Visible="False" VisibleIndex="8">
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn FieldName="Product" ReadOnly="True" Visible="False"
                                                                    VisibleIndex="9">
                                                                </dxe:GridViewDataTextColumn>

                                                                <dxe:GridViewDataTextColumn FieldName="ProductType" VisibleIndex="10" Caption="Product Type" Visible="false">
                                                                </dxe:GridViewDataTextColumn>

                                                                <dxe:GridViewDataTextColumn FieldName="ProductName" VisibleIndex="12"  Caption="Product(s)" Settings-AllowAutoFilterTextInputTimer="False">
                                                                     <DataItemTemplate>
                                                                           <asp:Label ID="lblProduct" runat="server" Text=""></asp:Label>

                                                                        <asp:LinkButton  runat="server" ID="lnkProduct"  OnClientClick='<%# string.Format("ShowDetailProduct(\"{0}\"); return false", Eval("act_id")) %>' ><img  src="/assests/images/Info.png"/  title="Details"></asp:LinkButton>
                                                                        </DataItemTemplate>
                                                                   <CellStyle HorizontalAlign="Center">
                                                                    </CellStyle>
                                                                </dxe:GridViewDataTextColumn>

                                                                
                                                                 <dxe:GridViewDataTextColumn FieldName="ProductClasName" VisibleIndex="11" Caption="Product Class" Settings-AllowAutoFilterTextInputTimer="False">
                                                                    <DataItemTemplate>
                                                                        <asp:Label ID="lblProductClass" runat="server" Text=""></asp:Label>
                                                                        <asp:LinkButton runat="server" ID="lnkProductClass" OnClientClick='<%# string.Format("ShowDetailProductClass(\"{0}\"); return false", Eval("sls_id")) %>'><img  src="/assests/images/Viewt1.png"/  title="Details"></asp:LinkButton>
                                                                    </DataItemTemplate>
                                                                    <CellStyle HorizontalAlign="Center">
                                                                    </CellStyle>
                                                                </dxe:GridViewDataTextColumn>




                                                                <dxe:GridViewDataTextColumn FieldName="ExpectedTime" VisibleIndex="13" Caption="Date Of Completion" Settings-AllowAutoFilterTextInputTimer="False"
                                                                    >
                                                                     <CellStyle HorizontalAlign="Center">
                                                                    </CellStyle>
                                                                </dxe:GridViewDataTextColumn>
                                                                 <dxe:GridViewDataTextColumn FieldName="PriorityName" VisibleIndex="14" Caption="Priority" Settings-AllowAutoFilterTextInputTimer="False">
                                                                    <CellStyle HorizontalAlign="Center">
                                                                    </CellStyle>
                                                                </dxe:GridViewDataTextColumn>
                                                                   <dxe:GridViewDataTextColumn FieldName="NextVisit"  VisibleIndex="15" Caption="Next Activity Date" Settings-AllowAutoFilterTextInputTimer="False">
                                                                    <CellStyle HorizontalAlign="Center">
                                                                    </CellStyle>
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn Visible="false" VisibleIndex="16" Caption="Reassign">
                                                                    <DataItemTemplate>
                                                                        <a href="javascript:void(0)" onclick="ShowDetailReassign('<%#Eval("sls_id") %>')">Reassign</a>
                                                                    </DataItemTemplate>
                                                                    <EditFormSettings Visible="False" />
                                                                </dxe:GridViewDataTextColumn>


                                                                <dxe:GridViewDataTextColumn Visible="false" VisibleIndex="17" Caption="Create Activity">
                                                                    <DataItemTemplate>
                                                                        <a href="javascript:void(0)" onclick="ShowCreateActivity('<%#Eval("LeadId") %>','<%#Eval("sls_id") %>','<%#Eval("sls_activity_id") %>','<%#Eval("act_assignedTo") %>','<%#Eval("act_activityNo") %>','<%#Eval("act_assign_task") %>')">Create Activity</a>
                                                                    </DataItemTemplate>
                                                                    <EditFormSettings Visible="False" />
                                                                </dxe:GridViewDataTextColumn>
                                                                
                                                            </Columns>

                                                                   <SettingsPager PageSize="10">
                                                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                                                            </SettingsPager>
                                                            <Styles>
                                                                <LoadingPanel ImageSpacing="10px">
                                                                </LoadingPanel>
                                                                <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                                </Header>
                                                                <Cell CssClass="gridcellleft">
                                                                </Cell>
                                                            </Styles>
                                                          <%--  <SettingsPager NumericButtonCount="20" ShowSeparators="True">
                                                                <FirstPageButton Visible="True">
                                                                </FirstPageButton>
                                                                <LastPageButton Visible="True">
                                                                </LastPageButton>
                                                            </SettingsPager>--%>
                                                            <Settings ShowGroupPanel="True"  ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu = "True"  />
                                                                  <SettingsSearchPanel Visible="True"  />
                                                            <ClientSideEvents EndCallback="function(s, e) {
	LastActivityDetailsCall(s.cpHeight);
}" />
                                                        </dxe:ASPxGridView>
                                                            
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                    </td>
                </tr>
            </table>
              <asp:SqlDataSource ID="SalesDetailsGridDataSource" runat="server" ></asp:SqlDataSource>
                 <dxe:ASPxPopupControl ID="Popup_Product" runat="server" ClientInstanceName="cPopup_Product"
            Width="400px" HeaderText="Product List" PopupHorizontalAlign="WindowCenter"
            BackColor="white" Height="100px" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton" 
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">

                      
                                                       <dxe:PanelContent runat="server">

                                                            <dxe:ASPxGridView ID="AspxProductGrid" ClientInstanceName="cAspxProductGrid" Width="100%"  runat="server" OnCustomCallback="AspxProductGrid_CustomCallback"
                        AutoGenerateColumns="False"  >

                                      <Columns>
                                            <dxe:GridViewDataTextColumn Visible="True" FieldName="sProducts_Name"   Caption="Product Name">
                                                <CellStyle CssClass="gridcellleft">
                                                </CellStyle>
                                                <EditFormSettings Visible="False"></EditFormSettings>
                                            </dxe:GridViewDataTextColumn>
                       
                        
                                         

                                     </Columns>
                                                             
                                                            <SettingsBehavior AllowSort="false" AllowGroup="false" />
                        </dxe:ASPxGridView>
          

                                                              </dxe:PanelContent>
                                              
                        </dxe:PopupControlContentControl>
               
            </ContentCollection>
            <HeaderStyle BackColor="LightGray" ForeColor="Black" />

          
        </dxe:ASPxPopupControl>

                <dxe:ASPxPopupControl ID="Popup_ProductClass" runat="server" ClientInstanceName="cPopup_Product_Class"
            Width="400px" HeaderText="Product Class List" PopupHorizontalAlign="WindowCenter"
            BackColor="white" Height="100px" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">


                    <dxe:PanelContent runat="server">

                        <dxe:ASPxGridView ID="ASPxGridProductClass" ClientInstanceName="cAspxProductClassGrid" Width="100%" runat="server" OnCustomCallback="AspxProductclassGrid_CustomCallback"
                            AutoGenerateColumns="False">

                            <Columns>
                                <dxe:GridViewDataTextColumn Visible="True" FieldName="ProductClass_Code" Caption="Product Class">
                                    <CellStyle CssClass="gridcellleft">
                                    </CellStyle>
                                    <EditFormSettings Visible="False"></EditFormSettings>
                                </dxe:GridViewDataTextColumn>



                            </Columns>

                            <SettingsBehavior AllowSort="false" AllowGroup="false" />
                        </dxe:ASPxGridView>


                    </dxe:PanelContent>

                </dxe:PopupControlContentControl>

            </ContentCollection>
            <HeaderStyle BackColor="LightGray" ForeColor="Black" />


        </dxe:ASPxPopupControl>

               <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="true" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
           <%-- <asp:SqlDataSource ID="FutureSalesDataSource" runat="server">
            </asp:SqlDataSource>--%>
      
               <dxe:ASPxPopupControl ID="ASPXPopupControl2" runat="server"
                CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popupbudget" Height="500px"
                Width="1310px" HeaderText="Budget" Modal="true" AllowResize="true" ResizingMode="Postponed">
                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">
                    </dxe:PopupControlContentControl>
                </ContentCollection>
                
                 <ClientSideEvents CloseUp="BudgetAfterHide" />
            </dxe:ASPxPopupControl>  
                </div>
    </div>
   </asp:Content>