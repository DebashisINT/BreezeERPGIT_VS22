<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="ImportAttendance.aspx.cs" Inherits="ERP.OMS.Management.Attendance.ImportAttendance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="panel-heading">
        <div class="panel-title">
            <h3>Attendance Import</h3>
        </div>

    </div>

    <div class="form_main" style="align-items: center;">

        <div class="col-md-3">
            <label>Choose File</label>
            <div>
                <asp:FileUpload ID="uploadProdSalesPrice" runat="server" Width="100%" />
            </div>
        </div>

        <div class="col-md-3">
            <label>&nbsp;</label>
            <div>
                <asp:Button ID="BtnSave" runat="server" Text="Import File" CssClass="btn btn-primary" OnClick="BtnSave_Click" />
             <%--   <asp:LinkButton ID="lnlDownloader" runat="server" OnClick="lnlDownloader_Click" CssClass="btn btn-info">Download Format</asp:LinkButton>--%>
            </div>
        </div>

        <div class="clear"></div>


    </div>





</asp:Content>
