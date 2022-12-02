<%@ Page Title="Pin / Zip" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.Master.management_master_pin" CodeBehind="frm_pinMaster.aspx.cs" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/assests/pluggins/choosen/choosen.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            ListBind();
            cityCode = "";
            stateCode = "";
        });
        function setCountry(obj) {
            if (obj) {
                var lstCntry = document.getElementById("lstCountry");

                for (var i = 0; i < lstCntry.options.length; i++) {
                    if (lstCntry.options[i].value == obj) {
                        lstCntry.options[i].selected = true;
                    }
                }
                $('#lstCountry').trigger("chosen:updated");
                onCountryChange();
            }
        }

        function SetCountryNothing() {

            var lstCntry = document.getElementById("lstCountry");

            for (var i = 0; i < lstCntry.options.length; i++) {
                if (lstCntry.options[i].selected) {
                    lstCntry.options[i].selected = false;
                }
            }
            $('#lstCountry').trigger("chosen:updated");
            onCountryChange();

        }

        function setState(obj) {
            if (obj) {
                var lstStae = document.getElementById("lstState");

                for (var i = 0; i < lstStae.options.length; i++) {
                    if (lstStae.options[i].value == obj) {
                        lstStae.options[i].selected = true;
                    }
                }
                $('#lstState').trigger("chosen:updated");
                onStateChange();
            }
        }
        function setCity(obj) {
            if (obj) {
                var lstCity = document.getElementById("lstCity");

                for (var i = 0; i < lstCity.options.length; i++) {
                    if (lstCity.options[i].value == obj) {
                        lstCity.options[i].selected = true;
                    }
                }
                $('#lstCity').trigger("chosen:updated");

            }
        }



        function onAreaChange() {

        }



        function onCountryChange() {
            var CountryId = "";
            if (document.getElementById('lstCountry').value) {
                CountryId = document.getElementById('lstCountry').value;
            } else {
                return;
            }
            var lState = $('select[id$=lstState]');
            var lCity = $('select[id$=lstCity]');

            lState.empty();
            lCity.empty();

            $('#lstCity').trigger("chosen:updated");

            $.ajax({
                type: "POST",
                url: "BranchAddEdit.aspx/GetStates",
                data: JSON.stringify({ CountryCode: CountryId }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var list = msg.d;
                    var listItems = [];
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

                        $(lState).append(listItems.join(''));

                        $('#lstState').fadeIn();
                        $('#lstState').trigger("chosen:updated");
                        if (stateCode) {
                            var stCode = stateCode;
                            stateCode = "";
                            setState(stCode);
                        }
                    }
                    else {
                        $('#lstState').fadeIn();
                        $('#lstState').trigger("chosen:updated");
                    }
                }
            });
        }

        function onStateChange() {
            var StateId = "";
            if (document.getElementById('lstState').value) {
                StateId = document.getElementById('lstState').value;
            }
            else {
                return;
            }
            var lCity = $('select[id$=lstCity]');

            lCity.empty();
            $.ajax({
                type: "POST",
                url: "BranchAddEdit.aspx/GetCities",
                data: JSON.stringify({ StateCode: StateId }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var list = msg.d;
                    var listItems = [];
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

                        $(lCity).append(listItems.join(''));

                        $('#lstCity').fadeIn();
                        $('#lstCity').trigger("chosen:updated");
                        if (cityCode) {
                            var ctCode = cityCode;
                            cityCode = "";
                            setCity(ctCode);
                        }
                    }
                    else {
                        $('#lstCity').fadeIn();
                        $('#lstCity').trigger("chosen:updated");
                    }
                }
            });
        }

        function onCityChange() {
            var CityId = "";
            if (document.getElementById('lstCity').value) {
                CityId = document.getElementById('lstCity').value;
            }
            else {
                return;
            }

            pinCodeWithAreaId = [];
            $.ajax({
                type: "POST",
                url: "BranchAddEdit.aspx/GetArea",
                data: JSON.stringify({ CityCode: CityId }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var list = msg.d;
                    var listItems = [];
                    if (list.length > 0) {

                        for (var i = 0; i < list.length; i++) {
                            var id = '';
                            var name = '';
                            id = list[i].split('|')[1];
                            name = list[i].split('|')[0];
                            pin = list[i].split('|')[2];
                            listItems.push('<option value="' +
                            id + '">' + name
                            + '</option>');
                            pinCodeWithAreaId[i] = id + '~' + pin;
                        }

                        $(lArea).append(listItems.join(''));

                        $('#lstArea').fadeIn();
                        $('#lstArea').trigger("chosen:updated");

                    }
                    else {
                        $('#lstArea').fadeIn();
                        $('#lstArea').trigger("chosen:updated");
                    }
                }
            });
        }

        function ListBind() {

            var config = {
                '.chsn': {},
                '.chsn-deselect': { allow_single_deselect: true },
                '.chsn-no-single': { disable_search_threshold: 10 },
                '.chsn-no-results': { no_results_text: 'Oops, nothing found!' },
                '.chsn-width': { width: "100%" }
            }
            for (var selector in config) {
                $(selector).chosen(config[selector]);
            }

        }
        function lstCountry() {
            $('#lstCountry').fadeIn();
        }



        function LastCall(obj) {
            if (grid.cpErrorMsg) {
                if (grid.cpErrorMsg.trim != "") {
                    jAlert(grid.cpErrorMsg);
                    grid.cpErrorMsg = '';
                    return;
                }
            }

            if (grid.cpSave != null) {
                if (grid.cpSave == 'Y') {
                    grid.cpSave = '';
                    jAlert("Saved Successfully");
                    cPopup_Empcitys.Hide();
                }

            }

            if (grid.cpEdit != null) {
                var pin = grid.cpEdit.split('~')[0];
                cityCode = grid.cpEdit.split('~')[1];
                stateCode = grid.cpEdit.split('~')[2];
                var countryCode = grid.cpEdit.split('~')[3];

                document.getElementById('txtpincode').value = pin;
                setCountry(countryCode);
            }

            if (grid.cpDelmsg != null) {
                if (grid.cpDelmsg.trim() != '') {
                    jAlert(grid.cpDelmsg);
                    grid.cpDelmsg = '';
                    cPopup_Empcitys.Hide();
                }
            }

        }

        function NewPin() {
            Status = 'SAVE_NEW';
            var lState = $('select[id$=lstState]');
            var lCity = $('select[id$=lstCity]');

            lState.empty();
            lCity.empty();

            $('#lstState').trigger("chosen:updated");
            $('#lstCity').trigger("chosen:updated");

            SetCountryNothing();
            document.getElementById('txtpincode').value = '';
            cPopup_Empcitys.Show();
        }
        function MakeRowInVisible() {
            $('#Mandatorytxtpincode').css({ 'display': 'none' });
            $('#MandatorylstCountry').css({ 'display': 'none' });
            $('#MandatorylstState').css({ 'display': 'none' });
            $('#MandatorylstCity').css({ 'display': 'none' });
            cPopup_Empcitys.Hide();
        }

        function isValid() {
            var retvalue = true;
            if (document.getElementById('txtpincode').value.trim() == '') {
                $('#Mandatorytxtpincode').css({ 'display': 'block' });
                retvalue = false;
            } else {
                $('#Mandatorytxtpincode').css({ 'display': 'none' });
            }

            if (document.getElementById('lstCountry').value.trim() == '') {
                $('#MandatorylstCountry').css({ 'display': 'block' });
                retvalue = false;
            } else {
                $('#MandatorylstCountry').css({ 'display': 'none' });
            }
            if (document.getElementById('lstState').value.trim() == '') {
                $('#MandatorylstState').css({ 'display': 'block' });
                retvalue = false;
            } else {
                $('#MandatorylstState').css({ 'display': 'none' });
            }
            if (document.getElementById('lstCity').value.trim() == '') {
                $('#MandatorylstCity').css({ 'display': 'block' });
                retvalue = false;
            } else {
                $('#MandatorylstCity').css({ 'display': 'none' });
            }
            return retvalue;
        }
        function Call_save() {
            if (!isValid()) {
                return;
            }
            document.getElementById('HdlstCity').value = document.getElementById('lstCity').value;
            grid.PerformCallback(Status);

        }

        function DeleteRow(keyValue) {

            jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    grid.PerformCallback('Delete~' + keyValue);
                }
            });
        }

        function OnEdit(obj) {
            Action = 'edit';
            Status = obj;
            grid.PerformCallback('BEFORE_' + obj);
            cPopup_Empcitys.Show();
        }

        function callback() {
            grid.PerformCallback();
        }
        function gridRowclick(s, e) {
            $('#gridPin').find('tr').removeClass('rowActive');
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
    <style type="text/css">
        .chosen-container.chosen-container-single {
            width: 100% !important;
        }

        .chosen-choices {
            width: 100% !important;
        }

        #lstCountry, #lstState, #lstCity {
            width: 200px;
        }

        #lstCountry, #lstState, #lstCity {
            display: none !important;
        }

        #lstCountry_chosen, #lstState_chosen, #lstCity_chosen {
            width: 253px !important;
        }

        #PageControl1_CC {
            overflow: visible !important;
        }

        #lstState_chosen, #lstCountry_chosen, #lstCity_chosen {
            margin-bottom: 5px;
        }
        #gridPin_DXPagerBottom {
        max-width:100% !important;
        min-width: 100% !important; 
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Pin / Zip</h3>
        </div>
    </div>
    <div class="PopUpArea">
        <dxe:ASPxPopupControl ID="Popup_Empcitys" runat="server" ClientInstanceName="cPopup_Empcitys"
            Width="400px" HeaderText="Add/Modify Pin / Zip" PopupHorizontalAlign="WindowCenter"
            BackColor="white" Height="100px" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True" >
            <contentcollection>
                    <dxe:PopupControlContentControl runat="server">
                        <%--<div style="Width:400px;background-color:#FFFFFF;margin:0px;border:1px solid red;">--%>
                        <div class="Top clearfix">
                           
                            <table>
                                    <tr>
                                      <td>
                                          Country:
                                     </td>
                                        <td>
                                            <asp:ListBox ID="lstCountry" CssClass="chsn"   runat="server" Font-Size="12px" Width="253px"   data-placeholder="Select..." onchange="onCountryChange()"></asp:ListBox>
                                             <span id="MandatorylstCountry" class="pullleftClass fa fa-exclamation-circle iconRed " style="color:red; position:absolute;right:26px;top:39px;display:none" title="Mandatory"></span>
                                        </td>
                                   </tr>
                                    <tr>
                                   <td> State:</td>
                                        <td>
                                            <asp:ListBox ID="lstState" CssClass="chsn"   runat="server" Font-Size="12px" Width="253px"   data-placeholder="Select State.."  onchange="onStateChange()"></asp:ListBox>
                                            <span id="MandatorylstState" class="pullleftClass fa fa-exclamation-circle iconRed " style="color:red; position:absolute;right:26px;top:74px;display:none" title="Mandatory"></span>
                                        </td>
                                    </tr> 
                                <tr>
                                    <td>City / District:</td>
                                    <td>
                                        <asp:ListBox ID="lstCity" CssClass="chsn"   runat="server" Font-Size="12px" Width="253px"   data-placeholder="Select City.."  ></asp:ListBox>
                                        <span id="MandatorylstCity" class="pullleftClass fa fa-exclamation-circle iconRed " style="color:red; position:absolute;right:26px;top:104px;display:none" title="Mandatory"></span>
                                         <asp:HiddenField ID="HdlstCity" runat="server" />
                                    </td>
                                </tr>
                                  
                                <tr>
                                    <td>Pin / Zip Code:</td>
                                    <td>
                                       <asp:TextBox ID="txtpincode" runat="server" MaxLength="10"></asp:TextBox>
                                        <span id="Mandatorytxtpincode" class="pullleftClass fa fa-exclamation-circle iconRed " style="color:red; position:absolute;right:26px;top:141px;display:none" title="Mandatory"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" style="padding-left:79px;">
                                            <input id="btnSave" class="btn btn-primary" onclick="Call_save(status)" type="button" value="Save" />
                                    <input id="btnCancel" class="btn btn-danger" onclick="MakeRowInVisible()" type="button" value="Cancel" />
                                        </td>
                                        
                                    </tr>
                                </table>


                        </div>
                         
                    </dxe:PopupControlContentControl>
                </contentcollection>
            <headerstyle backcolor="LightGray" forecolor="Black" />
        </dxe:ASPxPopupControl>

    </div>


    <div class="form_main">
        <table class="TableMain100">
            <tr>
                <td colspan="4">
                    <table class="TableMain100">
                        <tr>
                            <td colspan="4" style="text-align: left; vertical-align: top">
                                <table>
                                    <tr>
                                        <td id="ShowFilter">
                                            <% if (rights.CanAdd)
                                               { %>
                                            <asp:HyperLink ID="HyperLink2" runat="server"
                                                NavigateUrl="javascript:void(0)" onclick="javascript:NewPin()" class="btn btn-primary btn-radius"><span class="btn-icon"><i class="fa fa-plus" ></i></span>Add New</asp:HyperLink>
                                            <%} %>
                                        </td>
                                        <td id="Td1">
                                            <% if (rights.CanExport)
                                               { %>
                                            <asp:DropDownList ID="drdExport"  runat="server" CssClass="btn btn-primary btn-radius " OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true">
                                                <asp:ListItem Value="0">Export to</asp:ListItem>
                                                <asp:ListItem Value="1">PDF</asp:ListItem>
                                                <asp:ListItem Value="2">XLS</asp:ListItem>
                                                <asp:ListItem Value="3">RTF</asp:ListItem>
                                                <asp:ListItem Value="4">CSV</asp:ListItem>
                                            </asp:DropDownList>
                                             <%} %>
                                            <dxe:ASPxGridViewExporter ID="exporter" runat="server">
                                            </dxe:ASPxGridViewExporter>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="gridcellright"></td>
                        </tr>
                    </table>
                </td>
            </tr>

            <tr>
                <td class="relative">
                    <dxe:ASPxGridView ID="gridPin" runat="server" ClientInstanceName="grid" AutoGenerateColumns="False"
                        DataSourceID="SqlDataSource1" KeyFieldName="pin_id" Width="100%" OnCustomCallback="gridPin_CustomCallback" Settings-VerticalScrollableHeight="280" Settings-VerticalScrollBarMode="Auto"
                        OnCustomJSProperties="gridPin_CustomJSProperties" OnHtmlRowCreated="gridPin_HtmlRowCreated"  SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false"  >
                        <SettingsSearchPanel Visible="True" Delay="5000" />
                        <styles>
                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                            </Header>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                        </styles>
                       
                        <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true"/>

                        <columns>
                            <dxe:GridViewDataTextColumn FieldName="pin_id" ReadOnly="True" Visible="False" VisibleIndex="0">
                                <EditFormSettings Visible="False" />
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn FieldName="pin_code" Caption="Pin / Zip Code" Width="30%"
                                VisibleIndex="0" ShowInCustomizationForm="True">
                                <editcellstyle wrap="True">
                                </editcellstyle>
                                <CellStyle CssClass="gridcellleft" wrap="True">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                             
                             
                             <dxe:GridViewDataTextColumn FieldName="city_id" Caption="City / District" Width="30%"
                                VisibleIndex="1" ShowInCustomizationForm="True">
                                <editcellstyle wrap="True">
                                </editcellstyle>
                                <CellStyle CssClass="gridcellleft" wrap="True">
                                </CellStyle>
                                 <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn FieldName="state" Caption="State" Width="30%"
                                VisibleIndex="2" ShowInCustomizationForm="True">
                                <editcellstyle wrap="True">
                                </editcellstyle>
                                <CellStyle CssClass="gridcellleft" wrap="True">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>


                            <dxe:GridViewDataTextColumn Caption="" VisibleIndex="3" Width="0">
                                <CellStyle HorizontalAlign="Center">
                                </CellStyle>
                                <HeaderStyle HorizontalAlign="Center" />
                                <HeaderTemplate>
                                    
                                   
                                </HeaderTemplate>
                                <DataItemTemplate>
                                    <div class='floatedBtnArea'>
                                    <% if (rights.CanEdit)
                                       { %>
                                    <a href="javascript:void(0);" onclick="OnEdit('EDIT~'+'<%# Container.KeyValue %>')" class="">
                                        <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Edit</span></a>
                                    <% } %>
                                    <% if (rights.CanDelete)
                                       { %>
                                     <a href="javascript:void(0);" onclick="DeleteRow('<%#Eval("pin_id") %>')"  class="">
                                        <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>Delete</span></a>
                                     <% } %>
                                     </div>
                                </DataItemTemplate>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                      
                        </columns>
                        <SettingsContextMenu Enabled="true"></SettingsContextMenu>

                        <settingspager pagesize="50">
                            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="50,100,150,200"/>
                              </settingspager>
                        <settingscommandbutton>
                            
                            <EditButton Image-Url="../../../assests/images/Edit.png" ButtonType="Image" Image-AlternateText="Edit">
