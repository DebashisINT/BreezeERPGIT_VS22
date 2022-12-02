<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Reports_frm_attendance_PD_report" Codebehind="frm_attendance_PD_report.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />
    <script type="text/javascript" language="javascript">
    function height()
    {
        if(document.body.scrollHeight>=350)
        {
            window.frameElement.height = document.body.scrollHeight;
        }
        else
        {
            window.frameElement.height = '350px';
        }
        window.frameElement.width = document.body.scrollWidth;
    }
    </script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Leave Availed Employee Wise</h3>
        </div>

    </div> 
         <div class="form_main">
        <table>
          
            <tr>
                <td class="gridcellleft">
                    Employee ID:<asp:TextBox ID="txtID" runat="server" Width="74px"></asp:TextBox>
                </td>
                <td>
                    <asp:Button ID="btnShow" runat="server" Text="show" OnClick="btnShow_Click"></asp:Button>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="grdReport" runat="server" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField DataField="AttdMonth" HeaderText="AttdMonth" />
                            <asp:BoundField DataField="PL" HeaderText="PL" />
                            <asp:BoundField DataField="CL" HeaderText="CL" />
                            <asp:BoundField DataField="SL" HeaderText="SL" />
                            <asp:BoundField DataField="PD" HeaderText="PD" />
                            <asp:BoundField DataField="HC" HeaderText="HC" />
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
             </div>
</asp:Content>
