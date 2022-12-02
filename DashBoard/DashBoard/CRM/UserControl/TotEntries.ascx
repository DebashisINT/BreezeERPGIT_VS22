<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TotEntries.ascx.cs" Inherits="DashBoard.DashBoard.CRM.UserControl.TotEntries" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>


<script src="../CRM/Js/TotEntries.js?v=0.1"></script>

 


<div>
    <aside class="colWraper">
        <div class="diverh">
            <table>
                <tr>
                    <td class="firstCell" style="width:65px"><i class=" fa fa-line-chart" style="font-size: 25px;color: #f7ffb7;"></i><span class="trSpan">Activities</span></td>
                    <td class="pad5">


                        <dx:ASPxDateEdit ID="FromDateTE" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" AllowNull="false"
                            ClientInstanceName="cFormDateTE" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True"  data-toggle="tooltip" title="From Date"
                            Theme="PlasticBlue">
                            <ButtonStyle Width="13px">
                            </ButtonStyle>
                        </dx:ASPxDateEdit>
                    </td>


                    <td class="pad5">
                        <dx:ASPxDateEdit ID="TodateTE" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" AllowNull="false"
                            ClientInstanceName="cTodateTE" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True"  data-toggle="tooltip" title="To Dtae"
                            Theme="PlasticBlue">
                            <ButtonStyle Width="13px">
                            </ButtonStyle>
                        </dx:ASPxDateEdit>
                    </td>
                     
                    <td class="pad5"> 

                        <a href="#" data-toggle="tooltip" title="Generate" class="white">
                            <i class="fa fa-play-circle" onclick="TotActGenerate(event)"></i>
                        </a>
                    </td>
                    <td class="pad5">
                        <asp:LinkButton ID="LinkButton1TE" class="white" runat="server" OnClick="LinkButton1_Click" data-toggle="tooltip" title="Export to Excel">
                            <%--<i class="fa fa-file"></i>--%>
                             <img src="../../Dashboard/images/excel.png" class="excelIco" />
                        </asp:LinkButton>

                    </td>

                    <td class="pad5">
                        <span data-toggle="tooltip" title="Help">
                            <span data-toggle="popover" data-placement="left" data-html="true"  data-content="This report shows the Count of Total Activities entered by salesman for assigned task for the selected period. <br /><br /> <strong>Salesmen:</strong> Name of the user done the Activities. <br /><br /><strong>Total Entries:</strong> Shows the count of total entries for the assigned task in the given period."><i class="fa fa-question-circle"></i></span>
                        </span>
                    </td>

                </tr>
            </table>


        </div>






        <dx:ASPxGridViewExporter ID="exporterTE" runat="server" Landscape="false" PaperKind="A4" GridViewID="gridTE"
            PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true" FileName="Call Count">
        </dx:ASPxGridViewExporter>

        <dx:ASPxGridView ID="gridTE" runat="server" ClientInstanceName="cgridTE" KeyFieldName="cnt_internalId"
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

                <dx:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Salesmen" FieldName="SalesManName" Width="60%">
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Total Entries" FieldName="ActCount" Width="30%"
                    HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center">
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dx:GridViewDataTextColumn>

                 <dx:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="%(Percentage)" FieldName="Percentage" Width="30%"
                    HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center">
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dx:GridViewDataTextColumn>
                 
                 

            </Columns>

            <SettingsPager PageSize="10" NumericButtonCount="4">
                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200"  />
            </SettingsPager>
        </dx:ASPxGridView>
    </aside>
     
</div>
