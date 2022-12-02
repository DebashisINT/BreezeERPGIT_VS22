<%@ Page Language="C#" AutoEventWireup="true"  EnableEventValidation="false" MasterPageFile="~/OMS/MasterPage/ERP.Master"  CodeBehind="AdjustReturnFinanceBill.aspx.cs" Inherits="ERP.OMS.Management.Activities.AdjustReturnFinanceBill" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    </asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

     <div class="form_main">

         <table>
             <tr>
                 <td>
                     Sales Return Number
                 </td>
                 <td>
                     
                      <asp:TextBox ID="txtReturnNo" runat="server"></asp:TextBox>
                 </td>

             </tr>
              <tr>
                  <td>
                      Post Financer's value
                  </td>
                  <td>

                      <asp:DropDownList ID="ddlFinance" runat="server">
                          <asp:ListItem Value="-1">Select</asp:ListItem>
                          <asp:ListItem Value="1">Yes</asp:ListItem>
                          <asp:ListItem Value="2">No</asp:ListItem>
                      </asp:DropDownList>

                  </td>

              </tr>

              <tr>
                  <td colspan="2">

                 

                      <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Submit" />

                 

                  </td>

              </tr>
         </table>
         </div>
        </asp:Content>