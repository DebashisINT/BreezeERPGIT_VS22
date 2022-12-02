<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.Master.management_master_frm_FinancialYearAdd" CodeBehind="frm_FinancialYearAdd.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe000001" %>--%>




    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            var MaxLength = 100;
            $('#txtRemarks').keypress(function (e) {
                if ($(this).val().length >= MaxLength) {
                    e.preventDefault();
                }
            });
        });

        function numbersonly(myfield, e) {
            var key;
            var keychar;

            if (window.event)
                key = window.event.keyCode;
            else if (e)
                key = e.which;
            else
                return true;

            keychar = String.fromCharCode(key);

            // control keys
            if ((key == null) || (key == 0) || (key == 8) || (key == 9) || (key == 13) || (key == 27))
                return true;

                // numbers
            else if ((("0123456789").indexOf(keychar) > -1))
                return true;

                // only one decimal point
            else if ((keychar == ".")) {
                if (myfield.value.indexOf(keychar) > -1)
                    return false;
            }
            else
                return false;
        }

        function ValidatePage() {
            var returnValue = true;

            if (document.getElementById("txtFinYear").value.trim() == '') {
                $('#redtxtFinYear').css({ 'display': 'block' });               
                returnValue = false;
            } else {
                $('#redtxtFinYear').css({ 'display': 'none' });
            }
            if (document.getElementById("txtFinYear1").value.trim() == '') {
                $('#redtxtFinYear1').css({ 'display': 'block' });
                returnValue = false;
            } else {
                $('#redtxtFinYear1').css({ 'display': 'none' });
            }

            if (document.getElementById("txtFinYear").value.trim().length < 4) {
                jAlert('Enter a Valid Financial year');
                return false;
            }

            if (document.getElementById("txtFinYear1").value.trim().length < 4) {
                jAlert('Enter a Valid Financial year');
                return false;
            } 

           
            if (document.getElementById("txtFinYear").value.trim() != '' && document.getElementById("txtFinYear1").value.trim() != '') {
                
                var Year1 = document.getElementById("txtFinYear").value;
                var Year2 = document.getElementById("txtFinYear1").value;
                var Diff = parseInt(Year2) - parseInt(Year1)               
                if (Diff != 1) {
                    jAlert('Enter a Valid Financial year');
                    return false;
                }
            }


            if (txtStart.GetText() == '01-01-0100' || txtStart.GetText() == '01-01-1900') {
                $('#redtxtStart').css({ 'display': 'block' });
                // $("#redtxtStart").removeClass("hide");
                returnValue = false;
            } else {
                $('#redtxtStart').css({ 'display': 'none' });
            }

            if (txtEnd.GetText() == '01-01-0100' || txtEnd.GetText() == '01-01-1900') {

                $('#redtxtEnd').css({ 'display': 'block' });
                // $("#redtxtFinYear").removeClass("hide");
                returnValue = false;
            } else {
                $('#redtxtEnd').css({ 'display': 'none' });
            }

            return returnValue;
        }
        function Close() {
            // parent.editwin.close();
            window.location.href = "frm_FinancialYear.aspx";
        }
    </script>
    <style>
        .nestedinput {
            padding: 0;
            margin: 0;
        }
        .nestedinput li {
            list-style-type: none;
            display: inline-block;
            float: left;
        }
        .nestedinput li.dash {
            width: 26px;
            text-align: center;
            padding: 6px;
        }
        table.pad>tbody>tr>td {
            padding:5px 0px !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3><%Response.Write((Request.QueryString["id"] != null ? Request.QueryString["id"].ToString() : "").ToString()); %> Finacial Year</h3>
            <div class="crossBtn"><a href="frm_FinancialYear.aspx"><i class="fa fa-times"></i></a></div>
        </div>
    </div>
    <div class="form_main">
        <table width="500px" class="pad">
            <%--            <tr>
                <td colspan="2">
                    <h3><%Response.Write((Request.QueryString["id"] != null ? Request.QueryString["id"].ToString() : "").ToString()); %> Finacial Year</h3>
                </td>
            </tr>--%>

            <tr>
                <td class="gridcellleft">
                    <span class="Ecoheadtxt">Financial Year:<span style="color: red">*</span></span>
                </td>
                <td class="gridcellleft">
               <ul class="nestedinput">
                   <li><asp:TextBox ID="txtFinYear" runat="server" Width="90px" MaxLength="4" TabIndex="1" placeholder="YYYY" onkeypress="return numbersonly(this, event)"></asp:TextBox></li>
                   <li class="dash"> - </li>
                   <li><asp:TextBox ID="txtFinYear1" runat="server" Width="90px" MaxLength="4" TabIndex="2" placeholder="YYYY" onkeypress="return numbersonly(this, event)"></asp:TextBox></li>
               </ul>                
                   
                    <%--<div style="text-align: right; width: 202PX;">(Ex.2016-2017)</div>--%>
                    <%--<div id="redtxtFinYear" class="red hide">Mandatory</div>--%>
                    
                    <span id="redtxtFinYear" class="pullleftClass fa fa-exclamation-circle iconRed " style="color:red; position:absolute;top: 62px;left: 400px;display:none" title="Mandatory/Invalid"></span>
                <span id="redtxtFinYear1" class="pullleftClass fa fa-exclamation-circle iconRed " style="color:red; position:absolute;top: 62px;left: 400px;display:none" title="Mandatory/Invalid"></span>
                </td>
            </tr>
            <tr>
                <td class="gridcellleft">
                    <span class="Ecoheadtxt">Start Date:<span style="color: red">*</span></span>
                </td>
                <td class="gridcellleft">
                    <dxe:ASPxDateEdit ID="txtStart" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                        TabIndex="3" Width="208px">
                        <buttonstyle width="13px">
                        </buttonstyle>
                    </dxe:ASPxDateEdit>
                    <%--<div id="redtxtStart" class="red hide">Mandatory</div>--%> 
                    <span id="redtxtStart" class="pullleftClass fa fa-exclamation-circle iconRed " style="color:red; position:absolute; top:118px; left: 400px; display:none" title="Mandatory"></span>
                </td>
            </tr>
            <tr>
                <td class="gridcellleft">
                    <span class="Ecoheadtxt">End Date:<span style="color: red">*</span></span>
                </td>
                <td class="gridcellleft">
                    <dxe:ASPxDateEdit ID="txtEnd" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                        TabIndex="4" Width="208px">
                        <buttonstyle width="13px">
                        </buttonstyle>
                    </dxe:ASPxDateEdit>
                    <%--<div id="redtxtEnd" class="red hide">Mandatory</div>--%>
                    <span id="redtxtEnd" class="pullleftClass fa fa-exclamation-circle iconRed " style="color:red; position:absolute; top: 155px;left: 400px;display:none" title="Mandatory"></span>
                </td>
            </tr>
            <tr>
                <td class="gridcellleft">
                    <span class="Ecoheadtxt">Remarks:</span>
                </td>
                <td class="gridcellleft">
                    <asp:TextBox TextMode="MultiLine" ID="txtRemarks" MaxLength="100" runat="server" Width="208px" TabIndex="5"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td></td>
                <td colspan="2" class="gridcellleft">
                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary btnUpdate" OnClick="btnSave_Click"
                        TabIndex="6" ValidationGroup="a" />
                  
                    <input type="button" id="btnCancel" value="Cancel" class="btn btn-danger btnUpdate" onclick="Close()" />
                   

                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField ID="hdFinYearOld" runat="server" />
</asp:Content>

