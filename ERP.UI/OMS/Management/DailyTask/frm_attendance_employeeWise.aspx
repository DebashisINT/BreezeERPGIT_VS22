<%@ Page Title="Attendance Employee Wise" Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.DailyTask.management_DailyTask_frm_attendance_employeeWise" CodeBehind="frm_attendance_employeeWise.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <%--  <script type="text/javascript" src="/assests/js/init.js"></script>
    <script type="text/javascript" src="/assests/js/ajax-dynamic-list.js"></script>--%>

    <script type="text/javascript" src="/assests/js/jquery.timeentry.js"></script>

    <script type="text/javascript">
        $(function () {
            var i = 0;
            //            alert('aaa');
            for (i = 0; i < Noofrows; i++) {
                if (i < 8) {
                    var no = i + 2;
                    //ID1='#ctl00_ContentPlaceHolder3_grdUserAttendace_ctl0'+no+'_txtINtime';
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
            //	    $('#ctl00_ContentPlaceHolder3_grdUserAttendace_ctl04_txtINtime').timeEntry();
            //	    $('#ctl00_ContentPlaceHolder3_grdUserAttendace_ctl04_txtOUTtime').timeEntry();

        });
        function functionBind(obj) {//alert(obj);
            $(obj).timeEntry();
        }
    </script>

    <script type="text/javascript" language="javascript">
        function PageLoad(obj) {
            document.getElementById('txtName_hidden').style.display = "none";
            Noofrows = obj;
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
        function aftersave(obj1, obj2) {
            ShowEmployeeFilterForm(obj1);
        }
        function CallAjax(obj1, obj2, obj3) {
            var cmbcompany = document.getElementById('cmbCompany_I');
            //alert(cmbcompany.value);
            var cmbbranch = document.getElementById('cmbBranch_I');
            var obj4 = cmbcompany.value + '~' + cmbbranch.value
            //alert(obj4);
            ajax_showOptions(obj1, obj2, obj3, obj4);
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Attendance Employee Wise</h3>
        </div>
    </div>
    <div class="form_main">
        <div class="row">
            <div class="col-md-2">
                <label>
                    <span class="Ecoheadtxt"><strong>&nbsp</strong></span>
                  </label>
                <dxe:ASPxComboBox ID="cmbCompany" runat="server" ValueType="System.String"
                    EnableIncrementalFiltering="true" ClientInstanceName="company" Width="100%" Height="30px">
                    <ButtonStyle>
                    </ButtonStyle>
                    <DropDownButton Text="Company">
                    </DropDownButton>
                </dxe:ASPxComboBox>
            </div>
            <div class="col-md-2">
                <label>
                    <span class="Ecoheadtxt"><strong>&nbsp</strong></span>
                  </label>
                <dxe:ASPxComboBox ID="cmbBranch" runat="server" ValueType="System.String" EnableIncrementalFiltering="True"
                    Font-Overline="False" ClientInstanceName="branch" Width="100%" Height="30px">
                    <ButtonStyle>
                    </ButtonStyle>
                    <DropDownButton Text="Branch">
                    </DropDownButton>
                </dxe:ASPxComboBox>
            </div>
            <div class="col-md-2">
                <label>
                    <span class="Ecoheadtxt"><strong>&nbsp</strong></span>
                  </label>
                <dxe:ASPxDateEdit ID="cmbFromdate" runat="server" DateOnError="Today" EditFormat="Custom"
                    UseMaskBehavior="True" NullText="From Date" Width="100%" Height="30px">
                    <ButtonStyle>
                    </ButtonStyle>
                    <DropDownButton ImagePosition="Top" Text="From">
                    </DropDownButton>
                </dxe:ASPxDateEdit>
            </div>
            <div class="col-md-2">
                <label>
                    <span class="Ecoheadtxt"><strong>&nbsp</strong></span>
                  </label>
                <dxe:ASPxDateEdit ID="cmbDate" runat="server" DateOnError="Today" EditFormat="Custom"
                    UseMaskBehavior="True" NullText="To Date" Width="100%" Height="30px">
                    <ButtonStyle>
                    </ButtonStyle>
                    <DropDownButton ImagePosition="Top" Text="To">
                    </DropDownButton>
                </dxe:ASPxDateEdit>
            </div>
            <div class="col-md-2">
                 <label style="" id="tdname">
                    <span class="Ecoheadtxt"><strong>Name:</strong></span>
                  </label>
                 <div style="" id="tdtxtname">
                    <asp:TextBox ID="txtName" runat="server" CssClass="form-control" Width="100%" Height="30px"></asp:TextBox><asp:TextBox
                        ID="txtName_hidden" runat="server" Width="14px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtName"
                        Display="Dynamic" ErrorMessage="Please select name from list!" Font-Bold="True"></asp:RequiredFieldValidator>
                 </div>
            </div>
            <div class="col-md-2">
                <label class="btn-block">
                    <span class="Ecoheadtxt"><strong>&nbsp</strong></span>
                  </label>
                <asp:Button ID="btnShow" runat="server" Text="Show" OnClick="btnShow_Click" CssClass="btn btn-primary" />
            </div>

        </div>
        <table class="TableMain100" cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <table>
                        <tr>
                            <td>
                                
                            </td>
                            <td>
                                
                            </td>
                            <td style="text-align: left">
                                
                            </td>
                            <td style="text-align: left">
                                
                            </td>
                           
                            
                            <td class="gridcellleft">
                                
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="text-align: left">
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
                        BackColor="#DDECFE" BorderStyle="Solid" BorderWidth="2px" CellPadding="4" OnRowDataBound="grdUserAttendace_RowDataBound"
                        ForeColor="#0000C0" PageSize="200" OnSorting="grdUserAttendace_Sorting" AllowSorting="True">
                        <RowStyle BackColor="#DDECFE" ForeColor="#330099" BorderColor="#E6E8F3" BorderStyle="Double"
                            BorderWidth="1px"></RowStyle>
                        <SelectedRowStyle BackColor="#E6E8F3" ForeColor="SlateBlue" Font-Bold="True"></SelectedRowStyle>
                        <PagerStyle BackColor="#E6E8F3" ForeColor="SlateBlue" HorizontalAlign="Center"></PagerStyle>
                        <HeaderStyle BackColor="LightSteelBlue" ForeColor="Black" CssClass="EHEADER" Font-Bold="True"></HeaderStyle>
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label ID="lblId" runat="server" Text='<%# Eval("datetime")%>' Visible="false"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="1px" />
                                <HeaderStyle Width="1px" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="date" HeaderText="Date" SortExpression="datetime">
                                <ItemStyle HorizontalAlign="Left" Width="150px" />
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
                    &nbsp;
                </td>
            </tr>
            <tr id="TrLeaveDate" runat="server" visible="false" align="center" style="font-weight: bold">
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="Leaving Date" ForeColor="red"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblLEaveDate" runat="server" Text="" ForeColor="red"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="gridcellcenter">
                    <dxe:ASPxButton ID="btnSave" runat="server" Text="Mark Attendance" OnClick="btnSave_Click">
                    </dxe:ASPxButton>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>


