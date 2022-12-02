<%@ Page Title="UOM" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.Master.management_master_changeunit" CodeBehind="changeunit.aspx.cs" EnableEventValidation="false" %>


<%--<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe000001" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/assests/pluggins/choosen/choosen.min.js"></script>
    <script language="javascript" type="text/javascript">

        $(document).ready(function () {
            ListBind();
            ChangeSource();

        });
        function ListBind() {

            var config = {
                '.chsn': {},
                '.chsn-deselect': { allow_single_deselect: true },
                '.chsn-no-single': { disable_search_threshold: 10 },
                '.chsn-no-results': { no_results_text: 'Oops, nothing found!' },
                '.chsn-width': { width: "100%" }
            }
            for (var selector in config) {
                $(selector).chosen(config[selector]);
            }

        }
        function lstconverttounit() {

            $('#lstconverttounit').fadeIn();

        }

        function FunCallAjaxList(objID, objEvent, ObjType) {
            //alert(ObjType);
            var strQuery_Table = '';
            var strQuery_FieldName = '';
            var strQuery_WhereClause = '';
            var strQuery_OrderBy = '';
            var strQuery_GroupBy = '';
            var CombinedQuery = '';

            if (ObjType == 'ProductFo') {

                strQuery_Table = "Master_uom";
                strQuery_FieldName = "distinct top 10 (isnull(Uom_Name,''))+'[' + Uom_Shortname+']',Uom_id";

                strQuery_WhereClause = "  ( Uom_Name like (\'%RequestLetter%') or Uom_Shortname like (\'%RequestLetter%') )";

            }
            CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);

            ajax_showOptions(objID, 'GenericAjaxList', objEvent, replaceChars(CombinedQuery));

        }
        function ChangeSource() {
            var fname = "%";
            var lconverttounit = $('select[id$=lstconverttounit]');
            lconverttounit.empty();


            $.ajax({
                type: "POST",
                url: "changeunit.aspx/GetUOM",
                data: JSON.stringify({ reqStr: fname }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var list = msg.d;
                    var listItems = [];
                    if (list.length > 0) {

                        for (var i = 0; i < list.length; i++) {
                            var id = '';
                            var name = '';
                            id = list[i].split('|')[1];
                            name = list[i].split('|')[0];

                            $('#lstconverttounit').append($('<option>').text(name).val(id));

                        }

                        $(lconverttounit).append(listItems.join(''));

                        lstconverttounit();
                        $('#lstconverttounit').trigger("chosen:updated");

                        Changeselectedvalue();


                    }
                    else {
                        //   alert("No records found");
                        //lstReferedBy();
                        $('#lstconverttounit').trigger("chosen:updated");

                    }
                }
            });
            // }
        }
        function lstconverttounit() {

            $('#lstconverttounit').fadeIn();

        }
        function setvalue() {

            if ($('#hdAddEdit').val() == "Edit") {
                document.getElementById("txtconverttounit_hidden").value = document.getElementById("lstconverttounit").value;
                if (document.getElementById("txtconverttounit_hidden").value != '') {

                    return false;
                }
            }
            else {
                //var ReturnValue = true;
                if (ctxtUnit.GetText().trim() == "") {
                    // $('#MandatoryUnit').show();
                    return false;
                } else {
                    $('#RequiredFieldValidator1').hide();
                }

                if (ctxtShortUnit.GetText().trim() == "") {
                    // $('#MandatoryShortUnit').show();
                    return false;
                } else {
                    //$('#MandatoryShortUnit').hide();
                }
                if (ctxtUseFor.GetText().trim() == "") {
                    //  $('#MandatoryUseFor').show();
                    return false;
                } else {
                    // $('#MandatoryUseFor').hide();
                }
            }
            //return ReturnValue;
        }

        function Changeselectedvalue() {
            var lstconverttounit = document.getElementById("lstconverttounit");
            if (document.getElementById("txtconverttounit_hidden").value != '') {

                for (var i = 0; i < lstconverttounit.options.length; i++) {
                    if (lstconverttounit.options[i].value == document.getElementById("txtconverttounit_hidden").value) {
                        lstconverttounit.options[i].selected = true;
                    }
                }
                $('#lstconverttounit').trigger("chosen:updated");
            }

        }

        function replaceChars(entry) {
            out = "+"; // replace this
            add = "--"; // with this
            temp = "" + entry; // temporary holder

            while (temp.indexOf(out) > -1) {
                pos = temp.indexOf(out);
                temp = "" + (temp.substring(0, pos) + add +
                temp.substring((pos + out.length), temp.length));
            }
            return temp;
        }
        function CallAjax(obj1, obj2, obj3) {

            // FieldName='ctl00_ContentPlaceHolder1_Headermain1_cmbCompany';
            ajax_showOptions(obj1, obj2, obj3);
            //alert (ajax_showOptions);
        }


        FieldName = 'abcd';
        //function SignOff() {
        //    window.parent.SignOff();
        //}


        //function height() {
        //    if (document.body.scrollHeight >= 300)
        //        window.frameElement.height = document.body.scrollHeight;
        //    else
        //        window.frameElement.height = '300px';
        //    window.frameElement.Width = document.body.scrollWidth;
        //}

        function FillValues(obj) {
            parent.editwin.close(obj);



        }

        function Changestatus() {
            var URL = "frm_UOM.aspx";
            window.location.href = URL;
        }

    </script>
    <style>
        #rfvComname {
            position: absolute;
            right: 80px;
            top: 8px;
        }

        .chosen-container.chosen-container-single {
            width: 100% !important;
        }

        .chosen-choices {
            width: 100% !important;
        }

        #lstconverttounit {
            width: 200px;
        }

        #lstconverttounit {
            display: none !important;
        }

        #lstconverttounit_chosen {
            width: 100% !important;
        }

        .ctcclass {
            position: absolute;
            top: 10px;
            right: -16px;
        }

        .Unitclass {
            position: absolute;
            top: 65px;
            right: 854px;
        }

        .UnitShortclass {
            position: absolute;
            top: 95px;
            right: 854px;
        }

        .Unituseclass {
            position: absolute;
            top: 127px;
            right: 854px;
        }
    </style>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>
                <span class="">
                    <asp:Label ID="lblHeading" runat="server" Text="Add Units Of Measurement [UOM]"></asp:Label>
                </span>
            </h3>
            <div class="crossBtn"><a href="frm_UOM.aspx"><i class="fa fa-times"></i></a></div>
        </div>
    </div>
    <div class="form_main">
        <%--  <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="3600">
        </asp:ScriptManager>--%>
        <%--        <table class="TableMain100">
            <tr>
                <td class="EHEADER" style="text-align: center;">
                    <strong><span style="color: #000099">Change Unit</span></strong>
                </td>
                <td style="text-align: right">
                    <div class="crossBtn"><a href="frm_UOM.aspx"><i class="fa fa-times"></i></a></div>
                </td>
            </tr>
        </table>--%>
        <%--<table width="400px" align="center" style="border: solid 1px white;">
                
            </table>--%>
        <table class="">
            <tr id="trunit" runat="server">
                <td style="width: 112px; text-align: left;">
                    <label style="margin-top: 8px">Unit Name<span style="color: red">*</span></label>
                </td>
                <td style="text-align: left;">

                    <dxe:ASPxTextBox runat="server" ID="txtUnit" ClientInstanceName="ctxtUnit">
                    </dxe:ASPxTextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="a" runat="server" ControlToValidate="txtUnit" Display="Dynamic"
                        CssClass="fa fa-exclamation-circle Unitclass " ToolTip="Mandatory."
                        ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr id="trshort" runat="server">
                <td style="width: 112px; text-align: left;">
                    <label style="margin-top: 8px">Short Name<span style="color: red">*</span></label>
                </td>
                <td style="text-align: left;">

                    <dxe:ASPxTextBox runat="server" ID="txtShortUnit" ClientInstanceName="ctxtShortUnit">
                    </dxe:ASPxTextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup="a" runat="server" ControlToValidate="txtShortUnit" Display="Dynamic"
                        CssClass="fa fa-exclamation-circle UnitShortclass " ToolTip="Mandatory."
                        ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                </td>

                
            </tr>
            <tr id="truseFor" runat="server">
                <td style="width: 112px; text-align: left;">
                    <label style="margin-top: 8px">Use For<span style="color: red">*</span></label>
                </td>
                <td style="text-align: left;">

                    <dxe:ASPxTextBox runat="server" ID="txtUseFor" ClientInstanceName="ctxtUseFor">
                    </dxe:ASPxTextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup="a" runat="server" ControlToValidate="txtUseFor" Display="Dynamic"
                        CssClass="fa fa-exclamation-circle Unituseclass " ToolTip="Mandatory."
                        ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                </td>

            </tr>
            <tr id="trExistingUnit" runat="server">
                <td style="width: 112px; text-align: left;">
                    <span id="Span2" class="Ecoheadtxt" style="text-align: right;">Existing Unit</span>
                </td>
                <td class="Ecoheadtxt" style="text-align: left; height: 37px; width: 300px">
                    <strong><span id="litSegment" runat="server" style="color: black"></span></strong>
                </td>
            </tr>
            <tr id="trChangeTo" runat="server">
                <td style="width: 80px; text-align: left;">
                    <span id="Span1" class="Ecoheadtxt" style="text-align: right;">Change to</span> <span style="color: red;">*</span>
                </td>
                <td class="Ecoheadtxt relative" style="text-align: left; height: 37px; width: 256px; position: relative">
                    <%-- <asp:TextBox runat="server" Width="200px" ID="txtproduct"></asp:TextBox>--%>
                    <asp:TextBox
                        ID="txtproduct_hidden" runat="server" Width="14px" Style="display: none">
                    </asp:TextBox>
                    <asp:HiddenField ID="txtconverttounit_hidden" runat="server" />
                    <asp:ListBox ID="lstconverttounit" CssClass="chsn" runat="server" Width="100%" TabIndex="8" data-placeholder="Select..."></asp:ListBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" ValidationGroup="a" runat="server" ControlToValidate="lstconverttounit" Display="Dynamic"
                        CssClass="fa fa-exclamation-circle ctcclass " ToolTip="Mandatory."
                        ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <table>
                        <tr>
                            <td align="left" id="td_yes" runat="server" style="width: 40px;">
                                <asp:Button ID="btnYes" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnYes_Click" OnClientClick=" setvalue()" ValidationGroup="a" />
                            </td>
                            <td align="left" id="td_no" runat="server" style="width: 40px;">
                                <asp:Button ID="btnNo" runat="server" CssClass="btn btn-danger" Text="Cancel"
                                    OnClick="btnNo_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField ID="HiddenField1" runat="server" />
    <asp:HiddenField ID="hdAddEdit" runat="server" />
</asp:Content>
