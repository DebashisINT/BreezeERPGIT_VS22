<%@ Page Title="ChequePrint Routine" Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Reports_chequeprinting_new" Codebehind="chequeprinting_new.aspx.cs" %>

<%--<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxTabControl"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %>
<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxCallbackPanel"
    TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxPanel"
    TagPrefix="dxp" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/assests/js/ajaxList_rootfile.js"></script>
  <%--  <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />
    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <link rel="stylesheet" href="../modalfiles/modal.css" type="text/css" />
    <script type="text/javascript" src="../modalfiles/modal.js"></script>--%>

    <style type="text/css">    
    /*form {
		    display:inline;	
		    	
	    }*/
	 .frmleftCont{float:left; margin:2px; padding:2px; height:26px; }
   
	</style>

    <script type="text/javascript"> 
        $(document).ready(function() { 

            $(".water").each(function() { 
                if ($(this).val() == this.title) { 
                    $(this).addClass("opaque"); 
                } 
            }); 

            $(".water").focus(function() { 
                if ($(this).val() == this.title) { 
                    $(this).val(""); 
                    $(this).removeClass("opaque"); 
                }                 
            }); 

            $(".water").blur(function() { 
                if ($.trim($(this).val()) == "") { 
                    $(this).val(this.title); 
                    $(this).addClass("opaque"); 
                } 
                else { 
                    $(this).removeClass("opaque"); 
                } 
            }); 
        });       

    </script>       

    <script language="javascript" type="text/javascript">
    var RowID
    function OnGridFocusedRowChanged() 
    {

        grid.GetSelectedFieldValues('CashBankDetail_ID', OnGetRowValues);
        
    }
    function OnGetRowValues(values) 
    {

    
        RowID = 'n';
        for(var j = 0; j < values.length; j ++) {
            if(RowID != 'n')
                RowID += ',' + values[j];
            else
                RowID = values[j];
              
        }
        document.getElementById('hdnfrequency1').value=RowID;
//       if (RowID==undefined || RowID=='n')
//        {
//            
//            chq.SetText('');
//            chq.SetEnabled(false);
//        }
//       else
//        {
//        
//            chq.SetEnabled(true);
//        }
        //alert(counter+'test');
    }
    function OnAllCheckedChanged(s, e) {

  if (s.GetChecked())
  {
        
     grid.SelectRows();

     }
   
  else
  {
    
     grid.UnselectRows();
     }

}


    function Changestatus()
    {
    alert(RowID);
    var chkacc
    var chkucc
    var instdate
    var dateformat
    var chqno
    var bankid
    var custtype
    var fromdt
    var todt
    if(document.getElementById('chkparamacc').checked==true)
        {
        
            chkacc="true";
        }
     else
        {
            chkacc="false";
        }  
        if(document.getElementById('chkparamucc').checked==true)
        {
        
            chkucc="true";
        }
     else
        {
            chkucc="false";
        } 
        instdate= ChangeDateFormat_CalToDB(dtDateinstru.GetDate());
        fromdt=ChangeDateFormat_CalToDB(dtDate.GetDate());
        todt=ChangeDateFormat_CalToDB(dtToDate.GetDate());
        custtype=document.getElementById('ddlbasis').value;
        if(document.getElementById('cmbdtfarmat').value=='d1')
        {
        
            dateformat="true";
        }
        else
        {
        
             dateformat="false";
        
        }
        chqno=chq.GetText();
        bankid=GetObjectID('txtbank_hidden').value;
        
            if (RowID==undefined || RowID=='n')
            {
                
                alert ('Please Select Atleast one Item !!..');
            }
            else
            {
            
      var URL="changedpacc_cheque.aspx?acc="+chkacc+"&ucc="+chkucc+"&date="+instdate+"&dtformat="+dateformat+"&chqno="+chqno+"&rowid="+RowID+"&bankid="+bankid+"&fromdate="+fromdt+"&todate="+todt+"&custype="+custtype;
      alert (URL);
      
     OnMoreInfoClick(URL,"ChequePrint Routine~Without Close",'940px','500px','Y');
     parent.editwin.onclose=function()
             {

            
             HideShow('Tblfooter',"H");
 
            HideShow('divPageheader',"S");
            height();
            document.getElementById("Btnnext").disabled=true;

             }
             }

    }
