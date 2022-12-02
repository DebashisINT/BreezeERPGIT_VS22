<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="SrvProbMast.aspx.cs" Inherits="ERP.OMS.Management.Master.SrvProbMast" %>

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
    
        .dxgvHeader {
            border: 1px solid #2c4182 !important;
            background-color: #415698 !important;
        }

            .dxgvHeader, .dxgvHeader table {
                color: #fff !important;
            }
    </style>
    <script type="text/javascript" src="../../CentralData/JSScript/GenericJScript.js"></script>

    <script language="javascript" type="text/javascript">
      
        function gridRowclick(s, e) {
            //alert('hi');
            $('#gridcrmCampaign').find('tr').removeClass('rowActive');
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

      <script type="text/javascript">
          function ShowHideFilter(obj) {
              grid.PerformCallback(obj);
          }
          function fn_PopUpOpen() {
              $('#valid').attr('style', 'display:none;');
              chfID.Set("hfID", '');
              ctxtProbDesc.SetText('');
              cPopupProb.SetHeaderText('Add Problem');
             
              cPopupProb.Show();

          }
          function fn_EditProb(keyValue) {
              grid.PerformCallback('Edit~' + keyValue);
          }
          function fn_DeleteProb(keyValue) {
             
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
              cPopupProb.Hide();
          }
          function btnSave_Prob() {
              var Probnm = ctxtProbDesc.GetText();
              if (Probnm.trim() == '')
              {
                  $('#valid').attr('style', 'display:block;position: absolute;right: 32px;top: 17px;');
                  // alert('Please Enter Problem Name');
                  ctxtProbDesc.Focus();
              }
              else {
                  var id = chfID.Get('hfID');
                  if (id == '')
                      grid.PerformCallback('saveProb~' + ctxtProbDesc.GetText());
                  else
                      grid.PerformCallback('updateProb~' + chfID.Get('hfID'));
              }
          }


          function grid_EndCallBack() {
              if (grid.cpEdit != null) {
                  ctxtProbDesc.SetText(grid.cpEdit.split('~')[0]);
                  var hfid = grid.cpEdit.split('~')[1];
                  cPopupProb.SetHeaderText('Modify Problem');
                  chfID.Set("hfID", hfid);
                  cPopupProb.Show();
              }

              if (grid.cpinsert != null) {
                  if (grid.cpinsert == 'Success') {
                      jAlert('Saved successfully');
                      cPopupProb.Hide();
                  }
                  else {
                      jAlert("Error On Insertion\n'Please Try Again!!'");
                  }
              }

              if (grid.cpExists != null) {
                  if (grid.cpExists == 'Exists') {
                      jAlert('Duplicate value');
                      cPopupProb.Hide();
                  }

              }

              if (grid.cpUpdate != null) {
                  if (grid.cpUpdate == 'Success') {
                      jAlert('Updated successfully');
                      grid.cpUpdate = null;
                      cPopupProb.Hide();
                  }
                  else {
                      jAlert("Error on Updation\n'Please Try again!!'")
                      grid.cpUpdate = null;
                  }
              }


              if (grid.cpDelete != null) {
                  if (grid.cpDelete == 'Success') {
                      jAlert(grid.cpDelete);
                      grid.cpDelete = null;
                      grid.PerformCallback();
                  }
                  else {
                      jAlert(grid.cpDelete)
                      grid.cpDelete = null;
                      grid.PerformCallback();
                  }
              }
          }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

     <div class="panel-heading">
        <div class="panel-title">
            <h3>Problem</h3>
        </div>
    </div>
    <div class="form_main">
        <div class="Main">
          
            <div class="SearchArea clearfix">
                <div class="FilterSide">
                    <div style="float: left; padding-right: 5px;">
                        <% if (rights.CanAdd)
                           { %>
                        <a href="javascript:void(0);" onclick="fn_PopUpOpen()" class="btn btn-success btn-radius"><span class="btn-icon"><i class="fa fa-plus" ></i></span> Problem </a>
                        <% } %>
                        <%--<a href="javascript:ShowHideFilter('s');" class="btn btn-primary"><span>Show Filter</span></a>--%>
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
                                     
                </div>

            </div>

            <div class="GridViewArea relative">
                <dxe:ASPxGridView ID="GridProb" runat="server" AutoGenerateColumns="False" ClientInstanceName="grid"
                    KeyFieldName="ProblemID" Width="100%" OnHtmlEditFormCreated="GridProb_HtmlEditFormCreated" Settings-VerticalScrollableHeight="280" Settings-VerticalScrollBarMode="Auto"
                    OnCustomCallback="GridProb_CustomCallback" SettingsBehavior-AllowFocusedRow="true" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" >
                    <%--OnHtmlRowCreated="GridProb_HtmlRowCreated"--%>
                    <SettingsSearchPanel Visible="True" Delay="5000" />
                    <Columns>
                        <dxe:GridViewDataTextColumn Caption="Problem ID" FieldName="ProblemID" ReadOnly="True"
                            Visible="False" VisibleIndex="0">
                            <EditCellStyle HorizontalAlign="Left">
                            </EditCellStyle>
                            <EditFormSettings Visible="False" VisibleIndex="1" />
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="Problem Description" FieldName="ProblemDesc" VisibleIndex="1"
                            Width="100%">
                            <EditFormSettings Visible="True" />
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" Width="1px">
                            <HeaderTemplate>
                                <span></span>
                            </HeaderTemplate>
                            <DataItemTemplate>
                                <div class='floatedBtnArea'>
                                <% if (rights.CanEdit)
                                   { %>
                                <a href="javascript:void(0);" onclick="fn_EditProb('<%#Container.KeyValue %>')" class="" title="">
                                    <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Edit</span></a>
                                <% } %>
                                <% if (rights.CanDelete)
                                   { %>
                                <a href="javascript:void(0);" onclick="fn_DeleteProb('<%#Container.KeyValue %>')" title="">
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
                <dxe:ASPxPopupControl ID="PopupProb" runat="server" ClientInstanceName="cPopupProb"
                    Width="750px" Height="100px" HeaderText="Add/Modify Problem" PopupHorizontalAlign="Windowcenter"
                    PopupVerticalAlign="WindowCenter" CloseAction="closeButton" Modal="true">
                    <ContentCollection>
                        <dxe:PopupControlContentControl ID="ProbPopup" runat="server">
                            <div class="Top clearfix">
                                <div style="padding-top: 5px;" class="col-md-12">
                                    <div class="stateDiv" style="padding-top: 5px; width:100px;">Problem Desc.:<span style="color: red;">*</span></div>
                                    <div style="padding-top: 5px;">
                                        <dxe:ASPxTextBox ID="txtProbDesc" ClientInstanceName="ctxtProbDesc" ClientEnabled="true"
                                            runat="server" Width="600px" Height="30px" MaxLength="500">
                                        </dxe:ASPxTextBox>
                                       
                                        <div id="valid" style="display: none; position: absolute; right: -4px; top: 30px;">
                                            <img id="grid_DXPEForm_DXEFL_DXEditor2_EI" title="Mandatory" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-YRohc" alt="Required" /></div>
                                    </div>
                                </div>
                            </div>
                            <div class="ContentDiv">
                                <div class="ScrollDiv"></div>
                                <br style="clear: both;" />
                                <div class="Footer" style="padding-left: 84px;">
                                    <div style="float: left;">
                                        <dxe:ASPxButton ID="btnSave_Prob" ClientInstanceName="cbtnSave_States" runat="server"
                                            AutoPostBack="False" Text="Save" CssClass="btn btn-primary">
                                            <ClientSideEvents Click="function (s, e) {btnSave_Prob();}" />
                                        </dxe:ASPxButton>
                                    </div>
                                    <div style="">
                                        <dxe:ASPxButton ID="btnCancel_Prob" runat="server" AutoPostBack="False" Text="Cancel" CssClass="btn btn-danger">
                                            <ClientSideEvents Click="function (s, e) {fn_btnCancel();}" />
                                        </dxe:ASPxButton>
                                    </div>
                                    <br style="clear: both;" />
                                </div>
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
                <dxe:ASPxHiddenField runat="server" ClientInstanceName="chfID" ID="hfID">
                </dxe:ASPxHiddenField>
            </div>
        </div>
    </div>
  

</asp:Content>
