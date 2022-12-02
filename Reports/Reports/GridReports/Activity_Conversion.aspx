<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="Activity_Conversion.aspx.cs" Inherits="Reports.Reports.GridReports.Activity_Conversion" %>

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
    <style>
        #ShowGrid, #ShowGrid .dxgvCSD {
            width: 100% !important;
        }
    </style>
     <style>
        .plhead a {
            font-size: 16px;
            padding-left: 10px;
            position: relative;
            width: 100%;
            display: block;
            padding: 9px 10px 5px 10px;
        }

            .plhead a > i {
                position: absolute;
                top: 11px;
                right: 15px;
            }

        #accordion {
            margin-bottom: 10px;
        }

        .companyName {
            font-size: 16px;
            font-weight: bold;
            margin-bottom: 15px;
        }

        

        .plhead a.collapsed .fa-minus-circle {
            display: none;
        }
    </style>
    <script>
        function AllSalesman(obj) {
            if (obj == 'selectAllSalesman') {
                if (chkallSalesman.checked == true) {
                    ctxtSalesManAgent.SetText('');
                    GetObjectID('hdnSalesManAgentId').value = '';
                    document.getElementById("txtSalesManSearch").value = ""
                    ctxtSalesManAgent.SetEnabled(false);
                }
                else {
                    ctxtSalesManAgent.SetEnabled(true);
                }
            }
        }
        $(document).ready(function () {
            $('#SalesManModel').on('shown.bs.modal', function () {
                $('#txtSalesManSearch').focus();
            })
           
        });
        var SalesArr = new Array();
        $(document).ready(function () {
            var CustObj = new Object();
            CustObj.Name = "SalesManSource";
            CustObj.ArraySource = SalesArr;
            arrMultiPopup.push(CustObj);
        })

        function SalesManButnClick(s, e) {
            $('#SalesManModel').modal('show');
            $("#txtSalesManSearch").focus();
        }

        function SalesManbtnKeyDown(s, e) {
            if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
                $('#SalesManModel').modal('show');
                $("#txtSalesManSearch").focus();
            }
        }
        function SalesMankeydown(e) {

            var OtherDetails = {}
            OtherDetails.SearchKey = $("#txtSalesManSearch").val();
            if ($.trim($("#txtSalesManSearch").val()) == "" || $.trim($("#txtSalesManSearch").val()) == null) {
                return false;
            }
            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var HeaderCaption = [];
                HeaderCaption.push("Name");
                if ($("#txtSalesManSearch").val() != null && $("#txtSalesManSearch").val() != "") {

                    callonServerM("Services/Master.asmx/GetSalesManAgent", OtherDetails, "SalesManTable", HeaderCaption, "salesmanIndex", "SetSelectedValues", "SalesManSource");

                }
            }
            else if (e.code == "ArrowDown") {
                if ($("input[salesmanIndex=0]"))
                    $("input[salesmanIndex=0]").focus();
            }
        }
        function SetSelectedValues(Id, Name, ArrName) {
             if (ArrName == 'SalesManSource') {
                var key = Id;
                if (key != null && key != '') {
                    $('#SalesManModel').modal('hide');
                    ctxtSalesManAgent.SetText(Name);
                    GetObjectID('hdnSalesManAgentId').value = key;
                }
            }         

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">

        <div class="panel-group" id="accordion" role="tablist" aria-multiselectable="true">
            <div class="panel panel-info">
                <div class="panel-heading" role="tab" id="headingOne">
                    <h4 class="panel-title plhead">
                        <a role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="true" aria-controls="collapseOne" class="collapsed">
                            <asp:Label ID="RptHeading" runat="Server" Text="" Width="470px" Style="font-weight: bold;"></asp:Label>
                            <i class="fa fa-plus-circle"></i>
                            <i class="fa fa-minus-circle"></i>
                        </a>
                    </h4>
                </div>
                <div id="collapseOne" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingOne">
                    <div class="panel-body">
                        <div class="companyName">
                            <asp:Label ID="CompName" runat="Server" Text="" Width="470px"></asp:Label>
                        </div>
                        <%--Rev Subhra 11-12-2018   0017670--%>
                        <div>
                            <asp:Label ID="CompBranch" runat="Server" Text="" Width="470px"></asp:Label>
                        </div>
                        <%--End of Rev--%>
                        <div>
                            <asp:Label ID="CompAdd" runat="Server" Text="" Width="470px"></asp:Label>
                        </div>
                        <div>
                            <asp:Label ID="CompOth" runat="Server" Text="" Width="470px"></asp:Label>
                        </div>
                        <div>
                            <asp:Label ID="CompPh" runat="Server" Text="" Width="470px"></asp:Label>
                        </div>
                        <div>
                            <asp:Label ID="CompAccPrd" runat="Server" Text="" Width="470px"></asp:Label>
                        </div>
                        <div>
                            <asp:Label ID="DateRange" runat="Server" Text="" Width="470px"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="form_main">
        <div class="col-md-2 ">
            <dxe:ASPxLabel ID="ASPxLabel3" runat="server" style="color: #b5285f; font-weight: bold;" Text="Salesman/Agents">
            </dxe:ASPxLabel>
            <dxe:ASPxButtonEdit ID="txtSalesManAgent" runat="server" ReadOnly="true" ClientInstanceName="ctxtSalesManAgent"  Width="100%">
                <buttons>
                    <dxe:EditButton>
                    </dxe:EditButton>
                </buttons>
                <clientsideevents buttonclick="function(s,e){SalesManButnClick();}" keydown="SalesManbtnKeyDown" />
            </dxe:ASPxButtonEdit>
        </div>
        <div class="col-md-2 lblmTop8">
            <asp:CheckBox runat="server" Style="color: #b5285f; font-weight: bold;" ID="chkallSalesman" Checked="false" Text="All Salesman" />
        </div>
        <div class="col-md-2">
            <div style="color: #b5285f; font-weight: bold;" class="clsFrom">
                <asp:Label ID="lblFromDate" runat="Server" Text="From Date : " CssClass="mylabel1"
                    Width="92px"></asp:Label>
            </div>
            <div>
                <dxe:ASPxDateEdit ID="ASPxFromDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                    UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeFromDate">
                    <buttonstyle width="13px">
                        </buttonstyle>
                </dxe:ASPxDateEdit>
            </div>
        </div>

        <div class="col-md-2">
            <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                <asp:Label ID="lblToDate" runat="Server" Text="To Date : " CssClass="mylabel1"
                    Width="92px"></asp:Label>
            </div>
            <div>
                <dxe:ASPxDateEdit ID="ASPxToDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                    UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeToDate">
                    <buttonstyle width="13px">
                        </buttonstyle>

                </dxe:ASPxDateEdit>
            </div>
        </div>

         <div class="col-md-2">
            <div style="padding-top: 5px">
                <button id="btnShow" class="btn btn-primary" type="button" onclick="btn_ShowRecordsClick(this);">Show</button>
                <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary"
                     AutoPostBack="true" >
                    <asp:ListItem Value="0">Export to</asp:ListItem>
                    <asp:ListItem Value="1">PDF</asp:ListItem>
                    <asp:ListItem Value="2">XLSX</asp:ListItem>
                    <asp:ListItem Value="3">RTF</asp:ListItem>
                    <asp:ListItem Value="4">CSV</asp:ListItem>

                </asp:DropDownList>
            </div>
        </div>
     
        <table class="TableMain100">
            <tr>
                <td colspan="2">
                    <div>
                        <dxe:ASPxGridView runat="server" ID="ShowGrid" ClientInstanceName="Grid" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False"
                            Settings-HorizontalScrollBarMode="Visible" SettingsBehavior-ColumnResizeMode="Control"
                             >                          
                            <columns>


                                
                                <dxe:GridViewDataTextColumn FieldName="" Caption="Name of sales person" VisibleIndex="2" />
                                <dxe:GridViewDataTextColumn FieldName="" Caption="No of Future Act" VisibleIndex="3" />                             
                                <dxe:GridViewDataTextColumn FieldName="" Caption="Pending Future Act"  VisibleIndex="4" />
                                <dxe:GridViewDataTextColumn FieldName="" Caption="% Pending future Act"  VisibleIndex="5" />
                                <dxe:GridViewDataTextColumn FieldName="" Caption="No of customers contacted" VisibleIndex="6" />
                                <dxe:GridViewDataTextColumn FieldName="" Caption="No of activities done" VisibleIndex="7" />                             
                                <dxe:GridViewDataTextColumn FieldName="" Caption="No of visits done"  VisibleIndex="8" />
                                <dxe:GridViewDataTextColumn FieldName="" Caption="% of visits pending as per order not collected"  VisibleIndex="9" />
                                <dxe:GridViewDataTextColumn FieldName="" Caption="No of orders - Unique" VisibleIndex="10" />
                                <dxe:GridViewDataTextColumn FieldName="" Caption="% orders pending of future" VisibleIndex="11" />                             
                                <dxe:GridViewDataTextColumn FieldName="" Caption="Conversion ratio"  VisibleIndex="12" />
                                                          
                               


                            </columns>
                            <settingsbehavior confirmdelete="true" enablecustomizationwindow="true" enablerowhottrack="true" />
                            <settings showfooter="true" showgrouppanel="true" showgroupfooter="VisibleIfExpanded" />
                            <settingsediting mode="EditForm" />
                            <settingscontextmenu enabled="true" />
                            <settingsbehavior autoexpandallgroups="true" columnresizemode="Control" />
                            <settings showgrouppanel="True" showstatusbar="Visible" showfilterrow="true" />
                            <settingssearchpanel visible="false" />
                            <settingspager pagesize="10">
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100" />
                            </settingspager>
                            <settings showfilterrow="True" showfilterrowmenu="true" showstatusbar="Visible" usefixedtablelayout="true" />

                            
                        </dxe:ASPxGridView>

<%--                        <dx:LinqServerModeDataSource ID="GenerateEntityServerModeDataSource" runat="server" OnSelecting="GenerateEntityServerModeDataSource_Selecting"
                        ContextTypeName="ReportSourceDataContext" TableName="CUSTVENDLEDGER_REPORT" />--%>

                    </div>
                </td>
            </tr>
        </table>
        <%--SalesMan/Agent--%>
    <div class="modal fade" id="SalesManModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">SalesMan/Agent Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="SalesMankeydown(event)" id="txtSalesManSearch" autofocus width="100%" placeholder="Search By SalesMan/Agent Name" />

                    <div id="SalesManTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Name</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btnOkformultiselection btn-default" onclick="DeSelectAll('SalesManSource')">Deselect All</button>
                    <button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal" onclick="OKPopup('SalesManSource')">OK</button>
                    <%--<button type="button" class="btn btn-default" data-dismiss="modal">Close</button>--%>
                </div>
            </div>

        </div>
    </div>
    <%--SalesMan/Agent--%>
    <asp:HiddenField ID="hdnSalesManAgentId" runat="server" />
    </div>
</asp:Content>
