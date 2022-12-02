<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.Master.management_master_Showdocument" Codebehind="Showdocument.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    

    

    <!--___________________These files are for List Items__________________________-->

    <script type="text/javascript" src="/assests/js/init.js"></script>

    <script type="text/javascript" src="/assests/js/ajax-dynamic-list.js"></script>

    <!--___________________________________________________________________________-->

    <script type="text/javascript" language="javascript">
        function CallList(obj1,obj2,obj3)
        {
            var obj4=document.getElementById("drpDocumentEntity");
            var obj5=document.getElementById("drpDocumentType");
            var obj6=document.getElementById("chkSearch");
            var set_val = obj4.value + '~' + obj5.value+ '~' + obj6.checked;
            ajax_showOptions(obj1,obj2,obj3,set_val);
        }
        FieldName='btnSearch';

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
	
	form{
		display:inline;
	}
	
	</style>
    </asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
          <table width="100%">
                <tr>
                    <td class="EHEADER" style="text-align: center">Document
                    </td>
                </tr>
            </table>
        <table class="TableMain100">
         
            <tr>
                <td colspan="2" align="Center">
                    <asp:Label ID="lblError" runat="Server" ForeColor="red"></asp:Label></td>
            </tr>
            <tr>
                <td class="Ecoheadtxt" style="width: 43%">
                    Document Entity</td>
                <td style="text-align: left">
                    <asp:DropDownList ID="drpDocumentEntity" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpDocumentEntity_SelectedIndexChanged"
                        Width="250px">
                        <asp:ListItem>Products MF</asp:ListItem>
                        <asp:ListItem>Products Insurance</asp:ListItem>
                        <asp:ListItem>Products IPOs</asp:ListItem>
                        <asp:ListItem>Customer/Client</asp:ListItem>
                        <asp:ListItem>Lead</asp:ListItem>
                        <asp:ListItem>Employee</asp:ListItem>
                        <asp:ListItem>Sub Brokers</asp:ListItem>
                        <asp:ListItem>Franchisees</asp:ListItem>
                        <asp:ListItem>Data Vendors</asp:ListItem>
                        <asp:ListItem>Referral Agents</asp:ListItem>
                        <asp:ListItem>Recruitment Agents</asp:ListItem>
                        <asp:ListItem>AMCs</asp:ListItem>
                        <asp:ListItem>Insurance Companies</asp:ListItem>
                        <asp:ListItem>RTAs</asp:ListItem>
                        <asp:ListItem>Branches</asp:ListItem>
                        <asp:ListItem>Companies</asp:ListItem>
                        <asp:ListItem>Building</asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td class="Ecoheadtxt" style="width: 43%">
                    Document Type</td>
                <td style="text-align: left">
                    <asp:DropDownList ID="drpDocumentType" runat="Server" Width="250px">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td class="Ecoheadtxt" style="width: 43%">
                    Search By Document Name</td>
                <td style="text-align: left">
                    <asp:CheckBox ID="chkSearch" runat="server"></asp:CheckBox></td>
            </tr>
            <tr>
                <td class="Ecoheadtxt" style="width: 43%">
                    Search By Name</td>
                <td style="text-align: left">
                    <asp:TextBox ID="txtName" runat="Server" AutoCompleteType="Disabled" Width="244px"></asp:TextBox>
                    <asp:TextBox ID="txtName_hidden" runat="Server" Visible="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <asp:Button ID="btnSearch" runat="Server" Text="Search" OnClick="btnSearch_Click" /></td>
            </tr>
            <tr>
                <td colspan="2" align="center" style="width: 100%">
                    <asp:Panel ID="pnlDocuments" runat="Server" Visible="True">
                        <table width="100%">
                            <tr>
                                <td style="width: 100%">
                                    <asp:GridView ID="grdDocuments" runat="Server" AutoGenerateColumns="false" BorderColor="blue"
                                        BackColor="White" BorderStyle="solid" BorderWidth="2px" CellPadding="4" Width="100%">
                                        <RowStyle BackColor="LightSteelBlue" ForeColor="#330099"></RowStyle>
                                        <SelectedRowStyle BackColor="LightSteelBlue" ForeColor="SlateBlue" Font-Bold="True">
                                        </SelectedRowStyle>
                                        <PagerStyle BackColor="LightSteelBlue" ForeColor="SlateBlue" HorizontalAlign="Center">
                                        </PagerStyle>
                                        <HeaderStyle BackColor="LightSteelBlue" ForeColor="black" Font-Bold="True"></HeaderStyle>
                                        <EditRowStyle BorderColor="blue" />
                                        <FooterStyle BackColor="White" ForeColor="LightSteelBlue" />
                                        <Columns>
                                            <asp:TemplateField Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDocumentId" runat="Server" Text='<%# Eval("Id") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="DocumentType">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFileType" runat="Server" Text='<%# Eval("Type") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="DocumentName">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFileName" runat="Server" Text='<%# Eval("FileName") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Document Physical Location">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFilePath" runat="Server" Text='<%# Eval("FilePath") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <a href='viewImage.aspx?id=<%# Eval("Src") %>' target="_blank">View</a>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
</asp:Content>