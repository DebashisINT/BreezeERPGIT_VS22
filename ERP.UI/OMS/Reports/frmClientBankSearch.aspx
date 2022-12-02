<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Reports.Reports_frmClientBankSearch" Codebehind="frmClientBankSearch.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />
    <script type="text/javascript" src="/assests/js/init.js"></script>

<%--<script type="text/javascript" src="/assests/js/ajaxList.js"></script>--%>
    <%--script type="text/javascript" src="/assests/js/loaddata1.js"></script>--%>

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
    function CallAjax(obj1,obj2,obj3)
    { 
        ajax_showOptions(obj1,obj2,obj3);
    }
    function showOptions(obj1,obj2,obj3)
    {
        FieldName='txtPanNumber';   
//          FieldName='txtTableName';        
//        var aa=dtpDate.GetValue();
//        aa=(aa.getYear()+"-"+(aa.getMonth()+1)+"-"+aa.getDate());
         
            
        ajax_showOptions(obj1,obj2,obj3);

    }
    function checkchange()
    {
        alert(document.getElementById('chkDuplicate').Checked);
        if(document.getElementById('chkDuplicate').Checked=true)
        {
            alert('hello');
            document.getElementById('txtAccountNumber').disabled=false; 
            
        }
        else if(document.getElementById('chkDuplicate').Checked='undefined')
        {
        
             alert('Hi');
             document.getElementById('txtAccountNumber').disabled=true;
            
//            document.getElementById('txtAccountNumber').focus();
//            document.form1.getElementById('txtAccountNumber').focus();
        }
    }
     FieldName='txtPanNumber';
    </script>
<style type="text/css">
	
	/* Big box with list of options */
	#ajax_listOfOptions{
		position:absolute;	/* Never change this one */
		width:50px;	/* Width of box */
		height:auto;	/* Height of box */
		overflow:auto;	/* Scrolling features */
		border:1px solid Blue;	/* Blue border */
		background-color:#FFF;	/* White background color */
		text-align:left;
		font-size:0.9em;
		z-index:32767;
	}
	#ajax_listOfOptions div{	/* General rule for both .optionDiv and .optionDivSelected */
		margin:1px;		
		padding:1px;
		cursor:pointer;
		font-size:0.9em;
	}
	#ajax_listOfOptions .optionDiv{	/* Div for each item in list */
		
	}
	#ajax_listOfOptions .optionDivSelected{ /* Selected item in the list */
		background-color:#DDECFE;
		color:Blue;
	}
	#ajax_listOfOptions_iframe{
		background-color:#F00;
		position:absolute;
		z-index:3000;
	}
	
	.grid_scroll
    {
        overflow-y: no;  
        overflow-x: scroll; 
        width:99%;
        scrollbar-base-color: #C0C0C0;
    
    }
	
	form{
		display:inline;
	}
	
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div style="text-align: center">
            <table class="TableMain100">
                <tr>
                    <td class="EHEADER" style="text-align: center;">
                        <strong><span style="color: #000099">SEARCH DETAIL</span></strong>
                    </td>
                </tr>
            </table>
            <table class="TableMain100">
            <tr>
            <td>
                Account Number:
            </td>
            <td style="text-align: left; width: 217px;">
                <asp:TextBox ID="txtAccountNumber" runat="server"></asp:TextBox>
            </td>
            <td style="text-align: right;">
                <asp:Label ID="lblDuplicate" runat="server" Text="Duplicate Record:"></asp:Label>
            </td>
            <td style="text-align: left;">
                    <asp:CheckBox ID="chkDuplicate" runat="server" AutoPostBack="True" OnCheckedChanged="chkDuplicate_CheckedChanged" Width="152px" />
            </td>
            <td style="text-align: left;">
                <asp:Button ID="btnSearch" CssClass="btnUpdate" runat="server" Text="Search" OnClick="btnSearch_Click" Width="78px" />
            </td>
            <td style="text-align: left;">
                <asp:Button ID="btnClose" runat="server" Text="Refresh"  Visible="false" OnClick="btnClose_Click" />
            </td>
            </tr>
            </table>
            <table class="TableMain100">
                <tr>
                    <td>
                        <asp:GridView ID="GridView1" DataKeyNames="cbd_id" runat="server" Width="100%"
                            BorderColor="CornflowerBlue" ShowFooter="True" AutoGenerateColumns="false" BorderStyle="Solid"
                            BorderWidth="2px" CellPadding="4" ForeColor="#0000C0" AllowPaging="True" OnPageIndexChanging="GridView1_PageIndexChanging">
                            <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
                            <Columns>
                                <asp:TemplateField HeaderText="cbd_id" Visible="False">
                                    <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                    <ItemTemplate>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Customer Name">
                                    <ItemStyle BorderWidth="1px" Width="200px" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                    <ItemTemplate>
                                        <asp:Label ID="lblCustomerName" runat="server" Text='<%# Eval("CustomerName")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Account Name">
                                    <ItemStyle BorderWidth="1px" Width="200px" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                    <ItemTemplate>
                                        <asp:Label ID="lblAccountName" runat="server" Text='<%# Eval("AccountName")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Account Type">
                                    <ItemStyle BorderWidth="1px" Width="200px" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                    <ItemTemplate>
                                        <asp:Label ID="lblAccountType" runat="server" Text='<%# Eval("AccountType")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Account Category">
                                    <ItemStyle BorderWidth="1px" Width="200px" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                    <ItemTemplate>
                                        <asp:Label ID="lblAccountCategory" runat="server" Text='<%# Eval("AccountCategory")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Account Number">
                                    <ItemStyle BorderWidth="1px" Width="100px" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                    <ItemTemplate>
                                        <asp:Label ID="lblAccountNumber" runat="server" Text='<%# Eval("AccountNumber")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Bank Name">
                                    <ItemStyle BorderWidth="1px" Width="200px" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                    <ItemTemplate>
                                        <asp:Label ID="lblBankName" runat="server" Text='<%# Eval("BankName")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Bank Code" Visible="false">
                                    <ItemStyle BorderWidth="1px" Width="200px" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                    <ItemTemplate>
                                        <asp:Label ID="lblBankCode" runat="server" Text='<%# Eval("cbd_bankcode")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Branch Name">
                                    <ItemStyle BorderWidth="1px" Width="200px" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                    <ItemTemplate>
                                        <asp:Label ID="lblBranchName" runat="server" Text='<%# Eval("BranchName")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="MICR No.">
                                    <ItemStyle BorderWidth="1px" Width="200px" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                    <ItemTemplate>
                                        <asp:Label ID="lblMICR" runat="server" Text='<%# Eval("MICR")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <RowStyle BackColor="White" ForeColor="#330099" BorderColor="#BFD3EE" BorderStyle="Double"
                                BorderWidth="1px"></RowStyle>
                            <EditRowStyle BackColor="#E59930"></EditRowStyle>
                            <SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>
                            <PagerStyle ForeColor="White" HorizontalAlign="Center" BackColor="SteelBlue" BorderColor="Black"></PagerStyle>
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
