<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/PopUp.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.Master.management_master_contact_other" Codebehind="contact_other.aspx.cs" %>

<%--<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe000001" %>
<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxe" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe000001" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxTabControl" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxClasses" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
 <%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dxpc" %>
<%@ Register assembly="DevExpress.Web.v10.2" namespace="DevExpress.Web.ASPxTabControl" tagprefix="dx" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    

    


    <script type="text/javascript" src="/assests/js/ajax-dynamic-list.js"></script>
</asp:Content>
<%--    <title>Other</title>--%>
    

   
    
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script language="javascript" type="text/javascript">
    var HasWorkedOnKRADetail=false ;
  

    function disp_prompt(name)
    {
        if ( name == "tab0")
        {
        //alert(name);
        document.location.href="Contact_general.aspx"; 
        }
        if ( name == "tab1")
        {
        //alert(name);
        document.location.href="Contact_Correspondence.aspx"; 
        }
        else if ( name == "tab2")
        {
        //alert(name);
        document.location.href="Contact_BankDetails.aspx"; 
        }
        else if ( name == "tab3")
        {
        //alert(name);
        document.location.href="Contact_DPDetails.aspx"; 
        }
        else if ( name == "tab4")
        {
        //alert(name);
        document.location.href="Contact_Document.aspx"; 
        }
        else if ( name == "tab12")
        {
        //alert(name);
        document.location.href="Contact_FamilyMembers.aspx"; 
        }
        else if ( name == "tab5")
        {
        //alert(name);
        document.location.href="Contact_Registration.aspx"; 
        }
        else if ( name == "tab7")
        {
        //alert(name);
        document.location.href="Contact_GroupMember.aspx"; 
        }
        else if ( name == "tab8")
        {
        //alert(name);
        document.location.href="Contact_Deposit.aspx"; 
        }
        else if ( name == "tab9")
        {
        //alert(name);
        document.location.href="Contact_Remarks.aspx"; 
        }
        else if ( name == "tab10")
        {
        //alert(name);
        document.location.href="Contact_Education.aspx"; 
        }
        else if ( name == "tab11")
        {
        //alert(name);
        document.location.href="contact_brokerage.aspx"; 
        }
        else if ( name == "tab6")
        {
        //alert(name);
       // document.location.href="contact_other.aspx"; 
        }
        else if(name=="tab13")
        {
             document.location.href="contact_Subscription.aspx";
        }
        
    }
    function CallAjaxCustodian(objId,objParam,objevent)
    {
        ajax_showOptions(objId,objParam,objevent);
    }
    function CallAjaxDirector(objId,objParam,objevent)
    {
        ajax_showOptions(objId,objParam,objevent);
    }
    function CallAjaxGroupName(objId,objParam,objevent)
    {
        ajax_showOptions(objId,objParam,objevent);
    }
    function SearchByFundManager(objId,objParam,objevent)
    {
        ajax_showOptions(objId,objParam,objevent);
    }
    function relationshipTM(objVal)
    {
        if(objVal=='Y')
        {
            document.getElementById('TrDirector').style.display='inline';
        }
        else
        {
            document.getElementById('TrDirector').style.display='none';
        }
    }
    function associates(objVal)
    {
        if(objVal=='Y')
        {
            document.getElementById('tdFamGpCode1').style.display='inline';
            document.getElementById('tdFamGpCode').style.display='inline';
        }
        else
        {
            document.getElementById('tdFamGpCode1').style.display='none';
            document.getElementById('tdFamGpCode').style.display='none';
        }
    }
    function ShowMessage(objMessage)
    {
        if(objMessage!="")
        {
            if(objMessage=='1')
                alert('Update Successfully');
        }
    }
    function btnSave_Click()
    {
        compSegment.PerformCallback();
    }
    function TmCodeShow(obj)
    {
        if(obj=='Trading Member')
        {
            document.getElementById('tdTmCode1').style.display='inline';
            document.getElementById('tdTmCode2').style.display='inline';
        }
        else    
        {
            document.getElementById('tdTmCode1').style.display='none';
            document.getElementById('tdTmCode2').style.display='none';
        }
    }
    function VarificationOn()
    {
        document.getElementById("ASPxPageControl1_trVarDt").style.display="inline";
        //document.getElementById("ASPxPageControl1_trRemark").style.display="inline";
    }
    function VarificationOff()
    {
        document.getElementById("ASPxPageControl1_trVarDt").style.display="none";
      //  document.getElementById("ASPxPageControl1_trRemark").style.display="none";
    }
    function GetDetails(obj)
    {
      //var cmb=document.getElementById('cmbVerify');
      if(obj=='Y')
      {
        VarificationOn();
      }
      else
      {
        VarificationOff();
      }
    }
        function DateChange(DateObj)
        {
      
            KeyPress()
        }
        function KeyPress()
        {
         // alert("test");
          HasWorkedOnKRADetail=true;
        }
        function HideDiv()
        {
              divComp.style.display='none';
                divOthers.style.display='block';
        }
          function preventBackspace(e) {
            KeyPress();
                var evt = e || window.event;
                if (evt) {
                    var keyCode = evt.charCode || evt.keyCode;
                    if (keyCode === 8) {
                    
                        if (evt.preventDefault) {
                            evt.preventDefault();
                            evt.stopPropagation();
                        } 
                        else {
                            evt.returnValue = false;
                        }
                    }
                }
            }
