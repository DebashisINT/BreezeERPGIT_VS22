<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PhoneCallCount.ascx.cs" Inherits="DashBoard.DashBoard.CRM.UserControl.PhoneCallCount" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<script src="../CRM/Js/CRMDb.js?v=0.1"></script>
 

<div>
    <aside class="colWraper">
        <div class="diverh">
            <table>
                <tr>
                    <td class="firstCell" style="width: 40px;" ><i class="fa fa-phone" style="font-size: 21px; color: #f7ffb7;"></i><span class="trSpan">(Call)</span></td>
                    <td class="pad5">


                        <dx:ASPxDateEdit ID="FromDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" AllowNull="false"
                            ClientInstanceName="cFormDate" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True" data-toggle="tooltip" title="From Date"
                            Theme="PlasticBlue">
                            <ButtonStyle Width="13px">
                            </ButtonStyle>
                        </dx:ASPxDateEdit>
                    </td>


                    <td class="pad5">
                        <dx:ASPxDateEdit ID="Todate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" AllowNull="false"
                            ClientInstanceName="cTodate" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True" data-toggle="tooltip" title="To Date"
                            Theme="PlasticBlue">
                            <ButtonStyle Width="13px">
                            </ButtonStyle>
                        </dx:ASPxDateEdit>
                    </td>

                    <td class="pad5">
                        <asp:DropDownList ID="dpActivitylist" runat="server" Width="100%" Style="margin-top: 5px;" data-toggle="tooltip" title="Activity Status">
                            <asp:ListItem Text="Future Sale" Value="3"></asp:ListItem>
                            <asp:ListItem Text="Clarification Required" Value="5"></asp:ListItem>
                            <asp:ListItem Text="Document Collection" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Closed" Value="2"></asp:ListItem>
                        </asp:DropDownList>
                    </td>

                    <td class="pad5">
                        <%--<input type="button" value="Show" class="btn btn-success" onclick="CallCountGenerate()" />--%>

                        <a href="#" data-toggle="tooltip" title="Generate" class="white">
                            <i class="fa fa-play-circle" onclick="CallCountGenerate(event)"></i>
                        </a>
                    </td>
                    <td class="pad5">
                        <asp:LinkButton ID="LinkButton1" class="white" runat="server" OnClick="LinkButton1_Click" data-toggle="tooltip" title="Export to Excel">
                           <%-- <i class="fa fa-file"></i>--%>
                            <img src="../../Dashboard/images/excel.png" class="excelIco" />

                        </asp:LinkButton>

                    </td>

                    <td class="pad5">
                        <span data-toggle="tooltip" title="Help">
                            <span data-toggle="popover" data-placement="left" data-html="true"  data-content="This report shows the Count of Total Phone Calls Unique Customerwise on the given period. Here data is showing based on the status of Activity like whether it is done for 'Future  Sales' or 'Clarification required' etc. <br /><br /> <strong>Salesmen:</strong> Name of the user done the phone call. <br /><br /><strong>Phone Call:</strong> Shows the count of total Phone Call unique Customerwise in the given period. <br /><br /> <strong>Customer:</strong> Shows the count of unique customer to whom phone call done by salesmen in the given period."><i class="fa fa-question-circle"></i></span>
                        </span>
                    </td>

                </tr>
            </table>


        </div>






        <dx:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" GridViewID="gridPhone"
            PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true" FileName="Call Count">
        </dx:ASPxGridViewExporter>

        <dx:ASPxGridView ID="gridPhone" runat="server" ClientInstanceName="cgridPhone" KeyFieldName="cnt_internalId"
            Width="100%" Settings-HorizontalScrollBarMode="Auto"
            SettingsBehavior-ColumnResizeMode="Control" OnDataBinding="gridPhone_DataBinding1"
            Settings-VerticalScrollableHeight="237" SettingsBehavior-AllowSelectByRowClick="true"
            Settings-VerticalScrollBarMode="Auto" Theme="PlasticBlue"
            Settings-ShowFilterRow="true" Settings-ShowFilterRowMenu="true" >
               <SettingsDataSecurity AllowDelete="false" AllowEdit="false" AllowInsert="false" />
            <SettingsBehavior EnableCustomizationWindow="true"   />
                            <Settings ShowFooter="true"  /> 
                            <SettingsContextMenu Enabled="true" />    
            <Columns>

                <dx:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Salesman" FieldName="SalesManName" Width="40%">
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" /> 
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Phone Call" FieldName="CallCount" HeaderStyle-HorizontalAlign="Center" Width="20%">
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                     <CellStyle HorizontalAlign="Center" />
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Customer" FieldName="custCount" HeaderStyle-HorizontalAlign="Center" Width="20%">
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                     <CellStyle HorizontalAlign="Center" />
                </dx:GridViewDataTextColumn>


                <dx:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" Width="20%">
                    <DataItemTemplate>


                        <a href="javascript:void(0);" onclick="loadList('<%# Container.KeyValue %>')" class="pad" title="Follow-up History">
                            <i class="fa fa-search"></i>
                        </a>
                    </DataItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" CssClass="gridHeader"></HeaderStyle>
                    <CellStyle HorizontalAlign="Center"></CellStyle>
                    <HeaderTemplate>Details</HeaderTemplate>
                    <EditFormSettings Visible="False"></EditFormSettings>
                    <Settings AllowAutoFilterTextInputTimer="False" />

                </dx:GridViewDataTextColumn>

            </Columns>

            <SettingsPager PageSize="10" NumericButtonCount="4">
                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
            </SettingsPager>
        </dx:ASPxGridView>
    </aside>

    <asp:HiddenField runat="server" ID="hdSalesmanId" />
    <dx:ASPxTextBox ID="hidesalesman" ClientInstanceName="chideSalesman" runat="server" Width="170px" ClientVisible="false"></dx:ASPxTextBox>


