<%@ Page Language="C#" AutoEventWireup="true" Inherits="SignOff" Codebehind="SignOff.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>  
    <script language="javascript" type="text/javascript">
        window.history.forward();  
         function noBack() { window.history.forward(); }         
    </script>
</head>
<body  onload="noBack();" onpageshow="if (event.persisted) noBack();" onunload="">
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
</body>
</html>
