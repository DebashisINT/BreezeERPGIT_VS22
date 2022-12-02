<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Reports_frm_rptEmpBDaydtls" Codebehind="frm_rptEmpBDaydtls.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  

    <script language="javascript" type="text/javascript">
    
 
     function noNumbers(e,txtBox)
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
    function FillGrid(obj)
    {
        grid.PerformCallback(obj);
    }
    function SetReminder(obj)
    {
         grid.PerformCallback(obj);
    }
    function RefreshReminder(obj)
    {
       if(obj=='REMINDER')
       {
            var x = top.frames['iFrmReminder'].ParentCall('Parent');
            alert('Reminder set successfully');
       }
       else if(obj=='ALREADYEXIST')
       {
            alert('You have already set a reminder for this employee');
       }

    }
    </script>

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

    <script type="text/javascript">
  (function($){
    // call setMask function on the document.ready event
      $(function(){
        $('input:text').setMask();
      }
    );
  })(jQuery);
    </script>
    <style>
        
    </style>
</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title clearfix">
            <h3 class="pull-left pTop5">Forthcoming Birthdays and Anniversaries</h3>
            
        </div>

    </div> 
     <div class="form_main">
        <table>
           
            <tr>
                <td>
                    <asp:Label ID="Label2" runat="server" Text="Event Types:" CssClass="mylabel1"></asp:Label>
                </td>
                <td class="lt" >
                    <asp:DropDownList ID="dpSelect" runat="server" Width="100%">
                        <asp:ListItem Text="Birth Days" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Marriage Anniversaries" Value="2"></asp:ListItem>
                    </asp:DropDownList></td>
                
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label1" CssClass="mylabel1" runat="server" Text="For the next:" Width="84px"></asp:Label></td>
                <td >
                    <asp:TextBox ID="txtDays" runat="server" Width="100%" CssClass="water" Text="Days"
                        ToolTip="Days" onkeypress="return noNumbers(event)"></asp:TextBox></td>
                
            </tr>
            <tr>
                <td></td>
                <td>
                    <input id="Button1" type="button" value="Show" onclick="FillGrid('PC');" class="btn btn-primary" />
                </td>
            </tr>
        </table>
         <table width="100%">
             <tr>
                <td>
                    <dxe:ASPxGridView ID="grdDetails" runat="server" ClientInstanceName="grid" AutoGenerateColumns="False"
                        Width="100%" OnCustomCallback="grdDetails_CustomCallback" OnCustomJSProperties="grdDetails_CustomJSProperties">
                        <Columns>
                            <dxe:GridViewDataTextColumn FieldName="Code" ReadOnly="true" VisibleIndex="0">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Name" ReadOnly="True" VisibleIndex="1">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Designation" ReadOnly="True" VisibleIndex="2">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Branch" ReadOnly="True" VisibleIndex="3">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Department" ReadOnly="True" VisibleIndex="4">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataDateColumn FieldName="DateOfBirth" ReadOnly="True" VisibleIndex="5">
                                <PropertiesDateEdit DisplayFormatString="{0:dd MMM yyyy}" EditFormat="Custom">
                                </PropertiesDateEdit>
                            </dxe:GridViewDataDateColumn>
                            <dxe:GridViewDataTextColumn FieldName="PhoneNumber" ReadOnly="True" VisibleIndex="6">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="MailId" ReadOnly="True" VisibleIndex="7">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Set Reminder" VisibleIndex="8">
                                <DataItemTemplate>
                                    <a href="#" onclick="SetReminder('<%#Eval("Code") %>')">SetReminder</a>
                                </DataItemTemplate>
                                <EditFormSettings Visible="False" />
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                        <Styles>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                            </Header>
                        </Styles>
                        <SettingsPager ShowSeparators="True">
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </SettingsPager>
                        <ClientSideEvents EndCallback="function(s,e){RefreshReminder(s.cpa);}" />
                    </dxe:ASPxGridView>
                </td>
            </tr>
         </table>
            </div> 
</asp:Content>
