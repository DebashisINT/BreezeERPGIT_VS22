<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="ProjectSummary.aspx.cs" Inherits="Reports.Reports.GridReports.ProjectSummary" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/SearchPopup.css" rel="stylesheet" />
    <script src="JS/SearchMultiPopup.js"></script>
    <script src="/assests/pluggins/choosen/choosen.min.js"></script>
    <link href="https://fonts.googleapis.com/css2?family=Poppins:wght@200;300;500;600;700;800;900&display=swap" rel="stylesheet">
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

        .hide {
            display: none;
        }

        .dxtc-activeTab .dxtc-link {
            color: #fff !important;
        }
    </style>

    <script type="text/javascript">

        $(function () {
            cBranchComponentPanel.PerformCallback('BindComponentGrid' + '~' + $("#ddlbranchHO").val());
        });

        $(function () {
            cProjectPanel.PerformCallback('BindProjectGrid');
        });

        $(document).ready(function () {
            $("#ListBoxBranches").chosen().change(function () {
                var Ids = $(this).val();
                $('#<%=hdnSelectedBranches.ClientID %>').val(Ids);
                $('#MandatoryActivityType').attr('style', 'display:none');
            })

            $("#ddlbranchHO").change(function () {
                var Ids = $(this).val();
                $('#MandatoryActivityType').attr('style', 'display:none');
                $("#hdnSelectedBranches").val('');
                cBranchComponentPanel.PerformCallback('BindComponentGrid' + '~' + $("#ddlbranchHO").val());
            })

        })
    </script>
 <%-- For multiselection when click on ok button--%>
    <script type="text/javascript">
        function SetSelectedValues(Id, Name, ArrName) {
            if (ArrName == 'CustomerSource') {
                var key = Id;
                if (key != null && key != '') {
                    $('#CustModel').modal('hide');
                    ctxtCustName.SetText(Name);
                    GetObjectID('hdnCustomerId').value = key;
                    cProjectPanel.PerformCallback('BindProjectGridwithCustomer' + '~' + key);
                }
                else {
                    ctxtCustName.SetText('');
                    GetObjectID('hdnCustomerId').value = '';
                }
            }

        }

    </script>
  <%-- For multiselection when click on ok button--%>
   <%-- For multiselection--%>
     <script type="text/javascript">
         $(document).ready(function () {
             $('#CustModel').on('shown.bs.modal', function () {
                 $('#txtCustSearch').focus();
             })
         })
         var CustArr = new Array();
         $(document).ready(function () {
             var CustObj = new Object();
             CustObj.Name = "CustomerSource";
             CustObj.ArraySource = CustArr;
             arrMultiPopup.push(CustObj);
         })
         function CustomerButnClick(s, e) {
             $('#CustModel').modal('show');
         }

         function Customer_KeyDown(s, e) {
             if (e.htmlEvent.key == "Enter") {
                 $('#CustModel').modal('show');
             }
         }

         function Customerkeydown(e) {
             var OtherDetails = {}

             if ($.trim($("#txtCustSearch").val()) == "" || $.trim($("#txtCustSearch").val()) == null) {
                 return false;
             }
             OtherDetails.SearchKey = $("#txtCustSearch").val();

             if (e.code == "Enter" || e.code == "NumpadEnter") {

                 var HeaderCaption = [];
                 HeaderCaption.push("Customer Name");
                 HeaderCaption.push("Unique ID");
                 HeaderCaption.push("Alternate No.");
                 HeaderCaption.push("Address");

                 if ($("#txtCustSearch").val() != "") {
                     callonServerM("Services/Master.asmx/GetCustomer", OtherDetails, "CustomerTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "CustomerSource");
                 }
             }
             else if (e.code == "ArrowDown") {
                 if ($("input[dPropertyIndex=0]"))
                     $("input[dPropertyIndex=0]").focus();
             }
         }


         function SetfocusOnseach(indexName) {
             if (indexName == "dPropertyIndex")
                 $('#txtCustSearch').focus();
             else
                 $('#txtCustSearch').focus();
         }
   </script>
      <%-- For multiselection--%>

    <script>

        function Callback_EndCallback() {
            //$("#drdExport").val(0);
            //$("#ddlbranchHO").val(1);
            //$("#cmbDetailITC").val(0);
        }


    </script>

    <script type="text/javascript">
        function btn_ShowRecordsClick(e) {
            var BranchSelection = document.getElementById('hdnBranchSelection').value;
            e.preventDefault;
            var data = "OnDateChanged";

            if (gridprojectLookup.GetValue() == null) {
                jAlert('Please select atleast one Project for generate the report.');
            }
            else {
                if (BranchSelection == "Yes" && gridbranchLookup.GetValue() == null) {
                    jAlert('Please select atleast one branch for generate the report.');
                }
                else {
                    cProjSummGridPanel.PerformCallback(data + '~' + $("#ddlbranchHO").val());
                }
            }

            var ToDate = (cxdeAsOnDate.GetValue() != null) ? cxdeAsOnDate.GetValue() : "";

            ToDate = GetDateFormat(ToDate);
            document.getElementById('<%=DateRange.ClientID %>').innerHTML = "As On: " + ToDate;
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

        function selectAll() {
            gridbranchLookup.gridView.SelectRows();
        }
        function unselectAll() {
            gridbranchLookup.gridView.UnselectRows();
        }

        function CloseGridBranchLookup() {
            gridbranchLookup.ConfirmCurrentSelection();
            gridbranchLookup.HideDropDown();
            gridbranchLookup.Focus();
        }

        function selectAll_Project() {
            gridprojectLookup.gridView.SelectRows();
        }

        function unselectAll_Project() {
            gridprojectLookup.gridView.UnselectRows();
        }
        function CloseGridProjectLookup() {
            gridprojectLookup.ConfirmCurrentSelection();
            gridprojectLookup.HideDropDown();
            gridprojectLookup.Focus();
        }

    </script>
    <script>        

        //function CallbackCustOut_BeginCallback() {
        //    $("#drdExport").val(0);
        //}

        function EndCallback() {
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
        .paddingTbl>tbody>tr>td {
            padding-right:20px;
        }
        .marginTop10 {
            margin-top:10px;
        }
        .colorGrid1 .dxgvHeader_PlasticBlue {
            border-top: 1px none #c06517;
            border: 1px solid #ad5910;
            background: #d0701e !important;
        }
        .c1{
            color: #d0701e;
        }
        .colorGrid2 .dxgvHeader_PlasticBlue {
            border-top: 1px none #283dd0;
            border: 1px solid #4457dc;
            background: #283dd0 !important;
        }
        .c2{
            color: #283dd0;
        }
        .colorGrid3 .dxgvHeader_PlasticBlue {
            border-top: 1px none #289ad0;
            border: 1px solid #156e99;
            background: #289ad0 !important;
        }
        .c3{
            color: #289ad0;
        }
        .colorGrid4 .dxgvHeader_PlasticBlue{
            border-top: 1px none #583b86;
            border: 1px solid #44217c;
            background: #583b86 !important;
        }
        .c4{
            color: #583b86;
        }
        #CostGrid_DXMainTable>tbody>tr.dxgvDataRow_PlasticBlue:nth-child(odd) td,
        #RevGrid_DXMainTable>tbody>tr.dxgvDataRow_PlasticBlue:nth-child(odd) td,
        #IRSummGrid_DXMainTable>tbody>tr.dxgvDataRow_PlasticBlue:nth-child(odd) td,
        #CustSummGrid_DXMainTable>tbody>tr.dxgvDataRow_PlasticBlue:nth-child(odd) td,
        #VendSummGrid_DXMainTable>tbody>tr.dxgvDataRow_PlasticBlue:nth-child(odd) td,
        #OTHGrid_DXMainTable>tbody>tr.dxgvDataRow_PlasticBlue:nth-child(odd) td {
            background-color:#e8f5f2;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                cCostGrid.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                cCostGrid.SetWidth(cntWidth);
            }

            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    cCostGrid.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    cCostGrid.SetWidth(cntWidth);
                }

            });
        });
    </script>
    <%--<style>
        /*Tab style*/
        .fontPP {
             font-family: 'Poppins', sans-serif !important;
        }
        .tabStyled .nav-tabs>li>a {
            font-size:14px;
            text-transform:uppercase;
        }
        .tabStyled .nav-tabs>li>a {
             background-color: #EEEEEE !important;
             border-bottom: 1px solid #ccc;
            padding: 5px 18px;
            color: #333;
            font-weight: 500;
        }
        .tabStyled .nav-tabs>li.active>a, .tabStyled .nav-tabs>li.active>a:hover, .tabStyled .nav-tabs>li>a:hover {
            background-color: #3284b5 !important;
            border: 1px solid #226c98 !important;
            color: #fff !important;
        }
       .tabStyled .tab-content {
           padding:20px;
           border:1px solid #ccc;
           border-top:none;
       }
       /*.shadBoxes {
           box-shadow: 0 0 5px rgba(0,0,0,0.12);
            border-radius: 10px;
            padding: 0 0px;
       }*/
    </style>--%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div>
        <asp:HiddenField ID="hdnexpid" runat="server" />
    </div>

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
                   <%--Rev Subhra 24-12-2018   0017670--%>
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

        <div>
            
        </div>
        
    </div>

    <div class="form_main">
        <asp:HiddenField runat="server" ID="hdndaily" />
        <asp:HiddenField runat="server" ID="hdtid" />
        <div class="row fontPP">
            <div class="col-md-2 col-lg-2">
                <label style="color: #b5285f; font-weight: bold;" class="clsTo">Head Branch:</label>
                <div>
                    <asp:DropDownList ID="ddlbranchHO" runat="server" Width="100%"></asp:DropDownList>
                </div>
            </div>
            <div class="col-md-2 col-lg-2">
                <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="Label1" runat="Server" Text="Branch : " CssClass="mylabel1"></asp:Label></label>
                <asp:HiddenField ID="hdnActivityType" runat="server" />
                <dxe:ASPxCallbackPanel runat="server" ID="ComponentBranchPanel" ClientInstanceName="cBranchComponentPanel" OnCallback="Componentbranch_Callback">
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
                                                            <dxe:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll" UseSubmitBehavior="False" />
                                                      <%--  </div>--%>
                                                        <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAll" UseSubmitBehavior="False" />                                                        
                                                        <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridBranchLookup" UseSubmitBehavior="False" />
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

                <asp:HiddenField ID="hdnActivityTypeText" runat="server" />
                <span id="MandatoryActivityType" style="display: none" class="validclass">
                    <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor1112_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                <asp:HiddenField ID="hdnSelectedBranches" runat="server" />
            </div>

            <div class="col-md-2 col-lg-2">
                <label style="color: #b5285f; font-weight: bold;" class="clsFrom">
                    <asp:Label ID="lblFromDate" runat="Server" Text="As On Date : " CssClass="mylabel1"></asp:Label>
                </label>
                <dxe:ASPxDateEdit ID="ASPxAsOnDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                    UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeAsOnDate">
                    <ButtonStyle Width="13px">
                    </ButtonStyle>
                </dxe:ASPxDateEdit>
            </div>

            <div class="col-md-2 col-lg-2">
                <span style="margin-bottom: 5px;display: inline-block;"><dxe:ASPxLabel ID="lbl_Customer" style="color: #b5285f;" runat="server" Text="Customer:">
                </dxe:ASPxLabel></span>                        
                <dxe:ASPxButtonEdit ID="txtCustName" runat="server" ReadOnly="true" ClientInstanceName="ctxtCustName" Width="100%" TabIndex="5">
                    <Buttons>
                        <dxe:EditButton>
                        </dxe:EditButton>
                    </Buttons>
                    <ClientSideEvents ButtonClick="function(s,e){CustomerButnClick();}" KeyDown="function(s,e){Customer_KeyDown(s,e);}" />
                </dxe:ASPxButtonEdit>
                <span id="MandatorysCustomer" style="display: none" class="validclass">
                    <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
            </div>

            <div class="col-md-2">
                <label style="color: #b5285f;" class="clsTo ">
                    <asp:Label ID="Label2" runat="Server" Text="Project : " CssClass="mylabel1"></asp:Label>
                </label>
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
                                                        <%--<div class="hide">--%>
                                                            <dxe:ASPxButton ID="ASPxButton3" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll_Project" UseSubmitBehavior="False"/>
                                                        <%--</div>--%>
                                                        <dxe:ASPxButton ID="ASPxButton4" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAll_Project" UseSubmitBehavior="False"/>                                                        
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
            </div>

            <div class="clear"></div> 

            <div class="col-md-6" style="padding-top: 13px;margin-bottom: 6px;">
                <button id="btnShow" class="btn btn-success" type="button" onclick="btn_ShowRecordsClick(this);">Show</button>
               
                <%--<dxe:ASPxButton ID="btnExportAll" runat="server" Text="Export all to Excel"  UseSubmitBehavior="False" OnClick="btnExport_Click"></dxe:ASPxButton>--%>
                <asp:Button id="btnExportAll" runat="server" class="btn btn-info" type="button" Text="Export all to Excel" UseSubmitBehavior="false" OnClick="btnExport_Click"></asp:Button>
            </div>
                
            <div class="clear"></div> 
            
            <%--<div class="col-md-2" style="padding-top:19px;padding-left:0">
                <input type="button" class="btn btn-primary " onclick="ShowReportClick()" value="Show"/>
            </div>     --%>      
            
        </div>

        <div class="FilterSide clearfix shadBoxes">
            <h4 class="pull-left c2 fontPP">Cost</h4>

           <%-- <div style="float: right; padding-right: 5px;"> 
                <asp:DropDownList ID="DropDownList1" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" OnChange="if(!AvailableExportOption()){return false;}" AutoPostBack="true">
                    <asp:ListItem Value="0">Export to</asp:ListItem> 
                    <asp:ListItem Value="2">XLS</asp:ListItem> 
                </asp:DropDownList>
                        
            </div>--%>
                  
        
        <%--<div class="clearfix fontPP tabStyled">
            <div>
                  <!-- Nav tabs -->
                  <ul class="nav nav-tabs" role="tablist">
                    <li role="presentation" class="active"><a href="#home" aria-controls="home" role="tab" data-toggle="tab">Cost</a></li>
                    <li role="presentation"><a href="#profile" aria-controls="profile" role="tab" data-toggle="tab">Revenue</a></li>
                    <li role="presentation"><a href="#messages" aria-controls="messages" role="tab" data-toggle="tab">Summary - Initial & Revised</a></li>
                    <li role="presentation"><a href="#settings" aria-controls="settings" role="tab" data-toggle="tab">Summary - Customer</a></li>
                    <li role="presentation"><a href="#Vendor" aria-controls="Vendor" role="tab" data-toggle="tab">Summary - Vendor</a></li>
                    <li role="presentation"><a href="#Others" aria-controls="Others" role="tab" data-toggle="tab">Others, If Any</a></li>
                  </ul>
                  <!-- Tab panes -->
                  <div class="tab-content">
                    <div role="tabpanel" class="tab-pane active" id="home">
                        
                    </div>
                    <div role="tabpanel" class="tab-pane" id="profile">..sdhs.</div>
                    <div role="tabpanel" class="tab-pane" id="messages">..hds.</div>
                    <div role="tabpanel" class="tab-pane" id="settings">.h..</div>
                      <div role="tabpanel" class="tab-pane" id="Vendor">..sd.</div>
                      <div role="tabpanel" class="tab-pane" id="Others">..sdh.</div>
                  </div>
              </div>
        </div>--%>
        
        <dxe:ASPxCallbackPanel runat="server" ID="ProjSummGridPanel" ClientInstanceName="cProjSummGridPanel" OnCallback="Component_Callback" ClientSideEvents-BeginCallback="Callback_EndCallback">
            <PanelCollection>
                <dxe:PanelContent runat="server">

            <div class="GridViewArea clearfix  colorGrid2">
                <dxe:ASPxGridView ID="CostGrid" runat="server" AutoGenerateColumns="False" ClientInstanceName="cCostGrid"
                     OnDataBinding="cCostGrid_DataBinding" SettingsPager-Mode="ShowAllRecords" Settings-HorizontalScrollBarMode="Visible" Width="100%" CssClass="pull-left" >
                    <SettingsSearchPanel Visible="False" Delay="5000" />
                    <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowFilterRow="true" ShowFilterRowMenu="True" />
                    <Styles Header-Wrap="True" />  
                    <Columns>
                        <dxe:GridViewDataTextColumn Caption="Estimate Cost" FieldName="EC" ReadOnly="True" Width="100" Visible="True" VisibleIndex="1">
                            <EditFormSettings Visible="True" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>
                        
                          <dxe:GridViewDataTextColumn Caption="Group" FieldName="GRP" ReadOnly="True" Width="130" Visible="True" VisibleIndex="2">
                            <EditFormSettings Visible="True" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Accounting Ledger Tracking (if Any)" FieldName="ALT" ReadOnly="True" Width="150" Visible="True" VisibleIndex="3">
                            <EditFormSettings Visible="True" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Initial Est. Cost" FieldName="IC" ReadOnly="True" Width="80" Visible="True" VisibleIndex="4">
                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <CellStyle HorizontalAlign="Right"></CellStyle>
                            <HeaderStyle HorizontalAlign="Right" />
                            <EditFormSettings Visible="True" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Est. Revised Cost" FieldName="RC" ReadOnly="True" Width="100" Visible="True" VisibleIndex="5">
                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <CellStyle HorizontalAlign="Right"></CellStyle>
                            <HeaderStyle HorizontalAlign="Right" />
                            <EditFormSettings Visible="True" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="P.O./Service Order Placed Till Date" FieldName="POAMT" ReadOnly="True" Width="120" Visible="True" VisibleIndex="6">
                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <CellStyle HorizontalAlign="Right"></CellStyle>
                            <HeaderStyle HorizontalAlign="Right" />
                            <EditFormSettings Visible="True" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Balance order required to be Placed" FieldName="BALORD" ReadOnly="True" Width="130" Visible="True" VisibleIndex="7">
                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <CellStyle HorizontalAlign="Right"></CellStyle>
                            <HeaderStyle HorizontalAlign="Right" />
                            <EditFormSettings Visible="True" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Further Any Cost/Budget Required" FieldName="FANYCOST" ReadOnly="True" Width="140" Visible="True" VisibleIndex="8">
                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <CellStyle HorizontalAlign="Right"></CellStyle>
                            <HeaderStyle HorizontalAlign="Right" />
                            <EditFormSettings Visible="True" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Cost Booked Tll Date" FieldName="CB" ReadOnly="True" Width="100" Visible="True" VisibleIndex="9">
                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <CellStyle HorizontalAlign="Right"></CellStyle>
                            <HeaderStyle HorizontalAlign="Right" />
                            <EditFormSettings Visible="True" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Balance P.O to Invoice" FieldName="BALPOPB" ReadOnly="True" Width="100" Visible="True" VisibleIndex="10">
                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <CellStyle HorizontalAlign="Right"></CellStyle>
                            <HeaderStyle HorizontalAlign="Right" />
                            <EditFormSettings Visible="True" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Balance Est. to Invoice" FieldName="BALICPB" ReadOnly="True" Width="100" Visible="True" VisibleIndex="11">
                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <CellStyle HorizontalAlign="Right"></CellStyle>
                            <HeaderStyle HorizontalAlign="Right" />
                            <EditFormSettings Visible="True" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="%" FieldName="PERCENTG" ReadOnly="True" Width="100" Visible="True" VisibleIndex="12">
                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <CellStyle HorizontalAlign="Right"></CellStyle>
                            <HeaderStyle HorizontalAlign="Right" />
                            <EditFormSettings Visible="True" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                    </Columns>
                    
                    <SettingsBehavior ColumnResizeMode="NextColumn" /> 
                </dxe:ASPxGridView>

               <dxe:ASPxGridViewExporter ID="CostGridExporter" runat="server" GridViewID="CostGrid" />
            </div>
           

              <div class="FilterSide clearfix">
                    <h4 class="pull-left c1 fontPP">Revenue</h4>

                   <%-- <div style="float: right; padding-right: 5px;padding-top:5px"> 
                        <asp:DropDownList ID="DropDownList1" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged3_2"   AutoPostBack="true">
                            <asp:ListItem Value="0">Export to</asp:ListItem> 
                            <asp:ListItem Value="2">XLS</asp:ListItem> 
                        </asp:DropDownList>
                        
                    </div>--%>
                  
                </div>

               <div class="GridViewArea colorGrid1 fontPP">
                <dxe:ASPxGridView ID="RevGrid" runat="server" AutoGenerateColumns="False" ClientInstanceName="cRevGrid" OnDataBinding="cRevGrid_DataBinding" SettingsPager-Mode="ShowAllRecords"
                      Settings-HorizontalScrollBarMode="Visible" Width="100%" CssClass="pull-left"  >
                    <SettingsSearchPanel Visible="False" Delay="5000" />
                    <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowFilterRow="true" ShowFilterRowMenu="True" />
                    <Styles Header-Wrap="True" />  
                    <Columns>

                        <dxe:GridViewDataTextColumn Caption="Estimated Revenue" FieldName="ER" ReadOnly="True" Width="150" Visible="True" VisibleIndex="1">
                            <EditFormSettings Visible="True" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>
                        
                        <dxe:GridViewDataTextColumn Caption="Group" FieldName="GRP" ReadOnly="True" Width="150" Visible="True" VisibleIndex="2"> 
                            <EditFormSettings Visible="True" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Accounting Ledger <br/> Tracking (if Any)" FieldName="ALT" ReadOnly="True" Width="200" Visible="True" VisibleIndex="3">
                            <EditFormSettings Visible="True" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Initial Revenue" FieldName="IR" ReadOnly="True" Width="100" Visible="True" VisibleIndex="4">
                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <CellStyle HorizontalAlign="Right"></CellStyle>
                            <HeaderStyle HorizontalAlign="Right" />
                            <EditFormSettings Visible="True" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Revised Revenue" FieldName="RR" ReadOnly="True" Width="100" Visible="True" VisibleIndex="5">
                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <CellStyle HorizontalAlign="Right"></CellStyle>
                            <HeaderStyle HorizontalAlign="Right" />
                            <EditFormSettings Visible="True" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Revenue Booked Till Date" FieldName="RB" ReadOnly="True" Width="150" Visible="True" VisibleIndex="6">
                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <CellStyle HorizontalAlign="Right"></CellStyle>
                            <HeaderStyle HorizontalAlign="Right" />
                            <EditFormSettings Visible="True" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Balance S.O to Invoice" FieldName="BALSOSB" ReadOnly="True" Width="150" Visible="True" VisibleIndex="7">
                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <CellStyle HorizontalAlign="Right"></CellStyle>
                            <HeaderStyle HorizontalAlign="Right" />
                            <EditFormSettings Visible="True" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>                         
                    </Columns>
                    
                    <SettingsBehavior ColumnResizeMode="NextColumn" /> 
                </dxe:ASPxGridView>

                <dxe:ASPxGridViewExporter ID="RevGridExporter"  runat="server" GridViewID="RevGrid" />
            </div>
            <br />
            <br />
            <br />

              <div class="FilterSide clearfix">
                    <h4 class="pull-left c4 fontPP">Summary - Initial & Revised</h4>

                    <%--<div style="float: right; padding-right: 5px;padding-top:5px"> 
                        <asp:DropDownList ID="cmbDetailITC" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="DetailITC_SelectedIndexChanged3_2"   AutoPostBack="true">
                            <asp:ListItem Value="0">Export to</asp:ListItem> 
                            <asp:ListItem Value="2">XLS</asp:ListItem> 
                        </asp:DropDownList>
                        
                    </div>--%>
                  
                </div>

               <div class="GridViewArea colorGrid4">
                <dxe:ASPxGridView ID="IRSummGrid" runat="server" AutoGenerateColumns="False" ClientInstanceName="cIRSummGrid" OnDataBinding="cIRSummGrid_DataBinding" SettingsPager-Mode="ShowAllRecords"
                      Settings-HorizontalScrollBarMode="Visible" Width="100%" CssClass="pull-left"  >
                    <SettingsSearchPanel Visible="False" Delay="5000" />
                    <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowFilterRow="true" ShowFilterRowMenu="True" />
                    <Styles Header-Wrap="True" />  
                    <Columns>

                        <dxe:GridViewDataTextColumn Caption="Estimated (Initial) Profit Amount" FieldName="EIPAMT" ReadOnly="True" Width="120" Visible="True" VisibleIndex="1">
                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <CellStyle HorizontalAlign="Right"></CellStyle>
                            <HeaderStyle HorizontalAlign="Right" />
                            <EditFormSettings Visible="True" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>
                          

                        <dxe:GridViewDataTextColumn Caption="Estimated (Initial) % of Profit on Cost" FieldName="EIPC" ReadOnly="True" Width="180" Visible="True" VisibleIndex="2">
                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <CellStyle HorizontalAlign="Right"></CellStyle>
                            <HeaderStyle HorizontalAlign="Right" />
                            <EditFormSettings Visible="True" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>


                        <dxe:GridViewDataTextColumn Caption="Estimated (Initial) % of Profit on Sales" FieldName="EIPS" ReadOnly="True" Width="150" Visible="True" VisibleIndex="3">
                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <CellStyle HorizontalAlign="Right"></CellStyle>
                            <HeaderStyle HorizontalAlign="Right" />
                            <EditFormSettings Visible="True" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Revenue Recognized against Cost" FieldName="RIRC" ReadOnly="True" Width="150" Visible="True" VisibleIndex="4">
                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <CellStyle HorizontalAlign="Right"></CellStyle>
                            <HeaderStyle HorizontalAlign="Right" />
                            <EditFormSettings Visible="True" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Estimated (Revised) Profit Amount" FieldName="ERPA" ReadOnly="True" Width="150" Visible="True" VisibleIndex="5">
                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <CellStyle HorizontalAlign="Right"></CellStyle>
                            <HeaderStyle HorizontalAlign="Right" />
                            <EditFormSettings Visible="True" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Estimated (Revised) % of Profit on Cost" FieldName="ERPC" ReadOnly="True" Width="150" Visible="True" VisibleIndex="6">
                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <CellStyle HorizontalAlign="Right"></CellStyle>
                            <HeaderStyle HorizontalAlign="Right" />
                            <EditFormSettings Visible="True" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Estimated (Revised] % of Profit on Sales" FieldName="ERPS" ReadOnly="True" Width="150" Visible="True" VisibleIndex="7">
                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <CellStyle HorizontalAlign="Right"></CellStyle>
                            <HeaderStyle HorizontalAlign="Right" />
                            <EditFormSettings Visible="True" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Revenue Recognized against Cost" FieldName="RRAC" ReadOnly="True" Width="150" Visible="True" VisibleIndex="8">
                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <CellStyle HorizontalAlign="Right"></CellStyle>
                            <HeaderStyle HorizontalAlign="Right" />
                            <EditFormSettings Visible="True" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>
                         
                    </Columns>
                    
                    <SettingsBehavior ColumnResizeMode="NextColumn" /> 
                </dxe:ASPxGridView>

                <dxe:ASPxGridViewExporter ID="IRSummGridExporter"  runat="server" GridViewID="IRSummGrid" />
            </div>

                <div class="FilterSide clearfix">
                    <h4 class="pull-left c3 fontPP">Summary - Customer</h4>

                    <%--<div style="float: right; padding-right: 5px;padding-top:5px"> 
                        <asp:DropDownList ID="cmbDetailITC" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="DetailITC_SelectedIndexChanged3_2"   AutoPostBack="true">
                            <asp:ListItem Value="0">Export to</asp:ListItem> 
                            <asp:ListItem Value="2">XLS</asp:ListItem> 
                        </asp:DropDownList>
                        
                    </div>--%>
                  
                </div>

               <div class="GridViewArea colorGrid3">
                <dxe:ASPxGridView ID="CustSummGrid" runat="server" AutoGenerateColumns="False" ClientInstanceName="cCustSummGrid" OnDataBinding="cCustSummGrid_DataBinding" SettingsPager-Mode="ShowAllRecords"
                      Settings-HorizontalScrollBarMode="Visible" Width="100%" CssClass="pull-left"  >
                    <SettingsSearchPanel Visible="False" Delay="5000" />
                    <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowFilterRow="true" ShowFilterRowMenu="True" />
                    <Styles Header-Wrap="True" />  
                    <Columns>

                        <dxe:GridViewDataTextColumn Caption="Invoice to Customer As on" FieldName="INVTOCUST" ReadOnly="True" Width="160" Visible="True" VisibleIndex="1">
                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <CellStyle HorizontalAlign="Right"></CellStyle>
                            <HeaderStyle HorizontalAlign="Right" />
                            <EditFormSettings Visible="True" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>
                          

                        <dxe:GridViewDataTextColumn Caption="Collection As on" FieldName="COLLECTIONS" ReadOnly="True" Width="150" Visible="True" VisibleIndex="2">
                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <CellStyle HorizontalAlign="Right"></CellStyle>
                            <HeaderStyle HorizontalAlign="Right" />
                            <EditFormSettings Visible="True" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>


                        <dxe:GridViewDataTextColumn Caption="Retention to Customer As on" FieldName="RTOCUST" ReadOnly="True" Width="150" Visible="True" VisibleIndex="3">
                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <CellStyle HorizontalAlign="Right"></CellStyle>
                            <HeaderStyle HorizontalAlign="Right" />
                            <EditFormSettings Visible="True" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Outstanidng from Customer" FieldName="OUTSCUST" ReadOnly="True" Width="180" Visible="True" VisibleIndex="4">
                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <CellStyle HorizontalAlign="Right"></CellStyle>
                            <HeaderStyle HorizontalAlign="Right" />
                            <EditFormSettings Visible="True" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="ITDS" FieldName="ITDS" ReadOnly="True" Width="150" Visible="True" VisibleIndex="5">
                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <CellStyle HorizontalAlign="Right"></CellStyle>
                            <HeaderStyle HorizontalAlign="Right" />
                            <EditFormSettings Visible="True" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="STDS" FieldName="STDS" ReadOnly="True" Width="150" Visible="True" VisibleIndex="6">
                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <CellStyle HorizontalAlign="Right"></CellStyle>
                            <HeaderStyle HorizontalAlign="Right" />
                            <EditFormSettings Visible="True" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="LD" FieldName="LD" ReadOnly="True" Width="150" Visible="True" VisibleIndex="7">
                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <CellStyle HorizontalAlign="Right"></CellStyle>
                            <HeaderStyle HorizontalAlign="Right" />
                            <EditFormSettings Visible="True" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>
                         
                    </Columns>
                    
                    <SettingsBehavior ColumnResizeMode="NextColumn" /> 
                </dxe:ASPxGridView>

                <dxe:ASPxGridViewExporter ID="CustSummGridExporter" runat="server" GridViewID="CustSummGrid" />
            </div>

                <div class="FilterSide clearfix">
                    <h4 class="pull-left c2 fontPP">Summary - Vendor</h4>

                    <%--<div style="float: right; padding-right: 5px;padding-top:5px"> 
                        <asp:DropDownList ID="cmbDetailITC" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="DetailITC_SelectedIndexChanged3_2"   AutoPostBack="true">
                            <asp:ListItem Value="0">Export to</asp:ListItem> 
                            <asp:ListItem Value="2">XLS</asp:ListItem> 
                        </asp:DropDownList>
                        
                    </div>--%>
                  
                </div>

               <div class="GridViewArea colorGrid2">
                <dxe:ASPxGridView ID="VendSummGrid" runat="server" AutoGenerateColumns="False" ClientInstanceName="cVendSummGrid" OnDataBinding="cVendSummGrid_DataBinding" SettingsPager-Mode="ShowAllRecords"
                      Settings-HorizontalScrollBarMode="Visible" Width="100%" CssClass="pull-left"  >
                    <SettingsSearchPanel Visible="False" Delay="5000" />
                    <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowFilterRow="true" ShowFilterRowMenu="True" />
                    <Columns>

                        <dxe:GridViewDataTextColumn Caption="Invoice to Vendor as on" FieldName="INVTOVEND" ReadOnly="True" Width="160" Visible="True" VisibleIndex="1">
                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <CellStyle HorizontalAlign="Right"></CellStyle>
                            <HeaderStyle HorizontalAlign="Right" />
                            <EditFormSettings Visible="True" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>
                          

                        <dxe:GridViewDataTextColumn Caption="Vendor Pmt made as on" FieldName="VENDPAYMENT" ReadOnly="True" Width="150" Visible="True" VisibleIndex="2">
                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <CellStyle HorizontalAlign="Right"></CellStyle>
                            <HeaderStyle HorizontalAlign="Right" />
                            <EditFormSettings Visible="True" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>


                        <dxe:GridViewDataTextColumn Caption="Retention to Vendor" FieldName="RTOVEND" ReadOnly="True" Width="150" Visible="True" VisibleIndex="3">
                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <CellStyle HorizontalAlign="Right"></CellStyle>
                            <HeaderStyle HorizontalAlign="Right" />
                            <EditFormSettings Visible="True" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Vendor Pmt Outstanding as on" FieldName="OUTSVEND" ReadOnly="True" Width="180" Visible="True" VisibleIndex="4">
                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <CellStyle HorizontalAlign="Right"></CellStyle>
                            <HeaderStyle HorizontalAlign="Right" />
                            <EditFormSettings Visible="True" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>
                         
                    </Columns>
                    
                    <SettingsBehavior ColumnResizeMode="NextColumn" /> 
                </dxe:ASPxGridView>

                <dxe:ASPxGridViewExporter ID="VendSummGridExporter" runat="server" GridViewID="VendSummGrid" />
            </div>

                <div class="FilterSide clearfix">
                    <h4 class="pull-left c1 fontPP">Others, If Any</h4>

                    <%--<div style="float: right; padding-right: 5px;padding-top:5px"> 
                        <asp:DropDownList ID="cmbDetailITC" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="DetailITC_SelectedIndexChanged3_2"   AutoPostBack="true">
                            <asp:ListItem Value="0">Export to</asp:ListItem> 
                            <asp:ListItem Value="2">XLS</asp:ListItem> 
                        </asp:DropDownList>
                        
                    </div>--%>
                  
                </div>

               <div class="GridViewArea colorGrid1">
                <dxe:ASPxGridView ID="OTHGrid" runat="server" AutoGenerateColumns="False" ClientInstanceName="cOTHGrid" OnDataBinding="cOTHGrid_DataBinding" SettingsPager-Mode="ShowAllRecords"
                      Settings-HorizontalScrollBarMode="Visible" Width="100%" CssClass="pull-left">
                    <SettingsSearchPanel Visible="False" Delay="5000" />
                    <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowFilterRow="true" ShowFilterRowMenu="True" />
                    <Columns>

                        <dxe:GridViewDataTextColumn Caption="Bank Guarantee Amount" FieldName="BGAMT" ReadOnly="True" Width="160" Visible="True" VisibleIndex="1">
                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <CellStyle HorizontalAlign="Right"></CellStyle>
                            <HeaderStyle HorizontalAlign="Right" />
                            <EditFormSettings Visible="True" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>
                          

                        <dxe:GridViewDataTextColumn Caption="Validity if BG" FieldName="VALIDITY" ReadOnly="True" Width="150" Visible="True" VisibleIndex="2">
                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <CellStyle HorizontalAlign="Right"></CellStyle>
                            <HeaderStyle HorizontalAlign="Right" />
                            <EditFormSettings Visible="True" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>
                    </Columns>
                    
                    <SettingsBehavior ColumnResizeMode="NextColumn" /> 
                </dxe:ASPxGridView>

                <dxe:ASPxGridViewExporter ID="OTHGridExporter" runat="server" GridViewID="OTHGrid" />
            </div>

                    </dxe:PanelContent>
                </PanelCollection>
            </dxe:ASPxCallbackPanel>

    </div>

    <div>
    </div>

    <!--Customer Modal -->
    <div class="modal fade" id="CustModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Customer Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Customerkeydown(event)" id="txtCustSearch" width="100%" placeholder="Search By Customer Name or Unique ID" />
                    <div id="CustomerTable">
                        <table border='1' width="100%">

                            <tr class="HeaderStyle" style="font-size:small">
                                <th>Select</th>
                                 <th class="hide">id</th>
                                <th>Customer Name</th>
                                 <th>Unique ID</th>
                                <th>Alternate No.</th>
                                <th>Address</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btnOkformultiselection btn-default"  onclick="DeSelectAll('CustomerSource')">Deselect All</button>
                    <button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal" onclick="OKPopup('CustomerSource')">OK</button>
                    <%--<button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal">Close</button>--%>
                </div>
            </div>
        </div>
    </div>
    <!--Customer Modal -->

    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="true" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>
     <asp:HiddenField ID="hdnCustomerId" runat="server" />

    <asp:HiddenField ID="hdnBranchSelection" runat="server" />
</asp:Content>
