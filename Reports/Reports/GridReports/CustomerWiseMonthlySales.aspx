<%@ Page Title="Customer Wise Monthly Report" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true"
    CodeBehind="CustomerWiseMonthlySales.aspx.cs" Inherits="Reports.Reports.GridReports.CustomerWiseMonthlySales" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <link href="CSS/SearchPopup.css" rel="stylesheet" />
    <script src="JS/SearchMultiPopup.js"></script>

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
    <%-- For multiselection when click on ok button--%>
    <script type="text/javascript">
        function SetSelectedValues(Id, Name, ArrName) {
            if (ArrName == 'ClassSource') {
                var key = Id;
                if (key != null && key != '') {
                    $('#ClassModel').modal('hide');
                    ctxtClass.SetText(Name);
                    GetObjectID('hdnClassId').value = key;


                    ctxtProdName.SetText('');
                    $('#txtProdSearch').val('')

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
                    ctxtProdName.SetText('');
                    GetObjectID('hdncWiseProductId').value = '';
                }
            }

        }

    </script>
   <%-- For multiselection when click on ok button--%>
    <%-- For Class multiselection--%>
 
     <script type="text/javascript">
         $(document).ready(function () {
             $('#ClassModel').on('shown.bs.modal', function () {
                 $('#txtClassSearch').focus();
             })

         })
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
                     callonServerM("Services/Master.asmx/GetClassBind", OtherDetails, "ClassTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "ClassSource");
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
 <%-- For Product multiselection--%>
 
     <script type="text/javascript">
         $(document).ready(function () {
             $('#ProdModel').on('shown.bs.modal', function () {
                 $('#txtProdSearch').focus();
             })

         })
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

         function SetfocusOnseach(indexName) {
             if (indexName == "dPropertyIndex")
                 $('#txtProdSearch').focus();
             else
                 $('#txtProdSearch').focus();
         }
   </script>
      <%-- For Product multiselection--%>
    <script type="text/javascript">

        $(function () {
            csalesmanPanel.PerformCallback('Salesman~');
            cindustryPanel.PerformCallback('Industry~');
            //cproductPanel.PerformCallback('Products~');
        });


        function CloselookupSalesman() {
            gridsalesmanLookup.ConfirmCurrentSelection();
            gridsalesmanLookup.HideDropDown();
        }

        function CloselookupIndustry() {
            gridindustryLookup.ConfirmCurrentSelection();
            gridindustryLookup.HideDropDown();
        }


        //function CloselookupProducts() {
        //    gridproductLookup.ConfirmCurrentSelection();
        //    gridproductLookup.HideDropDown();

        //}



        function btn_ShowRecordsClick(e) {
           
            gridsalesmanLookup.GetGridView().GetSelectedFieldValues("ID", GotSelectedSalesmanValues);
      
          
        

        }
        function GotSelectedSalesmanValues(values) {
           //  alert(values);
             $("#hdnsalesman").val(values);
             //alert($("#hdnsalesman").val());

            //Rev Subhra  0017670   12-12-2018
             //gridindustryLookup.GetGridView().GetSelectedFieldValues("ID", GotSelectedProductsValues);
             gridindustryLookup.GetGridView().GetSelectedFieldValues("ID", GotSelectedIndustriesValues);
            //End of Rev 
        }
        function GotSelectedIndustriesValues(values1) {

           // alert(values1);

            $("#hdnIndustry").val(values1);
          //  alert($("#hdnIndustry").val());

            //gridproductLookup.GetGridView().GetSelectedFieldValues("ID", GotSelectedProductsValues);

            //Rev Subhra  0017670   12-12-2018
            Grid.PerformCallback('DelailsGrid~' + $("#hdnsalesman").val() + '~' + $("#hdnIndustry").val() );

            //End of Rev
        }

        //Rev Subhra  0017670   12-12-2018 commented
        //function GotSelectedProductsValues(values2) {
        //    //alert(values2);
        //    $("#hdnProducts").val(values2);
        // //   alert($("#hdnProducts").val());
        //    //  if ($("#hdnsalesman").val() != '' && $("#hdnIndustry").val() != '' ) {

        //  //  alert($("#hdnsalesman").val() + '-' + $("#hdnIndustry").val() + '-' + $("#hdnProducts").val());
        //    Grid.PerformCallback('DelailsGrid~' + $("#hdnsalesman").val() + '~' + $("#hdnIndustry").val() + '~' + $("#hdnProducts").val());

        //    //    }
        //    //    else
        //    //    {

        //    //      jAlert('All Fields are mandatory.');

        //    //  }

        //}
        //End of Rev
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
            <h3>Customer Wise Monthly Report</h3>
        </div>--%>
        <div class="panel-group" id="accordion" role="tablist" aria-multiselectable="true">
          <div class="panel panel-info">
            <div class="panel-heading" role="tab" id="headingOne">
              <h4 class="panel-title plhead">
                <a role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="true" aria-controls="collapseOne" class="collapsed">
                  <asp:Label ID="RptHeading" runat="Server" Text=""  style="font-weight:bold;"></asp:Label>
                    <i class="fa fa-plus-circle" ></i>
                    <i class="fa fa-minus-circle"></i>
                </a>
              </h4>
            </div>
            <div id="collapseOne" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingOne">
              <div class="panel-body">
                   
                    <div class="companyName">
                        <asp:Label ID="CompName" runat="Server" Text="" ></asp:Label>
                    </div>
                    <div>
                        <asp:Label ID="CompAdd" runat="Server" Text="" Width="470px" style="font-size: 12px;"></asp:Label>
                    </div>
                    <div>
                        <asp:Label ID="CompOth" runat="Server" Text="" Width="470px" style="font-size: 12px;"></asp:Label>
                    </div>
                    <div>
                        <asp:Label ID="CompPh" runat="Server" Text="" Width="470px" style="font-size: 12px;"></asp:Label>
                    </div>       
                    <div>
                        <asp:Label ID="CompAccPrd" runat="Server" Text="" Width="470px" style="font-size: 12px;"></asp:Label>
                    </div>
              </div>
            </div>
          </div>
        </div>

        <div>
            
        </div>
        
    </div>
    <div class="form_main">

        <div class="row">


            <div class="col-md-3">
                <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="Label1" runat="Server" Text="Salesman : " CssClass="mylabel1"
                        Width="92px"></asp:Label>
                </div>
                <div>


                    <span id="MandatoryActivityType" style="display: none" class="validclass">
                        <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor1112_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>


                    <dxe:ASPxCallbackPanel runat="server" ID="cpsalesman" ClientInstanceName="csalesmanPanel" OnCallback="Salesman_Callback">
                        <panelcollection>
                        <dxe:PanelContent runat="server">


                    <dxe:ASPxGridLookup ID="lookup_salesman" SelectionMode="Multiple" runat="server" ClientInstanceName="gridsalesmanLookup"
                        OnDataBinding="lookup_salesman_DataBinding"  TabIndex="1" 
                        KeyFieldName="ID" Width="100%" TextFormatString="{1}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                        <columns>
                                        <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="30" Caption=" " />

                                        <dxe:GridViewDataColumn FieldName="ID" Visible="true" VisibleIndex="1" Caption="ID" width="0">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataColumn>

                                        <dxe:GridViewDataColumn FieldName="CusName" Visible="true" VisibleIndex="2" Caption="Salesman Name" Settings-AutoFilterCondition="Contains">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataColumn>


                                    </columns>
                        <gridviewproperties settings-verticalscrollbarmode="Auto" settingspager-mode="ShowAllRecords">
                                        <Templates>
                                            <StatusBar>
                                                <table class="OptionsTable" style="float: right">
                                                    <tr>
                                                        <td>
                                                            <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloselookupSalesman" UseSubmitBehavior="False" />
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

                        </gridviewproperties>

                    </dxe:ASPxGridLookup>


                     </dxe:PanelContent>
                    </panelcollection>
                    </dxe:ASPxCallbackPanel>

                </div>
            </div>

            <div class="col-md-3">

                <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="Label3" runat="Server" Text="Industry : " CssClass="mylabel1"
                        Width="110px"></asp:Label>
                </div>

                <div>

                    <dxe:ASPxCallbackPanel runat="server" ID="cpIndustry" ClientInstanceName="cindustryPanel" OnCallback="Industry_Callback">
                        <panelcollection>
                        <dxe:PanelContent runat="server">
                    <dxe:ASPxGridLookup ID="lookup_Industry" SelectionMode="Multiple" runat="server" TabIndex="2" ClientInstanceName="gridindustryLookup"
                        OnDataBinding="lookup_industry_DataBinding" 
                        KeyFieldName="ID" Width="100%" TextFormatString="{1}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                        <columns>

                                        <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="5" Caption=" " />

                                        <dxe:GridViewDataColumn FieldName="ID" Visible="true" VisibleIndex="1" Caption="ID" width="0">
                                            <Settings AutoFilterCondition="Contains" />
                                               </dxe:GridViewDataColumn>
                                        <dxe:GridViewDataColumn FieldName="IndName" Visible="true" VisibleIndex="2" Caption="Industry" Width="40" Settings-AutoFilterCondition="Contains">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataColumn>
                                  



                                    </columns>
                        <gridviewproperties settings-verticalscrollbarmode="Auto" settingspager-mode="ShowAllRecords">
                                        <Templates>
                                            <StatusBar>
                                                <table class="OptionsTable" style="float: right">
                                                    <tr>
                                                        <td>

                                                         <dxe:ASPxButton ID="ASPxButton3" runat="server" AutoPostBack="false" Text="Close"  ClientSideEvents-Click="CloselookupIndustry" UseSubmitBehavior="False" />

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
                                    </gridviewproperties>

                    </dxe:ASPxGridLookup>
                                </dxe:PanelContent>
                    </panelcollection>
                    </dxe:ASPxCallbackPanel>



                    <span id="MandatoryCustomerType" style="display: none" class="validclass">
                        <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor1112_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>


                </div>

            </div>
             <div class="col-md-3">
                   <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                        <asp:Label ID="Label2" runat="Server" Text="Class : " CssClass="mylabel1"
                            Width="110px"></asp:Label>
                    </div>
                    <div>
                        <dxe:ASPxButtonEdit ID="txtClass" runat="server" ReadOnly="true" ClientInstanceName="ctxtClass" Width="100%" TabIndex="4">
                            <Buttons>
                                <dxe:EditButton>
                                </dxe:EditButton>
                            </Buttons>
                            <ClientSideEvents ButtonClick="function(s,e){ClassButnClick();}" KeyDown="function(s,e){Class_KeyDown(s,e);}" />
                        </dxe:ASPxButtonEdit>
                    </div>
             </div>

            <div class="col-md-3">

             <%--   <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="Label4" runat="Server" Text="Product : " CssClass="mylabel1" Width="110px"></asp:Label>
                </div>

                <div>
                    <dxe:ASPxCallbackPanel runat="server" ID="cpproduct" ClientInstanceName="cproductPanel" OnCallback="Product_Callback">
                        <panelcollection>
                        <dxe:PanelContent runat="server">

                    <dxe:ASPxGridLookup ID="lookup_product" SelectionMode="Multiple" runat="server" TabIndex="3" ClientInstanceName="gridproductLookup"
                        OnDataBinding="lookup_product_DataBinding"  
                        KeyFieldName="ID" Width="100%" TextFormatString="{1}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                        <columns>

                                        <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="20" Caption=" " />
                            
                                        <dxe:GridViewDataColumn FieldName="ID" Visible="true" VisibleIndex="1" Caption="ID" width="0">
                                            <Settings AutoFilterCondition="Contains" />
                                               </dxe:GridViewDataColumn>
                                        <dxe:GridViewDataColumn FieldName="ProdName" Visible="true" VisibleIndex="2" Caption="Product Name" Width="180" Settings-AutoFilterCondition="Contains">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataColumn>
                                    

                                    </columns>
                        <gridviewproperties settings-verticalscrollbarmode="Auto" settingspager-mode="ShowAllRecords">
                                        <Templates>
                                            <StatusBar>
                                                <table class="OptionsTable" style="float: right">
                                                    <tr>
                                                        <td>

                                                           
                                                            <dxe:ASPxButton ID="ASPxButton6" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloselookupProducts" UseSubmitBehavior="False" />

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
                                    </gridviewproperties>
                    </dxe:ASPxGridLookup>
                                    
                            
                    </dxe:PanelContent>
                    </panelcollection>
                    </dxe:ASPxCallbackPanel>

                    <asp:HiddenField ID="HiddenField1" runat="server" />


                    <span id="MandatoryCustomerType" style="display: none" class="validclass">
                        <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor1112_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>

                </div>--%>

                <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                        <asp:Label ID="Label4" runat="Server" Text="Product : " CssClass="mylabel1"
                            ></asp:Label>
                </div>
                <div>
                    <dxe:ASPxButtonEdit ID="txtProdName" runat="server" ReadOnly="true" ClientInstanceName="ctxtProdName" Width="100%" TabIndex="5">
                        <Buttons>
                            <dxe:EditButton>
                            </dxe:EditButton>
                        </Buttons>
                        <ClientSideEvents ButtonClick="function(s,e){ProductButnClick();}" KeyDown="function(s,e){Product_KeyDown(s,e);}" />
                    </dxe:ASPxButtonEdit>
                </div>

            </div>


           

            <div class="col-md-3" style="padding-top: 13px">
                <button id="btnShow" class="btn btn-primary" type="button" onclick="btn_ShowRecordsClick(this);" tabindex="4" >Show</button>
                <%-- <% if (rights.CanExport)
                           { %>--%>

                <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" tabindex="5"
                    AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                    <asp:ListItem Value="0">Export to</asp:ListItem>
                    <asp:ListItem Value="1">PDF</asp:ListItem>
                    <asp:ListItem Value="2">XLSX</asp:ListItem>
                    <asp:ListItem Value="3">RTF</asp:ListItem>
                    <asp:ListItem Value="4">CSV</asp:ListItem>

                </asp:DropDownList>
                <%-- <% } %>--%>
            </div>


        </div>



        <div class="pull-right">
        </div>
        <table class="TableMain100">
            <tr>
                <td >
                    <div>
                        <dxe:ASPxGridView runat="server" ID="ShowGrid" ClientInstanceName="Grid" Width="100%" EnableRowsCache="false" AutoGenerateColumns="True"
                          OnDataBound="ShowGrid_DataBound" OnCustomCallback="Grid_CustomCallback"
                            OnDataBinding="ShowGrid_DataBinding">



                            <%--  
                                 <columns>
                                  <dxe:GridViewDataTextColumn FieldName="Customer" Caption="Customer Name" VisibleIndex="2"/>

                                <dxe:GridViewDataTextColumn FieldName="Qty_Permonth" Caption="Budget Details" VisibleIndex="5" />
                                <dxe:GridViewDataTextColumn FieldName="January" Caption="January" VisibleIndex="6" />
                                <dxe:GridViewDataTextColumn FieldName="February" Caption="February" VisibleIndex="7"/>
                                <dxe:GridViewDataTextColumn FieldName="March" Caption="March" VisibleIndex="8"/>
                                <dxe:GridViewDataTextColumn FieldName="April" Caption="April" VisibleIndex="9"/>
                                <dxe:GridViewDataTextColumn FieldName="May" Caption="May" VisibleIndex="10" />
                                <dxe:GridViewDataTextColumn FieldName="June" Caption="June" VisibleIndex="11" >
                                      
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="July" Caption="July" VisibleIndex="12">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="August" Caption="August" VisibleIndex="13"  >
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="September" Caption="September" VisibleIndex="14" >
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="October" Caption="October" VisibleIndex="15">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="November" Caption="November" VisibleIndex="16">
                                </dxe:GridViewDataTextColumn>
                                   
                                <dxe:GridViewDataTextColumn FieldName="December" Caption="December" VisibleIndex="17" >
                                </dxe:GridViewDataTextColumn>
                                  </columns>
                            --%>

                            <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" />
                            <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
                            <SettingsEditing Mode="EditForm" />
                            <SettingsContextMenu Enabled="true" />
                            <SettingsBehavior AutoExpandAllGroups="true" ColumnResizeMode="Control" />
                            <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" />
                            <SettingsSearchPanel Visible="false" />
                            <SettingsPager PageSize="10">
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />

                            </SettingsPager>
                            <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />

          
                        </dxe:ASPxGridView>

                    </div>
                </td>
            </tr>
        </table>
    </div>

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

                            <tr class="HeaderStyle" style="font-size:small">
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
                    <button type="button" class="btnOkformultiselection btn-default"  onclick="DeSelectAll('ProductSource')">Deselect All</button>
                    <button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal" onclick="OKPopup('ProductSource')">OK</button>
                    <%--<button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal">Close</button>--%>
                </div>
            </div>
        </div>
    </div>
         <asp:HiddenField ID="hdncWiseProductId" runat="server" />
    
    <!--Product Modal -->

    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>

    <input type="hidden" id="hdnsalesman" />
    <input type="hidden" id="hdnIndustry" />
    <input type="hidden" id="hdnProducts" />

</asp:Content>

