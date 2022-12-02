<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="RptProductMasterReport.aspx.cs" Inherits="ERP.OMS.Reports.Master.RptProductMasterReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function ChangeState(value) {
            cProductCallbackPanel.PerformCallback(value);
        }
        function CloseGridLookup() {
            gridLookup.ConfirmCurrentSelection();
            gridLookup.HideDropDown();
            gridLookup.Focus();
        }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Product Catalogue Report</h3>
        </div>
    </div>
    <div class="form_main">
        <div class="Main">

            <%--For gridview selection --%>
            <div class="GridViewArea" style="display: none">
                <dxe:ASPxGridView ID="ProductGrid" runat="server" AutoGenerateColumns="False" ClientInstanceName="cProductGrid" DataSourceID="ProductDataSource" KeyFieldName="sProducts_ID"
                    Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" OnCustomCallback="ProductGrid_CustomCallback" SettingsPager-Mode="ShowAllRecords"
                    Settings-ShowFooter="false" Settings-VerticalScrollBarMode="Auto" Settings-VerticalScrollableHeight="400">
                    <SettingsSearchPanel Visible="false" />
                    <settings showgrouppanel="True" showstatusbar="Hidden" showfilterrow="true" showfilterrowmenu="True" />
                    <columns>

                    <dxe:GridViewCommandColumn ShowSelectCheckbox="true" Width="50" Caption="Select" />


                    <dxe:GridViewDataTextColumn Caption="Short Name (Unique)" FieldName="sProducts_Code" ReadOnly="True"
                        Visible="True" FixedStyle="Left"  >
                        <EditFormSettings Visible="True" />
                    </dxe:GridViewDataTextColumn>
                     
                    <dxe:GridViewDataTextColumn Caption="Product Name" FieldName="sProducts_Name" ReadOnly="True"
                        Visible="True" FixedStyle="Left"  >
                        <EditFormSettings Visible="True" />
                    </dxe:GridViewDataTextColumn>
                     

                    <dxe:GridViewDataTextColumn Caption="Description" FieldName="sProducts_Description" ReadOnly="True"
                        Visible="True" FixedStyle="Left"  >
                          <CellStyle Wrap="True" HorizontalAlign="Left" CssClass="gridcellleft"></CellStyle> 
                        <EditFormSettings Visible="True" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="Product Type" FieldName="sProducts_TypeFull" ReadOnly="True"
                        Visible="True" FixedStyle="Left"  >
                        <EditFormSettings Visible="True" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="Product Class" FieldName="ProductClass_Name" ReadOnly="True"
                        Visible="True" FixedStyle="Left" >
                        <EditFormSettings Visible="True" />
                    </dxe:GridViewDataTextColumn>

                </columns>
                    <settingsbehavior columnresizemode="NextColumn" />
                </dxe:ASPxGridView>
            </div>
            <%--For gridview selection --%>

            <%--For GridLookUp selection --%>
            <div class="col-md-3 pdLeft0">
                <dxe:ASPxCallbackPanel runat="server" id="ProductCallbackPanel" ClientInstanceName="cProductCallbackPanel" OnCallback="ProductCallbackPanel_Callback">
                    <panelcollection>
                                                       <dxe:PanelContent runat="server">

                                                      <dxe:ASPxGridLookup ID="GridLookup" runat="server" SelectionMode="Multiple" DataSourceID="ProductDataSource" ClientInstanceName="gridLookup" Height="30"
                                                                                                                KeyFieldName="sProducts_ID" Width="100%" TextFormatString="{0}" MultiTextSeparator=", " >
                                                                                                                <Columns>
                                                                                                                    <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="60" Caption=" "/>
                                                                                                                    <dxe:GridViewDataColumn FieldName="sProducts_Code" Caption="Product Code" Width="150"/>
                                                                                                                    <dxe:GridViewDataColumn FieldName="sProducts_Name" Caption="Product Name" Width="300"/>
                                                                                                                    <dxe:GridViewDataColumn FieldName="ProductClass_Name" Caption="Product Class" Width="300"/>
                                                                                                                </Columns>
                                                                                                                <GridViewProperties  Settings-VerticalScrollBarMode="Auto"   >
                                                                             
                                                                                                                    <Templates>
                                                                                                                        <StatusBar>
                                                                                                                            <table class="OptionsTable" style="float: right">
                                                                                                                                <tr>
                                                                                                                                    <td>
                                                                                                                                       
                                                                                                                                  <%--      <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridLookup" />--%>
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                            </table>
                                                                                                                        </StatusBar>
                                                                                                                    </Templates>
                                                                                                                    <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true"/>
                                                                                                                </GridViewProperties>
                                                                                                            </dxe:ASPxGridLookup>
                                                             </dxe:PanelContent>
                                                </panelcollection>
                </dxe:ASPxCallbackPanel>
                </div>

                <%--For GridLookUp selection --%>




                <div class="col-md-5 pdLeft0">
                    <input type="button" value="Select All Products" onclick="ChangeState('SelectAll')" class="btn btn-primary"></input>
                    <input type="button" value="De-select All Products" onclick="ChangeState('UnSelectAll')" class="btn btn-primary"></input>
                    <input type="button" value="Revert" onclick="ChangeState('Revart')" class="btn btn-primary"></input>

                    <asp:Button runat="server" Text="Preview" OnClick="Unnamed_Click" CssClass="btn btn-success" />
                </div>
            
        </div>
        <asp:SqlDataSource ID="ProductDataSource" runat="server"
            SelectCommand=" select sProducts_ID,sProducts_Code,sProducts_Name,sProducts_Description,CASE WHEN sProducts_Type ='A' THEN 'Raw Material'  
     WHEN sProducts_Type ='B' THEN 'Work-In-Process'    
     WHEN  sProducts_Type ='C' THEN 'Finished Goods' END AS sProducts_TypeFull,isnull((select ProductClass_Name from Master_ProductClass where ProductClass_ID=h.ProductClass_Code ),'')ProductClass_Name    from Master_sProducts h"></asp:SqlDataSource>

    </div>

</asp:Content>
