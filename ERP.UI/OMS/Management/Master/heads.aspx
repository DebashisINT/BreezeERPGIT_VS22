<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.Master.management_master_heads" Codebehind="heads.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />

    
    <!--___________________These files are for List Items__________________________-->

    <script type="text/javascript" src="/assests/js/init.js"></script>

    <script type="text/javascript" src="/assests/js/ajax-dynamic-list.js"></script>

    <!--___________________________________________________________________________-->
    <style type="text/css">
	
	/* Big box with list of options */
	#ajax_listOfOptions{
		position:absolute;	    /* Never change this one */
		width:50px;	        /* Width of box */
		height:auto;	        /* Height of box */
		overflow:auto;	        /* Scrolling features */
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
		z-index:900;
	}
	#ajax_listOfOptions_iframe{
		background-color:#F00;
		position:absolute;
		z-index:10;
	}
	
	form{
		display:inline;
	}
	
	</style>
    <style type="text/css">
       a:hover {background:#ffffff; text-decoration:none;} /*BG color is a must for IE6*/
       a.tooltip span {display:none; padding:2px 3px; font-size:smaller; margin-left:1px; width:100px;}
       a.tooltip:hover span{display:inline; position:horizontal; background:#ffffff; border:1px solid #cccccc; color:#6c6c6c; width:100px}
    
    </style>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div>
            <table class="TableMain100">
                <tr>
                    <td >
                    </td>
                </tr>
                <tr>
                    <td align="center"> 
                            <table style="width:100px; z-index:101;border:solid 1px white;">
                                            <tr>
                                                <td class="Ecoheadtxt" >
                                                    <dxe:ASPxLabel ID="ASPxLabel1" runat="server" Text="Charge Code:" width="100px" ></dxe:ASPxLabel>
                                                </td>
                                                <td class="Ecoheadtxt" style="text-align: left;">
                                                   <dxe:ASPxTextBox runat="server" Width="150px" ID="txtchargename"  >
                                                       <ValidationSettings ValidationGroup="a">
                                                       </ValidationSettings>
                                                   </dxe:ASPxTextBox>
                                                                                                 
                                                </td>
                                                
                                                   <td class="Ecoheadtxt"  >
                                                    <dxe:ASPxLabel ID="ASPxLabel28" runat="server" Text="Basis:" width="78px" ></dxe:ASPxLabel>
                                                </td>
                                                <td class="Ecoheadtxt" style="text-align: left;">
                                             
                                                   <dxe:ASPxComboBox runat="server" Width="160px" ID="ddlchargeon" DropDownStyle="DropDown" EnableSynchronization="False" EnableIncrementalFiltering="True">
                                                                                                            
                                                                   <Items>
                                                                        <dxe:ListEditItem Text="Turnover" Value="Turnover">
                                                                         </dxe:ListEditItem>
                                                                         <dxe:ListEditItem Text="Delivery Turnover" Value="Delivery Turnover">
                                                                         </dxe:ListEditItem>
                                                                         <dxe:ListEditItem Text="Sqroff Turnover" Value="Sqroff Turnover">
                                                                         </dxe:ListEditItem>
                                                                            <dxe:ListEditItem Text="Brokerage" Value="Brokerage">
                                                                         </dxe:ListEditItem>
                                                                            <dxe:ListEditItem Text="Delivery Brokerage" Value="Delivery Brokerage">
                                                                         </dxe:ListEditItem>
                                                                            <dxe:ListEditItem Text="Sqroff Brokerage" Value="Sqroff Brokerage">
                                                                         </dxe:ListEditItem>
                                                                            <dxe:ListEditItem Text="Sqroff Profits"  Value="Sqroff Profits">
                                                                         </dxe:ListEditItem>
                                                                            <dxe:ListEditItem Text="Sqroff Loss" Value="Sqroff Loss">
                                                                         </dxe:ListEditItem>
                                                                            <dxe:ListEditItem Text="Exposure" Value="Exposure">
                                                                         </dxe:ListEditItem>
                                                                            <dxe:ListEditItem Text="Daily Margin" Value="Daily Margin">
                                                                         </dxe:ListEditItem>
                                                                            <dxe:ListEditItem Text="Margin Shortage" Value="Margin Shortage">
                                                                         </dxe:ListEditItem>
                                                                            <dxe:ListEditItem Text="MTM Profits" Value="MTM Profits">
                                                                         </dxe:ListEditItem> 
                                                                            <dxe:ListEditItem Text="MTM Losses" Value="MTM Losses">
                                                                         </dxe:ListEditItem> 
                                                                            <dxe:ListEditItem Text="Future Final Settlement" Value="Future Final Settlement">
                                                                         </dxe:ListEditItem> 
                                                                            <dxe:ListEditItem Text="Options Exercise" Value="Options Exercise">
                                                                         </dxe:ListEditItem> 
                                                                            <dxe:ListEditItem Text="Options Assignments" Value="Options Assignments">
                                                                         </dxe:ListEditItem>   
                                                                            <dxe:ListEditItem Text="Options Final Settlement" Value="Options Final Settlement">
                                                                         </dxe:ListEditItem>                  
                                                                   </Items>
                   
                                                     </dxe:ASPxComboBox>
                                                    
                                                </td>
                                                                             
                                            </tr>
                                            
                                            <tr>
                                            <td></td>
                                              <td style="text-align: left; ">
                                                </td>
                                                <td colspan="3" style="text-align: right; height:1px"></td>
                                            
                                            </tr>
                                           
                                              <tr>
                                                 <td class="Ecoheadtxt" >
                                                    <dxe:ASPxLabel ID="ASPxLabel29" runat="server" Text="Frquency:"></dxe:ASPxLabel>
                                                </td>
                                                <td class="Ecoheadtxt" style="text-align: left;">
                                                     <dxe:ASPxComboBox runat="server" Width="130px" ID="ddlfreq" DropDownStyle="DropDown" EnableSynchronization="False" EnableIncrementalFiltering="True">
                                                           <ClientSideEvents SelectedIndexChanged="function(s, e) { OnTaxChanged(s); }"></ClientSideEvents>
                                                                 
                                                                   <Items>
                                                                        <dxe:ListEditItem Text="Daily" Value="Daily">
                                                                         </dxe:ListEditItem>
                                                                         <dxe:ListEditItem Text="Monthly" Value="Monthly">
                                                                         </dxe:ListEditItem>
                                                                          <dxe:ListEditItem Text="Quartely" Value="Quartely">
                                                                         </dxe:ListEditItem>
                                                                          <dxe:ListEditItem Text="Yearly" Value="Yearly">
                                                                         </dxe:ListEditItem>             
                                                                   </Items>
                   
                                                     </dxe:ASPxComboBox>
                                                
                                                </td>
                                                     <td class="Ecoheadtxt"  >
                                                    <dxe:ASPxLabel ID="ASPxLabel5" runat="server" Text="Charge Name:"></dxe:ASPxLabel>
                                                </td>
                                                <td class="Ecoheadtxt" style="text-align: left;" colspan="4">
                                                   <asp:TextBox ID="txtdesc" runat="server" TextMode="MultiLine" Width="200px"></asp:TextBox>
                                                </td>
                                              </tr>
                                                   <tr>
                                                     <td colspan="4" class="Ecoheadtxt" style="text-align:center; width:100%">
                                               <asp:Button id="btnSave" runat="server"  Text="Save" OnClick="btnSave_Click" CssClass="btnUpdate"  ValidationGroup="a"/>
                                            </td>
                                               </tr> 
                                        </table>
                    </td>
                </tr>

            </table>
       </div>
</asp:Content>
