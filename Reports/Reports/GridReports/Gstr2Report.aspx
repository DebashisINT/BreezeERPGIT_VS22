<%@ Page Title="GSTR-2 All Report" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" EnableEventValidation="false" 
    AutoEventWireup="true"
    CodeBehind="Gstr2Report.aspx.cs" Inherits="Reports.Reports.GridReports.Gstr2Report" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="/assests/pluggins/choosen/choosen.min.js"></script>

    <style>

        #pageControl, .dxtc-content {
            overflow: visible !important;
        }

        #MandatoryAssign {
            position: absolute;
            right: -17px;
            top: 6px;
        }

        #MandatorySupervisorAssign {
            position: absolute;
            right: 1px;
            top: 27px;
        }

        .chosen-container.chosen-container-multi,
        .chosen-container.chosen-container-single {
            width: 100% !important;
        }

        .chosen-choices {
            width: 100% !important;
        }

        #ListBoxBranches {
            width: 200px;
        }

        .hide {
            display: none;
        }

        .dxtc-activeTab .dxtc-link {
            color: #fff !important;
        }

        /*#B2B, #B2B .dxgvCSD {
            width: 100% !important;
        }*/
    </style>

    <script type="text/javascript">

        function fn_OpenDetails(keyValue) {
            //cPopup_Empcitys.SetHeaderText('Modify Products');
            Grid.PerformCallback('Edit~' + keyValue);

        }



        $(function () {

            ///   BindBranches(null);

            function OnWaitingGridKeyPress(e) {
                alert('1Hi');
                if (e.code == "Enter") {

                }

            }


        });


        $(document).ready(function () {
            $("#ListBoxBranches").chosen().change(function () {
                var Ids = $(this).val();
                // BindLedgerType(Ids);                    

                $('#<%=hdnSelectedBranches.ClientID %>').val(Ids);
                $('#MandatoryActivityType').attr('style', 'display:none');

            })

        })



    </script>

    <script type="text/javascript">



        function btn_ShowRecordsClick(e) {
            e.preventDefault;
            var v = $("#ddlgstn").val();
            var activeTab = page.GetActiveTab();
            if (activeTab.name == 'B2B') {

                Gridb2b.PerformCallback('ListData~' + v);

            }
            else if (activeTab.name == 'B2BUR') {

                cgridB2BUR.PerformCallback('ListData~' + v);

            }

            else if (activeTab.name == 'IMPS') {

                cgridIMPS.PerformCallback('ListData~' + v);

            }

            else if (activeTab.name == 'IMPG') {

                cgridIMPG.PerformCallback('ListData~' + v);

            }


            else if (activeTab.name == 'CDNR') {
                
                cgridCDNR.PerformCallback('ListData~' + v);

            }


            else if (activeTab.name == 'CDNUR') {

                cgridCDNUR.PerformCallback('ListData~' + v);

            }

            else if (activeTab.name == 'EXEMP') {

                cgridEXEMP.PerformCallback('ListData~' + v);

            }
            else if (activeTab.name == 'ITCR') {

                cgridITCR.PerformCallback('ListData~' + v);

            }
            else if (activeTab.name == 'HSNSUM') {

                cgridHSNSUM.PerformCallback('ListData~' + v);

            }
            else if (activeTab.name == 'GSTINDocumnetCount') {

                GSTNInDocumentCount.PerformCallback('ListData~' + v);
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

        function OnContextMenuItemClick(sender, args) {
            if (args.item.name == "ExportToPDF" || args.item.name == "ExportToXLS") {
                args.processOnServer = true;
                args.usePostBack = true;
            } else if (args.item.name == "SumSelected")
                args.processOnServer = true;
        }


        function OpenBillDetails(branch) {


            cgridPendingApproval.PerformCallback('BndPopupgrid~' + branch);
            cpopupApproval.Show();
            return true;
        }

        function popupHide(s, e) {

            cpopupApproval.Hide();
        }


        function Callback_BeginCallback() {


            $("#drdExport").val(0);
        }
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
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                Gridb2b.SetWidth(cntWidth);
                cgridB2BUR.SetWidth(cntWidth);
                cgridIMPS.SetWidth(cntWidth);
                cgridIMPG.SetWidth(cntWidth);
                cgridCDNR.SetWidth(cntWidth);
                cgridCDNUR.SetWidth(cntWidth);
                cgridEXEMP.SetWidth(cntWidth);
                cgridITCR.SetWidth(cntWidth);
                cgridHSNSUM.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                Gridb2b.SetWidth(cntWidth);
                cgridB2BUR.SetWidth(cntWidth);
                cgridIMPS.SetWidth(cntWidth);
                cgridIMPG.SetWidth(cntWidth);
                cgridCDNR.SetWidth(cntWidth);
                cgridCDNUR.SetWidth(cntWidth);
                cgridEXEMP.SetWidth(cntWidth);
                cgridITCR.SetWidth(cntWidth);
                cgridHSNSUM.SetWidth(cntWidth);
            }

            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    Gridb2b.SetWidth(cntWidth);
                    cgridB2BUR.SetWidth(cntWidth);
                    cgridIMPS.SetWidth(cntWidth);
                    cgridIMPG.SetWidth(cntWidth);
                    cgridCDNR.SetWidth(cntWidth);
                    cgridCDNUR.SetWidth(cntWidth);
                    cgridEXEMP.SetWidth(cntWidth);
                    cgridITCR.SetWidth(cntWidth);
                    cgridHSNSUM.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    Gridb2b.SetWidth(cntWidth);
                    cgridB2BUR.SetWidth(cntWidth);
                    cgridIMPS.SetWidth(cntWidth);
                    cgridIMPG.SetWidth(cntWidth);
                    cgridCDNR.SetWidth(cntWidth);
                    cgridCDNUR.SetWidth(cntWidth);
                    cgridEXEMP.SetWidth(cntWidth);
                    cgridITCR.SetWidth(cntWidth);
                    cgridHSNSUM.SetWidth(cntWidth);
                }

            });
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <div class="panel-heading">
        <%--<div class="panel-title">
            <h3>GSTR-2 All</h3>
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
    <div class="form_main">
        <asp:HiddenField runat="server" ID="hdndaily" />
        <asp:HiddenField runat="server" ID="hdtid" />
        <table class="pull-left">
            <tr>


                <td style="width: 254px; display: none">


                    <asp:HiddenField ID="hdnActivityType" runat="server" />
                    <asp:HiddenField ID="hdnActivityTypeText" runat="server" />
                    <span id="MandatoryActivityType" style="display: none" class="validclass">
                        <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor1112_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                    <asp:HiddenField ID="hdnSelectedBranches" runat="server" />


                </td>



                <td>
                    <div style="color: #b5285f; font-weight: bold;" class="clsFrom">
                        <asp:Label ID="lblFromDate" runat="Server" Text="GSTIN : " CssClass="mylabel1"
                            Width="92px"></asp:Label>
                    </div>
                </td>
                <td>


                    <asp:DropDownList ID="ddlgstn" runat="server" Width="150px"></asp:DropDownList>

                </td>


                <td>

                    <table>

                        <tr>

                            <td>
                                <div style="color: #b5285f; font-weight: bold;" class="clsFrom">
                                    <asp:Label ID="Label1" runat="Server" Text="From Date : " CssClass="mylabel1"
                                        Width="92px"></asp:Label>
                                </div>
                            </td>
                            <td>
                                <dxe:ASPxDateEdit ID="ASPxFromDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                                    UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeFromDate">
                                    <ButtonStyle Width="13px">
                                    </ButtonStyle>
                                </dxe:ASPxDateEdit>
                            </td>
                            <td style="padding-left: 15px">
                                <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                                    <asp:Label ID="lblToDate" runat="Server" Text="To Date : " CssClass="mylabel1"
                                        Width="92px"></asp:Label>
                                </div>
                            </td>
                            <td>
                                <dxe:ASPxDateEdit ID="ASPxToDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                                    UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeToDate">
                                    <ButtonStyle Width="13px">
                                    </ButtonStyle>
                                </dxe:ASPxDateEdit>
                            </td>
                            
                            <td style="padding-left: 10px; padding-top: 3px">
                                <button id="btnShow" class="btn btn-success" type="button" onclick="btn_ShowRecordsClick(this);">Show</button>
                            </td>
                            <td>
                                <label>&nbsp</label><br />
                                <asp:CheckBox runat="server" ID="chkdocument" Checked="false" Text="Show by Purchase Entry Date" />
                            </td>

                        </tr>
                    </table>
                </td>



            </tr>



            <tr>
            </tr>
        </table>
        <div class="pull-right">



            <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary"
                OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                <asp:ListItem Value="0">Export to</asp:ListItem>
                <asp:ListItem Value="1">PDF</asp:ListItem>
                <asp:ListItem Value="2">XLSX</asp:ListItem>
                <asp:ListItem Value="3">RTF</asp:ListItem>
                <asp:ListItem Value="4">CSV</asp:ListItem>

            </asp:DropDownList>

        </div>
        <%--onkeypress="OnWaitingGridKeyPress(event)"--%>
        <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" ClientInstanceName="page"
            Font-Size="12px" Width="100%">

            <TabPages>
                <dxe:TabPage Name="B2B" Text="GSTR-2 B2B">
                    <ContentCollection>
                        <dxe:ContentControl runat="server">

                            
                                          <div class="GridViewArea">
                                              
                                            <dxe:AspxGridView runat="server" ID="B2B" ClientInstanceName="Gridb2b" Width="100%" 
                                                EnableRowsCache="false" ClientSideEvents-BeginCallback="Callback_BeginCallback"
                                                OnSummaryDisplayText="B2B_SummaryDisplayText" OnDataBound="B2B_Datarepared"  
                                                OnCustomSummaryCalculate="B2B_CustomSummaryCalculate"
                                                OnCustomCallback="B2B_CustomCallback" OnDataBinding="B2B_DataBinding" 
                                                AutoGenerateColumns="False"   
                                                Settings-HorizontalScrollBarMode="Visible" Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Visible">

                                                <Columns>
                                                    <dxe:GridViewDataTextColumn FieldName="GSTIN_of_Supplier" Caption="GSTIN/UIN of Supplier" VisibleIndex="1" Width="130px">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="Invoice_Number" Caption="Invoice Number" VisibleIndex="2" Width="130px">
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="Invoice_Date" Caption="Invoice Date" VisibleIndex="3" Width="105px">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="Invoice_Value" Caption="Invoice Value" VisibleIndex="4" 
                                                        PropertiesTextEdit-DisplayFormatString="0.00" Width="90px">
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="Place_Of_Supply" Caption="Place of Supply" VisibleIndex="5" Width="100px">
                                                    </dxe:GridViewDataTextColumn>



                                                    <dxe:GridViewDataTextColumn FieldName="Reverse_Charge" Caption="Reverse Charge" VisibleIndex="6" Width="95px">
                                                    </dxe:GridViewDataTextColumn>



                                                    <dxe:GridViewDataTextColumn FieldName="Invoice_Type" Caption="Invoice Type" VisibleIndex="7" Width="80px">
                                                    </dxe:GridViewDataTextColumn>
                                                     

                                                   <%-- <dxe:GridViewDataTextColumn FieldName="GSTIN E-Commerce" Caption="E-Commerce GSTIN" VisibleIndex="8" Width="10%">
                                                    </dxe:GridViewDataTextColumn>--%> 



                                                    <dxe:GridViewDataTextColumn FieldName="Rate" Caption="Rate" VisibleIndex="8" Width="50px" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="Taxable_value" Caption="Taxable Value" VisibleIndex="9" 
                                                        PropertiesTextEdit-DisplayFormatString="0.00" Width="100px">
                                                    </dxe:GridViewDataTextColumn>

                                                      <dxe:GridViewDataTextColumn FieldName="Integrated_Tax_Paid" Caption="Integrated Tax Paid" VisibleIndex="10"
                                                           PropertiesTextEdit-DisplayFormatString="0.00" Width="130px">
                                                    </dxe:GridViewDataTextColumn>

                                                     <dxe:GridViewDataTextColumn FieldName="Central_Tax_Paid" Caption="Central Tax Paid" VisibleIndex="11" 
                                                         PropertiesTextEdit-DisplayFormatString="0.00" Width="100px">
                                                    </dxe:GridViewDataTextColumn>
                                                    
                                                     <dxe:GridViewDataTextColumn FieldName="State_Tax_Paid" Caption="State/UT Tax Paid" VisibleIndex="12" 
                                                         PropertiesTextEdit-DisplayFormatString="0.00" Width="110px">
                                                    </dxe:GridViewDataTextColumn>

                                                     <dxe:GridViewDataTextColumn FieldName="Cess_Paid" Caption="Cess Paid" VisibleIndex="13" 
                                                         PropertiesTextEdit-DisplayFormatString="0.00" Width="90px">
                                                    </dxe:GridViewDataTextColumn>

                                                     <dxe:GridViewDataTextColumn FieldName="Eligibility_For_ITC" Caption="Eligibility For ITC" VisibleIndex="14" 
                                                         PropertiesTextEdit-DisplayFormatString="0.00" Width="110px">
                                                    </dxe:GridViewDataTextColumn>

                                                     <dxe:GridViewDataTextColumn FieldName="Availed_ITC_Integrated_Tax" Caption="Availed ITC Integrated Tax" VisibleIndex="15" 
                                                         PropertiesTextEdit-DisplayFormatString="0.00" Width="160px">
                                                    </dxe:GridViewDataTextColumn>

                                                     <dxe:GridViewDataTextColumn FieldName="Availed_ITC_Central_Tax" Caption="Availed ITC Central Tax" VisibleIndex="16"
                                                          PropertiesTextEdit-DisplayFormatString="0.00" Width="140px">
                                                    </dxe:GridViewDataTextColumn>

                                                     <dxe:GridViewDataTextColumn FieldName="Availed_ITC_State_Tax" Caption="Availed ITC State/UT Tax" VisibleIndex="17" 
                                                         PropertiesTextEdit-DisplayFormatString="0.00" Width="150px">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="Availed_ITC_Cess" Caption="Availed ITC Cess" VisibleIndex="18" 
                                                        PropertiesTextEdit-DisplayFormatString="0.00" Width="100px">
                                                    </dxe:GridViewDataTextColumn>


                                                    <%--<dxe:GridViewDataTextColumn FieldName="cesss" Caption="Cess Amount" VisibleIndex="11" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>--%>


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

                                                  <Settings ShowGroupPanel="True"  ShowStatusBar="Hidden"  ShowFilterRow="true" ShowFilterRowMenu="true" />
                                               
                                                

                                                <TotalSummary>
                                                    <dxe:ASPxSummaryItem FieldName="GSTIN_of_Supplier" SummaryType="Custom" DisplayFormat="Count" />
                                                    <dxe:ASPxSummaryItem FieldName="Invoice_Number" SummaryType="Custom" DisplayFormat="Count" />
                                                    <dxe:ASPxSummaryItem FieldName="Invoice_Value" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="Taxable_value" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="Integrated_Tax_Paid" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="Central_Tax_Paid" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="State_Tax_Paid" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="Cess_Paid" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="Availed_ITC_Integrated_Tax" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="Availed_ITC_Central_Tax" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="Availed_ITC_State_Tax" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="Availed_ITC_Cess" SummaryType="Sum" /> 
                                                </TotalSummary>

                                            </dxe:AspxGridView>

                                        </div>
                                   

                        </dxe:ContentControl>
                    </ContentCollection>
                </dxe:TabPage>


                <dxe:TabPage Name="B2BUR" Text="GSTR-2 B2BUR">
                    <ContentCollection>
                        <dxe:ContentControl runat="server">

                            <%--<table class="TableMain100">

                                <tr>
                                    <td colspan="2">
                                        <div>--%>
                             <div class="GridViewArea">
                                            <dxe:ASPxGridView runat="server" ID="grid_B2BUR" ClientInstanceName="cgridB2BUR" Width="100%" EnableRowsCache="false"
                                                OnSummaryDisplayText="grid_B2BUR_SummaryDisplayText"
                                                OnCustomSummaryCalculate="grid_B2BUR_CustomSummaryCalculate" ClientSideEvents-BeginCallback="Callback_BeginCallback"
                                                OnCustomCallback="grid_B2BUR__CustomCallback" OnDataBinding="grid_B2BUR_DataBinding"
                                             AutoGenerateColumns="False"   Settings-HorizontalScrollBarMode="Visible" Settings-VerticalScrollableHeight="300" 
                                                Settings-VerticalScrollBarMode="Visible">

                                                <Columns>



                                                    <dxe:GridViewDataTextColumn FieldName="Supplier_Name" Caption="Supplier Name"  VisibleIndex="2" Width="130px">
                                                    </dxe:GridViewDataTextColumn>


                                                   <%-- <dxe:GridViewDataTextColumn FieldName="GSTIN_of_Supplier" Caption="Invoice Date" VisibleIndex="3">
                                                    </dxe:GridViewDataTextColumn>--%>

                                                    <dxe:GridViewDataTextColumn FieldName="Invoice_Number" Caption="Invoice Number" VisibleIndex="4"  Width="130px">
                                                    </dxe:GridViewDataTextColumn> 
                                                    <dxe:GridViewDataTextColumn FieldName="Invoice_Date" Caption="Invoice date" VisibleIndex="5" Width="110px">
                                                    </dxe:GridViewDataTextColumn> 
                                                    <dxe:GridViewDataTextColumn FieldName="Invoice_Value" Caption="Invoice Value" VisibleIndex="6" 
                                                        PropertiesTextEdit-DisplayFormatString="0.00" Width="100px">
                                                    </dxe:GridViewDataTextColumn>
                                                      <dxe:GridViewDataTextColumn FieldName="Place_Of_Supply" Caption="Place Of Supply" VisibleIndex="8" Width="130px">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="Supply_Type" Caption="Supply Type" VisibleIndex="9" Width="110px">
                                                    </dxe:GridViewDataTextColumn>
                                                     <dxe:GridViewDataTextColumn FieldName="Rate" Caption="Rate" VisibleIndex="9" Width="50px">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="Taxable_value" Caption="Taxable Value" VisibleIndex="9" Width="100px" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="Integrated_Tax_Paid" Caption="Integrated Tax Paid" VisibleIndex="9" Width="120px" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="Central_Tax_Paid" Caption="Central Tax Paid" VisibleIndex="9" Width="110px" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>
                                                      <dxe:GridViewDataTextColumn FieldName="State_Tax_Paid" Caption="State/UT Tax Paid" VisibleIndex="9" Width="120px" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="Cess_Paid" Caption="Cess Paid" VisibleIndex="9" Width="90px" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="Eligibility_For_ITC" Caption="Eligibility For ITC" VisibleIndex="9" Width="110px">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="Availed_ITC_Integrated_Tax" Caption="Availed ITC Integrated Tax" VisibleIndex="9" Width="155px" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="Availed_ITC_Central_Tax" Caption="Availed ITC Central Tax" VisibleIndex="9" Width="140px" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>                                                    
                                                    <dxe:GridViewDataTextColumn FieldName="Availed_ITC_State_Tax" Caption="Availed ITC State/UT Tax" VisibleIndex="9" Width="150px" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>                                                    
                                                    <dxe:GridViewDataTextColumn FieldName="Availed_ITC_Cess" Caption="Availed ITC Cess" VisibleIndex="9" Width="110px" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>



                                                    <%--<dxe:GridViewDataTextColumn FieldName="DOC_TYPE" Caption="Taxable value" Width="20%" VisibleIndex="7" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>--%> 

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

                                                  <Settings ShowGroupPanel="True" ShowStatusBar="Hidden"  ShowFilterRow="true" ShowFilterRowMenu="true" />
                                              

                                                <TotalSummary>
                                                    <dxe:ASPxSummaryItem FieldName="Invoice_Number" SummaryType="Custom" DisplayFormat="Count" />
                                                    <dxe:ASPxSummaryItem FieldName="Invoice_Value" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="Taxable_value" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="Integrated_Tax_Paid" SummaryType="Sum" />
                                                     <dxe:ASPxSummaryItem FieldName="Central_Tax_Paid" SummaryType="Sum" />
                                                     <dxe:ASPxSummaryItem FieldName="State_Tax_Paid" SummaryType="Sum" />
                                                     <dxe:ASPxSummaryItem FieldName="Cess_Paid" SummaryType="Sum" />
                                                     <dxe:ASPxSummaryItem FieldName="Availed_ITC_Integrated_Tax" SummaryType="Sum" />
                                                     <dxe:ASPxSummaryItem FieldName="Availed_ITC_Central_Tax" SummaryType="Sum" />
                                                     <dxe:ASPxSummaryItem FieldName="Availed_ITC_State_Tax" SummaryType="Sum" />
                                                     <dxe:ASPxSummaryItem FieldName="Availed_ITC_Cess" SummaryType="Sum" />

                                                    



                                                </TotalSummary>

                                            </dxe:ASPxGridView>

                                        </div>
                                    <%--</td>
                                </tr>
                            </table>--%>
                        </dxe:ContentControl>
                    </ContentCollection>
                </dxe:TabPage>


                <dxe:TabPage Name="IMPS" Text="GSTR-2 IMPS">
                    <ContentCollection>
                        <dxe:ContentControl runat="server">
                           <%-- <table class="TableMain100">

                                <tr>
                                    <td colspan="2">--%>
                                         
                                             <div class="GridViewArea">
                                            <dxe:ASPxGridView runat="server" ID="grid_IMPS" ClientInstanceName="cgridIMPS" Width="100%" EnableRowsCache="false"
                                                OnSummaryDisplayText="Grid_IMPS_SummaryDisplayText" ClientSideEvents-BeginCallback="Callback_BeginCallback"
                                                OnCustomCallback="Grid_IMPS__CustomCallback" OnDataBinding="grid_IMPS_DataBinding"
                                                OnCustomSummaryCalculate="GridView_IMPS_CustomSummaryCalculate"
                                             AutoGenerateColumns="False"   Settings-HorizontalScrollBarMode="Visible" Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Visible">

                                                <Columns>


                                                    <%--<dxe:GridViewDataTextColumn FieldName="GSTIN" Caption="Type" VisibleIndex="2">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="Document_Id" Caption="Place of Supply" VisibleIndex="2">
                                                    </dxe:GridViewDataTextColumn>--%>


                                                    <dxe:GridViewDataTextColumn FieldName="Invoice Number of Reg Recipient" Caption="Invoice Number of Reg Recipient" VisibleIndex="3"  Width="200px">
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="Invoice Date" Caption="Invoice Date" VisibleIndex="4"  Width="100px">
                                                    </dxe:GridViewDataTextColumn>

                                                   <%-- <dxe:GridViewDataTextColumn FieldName="DOC_TYPE" Caption="Cess Amount" VisibleIndex="5" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>--%>



                                                    <dxe:GridViewDataTextColumn FieldName="Invoice Value" Caption="Invoice Value" VisibleIndex="6" PropertiesTextEdit-DisplayFormatString="0.00" 
                                                        Width="100px">
                                                    </dxe:GridViewDataTextColumn>
                                                    
                                                     <%-- <dxe:GridViewDataTextColumn FieldName="StateId" Caption="E-Commerce GSTIN" VisibleIndex="6">
                                                    </dxe:GridViewDataTextColumn>
                                                      <dxe:GridViewDataTextColumn FieldName="State_code" Caption="E-Commerce GSTIN" VisibleIndex="6">
                                                    </dxe:GridViewDataTextColumn>--%>
                                                      <dxe:GridViewDataTextColumn FieldName="Place Of Supply" Caption="Place Of Supply" VisibleIndex="6" Width="100px">
                                                    </dxe:GridViewDataTextColumn>
                                                      <dxe:GridViewDataTextColumn FieldName="Rate" Caption="Rate" VisibleIndex="6" PropertiesTextEdit-DisplayFormatString="0.00" Width="50px">
                                                    </dxe:GridViewDataTextColumn>
                                                      <dxe:GridViewDataTextColumn FieldName="Taxable value" Caption="Taxable Value" VisibleIndex="6" PropertiesTextEdit-DisplayFormatString="0.00" 
                                                          Width="100px">
                                                    </dxe:GridViewDataTextColumn>
                                                      <dxe:GridViewDataTextColumn FieldName="Integrated Tax Paid" Caption="Integrated Tax Paid" VisibleIndex="6" 
                                                          PropertiesTextEdit-DisplayFormatString="0.00" Width="120px">
                                                    </dxe:GridViewDataTextColumn>
                                                      
                                                      <dxe:GridViewDataTextColumn FieldName="CESS_PAID" Caption="Cess Paid" VisibleIndex="6" PropertiesTextEdit-DisplayFormatString="0.00" Width="90px">
                                                    </dxe:GridViewDataTextColumn>



                                                    <%--<dxe:GridViewDataTextColumn FieldName="Total_CGST" Caption="E-Commerce GSTIN" VisibleIndex="6">
                                                    </dxe:GridViewDataTextColumn>
                                                      <dxe:GridViewDataTextColumn FieldName="Total_SGST" Caption="E-Commerce GSTIN" VisibleIndex="6">
                                                    </dxe:GridViewDataTextColumn>
                                                      <dxe:GridViewDataTextColumn FieldName="Total_UTGST" Caption="E-Commerce GSTIN" VisibleIndex="6">
                                                    </dxe:GridViewDataTextColumn>--%>

                                                    
                                                    <dxe:GridViewDataTextColumn FieldName="Eligibility_For_ITC" Caption="Eligibility For ITC" VisibleIndex="6" Width="120px">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="Availed ITC Integrated Tax" Caption="Availed ITC Integrated Tax" VisibleIndex="6" PropertiesTextEdit-DisplayFormatString="0.00" Width="160px">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="Availed ITC Cess" Caption="Availed ITC Cess" VisibleIndex="6" PropertiesTextEdit-DisplayFormatString="0.00" Width="130px">
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
                                                    
                                                    <dxe:ASPxSummaryItem FieldName="Invoice Number of Reg Recipient" SummaryType="Custom" DisplayFormat="Count" />
 
                                                    <dxe:ASPxSummaryItem FieldName="Invoice Value" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="Taxable value" SummaryType="Sum" />

                                                    <dxe:ASPxSummaryItem FieldName="Integrated Tax Paid" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="CESS_PAID" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="Availed ITC Integrated Tax" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="Availed ITC Cess" SummaryType="Sum" />




                                                </TotalSummary>

                                            </dxe:ASPxGridView>

                                        </div>
                                   <%-- </td>
                                </tr>
                            </table>--%>

                        </dxe:ContentControl>
                    </ContentCollection>
                </dxe:TabPage>



                <dxe:TabPage Name="IMPG" Text="GSTR-2 IMPG">
                    <ContentCollection>
                        <dxe:ContentControl runat="server">

                            <table class="TableMain100">
                                <tr>
                                    <td colspan="2">
                                        <div class="GridViewArea">
                                            <dxe:ASPxGridView runat="server" ID="grid_IMPG" ClientInstanceName="cgridIMPG" Width="100%"
                                                EnableRowsCache="false" ClientSideEvents-BeginCallback="Callback_BeginCallback"
                                                OnSummaryDisplayText="Grid_IMPG_SummaryDisplayText" Settings-HorizontalScrollBarMode="Visible"
                                                OnCustomSummaryCalculate="GridView_IMPG_CustomSummaryCalculate"
                                                OnCustomCallback="Grid_IMPG_CustomCallback" OnDataBinding="grid_IMPG_DataBinding">

                                                 <Columns>                                               
                                                  <%--  <dxe:GridViewDataTextColumn FieldName="Document_Id" Caption="Place of Supply" VisibleIndex="1" Width="150px">
                                                    </dxe:GridViewDataTextColumn>--%>


                                                    <dxe:GridViewDataTextColumn FieldName="Invoice Number of Reg Recipient" Caption="Bill Of Entry Number" VisibleIndex="2" Width="140px">
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="Invoice Date" Caption="Bill Of Entry Date" VisibleIndex="3"  Width="110px">
                                                    </dxe:GridViewDataTextColumn>
                                                      <dxe:GridViewDataTextColumn FieldName="Invoice Value" Caption="Bill Of Entry Value" VisibleIndex="4" PropertiesTextEdit-DisplayFormatString="0.00" Width="110px">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="DOC_TYPE" Caption="Document type" VisibleIndex="5" Width="120px">
                                                    </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewDataTextColumn FieldName="GSTIN" Caption="GSTIN Of SEZ Supplier" VisibleIndex="6" Width="140px">
                                                    </dxe:GridViewDataTextColumn>                                                   
                                                    
                                                     <%-- <dxe:GridViewDataTextColumn FieldName="StateId" Caption="E-Commerce GSTIN" VisibleIndex="6">
                                                    </dxe:GridViewDataTextColumn>
                                                      <dxe:GridViewDataTextColumn FieldName="State_code" Caption="E-Commerce GSTIN" VisibleIndex="6">
                                                    </dxe:GridViewDataTextColumn>--%>
                                              <%--        <dxe:GridViewDataTextColumn FieldName="Place Of Supply" Caption="Place Of Supply" VisibleIndex="6">
                                                    </dxe:GridViewDataTextColumn>--%>
                                                      <dxe:GridViewDataTextColumn FieldName="Rate" Caption="Rate" VisibleIndex="7" PropertiesTextEdit-DisplayFormatString="0.00" Width="60px">
                                                    </dxe:GridViewDataTextColumn>
                                                      <dxe:GridViewDataTextColumn FieldName="Taxable value" Caption="Taxable Value" VisibleIndex="8" PropertiesTextEdit-DisplayFormatString="0.00" Width="110px">
                                                    </dxe:GridViewDataTextColumn>
                                                      <dxe:GridViewDataTextColumn FieldName="Integrated Tax Paid" Caption="Integrated Tax Paid" VisibleIndex="9" PropertiesTextEdit-DisplayFormatString="0.00" Width="140px">
                                                    </dxe:GridViewDataTextColumn>
                                                      
                                                      <dxe:GridViewDataTextColumn FieldName="CESS_PAID" Caption="Cess Paid" VisibleIndex="10" PropertiesTextEdit-DisplayFormatString="0.00" Width="90px">
                                                    </dxe:GridViewDataTextColumn>



                                                    <%--<dxe:GridViewDataTextColumn FieldName="Total_CGST" Caption="E-Commerce GSTIN" VisibleIndex="6">
                                                    </dxe:GridViewDataTextColumn>
                                                      <dxe:GridViewDataTextColumn FieldName="Total_SGST" Caption="E-Commerce GSTIN" VisibleIndex="6">
                                                    </dxe:GridViewDataTextColumn>
                                                      <dxe:GridViewDataTextColumn FieldName="Total_UTGST" Caption="E-Commerce GSTIN" VisibleIndex="6">
                                                    </dxe:GridViewDataTextColumn>--%>

                                                    
                                                    <dxe:GridViewDataTextColumn FieldName="Eligibility_For_ITC" Caption="Eligibility For ITC" VisibleIndex="11" Width="140px">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="Availed ITC Integrated Tax" Caption="Availed ITC Integrated Tax" VisibleIndex="12" PropertiesTextEdit-DisplayFormatString="0.00" Width="160px">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="Availed ITC Cess" Caption="Availed ITC Cess" VisibleIndex="13" PropertiesTextEdit-DisplayFormatString="0.00" Width="140px">
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
                                                     <dxe:ASPxSummaryItem FieldName="Invoice Number of Reg Recipient" SummaryType="Custom" DisplayFormat="Count" />
 
                                                    <dxe:ASPxSummaryItem FieldName="Invoice Value" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="Taxable value" SummaryType="Sum" />

                                                    <dxe:ASPxSummaryItem FieldName="Integrated Tax Paid" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="CESS_PAID" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="Availed ITC Integrated Tax" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="Availed ITC Cess" SummaryType="Sum" />
                                                     


                                                   

                                                </TotalSummary>

                                            </dxe:ASPxGridView>

                                        </div>
                                    </td>
                                </tr>
                            </table>



                        </dxe:ContentControl>
                    </ContentCollection>
                </dxe:TabPage>


                <dxe:TabPage Name="CDNR" Text="GSTR-2 CDNR">
                    <ContentCollection>
                        <dxe:ContentControl runat="server">

 
                              <div class="GridViewArea">
                                        <div class="GridViewArea">
                                            <dxe:ASPxGridView runat="server" ID="grid_CDNR" ClientInstanceName="cgridCDNR" Width="100%" EnableRowsCache="false"
                                                 AutoGenerateColumns="False" BeginCallback="Callback_BeginCallback"
                                                OnCustomSummaryCalculate="grid_CDNR_CustomSummaryCalculate" OnCustomCallback="grid_CDNR_CustomCallback" 
                                                OnDataBinding="grid_CDNR_DataBinding" OnSummaryDisplayText="grid_CDNR_SummaryDisplayText"
                                                Settings-HorizontalScrollBarMode="Visible" Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Visible">

                                                <Columns>
                                                    
                                                     <dxe:GridViewDataTextColumn FieldName="GSTIN of Supplier" Caption="GSTIN of Supplier" VisibleIndex="1" Width="140px">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="Note/Refund Voucher Number" Caption="Note/Refund Voucher Number" VisibleIndex="2" Width="190px">
                                                    </dxe:GridViewDataTextColumn> 
                                                    <dxe:GridViewDataTextColumn FieldName="Note/Refund Voucher Date" Caption="Note/Refund Voucher date" VisibleIndex="3" Width="170px">
                                                    </dxe:GridViewDataTextColumn> 
                                                    <dxe:GridViewDataTextColumn FieldName="Invoice/Advance Payment Voucher Number" Caption="Invoice/Advance Payment Voucher Number" VisibleIndex="4" Width="250px">
                                                    </dxe:GridViewDataTextColumn> 
                                                    <dxe:GridViewDataTextColumn FieldName="Invoice/Advance Payment Voucher Date" Caption="Invoice/Advance Payment Voucher date" VisibleIndex="5" Width="230px">
                                                    </dxe:GridViewDataTextColumn> 
                                                    <dxe:GridViewDataTextColumn FieldName="Pre GST" Caption="Pre GST" VisibleIndex="6" Width="70px">
                                                    </dxe:GridViewDataTextColumn>  
                                                    <dxe:GridViewDataTextColumn FieldName="Document Type" Caption="Document Type" VisibleIndex="7" Width="100px">
                                                    </dxe:GridViewDataTextColumn> 
                                                    <dxe:GridViewDataTextColumn FieldName="Reason For Issuing Document" Caption="Reason For Issuing document" VisibleIndex="8" Width="180px">
                                                    </dxe:GridViewDataTextColumn> 
                                                    <dxe:GridViewDataTextColumn FieldName="Supply Type" Caption="Supply Type" VisibleIndex="9"  Width="100px">
                                                    </dxe:GridViewDataTextColumn> 
                                                    <dxe:GridViewDataTextColumn FieldName="Note/Refund Voucher Value" Caption="Note/Refund Voucher Value" VisibleIndex="10" PropertiesTextEdit-DisplayFormatString="0.00" Width="175px">
                                                    </dxe:GridViewDataTextColumn> 
                                                    <dxe:GridViewDataTextColumn FieldName="Rate" Caption="Rate" VisibleIndex="11" PropertiesTextEdit-DisplayFormatString="0.00" Width="50px">
                                                    </dxe:GridViewDataTextColumn> 
                                                    <dxe:GridViewDataTextColumn FieldName="Taxable Value" Caption="Taxable Value" VisibleIndex="12" PropertiesTextEdit-DisplayFormatString="0.00" Width="100px">
                                                    </dxe:GridViewDataTextColumn> 
                                                    <dxe:GridViewDataTextColumn FieldName="Integrated Tax Paid" Caption="Integrated Tax Paid" VisibleIndex="13" PropertiesTextEdit-DisplayFormatString="0.00" Width="130px">
                                                    </dxe:GridViewDataTextColumn>
                                                   
                                                     <dxe:GridViewDataTextColumn FieldName="Central Tax Paid" Caption="Central Tax Paid" VisibleIndex="14" PropertiesTextEdit-DisplayFormatString="0.00" Width="130px">
                                                    </dxe:GridViewDataTextColumn> 
                                                    <dxe:GridViewDataTextColumn FieldName="State/UT Tax Paid" Caption="State/UT Tax Paid" VisibleIndex="15" PropertiesTextEdit-DisplayFormatString="0.00" Width="130px">
                                                    </dxe:GridViewDataTextColumn> 
                                                    <dxe:GridViewDataTextColumn FieldName="Cess Paid" Caption="Cess Paid" VisibleIndex="16" PropertiesTextEdit-DisplayFormatString="0.00" Width="90px">
                                                    </dxe:GridViewDataTextColumn> 
                                                    <dxe:GridViewDataTextColumn FieldName="Eligibility For ITC" Caption="Eligibility For ITC" VisibleIndex="17" Width="120px">
                                                    </dxe:GridViewDataTextColumn> 
                                                    <dxe:GridViewDataTextColumn FieldName="Availed ITC Integrated Tax" Caption="Availed ITC Integrated Tax" VisibleIndex="18" PropertiesTextEdit-DisplayFormatString="0.00" Width="160px">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="Availed ITC Central Tax" Caption="Availed ITC Central Tax" VisibleIndex="19" PropertiesTextEdit-DisplayFormatString="0.00" Width="150px">
                                                    </dxe:GridViewDataTextColumn> 
                                                    <dxe:GridViewDataTextColumn FieldName="Availed ITC State/UT Tax" Caption="Availed ITC State/UT Tax" VisibleIndex="20" PropertiesTextEdit-DisplayFormatString="0.00" Width="160px">
                                                    </dxe:GridViewDataTextColumn> 
                                                    <dxe:GridViewDataTextColumn FieldName="Availed ITC Cess" Caption="Availed ITC Cess" VisibleIndex="21" PropertiesTextEdit-DisplayFormatString="0.00" Width="130px">
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

                                                    <dxe:ASPxSummaryItem FieldName="GSTIN of Supplier" SummaryType="Custom" DisplayFormat="Count" />

                                                    <dxe:ASPxSummaryItem FieldName="Note/Refund Voucher Number" SummaryType="Custom" DisplayFormat="Count" />

                                                    <dxe:ASPxSummaryItem FieldName="Invoice/Advance Payment Voucher Number" SummaryType="Custom" DisplayFormat="Count" />

                                                    <dxe:ASPxSummaryItem FieldName="Note/Refund Voucher Value" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="Taxable Value" SummaryType="Sum" />

                                                    <dxe:ASPxSummaryItem FieldName="Integrated Tax Paid" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="Central Tax Paid" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="State/UT Tax Paid" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="Cess Paid" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="Availed ITC Integrated Tax" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="Availed ITC Central Tax" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="Availed ITC State/UT Tax" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="Availed ITC Cess" SummaryType="Sum" />

                                                    
                                                </TotalSummary>

                                            </dxe:ASPxGridView>

                                        </div>
                                     
                                  </div>


                        </dxe:ContentControl>
                    </ContentCollection>
                </dxe:TabPage>



                <dxe:TabPage Name="CDNUR" Text="GSTR-2 CDNUR">
                    <ContentCollection>
                        <dxe:ContentControl runat="server">

                            
                                          <div class="GridViewArea">
                                            <dxe:ASPxGridView runat="server" ID="grid_CDNUR" ClientInstanceName="cgridCDNUR" Width="100%" EnableRowsCache="false" BeginCallback="Callback_BeginCallback"
                                                OnSummaryDisplayText="grid_CDNUR_SummaryDisplayText" 
                                                 OnCustomSummaryCalculate="grid_CDNUR_CustomSummaryCalculate"
                                                OnCustomCallback="grid_CDNUR_CustomCallback" OnDataBinding="grid_CDNUR_DataBinding"
                                              AutoGenerateColumns="False"  Settings-HorizontalScrollBarMode="Visible" Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Visible">
                                                 
                                                 <Columns>
                                                    <%--<dxe:GridViewDataTextColumn FieldName="GSTIN of Supplier" Caption="GSTIN of Supplier" VisibleIndex="1" Width="140px">
                                                    </dxe:GridViewDataTextColumn>--%>

                                                    <dxe:GridViewDataTextColumn FieldName="Note/Refund Voucher Number" Caption="Note/Voucher Number" VisibleIndex="2" Width="140px">
                                                    </dxe:GridViewDataTextColumn> 
                                                    <dxe:GridViewDataTextColumn FieldName="Note/Refund Voucher Date" Caption="Note/Voucher date" VisibleIndex="3" Width="130px">
                                                    </dxe:GridViewDataTextColumn> 
                                                    <dxe:GridViewDataTextColumn FieldName="Invoice/Advance Payment Voucher Number" Caption="Invoice/Advance Payment Voucher Number" VisibleIndex="4" Width="260px">
                                                    </dxe:GridViewDataTextColumn> 
                                                    <dxe:GridViewDataTextColumn FieldName="Invoice/Advance Payment Voucher Date" Caption="Invoice/Advance Payment Voucher date" VisibleIndex="5" Width="230px">
                                                    </dxe:GridViewDataTextColumn> 
                                                    <dxe:GridViewDataTextColumn FieldName="Pre GST" Caption="Pre GST" VisibleIndex="6" Width="60px">
                                                    </dxe:GridViewDataTextColumn>  
                                                    <dxe:GridViewDataTextColumn FieldName="Reason For Issuing Document" Caption="Reason For Issuing document" VisibleIndex="7" Width="170px">
                                                    </dxe:GridViewDataTextColumn> 
                                                    <dxe:GridViewDataTextColumn FieldName="Supply Type" Caption="Supply Type" VisibleIndex="8"  Width="90px">
                                                    </dxe:GridViewDataTextColumn> 
                                                    <dxe:GridViewDataTextColumn FieldName="Document Type" Caption="Document Type" VisibleIndex="9" Width="100px">
                                                    </dxe:GridViewDataTextColumn> 
                                                    <dxe:GridViewDataTextColumn FieldName="Note/Refund Voucher Value" Caption="Note/Voucher Value" VisibleIndex="10" PropertiesTextEdit-DisplayFormatString="0.00" Width="125px">
                                                    </dxe:GridViewDataTextColumn> 
                                                    <dxe:GridViewDataTextColumn FieldName="Rate" Caption="Rate" VisibleIndex="11" PropertiesTextEdit-DisplayFormatString="0.00" Width="50px">
                                                    </dxe:GridViewDataTextColumn> 
                                                    <dxe:GridViewDataTextColumn FieldName="Taxable Value" Caption="Taxable Value" VisibleIndex="12" PropertiesTextEdit-DisplayFormatString="0.00" Width="90px">
                                                    </dxe:GridViewDataTextColumn> 
                                                    <dxe:GridViewDataTextColumn FieldName="Integrated Tax Paid" Caption="Integrated Tax Paid" VisibleIndex="13" PropertiesTextEdit-DisplayFormatString="0.00" Width="125px">
                                                    </dxe:GridViewDataTextColumn>
                                                   
                                                     <dxe:GridViewDataTextColumn FieldName="Central Tax Paid" Caption="Central Tax Paid" VisibleIndex="14" PropertiesTextEdit-DisplayFormatString="0.00" Width="100px">
                                                    </dxe:GridViewDataTextColumn> 
                                                    <dxe:GridViewDataTextColumn FieldName="State/UT Tax Paid" Caption="State/UT Tax Paid" VisibleIndex="15" PropertiesTextEdit-DisplayFormatString="0.00" Width="110px">
                                                    </dxe:GridViewDataTextColumn> 
                                                    <dxe:GridViewDataTextColumn FieldName="Cess Paid" Caption="Cess Paid" VisibleIndex="16" PropertiesTextEdit-DisplayFormatString="0.00" Width="60px">
                                                    </dxe:GridViewDataTextColumn> 
                                                    <dxe:GridViewDataTextColumn FieldName="Eligibility For ITC" Caption="Eligibility For ITC" VisibleIndex="17"  Width="100px">
                                                    </dxe:GridViewDataTextColumn> 
                                                    <dxe:GridViewDataTextColumn FieldName="Availed ITC Integrated Tax" Caption="Availed ITC Integrated Tax" VisibleIndex="18" PropertiesTextEdit-DisplayFormatString="0.00" Width="160px">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="Availed ITC Central Tax" Caption="Availed ITC Central Tax" VisibleIndex="19" PropertiesTextEdit-DisplayFormatString="0.00" Width="140px">
                                                    </dxe:GridViewDataTextColumn> 
                                                    <dxe:GridViewDataTextColumn FieldName="Availed ITC State/UT Tax" Caption="Availed ITC State/UT Tax" VisibleIndex="20" PropertiesTextEdit-DisplayFormatString="0.00" Width="150px">
                                                    </dxe:GridViewDataTextColumn> 
                                                    <dxe:GridViewDataTextColumn FieldName="Availed ITC Cess" Caption="Availed ITC Cess" VisibleIndex="21" PropertiesTextEdit-DisplayFormatString="0.00" Width="110px">
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
                                                    
                                                    <dxe:ASPxSummaryItem FieldName="Note/Refund Voucher Number" SummaryType="Custom" DisplayFormat="Count" />
                                                    <dxe:ASPxSummaryItem FieldName="Invoice/Advance Payment Voucher Number" SummaryType="Custom" DisplayFormat="Count" />
                                                    <dxe:ASPxSummaryItem FieldName="Note/Refund Voucher Value" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="Taxable Value" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="Integrated Tax Paid" SummaryType="Sum" /> 
                                                    <dxe:ASPxSummaryItem FieldName="Central Tax Paid" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="State/UT Tax Paid" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="Cess Paid" SummaryType="Sum" /> 
                                                    <dxe:ASPxSummaryItem FieldName="Availed ITC Integrated Tax" SummaryType="Sum" /> 
                                                    <dxe:ASPxSummaryItem FieldName="Availed ITC Central Tax" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="Availed ITC State/UT Tax" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="Availed ITC Cess" SummaryType="Sum" /> 
                                                </TotalSummary>
                                            </dxe:ASPxGridView>
                                        </div>
                        </dxe:ContentControl>
                    </ContentCollection>
                </dxe:TabPage>

                <dxe:TabPage Name="EXEMP" Text="GSTR-2 EXEMP">
                    <ContentCollection>
                        <dxe:ContentControl runat="server">                            
                            <div class="GridViewArea">
                            <dxe:ASPxGridView runat="server" ID="grid_EXEMP" ClientInstanceName="cgridEXEMP" Width="100%" EnableRowsCache="false" BeginCallback="Callback_BeginCallback"
                                OnSummaryDisplayText="grid_EXEMP_SummaryDisplayText" 
                                    OnCustomSummaryCalculate="grid_EXEMP_CustomSummaryCalculate"
                                OnCustomCallback="grid_EXEMP_CustomCallback" OnDataBinding="grid_EXEMP_DataBinding"
                                AutoGenerateColumns="False"  Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Visible">                                                 
                                    <Columns>
                                    <dxe:GridViewDataTextColumn FieldName="Description" Caption="Description" VisibleIndex="1" Width="200">
                                    </dxe:GridViewDataTextColumn> 
                                    <dxe:GridViewDataTextColumn FieldName="Composition_taxable_person" Caption="Composition taxable person" VisibleIndex="2" Width="200">
                                    </dxe:GridViewDataTextColumn> 
                                    <dxe:GridViewDataTextColumn FieldName="Nil_Rated_Supplies" Caption="Nil Rated Supplies" VisibleIndex="3" PropertiesTextEdit-DisplayFormatString="0.00" Width="180">
                                    </dxe:GridViewDataTextColumn>                                                   
                                    <dxe:GridViewDataTextColumn FieldName="Exempted" Caption="Exempted (other than nil rated/non GST supply )" VisibleIndex="4" PropertiesTextEdit-DisplayFormatString="0.00" Width="300">
                                    </dxe:GridViewDataTextColumn> 
                                    <dxe:GridViewDataTextColumn FieldName="Non_GST_supplies" Caption="Non-GST supplies" VisibleIndex="5" PropertiesTextEdit-DisplayFormatString="0.00" Width="180">
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
                                    <dxe:ASPxSummaryItem FieldName="Description" SummaryType="Custom" DisplayFormat="Count" />
                                    <dxe:ASPxSummaryItem FieldName="Composition_taxable_person" SummaryType="Sum" /> 
                                    <dxe:ASPxSummaryItem FieldName="Nil_Rated_Supplies" SummaryType="Sum" />
                                    <dxe:ASPxSummaryItem FieldName="Exempted" SummaryType="Sum" />
                                    <dxe:ASPxSummaryItem FieldName="Non_GST_supplies" SummaryType="Sum" /> 
                                </TotalSummary>
                            </dxe:ASPxGridView>
                        </div>
                        </dxe:ContentControl>
                    </ContentCollection>
                </dxe:TabPage>

                <dxe:TabPage Name="ITCR" Text="GSTR-2 ITCR">
                    <ContentCollection>
                        <dxe:ContentControl runat="server">                            
                            <div class="GridViewArea">
                            <dxe:ASPxGridView runat="server" ID="grid_ITCR" ClientInstanceName="cgridITCR" Width="100%" EnableRowsCache="false" BeginCallback="Callback_BeginCallback"
                                OnSummaryDisplayText="grid_ITCR_SummaryDisplayText" 
                                    OnCustomSummaryCalculate="grid_ITCR_CustomSummaryCalculate"
                                OnCustomCallback="grid_ITCR_CustomCallback" OnDataBinding="grid_ITCR_DataBinding"
                                AutoGenerateColumns="False"  Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Visible">                                                 
                                    <Columns>
                                    <dxe:GridViewDataTextColumn FieldName="Description_for_reversal_of_ITC" Caption="Description for reversal of ITC" VisibleIndex="1" Width="230">
                                    </dxe:GridViewDataTextColumn> 
                                    <dxe:GridViewDataTextColumn FieldName="To_be_added_or_reduced_from_output_liability" Caption="To be added or reduced from output liability" VisibleIndex="2" Width="250">
                                    </dxe:GridViewDataTextColumn> 
                                    <dxe:GridViewDataTextColumn FieldName="ITC_Integrated_Tax_Amount" Caption="ITC Integrated Tax Amount" VisibleIndex="3" PropertiesTextEdit-DisplayFormatString="0.00" Width="180">
                                    </dxe:GridViewDataTextColumn>                                                   
                                    <dxe:GridViewDataTextColumn FieldName="ITC_Central_Tax_Amount" Caption="ITC Central Tax Amount" VisibleIndex="4" PropertiesTextEdit-DisplayFormatString="0.00" Width="180">
                                    </dxe:GridViewDataTextColumn> 
                                    <dxe:GridViewDataTextColumn FieldName="ITC_State_Tax_Amount" Caption="ITC State/UT Tax Amount" VisibleIndex="5" PropertiesTextEdit-DisplayFormatString="0.00" Width="180">
                                    </dxe:GridViewDataTextColumn> 
                                    <dxe:GridViewDataTextColumn FieldName="ITC_Cess_Amount" Caption="ITC Cess Amount" VisibleIndex="6" PropertiesTextEdit-DisplayFormatString="0.00" Width="60">
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
                                    <dxe:ASPxSummaryItem FieldName="Description_for_reversal_of_ITC" SummaryType="Custom" DisplayFormat="Count" />
                                    <dxe:ASPxSummaryItem FieldName="To_be_added_or_reduced_from_output_liability" SummaryType="Custom" DisplayFormat="Count" />
                                    <dxe:ASPxSummaryItem FieldName="ITC_Integrated_Tax_Amount" SummaryType="Sum" /> 
                                    <dxe:ASPxSummaryItem FieldName="ITC_Central_Tax_Amount" SummaryType="Sum" />
                                    <dxe:ASPxSummaryItem FieldName="ITC_State_Tax_Amount" SummaryType="Sum" />
                                    <dxe:ASPxSummaryItem FieldName="ITC_Cess_Amount" SummaryType="Sum" /> 
                                </TotalSummary>
                            </dxe:ASPxGridView>
                        </div>
                        </dxe:ContentControl>
                    </ContentCollection>
                </dxe:TabPage>

                <dxe:TabPage Name="HSNSUM" Text="GSTR-2 HSNSUM">
                    <ContentCollection>
                        <dxe:ContentControl runat="server">                            
                            <div class="GridViewArea">
                            <dxe:ASPxGridView runat="server" ID="grid_HSNSUM" ClientInstanceName="cgridHSNSUM" Width="100%" EnableRowsCache="false" BeginCallback="Callback_BeginCallback"
                                OnSummaryDisplayText="grid_HSNSUM_SummaryDisplayText" 
                                    OnCustomSummaryCalculate="grid_HSNSUM_CustomSummaryCalculate"
                                OnCustomCallback="grid_HSNSUM_CustomCallback" OnDataBinding="grid_HSNSUM_DataBinding"
                                AutoGenerateColumns="False" Settings-HorizontalScrollBarMode="Auto" Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Visible">                                                 
                                    <Columns>
                                    <dxe:GridViewDataTextColumn FieldName="sProducts_HsnCode" Caption="HSN/SAC" VisibleIndex="1" Width="150">
                                    </dxe:GridViewDataTextColumn> 
                                    <dxe:GridViewDataTextColumn FieldName="sProducts_Description" Caption="Description" VisibleIndex="2" Width="250">
                                    </dxe:GridViewDataTextColumn> 
                                    <dxe:GridViewDataTextColumn FieldName="UOM" Caption="UQC" VisibleIndex="3" Width="70">
                                    </dxe:GridViewDataTextColumn> 
                                    <dxe:GridViewDataTextColumn FieldName="Total_Qty" Caption="Total Quantity" VisibleIndex="4" PropertiesTextEdit-DisplayFormatString="0.00" Width="180">
                                    </dxe:GridViewDataTextColumn>                                                   
                                    <dxe:GridViewDataTextColumn FieldName="Total_Value" Caption="Total Value" VisibleIndex="5" PropertiesTextEdit-DisplayFormatString="0.00" Width="180">
                                    </dxe:GridViewDataTextColumn> 
                                    <dxe:GridViewDataTextColumn FieldName="Taxable_Value" Caption="Taxable Value" VisibleIndex="6" PropertiesTextEdit-DisplayFormatString="0.00" Width="180">
                                    </dxe:GridViewDataTextColumn> 
                                    <dxe:GridViewDataTextColumn FieldName="IGSTAmount" Caption="Integrated Tax Amount" VisibleIndex="7" PropertiesTextEdit-DisplayFormatString="0.00" Width="180">
                                    </dxe:GridViewDataTextColumn> 
                                    <dxe:GridViewDataTextColumn FieldName="CGSTAmount" Caption="Central Tax Amount" VisibleIndex="8" PropertiesTextEdit-DisplayFormatString="0.00" Width="180">
                                    </dxe:GridViewDataTextColumn> 
                                    <dxe:GridViewDataTextColumn FieldName="SGSTAmount_UTGSTAmount" Caption="State/UT Tax Amount" VisibleIndex="9" PropertiesTextEdit-DisplayFormatString="0.00" Width="180">
                                    </dxe:GridViewDataTextColumn> 
                                    <dxe:GridViewDataTextColumn FieldName="Cess_Amount" Caption="Cess Amount" VisibleIndex="10" PropertiesTextEdit-DisplayFormatString="0.00" Width="180">
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
                                    <dxe:ASPxSummaryItem FieldName="sProducts_HsnCode" SummaryType="Custom" DisplayFormat="Count" />
                                    <dxe:ASPxSummaryItem FieldName="Total_Qty" SummaryType="Sum" /> 
                                    <dxe:ASPxSummaryItem FieldName="Total_Value" SummaryType="Sum" />
                                    <dxe:ASPxSummaryItem FieldName="Taxable_Value" SummaryType="Sum" />
                                    <dxe:ASPxSummaryItem FieldName="IGSTAmount" SummaryType="Sum" /> 
                                    <dxe:ASPxSummaryItem FieldName="CGSTAmount" SummaryType="Sum" /> 
                                    <dxe:ASPxSummaryItem FieldName="SGSTAmount_UTGSTAmount" SummaryType="Sum" /> 
                                    <dxe:ASPxSummaryItem FieldName="Cess_Amount" SummaryType="Sum" /> 
                                </TotalSummary>
                            </dxe:ASPxGridView>
                        </div>
                        </dxe:ContentControl>
                    </ContentCollection>
                </dxe:TabPage>





                 




            </TabPages>
        </dxe:ASPxPageControl>




    </div>

    <div>
    </div>

    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>




</asp:Content>

