<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="Servicetax.aspx.cs" Inherits="ERP.OMS.Management.SettingsOptions.Servicetax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function SignOff() {
            window.parent.SignOff();
        }
        function height() {
            if (document.body.scrollHeight >= 500)
                window.frameElement.height = document.body.scrollHeight;
            else
                window.frameElement.height = '500px';


            window.frameElement.width = document.body.scrollWidth;
        }
        function OnMoreInfoClick(keyValue) {
            debugger;
            var url = 'tax.aspx?id=' + keyValue;
            window.location.href = url;
        
            //parent.OnMoreInfoClick(url, "Add Tax Details", '940px', '450px', "Y");

        }
        
        function DeleteRow(keyValue) {
            debugger;
            jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    grid.PerformCallback('Delete~' + keyValue);
                }
            });
        }

    
            //function OnAddButtonClick() {
            //    debugger;
            //   var e = parent.document.getElementById('cmbSegment').value;
            //   var url = 'tax.aspx?id=' + 'ADD';
            //   parent.OnMoreInfoClick(url, "Add Tax Details", '940px', '450px', "Y");

            //}

            //Subhabrata:Done
            function OnAddButtonClick() {
                debugger;
                var url = 'tax.aspx?id=ADD';
                //OnMoreInfoClick(url, "Add New Accout", '920px', '500px', "Y");
                window.location.href = url;
            }
            //End
    
            function PopulateGrid() {
                grid.PerformCallback();
            }
            function callback() {
                grid.PerformCallback();

            }
            function EndCall(obj) {
                height();
            }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Service Tax</h3>
        </div>
    </div>
    <div class="form_main">

        <table class="TableMain100">

            <tr>
                <td>
                    <%if (Session["PageAccess"].ToString().Trim() == "All" || Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "DelAdd")
                      { %>
                    <a href="javascript:void(0);" onclick="OnAddButtonClick( )" class="btn btn-primary"><span>Add New</span> </a>
                    <%} %>
                    <% if (rights.CanExport)
                                               { %>
                    <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Value="0">Export to</asp:ListItem>
                        <asp:ListItem Value="1">PDF</asp:ListItem>
                        <asp:ListItem Value="2">XLS</asp:ListItem>
                        <asp:ListItem Value="3">RTF</asp:ListItem>
                        <asp:ListItem Value="4">CSV</asp:ListItem>
                    </asp:DropDownList>
                     <% } %>
                    <%--<table>
                        <tr>
                            <td>
                                <dxe:ASPxComboBox ID="cmbExport" runat="server" AutoPostBack="true" BackColor="Navy"
                                    Font-Bold="False" ForeColor="White" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged"
                                    ValueType="System.Int32" Width="130px">
                                    <Items>
                                        <dxe:ListEditItem Text="Select" Value="0" />
                                        <dxe:ListEditItem Text="PDF" Value="1" />
                                        <dxe:ListEditItem Text="XLS" Value="2" />
                                        <dxe:ListEditItem Text="RTF" Value="3" />
                                        <dxe:ListEditItem Text="CSV" Value="4" />
                                    </Items>
                                    <ButtonStyle BackColor="#C0C0FF" ForeColor="Black">
                                    </ButtonStyle>
                                    <ItemStyle BackColor="Navy" ForeColor="White">
                                        <HoverStyle BackColor="#8080FF" ForeColor="White">
                                        </HoverStyle>
                                    </ItemStyle>
                                    <Border BorderColor="White" />
                                    <DropDownButton Text="Export">
                                    </DropDownButton>
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                    </table>--%>
                </td>
            </tr>
        </table>
        <dxe:ASPxGridView ID="ServiceTaxGrid" runat="server" KeyFieldName="ServTax_ID"
            AutoGenerateColumns="False" DataSourceID="ServiceTaxDataSource" Width="100%"
            ClientInstanceName="grid" OnCustomCallback="ServiceTaxGrid_CustomCallback" OnCustomJSProperties="ServiceTaxGrid_CustomJSProperties" OnCommandButtonInitialize="ServiceTaxGrid_CommandButtonInitialize">
            <columns>
                <dxe:GridViewDataTextColumn FieldName="ServTax_ID" Caption="ID" VisibleIndex="0"
                    Visible="False">
                    <CellStyle CssClass="gridcellleft">
                    </CellStyle>
                    <EditFormCaptionStyle HorizontalAlign="Right">
                    </EditFormCaptionStyle>
                    <EditFormSettings Visible="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn FieldName="cmp_Name" Caption="Company Name" VisibleIndex="0"
                    Width="25%">
                    <CellStyle CssClass="gridcellleft">
                    </CellStyle>
                    <EditFormCaptionStyle HorizontalAlign="Right">
                    </EditFormCaptionStyle>
                    <EditFormSettings Visible="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Segment" FieldName="ddd" VisibleIndex="1"
                    Width="8%">
                    <EditFormSettings Visible="False" />
                    <CellStyle CssClass="gridcellleft">
                    </CellStyle>
                    <EditFormCaptionStyle HorizontalAlign="Right">
                    </EditFormCaptionStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Group" FieldName="r" VisibleIndex="2" Width="22%">
                    <EditFormSettings Visible="False" />
                    <CellStyle CssClass="gridcellleft">
                    </CellStyle>
                    <EditFormCaptionStyle HorizontalAlign="Right">
                    </EditFormCaptionStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn FieldName="ServTax_Rate" Caption="Tax " VisibleIndex="3"
                    Width="8%">
                    <CellStyle CssClass="gridcellleft">
                    </CellStyle>
                    <EditFormCaptionStyle HorizontalAlign="Right">
                    </EditFormCaptionStyle>
                    <EditFormSettings Visible="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Edu.Cess" FieldName="ServTax_EduCess" VisibleIndex="4"
                    Width="6%">
                    <CellStyle CssClass="gridcellleft">
                    </CellStyle>
                    <EditFormCaptionStyle HorizontalAlign="Right">
                    </EditFormCaptionStyle>
                    <EditFormSettings Visible="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Hgr.Edu.Cess" FieldName="ServTax_HgrEduCess"
                    VisibleIndex="5" Width="6%">
                    <CellStyle CssClass="gridcellleft">
                    </CellStyle>
                    <EditFormCaptionStyle HorizontalAlign="Right">
                    </EditFormCaptionStyle>
                    <EditFormSettings Visible="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="From Date" FieldName="ServTax_DateFrom" VisibleIndex="6"
                    Width="12%">
                    <CellStyle CssClass="gridcellleft">
                    </CellStyle>
                    <EditFormCaptionStyle HorizontalAlign="Right">
                    </EditFormCaptionStyle>
                    <EditFormSettings Visible="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Untill Date" FieldName="ServTax_DateTo" VisibleIndex="7"
                    Width="12%">
                    <CellStyle CssClass="gridcellleft">
                    </CellStyle>
                    <EditFormCaptionStyle HorizontalAlign="Right">
                    </EditFormCaptionStyle>
                    <EditFormSettings Visible="False" />
                </dxe:GridViewDataTextColumn>
                
                <dxe:GridViewDataTextColumn Caption="Details" CellStyle-HorizontalAlign="Center" VisibleIndex="8" Width="10%">
                    <DataItemTemplate>
                         <% if (rights.CanEdit)
                            { %>
                        <a href="javascript:void(0);" onclick="OnMoreInfoClick('<%# Container.KeyValue %>')" class="pad">
                            <img src="../../../assests/images/info.png" /></a>
                        
                        <%} %>
                         <% if (rights.CanDelete)
                            { %>
                                     <a href="javascript:void(0);" onclick="DeleteRow('<%# Container.KeyValue %>')" title="Delete" class="pad">
                                        <img src="../../../assests/images/Delete.png" /></a>
                                     <% } %>
                    </DataItemTemplate>
                    <EditFormSettings Visible="False" />
                    <CellStyle Wrap="False">
                    </CellStyle>
                    <HeaderStyle HorizontalAlign="Center" />
                    <HeaderTemplate>
                        Actions
                    </HeaderTemplate>
                    <EditFormSettings Visible="False"></EditFormSettings>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewCommandColumn VisibleIndex="9" Caption="Delete">
                    <%--<DeleteButton Visible="True">
                    </DeleteButton>--%>


                </dxe:GridViewCommandColumn>
            </columns>
            <settingsbehavior columnresizemode="NextColumn" confirmdelete="True" />
            <settingssearchpanel visible="True" />
            <settings showgrouppanel="True" showstatusbar="Hidden" showfilterrow="true" showfilterrowmenu="True" />
            <styles>
                <LoadingPanel ImageSpacing="10px">
                </LoadingPanel>
                <Header ImageSpacing="5px" SortingImageSpacing="5px" CssClass="gridheader">
                </Header>
                <FocusedRow CssClass="gridselectrow" HorizontalAlign="Left" VerticalAlign="Top">
                </FocusedRow>
                <Footer CssClass="gridfooter">
                </Footer>
                <FocusedGroupRow CssClass="gridselectrow">
                </FocusedGroupRow>
            </styles>
            <settings showgroupbuttons="False" showstatusbar="Visible" showgrouppanel="True" />
            <templates>
                <TitlePanel>
                    <table style="width: 100%">
                        <tr>
                            <td align="right">
                                <table width="200px">
                                    <tr>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </TitlePanel>
                <EditForm>
                </EditForm>
            </templates>
            <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True" />
            <settingstext popupeditformcaption="Add/ Modify Employee" grouppanel="Service Tax" />
            <settingsediting mode="PopupEditForm" popupeditformhorizontalalign="Center" popupeditformmodal="True"
                popupeditformverticalalign="WindowCenter" popupeditformwidth="900px" editformcolumncount="3" />
            <settingsbehavior allowfocusedrow="True" confirmdelete="True" />
            <settingspager showseparators="True" alwaysshowpager="True" numericbuttoncount="20"
                pagesize="20">
                <FirstPageButton Visible="True">
                </FirstPageButton>
                <LastPageButton Visible="True">
                </LastPageButton>
            </settingspager>
        </dxe:ASPxGridView>
        <asp:SqlDataSource ID="ServiceTaxDataSource" runat="server"
            DeleteCommand="servicetax_delete" DeleteCommandType="StoredProcedure">
            <DeleteParameters>
                <asp:Parameter Name="ServTax_ID" Type="String" />
            </DeleteParameters>
        </asp:SqlDataSource>
        <br />
        <dxe:ASPxGridViewExporter ID="exporter" runat="server">
        </dxe:ASPxGridViewExporter>
    </div>
</asp:Content>
