<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.Master.management_master_OutsourcingComp" CodeBehind="OutsourcingComp.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
    <script type="text/javascript">
        function ClickOnContactInfo(keyValue) {
            $('#dvConPersList').load('/Ajax/_PartialContactPersonListForShow', { agentInternalId: keyValue }, function () {
                $('#dvgrpConPersList').modal('show');
            });
            ////var url = 'insurance_contactPerson.aspx?id=' + keyValue;

            //OnMoreInfoClick(url, "Name : " + name + "", '940px', '450px', "Y");

            ////window.location.href = url;
        }
        function ClickOnMoreInfo(keyValue) {
            var url = 'OutsourcingComp_general.aspx?id=' + keyValue;
            //OnMoreInfoClick(url, "Modify Contact Details", '940px', '450px', "Y");
            window.location.href = url;
        }
        function OnAddButtonClick() {
            var url = 'OutsourcingComp_general.aspx?id=' + 'ADD';
            //OnMoreInfoClick(url, "Add Contact Details", '940px', '450px', "Y");
            window.location.href = url;
        }
        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }
        function callback() {
            grid.PerformCallback();
        }
        //function LastCall(obj) {
        //    height();
        //}
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Outsourcing Agents/Companies</h3>
        </div>
    </div>
    <div class="form_main">
        <table class="TableMain100" cellpadding="0px" cellspacing="0px">
            <tr>
                <td>
                    <table width="100%">
                        <tr>
                            <td style="text-align: left; vertical-align: top">
                                <table>
                                    <tr>
                                        <td id="ShowFilter">
                                            <%--<% if (rights.CanAdd)
                                               { %>--%>
                                            <a href="javascript:void(0);" onclick="OnAddButtonClick();" class="btn btn-primary"><span>Add New</span></a>
                                            <%--<%} %>--%>
                                            <%--<a href="javascript:ShowHideFilter('s');" class="btn btn-primary"><span>Show Filter</span></a>--%>
                                        </td>
                                        <td id="Td1">
                                            <%--<a href="javascript:ShowHideFilter('All');" class="btn btn-primary"><span>All Records</span></a>--%>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td></td>
                            <td class="gridcellright pull-right">
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
                                    <ButtonStyle>
                                    </ButtonStyle>
                                    <ItemStyle>
                                        <HoverStyle>
                                        </HoverStyle>
                                    </ItemStyle>
                                    <Border BorderColor="black" />
                                    <DropDownButton Text="Export">
                                    </DropDownButton>
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="gridcellcenter">
                    <dxe:ASPxGridView ID="EmployeeGrid" runat="server" KeyFieldName="cnt_Id" AutoGenerateColumns="False"
                        DataSourceID="EmployeeDataSource" Width="100%" ClientInstanceName="grid" OnCustomCallback="EmployeeGrid_CustomCallback"
                        OnCustomJSProperties="EmployeeGrid_CustomJSProperties">
                        <Columns>
                            <dxe:GridViewDataTextColumn FieldName="Name" ReadOnly="True" VisibleIndex="0" Width="40%">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="BranchName" VisibleIndex="1">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Phone" FieldName="phone" VisibleIndex="2">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False" />
                            </dxe:GridViewDataTextColumn>
                            <%--<dxe:GridViewDataTextColumn Caption="Details" VisibleIndex="3" Width="5%">
                                <DataItemTemplate>
                                    <a href="javascript:void(0);" onclick="ClickOnMoreInfo('<%# Container.KeyValue %>')">More Info...</a>
                                </DataItemTemplate>
                                <CellStyle HorizontalAlign="Left" Wrap="False">
                                </CellStyle>
                                <EditFormSettings Visible="False" />
                                <HeaderStyle HorizontalAlign="Center" />
                            </dxe:GridViewDataTextColumn>--%>
                            <dxe:GridViewDataTextColumn Caption="Cont.Person" VisibleIndex="4" Width="5%">
                                <DataItemTemplate>
                                    <%--     <a href="javascript:void(0);" onclick="ClickOnContactInfo('<%#Eval("Id") %>','<%#Eval("Name") %>')">Show</a>--%>
                                    <a href="javascript:void(0);" onclick="ClickOnContactInfo('<%#Eval("Id") %>')" title="Members">
                                        <img src="../../../assests/images/Members.png" />
                                    </a>
                                </DataItemTemplate>
                                <CellStyle HorizontalAlign="Left" Wrap="False">
                                </CellStyle>
                                <EditFormSettings Visible="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="6" Width="6%">
                                <DataItemTemplate>
                                    <a href="javascript:void(0);" onclick="ClickOnMoreInfo('<%# Container.KeyValue %>')" title="More Info" class="pad" style="text-decoration: none;">
                                        <img src="../../../assests/images/info.png" />
                                    </a>
                                    <a href="javascript:void(0);" onclick="ClickOnContactInfo('<%#Eval("Id") %>','<%#Eval("Name") %>')" title="Show" style="text-decoration: none;">
                                        <img src="/OMS/images/show.png" />
                                    </a>
                                </DataItemTemplate>
                                <CellStyle HorizontalAlign="Center" Wrap="False">
                                </CellStyle>
                                <HeaderTemplate>Actions</HeaderTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                                <EditFormSettings Visible="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Created User" FieldName="user_name" Visible="False"
                                VisibleIndex="7">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False" />
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                        <ClientSideEvents EndCallback="function(s, e) {
	                        LastCall(s.cpHeight);
                        }" />
                        <Styles>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                            </Header>
                        </Styles>
                        <SettingsSearchPanel Visible="True" />
                        <Settings ShowGroupPanel="True" ShowTitlePanel="True" ShowStatusBar="Visible" ShowFilterRow="true"  ShowFilterRowMenu ="true"/>
                        <SettingsText PopupEditFormCaption="Add/ Modify Employee" />
                        <SettingsEditing Mode="PopupEditForm" PopupEditFormHorizontalAlign="Center" PopupEditFormModal="True"
                            PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="900px" EditFormColumnCount="3" />
                        <SettingsBehavior AllowFocusedRow="false" ConfirmDelete="True" AutoFilterRowInputDelay="1200" ColumnResizeMode="NextColumn" FilterRowMode="OnClick" />
                        <SettingsPager NumericButtonCount="20" PageSize="20" ShowSeparators="True" AlwaysShowPager="True">
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </SettingsPager>
                        <SettingsSearchPanel Visible="True" />
                        <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />

                    </dxe:ASPxGridView>
                </td>
            </tr>
        </table>
        <asp:SqlDataSource ID="EmployeeDataSource" runat="server" 
            SelectCommand=""></asp:SqlDataSource>
        <dxe:ASPxGridViewExporter ID="exporter" runat="server">
        </dxe:ASPxGridViewExporter>
    </div>

    <div class="modal   fade" id="dvgrpConPersList">
        <div class="modal-dialog" style="width: 450px" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                    <h4 class="modal-title">Contact List</h4>
                </div>
                <div class="modal-body" id="dvConPersList"></div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-success btnwidth" data-dismiss="modal">Ok</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
