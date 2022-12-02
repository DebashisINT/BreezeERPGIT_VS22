<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/OMS/MasterPage/PopUp.Master"
    Inherits="ERP.OMS.Management.Master.management_master_contact_brokerage" Codebehind="contact_brokerage.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


    <style type="text/css">
    .AccordionTitle, .AccordionContent, .AccordionContainer
{
  position:relative;
  width:950px;
}

.AccordionTitle
{
  height:20px;
  overflow:hidden;
  cursor:pointer;
  font-family:Arial;
  font-size:8pt;
  font-weight:bold;
  vertical-align:middle;
  width:99.8%;
  background-repeat:repeat-x;
  display:table-cell;
  background-color: #eee;
  -moz-user-select:none;
  border:1px solid #ddd;
  Margin:1px;
}

.AccordionContent
{
  height:0px;
  overflow:auto;
  display:block; 
}

.AccordionContainer
{
  border-top: solid 1px #C1C1C1;
  border-bottom: solid 1px #C1C1C1;
  border-left: solid 2px #C1C1C1;
  border-right: solid 2px #C1C1C1;
}
    </style>
</asp:Content>

   
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<%--<body style="margin: 0px 0px 0px 0px; background-color: #DDECFE" onload="runAccordion(1)">--%>

    <script language="javascript" type="text/javascript">
//     Date.prototype.yyyymmdd = function() {
//       var yyyy = this.getFullYear().toString();
//       var mm = (this.getMonth()+1).toString(); // getMonth() is zero-based
//       var dd  = this.getDate().toString();
//       return yyyy + (mm[1]?mm:"0"+mm[0]) + (dd[1]?dd:"0"+dd[0]); // padding
//      };
//function Page_Load()
//{
// var my_date=new Date('<% =Session["ServerDate"] %>');
//                var monthnumber = my_date.getMonth();
//                var monthday    = my_date.getDate();
//                var year        = my_date.getYear();
//                var OnlyDate=new Date(year, monthnumber, monthday);
//               
//                dtpdate.SetDate(OnlyDate);
//}

  
    var ContentHeight = 300;
var TimeToSlide = 250.0;

var openAccordion = '';

