<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Reports_frmClientsInBannedList" Codebehind="frmClientsInBannedList.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />

    <script language="javascript" type="text/javascript">
    function SignOff()
    {
        window.parent.SignOff();
    }
    function height()
    {
//        alert('Hi');
        if(document.body.scrollHeight>=350)
            window.frameElement.height = document.body.scrollHeight;
        else
            window.frameElement.height = '350px';
        window.frameElement.Width = document.body.scrollWidth;
    }
    function OpenLink(CntID)
    {
        var url="frmBannedClient.aspx?id="+CntID;
        OnMoreInfoClick(url,'Details For'+CntID,'940px','550px,resize=0','N');
        
    }
    function SebiDetail(objID)
    {
        var url="../management/frmBannedClientDetail.aspx?id="+objID;
        OnMoreInfoClick(url,'Details For','940px','450px,resize=0','N');
        
    }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div style="text-align: center">
            <table class="TableMain100">
                <tr>
                    <td class="EHEADER" style="text-align: center;">
                        <strong><span style="color: #000099">SEBI DETAIL</span></strong>
                    </td>
                </tr>
            </table>
            <table class="TableMain100">
            <tr>
            <td>
                PanNumber:
            </td>
            <td style="text-align: left;">
                <asp:TextBox ID="txtPanNumber" runat="server"></asp:TextBox>
            </td>
            <td style="text-align: left;">
                <asp:Button ID="btnPanNumber" runat="server" Text="Search" OnClick="btnPanNumber_Click" />
            </td>
            <td style="text-align: left;">
                <asp:Button ID="btnClose" runat="server" Text="Close" OnClick="btnClose_Click" />
            </td>
            </tr>
            </table>
            <table class="TableMain100">
                <tr>
                    <td>
                        <asp:GridView ID="GridView1" DataKeyNames="BannedEntity_ID" runat="server" Width="100%"
                            BorderColor="CornflowerBlue" ShowFooter="True" AutoGenerateColumns="false" BorderStyle="Solid"
                            BorderWidth="2px" CellPadding="4" ForeColor="#0000C0" AllowPaging="True" OnPageIndexChanging="GridView1_PageIndexChanging" PageSize="20">
                            <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
                            <Columns>
                                <asp:TemplateField HeaderText="BannedEntity_ID" Visible="False">
                                    <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                    <ItemTemplate>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Banned OrderDate">
                                    <ItemStyle BorderWidth="1px" Width="100px" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                    <ItemTemplate>
                                        <asp:Label ID="lblBannedOrderDate" runat="server" Text='<%# Eval("BannedOrderDate")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Description">
                                    <ItemStyle BorderWidth="1px" Width="500px" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                    <ItemTemplate>
                                        <asp:Label ID="lblDescription" runat="server" Text='<%# Eval("Description")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Pan">
                                    <ItemStyle BorderWidth="1px" Width="100px" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                    <ItemTemplate>
                                        <asp:Label ID="lblPan" runat="server" Text='<%# Eval("Pan")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="CircularNumber">
                                    <ItemStyle BorderWidth="1px" Width="100px" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                    <ItemTemplate>
                                        <asp:Label ID="lblCircularNumber" runat="server" Text='<%# Eval("CircularNumber")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="BanPeriod">
                                    <ItemStyle BorderWidth="1px" Width="200px" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                    <ItemTemplate>
                                        <asp:Label ID="lblBanPeriod" runat="server" Text='<%# Eval("BanPeriod")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="CircularLink">
                                    <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                    <ItemTemplate>
                                        <%--<asp:HyperLink ID="lblCircularLink" runat="server" Text='<%# Eval("CircularLink")%>'></asp:HyperLink>--%>
                                        <%--<asp:LinkButton ID="lnkCircularLink" runat="server" Text='<%# Eval("CircularLink")%>'></asp:LinkButton>--%>
                                        <a href="javascript:OpenLink('<%# Eval("CircularLink")%>')">Download File</a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="#">
                                    <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Wrap="False" HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                    <ItemTemplate>
                                        <a href="javascript:SebiDetail('<%# Eval("BannedEntity_ID")%>')">More Info....</a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <RowStyle BackColor="White" ForeColor="#330099" BorderColor="#BFD3EE" BorderStyle="Double"
                                BorderWidth="1px"></RowStyle>
                            <EditRowStyle BackColor="#E59930"></EditRowStyle>
                            <SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>
                            <PagerStyle ForeColor="White" HorizontalAlign="Center"></PagerStyle>
                            <HeaderStyle ForeColor="Black" BorderWidth="1px" CssClass="EHEADER" BorderColor="AliceBlue"
                                Font-Bold="False"></HeaderStyle>
                            <AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label3" runat="server"></asp:Label></td>
                </tr>
            </table>
            <%--<asp:Panel ID="Panelmain" runat="server" Visible="true">
        <table id="tbl_main" class="login" cellspacing="0" cellpadding="0" width="410">
            <tbody>
                <tr>
                    <td class="lt1" style="height: 22px">
                        <h5>
                            Imports SEBI &nbsp;Files
                        </h5>
                    </td>
                </tr>
                
                 
                <tr>
                    <td class="lt brdr" style="height: 280px">
                        <table cellspacing="0" cellpadding="0" align="center">
                            <tbody>
                                <tr>
                                    <td class="lt">
                                        <table class="width100per" cellspacing="12" cellpadding="0">
                                            <tbody>
                                                <tr>
                                                    <td class="lt" style="height: 22px">
                                                    </td>
                                                    <td class="lt" style="width: 278px; height: 22px;">
                                                        <asp:Label ID="importstatus" runat="server" Font-Size="XX-Small" Font-Names="Arial"
                                                            Font-Bold="True" ForeColor="Red" />
                                                    </td>
                                                </tr>
                                                 
                                                  <tr>
                                                    <td style="height: 22px">
                                                        &nbsp;&nbsp;&nbsp;<br />
                                                        <br />
                                                        <br />
                                                        <br />
                                                    </td>
                                                    <td class="lt" style="width: 278px; height: 22px;">
                                                        
                                                       
                                                        <table>
                                                        
                                                            <tr>
                                                                <td style="width: 6px">
                                                                    <asp:ScriptManager ID="ScriptManager1" runat="server">
                                                                    </asp:ScriptManager> 
                                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">                                         
                                                            <ContentTemplate>
                                                            <br />
                                                         <asp:Label id="lblMessage" runat="server" Width="270px"></asp:Label>
                                                         <asp:Button id="btnDownload" onclick="btnDownload_Click" runat="server" Text="Download File" Width="84px" CssClass="btn"></asp:Button>
                                                            </ContentTemplate>
                                                            
                                                       </asp:UpdatePanel>
                                                        
                                                               </td>
                                                            </tr>
                                                           
                                                            <tr>
                                                                <td style="width: 6px; height: 18px;">
                                                        <asp:FileUpload ID="OFDSelectFile" runat="server" Width="272px" Height="23px"/>
                                                        </td>
                                                        
                                                   
                                                            </tr>
                                                       <tr>
                                                       <td></td>
                                                       </tr>
                                                       <tr>
                                                            <td>
                                                                <asp:Button ID="BtnSave" runat="server" Text="Import File" CssClass="btn" OnClick="BtnSave_Click" Width="84px" />
                                                            </td>
                                                      </tr>
                                                        </table>
                                                         
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="lt" style="height: 22px">
                                                    </td>
                                                    <td class="lt" style="width: 278px; height: 22px;">
                                                        <br />
                                                        &nbsp;</td>
                                                </tr>
                                                 <tr>
                                                    <td class="lt" style="height: 22px">
                                                    </td>
                                                    <td class="lt" style="width: 278px; height: 22px;">
                                                        <asp:RadioButton ID="rdBtnmemcodetxt" runat="server" GroupName="imp" Visible="False" />
                                                        <asp:RadioButton ID="rdBtnmemcodeptxt" runat="server" GroupName="imp" Visible="False" Width="38px" />
                                                    </td>
                                                </tr>
                                                     <tr>
                                                    <td class="lt" style="height: 22px">
                                                    </td>
                                                    <td class="lt" style="width: 278px; height: 22px;">
                                                        <asp:RadioButton ID="rdBtntxt" runat="server" GroupName="imp" Visible="False" />
                                                        <asp:RadioButton ID="rdBtnmencodecsv" runat="server" GroupName="imp" Visible="False" Width="75px" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="lt" style="height: 22px">
                                                    </td>
                                                    <td align="right" style="width: 278px; height: 22px;">
                                                        <asp:HiddenField ID="hdnDate" runat="server" />
                                                        &nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td align="right" valign="middle" colspan="2">
                                                    </td>
                                                </tr>
                                                <tr style="display: none">
                                                    <td>
                                                        <asp:TextBox ID="txtTableName" runat="server" Width="272px">TempTable</asp:TextBox></td>
                                                    <td style="width: 278px">
                                                        <asp:TextBox ID="txtCSVDir" runat="server" Width="272px">Import/Table</asp:TextBox></td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    <asp:Timer ID="Timer1" runat="server">
                                                    </asp:Timer>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
            </tbody>
        </table>
    </asp:Panel>--%>
            <asp:HiddenField ID="hdnPath" runat="server" />
        </div>
</asp:Content>