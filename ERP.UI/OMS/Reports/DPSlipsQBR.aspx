<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"  Inherits="ERP.OMS.Reports.Reports_DPSlipsQBR" EnableEventValidation="false" Codebehind="DPSlipsQBR.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
 
    <script language="javascript" type="text/javascript">
    var oldgridSelectedColor;
    function setMouseOverColor(element) {

    oldgridSelectedColor = element.style.backgroundColor;
    element.style.backgroundColor='yellow';
    element.style.cursor='hand';
    element.style.textDecoration='underline';
    }

    function setMouseOutColor(element) {

    element.style.backgroundColor=oldgridSelectedColor;
    element.style.textDecoration='none';
    }

    function height()
        {
            if(document.body.scrollHeight>=350)
            {
                window.frameElement.height = document.body.scrollHeight;
            }
            else
            {
                window.frameElement.height = '350px';
            }
            window.frameElement.width = document.body.scrollWidth;
        }
        function large(obj)
        {     
         //OnMoreInfoClick(obj,"Large Image",'940px','450px',"N");     
         if('<%= dp %>'=='CDSL')
           {   
             var url='view_signature.aspx?id=' + obj+'[CDSL]';
             OnMoreInfoClick(url,"View Signature",'940px','450px',"Y");         
           }
         else
           {
            var url='view_signature.aspx?id=' + obj+'[NSDL]';
            OnMoreInfoClick(url,"View Signature",'940px','450px',"Y");      
           }    
        }   
     </script>
     
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
         <table>
                <tr>
                    <td colspan="8">
                       
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px" align="left" valign="top">
                        Slip Type</td>
                    <td style="width: 100px" align="left">
                        <dxe:ASPxComboBox id="ddlSlipType" runat="server" ClientInstanceName="SlipType" EnableIncrementalFiltering="True"
                            EnableSynchronization="False" SelectedIndex="0" tabIndex="1" ValueType="System.String"
                            Width="140px">
                            <items>
                                <dxe:ListEditItem Text="Combined Instruction Slips" Value="1" />
                                <dxe:ListEditItem Text="Combined Instruction Slips (CM)" Value="2" />
                                <dxe:ListEditItem Text="Inter-Settlement" Value="3" />
                                <dxe:ListEditItem Text="Delivery out [CM-Payin]" Value="4" />
                                <dxe:ListEditItem Text="Delivery Out [CM Payout]" Value="5" />
                                <dxe:ListEditItem Text="Pool-To-Pool Transfers" Value="6" />
                                <dxe:ListEditItem Text="On Market" Value="7" />
                                <dxe:ListEditItem Text="Off Market" Value="8" />
                                <dxe:ListEditItem Text="Inter-Depository" Value="9" />
                                <dxe:ListEditItem Text="Pledge&amp;Hypothecation" Value="10" />
                                <dxe:ListEditItem Text="SLB Instructions" Value="11" />
                                <dxe:ListEditItem Text="Demat Request Forms" Value="12" />
                                <dxe:ListEditItem Text="Remat Request Forms" Value="13" />
                            </items>
                        </dxe:ASPxComboBox></td>
                    <td style="width: 100px" align="left">
                        </td>
                    <td style="width: 100px" align="left">
                        </td>
                    <td style="width: 100px">
                    </td>
                    <td style="width: 100px">
                    </td>
                    <td style="width: 100px">
                    </td>
                    <td style="width: 100px">
                        <asp:DropDownList ID="ddlExport" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlExport_SelectedIndexChanged1">
                            <asp:ListItem Selected="True" Value="Ex">Export</asp:ListItem>
                            <asp:ListItem Value="E">Excel</asp:ListItem>
                            <asp:ListItem Value="P">PDF</asp:ListItem>
                            <asp:ListItem Value="PM">PDF DIFF PAGES</asp:ListItem>
                        </asp:DropDownList></td>
                </tr>
                <tr id="user">
                    <td style="width: 100px" align="left">
                        Slip Number</td>
                    <td style="width: 100px" align="left">
                        <asp:TextBox ID="txtsliipno" runat="server"></asp:TextBox></td>
                    <td colspan="2" id="usertext">
                        </td>
                    <td style="width: 100px">
                    </td>
                    <td style="width: 100px">
                    </td>
                    <td style="width: 100px">
                    </td>
                    <td style="width: 100px">
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 100px; height: 16px">
                    </td>
                    <td align="left" style="width: 100px; height: 16px">
                    </td>
                    <td colspan="2" style="height: 16px">
                    </td>
                    <td style="width: 100px; height: 16px">
                    </td>
                    <td style="width: 100px; height: 16px">
                    </td>
                    <td style="width: 100px; height: 16px">
                    </td>
                    <td style="width: 100px; height: 16px">
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px" align="left">
                        </td>
                    <td style="width: 100px">
                        <asp:Button ID="Button3" runat="server" CssClass="btnUpdate" Height="23px" OnClick="Button1_Click"
                            OnClientClick="javascript:selecttion();" Text="Show" Width="101px" /></td>
                    <td style="width: 100px">
                    </td>
                    <td style="width: 100px">
                    </td>
                    <td style="width: 100px">
                    </td>
                    <td style="width: 100px">
                    </td>
                    <td style="width: 100px">
                    </td>
                    <td style="width: 100px">
                    </td>
                </tr>
                <tr>
                    <td colspan="8" rowspan="2" align="left" style="height: 16px">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                </tr>
            </table>
        <asp:Panel ID="Panel1" runat="server" Width="100%" ScrollBars="Auto" Height="300px">
                                    <table>
                                        <tr>
                                            <td colspan="3" valign="top" style="width: 1172px; height: 144px;">
                                            <asp:UpdatePanel ID="update1" runat="server"><ContentTemplate>
                                                <asp:GridView ID="offlineGrid" runat="server" AllowSorting="True" BorderColor="CornflowerBlue" BorderStyle="Solid"   
                                                    BorderWidth="2px"  ForeColor="#0000C0" 
                                                    PageSize="35" ShowFooter="True" Width="1116px" SelectedRowStyle-BackColor="orange" AutoGenerateColumns="False" OnRowDataBound="offlineGrid_RowDataBound" >
                                                    
                                                    <RowStyle BackColor="White" BorderColor="#BFD3EE" BorderStyle="Double" BorderWidth="1px"
                                                        ForeColor="#330099"  />
                                                        
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="SrlNo.">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblid" runat="server"  Text='<%# Bind("SrlNo") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle BorderWidth="1px" HorizontalAlign="Left"/>
                                                            
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="SlipNo." >
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblslip" runat="server" Width="50" Text='<%# Eval("NsdlOffline_SlipNumber") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle BorderWidth="1px" HorizontalAlign="Right"  />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Client Name[ID].">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblclient" runat="server" Width="130" Text='<%# Eval("ClientId") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle BorderWidth="1px" HorizontalAlign="Left"  />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="ISIN.">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblisin" runat="server"  Width="85" Text='<%# Eval("NsdlOffline_ISIN") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle BorderWidth="1px" HorizontalAlign="Left"  />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="ISIN Name">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblisinname" runat="server" Width="120" Text='<%# Eval("ISIN_Name") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle BorderWidth="1px" HorizontalAlign="Left"  />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="QTY">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblqty" runat="server" Width="50" Text='<%# Eval("NsdlOffline_Quantity") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle BorderWidth="1px" HorizontalAlign="Right" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Amount">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblamt" runat="server" Width="80" Text='<%# Eval("amount") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle BorderWidth="1px" HorizontalAlign="Right" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Counter-Account">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblcounter" runat="server" Width="100" Text='<%# Eval("CounterID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                           <ItemStyle BorderWidth="1px" HorizontalAlign="Left"  />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Fromsettlment">
                                                            <ItemTemplate>
                                                            <table style="border-left-width:thin" >
                                                            <tr>
                                                                <td><asp:Label ID="Label2" runat="server" Width="100px" Text='<%# Bind("FromSettlement") %>' BorderStyle="Solid" BorderWidth="1px"></asp:Label></td>
                                                                <td><asp:Label ID="lblisinname" runat="server"  Width="100px" Text='<%# Eval("FromMarketType") %>' BorderStyle="Solid" BorderWidth="1px"></asp:Label></td>
                                                            </tr>
                                                            </table>
                                                                
                                                            </ItemTemplate>
                                                          <ItemStyle BorderWidth="1px" HorizontalAlign="Right"  />
                                                        </asp:TemplateField>
                                                         
                                                        <asp:TemplateField HeaderText="Tosettlement">
                                                            <ItemTemplate>
                                                            
                                                                <table>
                                                                <tr>
                                                                    <td><asp:Label ID="Label3" runat="server" Width="100px" Text='<%# Bind("ToSettlement") %>' BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"></asp:Label></td>
                                                                    <td><asp:Label ID="lblisinname" runat="server"  Width="100px" Text='<%# Eval("MarketType") %>' BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"></asp:Label></td>
                                                                </tr>
                                                                </table>
                                                            
                                                             </ItemTemplate>
                                                            <ItemStyle BorderWidth="1px" HorizontalAlign="Right"  />
                                                        </asp:TemplateField>
                                                        
                                                        <asp:TemplateField HeaderText="Exec.Date">
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label4" runat="server" Width="65" Text='<%# Bind("ExecutionDate") %>'></asp:Label>
                                                            </ItemTemplate>
                                                             <ItemStyle BorderWidth="1px" HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                       <asp:TemplateField HeaderText="Trans.Type">
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label5" runat="server" Width="100" Text='<%# Bind("TransactionType") %>'></asp:Label>
                                                            </ItemTemplate>
                                                             <ItemStyle BorderWidth="1px" HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Trans.Date">
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label6" runat="server" Width="65" Text='<%# Bind("TransactionDate") %>'></asp:Label>
                                                            </ItemTemplate>
                                                             <ItemStyle BorderWidth="1px" HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                       <asp:TemplateField HeaderText="View Signature">
                                                            <ItemTemplate>
                                                                <a href="javascript:void(0);" onclick="large('<%# Eval("IDForSignature") %>')">
                                                                   View Signature....
                                                                </a>
                                                            </ItemTemplate>
                                                            <ItemStyle BorderWidth="1px" HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                    <PagerStyle ForeColor="White" HorizontalAlign="Center" />
                                                   
                                                    <HeaderStyle BorderColor="AliceBlue" BorderWidth="1px" CssClass="EHEADER" Font-Bold="False"
                                                        ForeColor="Black" />
                                                    <SelectedRowStyle BackColor="Orange" />
                                                    
                                                  
                                                </asp:GridView>
                                                </ContentTemplate></asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
        </div>
</asp:Content>