function runAccordion(index)
{
//alert(index);

  var nID = "Accordion" + index + "Content";
  if(openAccordion == nID)
    nID = '';
    
  setTimeout("animate(" + new Date().getTime() + "," + TimeToSlide + ",'" 
      + openAccordion + "','" + nID + "')", 33);
 
  openAccordion = nID;
  

}
function animate(lastTick, timeLeft, closingId, openingId)
{  

  var curTick = new Date().getTime();
  var elapsedTicks = curTick - lastTick;
  
  var opening = (openingId == '') ? null : document.getElementById(openingId);
  var closing = (closingId == '') ? null : document.getElementById(closingId);
 
  if(timeLeft <= elapsedTicks)
  {
  
    if(opening != null)
      opening.style.height = ContentHeight + 'px';
    
    if(closing != null)
    {
      closing.style.display = 'none';
      closing.style.height = '0px';
    }
    
    if ((document.getElementById('T1').innerHTML=='[ + ]') &&(document.getElementById('Accordion1Content').style.display=='block'))
        {
        
        document.getElementById('T1').innerHTML="[ - ]";
        
        }
     else
        {
        
        document.getElementById('T1').innerHTML="[ + ]";
        }
        
        if ((document.getElementById('T2').innerHTML=='[ + ]') &&(document.getElementById('Accordion2Content').style.display=='block'))
        {
        
        document.getElementById('T2').innerHTML="[ - ]";
        
        }
     else
        {
        
        document.getElementById('T2').innerHTML="[ + ]";
        }
         if ((document.getElementById('T3').innerHTML=='[ + ]') &&(document.getElementById('Accordion3Content').style.display=='block'))
        {
        
        document.getElementById('T3').innerHTML="[ - ]";
        
        }
     else
        {
        
        document.getElementById('T3').innerHTML="[ + ]";
        }
//         if ((document.getElementById('T4').innerHTML=='[ + ]') &&(document.getElementById('Accordion4Content').style.display=='block'))
//        {
//        
//        document.getElementById('T4').innerHTML="[ - ]";
//        
//        }
//     else
//        {
//        
//        document.getElementById('T4').innerHTML="[ + ]";
//        }
    
      
    
    return;
  }
 
  timeLeft -= elapsedTicks;
  var newClosedHeight = Math.round((timeLeft/TimeToSlide) * ContentHeight);

  if(opening != null)
  {
 
    if(opening.style.display != 'block')
      opening.style.display = 'block';
    opening.style.height = (ContentHeight - newClosedHeight) + 'px';
     
    
  }
  
  if(closing != null)
    closing.style.height = newClosedHeight + 'px';

  setTimeout("animate(" + curTick + "," + timeLeft + ",'" 
      + closingId + "','" + openingId + "')", 33);


}
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
       // document.location.href="contact_brokerage.aspx"; 
        }
        else if ( name == "tab6")
        {
        //alert(name);
            document.location.href="contact_other.aspx"; 
        }
        else if(name=="tab13")
        {
             document.location.href="contact_Subscription.aspx";
        }
         
    }
    function BrokerageMode(obj)
    {
        if(obj=='0')
        {
            document.getElementById("trScheme").style.display="none";
            document.getElementById("trDate").style.display="inline";
        }
        else    
        {   
            if(obj=='1')
            {
                document.getElementById("div1").style.display="inline";
                document.getElementById("div2").style.display="none";
            }
            else
            {
                document.getElementById("div1").style.display="none";
                document.getElementById("div2").style.display="inline";
            }         
            document.getElementById("trScheme").style.display="inline";
            document.getElementById("trDate").style.display="inline";
        }
    }
    function change()
    {
        compSegment.PerformCallback();
    }
    function ShowError(obj)
    {
        if(obj!="" && obj!="ADD")
        {
       
            var brokObj=obj.split('~')
            
            if(brokObj[0,0]!='3')
                BrokerageMode(brokObj[0,0])
            else
            {
                if(brokObj[0,9]=='0')
                {
                    objAdd=brokObj[0,1]+'|'+brokObj[0,2]+'|'+brokObj[0,3]+'|'+brokObj[0,4]+'|'+brokObj[0,5]+'|'+brokObj[0,6]+'|'+brokObj[0,7]+'|'+brokObj[0,8];                    
                   // var url='BrokerageMain.aspx?id='+objAdd; 
                   clName=brokObj[0,5];
                   compName=brokObj[0,6]+' ['+brokObj[0,7]+']'     
                    var url='BrokerageBridge.aspx?id='+brokObj[0,2]+' &Cid='+brokObj[0,3]+' &Sid='+brokObj[0,4]+' &Addurl='+objAdd+' &BT=1';           
                     //parent.OnMoreInfoClick(url,"Add BrokerageMain",'300px','150px',"Y");
                    editwin=dhtmlmodal.open("Editbox", "iframe", url, "Add BrokerageMain", "width=470px,height=250px,center=1,resize=1,top=500", "recal")                 
                }
            }
        }
        if (obj=="ADD")
        {
        
//        var now = new Date();
//        now.format("mm/dd/yyyy");
//        dtpdate.SetDate (now, "mm , dd, yyyy");
        //dtpdate.SetDate(new Date());
        //dtpdate.SetDate (new Date("Date, Month, Year"));
        
        
               // var my_date==new Date();
              
                var my_date=new Date('<% =Session["ServerDate"] %>');
                var monthnumber = my_date.getMonth();
                var monthday    = my_date.getDate();
                var year        = my_date.getYear();
                var OnlyDate=new Date(year, monthnumber, monthday);
               
                dtpdate.SetDate(OnlyDate);
        }
    }
    

    function FillValues(objVal,objCusID,objCompID,objSegID)
    {
        editwin.close();
        var Grp='C';
        var objCharge='T'
        var url='BrokerageMain.aspx?id='+objVal+' &charge='+objCharge+' &CusID='+objCusID+' &CompID='+objCompID+' &SegID='+objSegID+' &Grp='+Grp+' &clName='+clName;
       //parent.OnMoreInfoClick(url,"Edit BrokerageMain Details",'940px','450px',"Y");
        editwin1=dhtmlmodal.open("Editbox", "iframe", url, "Edit BrokerageMain Details", "width=960px,height=410px,center=1,resize=1,top=500", "recal")
        
    }
    function FillValuesWithAdd(objVal)
    {
        editwin.close();
        var url='BrokerageMain.aspx?id='+objAdd;
       //parent.OnMoreInfoClick(url,"Edit BrokerageMain Details",'940px','450px',"Y");
        editwin1=dhtmlmodal.open("Editbox", "iframe", url, "Edit BrokerageMain Details", "width=960px,height=410px,center=1,resize=1,top=500", "recal")
        
    }
    function Profile()
    {
        compCode.PerformCallback();
    }
    </script>

  
        <div>
            <table class="TableMain100">
                <tr>
                    <td style="text-align: center">
                        <asp:Label ID="lblName" runat="server" Font-Bold="True"></asp:Label>
                    </td>
                </tr>
            </table>
            <table class="TableMain100">
                <tr>
                    <td class="EHEADER">
                    </td>
                </tr>
                <tr>
                    <td>
                        <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="11" ClientInstanceName="page"
                            Font-Size="12px">
                            <TabPages>
                                <dxe:TabPage Name="General">
                                    <TabTemplate>
                                        <span style="font-size: x-small">General</span>&nbsp;<span style="color: Red;">*</span>
                                    </TabTemplate>
                                    <ContentCollection>
                                        <dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
                                    </ContentCollection>
                                </dxe:TabPage>
                                <dxe:TabPage Name="CorresPondence">
                                    <TabTemplate>
                                        <span style="font-size: x-small">CorresPondence</span>&nbsp;<span style="color: Red;">*</span>
                                    </TabTemplate>
                                    <ContentCollection>
                                        <dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
                                    </ContentCollection>
                                </dxe:TabPage>
                                <dxe:TabPage Name="Bank Details">
                                    <TabTemplate>
                                        <span style="font-size: x-small">Bank</span>&nbsp;<span style="color: Red;">*</span>
                                    </TabTemplate>
                                    <ContentCollection>
                                        <dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
                                    </ContentCollection>
                                </dxe:TabPage>
                                <dxe:TabPage Name="DP Details">
                                    <TabTemplate>
                                        <span style="font-size: x-small">DP</span>&nbsp;<span style="color: Red;">*</span>
                                    </TabTemplate>
                                    <ContentCollection>
                                        <dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
                                    </ContentCollection>
                                </dxe:TabPage>
                                <dxe:TabPage Name="Documents">
                                    <TabTemplate>
                                        <span style="font-size: x-small">Documents</span>&nbsp;<span style="color: Red;">*</span>
                                    </TabTemplate>
                                    <ContentCollection>
                                        <dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
                                    </ContentCollection>
                                </dxe:TabPage>
                                <dxe:TabPage Name="Registration">
                                    <TabTemplate>
                                        <span style="font-size: x-small">Registration</span>&nbsp;<span style="color: Red;">*</span>
                                    </TabTemplate>
                                    <ContentCollection>
                                        <dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
                                    </ContentCollection>
                                </dxe:TabPage>
                                <dxe:TabPage Name="Other">
                                    <TabTemplate>
                                        <span style="font-size: x-small">Other</span>&nbsp;<span style="color: Red;">*</span>
                                    </TabTemplate>
                                    <ContentCollection>
                                        <dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
                                    </ContentCollection>
                                </dxe:TabPage>
                                <dxe:TabPage Name="Group Member" Text="Group">
                                    <ContentCollection>
                                        <dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
                                    </ContentCollection>
                                </dxe:TabPage>
                                <dxe:TabPage Name="Deposit" Text="Deposit">
                                    <ContentCollection>
                                        <dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
                                    </ContentCollection>
                                </dxe:TabPage>
                                <dxe:TabPage Name="Remarks" Text="Remarks">
                                    <ContentCollection>
                                        <dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
                                    </ContentCollection>
                                </dxe:TabPage>
                                <dxe:TabPage Name="Education" Text="Education">
                                    <ContentCollection>
                                        <dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
                                    </ContentCollection>
                                </dxe:TabPage>
                                <dxe:TabPage Name="Trad. Prof." Text="Trad. Prof.">
                                    <ContentCollection>
                                        <dxe:ContentControl runat="server">
                                            <div id="AccordionContainer" class="AccordionContainer">
                                                <div onclick="runAccordion(1);">
                                                    <div class="AccordionTitle" onselectstart="return false;">
                                                        Brokerage Details <span id="T1">[ + ]</span>
                                                    </div>
                                                </div>
                                                <div id="Accordion1Content" class="AccordionContent">
                                                    <dxe:ASPxGridView ID="gridBrokerage" ClientInstanceName="gridBrokerage" KeyFieldName="ChargeGroupMembers_ID"
                                                        runat="server" Width="100%" AutoGenerateColumns="False" OnRowInserting="gridBrokerage_RowInserting"
                                                        OnStartRowEditing="gridBrokerage_StartRowEditing" OnRowUpdating="gridBrokerage_RowUpdating"
                                                        OnCustomJSProperties="gridBrokerage_CustomJSProperties" OnRowValidating="gridBrokerage_RowValidating"
                                                        OnHtmlEditFormCreated="gridBrokerage_HtmlEditFormCreated" OnCommandButtonInitialize="gridbrokerage_CommandButtonInitialize"
                                                        OnRowDeleting="gridBrokerage_RowDeleting">
                                                        <Columns>
                                                            <dxe:GridViewDataTextColumn Caption="ID" FieldName="ChargeGroupMembers_ID" Visible="False"
                                                                VisibleIndex="0">
                                                                <CellStyle CssClass="gridcellleft">
                                                                </CellStyle>
                                                                <EditFormSettings Visible="False" />
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataComboBoxColumn Caption="CompanyName" FieldName="ChargeGroupMembers_CompanyID"
                                                                VisibleIndex="1" Visible="false">
                                                                <PropertiesComboBox DataSourceID="SqlComp" ValueField="cmp_internalid" TextField="cmp_name"
                                                                    ValueType="System.String">
                                                                </PropertiesComboBox>
                                                                <CellStyle CssClass="gridcellleft">
                                                                </CellStyle>
                                                                <EditFormSettings Visible="False" />
                                                            </dxe:GridViewDataComboBoxColumn>
                                                            <dxe:GridViewDataComboBoxColumn Caption="Segment" FieldName="ChargeGroupMembers_SegmentID"
                                                                VisibleIndex="2">
                                                                <PropertiesComboBox DataSourceID="SqlSegment1" ValueField="exch_internalId" TextField="Exchange"
                                                                    ValueType="System.String">
                                                                </PropertiesComboBox>
                                                                <CellStyle CssClass="gridcellleft">
                                                                </CellStyle>
                                                                <EditFormSettings Visible="False" />
                                                            </dxe:GridViewDataComboBoxColumn>
                                                            <dxe:GridViewDataComboBoxColumn Caption="Type" FieldName="ChargeGroupMembers_GroupType"
                                                                VisibleIndex="3">
                                                                <PropertiesComboBox ValueType="System.String">
                                                                    <Items>
                                                                        <dxe:ListEditItem Text="Specific" Value="1" />
                                                                        <dxe:ListEditItem Text="Brokerage Scheme" Value="2" />
                                                                        <dxe:ListEditItem Text="Brokerage Card" Value="3" />
                                                                    </Items>
                                                                </PropertiesComboBox>
                                                                <CellStyle CssClass="gridcellleft">
                                                                </CellStyle>
                                                                <EditFormSettings Visible="False" />
                                                            </dxe:GridViewDataComboBoxColumn>
                                                            <dxe:GridViewDataComboBoxColumn Caption="Code" FieldName="ChargeGroupMembers_GroupCode"
                                                                VisibleIndex="4">
                                                                <PropertiesComboBox DataSourceID="SqlCode" ValueField="ChargeGroup_ID" TextField="ChargeGroup_Name"
                                                                    ValueType="System.String">
                                                                </PropertiesComboBox>
                                                                <CellStyle CssClass="gridcellleft">
                                                                </CellStyle>
                                                                <EditFormSettings Visible="False" />
                                                            </dxe:GridViewDataComboBoxColumn>
                                                            <dxe:GridViewDataTextColumn Caption="From Date" FieldName="ChargeGroupMembers_FromDate"
                                                                VisibleIndex="5" Visible="False">
                                                                <CellStyle CssClass="gridcellleft">
                                                                </CellStyle>
                                                                <EditFormSettings Visible="False" />
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataTextColumn Caption="Untill Date" FieldName="ChargeGroupMembers_UntilDate"
                                                                VisibleIndex="6" Visible="False">
                                                                <CellStyle CssClass="gridcellleft">
                                                                </CellStyle>
                                                                <EditFormSettings Visible="False" />
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataTextColumn Caption="From Date" FieldName="ChargeGroupMembers_FromDate1"
                                                                VisibleIndex="7">
                                                                <CellStyle CssClass="gridcellleft">
                                                                </CellStyle>
                                                                <EditFormSettings Visible="False" />
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataTextColumn Caption="Untill Date" FieldName="ChargeGroupMembers_UntilDate1"
                                                                VisibleIndex="8">
                                                                <CellStyle CssClass="gridcellleft">
                                                                </CellStyle>
                                                                <EditFormSettings Visible="False" />
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewCommandColumn VisibleIndex="14" ShowEditButton="true">
                                                                
                                                            </dxe:GridViewCommandColumn>
                                                            <dxe:GridViewCommandColumn VisibleIndex="15" ShowDeleteButton="true">
                                                             
                                                                <HeaderTemplate>
                                                                    <a href="javascript:void(0);" onclick="gridBrokerage.AddNewRow()"><span >Add New</span></a>
                                                                </HeaderTemplate>
                                                            </dxe:GridViewCommandColumn>
                                                        </Columns>
                                                        	<SettingsCommandButton>
                                                                      <DeleteButton Text="Delete">
                                                                </DeleteButton>
                                                                        <EditButton  Text="Edit">
                                                                        </EditButton>
                                                                </SettingsCommandButton>
                                                        <Settings ShowTitlePanel="false" />
                                                        <SettingsText ConfirmDelete="Confirm delete?" />
                                                        <SettingsPager NumericButtonCount="20" PageSize="20" ShowSeparators="True" AlwaysShowPager="True">
                                                            <FirstPageButton Visible="True">
                                                            </FirstPageButton>
                                                            <LastPageButton Visible="True">
                                                            </LastPageButton>
                                                        </SettingsPager>
                                                        <SettingsBehavior AllowFocusedRow="true" ConfirmDelete="True" />
                                                        <Styles>
                                                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                            </Header>
                                                            <LoadingPanel ImageSpacing="10px">
                                                            </LoadingPanel>
                                                        </Styles>
                                                        <StylesEditors>
                                                            <ProgressBar Height="25px">
                                                            </ProgressBar>
                                                        </StylesEditors>
                                                        <ClientSideEvents EndCallback="function(s,e) { ShowError(s.cpInsertError);
                                                                                                 }" />
                                                        <Templates>
                                                            <EditForm>
                                                                <table style="text-align: center">
                                                                    <tr>
                                                                        <td style="width: 30%">
                                                                        </td>
                                                                        <td class="Ecoheadtxt" style="text-align: left; width: 10%">
                                                                            Company :
                                                                        </td>
                                                                        <td style="text-align: left">
                                                                            <dxe:ASPxComboBox ID="compCompany" runat="server" DataSourceID="SqlComp" ValueField="cmp_internalid"
                                                                                TextField="cmp_Name" ClientInstanceName="compCompany" EnableIncrementalFiltering="true"
                                                                                EnableSynchronization="False" OnCallback="compSegment_Callback" ValueType="System.String"
                                                                                Width="250px">
                                                                                <ButtonStyle Width="13px">
                                                                                </ButtonStyle>
                                                                                <ClientSideEvents SelectedIndexChanged="function(s,e){change();}" />
                                                                            </dxe:ASPxComboBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 30%">
                                                                        </td>
                                                                        <td class="Ecoheadtxt" style="text-align: left; width: 10%">
                                                                            Segment :
                                                                        </td>
                                                                        <td style="text-align: left">
                                                                            <dxe:ASPxComboBox ID="compSegment" runat="server" DataSourceID="SqlSegment" ValueField="exch_internalId"
                                                                                TextField="Exchange" ClientInstanceName="compSegment" EnableIncrementalFiltering="true"
                                                                                EnableSynchronization="False" OnCallback="compSegment_Callback" ValueType="System.String"
                                                                                Width="250px">
                                                                                <ButtonStyle Width="13px">
                                                                                </ButtonStyle>
                                                                            </dxe:ASPxComboBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 30%">
                                                                        </td>
                                                                        <td class="Ecoheadtxt" style="text-align: left; width: 10%">
                                                                            Mode :
                                                                        </td>
                                                                        <td style="text-align: left">
                                                                            <dxe:ASPxComboBox ID="compMode" runat="server" EnableIncrementalFiltering="true"
                                                                                Width="250px" EnableSynchronization="False" ValueType="System.String">
                                                                                <Items>
                                                                                    <dxe:ListEditItem Text="Specific" Value="1" />
                                                                                    <dxe:ListEditItem Text="Brokerage Scheme" Value="2" />
                                                                                    <dxe:ListEditItem Text="Brokerage Card" Value="3" />
                                                                                </Items>
                                                                                <ClientSideEvents ValueChanged="function(s,e){
                                                                                                       var indexr = s.GetSelectedIndex();
                                                                                                       BrokerageMode(indexr)
                                                                                                       }" />
                                                                                <ButtonStyle Width="13px">
                                                                                </ButtonStyle>
                                                                            </dxe:ASPxComboBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trScheme">
                                                                        <td style="width: 30%">
                                                                        </td>
                                                                        <td class="Ecoheadtxt" style="text-align: left; width: 10%">
                                                                            <div id="div1" class="Ecoheadtxt" style="text-align: left;">
                                                                                Group Code :</div>
                                                                            <div id="div2" class="Ecoheadtxt" style="display: none">
                                                                                Code Scheme :</div>
                                                                        </td>
                                                                        <td style="text-align: left">
                                                                            <dxe:ASPxComboBox ID="comboCode" runat="server" Width="250px" DataSourceID="SqlCode"
                                                                                ValueField="ChargeGroup_ID" TextField="ChargeGroup_Name" EnableIncrementalFiltering="true"
                                                                                EnableSynchronization="False" OnCallback="compSegment_Callback" ValueType="System.String">
                                                                                <ButtonStyle Width="13px">
                                                                                </ButtonStyle>
                                                                            </dxe:ASPxComboBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trDate">
                                                                        <td style="width: 30%">
                                                                        </td>
                                                                        <td class="Ecoheadtxt" style="text-align: left; width: 10%;">
                                                                            Effective From :
                                                                        </td>
                                                                        <td style="text-align: left">
                                                                            <dxe:ASPxDateEdit ID="dtDate" ClientInstanceName="dtpdate" Width="250px" runat="server"
                                                                                Value='<%# Bind("ChargeGroupMembers_FromDate") %>' EditFormat="Custom" EditFormatString="dd-MM-yyyy"
                                                                                UseMaskBehavior="True">
                                                                                <ButtonStyle Width="13px">
                                                                                </ButtonStyle>
                                                                            </dxe:ASPxDateEdit>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 30%">
                                                                        </td>
                                                                        <td style="text-align: right;">
                                                                            <dxe:ASPxButton ID="btnUpdate" runat="server" Text="Save" ToolTip="Update data" Height="18px"
                                                                                Width="88px" AutoPostBack="False">
                                                                                <ClientSideEvents Click="function(s, e) {gridBrokerage.UpdateEdit();}" />
                                                                            </dxe:ASPxButton>
                                                                        </td>
                                                                        <td style="text-align: left;" colspan="2">
                                                                            <dxe:ASPxButton ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel data"
                                                                                Height="18px" Width="88px" AutoPostBack="False">
                                                                                <ClientSideEvents Click="function(s, e) {gridBrokerage.CancelEdit();}" />
                                                                            </dxe:ASPxButton>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </EditForm>
                                                        </Templates>
                                                    </dxe:ASPxGridView>
                                                </div>
                                                <div onclick="runAccordion(2);">
                                                <div class="AccordionTitle" onselectstart="return false;">
                                                   <%--<span style="width:400px; float:left;"> &nbsp; </span>--%> 
                                                   Trading Profile Details
                                                    <span id="T2">[ + ]</span>
                                                </div>
                                            </div>
                                            <div id="Accordion2Content" class="AccordionContent">
                                                <dxe:ASPxGridView ID="gridTrading" ClientInstanceName="gridTrading" KeyFieldName="ProfileMember_ID"
                                                    runat="server" Width="100%" AutoGenerateColumns="False" OnRowInserting="gridTrading_RowInserting"
                                                    OnRowUpdating="gridTrading_RowUpdating" OnStartRowEditing="gridTrading_StartRowEditing"
                                                    OnCommandButtonInitialize="gridTrading_CommandButtonInitialize" OnRowDeleting="gridTrading_RowDeleting"
                                                    OnRowValidating="gridTrading_RowValidating">
                                                    <Columns>
                                                        <dxe:GridViewDataTextColumn Caption="ID" FieldName="ProfileMember_ID" Visible="False"
                                                            VisibleIndex="0">
                                                            <CellStyle CssClass="gridcellleft">
                                                            </CellStyle>
                                                            <EditFormSettings Visible="False" />
                                                        </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewDataComboBoxColumn Caption="Profile Type" FieldName="ProfileMember_Type"
                                                            VisibleIndex="1">
                                                            <PropertiesComboBox ValueType="System.String">
                                                                <Items>
                                                                    <dxe:ListEditItem Text="Risk & Exposure" Value="1" />
                                                                    <dxe:ListEditItem Text="Delivery" Value="2" />
                                                                    <dxe:ListEditItem Text="Funds" Value="3" />
                                                                </Items>
                                                            </PropertiesComboBox>
                                                            <CellStyle CssClass="gridcellleft">
                                                            </CellStyle>
                                                            <EditFormSettings Visible="False" />
                                                        </dxe:GridViewDataComboBoxColumn>
                                                        <dxe:GridViewDataComboBoxColumn Caption="Profile Code" FieldName="ProfileMember_Code"
                                                            VisibleIndex="2">
                                                            <PropertiesComboBox DataSourceID="SqlPPCode1" ValueField="TradingProfile_Code" TextField="TradingProfile_Name"
                                                                ValueType="System.String">
                                                            </PropertiesComboBox>
                                                            <CellStyle CssClass="gridcellleft">
                                                            </CellStyle>
                                                            <EditFormSettings Visible="False" />
                                                        </dxe:GridViewDataComboBoxColumn>
                                                        <dxe:GridViewDataTextColumn Caption="From Date" FieldName="ProfileMember_DateFrom"
                                                            VisibleIndex="3" Visible="False">
                                                            <CellStyle CssClass="gridcellleft">
                                                            </CellStyle>
                                                            <EditFormSettings Visible="False" />
                                                        </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewDataTextColumn Caption="From Date" FieldName="ProfileMember_DateFrom1"
                                                            VisibleIndex="4">
                                                            <CellStyle CssClass="gridcellleft">
                                                            </CellStyle>
                                                            <EditFormSettings Visible="False" />
                                                        </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewDataTextColumn Caption="Untill Date" FieldName="ProfileMember_DateTo1"
                                                            VisibleIndex="5">
                                                            <CellStyle CssClass="gridcellleft">
                                                            </CellStyle>
                                                            <EditFormSettings Visible="False" />
                                                        </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewCommandColumn VisibleIndex="6" ShowEditButton="true"
