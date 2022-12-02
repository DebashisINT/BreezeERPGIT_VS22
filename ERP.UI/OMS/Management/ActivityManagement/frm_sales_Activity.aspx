<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.ActivityManagement.management_activitymanagement_frm_sales_Activity" CodeBehind="frm_sales_Activity.aspx.cs" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title></title>

    <style>
        .mylabel1 {
            width: 150px;
        }
    </style>

    <script src="http://code.jquery.com/jquery-1.10.2.js"></script>
    <script src="http://code.jquery.com/ui/1.11.4/jquery-ui.js"></script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            GetAllUserListAutoComplete();
          //  $("#txtAssigned").on('focus', function () { GetAllUserListAutoComplete(); });
            // $("#txtAssigned").focus(function () { GetAllUserListAutoComplete(); });
         
        });

       
        var chkobj;
        var objchk = null;
        var obGenral = null;
        function SignOff() {
            //window.parent.SignOff()
        }

        function chkGenral(objGenral, val12) {
            var st = document.getElementById("txtGrdContact")

            if (obGenral == null) {
                obGenral = objGenral;
            }
            else {
                obGenral.checked = false;
                obGenral = objGenral;
                obGenral.checked = true;
            }
            st.value = val12;
        }
        function ShowDetails(Userid) {

            window.location = "frm_sales_Activity.aspx?id=" + Userid;
        }
        function frmOpenNewWindow() {

            var val123 = document.getElementById("txtAssigned").value;
            if (val123 == "") {
                alert('Assign to can not be blank');
            }
            else {
                ReturnAccording = "Product";
                window.location = "frmOfferedProduct_New.aspx?Type=Sales";
                //   OnMoreInfoClick("frmOfferedProduct_New.aspx?Type=Sales", "ADD PRODUCT", "950px", "500px", "Y");
            }
        }

        function funAddLead() {
            var val123 = document.getElementById("txtAssigned").value;
            if (val123 == "") {
                alert('Assign to can not be blank');
            }
            else {

                OnMoreInfoClick("Add_LeadNew.aspx?Call=PhoneCall&user=" + val123, "ADD LEAD", "950px", "500px", "N");
            }
        }

        function UserList() {
            ReturnAccording = "UserList";
            OnMoreInfoClick("UserList.aspx", "ADD USER", "950px", "500px", "Y");
        }
        function GetUserList() {
            var ob = document.getElementById("txtAssigned");
            return ob;
        }
        function GetHiddenUserList() {
            var ob2 = document.getElementById("hd1UserList");
            return ob2;
        }
        function callback() {

            if (ReturnAccording == "UserList") {

                document.getElementById('Button1').disabled = false;
            }
            else if (ReturnAccording == "Product") {

                document.getElementById('btnSubmit').Enabled = true;
            }
            else if (ReturnAccording == "Allot") {
                var val = document.getElementById("hduser").value;
                ShowDetails(val);
            }

        }
        function windowopenform1(val) {

            ReturnAccording = "Allot";
            OnMoreInfoClick("frmAllot_sales_new.aspx?Calling=Sales&id=" + val, "Allotment", "950px", "500px", "Y");

        }
        function CallList(obj1, obj2, obj3) {

            if (obj1.value == "") {
                obj1.value = "%";
            }
            var obj5 = '';
            if (obj5 != '18') {
                ajax_showOptionsTEST(obj1, obj2, obj3, obj5);

                if (obj1.value == "%") {
                    obj1.value = "";
                }
            }
        }




        function GetAllUserListAutoComplete() {
            var x = '';
            $('#<%=txtAssigned.ClientID %>').autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: 'frm_sales_Activity.aspx/GetAllUserListAutocomplete',
                        data: "{'KeyWord':'" + $('#<%=txtAssigned.ClientID %>').val() + "'}",
                        dataType: "json",
                        success: function (data) {
                            // alert(data.d);
                            response($.map(data.d, function (item) {
                                return {
                                    label: item.split('|')[0],
                                    val: item.split('|')[1]
                                }
                            }));
                        },
                        error: function (result) {
                            //alert("Error");
                        }
                    });
                },

                select: function (event, ui) {
                    // alert(ui.item.val);
                    //  $('#<%=txtAssigned.ClientID %>').text(ui.item.label);
                    $('#<%=hd1UserList.ClientID %>').val(ui.item.val);
                },
                minLength: 1
            });
        }
    </script>
    <style>
        .ui-autocomplete {
            background: #fff;
            border: 1px solid #1182C8;
            width: 300px;
            padding: 0;
        }

            .ui-autocomplete .ui-menu-item {
                padding: 5px 8px;
                list-style-type: none;
                margin: 0;
            }

                .ui-autocomplete .ui-menu-item:hover {
                    background: #1182C8;
                    color: #fff;
                }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Sales</h3>

        </div>

    </div>

    <div class="form_main">
        <table class="TableMain100">

            <tr>
                <td valign="top">
                    <table class="TableMain100">

                        <tr>
                            <td class="gridcellleft">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnCreate" Text="Add New" runat="server" CssClass="btn btn-primary" OnClick="btnCreate_Click" />
                                            <asp:Button ID="btnModify" Visible="false" Text="Modify" runat="server" CssClass="btn btn-primary"
                                                OnClick="btnModify_Click" /></td>
                                        <td class="mylabel1" align="right">
                                            <asp:Label ID="lblUserName" runat="server" Text="UserName :" Visible="False" Width="71px"></asp:Label>
                                            <asp:Label ID="txtUser" runat="server" Width="162px"></asp:Label></td>
                                        <td class="mylabel1">&nbsp;</td>
                                    </tr>
                                </table>
                            </td>
                        </tr>


                        <tr id="ActivityRow" runat="server">
                            <td style="text-align: center;">
                                <asp:Panel ID="PanelActivitySales" runat="server" ScrollBars="Horizontal" Width="1150px">
                                    <dxe:ASPxGridView ID="ActivitySalesGrid" runat="server" AutoGenerateColumns="False"
                                        Width="100%">
                                        <Columns>
                                            <dxe:GridViewDataTextColumn FieldName="User" ReadOnly="True" VisibleIndex="0">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="Pending Acttivity" ReadOnly="True" VisibleIndex="1">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="Scheduled End Date" VisibleIndex="2" ReadOnly="True"
                                                Width="5%">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="Expected End Date" ReadOnly="True" Visible="false">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="Pending Sales" VisibleIndex="3">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="Pending MF" VisibleIndex="4">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="Pending Ins" VisibleIndex="5">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="Pending EQ" VisibleIndex="6">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="Pending IPO" VisibleIndex="7">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="Completed MF" VisibleIndex="8">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="Completed Ins" VisibleIndex="9">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="Completed EQ" VisibleIndex="10">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="Completed IPO" VisibleIndex="11">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn VisibleIndex="13">
                                                <DataItemTemplate>
                                                    <a href="#" onclick="ShowDetails('<%#Eval("UserId") %>')">Detail</a>
                                                </DataItemTemplate>
                                                <EditFormSettings Visible="False" />
                                            </dxe:GridViewDataTextColumn>
                                        </Columns>
                                        <Styles>
                                            <LoadingPanel ImageSpacing="10px">
                                            </LoadingPanel>
                                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                            </Header>
                                            <Cell CssClass="gridcellleft">
                                            </Cell>
                                            <Header Font-Bold="true" ForeColor="black" CssClass="EHEADER" Border-BorderColor="AliceBlue">
                                            </Header>
                                            <AlternatingRow BackColor="White">
                                            </AlternatingRow>
                                        </Styles>
                                        <SettingsPager NumericButtonCount="20" PageSize="20" ShowSeparators="True">
                                            <FirstPageButton Visible="True">
                                            </FirstPageButton>
                                            <LastPageButton Visible="True">
                                            </LastPageButton>
                                        </SettingsPager>
                                    </dxe:ASPxGridView>
                                </asp:Panel>
                            </td>
                        </tr>
                        <%--  <tr>
                            <td class="gridcellleft">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnCreate" Text="Create" runat="server" CssClass="btn btn-primary" OnClick="btnCreate_Click" />
                                            <asp:Button ID="btnModify" Visible="false" Text="Modify" runat="server" CssClass="btn btn-primary"
                                                OnClick="btnModify_Click" /></td>
                                        <td class="mylabel1" align="right">
                                            <asp:Label ID="lblUserName" runat="server" Text="UserName :" Visible="False" Width="71px"></asp:Label>
                                            <asp:Label ID="txtUser" runat="server" Width="162px"></asp:Label></td>
                                        <td class="mylabel1">&nbsp;</td>
                                    </tr>
                                </table>
                            </td>
                        </tr>--%>
                        <tr>
                            <td align="left">
                                <table width="100%">
                                    <tr>
                                        <td align="center" valign="top">
                                            <asp:Panel ID="pnlShowDetail" runat="server" Width="100%">
                                                <asp:GridView ID="grdDetail" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None"
                                                    BorderWidth="1px" BorderColor="#507CD1" ShowFooter="True" Width="100%" OnRowDataBound="grdDetail_RowDataBound">
                                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                    <RowStyle BackColor="#EFF3FB" BorderWidth="1px" />
                                                    <EditRowStyle BackColor="#2461BF" />
                                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                                    <HeaderStyle Font-Bold="false" ForeColor="black" CssClass="EHEADER" BorderColor="AliceBlue"
                                                        BorderWidth="1px" />
                                                    <AlternatingRowStyle BackColor="White" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Activity">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkDetail" runat="server" />
                                                                <asp:Label ID="lblActivity" runat="server" Text='<%# Eval("Activity No")%>' Visible="false"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="15%" Wrap="False" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <HeaderStyle CssClass="gridheader" />
                                                </asp:GridView>
                                                <asp:TextBox ID="txtId" runat="server" Visible="false"></asp:TextBox>
                                                <asp:Button ID="btnReassign" Text="Reassign" Visible="false" runat="server" CssClass="btn btn-primary"
                                                    OnClick="btnReassign_Click" />
                                                <asp:Button ID="btnReschedule" Text="Reschedule" Visible="false" runat="server" CssClass="btn btn-primary"
                                                    OnClick="btnReschedule_Click" />
                                                <asp:Button ID="btnShowDetail" Text="Show Detail" Visible="false" runat="server"
                                                    CssClass="btn btn-primary" OnClick="btnShowDetail_Click" />

                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="">
                                            <asp:Panel ID="pnlCall" runat="server" Visible="false" Width="100%">
                                                <div class="crossBtn"><a href="frm_Sales_Activity.aspx"><i class="fa fa-times"></i></a></div>
                                                <table style="width: 800px">
                                                    <tr>
                                                        <td class="mylabel1" style="display: none">Activity Type :</td>
                                                        <td class="lt" style="display: none">
                                                            <asp:DropDownList ID="drpActType" Enabled="false" AutoPostBack="true" runat="server"
                                                                Width="302px">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>Assign To :</td>
                                                        <td style="position: relative;">
                                                            <asp:DropDownList ID="drpUserWork" runat="server" Width="100%" Visible="false" class="chzn-select">
                                                            </asp:DropDownList>
                                                            <asp:TextBox ID="txtAssigned" runat="server" Width="250px">
                                                            </asp:TextBox>
                                                            <%-- <asp:TextBox ID="txtReferedBy_hidden" runat="server"></asp:TextBox>--%>
                                                            <asp:HiddenField ID="hd1UserList" runat="server" />
                                                        </td>

                                                    </tr>
                                                    <tr>
                                                        <td class="mylabel1">
                                                            <asp:Label ID="Label1" runat="server" Text="Start Date/Start Time : " Width="138px"></asp:Label></td>
                                                        <td class="mylabel1">
                                                            <%--<asp:TextBox ID="txtStartDate" runat="server" Font-Size="12px" Width="140px"></asp:TextBox>
                                                        <asp:Image ID="Image1" runat="server" ImageUrl="~/images/calendar.jpg" />--%>
                                                            <dxe:ASPxDateEdit ID="txtStartDate" runat="server" EditFormat="Custom" UseMaskBehavior="true"
                                                                Width="250px">
                                                                <ButtonStyle Width="13px">
                                                                </ButtonStyle>
                                                            </dxe:ASPxDateEdit>
                                                        </td>
                                                        <td style="width: 130px">
                                                            <asp:Label ID="Label3" runat="server" Text="End Date/End Time :"></asp:Label>

                                                        </td>
                                                        <td class="mylabel1">
                                                            <%--<asp:TextBox ID="txtEndDate" runat="server" Font-Size="12px" Width="140px"></asp:TextBox>
                                                       <asp:Image ID="Image2" runat="server" ImageUrl="~/images/calendar.jpg" />--%>
                                                            <dxe:ASPxDateEdit ID="txtEndDate" runat="server" EditFormat="Custom" UseMaskBehavior="true"
                                                                Width="250px">
                                                                <ButtonStyle Width="13px">
                                                                </ButtonStyle>
                                                            </dxe:ASPxDateEdit>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="mylabel1">
                                                            <asp:Label ID="Label4" runat="server" Text="Priority :"></asp:Label></td>
                                                        <td class="lt" colspan="3">
                                                            <asp:DropDownList ID="drpPriority" runat="server" Width="250px">
                                                                <asp:ListItem Text="Low" Value="0"></asp:ListItem>
                                                                <asp:ListItem Text="Normal" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="High" Value="2"></asp:ListItem>
                                                                <asp:ListItem Text="Urgent" Value="3"></asp:ListItem>
                                                                <asp:ListItem Text="Immediate" Value="4"></asp:ListItem>
                                                            </asp:DropDownList>
                                                            <input type="button" value="Add Lead" id="Button1" class="btnUpdate btn btn-primary" onclick="funAddLead()"
                                                                disabled="disabled" />
                                                            <input type="button" value="Add Product" id="Button2" name="Button2" class="btnUpdate btn btn-primary"
                                                                visible="false" onclick="frmOpenNewWindow()" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td class="mylabel1">Description :</td>
                                                        <td class="lt" style="width: 20%">
                                                            <asp:TextBox ID="txtDesc" TextMode="MultiLine" Rows="2" runat="server" Width="100%"
                                                                Height="79px"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 130px">
                                                            <asp:Label ID="Label2" runat="server" Text="Instruction Notes :"></asp:Label></td>
                                                        <td colspan="3" class="lt">
                                                            <asp:TextBox ID="txtInstNote" runat="server" TextMode="MultiLine" Rows="5" Columns="55"
                                                                Height="91px" Width="100%"></asp:TextBox>
                                                        </td>
                                                    </tr>


                                                    <tr>
                                                        <td colspan="5" class="gridcellleft" style="padding-left: 150px;">
                                                            <asp:Button ID="btnSubmit" Text="Save" runat="server" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
                                                            <asp:Button ID="btnCancel" Text="Cancel" runat="server" CssClass="btn btn-danger" OnClick="btnCancel_Click" />&nbsp;
                                                          <%--  <asp:TextBox ID="txtUserList" runat="server" BackColor="Transparent" BorderColor="Transparent"
                                                                BorderStyle="None" ForeColor="#DDECFE"></asp:TextBox>--%>
                                                        </td>
                                                    </tr>
                                                </table>

                                            </asp:Panel>
                                            <asp:Panel ID="pnlActivityDetail" runat="server" Visible="false" Width="100%">
                                                <asp:GridView ID="grdActivityDetail" runat="server" CellPadding="4" ForeColor="#333333"
                                                    GridLines="None" BorderWidth="1px" BorderColor="#507CD1" ShowFooter="True" Width="100%">
                                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                    <RowStyle BackColor="#EFF3FB" BorderWidth="1px" />
                                                    <EditRowStyle BackColor="#2461BF" />
                                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                                    <HeaderStyle Font-Bold="false" ForeColor="black" CssClass="EHEADER" BorderColor="AliceBlue"
                                                        BorderWidth="1px" />
                                                    <AlternatingRowStyle BackColor="White" />
                                                </asp:GridView>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <asp:SqlDataSource ID="ActivitySalesDataSource" runat="server" ></asp:SqlDataSource>
        <asp:HiddenField ID="hduser" runat="server" />
    </div>
</asp:Content>
