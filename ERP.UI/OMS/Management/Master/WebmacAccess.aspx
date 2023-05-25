<%--================================================== Revision History =============================================
1.0   Pallab    V2.0.38      25-05-2023          0026224: User MAC Address module design modification & check in small device
====================================================== Revision History =============================================--%>

<%@ Page Title="User MaC" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" 
    Inherits="ERP.OMS.Management.Master.WebmacAccess" CodeBehind="WebmacAccess.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   

    <script language="javascript" type="text/javascript">
        FieldName = null;

        function EditBranch(bgid) {
            var url = 'WebMacaccessAdd.aspx?Macid=' + bgid;
            //window.open(url,'aa'); 
            //OnMoreInfoClick(url, "Edit BranchGroup", '940px', '450px', 'Y');
            window.location.href = url;

        }
        function AddBranch() {
            var url = 'WebMacaccessAdd.aspx?Macid=add'
            //window.open(url,'aa'); 
            //OnMoreInfoClick(url, "Add BranchGroup", '940px', '450px', 'Y');
            window.location.href = url;

        }
        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }


        function DeleteBranch(bgid) {

            //  var QuoteNo = ctxt_replcno.GetText();

            var CheckUniqueCode = false;
            $.ajax({
                type: "POST",
                url: "WebmacAccess.aspx/DeleteWebmac",
                data: JSON.stringify({ mac: bgid }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    CheckUniqueCode = msg.d;
                    if (CheckUniqueCode == true) {

                        grid.Refresh();
                    }
                    else {

                    }
                }
            });

        }
    </script>

    <%--Rev 1.0--%>
    <link href="/assests/css/custom/newcustomstyle.css" rel="stylesheet" />
    
    <style>
        select
        {
            z-index: 1;
            margin-bottom: 0;
            -webkit-appearance: auto;
        }

        /*#grid {
            max-width: 98% !important;
        }*/
        #FormDate , #toDate , #dtTDate , #dt_PLQuote , #dt_partyInvDt
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        #FormDate_B-1 , #toDate_B-1 , #dtTDate_B-1 , #dt_PLQuote_B-1 , #dt_partyInvDt_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #FormDate_B-1 #FormDate_B-1Img , #toDate_B-1 #toDate_B-1Img , #dtTDate_B-1 #dtTDate_B-1Img , #dt_PLQuote_B-1 #dt_PLQuote_B-1Img ,
        #dt_partyInvDt_B-1 #dt_partyInvDt_B-1Img
        {
            display: none;
        }

        /*select
        {
            -webkit-appearance: auto;
        }*/

        .calendar-icon
        {
                right: 18px;
                bottom: 6px;
        }
        .padTabtype2 > tbody > tr > td
        {
            vertical-align: bottom;
        }
        #rdl_Salesquotation
        {
            margin-top: 0px;
        }

        .lblmTop8>span, .lblmTop8>label
        {
            margin-top: 8px !important;
        }

        .col-md-2, .col-md-4 {
    margin-bottom: 5px;
}

        .simple-select::after
        {
                top: 34px;
            right: 13px;
        }

        .dxeErrorFrameWithoutError_PlasticBlue.dxeControlsCell_PlasticBlue
        {
            padding: 0;
        }

        .aspNetDisabled
        {
            background: #f3f3f3 !important;
        }

        .backSelect {
    background: #42b39e !important;
}

        #ddlInventory
        {
                -webkit-appearance: auto;
        }

        /*.wid-90
        {
            width: 100%;
        }
        .dxtcLite_PlasticBlue.dxtc-top > .dxtc-content
        {
            width: 97%;
        }*/
        .newLbl
        {
                margin: 3px 0 !important;
        }

        .lblBot > span, .lblBot > label
        {
                margin-bottom: 3px !important;
        }

        .col-md-2 > label, .col-md-2 > span, .col-md-1 > label, .col-md-1 > span
        {
            margin-top: 8px !important;
            font-size: 14px;
        }

        .col-md-6 span
        {
            font-size: 14px;
        }

        #gridDEstination
        {
            width:99% !important;
        }

        #txtEntity , #txtCustName
        {
            width: 100%;
        }
        .col-md-6 span
        {
            margin-top: 8px !important;
        }

        .rds
        {
            margin-top: 10px !important;
        }

        .dxeButtonEditSys.dxeButtonEdit_PlasticBlue , select
        {
            height: 30px !important;
            
        }
        select
        {
            background-color: transparent;
                padding: 0 20px 0 5px !important;
        }

        .newLbl
        {
            font-size: 14px;
            margin: 3px 0 !important;
            font-weight: 500 !important;
            margin-bottom: 0 !important;
            line-height: 20px;
        }

        .crossBtn {
            top: 25px !important;
            right: 25px !important;
        }

        .wrapHolder
        {
            height: 60px;
        }
        #rdl_SaleInvoice
        {
            margin-top: 12px;
        }

        .dxeRoot_PlasticBlue
        {
            width: 100% !important;
        }

        #ShowFilter {
            padding-bottom: 0px !important;
        }

        @media only screen and (max-width: 1380px) and (min-width: 1300px)
        {
            .col-xs-1, .col-xs-2, .col-xs-3, .col-xs-4, .col-xs-5, .col-xs-6, .col-xs-7, .col-xs-8, .col-xs-9, .col-xs-10, .col-xs-11, .col-xs-12, .col-sm-1, .col-sm-2, .col-sm-3, .col-sm-4, .col-sm-5, .col-sm-6, .col-sm-7, .col-sm-8, .col-sm-9, .col-sm-10, .col-sm-11, .col-sm-12, .col-md-1, .col-md-2, .col-md-3, .col-md-4, .col-md-5, .col-md-6, .col-md-7, .col-md-8, .col-md-9, .col-md-10, .col-md-11, .col-md-12, .col-lg-1, .col-lg-2, .col-lg-3, .col-lg-4, .col-lg-5, .col-lg-6, .col-lg-7, .col-lg-8, .col-lg-9, .col-lg-10, .col-lg-11, .col-lg-12
            {
                 padding-right: 10px;
                 padding-left: 10px;
            }
            .simple-select::after
        {
                top: 34px;
            right: 8px;
        }
            .calendar-icon
        {
                right: 14px;
                bottom: 6px;
        }
        }

    </style>
    <%--Rev end 1.0--%>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main clearfix">
        <div class="panel-heading">
        <div class="panel-title">
            <h3>User MAC Address</h3>
        </div>

    </div>
        <div class="form_main">
         <div class="SearchArea clearfix">
                <div class="FilterSide">
                    <div style="float: left; padding-right: 5px; margin-bottom: 10px">
               <% if (rights.CanAdd)
                           { %>
                      <a href="javascript:void(0);" onclick="AddBranch();" class="btn btn-success"><span>Add New</span> </a>
             
                      
                     
                        <% } %>
                      

                        <% if (rights.CanExport)
                                               { %>
                   
                        <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                            <asp:ListItem Value="0">Export to</asp:ListItem>
                            <asp:ListItem Value="1">PDF</asp:ListItem>
                            <asp:ListItem Value="2">XLS</asp:ListItem>
                            <asp:ListItem Value="3">RTF</asp:ListItem>
                            <asp:ListItem Value="4">CSV</asp:ListItem>
                        </asp:DropDownList>
             <% } %>
                    </div></div>
        <table class="TableMain100">
           
            <tr>
                <td>
                    <dxe:ASPxGridView ID="gridtabimei" KeyFieldName="Id" runat="Server" ClientInstanceName="grid"  SettingsBehavior-AllowFocusedRow="true">
                         <SettingsSearchPanel Visible="True" />
                        <Settings ShowGroupPanel="True" ShowFilterRow="true" ShowFilterRowMenu ="true" />
                        <Columns>
                            <dxe:GridViewDataTextColumn Caption="Branch" FieldName="branch" VisibleIndex="0">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="User" FieldName="UserId" VisibleIndex="1">
                            </dxe:GridViewDataTextColumn>
                               <dxe:GridViewDataTextColumn Caption="MAc Number" FieldName="Imei_No" VisibleIndex="2">
                            </dxe:GridViewDataTextColumn>


                            <dxe:GridViewDataTextColumn  VisibleIndex="3" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center" Width="5%">
                                <Settings AllowAutoFilter="False"></Settings>
                                <DataItemTemplate>
                                     <% if (rights.CanEdit)
                                        { %>
                                    <a href="javascript:void(0);" onclick="EditBranch('<%# Container.KeyValue %>')" title="Status"><img src="../../../assests/images/Edit.png" />                                                              
                                    </a> 

                                    <% } %>

                                          <% if (rights.CanDelete)
                                        { %>
                                    <a href="javascript:void(0);" onclick="DeleteBranch('<%# Container.KeyValue %>')" title="Status"><img src="../../../assests/images/delete.png" />                                                              
                                    </a> 

                                    <% } %>

                                </DataItemTemplate>
                                <EditFormSettings Visible="False" />
                                <CellStyle Wrap="False">
                                </CellStyle>
                                <HeaderStyle Wrap="False" />
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                        <SettingsSearchPanel Visible="True" />
                        <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                        <SettingsBehavior AllowFocusedRow="true" ColumnResizeMode="NextColumn" />
                      
                    </dxe:ASPxGridView>
                </td>
            </tr>

        </table>
    </div>
    </div>
    </div>
      <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
</asp:Content>

