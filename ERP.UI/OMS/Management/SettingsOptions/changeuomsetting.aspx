<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/popup.Master"
    Inherits="ERP.OMS.Management.SettingsOptions.management_SettingsOptions_changeuomsetting" Codebehind="changeuomsetting.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    


    <style type="text/css">
	
	/* Big box with list of options */
	#ajax_listOfOptions{
		position:absolute;	/* Never change this one */
		width:50px;	/* Width of box */
		height:auto;	/* Height of box */
		overflow:auto;	/* Scrolling features */
		border:1px solid Blue;	/* Blue border */
		background-color:#FFF;	/* White background color */
		text-align:left;
		font-size:0.9em;
		z-index:32767;
	}
	#ajax_listOfOptions div{	/* General rule for both .optionDiv and .optionDivSelected */
		margin:1px;		
		padding:1px;
		cursor:pointer;
		font-size:0.9em;
	}
	#ajax_listOfOptions .optionDiv{	/* Div for each item in list */
		
	}
	#ajax_listOfOptions .optionDivSelected{ /* Selected item in the list */
		background-color:#DDECFE;
		color:Blue;
	}
	#ajax_listOfOptions_iframe{
		background-color:#F00;
		position:absolute;
		z-index:3000;
	}
	
	form{
		display:inline;
	}
	.chosen-drop {
        max-height:150px;
        overflow:hidden;
	}

	</style>
    <script src="/assests/pluggins/choosen/choosen.min.js"></script>
    
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            debugger;
            ListBind();
            $('#txtBankName').trigger("chosen:updated");
            $('#txttoname').trigger("chosen:updated");
            //SetBankNames(cntry);
        });
        function OnCloseButtonClick(s, e) {

            var parentWindow = window.parent;
            parentWindow.grid.PerformCallback();
            parentWindow.popup.Hide();

        }
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
        function FunCallAjaxList(objID, objEvent, ObjType) {
            //alert(ObjType);
            var strQuery_Table = '';
            var strQuery_FieldName = '';
            var strQuery_WhereClause = '';
            var strQuery_OrderBy = '';
            var strQuery_GroupBy = '';
            var CombinedQuery = '';
            var alert3 = (document.getElementById('txttoname').value);
            var alert4 = (document.getElementById('txtfromname').value);

            var alert1 = (document.getElementById('txttoname_hidden').value);
            var alert2 = (document.getElementById('txtfromname_hidden').value);

            if (alert3 == "") {
                alert1 = ""
            }
            if (alert4 == "") {
                alert2 = ""
            }
            if (ObjType == 'ProductFo') {

                strQuery_Table = "Master_uom";
                strQuery_FieldName = "distinct top 10 (isnull(Uom_Name,''))+'[' + Uom_Shortname+']',Uom_id";
                if (alert1 != "") {
                    //                     alert ('1');
                    strQuery_WhereClause = "  ( Uom_Name like (\'%RequestLetter%') or Uom_Shortname like (\'%RequestLetter%')) and Uom_id not in ('" + document.getElementById('txttoname_hidden').value + "')";
                }
                else {
                    //alert ('2');
                    strQuery_WhereClause = "  ( Uom_Name like (\'%RequestLetter%') or Uom_Shortname like (\'%RequestLetter%') )";
                }

            }
            if (ObjType == 'ProductFo1') {

                strQuery_Table = "Master_uom";
                strQuery_FieldName = "distinct top 10 (isnull(Uom_Name,''))+'[' + Uom_Shortname+']',Uom_id";

                if (alert2 != "") {
                    //alert ('3')
                    strQuery_WhereClause = "  ( Uom_Name like (\'%RequestLetter%') or Uom_Shortname like (\'%RequestLetter%')) and Uom_id not in ('" + document.getElementById('txtfromname_hidden').value + "') ";
                }
                else {
                    //alert ('4');
                    strQuery_WhereClause = "  ( Uom_Name like (\'%RequestLetter%') or Uom_Shortname like (\'%RequestLetter%')  )";
                }

            }


            CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);

            ajax_showOptions(objID, 'GenericAjaxList', objEvent, replaceChars(CombinedQuery));
            //alert (CombinedQuery);

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

        //Done By:Subhabrata@ For Validation Purpose
        function Check() {
            
            var UOMFrom = document.getElementById('txtfromname');
            var UOMTo = document.getElementById('txttoname');
            var txtproduct = document.getElementById('txtproduct').value;
            var tLength_UOMFrom = UOMFrom.length;
            var tLength_UOMTo = UOMTo.length;



            var count_UOMFrom = 0;
            for (i = 0; i < tLength_UOMFrom; i++) {
                if (UOMFrom.options[i].selected == true) {
                    count_UOMFrom++;
                }

            }
            if (count_UOMFrom > 0) {
                $('#Mandatory_txtfromname').css({ 'display': 'none' });
            }
            else {
                $('#Mandatory_txtfromname').css({ 'display': 'block' });
                return false;
            }

            var count_UOMTo = 0;
            for (i = 0; i < tLength_UOMFrom; i++) {
                if (UOMFrom.options[i].selected == true) {
                    count_UOMTo++;
                }

            }
            if (count_UOMTo > 0) {
                $('#MandatoryFileNo').css({ 'display': 'none' });
            }
            else {
                $('#MandatoryFileNo').css({ 'display': 'block' });
                return false;
            }

            if (txtproduct.trim().length == 0) {
                $('#Mandatory_txtProduct').css({ 'display': 'block' });
                return false;
            }
            else {
                $('#Mandatory_txtProduct').css({ 'display': 'none' });
            }

        }
        //End
  

