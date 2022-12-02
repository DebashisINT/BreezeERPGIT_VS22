<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="closedUc.ascx.cs" Inherits="DashBoard.DashBoard.FollowUp.UserControl.closedUc" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
 
<script>

    function ClosedCountGenerate(e) {
        cgridClosed.Refresh();
        e.preventDefault();
    }

    $(document).ready(function () {
        $('[data-toggle="tooltip"]').tooltip();
        $('[data-toggle="popover"]').popover({ html: true });
    });


       
</script>

<div>
    <aside class="colWraper">
         
        <div class="diverh col-md-12">

            <table>
                <tr>
                    <td class="" style="width: 190px;"><i class="fa fa-bell" style="font-size: 25px; color: #f7ffb7;"></i><span class="trSpan besidepart">(Closed Followup)</span></td>
                </tr>
                <tr>
                    <td class="pad5">


                        <dx:ASPxDateEdit ID="closedFromdate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" AllowNull="false"
                            ClientInstanceName="cclosedFromdate" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True" data-toggle="tooltip" title="As on Date"
                            Theme="PlasticBlue">
                            <ButtonStyle Width="13px">
                            </ButtonStyle>
                        </dx:ASPxDateEdit>
                    </td>

                     <td class="pad5">


                        <dx:ASPxDateEdit ID="closedTodate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" AllowNull="false"
                            ClientInstanceName="cclosedTodate" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True" data-toggle="tooltip" title="As on Date"
                            Theme="PlasticBlue">
                            <ButtonStyle Width="13px">
                            </ButtonStyle>
                        </dx:ASPxDateEdit>
                    </td>



                    <td class="pad5">

                        <a href="#" data-toggle="tooltip" title="Generate" class="white">
                            <i class="fa fa-play-circle" onclick="ClosedCountGenerate(event)"></i>
                        </a>
                    </td>
                    <td class="pad5">
                        <asp:LinkButton ID="LinkButton1closed" OnClick="LinkButton1closed_Click" class="white" runat="server" data-toggle="tooltip" title="Export to Excel">
                             <img src="../../Dashboard/images/excel.png" class="excelIco" />
                        </asp:LinkButton>

                    </td>

                    <td class="pad5">
                        <span data-toggle="tooltip" title="Help">
                            <span data-toggle="popover" data-placement="left" data-html="true" data-content="This report shows the Count of Total Followup Activities which are marked as Closed in the system for the selected from date and to date and it is Document-wise.<br /><br />Closed By: Shows the respective name who marked the follow up as 'Closed'.<br /><br />Count: Shows the total count of followup activities marked as Closed.</br></br>Closed Followup : One new column 'Conversion Ratio', formula=(Activity wise total followup/activity wise closed followup)*100"><i class="fa fa-question-circle"></i></span>
                        </span>
                    </td>

                </tr>
            </table>


        </div>
         






        <dx:ASPxGridViewExporter ID="exporterclosed" runat="server" Landscape="false" PaperKind="A4" GridViewID="gridClosed"
            PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true" FileName="Closed Followup">
        </dx:ASPxGridViewExporter>

        <dx:ASPxGridView ID="gridClosed" runat="server" ClientInstanceName="cgridClosed" KeyFieldName="followedBy"
            Width="100%" Settings-HorizontalScrollBarMode="Auto"
            SettingsBehavior-ColumnResizeMode="Control" OnDataBinding="gridClosed_DataBinding"
            Settings-VerticalScrollableHeight="237" SettingsBehavior-AllowSelectByRowClick="true"
            Settings-VerticalScrollBarMode="Auto" Theme="PlasticBlue"
            Settings-ShowFilterRow="true" Settings-ShowFilterRowMenu="true">

            <SettingsDataSecurity AllowDelete="false" AllowEdit="false" AllowInsert="false" />
            <SettingsBehavior EnableCustomizationWindow="true" />
            <Settings ShowFooter="true" />
            <SettingsContextMenu Enabled="true" />
            <Columns>

                <dx:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Closed By" FieldName="user_name" Width="50%">
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Count" FieldName="cnt" Width="25%"
                    HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center">
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dx:GridViewDataTextColumn>

                  <dx:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Ratio" FieldName="ratio" Width="25%"
                    HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center">
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
