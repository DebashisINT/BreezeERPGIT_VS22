<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SalesVisitCount.ascx.cs" Inherits="DashBoard.DashBoard.CRM.UserControl.SalesVisitCount" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<script src="../CRM/Js/SalesVisit.js"></script>





<div>
    <aside class="colWraper">
        <div class="diverh">
            <table>
                <tr>
                    <td class="firstCell" style="width: 40px;"><i class="fa fa-motorcycle" style="font-size: 25px;color: #f7ffb7;"></i><span class="trSpan">(Visit)</span></td>
                    <td class="pad5">


                        <dx:ASPxDateEdit ID="FromDateSV" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" AllowNull="false"
                            ClientInstanceName="cFormDateSV" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True" data-toggle="tooltip" title="From Date"
                            Theme="PlasticBlue">
                            <ButtonStyle Width="13px">
                            </ButtonStyle>
                        </dx:ASPxDateEdit>
                    </td>


                    <td class="pad5">
                        <dx:ASPxDateEdit ID="TodateSV" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" AllowNull="false"
                            ClientInstanceName="cTodateSV" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True" data-toggle="tooltip" title="To Date"
                            Theme="PlasticBlue">
                            <ButtonStyle Width="13px">
                            </ButtonStyle>
                        </dx:ASPxDateEdit>
                    </td>

                    <td class="pad5">
                        <asp:DropDownList ID="dpActivitylistsv" runat="server" Width="100%" Style="margin-top: 5px;" data-toggle="tooltip" title="Activity Status">
                            <asp:ListItem Text="Future Sale" Value="3"></asp:ListItem>
                            <asp:ListItem Text="Clarification Required" Value="5"></asp:ListItem>
                            <asp:ListItem Text="Document Collection" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Closed" Value="2"></asp:ListItem>
                        </asp:DropDownList>
                    </td>

                    <td class="pad5"> 

                        <a href="#" data-toggle="tooltip" title="Generate" class="white">
                            <i class="fa fa-play-circle" onclick="SVCountGenerate(event)"></i>
                        </a>
                    </td>
                    <td class="pad5">
                        <asp:LinkButton ID="LinkButton1sv" class="white" runat="server" OnClick="LinkButton1_Click" data-toggle="tooltip" title="Export to Excel">
                            <%--<i class="fa fa-file"></i>--%>
                             <img src="../../Dashboard/images/excel.png" class="excelIco" />
                        </asp:LinkButton>

                    </td>

                    <td class="pad5">
                        <span data-toggle="tooltip" title="Help">
                            <span data-toggle="popover" data-placement="left" data-html="true"  data-content="This report shows the Count of Total Sales Visit Unique Customerwise on the given period. Here data is showing based on the status of Activity like whether it is done for 'Future  Sales' or 'Clarification required' etc. <br /><br /> <strong>Salesmen:</strong> Name of the user done the sales visit. <br /><br /><strong>Sales Visit:</strong> Shows the count of total Sales visit unique Customerwise in the given period. <br /><br /> <strong>Customer:</strong> Shows the count of unique Customer to whom sales visit done by salesmen in the given period."><i class="fa fa-question-circle"></i></span>
                        </span>
                    </td>

                </tr>
            </table>
             

        </div>






        <dx:ASPxGridViewExporter ID="exportersv" runat="server" Landscape="false" PaperKind="A4" GridViewID="gridSV"
            PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true" FileName="Sales Visit Count">
        </dx:ASPxGridViewExporter>

        <dx:ASPxGridView ID="gridSV" runat="server" ClientInstanceName="cgridSV" KeyFieldName="cnt_internalId"
            Width="100%" Settings-HorizontalScrollBarMode="Auto"
            SettingsBehavior-ColumnResizeMode="Control" OnDataBinding="gridPhone_DataBinding1"
            Settings-VerticalScrollableHeight="237" SettingsBehavior-AllowSelectByRowClick="true"
            Settings-VerticalScrollBarMode="Auto" Theme="PlasticBlue"
            Settings-ShowFilterRow="true" Settings-ShowFilterRowMenu="true">

               <SettingsDataSecurity AllowDelete="false" AllowEdit="false" AllowInsert="false" />
            <SettingsBehavior EnableCustomizationWindow="true"   />
                            <Settings ShowFooter="true"  /> 
                            <SettingsContextMenu Enabled="true" />    


            <Columns>

                <dx:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Salesmen" FieldName="SalesManName" Width="50%">
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Sales Visit" FieldName="SvCount" Width="25%" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center">
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Customer Count" FieldName="custCount" Width="25%" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center">
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