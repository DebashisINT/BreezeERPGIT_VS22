<%@ Page Title="City" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.Master.management_master_City" CodeBehind="City.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        
        .dxpc-contentWrapper{
            background: rgb(237,243,244);
        }
        .dxpc-headerContent{
            color: white;
        }
    </style>
    <%--    <link href="../../CentralData/CSS/GenericCss.css" rel="stylesheet" type="text/css" />--%>
    <script type="text/javascript" src="../../CentralData/JSScript/GenericJScript.js"></script>

    <script type="text/javascript">
        //function is called on changing country
        //        function OnCountryChanged(cmbCountry) 
        //        {
        //            grid.GetEditor("cou_country").PerformCallback(cmbCountry.GetValue().toString());
        //        }
        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }
    </script>

    <script type="text/javascript">
        //$(document).ready(function () {
        //    alert('1');
        //    $('#valid').attr('style', 'display:none');
        //});

        function fn_PopOpen() {
            $('#valid').attr('style', 'display:none');
            document.getElementById('<%=hiddenedit.ClientID%>').value = '';
            //            alert('HidenEdit-'+GetObjectID('<%=hiddenedit.ClientID%>').value);
            ctxtcityName.SetText('');
            cCmbCountryName.SetValue("1");
            cCmbState.SetSelectedIndex(0);
            //ctxtNseCode.SetText('');
            //ctxtBseCode.SetText('');
            //ctxtMcxCode.SetText('');
            //ctxtMcsxCode.SetText('');
            //ctxtNcdexCode.SetText('');
            //ctxtCdslCode.SetText('');
            //ctxtNsdlCode.SetText('');
            //ctxtNdmlCode.SetText('');
            //ctxtDotexCode.SetText('');
            //ctxtCvlCode.SetText('');
            cPopup_Empcitys.Show();
            OnCmbCountryName_ValueChange();
        }
        function btnSave_citys() {
            if (ctxtcityName.GetText() == '') {
                //alert('Please Enter City Name');
                $('#valid').attr('style', 'display:block;position: absolute;right: -4px;top: 30px;');
                ctxtcityName.Focus();
            }
            else {
                if (document.getElementById('<%=hiddenedit.ClientID%>').value == '')
                    grid.PerformCallback('savecity~');
                else
                    grid.PerformCallback('updatecity~' + GetObjectID('<%=hiddenedit.ClientID%>').value);
            }
        }
        function fn_btnCancel() {
            cPopup_Empcitys.Hide();
        }
        function fn_Editcity(keyValue) {
            grid.PerformCallback('Edit~' + keyValue);
        }
        function fn_Deletecity(keyValue) {
            //var result=confirm('Confirm delete?');
            //if(result)
            //{
            //    grid.PerformCallback('Delete~' + keyValue);
            //}
            jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {             
                if (r == true) {                  
                    grid.PerformCallback('Delete~' + keyValue);
                }
                else {
                    return false;
                }
            });
           
        }

        var editedState = "";
        function grid_EndCallBack() {
            
            if (grid.cpinsert != null) {
                if (grid.cpinsert == 'Success') {

                    jAlert('Saved successfully');
                    cPopup_Empcitys.Hide();
                   
                }
                else {
                    jAlert("Error On Insertion \n 'Please Try Again!!'");
                    cPopup_Empcitys.Hide();
                }
            }
            if (grid.cpEdit != null) {
                ctxtcityName.SetText(grid.cpEdit.split('~')[0]);
                //cCmbState.SetValue(grid.cpEdit.split('~')[1]);
                editedState = grid.cpEdit.split('~')[1];
                cCmbCountryName.SetValue(grid.cpEdit.split('~')[2]);
                OnCmbCountryName_ValueChange();
                //ctxtNseCode.SetText(grid.cpEdit.split('~')[3]);
                //ctxtBseCode.SetText(grid.cpEdit.split('~')[4]);
                //ctxtMcxCode.SetText(grid.cpEdit.split('~')[5]);
                //ctxtMcsxCode.SetText(grid.cpEdit.split('~')[6]);
                //ctxtNcdexCode.SetText(grid.cpEdit.split('~')[7]);
                //ctxtCdslCode.SetText(grid.cpEdit.split('~')[8]);
                //ctxtNsdlCode.SetText(grid.cpEdit.split('~')[9]);
                //ctxtNdmlCode.SetText(grid.cpEdit.split('~')[10]);
                //ctxtCvlCode.SetText(grid.cpEdit.split('~')[11]);
                //ctxtDotexCode.SetText(grid.cpEdit.split('~')[12]);
                GetObjectID('<%=hiddenedit.ClientID%>').value = grid.cpEdit.split('~')[13];
                cPopup_Empcitys.Show();
            }
            if (grid.cpUpdate != null) {
                if (grid.cpUpdate == 'Success') {
                    jAlert('Update Successfully');
                    cPopup_Empcitys.Hide();
                }
                else {
                    jAlert("Error on Updation\n'Please Try again!!'")
                    cPopup_Empcitys.Hide();
                }
            }
            if (grid.cpUpdateValid != null) {
                if (grid.cpUpdateValid == "StateInvalid") {
                    jAlert("Please Select State");
                    //cPopup_Empcitys.Show();
                    //cCmbState.Focus();
                    //alert(GetObjectID('<%=hiddenedit.ClientID%>').value);
                    //grid.PerformCallback('Edit~'+GetObjectID('<%=hiddenedit.ClientID%>').value);
                    //grid.cpUpdateValid=null;
                }
            }
            if (grid.cpDelete != null) {
                if (grid.cpDelete == 'Success') {
                    jAlert('Deleted sccessfully');
                    grid.cpDelete = null;
                    grid.PerformCallback();
                }
                else {
                    jAlert(grid.cpDelete);
                    grid.cpDelete = null;
                    grid.PerformCallback();
                }
            }
            if (grid.cpExists != null) {
                if (grid.cpExists == "Exists") {
                    jAlert('Duplicate value.');
                    cPopup_Empcitys.Hide();
                }
                else {
                    jAlert("Error on operation \n 'Please Try again!!'")
                    cPopup_Empcitys.Hide();
                }
            }
            

        }
        function OnCmbCountryName_ValueChange() {
            cCmbState.PerformCallback("BindState~" + cCmbCountryName.GetValue());
        }
        function CmbState_EndCallback() {
            cCmbState.SetSelectedIndex(0);
            if (editedState != "") {
                cCmbState.SetValue(editedState);
                editedState = "";
            }
            cCmbState.Focus();
        } 

        function gridRowclick(s, e) {
            $('#cityGrid').find('tr').removeClass('rowActive');
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
<div class="clearfix">

    <div class="panel-heading">
        <div class="panel-title">
            <h3>District</h3>
        </div>
    </div>
    
        <div class="form_main">
            <div class="rgth pull-left full">
                <div class="Main">
                    <%--<div class="TitleArea">
                    <strong><span style="color: #000099">City List</span></strong>
                </div>--%>
                    <div class="SearchArea clearfix">
                        <div class="FilterSide clearfix">
                            <div style="float: left; padding-right: 5px;">
                                <% if (rights.CanAdd)
                                   { %>
                                <a href="javascript:void(0);" onclick="fn_PopOpen()" class="btn btn-success btn-radius"><span class="btn-icon"><i class="fa fa-plus" ></i></span><span>Add New</span> </a><%} %>
                                <%--  <a href="javascript:ShowHideFilter('s');" class="btn btn-primary"><span >Show Filter</span></a>--%>
                                <a href="javascript:ShowHideFilter('All');" class="btn btn-primary btn-radius" style="display: none;"><span>All Records</span></a>
                               <% if (rights.CanExport)
                                               { %>
                                 <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-radius" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                                    <asp:ListItem Value="0">Export to</asp:ListItem>
                                    <asp:ListItem Value="1">PDF</asp:ListItem>
                                    <asp:ListItem Value="2">XLS</asp:ListItem>
                                    <asp:ListItem Value="3">RTF</asp:ListItem>
                                    <asp:ListItem Value="4">CSV</asp:ListItem>
                                </asp:DropDownList>
                                <% } %>
                            </div>

                            <%--<div class="ExportSide pull-right">

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

                        </div>--%>
                        </div>

                    </div>
                    <div class="clear"></div>
                </div>
                <div class="clear"></div>
                <div class="GridViewArea relative">
                    <dxe:ASPxGridView ID="cityGrid" runat="server" AutoGenerateColumns="False" ClientInstanceName="grid"
                        KeyFieldName="city_id" Width="100%" OnHtmlRowCreated="cityGrid_HtmlRowCreated" Settings-VerticalScrollableHeight="280" Settings-VerticalScrollBarMode="Auto"
                        OnHtmlEditFormCreated="cityGrid_HtmlEditFormCreated" OnCustomCallback="cityGrid_CustomCallback" SettingsBehavior-AllowFocusedRow="true" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" >
                        <SettingsSearchPanel Visible="True" Delay="5000" />
                        <Columns>
                            <dxe:GridViewDataTextColumn Caption="CityID" FieldName="city_id" ReadOnly="True"
                                Visible="False" FixedStyle="Left" VisibleIndex="0">
                                <EditFormSettings Visible="False" />
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="StateID" FieldName="state_id" ReadOnly="True"
                                Visible="False" FixedStyle="Left" VisibleIndex="1">
                                <EditFormSettings Visible="False" />
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="CountryID" FieldName="cou_id" ReadOnly="True"
                                Visible="False" FixedStyle="Left" VisibleIndex="2">
                                <EditFormSettings Visible="False" />
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="District" FieldName="city_name" Width="30%" FixedStyle="Left"
                                Visible="True" VisibleIndex="3" PropertiesTextEdit-MaxLength="50">
                                <EditFormSettings Visible="True" />
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="State" FieldName="state" Width="30%" FixedStyle="Left"
                                Visible="True" VisibleIndex="4">
                                <EditFormSettings Visible="True" />
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Country" FieldName="cou_country" Visible="True"
                                Width="30%" VisibleIndex="5">
                                <CellStyle CssClass="gridcellleft" Wrap="False">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <%-- <dxe:GridViewDataTextColumn Caption="NSECode" FieldName="City_NSECode" Visible="True"
                            Width="6%" VisibleIndex="6">
                            <CellStyle CssClass="gridcellleft" Wrap="False">
                            </CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="BSECode" FieldName="City_BSECode" Visible="True"
                            Width="6%" VisibleIndex="7">
                            <CellStyle CssClass="gridcellleft" Wrap="False">
                            </CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="MCXCode" FieldName="City_MCXCode" Visible="True"
                            Width="6%" VisibleIndex="8">
                            <CellStyle CssClass="gridcellleft" Wrap="False">
                            </CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="MCXSXCode" FieldName="City_MCXSXCode" Visible="True"
                            Width="6%" VisibleIndex="9">
                            <CellStyle CssClass="gridcellleft" Wrap="False">
                            </CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="NCDEXCode" FieldName="City_NCDEXCode" Visible="True"
                            Width="6%" VisibleIndex="10">
                            <CellStyle CssClass="gridcellleft" Wrap="False">
                            </CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="CDSLCode" FieldName="City_CDSLCode" Visible="True"
                            Width="6%" VisibleIndex="11">
                            <CellStyle CssClass="gridcellleft" Wrap="False">
                            </CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="NSDLCode" FieldName="City_NSDLCode" Visible="True"
                            Width="6%" VisibleIndex="12">
                            <CellStyle CssClass="gridcellleft" Wrap="False">
                            </CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="NDMLCode" FieldName="City_NDMLCode" Visible="True"
                            Width="6%" VisibleIndex="13">
                            <CellStyle CssClass="gridcellleft" Wrap="False">
                            </CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="CVLCode" FieldName="City_CVLCode" Visible="True"
                            Width="6%" VisibleIndex="14">
                            <CellStyle CssClass="gridcellleft" Wrap="False">
                            </CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="DotExCode" FieldName="City_DotExCode" Visible="True"
                            Width="6%" VisibleIndex="15">
                            <CellStyle CssClass="gridcellleft" Wrap="False">
                            </CellStyle>
                        </dxe:GridViewDataTextColumn>--%>
                            <dxe:GridViewDataTextColumn ReadOnly="True" Width="0" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                <HeaderTemplate>
                                    <span></span>
                                </HeaderTemplate>
                                <DataItemTemplate>
                                    <div class='floatedBtnArea'>
                                    <% if (rights.CanEdit)
                                       { %>
                                    <a href="javascript:void(0);" onclick="fn_Editcity('<%# Container.KeyValue %>')" class="" title="">
                                        <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Edit</span></a><%} %>
                                    <% if (rights.CanDelete)
                                       { %>
                                    <a href="javascript:void(0);" onclick="fn_Deletecity('<%# Container.KeyValue %>')" title="">
                                        <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>Delete</span></a><%} %>
                                    </div>
                                </DataItemTemplate>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                        <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                  
                        <Settings ShowGroupPanel="True" ShowFilterRow="true" ShowFilterRowMenu="true" />
                        <ClientSideEvents EndCallback="function (s, e) {grid_EndCallBack();}" RowClick="gridRowclick" />
                    </dxe:ASPxGridView>
                </div>
                <div class="PopUpArea">
                    <dxe:ASPxPopupControl ID="Popup_Empcitys" runat="server" ClientInstanceName="cPopup_Empcitys"
                        Width="700px" HeaderText="Add/Modify District" PopupHorizontalAlign="WindowCenter"
                        BackColor="white" Height="100px" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
                        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True" ba>
                        <ContentCollection>
                            <dxe:PopupControlContentControl runat="server">
                                <%--<div style="Width:400px;background-color:#FFFFFF;margin:0px;border:1px solid red;">--%>
                                <div class="Top clearfix">
                                    <div style="margin-bottom: 5px;" class="col-md-4">
                                        <div class="cityDiv" style="padding-top: 5px;">
                                            Country
                                        </div>
                                        <div class="Left_Content" style="padding-top: 5px;">
                                            <dxe:ASPxComboBox ID="CmbCountryName" ClientInstanceName="cCmbCountryName" runat="server"
                                                ValueType="System.String" Width="100%" EnableSynchronization="True">
                                                <ClientSideEvents ValueChanged="function(s,e){OnCmbCountryName_ValueChange()}"></ClientSideEvents>
                                            </dxe:ASPxComboBox>
                                        </div>
                                    </div>
                                    <div style="margin-bottom: 5px;" class="col-md-4">
                                        <div class="cityDiv" style="padding-top: 5px;">
                                            State
                                        </div>
                                        <div class="Left_Content" style="padding-top: 5px;">
                                            <dxe:ASPxComboBox ID="CmbState" ClientInstanceName="cCmbState" runat="server" ValueType="System.String"
                                                Width="100%" EnableSynchronization="True" OnCallback="CmbState_Callback">
                                                <ClientSideEvents EndCallback="CmbState_EndCallback"></ClientSideEvents>
                                            </dxe:ASPxComboBox>
                                        </div>
                                    </div>
                                    <div style="margin-bottom: 5px; position: relative" class="col-md-4">
                                        <div class="cityDiv" style="padding-top: 5px;">
                                            District<span style="color: red">*</span>
                                        </div>
                                        <div class="Left_Content" style="padding-top: 5px;">
                                            <dxe:ASPxTextBox ID="txtcityName" ClientInstanceName="ctxtcityName" runat="server" MaxLength="50"
                                                Width="100%">
                                            </dxe:ASPxTextBox>
                                            <div id="valid" style="display: none; position: absolute; right: -4px; top: 30px;">
                                                <img id="grid_DXPEForm_DXEFL_DXEditor2_EI" title="Mandatory" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-YRohc" alt="Required"></div>
                                            <%--<asp:RequiredFieldValidator ID="rvCity" runat="server"  ControlToValidate="txtcityName" ValidationGroup="cty" ErrorMessage="not Valid" ></asp:RequiredFieldValidator>--%>
                                        </div>

                                    </div>
                                </div>
                                <div class="ContentDiv">
                                    <%--<div style="height: 20px; width: 100%; background-color: Gray; text-align:center;">
                                    <h5>Static Code</h5>
                                </div>
                                <div class="col-md-6 text-center" style="background-color: Gray;">
                                    Exchange
                                </div>
                                <div class="col-md-6 text-center" style="background-color: Gray;">
                                    Value
                                </div>
                                <div style="clear: both"></div>
                                <div class="ScrollDiv">
                                    <div class="col-md-4 mBot10">
                                        <div class="cityDiv">
                                            NSE Code
                                        </div>
                                        <div style="padding-top: 5px;">
                                            <dxe:ASPxTextBox ID="txtNseCode" ClientInstanceName="ctxtNseCode" runat="server"
                                                CssClass="cityTextbox" Width="100%">
                                            </dxe:ASPxTextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-4 mBot10">
                                        <div class="cityDiv">
                                            BSE Code
                                        </div>
                                        <div>
                                            <dxe:ASPxTextBox ID="txtBseCode" ClientInstanceName="ctxtBseCode" runat="server"
                                                CssClass="cityTextbox" Width="100%">
                                            </dxe:ASPxTextBox>
                                        </div>
                                    </div>

                                    <div class="col-md-4 mBot10">
                                        <div class="cityDiv">
                                            MCX Code
                                        </div>
                                        <div>
                                            <dxe:ASPxTextBox ID="txtMcxCode" ClientInstanceName="ctxtMcxCode" runat="server"
                                                CssClass="cityTextbox" Width="100%">
                                            </dxe:ASPxTextBox>
                                        </div>
                                    </div>
                                    <br style="clear: both;" />
                                    <div class="col-md-4 mBot10">
                                        <div class="cityDiv">
                                            MCXSX Code
                                        </div>
                                        <div>
                                            <dxe:ASPxTextBox ID="txtMcsxCode" ClientInstanceName="ctxtMcsxCode" runat="server"
                                                CssClass="cityTextbox" Width="100%">
                                            </dxe:ASPxTextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-4 mBot10">

                                        <div class="cityDiv ">
                                            NCDEX Code
                                        </div>
                                        <div>
                                            <dxe:ASPxTextBox ID="txtNcdexCode" ClientInstanceName="ctxtNcdexCode" runat="server"
                                                CssClass="cityTextbox" Width="100%">
                                            </dxe:ASPxTextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-4 mBot10">
                                        <div class="cityDiv ">
                                            CDSL Code
                                        </div>
                                        <div>
                                            <dxe:ASPxTextBox ID="txtCdslCode" ClientInstanceName="ctxtCdslCode" CssClass="cityTextbox"
                                                runat="server" Width="100%">
                                            </dxe:ASPxTextBox>
                                        </div>
                                    </div>
                                    <br style="clear: both;" />
                                    <div class="col-md-4 mBot10">
                                        <div class="cityDiv ">
                                            NSDL Code
                                        </div>
                                        <div>
                                            <dxe:ASPxTextBox ID="txtNsdlCode" ClientInstanceName="ctxtNsdlCode" CssClass="cityTextbox"
                                                runat="server" Width="100%">
                                            </dxe:ASPxTextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-4 mBot10">
                                        <div class="cityDiv ">
                                            NDML Code
                                        </div>
                                        <div>
                                            <dxe:ASPxTextBox ID="txtNdmlCode" ClientInstanceName="ctxtNdmlCode" runat="server"
                                                CssClass="cityTextbox" Width="100%">
                                            </dxe:ASPxTextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-4 mBot10">
                                        <div class="cityDiv ">
                                            CVL Code
                                        </div>
                                        <div>
                                            <dxe:ASPxTextBox ID="txtCvlCode" ClientInstanceName="ctxtCvlCode" runat="server"
                                                CssClass="cityTextbox" Width="100%">
                                            </dxe:ASPxTextBox>
                                        </div>
                                    </div>
                                    <br style="clear: both;" />
                                    <div class="col-md-4 mBot10">
                                        <div class="cityDiv ">
                                            DOTEX Code
                                        </div>
                                        <div>
                                            <dxe:ASPxTextBox ID="txtDotexCode" ClientInstanceName="ctxtDotexCode" runat="server"
                                                CssClass="cityTextbox" Width="100%">
                                            </dxe:ASPxTextBox>
                                        </div>
                                    </div>
                                    <br style="clear: both;" />
                                </div>--%>
                                    <br style="clear: both;" />
                                    <div class="Footer">
                                        <div class="col-md-12">
                                            <dxe:ASPxButton ID="btnSave_citys" ClientInstanceName="cbtnSave_citys" runat="server"
                                                AutoPostBack="False" Text="Save" CssClass="btn btn-primary">
                                                <ClientSideEvents Click="function (s, e) {btnSave_citys();}" />
                                            </dxe:ASPxButton>
                                            <dxe:ASPxButton ID="btnCancel_citys" runat="server" AutoPostBack="False" Text="Cancel" CssClass="btn btn-danger">
                                                <ClientSideEvents Click="function (s, e) {fn_btnCancel();}" />
                                            </dxe:ASPxButton>
                                        </div>
                                    </div>
                                </div>
                                <%-- </div>--%>
                            </dxe:PopupControlContentControl>
                        </ContentCollection>
                        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
                    </dxe:ASPxPopupControl>
                    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
                    </dxe:ASPxGridViewExporter>
                </div>
                <div class="HiddenFieldArea" style="display: none;">
                    <asp:HiddenField runat="server" ID="hiddenedit" />
                </div>
            </div>
            <%--left conte slide area--%>
        <%--<div class="ltfg">
            <span class="Closer"><i class="fa fa-angle-right" aria-hidden="true"></i></span>
            <div class="innerCont">
                <div class="part">
                    <label>Current Bank Balance</label>
                    <div class="values">
                        12211.22
                    </div>
                </div>
                <div class="part">
                    <label>Account Balance</label>
                    <div class="values">
                        125463.22
                    </div>
                </div>
            </div> 
        </div>--%>
    </div>
    
    
</div>
    

</asp:Content>
