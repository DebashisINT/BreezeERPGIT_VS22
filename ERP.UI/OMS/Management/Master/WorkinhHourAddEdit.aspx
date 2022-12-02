<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WorkinhHourAddEdit.aspx.cs"
    Inherits="ERP.OMS.Management.Master.WorkinhHourAddEdit" MasterPageFile="~/OMS/MasterPage/ERP.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style>
        th {
            width: 190px !important;
        }

        .mTop15 {
            margin-top: 15px;
        }

        .w400 {
            width: 400px;
        }
        .table thead>tr>th, .table tbody>tr>th, .table tfoot>tr>th, .table thead>tr>td, .table tbody>tr>td, .table tfoot>tr>td {
    padding: 0px;
    line-height: 1.428571429;
    vertical-align: top;
    border-top: 1px solid #ddd;
}
        .table-bordered>thead > tr > th, 
        .table-bordered>tbody > tr > th, 
        .table-bordered>tfoot > tr > th, 
        .table-bordered thead > tr > td, 
        .table-bordered >tbody > tr > td, 
        .table-bordered >tfoot > tr > td {
            padding:8px;
        }
        .minput>table>tbody>tr>td {
            border-top:none !important;
        }
        .minput>table>tbody>tr>td >div {
            padding:2px 4px;
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="Js/WorkinhHourAddEdit.js?v=0.02"></script>



    <div class="panel-heading">
        <div class="panel-title">
            <h3>&nbsp;Add/Edit Working Roster</h3>
            <div class="crossBtn"><a href="frm_workingShedule.aspx"><i class="fa fa-times"></i></a></div>
        </div>
    </div>
    <div class="form_main">
        <label>Roster Name</label>
        <dxe:ASPxTextBox ID="txtName" runat="server" Width="170px" ClientInstanceName="ctxtName"></dxe:ASPxTextBox>



        <table class="table table-bordered mTop15 w400">
            <tr>

                <th></th>
                <th>Is Working Day?</th>
                <th style="width: 190px">Day Begin</th>
                <th style="width: 190px">Day End</th>
                <th style="width: 190px; display: none">Total Break</th>
                <th style="width: 190px">Grace Time (Minute)</th>
            </tr>
            <tr>

                <td>Monday:</td>
                <td>
                    <dxe:ASPxCheckBox ID="chkMonday" ClientInstanceName="cchkMonday" runat="server" ClientSideEvents-CheckedChanged="mondayChkChange"></dxe:ASPxCheckBox>
                </td>
                <td class="tdMonday minput">
                    <dxe:ASPxTimeEdit ID="beginMonday" runat="server" Width="190" EditFormat="Custom"
                        EditFormatString="h:mm tt" DisplayFormatString="h:mm tt" ClientInstanceName="cbeginMonday">
                        <ClearButton DisplayMode="OnHover"></ClearButton>
                        <ValidationSettings ErrorDisplayMode="None" />
                    </dxe:ASPxTimeEdit>
                </td>
                <td class="tdMonday minput">
                    <dxe:ASPxTimeEdit ID="Endmonday" runat="server" Width="190" EditFormat="Custom"
                        EditFormatString="h:mm tt" DisplayFormatString="h:mm tt" ClientInstanceName="cEndmonday">
                        <ClearButton DisplayMode="OnHover"></ClearButton>
                        <ValidationSettings ErrorDisplayMode="None" />
                    </dxe:ASPxTimeEdit>
                </td>
                <td class="tdMonday hide minput">
                    <dxe:ASPxTimeEdit ID="brkMonday" runat="server" Width="190" EditFormat="Custom"
                        EditFormatString="h:mm tt" DisplayFormatString="h:mm tt" ClientInstanceName="cbrkMonday">
                        <ClearButton DisplayMode="OnHover"></ClearButton>
                        <ValidationSettings ErrorDisplayMode="None" />
                    </dxe:ASPxTimeEdit>
                </td>
                <td class="tdMonday minput">
                    <dxe:ASPxTextBox ID="graceMonday" runat="server" Width="170px">
                        <MaskSettings Mask="&lt;0..999&gt;" AllowMouseWheel="false" />
                    </dxe:ASPxTextBox>
                </td>

            </tr>

            <tr>

                <td>Tuesday:</td>
                <td>
                    <dxe:ASPxCheckBox ID="chktuesday" ClientInstanceName="cchktuesday" runat="server" 
                        ClientSideEvents-CheckedChanged="tuesdayCheckChange"></dxe:ASPxCheckBox>
                </td>
                <td class="tdTuesday minput">
                    <dxe:ASPxTimeEdit ID="BeginTuesDay" runat="server" Width="190" EditFormat="Custom"
                        EditFormatString="h:mm tt" DisplayFormatString="h:mm tt" ClientInstanceName="cBeginTuesDay">
                        <ClearButton DisplayMode="OnHover"></ClearButton>
                        <ValidationSettings ErrorDisplayMode="None" />
                    </dxe:ASPxTimeEdit>
                </td>
                <td class="tdTuesday minput">
                    <dxe:ASPxTimeEdit ID="endTuesDay" runat="server" Width="190" EditFormat="Custom"
                        EditFormatString="h:mm tt" DisplayFormatString="h:mm tt" ClientInstanceName="cendTuesDay">
                        <ClearButton DisplayMode="OnHover"></ClearButton>
                        <ValidationSettings ErrorDisplayMode="None" />
                    </dxe:ASPxTimeEdit>
                </td> 
                <td class="tdTuesday hide minput">
                    <dxe:ASPxTimeEdit ID="brkTuesDay" runat="server" Width="190" EditFormat="Custom"
                        EditFormatString="h:mm tt" DisplayFormatString="h:mm tt" ClientInstanceName="cbrkTuesDay">
                        <ClearButton DisplayMode="OnHover"></ClearButton>
                        <ValidationSettings ErrorDisplayMode="None" />
                    </dxe:ASPxTimeEdit>
                </td>

                 <td class="tdTuesday minput">
                    <dxe:ASPxTextBox ID="graceTuesday" runat="server" Width="170px">
                        <MaskSettings Mask="&lt;0..999&gt;" AllowMouseWheel="false" />
                    </dxe:ASPxTextBox>
                </td>






            </tr>



            <tr>

                <td>Wednesday:</td>
                <td>
                    <dxe:ASPxCheckBox ID="chkWednesday" ClientInstanceName="cchkWednesday" runat="server" ClientSideEvents-CheckedChanged="WednesdayCheckChange"></dxe:ASPxCheckBox>
                </td>
                <td class="tdWednesday minput">
                    <dxe:ASPxTimeEdit ID="beginWednesday" runat="server" Width="190" EditFormat="Custom"
                        EditFormatString="h:mm tt" DisplayFormatString="h:mm tt" ClientInstanceName="cbeginWednesday">
                        <ClearButton DisplayMode="OnHover"></ClearButton>
                        <ValidationSettings ErrorDisplayMode="None" />
                    </dxe:ASPxTimeEdit>
                </td>
                <td class="tdWednesday minput">
                    <dxe:ASPxTimeEdit ID="endWednesday" runat="server" Width="190" EditFormat="Custom"
                        EditFormatString="h:mm tt" DisplayFormatString="h:mm tt" ClientInstanceName="cendWednesday">
                        <ClearButton DisplayMode="OnHover"></ClearButton>
                        <ValidationSettings ErrorDisplayMode="None" />
                    </dxe:ASPxTimeEdit>
                </td>
                <td class="tdWednesday hide minput">
                    <dxe:ASPxTimeEdit ID="brkWednesday" runat="server" Width="190" EditFormat="Custom"
                        EditFormatString="h:mm tt" DisplayFormatString="h:mm tt" ClientInstanceName="cbrkWednesday">
                        <ClearButton DisplayMode="OnHover"></ClearButton>
                        <ValidationSettings ErrorDisplayMode="None" />
                    </dxe:ASPxTimeEdit>
                </td>
                 <td class="tdWednesday minput">
                    <dxe:ASPxTextBox ID="graceWednesday" runat="server" Width="170px">
                        <MaskSettings Mask="&lt;0..999&gt;" AllowMouseWheel="false" />
                    </dxe:ASPxTextBox>
                </td>



            </tr>


            <tr>

                <td>Thursday:</td>
                <td>
                    <dxe:ASPxCheckBox ID="chkThursday" ClientInstanceName="cchkThursday" runat="server" ClientSideEvents-CheckedChanged="thursdayCheckChange"></dxe:ASPxCheckBox>
                </td>
                <td class="tdthursday minput">

                    <dxe:ASPxTimeEdit ID="beginThursday" runat="server" Width="190" EditFormat="Custom"
                        EditFormatString="h:mm tt" DisplayFormatString="h:mm tt" ClientInstanceName="cbeginThursday">
                        <ClearButton DisplayMode="OnHover"></ClearButton>
                        <ValidationSettings ErrorDisplayMode="None" />
                    </dxe:ASPxTimeEdit>
                </td>
                <td class="tdthursday minput">
                    <dxe:ASPxTimeEdit ID="endThursday" runat="server" Width="190" EditFormat="Custom"
                        EditFormatString="h:mm tt" DisplayFormatString="h:mm tt" ClientInstanceName="cendThursday">
                        <ClearButton DisplayMode="OnHover"></ClearButton>
                        <ValidationSettings ErrorDisplayMode="None" />
                    </dxe:ASPxTimeEdit>
                </td>
                <td class="tdthursday hide minput">
                    <dxe:ASPxTimeEdit ID="brkThursday" runat="server" Width="190" EditFormat="Custom"
                        EditFormatString="h:mm tt" DisplayFormatString="h:mm tt" ClientInstanceName="cbrkThursday">
                        <ClearButton DisplayMode="OnHover"></ClearButton>
                        <ValidationSettings ErrorDisplayMode="None" />
                    </dxe:ASPxTimeEdit>
                </td>
                 <td class="tdthursday minput">
                    <dxe:ASPxTextBox ID="gracethursday" runat="server" Width="170px">
                        <MaskSettings Mask="&lt;0..999&gt;" AllowMouseWheel="false" />
                    </dxe:ASPxTextBox>
                </td>



            </tr>


            <tr>

                <td>Friday:</td>
                <td>
                    <dxe:ASPxCheckBox ID="chkFriday" ClientInstanceName="cchkFriday" runat="server"
                        ClientSideEvents-CheckedChanged="friDayCheckChange">
                    </dxe:ASPxCheckBox>
                </td>
                <td class="tdFriday minput">
                    <dxe:ASPxTimeEdit ID="beginFriday" runat="server" Width="190" EditFormat="Custom"
                        EditFormatString="h:mm tt" DisplayFormatString="h:mm tt" ClientInstanceName="cbeginFriday">
                        <ClearButton DisplayMode="OnHover"></ClearButton>
                        <ValidationSettings ErrorDisplayMode="None" />
                    </dxe:ASPxTimeEdit>
                </td>
                <td class="tdFriday minput">
                    <dxe:ASPxTimeEdit ID="endFriday" runat="server" Width="190" EditFormat="Custom"
                        EditFormatString="h:mm tt" DisplayFormatString="h:mm tt" ClientInstanceName="cendFriday">
                        <ClearButton DisplayMode="OnHover"></ClearButton>
                        <ValidationSettings ErrorDisplayMode="None" />
                    </dxe:ASPxTimeEdit>
                </td>
                <td class="tdFriday hide minput">
                    <dxe:ASPxTimeEdit ID="brkFriday" runat="server" Width="190" EditFormat="Custom"
                        EditFormatString="h:mm tt" DisplayFormatString="h:mm tt" ClientInstanceName="cbrkFriday">
                        <ClearButton DisplayMode="OnHover"></ClearButton>
                        <ValidationSettings ErrorDisplayMode="None" />
                    </dxe:ASPxTimeEdit>
                </td>
                <td class="tdFriday minput">
                    <dxe:ASPxTextBox ID="graceFridDay" runat="server" Width="170px">
                        <MaskSettings Mask="&lt;0..999&gt;" AllowMouseWheel="false" />
                    </dxe:ASPxTextBox>
                </td>


            </tr>

            <tr>

                <td>Saturday:</td>
                <td>
                    <dxe:ASPxCheckBox ID="chkSaturday" ClientInstanceName="cchkSaturday" runat="server"
                        ClientSideEvents-CheckedChanged="SaturdayCheckChange">
                    </dxe:ASPxCheckBox>
                </td>
                <td class="tdSaturday minput">
                    <dxe:ASPxTimeEdit ID="beginSaturday" runat="server" Width="190" EditFormat="Custom"
                        EditFormatString="h:mm tt" DisplayFormatString="h:mm tt" ClientInstanceName="cbeginSaturday">
                        <ClearButton DisplayMode="OnHover"></ClearButton>
                        <ValidationSettings ErrorDisplayMode="None" />
                    </dxe:ASPxTimeEdit>
                </td>
                <td class="tdSaturday minput">
                    <dxe:ASPxTimeEdit ID="endSaturday" runat="server" Width="190" EditFormat="Custom"
                        EditFormatString="h:mm tt" DisplayFormatString="h:mm tt" ClientInstanceName="cendSaturday" Height="22">
                        <ClearButton DisplayMode="OnHover"></ClearButton>
                        <ValidationSettings ErrorDisplayMode="None" />
                    </dxe:ASPxTimeEdit>
                </td>
                <td class="tdSaturday hide minput">
                    <dxe:ASPxTimeEdit ID="brkSaturday" runat="server" Width="190" EditFormat="Custom"
                        EditFormatString="h:mm tt" DisplayFormatString="h:mm tt" ClientInstanceName="cbrkSaturday">
                        <ClearButton DisplayMode="OnHover"></ClearButton>
                        <ValidationSettings ErrorDisplayMode="None" />
                    </dxe:ASPxTimeEdit>
                </td>
                <td class="tdSaturday minput">
                    <dxe:ASPxTextBox ID="graceSaturday" runat="server" Width="170px">
                        <MaskSettings Mask="&lt;0..999&gt;" AllowMouseWheel="false" />
                    </dxe:ASPxTextBox>
                </td>
            </tr>


            <tr>

                <td>Sunday:</td>
                <td>
                    <dxe:ASPxCheckBox ID="chkSunday" ClientInstanceName="cchkSunday" runat="server"
                        ClientSideEvents-CheckedChanged="sundayCheckChange">
                    </dxe:ASPxCheckBox>
                </td>
                <td class="tdSunday minput">
                    <dxe:ASPxTimeEdit ID="beginSunday" runat="server" Width="190" EditFormat="Custom"
                        EditFormatString="h:mm tt" DisplayFormatString="h:mm tt" ClientInstanceName="cbeginSunday">
                        <ClearButton DisplayMode="OnHover"></ClearButton>
                        <ValidationSettings ErrorDisplayMode="None" />
                    </dxe:ASPxTimeEdit>
                </td>
                <td class="tdSunday minput">
                    <dxe:ASPxTimeEdit ID="endSunday" runat="server" Width="190" EditFormat="Custom"
                        EditFormatString="h:mm tt" DisplayFormatString="h:mm tt" ClientInstanceName="cendSunday">
                        <ClearButton DisplayMode="OnHover"></ClearButton>
                        <ValidationSettings ErrorDisplayMode="None" />
                    </dxe:ASPxTimeEdit>
                </td>
                <td class="tdSunday hide minput">
                    <dxe:ASPxTimeEdit ID="brkSunday" runat="server" Width="190" EditFormat="Custom"
                        EditFormatString="h:mm tt" DisplayFormatString="h:mm tt" ClientInstanceName="cbrkSunday">
                        <ClearButton DisplayMode="OnHover"></ClearButton>
                        <ValidationSettings ErrorDisplayMode="None" />
                    </dxe:ASPxTimeEdit>
                </td>
                  <td class="tdSunday minput">
                    <dxe:ASPxTextBox ID="graceSunday" runat="server" Width="170px">
                        <MaskSettings Mask="&lt;0..999&gt;" AllowMouseWheel="false" />
                    </dxe:ASPxTextBox>
                </td>
            </tr>


        </table>
    </div>



    <dxe:ASPxButton ID="Save" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="Save_Click" >
        <ClientSideEvents Click="Validate" />
    </dxe:ASPxButton>
    <asp:HiddenField ID="hdAddedit" runat="server" />
    <asp:HiddenField ID="hdId" runat="server" />
</asp:Content>





