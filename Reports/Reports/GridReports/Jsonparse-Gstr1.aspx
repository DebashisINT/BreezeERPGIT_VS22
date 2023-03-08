<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                08-03-2023        2.0.36           Pallab              25575 : Report pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Title="JSON Parse GSTR1" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true"
    CodeBehind="Jsonparse-Gstr1.aspx.cs" Inherits="Reports.Reports.GridReports.Jsonparse_Gstr1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>

        function Callback_BeginCallback() {


            $("#drdExport").val(0);

        }


        function OpenPOSDetails(invoice, type) {

            
            popupjsonrconcile.SetContentUrl('');

            if (type == 'POS') {

                url = '/OMS/Management/Activities/posSalesInvoice.aspx?key=' + invoice + '&IsTagged=1&Viemode=1';

                popupjsonrconcile.SetContentUrl(url);
                popupjsonrconcile.Show();


            }
            if (type == 'SI') {

                url = '/OMS/Management/Activities/SalesInvoice.aspx?key=' + invoice + '&IsTagged=1&req=V&type=' + type;

                popupjsonrconcile.SetContentUrl(url);
                popupjsonrconcile.Show();


            }
            
           

        }

        function checkFileSize(element) {
            var val = $(element).val(); //get file value

            var ext = val.substring(val.lastIndexOf('.') + 1).toLowerCase(); // get file extention 
            // alert(ext);
            if (ext == "json") {

            }

            else {
                jAlert('Only Json file to be allowed');
                $(element).val('');

            }


        }
        function JsonHide(s, e) {
            popupjsonrconcile.Hide();
        }

        function Callback_BeginCallbacks() {
            $("#drdExport").val(0);

            return true;
        }


        $(document).ready(function () {
            $('#btn_Conversion').click(function () {
                if ($("#<%= fileuploadjson.ClientID %>").val() == "") {
                    $("#error").html("File is required");

                    return false;
                }

                return true;
            });

        });

    </script>

    <style>
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

        /*Rev 1.0*/
        .outer-div-main {
            background: #ffffff;
            padding: 10px;
            border-radius: 10px;
            box-shadow: 1px 1px 10px #11111154;
        }

        /*.form_main {
            overflow: hidden;
        }*/

        label , .mylabel1, .clsTo, .dxeBase_PlasticBlue
        {
            color: #141414 !important;
            font-size: 14px !important;
                font-weight: 500 !important;
                margin-bottom: 0 !important;
                    line-height: 20px;
        }

        #GrpSelLbl .dxeBase_PlasticBlue
        {
                line-height: 20px !important;
        }

        select
        {
            height: 30px !important;
            border-radius: 4px;
            -webkit-appearance: none;
            position: relative;
            z-index: 1;
            background-color: transparent;
            padding-left: 10px !important;
            padding-right: 22px !important;
        }

        .dxeButtonEditSys.dxeButtonEdit_PlasticBlue , .dxeTextBox_PlasticBlue
        {
            height: 30px;
            border-radius: 4px;
        }

        .dxeButtonEditButton_PlasticBlue
        {
            background: #094e8c !important;
            border-radius: 4px !important;
            padding: 0 4px !important;
        }

        .calendar-icon {
            position: absolute;
            bottom: 7px;
            right: 19px;
            z-index: 0;
            cursor: pointer;
        }

        #ASPxFromDate , #ASPxToDate , #ASPxASondate , #ASPxAsOnDate
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        #ASPxFromDate_B-1 , #ASPxToDate_B-1 , #ASPxASondate_B-1 , #ASPxAsOnDate_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #ASPxFromDate_B-1 #ASPxFromDate_B-1Img , #ASPxToDate_B-1 #ASPxToDate_B-1Img , #ASPxASondate_B-1 #ASPxASondate_B-1Img , #ASPxAsOnDate_B-1 #ASPxAsOnDate_B-1Img
        {
            display: none;
        }

        .dxtcLite_PlasticBlue > .dxtc-stripContainer .dxtc-activeTab, .dxgvFooter_PlasticBlue
        {
            background: #1b5ea4 !important;
        }

        .simple-select::after {
            /*content: '<';*/
            content: url(../../../assests/images/left-arw.png);
            position: absolute;
            top: 6px;
            right: -2px;
            font-size: 16px;
            transform: rotate(269deg);
            font-weight: 500;
            background: #094e8c;
            color: #fff;
            height: 18px;
            display: block;
            width: 26px;
            /* padding: 10px 0; */
            border-radius: 4px;
            text-align: center;
            line-height: 19px;
            z-index: 0;
        }
        .simple-select {
            position: relative;
        }
        .simple-select:disabled::after
        {
            background: #1111113b;
        }
        select.btn
        {
            padding-right: 10px !important;
        }

        .panel-group .panel
        {
            box-shadow: 1px 1px 8px #1111113b;
            border-radius: 8px;
        }

        .dxpLite_PlasticBlue .dxp-current
        {
            background-color: #1b5ea4;
            padding: 3px 5px;
            border-radius: 2px;
        }

        #accordion {
            margin-bottom: 20px;
            margin-top: 10px;
        }

        .dxgvHeader_PlasticBlue {
    background: #1b5ea4 !important;
    color: #fff !important;
}
        #ShowGrid
        {
            margin-top: 10px;
        }

        .pt-25{
                padding-top: 25px !important;
        }

        .styled-checkbox {
        position: absolute;
        opacity: 0;
        z-index: 1;
    }

        .styled-checkbox + label {
            position: relative;
            /*cursor: pointer;*/
            padding: 0;
            margin-bottom: 0 !important;
        }

            .styled-checkbox + label:before {
                content: "";
                margin-right: 6px;
                display: inline-block;
                vertical-align: text-top;
                width: 16px;
                height: 16px;
                /*background: #d7d7d7;*/
                margin-top: 2px;
                border-radius: 2px;
                border: 1px solid #c5c5c5;
            }

        .styled-checkbox:hover + label:before {
            background: #094e8c;
        }


        .styled-checkbox:checked + label:before {
            background: #094e8c;
        }

        .styled-checkbox:disabled + label {
            color: #b8b8b8;
            cursor: auto;
        }

            .styled-checkbox:disabled + label:before {
                box-shadow: none;
                background: #ddd;
            }

        .styled-checkbox:checked + label:after {
            content: "";
            position: absolute;
            left: 3px;
            top: 9px;
            background: white;
            width: 2px;
            height: 2px;
            box-shadow: 2px 0 0 white, 4px 0 0 white, 4px -2px 0 white, 4px -4px 0 white, 4px -6px 0 white, 4px -8px 0 white;
            transform: rotate(45deg);
        }

        .dxgvEditFormDisplayRow_PlasticBlue td.dxgv, .dxgvDataRow_PlasticBlue td.dxgv, .dxgvDataRowAlt_PlasticBlue td.dxgv, .dxgvSelectedRow_PlasticBlue td.dxgv, .dxgvFocusedRow_PlasticBlue td.dxgv
        {
            padding: 6px 6px 6px !important;
        }

        #lookupCardBank_DDD_PW-1
        {
                left: -182px !important;
        }
        .plhead a>i
        {
                top: 9px;
        }

        .clsTo
        {
            display: flex;
    align-items: flex-start;
        }

        input[type="radio"], input[type="checkbox"]
        {
            margin-right: 5px;
        }
        .dxeCalendarDay_PlasticBlue
        {
                padding: 6px 6px;
        }

        .modal-dialog
        {
            width: 50%;
        }

        .modal-header
        {
            padding: 8px 4px 8px 10px;
            background: #094e8c !important;
        }

        .TableMain100 #ShowGrid , .TableMain100 #ShowGridList , .TableMain100 #ShowGridRet , .TableMain100 #ShowGridCustOut  
        /*#B2B , 
        #grid_B2BUR , 
        #grid_IMPS , 
        #grid_IMPG ,
        #grid_CDNR ,
        #grid_CDNUR ,
        #grid_EXEMP ,
        #grid_ITCR ,
        #grid_HSNSUM*/
        {
            max-width: 98% !important;
        }

        /*div.dxtcSys > .dxtc-content > div, div.dxtcSys > .dxtc-content > div > div
        {
            width: 95% !important;
        }*/

        .btn-info
        {
                background-color: #1da8d1 !important;
                background-image: none;
        }

        .for-cust-icon {
            position: relative;
            z-index: 1;
        }

        .dxeDisabled_PlasticBlue, .aspNetDisabled
        {
            background: #f3f3f3 !important;
        }

        .dxeButtonDisabled_PlasticBlue
        {
            background: #b5b5b5 !important;
            border-color: #b5b5b5 !important;
        }

        #ddlValTech
        {
            width: 100% !important;
            margin-bottom: 0 !important;
        }

        .dis-flex
        {
            display: flex;
            align-items: baseline;
        }

        input + label
        {
            line-height: 1;
                margin-top: 3px;
        }

        .dxtlHeader_PlasticBlue
        {
            background: #094e8c !important;
        }

        .dxeBase_PlasticBlue .dxichCellSys
        {
            padding-top: 2px !important;
        }

        .pBackDiv
        {
            border-radius: 10px;
            box-shadow: 1px 1px 10px #1111112e;
        }
        .HeaderStyle th
        {
            padding: 5px;
        }

        .for-cust-icon {
            position: relative;
            z-index: 1;
        }

        .dxtcLite_PlasticBlue.dxtc-top > .dxtc-stripContainer
        {
            padding-top: 15px;
        }

        .pt-2
        {
            padding-top: 5px;
        }
        .pt-10
        {
            padding-top: 10px;
        }

        .pt-15
        {
            padding-top: 15px;
        }

        .pb-10
        {
            padding-bottom: 10px;
        }

        .pTop10 {
    padding-top: 20px;
}
        .custom-padd
        {
            padding-top: 4px;
    padding-bottom: 10px;
        }

        input + label
        {
                margin-right: 10px;
        }

        .btn
        {
            margin-bottom: 0;
        }

        .pl-10
        {
            padding-left: 10px;
        }

        /*#lookup_project
        {
            max-width: 100% !important;
        }*/
        /*Rev end 1.0*/
    </style>

    <style>
        /*Rev 1.0*/
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
        /*Rev end 1.0*/
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                Grid.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                Grid.SetWidth(cntWidth);
            }

            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    Grid.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    Grid.SetWidth(cntWidth);
                }

            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="panel-heading">
        <%--<div class="panel-title clearfix">
            <h3 class="pull-left">GSTR-1 Reconciliation [With JSON]</h3>
        </div>--%>
       <div class="panel-group" id="accordion" role="tablist" aria-multiselectable="true">
          <div class="panel panel-info">
            <div class="panel-heading" role="tab" id="headingOne">
              <h4 class="panel-title plhead">
                <a role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="true" aria-controls="collapseOne" class="collapsed">
                  <asp:Label ID="RptHeading" runat="Server" Text="" Width="470px" style="font-weight:bold;"></asp:Label>
                    <i class="fa fa-plus-circle" ></i>
                    <i class="fa fa-minus-circle"></i>
                </a>
              </h4>
            </div>
            <div id="collapseOne" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingOne">
              <div class="panel-body">
                    <div class="companyName">
                        <asp:Label ID="CompName" runat="Server" Text="" Width="470px" ></asp:Label>
                    </div>
                    <div>
                        <asp:Label ID="CompAdd" runat="Server" Text="" Width="470px" ></asp:Label>
                    </div>
                    <div>
                        <asp:Label ID="CompOth" runat="Server" Text="" Width="470px" ></asp:Label>
                    </div>
                    <div>
                        <asp:Label ID="CompPh" runat="Server" Text="" Width="470px" ></asp:Label>
                    </div>       
                    <div>
                        <asp:Label ID="CompAccPrd" runat="Server" Text="" Width="470px" ></asp:Label>
                    </div>
                    <div>
                        <asp:Label ID="DateRange" runat="Server" Text="" Width="470px" ></asp:Label>
                    </div>
              </div>
            </div>
          </div>
        </div>
    </div>

    <%--Rev 1.0: "outer-div-main" class add: --%>
    <div class="outer-div-main">
        <div id="pageheaderContent">
        <div>
            <h4>JSON File</h4>

            <asp:FileUpload ID="fileuploadjson" runat="server" accept=".json" onchange="checkFileSize(this)" />
        </div>
        <br />
        <asp:Button ID="btn_Conversion" runat="server" Text="Reconcile" OnClick="Button_Click" CssClass="btn btn-sm btn-success" OnClientClick="return Callback_BeginCallbacks()" />
        <%--   <% if (rights.CanExport)
                                               { %>--%>
        <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary"
            OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true">
            <asp:ListItem Value="0">Export to</asp:ListItem>
            <asp:ListItem Value="1">PDF</asp:ListItem>
            <asp:ListItem Value="2">XLSX</asp:ListItem>
            <asp:ListItem Value="3">RTF</asp:ListItem>
            <asp:ListItem Value="4">CSV</asp:ListItem>

        </asp:DropDownList>
             <%--     <% } %>--%>


        <dxe:ASPxGridView runat="server" ID="ShowGrid" ClientInstanceName="Grid" Width="100%" EnableRowsCache="false" AutoGenerateColumns="false"
            Settings-HorizontalScrollBarMode="Auto" OnDataBinding="grid_DataBinding"  OnSummaryDisplayText="ShowGrid_SummaryDisplayText"
            ClientSideEvents-BeginCallback="Callback_BeginCallback" Settings-VerticalScrollableHeight="180" Settings-VerticalScrollBarMode="Auto">


            <Columns>
          <%--      <dxe:GridViewDataTextColumn FieldName="idt" Caption="Invoice Date" VisibleIndex="1" Width="200px">
                </dxe:GridViewDataTextColumn>


                <dxe:GridViewDataTextColumn FieldName="inum" Caption="Invoice Number" VisibleIndex="2" Width="130px">
                </dxe:GridViewDataTextColumn>--%>


                    <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="Bill Number" Width="200px" Caption="Invoice No.">
                    <CellStyle>
                    </CellStyle>
                    <HeaderStyle/>
                    <DataItemTemplate>

                        <a href="javascript:void(0)" target="_blank" onclick="OpenPOSDetails('<%#Eval("Invoice_Id") %>','<%#Eval("type") %>')">
                            <%#Eval("Bill Number")%>
                        </a>
                    </DataItemTemplate>

                    <EditFormSettings Visible="False" />
                </dxe:GridViewDataTextColumn>


                <dxe:GridViewDataTextColumn FieldName="Bill Date" Caption="Invoice Date" VisibleIndex="2" Width="150px">
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn FieldName="txval" Caption="Taxable Amount" VisibleIndex="3" Width="100px" PropertiesTextEdit-DisplayFormatString="0.00">
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn FieldName="rt" Caption="Rate" VisibleIndex="4" Width="100px" PropertiesTextEdit-DisplayFormatString="0.00" HeaderStyle-HorizontalAlign="Center" CellStyle-VerticalAlign="Middle" CellStyle-HorizontalAlign="Center">
                </dxe:GridViewDataTextColumn>




                <dxe:GridViewDataTextColumn FieldName="val" Caption="Total Amount" VisibleIndex="8" Width="100px" PropertiesTextEdit-DisplayFormatString="0.00">
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn FieldName="rchrg" Caption="Reverse Charge" VisibleIndex="9" Width="100px"  HeaderStyle-HorizontalAlign="Center" CellStyle-VerticalAlign="Middle" CellStyle-HorizontalAlign="Center">
                </dxe:GridViewDataTextColumn>


                <%--    <dxe:GridViewDataTextColumn FieldName="Bill Number" Caption="Our Invoice No." VisibleIndex="11" Width="20%">
                </dxe:GridViewDataTextColumn>--%>


                <dxe:GridViewDataTextColumn FieldName="Name" Caption="Party Name(Invoice)" VisibleIndex="13" Width="300px">
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn FieldName="NamejSON" Caption="Party Name(Json)" VisibleIndex="14" Width="300px">
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn FieldName="Partytype" Caption="Party Type" VisibleIndex="15" Width="100px"  HeaderStyle-HorizontalAlign="Center" CellStyle-VerticalAlign="Middle" CellStyle-HorizontalAlign="Center">
                </dxe:GridViewDataTextColumn>


                <dxe:GridViewDataTextColumn FieldName="CNT_GSTIN" Caption="GSTIN" VisibleIndex="16" Width="200px">
                </dxe:GridViewDataTextColumn>



                 <dxe:GridViewDataTextColumn FieldName="BranchCompanyGSTIN" Caption="Branch GSTIN" VisibleIndex="17" Width="200px">
                </dxe:GridViewDataTextColumn>



                <dxe:GridViewDataTextColumn FieldName="excess" Caption="Difference" VisibleIndex="18" Width="100px" PropertiesTextEdit-DisplayFormatString="0.00">
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn FieldName="Data status" Caption="Matched?" VisibleIndex="19" Width="100px"  HeaderStyle-HorizontalAlign="Center" CellStyle-VerticalAlign="Middle" CellStyle-HorizontalAlign="Center">
                </dxe:GridViewDataTextColumn>


                
                <dxe:GridViewDataTextColumn FieldName="Reason" Caption="Reason" VisibleIndex="20" Width="100px"  HeaderStyle-HorizontalAlign="Center" CellStyle-VerticalAlign="Middle" CellStyle-HorizontalAlign="Center">
                </dxe:GridViewDataTextColumn>


            </Columns>

            <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" ColumnResizeMode="Control" />
            <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
            <SettingsEditing Mode="EditForm" />
            <SettingsContextMenu Enabled="true" />
            <SettingsBehavior AutoExpandAllGroups="true" />
            <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" />
            <SettingsSearchPanel Visible="false" />
            <SettingsPager PageSize="10">
                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
            </SettingsPager>
            <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
            <TotalSummary>
                <dxe:ASPxSummaryItem FieldName="txval" SummaryType="Sum" />
                <dxe:ASPxSummaryItem FieldName="val" SummaryType="Sum" />
              
             
            </TotalSummary>

        </dxe:ASPxGridView>

    </div>
    </div>

    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>


    <dxe:ASPxPopupControl ID="ASPXPopupControl2" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popupjsonrconcile" Height="500px"
        Width="1200px" HeaderText="Details" Modal="true" AllowResize="true" ResizingMode="Postponed">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>

        <ClientSideEvents CloseUp="JsonHide" />
    </dxe:ASPxPopupControl>
</asp:Content>
