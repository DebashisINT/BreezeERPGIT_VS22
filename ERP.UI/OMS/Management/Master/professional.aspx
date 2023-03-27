<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                27-03-2023        2.0.36           Pallab              25733 : Master pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Title="Professions" Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.Master.management_master_professional" CodeBehind="professional.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <script type="text/javascript">
       
        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);

        }
        function fn_PopUpOpen() {
            document.getElementById("profreq").style.visibility = "hidden";
            chfProfId.Set('hfProfId', '');
            ctxtProfession.SetText('');
            cPopupProfession.Show();
            cPopupProfession.SetHeaderText("Add Profession Details");
            
        }
        function fn_EditProfession(keyValue) {
            document.getElementById("profreq").style.visibility = "hidden";
            grid.PerformCallback('Edit~' + keyValue);
            cPopupProfession.SetHeaderText("Modify Profession Details");
        }
        function fn_btnProfession() {
            document.getElementById("profreq").style.visibility = "hidden";
            cPopupProfession.Hide();
        }
        function grid_EndCallBack() {
            if (grid.cpEdit != null) {
                ctxtProfession.SetText(grid.cpEdit.split('~')[0]);
                var hfId = grid.cpEdit.split('~')[1];
                chfProfId.Set('hfProfId', hfId);
                cPopupProfession.Show();

            }
         

            if (grid.cpinsert != null) {
                if (grid.cpinsert == 'Success') {
                    alert('Saved Successfully');
                    cPopupProfession.Hide();
                }
                else {
                    alert("Error On Insertion\n'Please Try Again!!'");
                }
            }

            if (grid.cpUpdate != null) {
                if (grid.cpUpdate == 'Success') {
                    alert('Updated Successfully');
                    cPopupProfession.Hide();
                }
                else {
                    alert("Error On Updatio\n'Please Try Again!!'");
                }
            }

            if (grid.cpDelete != null) {
                if (grid.cpDelete == 'Success')
                    alert('Deleted Successfully');
                else if (grid.cpDelete == 'datalinked')
                    alert('Used in other module. Cannot delete.');
                else
                    alert("Error on deletion\n'Please Try again!!'")
            }

            if (grid.cpExists != null) {
                if (grid.cpExists == 'Exists') {
                    alert('Record Already Exists');
                    cPopupProfession.Hide();
                }

            }

        }
        function fn_DeleteProfession(keyValue) {

            if (confirm("Confirm Delete?")) {
                grid.PerformCallback('Delete~' + keyValue);
            }
        }

        function btnSave_Profession() {
          
            if (ctxtProfession.GetText() == '') {
                //alert('Please Enter Profession Name');
                document.getElementById("profreq").style.visibility = "visible";
                ctxtProfession.Focus();

            }
            else {
                var hfpid = chfProfId.Get('hfProfId');
                if (hfpid == '') {
                    document.getElementById("profreq").style.visibility = "hidden";
                    grid.PerformCallback('SaveProfession~' + ctxtProfession.GetText());
                }
                else {
                    document.getElementById("profreq").style.visibility = "hidden";
                    grid.PerformCallback('UpdateProfession~' + chfProfId.Get('hfProfId'));
                }
            }


        } 

        function gridRowclick(s, e) {
            $('#ProfQualGrid').find('tr').removeClass('rowActive');
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
    <style type="text/css">
        .profDiv {
            height: 25px;
            width: 100px;
            float: left;
            margin-left: 10px;
        }

        .StateTextbox {
            height: 25px;
            width: 50px;
        }

        .Top {
            height: 30px;
            width: 400px;
            padding-top: 5px;
        }

        .Footer {
            height: 30px;
            width: 400px;
            padding-top: 10px;
        }

        .dxpc-headerText.dx-vam {
            color: #fff;
        }

        /*Rev 1.0*/

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
        .TableMain100 #GrdHolidays , #ProfQualGrid
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
        /*Rev end 1.0*/
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                grid.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                grid.SetWidth(cntWidth);
            }

            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    grid.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    grid.SetWidth(cntWidth);
                }

            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main">
        <div class="panel-heading">
        <div class="panel-title">
            <h3>Professions</h3>
        </div>
    </div>
        <div class="form_main">
        <table class="TableMain100">
            <%--<tr>
                <td class="EHEADER" style="text-align: center;">
                    <strong><span style="color: #000099">Profession List</span></strong></td>
            </tr>--%>
            <tr>
                <td>
                    <table width="100%">
                        <tr>
                            <td style="text-align: left; vertical-align: top">
                                <table>
                                    <tr>
                                        <td id="ShowFilter">
                                            <% if (rights.CanAdd)
                                               { %>
                                            <a href="javascript:void(0);" onclick="fn_PopUpOpen()" class="btn btn-success btn-radius"><span class="btn-icon"><i class="fa fa-plus" ></i></span><span>Add New</span> </a>
                                            <% } %>
                                             <asp:DropDownList ID="cmbExport" runat="server" CssClass="btn btn-primary btn-radius" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true">
                                                <asp:ListItem Value="0">Export to</asp:ListItem>
                                                <asp:ListItem Value="1">PDF</asp:ListItem>
                                                <asp:ListItem Value="2">XLS</asp:ListItem>
                                                <asp:ListItem Value="3">RTF</asp:ListItem>
                                                <asp:ListItem Value="4">CSV</asp:ListItem>
                                            </asp:DropDownList>
                                            
                                            <%--<a href="javascript:ShowHideFilter('s');" class="btn btn-success"><span>Show Filter</span></a>--%>
                                        </td>
                                        <td id="Td1">
                                            <%--<a href="javascript:ShowHideFilter('All');" class="btn btn-primary"><span>All Records</span></a>--%>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td></td>
                            <td class="gridcellright pull-right">
                             <%--   <dxe:ASPxComboBox ID="cmbExport" runat="server" AutoPostBack="true"
                                    Font-Bold="False" ForeColor="black" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged"
                                    ValueType="System.Int32" Width="130px">
                                    <items>
                                        <dxe:ListEditItem Text="Select" Value="0" />
                                        <dxe:ListEditItem Text="PDF" Value="1" />
                                        <dxe:ListEditItem Text="XLS" Value="2" />
                                        <dxe:ListEditItem Text="RTF" Value="3" />
                                        <dxe:ListEditItem Text="CSV" Value="4" />
                                    </items>
                                    <buttonstyle>
                                    </buttonstyle>
                                    <itemstyle>
                                        <HoverStyle>
                                        </HoverStyle>
                                    </itemstyle>
                                    <border bordercolor="black" />
                                    <dropdownbutton text="Export">
                                    </dropdownbutton>
                                </dxe:ASPxComboBox>--%>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="relative">
                    <dxe:ASPxGridView ID="ProfQualGrid" ClientInstanceName="grid" runat="server" AutoGenerateColumns="False"
                        KeyFieldName="pro_id" Width="100%" OnHtmlEditFormCreated="ProfQualGrid_HtmlEditFormCreated" Settings-VerticalScrollableHeight="280" Settings-VerticalScrollBarMode="Auto"
                        OnHtmlRowCreated="ProfQualGrid_HtmlRowCreated" OnCustomCallback="ProfQualGrid_CustomCallback" OnRowDeleting="ProfQualGrid_RowDeleting" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false"  >
                        <SettingsSearchPanel Visible="true" Delay="6000" />
                          <ClientSideEvents EndCallback="function (s, e) {;
                              grid_EndCallBack();}" />
                        <columns>
                            <dxe:GridViewDataTextColumn FieldName="pro_id" ReadOnly="True" Visible="False"
                                VisibleIndex="0" Width="20%">
                                <EditFormSettings Visible="False" />
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Profession" FieldName="pro_professionName"
                                VisibleIndex="0" Width="80%">
                                <PropertiesTextEdit Width="300px">
                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="" ErrorTextPosition="Right"
                                        SetFocusOnError="True" ValidateOnLeave="False" >
                                        <RequiredField ErrorText="" IsRequired="True" />
                                    </ValidationSettings>
                                </PropertiesTextEdit>
                                <EditFormSettings Visible="True" />
                                <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                    <Paddings PaddingTop="15px" />
                                </EditCellStyle>
                                <CellStyle Wrap="False">
                                </CellStyle>
                                <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Width="0">
                                <HeaderStyle HorizontalAlign="Center" />
                                <HeaderTemplate>
                                    
                                    <%--<a href="javascript:void(0);" onclick="fn_PopUpOpen()"><span>Add New</span> </a>--%>
                                </HeaderTemplate>
                                <DataItemTemplate>
                                    <div class='floatedBtnArea'>
                                        <% if (rights.CanEdit)
                                           {  %>
                                        <a href="javascript:void(0);" onclick="fn_EditProfession('<%#Container.KeyValue %>')" class="">
                                            <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Edit</span></a>
                                        <%  } %>
                                        <% if (rights.CanDelete)
                                           {  %>
                                        <a href="javascript:void(0);" onclick="fn_DeleteProfession('<%#Container.KeyValue %>')">
                                            <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>Delete</span></a>
                                        <% } %>
                                    </div>
                                </DataItemTemplate>
                                <CellStyle HorizontalAlign="Center">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>



                            <%-- <dxe:GridViewCommandColumn VisibleIndex="1">
                                <DeleteButton Visible="True">
                                </DeleteButton>
                                <EditButton Visible="True">
                                </EditButton>
                                <HeaderStyle HorizontalAlign="Center" />
                                <HeaderTemplate>
                                    <%if (Session["PageAccess"].ToString().Trim() == "All" || Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "DelAdd")
                      { %>
                                    <a href="javascript:void(0);" onclick="fn_PopUpOpen()"><span style="color: #000099;
                                        text-decoration: underline">Add New</span> </a>
                                    <%} %>
                                </HeaderTemplate>
                            </dxe:GridViewCommandColumn>--%>
                        </columns>
                        <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                        <settingscommandbutton>                           
                            <EditButton Image-Url="../../../assests/images/Edit.png" ButtonType="Image" Image-AlternateText="Edit" Styles-Style-CssClass="pad">
                            </EditButton>
                             <DeleteButton Image-Url="../../../assests/images/Delete.png" ButtonType="Image" Image-AlternateText="Delete">
                            </DeleteButton>
                            <UpdateButton Text="Update" ButtonType="Button" Styles-Style-CssClass="btn btn-primary "></UpdateButton>
                            <CancelButton Text="Cancel" ButtonType="Button" Styles-Style-CssClass="btn btn-danger "></CancelButton>
                        </settingscommandbutton>
                        <styles>
                            <Cell CssClass="gridcellleft">
                            </Cell>
                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                            </Header>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                        </styles>
                        <ClientSideEvents RowClick="gridRowclick" />
                        <SettingsSearchPanel Visible="True" />
                        <settings showgrouppanel="True" showstatusbar="Visible" showfilterrow="true" showfilterrowmenu="true" />
                        <settingstext commandnew="Add" popupeditformcaption="Add/Modify Profession" confirmdelete="Confirm delete?" />
                        <settingsbehavior columnresizemode="NextColumn" confirmdelete="True" />
                        <settingsediting mode="PopupEditForm" popupeditformheight="200px" popupeditformhorizontalalign="Center"
                            popupeditformmodal="True" popupeditformverticalalign="Above" popupeditformwidth="600px"
                            editformcolumncount="1" />
                        <settingspager numericbuttoncount="20" pagesize="20" alwaysshowpager="True" showseparators="True">
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </settingspager>
                        <templates>
                            <EditForm>
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 100%">
                                            <controls>
                                                <dxe:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormEditors" ColumnID="" ID="Editors">
                                                </dxe:ASPxGridViewTemplateReplacement>                                                           
                                            </controls>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: center">
                                            <dxe:ASPxGridViewTemplateReplacement ID="UpdateButton" ReplacementType="EditFormUpdateButton"
                                                runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                            <dxe:ASPxGridViewTemplateReplacement ID="CancelButton" ReplacementType="EditFormCancelButton"
                                                runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                        </td>
                                    </tr>
                                </table>
                            </EditForm>
                        </templates>
                      <%--  <clientsideevents endcallback="function (s, e) {grid_EndCallBack();}" />--%>
                    </dxe:ASPxGridView>
                    <dxe:ASPxPopupControl ID="PopupProfession" runat="server" ClientInstanceName="cPopupProfession"
                        Width="400px" Height="120px" HeaderText="" PopupHorizontalAlign="Windowcenter"
                        PopupVerticalAlign="WindowCenter" CloseAction="closeButton" Modal="true">
                        <contentcollection>
                            <dxe:PopupControlContentControl ID="countryPopup" runat="server">
                                <div class="Top">
                                    <div>
                                        <div class="profDiv">
                                            Profession &nbsp; <span style="color:red;"> *</span>
                                        </div>
                                        <div style="position:relative">
                                            <dxe:ASPxTextBox ID="txtProfession" ClientInstanceName="ctxtProfession" ClientEnabled="true"
                                                runat="server" Height="25px" Width="240px" MaxLength="50">
                                            </dxe:ASPxTextBox>
           
                                            <span id="profreq" class="pullrightClass fa fa-exclamation-circle abs" style="color:red;visibility:hidden; position:absolute;right: 30px;top: 5px;"></span>
                                        </div>
                                    </div>
                                </div>
                                <div class="ContentDiv">
                                    <div class="Footer">
                                        <div style="margin-left: 110px; width: 70px; float: left; padding-top:12px;">
                                            <dxe:ASPxButton ID="btnSave_Profession" ClientInstanceName="cbtnSave_Profession" runat="server" CssClass="btn btn-primary"
                                                AutoPostBack="False" Text="Save">
                                                <ClientSideEvents Click="function (s, e) {btnSave_Profession();}" />
                                            </dxe:ASPxButton>
                                        </div>
                                        <div style="width: 200px; float: right; padding-top:12px;">
                                            <dxe:ASPxButton ID="btnCancel_Profession" CssClass="btn btn-danger" runat="server" AutoPostBack="False" Text="Cancel">
                                                <ClientSideEvents Click="function (s, e) {fn_btnProfession();}" />
                                            </dxe:ASPxButton>
                                        </div>
                                        <br style="clear: both;" />
                                    </div>
                                    <br style="clear: both;" />
                                </div>
                            </dxe:PopupControlContentControl>
                        </contentcollection>
                        <headerstyle backcolor="LightGray" forecolor="Black" />
                    </dxe:ASPxPopupControl>
                </td>
            </tr>
            <tr>
                <td>
                    <dxe:ASPxHiddenField runat="server" ClientInstanceName="chfProfId" ID="hfProfId"></dxe:ASPxHiddenField>
                </td>
            </tr>
        </table>
    </div>
    </div>
    <%--<asp:SqlDataSource ID="Professional" runat="server" ConflictDetection="CompareAllValues"
            DeleteCommand="DELETE FROM [tbl_master_profession] WHERE [pro_id] = @original_pro_id"
            InsertCommand="INSERT INTO [tbl_master_profession] ([pro_professionName], [CreateDate], [CreateUser]) VALUES (@pro_professionName, getdate(), @CreateUser)"
            OldValuesParameterFormatString="original_{0}" SelectCommand="SELECT [pro_id],[pro_professionName] FROM [tbl_master_profession]"
            UpdateCommand="UPDATE [tbl_master_profession] SET [pro_professionName] = @pro_professionName ,[LastModifyDate]=getdate(),[LastModifyUser]= @CreateUser WHERE [pro_id] = @original_pro_id">
            <DeleteParameters>
                <asp:Parameter Name="original_pro_id" Type="Decimal" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="pro_professionName" Type="String" />
                <asp:SessionParameter Name="CreateUser" Type="Decimal" SessionField="userid" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="pro_professionName" Type="String" />
                <asp:SessionParameter Name="CreateUser" Type="Decimal" SessionField="userid" />
            </InsertParameters>
        </asp:SqlDataSource>--%>
    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>
    <br />
</asp:Content>
