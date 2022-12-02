<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="BranchWiseTDSeFile.aspx.cs" Inherits="Reports.Reports.GridReports.BranchWiseTDSeFile" %>

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
    </style>


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
        .bxStyle2 {
            background: #f9f9f9;
            border-radius: 6px;
            padding: 8px 0px;
            border: 1px solid #d6d6d6;
            }
        .padTbl>tbody>tr>td {
            padding:10px 0;
            padding-right:15px;
        }

        table#statementType td {
            width: 70px;
        }
    </style>

    <script type="text/javascript">
        $(function () {
            cBranchComponentPanel.PerformCallback('BindComponentGrid' + '~' + $("#ddlbranchHO").val());

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

    <script>
        $(document).ready(function () {
            $("#rdl_SaleInvoice").click(function () {

                if ($("#rdl_SaleInvoice_1").is(":checked")) {
                    $("#txt_tokenNo").attr("disabled", "disabled");
                    $("#txt_tokenNo").val('');

                } else {
                    $("#txt_tokenNo").removeAttr("disabled");
                    $("#txt_tokenNo").focus();
                }
            });

            if ($("#rdl_SaleInvoice_1").is(":checked")) {
                $("#txt_tokenNo").attr("disabled", "disabled");
                $("#txt_tokenNo").val('');
            }

        })

        function CustomerButnClick(s, e) {

        }

        function Validate() {
            var TokenNo = $("#txt_tokenNo").val();
            var EPeriod = $("#rdl_SaleInvoice input:checked").val();
            if ((TokenNo == '') && (EPeriod == 'Y')) {
                jAlert('Token no. of previous regular statement is blank. Cannot proceed');
                return false;
            }
            else {
                return true;
            }
        }

        $(function () {
            $("#ddl_FormNo").change(function () {
                var end = $("#ddl_FormNo").val();
                if (end == '24Q') {
                    $("#lblFormTxt").text('Whether regular statement for Form 24Q filed for earlier period?');
                    $("#lblTokenTxt").text('Token no. of previous regular statement (Form No. 24Q)');
                }
                else if (end == '26Q') {
                    $("#lblFormTxt").text('Whether regular statement for Form 26Q filed for earlier period?');
                    $("#lblTokenTxt").text('Token no. of previous regular statement (Form No. 26Q)');
                }
            });
        });

        function Showalert() {
            jAlert('There are no sufficient data to generate the eFile');
        }


        function ddlAssesYR_SelectedIndexChanged() {
            $("#hdnAssessmentValue").val($("#ddlAssesYR").val())
            $.ajax({
                type: "POST",
                url: 'BranchWiseTDSeFile.aspx/ddlAssesYR_SelectedIndexChanged',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                data: "{ddlAssesYR:\"" + $("#ddlAssesYR").val() + "\"}",
                success: function (response) {

                    var obj = response.d.split('~');
                    $("#txt_finyr").val(obj[0]);
                    $("#hdnFinValue").val(obj[1]);

                    //console.log(response);
                }
            });
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

    </script>
    <style>
        .devCheck>table>tbody>tr>td:not(:first-child){
            padding-left:22px;
        }
        .mBot0{
            margin-bottom:0px !important;
        }
        .fullReq {
            color: red;
            /* display: inline-block; */
            margin: 15px;
            width: 80%;
            background: #ffcaca;
            padding: 5px;
            border: 1px solid #d54949;
            font-weight: 500;
            font-size: 14px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="panel-heading">
    <div id="td_contact1" class="panel-title">
        <h3>
            <span id="lblHeadTitle">Branch wise eTDS File Generation</span>
        </h3>
    </div>

</div>
    <div class="form_main">
        <div class="clearfix bxStyle2">
            <div class="col-md-12 fullReq" runat="server" id="reqMesg" style="color:red; ">Please select atleast one branch for eTDS File Generation.</div>
            <div class="col-md-3 col-lg-2">
                <label style="color: #b5285f; font-weight: bold;" class="clsTo">Head Branch:</label>
                <div>
                    <asp:DropDownList ID="ddlbranchHO" runat="server" Width="100%"></asp:DropDownList>
                </div>
            </div>
            <div class="col-md-3 col-lg-2">
                <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="Label1" runat="Server" Text="Branch : " CssClass="mylabel1"></asp:Label></label>
                <dxe:ASPxCallbackPanel runat="server" ID="ComponentBranchPanel" ClientInstanceName="cBranchComponentPanel" OnCallback="Componentbranch_Callback">
                    <PanelCollection>
                        <dxe:PanelContent runat="server">
                            <dxe:ASPxGridLookup ID="lookup_branch" SelectionMode="Single" runat="server" ClientInstanceName="gridbranchLookup"
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
                                                            <dxe:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll" UseSubmitBehavior="False" />
                                                        </div>
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

                <span id="MandatoryActivityType" style="display: none" class="validclass">
                    <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor1112_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                <asp:HiddenField ID="hdnSelectedBranches" runat="server" />
            </div>
            <div class="col-md-2">
                <label>Form Number</label>
                <div class="relative">
                    <select name="ctl00$ContentPlaceHolder1$PageControl1$cmbBranchType" id="ddl_FormNo" style="width:100%;" runat="server">
			            <option value="24Q">24Q</option>
			            <option value="26Q">26Q</option>
			            <%--<option value="27Q">27Q</option>
			            <option value="27EQ">27EQ</option>--%>
		            </select>
                </div>
            </div>
            <div class="col-md-6">

                <div><label>Select Type of Statement to be Prepared</label></div>

                <div class="radio-inline devCheck">
                    <asp:RadioButtonList id="statementType" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Selected="True" Value="R">Regular</asp:ListItem>
                        <asp:ListItem Value="C">Correction</asp:ListItem>
                     </asp:RadioButtonList>
                </div>

            </div>
            <div class="clear"></div>
            <div class="col-md-2">
                <label>Assessment Year </label>
                <%--<div class="relative">                   
                    <asp:TextBox runat="server" ID="txt_assementyr" ReadOnly></asp:TextBox>
                </div>--%>
                 <div>
                    <asp:DropDownList ID="ddlAssesYR" runat="server" Width="100%" AutoPostBack="false" onchange="javascript:ddlAssesYR_SelectedIndexChanged()"></asp:DropDownList>
                </div>
            </div>
            <div class="col-md-2">
                <label>Financial Year </label>
                <div class="relative">

                    <asp:TextBox runat="server" ID="txt_finyr" ReadOnly></asp:TextBox>

                </div>
            </div>
            <div class="col-md-2">
                <label>Quarter </label>
                <div class="relative">
                    <select name="ctl00$ContentPlaceHolder1$PageControl1$cmbBranchType" id="ddl_QuaterType" style="width:100%;" runat="server">
			            <option value="Q1">Q1</option>
			            <option value="Q2">Q2</option>
			            <option value="Q3">Q3</option>
			            <option value="Q4">Q4</option>

		            </select>
                </div>
            </div>
            <div class="col-md-2">
                <label>Minor Head Challan </label>
                <div class="relative">
                    <select name="ctl00$ContentPlaceHolder1$PageControl1$cmbBranchType" id="ddl_HeadChallan" style="width:100%;" runat="server">
			            <option value="200">200</option>
			            <option value="400">400</option>
		            </select>
                </div>
            </div>
            <div class="clear"></div>
            <div class="col-md-8">
                <table class="padTbl" width="607px">
                    <tr>
                        <%--<td>Whether regular statement for Form 26Q filed for earlier period? </td>--%>
                        <td>
                            <asp:Label ID="lblFormTxt" runat="Server" Text="Whether regular statement for Form 24Q filed for earlier period?" Width="470px" ></asp:Label>
                        </td>
                        <td>
                        <div class="devCheck">
                            <asp:RadioButtonList ID="rdl_SaleInvoice" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                <asp:ListItem Text="No" Value="N" Selected="True"></asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                        
                        </td>
                    </tr>
                        <tr>
                            <%--<td>Token no. of previous regular statement (Form No. 26Q)</td>--%>
                            <td>
                                <asp:Label ID="lblTokenTxt" runat="Server" Text="Token no. of previous regular statement (Form No. 24Q)" Width="470px" ></asp:Label>
                            </td>
                            <td><input type="text" class="form-control" id="txt_tokenNo" runat="server" /></td>
                        </tr>
                </table>
            </div>

            <div class="clear"></div>
            <div class="col-md-12 mTop5">
                <asp:LinkButton ID="lnbGenerateFile" runat="server" OnClientClick="if (!Validate()) return false;" OnClick="lnbGenerateFile_Click" CssClass="btn btn-info">Generate eFile</asp:LinkButton>
                
                <button class="btn btn-danger ">Close</button>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnAssessmentValue" runat="server"/>
    <asp:HiddenField ID="hdnFinValue" runat="server"/>
    <asp:HiddenField ID="hdnDSColCount" runat="server"/>
</asp:Content>
