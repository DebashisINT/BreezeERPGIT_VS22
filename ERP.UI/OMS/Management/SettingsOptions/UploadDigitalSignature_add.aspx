<%@ Page AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/PopUp.Master" Inherits="ERP.OMS.Management.SettingsOptions.management_SettingsOptions_UploadDigitalSignature_add" Language="C#" Codebehind="UploadDigitalSignature_add.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../CSS/style.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="/assests/js/init.js"></script>

    <script type="text/javascript" src="/assests/js/ajax-dynamic-list.js"></script>

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
		z-index:100;
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
		z-index:5;
	}
	
	form{
		display:inline;
	}
	
	</style>

    <script language="javascript" type="text/javascript">
  function SignOff()
    {
        window.parent.SignOff();
    }
function PageLoad()
         {
            FieldName='A4'; 
                 
            document.getElementById('txtEmpName_hidden').style.display="none";
            document.getElementById('txtValidUser_hidden').style.display="none";
            document.getElementById('TblUpload').style.display="none";
            document.getElementById('hiddenEmployee').value=="";   
            
            //EToken Change
            document.getElementById('TrSignOf').style.display="inline";
            document.getElementById('TrSignName').style.display="none";
            document.getElementById('TrBrowseSign').style.display="inline";
            document.getElementById('TrPwd').style.display="inline";
            document.getElementById('TrRePwd').style.display="inline";
          } 
    
function CallAjax(obj1,obj2,obj3)
         {
          ajax_showOptions(obj1,obj2,obj3);
         }
function validation()
        {    clientselectionfinal();
        
             var SignType=document.getElementById("ddl_SignType").value;
             if(document.getElementById('txtEmpName_hidden').value=="" || document.getElementById('txtEmpName_hidden').value=="No Record Found")
             {
                alert('Please Enter Employee Name');
                document.getElementById('txtEmpName').focus();
                return false;
             }
             else
            if(dtfrom.GetDate()==null )
             {
                alert('Please Enter Valid From Date.');
                
                return false;
             }
             else
           if(dtto.GetDate()==null )
             {
                alert('Please Enter Valid To Date.');
                return false;
             }
             else
           if(dtto.GetDate()<= dtfrom.GetDate())
             {
                alert('Please Enter Valid Date Range.');
                return false;
             }
             else
            if(document.getElementById('FileUpload1').value=="" && SignType=="N")
             {
                alert('Please Choose File Path.');
                return false;
             }

             
             else
            if(document.getElementById('txtPass').value=="" && SignType=="N")
             {
//                alert('Please Enter Password.');
//                document.getElementById('txtPass').focus();
//                return false;
             }
             else
           if(document.getElementById('txtRePass').value!=document.getElementById('txtPass').value && SignType=="N")
             {
                document.getElementById('txtRePass').focus();
                alert('Re-Enter Password Correctly.');
                return false;
             }
             else
            if(document.getElementById('hiddenEmployee').value=="" || document.getElementById('hiddenEmployee').value=="No Record Found" )
             {
                document.getElementById('txtValidUser').focus();
                alert('Please Enter Authorised User.');
                return false;
             }
           
            
         return true;
        }
       function displayPath()
       {
       var str=document.getElementById('FileUpload1').value;
       str=str.substring(str.lastIndexOf('\\')+1);
       document.getElementById('Path').innerHTML=str;
       
       } 
       
 function btnAddsubscriptionlist_click()
        {
            var userid = document.getElementById('txtValidUser');
            if(userid.value != '' || userid.value=='No Record Found')
            {
                var ids = document.getElementById('txtValidUser_hidden');
                var listBox = document.getElementById('lstSuscriptions');
                var tLength = listBox.length;
                //alert(tLength);
                
                var no = new Option();
                no.value = ids.value;
                no.text = userid.value;
                listBox[tLength]=no;
                var recipient = document.getElementById('txtValidUser');
                recipient.value='';
                
               var recipientid = document.getElementById('txtValidUser_hidden');
                recipientid.value='';

            }
            else
                alert('Please search name and then Add!')
            var s=document.getElementById('txtValidUser');
            s.focus();
            s.select();
        }
        function btnRemovefromsubscriptionlist_click()
        {
            
            var listBox = document.getElementById('lstSuscriptions');
            var tLength = listBox.length;
            
            var arrTbox = new Array();
            var arrLookup = new Array();
            var i;
            var j = 0;
            for (i = 0; i < listBox.options.length; i++) 
            {
                if (listBox.options[i].selected && listBox.options[i].value != "") 
                {
                    
                }
                else 
                {
                    arrLookup[listBox.options[i].text] = listBox.options[i].value;
                    arrTbox[j] = listBox.options[i].text;
                    j++;
                }
            }
            listBox.length = 0;
            for (i = 0; i < j; i++) 
            {
                var no = new Option();
                no.value = arrLookup[arrTbox[i]];
                no.text = arrTbox[i];
                listBox[i]=no;
            }
        }
        function clientselectionfinal()
	    {
	        var listBoxSubs = document.getElementById('lstSuscriptions');
            var listIDs='';
            var i;
            if(listBoxSubs.length > 0)
            {                
                for(i=0;i<listBoxSubs.length;i++)
                {
                    if(listIDs == '')
                        listIDs = listBoxSubs.options[i].value;
                    else
                        listIDs += ',' + listBoxSubs.options[i].value;
                }
               
            }
            
	     document.getElementById('hiddenEmployee').value=listIDs;
	    }

       
        function height()
        {
            if(document.body.scrollHeight>=350)
            {
                window.frameElement.height = document.body.scrollHeight;
            }
            else
            {
                window.frameElement.height = '350px';
            }
            window.frameElement.widht = document.body.scrollWidht;
        }
        function loadFocus()
        {
        document.getElementById('txtEmpName').focus();
        height();
        }

