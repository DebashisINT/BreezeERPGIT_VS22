<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.Master.management_Master_Lead" CodeBehind="Lead.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        
    <script type="text/javascript">

        function ClickOnMoreInfo(keyValue) {

            var url = 'Lead_general.aspx?id=' + keyValue;
            //OnMoreInfoClick(url, "Modify Lead Details", '940px', '450px', "Y");
            window.location.href = url;
        }
        function OnCreateActivityClick(KeyVal) {
            //alert(KeyVal);
            var url = 'Lead_Activity.aspx?id=' + KeyVal;
            //OnMoreInfoClick(url, "Lead Activity Creation", '940px', '450px', "Y");
            window.location.href = url;
        }
        function OnAddButtonClick() {
            var url = 'Lead_general.aspx?id=' + 'ADD';
            //OnMoreInfoClick(url, "Add New Lead Details", '940px', '450px', "Y");
            window.location.href = url;
        }
        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }
        function callback() {
            grid.PerformCallback();
        }
        function callheight(obj) {
            // parent.CallMessage();
        }
        function OnContactInfoClick(keyValue, CompName) {
            var url = 'insurance_contactPerson.aspx?id=' + keyValue;
            //OnMoreInfoClick(url, "Lead Name : " + CompName + "", '940px', '450px', "Y");
            window.location.href = url;
        }
        function OnHistoryInfoClick(keyValue, CompName) {
            var url = 'ShowHistory_Phonecall.aspx?id1=' + keyValue;
            //OnMoreInfoClick(url, "Lead  History", '940px', '450px', "Y");
            window.location.href = url;
        }
        //-->
    </script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="panel-heading">
        <div class="panel-title">
            <h3>All Lead Details</h3>
        </div>

    </div>
    <div class="form_main">
        <table class="TableMain100">
            <tr>
                <td>
                    <table class="TableMain100">
                        <tr>
                            <td style="text-align: left; vertical-align: top">
                                <table>
                                    <tr>
                                        <td id="ShowFilter">
                                            <%--<a href="javascript:ShowHideFilter('s');" class="btn btn-success"><span >Show Filter</span></a>--%>
                                            <a href="javascript:void(0);" class="btn btn-primary" onclick="OnAddButtonClick();"><span>Add New</span> </a>
                                        </td>
                                        <td id="Td1">
                                            <a href="javascript:ShowHideFilter('All');" class="btn btn-primary"><span>All Records</span></a>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="gridcellright pull-right">
                                <dxe:ASPxComboBox ID="cmbExport" runat="server" AutoPostBack="true"
                                    Font-Bold="False" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged"
                                    ValueType="System.Int32" Width="130px">
                                    <Items>
                                        <dxe:ListEditItem Text="Select" Value="0" />
                                        <dxe:ListEditItem Text="PDF" Value="1" />
                                        <dxe:ListEditItem Text="XLS" Value="2" />
                                        <dxe:ListEditItem Text="RTF" Value="3" />
                                        <dxe:ListEditItem Text="CSV" Value="4" />
                                    </Items>
                                    <Border BorderColor="Black" />
                                    <DropDownButton Text="Export">
                                    </DropDownButton>
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                    </table>
                </td>
                <td></td>
            </tr>
            <tr>
                <td>
                    <dxe:ASPxGridView ID="LeadGrid" runat="server" AutoGenerateColumns="False" KeyFieldName="cnt_id"
                        Width="100%" ClientInstanceName="grid" OnCustomCallback="LeadGrid_CustomCallback1"
                        OnCustomJSProperties="LeadGrid_CustomJSProperties">
                        <SettingsSearchPanel Visible="True" />
                        <Settings ShowGroupPanel="True" ShowFilterRow="true" ShowFilterRowMenu ="true" />
                        <Columns>
                            <dxe:GridViewDataTextColumn FieldName="Name" ReadOnly="True" VisibleIndex="0">
                                <CellStyle HorizontalAlign="Left">
                                </CellStyle>
                                <HeaderStyle HorizontalAlign="Center" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="BranchName" VisibleIndex="1">
                                <CellStyle HorizontalAlign="Left">
                                </CellStyle>
                                <HeaderStyle HorizontalAlign="Center" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="phone" Caption="Phone" ReadOnly="True" VisibleIndex="2">
                                <CellStyle HorizontalAlign="Left">
                                </CellStyle>
                                <HeaderStyle HorizontalAlign="Center" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Status" ReadOnly="True" VisibleIndex="3">
                                <CellStyle HorizontalAlign="Left">
                                </CellStyle>
                                <HeaderStyle HorizontalAlign="Center" />
                            </dxe:GridViewDataTextColumn>
                            <%--<dxe:GridViewDataTextColumn Caption="Create Activity" FieldName="Id" ReadOnly="True"
                                VisibleIndex="4">
                                <DataItemTemplate>
                                </DataItemTemplate>
                                <CellStyle HorizontalAlign="Left">
                                </CellStyle>
                                <HeaderStyle HorizontalAlign="Center" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="ADD/Modify "  ReadOnly="True"
                                VisibleIndex="5">
                                <DataItemTemplate>
                                </DataItemTemplate>
                                <EditFormSettings Visible="False" />
                                <CellStyle HorizontalAlign="Left">
                                </CellStyle>
                                <HeaderStyle HorizontalAlign="Center" />
                                <HeaderTemplate>
                                </HeaderTemplate>
                            </dxe:GridViewDataTextColumn>--%>
                            <dxe:GridViewDataTextColumn  VisibleIndex="6" Width="11%" >

                                <DataItemTemplate>
                                    <a href="javascript:void(0);" onclick="OnCreateActivityClick('<%# Eval("Id") %>')" title="Create Activity" class="pad" style="text-decoration:none;">

                                        <img src="/OMS/images/activity.png" />
                                    </a>
                                    <a href="javascript:void(0);" onclick="ClickOnMoreInfo('<%# Container.KeyValue %>')" title="More Info" class="pad" style="text-decoration:none;">
                                        <img src="../../../assests/images/info.png" />
                                    </a>
                                    <a href="javascript:void(0);" onclick="OnContactInfoClick('<%#Eval("Id") %>','<%#Eval("Name") %>')" title="Show" class="pad" style="text-decoration:none;">
                                        <img src="/OMS/images/show.png" />
                                    </a>
                                    <a href="javascript:void(0);" onclick="OnHistoryInfoClick('<%#Eval("Id") %>','<%#Eval("Name") %>')" title="History" style="text-decoration:none;">
                                        <img src="/OMS/images/history.png" />
                                    </a>
                                </DataItemTemplate>
                                  <HeaderTemplate>Actions</HeaderTemplate>
                                <CellStyle HorizontalAlign="Center" Wrap="False">
                                </CellStyle>
                                <HeaderStyle HorizontalAlign="Center" />
                                <EditFormSettings Visible="False" />
                            </dxe:GridViewDataTextColumn>
                            <%--  <dxe:GridViewDataTextColumn VisibleIndex="7">
                                <DataItemTemplate>

                                    <cellstyle horizontalalign="Left" wrap="False">
                                    </cellstyle>
                                    <editformsettings visible="False" />
                                </DataItemTemplate>
                            </dxe:GridViewDataTextColumn>--%>
                        </Columns>
                        <SettingsBehavior AllowFocusedRow="false" AllowSelectByRowClick="true" ColumnResizeMode="NextColumn" />
                        <Styles>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                            </Header>
                        </Styles>
                        <SettingsPager NumericButtonCount="20" PageSize="20" ShowSeparators="True" AlwaysShowPager="True">
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </SettingsPager>
                        <ClientSideEvents EndCallback="function(s, e) {
	callheight(s.cpHeight);
}" />
                    </dxe:ASPxGridView>
                </td>
            </tr>
        </table>
        <asp:SqlDataSource ID="LeadGridDataSource" runat="server" 
            SelectCommand="select ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name, tbl_master_branch.branch_description AS BranchName, (select top 1 isnull(case mp.phf_phonenumber when null then '' else (Select top 1  '(M)'+ m.phf_phonenumber from tbl_master_phonefax m where m.phf_cntid=cnt_internalId and m.phf_type = 'Mobile') end,'')  + isnull(case mp.phf_phonenumber  when null then '' else (Select top 1  '(R)'+ r.phf_phonenumber from tbl_master_phonefax r where r.phf_cntid=cnt_internalId and r.phf_type = 'Residence') end,'')  + isnull(case mp.phf_phonenumber  when null then '' else (Select top 1  '(R)'+ o.phf_phonenumber from tbl_master_phonefax o where o.phf_cntid=cnt_internalId and o.phf_type = 'Office') end ,'') from tbl_master_phonefax mp where mp.phf_cntid=cnt_internalId)as phone, case tbl_master_lead.cnt_Lead_Stage when 1 then 'Due' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status, tbl_master_lead.cnt_internalId AS Id, tbl_master_lead.createdate, tbl_master_lead.cnt_InternalId as cnt_UCC,tbl_master_lead.createUser,tbl_master_lead.cnt_id from tbl_master_lead, tbl_master_branch Where tbl_master_lead.CreateUser in (@userlist) and tbl_master_lead.cnt_branchid = tbl_master_branch.branch_id order by tbl_master_lead.CreateDate desc">
            <SelectParameters>
                <asp:SessionParameter Name="userlist" SessionField="userchildHierarchy" Type="string" />
            </SelectParameters>
        </asp:SqlDataSource>
        <dxe:ASPxGridViewExporter ID="exporter" runat="server">
        </dxe:ASPxGridViewExporter>
    </div>
</asp:Content>
