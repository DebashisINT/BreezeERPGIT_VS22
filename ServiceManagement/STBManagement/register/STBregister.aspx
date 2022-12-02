<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="STBregister.aspx.cs" Inherits="ServiceManagement.STBManagement.register.STBregister" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading clearfix">
        <div class="panel-title pull-left">
            <h3>
                Service Register
            </h3>
        </div>
    </div>
    <div class="form_main" >
        <div class="slick boxModel clearfix font-pp" id="locationMulti">
            <div class="row">
                <div class="col-md-2">
                    <label>Type <span style="color:red">*</span></label>
                    <div>
                        <select class="form-control">
                            <option>Select</option>
                        </select>
                    </div>
                </div>
                <div class="col-md-2">
                    <label>Report Type <span style="color:red">*</span></label>
                    <div>
                        <select class="form-control">
                            <option>Select</option>
                        </select>
                    </div>
                </div>
                <div class="col-md-2">
                    <label>From Date </label>
                    <div>
                        <input type="text" class="form-control" />
                    </div>
                </div>
                <div class="col-md-2">
                    <label>To Date </label>
                    <div>
                        <input type="text" class="form-control" />
                    </div>
                </div>
                <div class="col-md-2">
                    <label>Entity Code</label>
                    <div>
                        <select class="form-control">
                            <option>Select</option>
                        </select>
                    </div>
                </div>
                <div class="col-md-2">
                    <label>Location </label>
                    <div>
                        <select class="form-control">
                            <option>Select</option>
                        </select>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-2">
                    <label>Model</label>
                    <div>
                        <select class="form-control">
                            <option>Select</option>
                        </select>
                    </div>
                </div>
                <div class="col-md-2" style="padding-top: 25px;">
                    <div class="custom-control custom-checkbox">
                        <input type="checkbox" class="custom-control-input" id="customCheck1" />
                        <label class="custom-control-label" for="customCheck1">Details</label>
                    </div>
                </div>
            </div>
        </div>

        <div class="clearfix" style="margin-top:5px">
             <dxe:ASPxGridView ID="gridStatus" ClientInstanceName="grid" Width="100%"
                        KeyFieldName="branch_id" DataSourceID="" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" runat="server"
                        AutoGenerateColumns="False" OnCustomCallback="gridStatus_CustomCallback" SettingsBehavior-AllowFocusedRow="true" Settings-HorizontalScrollBarMode="Auto">
                        <SettingsSearchPanel Visible="True" Delay="5000" />
                        <ClientSideEvents EndCallback="function(s, e) {
	                                  EndCall(s.cpEND);
                                }" />
                        <Columns>
                            <dxe:GridViewDataTextColumn Visible="False" FieldName="branch_id" Caption="ID">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="0" FieldName="branch_code"
                                Caption="Document No">
                                <CellStyle CssClass="gridcellleft" Wrap="True">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="branch_description"
                                Caption="Date">
                                <CellStyle CssClass="gridcellleft" Wrap="True">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="ParentBranch"
                                Caption="Entry type">
                                <CellStyle CssClass="gridcellleft" Wrap="True">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="branch_cpPhone"
                                Caption="Location" Visible="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="Country"
                                Caption="Entity code" Visible="true">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="State"
                                Caption="Network name" Visible="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="a"
                                Caption="Contact name" Visible="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="b"
                                Caption="contact no" Visible="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="c"
                                Caption="Model" Visible="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="d"
                                Caption="Qty" Visible="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="e"
                                Caption="Price" Visible="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="f"
                                Caption="Req. Amount" Visible="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="g"
                                Caption="DAS" Visible="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="h"
                                Caption="Payment mode" Visible="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="i"
                                Caption="Amount" Visible="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="j"
                                Caption="Ref. No" Visible="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="k"
                                Caption="Cheque no" Visible="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="l"
                                Caption="Cheque Date" Visible="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="m"
                                Caption="Branch" Visible="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="n"
                                Caption="Remarks" Visible="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="0"
                                Caption="Status" Visible="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            
                            <dxe:GridViewDataTextColumn VisibleIndex="10" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" Width="0">
                                <%--<CellStyle CssClass="gridcellleft">
                                </CellStyle>--%>

                                <DataItemTemplate>
                                    <div class='floatedBtnArea'>
                                        
                                    
                                        <a href="javascript:void(0);" onclick="OnEditButtonClick('<%# Container.KeyValue %>')" title="" class="">
                                            <span class='ico ColorSix'><i class='fa fa-pencil'></i></span><span class='hidden-xs'>Edit</span>
                                        </a>
                                       
                                    </div>
                                </DataItemTemplate>
                                <HeaderTemplate></HeaderTemplate>
                                <%--<HeaderTemplate>--%>
                                <%-- <a href="javascript:void(0);" onclick="javascript:OnAddButtonClick();"><span style="color: #000099; text-decoration: underline">Add New</span> </a>--%>
                                <%--</HeaderTemplate>--%>
                                <EditFormSettings Visible="False"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                        </Columns>  
                        <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                        <SettingsText ConfirmDelete="Confirm delete?" />
                        <StylesEditors>
                            <ProgressBar Height="25px">
                            </ProgressBar>
                        </StylesEditors>
                        <SettingsSearchPanel Visible="True" />
                        <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowFilterRow="true" ShowFilterRowMenu="True" />
                        <%--<Styles>
                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                            </Header>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                        </Styles>--%>
                        <%--<SettingsPager NumericButtonCount="20" PageSize="20" ShowSeparators="True" AlwaysShowPager="True">
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </SettingsPager>--%>
                        <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True" />
                    </dxe:ASPxGridView>
        </div>
    </div>
</asp:Content>
