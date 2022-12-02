<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.DailyTask.Management_DailyTask_InvTransactionPrintForm" Codebehind="InvTransactionPrintForm.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        function fn_PrintPage() {
            var printButton = document.getElementById("PrintPage");
            printButton.style.visibility = 'hidden';
            window.print()
            printButton.style.visibility = 'visible';
        }
    </script>
    <style type="text/css">
        .dxbBtn input
        {
            padding: 3px;
            height:20px;
        }
          html, body
            {
                 
                font-family: "Arial Rounded MT", Arial, Helvetica, sans-serif;
                
                
            }
         .tdr
         {
         	text-align:left;
         	
         	}
    </style>
    
   
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="height:50px;">
        <input type="button" id="PrintPage" name="PrintPage" value="Print" onclick="fn_PrintPage();" class="dxbBtn"  />
        
        </div>
    <asp:Repeater ID="RepDetails" runat="server" OnItemDataBound="RepDetails_ItemDataBound">
        <ItemTemplate>
            <div style="display: inline-block; vertical-align: top; overflow:hidden; padding:15px; 
                 border:2px dashed #000" id="dvlbl" runat="server"> <%-- 40% 395px--%>
                <div style="text-align:center;padding-bottom:3%; font-weight:bold; font-size:13px;">
                    <div>
                        <%--<asp:Label ID="lblHeader" Text='<%#Eval("ContactName") %>' runat="server"></asp:Label>--%>
                        <br />
                    </div>
                    <div>
                        <asp:Label ID="lblOderNo" Text='<%#Eval("OrderRefNo") %>' runat="server"></asp:Label>
                        <%--<%#Eval("OrderNo") %>--%>
                    </div>
                </div>
                <table border="1" style="width: 100%; height: 200px; border-collapse: collapse; border-color:black; font-weight:bold;">
                    <thead>
                        <tr>
                            <td colspan="2" style="text-align: center;">
                                Packing Tag
                            </td>
                        </tr>
                    </thead>
                    <tbody style="text-align: center;">
                        <tr>
                            <td>
                                Pc. No.
                            </td>
                            <td class="tdr">
                                <asp:Label ID="lblPcNo" Text='<%#Eval("Inventory_PieceNo") %>' runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Col. No.
                            </td>
                            <td class="tdr">
                                <asp:Label ID="lblColNo" Text='<%#Eval("ColorNo") %>' runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Quality
                            </td>
                            <td class="tdr">
                                <asp:Label ID="lblQuantity" Text='<%#Eval("Quality") %>' runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Design No.
                            </td>
                            <td class="tdr">
                                <asp:Label ID="lblDesignNo" Text='<%#Eval("DesignNo") %>' runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Width
                            </td>
                            <td class="tdr">
                                <asp:Label ID="lblWidth" Text='<%#Eval("Width") %>' runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Lenght
                            </td>
                            <td class="tdr">
                                <asp:Label ID="Label5" Text='<%#Eval("PLength") %>' runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                weight
                            </td>/
                            <td class="tdr">
                                <asp:Label ID="lblWeight" Text='<%#Eval("Remarks") %>' runat="server"></asp:Label>
                            </td>
                        </tr>
                         <tr>
                            <td>
                                PO NO
                            </td>
                            <td class="tdr">
                                <asp:Label ID="Label1" Text='<%#Eval("OrderRefNo") %>' runat="server"></asp:Label>
                            </td>
                        </tr>
                         <tr>
                            <td>
                                LOT NO
                            </td>
                            <td class="tdr">
                                <asp:Label ID="Label2" Text='<%#Eval("ParentOrderRefNo") %>' runat="server"></asp:Label>
                            </td>
                        </tr>
                         <tr>
                            <td>
                                 INVOICE NO
                            </td>
                            <td class="tdr">
                                <asp:Label ID="Label3" Text='<%#Eval("INVOICE_NO") %>' runat="server"></asp:Label>
                            </td>
                        </tr>
                    </tbody>
                    <tfoot>
                        <tr>
                            <td colspan="2" colspan="2" style="text-align: center;">
                                MADE IN INDIA
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" colspan="2" style="text-align: center;">
                                <asp:Image ID="imgBarCode" Width="180" Height="50" runat="server"   />
                               
                            </td>
                        </tr>
                    </tfoot>
                </table>
            </div>
        </ItemTemplate>
    </asp:Repeater>
     <asp:Label ID="lblError" runat="server"></asp:Label>
      <asp:Label ID="lblError1" runat="server"></asp:Label>
 </asp:Content>