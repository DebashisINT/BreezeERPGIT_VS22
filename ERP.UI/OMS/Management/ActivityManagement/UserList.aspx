<%--<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.ActivityManagement.management_activitymanagement_UserList" CodeBehind="UserList.aspx.cs" %>--%>
<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/PopUp.Master" Inherits="ERP.OMS.Management.ActivityManagement.management_activitymanagement_UserList" CodeBehind="UserList.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<%--    <link href="../../CSS/style.css" rel="stylesheet" type="text/css" />--%>
    <script language="javascript" type="text/javascript">
        function CloseWindow1() {
            //changed by sanjib to make ready this page as good condition 13122016

            //var ob = parent.document.getElementById("IFRAME_ForAllPages").contentWindow.GetUserList();
            //var ob1 = parent.document.getElementById("IFRAME_ForAllPages").contentWindow.GetHiddenUserList();

            var ob = parent.document.getElementById("txtUserList");
            var ob1 = parent.document.getElementById("hd1UserList");
            var buttonsub = parent.document.getElementById("btnSubmit").disabled = true;
            var ob2 = document.getElementById("txtUserList");
            var ob3 = document.getElementById("hd1User");
            ob.value = ob2.value;
            ob1.value = ob3.value;

            var buttonsub = parent.document.getElementById("btnSubmit");
            buttonsub.disabled = false;
            parent.editwin.close();

            //end

        }
        function checkevent(chkobj) {
            if (chkobj.checked == true) {
                var st = chkobj.value.split("~");
                var ob = document.getElementById("txtUserList");
                var ob1 = document.getElementById("hd1User");
                //alert(st[1]);
                //alert(st[0]);
                if (ob.value == "") {
                    ob.value = st[1];
                    ob1.value = st[0];
                }
                else {
                    ob.value = ob.value + "," + st[1];
                    ob1.value = ob1.value + "," + st[0];
                }
            }
            else {
                var ob = document.getElementById("txtUserList");
                var ob1 = document.getElementById("hd1User");
                if (ob.value != "") {
                    var st = chkobj.value.split("~");
                    var obj12 = ob.value.split(",");
                    var obj123 = ob1.value.split(",");
                    var temp1 = "";
                    var temp2 = "";
                    for (i = 0 ; i < obj12.length; i++) {
                        if (obj12[i] == st[1]) {
                        }
                        else {
                            if (temp1 != "") {
                                temp1 = temp1 + "," + obj12[i];
                                temp2 = temp2 + "," + obj123[i];
                            }
                            else {
                                temp1 = obj12[i];
                                temp2 = obj123[i];
                            }
                        }
                    }
                    ob.value = temp1;
                    ob1.value = temp2;
                }
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table>
            <tr>
                <td>
                    <asp:Button ID="btnSelectAll" runat="server" Text="Select All" CssClass="btnUpdate"
                        OnClick="btnSelectAll_Click" />
                    <asp:HiddenField ID="txtUserList" runat="server" />
                    <asp:HiddenField ID="hd1User" runat="server" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
