<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.management_verify_transaction" MaintainScrollPositionOnPostback="true"
    EnableEventValidation="false" CodeBehind="verify_transaction.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <script lang="javascript" type="text/javascript">

	  function  confirmation()
               {
                 var combo=document.getElementById('ddlExport');
                 combo.value='Ex';               
                 return confirm('Are you sure you want to delete this?');                
               }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script lang="javascript" type="text/javascript">
       function selecttion()
        {
            var combo=document.getElementById('ddlExport');
            combo.value='Ex';
        }
    function large(obj)
    {     
         //OnMoreInfoClick(obj,"Large Image",'940px','450px',"N");     
         if('<%= dp %>'=='CDSL')
           {   
             var url='view_signature.aspx?id=' + obj+'[CDSL]';
             OnMoreInfoClick(url,"View Signature",'940px','450px',"Y");         
           }
         else
           {
            var url='view_signature.aspx?id=' + obj+'[NSDL]';
            OnMoreInfoClick(url,"View Signature",'940px','450px',"Y");      
           }    
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
            window.frameElement.width = document.body.scrollWidth;
        }
        
    function popup(obj2,obj1)
    {
        if(obj1==1)
        {
           var x=window.confirm("Already rejected.Do you want to accept it?");
                if(x)
                {
                  document.getElementById('reject').value=obj2;
                  document.getElementById('Button2').click();
                  //document.getElementById('Button1').click();
                  document.getElementById('btnsave').click();
                }
                else
                {                 

                }
        }
        else
        {
            var url='rejection_signature.aspx?id='+obj2+'~'+'<%= dp %>';
            OnMoreInfoClick(url,"Rejection",'540px','250px',"Y");    
        }
           
    }
    function ChangeRowColor(obj,obj1)
    {
//        alert(obj);
    }
   function SignOff()
        {
            window.parent.SignOff();
        }

      
      function PageLoad()
        {
            //alert(new Date());
            FieldName='btnshow';     
            document.getElementById('hide').style.display='none';    
            //document.getElementById("branch").style.display='block';   
            document.getElementById("user").style.display='block';   
            document.getElementById("usertext").style.display='none';  
            //document.getElementById('brtext').style.display='none';  
            //dtexec1.SetDate(new Date()); 
             if(dtexec1.GetDate()!=null)
            {
              if(dtexec1.GetDate()!=new Date())
             {
             }              
            }
            else
            {
              dtexec1.SetDate(new Date()); 
            }
        } 
        
           function callback() 
            {              
                selecttion();
                document.getElementById('Button1').click();
            }

           function select()
           {
            var e = document.getElementById("ddltransact"); 
            var transact = e.options[e.selectedIndex].text; 
            //alert(transact);
            if(transact=='Branch')
            {
             
             document.getElementById("user").style.display='none';    
             document.getElementById('branch').style.display='block';
            }
            else
            {
             
             document.getElementById("branch").style.display='none';   
             document.getElementById('user').style.display='block';
            }
           }
       function DisableBranch(obj)
        {
         
//            var gridview = document.getElementById('offlineGrid'); 
//            var rCount = gridview.rows.length;
//           
//            if(rCount<10)
//                rCount='0'+rCount;
//           
//            if(obj=='P')
//            {
//                document.getElementById("offlineGrid_ctl"+rCount+"_FirstPage").style.display='none';
//                document.getElementById("offlineGrid_ctl"+rCount+"_PreviousPage").style.display='none';
//                document.getElementById("offlineGrid_ctl"+rCount+"_NextPage").style.display='inline';
//                document.getElementById("offlineGrid_ctl"+rCount+"_LastPage").style.display='inline';
//            }
//            else
//            {
//                document.getElementById("offlineGrid_ctl"+rCount+"_NextPage").style.display='none';
//                document.getElementById("offlineGrid_ctl"+rCount+"_LastPage").style.display='none';
//            }
        }
                function CallAjax(obj1,obj2,obj3)
                {        
                    //alert('aa');                                                 
                    ajax_showOptions(obj1,obj2,obj3,null,'Main');
                }
                  function keyVal(obj)
                {  
//                    alert(obj); 
                    var a=new String(obj);

                   if(a.split(',')[1]==null)
                   {
                     document.getElementById("hidebranch").value=a.split(',')[0];
                   }
                   else
                   {
                     document.getElementById("hideuser").value=a.split(',')[1];
                   }
//                   alert(document.getElementById("hideuser").value);
                   //alert(document.getElementById("hidebranch").value);
                }
               function  selectbranch(obj)
               {
               /*
                    if(obj=='rdbranch')
                    {
                         var radioButtons = document.getElementsByName("rdbranch");
                          for (var x = 0; x < radioButtons.length; x ++) 
                          {
                            if (radioButtons[x].checked) 
                            {
                                    if(radioButtons[x].id=='rdbranch_0')
                                    {
                                         document.getElementById("usertext").style.display='none';  
                                         document.getElementById('brtext').style.display='none'; 
                                    }
                                    else
                                    {
                                         
                                         document.getElementById("usertext").style.display='none';   
                                         document.getElementById('brtext').style.display='block';
                                    }

                            }
                          }
                    }*/
                     if(obj=='rduser')
                    {
                         var radioButtons = document.getElementsByName("rduser");
                          for (var x = 0; x < radioButtons.length; x ++) 
                          {
                            if (radioButtons[x].checked) 
                            {
                                    if(radioButtons[x].id=='rduser_0')
                                    {
                                         document.getElementById("usertext").style.display='none';  
                                         //document.getElementById('brtext').style.display='none'; 
                                    }
                                    else
                                    {
                                         
                                         document.getElementById("usertext").style.display='block';   
                                         //document.getElementById('brtext').style.display='none';
                                    }

                            }
                          }
                    }
                }
    function OnMoreInfoClick(id,id1) 
    {  
                    //alert(id);       alert(id1);             
//                    document.getElementById("hideid").value='goto'+','+id;
//                    document.getElementById('Button1').click();
//                    alert(document.getElementById("hideid").value);
//                    if(document.getElementById("hideid").value=='')
//                    {
                        if(id1!='Demat Transaction')
                        {
                            if(dptype=='CDSL')
                            {
                            var url='edit_verification.aspx?id=' + id;
                            OnMoreInfoClick(url,"Edit Settlement Details",'940px','450px',"Y"); 
                            }
                            else
                            {
                             var url='edit_nsdl_verification.aspx?id=' + id;
                             OnMoreInfoClick(url,"Edit Settlement Details",'940px','450px',"Y"); 
                            }
                        }
                        else
                        {
                             if(dptype=='NSDL')
                                {                               
                                 var url='nsdl_demat_transaction.aspx?id=' + id;
                                 OnMoreInfoClick(url,"Edit Demat Details",'940px','450px',"Y"); 
                                }
                        }
//                    }   
             
          
    }
                    function changeddl()
                    {
                        var combo=document.getElementById('ddlExport');
                        combo.value='Ex';
                    }
    var dptype='<%= dp %>'; 
    </script>


    <div>
        <table align="left">
            <tr>
                <td colspan="8">&nbsp;<asp:ScriptManager ID="s1" runat="server" EnablePageMethods="true" AsyncPostBackTimeout="36000">
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

                </td>
            </tr>
            <tr>
                <td style="width: 100px" align="left" valign="top">Transaction Date</td>
                <td align="left" colspan="2" valign="top">
                    <dxe:ASPxDateEdit ID="dtexec" runat="server" ClientInstanceName="dtexec1" DateOnError="Today"
                        EditFormat="Custom" EditFormatString="dd-MM-yyyy" TabIndex="1" UseMaskBehavior="True"
                        Width="150px">
                    </dxe:ASPxDateEdit>
                </td>
                <td style="width: 100px" align="left" valign="top"></td>
                <td style="width: 100px" valign="top"></td>
                <td style="width: 100px" valign="top"></td>
                <td style="width: 100px" valign="top"></td>
                <td style="width: 100px" valign="top"></td>
            </tr>
            <tr id="user">
                <td style="width: 100px" align="left" valign="top">User</td>
                <td style="width: 51px" align="left" valign="top">
                    <asp:RadioButtonList ID="rduser" runat="server" RepeatDirection="Horizontal" TabIndex="3"
                        onclick="selectbranch('rduser')">
                        <asp:ListItem Selected="True">All</asp:ListItem>
                        <asp:ListItem>Selected</asp:ListItem>
                    </asp:RadioButtonList></td>
                <td colspan="2" id="usertext" align="left" valign="top">
                    <asp:TextBox ID="txtuser" runat="server" Width="95%"></asp:TextBox></td>
                <td style="width: 100px" valign="top"></td>
                <td style="width: 100px" valign="top"></td>
                <td style="width: 100px" valign="top"></td>
                <td style="width: 100px" valign="top"></td>
            </tr>
            <tr>
                <td style="width: 100px" align="left" valign="top">
                    <asp:Button ID="btnshow" runat="server" OnClick="btnshow_Click" OnClientClick="selecttion()"
                        Text="Show" TabIndex="4" /></td>
                <td style="width: 51px" align="left" valign="top">Export To</td>
                <td style="width: 100px" valign="top">
                    <asp:DropDownList ID="ddlExport" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlExport_SelectedIndexChanged"
                        Width="94px">
                        <asp:ListItem Value="Ex">Export</asp:ListItem>
                        <asp:ListItem Value="P">PDF</asp:ListItem>
                    </asp:DropDownList></td>
                <td style="width: 100px" valign="top"></td>
                <td style="width: 100px" valign="top"></td>
                <td style="width: 100px" valign="top"></td>
                <td style="width: 100px" valign="top"></td>
                <td style="width: 100px" valign="top"></td>
            </tr>
            <tr>
                <td align="left" style="width: 100px" valign="top"></td>
                <td style="width: 51px" valign="top"></td>
                <td style="width: 100px" valign="top">
                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel2">
                        <ProgressTemplate>
                            <div id='Div1' style='position: absolute; font-family: arial; font-size: 30; left: 50%; top: 50%; background-color: white; layer-background-color: white; height: 80; width: 150;'>
                                <table width='100' height='35' border='1' cellpadding='0' cellspacing='0' bordercolor='#C0D6E4'>
                                    <tr>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td height='25' align='center' bgcolor='#FFFFFF'>
                                                        <img src='/assests/images/progress.gif' width='18' height='18'></td>
                                                    <td height='10' width='100%' align='center' bgcolor='#FFFFFF'>
                                                        <font size='2' face='Tahoma'><strong align='center'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Loading..</strong></font></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </td>
                <td style="width: 100px" valign="top"></td>
                <td style="width: 100px" valign="top"></td>
                <td style="width: 100px" valign="top"></td>
                <td style="width: 100px" valign="top"></td>
                <td style="width: 100px" valign="top"></td>
            </tr>
            <tr>
                <td colspan="8" rowspan="2" align="left">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Panel ID="Panel1" runat="server" Height="420px" ScrollBars="Vertical" Visible="false">
                                <table width="98%">
                                    <tr>
                                        <td colspan="3">
                                            <asp:GridView ID="offlineGrid" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                BorderColor="CornflowerBlue" BorderStyle="Solid" BorderWidth="2px" DataKeyNames="id"
                                                ForeColor="#0000C0" OnRowCommand="offlineGrid_RowCommand" OnSorting="offlineGrid_Sorting"
                                                PageSize="5" ShowFooter="True" Width="100%" OnRowDataBound="offlineGrid_RowDataBound"
                                                OnRowCreated="offlineGrid_RowCreated" OnRowDeleting="offlineGrid_RowDeleting">
                                                <RowStyle BackColor="White" BorderColor="#BFD3EE" BorderStyle="Double" BorderWidth="1px"
                                                    ForeColor="#330099" />
                                                <Columns>
                                                    <asp:TemplateField Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblid" runat="server" Text='<%# Bind("id") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="SlipNo." SortExpression="SlipNumber">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblslip" runat="server" CssClass="gridstyleheight1" Text='<%# Eval("SlipNumber") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle Font-Bold="False" HorizontalAlign="Center" />
                                                        <ItemStyle BorderWidth="1px" HorizontalAlign="Left" Wrap="False" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Client Name[ID].">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblclient" runat="server" CssClass="gridstyleheight1" Text='<%# Eval("AccName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle Font-Bold="False" HorizontalAlign="Center" />
                                                        <ItemStyle BorderWidth="1px" HorizontalAlign="Left" Wrap="False" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="ISIN.">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblisin" runat="server" CssClass="gridstyleheight1" Text='<%# Eval("isin") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle Font-Bold="False" HorizontalAlign="Center" />
                                                        <ItemStyle BorderWidth="1px" HorizontalAlign="Left" Wrap="False" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="QTY">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblqty" runat="server" CssClass="gridstyleheight1" Text='<%# Eval("Quantity") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle Font-Bold="False" HorizontalAlign="Center" />
                                                        <ItemStyle BorderWidth="1px" HorizontalAlign="Left" Wrap="False" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Amount.">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblamount" runat="server" CssClass="gridstyleheight1" Text='<%# Eval("amount") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle Font-Bold="False" HorizontalAlign="Center" />
                                                        <ItemStyle BorderWidth="1px" HorizontalAlign="Left" Wrap="False" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Counter-Account">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblcounter" runat="server" CssClass="gridstyleheight1" Text='<%# Eval("counterid") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle Font-Bold="False" HorizontalAlign="Center" />
                                                        <ItemStyle BorderWidth="1px" HorizontalAlign="Center" Wrap="False" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Fromsettlment">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("fromsett") %>'></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("fromsett") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Tosettlement">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("tosett") %>'></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label3" runat="server" Text='<%# Bind("tosett") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="executiondate" HeaderText="Execution Date" />
                                                    <asp:BoundField DataField="trantype" HeaderText="Transaction Type" />
                                                    <asp:TemplateField HeaderText="Verify">
                                                        <HeaderTemplate>
                                                            Verify
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="CheckBox3" runat="server" />
                                                        </ItemTemplate>
                                                        <HeaderStyle Font-Bold="False" HorizontalAlign="Center" />
                                                        <ItemStyle BorderWidth="1px" HorizontalAlign="Right" Wrap="False" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Reject">
                                                        <HeaderTemplate>
                                                            Reject
                                                        </HeaderTemplate>
                                                        <HeaderStyle Font-Bold="False" HorizontalAlign="Center" />
                                                        <ItemStyle BorderWidth="1px" HorizontalAlign="Right" Wrap="False" />
                                                        <ItemTemplate>
                                                            <asp:Button ID="btnreject" runat="server" CommandArgument='<%# Eval("id") %>' CommandName="reject"
                                                                Text="Reject" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="View Signature">
                                                        <ItemTemplate>
                                                            <a href="javascript:void(0);" onclick="large('<%# Eval("ClientID") %>')">View Signature....
                                                            </a>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="verifyuser" HeaderText="verified" Visible="False" />
                                                    <asp:TemplateField HeaderText="rejected" Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("Rejected") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Edit">
                                                        <ItemTemplate>
                                                            <a href="javascript:void(0);" onclick="OnMoreInfoClick('<%# Eval("id") %>','<%# Eval("trantype")%>')">More Info...</a>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Delete">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                                                OnClientClick="return confirmation();" Text="Delete"></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <PagerStyle ForeColor="White" HorizontalAlign="Center" />
                                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                <HeaderStyle BorderColor="AliceBlue" BorderWidth="1px" CssClass="EHEADER" Font-Bold="False"
                                                    ForeColor="Black" />
                                                <EditRowStyle BackColor="#E59930" />
                                                <PagerTemplate>
                                                    <table>
                                                        <tr>
                                                            <td colspan="10" style="height: 34px">
                                                                <asp:LinkButton ID="FirstPage" runat="server" CommandName="First" Font-Bold="true"
                                                                    OnCommand="NavigationLink_Click1" Text="[First Page]"> </asp:LinkButton>
                                                                <asp:LinkButton ID="PreviousPage" runat="server" CommandName="Prev" Font-Bold="true"
                                                                    OnCommand="NavigationLink_Click1" Text="[Previous Page]">  </asp:LinkButton>
                                                                <asp:LinkButton ID="NextPage" runat="server" CommandName="Next" Font-Bold="true"
                                                                    OnCommand="NavigationLink_Click1" Text="[Next Page]">
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="LastPage" runat="server" CommandName="Last" Font-Bold="true"
                                                                    OnCommand="NavigationLink_Click1" Text="[Last Page]">
                                                                </asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </PagerTemplate>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100px" align="left">
                                            <asp:Button ID="btnsave" runat="server" OnClientClick="changeddl();" OnClick="btnsave_Click" Text="Save" Visible="False" /></td>
                                        <td style="width: 100px">
                                            <asp:HiddenField ID="CurrentPage" runat="server" />
                                        </td>
                                        <td style="width: 100px">
                                            <asp:HiddenField ID="TotalPages" runat="server" />
                                        </td>
                                    </tr>
                                    <tr id="Tr1">
                                        <td></td>
                                        <td>
                                            <asp:HiddenField ID="TotalClient" runat="server" />
                                        </td>
                                        <td>
                                            <asp:HiddenField ID="reject" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td id="Td1"></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnshow" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="Button2" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
            </tr>
            <tr>
                <td style="width: 100px">
                    <asp:Button ID="btncan" runat="server" Text="Cancel" OnClick="btncan_Click" Visible="False" /></td>
                <td style="width: 51px"></td>
                <td style="width: 100px"></td>
                <td style="width: 100px"></td>
                <td style="width: 100px"></td>
                <td style="width: 100px"></td>
                <td style="width: 100px"></td>
                <td style="width: 100px"></td>
            </tr>
            <tr>
                <td style="width: 100px" id="hide">
                    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Button" />
                    <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="Button" /></td>
                <td style="width: 51px">
                    <asp:HiddenField ID="hidebranch" runat="server" />
                </td>
                <td style="width: 100px">
                    <asp:HiddenField ID="hideuser" runat="server" />
                </td>
                <td style="width: 100px">
                    <asp:HiddenField ID="hideid" runat="server" />
                </td>
                <td style="width: 100px"></td>
                <td style="width: 100px"></td>
                <td style="width: 100px"></td>
                <td style="width: 100px"></td>
            </tr>
        </table>
    </div>
</asp:Content>
