<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                17-03-2023        2.0.36           Pallab              25733 : Master pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Title="Cost Centers/Departments" Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.Master.management_master_HRCostCenter" CodeBehind="HRCostCenter.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    


    <script language="javascript" type="text/javascript">
        //function SignOff() {
        //    window.parent.SignOff();
        //}
        //function height() {
        //    if (document.body.scrollHeight >= 500)
        //        window.frameElement.height = document.body.scrollHeight;
        //    else
        //        window.frameElement.height = '500px';
        //    window.frameElement.Width = document.body.scrollWidth;
        //}
        function ClickOnMoreInfo(keyValue) {
            var url = 'HRCost.aspx?id=' + keyValue;
            //OnMoreInfoClick(url, "Modify Cost Center", '940px', '450px', "Y");
            window.location.href = url;

        }
        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }
        function AddNewClick() {
            var url = "HRAddNewCostDept.aspx"
            //OnMoreInfoClick(url, "Modify Cost Center", '940px', '450px', "Y");
            window.location.href = url;
        }
        function gridcrmCampaignclick(s, e) {
            //alert('hi');
            $('#CostDepartmentGrid').find('tr').removeClass('rowActive');
            $('.floatedBtnArea').removeClass('insideGrid');
            //$('.floatedBtnArea a .ico').css({ 'opacity': '0' });
            $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea').addClass('insideGrid');
            $(s.GetRow(e.visibleIndex)).addClass('rowActive');
            setTimeout(function () {
                //alert('delay');
                var lists = $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a');
                //$(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a .ico').css({'opacity': '1'});
                //$(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a').each(function (e) {
                //    setTimeout(function () {
                //        $(this).fadeIn();
                //    }, 100);
                //});    
                $.each(lists, function (index, value) {
                    //console.log(index);
                    //console.log(value);
                    setTimeout(function () {
                        console.log(value);
                        $(value).css({ 'opacity': '1' });
                    }, 100);
                });
            }, 200);
        }
    </script>

    <style>
        /*Rev 1.0*/
        .outer-div-main {
            background: #ffffff;
            padding: 10px;
            border-radius: 10px;
            box-shadow: 1px 1px 10px #11111154;
        }

        /*.form_main {
            overflow: hidden;
        }*/

        label , .mylabel1, .clsTo, .dxeBase_PlasticBlue
        {
            color: #141414 !important;
            font-size: 14px !important;
                font-weight: 500 !important;
                margin-bottom: 0 !important;
                    line-height: 20px;
        }

        #GrpSelLbl .dxeBase_PlasticBlue
        {
                line-height: 20px !important;
        }

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

        .calendar-icon {
            position: absolute;
            bottom: 6px;
            right: 20px;
            z-index: 0;
            cursor: pointer;
        }

        #ASPxFromDate , #ASPxToDate , #ASPxASondate , #ASPxAsOnDate , #txtDOB , #txtAnniversary , #txtcstVdate , #txtLocalVdate ,
        #txtCINVdate , #txtincorporateDate , #txtErpValidFrom , #txtErpValidUpto , #txtESICValidFrom , #txtESICValidUpto
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        .dxeDisabled_PlasticBlue
        {
            z-index: 0 !important;
        }

        #ASPxFromDate_B-1 , #ASPxToDate_B-1 , #ASPxASondate_B-1 , #ASPxAsOnDate_B-1 , #txtDOB_B-1 , #txtAnniversary_B-1 , #txtcstVdate_B-1 ,
        #txtLocalVdate_B-1 , #txtCINVdate_B-1 , #txtincorporateDate_B-1 , #txtErpValidFrom_B-1 , #txtErpValidUpto_B-1 , #txtESICValidFrom_B-1 ,
        #txtESICValidUpto_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #ASPxFromDate_B-1 #ASPxFromDate_B-1Img , #ASPxToDate_B-1 #ASPxToDate_B-1Img , #ASPxASondate_B-1 #ASPxASondate_B-1Img , #ASPxAsOnDate_B-1 #ASPxAsOnDate_B-1Img ,
        #txtDOB_B-1 #txtDOB_B-1Img ,
        #txtAnniversary_B-1 #txtAnniversary_B-1Img ,
        #txtcstVdate_B-1 #txtcstVdate_B-1Img ,
        #txtLocalVdate_B-1 #txtLocalVdate_B-1Img , #txtCINVdate_B-1 #txtCINVdate_B-1Img , #txtincorporateDate_B-1 #txtincorporateDate_B-1Img ,
        #txtErpValidFrom_B-1 #txtErpValidFrom_B-1Img , #txtErpValidUpto_B-1 #txtErpValidUpto_B-1Img , #txtESICValidFrom_B-1 #txtESICValidFrom_B-1Img ,
        #txtESICValidUpto_B-1 #txtESICValidUpto_B-1Img
        {
            display: none;
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

        .styled-checkbox {
        position: absolute;
        opacity: 0;
        z-index: 1;
    }

        .styled-checkbox + label {
            position: relative;
            /*cursor: pointer;*/
            padding: 0;
            margin-bottom: 0 !important;
        }

            .styled-checkbox + label:before {
                content: "";
                margin-right: 6px;
                display: inline-block;
                vertical-align: text-top;
                width: 16px;
                height: 16px;
                /*background: #d7d7d7;*/
                margin-top: 2px;
                border-radius: 2px;
                border: 1px solid #c5c5c5;
            }

        .styled-checkbox:hover + label:before {
            background: #094e8c;
        }


        .styled-checkbox:checked + label:before {
            background: #094e8c;
        }

        .styled-checkbox:disabled + label {
            color: #b8b8b8;
            cursor: auto;
        }

            .styled-checkbox:disabled + label:before {
                box-shadow: none;
                background: #ddd;
            }

        .styled-checkbox:checked + label:after {
            content: "";
            position: absolute;
            left: 3px;
            top: 9px;
            background: white;
            width: 2px;
            height: 2px;
            box-shadow: 2px 0 0 white, 4px 0 0 white, 4px -2px 0 white, 4px -4px 0 white, 4px -6px 0 white, 4px -8px 0 white;
            transform: rotate(45deg);
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

        .TableMain100 #ShowGrid , .TableMain100 #ShowGridList , .TableMain100 #ShowGridRet , .TableMain100 #EmployeeGrid , .TableMain100 #RootGrid
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
                margin-top: 3px;
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

        .for-cust-icon {
            position: relative;
            z-index: 1;
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

        .mt-24
        {
            margin-top: 24px;
        }

        .col-md-3
        {
            margin-top: 8px;
        }

        #upldBigLogo , #upldSmallLogo
        {
            width: 100%;
        }

        .chosen-container-single .chosen-single span
        {
            min-height: 30px;
            line-height: 30px;
        }

        .chosen-container-single .chosen-single div {
        background: #094e8c;
        color: #fff;
        border-radius: 4px;
        height: 26px;
        top: 1px;
        right: 1px;
        /*position:relative;*/
    }

        .chosen-container-single .chosen-single div b {
            display: none;
        }

        .chosen-container-single .chosen-single div::after {
            /*content: '<';*/
            content: url(../../../assests/images/left-arw.png);
            position: absolute;
            top: 2px;
            right: 5px;
            font-size: 18px;
            transform: rotate(269deg);
            font-weight: 500;
        }

    .chosen-container-active.chosen-with-drop .chosen-single div {
        background: #094e8c;
        color: #fff;
    }

        .chosen-container-active.chosen-with-drop .chosen-single div::after {
            transform: rotate(90deg);
            right: 5px;
        }

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


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main">
        <div class="panel-heading">
        <div class="panel-title">
            <h3>Cost Centers/Departments</h3>
        </div>
    </div>
        <div class="form_main">
        <table class="TableMain100" width="100%">
            <%--<tr>
                <td class="EHEADER" style="text-align: center" colspan="2">
                    <strong><span style="color: #000099">Cost Centers/Departments</span></strong>
                </td>
            </tr>--%>
            <tr>
                <td style="text-align: left; vertical-align: top; width: 536px;">
                    <table>
                        <tr>
                            <td id="ShowFilter">
                                <%-- <%if (Session["PageAccess"].ToString().Trim() == "All" || Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "DelAdd")
                                  { %>--%>
                                <% if (rights.CanAdd)
                                   { %>
                                <a href="javascript:void(0);" onclick="AddNewClick();" class="btn btn-success btn-radius"><span class="btn-icon"><i class="fa fa-plus" ></i></span>Add New </a>
                                <%} %>
                                <%-- <a href="javascript:ShowHideFilter('s');" class="btn btn-primary"><span>Show Filter</span></a>--%>
                              <% if (rights.CanExport)
                                               { %>
                                  <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-radius" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true"  >
                                    <asp:ListItem Value="0">Export to</asp:ListItem>
                                    <asp:ListItem Value="1">PDF</asp:ListItem>
                                        <asp:ListItem Value="2">XLS</asp:ListItem>
                                        <asp:ListItem Value="3">RTF</asp:ListItem>
                                        <asp:ListItem Value="4">CSV</asp:ListItem>
                                </asp:DropDownList>
                                <% } %>
                            </td>
                            <td id="Td1">
                                <%--<a href="javascript:ShowHideFilter('All');" class="btn btn-primary"><span>All Records</span></a>--%> <%--ag18102016 ---0011370--%>
                            </td>
                        </tr>
                    </table>
                </td>
                <%--<td class="gridcellright" style="float: right">
                    <dxe:ASPxComboBox ID="cmbExport" runat="server" AutoPostBack="true"
                        Font-Bold="False" ForeColor="black" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged"
                        ValueType="System.Int32" Width="130px">
                        <Items>
                            <dxe:ListEditItem Text="Select" Value="0" />
                            <dxe:ListEditItem Text="PDF" Value="1" />
                            <dxe:ListEditItem Text="XLS" Value="2" />
                            <dxe:ListEditItem Text="RTF" Value="3" />
                            <dxe:ListEditItem Text="CSV" Value="4" />
                        </Items>
                        <Border BorderColor="black" />
                        <DropDownButton Text="Export">
                        </DropDownButton>
                    </dxe:ASPxComboBox>
                </td>--%>
            </tr>
            <tr>
                <%--ag18102016 ---0011370--%>
                <td colspan="2" class="relative">
                    <dxe:ASPxGridView ID="CostDepartmentGrid" runat="server" ClientInstanceName="grid"
                        AutoGenerateColumns="False" DataSourceID="DepartmentSource" KeyFieldName="cost_id" 
                        Width="100%" OnCustomCallback="CostDepartmentGrid_CustomCallback" OnRowUpdated="CostDepartmentGrid_RowUpdated" OnRowCommand="CostDepartmentGrid_RowCommand" SettingsBehavior-AllowFixedGroups="true"  SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false"  >
                        <SettingsSearchPanel Visible="True" Delay="5000" />
                        <Columns>
                            <dxe:GridViewDataTextColumn FieldName="cost_id" ReadOnly="True" VisibleIndex="0"
                                Visible="False">
                                <EditFormSettings Visible="False" />
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataComboBoxColumn Caption="Cost Center/Department Type" FieldName="cost_costCenterType"
                                VisibleIndex="1" Width="50%">
                                <PropertiesComboBox ValueType="System.String" EnableIncrementalFiltering="True">
                                    <Items>
                                        <dxe:ListEditItem Text="Department" Value="Department"></dxe:ListEditItem>
                                        <dxe:ListEditItem Text="Employee" Value="Employee"></dxe:ListEditItem>
                                        <dxe:ListEditItem Text="Branch" Value="Branch"></dxe:ListEditItem>
                                    </Items>
                                </PropertiesComboBox>
                                <EditFormSettings Visible="True" />
                                <CellStyle CssClass="gridcellleft" HorizontalAlign="Left">
                                </CellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Right" Wrap="False">
                                </EditFormCaptionStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataComboBoxColumn>
                            <dxe:GridViewDataTextColumn Caption="Cost Centers/Departments" FieldName="cost_description" VisibleIndex="0"
                                Width="50%">
                                <EditFormSettings Visible="True" />
                                <CellStyle CssClass="gridcellleft" HorizontalAlign="Left">
                                </CellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Right" Wrap="False">
                                </EditFormCaptionStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataComboBoxColumn FieldName="cost_costCenterHead" Visible="False"
                                VisibleIndex="2">
                                <PropertiesComboBox DataSourceID="HeadSource" TextField="cnt_firstName" ValueField="cnt_internalId"
                                    ValueType="System.String" EnableIncrementalFiltering="True">
                                </PropertiesComboBox>
                                <EditFormSettings Visible="True" Caption="Head Of department" />
                                <EditFormCaptionStyle HorizontalAlign="Right" Wrap="False">
                                </EditFormCaptionStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataComboBoxColumn>
                            <dxe:GridViewDataComboBoxColumn FieldName="cost_principlalDepartment" Visible="False"
                                VisibleIndex="3">
                                <PropertiesComboBox DataSourceID="ParentSource" TextField="cost_description" ValueField="cost_id"
                                    ValueType="System.String" EnableIncrementalFiltering="True">
                                </PropertiesComboBox>
                                <EditFormSettings Visible="True" Caption="Parent Cost  Center/Dept." />
                                <EditFormCaptionStyle HorizontalAlign="Right" Wrap="False">
                                </EditFormCaptionStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataComboBoxColumn>
                            <dxe:GridViewDataCheckColumn FieldName="cost_operationIn" Visible="False" VisibleIndex="5">
                                <EditFormSettings Visible="True" Caption="Mutual Fund" />
                                <EditFormCaptionStyle HorizontalAlign="Right" Wrap="False">
                                </EditFormCaptionStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataCheckColumn>
                            <dxe:GridViewDataCheckColumn FieldName="cost_operationIn1" Visible="False" VisibleIndex="6">
                                <EditFormSettings Visible="True" Caption="Broking" />
                                <EditFormCaptionStyle HorizontalAlign="Right" Wrap="False">
                                </EditFormCaptionStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataCheckColumn>
                            <dxe:GridViewDataCheckColumn FieldName="cost_operationIn2" Visible="False" VisibleIndex="7">
                                <EditFormSettings Visible="True" Caption="Insurance" />
                                <EditFormCaptionStyle HorizontalAlign="Right" Wrap="False">
                                </EditFormCaptionStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataCheckColumn>
                            <dxe:GridViewDataCheckColumn FieldName="cost_operationIn3" Visible="False" VisibleIndex="8">
                                <EditFormSettings Visible="True" Caption="Depository" />
                                <EditFormCaptionStyle HorizontalAlign="Right" Wrap="False">
                                </EditFormCaptionStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataCheckColumn>
                            <dxe:GridViewDataTextColumn FieldName="cost_email" Visible="False" VisibleIndex="4">
                                <EditFormSettings Visible="True" Caption="Email-Id" />
                                <EditFormCaptionStyle HorizontalAlign="Right" Wrap="False">
                                </EditFormCaptionStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Caption="Details" VisibleIndex="2" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" Width="0">
                                <DataItemTemplate>
                                    <div class='floatedBtnArea'>
                                    <% if (rights.CanEdit)
                                               { %>
                                    <a href="javascript:void(0);" onclick="ClickOnMoreInfo('<%# Container.KeyValue %>')" title="" class="pad" style="text-decoration: none;">
                                        <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Edit</span>
                                    </a>
                                     <% } %>
                                      <% if (rights.CanDelete)
                                               { %>
                                    <asp:LinkButton ID="btn_delete" runat="server" OnClientClick="return confirm('Confirm Delete?');" CommandArgument='<%# Container.KeyValue %>' CommandName="delete" ToolTip="" Font-Underline="false">
                                        <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>Delete</span>
                                    </asp:LinkButton>
                                     <% } %>
                                        </div>
                                </DataItemTemplate>
                                <EditFormSettings Visible="False" />
                                <HeaderStyle HorizontalAlign="Center" />
                                <HeaderTemplate>
                                    <span>Actions</span>
                                </HeaderTemplate>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <%--<dxe:GridViewCommandColumn VisibleIndex="3" ShowDeleteButton="true" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" Width="5%">
                                <HeaderTemplate>
                                    <span>Actions</span>
                                </HeaderTemplate>
                            </dxe:GridViewCommandColumn>--%>
                        </Columns>
                        <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                        <Templates>
                            <EditForm>
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 25%"></td>
                                        <td style="width: 50%">

                                            <dxe:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormEditors" ColumnID="" ID="Editors"></dxe:ASPxGridViewTemplateReplacement>

                                            <div style="text-align: right; padding: 2px 2px 2px 2px">
                                                <dxe:ASPxGridViewTemplateReplacement ID="UpdateButton" ReplacementType="EditFormUpdateButton"
                                                    runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                <dxe:ASPxGridViewTemplateReplacement ID="CancelButton" ReplacementType="EditFormCancelButton"
                                                    runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                            </div>
                                        </td>
                                        <td style="width: 25%"></td>
                                    </tr>
                                </table>
                            </EditForm>
                        </Templates>
                        <%--<Styles>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                            </Header>
                        </Styles>--%>
                    
                        <Settings ShowTitlePanel="false" ShowFooter="false" ShowStatusBar="Hidden" ShowFilterRow="true" ShowGroupPanel="true" ShowFilterRowMenu ="true" />
                        <%--<SettingsCommandButton>
                            <EditButton Text="Edit"></EditButton>
                            <DeleteButton ButtonType="Image" Image-Url="../../../assests/images/Delete.png"></DeleteButton>

                        </SettingsCommandButton>--%>
                        <ClientSideEvents RowClick="gridcrmCampaignclick" />
                        <SettingsText PopupEditFormCaption="Add/ Modify CostCenter" />
                        <SettingsEditing Mode="PopupEditForm" PopupEditFormHorizontalAlign="Center" PopupEditFormModal="True"
                            PopupEditFormVerticalAlign="TopSides" PopupEditFormWidth="900px" EditFormColumnCount="1" />
                        <SettingsBehavior AllowFocusedRow="false" ColumnResizeMode="NextColumn" ConfirmDelete="True" />
                        <%--<SettingsPager AlwaysShowPager="True" PageSize="20" ShowSeparators="True">
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </SettingsPager>--%>
                    </dxe:ASPxGridView>
                </td>
            </tr>
        </table>
        <dxe:ASPxGridViewExporter ID="exporter" runat="server">
        </dxe:ASPxGridViewExporter>
        <asp:SqlDataSource ID="DepartmentSource" runat="server"
            SelectCommand="HrCostCenterSelect" DeleteCommand="delete from tbl_master_costCenter where cost_id=@cost_id"
            SelectCommandType="StoredProcedure" InsertCommand="HrCostCenterInsert" InsertCommandType="StoredProcedure"
            UpdateCommand="HrCostCenterUpdate" UpdateCommandType="StoredProcedure">
            <InsertParameters>
                <asp:Parameter Name="cost_costCenterType" Type="String" />
                <asp:Parameter Name="cost_description" Type="String" />
                <asp:Parameter Name="cost_costCenterHead" Type="String" />
                <asp:Parameter Name="cost_principlalDepartment" Type="String" />
                <asp:Parameter Name="cost_operationIn" Type="String" />
                <asp:Parameter Name="cost_operationIn1" Type="String" />
                <asp:Parameter Name="cost_operationIn2" Type="String" />
                <asp:Parameter Name="cost_operationIn3" Type="String" />
                <asp:Parameter Name="cost_email" Type="String" />
                <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="String" />
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="cost_id" Type="String" />
                <asp:Parameter Name="cost_costCenterType" Type="String" />
                <asp:Parameter Name="cost_description" Type="String" />
                <asp:Parameter Name="cost_costCenterHead" Type="String" />
                <asp:Parameter Name="cost_principlalDepartment" Type="String" />
                <asp:Parameter Name="cost_operationIn" Type="String" />
                <asp:Parameter Name="cost_operationIn1" Type="String" />
                <asp:Parameter Name="cost_operationIn2" Type="String" />
                <asp:Parameter Name="cost_operationIn3" Type="String" />
                <asp:Parameter Name="cost_email" Type="String" />
                <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="String" />
            </UpdateParameters>
            <DeleteParameters>
                <asp:Parameter Name="cost_id" Type="String" />
            </DeleteParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="ParentSource" runat="server"  SelectCommand="select cost_description,cost_id from tbl_master_costCenter where cost_costCenterType='Department' order by cost_description"></asp:SqlDataSource>
        <asp:SqlDataSource ID="HeadSource" runat="server" SelectCommand="select cnt_firstName+' '+isnull(cnt_middleName,'')+' '+isnull(cnt_lastName,'')+' ['+isnull(rtrim(cnt_shortName),'')+']' as cnt_firstName, cnt_internalId from tbl_master_contact where cnt_internalId LIKE '%em_%' order by cnt_firstName"></asp:SqlDataSource>
        <br />
    </div>
    </div>
</asp:Content>
