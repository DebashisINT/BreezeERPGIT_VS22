<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.Master.management_master_Addmember_usergroup" CodeBehind="Addmember_usergroup.aspx.cs" %>

<%--<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">--%>
<%--<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe000001" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   <%-- <link href="../../CSS/style.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="/assests/js/init.js"></script>

    <script type="text/javascript" src="/assests/js/ajaxList.js"></script>

    <script type="text/javascript" src="/assests/js/jquery-1.3.2.js"></script>--%>

    <script language="javascript" type="text/javascript">

        FieldName = 'abcd';      
      
        function Page_Load() {
            //document.getElementById('Div1').style.display="none";
        }
        function FillValues(obj) {
            parent.editwin.close(obj);


            //        var url='Member_usergroup.aspx?id='+obj;
            //       
            //        alert (obj);
            //        editwin=dhtmlmodal.open("Editbox", "iframe", url,"Add Member","width=540px,height=350px,center=1,resize=1,top=500", "recal");

            //alert (obj);
        }

        function FunCallAjaxList(objID, objEvent, ObjType) {

            var strQuery_Table = '';
            var strQuery_FieldName = '';
            var strQuery_WhereClause = '';
            var strQuery_OrderBy = '';
            var strQuery_GroupBy = '';
            var CombinedQuery = '';

            if (ObjType == 'Digital') {
                var alert1 = document.getElementById('HiddenField1').value;
                //alert(alert1);
                strQuery_Table = "tbl_master_user";
                strQuery_FieldName = "distinct top 10 USER_NAME+' ['+isnull(user_loginId,'')+']' AS loginid,user_id";
                strQuery_WhereClause = " user_group not like ('" + document.getElementById('HiddenField1').value + "') and ( USER_NAME like (\'%RequestLetter%') or user_loginId like (\'%RequestLetter%'))";
                // strQuery_WhereClause = " user_group not IN (\%52%') and ( USER_NAME like (\'%RequestLetter%') or user_loginId like (\'%RequestLetter%'))";

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

        //    function btn_Click()
        //    {
        //         document.getElementById('Div1').style.display="inline";
        //        combo.PerformCallback();
        //    }
        //    function ShowError(obj)
        //    {
        //        document.getElementById('Div1').style.display="none";
        //        if(obj=="b")
        //        {
        //            alert('Accounts Ledger Repost !!');
        //        }
        //        else
        //        {
        //            alert('No Data In This Company And Segment !!');
        //        }
        //         
        //    }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="3600">
        </asp:ScriptManager>
        <table class="TableMain100">
            <tr>
                <td class="EHEADER" style="text-align: center;">
                    <strong><span style="color: #000099">Add Member</span></strong>
                </td>
            </tr>
        </table>
        <%-- <div id='Div1' style='position:absolute; font-family:arial; font-size:30; left:40%; top:25%; background-color:white; layer-background-color:white; height:80; width:150;'>
                    <table class="TableMain100"> 
                      <tr><td><table><tr> 
                         <td height='25' align='center' bgcolor='#FFFFFF'> 
                           <img src='/assests/images/progress.gif' width='18' height='18'></td>  
                            <td height='10' width='100%' align='center' bgcolor='#FFFFFF'><font size='2' face='Tahoma'> 
 	                        <strong align='center'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Please Wait..</strong></font></td> 
                            </tr>  </table></td></tr>
                            </table> 
                    </div>--%>
        <table width="400px" align="center" style="border: solid 1px white;">
            <tr>
                <td>
                    <table>


                        <td style="width: 150px; text-align: right;">Member :</td>
                        <td style="width: 60%" align="left">
                            <asp:TextBox ID="txtReportTo" runat="server" Width="300px" onkeyup="FunCallAjaxList(this,event,'Digital')"></asp:TextBox>
                            <asp:TextBox ID="txtReportTo_hidden" runat="server" Width="100px"
                                Style="display: none"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtReportTo"
                                ErrorMessage="Required." ValidationGroup="a"></asp:RequiredFieldValidator>
                            <%--<asp:HiddenField ID="txtReportTo_hidden" runat="server" />--%>
                        
                  
                        </td>
                        <%--</tr>
    <tr>--%>
                    </table>
                </td>
            </tr>
            <tr style="height: 30px;">
            </tr>
            <tr>
                <td>
                    <table>

                        <td style="width: 120px;"></td>
                        <td align="left" id="td_yes" runat="server">
                            <asp:Button ID="btnYes" runat="server" CssClass="btn" Text="Save" Width="120px" OnClick="btnYes_Click" />
                        </td>
                        <td style="width: 120px;"></td>
                        <td align="left" id="td_no" runat="server">
                            <asp:Button ID="btnNo" runat="server" CssClass="btn" Text="Cancel" Width="120px" OnClick="btnNo_Click" />

                        </td>
                    </table>
                </td>
            </tr>


            <%-- <tr id="tr_time" runat="server">
   <%-- <td align="right">
    
    <asp:CheckBox ID="TIME" Checked="false" runat="server" />
    </td>--%>
            <%--<td class="gridcellleft" bgcolor="#B7CEEC">
                                            Report Type :</td>--%>
            <%--<td id="tr_time" runat="server" align="right" style="font-size:small; font-weight:bolder"> Run for how long :
                                            <asp:DropDownList ID="DdlRptType" runat="server" Width="100px" Font-Size="12px" >
                                                <asp:ListItem Value="300000">5 mint</asp:ListItem>
                                                <asp:ListItem Value="600000">10 mint</asp:ListItem>
                                                <asp:ListItem Value="1200000">20 mint</asp:ListItem>
                                                <asp:ListItem Value="1800000">30 mint</asp:ListItem>
                                                <asp:ListItem Value="3600000">1 hr</asp:ListItem>
                                                <asp:ListItem Value="7200000">2 hr</asp:ListItem>
                                                <asp:ListItem Value="18000000">5 hr</asp:ListItem>
                                                
                                                <asp:ListItem Value="864000000">24 hr</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
    </tr>--%>
            <%--<tr id="tr_timer">
    <asp:Label runat="server" ID="remaing" Text="Remaing Time" ></asp:Label>
    <asp:Literal ID="litTimerLabels" runat="server"></asp:Literal>
<input type="hidden" name="timerval" id="timerval" value=""/>
<div id="timepanel" style="font-weight:bold;font-size:12px;float:left;padding:2px 20px 0 0;color:Red;"></div>
</tr>--%>
        </table>


    </div>
    <asp:HiddenField ID="HiddenField1" runat="server" />

</asp:Content>
