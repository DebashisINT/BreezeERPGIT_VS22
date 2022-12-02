<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="BackUp.aspx.cs" Inherits="ERP.OMS.Management.Master.BackUp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .flexCont{
            display: flex;
            /* width: 50%; */
            min-height: 80vh;
            justify-content: center;
            align-items: center;
            flex-direction:column
        }
        .erpWrp {
            width: 50%;
            background: #e6fff6;
            padding: 15px;
            border: 1px solid #a6e2cc;
            border-radius:5px
        }
        .tll {
            font-size: 20px;
            margin-bottom: 15px;
            border-bottom: 1px dashed #4ec399;
        }
    </style>
<script>
    function onpageredirect(msg) {
        jAlert(msg, 'Alert', function () {
            window.location.href = "../ProjectMainPage.aspx";
        });
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            
        </div>
    </div>
    <div class="">
        <div class="clearfix">
            <div class="flexCont">
                <div class="tll">Backup</div>
                <div class="erpWrp">
                <div class="col-md-4">
                    <label>Sever Name</label>
                    <div><asp:DropDownList ID="cbservername" runat="server" Width="100%"></asp:DropDownList></div>
                </div>
                <div class="col-md-4">
                     <label>Database Name</label>
                    <div>
                        <asp:DropDownList ID="cbdatabasename" runat="server" Width="100%"></asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-3" style="padding-top: 18px;">
                    <div>
                        <asp:Button runat="server" OnClick="Unnamed_Click" CssClass="btn btn-success" Text="Take Backup" />
                    </div>
                </div>
                </div>
            </div>
        </div>
 
        
    </div>
</asp:Content>