//   function EnableDisableControls(b)
//   {
//   alert("1a");
//    if (b==1)
//    {
//        alert("2b");
//        $("#divKraDetail").children().attr("disabled","disabled");
//         var nodes = document.getElementById("divKraDetail").getElementsByTagName('*');
//            for(var i = 0; i < nodes.length; i++)
//            {
//                 nodes[i].disabled = true;
//            }
//      }
//      else 
//      {
//        var nodes = document.getElementById("divKraDetail").getElementsByTagName('*');
//            for(var i = 0; i < nodes.length; i++)
//            {
//                 nodes[i].disabled = false;
//            }
//      }
//   }
    function ShowHideDiv(li)
    { 
     if (typeof li !== 'undefined')
     {
      //  alert ("2");
         KeyPress();
          var i=li.value;
          if (i==1)
          {
            divComp.style.display='block';
            divOthers.style.display='none';
          }
          else 
          {
             divComp.style.display='none';
            divOthers.style.display='block';
          }
      }
      else 
      {
        
      }
    }
    function KeepPopUpOpened()
    {
      ShowKraDetailPopUp();
    }
    function SavingState()
    {
       document.getElementById("hdnISSavable").value="t";
    }
    function ShowKraDetailPopUp(para)
    {
   // alert("ABCD");
    
       document.getElementById("divOverlapping").style.display = 'block';
       document.getElementById("divKraPopUp").style.display = 'block';
        if(para==1)
         {
             document.getElementById("divBlankMessage").style.display = 'block';
         }
     
    }
    function CloseKraDeatailPopUp(para)
    {
      // alert(para+HasWorkedOnKRADetail);
      document.getElementById("divBlankMessage").style.display = 'none';
      if(para==1)
      {
      document.getElementById("divOverlapping").style.display = 'none';
      document.getElementById("divKraPopUp").style.display = 'none';
      }
     else if(para==0 && HasWorkedOnKRADetail==true)
      {
         var i=confirm("All your entered data will be lost. Are you sure to close this form ?");
         if(i==true)
         {
            // document.getElementById("divOverlapping").style.display = 'none';
            //  document.getElementById("divKraPopUp").style.display = 'none';
              HasWorkedOnKRADetail=false;
              CloseKraDeatailPopUp('1');
         }
         
      }
      else if(para==0 && HasWorkedOnKRADetail==false)
      {
         CloseKraDeatailPopUp('1');
      }
    }
    function DeleteConfirmation(param)
    {
//     var i=confirm("Are you sure to delete this data ?");
//     if (i==true)
//     {
//      var j=confirm("Are you sure to delete this data ?");
//      if (j==true)
//      {
//        var k=confirm("Are you sure to delete this data ?");
//        if (k==true)
//        {
//            return true;
//        }
//        else 
//        {
//          return false;
//        }
//      }
//      else 
//      {
//         return false;
//      }
//     }
//     else 
//     {
//       return false;
//     }
        if(param==0)
        {
         cConfirmPopUp1.Show();
         return false;
         }
         else if(param==1)
         {
            cConfirmPopUp1.Hide();
            cConfirmPopUp2.Show();
            return false;
         }
         else if (param==2)
         {
             cConfirmPopUp1.Hide();
            return false;
         }
         else if(param==3)
         {
            cConfirmPopUp2.Hide();
            cConfirmPopUp3.Show();
            return false;
         }
          else if (param==4)
         {
            cConfirmPopUp2.Hide();
            return false;
         }
          else if(param==5)
         {
            cConfirmPopUp3.Hide();
           // cConfirmPopUp3.Show();
            return true;
         }
          else if (param==6)
         {
            cConfirmPopUp3.Hide();
            return false;
         }
         
    }
  	 function CallList(obj1,obj2,obj3)
        {
        var obj5='';
                   ajax_showOptions(obj1,obj2,obj3,obj5);
        }
    
    FieldName='btnSave'
    </script>

     <asp:HiddenField ID="hdnISSavable" runat="server" Value="f" />
        <div>
            <table width="100%">
                <tr>
                    <td class="EHEADER" style="text-align: center">
                        <asp:Label ID="lblName" runat="server" Font-Bold="True"></asp:Label>
                    </td>
                </tr>
            </table>
            <table class="TableMain100">
                <tr>
                    <td>
                        <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="6" ClientInstanceName="page"
                            Width="100%" Font-Size="12px">
                            <TabPages>
                                <dxe:TabPage  Name="General"><TabTemplate >
