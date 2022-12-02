<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" 
    Inherits="ERP.OMS.Reports.Reports_InactiveClients" EnableEventValidation="false" Codebehind="InactiveClients.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../CSS/style.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="/assests/js/init.js"></script>

    <script type="text/javascript" src="/assests/js/ajax-dynamic-list_rootfile.js"></script>

    

    <style type="text/css">
	.tableClass {
    /* border: 0px; */
    border: 1px solid #aaa !important;
    border-collapse: collapse !important;
}
.tableBorderClass {
    /* border: 0px; */
    border: 1px solid #aaa !important;

}	
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
	.grid_scroll
    {
        overflow-y: no;  
        overflow-x: scroll; 
        width:98%;
        scrollbar-base-color: #C0C0C0;
    
    }
	</style>

    <script language="javascript" type="text/javascript">
  
  
    function Page_Load()///Call Into Page Load
            {
               FieldName='btnshow';
               Hide('filter');
               height();
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
            window.frameElement.width = document.body.scrollwidth;
        }
    function Hide(obj)
            {
             document.getElementById(obj).style.display='none';
            }
    function Show(obj)
            {
             document.getElementById(obj).style.display='inline';
            }
  
  function NORECORD()
  {
      Hide('tblDisplay');
      Show('tab2');
      alert('No Record Found!!');
      height();
  }
 
   function line()
   {
     Show('tblDisplay');
     Show('filter');
     Hide('tab2');
     height();
   }
    function filter() 
        {
          
          Hide('filter');
          Hide('tblDisplay');
          Show('tab2');
          height();
        }     
