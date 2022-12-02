<%@ Page Title="Industry Map" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="AssignIndustry.aspx.cs" Inherits="ERP.OMS.Management.Master.AssignIndustry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        <%--        $(document).ready(function () {
            alert('sss');
            
                var x = document.getElementById("<%=lbAvailable.ClientID %>");
                for (var i = 0; i < x.options.length; i++) {
                    if (x.options[i].selected == true) {
                        alert(x.options[i].selected);
                    }
                }
           
        });--%>
    </script>



    <script type="text/javascript">

        function GetChar(event) {
            var chCode = ('charCode' in event) ? event.charCode : event.keyCode;

            if (event.keyCode) {
                __doPostBack('<%=btnCancel.UniqueID%>', "");
            }
        }
        function ShowAvailable(name) {

            // alert('aaa')
            var a = $('#txtAvailable').val()

            if (a.length > 3) {
                __doPostBack("txtAvailable", "TextChanged");
            }
        }

        function AddSelectedItems() {
            MoveSelectedItems(lbAvailable, lbChoosen);
            UpdateButtonState();
        }
        function AddAllItems() {
            MoveAllItems(lbAvailable, lbChoosen);
            UpdateButtonState();
        }
        function RemoveSelectedItems() {
            MoveSelectedItems(lbChoosen, lbAvailable);
            UpdateButtonState();
        }
        function RemoveAllItems() {
            MoveAllItems(lbChoosen, lbAvailable);
            UpdateButtonState();
        }
        function MoveSelectedItems(srcListBox, dstListBox) {
            srcListBox.BeginUpdate();
            dstListBox.BeginUpdate();
            var items = srcListBox.GetSelectedItems();
            for (var i = items.length - 1; i >= 0; i = i - 1) {
                dstListBox.AddItem(items[i].text, items[i].value);
                srcListBox.RemoveItem(items[i].index);
            }
            srcListBox.EndUpdate();
            dstListBox.EndUpdate();
        }
        function MoveAllItems(srcListBox, dstListBox) {
            srcListBox.BeginUpdate();
            var count = srcListBox.GetItemCount();
            for (var i = 0; i < count; i++) {
                var item = srcListBox.GetItem(i);
                dstListBox.AddItem(item.text, item.value);
            }
            srcListBox.EndUpdate();
            srcListBox.ClearItems();
        }
        function UpdateButtonState() {

            //btnMoveAllItemsToRight.SetEnabled(lbAvailable.GetItemCount() > 0);
            //btnMoveAllItemsToLeft.SetEnabled(lbChoosen.GetItemCount() > 0);
            //btnMoveSelectedItemsToRight.SetEnabled(lbAvailable.GetSelectedItems().length > 0);
            //btnMoveSelectedItemsToLeft.SetEnabled(lbChoosen.GetSelectedItems().length > 0);
        }

        function CheckBoxListEdu_Init(s, e) {
            document.getElementById('divqualilist').innerHTML = '';
            //alert(document.getElementById('divqualilist').innerHTML);
            var s9 = '';
            var s1 = s.GetSelectedItems();
            var s2 = s1.length;
            var s3 = "";
            if (s2 != 0) {
                for (var i = 0; i < s2; i++) {
                    s9 = s1[i].text;
                    s3 += s9.replace(/-/g, ' ') + ", ";
                }
            }

            if (s3 != null || s3 != ', ') {
                s3 = s3.slice(0, s3.lastIndexOf(", "));
                document.getElementById('divqualilist').innerHTML = s3;
            }

        }
    </script>
    <style>
        #lbAvailable {
            min-width:352px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>
                <asp:Label ID="lblEntityType" runat="server"></asp:Label>
                - Industry Map
                <asp:Label ID="lblEntityUserName" runat="server"></asp:Label></h3>

            <div class="crossBtn">
                <asp:LinkButton ID="goBackCrossBtn" runat="server" OnClick="goBackCrossBtn_Click"><i class="fa fa-times"></i></asp:LinkButton>
                <%--<a href="frmContactMain.aspx" id="goBackCrossBtn"><i class="fa fa-times"></i></a>--%>
                <asp:HiddenField ID="hidbackPagerequesttype" runat="server" />
            </div>

        </div>
    </div>
    <div class="form_main" style="border: 1px solid #ccc; padding: 5px 15px">
        <table class="TableMain100">

            <tr>


                <td style="width: 4%">
                   <%-- <span style="font-size: 14px; display: inline-block; margin-bottom: 8px">Industry Name</span>--%>
                    <div id="divqualilist" runat="server" clientidmode="Static" style="width: 353px; font-size: xx-small; font-family: Arial; color: #0000FF; font-weight: bold;">
                       
                    </div>
                  
                     <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                    <div style="width:400px"><asp:TextBox ID="txtAvailable" runat="server" AutoPostBack="true" autocomplete="off" OnTextChanged="txtAvailable_TextChanged" onkeyup="ShowAvailable(this)" Width="353px"  se placeholder="Type Industry name to search"></asp:TextBox>
                      <span  style="font-weight:600;color:brown"> Industry (Min. 4 Char)</span>  </div>
                    <dxe:ASPxListBox ID="lbAvailable" runat="server" ClientInstanceName="lbAvailable"
                        Height="240px" SelectionMode="CheckColumn" Caption="" >
                       <%-- <ClientSideEvents SelectedIndexChanged="CheckBoxListEdu_Init" />--%>
                    </dxe:ASPxListBox>

                </td>

                <td style="padding: 100px 0px; display: none" align="center" width="10%">
                    <div>
                        <dxe:ASPxButton ID="btnMoveSelectedItemsToRight" runat="server" ClientInstanceName="btnMoveSelectedItemsToRight"
                            AutoPostBack="False" Text="Add >" CssClass="btn btn-primary btn-xs" Width="150px" ClientEnabled="False"
                            ToolTip="Add selected items">
                            <ClientSideEvents Click="function(s, e) { AddSelectedItems(); }" />
                        </dxe:ASPxButton>
                    </div>
                    <div class="TopPadding">
                        <dxe:ASPxButton ID="btnMoveAllItemsToRight" runat="server" ClientInstanceName="btnMoveAllItemsToRight"
                            AutoPostBack="False" Text="Add All >>" CssClass="btn btn-primary btn-xs" Width="150px" ToolTip="Add all items">
                            <ClientSideEvents Click="function(s, e) { AddAllItems(); }" />
                        </dxe:ASPxButton>
                    </div>
                    <div style="height: 32px">
                    </div>
                    <div>
                        <dxe:ASPxButton ID="btnMoveSelectedItemsToLeft" runat="server" ClientInstanceName="btnMoveSelectedItemsToLeft"
                            AutoPostBack="False" Text="< Remove" CssClass="btn btn-danger btn-xs" Width="150px" ClientEnabled="False"
                            ToolTip="Remove selected items">
                            <ClientSideEvents Click="function(s, e) { RemoveSelectedItems(); }" />
                        </dxe:ASPxButton>
                    </div>
                    <div class="TopPadding">
                        <dxe:ASPxButton ID="btnMoveAllItemsToLeft" runat="server" ClientInstanceName="btnMoveAllItemsToLeft"
                            AutoPostBack="False" Text="<< Remove All" CssClass="btn btn-danger btn-xs" Width="150px" ClientEnabled="False"
                            ToolTip="Remove all items">
                            <ClientSideEvents Click="function(s, e) { RemoveAllItems(); }" />
                        </dxe:ASPxButton>
                    </div>
                </td>
                <td style="width: 35%; display: none">
                    <dxe:ASPxListBox ID="lbChoosen" runat="server" ClientInstanceName="lbChoosen" Width="350px"
                        Height="240px" SelectionMode="CheckColumn" Caption="Selected Industry">
                        <CaptionSettings Position="Top" />
                        <ClientSideEvents SelectedIndexChanged="function(s, e) { UpdateButtonState(); }"></ClientSideEvents>
                    </dxe:ASPxListBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnsubmit" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnsubmit_click" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-danger" OnClick="goBackCrossBtn_Click" />






                </td>
            </tr>


        </table>
    </div>
</asp:Content>
