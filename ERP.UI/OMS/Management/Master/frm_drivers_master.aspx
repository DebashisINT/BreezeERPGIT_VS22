<%@ Page Title="Driver Master" Language="C#" AutoEventWireup="true"  MasterPageFile="~/OMS/MasterPage/ERP.Master" CodeBehind="frm_drivers_master.aspx.cs" Inherits="ERP.OMS.Management.Master.frm_drivers_master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script>

        function OpenMapUsers(sid,useruniqueID,InternalId) {

            cPopup_DriverUser.Show();
            $("#hdnUniqueID").val($("#drpuser").val());
            $("#hdnUniqueName").val(useruniqueID);
            $("#hdndriverinternalId").val(InternalId);
            grid.PerformCallback('DriverUsersModifylist~' + $("#hdnUniqueName").val() + '~' + $("#hdnUniqueID").val());
        }

        function CallDrivers_save() {
                cPopup_DriverUser.Hide();
                grid.PerformCallback('DriverSaveMapUser~' + $("#drpuser").val() + '~' + $("#hdnUniqueName").val()+'~'+  $("#hdndriverinternalId").val());
                grid.Refresh();
               
        }


        function CancelCallDrivers_save() {
            cPopup_DriverUser.Hide();
        }



    </script>

    <script language="javascript" type="text/javascript">
        function OnViewClick(keyValue) {           
            cproductpopup.Show();
            popproductPanel.PerformCallback('ShowHistory~' + keyValue);
        }


        function OnEditButtonClick(keyValue) {
          
            var url = 'DriverAddEdit.aspx?id=' + keyValue;
           
            window.location.href = url;
        }
        function EndCall() {
            if (grid.cpDelmsg != null) {
             
                jAlert(grid.cpDelmsg);
            }

            else if (grid.cpsuccessmsg == "Already assigned to different User.") {
                alert();
                cPopup_DriverUser.Show();
               
                jAlert(grid.cpsuccessmsg);

            }
           else  if (grid.cpsuccessmsg == "Succesfully Inserted.") {
                cPopup_DriverUser.Hide();

                jAlert(grid.cpsuccessmsg);

            }

            else  if (grid.cpuserbind != null) {
                $("#drpuser").val(grid.cpuserbind);
                //cPopup_DriverUser.Show();
            }
           else   if (grid.cppopupopen != null) {
           
                //cPopup_DriverUser.Show();
            }


        }
        function OnAddButtonClick() {
            var url = 'DriverAddEdit.aspx?id=ADD';
          
            window.location.href = url;
        }






        function DeleteRow(keyValue) {

            jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    grid.PerformCallback('Delete~' + keyValue);
                }
            });
        }


        function ShowHideFilter1(obj) {
            gridTerminal.PerformCallback(obj);
        }

        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }
        function Page_Load() {
            document.getElementById("TdCombo").style.display = "none";
        }
        function callback() {
            grid.PerformCallback();
        }
        function gridRowclick(s, e) {
            $('#gridStatus').find('tr').removeClass('rowActive');
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
            <h3>Driver Master</h3>
        </div>
    </div>

    <div class="form_main">
        <table class="TableMain100" style="width: 100%">
            <tr>
                <td style="text-align: left; vertical-align: top">
                    <table>
                        <tr>
                            <td id="ShowFilter">
                                 <% if (rights.CanAdd)
                                   { %>
                                <a href="javascript:void(0);" onclick="javascript:OnAddButtonClick();" class="btn btn-success btn-radius"><span class="btn-icon"><i class="fa fa-plus" ></i></span> Add New</a>
                                  <% } %>
                                <% if (rights.CanExport)
                                               { %>
                                <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-radius " OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true"  >
                                    <asp:ListItem Value="0">Export to</asp:ListItem>
                                    <asp:ListItem Value="1">PDF</asp:ListItem>
                                     <asp:ListItem Value="2">XLS</asp:ListItem>
                                     <asp:ListItem Value="3">RTF</asp:ListItem>
                                     <asp:ListItem Value="4">CSV</asp:ListItem>
                        
                                </asp:DropDownList>
                                 <% } %>
                               
                              
                            </td>
                            <td id="Td1">
                              
                            </td>
                        </tr>
                    </table>
                </td>
                
            </tr>
            <tr>
                <td style="vertical-align: top; text-align: left" colspan="2" class="relative">
                    <dxe:ASPxGridView ID="gridStatus" ClientInstanceName="grid" Width="100%"
                        KeyFieldName="cnt_id" DataSourceID="gridStatusDataSource" runat="server" Settings-VerticalScrollableHeight="280" Settings-VerticalScrollBarMode="Auto"
                        AutoGenerateColumns="False" OnCustomCallback="gridStatus_CustomCallback"  SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false"   >
                          <SettingsSearchPanel Visible="True" Delay="5000" />         
                         <clientsideevents endcallback="EndCall" />
                        <Columns>
                            <dxe:GridViewDataTextColumn FieldName="cnt_UCC" Caption="Unique ID" VisibleIndex="1">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="cnt_firstName"
                                Caption="Driver Name">
                                <CellStyle CssClass="gridcellleft" Wrap="True">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                             <dxe:GridViewDataTextColumn Caption="Assigned Vehicle" FieldName="VehicleNO" VisibleIndex="3">
                                 <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>

                             <dxe:GridViewDataTextColumn Caption="Branch Name" FieldName="branch_description" VisibleIndex="4">
                                 <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Assigned User" FieldName="Assigneduser" VisibleIndex="5">
                                 <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>


                            <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="cnt_VerifcationRemarks"
                                Caption="Remarks">
                                <CellStyle CssClass="gridcellleft" Wrap="True">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="7" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" Width="0">
                               
                                <DataItemTemplate>
                                    <div class='floatedBtnArea'>
                                  <a href="javascript:void(0)" onclick="OpenMapUsers('<%# Container.KeyValue %>','<%#Eval("cnt_UCC") %>','<%#Eval("cnt_internalId") %>')" class="pad">
                                        <span class='ico ColorSeven'><i class='fa fa-user'></i></span><span class='hidden-xs'>Map User</span> </a>
                                  </a>


                                     <% if (rights.CanHistory)
                            { %>
                        <a href="javascript:void(0);" onclick="OnViewClick('<%# Container.KeyValue %>')" class="pad" title="">
                            <span class='ico ColorFour'><i class='fa fa-history'></i></span><span class='hidden-xs'>View History</span></a>
                           <% } %>
                                    
                                   
                                   
                                     <% if (rights.CanEdit)
                                        { %>
                                    <a href="javascript:void(0);" onclick="OnEditButtonClick('<%# Container.KeyValue %>')" title="" class="">
                                        <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Edit</span>
                                    </a>
                                      <% } %>
                                     <% if (rights.CanDelete)
                                       { %>
                                     <a href="javascript:void(0);" onclick="DeleteRow('<%# Container.KeyValue %>')" title="">
                                        <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>Delete</span></a>
                                     <% } %>
                                        </div>
                                </DataItemTemplate>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                <HeaderTemplate>Actions</HeaderTemplate>
                               
                                <EditFormSettings Visible="False"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Caption="Id" FieldName="cnt_id" Visible="False" VisibleIndex="0">
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>

                           

                        </Columns>
                        <ClientSideEvents RowClick="gridRowclick" />
                        <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                          <settingspager pagesize="10">
                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200"/>
                    </settingspager>
                        <SettingsText ConfirmDelete="Confirm delete?" />
                        <StylesEditors>
                            <ProgressBar Height="25px">
                            </ProgressBar>
                        </StylesEditors>
                        <SettingsSearchPanel Visible="True" />
                        <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowFilterRow="true" ShowFilterRowMenu = "True" />
                        
                        <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True" />
                    </dxe:ASPxGridView>
                </td>
            </tr>
        </table>
        <dxe:ASPxGridViewExporter ID="exporter" runat="server">
        </dxe:ASPxGridViewExporter>
        <asp:SqlDataSource ID="gridStatusDataSource" runat="server" 
            SelectCommand="">
            <SelectParameters>
                <asp:SessionParameter Name="userlist" SessionField="userchildHierarchy" Type="string" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>

    <%--Product Name Detail Invoice Wise--%>
<dxe:ASPxPopupControl ID="productpopup" ClientInstanceName="cproductpopup" runat="server"
AllowDragging="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" HeaderText="Driver's Assigned Vehicle History"
EnableHotTrack="False" BackColor="#DDECFE" Width="660px" CloseAction="CloseButton">
<ContentCollection>
<dxe:PopupControlContentControl runat="server">
<dxe:ASPxCallbackPanel ID="propanel" runat="server" Width="650px" ClientInstanceName="popproductPanel"
    OnCallback="propanel_Callback1" >
    <PanelCollection>
        <dxe:PanelContent runat="server">
                <div>
                    <dxe:ASPxGridView ID="grdproduct" runat="server" KeyFieldName="DriversInternalID" AutoGenerateColumns="False"
                            Width="100%" ClientInstanceName="cpbproduct">
                            <Columns>
                                <dxe:GridViewDataTextColumn  FieldName="VehiclesRegNo" Caption="Vehicle No." HeaderStyle-CssClass="text-center"
                                    VisibleIndex="0" FixedStyle="Left" Width="150px">
                                    <CellStyle CssClass="text-center" Wrap="true" >
                                    </CellStyle>
                                    <Settings AutoFilterCondition="Contains"  />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn  FieldName="CreatedBy" Caption="Entered By" HeaderStyle-CssClass="text-center"
                                    VisibleIndex="0" FixedStyle="Left" Width="200px">
                                    <CellStyle CssClass="text-center" Wrap="true" >
                                    </CellStyle>
                                    <Settings AutoFilterCondition="Contains"  />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="CreatedDate" Caption="Assigned On" HeaderStyle-CssClass="text-center"
                                    VisibleIndex="0" FixedStyle="Left" Width="150px">
                                    <CellStyle CssClass="text-center" Wrap="true" >
                                    </CellStyle>
                                    <Settings AutoFilterCondition="Contains"  />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn  FieldName="ChangedDate" Caption="Unassigned On" HeaderStyle-CssClass="text-center"
                                    VisibleIndex="0" FixedStyle="Left" Width="150px">
                                    <CellStyle CssClass="text-center" Wrap="true" >
                                    </CellStyle>
                                    <Settings AutoFilterCondition="Contains"  />
                                </dxe:GridViewDataTextColumn>
                                
                                </Columns>
                        </dxe:ASPxGridView>
                </div>
        </dxe:PanelContent>
    </PanelCollection> 
</dxe:ASPxCallbackPanel>
</dxe:PopupControlContentControl>
</ContentCollection>
<HeaderStyle HorizontalAlign="Left">
<Paddings PaddingRight="6px" />
</HeaderStyle>
<SizeGripImage Height="16px" Width="16px" />
<CloseButtonImage Height="12px" Width="13px" />
<ClientSideEvents CloseButtonClick="function(s, e) {
cproductpopup.Hide();
}" />
</dxe:ASPxPopupControl>
     <%--Product Name Detail Invoice Wise--%>

    <%--Driver Assign--%>
    <dxe:ASPxPopupControl ID="Popup_Driveruser" runat="server" ClientInstanceName="cPopup_DriverUser"
            Width="400px" HeaderText="Map User" PopupHorizontalAlign="WindowCenter"
            BackColor="white" Height="100px" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <div class="Top clearfix">

                        <table style="width:94%">
                           
                            <tr><td>Add User Map<span style="color: red">*</span></td>
                                <td class="relative">
                                    <asp:DropDownList ID="drpuser" runat="server"></asp:DropDownList>
                                         
                                </td></tr>
              
                            <tr>
                                <td colspan="3" style="padding-left: 121px;">
                                    <input id="btnFeedbackSave" class="btn btn-primary" onclick="CallDrivers_save()" type="button" value="Save" />
                                    <input id="btnFeedbackCancel" class="btn btn-danger" onclick="CancelCallDrivers_save()" type="button" value="Cancel" />
                                </td>

                            </tr>
                        </table>


                    </div>

                </dxe:PopupControlContentControl>
            </ContentCollection>
            <HeaderStyle BackColor="LightGray" ForeColor="Black" />

          
        </dxe:ASPxPopupControl>


    <asp:HiddenField ID="hdnUniqueID" runat="server" />
    <asp:HiddenField ID="hdnUniqueName" runat="server" />
    <asp:HiddenField ID="hdndriverinternalId" runat="server" />
    <%--Driver Assign--%>


</asp:Content>