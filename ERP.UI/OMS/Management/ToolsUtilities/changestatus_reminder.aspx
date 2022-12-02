<%@ Page Language="C#" AutoEventWireup="true" Inherits="ERP.OMS.Management.ToolsUtilities.management_utilities_changestatus_reminder" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Codebehind="changestatus_reminder.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
      <link href="../../CSS/style.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="/assests/js/init.js"></script>

    <script type="text/javascript" src="/assests/js/ajaxList.js"></script>

    <script type="text/javascript" src="/assests/js/jquery-1.3.2.js"></script>

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
	

	</style>
 <script type="text/javascript"> 

  FieldName='abcd';
//   function CallAjax(obj1,obj2,obj3)
//         { 
//        // alert ('1');
//            ajax_showOptions(obj1,obj2,obj3);
//         }
         // Fieldname = 'none'
    function SignOff()
    {
        window.parent.SignOff();
    }
      
//window.onload = autoStartTimer;
    function height()
    {
        if(document.body.scrollHeight>=300)
            window.frameElement.height = document.body.scrollHeight;
        else
            window.frameElement.height = '300px';
        window.frameElement.Width = document.body.scrollWidth;
    }
    function Page_Load()
    {
        //document.getElementById('Div1').style.display="none";
    }
     function FillValues(obj)
    {
    parent.editwin.close(obj);
     
       
//        var url='Member_usergroup.aspx?id='+obj;
//       
//        alert (obj);
//        editwin=dhtmlmodal.open("Editbox", "iframe", url,"Add Member","width=540px,height=350px,center=1,resize=1,top=500", "recal");
       
     //alert (obj);
    }
    
     function FunCallAjaxList(objID,objEvent,ObjType)
        {
        
               var strQuery_Table = '';
               var strQuery_FieldName = '';
               var strQuery_WhereClause = '';
               var strQuery_OrderBy='';
               var strQuery_GroupBy='';
               var CombinedQuery='';
               
               if(ObjType=='Digital')
                {
                var alert1=document.getElementById('HiddenField1').value;
                //alert(alert1);
                     strQuery_Table = "tbl_master_user";
                     strQuery_FieldName = "distinct top 10 USER_NAME+' ['+isnull(user_loginId,'')+']' AS loginid,user_id";
                     strQuery_WhereClause = " user_group not like ('"+document.getElementById('HiddenField1').value+"') and ( USER_NAME like (\'%RequestLetter%') or user_loginId like (\'%RequestLetter%'))";
                   // strQuery_WhereClause = " user_group not IN (\%52%') and ( USER_NAME like (\'%RequestLetter%') or user_loginId like (\'%RequestLetter%'))";
                    
                }
                CombinedQuery=new String(strQuery_Table+"$"+strQuery_FieldName+"$"+strQuery_WhereClause+"$"+strQuery_OrderBy+"$"+strQuery_GroupBy);
                 ajax_showOptions(objID,'GenericAjaxList',objEvent,replaceChars(CombinedQuery));
                 //alert (CombinedQuery);
        }

       function replaceChars(entry) 
       {
            out = "+"; // replace this
            add = "--"; // with this
            temp = "" + entry; // temporary holder

            while (temp.indexOf(out)>-1) {
            pos= temp.indexOf(out);
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
                        <strong><span style="color: #000099">Change Task Updation Log/Status</span></strong>
                    </td>
                </tr></table>
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
<table  width="400px" align="center" style="border:solid 1px white;">
<tr>
<td>
<table>
<td style="text-align: left;" id="td2" runat="server">
                                                            <span id="Span18" class="Ecoheadtxt" style="text-align:right;">Attend Status :</span>
                                                            </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlattend" runat="server">
                                                                
                                                                <asp:ListItem Value="1" Text="Completed"></asp:ListItem>
                                                                <asp:ListItem Value="2" Text="Open"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
</table>
</td>
</tr>
<tr >
<td>
<table>


        <td style="width: 150px; text-align: right;">
                                                       <span id="Span1" class="Ecoheadtxt" style="text-align:right;">Content :</span>
                                                        </td>
                                                    <td style="width: 60%" align="left">
                       <asp:TextBox ID="txtReportTo" runat="server" Width="600px" TextMode="MultiLine" Height="50px" ></asp:TextBox>
                      
                         <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtReportTo"
                            ErrorMessage="Required." ValidationGroup="a"></asp:RequiredFieldValidator>
                        <%--<asp:HiddenField ID="txtReportTo_hidden" runat="server" />--%>
                        
                  
                </td>
                
    <%--</tr>
    <tr>--%>
        
        </table>
</td>
    </tr>
    <tr style="height:10px;">
    </tr>
    <tr>
    <td>
    <table>
    
    <td style="width: 60px;"></td>
    <td align="left" id="td_yes" runat="server">
            <asp:Button ID="btnYes" runat="server" CssClass="btn" Text="Save" Width="120px" OnClick="btnYes_Click"  ValidationGroup="a"  />
        </td>
        <td style="width: 20px;"></td>
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
<tr> 
                <td>
                 
                            <div id="display" runat="server">
                            </div>
                       
                </td>
                 
            </tr>
</asp:Content>