function closeWin()
        { 
        parent.editwin.close();
       // parent.DhtmlClose();
        }
        
        //EToken Change
        function ddl_SignTypeChange()
        {
            if(document.getElementById("ddl_SignType").value=="N")
            {
                document.getElementById('TrSignName').style.display="none";
                document.getElementById('TrBrowseSign').style.display="inline";
                document.getElementById('TrPwd').style.display="inline";
                document.getElementById('TrRePwd').style.display="inline";
            }
            else
            {
                document.getElementById('TrSignName').style.display="inline";
                document.getElementById('TrBrowseSign').style.display="none";
                document.getElementById('TrPwd').style.display="none";
                document.getElementById('TrRePwd').style.display="none";
            }
        }

    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div align="center">
            <asp:HiddenField ID="hiddenEmployee" runat="server" />
            <table class="TableMain100">
                <tr>
                    <td colspan="5" class="EHEADER" style="text-align: center">
                        <strong><span style="color: #000099">Upload Digital Signature</span></strong>
                    </td>
                </tr>
            </table>
            <table border="0" style="border: solid 1px white; width: 650px">
                <tr>
                    <td style="width: 15%" class="gridcellleft">
                        <span class="Ecoheadtxt">Sign Type:</span>
                    </td>
                    <td class="gridcellleft" colspan="4">
                        <asp:DropDownList ID="ddl_SignType" runat="server" Width="112px" onchange="ddl_SignTypeChange()">
                            <asp:ListItem Selected="True" Value="N">Normal</asp:ListItem>
                            <asp:ListItem Value="E">EToken</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr id="TrSignOf">
                    <td style="width: 15%" class="gridcellleft">
                        <span class="Ecoheadtxt">Signature of:</span>
                    </td>
                    <td colspan="4" class="gridcellleft">
                        <asp:TextBox ID="txtEmpName" runat="server" Width="238px" TabIndex="2"></asp:TextBox>
                        <asp:TextBox ID="txtEmpName_hidden" runat="server"></asp:TextBox>
                    </td>
                </tr>
                 <tr id="TrSignName" style="display:none">
                    <td style="width: 15%" class="gridcellleft">
                        <span class="Ecoheadtxt">Sign Name:</span>
                    </td>
                    <td colspan="4" class="gridcellleft">
                        <asp:TextBox ID="TxtSignName" runat="server" Width="238px" TabIndex="2"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%" class="gridcellleft">
                        <span class="Ecoheadtxt">Valid:</span>
                    </td>
                    <td style="width: 10%" class="gridcellleft">
                        <dxe:ASPxDateEdit ID="dtFrom" runat="server" EditFormat="Custom" UseMaskBehavior="True" Font-Size="12px" Width="112px" ClientInstanceName="dtfrom" tabIndex="3">
                            <dropdownbutton text="From"></dropdownbutton>
                        </dxe:ASPxDateEdit>
                    </td>
                    <td style="width: 10%;" class="gridcellleft">
                        <dxe:ASPxDateEdit ID="dtTo" runat="server" EditFormat="Custom" UseMaskBehavior="True" Font-Size="12px" Width="112px" ClientInstanceName="dtto" tabIndex="4">
                            <dropdownbutton text="To"></dropdownbutton>
                        </dxe:ASPxDateEdit>
                    </td>
                    <td class="gridcellleft" colspan="2" style="width: 267px">
                    </td>
                </tr>
                <tr id="TrBrowseSign">
                    <td style="width: 15%" class="gridcellleft">
                        <span class="Ecoheadtxt">Signature:</span></td>
                    <td colspan="4" class="gridcellleft">
                        <asp:FileUpload ID="FileUpload1" runat="server" onchange="displayPath();" onkeydown="javascript:return
                        false;" onkeypress="javascript:return false;" onpaste="javascript:return false;" Width="325px" TabIndex="5" />
                    </td>
                </tr>
                
                <tr id="TrPwd">
                    <td style="width: 20%" class="gridcellleft">
                        <span class="Ecoheadtxt">Password: </span>
                    </td>
                    <td colspan="3" class="gridcellleft" style="width: 25%">
                        <asp:TextBox ID="txtPass" Width="238px" runat="server" TextMode="Password" TabIndex="6"></asp:TextBox>
                    </td>
                    
                </tr>
                <tr id="TrRePwd">
                    <td class="gridcellleft" style="width: 20%">
                        <span class="Ecoheadtxt">Re-enter password:</span></td>
                    <td class="gridcellleft" colspan="3">
                        <asp:TextBox ID="txtRePass" runat="server" TextMode="Password" Width="238px" TabIndex="7"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="gridcellleft">
                        <span class="Ecoheadtxt">Authorised User:</span>
                    </td>
                    <td colspan="3" class="gridcellleft">
                        <asp:TextBox ID="txtValidUser" runat="server" Width="238px" TabIndex="8"></asp:TextBox>
                        <br />
                        <a id="A4" href="javascript:void(0);" onclick="btnAddsubscriptionlist_click()" tabindex="9"><span style="color: #009900; text-decoration: underline; font-size: 8pt;">Add to List</span></a>
                    </td>
                </tr>
                <tr>
                    <td class="gridcellleft">
                    </td>
                    <td class="gridcellleft" colspan="3">
                        <span class="Ecoheadtxt">Authorised User List:</span><br />
                        <asp:ListBox ID="lstSuscriptions" runat="server" Font-Size="12px" Height="70px" Width="220px" TabIndex="10"></asp:ListBox><br />
                        <a id="A1" href="javascript:void(0);" onclick="btnRemovefromsubscriptionlist_click()" tabindex="12"><span style="color: #cc3300; text-decoration: underline; font-size: 8pt;">Remove</span></a>
                    </td>
                </tr>
                <tr>
                    <td class="gridcellleft">
                    </td>
                    <td colspan="3" class="gridcellleft">
                        <table width="100%">
                            <tr>
                                <td align="left">
                                    <asp:Button ID="btnSave" runat="server" CssClass="btnUpdate btn btn-primary" Height="23px" OnClick="btnSave_Click" OnClientClick="javascript:return validation();" TabIndex="13" Text="Upload" Width="101px" />
                                </td>
                                <td align="right">
                                    <asp:Button ID="btnClose" runat="server" CssClass="btnUpdate btn btn-danger" Height="23px" OnClientClick="javascript:closeWin();" TabIndex="13" Text="Close" Width="101px" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr style="display: none;">
                    <td class="gridcellleft">
                    </td>
                    <td class="gridcellleft" colspan="3">
                        &nbsp;&nbsp;<br />
                        <asp:TextBox ID="txtValidUser_hidden" runat="server"></asp:TextBox>
                    </td>
                    <td align="left" class="gridcellleft" colspan="1" rowspan="1" style="width: 60%" valign="top">
                    </td>
                </tr>
            </table>
        </div>
</asp:Content>