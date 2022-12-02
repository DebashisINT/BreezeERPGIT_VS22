<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.ActivityManagement.management_activitymanagement_frm_SalesVisitActivityWithIframe" Codebehind="frm_SalesVisitActivityWithIframe.aspx.cs" %>

<%--<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe000001" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  <%--  <link href="../../CSS/style.css" rel="stylesheet" type="text/css" />--%>

    <script language="javascript" type="text/javascript">
        var chkobj;
        var objchk=null;
        var obGenral=null;
        function chkGenral(objGenral,val12)
        {
            var ReturnAccording;
            var st = document.getElementById("txtGrdContact")
            
            if (obGenral == null)
            {
                obGenral = objGenral;
            }
            else
            {
                obGenral.checked = false;
                obGenral = objGenral;
                obGenral.checked = true;
            }
            st.value = val12;
        }
        function btnAddProductWindow()
        {   
            var val123=document.getElementById("txtAssigned").value;
            if(val123=="")
            {
                alert('Assign to can not be blank');
            } 
            OnMoreInfoClick("frmOfferedProduct_New.aspx","ADD PRODUCT","950px","500px","N");
           
        }
        function frmOpenNewWindow1(location,v_height,v_weight)
        {
            var x=(screen.availHeight-v_height)/2;
            var y=(screen.availWidth-v_weight)/2
            window.open(location,"Search_Conformation_Box","height="+ v_height +",width="+ v_weight +",top="+ x +",left="+ y +",location=no,directories=no,menubar=no,toolbar=no,status=yes,scrollbars=yes,resizable=no,dependent=no'");       
        }
        function funAddLead()
          { 
            var val123=document.getElementById("txtAssigned").value;
            if(val123=="")
            {
                alert('Assign to can not be blank');
            }
            else
            {   
                ReturnAccording="AddLead";        
                OnMoreInfoClick("Add_LeadNew.aspx?Call=PhoneCall&user="+val123,"ADD LEAD","950px","500px","Y");
            }
          }

          function GetUserWork()
          {
            var con=document.getElementById("drpUserWork").value; 
            return con;
          }
          
//       function funSaveClick()
//        {
//            var st = document.getElementById("txtLeadId")
//            frmOpenNewWindow1('frmCancelMeeting.aspx?id=' + st.value ,500,500)
//        }
//       function funCheckFunction()
//        {
//             var st = document.getElementById("txtLeadId")
//             frmOpenNewWindow1('frmupdateMeeting.aspx?id=' + st.value ,500,500)
//        }
       function windowopenform()
        {
            
            ReturnAccording="Allot";
            var st = document.getElementById("txtLeadId")
//            frmOpenNewWindow1('frmAllot_sales.aspx?id=' + st.value ,500,500)
           
            OnMoreInfoClick("frmAllot_new.aspx?Calling=SalesVisit&id=" + st.value,"Allotment","950px","500px","Y");
        }
        function windowopenform1()
        {
            ReturnAccording="Allot";
            var st = document.getElementById("txtLeadId")
            OnMoreInfoClick("frmAllot_sales_new.aspx?Calling=SalesVisit&id=" + st.value,"Allotment","950px","500px","Y");
            
        }
        function CallingExport()
        {
            OnMoreInfoClick("frmmessage_history.aspx","Export Files","950px","500px","N");
        }
       function frmOpenNewWindow_custom(location,v_height,v_weight,top,left)
        {   
           window.open(location,"Search_Conformation_Box","height="+ v_height +",width="+ v_weight +",top="+ top +",left="+ left +",location=no,directories=no,menubar=no,toolbar=no,status=no,scrollbars=yes,resizable=no,dependent=no'");       
        }
       function FillLeadId(obj,val)
        {
            var ob = document.getElementById("txtLeadId")
            
            if (ob.value == null)
            {
                if (obj.checked == true)
                {
                    ob.value = val + ','
                }
            }
            else
            {
                if (obj.checked == true)
                {
                    ob.value = ob.value + val + ','
                }
                else
                {
                    var st = ob.value.split(",")
                    //ob.value = null;
                    var tt = ''
                    ob.value =tt;
                    for (var i = 0; i < st.length; i++)
                    {
                        if (st[i] == val)
                        {
                           
                        }
                        else
                        {
                            if (st[i] == tt)
                            {
                            }
                            else
                            {
                                ob.value = ob.value + st[i] + ',';
                            }
                        }
                    }
                }
            }             
        }
      function fun(obj,str)
        {
          
           document.getElementById("drpProduct").disabled = str;       
        }
        
        function TextVal1()
        {
            var btn=document.getElementById("btnGenratedSales");
            btn.click();
            var btn1=document.getElementById("btnshowGenratedSaleVisit");
            btn1.click();
        } 
