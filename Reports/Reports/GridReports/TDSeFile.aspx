<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="TDSeFile.aspx.cs" Inherits="Reports.Reports.GridReports.TDSeFile" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


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
            if ((TokenNo == '') && (EPeriod=='Y')) {
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
                url: 'TDSeFile.aspx/ddlAssesYR_SelectedIndexChanged',
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

    </script>
    <style>
        .devCheck>table>tbody>tr>td:not(:first-child){
            padding-left:22px;
        }
        .mBot0{
            margin-bottom:0px !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <div class="panel-heading">
    <div id="td_contact1" class="panel-title">
        <h3>
            <span id="lblHeadTitle">e TDS File Generation</span>
        </h3>
    </div>

</div>
    <div class="form_main">
        <div class="clearfix bxStyle2">
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



