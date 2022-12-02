<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.Management_InterestSchemeMaster" CodeBehind="InterestSchemeMaster.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%-- <%@ Register Assembly="DevExpress.Web.v15.1" Namespace="DevExpress.Web"
    TagPrefix="dxcp" %>
<%@ Register assembly="DevExpress.Web.v15.1" namespace="DevExpress.Web" tagprefix="dx" %>--%>
    <script type="text/javascript">
        function height() {
            if (document.body.scrollHeight >= 500)
                window.frameElement.height = document.body.scrollHeight;
            else
                window.frameElement.height = '600px';
            window.frameElement.Width = document.body.scrollWidth;
        }
        function ShowPopUp() {
            // alert("Shown");
            document.getElementById("divOverlapping").style.display = 'block';
            document.getElementById("divPopUp").style.display = 'block';
        }
        function ClosePopUp() {
            document.getElementById("divOverlapping").style.display = 'none';
            document.getElementById("divPopUp").style.display = 'none';
        }
        function DeleteConfirmation() {

            var i = window.confirm("Are you sure to delete this record?");
            if (i == true) {
                var j = window.confirm("Are you sure to delete this record?");
                if (j == true) {
                    var k = window.confirm("Are you sure to delete this record?");
                    if (k == true) {
                        return true;
                    }
                }
            }
            return false;
        }
        function CallingCBDelete(v) {

            v += '*delete';
            var is = DeleteConfirmation();
            // alert(is);
            if (is == true) {
                ccbpnlTest.PerformCallback(v);
            }
        }
        function CallingCB(v) {
            alert(v);
            // document.getElementById("hdnSaveId").value=v;
            if (v == 0) {
                alert(document.getElementById("hdnSaveId").value);
                ccbpnlTest.PerformCallback("Save");
            }
            //        else 
            //        {
            //           v+='*delete';
            //         var is=DeleteConfirmation();
            //        // alert(is);
            //            if (is==true)
            //            {
            //              ccbpnlTest.PerformCallback(v);
            //            }
            //        }
            return false;
        }
        function PostDeleteMessage() {
            alert('Data deleted successfully');
            __doPostBack();

        }
        function Pnl_EndCallback() {

            try {
                alert('1');
                //  alert(ccbpnlTest.cpStatus) ;

                if (ccbpnlTest.cpStatus == "Saved") {
                    clearFields();
                    alert("Data saved successfully");
                    ClosePopUp();
                    __doPostBack();
                }
                else if (ccbpnlTest.cpStatus == "DuplicateCode") {
                    alert("Interest code already exists");
                }
                else if (ccbpnlTest.cpStatus == "DefaultAlreadyExists") {
                    alert("Default already exists");
                }
                else if (ccbpnlTest.cpStatus.indexOf("*") != -1 && ccbpnlTest.cpStatus.split('*')[0] == "Show") {
                    document.getElementById("txtIntSchemeCode").value = ccbpnlTest.cpStatus.split('*')[1];
                    document.getElementById("txtIntSchemeName").value = ccbpnlTest.cpStatus.split('*')[2];
                    //  document.getElementById("hdnSaveId").value="";
                    ShowPopUp();
                }
                else if (ccbpnlTest.cpStatus == "Deleted") {
                    alert('Data deleted successfully');
                    // __doPostBack();
                }
            }
            catch (ex) {
                alert(ex);
            }

        }
        function clearFields() {
            document.getElementById("txtIntSchemeCode").disabled = false;
            document.getElementById("txtIntSchemeCode").value = "";
            document.getElementById("txtIntSchemeName").value = "";
        }
        function height() {

            if (document.body.scrollHeight >= 500)
                window.frameElement.height = document.body.scrollHeight;
            else
                window.frameElement.height = '600px';
            window.frameElement.Width = document.body.scrollWidth;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="content">
        <div style="font-size: medium; font-weight: bold; padding-left: 40%; padding-top: -5px; padding-bottom: 5px;">
            Interest Scheme  Master
        </div>
        <div style="font-size: small;">
            <asp:GridView ID="gvInresetMaster" runat="server" AutoGenerateColumns="false" Width="98%" PageSize="10" AllowPaging="true" OnPageIndexChanged="gvInresetMaster_PageIndexChanged" OnPageIndexChanging="gvInresetMaster_PageIndexChanging" OnRowCommand="gvInresetMaster_RowCommand" HeaderStyle-BackColor="#B7CEEC" EmptyDataRowStyle-Font-Size="Large">
                <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            Code
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%#Eval("IntScheme_Code")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            Name
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%#Eval("IntScheme_Name")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            Is Default Value
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%#Eval("IntScheme_Default")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <%-- <asp:LinkButton  ID="lbtnAdd" Text="Add New" runat="server" OnClientClick="clearFields();ShowPopUp();return false;"></asp:LinkButton>--%>
                            <asp:LinkButton ID="lbtnAdd" Text="Add New" runat="server" CommandArgument='<%#Eval("IntScheme_ID")%>' CommandName="ADD"></asp:LinkButton>

                        </HeaderTemplate>
                        <ItemTemplate>
                            <%--  <asp:LinkButton  ID="lbtnInfo" Text="More Info......" runat="server"  CommandArgument='<%#Eval("IntScheme_ID")%>' OnClientClick='<%# Eval("IntScheme_ID", "javascript:return CallingCB({0});") %>' CommandName="info"></asp:LinkButton>--%>
                            <asp:LinkButton ID="lbtnInfo" Text="More Info......" runat="server" CommandArgument='<%#Eval("IntScheme_ID")%>' CommandName="ed"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            Delete
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%-- <asp:LinkButton  ID="lbtnDelete" Text="Delete" runat="server" OnClientClick='<%# Eval("IntScheme_ID", "javascript:CallingCB({0});") %>' ></asp:LinkButton>--%>
                            <asp:LinkButton ID="lbtnDelete" Text="Delete" runat="server" CommandArgument='<%#Eval("CanDeletablePlusId")%>' CommandName="del" OnClientClick="return DeleteConfirmation();"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>
                <EmptyDataTemplate>
                    No Record Found.Click 
                    <asp:LinkButton ID="lbtnAdds" ForeColor="blue" Text="here" runat="server" OnClientClick="clearFields();ShowPopUp();return false;"></asp:LinkButton>
                    to add new record
                </EmptyDataTemplate>
            </asp:GridView>
        </div>
        <dxe:ASPxCallbackPanel ID="cbpnlTest" runat="server" ClientInstanceName="ccbpnlTest" OnCallback="CallBack">

            <ClientSideEvents EndCallback="function(s, e)  {Pnl_EndCallback();}" />
        </dxe:ASPxCallbackPanel>
    </div>
    <div id="divOverlapping" style="position: fixed; height: 100%; width: 100%; background-color: #000; top: 0px; left: 0px; opacity: 0.5; filter: alpha(opacity=50); z-index: 60; display: none;">
    </div>
    <div id="divPopUp" style="background-color: #B7CEEC; height: 200px; left: 481px; position: absolute; top: 25px; width: 640px; left: 90px; z-index: 100; display: none; padding-bottom: 10px;">
        <div style="background-color: #B7CEEC; height: 7px; font-size: small; color: White; padding-bottom: 7px; padding-left: 0px;">
            <div style="background-color: Black; font-weight: bold; font-size: medium;">
                Interest Scheme Form
              
             <%-- <img  src="../windowfiles/close.gif" height="16px" alt="CLOSE" onclick="ClosePopUp();" style="padding-left:98%; margin: 0px;"  />--%>
                <asp:ImageButton ID="ibtnClosing" runat="server" ImageUrl="../windowfiles/close.gif" OnClick="ibtn_Close" Width="20px" Style="padding-left: 97%; margin: 0px; padding-top: -25px; top: -10px;" />
            </div>
            <div style="background-color: #B7CEEC; color: Black;">

                <table cellspacing="5" style="background-color: #B7CEEC;">
                    <tr>
                        <td align="right">
                            <b>Interest Scheme Code</b>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtIntSchemeCode" runat="server" Width="300" MaxLength="10" ValidationGroup="vg"></asp:TextBox>
                        </td>
                        <td align="left">
                            <asp:RequiredFieldValidator ID="reqCode" runat="server" Display="Dynamic" ControlToValidate="txtIntSchemeCode" ErrorMessage="Enter Interest Scheme Code" ForeColor="yellow" Font-Bold="true" ValidationGroup="vg">
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <b>Interest Scheme Name</b>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtIntSchemeName" runat="server" Width="300" MaxLength="50" ValidationGroup="vg"></asp:TextBox>
                        </td>
                        <td align="left">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="Dynamic" ControlToValidate="txtIntSchemeName" ErrorMessage="Enter Interest Scheme Code" ForeColor="yellow" Font-Bold="true" ValidationGroup="vg">
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <b>Is Default Scheme ?</b>
                        </td>
                        <td align="left" colspan="2">
                            <asp:RadioButtonList ID="rbtnlstIsDefault" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Value="1">Yes</asp:ListItem>
                                <asp:ListItem Value="0" Selected="True">No</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:HiddenField ID="hdnSaveId" runat="server" Value="0" />
                        </td>
                        <td colspan="2" align="left">
                            <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" ValidationGroup="vg" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</asp:Content>