<span style="font-size:x-small">General</span>&nbsp;<span style="color:Red;">*</span> 
</TabTemplate>
<ContentCollection>
<dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
</ContentCollection>
</dxe:TabPage>
                                <dxe:TabPage  Name="CorresPondence"><TabTemplate >
<span style="font-size:x-small">CorresPondence</span>&nbsp;<span style="color:Red;">*</span> 
</TabTemplate>
<ContentCollection>
<dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
</ContentCollection>
</dxe:TabPage>
                                <dxe:TabPage Name="Bank Details"><TabTemplate >
<span style="font-size:x-small">Bank</span>&nbsp;<span style="color:Red;">*</span> 
</TabTemplate>
<ContentCollection>
<dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
</ContentCollection>
</dxe:TabPage>
                                <dxe:TabPage Name="DP Details" ><TabTemplate >
<span style="font-size:x-small">DP</span>&nbsp;<span style="color:Red;">*</span> 
</TabTemplate>
<ContentCollection>
<dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
</ContentCollection>
</dxe:TabPage>
                                <dxe:TabPage Name="Documents" ><TabTemplate >
<span style="font-size:x-small">Documents</span>&nbsp;<span style="color:Red;">*</span> 
</TabTemplate>
<ContentCollection>
<dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
</ContentCollection>
</dxe:TabPage>
                                
                                <dxe:TabPage Name="Registration"><TabTemplate >
<span style="font-size:x-small">Registration</span>&nbsp;<span style="color:Red;">*</span> 
</TabTemplate>
<ContentCollection>
<dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
</ContentCollection>
</dxe:TabPage>
                                
                                 <dxe:TabPage Name="Other" ><TabTemplate >
<span style="font-size:x-small">Other</span>&nbsp;<span style="color:Red;">*</span> 
</TabTemplate>
<ContentCollection>
<dxe:ContentControl runat="server"><table width="100%"><tr><td><table style="text-align: left; background-color: #AFDCEC" width="100%"><tr ><td style="text-align: left;">Client Type : </td><td style="text-align: left; width: 178px;"><asp:DropDownList ID="ddlClientType" runat="server" Width="150px" onchange="TmCodeShow(this.value)"><asp:ListItem Value="Retail">Retail</asp:ListItem>
<asp:ListItem Value="HNI">HNI</asp:ListItem>
<asp:ListItem Value="NRI">NRI</asp:ListItem>
<asp:ListItem Value="Pro Trading">Pro Trading</asp:ListItem>
<asp:ListItem Value="Pro Investment">Pro Investment</asp:ListItem>
<asp:ListItem Value="FI">FI</asp:ListItem>
<asp:ListItem Value="FII">FII</asp:ListItem>
<asp:ListItem Value="Trading Member">Trading Member</asp:ListItem>
<asp:ListItem Value="PMS Client">PMS Client</asp:ListItem>
<asp:ListItem Value="NBFC Client">NBFC Client</asp:ListItem>
<asp:ListItem Value="Foreign National">Foreign National</asp:ListItem>
</asp:DropDownList>
 </td><td style="text-align: left;" id="tdTmCode1">TMCode : </td><td id="tdTmCode2"><asp:TextBox ID="txtTMCode" MaxLength="10" runat="server"></asp:TextBox>
 </td><td style="text-align: left;">Custodian : </td><td style="text-align: left;"><asp:TextBox ID="txtCustodian" runat="server" Width="300px" onkeyup="CallAjaxCustodian(this,'SearchByCustodian',event)"></asp:TextBox>
 <asp:HiddenField ID="txtCustodian_hidden" runat="server" />
 </td></tr><tr><td style="text-align: left;">Settlement Mode : </td><td style="text-align: left;"><asp:DropDownList ID="ddlSettlementMode" runat="server" Width="150px"><asp:ListItem Value="T">Trading Member</asp:ListItem>
<asp:ListItem Value="C">Custodian</asp:ListItem>
<asp:ListItem Value="B">Both</asp:ListItem>
</asp:DropDownList>
 </td><td style="text-align: left;">Contract Delivery Option : </td><td style="text-align: left; width: 178px;"><asp:DropDownList ID="ddlcontract" runat="server" Width="150px"><asp:ListItem Value="B">Both ECN & Print</asp:ListItem>
<asp:ListItem Value="E">Only ECN</asp:ListItem>
<asp:ListItem Value="P">Only Print</asp:ListItem>
</asp:DropDownList>
 </td><td style="text-align: left;">Is Direct Client of TM : </td><td style="text-align: left;"><asp:DropDownList ID="ddlDirClirnt" runat="server" Width="150px"><asp:ListItem Value="Y">Yes</asp:ListItem>
<asp:ListItem Value="N">No</asp:ListItem>
</asp:DropDownList>
 </td></tr><tr><td style="text-align: left;">Relationship with TM/Directors of the TM : </td><td style="text-align: left;"><asp:DropDownList ID="ddlRelationshipTM" runat="server" Width="150px" onchange="relationshipTM(this.value)"><asp:ListItem Value="N">No</asp:ListItem>
