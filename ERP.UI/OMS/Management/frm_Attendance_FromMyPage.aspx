<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.Reports_frm_Attendance_FromMyPage" CodeBehind="frm_Attendance_FromMyPage.aspx.cs" %>

<%--<%@ Register Assembly="DevExpress.Web.v15.1" Namespace="DevExpress.Web"
    TagPrefix="dxe000001" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxpc" %>--%>

<%@ Register Assembly="DevExpress.Web.v15.1" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />

    <script type="text/javascript" src="/assests/js/loaddata1.js"></script>

    <script type="text/javascript" src="/assests/js/init.js"></script>

    <script type="text/javascript" src="/assests/js/ajax-dynamic-list_wofolder.js"></script>

    <style type="text/css">
        /* Big box with list of options */
        #ajax_listOfOptions {
            position: absolute; /* Never change this one */
            width: 50px; /* Width of box */
            height: auto; /* Height of box */
            overflow: auto; /* Scrolling features */
            border: 1px solid Blue; /* Blue border */
            background-color: #FFF; /* White background color */
            text-align: left;
            font-size: 0.9em;
            z-index: 32767;
        }

            #ajax_listOfOptions div { /* General rule for both .optionDiv and .optionDivSelected */
                margin: 1px;
                padding: 1px;
                cursor: pointer;
                font-size: 0.9em;
            }

            #ajax_listOfOptions .optionDiv { /* Div for each item in list */
            }

            #ajax_listOfOptions .optionDivSelected { /* Selected item in the list */
                background-color: #DDECFE;
                color: Blue;
            }

        #ajax_listOfOptions_iframe {
            background-color: #F00;
            position: absolute;
            z-index: 3000;
        }

        form {
            display: inline;
        }
    </style>

    <script language="javascript" type="text/javascript">
        function SignOff() {
            window.parent.SignOff()
        }
        function height() {
            window.frameElement.height = document.body.scrollHeight;
            window.frameElement.widht = document.body.scrollWidht;
        }

        function validation() {
            ASPx_EmployeeAtdd.PerformCallback();
        }
        function PageLoad() {
            FieldName = 'btnShow';
            document.getElementById('txtName_hidden').style.display = "none";
            ShowEmployeeFilterForm('A');
            height();
        }
        function ShowEmployeeFilterForm(obj) {//alert(obj);
            document.getElementById('txtName_hidden').value = "";
            if (obj == 'A') {
                hide('tdtxtname');
                hide('tdname');
                document.getElementById('txtName_hidden').style.display = "none";
            }
            if (obj == 'S') {
                show('tdtxtname');
                show('tdname');
                document.getElementById('txtName_hidden').style.display = "none";
            }
            height();
        }
        function NoOfRows(obj) {
            //alert(obj);
            Noofrows = obj;
            document.getElementById('txtName_hidden').style.display = "none";
        }
        function show(obj1) {
            //alert(obj1);
            document.getElementById(obj1).style.display = 'inline';
        }
        function hide(obj1) {
            //alert(obj1);
            document.getElementById(obj1).style.display = 'none';
        }
        FieldName = 'btnShow'
        //      function CallAjax(obj1,obj2,obj3)
        //    {
        //        var cmbcompany=document.getElementById('cmbCompany_VI');
        //        var cmbbranch=document.getElementById('cmbBranch_VI');
        //        var obj4=cmbcompany.value+'~'+cmbbranch.value
        //        //alert(obj4);
        //        ajax_showOptions(obj1,obj2,obj3,obj4);
        //    }
        function openLegendPage() {
            window.open('frmLegendReport_popup.aspx', '50', 'resizable=1,height=250px,width=100px');
        }
        function height() {
            if (document.body.scrollHeight > 300)
                window.frameElement.height = document.body.scrollHeight;
            else
                window.frameElement.height = '300px';
            //        alert(window.frameElement.width);
            //        alert(document.documentElement.clientWidth);
            window.frameElement.width = document.documentElement.clientWidth;
            //        alert(window.frameElement.width);
        }
        function LastCall(obj) {
            height();
        }
        function btnShowClick() {
            if (ReportType.GetValue() == 'Screen') {
                ASPx_EmployeeAtdd.PerformCallback();
            }
            else {
                var user = '';
                if (User.GetValue() == 'A')
                    user = 'All';
                else {
                    var a = document.getElementById("txtName_hidden");
                    user = a.value;
                }
                var url = 'frmReport_Attendance_Print.aspx?id=' + Year.GetValue() + '~' + Month.GetValue() + '~' + Company.GetValue() + '~' + Branch.GetValue() + '~' + user;
                //alert(url)
                OnMoreInfoClick(url, "Employee Attendance Details", '940px', '450px', "N");
            }
        }
    </script>

    <link href="/aspnet_client/System_Web/2_0_50727/CrystalReportWebFormViewer3/css/default.css"
        rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table class="TableMain100" cellpadding="opx" cellspacing="0px">
            <tr>
                <td class="EHEADER" style="text-align: center">
                    <strong><span style="color: #000099">Employee's Attendance Report</span></strong>
                </td>
            </tr>
            <tr>
                <td>
                    <table border="0" class="TableMain" cellpadding="0" cellspacing="0" style="width: 100%; padding-right: 0px; padding-left: 0px; padding-bottom: 0px; padding-top: 0px;">
                        <tr>
                            <td valign="bottom" class="gridcellleft">
                                <table>
                                    <tr>
                                        <td style="text-align: left" valign="top">
                                            <a href="#" onclick="javascript:openLegendPage();"><span class="Ecoheadtxt" style="color: Blue">
                                                <strong>Legends</strong></span></a>
                                        </td>
                                        <td class="gridcellleft" style="width: 50px;" valign="top">
                                            <dxe:ASPxComboBox ID="cmbYear" Width="120px" runat="server" Font-Size="12px" ValueType="System.String"
                                                Font-Bold="False" ClientInstanceName="Year">
                                                <ButtonStyle Width="13px">
                                                </ButtonStyle>
                                                <DropDownButton Text="Year">
                                                </DropDownButton>
                                            </dxe:ASPxComboBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="cmbYear"
                                                runat="server" ErrorMessage="Invalid Year" Width="117px"></asp:RequiredFieldValidator></td>
                                        <td class="gridcellleft" style="width: 50px;" valign="top">
                                            <dxe:ASPxComboBox ID="cmbMonth" Width="120px" runat="server" Font-Size="12px" ValueType="System.String"
                                                Font-Bold="False" ClientInstanceName="Month">
                                                <Items>
                                                    <dxe:ListEditItem Text="January" Value="1" />
                                                    <dxe:ListEditItem Text="February" Value="2" />
                                                    <dxe:ListEditItem Text="March" Value="3" />
                                                    <dxe:ListEditItem Text="April" Value="4" />
                                                    <dxe:ListEditItem Text="May" Value="5" />
                                                    <dxe:ListEditItem Text="June" Value="6" />
                                                    <dxe:ListEditItem Text="July" Value="7" />
                                                    <dxe:ListEditItem Text="August" Value="8" />
                                                    <dxe:ListEditItem Text="September" Value="9" />
                                                    <dxe:ListEditItem Text="October" Value="10" />
                                                    <dxe:ListEditItem Text="November" Value="11" />
                                                    <dxe:ListEditItem Text="December" Value="12" />
                                                </Items>
                                                <ButtonStyle Width="13px">
                                                </ButtonStyle>
                                                <DropDownButton Text="Month">
                                                </DropDownButton>
                                            </dxe:ASPxComboBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="cmbMonth"
                                                runat="server" ErrorMessage="Invalid Month" Width="118px"></asp:RequiredFieldValidator></td>
                                        <td class="gridcellleft" valign="top">
                                            <dxe:ASPxComboBox ID="cmbCompany" Width="160px" runat="server" Font-Size="12px" ValueType="System.String"
                                                Font-Bold="False" ClientInstanceName="Company">
                                                <ButtonStyle Width="13px"></ButtonStyle>
                                                <DropDownButton Text="Company"></DropDownButton>
                                            </dxe:ASPxComboBox>
                                        </td>
                                        <td class="gridcellleft" valign="top">
                                            <dxe:ASPxComboBox ID="cmbBranch" Width="160px" runat="server" ValueType="System.String"
                                                Font-Size="12px" ClientInstanceName="Branch" EnableIncrementalFiltering="True"
                                                EnableTheming="False" Font-Overline="False">
                                                <ButtonStyle Width="13px"></ButtonStyle>
                                                <DropDownButton Text="Branch" Width="40px"></DropDownButton>
                                            </dxe:ASPxComboBox>
                                        </td>

                                        <td valign="top">
                                            <dxe:ASPxButton ID="btnShow" runat="server" AutoPostBack="false" Text="Show">
                                                <ClientSideEvents Click="function(s,e){btnShowClick();}"></ClientSideEvents>
                                            </dxe:ASPxButton>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr style="visibility: hidden">
                            <td style="vertical-align: top; text-align: left;">
                                <table>
                                    <tr>
                                        <td class="gridcellleft" valign="top">
                                            <span class="Ecoheadtxt" style="color: Blue"><strong>User:</strong></span></td>
                                        <td class="gridcellleft" valign="top">
                                            <dxe:ASPxRadioButtonList ID="rbUser" runat="server" SelectedIndex="1" ItemSpacing="10px"
                                                RepeatDirection="Horizontal" TextWrap="False" Font-Size="12px" ClientInstanceName="User">
                                                <Items>
                                                    <dxe:ListEditItem Text="All" Value="A" />
                                                    <dxe:ListEditItem Text="Specific" Value="S" />
                                                </Items>
                                                <ClientSideEvents ValueChanged="function(s, e) {ShowEmployeeFilterForm(s.GetValue());}" />
                                                <DisabledStyle>
                                                    <Border BorderStyle="None" BorderWidth="0px" />
                                                </DisabledStyle>
                                                <Paddings Padding="0px" />
                                                <Border BorderStyle="None" BorderWidth="0px" />
                                            </dxe:ASPxRadioButtonList>
                                        </td>
                                        <td class="gridcellleft" id="tdname" valign="top">
                                            <span class="Ecoheadtxt" style="color: Blue"><strong>Name:</strong></span></td>
                                        <td class="gridcellleft" id="tdtxtname" style="vertical-align: top">
                                            <asp:TextBox ID="txtName" runat="server" Width="252px" Font-Size="11px"></asp:TextBox>
                                            <asp:TextBox ID="txtName_hidden" runat="server" Width="14px"></asp:TextBox></td>

                                        <td class="gridcellleft" valign="top">
                                            <span style="color: Blue"><strong>Report Type:</strong></span></td>
                                        <td class="gridcellleft" valign="top">
                                            <dxe:ASPxRadioButtonList ID="RBReportType" runat="server" RepeatDirection="Horizontal"
                                                SelectedIndex="0" TextSpacing="3px" ItemSpacing="1px" ClientInstanceName="ReportType">
                                                <Items>
                                                    <dxe:ListEditItem Text="Screen" Value="Screen" />
                                                    <dxe:ListEditItem Text="Print" Value="Print" />
                                                </Items>
                                                <ValidationSettings ErrorText="Error has occurred">
                                                    <ErrorImage Width="14px" />
                                                </ValidationSettings>
                                                <Border BorderWidth="0px" />
                                                <Paddings Padding="0px" />
                                            </dxe:ASPxRadioButtonList>
                                        </td>
                                        <td class="gridcellleft" valign="top" style="width: 61px">&nbsp;</td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table class="TableMain100" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="gridcellcenter">
                                <dxe:ASPxGridView ID="ASPx_EmployeeAtdd" ClientInstanceName="ASPx_EmployeeAtdd"
                                    runat="server" AutoGenerateColumns="False" DataSourceID="SDSAttdMain" OnCustomCallback="ASPx_EmployeeAtdd_CustomCallback"
                                    Width="950px" OnHtmlDataCellPrepared="ASPx_EmployeeAtdd_HtmlDataCellPrepared"
                                    Font-Size="12px" OnCustomJSProperties="ASPx_EmployeeAtdd_CustomJSProperties">
                                    <Styles>
                                        <Header ImageSpacing="8px" SortingImageSpacing="8px">
                                        </Header>
                                        <LoadingPanel ImageSpacing="10px">
                                        </LoadingPanel>
                                    </Styles>
                                    <Columns>
                                        <dxe:GridViewDataTextColumn Caption="Employee Name" FieldName="empName" Width="200px"
                                            VisibleIndex="0" FixedStyle="Left">
                                            <CellStyle Wrap="False" CssClass="gridcellleft">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Emp. Code" FieldName="code" Width="50px" VisibleIndex="1"
                                            FixedStyle="Left">
                                            <CellStyle Wrap="False" CssClass="gridcellleft">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Company Name" FieldName="cmp_Name" Width="300px"
                                            VisibleIndex="2">
                                            <CellStyle Wrap="False" CssClass="gridcellleft">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Branch" FieldName="branch_description" Width="200px"
                                            VisibleIndex="3">
                                            <CellStyle Wrap="False" CssClass="gridcellleft">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="1" FieldName="day1" Width="10px" VisibleIndex="4">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="2" FieldName="day2" Width="10px" VisibleIndex="5">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="3" FieldName="day3" Width="10px" VisibleIndex="6">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="4" FieldName="day4" Width="10px" VisibleIndex="7">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="5" FieldName="day5" Width="10px" VisibleIndex="8">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="6" FieldName="day6" Width="10px" VisibleIndex="9">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="7" FieldName="day7" Width="10px" VisibleIndex="10">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="8" FieldName="day8" Width="10px" VisibleIndex="11">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="9" FieldName="day9" Width="10px" VisibleIndex="12">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="10" FieldName="day10" Width="10px" VisibleIndex="13">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="11" FieldName="day11" Width="10px" VisibleIndex="14">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="12" FieldName="day12" Width="10px" VisibleIndex="15">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="13" FieldName="day13" Width="10px" VisibleIndex="16">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="14" FieldName="day14" Width="10px" VisibleIndex="17">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="15" FieldName="day15" Width="10px" VisibleIndex="18">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="16" FieldName="day16" Width="10px" VisibleIndex="19">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="17" FieldName="day17" Width="10px" VisibleIndex="20">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="18" FieldName="day18" Width="10px" VisibleIndex="21">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="19" FieldName="day19" Width="10px" VisibleIndex="22">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="20" FieldName="day20" Width="10px" VisibleIndex="23">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="21" FieldName="day21" Width="10px" VisibleIndex="24">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="22" FieldName="day22" Width="10px" VisibleIndex="25">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="23" FieldName="day23" Width="10px" VisibleIndex="26">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="24" FieldName="day24" Width="10px" VisibleIndex="27">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="25" FieldName="day25" Width="10px" VisibleIndex="28">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="26" FieldName="day26" Width="10px" VisibleIndex="29">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="27" FieldName="day27" Width="10px" VisibleIndex="30">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="28" FieldName="day28" Width="10px" VisibleIndex="31">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="29" FieldName="day29" Width="10px" VisibleIndex="32">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="30" FieldName="day30" Width="10px" VisibleIndex="33">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="31" FieldName="day31" VisibleIndex="34">
                                        </dxe:GridViewDataTextColumn>
                                    </Columns>
                                    <SettingsPager PageSize="20" ShowSeparators="True">
                                        <FirstPageButton Visible="True">
                                        </FirstPageButton>
                                        <LastPageButton Visible="True">
                                        </LastPageButton>
                                    </SettingsPager>
                                    <ClientSideEvents EndCallback="function(s, e) {
	                                        LastCall(s.cpHeight);
                                        }" />
                                    <Settings ShowHorizontalScrollBar="True" />
                                    <SettingsBehavior ColumnResizeMode="NextColumn" />
                                </dxe:ASPxGridView>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="height: 22px">
                                <%--<CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true"
                                    DisplayGroupTree="False" />--%>
                                <%--<CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true"
                                    DisplayGroupTree="False" />
                                <CR:CrystalReportPartsViewer ID="CrystalReportPartsViewer1" runat="server" AutoDataBind="True"
                                    ReportSourceID="CrystalReportSource1" />
                                <CR:CrystalReportSource ID="CrystalReportSource1" runat="server">
                                    <Report FileName="Reports\AttendenceRport.rpt">
                                    </Report>
                                </CR:CrystalReportSource>--%>
                                <asp:SqlDataSource ID="SDSAttdMain" runat="server" ></asp:SqlDataSource>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
