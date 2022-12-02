<%@ Page Title="Working Hour Schedule" Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.Master.management_master_Working_Schedule_General" CodeBehind="Working_Schedule_General.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        table td {
            padding: 2px 20px 2px 0;
            /* vertical-align: top; */
        }
    </style>
    <script type="text/javascript" src="/assests/js/jquery.timeentry.js"></script>
    <script type="text/javascript">
        $(function () {
            var i = 0;
            for (i = 0; i < 7; i++) {
                ID1 = '#txtINtime' + i;
                functionBind(ID1);
                ID2 = '#txtOUTtime' + i;
                functionBind(ID2);
            }
            //	    $('#ctl00_ContentPlaceHolder3_grdUserAttendace_ctl04_txtINtime').timeEntry();
            //	    $('#ctl00_ContentPlaceHolder3_grdUserAttendace_ctl04_txtOUTtime').timeEntry();

        });
        function functionBind(obj) {
            $(obj).timeEntry();
        }
    </script>

    <script type="text/javascript" language="javascript">
        function PageLoad() {

            showHideField('1', chkSun);
            showHideField('2', chkMon);
            showHideField('3', chktues);
            showHideField('4', chkwed);
            showHideField('5', chkthur);
            showHideField('6', chkfri);
            showHideField('7', chksat);
        }
        function showHideField(obj1, obj2) {
            //debugger;
            //alert(obj1); alert(obj2);
            //var checkBox = document.getElementById(obj2);
            //var checkBox = obj2.replace("_S", "");
            //alert(checkBox);
            //alert(obj2.GetChecked());
            var id = 'td' + obj1;
            //alert(id);
            if (obj2.GetChecked()) {
                var id1 = id + 'a';
                show(id1);
                id1 = id + 'b';
                show(id1);
                id1 = id + 'c';
                show(id1);

            }
            else {
                var id1 = id + 'a';
                hide(id1);
                id1 = id + 'b';
                hide(id1);
                id1 = id + 'c';
                hide(id1);
            }
        }
        function show(obj1) {
            //document.getElementById(obj1).style.display = 'inline';
            document.getElementById(obj1).style.visibility = 'visible';

        }
        function hide(obj1) {
            //document.getElementById(obj1).style.display = 'none';
            document.getElementById(obj1).style.visibility = 'hidden';
        }
        function validatefields() {
            if (document.getElementById("txtSheduleName").value == "") {
                jAlert("Please enter the schedule name.");
                document.getElementById("txtSheduleName").focus();
                return false;
            }
        }
        Fieldname = 'pnl';
    </script>
    <script type="text/javascript">
        $(function () {
            jAlert(1);
            $('.rightSide').removeAttr('style');
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>&nbsp;Add/Edit Working Shedule</h3>
            <div class="crossBtn"><a href="frm_workingShedule.aspx"><i class="fa fa-times"></i></a></div>
        </div>
    </div>
    <div class="form_main">
        <table class="TableMain100">
            <%--                <tr>
                    <td class="EHEADER" style="text-align: center">
                        <span style="color: black"><strong>Working Shedule</strong></span>
                    </td>
                </tr>--%>
            <tr>
                <td><%-- style="text-align: center"--%>
                    <table>
                        <tr>
                            <td style="text-align: left;">
                                <span class="Ecoheadtxt" style="color: black"><strong>Name:</strong><span style="color: red">*</span></span>
                            </td>
                            <td style="padding-left: 34px;">

                                <asp:TextBox ID="txtSheduleName" runat="server" MaxLength="50" ></asp:TextBox>


                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtSheduleName"
                                    Display="Dynamic" ErrorMessage="Mandatory!" Font-Bold="True" ForeColor="Red"></asp:RequiredFieldValidator>

                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td><%--style="text-align: center;"--%>
                    <table cellpadding="0px" cellspacing="0px">
                        <%--style="border: solid 3px #EBDAAB"--%>
                        <tr>
                            <td>
                                <table>
                                    <%--style="border: solid 1px #DCBE6A"--%>
                                    <tr>
                                        <td></td>
                                        <td style="text-align: center;">
                                            <span class="Ecoheadtxt" style="color: black"><strong>Is Working Day?</strong></span>
                                        </td>
                                        <td >
                                            <span class="Ecoheadtxt" style="color: black"><strong>Day Begin</strong></span></td>
                                        <td >
                                            <span class="Ecoheadtxt" style="color: black"><strong>Day End</strong></span></td>
                                        <td style="text-align: center;">
                                            <span class="Ecoheadtxt" style="color: black"><strong>Total Break</strong></span></td>
                                    </tr>
                                    <tr>
                                        <td class="gridcellleft">
                                            <span class="Ecoheadtxt" style="color: black"><strong>Monday:</strong></span>
                                        </td>
                                        <td class="gridcellleft">
                                            <dxe:ASPxCheckBox ID="chkMon" runat="server" ClientIDMode="Static">
                                                <ClientSideEvents CheckedChanged="function(s, e) {
	showHideField('2',s);}" />
                                            </dxe:ASPxCheckBox>
                                        </td>
                                        <td class="gridcellcenter" id="td2a">
                                            <asp:TextBox ID="txtINtime0" runat="server" Width="84px"></asp:TextBox>
                                        </td>
                                        <td class="gridcellcenter" id="td2b">
                                            <asp:TextBox ID="txtOUTtime0" runat="server" Width="84px" /></td>
                                        <td class="gridcellcenter" id="td2c">
                                            <asp:DropDownList ID="cmbl2" runat="server" Font-Size="12px">
                                                <asp:ListItem>30</asp:ListItem>
                                                <asp:ListItem>40</asp:ListItem>
                                                <asp:ListItem>45</asp:ListItem>
                                                <asp:ListItem>50</asp:ListItem>
                                                <asp:ListItem>60</asp:ListItem>
                                                <asp:ListItem>70</asp:ListItem>
                                                <asp:ListItem>75</asp:ListItem>
                                                <asp:ListItem>80</asp:ListItem>
                                                <asp:ListItem>85</asp:ListItem>
                                                <asp:ListItem>90</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="gridcellleft">
                                            <span class="Ecoheadtxt" style="color: black"><strong>Tuesday:</strong></span>
                                        </td>
                                        <td class="gridcellleft">
                                            <dxe:ASPxCheckBox ID="chktues" runat="server" Text="">
                                                <ClientSideEvents CheckedChanged="function(s, e) {
	showHideField('3',s);}" />
                                            </dxe:ASPxCheckBox>
                                        </td>
                                        <td class="gridcellcenter" id="td3a">
                                            <asp:TextBox ID="txtINtime1" runat="server" Width="84px"></asp:TextBox></td>
                                        <td class="gridcellcenter" id="td3b">
                                            <asp:TextBox ID="txtOUTtime1" runat="server" Width="84px" /></td>
                                        <td class="gridcellcenter" id="td3c">
                                            <asp:DropDownList ID="cmbl3" runat="server" Font-Size="12px">
                                                <asp:ListItem>30</asp:ListItem>
                                                <asp:ListItem>40</asp:ListItem>
                                                <asp:ListItem>45</asp:ListItem>
                                                <asp:ListItem>50</asp:ListItem>
                                                <asp:ListItem>60</asp:ListItem>
                                                <asp:ListItem>70</asp:ListItem>
                                                <asp:ListItem>75</asp:ListItem>
                                                <asp:ListItem>80</asp:ListItem>
                                                <asp:ListItem>85</asp:ListItem>
                                                <asp:ListItem>90</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="gridcellleft">
                                            <span class="Ecoheadtxt" style="color: black"><strong>Wednesday:</strong></span>
                                        </td>
                                        <td class="gridcellleft">
                                            <dxe:ASPxCheckBox ID="chkwed" runat="server" Text="">
                                                <ClientSideEvents CheckedChanged="function(s, e) {
	showHideField('4',s);}" />
                                            </dxe:ASPxCheckBox>
                                        </td>
                                        <td class="gridcellcenter" id="td4a">
                                            <asp:TextBox ID="txtINtime2" runat="server" Width="84px"></asp:TextBox></td>
                                        <td class="gridcellcenter" id="td4b">
                                            <asp:TextBox ID="txtOUTtime2" runat="server" Width="84px" /></td>
                                        <td class="gridcellcenter" id="td4c">
                                            <asp:DropDownList ID="cmbl4" runat="server" Font-Size="12px">
                                                <asp:ListItem>30</asp:ListItem>
                                                <asp:ListItem>40</asp:ListItem>
                                                <asp:ListItem>45</asp:ListItem>
                                                <asp:ListItem>50</asp:ListItem>
                                                <asp:ListItem>60</asp:ListItem>
                                                <asp:ListItem>70</asp:ListItem>
                                                <asp:ListItem>75</asp:ListItem>
                                                <asp:ListItem>80</asp:ListItem>
                                                <asp:ListItem>85</asp:ListItem>
                                                <asp:ListItem>90</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="gridcellleft">
                                            <span class="Ecoheadtxt" style="color: black"><strong>Thursday:</strong></span>
                                        </td>
                                        <td class="gridcellleft">
                                            <dxe:ASPxCheckBox ID="chkthur" runat="server" Text="">
                                                <ClientSideEvents CheckedChanged="function(s, e) {
	showHideField('5',s);}" />
                                            </dxe:ASPxCheckBox>
                                        </td>
                                        <td class="gridcellcenter" id="td5a">
                                            <asp:TextBox ID="txtINtime3" runat="server" Width="84px"></asp:TextBox></td>
                                        <td class="gridcellcenter" id="td5b">
                                            <asp:TextBox ID="txtOUTtime3" runat="server" Width="84px" /></td>
                                        <td class="gridcellcenter" id="td5c">
                                            <asp:DropDownList ID="cmbl5" runat="server" Font-Size="12px">
                                                <asp:ListItem>30</asp:ListItem>
                                                <asp:ListItem>40</asp:ListItem>
                                                <asp:ListItem>45</asp:ListItem>
                                                <asp:ListItem>50</asp:ListItem>
                                                <asp:ListItem>60</asp:ListItem>
                                                <asp:ListItem>70</asp:ListItem>
                                                <asp:ListItem>75</asp:ListItem>
                                                <asp:ListItem>80</asp:ListItem>
                                                <asp:ListItem>85</asp:ListItem>
                                                <asp:ListItem>90</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="gridcellleft">
                                            <span class="Ecoheadtxt" style="color: black"><strong>Friday:</strong></span>
                                        </td>
                                        <td class="gridcellleft">
                                            <dxe:ASPxCheckBox ID="chkfri" runat="server" Text="">
                                                <ClientSideEvents CheckedChanged="function(s, e) {
	showHideField('6',s);}" />
                                            </dxe:ASPxCheckBox>
                                        </td>
                                        <td class="gridcellcenter" id="td6a">
                                            <asp:TextBox ID="txtINtime4" runat="server" Width="84px"></asp:TextBox></td>
                                        <td class="gridcellcenter" id="td6b">
                                            <asp:TextBox ID="txtOUTtime4" runat="server" Width="84px" /></td>
                                        <td class="gridcellcenter" id="td6c">
                                            <asp:DropDownList ID="cmbl6" runat="server" Font-Size="12px">
                                                <asp:ListItem>30</asp:ListItem>
                                                <asp:ListItem>40</asp:ListItem>
                                                <asp:ListItem>45</asp:ListItem>
                                                <asp:ListItem>50</asp:ListItem>
                                                <asp:ListItem>60</asp:ListItem>
                                                <asp:ListItem>70</asp:ListItem>
                                                <asp:ListItem>75</asp:ListItem>
                                                <asp:ListItem>80</asp:ListItem>
                                                <asp:ListItem>85</asp:ListItem>
                                                <asp:ListItem>90</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="gridcellleft">
                                            <span class="Ecoheadtxt" style="color: black"><strong>Saturday:</strong></span>
                                        </td>
                                        <td class="gridcellleft">
                                            <dxe:ASPxCheckBox ID="chksat" runat="server" Text="">
                                                <ClientSideEvents CheckedChanged="function(s, e) {
	showHideField('7',s);}" />
                                            </dxe:ASPxCheckBox>
                                        </td>
                                        <td class="gridcellcenter" id="td7a">
                                            <asp:TextBox ID="txtINtime5" runat="server" Width="84px"></asp:TextBox></td>
                                        <td class="gridcellcenter" id="td7b">
                                            <asp:TextBox ID="txtOUTtime5" runat="server" Width="84px" /></td>
                                        <td class="gridcellcenter" id="td7c">
                                            <asp:DropDownList ID="cmbl7" runat="server" Font-Size="12px">
                                                <asp:ListItem>30</asp:ListItem>
                                                <asp:ListItem>40</asp:ListItem>
                                                <asp:ListItem>45</asp:ListItem>
                                                <asp:ListItem>50</asp:ListItem>
                                                <asp:ListItem>60</asp:ListItem>
                                                <asp:ListItem>70</asp:ListItem>
                                                <asp:ListItem>75</asp:ListItem>
                                                <asp:ListItem>80</asp:ListItem>
                                                <asp:ListItem>85</asp:ListItem>
                                                <asp:ListItem>90</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="gridcellleft">
                                            <span class="Ecoheadtxt" style="color: black"><strong>Sunday:</strong></span>
                                        </td>
                                        <td class="gridcellleft">
                                            <dxe:ASPxCheckBox ID="chkSun" runat="server" Text="">
                                                <ClientSideEvents CheckedChanged="function(s, e) {
	showHideField('1',s);}" />
                                            </dxe:ASPxCheckBox>
                                        </td>
                                        <td class="gridcellcenter" id="td1a">
                                            <asp:TextBox ID="txtINtime6" runat="server" Width="84px"></asp:TextBox></td>
                                        <td class="gridcellcenter" id="td1b">
                                            <asp:TextBox ID="txtOUTtime6" runat="server" Width="84px" /></td>
                                        <td class="gridcellcenter" id="td1c">
                                            <asp:DropDownList ID="cmbl1" runat="server" Font-Size="12px">
                                                <asp:ListItem>30</asp:ListItem>
                                                <asp:ListItem>40</asp:ListItem>
                                                <asp:ListItem>45</asp:ListItem>
                                                <asp:ListItem>50</asp:ListItem>
                                                <asp:ListItem>60</asp:ListItem>
                                                <asp:ListItem>70</asp:ListItem>
                                                <asp:ListItem>75</asp:ListItem>
                                                <asp:ListItem>80</asp:ListItem>
                                                <asp:ListItem>85</asp:ListItem>
                                                <asp:ListItem>90</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="5" class="gridcellright" style="padding-left: 102px;">
                                            <dxe:ASPxButton ID="btnSubmit" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnSubmit_Click">
                                            </dxe:ASPxButton>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>


                </td>
            </tr>
        </table>
    </div>
</asp:Content>