<asp:ListItem Value="Y">Yes</asp:ListItem>
</asp:DropDownList>
 </td><td style="text-align: left;" id="tdFamGpCode">Family Group Code : </td><td style="text-align: left;" id="tdFamGpCode1"><asp:TextBox ID="txtFamilygrpCode" runat="server" Width="147px" onkeyup="CallAjaxGroupName(this,'SearchByGroupName',event)"></asp:TextBox>
 <asp:HiddenField ID="txtFamilygrpCode_hidden" runat="server" />
 </td></tr><tr id="TrDirector"><td style="text-align: left;">Director : </td><td style="text-align: left; width: 178px;"><asp:TextBox ID="txtdirector" runat="server" Width="147px" onkeyup="CallAjaxDirector(this,'SearchByContact',event)"></asp:TextBox>
 <asp:HiddenField ID="txtdirector_hidden" runat="server" />
 </td><td style="text-align: left;">Relationship : </td><td style="text-align: left;"><asp:DropDownList ID="ddlRelationship" runat="server" Width="150px"></asp:DropDownList>
 </td><td></td><td></td></tr><tr><td colspan="3" style="text-align: left;">Does the Client has any other A/C with the same TM for any of the family member/associates ? : </td><td style="text-align: left;"><asp:DropDownList ID="ddlAssociates" runat="server" Width="150px" onchange="associates(this.value)"><asp:ListItem Value="N">No</asp:ListItem>
<asp:ListItem Value="Y">Yes</asp:ListItem>
</asp:DropDownList>
 </td></tr><tr><td style="text-align: left;">What is their Settlement Mode : </td><td style="text-align: left; width: 178px;"><asp:DropDownList ID="ddlSettmode" runat="server" Width="150px"><asp:ListItem Value="I">Independent</asp:ListItem>
<asp:ListItem Value="J">Joint</asp:ListItem>
</asp:DropDownList>
 </td><td style="text-align: left;">STP Provider : </td><td style="text-align: left;"><asp:DropDownList ID="ddlSTP" runat="server" Width="150px"><asp:ListItem Value="O">Select</asp:ListItem>
<asp:ListItem Value="F">FT</asp:ListItem>
<asp:ListItem Value="N">NSEIT</asp:ListItem>
<asp:ListItem Value="S">NSDL</asp:ListItem>
</asp:DropDownList>
 </td><td style="text-align: left;">Fund Manager : </td><td style="text-align: left;"><asp:TextBox ID="txtFundManager" runat="server" Width="147px" onkeyup="CallAjaxFundManager(this,'SearchByFundManager',event)"></asp:TextBox>
 <asp:HiddenField ID="txtFundManager_hidden" runat="server" />
 </td></tr></table></td></tr><tr><td><table style="text-align: left; background-color: #FFF8C6" width="100%"><tr><td style="text-align: left;">Special Category : </td><td style="text-align: left;"><asp:DropDownList ID="cmbCategory" runat="server" Width="150px"><asp:ListItem Value="None">None</asp:ListItem>
<asp:ListItem Value="Politician">Politician</asp:ListItem>
<asp:ListItem Value="NRI">NRI</asp:ListItem>
<asp:ListItem Value="Foreign National">Foreign National</asp:ListItem>
<asp:ListItem Value="HNI">HNI</asp:ListItem>
<asp:ListItem Value="TRUST">TRUST</asp:ListItem>
<asp:ListItem Value="CHARITIES">CHARITIES</asp:ListItem>
<asp:ListItem Value="NGO">NGO</asp:ListItem>
</asp:DropDownList>
 </td><td style="text-align: left;">PEP : </td><td style="text-align: left;"><asp:DropDownList ID="ddlpep" runat="server" Width="150px"></asp:DropDownList>
 </td><td style="text-align: left;">Risk Category : </td><td style="text-align: left;"><asp:DropDownList ID="cmbRisk" runat="server" Width="150px"><asp:ListItem Value="Low">Low</asp:ListItem>
<asp:ListItem Value="Medium">Medium</asp:ListItem>
<asp:ListItem Value="High">High</asp:ListItem>
</asp:DropDownList>
 </td><td></td></tr></table></td></tr><tr><td><table width="100%" style="text-align: left; background-color:#E3E4FA";><tr><td style="text-align: left;" >In Person Verification Done : </td><td colspan="5" align="left" style="color:Red; font-size:medium;">* <asp:DropDownList ID="cmbVerify" runat="server" onchange="GetDetails(this.value)"><asp:ListItem Text="No" Value="N"></asp:ListItem>
<asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
</asp:DropDownList>
 </td></tr><tr id="trVarDt" runat="server"><td style="text-align: left;">Verification Done in : </td>
<td><dxe:ASPxDateEdit ID="StDate" runat="server" ClientInstanceName="StDate" EditFormat="Custom"
                                                                        UseMaskBehavior="True" Width="100px" Font-Size="12px" TabIndex="21">
