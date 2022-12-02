<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.management_frmAddProductBrochures" CodeBehind="frmAddProductBrochures.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../CSS/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="/assests/js/init.js"></script>
    <script type="text/javascript" src="/assests/js/ajax-dynamic-list_wofolder.js"></script>
    <style type="text/css">
        /* Big box with list of options */
        #ajax_listOfOptions {
            position: absolute; /* Never change this one */
            width: 50px; /* Width of box */
            height: auto; /* Height of box */
            overflow: auto; /* Scrolling features */
            border: 1px solid Blue; /* Blue border */
            background-color: #FFF; /* White background color */
            text-align: left;
            font-size: 0.9em;
            z-index: 100;
        }

            #ajax_listOfOptions div { /* General rule for both .optionDiv and .optionDivSelected */
                margin: 1px;
                padding: 1px;
                cursor: pointer;
                font-size: 0.9em;
            }

            #ajax_listOfOptions .optionDiv { /* Div for each item in list */
            }

            #ajax_listOfOptions .optionDivSelected { /* Selected item in the list */
                background-color: #DDECFE;
                color: Blue;
            }

        #ajax_listOfOptions_iframe {
            background-color: #F00;
            position: absolute;
            z-index: 5;
        }

        form {
            display: inline;
        }
    </style>
    <script language="javascript" type="text/javascript">
	    var ob = null;
               var obchk = null;
               var obGenral = null;

               function btnClose() {
                   window.close();
               }
               function SaveAddProduct() {
                   var ptxt, txt;
                   ptxt = window.parent.document.getElementById("txtProductBrochures");
                   txt = document.getElementById("txtProductBrochures");
                   ptxt.value = txt.value;
                   window.close();
               }
               function chkGenral(objGenral, val12) {
                   var st = document.getElementById("txtGrdContact")
                   if (obGenral == null) {
                       obGenral = objGenral;
                   }
                   else {
                       obGenral.checked = false;
                       obGenral = objGenral;
                       obGenral.checked = true;
                   }
                   st.value = val12;
               }
               function call_ajax(obj1, obj2, obj3) {
                   var set_value
                   var ob = document.getElementById("drpDocumentEntity")
                   var ob1 = document.getElementById("drpDocumentType")
                   set_value = ob.value + '-' + ob1.value;
                   ajax_showOptions(obj1, obj2, obj3, set_value)
                   //}
               }
               FieldName = 'btnSearch';
               </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table class="TableMain">
            <tr>
                <td colspan="2" class="Ecoheadtxt" style="text-align: center"><strong>Branch Information</strong></td>
            </tr>
            <tr>
                <td class="Ecoheadtxt" style="width: 17%">Document Entity</td>
                <td>
                    <asp:DropDownList ID="drpDocumentEntity" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpDocumentEntity_SelectedIndexChanged" Width="163px">
                        <asp:ListItem>Products MF</asp:ListItem>
                        <asp:ListItem>Products Insurance</asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td class="Ecoheadtxt" style="width: 17%">Document Type</td>
                <td>
                    <asp:DropDownList ID="drpDocumentType" runat="Server" Width="163px"></asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="drpDocumentType"
                        ErrorMessage="Required."></asp:RequiredFieldValidator></td>
            </tr>
            <tr>
                <td class="Ecoheadtxt" style="width: 17%">Search By Name</td>
                <td>
                    <asp:TextBox ID="txtName" runat="Server" AutoCompleteType="Disabled" Width="160px"></asp:TextBox>
                    <asp:TextBox ID="txtName_hidden" runat="Server" AutoCompleteType="Disabled" Width="160px" Visible="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="btnSearch" runat="Server" Text="Search" CssClass="btnUpdate" OnClick="btnSearch_Click" /></td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Panel ID="pnlDocuments" runat="Server" Visible="True" Width="90%">
                        <table class="TableMain">
                            <tr>
                                <td>
                                    <asp:GridView ID="grdDocuments" runat="Server" AutoGenerateColumns="false" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None" BorderWidth="1px" BorderColor="#507CD1">
                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                        <RowStyle BackColor="#EFF3FB" BorderWidth="1px" />
                                        <EditRowStyle BackColor="#2461BF" />
                                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                        <HeaderStyle Font-Bold="false" ForeColor="black" CssClass="EHEADER" BorderColor="AliceBlue" BorderWidth="1px" />
                                        <AlternatingRowStyle BackColor="White" />
                                        <RowStyle HorizontalAlign="Center" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sel">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkSel" runat="Server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDocumentId" CssClass="Ecoheadtxt" runat="Server" Text='<%# Eval("Id") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Document Type">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFileType" CssClass="Ecoheadtxt" runat="Server" Text='<%# Eval("Type") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="DocumentName">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFileName" CssClass="Ecoheadtxt" runat="Server" Text='<%# Eval("FileName") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <a href='viewImages.aspx?val=<%# Eval("Src") %>' target="_blank" class="Ecoheadtxt">View</a>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <input type="hidden" id="txtProductBrochures" name="txtProductBrochures" />
                                </td>
                            </tr>
                            <tr id="ShowButton" runat="server" visible="false">
                                <td>
                                    <input type="button" id="btnSave" name="btnSave" value="Add Product Brochures" onclick="SaveAddProduct()" class="btnUpdate" />
                                    <input type="button" id="btnClose" name="btnClose" value="Close" onclick="btnClose();" class="btnUpdate" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
