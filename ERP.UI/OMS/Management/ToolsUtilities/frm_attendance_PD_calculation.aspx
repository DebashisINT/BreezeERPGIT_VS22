<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.ToolsUtilities.management_utilities_frm_attendance_PD_calculation" CodeBehind="frm_attendance_PD_calculation.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/assests/js/jquery-1.3.2.js"></script>
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
    <style type="text/css">
        .demoheading {
            padding-bottom: 20px;
            color: #5377A9;
            font-family: Arial, Sans-Serif;
            font-weight: bold;
            font-size: 1.5em;
        }

        .water {
            font-family: Tahoma,Arial, Verdana, sans-serif;
            font-size: 100%;
        }

        .opaque {
            color: Gray;
        }
    </style>

    <script type="text/javascript" language="javascript">
        function HideTRADD() {
            document.getElementById("TRADD").style.display = 'none';
        }
        function HideTRSearch() {
            document.getElementById("TDFILTER").style.display = 'none';
        }
        function ShowADD() {
            document.getElementById("TRADD").style.display = 'inline';
            document.getElementById("TDFILTER").style.display = 'none';
        }
        function ShowFilter() {
            document.getElementById("TDFILTER").style.display = 'inline';
            document.getElementById("TRADD").style.display = 'none';
        }
        function height() {
            if (document.body.scrollHeight > 500)
                window.frameElement.height = document.body.scrollHeight;
            else
                window.frameElement.height = '500px';
            window.frameElement.widht = document.body.scrollWidht;
            parent.height();
        }
        function firesearch() {
            var dd = document.getElementById("btnSearch");
            dd.click();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Convert PD to CL</h3>
        </div>
    </div>
    <div class="form_main">
        <table class="TableMain100">
            <tr>
                <td style="text-align: left; vertical-align: top; width: 105px;" rowspan="2">
                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="javascript:ShowADD()"
                        Font-Bold="True" Font-Underline="True" ForeColor="#000099">Add New</asp:HyperLink>
                    &nbsp;
                        <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="javascript:ShowFilter()"
                            Font-Bold="True" Font-Underline="True" ForeColor="#000099">Search</asp:HyperLink>
                </td>
                <td id="TRADD" style="display: none; text-align: left">
                    <table>
                        <tr>
                            <td class="gridcellleft">
                                <asp:DropDownList ID="cmbCompanyI" runat="server" DataSourceID="dataCompany" DataTextField="cmp_Name"
                                    DataValueField="cmp_internalid" Width="272px" CssClass="Ecoheadtxt">
                                </asp:DropDownList>
                            </td>
                            <td class="gridcellleft">
                                <asp:DropDownList ID="cmbBranchI" runat="server" DataSourceID="databranch" DataTextField="branch_description"
                                    DataValueField="branch_id" Width="200px" CssClass="Ecoheadtxt">
                                </asp:DropDownList>
                            </td>
                            <td class="gridcellleft">
                                <asp:DropDownList ID="cmbMonthI" runat="server" Width="50px" CssClass="Ecoheadtxt">
                                    <asp:ListItem Value="1">Jan</asp:ListItem>
                                    <asp:ListItem Value="2">Feb</asp:ListItem>
                                    <asp:ListItem Value="3">Mar</asp:ListItem>
                                    <asp:ListItem Value="4">Apr</asp:ListItem>
                                    <asp:ListItem Value="5">May</asp:ListItem>
                                    <asp:ListItem Value="6">Jun</asp:ListItem>
                                    <asp:ListItem Value="7">Jul</asp:ListItem>
                                    <asp:ListItem Value="8">Aug</asp:ListItem>
                                    <asp:ListItem Value="9">Sep</asp:ListItem>
                                    <asp:ListItem Value="10">Oct</asp:ListItem>
                                    <asp:ListItem Value="11">Nov</asp:ListItem>
                                    <asp:ListItem Value="12">Dec</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td class="gridcellleft">
                                <asp:DropDownList ID="cmbYearI" runat="server" Width="70px" CssClass="Ecoheadtxt"
                                    DataSourceID="dataYear" DataTextField="yer" DataValueField="yer">
                                </asp:DropDownList>
                            </td>
                            <td class="gridcellleft">
                                <asp:DropDownList ID="cmbLockI" runat="server" Width="50px" CssClass="Ecoheadtxt">
                                    <asp:ListItem Value="Y" Selected="True">Lock</asp:ListItem>
                                    <asp:ListItem Value="N">Open</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td class="gridcellleft" style="vertical-align: top;">
                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btnUpdate" OnClick="btnSave_Click"
                                    Height="21px" Width="36px" />
                                <input id="btnCancel" type="button" value="Cancel" class="btnUpdate" onclick="HideTRADD()"
                                    style="width: 49px; height: 21px" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="text-align: left; vertical-align: top; display: none;" id="TDFILTER">
                    <table>
                        <tr>
                            <td class="gridcellleft" style="vertical-align: top;">
                                <asp:TextBox ID="txtCompany" runat="server" CssClass="water" Text="Company" Width="257px" ToolTip="Company"></asp:TextBox>
                            </td>
                            <td style="vertical-align: top;" class="gridcellleft">
                                <asp:TextBox ID="txtBranch" runat="server" CssClass="water" Text="Branch" Width="197px" ToolTip="Branch"></asp:TextBox>
                            </td>
                            <td class="gridcellleft" style="vertical-align: top;">
                                <asp:TextBox ID="txtMonth" runat="server" CssClass="water" Text="Month" Width="49px" ToolTip="Month"></asp:TextBox>
                            </td>
                            <td class="gridcellleft" style="vertical-align: top;">
                                <asp:TextBox ID="txtYear" runat="server" CssClass="water" Text="Year" Width="61px" ToolTip="Year"></asp:TextBox>
                            </td>
                            <td class="gridcellleft" style="vertical-align: top;">
                                <asp:TextBox ID="txtLock" runat="server" CssClass="water" Text="Status" Width="67px" onblur="firesearch();" ToolTip="Status"></asp:TextBox>
                            </td>
                            <td class="gridcellleft" style="vertical-align: top;">
                                <asp:Button ID="btnSearch" runat="server" Text="Go" CssClass="btnUpdate" Height="20px" Width="49px" OnClick="btnSearch_Click" />
                                <input id="Button1" type="button" value="Cancel" class="btnUpdate" onclick="HideTRSearch()"
                                    style="width: 49px; height: 21px" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:GridView ID="GrdAttendanceLock" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                        DataKeyNames="al_id" PageSize="25" BorderStyle="Groove" EnableTheming="False"
                        AllowSorting="True" BorderColor="Navy" BorderWidth="1px" ShowFooter="True" DataSourceID="SqlDataSource1"
                        Width="100%" OnRowUpdating="GrdAttendanceLock_RowUpdating" OnSorting="GrdAttendanceLock_Sorting" EmptyDataText="No Data Found!">
                        <Columns>
                            <asp:TemplateField HeaderText="Company" SortExpression="al_company">
                                <ItemTemplate>
                                    <asp:Label ID="lblCompany" runat="server" Text='<%# Eval("al_companyD") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="gridcellleft" />
                                <EditItemTemplate>
                                    <asp:DropDownList ID="cmbCompanyE" runat="server" DataSourceID="dataCompany" DataTextField="cmp_Name"
                                        DataValueField="cmp_internalid" SelectedValue='<%# Eval("al_company") %>' Width="100%" Enabled="false"
                                        CssClass="Ecoheadtxt">
                                    </asp:DropDownList>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Branch" SortExpression="al_branch">
                                <ItemTemplate>
                                    <asp:Label ID="lblBranch" runat="server" Text='<%# Eval("al_branchD") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="gridcellleft" />
                                <EditItemTemplate>
                                    <asp:DropDownList ID="cmbBranchE" runat="server" DataSourceID="databranch" DataTextField="branch_description"
                                        DataValueField="branch_id" SelectedValue='<%# Eval("al_branch") %>' Width="100%" Enabled="false"
                                        CssClass="Ecoheadtxt">
                                    </asp:DropDownList>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Month" SortExpression="al_month">
                                <ItemTemplate>
                                    <asp:Label ID="lblMonth" runat="server" Text='<%# Eval("al_monthD") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="gridcellleft" />
                                <EditItemTemplate>
                                    <asp:DropDownList ID="cmbMonthE" runat="server" SelectedValue='<%# Eval("al_month") %>' Enabled="false"
                                        Width="100%" CssClass="Ecoheadtxt">
                                        <asp:ListItem Value="1">Jan</asp:ListItem>
                                        <asp:ListItem Value="2">Feb</asp:ListItem>
                                        <asp:ListItem Value="3">Mar</asp:ListItem>
                                        <asp:ListItem Value="4">Apr</asp:ListItem>
                                        <asp:ListItem Value="5">May</asp:ListItem>
                                        <asp:ListItem Value="6">Jun</asp:ListItem>
                                        <asp:ListItem Value="7">Jul</asp:ListItem>
                                        <asp:ListItem Value="8">Aug</asp:ListItem>
                                        <asp:ListItem Value="9">Sep</asp:ListItem>
                                        <asp:ListItem Value="10">Oct</asp:ListItem>
                                        <asp:ListItem Value="11">Nov</asp:ListItem>
                                        <asp:ListItem Value="12">Dec</asp:ListItem>
                                    </asp:DropDownList>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Year" SortExpression="al_year">
                                <ItemTemplate>
                                    <asp:Label ID="lblYear" runat="server" Text='<%# Eval("al_year") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="gridcellleft" />
                                <EditItemTemplate>
                                    <asp:DropDownList ID="cmbYearE" runat="server" SelectedValue='<%# Eval("al_year") %>'
                                        Width="100%" CssClass="Ecoheadtxt" DataSourceID="dataYear" DataTextField="yer" Enabled="false"
                                        DataValueField="yer">
                                    </asp:DropDownList>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Lock" SortExpression="la_PD_Lock">
                                <ItemTemplate>
                                    <asp:Label ID="lblLock" runat="server" Text='<%# Eval("al_lockD") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="gridcellleft" />
                                <EditItemTemplate>
                                    <asp:DropDownList ID="cmbLockE" runat="server" SelectedValue='<%# Eval("la_PD_Lock") %>'
                                        Width="100%" CssClass="Ecoheadtxt">
                                        <asp:ListItem Value="Y">Lock</asp:ListItem>
                                        <asp:ListItem Value="N">Open</asp:ListItem>
                                    </asp:DropDownList>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <EditItemTemplate>
                                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" CommandName="Update"
                                        Text="Update"></asp:LinkButton>
                                    <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Cancel"
                                        Text="Cancel"></asp:LinkButton>
                                </EditItemTemplate>
                                <HeaderStyle Width="5%" />
                                <ItemStyle Wrap="False" CssClass="gridcellleft" />
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Edit"
                                        Text="Edit"></asp:LinkButton>
                                    <%--<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Delete"
                                            Text="Delete" OnClientClick="javascript:return confirm('Do You Want To Delete This Record ?')"></asp:LinkButton>--%>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerSettings FirstPageImageUrl="~/images/pFirst.png" LastPageImageUrl="~/images/pLast.png"
                            Mode="NextPreviousFirstLast" NextPageImageUrl="~/images/pNext.png" PreviousPageImageUrl="~/images/pPrev.png" />
                        <PagerStyle CssClass="GridPager" HorizontalAlign="Center" />
                        <HeaderStyle CssClass="GridHeader" />
                        <RowStyle BorderColor="Red" BorderWidth="9px" />
                        <FooterStyle BorderStyle="None" CssClass="GridFooter" />
                    </asp:GridView>
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConflictDetection="OverwriteChanges"
                        DeleteCommand="insert into tbl_trans_attendanceLock_log select al_id,al_company,al_branch,al_month,al_year,al_lock,la_PD_Lock,'D',al_createDate,al_CreateUser,al_lastmodifiedDate,al_lastmodifiedUser,@user,getdate() from tbl_trans_attendanceLock where al_id=@al_id  DELETE FROM [tbl_trans_AttendanceLock] WHERE [al_id] = @al_id "
                        SelectCommand="SELECT [al_id], [al_company],(select ltrim(rtrim(cmp_name)) from tbl_master_company where cmp_internalid=al_company) as al_companyD, [al_branch],(select branch_description from tbl_master_branch where branch_id=al_branch) as al_branchD, [al_month],(case when al_month=1 then 'Jan' when al_month=2 then 'Feb' when al_month=3 then 'Mar' when al_month=4 then 'Apr' when al_month=5 then 'May' when al_month=6 then 'Jun' when al_month=7 then 'Jul' when al_month=8 then 'Aug' when al_month=9 then 'Sep' when al_month=10 then 'Oct' when al_month=11 then 'Nov' when al_month=12 then 'Dec' else '' end) as al_monthD, [al_year],  ISNULL(la_PD_Lock,'N') as la_PD_Lock,(case when la_PD_Lock='Y' then 'Locked' else 'Open' end) as al_lockD FROM [tbl_trans_AttendanceLock] ORDER BY [al_month] DESC, [al_year] DESC"
                        UpdateCommand="update table1 set temp123=123">
                        <DeleteParameters>
                            <asp:Parameter Name="al_id" Type="Int64" />
                            <asp:SessionParameter Name="user" SessionField="userid" Type="Int32" />
                        </DeleteParameters>
                        <UpdateParameters>
                            <asp:Parameter Name="al_company" Type="String" />
                            <asp:Parameter Name="al_branch" Type="Int32" />
                            <asp:Parameter Name="al_month" Type="Int32" />
                            <asp:Parameter Name="al_year" Type="Int32" />
                            <asp:Parameter Name="al_lock" Type="String" />
                            <asp:Parameter Name="al_id" Type="Int64" />
                            <asp:SessionParameter Name="user" SessionField="userid" Type="Int32" />
                        </UpdateParameters>
                        <InsertParameters>
                            <asp:Parameter Name="al_company" Type="String" />
                            <asp:Parameter Name="al_branch" Type="Int32" />
                            <asp:Parameter Name="al_month" Type="Int32" />
                            <asp:Parameter Name="al_year" Type="Int32" />
                            <asp:Parameter Name="al_lock" Type="String" />
                        </InsertParameters>
                    </asp:SqlDataSource>
                    <asp:SqlDataSource ID="dataCompany" runat="server" 
                        SelectCommand="SELECT [cmp_internalid], [cmp_Name] FROM [tbl_master_company] ORDER BY [cmp_Name]"></asp:SqlDataSource>
                    <asp:SqlDataSource ID="databranch" runat="server" 
                        SelectCommand="SELECT [branch_id], [branch_description] FROM [tbl_master_branch] ORDER BY [branch_description]"></asp:SqlDataSource>
                    <asp:SqlDataSource ID="dataYear" runat="server" 
                        SelectCommand="select year(getdate()) as yer union all select year(dateadd(yy,-1,getdate())) as yer"></asp:SqlDataSource>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
