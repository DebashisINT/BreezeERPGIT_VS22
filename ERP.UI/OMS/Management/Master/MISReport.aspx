<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="MISReport.aspx.cs" Inherits="ERP.OMS.Management.Master.MISReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .boxedView {
            border: 1px solid #d8d8d8;
            padding: 7px 15px 15px 15px;
            background: #ffffff;
            background: -moz-linear-gradient(top, #ffffff 0%, #f2f2f2 100%);
            background: -webkit-linear-gradient(top, #ffffff 0%,#f2f2f2 100%);
            background: linear-gradient(to bottom, #ffffff 0%,#f2f2f2 100%);
            filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#ffffff', endColorstr='#f2f2f2',GradientType=0 );
            margin-bottom:10px
        }

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading clearfix">
        <div class="panel-title pull-left">
            <h3>MIS Report</h3>
        </div>
        <%--<div id="btncross" class="crossBtn" style="display: block; margin-left: 50px;" onclick="back()"><a href="javascript:ReloadPage()"><i class="fa fa-times"></i></a></div>--%>
    </div>
    <div class="form_main">
        <div>
            <div class="boxedView">
               <div class="row">
                   <div class="col-md-2">
                       <label>From Month <span style="color:red"> *</span></label>
                       <div>
                           <dxe:ASPxDateEdit ID="ASPxDateEdit1" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDate" Width="100%">
                            <ButtonStyle Width="13px">
                            </ButtonStyle>
                            </dxe:ASPxDateEdit>
                       </div>
                   </div>
                   <div class="col-md-2">
                       <label>To Month</label>
                       <div>
                           <dxe:ASPxDateEdit ID="toDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDate" Width="100%">
                            <ButtonStyle Width="13px">
                            </ButtonStyle>
                            </dxe:ASPxDateEdit>
                       </div>
                   </div>
                   <div class="col-md-2">
                       <label>Branch <span style="color:red"> *</span></label>
                       <div>
                          <dxe:ASPxComboBox ID="cmbBranchfilter" runat="server" ClientInstanceName="ccmbBranchfilter" Width="100%">
                            </dxe:ASPxComboBox>
                       </div>
                   </div>
                   <div class="col-md-2">
                       <label>Comparison </label>
                       <div>
                          <select class="form-control">
                              <option>Select</option>
                          </select>
                       </div>
                   </div>
                   <div class="col-md-2">
                       <label>&nbsp </label>
                       <div>
                          <button class="btn btn-success">Submit</button>
                       </div>
                   </div>
               </div>
            </div>
            <div class="mTop5">
                <dxe:ASPxGridView ID="EmployeeGrid" OnDataBinding="EmployeeGrid_DataBinding" runat="server"  KeyFieldName="id" AutoGenerateColumns="False"
                        Width="100%" ClientInstanceName="grid"
                        SettingsBehavior-AllowFocusedRow="true" Settings-HorizontalScrollBarMode="Visible" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false">
                        <SettingsSearchPanel Visible="True" Delay="5000" />
                        <Settings ShowFilterRow="true" ShowGroupPanel="true" ShowFilterRowMenu="true" />
                        <SettingsBehavior ConfirmDelete="True" ColumnResizeMode="NextColumn" FilterRowMode="Auto" />

                        <Columns>
                            <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="2" FieldName="group_name" Caption="Setting Name" Width="20%">
                                <CellStyle CssClass="gridcellleft" Wrap="True">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="cr_by" Caption="Created By" Width="15%">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataDateColumn VisibleIndex="3" PropertiesDateEdit-DisplayFormatString="dd-MM-yyyy" PropertiesDateEdit-EditFormatString="dd-MM-yyyy" FieldName="created_on" Caption="Created On" Width="15%" >
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                            </dxe:GridViewDataDateColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="mod_by" Caption="Modified By" Width="15%">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataDateColumn VisibleIndex="5" FieldName="mod_on" PropertiesDateEdit-DisplayFormatString="dd-MM-yyyy" PropertiesDateEdit-EditFormatString="dd-MM-yyyy" Caption="Modified On" Width="15%">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                            </dxe:GridViewDataDateColumn>
                            <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="6" CellStyle-HorizontalAlign="Center" Width="21%">
                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <HeaderTemplate>Actions</HeaderTemplate>
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <DataItemTemplate>
                                    <a href="javascript:void(0);" onclick="ClickOnEdit('<%# Eval("id") %>')" title="Edit" class="pad" style="text-decoration: none;">
                                        <img src="../../../assests/images/info.png" />
                                    </a>
                                </DataItemTemplate>
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                        <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                        <SettingsPager PageSize="10">
                            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                        </SettingsPager>
                    </dxe:ASPxGridView>
            </div>
        </div>
    </div>
</asp:Content>
