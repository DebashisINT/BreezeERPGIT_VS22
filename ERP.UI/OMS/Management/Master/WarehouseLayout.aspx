<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="WarehouseLayout.aspx.cs" Inherits="ERP.OMS.Management.Master.WarehouseLayout" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<style>
    #warehouseLayoutList {
        padding:0;
        list-style-type:none;
    }
    #warehouseLayoutList input.inp {
        max-width:220px;
    }
    .makeBorder {
        position:relative;
    }
    .makeBorder:before{
               content: '';
            width: 22px;
            height: 2px;
            background: #7343d2;
            position: absolute;
            left: -22px;
            top: 33px;
    }
    .makeBorder:after {
      content: '';
    width: 2px;
    height: 33px;
    background: #7343d2;
    position: absolute;
    top: 0px;
    left: -22px;
    }
    /*.arr {
       width: 0;
        height: 0;
        border-top: 6px solid transparent;
        border-bottom: 6px solid transparent;
        border-left: 11px solid #7343d2;
        POSITION: absolute;
        top: 28px;
        left: -23px;
    }*/
    .pane {
        background: #f5f4f3; padding: 8px 0; margin-bottom: 0px; border-radius: 4px; border: 1px solid #ccc;
    }
</style>
    <script>
        $(document).ready(function () {
            var list = $('#warehouseLayoutList li');
            var amnt = 0;
            $(list).each(function () {
                $(this).css({ 'margin-left': amnt + 'px' });
                    //$(this).css({ 'padding-left': amnt + 'px' });
                    amnt = amnt +80;
                    console.log(amnt);
                    //$(this).addClass('makeBorder');
            });
        });
    </script>


    <div class="panel-heading">
        <div class="panel-title">
            <h3>Warehouse Layout</h3>
        </div>
    </div>
<div class="form_main">
    <div class="pane">

    <div class="col-md-12">
        <ul id="warehouseLayoutList" >
        <li>
            <label><b>First Level</b></label>
            <input runat="server" type="text" id="level1" maxlength="100" class="inp" /></li>
        <li class="makeBorder">
            <span class="arr"></span>
            
            <label><b>Second Level</b></label><input runat="server"  maxlength="100" class="inp " type="text" id="level2" /></li>
        <li class="makeBorder"> <span class="arr"></span><label><b>Third Level</b></label><input  maxlength="100" runat="server" class="inp " type="text" id="level3" /></li>
        <li class="makeBorder"> <span class="arr"></span><label><b>Fourth Level</b></label><input runat="server"  maxlength="100" class="inp " type="text" id="level4" /></li>
        <li class="makeBorder"> <span class="arr"></span><label><b>Fifth Level</b></label><input runat="server"  maxlength="100" class="inp makeBorder"   type="text" id="level5" /></li>
    </ul>
    </div>
    
    <div class="col-md-12">
        <asp:Button OnClick="Unnamed_Click" runat="server" ID="SaveBtn" Text="Save" class="btn btn-primary"></asp:Button>
        <button class="btn btn-danger">Cancel</button>
    </div>
        </div>
</div>

    
    
    
    
    


</asp:Content>
