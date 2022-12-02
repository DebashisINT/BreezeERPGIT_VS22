<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="ABCAnalysis.aspx.cs" Inherits="Reports.Reports.GridReports.ABCAnalysis" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <link href="CSS/SearchPopup.css" rel="stylesheet" />
    <script src="JS/SearchMultiPopup.js"></script>
      <script src="/assests/pluggins/choosen/choosen.min.js"></script>
        <style>
         .btnOkformultiselection {
            border-width: 1px;
            padding: 4px 10px;
            font-size: 13px !important;
            margin-right: 6px;
            }

         /*rev Pallab*/
        .branch-selection-box .dxlpLoadingPanel_PlasticBlue tbody, .branch-selection-box .dxlpLoadingPanelWithContent_PlasticBlue tbody, #divProj .dxlpLoadingPanel_PlasticBlue tbody, #divProj .dxlpLoadingPanelWithContent_PlasticBlue tbody
        {
            bottom: 0 !important;
        }
        .dxlpLoadingPanel_PlasticBlue tbody, .dxlpLoadingPanelWithContent_PlasticBlue tbody
        {
            position: absolute;
            bottom: 60px;
            background: #fff;
            box-shadow: 1px 1px 10px #0000001c;
            right: 55%;
        }
        /*rev end Pallab*/

    </style>
    <%-- For multiselection when click on ok button--%>
    <script type="text/javascript">
        function SetSelectedValues(Id, Name, ArrName) {
            if (ArrName == 'ClassSource') {
                var key = Id;
                if (key != null && key != '') {
                    $('#ClassModel').modal('hide');
                    ctxtClass.SetText(Name);
                    GetObjectID('hdnClassId').value = key;

                    var OtherDetailsProd = {}
                    OtherDetailsProd.SearchKey = 'undefined text';
                    OtherDetailsProd.ClassID = '';
                    var HeaderCaption = [];
                    HeaderCaption.push("Code");
                    HeaderCaption.push("Name");
                    HeaderCaption.push("Hsn");

                    callonServerM("Services/Master.asmx/GetClassWiseProduct", OtherDetailsProd, "ProductTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "ProductSource");

                }
                else {
                    ctxtClass.SetText('');
                    GetObjectID('hdnClassId').value = '';
                }
            }
            else if (ArrName == 'ProductSource') {
                var key = Id;
                if (key != null && key != '') {
                    $('#ProdModel').modal('hide');
                    ctxtProdName.SetText(Name);
                    GetObjectID('hdncWiseProductId').value = key;
                }
                else {
                    GetObjectID('hdncWiseProductId').value = '';
                }
            }
            else if (ArrName == 'BrandSource') {
                var key = Id;
                if (key != null && key != '') {
                    $('#BrandModel').modal('hide');
                    ctxtBrandName.SetText(Name);
                    GetObjectID('hdnBranndId').value = key;
                }
                else {
                    ctxtBrandName.SetText('');
                    GetObjectID('hdnBranndId').value = '';
                }
            }

        }

    </script>
   <%-- For multiselection when click on ok button--%>

   <%-- For Class multiselection--%>
 
     <script type="text/javascript">

         function txtIndicatorA_lostFocus()
         {
             if (parseInt(CtxtIndicatorA.GetValue()) > 0) {

                 var AVal = parseInt(CtxtIndicatorA.GetValue()) + parseInt(CtxtIndicatorB.GetValue());
                 AVal = (100 - AVal);
                 if ((AVal > 100) || (AVal < 0)) {
                     CtxtIndicatorA.SetValue('0');
                     jAlert("Value Should Not be greater than 100");
                 }
                 else {
                     CtxtIndicatorC.SetValue(AVal)
                 }
                // CtxtIndicatorC.SetText(100 - (parseInt(CtxtIndicatorA.GetValue()) + parseInt(CtxtIndicatorB.GetValue())))
             }
         }

         function txtIndicatorB_lostFocus() {
             if (parseInt(CtxtIndicatorB.GetValue()) > 0) {

                 var BVal = parseInt(CtxtIndicatorA.GetValue()) + parseInt(CtxtIndicatorB.GetValue());
                 BVal = (100 - BVal);

                 if ((BVal > 100) || (BVal < 0)) {
                     CtxtIndicatorB.SetValue('0');
                     jAlert("Value Should Not be greater than 100");
                 }
                 else {
                     CtxtIndicatorC.SetValue(BVal)
                 }
                 // CtxtIndicatorC.SetText(100 - (parseInt(CtxtIndicatorA.GetValue()) + parseInt(CtxtIndicatorB.GetValue())))
             }
         }

         function OnEndCallback(s, e) {
             $("#spntotalproduct").text(cShowGrid.cpTotalProduct);
             $("#spntotalvalueA").text(cShowGrid.cpTotalA);
             $("#spntotalvalueB").text(cShowGrid.cpTotalB);
             $("#spntotalvalueC").text(cShowGrid.cpTotalC);
         }

         $(document).ready(function () {
            // if (CtxtIndicatorA.GetValue())
             CtxtIndicatorA.SetValue('80');
             CtxtIndicatorB.SetValue('15');
             CtxtIndicatorC.SetValue('5');

             if ($("#ddlOnCriteria").val() == "S") {
                 $("#ddlValTech").prop('disabled', true);
             }
             $('#ddlOnCriteria').change(function () {
                 
                 if($("#ddlOnCriteria").val()=="S")
                 {
                     $("#ddlValTech").prop('disabled', true);
                 }
                 else
                 {
                     $("#ddlValTech").prop('disabled', false);
                 }


             });


             $('#ClassModel').on('shown.bs.modal', function () {
                 $('#txtClassSearch').focus();
             })
             
         });
         var ClassArr = new Array();
         $(document).ready(function () {
             var ClassObj = new Object();
             ClassObj.Name = "ClassSource";
             ClassObj.ArraySource = ClassArr;
             arrMultiPopup.push(ClassObj);
         })
         function ClassButnClick(s, e) {
             $('#ClassModel').modal('show');
         }

         function Class_KeyDown(s, e) {
             if (e.htmlEvent.key == "Enter") {
                 $('#ClassModel').modal('show');
             }
         }

         function Classkeydown(e) {
             var OtherDetails = {}

             if ($.trim($("#txtClassSearch").val()) == "" || $.trim($("#txtClassSearch").val()) == null) {
                 return false;
             }
             OtherDetails.SearchKey = $("#txtClassSearch").val();

             if (e.code == "Enter" || e.code == "NumpadEnter") {

                 var HeaderCaption = [];
                 HeaderCaption.push("Class Name");

                 if ($("#txtClassSearch").val() != "") {
                     callonServerM("Services/Master.asmx/GetClass", OtherDetails, "ClassTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "ClassSource");
                 }
             }
             else if (e.code == "ArrowDown") {
                 if ($("input[dPropertyIndex=0]"))
                     $("input[dPropertyIndex=0]").focus();
             }
         }

         function SetfocusOnseach(indexName) {
             if (indexName == "dPropertyIndex")
                 $('#txtClassSearch').focus();
             else
                 $('#txtClassSearch').focus();
         }
   </script>
 <%-- For Class multiselection--%>
     <%-- For Brand multiselection--%>
 
     <script type="text/javascript">
         $(document).ready(function () {
             $('#BrandModel').on('shown.bs.modal', function () {
                 $('#txtBrandSearch').focus();
             })

         })
         var BrandArr = new Array();
         $(document).ready(function () {
             var BrandObj = new Object();
             BrandObj.Name = "BrandSource";
             BrandObj.ArraySource = BrandArr;
             arrMultiPopup.push(BrandObj);
         })
         function BrandButnClick(s, e) {
             $('#BrandModel').modal('show');
         }

         function Brand_KeyDown(s, e) {
             if (e.htmlEvent.key == "Enter") {
                 $('#BrandModel').modal('show');
             }
         }

         function Brandkeydown(e) {
             var OtherDetails = {}

             if ($.trim($("#txtBrandSearch").val()) == "" || $.trim($("#txtBrandSearch").val()) == null) {
                 return false;
             }
             OtherDetails.SearchKey = $("#txtBrandSearch").val();

             if (e.code == "Enter" || e.code == "NumpadEnter") {

                 var HeaderCaption = [];
                 HeaderCaption.push("Name");

                 if ($("#txtBrandSearch").val() != "") {
                     callonServerM("Services/Master.asmx/GetBrand", OtherDetails, "BrandTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "BrandSource");
                 }
             }
             else if (e.code == "ArrowDown") {
                 if ($("input[dPropertyIndex=0]"))
                     $("input[dPropertyIndex=0]").focus();
             }
         }

         function SetfocusOnseach(indexName) {
             if (indexName == "dPropertyIndex")
                 $('#txtBrandSearch').focus();
             else
                 $('#txtBrandSearch').focus();
         }
   </script>
      <%-- For Brand multiselection--%>
    <script>
        function btn_ShowRecordsClick(e) {
            $("#drdExport").val(0);
            $("#hfIsABCAnalysis").val("Y");
            $("#ddldetails").val(0);
            if (CtxtIndicatorA.GetValue() == "0" && CtxtIndicatorB.GetValue() == "0" && CtxtIndicatorC.GetValue() == "0") {
                jAlert('Please input Indicator for generate the report.');
            }
            else {
                cCallbackPanel.PerformCallback();
            }

            var FromDate = (cxdeFromDate.GetValue() != null) ? cxdeFromDate.GetValue() : "";
            var ToDate = (cxdeToDate.GetValue() != null) ? cxdeToDate.GetValue() : "";

            FromDate = GetDateFormat(FromDate);
            ToDate = GetDateFormat(ToDate);
            document.getElementById('<%=DateRange.ClientID %>').innerHTML = "For the period: " + FromDate + " To " + ToDate;
        }

        function GetDateFormat(today) {
            if (today != "") {
                var dd = today.getDate();
                var mm = today.getMonth() + 1; //January is 0!

                var yyyy = today.getFullYear();
                if (dd < 10) {
                    dd = '0' + dd;
                }
                if (mm < 10) {
                    mm = '0' + mm;
                }
                today = dd + '-' + mm + '-' + yyyy;
            }

            return today;
        }

        $(document).keydown(function (e) {
            if (e.keyCode == 27) {
                cpopup.Hide();
                cShowGrid.Focus();
            }
        });

        function CallbackPanelEndCall(s, e) {
            cShowGrid.Refresh();
        }
    </script>

    <style>
        .pdbot > tbody > tr > td {
            padding-bottom: 10px;
        }
    </style>

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
        .pb-5 {
            padding-bottom:5px
        }
        .padTableR{
            width:auto;
            margin-top:5px;
        }
        .padTableR>tbody>tr>td:not(:last-child){
            padding-right:10px;
            vertical-align:middle;
        }
        .popover{
            max-width:50%;
        }
        .cPoint{
            cursor:pointer;
        }
        /*btn badge items*/
        .badge {
            display: inline-block;
            min-width: 10px;
            padding: 5px 8px;
            font-size: 12px;
            font-weight: bold;
            line-height: 1;
            color: #fff;
            text-align: center;
            white-space: nowrap;
            vertical-align: baseline;
            background-color: #929ef0;
            border-radius: 10px;
        }
        .btn .badge {
            background:#fff;
            color:#333;
            margin-left:4px;
            font-size: 14px;
        }
        .btn-badge-color1:hover, .btn-badge-color2:hover, .btn-badge-color3:hover,.btn-badge-color4:hover,.btn-badge-color5:hover,.btn-badge-color6:hover,
        .btn-badge-color1:focus, .btn-badge-color2:focus, .btn-badge-color3:focus,.btn-badge-color4:focus,.btn-badge-color5:focus,.btn-badge-color6:focus {
            color:#fff;
            opacity:0.8;
        }
        .btn-badge-color1 {
            /*background:chocolate;
            color:#fff;*/
            /*background:#d9c015;*/
            background:#ecd21dcc;
            color:#fff;
        }
        .btn-badge-color2 {
            background:#30cb57;
            color:#fff;
        }
        .btn-badge-color3 {
            background:#ff6a00;
            color:#fff;
        }
        .btn-badge-color4 {
            background:#83bcdd;
            color:#fff;
        }
        .btn-badge-color5 {
            background:#7cdfcc;
            color:#fff;
        }
         .btn-badge-color6 {
            background:#3366FF;
            color:#fff;
        }
        .nocursor{
            cursor:default;
        }
        .auto-style1 {
            height: 145px;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                cShowGrid.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                cShowGrid.SetWidth(cntWidth);
            }

            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    cShowGrid.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    cShowGrid.SetWidth(cntWidth);
                }

            });
        });
        $(function () {
            $('[data-toggle="popover"]').popover()
        })
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
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
                        <asp:Label ID="CompBranch" runat="Server" Text="" Width="470px" ></asp:Label>
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
    <div class="form_main">
        <div class="row">
            <div class="col-md-2 lblmTop8">
                <div style="color: #b5285f; font-weight: bold;" class="clsFrom">
                    <asp:Label ID="lblFromDate" runat="Server" Text="From Date:  " CssClass="mylabel1"></asp:Label>
                </div>
                <dxe:ASPxDateEdit ID="ASPxFromDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                    UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeFromDate">
                    <ButtonStyle Width="13px">
                    </ButtonStyle>
                </dxe:ASPxDateEdit>
            </div>
            <div class="col-md-2 lblmTop8">
                <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="lblToDate" runat="Server" Text="To Date:  " CssClass="mylabel1"
                        Width="92px"></asp:Label>
                </div>
                <dxe:ASPxDateEdit ID="ASPxToDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                    UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeToDate">
                    <ButtonStyle Width="13px">
                    </ButtonStyle>

                </dxe:ASPxDateEdit>
            </div>
            <div class="col-md-2">
                <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                        <asp:Label ID="Label4" runat="Server" Text="Class:  " CssClass="mylabel1"></asp:Label>
                </div>
                <asp:HiddenField ID="hflookupClassAllFlag" runat="server" Value="" />
                 <dxe:ASPxButtonEdit ID="txtClass" runat="server" ReadOnly="true" ClientInstanceName="ctxtClass" Width="100%" TabIndex="5">
                    <Buttons>
                        <dxe:EditButton>
                        </dxe:EditButton>
                    </Buttons>
                    <ClientSideEvents ButtonClick="function(s,e){ClassButnClick();}" KeyDown="function(s,e){Class_KeyDown(s,e);}" />
                </dxe:ASPxButtonEdit>
            </div>
            <div class="col-md-2">
                <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                   <asp:Label ID="Label3" runat="Server" Text="Brand:  " CssClass="mylabel1"></asp:Label>
                </div>
                <asp:HiddenField ID="hflookupBrandAllFlag" runat="server" Value="" />
                  <dxe:ASPxButtonEdit ID="txtBrandName" runat="server" ReadOnly="true" ClientInstanceName="ctxtBrandName" Width="100%" TabIndex="5">
                    <Buttons>
                        <dxe:EditButton>
                        </dxe:EditButton>
                    </Buttons>
                    <ClientSideEvents ButtonClick="function(s,e){BrandButnClick();}" KeyDown="function(s,e){Brand_KeyDown(s,e);}" />
                </dxe:ASPxButtonEdit>
            </div>
            
            <div class="col-md-2">
                <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="Label2" runat="Server" Text="On:  " CssClass="mylabel1"></asp:Label>
                </div>
                <asp:DropDownList ID="ddlOnCriteria" runat="server" Width="100%">
                    <asp:ListItem Text="Sales" Value="S"></asp:ListItem>
                    <asp:ListItem Text="Cost" Value="C"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col-md-2">
                <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="Label1" runat="Server" Text="Valuation Technique:  " CssClass="mylabel1"></asp:Label>
                </div>
                <asp:DropDownList ID="ddlValTech" runat="server" Width="100%">                    
                    <asp:ListItem Text="FIFO" Value="F"></asp:ListItem>
                    <asp:ListItem Text="Average" Value="A"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="clear"></div>
            <div class="col-md-4">
            <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                <label>Indicator :<span class="cPoint" data-container="body" data-toggle="popover" data-placement="right" data-content="When it comes to stock or inventory management, ABC analysis typically segregates inventory into three categories based on its revenue and control measures required: A is 20% of items with 80% of total revenue and hence asks for tight control; B is 30% items with 15% revenue; whereas ‘C’ is 50% of the things with least 5% revenue and hence treated as most liberal.
