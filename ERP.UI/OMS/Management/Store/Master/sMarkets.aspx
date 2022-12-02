<%@ Page Title="Markets" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.Store.Master.management_master_Store_sMarkets" CodeBehind="sMarkets.aspx.cs" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
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
        function GetObjectID(obj) {
            return document.getElementById(obj);
        }
        function fn_PopOpen() {
            document.getElementById('hiddenedit').value = "";
            // ctxtcityName.SetText('');
            ctxtMarkets_Code.SetText('');
            ctxtMarkets_Name.SetText('');
            ctxtMarkets_Description.SetText('');
            cCmbCountryName.SelectedIndex = -1;
            cCmbState.ClearItems();
            cCmbCity.ClearItems();

            ctxtMarkets_Address.SetText('');
            ctxtMarkets_Email.SetText('');
            ctxtMarkets_Phones.SetText('');
            ctxtMarkets_WebSite.SetText('');
            ctxtMarkets_ContactPerson.SetText('');
            cPopup_market.SetHeaderText('Add Market');
            cPopup_market.Show();
        }


        function btnSave_citys() {

            var valiEmail = false;

            var validPhNo = false;

            var validweburl=false;

            var CheckUniqueCode = false;

            var reg = /^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,4})$/;
            if (reg.test(ctxtMarkets_Email.GetText())) {
                valiEmail = true;
            }

            if (!isNaN(ctxtMarkets_Phones.GetText()) && ctxtMarkets_Phones.GetText().length == 10) {
                validPhNo = true;
            }
            
            //comment by sanjib due to wrong regex 23122016
            //var webreg = /\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*/;
            var webreg = /^http(s)?:\/\/(www\.)?[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$/;
            if (webreg.test(ctxtMarkets_WebSite.GetText())) {
                validweburl = true;
            }
          


            //for unique code ajax call
            var MarketsCode = ctxtMarkets_Code.GetText();
            //$.ajax({
            //    type: "POST",
            //    url: "sMarkets.aspx/CheckUniqueCode",
            //    data: "{'MarketsCode':'" + MarketsCode + "'}",
            //    contentType: "application/json; charset=utf-8",
            //    dataType: "json",
            //    async: false,
            //    success: function (msg) {
            //        //                    CheckUniqueCode = msg.d;
            //        if (document.getElementById('hiddenedit').value == '') {
            //            CheckUniqueCode = msg.d;
            //        }
            //        else {
            //            CheckUniqueCode == false
            //        }
           // jAlert(ctxtMarkets_Code.GetText() + "/" + ctxtMarkets_Name.GetText() + "/" + ctxtMarkets_Email.GetText() + "/" + ctxtMarkets_Phones.GetText() + "/" + ctxtMarkets_WebSite.GetText() + validweburl);
            //comment by sanjib due to already validate why again validate regex 23122016
            //if (ctxtMarkets_Code.GetText() != '' && ctxtMarkets_Name.GetText() != '' && (ctxtMarkets_Email.GetText() != '' || valiEmail == true) && (ctxtMarkets_Phones.GetText() != '' || validPhNo == true) && (ctxtMarkets_WebSite.GetText() == '' || validweburl == true)) {
            if (ctxtMarkets_Code.GetText() != '' && ctxtMarkets_Name.GetText() != '' && (ctxtMarkets_Email.GetText() != '') && (ctxtMarkets_Phones.GetText() != '' ) && (ctxtMarkets_WebSite.GetText() != '')) {
              
                if (document.getElementById('hiddenedit').value == '')
                    grid.PerformCallback('savecity~');
                else
                    grid.PerformCallback('updatecity~' + GetObjectID('hiddenedit').value);
            } else {

                
            }
                    //else if (CheckUniqueCode == true) {
                    //    jAlert('Please enter unique market code');
                    //    ctxtMarkets_Code.Focus();
                    //}
                    //else if (ctxtMarkets_Code.GetText() == '') {
                    //    //jAlert('Please Enter Short Name (Unique)');
                    //    ctxtMarkets_Code.Focus();
                    //    return false;
                    //}
                    //else if (ctxtMarkets_Name.GetText() == '') {
                    //    //jAlert('Please Enter Markets Name');
                    //    ctxtMarkets_Name.Focus();
                    //    return false;
                    //}
                    //else if (!reg.test(ctxtMarkets_Email.GetText())) {
                    //    //jAlert('Please enter valid email');
                    //    ctxtMarkets_Email.Focus();
                    //    return false;
                    //}
                    //else if (isNaN(ctxtMarkets_Phones.GetText()) || ctxtMarkets_Phones.GetText().length != 10) {
                    //    //jAlert('Please enter valid Phone No');
                    //    ctxtMarkets_Phones.Focus();
                    //    return false;
                    //}

                }

            //});


        //}
        function fn_btnCancel() {
            cPopup_market.Hide();
        }
        function fn_Editcity(keyValue) {
            grid.PerformCallback('Edit~' + keyValue);
        }
        function fn_Deletecity(keyValue) {
            jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                if (r == true) {

                    grid.PerformCallback('Delete~' + keyValue);
                }
            });
                
           
        }
        function grid_EndCallBack() {
            //debugger;
            
            if (grid.cpinsert != null) {
                if (grid.cpinsert == 'Success') {
                    jAlert('Saved Successfully');
                    cPopup_market.Hide();
                }
                else {
                    jAlert("Error On Insertion \n 'Please Try Again!!'");
                    cPopup_market.Hide();
                }
            }
            if (grid.cpEdit != null) {
                //debugger;
                //console.log(grid.cpEdit);
                //  jAlert(grid.cpEdit.split('~')[9]);
                ctxtMarkets_Code.SetText(grid.cpEdit.split('~')[0]);

                ctxtMarkets_Name.SetText(grid.cpEdit.split('~')[1]);

                ctxtMarkets_Description.SetText(grid.cpEdit.split('~')[2]);

                ctxtMarkets_Address.SetText(grid.cpEdit.split('~')[3]);
                ctxtMarkets_Email.SetText(grid.cpEdit.split('~')[4]);
                ctxtMarkets_Phones.SetText(grid.cpEdit.split('~')[5]);
                ctxtMarkets_WebSite.SetText(grid.cpEdit.split('~')[6]);
                ctxtMarkets_ContactPerson.SetText(grid.cpEdit.split('~')[7]);
                $("#<%=hdnMode.ClientID%>").val("edit");
                $("#<%=hdnCountryId.ClientID%>").val(grid.cpEdit.split('~')[10]);
                $("#<%=hdnStateId.ClientID%>").val(grid.cpEdit.split('~')[8]);
                $("#<%=hdnCityId.ClientID%>").val(grid.cpEdit.split('~')[9]);
                cCmbCountryName.SetValue(grid.cpEdit.split('~')[10]);
                cCmbState.PerformCallback("BindState~" + cCmbCountryName.GetValue());
                //cCmbState.SetValue(grid.cpEdit.split('~')[8]);
                //cCmbCity.PerformCallback("BindCity~" + cCmbState.GetValue());
                //cCmbCity.SetValue(grid.cpEdit.split('~')[9]);
                $("#<%=hdnCityId.ClientID%>").val(grid.cpEdit.split('~')[9]);
                GetObjectID('hiddenedit').value = grid.cpEdit.split('~')[11];
                cPopup_market.SetHeaderText('Modify Market');
                cPopup_market.Show();
                //    cCmbState.SetValue(grid.cpEdit.split('~')[8]);
            }else{
            if (grid.cpUpdate != null) {
                if (grid.cpUpdate == 'Success') {
                    //jAlert('Update Successfully');
                    jAlert('Update Successfully');
                    cPopup_market.Hide();
                }
                else {
                    jAlert("Error on Updation\n'Please Try again!!'")
                    cPopup_market.Hide();
                }
            }
            if (grid.cpUpdateValid != null) {
                if (grid.cpUpdateValid == "StateInvalid") {
                    jAlert("Please Select proper country state and city");
                    //cPopup_market.Show();
                    //cCmbState.Focus();
                    //jAlert(GetObjectID('<%#hiddenedit.ClientID%>').value);
                    //grid.PerformCallback('Edit~'+GetObjectID('<%#hiddenedit.ClientID%>').value);
                    //grid.cpUpdateValid=null;
                }
            }
            if (grid.cpDelete != null) {
                if (grid.cpDelete == 'Success') {
                    jAlert('Deleted Successfully');
                    grid.Refresh();
                }
                else {
                    
                    jAlert("Error on deletion\n'Please Try again!!'")
                }
            }
            if (grid.cpExists != null) {
                if (grid.cpExists == "Exists") {
                    jAlert('Record already Exists');
                    cPopup_market.Hide();
                }
                else {
                    jAlert("Error on operation \n 'Please Try again!!'")
                    cPopup_market.Hide();
                }
            }
            cPopup_market.Hide();
            }
        }
        function OnCmbCountryName_ValueChange() {
            //debugger;
            cCmbState.PerformCallback("BindState~" + cCmbCountryName.GetValue());
            //console.log("Country name: " + cCmbCountryName.GetValue());
            $("#<%=hdnCountryId.ClientID%>").val(cCmbCountryName.GetValue());
        }
        function CmbState_EndCallback() {
            //debugger;
            if ($("#<%=hdnMode.ClientID%>").val() == "edit") {
               <%-- console.log($("#<%=hdnStateId.ClientID%>").val());--%>
                cCmbState.SetValue($("#<%=hdnStateId.ClientID%>").val());
                // cCmbState.Focus();
                cCmbCity.PerformCallback("BindCity~" + $("#<%=hdnStateId.ClientID%>").val());
                //console.log("State name: " + cCmbState.GetValue());
            }
            else {
                //  cCmbState.SetSelectedIndex(0);
                //  cCmbState.Focus();
            }
        }
        function OnCmbStateName_ValueChange() {
            if ($("#<%=hdnMode.ClientID%>").val() != "edit") {
                cCmbCity.PerformCallback("BindCity~" + cCmbState.GetValue());
                $("#<%=hdnStateId.ClientID%>").val(cCmbState.GetValue());
            }
        }
        function CmbCity_ValuChanged(s, e) {
            if ($("#<%=hdnMode.ClientID%>").val() != "edit") {
                $('#hdnCityId').val(s.GetValue());
            }
        }
        function CmbCity_EndCallback() {
            if ($("#<%=hdnMode.ClientID%>").val() == "edit") {
                cCmbCity.SetValue($("#<%=hdnCityId.ClientID%>").val());
                //  cCmbCity.Focus();
                $("#<%=hdnMode.ClientID%>").val("");
            }
            else {
                // cCmbCity.SetSelectedIndex(0);
                //  cCmbCity.Focus();
                // $("#<%=hdnMode.ClientID%>").val("");
            }
        }
    </script>

    <style type="text/css">
        .cityDiv {
            height: 25px;
            width: 130px;
            float: left;
            margin-left: 70px;
        }

        .cityTextbox {
            height: 25px;
            width: 50px;
        }

        .Top {
            height: 90px;
            width: 400px;
            background-color: Silver;
            padding-top: 5px;
            valign: top;
        }

        .Footer {
            height: 30px;
            width: 400px;
            padding-top: 10px;
        }

        .ScrollDiv {
            height: 250px;
            width: 400px;
            background-color: Silver;
            overflow-x: hidden;
            overflow-y: scroll;
        }

        .ContentDiv {
            width: 400px;
            height: 300px;
            border: 2px;
        }

        

        .TitleArea {
            height: 20px;
            padding-left: 10px;
            padding-right: 3px;
            background-image: url( '../images/EHeaderBack.gif' );
            background-repeat: repeat-x;
            background-position: bottom;
            text-align: center;
        }

        .FilterSide {
            float: left;
            width: 50%;
        }

        .SearchArea {
            width: 100%;
            height: 30px;
            padding-top: 5px;
        }
    </style>
     <script type="text/javascript">

         function Page_Load() {
             cPopup_market.Hide();
         }

         function fn_ctxtMarkets_Code_TextChanged() {
             var procode = 1;
             if (GetObjectID('hiddenedit').value != '') {
                 procode = GetObjectID('hiddenedit').value;
             }

             //var ProductName = ctxtPro_Name.GetText();
             var ProductName = ctxtMarkets_Code.GetText();
             $.ajax({
                 type: "POST",
                 url: "sMarkets.aspx/CheckUniqueName",
                 //data: "{'ProductName':'" + ProductName + "'}",
                 data: JSON.stringify({ MarketsCode: ProductName, procode: procode }),
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 success: function (msg) {
                     var data = msg.d;

                     if (data == true) {
                         jAlert("Please enter unique name");

                         ctxtMarkets_Code.SetText("");
                         ctxtMarkets_Code.SetFocus(true);
                         //document.getElementById("Popup_market_ctxtPro_Code_I").focus();
                         //document.getElementById("ctxtMarkets_Code_I").focus();

                         return false;
                     }
                 }

             });
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Markets</h3>
        </div>
    </div>
    <div class="form_main">
        <div class="">
            <div class="clearfix">
                <div class="FilterSide">
                    <div style="float: left; padding-right: 5px;">
                        <% if(rights.CanAdd)
                           { %>
                        <a href="javascript:void(0);" onclick="fn_PopOpen();return false;" class="btn btn-success btn-radius"><span class="btn-icon"><i class="fa fa-plus" ></i></span><span>Add New</span></a>
                        <% } %>
                    </div>
                    <% if (rights.CanExport)
                                               { %>
                    <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-radius" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true"  OnChange="if(!AvailableExportOption()){return false;}">
                                                <asp:ListItem Value="0">Export to</asp:ListItem>
                                                <asp:ListItem Value="1">PDF</asp:ListItem>
                                                 <asp:ListItem Value="2">XLS</asp:ListItem>
                                                 <asp:ListItem Value="3">RTF</asp:ListItem>
                                                 <asp:ListItem Value="4">CSV</asp:ListItem>
                                            </asp:DropDownList>
                      <% } %>
                  
            </div>
        </div>
            <div class="clear"></div>
        <div class="GridViewArea relative">
            <dxe:ASPxGridView ID="cityGrid" runat="server" AutoGenerateColumns="False" ClientInstanceName="grid"
                KeyFieldName="Markets_ID" Width="100%" OnHtmlRowCreated="cityGrid_HtmlRowCreated" Settings-VerticalScrollableHeight="280" Settings-VerticalScrollBarMode="Auto"
                OnHtmlEditFormCreated="cityGrid_HtmlEditFormCreated" OnCustomCallback="cityGrid_CustomCallback" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false"  >
                <SettingsSearchPanel Visible="True"  Delay="5000"/>
                <settings showgrouppanel="True" showstatusbar="Hidden" showfilterrow="true" ShowFilterRowMenu="True" />
                <settingsbehavior allowfocusedrow="true" />
                <columns>
                    <dxe:GridViewDataTextColumn Caption="ID" FieldName="Markets_ID" ReadOnly="True"
                        Visible="False" FixedStyle="Left" VisibleIndex="0">
                        <EditFormSettings Visible="False" />
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="id" FieldName="cou_id" ReadOnly="True" Visible="False"
                        FixedStyle="Left" VisibleIndex="0">
                        <EditFormSettings Visible="False" />
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="State" FieldName="Markets_State" ReadOnly="True"
                        Visible="False" FixedStyle="Left" VisibleIndex="1">
                        <EditFormSettings Visible="False" />
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="City" FieldName="Markets_City" ReadOnly="True"
                        Visible="False" FixedStyle="Left" VisibleIndex="2">
                        <EditFormSettings Visible="False" />
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Short Name (Unique)" FieldName="Markets_Code" Width="12%"
                        FixedStyle="Left" Visible="True" VisibleIndex="3" CellStyle-Wrap="True">
                        <EditFormSettings Visible="True"  />

<CellStyle Wrap="True"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Name" FieldName="Markets_Name" Width="12%" CellStyle-Wrap="True"
                        FixedStyle="Left" Visible="True" VisibleIndex="4">
                        <EditFormSettings Visible="True" />

<CellStyle Wrap="True"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Description" FieldName="Markets_Description"
                        Width="12%" FixedStyle="Left" Visible="True" VisibleIndex="5">
                        <EditFormSettings Visible="True" />
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Country" FieldName="cou_country" Visible="True"
                        Width="7%" VisibleIndex="6">
                        <CellStyle CssClass="gridcellleft" Wrap="False">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="State" FieldName="state" Width="12%" FixedStyle="Left"
                        Visible="True" VisibleIndex="7">
                        <EditFormSettings Visible="True" />
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="City Name" FieldName="city_name" Width="12%"
                        FixedStyle="Left" Visible="True" VisibleIndex="8">
                        <EditFormSettings Visible="True" />
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn ReadOnly="True" Width="0">
                        <HeaderStyle HorizontalAlign="Center" />
                        <HeaderTemplate>
                            
                        </HeaderTemplate>
                        <DataItemTemplate>
                            <div class='floatedBtnArea'>
                            <% if(rights.CanEdit)
                               { %>
                            <a href="javascript:void(0);" onclick="fn_Editcity('<%# Container.KeyValue %>')" class="">
                                <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Edit</span></a>
                            <% } %>
                            <% if(rights.CanDelete)
                               { %>
                            <a href="javascript:void(0);" onclick="fn_Deletecity('<%# Container.KeyValue %>')">
                                <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>Delete</span></a>
                            <% } %>
                            </div>
                        </DataItemTemplate>
                        <CellStyle HorizontalAlign="Center">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                </columns>
                <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                <clientsideevents endcallback="function (s, e) {grid_EndCallBack();}" RowClick="gridRowclick" />
            </dxe:ASPxGridView>
        </div>
        <div class="PopUpArea">
            <%--<dxe:ASPxPopupControl ID="Popup_market" runat="server" ClientInstanceName="cPopup_market"
                Width="400px" HeaderText="Add Market Details" PopupHorizontalAlign="WindowCenter"
                BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
                Modal="True" ContentStyle-VerticalAlign="Top" >--%>
            <dxe:ASPxPopupControl ID="Popup_market" runat="server"
        CloseAction="CloseButton" Top="50" Left="600" ClientInstanceName="cPopup_market" Height="400px"
        Width="450px"  Modal="true" AllowResize="true" ResizingMode="Postponed" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
               <%-- EnableHierarchyRecreation="True"--%>
                <contentcollection>
                    <dxe:PopupControlContentControl runat="server">
                        <%--<div style="Width:400px;background-color:#FFFFFF;margin:0px;border:1px solid red;">--%>
                        <div>
                            <div>
                                <div class="cityDiv" style="display: inline-block; height: auto; margin-left: 40px;">
                                    Short Name (Unique)<span style="color:red"> *</span>
                                </div>
                                <div class="Left_Content" style="display: inline-block">
                                    <dxe:ASPxTextBox ID="txtMarkets_Code" MaxLength="80" ClientInstanceName="ctxtMarkets_Code"
                                        runat="server" Width="180px">
                                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="true" ValidationGroup="ma">
                                            <RequiredField IsRequired="true" ErrorText="Mandatory" />
                                        </ValidationSettings>
                                        <ClientSideEvents TextChanged="function(s,e){fn_ctxtMarkets_Code_TextChanged()}" />
                                     </dxe:ASPxTextBox>
                                    
                                </div>
                            </div>
                            <div>
                                <div class="cityDiv" style="display: inline-block; height: auto; margin-left: 40px;">
                                    Name<span style="color:red"> *</span>
                                </div>
                                <div class="Left_Content" style="display: inline-block">
                                    <dxe:ASPxTextBox ID="txtMarkets_Name" MaxLength="100" ClientInstanceName="ctxtMarkets_Name" runat="server" Width="180px">
                                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="true" ValidationGroup="ma">
                                            <RequiredField IsRequired="true" ErrorText="Mandatory"/>
                                        </ValidationSettings>
                                    </dxe:ASPxTextBox>
                                </div>
                            </div>
                            <div>
                                <div class="cityDiv" style="display: inline-block; height: auto; margin-left: 40px;">
                                    Description
                                </div>
                                <div class="Left_Content" style="display: inline-block">
                                    <dxe:ASPxMemo ID="txtMarkets_Description" MaxLength="300" ClientInstanceName="ctxtMarkets_Description"
                                        runat="server" Width="180px" Height="60px" Text='<%# Bind("txtMarkets_Description") %>'>
                                    </dxe:ASPxMemo>
                                </div>
                            </div>
                            <div>
                                <div class="cityDiv" style="display: inline-block; height: auto; margin-left: 40px;">
                                    Address
                                </div>
                                <div class="Left_Content" style="display: inline-block">
                                    <dxe:ASPxTextBox ID="txtMarkets_Address" MaxLength="120" ClientInstanceName="ctxtMarkets_Address"
                                        runat="server" Width="180px">
                                    </dxe:ASPxTextBox>
                                </div>
                            </div>
                            <div>
                                <div class="cityDiv" style="display: inline-block; height: auto; margin-left: 40px;">
                                    Country
                                </div>
                                <div class="Left_Content" style="display: inline-block">
                                    <dxe:ASPxComboBox ID="CmbCountryName" ClientInstanceName="cCmbCountryName" runat="server" NullText="Select" SelectedIndex="-1"
                                        ValueType="System.String" Width="180px" EnableSynchronization="True" EnableIncrementalFiltering="True">
                                        <ClientSideEvents ValueChanged="function(s,e){OnCmbCountryName_ValueChange()}"></ClientSideEvents>
                                    </dxe:ASPxComboBox>
                                </div>
                            </div>
                            <div>
                                <div class="cityDiv" style="display: inline-block; height: auto; margin-left: 40px;">
                                    State
                                </div>
                                <div class="Left_Content" style="display: inline-block">
                                    <dxe:ASPxComboBox ID="CmbState" ClientInstanceName="cCmbState" runat="server" ValueType="System.String" NullText="Select" SelectedIndex="-1"
                                        Width="180px" EnableSynchronization="True" OnCallback="CmbState_Callback" EnableIncrementalFiltering="True">
                                        <ClientSideEvents EndCallback="CmbState_EndCallback" ValueChanged="function(s,e){OnCmbStateName_ValueChange()}"></ClientSideEvents>
                                    </dxe:ASPxComboBox>
                                </div>
                            </div>
                            <div>
                                <div class="cityDiv" style="display: inline-block; height: auto; margin-left: 40px;">
                                    City
                                </div>
                                <div class="Left_Content" style="display: inline-block">
                                    <%-- <dxe:ASPxTextBox ID="txtcityName" ClientInstanceName="ctxtcityName" runat="server"
                                    Width="180px">
                                </dxe:ASPxTextBox>--%>
                                    <dxe:ASPxComboBox ID="CmbCity" ClientInstanceName="cCmbCity" runat="server" NullText="Select" SelectedIndex="-1"
                                        Width="180px" EnableSynchronization="True" OnCallback="CmbCity_Callback" EnableIncrementalFiltering="True">
                                        <ClientSideEvents EndCallback="CmbCity_EndCallback" ValueChanged="CmbCity_ValuChanged"></ClientSideEvents>
                                    </dxe:ASPxComboBox>
                                    <asp:HiddenField runat="server" ID="hdnMode" ClientIDMode="Static" Value="" />
                                    <asp:HiddenField runat="server" ID="hdnCountryId" ClientIDMode="Static" Value="" />
                                    <asp:HiddenField runat="server" ID="hdnStateId" ClientIDMode="Static" Value="" />
                                    <asp:HiddenField runat="server" ID="hdnCityId" ClientIDMode="Static" Value="" />
                                </div>
                            </div>
                            <div>
                                <div class="cityDiv" style="display: inline-block; height: auto; margin-left: 40px;">
                                    Email
                                </div>
                                <div class="Left_Content" style="display: inline-block">
                                    <dxe:ASPxTextBox ID="txtMarkets_Email" MaxLength="100" ClientInstanceName="ctxtMarkets_Email" runat="server" 
                                        ValidationSettings-ValidationGroup="ma"
                                        Width="180px">
                                         <ValidationSettings ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="true">
                                            <RequiredField IsRequired="true" ErrorText="Mandatory"/>
                                               <RegularExpression ErrorText="Enter Valid Email ID" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                                        </ValidationSettings>
                                    </dxe:ASPxTextBox>
                                </div>
                            </div>
                            <div>
                                <div class="cityDiv" style="display: inline-block; height: auto; margin-left: 40px;">
                                    Phones
                                </div>
                                <div class="Left_Content" style="display: inline-block">
                                    <dxe:ASPxTextBox ID="txtMarkets_Phones" MaxLength="10" ClientInstanceName="ctxtMarkets_Phones" ValidationSettings-ValidationGroup="ma"
                                        runat="server" Width="180px">
                                         <ValidationSettings ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="true">
                                            <RequiredField IsRequired="true" ErrorText="Mandatory"/>
                                        </ValidationSettings>
                                         <ClientSideEvents KeyPress="function(s,e){ fn_AllowonlyNumeric(s,e);}" />
                                    </dxe:ASPxTextBox>
                                </div>
                            </div>
                            <div>
                                <div class="cityDiv" style="display: inline-block; height: auto; margin-left: 40px;">
                                    Website
                                </div>
                                <div class="Left_Content" style="display: inline-block">
                                    <dxe:ASPxTextBox ID="txtMarkets_WebSite" ClientInstanceName="ctxtMarkets_WebSite" MaxLength="100"
                                        runat="server" Width="180px">
                                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" SetFocusOnError="true">
                                            <RegularExpression ValidationExpression="(http(s)?://)?([\w-]+\.)+[\w-]+[\w-]+[\.]+[\.com]+([./?%&=]*)?" ErrorText="Enter valid url" />
                                        </ValidationSettings>
                                    </dxe:ASPxTextBox>
                                </div>
                            </div>
                            <div>
                                <div class="cityDiv" style="display: inline-block; height: auto; margin-left: 40px;">
                                    Contact Person
                                </div>
                                <div class="Left_Content" style="display: inline-block">
                                    <dxe:ASPxTextBox ID="txtMarkets_ContactPerson" MaxLength="300" ClientInstanceName="ctxtMarkets_ContactPerson"
                                        runat="server" Width="180px">
                                    </dxe:ASPxTextBox>
                                </div>
                            </div>
                        </div>
                        <div class="ContentDiv" style="height: auto">
                            <div style="display: none">
                                <div style="height: 20px; width: 280px; background-color: Gray; padding-left: 120px;">
                                    <h5>Static Code</h5>
                                </div>
                                <div style="height: 20px; width: 130px; padding-left: 70px; background-color: Gray; float: left;">
                                    Exchange
                                </div>
                                <div style="height: 20px; width: 200px; background-color: Gray; text-align: left;">
                                    Value
                                </div>
                                <div class="ScrollDiv">
                                    <div class="cityDiv" style="padding-top: 5px;">
                                        NSE Code
                                    </div>
                                    <div style="padding-top: 5px;">
                                        <dxe:ASPxTextBox ID="txtNseCode" ClientInstanceName="ctxtNseCode" runat="server"
                                            CssClass="cityTextbox">
                                        </dxe:ASPxTextBox>
                                    </div>
                                    <br style="clear: both;" />
                                    <div class="cityDiv">
                                        BSE Code
                                    </div>
                                    <div>
                                        <dxe:ASPxTextBox ID="txtBseCode" ClientInstanceName="ctxtBseCode" runat="server"
                                            CssClass="cityTextbox">
                                        </dxe:ASPxTextBox>
                                    </div>
                                    <br style="clear: both;" />
                                    <div class="cityDiv">
                                        MCX Code
                                    </div>
                                    <div>
                                        <dxe:ASPxTextBox ID="txtMcxCode" ClientInstanceName="ctxtMcxCode" runat="server"
                                            CssClass="cityTextbox">
                                        </dxe:ASPxTextBox>
                                    </div>
                                    <br style="clear: both;" />
                                    <div class="cityDiv">
                                        MCXSX Code
                                    </div>
                                    <div>
                                        <dxe:ASPxTextBox ID="txtMcsxCode" ClientInstanceName="ctxtMcsxCode" runat="server"
                                            CssClass="cityTextbox">
                                        </dxe:ASPxTextBox>
                                    </div>
                                    <br style="clear: both;" />
                                    <div class="cityDiv">
                                        NCDEX Code
                                    </div>
                                    <div>
                                        <dxe:ASPxTextBox ID="txtNcdexCode" ClientInstanceName="ctxtNcdexCode" runat="server"
                                            CssClass="cityTextbox">
                                        </dxe:ASPxTextBox>
                                    </div>
                                    <br style="clear: both;" />
                                    <div class="cityDiv">
                                        CDSL Code
                                    </div>
                                    <div>
                                        <dxe:ASPxTextBox ID="txtCdslCode" ClientInstanceName="ctxtCdslCode" CssClass="cityTextbox"
                                            runat="server">
                                        </dxe:ASPxTextBox>
                                    </div>
                                    <br style="clear: both;" />
                                    <div class="cityDiv">
                                        NSDL Code
                                    </div>
                                    <div>
                                        <dxe:ASPxTextBox ID="txtNsdlCode" ClientInstanceName="ctxtNsdlCode" CssClass="cityTextbox"
                                            runat="server">
                                        </dxe:ASPxTextBox>
                                    </div>
                                    <br style="clear: both;" />
                                    <div class="cityDiv">
                                        NDML Code
                                    </div>
                                    <div>
                                        <dxe:ASPxTextBox ID="txtNdmlCode" ClientInstanceName="ctxtNdmlCode" runat="server"
                                            CssClass="cityTextbox">
                                        </dxe:ASPxTextBox>
                                    </div>
                                    <br style="clear: both;" />
                                    <div class="cityDiv">
                                        CVL Code
                                    </div>
                                    <div>
                                        <dxe:ASPxTextBox ID="txtCvlCode" ClientInstanceName="ctxtCvlCode" runat="server"
                                            CssClass="cityTextbox">
                                        </dxe:ASPxTextBox>
                                    </div>
                                    <br style="clear: both;" />
                                    <div class="cityDiv">
                                        DOTEX Code
                                    </div>
                                    <div>
                                        <dxe:ASPxTextBox ID="txtDotexCode" ClientInstanceName="ctxtDotexCode" runat="server"
                                            CssClass="cityTextbox">
                                        </dxe:ASPxTextBox>
                                    </div>
                                </div>
                            </div>
                            <br style="clear: both;" />
                            <div class="Footer">
                                <div style="padding-left:171px">
                                    <dxe:ASPxButton ID="btnSave_citys" ClientInstanceName="cbtnSave_citys" runat="server" ValidationGroup="ma" CausesValidation="true"
                                         Text="Save" CssClass="btn btn-primary">
                                        <ClientSideEvents Click="function (s, e) { btnSave_citys(); e.processOnServer = false; }" />
                                    </dxe:ASPxButton>
                                    <dxe:ASPxButton ID="btnCancel_citys" runat="server" AutoPostBack="False" Text="Cancel" CssClass="btn btn-danger">
                                        <ClientSideEvents Click="function (s, e) {fn_btnCancel();}" />
                                    </dxe:ASPxButton>
                                </div>
                                <br style="clear: both;" />
                            </div>
                            <br style="clear: both;" />
                        </div>
                        <%-- </div>--%>
                    </dxe:PopupControlContentControl>
                </contentcollection>
                <headerstyle forecolor="White" />
            </dxe:ASPxPopupControl>
            <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="true" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
            </dxe:ASPxGridViewExporter>
        </div>
        <div class="HiddenFieldArea" style="display: none;">
            <asp:HiddenField runat="server" ID="hiddenedit" />
        </div>
    </div>

</asp:Content>