function PopulateGrid(obj)
 {
 alert("callback2");
   grid.PerformCallback(obj);
 }

function callback()
{
alert("2");
//chq.SetText('');
            //height();
           // chq.SetEnabled(false);
   grid.PerformCallback();

}

    function isNumberKey(e)      
        {     
       
            var keynum;
            var keychar;
            var numcheck;
            var txt;
            if(window.event)//IE
            {
            
                txt=chq.GetText();
                
                
                keynum = e.keyCode 
                if(keynum>=48 && keynum<=57 || keynum==46)
                   {
                        if (txt.length>0)
                            {
                                document.getElementById("Btnnext").disabled=false;
                             }
                         else
                            {
                                document.getElementById("Btnnext").disabled=true;
                             }
                      return true;
                   }
                else
                    {
                     alert("Please Insert Numeric Only");
                      if (txt.length>0)
                            {
                                document.getElementById("Btnnext").disabled=false;
                             }
                         else
                            {
                                document.getElementById("Btnnext").disabled=true;
                             }
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
    

    </script>

    <script language="javascript" type="text/javascript">
     function gridasset_EndCallBack()
     {  
     if(grid.cpproperties=="nullvalue")
     {
        HideShow('Tblfooter',"H");
 
        HideShow('divPageheader',"S");
        alert('No record Found !!');
         
     }
     if(grid.cpproperties=="nullvaluerespective")
     {
        HideShow('Tblfooter',"H");
 
        HideShow('divPageheader',"S");
       
         
     }
     if(grid.cpproperties=="forother")
     {
        HideShow('divparameter',"H");
 
       
         
     }
     if (grid.cpproperties=="noitem")
     {
        HideShow('Tblfooter',"S");
 
        HideShow('divPageheader',"H");
        alert ('Please Select Atleast one Item !!..');
     }
     if (grid.cpproperties=="senddata")
     {

        document.getElementById('BtnForExportEvent').click();
     }
        height();
     }
     function openpopup()
     {
             var URL="changedpacc_cheque.aspx?"
      OnMoreInfoClick(URL,"ChequePrint Routine~Without Close",'940px','500px',"Y");
     parent.editwin.onclose=function()
             {
            
             
             HideShow('Tblfooter',"S");
 
            HideShow('divPageheader',"H");
            height();
//            document.getElementById("Btnnext").disabled=true;
            grid.PerformCallback("Show~~~");

             }
     }
function Btnprevious_Click()
    {
        HideShow('Tblfooter',"H");
 
        HideShow('divPageheader',"S");
    }
      function Page_Load()
        {
        
        HideShow('Tblfooter',"H");
 
        HideShow('divPageheader',"S");
            height();
//            document.getElementById("Btnnext").disabled=true;
            //chq.SetEnabled(false);
        }
        
        
    function BtnShow_Click()
        {
            HideShow('Tblfooter',"S");
            HideShow('divPageheader',"H");
            chq.SetText('');
            height();
            //grid.UnselectRows();
           
           //RowID='n';
            //chq.SetEnabled(false);
            grid.PerformCallback("Show~~~");
            
        }
     function Btnnext_Click()
      {
        
        var txt=chq.GetText();
         if (txt.length>0)
            {
            var dt=ChangeDateFormat_CalToDB(dtDateinstru.GetDate());

                HideShow('Tblfooter',"S");

                HideShow('divPageheader',"H");
        
//                Changestatus();
                grid.PerformCallback("Next~~~");
            }
                 else
                 {
                    alert('Please Fill Cheque Number');
                  }
         }
          

    function ShowHideFilter(obj)
     {
        grid.PerformCallback(obj);
   
     }

         function ChangeDateFormat_CalToDB(obj)
    {
        var SelectedDate = new Date(obj);            
        var monthnumber = SelectedDate.getMonth() + 1;
        var monthday    = SelectedDate.getDate();
        var year        = SelectedDate.getYear();            
        var changedDateValue=year+'-'+monthnumber+'-'+monthday;
        return changedDateValue;
    }
       function SignOff()
        {
            window.parent.SignOff();
        }
         FieldName='Button1';
        function showOptions(obj1,obj2,obj3)
         {
            var cmb=document.getElementById('cmbsearchOption');
            ajax_showOptions(obj1,obj2,obj3,cmb.value);
         }
         function ShowBankName(obj1,obj2,obj3)
         {
            ajax_showOptions(obj1,obj2,obj3);
         }
        //function height()
        //{
        //    if(document.body.scrollHeight>=350)
        //    {
        //        window.frameElement.height = document.body.scrollHeight;
        //    }
        //    else
        //    {
        //        window.frameElement.height = '350px';
        //    }
        //    window.frameElement.Width = document.body.scrollWidth;
        //}
        
 function ShowInstTypePopUp(id)       
	    {
	        CmbClientBankCI.PerformCallback("ClientBankBind~"+id);
	        cPopUp_InstrumentDetail.Show();
	    }
function OnCancelButtonClick()
        {
        cPopUp_InstrumentDetail.Hide();
        }
      
function OnAddButtonClick()
       {     
            grid.PerformCallback("gridbindfrompopup~~");
            cPopUp_InstrumentDetail.Hide();
       }
function CmbClientBank_EndCallBack()
        {
       
	    }

 function DateChange(positionDate)
        {
            var FYS='<%=Session["FinYearStart"]%>';
            var FYE='<%=Session["FinYearEnd"]%>';
            var LFY='<%=Session["LastFinYear"]%>';
            DevE_CheckForFinYear(positionDate,FYS,FYE,LFY);
            if(dtDate.GetDate()==positionDate.GetDate())
            {
                var setFromDate='<%=fDate%>'; 
                CompareDate(dtDate.GetDate(),dtToDate.GetDate(),'LE','From Date Can Not Be Greater Than To Date',dtDate,setFromDate);
            }
            else if(dtToDate.GetDate()==positionDate.GetDate())            
            {
                var setToDate='<%=tDate%>'; 
                CompareDate(dtDate.GetDate(),dtToDate.GetDate(),'LE','To Date Can Not Be Less Than From Date',dtToDate,setToDate);
            }
        }	      
       
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="panel-heading">
        <div class="panel-title">
            <h3>ChequePrint Routine</h3>
        </div>

    </div>
    <div class="form_main">
        <%--<div class="TableMain100">
            <div class="EHEADER" style="text-align: center;">
                <strong><span style="color: #000099">ChequePrint Routine</span></strong>
            </div>
        </div>--%>
        <div class="pageContent">
            <div id="divPageheader">
                <div id="dvMainFilter" class="frmContent" style="width: 500px;">
                    <div id="divhdr">
                        <div id="Trbankname">
                            <div class="frmleftCont" style="width: 110px; line-height: 20px">
                                <asp:Label ID="lblaccount" runat="server" Text="Bank Name : "></asp:Label>
                            </div>
                            <div class="frmleftContent">
                                <asp:TextBox ID="txtbank" runat="server" Width="200px" Font-Size="12px" Height="20px"
                                    TabIndex="0"></asp:TextBox>
                                <asp:TextBox ID="txtbank_hidden" runat="server" Width="2px" Font-Size="1px" Height="2px"
                                    Style="display: none" TabIndex="0"></asp:TextBox>
                            </div>
                        </div>
                        <div class="clear"></div>
                        <div id="divtype">
                            <div class="frmleftCont" style="width: 110px; line-height: 20px">
                                <dxe:ASPxLabel ID="lbltype" runat="server" Text="Client Type">
                                </dxe:ASPxLabel>
                            </div>
                            <div class="frmleftContent">
                                <%--<dxe:ASPxComboBox runat="server" Width="150px" ID="" DropDownStyle="DropDown"
                                    EnableSynchronization="False" EnableIncrementalFiltering="True">
                                    <Items>
                                        <dxe:ListEditItem Text="CUSTOMERS" Value="C"></dxe:ListEditItem>
                                        <dxe:ListEditItem Text="OTHERS" Value="O"></dxe:ListEditItem>
                                    </Items>
                                </dxe:ASPxComboBox>--%>
                                <asp:DropDownList ID="ddlbasis" runat="server" Font-Size="12px" Width="100px">
                                    <asp:ListItem Selected="True" Value="C">CUSTOMERS</asp:ListItem>
                                    <asp:ListItem Value="O">OTHERS</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <span class="clear"></span>
                        <div id="divdate">
                            <div class="frmleftCont" style="width: 110px; line-height: 20px">
                                <dxe:ASPxLabel ID="lbldate" runat="server" Text="Voucher Date">
                                </dxe:ASPxLabel>
                            </div>
                            <div class="frmleftContent">
                                <dxe:ASPxDateEdit ID="dtDate" runat="server" EditFormat="Custom" ClientInstanceName="dtDate"
                                    EditFormatString="dd-MM-yyyy" UseMaskBehavior="True" DateOnError="Today" Width="200px">
                                    <DropDownButton Text="From ">
                                    </DropDownButton>
                                    <ClientSideEvents DateChanged="function(s,e){DateChangeForFrom(dtDate,dtToDate);}" />
                                    <ClientSideEvents DateChanged="function(s,e){DateChange(dtDate);}"></ClientSideEvents>
                                </dxe:ASPxDateEdit>
                            </div>
                            <div class="frmleftContent">
                                <dxe:ASPxDateEdit ID="dtToDate" runat="server" EditFormat="Custom" ClientInstanceName="dtToDate"
                                    EditFormatString="dd-MM-yyyy" UseMaskBehavior="True" DateOnError="Today" Width="200px">
                                    <DropDownButton Text="To">
                                    </DropDownButton>
                                    <ClientSideEvents DateChanged="function(s,e){DateChangeForTo(dtDate,dtToDate);}" />
                                    <ClientSideEvents DateChanged="function(s,e){DateChange(dtToDate);}"></ClientSideEvents>
                                </dxe:ASPxDateEdit>
                            </div>
                        </div>
                        <span class="clear"></span><span class="clear" style="height: 5px;">&nbsp;</span>
                        <div id="divbutton">
                            <div style="width: 120px; float: left;">
                            </div>
                            <div style="width: 110px; line-height: 20px;">
                                <dxe:ASPxButton ID="BtnShow" runat="server" CssClass="btn btn-primary" AutoPostBack="False" Text="SHOW" Width="85px"
                                    AccessKey="O">
                                    <ClientSideEvents Click="function (s, e) {BtnShow_Click();}" />
                                </dxe:ASPxButton>
                            </div>
                        </div>
                        <span class="clear" style="height: 5px;">&nbsp;</span>
                    </div>
                </div>
            </div>
        </div>
        <div id="Tblfooter">
            <div id="div1">
                <div id="Div2" class="frmContent" style="width: 600px;">
                    <table style="width: 363px;">
                        <tr id="divinstr">
                            <td>
                                <dxe:ASPxLabel ID="lblinstrudt" runat="server" Text="Instrument Date">
                                </dxe:ASPxLabel>
                            </td>
                            <td>
                                <dxe:ASPxDateEdit ID="dtinstrudate" runat="server" EditFormat="Custom" ClientInstanceName="dtDateinstru"
                                    EditFormatString="dd-MM-yyyy" UseMaskBehavior="True" Width="200px">
                                    <DropDownButton Text="Inst.Dt">
                                    </DropDownButton>
                                </dxe:ASPxDateEdit>
                            </td>
                        </tr>
                        <tr id="divdtformat">
                            <td>
                                <dxe:ASPxLabel ID="lbldtformat" runat="server" Text="Inst.Date Format">
                                </dxe:ASPxLabel>
                            </td>
                            <td>
                                <asp:DropDownList ID="cmbdtfarmat" runat="server" Style="width: 200px; line-height: 20px">
                                    <asp:ListItem Selected="True" Value="d1">DDMMYYYY</asp:ListItem>
                                    <asp:ListItem Value="d2">DDMONYYYY</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr id="divparameter">
                            <td>
                                <dxe:ASPxLabel ID="lblacc" runat="server" Text="Print Acc/No">
                                </dxe:ASPxLabel>
                            </td>
                            <td>
                                 <asp:CheckBox ID="chkparamacc" runat="server" Checked="true" Width="200px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <dxe:ASPxLabel ID="lblucc" runat="server" Text="Print UCC">
                                </dxe:ASPxLabel>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkparamucc" runat="server" Checked="false" Width="200px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <dxe:ASPxLabel ID="ASPxLabel1" runat="server" Text="Select Format">
                                </dxe:ASPxLabel>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlbank" runat="server" Style="width: 200px; line-height: 20px">
                                    <asp:ListItem Selected="True" Value="H">HDFC</asp:ListItem>
                                    <asp:ListItem Value="A">AXIS</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr id="divtxtchqno">
                            <td>
                                <dxe:ASPxLabel ID="lblchqno" runat="server" Text="Starting Cheque No">
                                </dxe:ASPxLabel>
                            </td>
                            <td>
                                <dxe:ASPxTextBox ID="txtchqno" ClientInstanceName="chq" runat="server" Width="200px"
                                    MaxLength="8" Onkeypress="return isNumberKey(event)">
                                </dxe:ASPxTextBox>
                            </td>
                        </tr>
                    </table>
                    
                    
                    <span class="clear"></span>
                    
                    <span class="clear"></span><span class="clear" style="height: 5px;">&nbsp;</span>
                    <div id="divbtnnext">
                        <div>
                            <dxe:ASPxButton ID="Btnprevious" runat="server" AutoPostBack="False" Text="<< [P]REVIOUS"
                                Width="100px" AccessKey="P" CssClass="btn btn-primary">
                                <ClientSideEvents Click="function (s, e) {Btnprevious_Click();}" />
                            </dxe:ASPxButton>
                        
                            <dxe:ASPxButton ID="Btnnext" runat="server" AutoPostBack="False" Text="NE[X]T >>"
                                Width="100px" AccessKey="X" CssClass="btn btn-primary">
                                <ClientSideEvents Click="function (s, e) {Btnnext_Click();}" />
                            </dxe:ASPxButton>
                        </div>
                    </div>
                    <span class="clear" style="height: 5px;">&nbsp;</span>
                </div>
                <div>
                    <div class="left" style="width: 70px;">
                        <a href="javascript:ShowHideFilter('s');"><span style="color: Gray; text-decoration: underline;
                            font-size: 12px">Search</span></a>
                    </div>
                    <div class="left" style="width: 70px;">
                        <a href="javascript:ShowHideFilter('All');"><span style="color: Gray; text-decoration: underline;
                            font-size: 12px">ShowAll</span></a>
                    </div>
                </div> 
            </div>
            <span class="clear" style="height: 5px;">&nbsp;</span>
            <dxe:ASPxGridView ID="gridasset" runat="server" AutoGenerateColumns="False" KeyFieldName="CashBankDetail_ID"
                Width="95%" ClientInstanceName="grid" OnHtmlRowCreated="gridasset_HtmlRowCreated"
                OnCustomCallback="gridasset_CustomCallback" OnPageIndexChanged="gridasset_PageIndexChanging">
                <SettingsBehavior ConfirmDelete="True" AllowFocusedRow="True" />
                <Styles>
                    <LoadingPanel ImageSpacing="10px">
                    </LoadingPanel>
                    <FocusedGroupRow CssClass="gridselectrow">
                    </FocusedGroupRow>
                    <FocusedRow CssClass="gridselectrow">
                    </FocusedRow>
                </Styles>
                <SettingsPager AlwaysShowPager="True" NumericButtonCount="20" ShowSeparators="True">
                    <FirstPageButton Visible="True">
                    </FirstPageButton>
                    <LastPageButton Visible="True">
                    </LastPageButton>
                </SettingsPager>
                <SettingsEditing Mode="PopupEditForm" PopupEditFormHeight="200px" PopupEditFormHorizontalAlign="Center"
                    PopupEditFormModal="True" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="600px"
                    EditFormColumnCount="1" />
                <SettingsText PopupEditFormCaption="Add/Modify " ConfirmDelete="Are you sure to Delete this Record!" />
                <ClientSideEvents EndCallback="function(s, e) {gridasset_EndCallBack();}" />
                <ClientSideEvents SelectionChanged="function(s, e) { OnGridFocusedRowChanged(); }" />
                <Columns>
                    <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="2%" VisibleIndex="0">
                        <HeaderTemplate>
                            <dxe:ASPxCheckBox ID="cbAll" runat="server" ClientInstanceName="cbAll" ToolTip="Select/Deselect all rows"
                                BackColor="White" OnInit="cbAll_Init">
                                <ClientSideEvents CheckedChanged="OnAllCheckedChanged" />
                            </dxe:ASPxCheckBox>
                        </HeaderTemplate>
                    </dxe:GridViewCommandColumn>
                    <dxe:GridViewDataTextColumn FieldName="MainAccount_Name" Caption="Account Name"
                        ReadOnly="true" VisibleIndex="1" Width="15%">
                        <CellStyle CssClass="gridcellleft" Wrap="False">
                        </CellStyle>
                        <HeaderStyle HorizontalAlign="Left" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="cashbank_vouchernumber" Caption="Voucher No."
                        ReadOnly="true" VisibleIndex="2" Width="5%">
                        <CellStyle CssClass="gridcellleft" Wrap="False">
                        </CellStyle>
                        <HeaderStyle HorizontalAlign="Left" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="CashBank_TransactionDate" Caption="Transaction Dt."
                        ReadOnly="true" VisibleIndex="3" Width="6%">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <HeaderStyle HorizontalAlign="Left" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="4" Width="6%" FieldName="Payment" Caption="Ammount"
                        ReadOnly="true">
                        <CellStyle CssClass="gridcellleft" Wrap="true" HorizontalAlign="Right">
                        </CellStyle>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Account No With Bank" FieldName="accountname"
                        ReadOnly="true" VisibleIndex="8" Width="22%">
                        <CellStyle CssClass="gridcellleft" Wrap="False">
                        </CellStyle>
                        <HeaderStyle HorizontalAlign="Left" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="accid" FieldName="cbd_bankCode" VisibleIndex="9"
                        Visible="false">
                        <CellStyle CssClass="gridcellleft" Wrap="False">
                        </CellStyle>
                        <HeaderStyle HorizontalAlign="Left" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Clientid" FieldName="CashBankDetail_Subaccountid"
                        VisibleIndex="10" Visible="false">
                        <CellStyle CssClass="gridcellleft" Wrap="False">
                        </CellStyle>
                        <HeaderStyle HorizontalAlign="Left" />
                    </dxe:GridViewDataTextColumn>
                </Columns>
                <Settings ShowFooter="true" />
                <Settings ShowHorizontalScrollBar="false" ShowGroupButtons="False" ShowGroupPanel="false"
                    ShowStatusBar="Hidden" />
            </dxe:ASPxGridView>
            <dxe:ASPxPopupControl ID="PopUp_InstrumentDetail" runat="server" ClientInstanceName="cPopUp_InstrumentDetail"
                HeaderText="Client Bank Details" ShowSizeGrip="False" Width="400px" CloseAction="None"
                Font-Bold="True" PopupHorizontalAlign="LeftSides" PopupVerticalAlign="TopSides"
                Modal="True">
                <ContentCollection>
                    <dxe:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                        <dxe:ASPxCallbackPanel ID="InstrumentDetail_CallbackPanel" runat="server" Width="200px"
                            ClientInstanceName="cInstrumentDetail_CallbackPanel" OnCallback="InstrumentDetail_CallbackPanel_Callback">
                            <PanelCollection>
                                <dxe:panelcontent runat="server">
                                    <table style="width: 546px; font-weight: normal;" border="1" id="TABLE1">
                                        <tr>
                                            <td valign="top" id="tdCBankLable" style="background-color: gainsboro; vertical-align: top;
                                                height: 11px; text-align: left;">
                                                Customer Bank</td>
                                            <td colspan="5" valign="top" id="tdCBankValue">
                                                <dxe:ASPxComboBox ID="CmbClientBank" runat="server" CallbackPageSize="30" ClientInstanceName="CmbClientBankCI"
                                                    DropDownWidth="550px" EnableIncrementalFiltering="True" Height="20px" OnCallback="CmbClientBank_OnCallback"
                                                    TextFormatString="{0} [{2}]" ValueField="cbd_id" ValueType="System.String" Width="370px">
                                                    <ClientSideEvents ValueChanged="function(s,e){
                                                                                    var indexr = s.GetSelectedIndex();
                                                                                    OnCmbClientBank_ValueChange(indexr)
                                                                                    }" EndCallback="CmbClientBank_EndCallBack" />
                                                    <Columns>
                                                        <dxe:ListBoxColumn FieldName="bnk_bankName" Caption="Bank Name" ToolTip="Bank Name"
                                                            Width="150px"></dxe:ListBoxColumn>
                                                        <dxe:ListBoxColumn FieldName="cbd_accountName" Caption="Account Holder Name" ToolTip="Account Holder Name"
                                                            Width="200px"></dxe:ListBoxColumn>
                                                        <dxe:ListBoxColumn FieldName="cbd_accountNumber" Caption="Account Number" ToolTip="Account Number"
                                                            Width="120px"></dxe:ListBoxColumn>
                                                        <dxe:ListBoxColumn FieldName="bnk_micrno" Caption="MICR Number" ToolTip="MICR Number"
                                                            Width="80px" Visible="false"></dxe:ListBoxColumn>
                                                        <dxe:ListBoxColumn FieldName="cbd_Accountcategory" Caption="Account Type" ToolTip="MICR Number"
                                                            Width="80px"></dxe:ListBoxColumn>
                                                    </Columns>
                                                </dxe:ASPxComboBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="text-align: right" valign="top">
                                                <table border="1">
                                                    <tr>
                                                        <td id="TdAdd" style="width: 100px">
                                                            <dxe:ASPxButton ID="btnAdd" runat="server" AutoPostBack="False" Text="Change" Width="85px">
                                                                <ClientSideEvents Click="function (s, e) { OnAddButtonClick(); }" />
                                                            </dxe:ASPxButton>
                                                        </td>
                                                        <td id="TdUpdate" style="width: 100px; display: none;">
                                                            <dxe:ASPxButton ID="btnUpdate" runat="server" AutoPostBack="False" Text="Update"
                                                                Width="85px">
                                                                <ClientSideEvents Click="function (s, e) { OnUpdateButtonClick(); }" />
                                                            </dxe:ASPxButton>
                                                        </td>
                                                        <td id="TdCancel" style="width: 100px">
                                                            <dxe:ASPxButton ID="btnCancel" runat="server" AutoPostBack="False" Text="Cancel"
                                                                Width="85px">
                                                                <ClientSideEvents Click="function (s, e) {OnCancelButtonClick();}" />
                                                            </dxe:ASPxButton>
                                                        </td>
                                                        <td id="TdCancelE" style="width: 100px; display: none;">
                                                            <dxe:ASPxButton ID="btnCancelE" runat="server" AutoPostBack="False" Text="Cancel"
                                                                Width="85px">
                                                                <ClientSideEvents Click="function (s, e) {OnCancelEButtonClick();}" />
                                                            </dxe:ASPxButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </dxe:panelcontent>
                            </PanelCollection>
                            <ClientSideEvents EndCallback="function(s, e) {
	                                                    InstrumentDetail_CallbackPanel_EndCallBack();
	                                                }" />
                        </dxe:ASPxCallbackPanel>
                    </dxe:PopupControlContentControl>
                </ContentCollection>
            </dxe:ASPxPopupControl>
        </div>
        <div style="display: none">
            <asp:HiddenField ID="txtSelection_hidden" runat="server" />
            <asp:HiddenField ID="HiddenField_Segment" runat="server" />
            <asp:HiddenField ID="HiddenField_Branch" runat="server" />
            <asp:HiddenField ID="HiddenFieldpagenav" runat="server" />
            <asp:HiddenField ID="hdn_GridBindOrNotBind" runat="server" />
            <asp:HiddenField ID="hdnfrequency" runat="server" />
            <asp:HiddenField ID="hdnfrequency1" runat="server" />
            <asp:Button ID="BtnForExportEvent" runat="server" OnClick="cmbExport_SelectedIndexChanged"
                BackColor="#DDECFE" BorderStyle="None" />
        </div>
        </div>
</asp:Content>
