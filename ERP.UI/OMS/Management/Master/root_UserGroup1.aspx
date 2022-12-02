<%@ Page Language="C#" AutoEventWireup="True"
    Inherits="ERP.OMS.Management.Master.management_master_root_UserGroup1" MasterPageFile="~/OMS/MasterPage/ERP.Master" CodeBehind="root_UserGroup1.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {

            $(".water").each(function () {
                if ($(this).val() == this.title) {
                    $(this).addClass("opaque");
                }
            });

            $(".water").focus(function () {
                if ($(this).val() == this.title) {
                    $(this).val("");
                    $(this).removeClass("opaque");
                }
            });

            $(".water").blur(function () {
                if ($.trim($(this).val()) == "") {
                    $(this).val(this.title);
                    $(this).addClass("opaque");
                }
                else {
                    $(this).removeClass("opaque");
                }
            });
        });

    </script>

    <script language="javascript" type="text/javascript">
        var chkobj;
        var objchk = null;
        function chkclicked(obj, msg12) {
            if (objchk == null) {
                objchk = obj;
                objchk.checked = true;
            }
            else {
                objchk.checked = false;
                objchk = obj;
                objchk.checked = true;
            }

        }
        function callList(obj1, obj2, obj3) {
            //alert('eee');
            var obj4 = document.getElementById("btnGetRelatedUser_hidden");
            var obj5 = obj4.value;
            alert(obj5);
            ajax_showOptions(obj1, obj2, obj3, obj5);
            //alert('fff');
        }
        FieldName = 'btnSave';
        function client_OnTreeNodeChecked() {
            var obj = window.event.srcElement;
            //alert(obj.tagName+'='+obj.type);
            var treeNodeFound = false;
            var checkedState;
            if (obj.tagName == "INPUT" && obj.type == "checkbox") {
                var treeNode = obj;
                checkedState = treeNode.checked;
                if (treeNode.parentElement != null) {
                    var ggg = treeNode.parentElement;
                    alert('yyyy-' + ggg.type + '--' + checkedState);
                    var hh = ggg.getElementsByTagName("INPUT");
                    alert(hh[0].node);
                    hh[0].checked = checkedState;
                    alert(hh[0].checked);
                }
                do {
                    obj = obj.parentElement;
                } while (obj.tagName != "TABLE")
                var parentTreeLevel = obj.rows[0].cells.length;
                var parentTreeNode = obj.rows[0].cells[0];
                var tables = obj.parentElement.getElementsByTagName("TABLE");
                var numTables = tables.length
                if (numTables >= 1) {
                    for (i = 0; i < numTables; i++) {
                        if (tables[i] == obj) {
                            treeNodeFound = true;
                            i++;
                            if (i == numTables) {
                                return;
                            }
                        }
                        if (treeNodeFound == true) {
                            var childTreeLevel = tables[i].rows[0].cells.length;
                            if (childTreeLevel > parentTreeLevel) {
                                var cell = tables[i].rows[0].cells[childTreeLevel - 1];
                                var inputs = cell.getElementsByTagName("INPUT");
                                inputs[0].checked = checkedState;

                            }
                            else {//alert('yyyy');
                                return;
                                //                                if(treeNode.Parent != null)
                                //                                { alert('yyyy');
                                //                                    //node.Parent.Select();
                                //                                    //SelectParents(treeNode,checkedState);
                                //                                } 
                            }
                        }
                    }
                }
            }
        }
        function SelectParents(node, checkedState) {
            //check whether the Ctrl key has been pressed and return false so that the node cannot be selected
            if (node.Parent != null) {
                node.Parent.checked = checkedState;
                SelectParents(node.Parent, checkedState);
            }
        }

        function Reload() {

            window.location("root_UserGroup1.aspx");

        }
        function ClickOnMoreInfo(keyValue) {

            var grp = keyValue.substr(keyValue.indexOf(',') + 1);
            keyValue = keyValue.substr(',', keyValue.indexOf(','));

            var url = 'Member_usergroup.aspx?id=' + keyValue;
            OnMoreInfoClick(url, "Members Details" + '-' + grp, '940px', '450px', "Y");

        }

        function addToHidden(who) {
            //alert('add');
            if (who == 'seg') {
                var ListBoxSegment = document.getElementById("ListBoxSegmentCurrent");
                var listitemSegment = "";
                var i;
                for (i = 0; i < ListBoxSegment.options.length; i++) {
                    if (listitemSegment != "") {
                        listitemSegment += '~' + ListBoxSegment.options[i].value + ',' + ListBoxSegment.options[i].text;
                    }
                    else {
                        listitemSegment = ListBoxSegment.options[i].value + ',' + ListBoxSegment.options[i].text;
                        //alert(listitemSegment);
                    }
                }
                var HiddenSeg = document.getElementById("HDSegment");
                HiddenSeg.value = listitemSegment;
            }
            if (who == 'user') {   //alert('user');
                var ListBoxUser = document.getElementById("ListBoxUserCurrent");
                var listitemsUser = "";
                var i;
                for (i = 0; i < ListBoxUser.options.length; i++) {
                    if (listitemsUser != "") {
                        listitemsUser += '~' + ListBoxUser.options[i].value + ',' + ListBoxUser.options[i].text;
                    }
                    else {
                        listitemsUser = ListBoxUser.options[i].value + ',' + ListBoxUser.options[i].text;
                    }
                }
                var HiddenUser = document.getElementById("HDUser");
                HiddenUser.value = listitemsUser;
            }

        }


        function BtnCancelSegment_click() {
            document.getElementById("SegmentListRow").style.visibility = 'hidden';
        }

        function btnCancelUser_click() {
            document.getElementById("FormListUser").style.visibility = 'hidden';
        }
        function openLegendPage() {
            window.open('frm_legendPopUp.aspx', '50', 'resizable=1,height=250px,width=100px');

        }
        function openSubmenu(id) {
            var location = '../management/root_UserGroup_POPUP.aspx?ID=' + id;
            window.open(location, 'SubmenuAccess', 'location=no,directories=no,menubar=no,toolbar=no,status=yes,scrollbars=yes,resizable=yes,dependent=no');
            //window.open(location,'Submenu Access','width=200,height=200,directories=yes,location=yes,menubar=yes,scrollbars=yes,status=yes,toolbar=yes,resizable=yes');
        }
        function CallHeight(obj) {             
        }
      

        function SelectAll(objCheckAll) {

            var inputs = document.getElementsByTagName("input");
            for (var i = 0; i < inputs.length; i++) {
                if (inputs[i].type == "checkbox") {
                    if (inputs[i].className == 'dxtl__Sel') {
                        if (objCheckAll.checked == true) {
                            if (inputs[i].checked == false)
                                inputs[i].click();
                        }
                        else
                            if (inputs[i].checked == true)
                                inputs[i].click();
                    }

                }

            }
        }
        function ShowHideFilter(obj) {
            GridUserGroup1.PerformCallback(obj);
            //alert(obj);
            //height();
        }
        function OnAllCheckedChanged(s, e) {
            if (s.GetChecked())
                GridUserGroup.SelectRows();
            else
                GridUserGroup.UnselectRows();
        }

        function SelectExclude(objCheckAllExc) {
            var inputs = document.getElementsByTagName("input");
            //alert(inputs.length);
            for (var i = 0; i < inputs.length; i++) {
                if (inputs[i].type == "checkbox") {
                    if (inputs[i].className == 'dxtl__Sel') {
                        if (objCheckAllExc.checked == true) {
                            inputs[i].click();
                        }
                        else
                            if (inputs[i].checked == true)
                                inputs[i].click();
                    }

                }

            }


        }

    </script>


    <!--___________________________________________________________________________-->




