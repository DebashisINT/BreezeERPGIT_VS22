<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="chatGroupAdd.aspx.cs" Inherits="ERP.OMS.Management.Master.chatGroupAdd" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/assests/pluggins/Transfer/icon_font/css/icon_font.css" rel="stylesheet" />
    <link href="/assests/pluggins/Transfer/css/jquery.transfer.css" rel="stylesheet" />
    <script src="/assests/pluggins/Transfer/jquery.transfer.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            var dataArray1 = [
        {
            "city": "Admin",
            "value": 132
        },
        {
            "city": "Goutam Das",
            "value": 422
        },
        {
            "city": "Indranil Dey",
            "value": 232
        },
        {
            "city": "Susanta Kundu",
            "value": 765
        },
        {
            "city": "Pijush Bhattachariya",
            "value": 876
        },
        {
            "city": "Sumon ",
            "value": 453
        },
        {
            "city": "Chinmoy",
            "value": 125
        }
            ];
            var settings1 = {
                "dataArray": dataArray1,
                "itemName": "city",
                "valueName": "value",
                "tabNameText":"Users",
                "rightTabNameText": "Selected users",
                "searchPlaceholderText":"search in users",
                "callable": function (items) {
                    console.dir(items)
                }
            };
            $("#transfer1").transfer(settings1);
        })
        
    </script>
    <style>
        .transfer-demo {
            width: 640px;
            height: 351px;
        }
        .transfer-demo  .transfer-double-header{
            display:none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading clearfix">
        <div class="panel-title clearfix">
            <div style="padding-right: 5px;">
                <h3 class="pull-left">Create Chat Group
                </h3>
            </div>
        </div>
    </div>
    <div class="form_main">
        <div class="row">
            <div class="col-md-4">
                <label>Group Name <span class="red">*</span></label>
                <div>
                    <input type="text" class="form-control" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-8">
                <label>Select users</label>
                <div>
                   <div id="transfer1" class="transfer-demo"></div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-8">
                <button type="button" class="btn btn-success">Create Group</button>
                <button type="button" class="btn btn-danger">Cancel</button>
            </div>
        </div>
    </div>
</asp:Content>