function AllowNumericOnly(e)
    {
        var keycode;
        if (window.event) keycode = window.event.keyCode;
        else if (event) keycode = event.keyCode;
        else if(e) keycode = e.which;
        else return true;
        if( (keycode > 47 && keycode <= 57) )
        {
          return true;
        }
        else
        {
          return false;
        }
      return true;  
    }
      function selecttion()
        {
            var combo=document.getElementById('ddlExport');
            combo.value='Ex';
        }  
     function ChangeRowColor(rowID,rowNumber) 
        {             
         
            var gridview = document.getElementById('grdTradeRegister'); 
            var rCount = gridview.rows.length; 
            var rowIndex=1;
            var rowCount=0;
            
            if(rCount==28)
                 rowCount=25;
            else    
               rowCount=rCount-2;
            if(rowNumber>25 && rCount<28)
                rowCount=rCount-3;
            for (rowIndex; rowIndex<=rowCount; rowIndex++) 
            { 
                var rowElement = gridview.rows[rowIndex]; 
                rowElement.style.backgroundColor='#FFFFFF'
            }
            var color = document.getElementById(rowID).style.backgroundColor;
            if(color != '#ffe1ac') 
            {
                oldColor = color;
            }
            if(color == '#ffe1ac') 
            {
                document.getElementById(rowID).style.backgroundColor = oldColor;
            }
            else 
                document.getElementById(rowID).style.backgroundColor = '#ffe1ac'; 

        }
        
 
   FieldName='lstSlection';
    </script>

    <script type="text/ecmascript">   
     
    </script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="3600">
        </asp:ScriptManager>

        <script language="javascript" type="text/javascript"> 
  var prm = Sys.WebForms.PageRequestManager.getInstance(); 
   prm.add_initializeRequest(InitializeRequest); 
   prm.add_endRequest(EndRequest); 
   var postBackElement; 
   function InitializeRequest(sender, args) 
   { 
      if (prm.get_isInAsyncPostBack()) 
         args.set_cancel(true); 
            postBackElement = args.get_postBackElement(); 
         $get('UpdateProgress1').style.display = 'block'; 
   } 
   function EndRequest(sender, args) 
   { 
          $get('UpdateProgress1').style.display = 'none'; 
 
   } 
        </script>

        <table class="TableMain100">
            <tr>
                <td class="EHEADER" style="text-align: center; width: 709px;">
                    <strong><span style="color: #000099">In Active Clients </span></strong>
                </td>
                <td class="EHEADER" width="25%" id="filter">
                    <a href="javascript:void(0);" onclick="filter();"><span style="color: Blue; text-decoration: underline;
                        font-size: 8pt; font-weight: bold">Filter</span></a> ||
                    <asp:DropDownList ID="ddlExport" Font-Size="Smaller" runat="server" AutoPostBack="True"
                        Height="16px" OnSelectedIndexChanged="ddlExport_SelectedIndexChanged">
                        <asp:ListItem Selected="True" Value="Ex">Export</asp:ListItem>
                        <asp:ListItem Value="E">Excel</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <table id="tab2" border="0" cellpadding="0" cellspacing="0">
            <tr valign="top">
                <td class="gridcellleft">
                    <table class="tableClass">
                        <tr valign="top">
                            <td>
                                <table class="tableClass"cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td bgcolor="#B7CEEC">
                                            <strong>Inactive For Last :</strong>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNo" runat="server" onkeypress="return AllowNumericOnly(this);"
                                                Width="50px">30</asp:TextBox></td>
                                        <td>
                                            <asp:DropDownList ID="ddlActive" runat="server" onchange="fnActive(this.value)">
                                                <asp:ListItem Value="0">Days</asp:ListItem>
                                                <asp:ListItem Value="1">Month</asp:ListItem>
                                                <asp:ListItem Value="2">Year</asp:ListItem>
                                            </asp:DropDownList></td>
                                        <td bgcolor="#B7CEEC">
                                            <strong>AS ON :</strong>
                                        </td>
                                        <td style="vertical-align:bottom;">
                                            <dxe:ASPxDateEdit ID="dtdate" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                Font-Size="12px" Width="108px" EditFormatString="dd-MM-yyyy" ClientInstanceName="cdtdate">
                                                <dropdownbutton>
                                                    </dropdownbutton>
                                            </dxe:ASPxDateEdit>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table class="tableClass"cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td bgcolor="#B7CEEC" style="width: 109px">
                                            <strong>Segment :</strong></td>
                                        <td style="width: 53px">
                                            <asp:RadioButton ID="rdbSegAll" runat="server" GroupName="e" />
                                            All
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdbSegSelected" runat="server" Checked="True" GroupName="e" />
                                            Cuurent
                                        </td>
                                        <td>
                                            [ <span id="litSegment" runat="server" style="color: Maroon"></span>]
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table id="tab1">
                        <tr>
                            <asp:HiddenField ID="HiddenField_Segment" runat="server" />
                            <td id="td_export" class="gridcellleft" style="width: 267px; height: 42px;">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnexport" runat="server" CssClass="btnUpdate" Height="20px" Text="Export To Excel"
                                                Width="120px" OnClick="btnexport_Click" /></td>
                                        <td id="td_btnshow" class="gridcellleft">
                                            <asp:Button ID="btnshow" runat="server" CssClass="btnUpdate" Height="20px" Text="Show"
                                                Width="101px" OnClick="btnshow_Click" OnClientClick="selecttion();" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td>
                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                        <ProgressTemplate>
                            <div id='Div1' style='position: absolute; font-family: arial; font-size: 30; left: 50%;
                                top: 50%; background-color: white; layer-background-color: white; height: 80;
                                width: 150;'>
                                <table width='100' height='35' border='1' cellpadding='0' cellspacing='0' bordercolor='#C0D6E4'>
                                    <tr>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td height='25' align='center' bgcolor='#FFFFFF'>
                                                        <img src='/assests/images/progress.gif' width='18' height='18'></td>
                                                    <td height='10' width='100%' align='center' bgcolor='#FFFFFF'>
                                                        <font size='1' face='Tahoma'><strong align='center'>Please Wait..</strong></font></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </td>
            </tr>
        </table>
        <div id="tblDisplay" style="display: none;" width="100%">
            <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                <ContentTemplate>
                    <table width="100%" border="1">
                        <tr style="display: none;">
                            <td>
                                <asp:HiddenField ID="hiddencount" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div id="displayALL" runat="server">
                                </div>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnshow" EventName="Click"></asp:AsyncPostBackTrigger>
                </Triggers>
            </asp:UpdatePanel>
        </div>
</asp:Content>
