<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PendingFollowup.ascx.cs" Inherits="DashBoard.DashBoard.FollowUp.UserControl.PendingFollowup" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
 
<script>

    function PactCountGenerate(e) {
        cgridpact.Refresh();
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
                    <td class="" style="width: 190px;"><i class="fa fa-bell" style="font-size: 25px; color: #f7ffb7;"></i><span class="trSpan besidepart">(Pending Followup)</span></td>
                </tr>
                <tr>
                    <td class="pad5">


                        <dx:ASPxDateEdit ID="FromDatePAct" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" AllowNull="false"
                            ClientInstanceName="cFormDatePAct" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True" data-toggle="tooltip" title="As on Date"
                            Theme="PlasticBlue">
                            <ButtonStyle Width="13px">
                            </ButtonStyle>
                        </dx:ASPxDateEdit>
                    </td>

                    



                    <td class="pad5">

                        <a href="#" data-toggle="tooltip" title="Generate" class="white">
                            <i class="fa fa-play-circle" onclick="PactCountGenerate(event)"></i>
                        </a>
                    </td>
                    <td class="pad5">
                        <asp:LinkButton ID="LinkButton1pactfollowup" class="white" runat="server" OnClick="LinkButton1pactfollowup_Click" data-toggle="tooltip" title="Export to Excel">
                            <%--<i class="fa fa-file"></i>--%>
                             <img src="../../Dashboard/images/excel.png" class="excelIco" />
                        </asp:LinkButton>

                    </td>

                    <td class="pad5">
                        <span data-toggle="tooltip" title="Help">
                            <span data-toggle="popover" data-placement="left" data-html="true" data-content="This report shows the Count of Total Pending Activities. Pending Activities means those activities which are having 'Next Activity Date' <= 'As On Date'.<br/><br/> Customer:Shows the list of customer not completed task for next activity date.<br/><br/>Pending Followup:Shows the count of total pending followup for the respective customer."><i class="fa fa-question-circle"></i></span>
                        </span>
                    </td>

                </tr>
            </table>


        </div>
         






        <dx:ASPxGridViewExporter ID="exporterpending" runat="server" Landscape="false" PaperKind="A4" GridViewID="gridpact"
            PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true" FileName="Pending Followup">
        </dx:ASPxGridViewExporter>

        <dx:ASPxGridView ID="gridpact" runat="server" ClientInstanceName="cgridpact" KeyFieldName="CustID"
            Width="100%" Settings-HorizontalScrollBarMode="Auto"
            SettingsBehavior-ColumnResizeMode="Control" OnDataBinding="gridpact_DataBinding"
            Settings-VerticalScrollableHeight="237" SettingsBehavior-AllowSelectByRowClick="true"
            Settings-VerticalScrollBarMode="Auto" Theme="PlasticBlue"
            Settings-ShowFilterRow="true" Settings-ShowFilterRowMenu="true">

            <SettingsDataSecurity AllowDelete="false" AllowEdit="false" AllowInsert="false" />
            <SettingsBehavior EnableCustomizationWindow="true" />
            <Settings ShowFooter="true" />
            <SettingsContextMenu Enabled="true" />
            <Columns>

                <dx:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Customer" FieldName="name" Width="75%">
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Pending Followup" FieldName="cnt" Width="25%"
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

    <div class="modal fade" id="pendActModel" role="dialog">
        <div class="modal-dialog">

            <div class="modal-content">
                <%--<div class="modal-header">
                <button type="button" class="close" data-dismiss="modal">&times;</button>
                <h4 class="modal-title">Efficiency Ratio Salesmen wise</h4>
            </div>--%>
                <div class="modal-body">
                    <div>
                        <table width="100%">
                            <tr>
                                <td width="90%" class="headerTdClass">Pending Activity Salesmen wise</td>
                                <td width="10%" style="text-align: right;"><span data-toggle="tooltip" title="Click to close the chart." data-dismiss="modal" style="cursor: pointer"><i class="fa fa-times closePopup"></i></span></td>
                            </tr>
                        </table>
                    </div>
                    <div id="pendActChart" style="height: 500px"></div>
                    <div class="FooterTdClass">Salesmen</div>
                </div>
                <%-- <div class="modal-footer">
                <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
            </div>--%>
            </div>

        </div>
    </div>





</div>
