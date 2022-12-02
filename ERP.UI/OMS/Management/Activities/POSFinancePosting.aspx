<%@ Page Language="C#" AutoEventWireup="true"  EnableEventValidation="false" MasterPageFile="~/OMS/MasterPage/ERP.Master"  CodeBehind="POSFinancePosting.aspx.cs" Inherits="ERP.OMS.Management.Activities.POSFinancePosting" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

      <style type="text/css">
          .auto-style1 {
              width: 50px;
          }
          .auto-style2 {
              width: 994px;
          }
          .auto-style3 {
              width: 50px;
              height: 50px;
          }
          .auto-style4 {
              width: 994px;
              height: 293px;
          }
          .auto-style5 {
              height: 150px;
          }
          .auto-style6 {
              width: 108px;
          }
          .auto-style7 {
              width: 201px;
          }
      </style>


    <script>

        function ClearMessage() {
            $('#<%=lblMessage.ClientID%>').html(""); 
        }

</script>
    </asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

      <div class="panel-heading">
        <div class="panel-title">
            <h3>POS Finance Posting
            </h3>
          
          
        </div>
          </div>
     <div class="form_main">

         <table style="width: 100%;margin-top: 10px;">

                <tr  id="trmsgid" runat="server">
                           
                    <td colspan="3"> <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label></td>
                </tr>
                          
                <tr>
                    <td class="auto-style6">Document Number</td>
                    <td class="auto-style7"><asp:TextBox ID="txtReturnNo" runat="server" ></asp:TextBox></td>
                    <td style="padding-left:20px"  > <asp:RequiredFieldValidator ID="ReqRetNo" runat="server" ErrorMessage="Required" ControlToValidate="txtReturnNo" ValidationGroup="FinAdjust"></asp:RequiredFieldValidator></td>
                </tr>
                <tr>
                    <td class="auto-style6"> Post Financer </td>
                      <td colspan="2" >
                    <asp:Button ID="btnSubmit" CssClass="btn btn-primary" runat="server" OnClientClick="javascript:ClearMessage(); "  OnClick="btnSubmit_Click" Text="Submit"  ValidationGroup="FinAdjust"/>

                    </td>
                   <%-- <td class="auto-style7">    <asp:DropDownList ID="ddlFinance" runat="server" width="100%">
                <asp:ListItem Value="-1">Select</asp:ListItem>
                <asp:ListItem Value="1">Yes</asp:ListItem>
                <asp:ListItem Value="2">No</asp:ListItem>
            </asp:DropDownList></td>
                    <td style="padding-left:20px" > <asp:RequiredFieldValidator ID="ReqddlFinance" InitialValue="-1" runat="server" ErrorMessage="Required" ControlToValidate="ddlFinance" ValidationGroup="FinAdjust"></asp:RequiredFieldValidator>
                    </td>--%>
                </tr>
               <%-- <tr>
                <td></td>
                    <td colspan="2" >
                    <asp:Button ID="btnSubmit" CssClass="btn btn-primary" runat="server"  OnClick="btnSubmit_Click" Text="Submit"  ValidationGroup="FinAdjust"/>

                    </td>
                </tr>--%>
            </table>
         </div>
        </asp:Content>