<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" CodeBehind="StatementOfAccount.aspx.cs" Inherits="Reports.Reports.GridReports.StatementOfAccount" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
     Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <link href="CSS/SearchPopup.css" rel="stylesheet" />
        <script src="JS/SearchPopup.js"></script>
        <script src="JS/SearchMultiPopup.js"></script>

    <script src="/assests/pluggins/choosen/choosen.min.js"></script>
    <style>
        .colDisable {
        cursor:default !important;
        }
        #pageControl, .dxtc-content {
            overflow: visible !important;
        }

        #MandatoryAssign {
            position: absolute;
            right: -17px;
            top: 6px;
        }
       .btnOkformultiselection {
        border-width: 1px;
        padding: 4px 10px;
        font-size: 13px !important;
        margin-right: 6px;
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

        /*#ListBoxProjects{
            width: 200px;
        }*/

        .hide {
            display: none;
        }

        .dxtc-activeTab .dxtc-link {
            color: #fff !important;
        }
    </style>
   
     <%-- For multiselection when click on ok button--%>
    <script type="text/javascript">
        function SetSelectedValues(Id, Name, ArrName) {
            if (ArrName == 'PartyLedgerSource') {
                var key = Id;
                if (key != null && key != '') {
                    $('#PartyLedgModel').modal('hide');
                    ctxtPartyLedger.SetText(Name);
                    GetObjectID('hdnSelectedPartyLedger').value = key;
                }
                else {
                    ctxtPartyLedger.SetText('');
                    GetObjectID('hdnSelectedPartyLedger').value = '';
                }
            }

        }

    </script>
  <%-- For multiselection when click on ok button--%>
     <%-- For multiselection--%>
     <script type="text/javascript">
         $(document).ready(function () {
             $('#PartyLedgModel').on('shown.bs.modal', function () {
                 $('#txtPartyLedgerSearch').focus();
             });

             $("#ddlCriteria").change(function () {
                 var Values = $("#ddlCriteria").val();
                 $("#ckparty").attr('style', 'display:inline-block; padding-left: 15px; padding-top: 3px;color: #b5285f; font-weight: bold;" class="clsTo;');

                 if (Values == 'ALL') {
                     CchkAllParty.SetText('All Parties');
                 }
                 else if (Values == 'EM') {
                     CchkAllParty.SetText('All Employees');
                 }
                 else if (Values == 'CL') {
                     CchkAllParty.SetText('All Customers');
                 }
                 else if (Values == 'DV') {
                     CchkAllParty.SetText('All Vendors');
                 }
                 else if (Values == 'TR') {
                     CchkAllParty.SetText('All Transporter');
                 }
                 else if (Values == 'RA') {
                     CchkAllParty.SetText('All Influencers');
                 }
                 else if (Values == 'SL') {
                     CchkAllParty.SetText('All Sub Ledgers');
                 }
                 $("#ckparty").attr('style', 'padding-left: 1px; padding-top: 15px;color: #b5285f; font-weight: bold;" class="clsTo;');
             });
         })
         var PartyLedgArr = new Array();
         $(document).ready(function () {
             var PartyLedgObj = new Object();
             PartyLedgObj.Name = "PartyLedgerSource";
             PartyLedgObj.ArraySource = PartyLedgArr;
             arrMultiPopup.push(PartyLedgObj);
         })
         function PartyLedgerButnClick(s, e) {
             $('#PartyLedgModel').modal('show');
         }

         function PartyLedger_KeyDown(s, e) {
             if (e.htmlEvent.key == "Enter") {
                 $('#PartyLedgModel').modal('show');
             }
         }

         function PartyLedgerkeydown(e) {
             var OtherDetails = {}

             if ($.trim($("#txtPartyLedgerSearch").val()) == "" || $.trim($("#txtPartyLedgerSearch").val()) == null) {
                 return false;
             }
             OtherDetails.SearchKey = $("#txtPartyLedgerSearch").val();

             if (e.code == "Enter" || e.code == "NumpadEnter") {
                 var HeaderCaption = [];
                 HeaderCaption.push("Description");
                 HeaderCaption.push("Type");
                 HeaderCaption.push("Address");

                 if ($("#txtPartyLedgerSearch").val() != "") {
                     //callonServerM("Services/Master.asmx/GetPartyLedger", OtherDetails, "PartyLedgerTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "PartyLedgerSource");
                     if (ddlCriteria.value == "ALL") {
                         OtherDetails.Type = "ALL"
                         callonServerM("Services/Master.asmx/GetPartyLedgerAll", OtherDetails, "PartyLedgerTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "PartyLedgerSource");
                     }
                     else if (ddlCriteria.value == "EM") {
                         OtherDetails.Type = "EM"
                         callonServerM("Services/Master.asmx/GetPartyLedgerAll", OtherDetails, "PartyLedgerTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "PartyLedgerSource");
                     }
                     else if (ddlCriteria.value == "CL") {
                         OtherDetails.Type = "CL"
                         callonServerM("Services/Master.asmx/GetPartyLedgerAll", OtherDetails, "PartyLedgerTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "PartyLedgerSource");
                     }
                     else if (ddlCriteria.value == "DV") {
                         OtherDetails.Type = "DV"
                         callonServerM("Services/Master.asmx/GetPartyLedgerAll", OtherDetails, "PartyLedgerTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "PartyLedgerSource");
                     }
                     else if (ddlCriteria.value == "TR") {
                         OtherDetails.Type = "TR"
                         callonServerM("Services/Master.asmx/GetPartyLedgerAll", OtherDetails, "PartyLedgerTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "PartyLedgerSource");
                     }
                     else if (ddlCriteria.value == "RA") {
                         OtherDetails.Type = "RA"
                         callonServerM("Services/Master.asmx/GetPartyLedgerAll", OtherDetails, "PartyLedgerTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "PartyLedgerSource");
                     }
                     else if (ddlCriteria.value == "SL") {
                         OtherDetails.Type = "SL"
                         callonServerM("Services/Master.asmx/GetPartyLedgerAll", OtherDetails, "PartyLedgerTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "PartyLedgerSource");
                     }
                 }
             }
             else if (e.code == "ArrowDown") {
                 if ($("input[dPropertyIndex=0]"))
                     $("input[dPropertyIndex=0]").focus();
             }
         }

         function SetfocusOnseach(indexName) {
             if (indexName == "dPropertyIndex")
                 $('#txtPartyLedgerSearch').focus();
             else
                 $('#txtPartyLedgerSearch').focus();
         }
   </script>
      <%-- For multiselection--%>

    <script type="text/javascript">

        $(function () {
            cProductbranchPanel.PerformCallback('BindComponentGrid' + '~' + $("#ddlbranchHO").val());
            function OnWaitingGridKeyPress(e) {
                if (e.code == "Enter") {
                }
            }
        });

        $(document).ready(function () {
            $("#ListBoxBranches").chosen().change(function () {
                var Ids = $(this).val();
                $('#<%=hdnSelectedBranches.ClientID %>').val(Ids);
                $('#MandatoryActivityType').attr('style', 'display:none');
            })

            //var ProjectSelection = document.getElementById('hdnProjectSelection').value;
            //if (ProjectSelection == "0") {
            //    $('#divProj').addClass('hidden');
            //}
            //else {
            //    $('#divProj').removeClass('hidden');
            //}

            $("#ddlbranchHO").change(function () {
                var Ids = $(this).val();
                $('#MandatoryActivityType').attr('style', 'display:none');
                $("#hdnSelectedBranches").val('');
                cProductbranchPanel.PerformCallback('BindComponentGrid' + '~' + $("#ddlbranchHO").val());
            })

        })

        function OnGetRowValuesCallback(values) {
            alert(values);
        }

        //$(function () {
        //    cProjectPanel.PerformCallback('BindProjectGrid');
        //});

        $(document).ready(function () {

            $("#ListBoxBranches").chosen().change(function () {
                var Ids = $(this).val();
                cProductComponentPanel_ledger.PerformCallback('BindComponentGrid' + '~' + Ids);
                $('#<%=hdnSelectedBranches.ClientID %>').val(Ids);
                $('#MandatoryActivityType').attr('style', 'display:none');
            })
        })


    </script>

    <script type="text/javascript">

        function cxdeToDate_OnChaged(s, e) {
            var data = "OnDateChanged";
            data += '~' + cxdeFromDate.GetDate();
            data += '~' + cxdeToDate.GetDate();
        }

        function btn_ShowRecordsClick(e) {
            var BranchSelection = document.getElementById('hdnBranchSelection').value;
            //var ProjectSelection = document.getElementById('hdnProjectSelection').value;
            //var ProjectSelectionInReport = document.getElementById('hdnProjectSelectionInReport').value;
            var data = "OnDateChanged";

            data += '~' + cxdeFromDate.GetDate();
            data += '~' + cxdeToDate.GetDate();

            var FromDate = (cxdeFromDate.GetValue() != null) ? cxdeFromDate.GetValue() : "";
            var ToDate = (cxdeToDate.GetValue() != null) ? cxdeToDate.GetValue() : "";
            var partyledgerid = hdnSelectedPartyLedger.value;

            FromDate = GetDateFormat(FromDate);
            ToDate = GetDateFormat(ToDate);
            document.getElementById('<%=DateRange.ClientID %>').innerHTML = "For the period: " + FromDate + " To " + ToDate;

            //if (ctxtPartyLedger.GetValue() == null) {
            if (ctxtPartyLedger.GetValue() == null && CchkAllParty.GetChecked() == false) {
                jAlert('Please select atleast one Party for generate the report.');
            }
            //else if (ProjectSelection == "1") {
            //    if (ProjectSelectionInReport == "Yes" && gridprojectLookup.GetValue() == null) {
            //        jAlert('Please select atleast one Project for generate the report.');
            //    }
            //    else if (BranchSelection == "Yes" && gridbranchLookup.GetValue() == null) {
            //        jAlert('Please select atleast one branch for generate the report.');
            //    }
            //    else {
            //        cCallbackPanel.PerformCallback(data + '~' + $("#ddlbranchHO").val() + '~' + partyledgerid);
            //    }
            //}
            else {
                if (BranchSelection == "Yes" && gridbranchLookup.GetValue() == null) {
                    jAlert('Please select atleast one branch for generate the report.');
                }
                else {
                    cCallbackPanel.PerformCallback(data + '~' + $("#ddlbranchHO").val() + '~' + partyledgerid);
                }
            }

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
            document.getElementById('<%=CompBranch.ClientID %>').innerHTML = cCallbackPanel.cpBranchNames
            if (cCallbackPanel.cpPreviewUrl != "") {
                var parameter;
                parameter = cCallbackPanel.cpPreviewUrl;
                var reportName = parameter.split("\\")[0];
                var FROMDATE = parameter.split("\\")[1];
                var TODATE = parameter.split("\\")[2];
                var BRANCH_ID = parameter.split("\\")[3];
                var PARTYLEDGERIDS = parameter.split("\\")[4];
                var Criteria = parameter.split("\\")[5];
                var ShowAllParty = parameter.split("\\")[6];
                var PROJECT_ID = parameter.split("\\")[7];
                var ShowHeader = parameter.split("\\")[8];
                var ShowFooter = parameter.split("\\")[9];
                var RptModuleName = parameter.split("\\")[10];
                window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + "&&StartDate=" + FROMDATE + "&&EndDate=" + TODATE + "&&BranId=" + BRANCH_ID + "&&PartyIds=" + PARTYLEDGERIDS + "&&Criteria=" + Criteria + "&&ShowAllParty=" + ShowAllParty + "&&ProjList=" + PROJECT_ID + "&&ShowHeader=" + ShowHeader + "&&ShowFooter=" + ShowFooter + "&&reportname=" + RptModuleName, '_blank')
            }
        }

        function CloseGridemployeeeLookup() {
            gridemployeeLookup.ConfirmCurrentSelection();
            gridemployeeLookup.HideDropDown();
            gridemployeeLookup.Focus();
        }

        function CloseGridBranchLookupbranch() {
            gridbranchLookup.ConfirmCurrentSelection();
            gridbranchLookup.HideDropDown();
            gridbranchLookup.Focus();
        }

        function selectAll_Branch() {
            gridbranchLookup.gridView.SelectRows();
        }

        function unselectAll_Branch() {
            gridbranchLookup.gridView.UnselectRows();
        }

        function selectAll() {
            gridledgerLookup.gridView.SelectRows();
        }

        function unselectAll() {
            gridledgerLookup.gridView.UnselectRows();
        }

        //function selectAll_Project() {
        //    gridprojectLookup.gridView.SelectRows();
        //}
        //function unselectAll_Project() {
        //    gridprojectLookup.gridView.UnselectRows();
        //}
        //function CloseGridProjectLookup() {
        //    gridprojectLookup.ConfirmCurrentSelection();
        //    gridprojectLookup.HideDropDown();
        //    gridprojectLookup.Focus();
        //}

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
        function CheckConsAllParty(s, e) {
            if (s.GetCheckState() == 'Checked') {
                ctxtPartyLedger.SetEnabled(false);
                ctxtPartyLedger.SetText('');
                GetObjectID('hdnSelectedPartyLedger').value = '';
               <%-- $(<%=ddlCriteria.ClientID%>).prop("disabled", true)
                $("#ddlCriteria").val("ALL");      --%>
            }
            else {
                ctxtPartyLedger.SetEnabled(true);
                <%-- $(<%=ddlCriteria.ClientID%>).prop("disabled", false)
                $("#ddlCriteria").val("ALL");--%>
            }
        }
    </script>
    <style>
        .pBackDiv {
            border: 1px solid #ddd;
            padding: 15px;
            border-radius: 4px;
            background: #f9f9f9;
            width: 50%;
            margin: 0 auto;
            border-top: 5px solid #373980;
            margin-top:50px
        }
        #ddlbranchHO {
            margin-right:0
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    
    <div class="clearfix">
        <asp:HiddenField runat="server" ID="hdndaily" />
        <asp:HiddenField runat="server" ID="hdtid" />
        <div class="pBackDiv">
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
            <%--<div id="collapseOne" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne">--%>
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
        <div class="row">
            
            <div class="col-md-4">
                    <div style="color: #b5285f" class="clsTo">
                        <asp:Label ID="Label3" runat="Server" Text="Head Branch : " CssClass="mylabel1"
                            Width="92px"></asp:Label>
                    </div>
                <div>
                    <asp:DropDownList ID="ddlbranchHO" runat="server" Width="100%"></asp:DropDownList>
                </div>
            </div>
            <div class="col-md-4">
                <div style="color: #b5285f" class="clsTo">
                        <asp:Label ID="Label1" runat="Server" Text="Branch : " CssClass="mylabel1"
                            Width="92px"></asp:Label>
                    </div>
                <div>

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
                                                            <%--<div class="hide">--%>
                                                                <dxe:ASPxButton ID="ASPxButton3" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll_Branch" UseSubmitBehavior="False"/>
                                                            <%--</div>--%>
                                                            <dxe:ASPxButton ID="ASPxButton4" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAll_Branch" UseSubmitBehavior="False"/>                                                            
                                                            <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridBranchLookupbranch" UseSubmitBehavior="False" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </StatusBar>
                                        </Templates>
                                        <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" />
                                        <SettingsPager Mode="ShowPager">
                                        </SettingsPager>

                                        <SettingsPager PageSize="20">
                                            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,20,50,100" />
                                        </SettingsPager>

                                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                    </GridViewProperties>
                                </dxe:ASPxGridLookup>
                            </dxe:PanelContent>
                        </PanelCollection>

                    </dxe:ASPxCallbackPanel>

                </div>
            </div>

            <div class="col-md-4">
                <div style="color: #b5285f" class="clsTo">
                    <asp:Label ID="Label4" runat="Server" Text="Party : " CssClass="mylabel1"
                        Width="92px"></asp:Label>
                </div>
                <div>
                      <dxe:ASPxButtonEdit ID="txtPartyLedger" runat="server" ReadOnly="true" ClientInstanceName="ctxtPartyLedger" Width="100%" TabIndex="5">
                            <Buttons>
                                <dxe:EditButton>
                                </dxe:EditButton>
                            </Buttons>
                            <ClientSideEvents ButtonClick="function(s,e){PartyLedgerButnClick();}" KeyDown="function(s,e){PartyLedger_KeyDown(s,e);}" />
                      </dxe:ASPxButtonEdit>
                    <asp:HiddenField ID="hdnSelectedPartyLedger" runat="server" />

                </div>
            </div>
            <div class="clear"></div>
            <div class="col-md-4" style="padding-top: 1px;color: #b5285f;">
                <div style="color: #b5285f" class="clsTo">
                    <asp:Label ID="Label2" runat="Server" Text="Criteria : " CssClass="mylabel1"></asp:Label>
                </div>
                <asp:DropDownList ID="ddlCriteria" runat="server" Width="100%">
                    <%--<asp:ListItem Text="All" Value="ALL"></asp:ListItem>--%>
                    <asp:ListItem Text="Employee" Value="EM" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="Customer" Value="CL"></asp:ListItem>
                    <asp:ListItem Text="Vendor" Value="DV"></asp:ListItem>
                    <asp:ListItem Text="Transporter" Value="TR"></asp:ListItem>
                    <asp:ListItem Text="Influencer" Value="RA"></asp:ListItem>
                    <asp:ListItem Text="Sub Ledger" Value="SL"></asp:ListItem>
                </asp:DropDownList>
            </div>

            

            <%--<div class="clear"></div>--%>
            <%--<div class="col-md-2" style="padding-top: 1px; width:235px" id="divProj">
                <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="lblProj" runat="Server" Text="Project : " CssClass="mylabel1"></asp:Label>
                </div>
                <asp:ListBox ID="ListBoxProjects" Visible="false" runat="server" SelectionMode="Multiple" Font-Size="12px" Height="60px" Width="100%" CssClass="mb0 chsnProduct  hide" data-placeholder="--- ALL ---"></asp:ListBox>
                <dxe:ASPxCallbackPanel runat="server" ID="ProjectPanel" ClientInstanceName="cProjectPanel" OnCallback="Project_Callback">
                    <PanelCollection>
                        <dxe:PanelContent runat="server">
                            <dxe:ASPxGridLookup ID="lookup_project" SelectionMode="Multiple" runat="server" ClientInstanceName="gridprojectLookup"
                                OnDataBinding="lookup_project_DataBinding"
                                KeyFieldName="ID" Width="100%" TextFormatString="{1}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                <Columns>
                                    <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                    <dxe:GridViewDataColumn FieldName="project_code" Visible="true" VisibleIndex="1" width="200px" Caption="Project code" Settings-AutoFilterCondition="Contains">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataColumn>

                                    <dxe:GridViewDataColumn FieldName="project_name" Visible="true" VisibleIndex="2" width="200px" Caption="Project Name" Settings-AutoFilterCondition="Contains">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataColumn>
                                </Columns>
                                <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                    <Templates>
                                        <StatusBar>
                                            <table class="OptionsTable" style="float: right">
                                                <tr>
                                                    <td>
                                                        <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll_Project" UseSubmitBehavior="False"/>
                                                        <dxe:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAll_Project" UseSubmitBehavior="False"/>                                                        
                                                        <dxe:ASPxButton ID="ASPxButton5" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridProjectLookup" UseSubmitBehavior="False" />
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
                            </dxe:ASPxGridLookup>
                        </dxe:PanelContent>
                    </PanelCollection>
                </dxe:ASPxCallbackPanel>

                <span id="MandatoryActivityType" style="display: none" class="validclass">
                    <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor1112_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                <asp:HiddenField ID="hdnSelectedProjects" runat="server" />
            </div>--%>

            <div class="col-md-4" style="padding-top: 1px;color: #b5285f;">
                    <div style="color: #b5285f" class="clsFrom">
                        <asp:Label ID="lblFromDate" runat="Server" Text="From Date : " CssClass="mylabel1"
                            Width="92px"></asp:Label>
                    </div>
                
                <div>
                    <dxe:ASPxDateEdit ID="ASPxFromDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                        UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeFromDate">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                </div>
            </div>
            
            <div class="col-md-4" style="padding-top: 1px;color: #b5285f;max-width: 200px;">
                    <div style="color: #b5285f" class="clsTo">
                        <asp:Label ID="lblToDate" runat="Server" Text="To Date : " CssClass="mylabel1"
                            Width="92px"></asp:Label>
                    </div>              
                    <div>
                        <dxe:ASPxDateEdit ID="ASPxToDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                            UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeToDate">
                            <ButtonStyle Width="13px">
                            </ButtonStyle>

                        </dxe:ASPxDateEdit>
                    </div>
            </div>
            <div class="clear"></div>
            <div class="col-md-3" style="padding-top: 1px;color: #b5285f;padding-right: 0;">
                <div id="ckparty" style="padding-right: 10px; vertical-align: middle; padding-top: 15px">
                    <dxe:ASPxCheckBox ID="chkAllParty" runat="server" Checked="false" Text="All Employees" ClientInstanceName="CchkAllParty">
                        <ClientSideEvents CheckedChanged="CheckConsAllParty" />
                    </dxe:ASPxCheckBox>
                </div>
            </div>
            <div class="col-md-3" style="padding-top: 1px;color: #b5285f;padding-right: 0;display: none">
                <div id="ckparty" style="padding-right: 0px; vertical-align: middle; padding-top: 15px">
                    <dxe:ASPxCheckBox ID="chkShowHeader" runat="server" Checked="false" Text="Show Header">
                    </dxe:ASPxCheckBox>
                </div>
            </div>

            <div class="col-md-3" style="padding-top: 1px;color: #b5285f;padding-right: 0;display: none">
                <div id="ckparty" style="padding-right: 0px; vertical-align: middle; padding-top: 15px">
                    <dxe:ASPxCheckBox ID="chkShowFooter" runat="server" Checked="false" Text="Show Footer">
                    </dxe:ASPxCheckBox>
                </div>
            </div>
            <div class="clear"></div>
            <div class="col-md-2" style="padding-right: 0px; padding-top: 12px;">
                <div>
                    <button id="btnPreview" class="btn btn-success" type="button" onclick="btn_ShowRecordsClick(this);">Preview</button>
                </div>
            </div>
        </div>
            </div>
    </div>
    
    <!--Party Modal -->
    <div class="modal fade" id="PartyLedgModel" role="dialog">
        <div class="modal-dialog">
            <!-- Party content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Party Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="PartyLedgerkeydown(event)" id="txtPartyLedgerSearch" autofocus width="100%" placeholder="Search By Party Name" />
                    <div id="PartyLedgerTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th>Select</th>
                                <th class="hide">cnt_internalId</th>
                                <th>Description</th>
                                <th>Type</th>
                                <th>Address</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <%--<button type="button" class="btn btn-default" data-dismiss="modal">Close</button>--%>
                     <button type="button" class="btnOkformultiselection btn-default"  onclick="DeSelectAll('PartyLedgerSource')">Deselect All</button>
                    <button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal" onclick="OKPopup('PartyLedgerSource')">OK</button>
                </div>
            </div>
        </div>
    </div>
   <!--Party Modal -->

    <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
    <ClientSideEvents EndCallback="CallbackPanelEndCall" />
    </dxe:ASPxCallbackPanel>

    <asp:HiddenField ID="hdnBranchSelection" runat="server" />
<%--    <asp:HiddenField ID="hdnProjectSelection" runat="server" />
    <asp:HiddenField ID="hdnProjectSelectionInReport" runat="server" />--%>
</asp:Content>