<Image AlternateText="Edit" Url="../../../assests/images/Edit.png"></Image>
                            </EditButton>
                             <DeleteButton Image-Url="../../../assests/images/Delete.png" ButtonType="Image" Image-AlternateText="Delete">
<Image AlternateText="Delete" Url="../../../assests/images/Delete.png"></Image>
                            </DeleteButton>
                            <UpdateButton Text="Save" ButtonType="Button" Styles-Style-CssClass="btn btn-primary" Image-Width>
<Styles>
<Style CssClass="btn btn-primary"></Style>
</Styles>
                            </UpdateButton>
                            <CancelButton Text="Cancel" ButtonType="Button"></CancelButton>
                        </settingscommandbutton>
                        <styleseditors>
                            <ProgressBar Height="25px">
                            </ProgressBar>
                        </styleseditors>
                        <SettingsSearchPanel Visible="True" />
                        <settings showgrouppanel="True" showstatusbar="Visible" showfilterrow="true" ShowFilterRowMenu ="true" />
                        <settingsediting mode="PopupEditForm" popupeditformheight="200px" popupeditformhorizontalalign="Center"
                            popupeditformmodal="True" popupeditformverticalalign="WindowCenter" popupeditformwidth="600px"
                            editformcolumncount="1" />
                        <settingstext popupeditformcaption="Add/Modify Category" confirmdelete="Confirm delete?" />
                        <settingspager numericbuttoncount="20" pagesize="50" showseparators="True" alwaysshowpager="True">
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </settingspager>
                        <settingsbehavior confirmdelete="True" columnresizemode="NextColumn" />

                        <clientsideevents endcallback="function(s, e) {
	 LastCall(s.cpHeight);
}" RowClick="gridRowclick" />
                    </dxe:ASPxGridView>
                </td>
            </tr>
        </table>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server"
            DeleteCommand="DELETE FROM [tbl_master_pinzip] WHERE [pin_id] = @id"
            InsertCommand="INSERT INTO [tbl_master_pinzip] ([pin_code],[[city_id]]) VALUES (@cat_description,@cat_applicablefor)"
            SelectCommand="select  pin_id,pin_code,d.city_name as city_id,s.state from tbl_master_pinzip h inner join tbl_master_city d on h.city_id=d.city_id inner join tbl_master_state s on s.id=d.state_id order by pin_id"
            UpdateCommand="UPDATE [tbl_master_pinzip] SET [pin_code] = @cat_description,cat_applicablefor=@cat_applicablefor WHERE [pin_id] = @pin_id">
            <DeleteParameters>
                <asp:Parameter Name="pin_id" Type="Int32" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="pin_code" Type="String" />
                <asp:Parameter Name="city_id" Type="String" />
                <asp:Parameter Name="pin_id" Type="Int32" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="pin_code" Type="String" />
                <asp:Parameter Name="city_id" Type="String" />
            </InsertParameters>
            <FilterParameters>
                <asp:Parameter Name="pin_code" Type="String" />
                <asp:Parameter Name="city_id" Type="String" />
            </FilterParameters>
        </asp:SqlDataSource>
        <br />
    </div>
</asp:Content>