>
                                                           
                                                        </dxe:GridViewCommandColumn>
                                                        <dxe:GridViewCommandColumn VisibleIndex="7" ShowDeleteButton="true">
                                                           
                                                            <HeaderTemplate>
                                                                <a href="javascript:void(0);" onclick="gridTrading.AddNewRow()"><span >Add New</span></a>
                                                            </HeaderTemplate>
                                                        </dxe:GridViewCommandColumn>
                                                    </Columns>
                                                    
  								<SettingsCommandButton><DeleteButton Text="Delete">
                                                            </DeleteButton>
                                                                      
                                                                        <EditButton  Text="Edit">
                                                                        </EditButton>
                                                                </SettingsCommandButton>
                                                    <Settings ShowTitlePanel="false" />
                                                    <SettingsText ConfirmDelete="Confirm delete?" />
                                                    <SettingsPager NumericButtonCount="20" PageSize="20" ShowSeparators="True" AlwaysShowPager="True">
                                                        <FirstPageButton Visible="True">
                                                        </FirstPageButton>
                                                        <LastPageButton Visible="True">
                                                        </LastPageButton>
                                                    </SettingsPager>
                                                    <SettingsBehavior AllowFocusedRow="true" ConfirmDelete="True" />
                                                    <Styles>
                                                        <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                        </Header>
                                                        <LoadingPanel ImageSpacing="10px">
                                                        </LoadingPanel>
                                                    </Styles>
                                                    <StylesEditors>
                                                        <ProgressBar Height="25px">
                                                        </ProgressBar>
                                                    </StylesEditors>
                                                    <Templates>
                                                        <EditForm>
                                                            <table style="text-align: center">
                                                                <tr>
                                                                    <td style="width: 30%">
                                                                    </td>
                                                                    <td class="Ecoheadtxt" style="text-align: left; width: 10%">
                                                                        Profile Type :
                                                                    </td>
                                                                    <td style="text-align: left">
                                                                        <dxe:ASPxComboBox ID="compType" runat="server" EnableIncrementalFiltering="true"
                                                                            EnableSynchronization="False" ValueType="System.String">
                                                                            <Items>
                                                                                <dxe:ListEditItem Text="Risk & Exposer" Value="1" />
                                                                                <dxe:ListEditItem Text="Delivery" Value="2" />
                                                                                <dxe:ListEditItem Text="Funds" Value="3" />
                                                                            </Items>
                                                                            <ClientSideEvents SelectedIndexChanged="function(s,e){Profile()}" />
                                                                            <ButtonStyle Width="13px">
                                                                            </ButtonStyle>
                                                                        </dxe:ASPxComboBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 30%">
                                                                    </td>
                                                                    <td class="Ecoheadtxt" style="text-align: left; width: 10%">
                                                                        Profile Code :
                                                                    </td>
                                                                    <td style="text-align: left">
                                                                        <dxe:ASPxComboBox ID="compCode" runat="server" DataSourceID="SqlPPCode" ValueField="TradingProfile_Code"
                                                                            TextField="TradingProfile_Name" ClientInstanceName="compCode" EnableIncrementalFiltering="true"
                                                                            EnableSynchronization="False" ValueType="System.String" OnCallback="compCode_Callback">
                                                                            <ButtonStyle Width="13px">
                                                                            </ButtonStyle>
                                                                        </dxe:ASPxComboBox>
                                                                    </td>
                                                                </tr>
                                                                <tr id="trDate">
                                                                    <td style="width: 30%">
                                                                    </td>
                                                                    <td class="Ecoheadtxt" style="text-align: left; width: 10%">
                                                                        W.C.F :
                                                                    </td>
                                                                    <td style="text-align: left">
                                                                        <dxe:ASPxDateEdit ID="dDate" ClientInstanceName="dtp8date" runat="server" Value='<%# Bind("ProfileMember_DateFrom") %>'
                                                                            EditFormat="Custom" EditFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                                                                            <ButtonStyle Width="13px">
                                                                            </ButtonStyle>
                                                                        </dxe:ASPxDateEdit>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 30%">
                                                                    </td>
                                                                    <td style="text-align: right;">
                                                                        <dxe:ASPxButton ID="btnUpdate1" runat="server" Text="Save" ToolTip="Update data"
                                                                            Height="18px" Width="88px" AutoPostBack="False">
                                                                            <ClientSideEvents Click="function(s, e) {gridTrading.UpdateEdit();}" />
                                                                        </dxe:ASPxButton>
                                                                    </td>
                                                                    <td style="text-align: left;" colspan="2">
                                                                        <dxe:ASPxButton ID="btnCancel1" runat="server" Text="Cancel" ToolTip="Cancel data"
                                                                            Height="18px" Width="88px" AutoPostBack="False">
                                                                            <ClientSideEvents Click="function(s, e) {gridTrading.CancelEdit();}" />
                                                                        </dxe:ASPxButton>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </EditForm>
                                                    </Templates>
                                                </dxe:ASPxGridView>
                                                </div>
                                                <div onclick="runAccordion(3);">
                                                <div class="AccordionTitle" onselectstart="return false;">
                                                   <%--<span style="width:400px; float:left;"> &nbsp; </span>--%>
                                                   Other Charges Details
                                                    <span id="T3">[ + ]</span>
                                                </div>
                                            </div>
                                             <div id="Accordion3Content" class="AccordionContent">
                                                <dxe:ASPxGridView ID="gridOtherCharges" OnHtmlEditFormCreated="gridOtherCharges_HtmlEditFormCreated"
                                                    ClientInstanceName="gridOtherCharges" KeyFieldName="OtherChargeMember_ID" OnRowDeleting="gridOtherCharges_RowDeleting"
                                                    runat="server" Width="100%" AutoGenerateColumns="False" OnRowInserting="gridOtherCharges_RowInserting"
                                                    OnRowUpdating="gridOtherCharges_RowUpdating" OnCommandButtonInitialize="gridOtherCharges_CommandButtonInitialize"
                                                    OnRowValidating="gridOtherCharges_RowValidating" OnStartRowEditing="gridOtherCharges_StartRowEditing">
                                                    <Columns>
                                                        <dxe:GridViewDataTextColumn Caption="ID" FieldName="OtherChargeMember_ID" Visible="False"
                                                            VisibleIndex="0">
                                                            <CellStyle CssClass="gridcellleft">
                                                            </CellStyle>
                                                            <EditFormSettings Visible="False" />
                                                        </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewDataComboBoxColumn Caption="CompanyName" FieldName="OtherChargeMember_CompanyID"
                                                            VisibleIndex="1" Visible="true">
                                                            <PropertiesComboBox DataSourceID="SqlComp" ValueField="cmp_internalid" TextField="cmp_name"
                                                                ValueType="System.String">
                                                            </PropertiesComboBox>
                                                            <CellStyle CssClass="gridcellleft">
                                                            </CellStyle>
                                                            <EditFormSettings Visible="False" />
                                                        </dxe:GridViewDataComboBoxColumn>
                                                        <dxe:GridViewDataComboBoxColumn Caption="Segment" FieldName="OtherChargeMember_SegmentID"
                                                            VisibleIndex="2">
                                                            <PropertiesComboBox DataSourceID="SqlSegment1" ValueField="exch_internalId" TextField="Exchange"
                                                                ValueType="System.String">
                                                            </PropertiesComboBox>
                                                            <CellStyle CssClass="gridcellleft">
                                                            </CellStyle>
                                                            <EditFormSettings Visible="False" />
                                                        </dxe:GridViewDataComboBoxColumn>
                                                        <dxe:GridViewDataComboBoxColumn Caption="Other Charge Code" FieldName="OtherChargeMember_OtherChargeCode"
                                                            VisibleIndex="3">
                                                            <PropertiesComboBox DataSourceID="SqlCharges" ValueField="OtherCharges_Code" TextField="OtherCharges_Name"
                                                                ValueType="System.String">
                                                            </PropertiesComboBox>
                                                            <CellStyle CssClass="gridcellleft">
                                                            </CellStyle>
                                                            <EditFormSettings Visible="False" />
                                                        </dxe:GridViewDataComboBoxColumn>
                                                        <dxe:GridViewDataTextColumn Caption="From Date" FieldName="OtherChargeMember_DateFrom"
                                                            VisibleIndex="4" Visible="False">
                                                            <CellStyle CssClass="gridcellleft">
                                                            </CellStyle>
                                                            <EditFormSettings Visible="False" />
                                                        </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewDataTextColumn Caption="From Date" FieldName="OtherChargeMember_DateFrom1"
                                                            VisibleIndex="5">
                                                            <CellStyle CssClass="gridcellleft">
                                                            </CellStyle>
                                                            <EditFormSettings Visible="False" />
                                                        </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewDataTextColumn Caption="Untill Date" FieldName="OtherChargeMember_DateTo1"
                                                            VisibleIndex="6">
                                                            <CellStyle CssClass="gridcellleft">
                                                            </CellStyle>
                                                            <EditFormSettings Visible="False" />
                                                        </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewCommandColumn VisibleIndex="7" ShowDeleteButton="true" ShowEditButton="true">
                                                           
                                                        </dxe:GridViewCommandColumn>
                                                        <dxe:GridViewCommandColumn VisibleIndex="8">
                                                           
                                                            <HeaderTemplate>
                                                                <a href="javascript:void(0);" onclick="gridOtherCharges.AddNewRow()"><span >Add New</span></a>
                                                            </HeaderTemplate>
                                                        </dxe:GridViewCommandColumn>
                                                    </Columns>
                                                    
  								<SettingsCommandButton>
                                                                      <DeleteButton  Text="Delete">
                                                                        </DeleteButton>
                                                                        <EditButton  Text="Edit">
                                                                        </EditButton>
                                                                </SettingsCommandButton>
                                                    <Settings ShowTitlePanel="false" />
                                                    <SettingsText ConfirmDelete="Confirm delete?" />
                                                    <SettingsPager NumericButtonCount="20" PageSize="20" ShowSeparators="True" AlwaysShowPager="True">
                                                        <FirstPageButton Visible="True">
                                                        </FirstPageButton>
                                                        <LastPageButton Visible="True">
                                                        </LastPageButton>
                                                    </SettingsPager>
                                                    <SettingsBehavior AllowFocusedRow="true" ConfirmDelete="True" />
                                                    <Styles>
                                                        <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                        </Header>
                                                        <LoadingPanel ImageSpacing="10px">
                                                        </LoadingPanel>
                                                    </Styles>
                                                    <StylesEditors>
                                                        <ProgressBar Height="25px">
                                                        </ProgressBar>
                                                    </StylesEditors>
                                                    <Templates>
                                                        <EditForm>
                                                            <table style="text-align: center">
                                                                <tr>
                                                                    <td style="width: 30%">
                                                                    </td>
                                                                    <td class="Ecoheadtxt" style="text-align: left; width: 10%;">
                                                                        Company :
                                                                    </td>
                                                                    <td style="text-align: left">
                                                                        <dxe:ASPxComboBox ID="cmbCompany" runat="server" DataSourceID="SqlComp" ValueField="cmp_internalid"
                                                                            TextField="cmp_Name" ClientInstanceName="compCompany" EnableIncrementalFiltering="true"
                                                                            EnableSynchronization="False" OnCallback="compSegment_Callback" ValueType="System.String"
                                                                            Width="250px">
                                                                            <ButtonStyle Width="13px">
                                                                            </ButtonStyle>
                                                                            <ClientSideEvents SelectedIndexChanged="function(s,e){change();}" />
                                                                        </dxe:ASPxComboBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 30%">
                                                                    </td>
                                                                    <td class="Ecoheadtxt" style="text-align: left; width: 10%">
                                                                        Segment :
                                                                    </td>
                                                                    <td style="text-align: left">
                                                                        <dxe:ASPxComboBox ID="cmbSegment" runat="server" DataSourceID="SqlSegment1" ValueField="exch_internalId"
                                                                            TextField="Exchange" ClientInstanceName="compSegment" EnableIncrementalFiltering="true"
                                                                            EnableSynchronization="False" OnCallback="compSegment_Callback" ValueType="System.String"
                                                                            Width="250px">
                                                                            <ButtonStyle Width="13px">
                                                                            </ButtonStyle>
                                                                        </dxe:ASPxComboBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 30%">
                                                                    </td>
                                                                    <td class="Ecoheadtxt" style="text-align: left; width: 15%">
                                                                        Other Charge Code :
                                                                    </td>
                                                                    <td style="text-align: left">
                                                                        <dxe:ASPxComboBox ID="compOtherCCode" runat="server" Value='<%# Bind("OtherChargeMember_OtherChargeCode") %>'
                                                                            DataSourceID="SqlCharges" ValueField="OtherCharges_Code" TextField="OtherCharges_Name"
                                                                            EnableIncrementalFiltering="true" EnableSynchronization="False" Width="250px"
                                                                            ValueType="System.String">
                                                                            <ButtonStyle Width="13px">
                                                                            </ButtonStyle>
                                                                        </dxe:ASPxComboBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 30%">
                                                                    </td>
                                                                    <td class="Ecoheadtxt" style="text-align: left; width: 15%">
                                                                        W.C.F :
                                                                    </td>
                                                                    <td style="text-align: left">
                                                                        <dxe:ASPxDateEdit ID="dtOtherCDate" runat="server" Value='<%# Bind("OtherChargeMember_DateFrom") %>'
                                                                            EditFormat="Custom" EditFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                                                                            <ButtonStyle Width="13px">
                                                                            </ButtonStyle>
                                                                        </dxe:ASPxDateEdit>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 30%">
                                                                    </td>
                                                                    <td style="text-align: right;">
                                                                        <dxe:ASPxButton ID="btnUpdate2" runat="server" Text="Save" ToolTip="Update data"
                                                                            Height="18px" Width="88px" AutoPostBack="False">
                                                                            <ClientSideEvents Click="function(s, e) {gridOtherCharges.UpdateEdit();}" />
                                                                        </dxe:ASPxButton>
                                                                    </td>
                                                                    <td style="text-align: left;" colspan="2">
                                                                        <dxe:ASPxButton ID="btnCancel2" runat="server" Text="Cancel" ToolTip="Cancel data"
                                                                            Height="18px" Width="88px" AutoPostBack="False">
                                                                            <ClientSideEvents Click="function(s, e) {gridOtherCharges.CancelEdit();}" />
                                                                        </dxe:ASPxButton>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </EditForm>
                                                    </Templates>
                                                </dxe:ASPxGridView>
                                            </div>
                                            </div>
                                        </dxe:ContentControl>
                                    </ContentCollection>
                                </dxe:TabPage>
                                <dxe:TabPage Name="Family Members" Text="Family">
                                    <ContentCollection>
                                        <dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
                                    </ContentCollection>
                                </dxe:TabPage>
                                <dxe:TabPage Name="Subscription" Text="Subscription">
                                    <ContentCollection>
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
                        <asp:SqlDataSource ID="SqlComp" runat="server" 
                            SelectCommand="SELECT [cmp_internalid], [cmp_Name] FROM [tbl_master_company] where cmp_internalid in(select distinct exch_compId from tbl_master_companyExchange)">
                        </asp:SqlDataSource>
                        <asp:SqlDataSource ID="SqlSegment" runat="server"
                            ConflictDetection="CompareAllValues" SelectCommand=""></asp:SqlDataSource>
                        <asp:SqlDataSource ID="SqlSegment1" runat="server" 
                            ConflictDetection="CompareAllValues" SelectCommand="select exch_internalId,isnull(((select exh_shortName from tbl_master_exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+ exch_segmentId),exch_membershipType) as Exchange from tbl_master_companyExchange">
                        </asp:SqlDataSource>
                        <asp:SqlDataSource ID="SqlCode" runat="server" 
                            ConflictDetection="CompareAllValues" SelectCommand="select ltrim(rtrim(ChargeGroup_code)) as ChargeGroup_ID,ChargeGroup_Name+' ['+ltrim(rtrim(ChargeGroup_code))+']' as ChargeGroup_Name from Master_ChargeGroup where ChargeGroup_Type=1">
                        </asp:SqlDataSource>
                        <asp:SqlDataSource ID="SqlPPCode" runat="server" 
                            ConflictDetection="CompareAllValues" SelectCommand=""></asp:SqlDataSource>
                        <asp:SqlDataSource ID="SqlPPCode1" runat="server" 
                            ConflictDetection="CompareAllValues" SelectCommand="select ltrim(rtrim(TradingProfile_Code)) as TradingProfile_Code,TradingProfile_Name from Master_TradingProfile">
                        </asp:SqlDataSource>
                        <asp:SqlDataSource ID="SqlCharges" runat="server" 
                            SelectCommand="SELECT [OtherCharges_Code], [OtherCharges_Name]+'['+rtrim(ltrim(OtherCharges_Code))+']' as OtherCharges_Name FROM [Master_OtherCharges]">
                        </asp:SqlDataSource>
                    </td>
                </tr>
            </table>
        </div>
</asp:Content>