Understanding your sales over a certain period will help you evaluate and segregate which product belongs in which category i.e., A, B, or C. This will also assist the purchase Department in analyzing what to buy, and in what quantity."><i class="fa fa-question-circle" aria-hidden="true"></i></span></label>
            </div>
                <div style="padding: 2px 4px;border: 1px solid #b9c1cc;padding-bottom: 5px;border-radius: 3px;">
                    <table class="padTableR">
                        <tr>
                            <td><b>A:</b></td>
                            <td>
                                <dxe:ASPxTextBox ID="txtIndicatorA" runat="server" TextMode="Number" Width="100%" MaxLength="100" CssClass="upper" ClientInstanceName="CtxtIndicatorA">
                                    <ClientSideEvents LostFocus="txtIndicatorA_lostFocus"/>
                                    <MaskSettings Mask="<0..999>" AllowMouseWheel="false" />
                                    
                                </dxe:ASPxTextBox>
                            </td>
                            <td><b>B:</b></td>
                            <td>
                                <dxe:ASPxTextBox ID="txtIndicatorB" runat="server" TextMode="Number" Width="100%" MaxLength="100" CssClass="upper" ClientInstanceName="CtxtIndicatorB">
                                    <ClientSideEvents LostFocus="txtIndicatorB_lostFocus"/>
                                    <MaskSettings Mask="<0..999>" AllowMouseWheel="false" />
                                </dxe:ASPxTextBox>
                            </td>
                            <td><b>C:</b></td>
                            <td>
                                <dxe:ASPxTextBox ID="txtIndicatorC" runat="server" TextMode="Number" ClientEnabled="false" Width="100%" MaxLength="100" CssClass="upper" ClientInstanceName="CtxtIndicatorC">
                                    <MaskSettings Mask="<0..999>" AllowMouseWheel="false" />
                                </dxe:ASPxTextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                
            </div>
            
            
             <div class="col-md-8 pb-5">
                
                <label style="margin-bottom: 0;margin-top: 5px;">&nbsp</label>
                <div class="">
                     <button id="btntotalproduct" class="btn btn-badge-color6 nocursor" type="button">Total :<span class="badge" id="spntotalproduct">0.00</span></button>
                     <button id="btntotalvalueA" class="btn btn-badge-color2 nocursor" type="button">A :<span id="spncurrencyA"></span><span class="badge" id="spntotalvalueA">0.00</span></button>
                     <button id="btntotalvalueB" class="btn btn-badge-color1 nocursor" type="button">B :<span id="spncurrencyB"></span><span class="badge" id="spntotalvalueB">0.00</span></button>
                     <button id="btntotalvalueC" class="btn btn-badge-color3 nocursor" type="button">C :<span id="spncurrencyC"></span><span class="badge" id="spntotalvalueC">0.00</span></button>
                     
                    <button id="btnShow" class="btn btn-success" type="button" onclick="btn_ShowRecordsClick(this);">Show</button>
                   <%-- <% if (rights.CanExport)
                    { %> --%>
                        <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="drdExport_SelectedIndexChanged"
                            AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                            <asp:ListItem Value="0">Export to</asp:ListItem>
                            <asp:ListItem Value="1">XLSX</asp:ListItem>
                            <asp:ListItem Value="2">PDF</asp:ListItem>
                            <asp:ListItem Value="3">CSV</asp:ListItem>
                            <asp:ListItem Value="4">RTF</asp:ListItem>
                        </asp:DropDownList>
                   <%-- <% } %>--%>
                </div>
            </div>
        </div>

        <div class="clearfix">
            <dxe:ASPxGridView runat="server" ID="ShowGrid" ClientInstanceName="cShowGrid" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False" KeyFieldName="SEQ"
                DataSourceID="GenerateEntityServerModeDataSource"  OnSummaryDisplayText="ShowGrid_SummaryDisplayText" KeyboardSupport="true" OnHtmlDataCellPrepared="ShowGrid_HtmlDataCellPrepared">
                <Columns>
                    <dxe:GridViewDataTextColumn FieldName="CATEGORY" Caption="Category" Width="15%" VisibleIndex="1" HeaderStyle-CssClass="colDisable">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="CLASS" Caption="Class" Width="15%" VisibleIndex="2" HeaderStyle-CssClass="colDisable">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="PARTICULARS" Caption="Particulars" Width="30%" VisibleIndex="3" HeaderStyle-CssClass="colDisable">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="NET_SALE" Caption="Net Sale(Qty)" Width="8%" VisibleIndex="4" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">
                         <HeaderStyle HorizontalAlign="Right" />
                        <%--<PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>--%>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="SALES_RATE" Caption="Rate" Width="8%" VisibleIndex="5" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">
                         <HeaderStyle HorizontalAlign="Right" />
                        <%--<PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>--%>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="SALES_VALUE" Caption="Sale Value" Width="8%" VisibleIndex="6" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">
                         <HeaderStyle HorizontalAlign="Right" />
                        <%--<PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>--%>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="VALUATION_RATE" Caption="Cost Per Unit(Valuation Rate)" Width="15%" VisibleIndex="7" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">
                         <HeaderStyle HorizontalAlign="Right" />
                        <%--<PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>--%>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="COSTOFSALE" Caption="Cost of Sales" Width="8%" VisibleIndex="8" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">
                         <HeaderStyle HorizontalAlign="Right" />
                        <%--<PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>--%>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="WEIGHTAGEONSALE" Caption="Weightage(On Sale)" Width="8%" VisibleIndex="9" Visible="false" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">
                         <HeaderStyle HorizontalAlign="Right" />
                        <%--<PropertiesTextEdit DisplayFormatString="0.00000"></PropertiesTextEdit>--%>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="WEIGHTAGEONCOST" Caption="Weightage(On Cost)" Width="8%" VisibleIndex="10" Visible="false" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">
                         <HeaderStyle HorizontalAlign="Right" />
                        <%--<PropertiesTextEdit DisplayFormatString="0.00000"></PropertiesTextEdit>--%>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="CUMUTATIVEWEIGHTAGE" Caption="Cumutative Weightage" Width="8%" VisibleIndex="11" CellStyle-HorizontalAlign="Right" Visible="false" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">
                         <HeaderStyle HorizontalAlign="Right" />
                        <%--<PropertiesTextEdit DisplayFormatString="0.00000"></PropertiesTextEdit>--%>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="ABCONSALEVALUE" Caption="ABC(On Sale Value)" Width="8%" VisibleIndex="12" HeaderStyle-CssClass="colDisable">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="ABCONCOSTVALUE" Caption="ABC(On Cost)" Width="8%" VisibleIndex="13" HeaderStyle-CssClass="colDisable">
                    </dxe:GridViewDataTextColumn>
                </Columns>
                <SettingsDataSecurity AllowDelete="false" AllowEdit="false" AllowInsert="false" />
                <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" ColumnResizeMode="Control" />
                <Settings ShowFooter="true" ShowGroupPanel="false" ShowGroupFooter="VisibleIfExpanded" />
                <SettingsEditing Mode="EditForm" />
                <SettingsContextMenu Enabled="true" />
                <settingssearchpanel visible="False" />
                <SettingsBehavior AutoExpandAllGroups="true"  AllowSort="False"/>
                <Settings ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" /> 
                <SettingsPager PageSize="50">
                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100" />
                </SettingsPager>
                <TotalSummary>
                    <dxe:ASPxSummaryItem FieldName="NET_SALE" SummaryType="Sum" />
                    <dxe:ASPxSummaryItem FieldName="SALES_VALUE" SummaryType="Sum" />
                    <dxe:ASPxSummaryItem FieldName="COSTOFSALE" SummaryType="Sum" />
                    <dxe:ASPxSummaryItem FieldName="WEIGHTAGEONSALE" SummaryType="Sum" />
                    <dxe:ASPxSummaryItem FieldName="WEIGHTAGEONCOST" SummaryType="Sum" />
                </TotalSummary>
                <ClientSideEvents EndCallback="OnEndCallback" />
            </dxe:ASPxGridView>
             <dx:LinqServerModeDataSource ID="GenerateEntityServerModeDataSource" runat="server" OnSelecting="GenerateEntityServerModeDataSource_Selecting"
                        ContextTypeName="ReportSourceDataContext" TableName="ABCANALYSIS_REPORT"></dx:LinqServerModeDataSource>
        </div>
    </div>

    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="true" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>

  <dxe:ASPxCallbackPanel runat="server" ID="ASPxCallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
    <PanelCollection>
        <dxe:PanelContent runat="server">
        <asp:HiddenField ID="hfIsABCAnalysis" runat="server" />
        </dxe:PanelContent>
    </PanelCollection>
    <ClientSideEvents EndCallback="CallbackPanelEndCall" />
 </dxe:ASPxCallbackPanel>

       <!--Class Modal -->
    <div class="modal fade" id="ClassModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Class Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Classkeydown(event)" id="txtClassSearch" width="100%" placeholder="Search By Class Name" />
                    <div id="ClassTable">
                        <table border='1' width="100%">

                            <tr class="HeaderStyle" style="font-size:small">
                                <th>Select</th>
                                 <th class="hide">id</th>
                                <th>Class Name</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btnOkformultiselection btn-default"  onclick="DeSelectAll('ClassSource')">Deselect All</button>
                    <button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal" onclick="OKPopup('ClassSource')">OK</button>
                    <%--<button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal">Close</button>--%>
                </div>
            </div>
        </div>
    </div>
      <asp:HiddenField ID="hdnClassId" runat="server" />
    <!--Class Modal -->
     <!--Brand Modal -->
    <div class="modal fade" id="BrandModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Brand Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Brandkeydown(event)" id="txtBrandSearch" width="100%" placeholder="Search By Brand Name" />
                    <div id="BrandTable">
                        <table border='1' width="100%">

                            <tr class="HeaderStyle" style="font-size:small">
                                <th>Select</th>
                                 <th class="hide">id</th>
                                <th>Name</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btnOkformultiselection btn-default"  onclick="DeSelectAll('BrandSource')">Deselect All</button>
                    <button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal" onclick="OKPopup('BrandSource')">OK</button>
                    <%--<button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal">Close</button>--%>
                </div>
            </div>
        </div>
    </div>
      <asp:HiddenField ID="hdnBranndId" runat="server" />
    <!--Brand Modal -->
</asp:Content>
