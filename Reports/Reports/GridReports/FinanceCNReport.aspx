<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="FinanceCNReport.aspx.cs" 
    Inherits="Reports.Reports.GridReports.FinanceCNReport" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
     Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/SearchPopup.css" rel="stylesheet" />
    <script src="JS/SearchMultiPopup.js"></script>
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
        /*rev Pallab*/
        .branch-selection-box .dxlpLoadingPanel_PlasticBlue tbody, .branch-selection-box .dxlpLoadingPanelWithContent_PlasticBlue tbody, #divProj .dxlpLoadingPanel_PlasticBlue tbody, #divProj .dxlpLoadingPanelWithContent_PlasticBlue tbody
        {
            bottom: 0 !important;
        }
        .dxlpLoadingPanel_PlasticBlue tbody, .dxlpLoadingPanelWithContent_PlasticBlue tbody
        {
            position: absolute;
            bottom: 80px;
            background: #fff;
            box-shadow: 1px 1px 10px #0000001c;
            right: 50%;
        }
        /*rev end Pallab*/
    </style>

    <%-- For multiselection when click on ok button--%>
    <script type="text/javascript">
        function SetSelectedValues(Id, Name, ArrName) {
            if (ArrName == 'FinancerSource') {
                var key = Id;
                if (key != null && key != '') {
                    $('#FinancerModel').modal('hide');
                    ctxtFinancerName.SetText(Name);
                    GetObjectID('hdnFinancerId').value = key;
                }
                else {
                    ctxtFinancerName.SetText('');
                    GetObjectID('hdnFinancerId').value = '';
                }
            }
        }

    </script>
  <%-- For multiselection when click on ok button--%>

    <%-- For multiselection--%>
     <script type="text/javascript">
         $(document).ready(function () {
             $('#FinancerModel').on('shown.bs.modal', function () {
                 $('#txtFinancerSearch').focus();
             })
         })
         var FinancerArr = new Array();
         $(document).ready(function () {
             var FinancerObj = new Object();
             FinancerObj.Name = "FinancerSource";
             FinancerObj.ArraySource = FinancerArr;
             arrMultiPopup.push(FinancerObj);
         })
         function FinancerButnClick(s, e) {
             $('#FinancerModel').modal('show');
         }

         function Financer_KeyDown(s, e) {
             if (e.htmlEvent.key == "Enter") {
                 $('#FinancerModel').modal('show');
             }
             ctxtFinancerName.Focus();
         }

         function Financerkeydown(e) {
             var OtherDetails = {}

             if ($.trim($("#txtFinancerSearch").val()) == "" || $.trim($("#txtFinancerSearch").val()) == null) {
                 return false;
             }
             OtherDetails.SearchKey = $("#txtFinancerSearch").val();
             // OtherDetails.BranchID = $('#ddl_Branch').val();
             OtherDetails.BranchID = hdnSelectedBranches.value;

             if (e.code == "Enter" || e.code == "NumpadEnter") {

                 var HeaderCaption = [];
                 HeaderCaption.push("Financer Name");

                 if ($("#txtFinancerSearch").val() != "") {
                     callonServerM("Services/Master.asmx/GetOnlyFinancer", OtherDetails, "FinancerTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "FinancerSource");
                 }
             }
             else if (e.code == "ArrowDown") {
                 if ($("input[dPropertyIndex=0]"))
                     $("input[dPropertyIndex=0]").focus();
             }
         }


         function SetfocusOnseach(indexName) {
             if (indexName == "dPropertyIndex")
                 $('#txtFinancerSearch').focus();
             else
                 $('#txtFinancerSearch').focus();
         }
   </script>
      <%-- For multiselection--%>

    <script type="text/javascript">
        //$(function () {
        //    //cProductbranchPanel.PerformCallback('BindComponentGrid' + '~' + $("#ddlbranchHO").val());
        //    //cProductfinancerPanel.PerformCallback('BindComponentGrid' + '~' + $("#ddlbranchHO").val());
        //    function OnWaitingGridKeyPress(e) {
        //        alert('1Hi');
        //        if (e.code == "Enter") {
        //            alert('Hi');
        //        }
        //    }
        //});

        //function BEginClickfinancerBind() {
        //    cProductfinancerPanel.PerformCallback('BindComponentGrid' + '~' + 0);
        //}

        $(document).ready(function () {

            //$("#ddlbranchHO").change(function () {
            //    var Ids = $(this).val();

                //  $('#MandatoryActivityType').attr('style', 'display:none');
                //  $("#hdnSelectedBranches").val('');

                $("#ddlbranchHO").change(function () {
                    var Ids = $(this).val();
                    $('#MandatoryActivityType').attr('style', 'display:none');
                    $("#hdnSelectedBranches").val('');
                    cProductbranchPanel.PerformCallback('BindComponentGrid' + '~' + $("#ddlbranchHO").val());
                    cProductfinancerPanel.PerformCallback('BindComponentGrid' + '~' + $("#ddlbranchHO").val());
                });
        })
                //cProductbranchPanel.PerformCallback('BindComponentGrid' + '~' + $("#ddlbranchHO").val());
                //cProductfinancerPanel.PerformCallback('BindComponentGrid' + '~' + $("#ddlbranchHO").val());
            //});

           <%-- $("#ListBoxBranches").chosen().change(function () {
                var Ids = $(this).val();
                BindLedgerType(Ids);
                //BindCustomerVendor(Ids);

                $('#<%=hdnSelectedBranches.ClientID %>').val(Ids);
                $('#MandatoryActivityType').attr('style', 'display:none');

            })--%>

           <%-- $("#ListBoxCustomerVendor").chosen().change(function () {
                var Ids = $(this).val();

                $('#<%=hdnSelectedCustomerVendor.ClientID %>').val(Ids);
                $('#MandatoryCustomerType').attr('style', 'display:none');

            })--%>


            //var myDate = new Date();
            //// var date = myDate.GetDate();
            /////  alert(myDate);
            //cxdeFromDate.SetDate(myDate);
            //cxdeToDate.SetDate(myDate);
        //})

        function BranchValuewisefinancer() {
            gridbranchLookup.GetGridView().GetSelectedFieldValues("ID", GotSelectedValues);
        }
        function GotSelectedValues(selectedValues) {
            if (selectedValues.length == 0) {
            }
            else {
                values = "";
                for (i = 0; i < selectedValues.length; i++) {
                    if (values == "") {
                        values = selectedValues[i] + "";
                    }
                    else {
                        values = values + ',' + selectedValues[i] + "";
                    }

                }
                $("#hdnSelectedBranches").val(values);
            }

        }

        $(function () {
            cProductbranchPanel.PerformCallback('BindComponentGrid' + '~' + $("#ddlbranchHO").val());
        });


    </script>


    <script type="text/javascript">


        //function cxdeToDate_OnChaged(s, e) {
        //    var data = "OnDateChanged";
        //    data += '~' + cxdeFromDate.GetDate();
        //    data += '~' + cxdeToDate.GetDate();
        //    //CallServer(data, "");
        //    // Grid.PerformCallback('');
        //}


        function btn_ShowRecordsClick(e) {
            //e.preventDefault;
            var data = "OnDateChanged";
            $("#hfIsFinanceRegFilter").val("Y");
            var BranchSelection = document.getElementById('hdnBranchSelection').value;
            //data += '~' + cxdeFromDate.GetDate();
            //data += '~' + cxdeToDate.GetDate();
            //CallServer(data, "");
            // alert( data);

            //if (gridbranchLookup.GetValue() == null) {
            //    jAlert('Please select atleast one branch');
            //}
            //else {

            //Grid.PerformCallback(data);
            //Grid.PerformCallback(data + '~' + $("#ddlbranchHO").val());
            //cCallbackPanel.PerformCallback(data + '~' + $("#ddlbranchHO").val());
            if (BranchSelection == "Yes" && gridbranchLookup.GetValue() == null) {
                jAlert('Please select atleast one branch for generate the report.');
            }
            else {
                cCallbackPanel.PerformCallback(data + '~' + $("#ddlbranchHO").val());
            }

            var FromDate = (cxdeFromDate.GetValue() != null) ? cxdeFromDate.GetValue() : "";
            var ToDate = (cxdeToDate.GetValue() != null) ? cxdeToDate.GetValue() : "";

            FromDate = GetDateFormat(FromDate);
            ToDate = GetDateFormat(ToDate);
            document.getElementById('<%=DateRange.ClientID %>').innerHTML = "For the period: " + FromDate + " To " + ToDate;
            //}
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

        function CallbackPanelEndCall(s, e) {
             <%--Rev Subhra 18-12-2018   0017670--%>
            document.getElementById('<%=CompBranch.ClientID %>').innerHTML = cCallbackPanel.cpBranchNames
            //End of Subhra
            Grid.Refresh();
        }


        function OnContextMenuItemClick(sender, args) {
            if (args.item.name == "ExportToPDF" || args.item.name == "ExportToXLS") {
                args.processOnServer = true;
                args.usePostBack = true;
            } else if (args.item.name == "SumSelected")
                args.processOnServer = true;
        }

        function OpenPOSDetails(Uniqueid, type) {
            //  alert(type);
            var url = '';
            if (type == 'POS') {
                //  window.location.href = '/OMS/Management/Activities/posSalesInvoice.aspx?key=' + invoice + '&Viemode=1';
                //   window.open('/OMS/Management/Activities/posSalesInvoice.aspx?key=' + Uniqueid + '&Viemode=1', '_blank')

                url = '/OMS/Management/Activities/posSalesInvoice.aspx?key=' + Uniqueid + '&IsTagged=1&Viemode=1';
            }



            popupbudget.SetContentUrl(url);
            popupbudget.Show();

        }
        function BudgetAfterHide(s, e) {
            popupbudget.Hide();
        }

        function Callback_EndCallback() {

            // alert('');
            $("#drdExport").val(0);
        }

        function CloseGridQuotationLookup() {
            gridbranchLookup.ConfirmCurrentSelection();
            gridbranchLookup.HideDropDown();
            gridbranchLookup.Focus();
        }

        function CloseGridQuotationLookup2() {
            gridfinancerLookup.ConfirmCurrentSelection();
            gridfinancerLookup.HideDropDown();
            gridfinancerLookup.Focus();
        }


        function selectAll() {
            gridfinancerLookup.gridView.SelectRows();
        }

        function unselectAll() {
            gridfinancerLookup.gridView.UnselectRows();
        }

        function selectAll_branch() {
            gridbranchLookup.gridView.SelectRows();
        }

        function unselectAll_branch() {
            gridbranchLookup.gridView.UnselectRows();
        }

        $(document).keyup(function (e) {
            popupbudget.Hide();
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
        <%--<div class="panel-title">
            <h3>Finance Register</h3>
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
                   <%--Rev Subhra 18-12-2018   0017670--%>
                    <div>
                        <asp:Label ID="CompBranch" runat="Server" Text="" Width="470px" ></asp:Label>
                    </div>
                   <%--End of Rev--%>
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
        <table>
            <tr>
                <td style="">
                    <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                        <asp:Label ID="Label4" runat="Server" Text="Head Branch : " CssClass="mylabel1"
                            Width="92px"></asp:Label>
                    </div>
                </td>

                <td>
                    <asp:DropDownList ID="ddlbranchHO" runat="server" Width="100%"></asp:DropDownList>
                </td>


                <td style="padding-left: 15px">
                    <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                        <asp:Label ID="Label1" runat="Server" Text="Branch : " CssClass="mylabel1"
                            Width="92px"></asp:Label>
                    </div>
                </td>


                <td style="width: 254px">

                    <%--<asp:ListBox ID="ListBoxBranches" Visible="false" runat="server" SelectionMode="Multiple" Font-Size="12px" Height="90px" Width="253px" CssClass="mb0 chsnProduct  hide" data-placeholder="--- ALL ---"></asp:ListBox>--%>
                    <asp:HiddenField ID="hdnActivityType" runat="server" />
                    <asp:HiddenField ID="hdnActivityTypeText" runat="server" />
                    <span id="MandatoryActivityType" style="display: none" class="validclass">
                        <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor1112_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                    <asp:HiddenField ID="hdnSelectedBranches" runat="server" />
                    <dxe:ASPxCallbackPanel runat="server" ID="ASPxCallbackPanel1" ClientInstanceName="cProductbranchPanel" OnCallback="Componentbranch_Callback">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <dxe:ASPxGridLookup ID="lookup_branch" SelectionMode="Multiple" runat="server" ClientInstanceName="gridbranchLookup"
                                    OnDataBinding="lookup_branch_DataBinding"
                                    KeyFieldName="ID" Width="100%" TextFormatString="{1}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                    <Columns>
                                        <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                        <dxe:GridViewDataColumn FieldName="branch_code" Visible="true" VisibleIndex="1" width="200px" Caption="Branch code" Settings-AutoFilterCondition="Contains">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataColumn>

                                        <dxe:GridViewDataColumn FieldName="branch_description" Visible="true" VisibleIndex="2" width="200px" Caption="Branch Name" Settings-AutoFilterCondition="Contains">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataColumn>
                                    </Columns>
                                    <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                        <Templates>
                                            <StatusBar>
                                                <table class="OptionsTable" style="float: right">
                                                    <tr>
                                                        <td>
                                                            <div class="hide">
                                                                <dxe:ASPxButton ID="btn_select" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll_branch" UseSubmitBehavior="False" />
                                                            </div>
                                                            <dxe:ASPxButton ID="btn_unselect" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAll_branch" UseSubmitBehavior="False" />                                                            
                                                            <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridQuotationLookup" UseSubmitBehavior="False" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </StatusBar>
                                        </Templates>
                                        <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" />
                                        <SettingsPager Mode="ShowPager">
                                        </SettingsPager>

                                        <SettingsPager PageSize="20">
                                            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,20,50,100,150,200" />
                                        </SettingsPager>

                                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />

                                    </GridViewProperties>
                                    <%--<ClientSideEvents ValueChanged="BEginClickfinancerBind" />--%>
                                    <ClientSideEvents ValueChanged="BranchValuewisefinancer" />
                                </dxe:ASPxGridLookup>
                            </dxe:PanelContent>
                        </PanelCollection>

                    </dxe:ASPxCallbackPanel>



                </td>
                <td style="padding-left: 15px">
                    <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                        <span>Outstanding</span>
                    </div>
                </td>
                <td style="padding: 0 15px;">
                    <asp:DropDownList ID="ddlfinanceout" runat="server" Width="90px">
                        <asp:ListItem Value="1" Text="Yes"></asp:ListItem>
                        <asp:ListItem Value="0" Text="All"></asp:ListItem>
                    </asp:DropDownList>
                </td>
               <%-- <td style="">
                    <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                        <asp:Label ID="Label3" runat="Server" Text="Financer : " CssClass="mylabel1"
                            Width="92px"></asp:Label>
                    </div>
                </td>--%>
                <%--<td style="width: 254px">--%>
                    <%--<asp:ListBox ID="ListBoxCustomerVendor" Visible="false" runat="server" SelectionMode="Multiple" Font-Size="12px" Height="90px" Width="100%" CssClass="mb0 chsnProduct  hide" data-placeholder="--- ALL ---"></asp:ListBox>--%>
                   <%-- <asp:HiddenField ID="hdnSelectedCustomerVendor" runat="server" />--%>

                    <%--<dxe:ASPxCallbackPanel runat="server" ID="cpFinancer" ClientInstanceName="cProductfinancerPanel" OnCallback="Componentfinancer_Callback">--%>
                       <%-- <PanelCollection>--%>
                            <%--<dxe:PanelContent runat="server">--%>
                                <%--<dxe:ASPxGridLookup ID="gridfinancerLookup" SelectionMode="Multiple" runat="server" ClientInstanceName="gridfinancerLookup"
                                    OnDataBinding="lookup_financer_DataBinding"
                                    KeyFieldName="ID" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">--%>
                                    <%--<Columns>--%>
                                       <%-- <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="50" Caption=" " />--%>


                                       <%-- <dxe:GridViewDataColumn FieldName="Name" Visible="true" VisibleIndex="1" Width="150" Caption="Financer Name" Settings-AutoFilterCondition="Contains">
                                        </dxe:GridViewDataColumn>--%>
                                    <%--</Columns>
                                    <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                        <Templates>
                                            <StatusBar>--%>
                                                <%--<table class="OptionsTable" style="float: right">
                                                    <tr>
                                                        <td>
                                                            <div class="hide">
                                                                <dxe:ASPxButton ID="ASPxButton2_finance" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll" />
                                                            </div>
                                                            <dxe:ASPxButton ID="ASPxButton1_finance" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAll" />                                                            
                                                            <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridQuotationLookup2" UseSubmitBehavior="False" />

                                                        </td>--%>
                                                    <%--</tr>
                                                </table>
                                            </StatusBar>
                                        </Templates>
                                        <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" />
                                        <SettingsPager Mode="ShowPager">
                                        </SettingsPager>--%>

                                        <%--<SettingsPager PageSize="20">
                                            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,20,50,100,150,200" />
                                        </SettingsPager>

                                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                    </GridViewProperties>--%>
                               <%-- </dxe:ASPxGridLookup>
                            </dxe:PanelContent>
                        </PanelCollection>

                    </dxe:ASPxCallbackPanel>--%>
                    <%--<span id="MandatoryCustomerType" style="display: none" class="validclass">--%>
                        <%--<img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor1112_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>--%>
               <%-- </td>--%>
                <td style="">
                   <dxe:ASPxLabel ID="lbl_Financer" style="color: #b5285f;font-weight: bold;" runat="server" Text="Financer :">
                        </dxe:ASPxLabel>
               </td>
                <td style="padding-left: 15px">
                        <dxe:ASPxButtonEdit ID="txtFinancerName" runat="server" ReadOnly="true" ClientInstanceName="ctxtFinancerName" Width="100%" TabIndex="5">
                            <Buttons>
                                <dxe:EditButton>
                                </dxe:EditButton>
                            </Buttons>
                            <ClientSideEvents ButtonClick="function(s,e){FinancerButnClick();}" KeyDown="function(s,e){Financer_KeyDown(s,e);}" />
                     </dxe:ASPxButtonEdit>
                </td>
            </tr>
        </table>
        <table>

            <tr>

                <td>
                    <div style="color: #b5285f; font-weight: bold;" class="clsFrom">
                        <asp:Label ID="lblFromDate" runat="Server" Text="From Date : " CssClass="mylabel1"
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
                    <button id="btnShow" class="btn btn-primary" type="button" onclick="btn_ShowRecordsClick(this);">Show</button>
                    <%-- <% if (rights.CanExport)
                    { %>--%>

                    <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary"
                        OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                        <asp:ListItem Value="0">Export to</asp:ListItem>
                        <asp:ListItem Value="1">PDF</asp:ListItem>
                        <asp:ListItem Value="2">XLSX</asp:ListItem>
                        <asp:ListItem Value="3">RTF</asp:ListItem>
                        <asp:ListItem Value="4">CSV</asp:ListItem>

                    </asp:DropDownList>
                    <%-- <% } %>--%>
                </td>
            </tr>
        </table>
        <div class="pull-right">
        </div>
        <table class="TableMain100">


            <tr>

                <td colspan="2">
                    <div>
                        <%-- <div  id="divRowview">--%>
                        <dxe:ASPxGridView runat="server" ID="ShowGrid" ClientInstanceName="Grid" Width="100%" EnableRowsCache="false" KeyFieldName="SEQ"
                            AutoGenerateColumns="False" KeyboardSupport="true" OnDataBound="Showgrid_Htmlprepared"
                            OnSummaryDisplayText="ShowGrid_SummaryDisplayText" ClientSideEvents-BeginCallback="Callback_EndCallback"
                            SettingsBehavior-AllowFocusedRow="true" SettingsBehavior-AllowSelectSingleRowOnly="true" 
                            DataSourceID="GenerateEntityServerModeDataSource" 
                            Settings-HorizontalScrollBarMode="Visible" SettingsBehavior-AutoExpandAllGroups="true">
                            <%--KeyFieldName="Invoice_Id" OnCustomCallback="Grid_CustomCallback" OnDataBinding="ShowGrid_DataBinding" --%>
                            <Columns>

                                <%--    <dxe:GridViewDataTextColumn FieldName="BRANCH_DESC" Caption="Unit"  VisibleIndex="1" />--%>

                                <dxe:GridViewDataTextColumn FieldName="Branch" Caption="Unit" FixedStyle="Left">
                                    <%--   <PropertiesComboBox
                                        ValueField="AssignTo" TextField="AssignTo" />--%>
                                </dxe:GridViewDataTextColumn>
                                <%--SortOrder="Descending"--%> 


                                <dxe:GridViewDataTextColumn FieldName="Date" Caption="Date" VisibleIndex="1" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy" FixedStyle="Left" />
                                <%--     <dxe:GridViewDataTextColumn FieldName="Bill Nubmer" Caption="Bill Number" VisibleIndex="2" Width="180"  FixedStyle="Left" />--%>



                                <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="Bill_Nubmer" Caption="Bill Number" FixedStyle="Left" Width="150">
                                    <CellStyle HorizontalAlign="Left">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <DataItemTemplate>

                                        <a href="javascript:void(0)" onclick="OpenPOSDetails('<%#Eval("Invoice_Id") %>','<%#Eval("Module_Type") %>')" class="pad">
                                            <%#Eval("Bill_Nubmer")%>
                                        </a>
                                    </DataItemTemplate>

                                    <EditFormSettings Visible="False" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="Challan_No" Caption="Challan No" VisibleIndex="3" Width="150" />
                                <dxe:GridViewDataTextColumn FieldName="SRN" Caption="SRN?" VisibleIndex="4" Width="35" />
                                <dxe:GridViewDataTextColumn FieldName="Return_Number" Caption="Return Number" VisibleIndex="5" Width="150" />
                                <dxe:GridViewDataTextColumn FieldName="Return_Date" Caption="Return Date" VisibleIndex="6" Width="100" />
                                <dxe:GridViewDataTextColumn FieldName="Party_Name" Caption="Customer" VisibleIndex="7" />
                                <dxe:GridViewDataTextColumn FieldName="Product_Descprition" Caption="Product Descprition" VisibleIndex="8" />
                                <dxe:GridViewDataTextColumn FieldName="Bill_Amount" Caption="Bill Amount" VisibleIndex="9" PropertiesTextEdit-DisplayFormatString="0.00" />
                                <dxe:GridViewDataTextColumn FieldName="Arn_No" Caption="Arn.No1" VisibleIndex="10" PropertiesTextEdit-DisplayFormatString="0.00">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="Down_Pay1" Caption="Down Pay1" VisibleIndex="11" PropertiesTextEdit-DisplayFormatString="0.00" />
                                <dxe:GridViewDataTextColumn FieldName="Arn_No2" Caption="Arn.No2" VisibleIndex="12" PropertiesTextEdit-DisplayFormatString="0.00" />
                                <dxe:GridViewDataTextColumn FieldName="Down_Pay2" Caption="Down Pay2" VisibleIndex="13" PropertiesTextEdit-DisplayFormatString="0.00" />
                                <dxe:GridViewDataTextColumn FieldName="Disb_Doc_No1" Caption="Disb. Doc No1" VisibleIndex="14" PropertiesTextEdit-DisplayFormatString="0.00" />
                                <dxe:GridViewDataTextColumn FieldName="Disb_Date1" Caption="Disb. Date1" VisibleIndex="15" PropertiesTextEdit-DisplayFormatString="0.00" />
                                <dxe:GridViewDataTextColumn FieldName="Disb_Amount1" Caption="Disb. Amount1" VisibleIndex="16" PropertiesTextEdit-DisplayFormatString="0.00" />
                                <dxe:GridViewDataTextColumn FieldName="Disb_Doc_No2" Caption="Disb. Doc No2" VisibleIndex="17" PropertiesTextEdit-DisplayFormatString="0.00" />
                                <dxe:GridViewDataTextColumn FieldName="Disb_Date2" Caption="Disb. Date2" VisibleIndex="18" PropertiesTextEdit-DisplayFormatString="0.00" />
                                <dxe:GridViewDataTextColumn FieldName="Disb_Amount2" Caption="Disb. Amount2" VisibleIndex="19" PropertiesTextEdit-DisplayFormatString="0.00" />
                                <dxe:GridViewDataTextColumn FieldName="Disb_Doc_No3" Caption="Disb. Doc No3" VisibleIndex="20" PropertiesTextEdit-DisplayFormatString="0.00" />
                                <dxe:GridViewDataTextColumn FieldName="Disb_Date3" Caption="Disb. Date3" VisibleIndex="21" PropertiesTextEdit-DisplayFormatString="0.00" />
                                <dxe:GridViewDataTextColumn FieldName="Disb_Amount3" Caption="Disb. Amount3" VisibleIndex="22" PropertiesTextEdit-DisplayFormatString="0.00" />
                                <dxe:GridViewDataTextColumn FieldName="Processing_Fee" Caption="Processing Fee" VisibleIndex="23" PropertiesTextEdit-DisplayFormatString="0.00" />
                                <dxe:GridViewDataTextColumn FieldName="DBD_Amt" Caption="DBD Amt." VisibleIndex="24" PropertiesTextEdit-DisplayFormatString="0.00" />
                                <dxe:GridViewDataTextColumn FieldName="DBD_Percentage" Caption="DBD%" VisibleIndex="25" PropertiesTextEdit-DisplayFormatString="0.00" />
                                <dxe:GridViewDataTextColumn FieldName="Emaicharge" Caption="Other Charges" VisibleIndex="26" PropertiesTextEdit-DisplayFormatString="0.00" />
                                <dxe:GridViewDataTextColumn FieldName="Arn_No2" Caption="Arn. No." VisibleIndex="27" PropertiesTextEdit-DisplayFormatString="0.00" />
                                <dxe:GridViewDataTextColumn FieldName="Final_Pay" Caption="Final Pay" VisibleIndex="28" PropertiesTextEdit-DisplayFormatString="0.00" />
                                <dxe:GridViewDataTextColumn FieldName="Chq_No" Caption="Chq No" VisibleIndex="29" PropertiesTextEdit-DisplayFormatString="0.00" />
                                <dxe:GridViewDataTextColumn FieldName="Finance_Amt" Caption="Finance Amt" VisibleIndex="30" PropertiesTextEdit-DisplayFormatString="0.00" />
                                <dxe:GridViewDataTextColumn FieldName="MBD_Percentage" Caption="MBD%" VisibleIndex="31" PropertiesTextEdit-DisplayFormatString="0.00" />
                                <dxe:GridViewDataTextColumn FieldName="MBD_Amt" Caption="MBD Amt." VisibleIndex="32" PropertiesTextEdit-DisplayFormatString="0.00" />
                                <dxe:GridViewDataTextColumn FieldName="Outstandingref" Caption="Outstanding" VisibleIndex="33" PropertiesTextEdit-DisplayFormatString="0.00" />
                                <dxe:GridViewDataTextColumn FieldName="ShortAmt" Caption="Short" VisibleIndex="34" PropertiesTextEdit-DisplayFormatString="0.00" />
                                <dxe:GridViewDataTextColumn FieldName="Excess" Caption="Excess" VisibleIndex="35" PropertiesTextEdit-DisplayFormatString="0.00" />
                                <dxe:GridViewDataTextColumn FieldName="SFCode" Caption="SFCode" VisibleIndex="36" PropertiesTextEdit-DisplayFormatString="0.00" />
                                <dxe:GridViewDataTextColumn FieldName="Pos_FinChallanNo" Caption="Finance Challan No." VisibleIndex="37" />
                                <dxe:GridViewDataTextColumn FieldName="Pos_FinChallanDate" Caption="Challan Date" VisibleIndex="38" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy" />
                                <dxe:GridViewDataTextColumn FieldName="Tot_Payment" Caption="Tot.Payment" VisibleIndex="39" PropertiesTextEdit-DisplayFormatString="0.00" />
                                <dxe:GridViewDataTextColumn FieldName="Otstatus" Caption="Status" VisibleIndex="40" PropertiesTextEdit-DisplayFormatString="0.00" />

                                <%--  <dxe:GridViewDataTextColumn FieldName="SerialNo" Caption="Serials" VisibleIndex="39" PropertiesTextEdit-DisplayFormatString="0.00" />--%>
                            </Columns>
                            <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" />
                            <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
                            <SettingsEditing Mode="EditForm" />
                            <SettingsContextMenu Enabled="true" />
                            <SettingsBehavior AutoExpandAllGroups="true" ColumnResizeMode="Control" />
                            <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" />
                         
                            <SettingsPager PageSize="10">
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100" />
                            </SettingsPager>
                            <Settings ShowFilterRow="True" ShowStatusBar="Visible" UseFixedTableLayout="true" />

                            <TotalSummary>
                                <dxe:ASPxSummaryItem FieldName="Bill_Amount" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="Disb_Amount1" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="Disb_Amount3" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="Finance_Amt" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="Down_Pay1" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="Down_Pay2" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="Disb_Amount2" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="DBD_Amt" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="MBD_Percentage" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="MBD_Amt" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="Processing_Fee" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="Tot_Payment" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="Outstandingref" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="ShortAmt" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="Excess" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="Emaicharge" SummaryType="Sum" />
                            </TotalSummary>

                        </dxe:ASPxGridView>
                        <dx:LinqServerModeDataSource ID="GenerateEntityServerModeDataSource" runat="server" OnSelecting="GenerateEntityServerModeDataSource_Selecting"
                        ContextTypeName="ReportSourceDataContext" TableName="FINANCEREGISTER_REPORT" ></dx:LinqServerModeDataSource>
                    </div>
                </td>
            </tr>
        </table>
    </div>

   <%-- <asp:SqlDataSource ID="SalesDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:crmConnectionString %>"
        SelectCommand="sp_DailySales_Report" SelectCommandType="StoredProcedure"></asp:SqlDataSource>--%>
    <%--    <asp:SqlDataSource ID="EntityDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:crmConnectionString %>"
        SelectCommand="SELECT aty_id ,aty_activityType Type FROM tbl_master_activitytype where (Is_Active=1 or aty_id=9)order by aty_id"></asp:SqlDataSource>--%>

    <%--<asp:SqlDataSource ID="EntityDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:crmConnectionString %>"
        SelectCommand="sp_DailySales_Report" SelectCommandType="StoredProcedure"></asp:SqlDataSource>--%>
    <%--<asp:SqlDataSource ID="EntityDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:crmConnectionString %>"
        SelectCommand="sp_DailySales_Report" SelectCommandType="StoredProcedure"></asp:SqlDataSource>--%>

    <!--Financer Modal -->
    <div class="modal fade" id="FinancerModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Financer Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Financerkeydown(event)" id="txtFinancerSearch" width="100%" placeholder="Search By Financer Name" />
                    <div id="FinancerTable">
                        <table border='1' width="100%">

                            <tr class="HeaderStyle" style="font-size:small">
                                <th>Select</th>
                                 <th class="hide">id</th>
                                <th>Financer Name</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btnOkformultiselection btn-default"  onclick="DeSelectAll('FinancerSource')">Deselect All</button>
                    <button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal" onclick="OKPopup('FinancerSource')">OK</button>
                </div>
            </div>
        </div>
    </div>
    <!--Financer Modal -->
    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>
    <asp:HiddenField ID="hdnFinancerId" runat="server" />
     <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
            <asp:HiddenField ID="hfIsFinanceRegFilter" runat="server" />
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="CallbackPanelEndCall" />
    </dxe:ASPxCallbackPanel>


    <dxe:ASPxPopupControl ID="ASPXPopupControl2" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popupbudget" Height="500px"
        Width="1310px" HeaderText="Details" Modal="true" AllowResize="true" ResizingMode="Postponed">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>

        <ClientSideEvents CloseUp="BudgetAfterHide" />
    </dxe:ASPxPopupControl>

    <asp:HiddenField ID="hdnBranchSelection" runat="server" />
</asp:Content>