<ButtonStyle Width="13px">
                                                                        </ButtonStyle>
</dxe:ASPxDateEdit>
 </td>
<td style="text-align: left;">Verification Done By: </td>
<td><asp:TextBox ID="txtVerify" runat="server" Width="250px"></asp:TextBox>
 </td>
<td><asp:HiddenField ID="txtVerify_hidden" runat="server" />
 </td>
<td></td>
</tr>
<tr id="trRemark" runat="server"><td style="text-align: left;" valign="top">Remarks : </td>
<td colspan="5"><asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Width="400px" Height="40px"></asp:TextBox>
 </td>
</tr>
<tr><td colspan="4" style="text-align: right;"><input id="btnSave" type="button" value="Save" class="btnUpdate" onclick="btnSave_Click()"
                                                                        style="width: 68px; height: 23px" /> </td><td colspan="2" style="display: none"><dxe:ASPxComboBox ID="compInsert" runat="server" ClientInstanceName="compSegment"
                                                                        Width="175px" OnCallback="compInsert_Callback" OnCustomJSProperties="compInsert_CustomJSProperties">
<ClientSideEvents EndCallback="function(s,e){ShowMessage(s.cpUpdate);}" />
</dxe:ASPxComboBox>
 </td></tr><tr><td colspan="2" align="left"><asp:LinkButton ID="lbtnKraDetailPopUp" runat="server" Text="KRA Detail" OnClick="lbtnKraDetailPopUp_Click" ToolTip="KRA Detail Form" Font-Underline="true" ForeColor="blue" />
 </td></tr></table></td></tr></table></dxe:ContentControl>
</ContentCollection>
</dxe:TabPage>
                                <dxe:TabPage Name="Group Member" Text="Group"><ContentCollection>
<dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
</ContentCollection>
</dxe:TabPage>
                                <dxe:TabPage Name="Deposit" Text="Deposit"><ContentCollection>
<dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
</ContentCollection>
</dxe:TabPage>
                                <dxe:TabPage Name="Remarks" Text="Remarks"><ContentCollection>
<dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
</ContentCollection>
</dxe:TabPage>
                                <dxe:TabPage Name="Education" Text="Education"><ContentCollection>
<dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
</ContentCollection>
</dxe:TabPage>
                                <dxe:TabPage Name="Trad. Prof." Text="Trad.Prof"><%--<TabTemplate ><span style="font-size:x-small">Trad.Prof</span>&nbsp;<span style="color:Red;">*</span> </TabTemplate>--%>
                                    <ContentCollection>
<dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
</ContentCollection>
</dxe:TabPage>
                                <dxe:TabPage Name="FamilyMembers" Text="Family"><ContentCollection>
<dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
</ContentCollection>
</dxe:TabPage>
                               
                                <dxe:TabPage Name="Subscription" Text="Subscription"><ContentCollection>
<dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
</ContentCollection>
</dxe:TabPage>
                                
                            </TabPages>
                            <ClientSideEvents ActiveTabChanged="function(s, e) {
	                                            var activeTab   = page.GetActiveTab();
	                                            var Tab0 = page.GetTab(0);
	                                            var Tab1 = page.GetTab(1);
	                                            var Tab2 = page.GetTab(2);
	                                            var Tab3 = page.GetTab(3);
	                                            var Tab4 = page.GetTab(4);
	                                            var Tab5 = page.GetTab(5);
	                                            var Tab6 = page.GetTab(6);
	                                            var Tab7 = page.GetTab(7);
	                                            var Tab8 = page.GetTab(8);
	                                            var Tab9 = page.GetTab(9);
	                                            var Tab10 = page.GetTab(10);
	                                            var Tab11 = page.GetTab(11);
	                                            var Tab12 = page.GetTab(12);
	                                             var Tab13=page.GetTab(13);
	                                              var Tab14=page.GetTab(14);
	                                            if(activeTab == Tab0)
	                                            {
	                                                disp_prompt('tab0');
	                                            }
	                                            if(activeTab == Tab1)
	                                            {
	                                                disp_prompt('tab1');
	                                            }
	                                            else if(activeTab == Tab2)
	                                            {
	                                                disp_prompt('tab2');
	                                            }
	                                            else if(activeTab == Tab3)
	                                            {
	                                                disp_prompt('tab3');
	                                            }
	                                            else if(activeTab == Tab4)
	                                            {
	                                                disp_prompt('tab4');
	                                            }
	                                            else if(activeTab == Tab5)
	                                            {
	                                                disp_prompt('tab5');
	                                            }
	                                            else if(activeTab == Tab6)
	                                            {
	                                                disp_prompt('tab6');
	                                            }
	                                            else if(activeTab == Tab7)
	                                            {
	                                                disp_prompt('tab7');
	                                            }
	                                            else if(activeTab == Tab8)
	                                            {
	                                                disp_prompt('tab8');
	                                            }
	                                            else if(activeTab == Tab9)
	                                            {
	                                                disp_prompt('tab9');
	                                            }
	                                            else if(activeTab == Tab10)
	                                            {
	                                                disp_prompt('tab10');
	                                            }
	                                            else if(activeTab == Tab11)
	                                            {
	                                                disp_prompt('tab11');
	                                            }
	                                            else if(activeTab == Tab12)
	                                            {
	                                                disp_prompt('tab12');
	                                            }
	                                             else if(activeTab == Tab13)
	                                            {
	                                               disp_prompt('tab13');
	                                            }
	                                            
	                                            }"></ClientSideEvents>
                            <ContentStyle>
                                <Border BorderColor="#002D96" BorderStyle="Solid" BorderWidth="1px" />
                            </ContentStyle>
                            <LoadingPanelStyle ImageSpacing="6px">
                            </LoadingPanelStyle>
                        </dxe:ASPxPageControl>
                    </td>
                </tr>
            </table>
        </div>
    
        
        
         <div id="divOverlapping" style=" position: fixed;height:100%;width:100%;background-color: #000;top: 0px;left: 0px; opacity: 0.4;filter: alpha(opacity=40);z-index: 50; display:block;">
        </div>
        <div id="divKraPopUp" style=" background-color:#fff; height:450px;left:481px;position:absolute;top:25px;  width: 760px; left:90px;z-index:75;display: block; padding-bottom:10px; min-height:390px; ">
        <div style="background-color:Gray;height:7px; font-size:larger; color:White; font-weight:bold; padding-bottom:27px; padding-left:2px;">
        
               KRA Detail Form
              <img  src="../../windowfiles/close.gif" height="16px" alt="CLOSE" onclick="CloseKraDeatailPopUp('0');" style="padding-left:98%; margin: 0px;"  />
        </div>
           <div>
              <div style=" padding-left:87%; padding-top:7px; margin:1px;">
                 <asp:LinkButton ID="lbtnAdd" runat="server" Text="Add" OnClick="lbtnAdd_Click" OnClientClick="KeepPopUpOpened();"></asp:LinkButton>
                 <asp:LinkButton ID="lbtnModify" runat="server" Text="Modify" OnClick="lbtnModify_Click" OnClientClick="KeepPopUpOpened();"></asp:LinkButton>
                  <asp:LinkButton ID="lbtnDelete" runat="server" Text="Delete"  OnClientClick="return DeleteConfirmation('0');" ></asp:LinkButton>
              </div>
             <div id="divKraDetail" runat="server" style="padding-left:25%;padding-bottom:5px;" >
            <table cellpadding="2" cellspacing="3" >
              
                <tr>
                    <td align="right">
                        Reg. Number
                    </td>
                    <td align="left">
                        <dxe:ASPxTextBox ID="txtRegistrationNumber" runat="server" onKeyDown="KeyPress();" ClientInstanceName=                                  "ctxtRegistrationNumber" TabIndex="1" Width="160px">
                       <%-- <ClientSideEvents TextChanged="onValid"/>--%>
                        </dxe:ASPxTextBox>
                    </td>
        <td align="left">
          <asp:RequiredFieldValidator ID="reqRegistationNumber" runat="server" ControlToValidate="txtRegistrationNumber" ErrorMessage="Enter Registration Number" ValidationGroup="kra" Display="Static"></asp:RequiredFieldValidator>
        </td>
                </tr>
                <tr>
                    <td align="right">
                        Reg. Agency
                    </td>
                    <td align="left">
                        
                        <%--<dxe:ASPxComboBox ID="ddlRegistrationAgency" runat="server" ClientInstanceName="cddlRegistrationAgency" TabIndex="2" Width="160px">
                        </dxe:ASPxComboBox>--%>
                        <asp:DropDownList ID="ddlRegistrationAgency" runat="server" TabIndex="2" Width="160px" onKeyDown="preventBackspace();"></asp:DropDownList>
                    </td>
                    <td>
          <asp:RequiredFieldValidator ID="reqAgency" runat="server" ControlToValidate="ddlRegistrationAgency" ErrorMessage="Select Registration Agency" InitialValue="0" ValidationGroup="kra"></asp:RequiredFieldValidator>
        </td>
                </tr>
                <tr>
                    <td align="right">
                        Reg. Date
                    </td>
                    <td align="left" colspan="2">
                        <dxe:ASPxDateEdit ID="txtRegistrationDate" runat="server"  EditFormat="Custom" 
                            EditFormatString="dd-MM-yyyy" UseMaskBehavior="True" ClientInstanceName="ctxtRegistrationDate" TabIndex="3" Width="160px">
                         
                                <clientsideevents datechanged="function(s,e){DateChange(ctxtRegistrationDate);}"></clientsideevents>
                        </dxe:ASPxDateEdit>
                    </td>
                   
                </tr>
                <tr>
                    <td align="right" valign="top">
                        Reg. Intermediary
                    </td>
                    <td align="left" colspan="2" valign="middle">
                          <asp:RadioButtonList ID="rbtnlstRegInter" runat="server" RepeatDirection="Horizontal"  AutoPostBack="false" CellSpacing="1" TabIndex="4" Width="190px" RepeatLayout="Table">
                         <asp:ListItem Selected="True" Value="1" Text="&nbsp;Own" onclick="ShowHideDiv(this);"></asp:ListItem>
                         <asp:ListItem Value="2" Text="&nbsp;Other&nbsp;Entity" onclick="ShowHideDiv(this);"></asp:ListItem>
                         </asp:RadioButtonList>
                          
                    </td>
                </tr>
                <tr id="trCompany">
                    <td align="left" colspan="2">
                        
                        <div id="divComp" style="display:block;">
                            <table  style="position:relative; padding-left:11px;">
                                <tr>
                                    <td style="width: 88px">
                                        Company&nbsp;Name
                                    </td>
                                    <td colspan="2">
                                    <%-- <dxe:ASPxComboBox ID="ddlCompany" runat="server" ValueType="System.String" TabIndex="5" Width="159px">
                                     </dxe:ASPxComboBox>--%>
                                  <asp:DropDownList ID="ddlCompany" runat="server" TabIndex="5" Width="159px" onKeyDown="preventBackspace();">                                     </asp:DropDownList>
                                    </td>
                                  
                                </tr>
                            </table>
                        </div>
                     <div id="divOthers" style="display:none;">
                            <table  style="position:relative; padding-left:29px;">
                                <tr >
                                    <td style="width: 89px">
                                        Other&nbsp;Entity
                                    </td>
                                    <td colspan="2">
                                        <dxe:ASPxTextBox ID="txtOtherEntity" runat="server" TabIndex="6" Width="160px" onKeyDown="KeyPress();">
                                        </dxe:ASPxTextBox>
                                    </td>
                                    
                                </tr>
                            </table>
                        </div>
                       
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td align="right">
                        New&nbsp;KYC&nbsp;Date
                    </td>
                    <td align="left" colspan="2">
                        <dxe:ASPxDateEdit ID="txtNewKYCDate" runat="server"  EditFormat="Custom"                                         ClientInstanceName="ctxtNewKYCDate"
                            EditFormatString="dd-MM-yyyy" UseMaskBehavior="True" TabIndex="7" Width="158px">
                            <ButtonStyle Width="13px">
                            </ButtonStyle>
                            <clientsideevents datechanged="function(s,e){DateChange(txtNewKYCDate);}">
                            </clientsideevents>
                        </dxe:ASPxDateEdit>
                    </td>
                    
                </tr>
                <tr>
                    <td align="right">
                        KYC&nbsp;Mod&nbsp;Date
                    </td>
                    <td align="left" colspan="2">
                        <dxe:ASPxDateEdit ID="txtKYCModDate" runat="server" Width="156px" EditFormat="Custom"
                            EditFormatString="dd-MM-yyyy" UseMaskBehavior="True" TabIndex="8" ClientInstanceName="ctxtKYCModDate">
                            <ButtonStyle >
                            </ButtonStyle>
                              <clientsideevents datechanged="function(s,e){DateChange(ctxtKYCModDate);}">
                            </clientsideevents>
                        </dxe:ASPxDateEdit>
                    </td>
                    
                </tr>
                <tr>
                    <td align="right">
                        Status
                    </td>
                    <td align="left" colspan="2">
                        <asp:DropDownList ID="ddlStatus" runat="server" TabIndex="9" Width="157px"           onKeyDown="preventBackspace();" >
                            <asp:ListItem Value="0">Select</asp:ListItem>
                            <asp:ListItem Value="Verified">Verified</asp:ListItem>
                            <asp:ListItem Value="Under Process">Under Process</asp:ListItem>
                            <asp:ListItem Value="On Hold">On Hold</asp:ListItem>
                            <asp:ListItem Value="Rejected">Rejected</asp:ListItem>
                        </asp:DropDownList>
                        
                    </td>
                    
                </tr>
                <tr>
                    <td align="right">
                        Status Date
                    </td>
                    <td align="left" colspan="2">
                       
                         <dxe:ASPxDateEdit ID="txtStatusDate" runat="server" Width="156px" EditFormat="Custom"
                            EditFormatString="dd-MM-yyyy" UseMaskBehavior="True" TabIndex="10" ClientInstanceName="ctxtStatusDate">
                            <ButtonStyle >
                            </ButtonStyle>
                             <clientsideevents datechanged="function(s,e){DateChange(ctxtStatusDate);}">
                            </clientsideevents>
                        </dxe:ASPxDateEdit>
                    </td>
                    
                </tr>
                <tr>
                    <td align="right">
                        Document Source
                    </td>
                    <td align="left" colspan="2">
                        <asp:DropDownList ID="ddlDocumentSource" runat="server" Width="157px" TabIndex="11" onKeyDown="preventBackspace();">
                            <asp:ListItem Value="0">Select</asp:ListItem>
                            <asp:ListItem Value="Fetched">Fetched</asp:ListItem>
                            <asp:ListItem Value="Scanned">Scanned</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                     
                </tr>
                <tr>
                   <td></td>
                    <td colspan="2">
                     <asp:Button ID="btnSaveDatas" runat="server" Text="Save" OnClick="btnSaveData_Click" TabIndex="12" OnClientClick="SavingState();"  ValidationGroup="kra" Width="60px" /> &nbsp;&nbsp; 
                      <asp:Button ID="btnCancels" runat="server" Text="Cancel" OnClick="btnCancels_Click"  OnClientClick="CloseKraDeatailPopUp('0'); return false;" TabIndex="13"  ValidationGroup="none" Width="60px" />
                    </td>
                   
                </tr>
            </table>
            </div>
        </div>
              <div id="divBlankMessage" style="font-weight:100; font-size:x-large; display:none; padding-left:300px; padding-top:140px;">
                NO DATA FOUND
              </div>
              <div id="divPopUpControls">
              <dxe:ASPxPopupControl ID="ConfirmPopUp1" runat="server" ClientInstanceName="cConfirmPopUp1"
            CloseAction="None" HeaderText="Confirmation Message" Modal="True" Left="100"
            PopupHorizontalAlign="Center"  PopupVerticalAlign="BottomSides" PopupHorizontalOffset="100"                     PopupVerticalOffset="200"
             RenderIFrameForPopupElements="True" ShowSizeGrip="False" Width="270px">
            <ContentCollection>
                <dxe:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                    <table cellspacing="0">
                        <tr>
                            <td colspan="2" align="center" style="font-size:small;">
                               Are you sure you to Delete this Data?
                               </td>
                        </tr>
                        <tr>
                          
                            <td style="width: 7px" align="right">
                                
                                <asp:Button ID="btnYes1" runat="server"  Text="&nbsp;Yes&nbsp;" OnClientClick=" return DeleteConfirmation('1')" />
                            </td>
                            <td style="width: 7px" align="left">
                               
                                <asp:Button ID="btnNo1" runat="server" Text="&nbsp;No&nbsp;&nbsp;" OnClientClick=" return DeleteConfirmation('2')" />
                            </td>
                        </tr>
                    </table>
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>
        
          <dxe:ASPxPopupControl ID="ConfirmPopUp2" runat="server" ClientInstanceName="cConfirmPopUp2"
            CloseAction="None" HeaderText="Confirmation Message"  Modal="True" Left="400"
            PopupHorizontalAlign="Center"  PopupVerticalAlign="TopSides" PopupHorizontalOffset="200"                     PopupVerticalOffset="300"
             RenderIFrameForPopupElements="True" ShowSizeGrip="False" Width="270px">
            <ContentCollection>
                <dxe:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                    <table cellspacing="0">
                        <tr>
                            <td colspan="2" align="center" style="font-size:small;">
                                Are you sure you to Delete this Data?
                             </td>
                        </tr>
                        <tr>
                            
                            <td style="width: 7px" align="right">
                                
                                <asp:Button ID="btnYes2" runat="server" Text="&nbsp;Yes&nbsp;" OnClientClick=" return DeleteConfirmation('3')" />
                            </td>
                            <td style="width: 7px" align="left">
                               
                                <asp:Button ID="btnNo2" runat="server" Text="&nbsp;No&nbsp;&nbsp;" OnClientClick=" return DeleteConfirmation('4')" />
                            </td>
                        </tr>
                    </table>
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>
        
        
          <dxe:ASPxPopupControl ID="ConfirmPopUp3" runat="server" ClientInstanceName="cConfirmPopUp3"
            CloseAction="None" HeaderText="Confirmation Message"  Modal="True" Left="250"
            PopupHorizontalAlign="Center"  PopupVerticalAlign="Middle" PopupHorizontalOffset="300"                     PopupVerticalOffset="400"
             RenderIFrameForPopupElements="True" ShowSizeGrip="False" Width="270px">
            <ContentCollection>
                <dxe:PopupControlContentControl ID="PopupControlContentControl3" runat="server">
                    <table cellspacing="0">
                        <tr>
                            <td colspan="2" align="center" style="font-size:small;">
                               Are you sure you to Delete this Data?
                               </td>
                        </tr>
                        <tr>
                            <td style="width: 7px" align="right">
                                <asp:Button ID="btnYes3" runat="server" Text="&nbsp;Yes&nbsp;" OnClientClick=" return DeleteConfirmation('5')" OnClick="btnDelete_Click" />
                            </td>
                            <td style="width: 7px" align="left">
                               
                                <asp:Button ID="btnNo3" runat="server" Text="&nbsp;No&nbsp;&nbsp;" OnClientClick=" return DeleteConfirmation('6')" />
                            </td>
                        </tr>
                    </table>
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>
     </div>
        </div>

   </asp:Content>