//       function height()
//         {
////        alert(document.body.scrollHeight);
//        if(document.body.scrollHeight>400)
//        {   
//            window.frameElement.height = document.body.scrollHeight;
//            window.frameElement.Width = document.body.scrollWidth;
//        }
//        else
//        {
//            window.frameElement.height="400";
//            window.frameElement.Width = document.body.scrollWidth;
//        }
//         }
   function UserList()
       {
          
          OnMoreInfoClick("UserList.aspx","ADD USER","950px","500px","N");
       }
       function GetUserList()
    {
     
         var ob=document.getElementById("txtAssigned");
         return ob;
     
//     var ob1=document.getElementById("hd1UserList");
    }
    function GetHiddenUserList()
    {
        var ob2=document.getElementById("hd1UserList");
        return ob2;
    }
    function MakeButtonDisable()
    {
        Button1.disabled =true;
    }
    function SetAddLeadEnable()
    {
         Button1.disabled =false;
    }
 
    
     function callback()
    {
       
        
          if(ReturnAccording=="AddLead")
          {     
                
                document.getElementById('btnAddDocument').disabled =false;
          }
          else if(ReturnAccording=="Allot")
          {
                document.getElementById('btnshowGenratedSaleVisit').click();
          }
    }
    function Disable(obj)
        {
            var gridview = document.getElementById('grdGenratedSalesVisit'); 
            var rCount = gridview.rows.length+1; 
            if(rCount<10)
                rCount='0'+rCount;
            if(obj=='P')
            {
                document.getElementById("grdGenratedSalesVisit_ctl09_FirstPage").style.display='none';
                document.getElementById("grdGenratedSalesVisit_ctl09_PreviousPage").style.display='none';
                document.getElementById("grdGenratedSalesVisit_ctl09_NextPage").style.display='inline';
                document.getElementById("grdGenratedSalesVisit_ctl09_LastPage").style.display='inline';
            }
            else
            {
               
                document.getElementById("grdGenratedSalesVisit_ctl"+rCount+"_NextPage").style.display='none';
                document.getElementById("grdGenratedSalesVisit_ctl"+rCount+"_LastPage").style.display='none';
            }
        }
