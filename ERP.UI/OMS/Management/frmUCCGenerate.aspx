<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/OMS/MasterPage/ERP.Master"  Inherits="ERP.OMS.Management.management_frmUCCGenerate" Codebehind="frmUCCGenerate.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <script type="text/javascript" language="javascript">
 var chkobj;
    var objchk=null;
    function chkclicked(obj)
    {
       if (objchk == null)
        {
            objchk = obj;
            objchk.checked = true;
        }
        else
        {
            objchk.checked = false;
            objchk = obj;
            objchk.checked = true;
        }
       
    }
    </script>
  </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
      <table style="width: 123px">
            <tr>
                <td>
                     <%ShowList(); %>
                </td>
                <td style="vertical-align:top; text-align:center">
                    <asp:Button ID="BtnAdd" runat="server" Text="Add" OnClick="BtnAdd_Click" />
                   
                </td>
            </tr>
            <tr>
                
            </tr>
        </table>
    </div>
   </asp:Content>