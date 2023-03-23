<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                23-03-2023        2.0.36           Pallab              25733 : Master pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Title="Brand Master" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.Master.management_frm_Brand" CodeBehind="frm_Brand.aspx.cs" %>

 

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <style type="text/css">
        .stateDiv {
            height: 25px;
            width: 68px;
            float: left;
        }

        .dxpc-headerContent {
            color: white;
        }
    </style>
    <script type="text/javascript" src="../../CentralData/JSScript/GenericJScript.js"></script>

    <script type="text/javascript"> 
        function fn_ctxtBrandNBame_TextChanged(s, e) {
           
            //var ProductName = ctxtPro_Name.GetText();
            var brandCode = 0;
            if (status == 'updateBrand') {
                brandCode = document.getElementById('hdBrandId').value;
            }
            var BrandName = ctxtBrandNBame.GetText().trim();
            $.ajax({
                type: "POST",
                url: "frm_Brand.aspx/CheckUniqueName",
                //data: "{'ProductName':'" + ProductName + "'}",
                data: JSON.stringify({ BrandName: BrandName, BrandCode: brandCode }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var data = msg.d;

                    if (data == true) {
                        jAlert("Please Enter Unique Brand Name");
                        ctxtBrandNBame.SetText("");
                        ctxtBrandNBame.SetFocus();
                        //document.getElementById("Popup_Empcitys_ctxtPro_Code_I").focus();
                        document.getElementById("txtBrandNBame").focus();

                        return false;
                    }
                }

            });
        }



        var status = 'saveBrand';

        function fn_PopUpOpen() {
            status = 'saveBrand';
            $('#valid').attr('style', 'display:none;');
            ctxtBrandNBame.SetText('');
            ctxtContactNo.SetText('');
            ctxtEmail.SetText('');
            cPopupBrand.SetHeaderText('Add Brand'); 
            cPopupBrand.Show();

        }
        function fn_EditCountry(keyValue) {
            document.getElementById('hdBrandId').value = keyValue;
            status = 'updateBrand';
            grid.PerformCallback('Edit~' + keyValue);
        }
        function fn_DeleteCountry(keyValue) {
            jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    grid.PerformCallback('Delete~' + keyValue);
                }
                else {
                    return false;
                }
            });


        }
        function fn_btnCancel() {
            ctxtBrandNBame.SetText("");
            status = 'saveBrand';
            cPopupBrand.Hide();
        }
        function btnSave_Brand() {
            var countrynm = ctxtBrandNBame.GetText();
            if (countrynm.trim() == '') {
                $('#valid').attr('style', 'display:block;position: absolute;right: 32px;top: 17px;');
               
                ctxtBrandNBame.Focus();
            }
            else {
                grid.PerformCallback(status);
               
            }
        }


        function grid_EndCallBack() {

            if (grid.cpMsg != null) {
                if (grid.cpMsg != '') {
                    jAlert(grid.cpMsg);
                    fn_btnCancel();
                    grid.cpMsg = null;
                }
            }

            if (grid.cpEdit) {
                if (grid.cpEdit != '') {
                    var ReturnData = grid.cpEdit.split('|@|');
                    ctxtBrandNBame.SetText(ReturnData[0]);
                    ctxtContactNo.SetText(ReturnData[1]);
                    ctxtEmail.SetText(ReturnData[2]);
                    cPopupBrand.Show();
                    grid.cpEdit = null;
                }
            }



        }
    </script>



    <style>
        .dxgvHeader {
            border: 1px solid #2c4182 !important;
            background-color: #415698 !important;
        }

            .dxgvHeader, .dxgvHeader table {
                color: #fff !important;
            }

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

        #ASPxFromDate , #ASPxToDate , #ASPxASondate , #ASPxAsOnDate , #txtDOB , #txtAnniversary , #txtcstVdate , #txtLocalVdate ,
        #txtCINVdate , #txtincorporateDate , #txtErpValidFrom , #txtErpValidUpto , #txtESICValidFrom , #txtESICValidUpto , #FormDate , #toDate
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
        #txtESICValidUpto_B-1 , #FormDate_B-1 , #toDate_B-1
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
        #txtESICValidUpto_B-1 #txtESICValidUpto_B-1Img , #FormDate_B-1 #FormDate_B-1Img , #toDate_B-1 #toDate_B-1Img
        {
            display: none;
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

        #DivSetAsDefault
        {
            margin-top: 25px;
        }

        .dxeBase_PlasticBlue .dxichTextCellSys label
        {
            color: #fff !important;
        }

        #actv-warh label
        {
            color: #111 !important;
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
    <script>
        function gridRowclick(s, e) {
            $('#GridBrand').find('tr').removeClass('rowActive');
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
                        $(value).css({ 'opacity': '1' });
                    }, 100);
                });
            }, 200);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main">
        <div class="panel-heading">
        <div class="panel-title">
            <h3>Brand</h3>
        </div>
    </div>
        <div class="form_main">
        <div class="Main"> 

            <div class="SearchArea clearfix">
                <div class="FilterSide">
                    <div style="float: left; padding-right: 5px;">
                        <% if (rights.CanAdd)
                           { %>
                        <a href="javascript:void(0);" onclick="fn_PopUpOpen()" class="btn btn-success btn-radius btn-xs"><span class="btn-icon"><i class="fa fa-plus" ></i></span><span>Add New</span> </a>
                        <% } %> 
                        <% if (rights.CanExport)
                                               { %>
                        <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-radius btn-xs" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
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

            <div class="GridViewArea relative">
                <dxe:ASPxGridView ID="GridBrand" runat="server" AutoGenerateColumns="False" ClientInstanceName="grid"
                    KeyFieldName="Brand_Id" Width="100%" OnHtmlEditFormCreated="GridBrand_HtmlEditFormCreated"
                    OnCustomCallback="GridBrand_CustomCallback" SettingsBehavior-AllowFocusedRow="true"  SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" > 
                   <SettingsSearchPanel Visible="True" Delay="5000"  />
                     <Columns>
                        <dxe:GridViewDataTextColumn Caption="Brand ID" FieldName="Brand_Id" ReadOnly="True"
                            Visible="False" VisibleIndex="0">
                            <EditCellStyle HorizontalAlign="Left">
                            </EditCellStyle>
                            <EditFormSettings Visible="False" VisibleIndex="1" />
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="Brand Name" FieldName="Brand_Name" VisibleIndex="1"
                            Width="90%">
                            <EditFormSettings Visible="True" />
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
 
                        <dxe:GridViewDataTextColumn CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" Width="0">
                            <HeaderTemplate>
                                <span></span>
                            </HeaderTemplate>
                            <DataItemTemplate>
                                <div class='floatedBtnArea'>
                                <% if (rights.CanEdit)
                                   { %>
                                <a href="javascript:void(0);" onclick="fn_EditCountry('<%#Container.KeyValue %>')" class="" title="">
                                    <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Edit</span></a>
                                <% } %>
                                <% if (rights.CanDelete)
                                   { %>
                                <a href="javascript:void(0);" onclick="fn_DeleteCountry('<%#Container.KeyValue %>')" title="">
                                    <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>Delete</span></a>
                                <% } %>
                                </div>
                            </DataItemTemplate>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                    </Columns>
                    <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                    <SettingsSearchPanel Visible="True" />
                    <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />
                    



                    <ClientSideEvents EndCallback="function (s, e) {grid_EndCallBack();}" RowClick="gridRowclick" />
                </dxe:ASPxGridView>
            </div>

            <div class="PopUpArea">
                <dxe:ASPxPopupControl ID="PopupBrand" runat="server" ClientInstanceName="cPopupBrand"
                    Width="400px" Height="90px" HeaderText="Add/Modify Brand" PopupHorizontalAlign="Windowcenter"
                    PopupVerticalAlign="WindowCenter" CloseAction="closeButton" Modal="true">
                    <ContentCollection>
                        <dxe:PopupControlContentControl ID="countryPopup" runat="server">
                            <div class="Top clearfix">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <div class="" style="padding-top: 5px;">Brand Name:<span style="color: red;">*</span></div>
                                        </td>
                                        <td class="relative">
                                            <div style="padding-top: 5px;">
                                                <dxe:ASPxTextBox ID="txtBrandNBame" ClientInstanceName="ctxtBrandNBame" ClientEnabled="true"
                                                    runat="server" Width="90%" MaxLength="50">
                                                     <ClientSideEvents TextChanged="function(s,e){fn_ctxtBrandNBame_TextChanged(s,e)}" />
                                                </dxe:ASPxTextBox>
                                                <div id="valid" style="display: none; position: absolute; right: 1px;top: 11px;">
                                                    <img id="grid_DXPEForm_DXEFL_DXEditor2_EI" title="Mandatory" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-YRohc" alt="Required" /></div>
                                            </div>
                                        </td>
                                    </tr>

                                     <tr>
                                        <td>
                                            <div class="" style="padding-top: 5px;">Contact No.</div>
                                        </td>
                                        <td class="relative">
                                            <div style="padding-top: 5px;">
                                                <dxe:ASPxTextBox ID="txtContactNo" ClientInstanceName="ctxtContactNo" ClientEnabled="true"
                                                    runat="server" Width="90%" MaxLength="100"> 
                                                </dxe:ASPxTextBox>
                                                
                                        </td>
                                    </tr>


                                     <tr>
                                        <td>
                                            <div class="" style="padding-top: 5px;">Email:</div>
                                        </td>
                                        <td class="relative">
                                            <div style="padding-top: 5px;">
                                                <dxe:ASPxTextBox ID="txtEmail" ClientInstanceName="ctxtEmail" ClientEnabled="true"
                                                    runat="server" Width="90%" MaxLength="100"> 
                                                </dxe:ASPxTextBox>
                                                
                                        </td>
                                    </tr>


                                    <tr>
                                        <td></td>
                                        <td style="padding-top: 15px;">
                                            <div class="Footer" >
                                            <div style="float: left;">
                                                <dxe:ASPxButton ID="btnSave_Country" ClientInstanceName="cbtnSave_States" runat="server"
                                                    AutoPostBack="False" Text="Save" CssClass="btn btn-primary">
                                                    <ClientSideEvents Click="function (s, e) {btnSave_Brand();}" />
                                                </dxe:ASPxButton>
                                            </div>
                                            <div style="">
                                                <dxe:ASPxButton ID="btnCancel_Country" runat="server" AutoPostBack="False" Text="Cancel" CssClass="btn btn-danger">
                                                    <ClientSideEvents Click="function (s, e) {fn_btnCancel();}" />
                                                </dxe:ASPxButton>
                                            </div>
                                            <br style="clear: both;" />
                                        </div>
                                        </td>
                                    </tr>
                                </table>                               
                            </div>
                            <div class="ContentDiv">
                                <div class="ScrollDiv"></div>
                                <br style="clear: both;" />
                                
                                <br style="clear: both;" />
                            </div>
                        </dxe:PopupControlContentControl>
                    </ContentCollection>
                    <HeaderStyle BackColor="LightGray" ForeColor="Black" />
                </dxe:ASPxPopupControl>
                <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
            </div>

            <div class="HiddenFieldArea" style="display: none;">
                <asp:HiddenField runat="server"  ID="hdBrandId"/>
                
            </div>
        </div>
    </div>
   </div>
</asp:Content>