</div>


<!--Customer Modal -->
<div class="modal fade" id="CustModel" role="dialog">
    <div class="modal-dialog">

        <!-- Modal content-->
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal">&times;</button>
                <h4 class="modal-title">Customer Search</h4>
            </div>
            <div class="modal-body">
                <input type="text" onkeydown="Customerkeydown(event)" id="txtCustSearch" autofocus width="100%" placeholder="Search By Customer Name or Unique Id" />

                <div id="CustomerTable">
                    <table border='1' width="100%" class="dynamicPopupTbl">
                        <tr class="HeaderStyle">
                            <th class="hide">id</th>
                            <th>Customer Name</th>
                            <th>Unique Id</th>
                            <th>Address</th>
                        </tr>
                    </table>
                </div>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
            </div>
        </div>

    </div>
</div>








<!--Details of call Modal -->
<div class="modal fade" id="callditailsmod" role="dialog">
    <div class="modal-dialog">

        <!-- Modal content-->
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal">&times;</button>
                <h4 class="modal-title">Call Details</h4>
            </div>
            <div class="modal-body">

                <dx:ASPxGridView ID="gridPhoneDet" runat="server" ClientInstanceName="cgridPhoneDet" KeyFieldName="phd_id"
                    Width="100%" Settings-HorizontalScrollBarMode="Auto"
                    SettingsBehavior-ColumnResizeMode="Control" DataSourceID="EntityServerModePhoneDet"
                    Settings-VerticalScrollableHeight="275" SettingsBehavior-AllowSelectByRowClick="true"
                    Settings-VerticalScrollBarMode="Auto" Theme="PlasticBlue"
                    Settings-ShowFilterRow="true" Settings-ShowFilterRowMenu="true">

                    <Columns>

                        <dx:GridViewDataDateColumn Caption="Call Date" Width="20%" FieldName="phd_callDate"
                            PropertiesDateEdit-DisplayFormatString="dd-MM-yyyy" PropertiesDateEdit-EditFormatString="dd-MM-yyyy">
                            <Settings AllowAutoFilterTextInputTimer="False" />
                            <Settings AutoFilterCondition="Equals" />
                        </dx:GridViewDataDateColumn>




                        <dx:GridViewDataTextColumn Caption="Note" FieldName="note" Width="80%">
                            <Settings AllowAutoFilterTextInputTimer="False" />
                            <Settings AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>

                    </Columns>
                    <SettingsPager Mode="EndlessPaging" PageSize="20" />
                </dx:ASPxGridView>

                <dx:LinqServerModeDataSource ID="EntityServerModePhoneDet" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
                   />


            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
            </div>
        </div>

    </div>
</div>
