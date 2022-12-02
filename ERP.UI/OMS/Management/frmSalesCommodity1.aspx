<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.management_frmSalesCommodity1" Codebehind="frmSalesCommodity1.aspx.cs" %>

<%--<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe000001" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../CSS/style.css" rel="stylesheet" type="text/css" />

    <script language="javascript" type="text/javascript">
        function callfunction(chkObj,txtObj)
        {   
            var c= document.getElementById(chkObj);
            var a= document.getElementById(txtObj);
           
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
    function CallForms(val)
    {
       document.getElementById("drpBanks").style.display ='none';
       document.getElementById("drpLastYearIT").style.display ='none';
       document.getElementById("drpAddressProof").style.display ='none';
       document.getElementById("drpBankAccountProof").style.display ='none';
       document.getElementById("drpPOAAgreement").style.display ='none';
       document.getElementById("drpKYCDocument").style.display ='none';
       document.getElementById("drpSignatureProof").style.display ='none';
       document.getElementById("drpAgeProof").style.display ='none';
       document.getElementById("drpAddressProof").style.display ='none';
       document.getElementById("drpPhotoIdProof").style.display ='none';
       document.getElementById("drpPhotoIdProof").style.display ='none';
       document.getElementById("drpNextVisitPurpose").style.display ='none';
       document.getElementById("DrpNextVisitPlace").style.display ='none'; 
       document.getElementById("drpSalesStage").style.display ='none'; 
       document.getElementById("DrpVisitPlace").style.display ='none'; 
       document.getElementById("drpVisitPurpose").style.display ='none'; 
       document.getElementById("drpTripartiteAgreement").style.display ='none'; 
       document.getElementById("drpBankVerificationLetter").style.display ='none'; 
       document.getElementById("drpBanksMargin").style.display ='none'; 
       document.getElementById("drpPhotoIdProofDocument").style.display ='none';
       document.getElementById("drpBankAccountProofDocument").style.display ='none';
       document.getElementById("drpAddressProofDocument").style.display ='none';
       parent.CallForms(val);
 
    }
    function ClosingDHTML()
    {
        document.getElementById("drpBanks").style.display ='inline';
        document.getElementById("drpLastYearIT").style.display ='inline';
        document.getElementById("drpAddressProof").style.display ='inline';
        document.getElementById("drpBankAccountProof").style.display ='inline';
        document.getElementById("drpPhotoIdProof").style.display ='inline';
        document.getElementById("drpKYCDocument").style.display ='inline';
        document.getElementById("drpSignatureProof").style.display ='inline';
        document.getElementById("drpAgeProof").style.display ='inline';
        document.getElementById("drpAddressProof").style.display ='inline';
        document.getElementById("drpPhotoIdProof").style.display ='inline';
        document.getElementById("drpPOAAgreement").style.display ='inline';
        document.getElementById("drpNextVisitPurpose").style.display ='inline';
        document.getElementById("DrpNextVisitPlace").style.display ='inline'; 
        document.getElementById("drpSalesStage").style.display ='inline'; 
        document.getElementById("DrpVisitPlace").style.display ='inline';
        document.getElementById("drpVisitPurpose").style.display ='inline';
        document.getElementById("drpTripartiteAgreement").style.display ='inline';
        document.getElementById("drpBankVerificationLetter").style.display ='inline'; 
        document.getElementById("drpBanksMargin").style.display ='inline'; 
        document.getElementById("drpPhotoIdProofDocument").style.display ='inline';
        document.getElementById("drpBankAccountProofDocument").style.display ='inline'; 
        document.getElementById("drpAddressProofDocument").style.display ='inline';  
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
            if (obj.value == 2)
            {
                document.getElementById("rdrCall").disabled = true;
                document.getElementById("rdrVisit").disabled= true;
                document.getElementById("NVisit").style.display = 'none';
                document.getElementById("NVisit1").style.display = 'none';
                document.getElementById("drpNextVisitPurpose").disabled = true;
                document.getElementById("DrpNextVisitPlace").disabled = true;
            }
            else
            {
                document.getElementById("rdrCall").disabled = false;
                document.getElementById("rdrVisit").disabled = false;
                document.getElementById("NVisit").style.display = 'inline';
                document.getElementById("NVisit1").style.display = 'inline';
                document.getElementById("drpNextVisitPurpose").disabled = false; 
                document.getElementById("DrpNextVisitPlace").disabled = false;  
               
            }
        }
        function saveAddressString(str)
        {
            var sel = document.getElementById('drpBanks');
            var sel1 = document.getElementById('drpBanksMargin');
            var i = 0;
            while (sel.options.length != 0) 
            { 
                sel.options[0] = null;  
                i = i + 1;
            }
            while (sel1.options.length != 0)
            {
                sel1.options[0] = null;  
                i = i + 1;
            }
            var txt; 
            var addOption;
            var st = str.split('||');
            addOption123(sel,"Select Bank",0)
            addOption123(sel1,"Select Bank",0)
            for (j = 0; j < st.length-1; j++)
            {
                var s = st[j].split("@@@@")
                addOption123(sel,s[0],s[1])
                addOption123(sel1,s[0],s[1])
            }
            //window.close();
        }
        function addOption123(selectbox,text,value)
        {
            var optn = document.createElement("OPTION");
            optn.text = text;
            optn.value = value;
            selectbox.options.add(optn);            
        }
    function height()
    {
             
        window.frameElement.height = document.body.scrollHeight;
        window.frameElement.Width = document.body.scrollWidth;
        parent.height();
    }
         
    function ActionAfterSave()
    {
        
        parent.AfterSave();
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

            <table class="TableMain100" id="showData" runat="Server">
                <tr>
                    <td style="border: 1px; width: 100%" valign="top" id="ShowCallInformation" runat="Server"
                        visible="False" colspan="2">
                        <table class="TableMain100">
                            <tr>
                                <td style="width: 1%">
                                    <asp:Label ID="Label1" CssClass="mylabel" runat="server" Text="Alloated By" Width="60px"></asp:Label></td>
                                <td style="width: 2%">
                                    <asp:Label ID="txtAlloatedBy" runat="Server" Width="167px"></asp:Label>
                                </td>
                                <td style="width: 1%">
                                    <asp:Label ID="Label3" runat="server" CssClass="mylabel" Text="Allotted On : " Width="58px"></asp:Label></td>
                                <td style="width: 6%">
                                    <asp:Label ID="txtDateOfAllottment" runat="Server" Width="111px"></asp:Label>
                                </td>
                                <td style="width: 1%">
                                    <asp:Label ID="Label5" runat="server" CssClass="mylabel" Text="Priority :" Width="60px"></asp:Label></td>
                                <td style="width: 1%">
                                    <asp:Label ID="txtPriority" runat="Server" Width="90px"></asp:Label>
                                </td>
                                <td style="width: 1%">
                                    <asp:Label ID="Label2" runat="server" CssClass="mylabel" Text="Start By :" Width="60px"></asp:Label></td>
                                <td>
                                    <asp:Label ID="txtSeheduleStart" runat="server"></asp:Label>
                                    <asp:Label ID="txtTotalNumberofCalls" runat="Server" Visible="False" Width="1px"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label4" runat="server" CssClass="mylabel" Text="End By : " Width="60px"></asp:Label></td>
                                <td>
                                    <asp:Label ID="txtSeheduleEnd" runat="server"></asp:Label></td>
                                <td style="width: 1%">
                                    <asp:Label ID="Label6" runat="server" CssClass="mylabel" Text="Started On:" Width="60px"></asp:Label></td>
                                <td>
                                    <asp:Label ID="txtAcutalStart" runat="Server"></asp:Label></td>
                                <td>
                                    <asp:Label ID="Label7" runat="server" CssClass="mylabel" Text="Instruction:" Width="60px"></asp:Label></td>
                                <td colspan="3">
                                    <asp:Label ID="lblShortNote" runat="Server" Width="100%"></asp:Label></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%;">
                        <table class="TableMain100">
                            <tr>
                                <td class="lt">
                                    <asp:Button ID="btnEdit" runat="Server" Text="Update Visit" CssClass="btnUpdate"
                                        OnClick="btnEdit_Click" Height="18px" Width="82px" />
                                    <asp:Button ID="btnPhoneFollowup" runat="Server" Text="Update Phone FollowUp" CssClass="btnUpdate"
                                        OnClick="btnPhoneFollowup_Click" Height="18px" Width="141px" />
                                </td>
                                <td class="lt">
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
                        <table class="TableMain100">
                            <asp:Panel ID="pnl" runat="Server" Enabled="False">
                                <tr>
                                    <td colspan="4" align="Center">
                                        <asp:Label ID="lblError" runat="Server" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="border: solid 1px black">
                                        <table>
                                            <tr>
                                                <td valign="top" class="mylabel1" style="width: 3%">
                                                    Name</td>
                                                <td valign="Top" style="width: 12%;">
                                                    <asp:Label ID="txtName" runat="Server" Font-Bold="true"></asp:Label></td>
                                            </tr>
                                            <tr>
                                                <td class="mylabel1">
                                                    Branch</td>
                                                <td style="width: 12%;">
                                                    <asp:Label ID="lblBranch" runat="Server" Font-Bold="True"></asp:Label></td>
                                            </tr>
                                            <tr>
                                                <td class="mylabel1">
                                                    Legal Status</td>
                                                <td style="width: 12%">
                                                    <asp:Label ID="lblLegalStatus" runat="Server" Font-Bold="True"></asp:Label></td>
                                            </tr>
                                            <tr>
                                                <td class="mylabel1">
                                                    Introducer</td>
                                                <td style="width: 12%">
                                                    <asp:Label ID="lblIntroducer" runat="Server" Font-Bold="True"></asp:Label></td>
                                            </tr>
                                            <tr>
                                                <td id="lblVisitDate" runat="server" class="mylabel1">
                                                    Visit Date
                                                </td>
                                                <td style="width: 12%;">
                                                    <dxe:ASPxDateEdit ID="ASPxvisitDate" runat="server" 
                                                        EditFormat="Custom" EditFormatString="dd MMMM yyyy hh:mm tt" UseMaskBehavior="True"
                                                        Width="200px">
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
                                                    <asp:DropDownList ID="DrpVisitPlace" runat="server" Width="200px">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 4%;">
                                                    <asp:Label ID="lblVisitPurpose" CssClass="mylabel1" runat="Server" Text="Visit Purpose"></asp:Label></td>
                                                <td style="width: 12%">
                                                    <asp:DropDownList ID="drpVisitPurpose" runat="Server" Width="200px">
                                                        <asp:ListItem Value="Product Presentation" Text="Product Presentation"></asp:ListItem>
                                                        <asp:ListItem Value="Document Collection" Text="Document Collection"></asp:ListItem>
                                                        <asp:ListItem Value="Cheque Collection" Text="Cheque Collection"></asp:ListItem>
                                                        <asp:ListItem Value="Medical Reports" Text="Medical Reports"></asp:ListItem>
                                                    </asp:DropDownList></td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td colspan="2" style="border: solid 1px black; padding-right: 10px">
                                        <table>
                                            <tr>
                                                <td class="mylabel1">
                                                    Sales Stage</td>
                                                <td>
                                                    <asp:DropDownList ID="drpSalesStage" runat="server" Width="200px">
                                                    </asp:DropDownList></td>
                                            </tr>
                                            <tr>
                                                <td class="mylabel1">
                                                    Next Activity Type</td>
                                                <td style="width: 435px">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:RadioButton ID="rdrCall" runat="Server" GroupName="rdr" Text="Phone FollowUp" />
                                                            </td>
                                                            <td>
                                                                <asp:RadioButton ID="rdrVisit" runat="Server" GroupName="rdr" Text="Visit" Checked="true"
                                                                    Width="53px" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td id="NVisit">
                                                    <asp:Label ID="lblNextVisitDate" CssClass="mylabel1" runat="Server" Text="Next Visit Date"></asp:Label></td>
                                                <td id="NVisit1">
                                                    <dxe:ASPxDateEdit ID="ASPxNextVisitDate" ClientInstanceName ="NDate" runat="server" 
                                                        EditFormat="Custom" EditFormatString="dd MMMM yyyy hh:mm tt" UseMaskBehavior="True"
                                                        Width="200px">
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
                                                    <asp:DropDownList ID="DrpNextVisitPlace" runat="server" Width="200px">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblNextVisitPurpose" CssClass="mylabel1" runat="Server" Text="Next Visit Purpose"></asp:Label></td>
                                                <td>
                                                    <asp:DropDownList ID="drpNextVisitPurpose" runat="Server" Width="200px">
                                                        <asp:ListItem Value="Product Presentation" Text="Product Presentation"></asp:ListItem>
                                                        <asp:ListItem Value="Document Collection" Text="Document Collection"></asp:ListItem>
                                                        <asp:ListItem Value="Cheque Collection" Text="Cheque Collection"></asp:ListItem>
                                                        <asp:ListItem Value="Medical Reports" Text="Medical Reports"></asp:ListItem>
                                                    </asp:DropDownList></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" class="mylabel1" colspan="1" style="width: 1%">
                                        Remarks/Note</td>
                                    <td colspan="4" valign="top">
                                        <asp:TextBox ID="txtNote" runat="Server" TextMode="MultiLine" Width="792px"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 50%;" colspan="4" align="left" valign="top">
                                        <table style="border: 1px solid black" width="100%">
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <b>Trading For</b></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="chkNSEEquity" runat="Server" /></td>
                                                <td class="mylabel1">
                                                    NSE-Equity</td>
                                                <td id="tdNSEEquity" runat="Server" visible="False">
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkNSEDerivatives" runat="Server" /></td>
                                                <td class="mylabel1">
                                                    NSE-Derivatives</td>
                                                <td id="tdNSEDerivatives" runat="Server" visible="False">
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkBSEEquity" runat="Server" /></td>
                                                <td class="mylabel1">
                                                    BSE-Equity</td>
                                                <td id="tdBSEEquity" runat="Server" visible="False">
                                                </td>
                                                <td style="width: 110px">
                                                    <asp:CheckBox ID="chkBSEDerivatives" runat="Server" /></td>
                                                <td class="mylabel1">
                                                    BSE-Derivatives</td>
                                                <td visible="False" id="tdBSEDerivatives" runat="Server">
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkNCDEX" runat="Server" /></td>
                                                <td class="mylabel1" style="width: 26px">
                                                    NCDEX</td>
                                                <td visible="False" id="tdNCDEX" runat="Server">
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkMCX" runat="Server" /></td>
                                                <td class="mylabel1">
                                                    MCX</td>
                                                <td visible="False" id="tdMCX" runat="Server">
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkCDSL" runat="Server" /></td>
                                                <td class="mylabel1">
                                                    CDSL</td>
                                                <td visible="False" id="tdCDSL" runat="Server">
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkNSDL" runat="Server" /></td>
                                                <td class="mylabel1">
                                                    NSDL</td>
                                                <td visible="False" id="tdNSDL" runat="Server">
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="border: 1px solid black;" colspan="4" align="left" valign="top">
                                        <table>
                                            <tr>
                                                <td colspan="2" align="Center">
                                                    <strong>Checklist</strong></td>
                                            </tr>
                                            <tr>
                                                <td style="width: 35%;" valign="top">
                                                    <table class="TableMain100">
                                                        <tr>
                                                            <td>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="drpProductApplicationForm" runat="Server" Visible="False">
                                                                    <asp:ListItem>Pending</asp:ListItem>
                                                                    <asp:ListItem>Recevied</asp:ListItem>
                                                                    <asp:ListItem>Not Applicable</asp:ListItem>
                                                                </asp:DropDownList></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="mylabel1">
                                                                Bank Account Proof</td>
                                                            <td>
                                                                <asp:DropDownList ID="drpBankAccountProof" runat="Server">
                                                                    <asp:ListItem>Pending</asp:ListItem>
                                                                    <asp:ListItem>Recevied</asp:ListItem>
                                                                    <asp:ListItem>Not Applicable</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <asp:DropDownList ID="drpBankAccountProofDocument" runat="Server" Width="100px">
                                                                    <asp:ListItem>Pass Card</asp:ListItem>
                                                                    <asp:ListItem>Cancelled Cheque</asp:ListItem>
                                                                    <asp:ListItem>Bank Statement</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="mylabel1">
                                                                Photo ID Proof</td>
                                                            <td>
                                                                <asp:DropDownList ID="drpPhotoIdProof" runat="Server">
                                                                    <asp:ListItem>Pending</asp:ListItem>
                                                                    <asp:ListItem>Recevied</asp:ListItem>
                                                                    <asp:ListItem>Not Applicable</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <asp:DropDownList ID="drpPhotoIdProofDocument" runat="Server" Width="100px">
                                                                    <asp:ListItem>Pan Card</asp:ListItem>
                                                                    <asp:ListItem>Voters Id</asp:ListItem>
                                                                    <asp:ListItem>Passport</asp:ListItem>
                                                                    <asp:ListItem>Driving License</asp:ListItem>
                                                                    <asp:ListItem>Employee Card</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="mylabel1">
                                                                Address Proof</td>
                                                            <td>
                                                                <asp:DropDownList ID="drpAddressProof" runat="Server">
                                                                    <asp:ListItem>Pending</asp:ListItem>
                                                                    <asp:ListItem>Recevied</asp:ListItem>
                                                                    <asp:ListItem>Not Applicable</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <asp:DropDownList ID="drpAddressProofDocument" runat="Server" Width="100px">
                                                                    <asp:ListItem>Electricity Bill</asp:ListItem>
                                                                    <asp:ListItem>Residence Telephone Bill</asp:ListItem>
                                                                    <asp:ListItem>Agreement for Sale</asp:ListItem>
                                                                    <asp:ListItem>Self Declartion By High Court</asp:ListItem>
                                                                    <asp:ListItem>Identity Card</asp:ListItem>
                                                                    <asp:ListItem>Ration Card</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="mylabel1">
                                                                Age Proof</td>
                                                            <td>
                                                                <asp:DropDownList ID="drpAgeProof" runat="Server">
                                                                    <asp:ListItem>Pending</asp:ListItem>
                                                                    <asp:ListItem>Recevied</asp:ListItem>
                                                                    <asp:ListItem>Not Applicable</asp:ListItem>
                                                                </asp:DropDownList></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="mylabel1">
                                                                Bank Verification Letter</td>
                                                            <td>
                                                                <asp:DropDownList ID="drpBankVerificationLetter" runat="Server">
                                                                    <asp:ListItem>Pending</asp:ListItem>
                                                                    <asp:ListItem>Recevied</asp:ListItem>
                                                                    <asp:ListItem>Not Applicable</asp:ListItem>
                                                                </asp:DropDownList></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td style="width: 30%;" valign="top">
                                                    <table class="TableMain100">
                                                        <tr>
                                                            <td style="width: 180px" class="mylabel1">
                                                                KYC Document</td>
                                                            <td>
                                                                <asp:DropDownList ID="drpKYCDocument" runat="Server">
                                                                    <asp:ListItem>Pending</asp:ListItem>
                                                                    <asp:ListItem>Recevied</asp:ListItem>
                                                                    <asp:ListItem>Not Applicable</asp:ListItem>
                                                                </asp:DropDownList></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="mylabel1">
                                                                Tripartite Agreement</td>
                                                            <td>
                                                                <asp:DropDownList ID="drpTripartiteAgreement" runat="Server">
                                                                    <asp:ListItem>Pending</asp:ListItem>
                                                                    <asp:ListItem>Recevied</asp:ListItem>
                                                                    <asp:ListItem>Not Applicable</asp:ListItem>
                                                                </asp:DropDownList></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="mylabel1">
                                                                POA Agreement</td>
                                                            <td>
                                                                <asp:DropDownList ID="drpPOAAgreement" runat="Server">
                                                                    <asp:ListItem>Pending</asp:ListItem>
                                                                    <asp:ListItem>Recevied</asp:ListItem>
                                                                    <asp:ListItem>Not Applicable</asp:ListItem>
                                                                </asp:DropDownList></td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="drpMedicalReports" runat="Server" Visible="False">
                                                                    <asp:ListItem>Pending</asp:ListItem>
                                                                    <asp:ListItem>Recevied</asp:ListItem>
                                                                    <asp:ListItem>Not Applicable</asp:ListItem>
                                                                </asp:DropDownList></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="mylabel1">
                                                                Signature Proof</td>
                                                            <td>
                                                                <asp:DropDownList ID="drpSignatureProof" runat="Server">
                                                                    <asp:ListItem>Pending</asp:ListItem>
                                                                    <asp:ListItem>Recevied</asp:ListItem>
                                                                    <asp:ListItem>Not Applicable</asp:ListItem>
                                                                </asp:DropDownList></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="mylabel1">
                                                                Last Year's IT Returns Or Form 16</td>
                                                            <td>
                                                                <asp:DropDownList ID="drpLastYearIT" runat="Server">
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
                                <tr>
                                    <td style="border: 1px solid black" colspan="4">
                                        <table>
                                            <tr>
                                                <td colspan="2" align="Center" style="width: 60%">
                                                    <b>Account Opening Charges</b></td>
                                                <td colspan="2" align="Center" style="width: 40%">
                                                    <b>Margin Deposit</b></td>
                                            </tr>
                                            <tr>
                                                <td class="mylabel1">
                                                    Cheque Number</td>
                                                <td>
                                                    <asp:TextBox ID="txtChequeNumber" runat="Server"></asp:TextBox></td>
                                                <td class="mylabel1">
                                                    Cheque Number</td>
                                                <td>
                                                    <asp:TextBox ID="txtChequeNumberMargin" runat="Server"></asp:TextBox></td>
                                            </tr>
                                            <tr>
                                                <td class="mylabel1">
                                                    Cheque Date</td>
                                                <td>
                                                    <%--<asp:TextBox ID="ASPxChqDate1" runat="server" Width="120px"></asp:TextBox>
                                                    <asp:Image ID="ImgChqDate1" runat="server" ImageUrl="~/images/calendar.jpg" />--%>
                                                    <dxe:ASPxDateEdit ID="ASPxChqDate1" runat="server" 
                                                        EditFormat="Custom" EditFormatString="dd MMMM yyyy hh:mm tt" UseMaskBehavior="True">
                                                        <ButtonStyle Width="13px">
                                                        </ButtonStyle>
                                                    </dxe:ASPxDateEdit>
                                                </td>
                                                <td class="mylabel1">
                                                    Cheque Date</td>
                                                <td>
                                                    <%--<asp:TextBox ID="ASPxChqDate2" runat="server" Width="120px"></asp:TextBox>
                                                    <asp:Image ID="ImgChqDate2" runat="server" ImageUrl="~/images/calendar.jpg" />--%>
                                                    <dxe:ASPxDateEdit ID="ASPxChqDate2" runat="server" 
                                                        EditFormat="Custom" EditFormatString="dd MMMM yyyy hh:mm tt" UseMaskBehavior="True">
                                                        <ButtonStyle Width="13px">
                                                        </ButtonStyle>
                                                    </dxe:ASPxDateEdit>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="mylabel1">
                                                    Cheque Amount</td>
                                                <td>
                                                    <asp:TextBox ID="txtChequeAmount" runat="Server" Text="200"></asp:TextBox>[Rs.]</td>
                                                <td class="mylabel1">
                                                    Cheque Amount</td>
                                                <td>
                                                    <asp:TextBox ID="txtChequeAmountMargin" runat="Server"></asp:TextBox>[Rs.]</td>
                                            </tr>
                                            <tr>
                                                <td class="mylabel1">
                                                    Bank</td>
                                                <td>
                                                    <asp:DropDownList ID="drpBanks" runat="Server" Width="154px" /></td>
                                                <td class="mylabel1">
                                                    Bank</td>
                                                <td>
                                                    <asp:DropDownList ID="drpBanksMargin" runat="Server" Width="155px" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </asp:Panel>
                        </table>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td colspan="2" align="Center">
                        <asp:Button ID="btnSave" runat="Server" Text="Save" Enabled="False" CssClass="btnUpdate"
                            OnClick="btnSave_Click" />
                        <asp:Button ID="btnDiscard" runat="Server" Text="Discard" Enabled="false" CssClass="btnUpdate"
                            OnClick="btnDiscard_Click" />
                    </td>
                </tr>
            </table>
      
   </asp:Content>