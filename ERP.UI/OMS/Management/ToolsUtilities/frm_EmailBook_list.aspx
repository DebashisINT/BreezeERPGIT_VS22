<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/PopUp.Master"
    Inherits="ERP.OMS.Management.ToolsUtilities.management_toolsutilities_frm_EmailBook_list" CodeBehind="frm_EmailBook_list.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="Stylesheet" href="../../CSS/style.css" type="text/css" />

    <script type="text/javascript">
        //document.onkeyup = KeyCheck;       
        document.onkeydown = KeyCheck;
        //document.onhelp = returnFalse;
        document.onkeypress = KeyCheck;

        function KeyCheck(e) {
            var KeyID = (window.event) ? event.keyCode : e.keyCode;
            //alert(KeyID);
            switch (KeyID) {
                case 27:
                    window.close();
                    break;
                    //          case 112:
                    //            alert(KeyID);
                    //            event.keyCode=0;
                    //            break;      
                    //          case 114:
                    //            alert(KeyID);
                    //            event.keyCode=0;
                    //            break;      
                    //          case 116:
                    //            alert(KeyID);
                    //            event.keyCode=0;
                    //            break;   
                    //          default:
                    //            alert(KeyID);
                    //            break; 
            }
            return false;
        }
        function cancelKeyEvent(evt) {
            evt.stopPropagation();
            if (window.createPopup) {
                evt.keyCode = 0;
            }
            else
                evt.preventDefault();
            alert(evt.keyCode);
        }

    </script>
    <style>
        #ASPxDataView1_ICell td.dxdvItem_PlasticBlue {
            background:red !important;
        }
        .dxgvHeader_PlasticBlue th{
            cursor: pointer;
            white-space: nowrap;
            padding: 7px 6px;
            border-top: 1px none #2c4182;
            border: 1px solid #2c4182;
            background: #415698 url(/DXR.axd?r=0_4426-RqHhd) repeat-x top;
            overflow: hidden;
            font-weight: normal;
            text-align: left;
            color:#fff !important;
        }
        .dxgvHeader_PlasticBlue th a{
            color:#fff !important;
        }
        #grdListData td {
            padding: 7px 6px;
        }
    </style>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <asp:GridView ID="grdListData" runat="server" BorderColor="#507CD1" BorderWidth="1px"
            CellPadding="4" ForeColor="#333333" GridLines="None" PageSize="4000" Width="100%"
            AutoGenerateColumns="false" AllowSorting="true" OnSorting="grdListData_Sorting">
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#EFF3FB" BorderWidth="1px" />
            <EditRowStyle BackColor="#2461BF" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <HeaderStyle BorderColor="AliceBlue" BorderWidth="1px" CssClass="EHEADER dxgvHeader_PlasticBlue " Font-Bold="false"
                ForeColor="black" />
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:TemplateField HeaderText="Email ID">
                    <ItemTemplate>
                        <label></label><asp:Label ID="lblName" runat="server" Text='<%# Eval("Email")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Type" SortExpression="type">
                    <ItemTemplate>
                        <asp:Label ID="lblType" runat="server" Text='<%# Eval("type")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Name" SortExpression="name">
                    <ItemTemplate>
                        <asp:Label ID="lblName" runat="server" Text='<%# Eval("name")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Code" SortExpression="code">
                    <ItemTemplate>
                        <asp:Label ID="lblcode" runat="server" Text='<%# Eval("code")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Company" SortExpression="comp">
                    <ItemTemplate>
                        <asp:Label ID="lblcomp" runat="server" Text='<%# Eval("comp")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Branch" SortExpression="branch">
                    <ItemTemplate>
                        <asp:Label ID="lblbranch" runat="server" Text='<%# Eval("branch")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
