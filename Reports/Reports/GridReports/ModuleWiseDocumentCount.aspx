<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="ModuleWiseDocumentCount.aspx.cs" Inherits="Reports.Reports.GridReports.ModuleWiseDocumentCount" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .colDisable {
        cursor:default !important;
        }
        .plhead a {
            font-size:16px;
            padding-left:10px;
            position:relative;
            width:100%;
            display:block;
            padding:9px 10px 5px 10px;
        }
        .plhead a>i {
            position:absolute;
            top:11px;
            right:15px;
        }
        #accordion {
            margin-bottom:10px;
        }
        .companyName {
            font-size:16px;
            font-weight:bold;
            margin-bottom:15px;
        }
       
        .plhead a.collapsed .fa-minus-circle{
            display:none;
        }

        #ListBoxProjects{
            width: 200px;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                //cShowGrid.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                //cShowGrid.SetWidth(cntWidth);
            }

            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    //cShowGrid.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    //cShowGrid.SetWidth(cntWidth);
                }

            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <%--<div class="panel-title">
            <h3>Cash/Bank Book - Detail</h3>
        </div>--%>
       <div class="panel-group" id="accordion" role="tablist" aria-multiselectable="true">
          <div class="panel panel-info">
            <div class="panel-heading" role="tab" id="headingOne">
              <h4 class="panel-title plhead">
                <a role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="true" aria-controls="collapseOne" class="collapsed">
                  Module Wise Document Count
                    <i class="fa fa-plus-circle" ></i>
                    <i class="fa fa-minus-circle"></i>
                </a>
              </h4>
            </div>
            <div id="collapseOne" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingOne">
              <div class="panel-body">
                    <div class="companyName">
                        Demo Group of Co
                    </div>
                 
                    <div>
                        <p>7/1 Lord Sinha Road, Lords - 506, Kolkata - 700071, Kolkata</p>
                         <p>Pin - 700071, West Bengal, India</p>
                         <p>Ph: 654645654</p>
                    </div>
                    <div>
                        <asp:Label ID="CompOth" runat="Server" Text="" Width="470px" ></asp:Label>
                    </div>
                    <div>
                        Accounting Period: 01-04-2018 To 31-03-2021
                    </div>       
                    
              </div>
            </div>
          </div>
        </div>
    </div>

    <div class="form_main">
        <div class="row" style="margin-top:5px">
            <div class="col-md-2">
                <label style="color: #b5285f; font-weight: bold;" class="clsFrom">
                    From Date : 
                </label>
                <dxe:ASPxDateEdit ID="ASPxFromDate" runat="server"  EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                    UseMaskBehavior="True" Width="100%" >
                    <ButtonStyle Width="13px">
                    </ButtonStyle>
                </dxe:ASPxDateEdit>
            </div>
         
        <div class="col-md-2">
            <label style="color: #b5285f; font-weight: bold;" class="clsFrom">
               To Date : 
            </label>
            <dxe:ASPxDateEdit ID="ASPxToDate" runat="server"  EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                UseMaskBehavior="True" Width="100%" >
                <ButtonStyle Width="13px">
                </ButtonStyle>
            </dxe:ASPxDateEdit>
        </div>
        <div class="col-md-2">
            <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                Module Name : 

                </label>
            </div>
            <div>
                <select class="form-control">
                    <option>Select</option>
                </select>
            </div>
        </div>
        <div class="col-md-2">
            <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                User : </label>
            </div>
            <div>
                <select class="form-control">
                    <option>Select</option>
                </select>
            </div>
        </div>
        <div class="col-md-2">
            <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                Unit/Branch : </label>
            </div>
            <div>
                <select class="form-control">
                    <option>Select</option>
                </select>
            </div>
        </div>
        <div class="col-md-2" style="padding-top: 19px;">
            <button id="btnShow" class="btn btn-primary" type="button" onclick="">Show</button>                   
            <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" >
                <asp:ListItem Value="0">Export to</asp:ListItem>
                <asp:ListItem Value="1">XLSX</asp:ListItem>
                <asp:ListItem Value="2">PDF</asp:ListItem>
                <asp:ListItem Value="3">CSV</asp:ListItem>
                <asp:ListItem Value="4">RTF</asp:ListItem>
            </asp:DropDownList>           
        </div>
    </div>

        <div class="row">
            <div class="col-md-12">
                Grid Here
            </div>
        </div>
</div>
</asp:Content>
