<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="allFollowup.ascx.cs" Inherits="DashBoard.DashBoard.FollowUp.UserControl.allFollowup" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<script>
    function Allfollowup(e) {
        cgridEF.Refresh();
        e.preventDefault();
    }
</script>

<div>
    <aside class="colWraper">
         
        <div class="diverh col-md-12">

            <table>
                <tr>
                    <td class="" style="width: 250px;"><i class="fa fa-bell" style="font-size: 25px; color: #f7ffb7;"></i><span class="trSpan besidepart">(Total Followup)</span></td>
                </tr>
                <tr>
                     <td class="pad5">

                        <dx:ASPxDateEdit ID="allFormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" AllowNull="false"
                            ClientInstanceName="callFormDate" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                            <ButtonStyle Width="13px">
                            </ButtonStyle>
                        </dx:ASPxDateEdit>
                    </td>
                    
                    <td class="pad5">

                        <dx:ASPxDateEdit ID="alltoDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" AllowNull="false"
                            ClientInstanceName="calltoDate" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                            <ButtonStyle Width="13px">
                            </ButtonStyle>
                        </dx:ASPxDateEdit>
                    </td>

                    



                    <td class="pad5">

                        <a href="#" data-toggle="tooltip" title="Generate" class="white">
                            <i class="fa fa-play-circle" onclick="Allfollowup(event)"></i>
                        </a>
                    </td>
                    <td class="pad5">
                        <asp:LinkButton ID="LinkButton1pactallfollow" OnClick="LinkButton1pactallfollow_Click" class="white" runat="server"  data-toggle="tooltip" title="Export to Excel">
                             <img src="../../Dashboard/images/excel.png" class="excelIco" />
                        </asp:LinkButton>

                    </td>

                    <td class="pad5">
                        <span data-toggle="tooltip" title="Help">
                            <span data-toggle="popover" data-placement="left" data-html="true" data-content="This report shows the Count of Total Followup Activities. Show data in the given from date and to date and it is Document-wise.<br/><br/>Customer: Shows the respective customer for which follow up done.<br/><br/>Follow up by: Shows the name who entered the followup entry in the system.<br/><br/>Document: Shows the respective document/blank based on selection for which follow up done.<br/><br/>Count: Shows the total count of followup done for Respective customer and selected document"><i class="fa fa-question-circle"></i></span>
                        </span>
                    </td>

                </tr>
            </table>


        </div>
         






        <dx:ASPxGridViewExporter ID="exporterpactallfollowup" runat="server" Landscape="false" PaperKind="A4" GridViewID="gridEF"
            PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true" FileName="Total Followup">
        </dx:ASPxGridViewExporter>

        
        
<dx:ASPxGridView ID="gridEF" runat="server" ClientInstanceName="cgridEF" KeyFieldName="CustID"
            Width="100%" Settings-HorizontalScrollBarMode="Auto"
            SettingsBehavior-ColumnResizeMode="Control" OnDataBinding="gridEF_DataBinding"
            Settings-VerticalScrollableHeight="237" SettingsBehavior-AllowSelectByRowClick="true"
            Settings-VerticalScrollBarMode="Auto" Theme="PlasticBlue"
            Settings-ShowFilterRow="true" Settings-ShowFilterRowMenu="true">
               <SettingsDataSecurity AllowDelete="false" AllowEdit="false" AllowInsert="false" />
            <SettingsBehavior EnableCustomizationWindow="true"   />
                            <Settings ShowFooter="true"  /> 
                            <SettingsContextMenu Enabled="true" />    
            <Columns>

                <dx:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Customer" FieldName="name" Width="30%">
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dx:GridViewDataTextColumn>

                   <dx:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Followup By" FieldName="user_name" Width="20%">
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Document" FieldName="Document" Width="30%">
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Count" FieldName="cnt" Width="20%">
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



