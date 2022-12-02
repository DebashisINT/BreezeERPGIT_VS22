<%@ Page title="Industry Map" Language="C#" AutoEventWireup="true" CodeBehind="IndustryMap.aspx.cs" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.Master.IndustryMap" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
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
            btnMoveAllItemsToRight.SetEnabled(lbAvailable.GetItemCount() > 0);
            btnMoveAllItemsToLeft.SetEnabled(lbChoosen.GetItemCount() > 0);
            btnMoveSelectedItemsToRight.SetEnabled(lbAvailable.GetSelectedItems().length > 0);
            btnMoveSelectedItemsToLeft.SetEnabled(lbChoosen.GetSelectedItems().length > 0);
        }

function Error()
{
    //alert('Please Select a name');
    return false;

}




    </script>
    <style>
        .lbsd {
            float:left;
            margin-right:20px;
        }
        
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Industry Map <asp:Literal ID="ltrName" runat="server"></asp:Literal></h3>
           
             <div class="crossBtn"><a href="industrySector.aspx"><i class="fa fa-times"></i></a></div>
        </div>
    </div>
    <div class="form_main" style="border: 1px solid #ccc; padding: 5px 15px">
        <table class="TableMain100">
            <tr>
                <td style="width: 90px" valign="top" >
                    <dxe:ASPxLabel ID="ASPxLabel1" runat="server" Text="Entity Type" CssClass="lbsd">
                    </dxe:ASPxLabel>
                    
                 
                     


                </td>
                <td colspan="2"></td>
            </tr>
            <tr>
                <td>
                    <dxe:ASPxComboBox ID="cmbContactType" runat="server" AutoPostBack="true"
                            Font-Bold="False" ForeColor="black" OnSelectedIndexChanged="cmbContactType_SelectedIndexChanged"
                            ValueType="System.Int32" Width="353px" CssClass="pull-left">
                          
                        </dxe:ASPxComboBox>
                      
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="txtAvailable" runat="server"  AutoPostBack="true"  OnTextChanged="txtAvailable_TextChanged" Width="353px" placeholder="Search"></asp:TextBox>
                </td>
            </tr>
            <tr>

              
            <td style="width: 35%">
                <span style="font-size:14px;margin-bottom:8px; display:inline-block">Name</span>
                 <dxe:ASPxListBox ID="lbAvailable" runat="server" ClientInstanceName="lbAvailable"
                    Width="350px" Height="240px" SelectionMode="CheckColumn" Caption="">
                
                </dxe:ASPxListBox>
            
            </td>
            <td style="padding: 40px 60px;display:none;">
                <div>
                    <dxe:ASPxButton ID="btnMoveSelectedItemsToRight" runat="server" ClientInstanceName="btnMoveSelectedItemsToRight"
                        AutoPostBack="False" Text="Add >" Width="135px" CssClass="btn btn-primary" ClientEnabled="False"
                        ToolTip="Add selected items">
                        <ClientSideEvents Click="function(s, e) { AddSelectedItems(); }" />
                    </dxe:ASPxButton>
                </div>
                <div class="TopPadding">
                    <dxe:ASPxButton ID="btnMoveAllItemsToRight" runat="server" ClientInstanceName="btnMoveAllItemsToRight"
                        AutoPostBack="False" Text="Add All >>" Width="135px" CssClass="btn btn-primary" ToolTip="Add all items">
                        <ClientSideEvents Click="function(s, e) { AddAllItems(); }" />
                    </dxe:ASPxButton>
                </div>
                <div style="height: 32px">
                </div>
                <div>
                    <dxe:ASPxButton ID="btnMoveSelectedItemsToLeft" runat="server" ClientInstanceName="btnMoveSelectedItemsToLeft"
                        AutoPostBack="False" Text="< Remove" Width="135px" CssClass="btn btn-danger" ClientEnabled="False"
                        ToolTip="Remove selected items">
                        <ClientSideEvents Click="function(s, e) { RemoveSelectedItems(); }" />
                    </dxe:ASPxButton>
                </div>
                <div class="TopPadding">
                    <dxe:ASPxButton ID="btnMoveAllItemsToLeft" runat="server" ClientInstanceName="btnMoveAllItemsToLeft"
                        AutoPostBack="False" Text="<< Remove All" Width="135px" CssClass="btn btn-danger" ClientEnabled="False"
                        ToolTip="Remove all items">
                        <ClientSideEvents Click="function(s, e) { RemoveAllItems(); }" />
                    </dxe:ASPxButton>
                </div>
            </td>
            <td style="width: 35%;display:none;">
                <dxe:ASPxListBox ID="lbChoosen" runat="server" ClientInstanceName="lbChoosen"
                   Width="350px" Height="240px"  SelectionMode="CheckColumn" Caption="Selected Name">
                    <CaptionSettings Position="Top" />
                    <ClientSideEvents SelectedIndexChanged="function(s, e) { UpdateButtonState(); }">
                    </ClientSideEvents>
                </dxe:ASPxListBox>
            </td>
        </tr>
            <tr><td colspan="3">
                  <asp:Button ID="btnsubmit" runat="server" OnClick="btnsubmit_click" Text="Save" CssClass="btn btn-primary" />
                 <asp:Button ID="btnCancel" runat="server"  OnClick="btncancel_click" Text="Cancel" CssClass="btn btn-danger" />
                </td></tr>
          
        
        </table>
    </div>
</asp:Content>
