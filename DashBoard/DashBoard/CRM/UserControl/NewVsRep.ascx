<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewVsRep.ascx.cs" Inherits="DashBoard.DashBoard.CRM.UserControl.NewVsRep" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<script>
    function NvRCountGenerate(e) {
        cgridNvR.Refresh();
        e.preventDefault();
    }
    </script>


<div>
    <aside class="colWraper">
        <div class="diverh">
            <table>
                <tr>
                    <td class="besidepart" style="width:200px"><i class="fa fa-line-chart" style="font-size: 25px; color: #f7ffb7;"></i><span class="trSpan">(New Sale Vs Repeat Sale)</span></td>
                    <td class="pad5">


                        <dx:ASPxDateEdit ID="FromDateNvR" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" AllowNull="false"
                            ClientInstanceName="cFormDateNvR" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True" data-toggle="tooltip" title="As on Date"
                            Theme="PlasticBlue">
                            <ButtonStyle Width="13px">
                            </ButtonStyle>
                        </dx:ASPxDateEdit>
                    </td>
                     
                    <td class="pad5">
                        <asp:DropDownList ID="ddProdOrClass" runat="server" Width="100%" Style="margin-top: 5px;" data-toggle="tooltip" title="Productwise or Classwise">
                            <asp:ListItem Text="Product wise" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Class wise" Value="2"></asp:ListItem> 
                        </asp:DropDownList>
                    </td>



                    <td class="pad5">
                        <%--<input type="button" value="Show" class="btn btn-success" onclick="CallCountGenerate()" />--%>

                        <a href="#" data-toggle="tooltip" title="Generate" class="white">
                            <i class="fa fa-play-circle" onclick="NvRCountGenerate(event)"></i>
                        </a>
                    </td>
                    <td class="pad5">
                        <asp:LinkButton ID="LinkButton1NvR" class="white" runat="server" OnClick="LinkButton1_Click" data-toggle="tooltip" title="Export to Excel">
                         <%--   <i class="fa fa-file"></i>--%>
                             <img src="../../Dashboard/images/excel.png" class="excelIco" />
                        </asp:LinkButton>

                    </td>

                    <td class="pad5">
                        <span data-toggle="tooltip" title="Help">
                            <span data-toggle="popover" data-placement="left" data-html="true"  data-content="This report shows the Count of Sale Order(s) taken for the first time as 'New' orders for each unique Customers for different products. Any order for the same Customer and Product to be considered in 'Repeat'"><i class="fa fa-question-circle"></i></span>
                        </span>
                    </td>

                </tr>
            </table>


        </div>






        <dx:ASPxGridViewExporter ID="exporterNvR" runat="server" Landscape="false" PaperKind="A4" GridViewID="gridNvR"
            PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true" FileName="Call Count">
        </dx:ASPxGridViewExporter>

        <dx:ASPxGridView ID="gridNvR" runat="server" ClientInstanceName="cgridNvR" KeyFieldName="CreatedBy"
            Width="100%" Settings-HorizontalScrollBarMode="Auto"
            SettingsBehavior-ColumnResizeMode="Control" OnDataBinding="gridNvR_DataBinding"
            Settings-VerticalScrollableHeight="237" SettingsBehavior-AllowSelectByRowClick="true"
            Settings-VerticalScrollBarMode="Auto" Theme="PlasticBlue"
            Settings-ShowFilterRow="true" Settings-ShowFilterRowMenu="true">
               <SettingsDataSecurity AllowDelete="false" AllowEdit="false" AllowInsert="false" />
            <SettingsBehavior EnableCustomizationWindow="true"   />
                            <Settings ShowFooter="true"  /> 
                            <SettingsContextMenu Enabled="true" />    
            <Columns>
                 
                <dx:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="New" FieldName="New" Width="20%"
                    HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center">
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Repeat" FieldName="Regular" Width="20%"
                    HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center">
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dx:GridViewDataTextColumn>
                 
                <dx:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Salesman" FieldName="SalesManName" Width="60%">
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dx:GridViewDataTextColumn>

            </Columns>

            <SettingsPager PageSize="10" NumericButtonCount="4">
                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
            </SettingsPager>
        </dx:ASPxGridView>
    </aside>
     
</div>