</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table class="TableMain100">
            <tr>
                <td class="EHEADER" style="text-align: center">
                    <span style="color: Blue"><strong>User Group</strong></span>
                </td>
            </tr>
            <tr>
                <td style="text-align: left">
                    <table style="width: 100%">
                        <tr>
                            <td id="td_false" runat="server">
                                <table width="100%">
                                    <tr>
                                        <td style="text-align: left; vertical-align: top">
                                            <table>
                                                <tr>
                                                    <td id="ShowFilter">
                                                        <a href="javascript:ShowHideFilter('s');"><span style="color: #000099; text-decoration: underline">Show Filter</span></a>
                                                    </td>
                                                    <td id="Td1">
                                                        <a href="javascript:ShowHideFilter('All');"><span style="color: #000099; text-decoration: underline">All Records</span></a>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td></td>
                                        <td class="gridcellright" align="left">
                                            <dxe:ASPxComboBox ID="cmbExport" runat="server" AutoPostBack="true" BackColor="Black"
                                                Font-Bold="False" ForeColor="White" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged"
                                                ValueType="System.Int32" Width="130px">
                                                <Items>
                                                    <dxe:ListEditItem Text="Select" Value="0" />
                                                    <dxe:ListEditItem Text="PDF" Value="1" />
                                                    <dxe:ListEditItem Text="XLS" Value="2" />
                                                    <dxe:ListEditItem Text="RTF" Value="3" />
                                                    <dxe:ListEditItem Text="CSV" Value="4" />
                                                </Items>
                                                <ButtonStyle BackColor="#C0C0FF" ForeColor="Black">
                                                </ButtonStyle>
                                                <ItemStyle BackColor="Navy" ForeColor="White">
                                                    <HoverStyle BackColor="#8080FF" ForeColor="White">
                                                    </HoverStyle>
                                                </ItemStyle>
                                                <Border BorderColor="White" />
                                                <DropDownButton Text="Export">
                                                </DropDownButton>
                                            </dxe:ASPxComboBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <%-- <td id="trSelect" runat="Server">
                                            <table>
                                                <tr>
                                                    <td>
                                                        Select / Deselect All
                                                    </td>
                                                    <td style="width:40px">
                                                       <asp:CheckBox ID="chkAll" onclick="SelectAll(this);"  runat="Server" />
                                                    </td>
                                                    <td>
                                                        Exclude selected
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkExclude" onclick="SelectExclude(this);" runat="Server" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>--%>
                        </tr>
                        <tr>
                            <td id="td_false2" style="width: 45%; vertical-align: top;" runat="server">

                                <dxe:ASPxGridView ID="GridUserGroup" runat="server" Width="100%" AutoGenerateColumns="False" OnCustomCallback="GridUserGroup_CustomCallback"
                                    ClientInstanceName="GridUserGroup1" KeyFieldName="grp_id" OnRowCommand="GridUserGroup_RowCommand" OnRowDeleting="GridUserGroup_RowDeleting" OnPageIndexChanged="GridUserGroup_PageIndexChanging"
                                    Border-BorderStyle="NotSet" OnBeforeGetCallbackResult="GridUserGroup_BeforeGetCallbackResult">

                                    <ClientSideEvents SelectionChanged="function(s, e) { OnGridFocusedRowChanged(); }" />
                                    <Columns>
                                        <%--<dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Visible="False">
                                                            <ClearFilterButton Visible="True">
                                                            </ClearFilterButton>
                                                        </dxe:GridViewCommandColumn>
                                                        <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="3%">
                                                            <HeaderTemplate>
                                                                <input type="checkbox" id="chkDel" onclick="grid.SelectAllRowsOnPage(this.checked);" style="vertical-align: middle;"
                                                                    title="Select/Unselect all rows on the page"></input>
                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="Center">
                                                                <Paddings PaddingBottom="1px" PaddingTop="1px" />
                                                            </HeaderStyle>
                                                        </dxe:GridViewCommandColumn>--%>
                                        <%--<dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="50px" VisibleIndex="0">
                                                <HeaderTemplate>
                                                   <dxe:ASPxCheckBox ID="chkDel" runat="server" ClientInstanceName="chkDel" ToolTip="Select all rows"
                                                        BackColor="White"  >
                                                     
                                                         <ClientSideEvents CheckedChanged="OnAllCheckedChanged" />
                                                    </dxe:ASPxCheckBox>--%>
                                        <%--<dxe:ASPxCheckBox ID="cbPage" runat="server" ClientInstanceName="cbPage" ToolTip="Select all rows within the page"
                                                        OnInit="cbPage_Init">
                                                        <ClientSideEvents CheckedChanged="OnPageCheckedChanged" />
                                                    </dxe:ASPxCheckBox>
                                                </HeaderTemplate>
                                            </dxe:GridViewCommandColumn>--%>
                                        <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="grp_name"
                                            Caption="Group Name">
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="segname"
                                            Caption="Segment Name">
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn VisibleIndex="4">
                                            <%-- <HeaderTemplate>
                                                         <a href="#" onclick="javascript:openLegendPage();"><span class="Ecoheadtxt" style="color: Blue">
                                                            <strong>Legends</strong></span></a>
                                                        
                                                        </HeaderTemplate>--%>
                                            <DataItemTemplate>
                                                <asp:LinkButton ID="btn_show" runat="server" Text="Edit" CommandArgument='<%# Container.KeyValue %>' CommandName="res"> </asp:LinkButton>
                                            </DataItemTemplate>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn VisibleIndex="5">
                                            <%-- <HeaderTemplate>
                                                         <a href="#" onclick="javascript:openLegendPage();"><span class="Ecoheadtxt" style="color: Blue">
                                                            <strong>Legends</strong></span></a>
                                                        
                                                        </HeaderTemplate>--%>
                                            <DataItemTemplate>
                                                <a href="javascript:void(0);" onclick="ClickOnMoreInfo('<%# Container.KeyValue %> ,<%# Eval("grp_name") %>')">Members</a>

                                            </DataItemTemplate>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewCommandColumn VisibleIndex="6" ShowDeleteButton="True">
                                            <HeaderTemplate>
                                                <asp:Button ID="bt_1" runat="server" Text="Add New" OnClick="btnAdd_Mod_Click"/>
                                                <%--<a href="javascript:ShowHideFilter('s');" onclick="btnAdd_Mod_Click" runat="server"><span style="color: #000099; text-decoration: underline">
                                                                                                    Add New</span></a>--%>
                                            </HeaderTemplate>
                                            <HeaderStyle HorizontalAlign="Center"/>
                                        </dxe:GridViewCommandColumn>

                                    </Columns>
                                    <SettingsBehavior AllowFocusedRow="True" ConfirmDelete="True" />
                                    <SettingsText ConfirmDelete="Are u want to delete this?" />

                                    <SettingsPager NumericButtonCount="10" ShowSeparators="True" Mode="ShowPager"
                                        PageSize="10">
                                        <FirstPageButton Visible="True">
                                        </FirstPageButton>
                                        <LastPageButton Visible="True">
                                        </LastPageButton>
                                    </SettingsPager>
                                    <Styles>
                                        <FocusedRow BackColor="#FFC080" Font-Bold="False">
                                        </FocusedRow>
                                        <Header BackColor="ControlLight" Font-Bold="True" ForeColor="Black" HorizontalAlign="Center">
                                        </Header>
                                    </Styles>
                                    <%--<ClientSideEvents EndCallback="function(s, e) {
	callheight(s.cpHeight);
}" />--%>
                                </dxe:ASPxGridView>
                            </td>

                            <td style="vertical-align: top; text-align: left; width: 55%;">
                                <table style="width: 100%;">
                                    <tr id="trSelect" runat="Server">
                                        <td>
                                            <table>
                                                <tr>
                                                    <td>Select / Deselect All
                                                    </td>
                                                    <td style="width: 40px">
                                                        <asp:CheckBox ID="chkAll" onclick="SelectAll(this);" runat="Server" />
                                                    </td>
                                                    <td>Exclude selected
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkExclude" onclick="SelectExclude(this);" runat="Server" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table>

                                                <td id="td_onlyadd" runat="server" style="width: 203px">
                                                    <asp:Button ID="btnsaveaccount" runat="server" Text="Save" CssClass="btnUpdate" Width="70px"
                                                        OnClick="btnsaveaccount_Click" ValidationGroup="a" Height="23px" />
                                                </td>
                                                <td id="td_cancelonly" runat="server" style="width: 203px">
                                                    <asp:Button ID="btncancelaccount" runat="server" Text="Cancel" CssClass="btnUpdate" Width="70px"
                                                        OnClick="btncancelaccount_Click" ValidationGroup="a" Height="23px" />
                                                </td>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table>

                                                <tr>
                                                    <td id="tr_2nd" runat="server" style="width: 203px">
                                                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btnUpdate" Width="70px"
                                                            OnClick="btnSave_Click" ValidationGroup="a" Height="23px" />
                                                    </td>
                                                    <td id="tr_3rd" runat="server" style="width: 203px">
                                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btnUpdate" Width="69px"
                                                            OnClick="btnCancel_Click" Height="24px" />
                                                    </td>
                                                    <%--<td>
                                                    
                                                    </td>--%>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr id="EditSection" runat="server">
                                        <td style="vertical-align: top; text-align: left">
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td style="width: 156px; text-align: right;">
                                                        <span class="Ecoheadtxt">Segment Name:</span></td>
                                                    <td class="EcoheadCon" style="width: 203px; text-align: left;">
                                                        <asp:DropDownList ID="cmbSegmentForAdd" runat="server" Width="203px" AutoPostBack="True"
                                                            OnSelectedIndexChanged="cmbSegmentForAdd_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td style="width: 91px; text-align: right;">
                                                        <span class="Ecoheadtxt">Group Name:</span></td>
                                                    <td class="EcoheadCon" style="width: 203px; text-align: left;">
                                                        <asp:TextBox ID="txtGroupName" runat="server" Width="200px" ValidationGroup="a"></asp:TextBox>
                                                    </td>

                                                    <%--<td style="width: 203px">
                                                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btnUpdate" Width="70px"
                                                            OnClick="btnSave_Click" ValidationGroup="a" Height="23px" />
                                                    </td>--%>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtGroupName"
                                                            Display="Dynamic" ErrorMessage="User Group can not be null!" Font-Bold="True"
                                                            ValidationGroup="a"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>
                                            <table>

                                                <td style="vertical-align: top; text-align: left;" id="ListSection" runat="server">
                                                    <dxeTreeList:aspxtreelist id="TLgrid" runat="server" autogeneratecolumns="False" keyfieldname="menuID"
                                                        parentfieldname="menuParentID"
                                                        onhtmlrowprepared="TLgrid_HtmlRowPrepared" oncustomjsproperties="TLgrid_CustomJSProperties">
                                                <Columns>
                                                    <dxeTreeList:TreeListTextColumn Caption="name" FieldName="name" VisibleIndex="0">
                                                    </dxeTreeList:TreeListTextColumn>
                                                    <dxeTreeList:TreeListTextColumn Caption="Option" VisibleIndex="1">
                                                        <DataCellTemplate>
                                                            <dxe:ASPxTextBox ID="ASPxTextBox1" runat="server" Width="77px" Text='<%# Eval("Mode") %>'>
                                                                <MaskSettings ShowHints="True" Mask="&lt;All|Add|*View|DelAdd|Modify|Delete&gt;" />
                                                            </dxe:ASPxTextBox>
                                                        </DataCellTemplate>
                                                        <HeaderStyle Wrap="False" />
                                                        <CellStyle Wrap="False">
                                                        </CellStyle>
                                                    </dxeTreeList:TreeListTextColumn>
                                                    <%--<dxeTreeList:TreeListTextColumn FieldName="HaveSubMenu" Caption="#" VisibleIndex="2" Width="5%">
                                                        <DataCellTemplate>
                                                            <dxe:ASPxHyperLink ID="ASPxHyperLink1" runat="server" Text="Sub Menu" NavigateUrl='<%# "javascript: openSubmenu("+Eval("menuID")+");" %>'
                                                                Font-Underline="true">
                                                            </dxe:ASPxHyperLink>
                                                        </DataCellTemplate>
                                                        <CellStyle Wrap="False">
                                                        </CellStyle>
                                                    </dxeTreeList:TreeListTextColumn>--%>
                                                </Columns>
                                                
                                                <Settings GridLines="Both"    />
                                                <SettingsBehavior AllowSort="False" AutoExpandAllNodes="True"   />
                                                <SettingsSelection Enabled="True"   />
                                                <ClientSideEvents EndCallback="function(s, e) {
	CallHeight(cpHeight);
}" />
                                            </dxeTreeList:aspxtreelist>
                                                </td>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <%--</td>
            </tr>
        </table>--%>
                    <br />
                    <%--<asp:SqlDataSource ID="grp_id" runat="server" 
                SelectCommand=""></asp:SqlDataSource>--%>
                    <dxe:ASPxGridViewExporter ID="exporter" runat="server">
                    </dxe:ASPxGridViewExporter>
    </div>
</asp:Content>
