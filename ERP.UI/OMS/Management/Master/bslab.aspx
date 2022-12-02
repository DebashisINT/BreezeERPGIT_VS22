<%@ Page Title="Tax Slabs" Language="C#" AutoEventWireup="true" Inherits="ERP.OMS.Management.Master.management_master_bslab" Codebehind="bslab.aspx.cs" MasterPageFile="~/OMS/MasterPage/ERP.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	

	
    <%--<style type="text/css">
       a:hover {background:#ffffff; text-decoration:none;} /*BG color is a must for IE6*/
       a.tooltip span {display:none; padding:2px 3px; font-size:smaller; margin-left:1px; width:100px;}
       a.tooltip:hover span{display:inline; position:horizontal; background:#ffffff; border:1px solid #cccccc; color:#6c6c6c; width:100px}
    
    </style>--%>

    <script language="javascript" type="text/javascript">

       function noNumbers(e)
    {
        var keynum
        var keychar
        var numcheck
        
        if(window.event)//IE
        {
            keynum = e.keyCode                        
            if(keynum>=48 && keynum<=57 || keynum==46)
                 {
                  return true;
                 }
                 else
                {
                 alert("Please Insert Numeric Only");
                 return false;
                }
         } 
         
          else if(e.which) // Netscape/Firefox/Opera
       {
           keynum = e.which  
           
           if(keynum>=48 && keynum<=57 || keynum==46)
                 {
                  return true;
                 }
                 else
                 {
                 alert("Please Insert Numeric Only");
                 return false;
                 }     
        }
  } 
  
   function disableminrate()
 {  
   var gg= document.getElementById("txtfixedamt").value;
   if(gg!="")
   {
   document.getElementById("txtrate").disabled=true;
   document.getElementById("txtminamt").disabled=true;
   }
   
   else
   
   {
    document.getElementById("txtrate").disabled=false;
   document.getElementById("txtminamt").disabled=false;
   }
 }
 
 function disablefixedamt()
 {
   var gf= document.getElementById("txtminamt").value;
   if(gf!="")
   document.getElementById("txtfixedamt").disabled=true;
   else
   document.getElementById("txtfixedamt").disabled=false;
  
 }
 
 function disablefixedamt1()
 {
   var rr= document.getElementById("txtrate").value;
   if(rr!="")
   document.getElementById("txtfixedamt").disabled=true;
   else
    document.getElementById("txtfixedamt").disabled=false;
  
 }
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Add Slab</h3>
             <div class="crossBtn"><a href="brokerageslab.aspx"><i class="fa fa-times"></i></a></div>
        </div>
    </div>
    <div class="form_main" style="border:1px solid #ccc; padding:10px 15px;">
            <table  class="TableMain100">
                <tr>
                    <td >
                    </td>
                </tr>
                <tr>
                    <td>
                   
                            <table style="width: 530px;">
                                <tr>
                                    <td>
                                        <table style="width:100px; z-index:101;border:solid 1px white">
                                            <tr>
                                            <asp:HiddenField id="hd1" runat="server"/>
                                                <td class="Ecoheadtxt" >
                                                    <dxe:ASPxLabel ID="ASPxLabel1" runat="server" Text="Slab Code:"></dxe:ASPxLabel>
                                                </td>
                                                <td class="Ecoheadtxt" style="text-align: left;">
                                                   <dxe:ASPxTextBox runat="server" Width="220px" MaxLength="4"  ID="txtslabcode"></dxe:ASPxTextBox>
                                                                                                     
                                                </td>
                                                <td class="Ecoheadtxt"  >
                                                    <dxe:ASPxLabel ID="ASPxLabel5" Visible="false" runat="server" Text="Slab Type:"></dxe:ASPxLabel>
                                                </td>
                                                <td class="Ecoheadtxt" style="text-align: left;padding:3px;">
                                                                   
                                                   <dxe:ASPxComboBox Visible="false" runat="server" Width="220px" ID="ddltype" DropDownStyle="DropDown" EnableSynchronization="False" EnableIncrementalFiltering="True">
                                                                                                            
                                                                   <Items>
                                                                        <dxe:ListEditItem Text="Unit Price" Value="Unit Price">
                                                                         </dxe:ListEditItem>
                                                                         <dxe:ListEditItem Text="Volume" Value="Volume">
                                                                         </dxe:ListEditItem>
                                                                       <dxe:ListEditItem Text="Delivery Volume" Value="Delivery Volume">
                                                                         </dxe:ListEditItem>
                                                                            <dxe:ListEditItem Text="Square-Off Volume" Value="Square-Off Volume">
                                                                         </dxe:ListEditItem>
                                                                                         
                                                                   </Items>
                   
                                                     </dxe:ASPxComboBox>
                                                
                                                </td>
                                                
                    
                                            </tr>

                                           <tr>
                                                   <td class="Ecoheadtxt"  >
                                                        <dxe:ASPxLabel ID="ASPxLabel28" runat="server" Text="Min:" ></dxe:ASPxLabel>
                                                    </td>
                                                    <td class="Ecoheadtxt" style="text-align: left;">
                                                      <asp:TextBox  runat="server" Width="220px" ID="txtmin" onkeypress="return noNumbers(event)" MaxLength="19"></asp:TextBox>
                                                       <%--<dxe:ASPxTextBox runat="server" Width="150px"  ID="txtmin" onkeypress="return noNumbers(event)">--%>
                                                     <%--  <MaskSettings Mask="<0..999999999999g>.<00..99>"  />
                                                       <ValidationSettings ErrorDisplayMode="None"></ValidationSettings>--%>
                                          <%--             </dxe:ASPxTextBox>--%>
                                                        
                                                    </td>
                                                    
                                                          <td class="Ecoheadtxt" >
                                                      <dxe:ASPxLabel ID="ASPxLabel29" runat="server" Text="Max:"></dxe:ASPxLabel>
                                                    </td>
                                                    <td class="Ecoheadtxt" style="text-align:left;">
                                                    <asp:TextBox runat="server" Width="220px"  MaxLength="19" ID="txtmax" onkeypress="return noNumbers(event)"></asp:TextBox>
                                                  <%--   <dxe:ASPxTextBox runat="server" Width="150px"  ID="txtmax" onkeypress="return noNumbers(event)">
                                                 
                                                     </dxe:ASPxTextBox>--%>
                                                    </td>
                                           </tr>
                                              <tr>
                                                                                              
                                          
                                                
                                                 <td class="Ecoheadtxt" >
                                                    <dxe:ASPxLabel ID="ASPxLabel30" runat="server" Text="Fixed Amt:" Width="93px"></dxe:ASPxLabel>
                                                </td>
                                                <td class="Ecoheadtxt" style="text-align: left">
                                               <%--  <dxe:ASPxTextBox runat="server" Width="150px"  ID="txtfixedamt" onkeypress="return noNumbers(event)">
                                                </dxe:ASPxTextBox>--%>
                                                <asp:TextBox runat="server" Width="220px" ID="txtfixedamt" onkeypress="return noNumbers(event)" MaxLength="19"></asp:TextBox>
                                                </td>
                                                <td class="Ecoheadtxt" >
                                                    <dxe:ASPxLabel ID="ASPxLabel38" runat="server" Text="Rate(%):" Width="93px"></dxe:ASPxLabel>
                                                </td>
                                                <td class="Ecoheadtxt" style="text-align: left">
                                               <%--     <dxe:ASPxTextBox runat="server" Width="150px"  ID="txtrate" onkeypress="return noNumbers(event)">
                                                        </dxe:ASPxTextBox>--%>
                                                  <asp:TextBox runat="server" Width="220px" ID="txtrate" onkeypress="return noNumbers(event)" MaxLength="19"></asp:TextBox>
                                                </td>
                                              </tr>
                                              <tr>
                                                  
                                                  <td class="Ecoheadtxt" >
                                                    <dxe:ASPxLabel Visible="false" ID="ASPxLabel90" runat="server" Text="Min Amt:" Width="93px"></dxe:ASPxLabel>
                                                </td>
                                                <td class="Ecoheadtxt" style="text-align: left">
                                           <%--       <dxe:ASPxTextBox runat="server" Width="150px"  ID="txtminamt" onkeypress="return noNumbers(event)">
                                                    </dxe:ASPxTextBox>--%>
                                                    
                                                    <asp:TextBox Visible="false" runat="server" Width="150px"  ID="txtminamt" onkeypress="return noNumbers(event)" MaxLength="19"></asp:TextBox>
                                                  </td>
                                                 
                                              </tr>
                                              <tr>
                                                  <td></td>
                                                    <td colspan="3" class="Ecoheadtxt">
                                                    <asp:Button id="btnSave" runat="server" style="cursor:pointer" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click"/>
                                                    </td>
                                             </tr>        
                                            
                                        </table>
                                    </td>
                                </tr>
                               
                              
                            </table>
          
                    </td>
                </tr>
<%--                <tr>
                    <td style="height: 8px">
                        <table style="width: 100%;">
                            <tr>
                                <td align="right" style="width: 843px">
                                    <asp:HiddenField ID="hdReferenceBy" runat="server" />
                                    <table>
                                        <tr>
                                            <td style="height: 43px">
                                             <asp:Button id="btnSave" runat="server" style="cursor:pointer" Text="Save" OnClick="btnSave_Click" OnClientClick="return aa();"/>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>--%>
            </table>
        </div>
   
    </asp:Content>