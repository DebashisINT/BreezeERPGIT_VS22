<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.management_frmSalesInsurance1" Codebehind="frmSalesInsurance1.aspx.cs" %>

<%--<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe000001" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../CSS/style.css" rel="stylesheet" type="text/css" />
    <link type="text/css" rel="stylesheet" href="../CSS/dhtmlgoodies_calendar.css" media="screen" />

    <script type="text/javascript" src="/assests/js/dhtmlgoodies_calendar.js?random=20060118"></script>

    <script language="javascript" type="text/javascript">
        function callfunction(chkObj,txtObj)
        {   
            var c= document.getElementById(chkObj);
            var a= document.getElementById(txtObj);
            alert(c.checked)
            if (c.checked == true)
            {
                a.disabled = true;
            }
            else
            {
                a.disabled = false;
            }
        }
        function frmOpenNewWindow1(location,v_height,v_weight)
        {
            var y=(screen.availHeight-v_height)/2;
            var x=(screen.availWidth-v_weight)/2;
            window.open(location,"Search_Conformation_Box","height="+ v_height +",width="+ v_weight +",top="+ y +",left="+ x +",location=no,directories=no,menubar=no,toolbar=no,status=yes,scrollbars=no,resizable=no,dependent=no");       
        }

        function setvisibleforcall()
        {
         document.getElementById("TdVisitPlace").style.display = 'none';
         document.getElementById("TdVisitPlace1").style.display = 'none';
        }
        function setvisibleforvisit()
        {
        document.getElementById("TdVisitPlace").style.display = 'inline';
        document.getElementById("TdVisitPlace1").style.display = 'inline';
        }
        function CallForms(val)
        {
         parent.CallForms(val);
   
        }

    function CallForms(val)
    {
      
       document.getElementById("drpBanks").style.display ='none';
       document.getElementById("drpSubsequentMethod").style.display ='none';
       document.getElementById("drpMedicalReports").style.display ='none';
       document.getElementById("drpMode").style.display ='none';
       document.getElementById("drpMedical").style.display ='none';
       document.getElementById("drpKYCDocument").style.display ='none';
       document.getElementById("drpSignatureProof").style.display ='none';
       document.getElementById("drpAgeProof").style.display ='none';
       document.getElementById("drpAddressProof").style.display ='none';
       document.getElementById("drpPhotoIdProof").style.display ='none';
       document.getElementById("drpProductApplicationForm").style.display ='none';
       document.getElementById("drpNextVisitPurpose").style.display ='none';
       document.getElementById("DrpNextVisitPlace").style.display ='none'; 
       document.getElementById("drpSalesStage").style.display ='none'; 
//       document.getElementById("DrpVisitPlace").style.display ='none'; 
//       document.getElementById("drpVisitPurpose").style.display ='none'; 
       parent.CallForms(val);
 
    }
    function ClosingDHTML()
    {
        
        document.getElementById("drpBanks").style.display ='inline';
        document.getElementById("drpSubsequentMethod").style.display ='inline';
        document.getElementById("drpMedicalReports").style.display ='inline';
        document.getElementById("drpMode").style.display ='inline';
        document.getElementById("drpMedical").style.display ='inline';
        document.getElementById("drpKYCDocument").style.display ='inline';
        document.getElementById("drpSignatureProof").style.display ='inline';
        document.getElementById("drpAgeProof").style.display ='inline';
        document.getElementById("drpAddressProof").style.display ='inline';
        document.getElementById("drpPhotoIdProof").style.display ='inline';
        document.getElementById("drpProductApplicationForm").style.display ='inline';
        document.getElementById("drpNextVisitPurpose").style.display ='inline';
        document.getElementById("DrpNextVisitPlace").style.display ='inline'; 
        document.getElementById("drpSalesStage").style.display ='inline'; 
        document.getElementById("DrpVisitPlace").style.display ='inline';
        document.getElementById("drpVisitPurpose").style.display ='inline';  
    }
    function ActionAfterSave()
    {
       
        parent.AfterSave();
    }
    
   
    function height()
    {
             
        window.frameElement.height = document.body.scrollHeight;
        window.frameElement.Width = document.body.scrollWidth;
        parent.height();
    }
         function heightchild()
    {
 
        window.frameElement.height = document.body.scrollHeight;
        window.frameElement.Width = document.body.scrollWidth;
    }
        
        function funChangeNext(obj)
        {
            
            var o = document.getElementById("lblNextVisitDate")
            var s = document.getElementById("lblNextVisitPurpose")
            if (obj.id == "rdrCall")
            {
                o.innerText = "Next Call Date"
                s.innerText = "Next Call Purpose"
                document.getElementById("TdVisitPlace").style.display = 'none';
                document.getElementById("TdVisitPlace1").style.display = 'none';
            }
            else
            {
                o.innerText = "Next Visit Date"
                s.innerText = "Next Visit Purpose"
                document.getElementById("TdVisitPlace").style.display = 'inline';
                document.getElementById("TdVisitPlace1").style.display = 'inline';
            }
        }
        function functionSalesStage(obj1)
        {
            
            var obj = document.getElementById(obj1)
            var s = document.getElementById('lbllogindate')
            if (obj.value == 2)
            {
                s.innerText = 'Login Date';
                document.getElementById("Login").style.display = 'inline';
                document.getElementById("Login1").style.display = 'inline';
                document.getElementById("rdrCall").disabled = true;
                document.getElementById("rdrVisit").disabled= true;
                document.getElementById("NVisit").style.display = 'none';
                document.getElementById("NVisit1").style.display = 'none';
                document.getElementById("drpNextVisitPurpose").disabled = true;
                document.getElementById("DrpNextVisitPlace").disabled = true;
            }
            else
            {
                if(obj.value == 3)
                {
                    s.innerText = 'Probable Closing Date';
                    document.getElementById("Login").style.display = 'inline';
                    document.getElementById("Login1").style.display = 'inline';
                    document.getElementById("rdrCall").disabled = false;
                    document.getElementById("rdrVisit").disabled = false;
                    document.getElementById("NVisit").style.display = 'inline';
                    document.getElementById("NVisit1").style.display = 'inline';
                    document.getElementById("drpNextVisitPurpose").disabled = false; 
                    document.getElementById("DrpNextVisitPlace").disabled = false;  
                }
                else
                {
                    document.getElementById("Login").style.display = 'none';
                    document.getElementById("Login1").style.display = 'none';
                    document.getElementById("rdrCall").disabled = false;
                    document.getElementById("rdrVisit").disabled = false;
                    document.getElementById("NVisit").style.display = 'inline';
                    document.getElementById("NVisit1").style.display = 'inline';
                    document.getElementById("drpNextVisitPurpose").disabled = false; 
                    document.getElementById("DrpNextVisitPlace").disabled = false;  
                }
            }
        }
         function CheckDate()
        {

           var bdate=NDate.GetDate();
           var tdate=new Date();
         
           var date1 = tdate.getMonth()+1 +'/'+ tdate.getDate()+'/'+tdate.getFullYear();
           var date2 = bdate.getMonth()+1 +'/'+ bdate.getDate()+'/'+bdate.getFullYear();
        
           var new_date1=new Date(date1);
           var new_date2=new Date(date2);
           if(new_date1>new_date2)
           {
            alert('Date can not be less than Current Date');
           }
           
        }
    </script>

    <!-- THis code will help us to open the pages in the Modal DHTML window -->
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />

    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>

    <link rel="stylesheet" href="../modalfiles/modal.css" type="text/css" />

    <script type="text/javascript" src="../modalfiles/modal.js"></script>
    </asp:Content>
 
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        
            <table class="TableMain100" id="showData" runat="Server">
                <tr>
                    <td id="ShowCallInformation" runat="Server" visible="False" colspan="2">
                        <table class="TableMain100">
                            <tr>
                                <td style="width: 2%">
                                    <asp:Label ID="Label1" runat="server" Width="70px" CssClass="mylabel">Alloated By:</asp:Label></td>
                                <td style="width: 4%">
                                    <asp:Label ID="txtAlloatedBy" runat="Server" Width="100%"></asp:Label>
                                </td>
                                <td style="width: 2%">
                                    <asp:Label ID="Label3" runat="server" CssClass="mylabel" Width="70px">Allotted On :</asp:Label></td>
                                <td style="width: 2%">
                                    <asp:Label ID="txtDateOfAllottment" runat="Server" Width="131px"></asp:Label>
                                </td>
                                <td style="width: 2%">
                                    <asp:Label ID="Label5" runat="server" Width="70px" CssClass="mylabel">Priority : </asp:Label></td>
                                <td style="width: 2%">
                                    <asp:Label ID="txtPriority" runat="Server" Width="133px"></asp:Label></td>
                                <td style="width: 2%">
                                    <asp:Label ID="Label2" runat="server" CssClass="mylabel" Width="60px">Start By :</asp:Label></td>
                                <td>
                                    <asp:Label ID="txtSeheduleStart" runat="server"></asp:Label>
                                    <asp:Label ID="txtTotalNumberofCalls" runat="Server" Visible="false"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label4" runat="server" CssClass="mylabel" Width="70px">End By :</asp:Label></td>
                                <td>
                                    <asp:Label ID="txtSeheduleEnd" runat="server"></asp:Label></td>
                                <td>
                                    <asp:Label ID="Label6" runat="server" Width="70px" CssClass="mylabel">Started On: </asp:Label></td>
                                <td>
                                    <asp:Label ID="txtAcutalStart" runat="Server" Width="129px"></asp:Label></td>
                                <td>
                                    <asp:Label ID="Label7" runat="server" CssClass="mylabel" Width="70px">Instruction:</asp:Label></td>
                                <td colspan="3">
                                    <asp:Label ID="lblShortNote" runat="Server" Width="100%"></asp:Label></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table class="TableMain100">
                            <tr>
                                <td class="lt">
                                    <asp:Button ID="btnEdit" runat="Server" Text="Update Visit" CssClass="btnUpdate"
                                        OnClick="btnEdit_Click" Height="18px" />
                                    <asp:Button ID="btnPhoneFollowup" runat="Server" Text="Update Phone FollowUp" CssClass="btnUpdate"
                                        OnClick="btnPhoneFollowup_Click" Height="18px" Width="144px" /></td>
                                <td class="lt" style="height: 12px">
                                    <asp:HyperLink ID="hpaddphone" runat="server" CssClass="decoration" NavigateUrl="javascript:void(0)"
                                        onclick="CallForms('Address')" ForeColor="#8080FF" Width="63px">Address/Phone</asp:HyperLink>
                                    &nbsp;
                                    <asp:HyperLink ID="hpContact" runat="server" CssClass="decoration" NavigateUrl="javascript:void(0)"
                                        onclick="CallForms('Contact')" ForeColor="#8080FF" Width="41px">Contact</asp:HyperLink>
                                    &nbsp;
                                    <asp:HyperLink ID="hpFinance" runat="server" CssClass="decoration" NavigateUrl="javascript:void(0)"
                                        onclick="CallForms('Bank')" ForeColor="#8080FF" Width="63px">Bank/Finance</asp:HyperLink>
                                    &nbsp;
                                    <asp:HyperLink ID="hpReg" runat="server" CssClass="decoration" NavigateUrl="javascript:void(0)"
                                        onclick="CallForms('Registration')" ForeColor="#8080FF" Width="60px">Registration</asp:HyperLink>
                                    &nbsp;&nbsp;<asp:HyperLink ID="hpDocument" runat="server" CssClass="decoration" NavigateUrl="javascript:void(0)"
                                        onclick="CallForms('Document')" ForeColor="#8080FF" Width="50px">Document</asp:HyperLink>&nbsp;
                                    <asp:HyperLink ID="hpHistory" runat="server" CssClass="decoration" NavigateUrl="javascript:void(0)"
                                        onclick="CallForms('History')" ForeColor="#8080FF" Width="45px">History</asp:HyperLink>&nbsp;
                                    <asp:HyperLink ID="HyperLink1" runat="server" CssClass="decoration" NavigateUrl="javascript:void(0)"
                                        onclick="CallForms('Expences')" ForeColor="#8080FF" Width="58px">Expences</asp:HyperLink>&nbsp;
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table class="TableMain100" style="border: solid 1px black">
                            <asp:Panel ID="pnl" runat="Server" Enabled="False">
                                <tr>
                                    <td colspan="2" >
                                        <table>
                                            <tr>
                                                <td class="mylabel1">
                                                    Name</td>
                                                <td>
                                                    <asp:Label ID="txtName" runat="Server" Font-Bold="true"></asp:Label></td>
                                            </tr>
                                            <tr>
                                                <td valign="top" rowspan="2" class="mylabel1">
                                                    Remarks/Note</td>
                                                <td valign="top" rowspan="2">
                                                    <asp:TextBox ID="txtNote" runat="Server" TextMode="MultiLine" Height="75px" Width="401px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                            </tr>
                                            <tr>
                                                <td id="lblVisitDate" runat="server" class="mylabel1">
                                                    Visit Date
                                                </td>
                                                <td>
                                                    <dxe:ASPxDateEdit ID="ASPxVisitDate" runat="server" 
                                                        EditFormat="Custom" EditFormatString="dd MMMM yyyy hh:mm tt" UseMaskBehavior="True"
                                                        Width="212px">
                                                        <ButtonStyle Width="13px">
                                                        </ButtonStyle>
                                                    </dxe:ASPxDateEdit>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td id="TdPlace" runat="server" class="mylabel1">
                                                    Visit Place
                                                </td>
                                                <td id="TdPlace1" runat="server">
                                                    <asp:DropDownList ID="DrpVisitPlace" runat="server" Width="209px">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblVisitPurpose" runat="Server" Text="Visit Purpose" class="mylabel1"></asp:Label></td>
                                                <td>
                                                    <asp:DropDownList ID="drpVisitPurpose" runat="Server" Width="209px">
                                                        <asp:ListItem Value="Product Presentation" Text="Product Presentation"></asp:ListItem>
                                                        <asp:ListItem Value="Document Collection" Text="Document Collection"></asp:ListItem>
                                                        <asp:ListItem Value="Cheque Collection" Text="Cheque Collection"></asp:ListItem>
                                                        <asp:ListItem Value="Medical Reports" Text="Medical Reports"></asp:ListItem>
                                                    </asp:DropDownList></td>
                                            </tr>
                                            <tr>
                                                <td class="mylabel1">
                                                    Insurance Company</td>
                                                <td>
                                                    <asp:Label ID="txtInsurnaceCompany" runat="Server" Font-Bold="True" Width="206px"></asp:Label></td>
                                            </tr>
                                            <tr>
                                                <td class="mylabel1">
                                                    Insurance Plan</td>
                                                <td>
                                                    <asp:Label ID="txtInsurancePlan" runat="Server" Font-Bold="True" Width="205px"></asp:Label></td>
                                            </tr>
                                            <tr>
                                                <td class="mylabel1">
                                                    Application No</td>
                                                <td>
                                                    <asp:TextBox ID="txtApplicationNo" runat="Server" Width="199px"></asp:TextBox></td>
                                            </tr>
                                            <tr>
                                                <td class="mylabel1">
                                                    Sum Assured</td>
                                                <td>
                                                    <asp:TextBox ID="txtSumAssured" runat="Server" Width="199px"></asp:TextBox>[Rs.]</td>
                                            </tr>
                                            <tr>
                                                <td class="mylabel1">
                                                    Premium Paying Term</td>
                                                <td>
                                                    <asp:TextBox ID="txtPremiumPayingTerm" runat="Server" Width="36px"></asp:TextBox>[Years]</td>
                                            </tr>
                                            <tr>
                                                <td class="mylabel1">
                                                    Policy Term</td>
                                                <td>
                                                    <asp:TextBox ID="txtPolicyTerm" runat="Server" Width="36px"></asp:TextBox>[Years]</td>
                                            </tr>
                                            <tr>
                                                <td class="mylabel1">
                                                    Premium</td>
                                                <td>
                                                    <asp:TextBox ID="txtPremium" runat="Server" Width="197px"></asp:TextBox></td>
                                            </tr>
                                            <tr>
                                                <td class="mylabel1">
                                                    Mode</td>
                                                <td>
                                                    <asp:DropDownList ID="drpMode" runat="Server" Width="200px">
                                                        <asp:ListItem>Monthly</asp:ListItem>
                                                        <asp:ListItem>Quarterly</asp:ListItem>
                                                        <asp:ListItem>Half Yearly</asp:ListItem>
                                                        <asp:ListItem>Yearly</asp:ListItem>
                                                        <asp:ListItem>One Time</asp:ListItem>
                                                    </asp:DropDownList></td>
                                            </tr>
                                            <tr>
                                                <td class="mylabel1">
                                                    Medical/Non-Medical</td>
                                                <td>
                                                    <asp:DropDownList ID="drpMedical" runat="Server" Width="200px">
                                                        <asp:ListItem>Medical</asp:ListItem>
                                                        <asp:ListItem>Non-Medical</asp:ListItem>
                                                    </asp:DropDownList></td>
                                            </tr>
                                            <tr>
                                                <td class="mylabel1">
                                                    Cheque Number</td>
                                                <td>
                                                    <asp:TextBox ID="txtChequeNumber" runat="Server" Width="195px"></asp:TextBox></td>
                                            </tr>
                                            <tr>
                                                <td class="mylabel1">
                                                    Cheque Date</td>
                                                <td>
                                                    <dxe:ASPxDateEdit ID="ASPxChequeDate" runat="server" 
                                                        EditFormat="Custom" EditFormatString="dd MMMM yyyy hh:mm tt" UseMaskBehavior="True"
                                                        Width="200px">
                                                        <ButtonStyle Width="13px">
                                                        </ButtonStyle>
                                                    </dxe:ASPxDateEdit>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="mylabel1">
                                                    Cheque Amount</td>
                                                <td>
                                                    <asp:TextBox ID="txtChequeAmount" runat="Server" Width="196px"></asp:TextBox>[Rs.]</td>
                                            </tr>
                                            <tr>
                                                <td class="mylabel1">
                                                    Bank</td>
                                                <td>
                                                    <asp:DropDownList ID="drpBanks" runat="Server" Width="199px" /></td>
                                            </tr>
                                            <tr>
                                                <td class="mylabel1">
                                                    Subsequent Premium Paying Method</td>
                                                <td>
                                                    <asp:DropDownList ID="drpSubsequentMethod" runat="Server" Width="198px">
                                                        <asp:ListItem>Cheque</asp:ListItem>
                                                        <asp:ListItem>ECS</asp:ListItem>
                                                    </asp:DropDownList></td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td colspan="2" style="border-left: solid 1px black;vertical-align :top">
                                        <table>
                                            <tr>
                                                <td class="mylabel1">
                                                    Sales Stage</td>
                                                <td>
                                                    <asp:DropDownList ID="drpSalesStage" runat="server" Width="191px" /></td>
                                            </tr>
                                            <tr>
                                                <td id="Login">
                                                    <asp:Label ID="lbllogindate" runat="Server" CssClass="mylabel1" Text="Login Date"></asp:Label></td>
                                                <td id="Login1">
                                                    <dxe:ASPxDateEdit ID="ASPxLoginDate" runat="server" 
                                                        EditFormat="Custom" EditFormatString="dd MMMM yyyy hh:mm tt" UseMaskBehavior="True"
                                                        Width="189px">
                                                        <ButtonStyle Width="13px">
                                                        </ButtonStyle>
                                                    </dxe:ASPxDateEdit>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="mylabel1">
                                                    Next Activity Type</td>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:RadioButton ID="rdrCall" runat="Server" GroupName="rdr" Text="Phone FollowUp"
                                                                    onclick="javascript:funChangeNext(this);" />
                                                            </td>
                                                            <td style="padding-left :12px">
                                                                <asp:RadioButton ID="rdrVisit" runat="Server" GroupName="rdr" Text="Visit" Checked="true" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td id="NVisit" class="mylabel1">
                                                    <asp:Label ID="lblNextVisitDate" runat="Server" Text="Next Visit Date"></asp:Label></td>
                                                <td id="NVisit1">
                                                    <dxe:ASPxDateEdit ID="ASPxNextVisitDate" ClientInstanceName ="NDate" runat="server" 
                                                        EditFormat="Custom" EditFormatString="dd MMMM yyyy hh:mm tt" UseMaskBehavior="True"
                                                        Width="192px">
                                                         <ClientSideEvents DateChanged="function(s,e){CheckDate();}" />
                                                        <ButtonStyle Width="13px">
                                                        </ButtonStyle>
                                                    </dxe:ASPxDateEdit>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td id="TdVisitPlace" class="mylabel1">
                                                    Next Visit Place
                                                </td>
                                                <td id="TdVisitPlace1">
                                                    <asp:DropDownList ID="DrpNextVisitPlace" runat="server" Width="191px">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblNextVisitPurpose" runat="Server" Text="Next Visit Purpose" class="mylabel1"></asp:Label></td>
                                                <td>
                                                    <asp:DropDownList ID="drpNextVisitPurpose" runat="Server" Width="191px">
                                                        <asp:ListItem Value="Product Presentation" Text="Product Presentation"></asp:ListItem>
                                                        <asp:ListItem Value="Document Collection" Text="Document Collection"></asp:ListItem>
                                                        <asp:ListItem Value="Cheque Collection" Text="Cheque Collection"></asp:ListItem>
                                                        <asp:ListItem Value="Medical Reports" Text="Medical Reports"></asp:ListItem>
                                                    </asp:DropDownList></td>
                                            </tr>
                                            <tr>
                                            </tr>
                                            <tr>
                                                <td colspan="2" style="border: 1px solid white;">
                                                    <table>
                                                        <tr>
                                                            <td colspan="2" align="Center">
                                                                <strong>Checklist</strong></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="mylabel1">
                                                                Product Application form</td>
                                                            <td>
                                                                <asp:DropDownList ID="drpProductApplicationForm" runat="Server" Width="149px">
                                                                    <asp:ListItem>Pending</asp:ListItem>
                                                                    <asp:ListItem>Recevied</asp:ListItem>
                                                                    <asp:ListItem>Not Applicable</asp:ListItem>
                                                                </asp:DropDownList></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="mylabel1">
                                                                Photo ID Proof</td>
                                                            <td>
                                                                <asp:DropDownList ID="drpPhotoIdProof" runat="Server" Width="149px">
                                                                    <asp:ListItem>Pending</asp:ListItem>
                                                                    <asp:ListItem>Recevied</asp:ListItem>
                                                                    <asp:ListItem>Not Applicable</asp:ListItem>
                                                                </asp:DropDownList></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="mylabel1">
                                                                Address Proof</td>
                                                            <td>
                                                                <asp:DropDownList ID="drpAddressProof" runat="Server" Width="149px">
                                                                    <asp:ListItem>Pending</asp:ListItem>
                                                                    <asp:ListItem>Recevied</asp:ListItem>
                                                                    <asp:ListItem>Not Applicable</asp:ListItem>
                                                                </asp:DropDownList></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="mylabel1">
                                                                Age Proof</td>
                                                            <td>
                                                                <asp:DropDownList ID="drpAgeProof" runat="Server" Width="149px">
                                                                    <asp:ListItem>Pending</asp:ListItem>
                                                                    <asp:ListItem>Recevied</asp:ListItem>
                                                                    <asp:ListItem>Not Applicable</asp:ListItem>
                                                                </asp:DropDownList></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="mylabel1">
                                                                Signature Proof</td>
                                                            <td>
                                                                <asp:DropDownList ID="drpSignatureProof" runat="Server" Width="149px">
                                                                    <asp:ListItem>Pending</asp:ListItem>
                                                                    <asp:ListItem>Recevied</asp:ListItem>
                                                                    <asp:ListItem>Not Applicable</asp:ListItem>
                                                                </asp:DropDownList></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="mylabel1">
                                                                KYC Document</td>
                                                            <td>
                                                                <asp:DropDownList ID="drpKYCDocument" runat="Server" Width="149px">
                                                                    <asp:ListItem>Pending</asp:ListItem>
                                                                    <asp:ListItem>Recevied</asp:ListItem>
                                                                    <asp:ListItem>Not Applicable</asp:ListItem>
                                                                </asp:DropDownList></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="mylabel1">
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="drpTripartiteAgreement" runat="Server" Visible="False" Width="149px">
                                                                    <asp:ListItem>Pending</asp:ListItem>
                                                                    <asp:ListItem>Recevied</asp:ListItem>
                                                                    <asp:ListItem>Not Applicable</asp:ListItem>
                                                                </asp:DropDownList></td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="drpPOAAgreement" runat="Server" Visible="False" Width="149px">
                                                                    <asp:ListItem>Pending</asp:ListItem>
                                                                    <asp:ListItem>Recevied</asp:ListItem>
                                                                    <asp:ListItem>Not Applicable</asp:ListItem>
                                                                </asp:DropDownList></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="mylabel1">
                                                                Medical Reports</td>
                                                            <td>
                                                                <asp:DropDownList ID="drpMedicalReports" runat="Server" Width="149px">
                                                                    <asp:ListItem>Pending</asp:ListItem>
                                                                    <asp:ListItem>Recevied</asp:ListItem>
                                                                    <asp:ListItem>Not Applicable</asp:ListItem>
                                                                </asp:DropDownList></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                               
                            </asp:Panel>
                        </table>
                    </td>
                </tr>
                 <tr>
                                    <td colspan="4" align="center">
                                        <asp:Button ID="btnSave" runat="server" Enabled="false" Text="Save" CssClass="btnUpdate"
                                            OnClick="btnSave_Click" />
                                        <asp:Button ID="btnCancel" runat="Server" Enabled="False" Text="Discard" CssClass="btnUpdate"
                                            OnClick="btnCancel_Click" />
                                    </td>
                                </tr>
            </table>
   
       </asp:Content>