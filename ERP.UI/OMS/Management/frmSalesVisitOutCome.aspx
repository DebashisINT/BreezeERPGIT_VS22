<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"  Inherits="ERP.OMS.Management.management_frmSalesVisitOutCome" Codebehind="frmSalesVisitOutCome.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script language="javascript" type="text/javascript">
        function checkevent(Obj,chkObj,txtdate,txttime,nowdate,nowtime,cdate,ctime)
        {  
            var st = chkObj.value.split("~")            
            var ob = window.opener.document.getElementById(Obj) 
            var ob1 = window.opener.document.getElementById("TxtOut")
            ob.value = st[1];  
            ob1.value = st[0];
            window.opener.document.getElementById("btnSavePhoneCallDetails").disabled=false;
            window.opener.Changedata123(txtdate,txttime,nowdate,nowtime,cdate,ctime); 
            window.close(); 
            
        }
        function lostFocus()
        {
            var str = window.opener.document.getElementById("txtNote");
            str.focus();
        }
    </script>
    </asp:Content>
   <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
    </div>
       </asp:Content>
