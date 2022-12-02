<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FollowupUsing.ascx.cs" Inherits="DashBoard.DashBoard.FollowUp.UserControl.FollowupUsing" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<script>
    function UsingAllfollowup(e) {
        cgridusing.Refresh();
        e.preventDefault();
    }
</script>

<div>
    <aside class="colWraper">
         
        <div class="diverh col-md-12">

            <table>
                <tr>
                    <td class="" style="width: 250px;"><i class="fa fa-bell" style="font-size: 25px; color: #f7ffb7;"></i><span class="trSpan besidepart">(Followup Using)</span></td>
                </tr>
                <tr>
                     <td class="pad5">

                        <dx:ASPxDateEdit ID="usingFormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" AllowNull="false"
                            ClientInstanceName="cusingFormDate" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                            <ButtonStyle Width="13px">
                            </ButtonStyle>
                        </dx:ASPxDateEdit>

                    </td>
                    
                    <td class="pad5">

                        <dx:ASPxDateEdit ID="usingTodate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" AllowNull="false"
                            ClientInstanceName="cusingTodate" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                            <ButtonStyle Width="13px">
                            </ButtonStyle>
                        </dx:ASPxDateEdit>
                    </td>

                     <td class="pad5">
                     <asp:DropDownList ID="ddlusning" runat="server" >
                         <asp:ListItem Text="Open" Value="Open" Selected="True"></asp:ListItem>
                         <asp:ListItem Text="Close" Value="Close"></asp:ListItem>
                         <asp:ListItem Text="Disputed" Value="Disputed"></asp:ListItem>
                     </asp:DropDownList>
                     </td>
                    



                    <td class="pad5">

                        <a href="#" data-toggle="tooltip" title="Generate" class="white">
                            <i class="fa fa-play-circle" onclick="UsingAllfollowup(event)"></i>
                        </a>
                    </td>
                    <td class="pad5">
                        <asp:LinkButton ID="LinkButton1pactallusing" OnClick="LinkButton1pactallusing_Click" class="white" runat="server"  data-toggle="tooltip" title="Export to Excel">
                             <img src="../../Dashboard/images/excel.png" class="excelIco" />
                        </asp:LinkButton>

                    </td>

                    <td class="pad5">
                        <span data-toggle="tooltip" title="Help">
                            <span data-toggle="popover" data-placement="left" data-html="true" data-content="This report shows the Count of Total Pending Activities. Pending Activities means those activities which are having 'Next Activity Date' <= 'As On Date' and it is Document-wise. <br /><br /> <strong>Salesmen:</strong> Shows the list of salesmen not completed task for next activity date.<br /><br /><strong>Activities:</strong> Shows the count of total pending task for the assigned salesmen in the given period."><i class="fa fa-question-circle"></i></span>
                        </span>
                    </td>

                </tr>
            </table>


        </div>
         






        <dx:ASPxGridViewExporter ID="exporterpactusing" runat="server" Landscape="false" PaperKind="A4" GridViewID="gridusing"
            PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true" FileName="Followup Using">
        </dx:ASPxGridViewExporter>

        
        
<dx:ASPxGridView ID="gridusing" runat="server" ClientInstanceName="cgridusing" KeyFieldName="id"
            Width="100%" Settings-HorizontalScrollBarMode="Auto"
            SettingsBehavior-ColumnResizeMode="Control"  DataSourceID="EntityServerModePhoneDet"
            Settings-VerticalScrollableHeight="237" SettingsBehavior-AllowSelectByRowClick="true"
            Settings-VerticalScrollBarMode="Auto" Theme="PlasticBlue"
            Settings-ShowFilterRow="true" Settings-ShowFilterRowMenu="true">
               <SettingsDataSecurity AllowDelete="false" AllowEdit="false" AllowInsert="false" />
            <SettingsBehavior EnableCustomizationWindow="true"   />
                            <Settings ShowFooter="true"  /> 
                            <SettingsContextMenu Enabled="true" />    
            <Columns>

               <dx:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Customer" FieldName="name" Width="40%" >
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dx:GridViewDataTextColumn>

                 


                    <dx:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Document" FieldName="Document" Width="30%" >
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dx:GridViewDataTextColumn>


                    <dx:GridViewDataDateColumn Caption="Date" Width="15%" FieldName="DocDate"
                            PropertiesDateEdit-DisplayFormatString="dd-MM-yyyy" PropertiesDateEdit-EditFormatString="dd-MM-yyyy">
                            <Settings AllowAutoFilterTextInputTimer="False" />
                            <Settings AutoFilterCondition="Equals" />
                        </dx:GridViewDataDateColumn> 

              
                
                    <dx:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Using" FieldName="FollowUsing" Width="15%"  >
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dx:GridViewDataTextColumn> 

            </Columns>

            <SettingsPager PageSize="10" NumericButtonCount="4">
                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
            </SettingsPager>
        </dx:ASPxGridView>


    </aside>
      <dx:LinqServerModeDataSource ID="EntityServerModePhoneDet" runat="server" OnSelecting="EntityServerModePhoneDet_Selecting"
                   />

</div>