//           function DisableFirst()
//	    {
//	        var gridview = document.getElementById('grdGeneralTrial'); 
//            var rCount = gridview.rows.length; 
//            if(rCount<10)
//                rCount='0'+(rCount-1);
//            document.getElementById("grdGenratedSalesVisit_ctl"+rCount+"_NextPage").style.display='none';
//            document.getElementById("grdGenratedSalesVisit_ctl"+rCount+"_LastPage").style.display='none';
//            document.getElementById("grdGenratedSalesVisit_ctl"+rCount+"_FirstPage").style.display='none';
//            document.getElementById("grdGenratedSalesVisit_ctl"+rCount+"_PreviousPage").style.display='none';
//	    }
   
    </script>

    <link rel="stylesheet" href="../../windowfiles/dhtmlwindow.css" type="text/css" />

    <script type="text/javascript" src="../../windowfiles/dhtmlwindow.js"></script>

    <link rel="stylesheet" href="../../modalfiles/modal.css" type="text/css" />

    <script type="text/javascript" src="../../modalfiles/modal.js"></script>

    <!-- -----------------------------------------------------------   -->
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div class="form_main">
        <table class="TableMain100">
            <tr>
                <td class="lt">
                    <table>
                        <tr>
                            <td>
                                <asp:Button ID="btnCreate" runat="server" Text="Create" CssClass="btn btn-primary" OnClick="btnCreate_Click"
                                    />
                            </td>
                            <td>
                                <asp:Button ID="btnModify" runat="server" Text="Modify" CssClass="btn btn-primary" OnClick="btnModify_Click"
                                    Visible="False" />
                            </td>
                            <td>
                                <asp:Button ID="btnReallocateSalesVisit" runat="server" Text="Alloted Sales Visit"
                                    CssClass="btn btn-primary" OnClick="btnReallocateSalesVisit_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnGenratedSales" runat="server" Text="Generated Sales" CssClass="btn btn-primary"
                                     OnClick="btnGenratedSales_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnReAssign1" runat="server" Text="Reassign Sales Visit" CssClass="btn btn-primary"
                                     Visible="False" />
                            </td>
                            <td>
                                <asp:Button ID="btnMainCancel" runat="server" Text="Cancel" CssClass="btn btn-danger"
                                    Visible="False" OnClick="btnMainCancel_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr runat="server" id="PnlBtn">
                <td visible="false">
                    <table>
                        <tr>
                            <td>
                                <asp:Button ID="btnReassign" runat="server" Text="Reassign" CssClass="btn btn-primary"
                                    OnClick="btnReassign_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnReschedule" runat="server" Text="Reschedule" CssClass="btn btn-primary"
                                    OnClick="btnReschedule_Click"/>
                            </td>
                            <td>
                                <asp:Button ID="btnShowDetail" runat="server" Text="Show Details" CssClass="btn btn-primary"
                                    OnClick="btnShowDetail_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnDelegate" runat="server" Text="Delegate To" CssClass="btn btn-primary"
                                   />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr runat="server" id="userInfo1">
                <td>
                    <asp:GridView EnableViewState="true" ID="grdUserInfo" AutoGenerateColumns="false"
                        runat="server" AllowPaging="True" Width="100%" CellPadding="4" ForeColor="#333333"
                        GridLines="None" BorderWidth="1px" BorderColor="#507CD1" OnPageIndexChanging="grdUserInfo_PageIndexChanging">
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#EFF3FB" BorderWidth="1px" />
                        <EditRowStyle BackColor="#2461BF" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <HeaderStyle Font-Bold="false" ForeColor="black" BorderColor="AliceBlue" BorderWidth="1px" />
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:HyperLinkField HeaderText="User Id" DataNavigateUrlFormatString="~/management/frm_SalesVisitActivity.aspx?id={0} &amp; type=SW"
                                NavigateUrl="~/management/frm_SalesVisitActivity.aspx" DataTextField="UserId"
                                Visible="False" DataNavigateUrlFields="UserId" />
                            <asp:HyperLinkField DataNavigateUrlFormatString="~/management/frm_SalesVisitActivity.aspx?id={0}&amp;type=SW"
                                NavigateUrl="~/management/frm_SalesVisitActivity.aspx" DataTextField="SNo" HeaderText="S.No."
                                Visible="False" DataNavigateUrlFields="userid" />
                            <asp:HyperLinkField DataNavigateUrlFormatString="~/management/frm_SalesVisitActivity.aspx?id={0}&amp;type=SW"
                                NavigateUrl="~/management/frm_SalesVisitActivity.aspx" DataTextField="user" HeaderText="User"
                                DataNavigateUrlFields="userid" />
                            <asp:HyperLinkField DataNavigateUrlFormatString="~/management/frm_SalesVisitActivity.aspx?id={0}&amp;type=SW"
                                NavigateUrl="~/management/frm_SalesVisitActivity.aspx" DataTextField="Pending Acttivity"
                                HeaderText="Pending Activity" DataNavigateUrlFields="userid" />
                            <asp:HyperLinkField DataNavigateUrlFormatString="~/management/frm_SalesVisitActivity.aspx?id={0}&amp;type=SW"
                                NavigateUrl="~/management/frm_SalesVisitActivity.aspx" DataTextField="Scheduled End Date"
                                HeaderText="Scheduled End Date" DataNavigateUrlFields="userid">
                                <ControlStyle Width="90px" />
                            </asp:HyperLinkField>
                            <asp:HyperLinkField DataNavigateUrlFormatString="~/management/frm_SalesVisitActivity.aspx?id={0}&amp;type=SW"
                                NavigateUrl="~/management/frm_SalesVisitActivity.aspx" DataTextField="Expected End Date"
                                HeaderText="Expected End Date" DataNavigateUrlFields="userid">
                                <ControlStyle Width="90px" />
                            </asp:HyperLinkField>
                            <asp:HyperLinkField DataNavigateUrlFormatString="~/management/frm_SalesVisitActivity.aspx?id={0}&amp;type=SW"
                                NavigateUrl="~/management/frm_SalesVisitActivity.aspx" DataTextField="Pending Call"
                                HeaderText="Pending Calls" DataNavigateUrlFields="userid" />
                            <asp:HyperLinkField DataNavigateUrlFormatString="~/management/frm_SalesVisitActivity.aspx?id={0}&amp;type=SW"
                                NavigateUrl="~/management/frm_SalesVisitActivity.aspx" DataTextField="Int/Pipeline"
                                HeaderText="Int/Pipeline" DataNavigateUrlFields="userid" />
                            <asp:HyperLinkField DataNavigateUrlFormatString="~/management/frm_SalesVisitActivity.aspx?id={0}&amp;type=SW"
                                NavigateUrl="~/management/frm_SalesVisitActivity.aspx" DataTextField="Refixed By FOS"
                                HeaderText="Refixed By FOS" DataNavigateUrlFields="userid" />
                            <asp:HyperLinkField DataNavigateUrlFormatString="~/management/frm_SalesVisitActivity.aspx?id={0}&amp;type=SW"
                                NavigateUrl="~/management/frm_SalesVisitActivity.aspx" DataTextField="Refixed By Client"
                                HeaderText="Refixed By Client" DataNavigateUrlFields="userid" />
                            <asp:HyperLinkField DataNavigateUrlFormatString="~/management/frm_SalesVisitActivity.aspx?id={0}&amp;type=SW"
                                NavigateUrl="~/management/frm_SalesVisitActivity.aspx" DataTextField="Won/Confirm Sale"
                                HeaderText="Won/Confirm Sale" DataNavigateUrlFields="userid" />
                            <asp:HyperLinkField DataNavigateUrlFormatString="~/management/frm_SalesVisitActivity.aspx?id={0}&amp;type=SW"
                                NavigateUrl="~/management/frm_SalesVisitActivity.aspx" DataTextField="Non Contactable"
                                HeaderText="Non Contactable" DataNavigateUrlFields="userid" />
                            <asp:HyperLinkField DataNavigateUrlFormatString="~/management/frm_SalesVisitActivity.aspx?id={0}&amp;type=SW"
                                NavigateUrl="~/management/frm_SalesVisitActivity.aspx" DataTextField="Non Usable/Fake"
                                HeaderText="Non Usable" DataNavigateUrlFields="userid" />
                            <asp:HyperLinkField DataNavigateUrlFormatString="~/management/frm_SalesVisitActivity.aspx?id={0}&amp;type=SW"
                                NavigateUrl="~/management/frm_SalesVisitActivity.aspx" DataTextField="Lost/Not Int"
                                HeaderText="Lost" DataNavigateUrlFields="userid" />
                            <asp:HyperLinkField DataNavigateUrlFormatString="~/management/frm_SalesVisitActivity.aspx?id={0}&amp;type=SW"
                                NavigateUrl="~/management/frm_SalesVisitActivity.aspx" DataTextField="Refixed By TeleCaller"
                                HeaderText="Refixed By TeleCaller" DataNavigateUrlFields="userid" />
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td class="lt">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblUserName" runat="server" Text="UserName" ForeColor="Red" Visible="False"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="txtUser" runat="server" ForeColor="Red" Visible="False"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr runat="server" id="pnlShowDetail">
                <td>
                    <asp:GridView ID="grdDetail" runat="server" Width="100%" CellPadding="4" ForeColor="#333333"
                        GridLines="None" BorderWidth="1px" BorderColor="#507CD1" AllowPaging="True" OnRowDataBound="grdDetail_RowDataBound"
                        OnPageIndexChanging="grdDetail_PageIndexChanging">
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#EFF3FB" BorderWidth="1px" />
                        <EditRowStyle BackColor="#2461BF" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <HeaderStyle Font-Bold="false" ForeColor="black" CssClass="EHEADER" BorderColor="AliceBlue"
                            BorderWidth="1px" />
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:TemplateField HeaderText="Activity">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkDetail" runat="server" />
                                    <asp:Label Visible="False" ID="lblActNo" runat="Server" Text='<%# Eval("Activity No") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle CssClass="gridheader" />
                    </asp:GridView>
                    &nbsp;
                    <asp:TextBox ID="txtId" runat="server" Visible="false"></asp:TextBox>
                </td>
            </tr>
            <tr id="frmShowCall" runat="Server">
                <td>
                    <asp:Panel ID="pnlCall" runat="server" Width="100%" Visible="false">
                        <table class="TableMain100">
                            <tr>
                                <td class="mylabel1">
                                    Activity Type :
                                </td>
                                <td class="lt" style="width: 278px">
                                    <asp:DropDownList ID="drpActType" AutoPostBack="true" runat="server" Width="98%"
                                        Enabled="False">
                                    </asp:DropDownList>
                                </td>
                                <td class="mylabel1" style="width: 18%">
                                    Start Date/Start Time:
                                </td>
                                <td class="lt" colspan="2">
                                    <%--<asp:TextBox ID="TxtStartDate" runat="server"></asp:TextBox>
                     <asp:Image ID="ImgStartDate" runat="server" ImageUrl="~/images/calendar.jpg" />
                     <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TxtStartDate"
                         ErrorMessage="Required" ValidationGroup="a"></asp:RequiredFieldValidator>--%>
                                    <dxe:ASPxDateEdit ID="TxtStartDate" runat="server" EditFormat="Custom" UseMaskBehavior="True">
                                        <ButtonStyle Width="13px">
                                        </ButtonStyle>
                                    </dxe:ASPxDateEdit>
                                </td>
                            </tr>
                            <tr>
                                <td class="mylabel1">
                                    Assign To :
                                </td>
                                <td class="lt" style="width: 278px">
                                    <asp:DropDownList ID="drpUserWork" runat="server" Width="65%" Visible="false">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txtAssigned" runat="server" onclick="UserList();" Height="19px"
                                        Width="96%"></asp:TextBox><asp:HiddenField ID="hd1UserList" runat="server" />
                                </td>
                                <td class="mylabel1" style="width: 10%">
                                    End Date/End Time :</td>
                                <td class="lt">
                                    <%--<asp:TextBox ID="TxtEndDate" runat="server"></asp:TextBox>
                      <asp:Image ID="ImgEndDate" runat="server" ImageUrl="~/images/calendar.jpg" />
                      <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="TxtEndDate"
                          ErrorMessage="Required" ValidationGroup="a"></asp:RequiredFieldValidator>--%>
                                    <dxe:ASPxDateEdit ID="TxtEndDate" runat="server" EditFormat="Custom" UseMaskBehavior="True">
                                        <ButtonStyle Width="13px">
                                        </ButtonStyle>
                                    </dxe:ASPxDateEdit>
                                </td>
                                <td class="lt">
                                </td>
                            </tr>
                            <tr>
                                <td class="mylabel1">
                                    Description :
                                </td>
                                <td class="lt" style="width: 278px">
                                    <asp:TextBox ID="txtDesc" TextMode="MultiLine" Rows="2" runat="server" Width="97%"
                                        Height="57px"></asp:TextBox>
                                </td>
                                <td class="mylabel1" style="width: 10%">
                                    Priority :
                                </td>
                                <td style="width: 7%; text-align: left;">
                                    <asp:DropDownList ID="drpPriority" runat="server" Width="97%">
                                        <asp:ListItem Text="Low" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Normal" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="High" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Urgent" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="Immediate" Value="4"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td class="mylabel1">
                                    <table>
                                        <tr>
                                            <td>
                                                <input type="button" value="Add Lead" onclick="funAddLead()" id="Button1" class="btn btn-primary"
                                                    style="height: 21px" /></td>
                                            <td>
                                                <input type="button" id="btnAddDocument" name="btnAddDocument" class="btn btn-primary"
                                                    disabled="disabled" value="Add Product" onclick="btnAddProductWindow()" style="height: 21px" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="mylabel1" style="width: 15%">
                                    Instruction Notes :
                                </td>
                                <td class="lt" colspan="4">
                                    <asp:TextBox ID="txtInstNote" runat="server" TextMode="MultiLine" Rows="5" Width="92%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="5">
                                    <asp:Button ID="btnSubmit" Text="Save" runat="server" Width="95px" CssClass="btn btn-primary"
                                        Enabled="true" ValidationGroup="a" OnClick="btnSubmit_Click" Height="21px" />
                                    <asp:Button ID="btnCancel" Text="Cancel" runat="server" Width="95px" CssClass="btn btn-danger"
                                        OnClick="btnCancel_Click" Height="21px" />
                                    <asp:TextBox ID="txtUserList" runat="server" BackColor="Transparent" BorderColor="Transparent"
                                        BorderStyle="None"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="pnlActivityDetail" runat="server" Visible="false" Width="100%">
                        <asp:GridView ID="grdActivityDetail" runat="server" Width="100%" CellPadding="4"
                            ForeColor="#333333" GridLines="None" BorderWidth="1px" BorderColor="#507CD1"
                            OnPageIndexChanging="grdActivityDetail_PageIndexChanging">
                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <RowStyle BackColor="#EFF3FB" BorderWidth="1px" />
                            <EditRowStyle BackColor="#2461BF" />
                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                            <HeaderStyle Font-Bold="false" ForeColor="black" CssClass="EHEADER" BorderColor="AliceBlue"
                                BorderWidth="1px" />
                            <AlternatingRowStyle BackColor="White" />
                        </asp:GridView>
                    </asp:Panel>
                </td>
            </tr>
            <tr id="frmShowGenratedSalesvisit" runat="Server" visible="False">
                <td>
                    <table class="TableMain100">
                        <tr>
                            <td>
                                <table class="TableMain100">
                                    <tr style="display: none">
                                        <td class="lt" colspan="6" style="height: 20px">
                                            <input type="text" runat="Server" id="txtLeadId" name="txtLeadId" style="background-color: #DDECFE;
                                                border-color: #DDECFE; border-style: none; color: #DDECFE" readonly="readOnly"
                                                visible="true" />
                                            <asp:Label ID="lblError" runat="server" ForeColor="#DDECFE" BorderStyle="none" BorderColor="#DDECFE"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td colspan="6">
                                            <table style="border: solid 1px black">
                                                <tr>
                                                    <td colspan="6">
                                                        <table>
                                                            <tr>
                                                                <td class="rt">
                                                                    <asp:RadioButton ID="Lrd" runat="server" GroupName="a" Checked="True" />
                                                                </td>
                                                                <td class="mylabel1">
                                                                    <asp:Label ID="Label4" runat="server" Text="From Lead Data" Font-Size="X-Small" Width="101px"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:RadioButton ID="Erd" runat="server" GroupName="a" />
                                                                </td>
                                                                <td class="mylabel1" colspan="3">
                                                                    <asp:Label ID="Label5" runat="server" Text="From Existing Customer Data" Font-Size="X-Small"
                                                                        Width="176px"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="mylabel1" style="width: 22%;">
                                                        Select Date Range :</td>
                                                    <td style="width: 15%;">
                                                        <dxe:ASPxDateEdit ID="FromDate" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                            Width="178px">
                                                            <ButtonStyle Width="13px">
                                                            </ButtonStyle>
                                                        </dxe:ASPxDateEdit>
                                                    </td>
                                                    <td class="mylabel1" style="width: 2%;">
                                                        To :</td>
                                                    <td style="width: 5%;">
                                                        <dxe:ASPxDateEdit ID="ToDate" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                            Width="178px">
                                                            <ButtonStyle Width="13px">
                                                            </ButtonStyle>
                                                        </dxe:ASPxDateEdit>
                                                    </td>
                                                    <td class="mylabel1" style="width: 10%">
                                                        Assigned/UnAssigned:
                                                    </td>
                                                    <td style="width: 247px">
                                                        <asp:DropDownList ID="drpSelect" runat="server" Width="184px">
                                                            <asp:ListItem>All</asp:ListItem>
                                                            <asp:ListItem>Assigned</asp:ListItem>
                                                            <asp:ListItem>UnAssigned</asp:ListItem>
                                                        </asp:DropDownList></td>
                                                </tr>
                                                <tr>
                                                    <td class="mylabel1">
                                                        Product Type :</td>
                                                    <td colspan="3" style="border: solid 1px white;">
                                                        <table class="TableMain100">
                                                            <tr>
                                                                <td colspan="2">
                                                                    <input type="radio" runat="Server" id="Radio1" name="rdr" value="All" checked="True"
                                                                        onclick="javascript:fun(this,true)" />
                                                                    All
                                                                </td>
                                                                <td>
                                                                    <input type="radio" runat="Server" id="Radio2" name="rdr" value="Select" onclick="javascript:fun(this,false)" />
                                                                    Selective</td>
                                                                <td>
                                                                    <asp:DropDownList ID="drpProduct" runat="server" Enabled="false" Width="251px">
                                                                        <asp:ListItem Text="Broking &amp; DP Account" Value="Broking &amp; DP Account"></asp:ListItem>
                                                                        <asp:ListItem Text="Mutual Fund" Value="Mutual Fund"></asp:ListItem>
                                                                        <asp:ListItem Text="Insurance" Value="Insurance"></asp:ListItem>
                                                                    </asp:DropDownList></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td colspan="2" valign="bottom" align="left">
                                                        <asp:Button ID="btnshowGenratedSaleVisit" runat="server" Text="Show" CssClass="btn btn-primary"
                                                            OnClick="btnshowGenratedSaleVisit_Click" Height="21px" /></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr id="TdAllot" runat="server">
                                        <td colspan="6">
                                            <asp:Button ID="btnSelectAll" runat="server" Text="Select All" Visible="false" CssClass="btn btn-primary"
                                                OnClick="btnSelectAll_Click" Height="21px" />
                                            <asp:Button ID="btnExport" runat="server" Text="Export" Visible="false" CssClass="btn btn-primary"
                                                Height="21px" OnClick="btnExport_Click" />
                                            <asp:Label ID="lblTotalRecord" runat="server" ForeColor="red"></asp:Label>
                                            <br />
                                            <br />
                                            <asp:GridView ID="grdGenratedSalesVisit" runat="server" AutoGenerateColumns="true"
                                                AllowPaging="True" PageSize="6" Width="100%" AllowSorting="True" CellPadding="4"
                                                ForeColor="#333333" GridLines="None" BorderWidth="1px" BorderColor="#507CD1"
                                                OnRowDataBound="grdGenratedSalesVisit_RowDataBound" OnPageIndexChanging="grdGenratedSalesVisit_PageIndexChanging"
                                                OnSorting="grdGenratedSalesVisit_Sorting">
                                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Sel">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkSel" runat="server"></asp:CheckBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblId" runat="Server" Text='<%# Eval("LeadId") + "@@@@" +  Eval("ProductId") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <PagerTemplate>
                                                    <table width="100%">
                                                        <tr>
                                                            <td colspan="4" style="text-align: right; color: white; font-weight: bold">
                                                                [Records
                                                                <%if (grdGenratedSalesVisit.PageCount == grdGenratedSalesVisit.PageIndex + 1)
                                                                  {%>
                                                                <%= grdGenratedSalesVisit.PageIndex * grdGenratedSalesVisit.PageSize%>
                                                                -
                                                                <%= grdGenratedSalesVisit.PageIndex * grdGenratedSalesVisit.PageSize + grdGenratedSalesVisit.Rows.Count - 1%>
                                                                <%}
                                                                  else
                                                                  {%>
                                                                <%= grdGenratedSalesVisit.PageIndex * grdGenratedSalesVisit.PageSize%>
                                                                -
                                                                <%= grdGenratedSalesVisit.PageIndex * grdGenratedSalesVisit.PageSize + grdGenratedSalesVisit.PageSize - 1%>
                                                                <%}%>
                                                                ]</td>
                                                            <td style="text-align: left;">
                                                                <asp:ImageButton ID="FirstPage" runat="server" CommandName="First" OnCommand="NavigationLink_Click"
                                                                    ImageUrl="~/images/pFirst.png" />
                                                                <asp:ImageButton ID="PreviousPage" runat="server" CommandName="Prev" OnCommand="NavigationLink_Click"
                                                                    ImageUrl="~/images/pPrev.png" />
                                                                <asp:ImageButton ID="NextPage" runat="server" CommandName="Next" OnCommand="NavigationLink_Click"
                                                                    ImageUrl="~/images/pNext.png" />
                                                                <asp:ImageButton ID="LastPage" runat="server" CommandName="Last" OnCommand="NavigationLink_Click"
                                                                    ImageUrl="~/images/pLast.png" />
                                                            </td>
                                                            <td colspan="2" style="text-align: right; font-weight: bold">
                                                                <asp:Literal ID="litDiff" runat="server"></asp:Literal>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </PagerTemplate>
                                                <RowStyle BackColor="#EFF3FB" BorderWidth="1px" />
                                                <EditRowStyle BackColor="#2461BF" />
                                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                                <HeaderStyle Font-Bold="False" ForeColor="Black" BorderColor="AliceBlue" BorderWidth="1px" />
                                                <AlternatingRowStyle BackColor="White" />
                                            </asp:GridView>
                                            <asp:HiddenField ID="CurrentPage" runat="server"></asp:HiddenField>
                                            <asp:HiddenField ID="TotalPages" runat="server"></asp:HiddenField>
                                            <asp:HiddenField ID="TotalClient" runat="server" />
                                        </td>
                                    </tr>
                                    <tr id="frmAllot" runat="Server" visible="False">
                                        <td colspan="6" style="height: 23px">
                                            <input type="button" id="btnAllot" name="btnAllot" value="Allot" onclick="windowopenform()"
                                                class="btn btn-primary"  />
                                            <asp:Button ID="btnCancelGenratedSalesVisit" runat="server" Text="Cancel" CssClass="btn btn-danger"
                                                OnClick="btnCancelGenratedSalesVisit_Click" />
                                        </td>
                                    </tr>
                                    <tr id="Td1" runat="Server" visible="False">
                                        <td colspan="6">
                                            <input type="button" id="Button2" name="btnAllot" value="Allot" onclick="windowopenform1()"
                                                class="btn btn-primary" />
                                            <asp:Button ID="Button3" runat="server" Text="Cancel" CssClass="btn btn-danger" OnClick="btnCancelGenratedSalesVisit_Click"
                                                 />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
            </div>
</asp:Content>
