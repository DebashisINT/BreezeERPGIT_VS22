<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/PopUp.Master"   CodeBehind="SalesmanBudget.aspx.cs" Inherits="ERP.OMS.Management.Activities.SalesmanBudget" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
    <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

       <dxe:ASPxGridView ID="BudgetGrid" runat="server" AutoGenerateColumns="False"  KeyFieldName="rank"
                                                            DataSourceID="BudgetDataSource" Width="100%"  >
                                                            <Columns>
                                                                  <dxe:GridViewDataTextColumn FieldName="act_activityNo" Caption="Activity ID" VisibleIndex="1" Width="10%">
                                                                </dxe:GridViewDataTextColumn>
                                                                  <dxe:GridViewDataTextColumn FieldName="Industry" Caption="Industry" VisibleIndex="2" Width="10%">
                                                                </dxe:GridViewDataTextColumn>

                                                                <dxe:GridViewDataTextColumn FieldName="Productclass" Caption="Product Class" VisibleIndex="3" Width="18%">
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn FieldName="name" Caption="Customer" VisibleIndex="4" Width="18%">
                                                                </dxe:GridViewDataTextColumn>
                                                                 <dxe:GridViewDataTextColumn FieldName="ProductName" Caption="Product" VisibleIndex="5" Width="18%">
                                                                </dxe:GridViewDataTextColumn>

                                                                <dxe:GridViewDataTextColumn FieldName="Qty_CurrentFY" Caption="Qty(Current Fiscal Year)" VisibleIndex="6" Width="8%">
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn FieldName="Qty_PreviousFY" Caption="Qty(Previous Fiscal Year)" VisibleIndex="7" Width="8%">
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn FieldName="Qty_Permonth" Caption="Per month" VisibleIndex="8"
                                                                    Width="8%">
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn FieldName="Description" Caption="Description" VisibleIndex="9"
                                                                    Width="18%">
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn FieldName="UOM" Caption ="Stock UOM"
                                                                    VisibleIndex="10">
                                                                </dxe:GridViewDataTextColumn>

                                                             

                                                            </Columns>

                                                            <Styles>
                                                                <LoadingPanel ImageSpacing="10px">
                                                                </LoadingPanel>
                                                                <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                                </Header>
                                                                <Cell CssClass="gridcellleft">
                                                                </Cell>
                                                            </Styles>

                                                             <settingspager pagesize="10">
                            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200"/>
                              </settingspager>

                                                         
                                                         
                                                        </dxe:ASPxGridView>

                <asp:SqlDataSource ID="BudgetDataSource" runat="server" ></asp:SqlDataSource>
        </asp:Content>
