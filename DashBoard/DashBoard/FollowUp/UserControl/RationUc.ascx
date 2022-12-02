<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RationUc.ascx.cs" Inherits="DashBoard.DashBoard.FollowUp.UserControl.RationUc" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
 
<script>

    function RatioCountGenerate(e) {
        cratio.Refresh();
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
                    <td class="" style="width: 215px;"><i class="fa fa-bell" style="font-size: 25px; color: #f7ffb7;"></i><span class="trSpan besidepart">(Customer Realisation Ratio)</span></td>
                </tr>
                <tr>
                    <td class="pad5">


                        <dx:ASPxDateEdit ID="ratioFromdate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" AllowNull="false"
                            ClientInstanceName="cratioFromdate" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True" data-toggle="tooltip" title="As on Date"
                            Theme="PlasticBlue">
                            <ButtonStyle Width="13px">
                            </ButtonStyle>
                        </dx:ASPxDateEdit>
                    </td>

                     <td class="pad5">


                        <dx:ASPxDateEdit ID="ratioTodate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" AllowNull="false"
                            ClientInstanceName="cratioTodate" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True" data-toggle="tooltip" title="As on Date"
                            Theme="PlasticBlue">
                            <ButtonStyle Width="13px">
                            </ButtonStyle>
                        </dx:ASPxDateEdit>
                    </td>



                    <td class="pad5">

                        <a href="#" data-toggle="tooltip" title="Generate" class="white">
                            <i class="fa fa-play-circle" onclick="RatioCountGenerate(event)"></i>
                        </a>
                    </td>
                    <td class="pad5">
                        <asp:LinkButton ID="LinkButton1closedrtio" OnClick="LinkButton1closedrtio_Click" class="white" runat="server" data-toggle="tooltip" title="Export to Excel">
                            <%--<i class="fa fa-file"></i>--%>
                             <img src="../../Dashboard/images/excel.png" class="excelIco" />
                        </asp:LinkButton>

                    </td>

                    <td class="pad5">
                        <span data-toggle="tooltip" title="Help">
                            <span data-toggle="popover" data-placement="left" data-html="true" data-content="This report shows conversion ratio of the followup done to particular customer.<br /><br />Customer: Shows the respective name who marked the follow up as 'Closed'.<br /><br />Ratio: Shows the total count of followup activities marked as Closed out of Total number of activities done."><i class="fa fa-question-circle"></i></span>
                        </span>
                    </td>

                </tr>
            </table>


        </div>
         






        <dx:ASPxGridViewExporter ID="exporterclosedratio" runat="server" Landscape="false" PaperKind="A4" GridViewID="ratio"
            PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true" FileName="Conversion Ratio of Closing">
        </dx:ASPxGridViewExporter>

        <dx:ASPxGridView ID="ratio" runat="server" ClientInstanceName="cratio" KeyFieldName="CustID"
                                Width="100%" Settings-HorizontalScrollBarMode="Auto"
                                SettingsBehavior-ColumnResizeMode="Control" OnDataBinding="ASPxGridView1_DataBinding"
                                Settings-VerticalScrollableHeight="237" SettingsBehavior-AllowSelectByRowClick="true"
                                Settings-VerticalScrollBarMode="Auto" Theme="PlasticBlue"
                                Settings-ShowFilterRow="true" Settings-ShowFilterRowMenu="true">
                                <SettingsDataSecurity AllowDelete="false" AllowEdit="false" AllowInsert="false" />
                                <SettingsBehavior EnableCustomizationWindow="true"   />
                                    <Settings ShowFooter="true"  /> 
                                    <SettingsContextMenu Enabled="true" />    
                                <Columns>

                                <dx:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Customer" FieldName="name" Width="75%">
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <Settings AutoFilterCondition="Contains" />
                                </dx:GridViewDataTextColumn>

                                <dx:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Ratio" FieldName="Ratio" Width="25%">
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
