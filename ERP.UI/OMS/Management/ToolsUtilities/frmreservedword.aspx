<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/Popup.Master"
     Inherits="ERP.OMS.Management.ToolsUtilities.management_utilities_frmreservedword" Codebehind="frmreservedword.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" language="javascript">
    
//        function setvalue(obj,obj2)
//        {
//        alert("111");
//            alert(obj + '_' + obj2);
////            var textfield = window.opener.document.aspnetForm.ctl00_ContentPlaceHolder3_txtMessageHeader;
////            textfield.value = textfield.value + '< ' + obj + '>';
//        }
        
        function PostReservedWord(obj)
        {
            //parent["txt_msg"].SetText("NewText");
            //window.opener.document.getElementById("txt_msg").value =   window.opener.document.getElementById("txt_msg").value +  '< ' + obj + '>';      

            var txt =window.parent.parent.popup.GetContentIFrame().contentWindow.document.getElementById('txt_msg');

           // window.opener.document.getElementById('txt_msg').value = "1111111111111";
            //document.getElementById("ctl00$ContentPlaceHolder1$txt_msg").value = "1111111111111";
            //window.opener.document.getElementById("txt_msg").value = "";
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="Div" runat="server">
      <%--<input style="border-right: 1px groove; border-top: 1px groove; font-size: 8pt;  border-left: 1px groove; color: black; border-bottom: 1px groove; font-style: normal; font-family: Verdana; width: 125px" onclick="window.opener.document.aspnetForm.txt_msg.value=window.opener.document.aspnetForm.txt_msg.value+'< '+this.value+'>';" type="button" id="chk" name="chk" value="Recipient Address">--%>
      <%--<input style="border-right: 1px groove; border-top: 1px groove; font-size: 8pt;  border-left: 1px groove; color: black; border-bottom: 1px groove; font-style: normal; font-family: Verdana; width: 125px" onclick="PostReservedWord(this.value);" type="button" id="Button1" name="chk" value="Recipient Address">--%>
    </div>
</asp:Content>
