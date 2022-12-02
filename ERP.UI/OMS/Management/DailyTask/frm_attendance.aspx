<%@ Page Title="Date Wise Attendance" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.DailyTask.management_DailyTask_frm_attendance" CodeBehind="frm_attendance.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe000001" %>

<%@ Register assembly="DevExpress.Web.v15.1" namespace="DevExpress.Web" tagprefix="dx" %>--%>
    

    

    <!--___________________These files are for List Items__________________________-->

    <script type="text/javascript" src="/assests/js/init.js"></script>

    <script type="text/javascript" src="/assests/js/ajax-dynamic-list.js"></script>

    <!--___________________________________________________________________________-->

    <script type="text/javascript" src="/assests/js/jquery-1.3.1.js"></script>

    <script type="text/javascript" src="/assests/js/jquery.timeentry.js"></script>

    <script type="text/javascript">
        $(function () {
            var i = 0;
            //alert('aaa');
            if (Noofrows != 'n') {
                for (i = 0; i < Noofrows; i++) {
                    if (i < 8) {
                        var no = i + 2;
                        ID1 = '#grdUserAttendace_ctl0' + no + '_txtINtime';
                        functionBind(ID1);
                        ID2 = '#grdUserAttendace_ctl0' + no + '_txtOUTtime';
                        functionBind(ID2);
                    }
                    else {
                        var no = i + 2;
                        ID1 = '#grdUserAttendace_ctl' + no + '_txtINtime';
                        functionBind(ID1);
                        ID2 = '#grdUserAttendace_ctl' + no + '_txtOUTtime';
                        functionBind(ID2);
                    }

                }
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
            FieldName = 'btnShow';
            document.getElementById('txtName_hidden').style.display = "none";
            ShowEmployeeFilterForm('A');
            Noofrows = 'n';
            height();
        }
        function height() {
            if (document.body.scrollHeight >= 350) {
                window.frameElement.height = document.body.scrollHeight;
            }
            else {
                window.frameElement.height = '350px';
            }
            window.frameElement.widht = document.body.scrollWidht;
        }
        function NoOfRows(obj) {
            //alert(obj);
            Noofrows = obj;

            document.getElementById('txtName_hidden').style.display = "none";
        }
        function ShowEmployeeFilterForm(obj) {//alert(obj);
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
        function aftersave(obj1, obj2) {
            ShowEmployeeFilterForm(obj1);
        }
        function CallAjax(obj1, obj2, obj3) {
            var cmbcompany = document.getElementById('cmbCompany_I');
            var cmbbranch = document.getElementById('cmbBranch_I');
            var obj4 = cmbcompany.value + '~' + cmbbranch.value
            //alert(obj4);
            ajax_showOptions(obj1, obj2, obj3, obj4);
        }
        function SignOff() {
            parent.window.SignOff();
        }
    </script>

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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="panel-heading">
       <div class="panel-title">
           <h3>Attendance</h3>
       </div>

   </div> 
     <div class="form_main">
    <table class="TableMain100">
       
        <tr>
            <td>
                <table class="TableMain100">
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <dxe:ASPxComboBox ID="cmbCompany" runat="server" Font-Size="12px" ValueType="System.String"
                                            Font-Bold="False">
                                            <ButtonStyle Width="13px">
                                            </ButtonStyle>
                                            <DropDownButton Text="Company">
                                            </DropDownButton>
                                        </dxe:ASPxComboBox>
                                    </td>
                                    <td class="gridcellleft" style="width: 50px">
                                        <dxe:ASPxComboBox ID="cmbBranch" runat="server" ValueType="System.String" Width="130px"
                                            Font-Size="12px" EnableIncrementalFiltering="True" EnableTheming="False" Height="10px"
                                            Font-Overline="False">
                                            <ButtonStyle Width="13px">
                                            </ButtonStyle>
                                            <DropDownButton Text="Branch" Width="40px">
                                            </DropDownButton>
                                        </dxe:ASPxComboBox>
                                    </td>
                                    <td style="width: 81px; text-align: left">
                                        <dxe:ASPxDateEdit ID="cmbDate" runat="server" DateOnError="Today" EditFormat="Custom"
                                            UseMaskBehavior="true" Font-Size="12px" Width="100px">
                                            <ButtonStyle Width="13px">
                                            </ButtonStyle>
                                            <DropDownButton ImagePosition="Top" Text="Date" Width="30px">
                                            </DropDownButton>
                                        </dxe:ASPxDateEdit>
                                    </td>
                                    <td style="text-align: right; width: 41px;">
                                        <span class="Ecoheadtxt" style="color: Blue"><strong>User:</strong></span>
                                    </td>
                                    <td style="width: 136px">
                                        <dxe:ASPxRadioButtonList ID="rbUser" runat="server" SelectedIndex="0" ItemSpacing="10px"
                                            RepeatDirection="Horizontal" TextWrap="False" Font-Size="12px">
                                            <Items>
                                                <dxe:ListEditItem Text="All" Value="A" />
                                                <dxe:ListEditItem Text="Specific" Value="S" />
                                            </Items>
                                            <ClientSideEvents ValueChanged="function(s, e) {
	ShowEmployeeFilterForm(s.GetValue());}" />
                                            <Border BorderWidth="0px" />
                                        </dxe:ASPxRadioButtonList>
                                    </td>
                                    <td class="gridcellright" style="width: 27px" id="tdname">
                                        <span class="Ecoheadtxt" style="color: Blue"><strong>Name:</strong></span>
                                    </td>
                                    <td class="gridcellleft" style="width: 164px" id="tdtxtname">
                                        <asp:TextBox ID="txtName" runat="server" Width="252px" Font-Size="11px"></asp:TextBox><asp:TextBox
                                            ID="txtName_hidden" runat="server" Width="14px"></asp:TextBox>
                                    </td>
                                    <td class="gridcellright" style="width: 94px">
                                        <dxe:ASPxButton ID="btnShow" runat="server" Text="Show" OnClick="btnShow_Click">
                                        </dxe:ASPxButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="gridcellleft">
                            <strong>Status</strong>: <strong style="color: Maroon">P</strong> – present ;<strong
                                style="color: Maroon">OV</strong> – Official Visit ;<strong style="color: maroon">OD</strong>-Official
                                Delay;<strong style="color: maroon">PD</strong>-Personal Delay;<strong style="color: maroon">A</strong>
                            – Absent or Leave without Pay; <strong>PL</strong>– Privilege Leave; <strong style="color: Maroon">CL</strong> – Casual Leave; <strong style="color: Maroon">SL</strong> – Sick
                                Leave; <strong style="color: Maroon">HC </strong>– Half day(Casual);<strong style="color: Maroon">HS
                                </strong>– Half day(Sick); <strong style="color: Maroon">WO</strong> – weekly Off;
                                <strong style="color: Maroon">PH</strong>– Paid holiday; <strong style="color: Maroon">CO</strong>– Compensatory off.
                        </td>
                    </tr>
                    <tr>
                        <td class="gridcellcenter">
                            <asp:GridView ID="grdUserAttendace" runat="Server" AutoGenerateColumns="False" BorderColor="CornflowerBlue"
                                BackColor="#DDECFE" BorderStyle="Solid" BorderWidth="2px" CellPadding="4" Width="100%"
                                OnRowDataBound="grdUserAttendace_RowDataBound" ForeColor="#0000C0" PageSize="200"
                                OnSorting="grdUserAttendace_Sorting" AllowSorting="True">
                                <RowStyle BackColor="#DDECFE" ForeColor="#330099" BorderColor="#E6E8F3" BorderStyle="Double"
                                    BorderWidth="1px"></RowStyle>
                                <SelectedRowStyle BackColor="#E6E8F3" ForeColor="SlateBlue" Font-Bold="True"></SelectedRowStyle>
                                <PagerStyle BackColor="#E6E8F3" ForeColor="SlateBlue" HorizontalAlign="Center"></PagerStyle>
                                <HeaderStyle BackColor="LightSteelBlue" ForeColor="Black" CssClass="EHEADER" Font-Bold="True"></HeaderStyle>
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Label ID="lblId" runat="server" Text='<%# Eval("ID")%>' Visible="false"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="1px" />
                                        <HeaderStyle Width="1px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="name" HeaderText="Name" SortExpression="name">
                                        <ItemStyle HorizontalAlign="Left" Width="150px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="EMPID" HeaderText="ID" SortExpression="EMPID">
                                        <ItemStyle HorizontalAlign="Left" Width="50px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="branch" HeaderText="Branch">
                                        <ItemStyle HorizontalAlign="Left" Width="200px" />
                                        <HeaderStyle />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Status">
                                        <ItemStyle HorizontalAlign="Center" Width="40px" />
                                        <ItemTemplate>
                                            <asp:DropDownList ID="cmbStatus" runat="server" AppendDataBoundItems="True" Width="60px"
                                                Font-Size="12px">
                                                <asp:ListItem Value="P">P</asp:ListItem>
                                                <asp:ListItem Value="OV">OV</asp:ListItem>
                                                <asp:ListItem Value="OD">OD</asp:ListItem>
                                                <asp:ListItem Value="PD">PD</asp:ListItem>
                                                <asp:ListItem Value="A">A</asp:ListItem>
                                                <asp:ListItem Value="PL">PL</asp:ListItem>
                                                <asp:ListItem Value="CL">CL</asp:ListItem>
                                                <asp:ListItem Value="SL">SL</asp:ListItem>
                                                <asp:ListItem Value="HC">HC</asp:ListItem>
                                                <asp:ListItem Value="HS">HS</asp:ListItem>
                                                <asp:ListItem Value="WO">WO</asp:ListItem>
                                                <asp:ListItem Value="PH">PH</asp:ListItem>
                                                <asp:ListItem Value="CO">CO</asp:ListItem>
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="In Time">
                                        <ItemStyle HorizontalAlign="Center" Width="70px" />
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtINtime" runat="server" Width="60px"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Out Time">
                                        <ItemStyle HorizontalAlign="Center" Width="70px" />
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtOUTtime" runat="server" Width="60px"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <dxe:ASPxButton ID="btnSave" runat="server" Text="Mark Attendance" OnClick="btnSave_Click">
                            </dxe:ASPxButton>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
         </div>
</asp:Content>