FieldName='none';
function SignOff() {
    window.parent.SignOff();
}
    
      
function displayrate() {
    document.getElementById("trrate").style.display = "inline";
}
function noNumbers(e) {
    //alert (e);
    var keynum
    var keychar
    var numcheck

    if (window.event)//IE
    {
        keynum = e.keyCode
        if (keynum >= 48 && keynum <= 57 || keynum == 46) {
            return true;
        }
        else {
            alert("Please Insert Numeric Only");
            return false;
        }
        //alert (keynum);
    }

    else if (e.which) // Netscape/Firefox/Opera
    {
        keynum = e.which

        if (keynum >= 48 && keynum <= 57 || keynum == 46) {
            return true;
        }
        else {
            alert("Please Insert Numeric Only");
            return false;
        }
    }
}
    
    
function FillValues(obj) {
    parent.editwin.close(obj);



}
    
     
      
    

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div>
      <%--      <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>--%>
            
            <%--<table width="400px" align="center" style="border: solid 1px white;">
                
            </table>--%>
            <table width="400px" align="center">
            <tr id="tr_11" runat="server">
                    <td>
                        <table width="100%">
                            <td style="width: 110px; text-align: left;">
                                <span id="Span4" class="Ecoheadtxt" style="text-align: right;">From Name</span>
                            </td>
                             <td class="Ecoheadtxt relative" style="text-align: left; height: 37px; width: 290px">
                                <%--<asp:TextBox runat="server" Width="250px" ID="txtfromname"></asp:TextBox>--%>
                                 <asp:ListBox ID="txtfromname" CssClass="chsn"   runat="server" Width="100%"   
                                        data-placeholder="Select..." ></asp:ListBox>
                                 <span id="Mandatory_txtfromname" class="pullleftClass fa fa-exclamation-circle iconRed" style="color:red; position:absolute;right:-15px;top:10px;display:none" title="Mandatory"></span>
                                 <asp:TextBox
                                    ID="txtfromname_hidden" runat="server" Width="14px" Style="display: none">
                                </asp:TextBox>
                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtfromname"
                            ErrorMessage="Required." ValidationGroup="a"></asp:RequiredFieldValidator>--%>
                            </td>
                        </table>
                    </td>
                </tr>
                
                <tr id="tr_12" runat="server">
                    <td>
                        <table width="100%">
                            <td style="width: 110px; text-align: left;">
                                <span id="Span6" class="Ecoheadtxt" style="text-align: right;">To Name</span>
                            </td> 
                             <td class="Ecoheadtxt relative" style="text-align: left; height: 37px; width: 290px">
                                <%--<asp:TextBox runat="server" Width="250px" ID="txttoname"></asp:TextBox>--%>
                                 <asp:ListBox ID="txttoname" CssClass="chsn"   runat="server" Width="100%"   
                                        data-placeholder="Select..." ></asp:ListBox>
                                 <asp:TextBox
                                    ID="txttoname_hidden" runat="server" Width="14px" Style="display: none">
                                </asp:TextBox>
                                 <span id="Mandatory_txtToname" class="pullleftClass fa fa-exclamation-circle iconRed" style="color:red; position:absolute;right:-18px;top:10px;display:none" title="Mandatory"></span>
                                 <%--<asp:TextBox--%>
                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txttoname"
                            ErrorMessage="Required." ValidationGroup="a"></asp:RequiredFieldValidator>--%>
                            </td>
                        </table>
                    </td>
                </tr>
                <tr id="tr_1" runat="server">
                    <td>
                        <table width="100%">
                            <td style="width: 110px; text-align: left;">
                                <span id="Span2" class="Ecoheadtxt" style="text-align: right;">Existing From Name</span>
                            </td>
                            <td class="Ecoheadtxt" style="text-align: left; height: 37px; width: 290px">
                                <strong><span id="litSegment" runat="server" style="color: Maroon"></span></strong>
                            </td>
                        </table>
                    </td>
                </tr>
                <tr id="tr_2" runat="server">
                    <td>
                        <table width="100%">
                            <td style="width: 110px; text-align: left;">
                                <span id="Span3" class="Ecoheadtxt" style="text-align: right;">Existing To Name</span>
                            </td>
                            <td class="Ecoheadtxt" style="text-align: left; height: 37px; width: 290px">
                                <strong><span id="litSegment1" runat="server" style="color: Maroon"></span></strong>
                            </td>
                        </table>
                    </td>
                </tr>
                <tr id="tr_3" runat="server">
                    <td>
                        <table width="100%">
                            <td style="width: 110px; text-align: left;">
                                <span id="Span5" class="Ecoheadtxt" style="text-align: right;">Existing Multiplier</span>
                            </td>
                            <td class="Ecoheadtxt" style="text-align: left; height: 37px; width: 290px">
                                <strong><span id="litSegment2" runat="server" style="color: Maroon"></span></strong>
                            </td>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="100%">
                            <td style="width: 110px; text-align: left;">
                                <span id="Span1" class="Ecoheadtxt" style="text-align: right;">Insert Multiplier :</span>
                            </td>
                            <td class="Ecoheadtxt relative" style="text-align: left; height: 37px;">
                                <asp:TextBox runat="server" Width="99%" ID="txtproduct" onkeypress="return noNumbers(event)"></asp:TextBox>
                                <span id="Mandatory_txtProduct" class="pullleftClass fa fa-exclamation-circle iconRed" style="color:red; position:absolute;right:-15px;top:10px;display:none" title="Mandatory"></span>
                                <asp:TextBox
                                    ID="txtproduct_hidden" runat="server" Width="14px" Style="display: none">
                                </asp:TextBox>
                            </td>
                        </table>
                    </td>
                </tr>
                <tr style="height: 10px;">
                    <td>
                        
                    </td>
                </tr>
                <tr>
                    <td>
                        <div style="padding-left: 110px;">
                            <span id="td_yes" runat="server">
                                <asp:Button ID="btnYes" runat="server" CssClass="btn btn btn-primary" OnClientClick="return Check();" Text="Save"  OnClick="btnYes_Click"
                                     />
                            </span>
                            <span id="td_no" runat="server">
                                <asp:Button ID="btnNo" runat="server" CssClass="btn btn btn-danger" Text="Cancel"
                                    OnClick="btnNo_Click" />
                            </span>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <asp:HiddenField ID="HiddenField1" runat="server" />
</asp:Content>