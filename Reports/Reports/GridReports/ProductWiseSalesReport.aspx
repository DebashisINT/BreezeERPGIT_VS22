<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="ProductWiseSalesReport.aspx.cs" Inherits="Reports.Reports.GridReports.ProductWiseSalesReport" %>

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
        function AllProduct(obj) {
            if (obj == 'selectAllProduct') {
                if (chkallProduct.checked == true) {
                    ctxtProdName.SetText('');
                    GetObjectID('hdncWiseProductId').value = '';
                    document.getElementById("txtProdSearch").value = ""
                    ctxtProdName.SetEnabled(false);
                }
                else {
                    ctxtProdName.SetEnabled(true);
                }
            }
        }
        $(document).ready(function () {

            $('#ProdModel').on('shown.bs.modal', function () {
                $('#txtProdSearch').focus();
            })
        });
        var ProdArr = new Array();
        $(document).ready(function () {
            var ProdObj = new Object();
            ProdObj.Name = "ProductSource";
            ProdObj.ArraySource = ProdArr;
            arrMultiPopup.push(ProdObj);
        })
        function ProductButnClick(s, e) {
            $('#ProdModel').modal('show');
        }

        function Product_KeyDown(s, e) {
            if (e.htmlEvent.key == "Enter") {
                $('#ProdModel').modal('show');
            }
        }

        function Productkeydown(e) {
            var OtherDetails = {}

            if ($.trim($("#txtProdSearch").val()) == "" || $.trim($("#txtProdSearch").val()) == null) {
                return false;
            }
            OtherDetails.SearchKey = $("#txtProdSearch").val();
            OtherDetails.ClassID = hdnClassId.value;

            if (e.code == "Enter" || e.code == "NumpadEnter") {

                var HeaderCaption = [];
                HeaderCaption.push("Code");
                HeaderCaption.push("Name");
                HeaderCaption.push("Hsn");


                if ($("#txtProdSearch").val() != "") {
                    callonServerM("Services/Master.asmx/GetClassWiseProduct", OtherDetails, "ProductTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "ProductSource");
                }
            }
            else if (e.code == "ArrowDown") {
                if ($("input[dPropertyIndex=0]"))
                    $("input[dPropertyIndex=0]").focus();
            }
        }
        function SetSelectedValues(Id, Name, ArrName) {
            if (ArrName == 'ProductSource') {
                var key = Id;
                if (key != null && key != '') {
                    $('#ProdModel').modal('hide');
                    ctxtProdName.SetText(Name);
                    GetObjectID('hdncWiseProductId').value = key;
                }
                else {
                    ctxtProdName.SetText('');
                    GetObjectID('hdncWiseProductId').value = '';
                }
            }

        }
        function SetfocusOnseach(indexName) {


            if (indexName == "dPropertyIndex")
                $('#txtProdSearch').focus();
            else
                $('#txtProdSearch').focus();
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
        <div class="col-md-2">
            <dxe:ASPxLabel ID="lbl_Product" style="color: #b5285f; font-weight: bold;" runat="server" Text="Product :">
            </dxe:ASPxLabel>
            <dxe:ASPxButtonEdit ID="txtProdName" runat="server" ReadOnly="true" ClientInstanceName="ctxtProdName" Width="100%" TabIndex="5">
                <buttons>
                        <dxe:EditButton>
                        </dxe:EditButton>
                    </buttons>
                <clientsideevents buttonclick="function(s,e){ProductButnClick();}" keydown="function(s,e){Product_KeyDown(s,e);}" />
            </dxe:ASPxButtonEdit>
        </div>
        <div class="col-md-2 lblmTop8">
            <asp:CheckBox runat="server" Style="color: #b5285f; font-weight: bold;" ID="chkallProduct" Checked="false" Text="All Product" />
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
                    AutoPostBack="true">
                    <asp:ListItem Value="0">Export to</asp:ListItem>
                    <asp:ListItem Value="1">PDF</asp:ListItem>
                    <asp:ListItem Value="2">XLSX</asp:ListItem>
                    <asp:ListItem Value="3">RTF</asp:ListItem>
                    <asp:ListItem Value="4">CSV</asp:ListItem>

                </asp:DropDownList>
            </div>
        </div>

        <div>
            <br />
        </div>
          <%--<table class="TableMain100">
            <tr>
                <td colspan="2">--%>
                    <div>
                        <dxe:ASPxGridView runat="server" ID="ShowGrid" ClientInstanceName="Grid" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False"
                            Settings-HorizontalScrollBarMode="Visible" SettingsBehavior-ColumnResizeMode="Control"  
                             >                          
                            <columns>


                                
                                <dxe:GridViewDataTextColumn FieldName="" Caption="Product" VisibleIndex="2" />
                                <dxe:GridViewDataTextColumn FieldName="" Caption="Cumulative Sales % Till Date" VisibleIndex="3" />                             
                                <dxe:GridViewDataTextColumn FieldName="" Caption="Weightage cumulative  % Achd"  VisibleIndex="4" />
                                <dxe:GridViewDataTextColumn FieldName="" Caption="% April Sales"  VisibleIndex="5" />
                                <dxe:GridViewDataTextColumn FieldName="" Caption="Weightage % Achd" VisibleIndex="6" />
                                                           
                               


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
               <%-- </td>
            </tr>
        </table>--%>
        




        <!--Product Modal -->
        <div class="modal fade" id="ProdModel" role="dialog">
            <div class="modal-dialog">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">Product Search</h4>
                    </div>
                    <div class="modal-body">
                        <input type="text" onkeydown="Productkeydown(event)" id="txtProdSearch" width="100%" placeholder="Search By Product Name" />
                        <div id="ProductTable">
                            <table border='1' width="100%">

                                <tr class="HeaderStyle" style="font-size: small">
                                    <th>Select</th>
                                    <th class="hide">id</th>
                                    <th>Code</th>
                                    <th>Name</th>
                                    <th>Hsn</th>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btnOkformultiselection btn-default" onclick="DeSelectAll('ProductSource')">Deselect All</button>
                        <button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal" onclick="OKPopup('ProductSource')">OK</button>
                        <%--<button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal">Close</button>--%>
                    </div>
                </div>
            </div>
        </div>
        <asp:HiddenField ID="hdncWiseProductId" runat="server" />
        <asp:HiddenField ID="hdnClassId" runat="server" />
        <!--Product Modal -->
    </div>
</asp:Content>
