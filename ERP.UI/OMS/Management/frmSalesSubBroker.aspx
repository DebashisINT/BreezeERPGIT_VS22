<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.management_frmSalesSubBroker" Codebehind="frmSalesSubBroker.aspx.cs" %>

<%--<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe000001" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../CSS/style.css" rel="stylesheet" type="text/css" />

    <script language="javascript" type="text/javascript">
    function CallForms(val)
    {
       parent.CallForms(val);
 
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
    function height()
    {
             
        window.frameElement.height = document.body.scrollHeight;
        window.frameElement.Width = document.body.scrollWidth;
        parent.height();
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
        function visibility(obj)
        {
            
            if( document.getElementById("tdReg2").style.display=='inline')
            {
                document.getElementById("tdReg1").style.display='none';
                document.getElementById("tdReg2").style.display='none';
            }
            else
            {
                document.getElementById("tdReg1").style.display='inline';
                document.getElementById("tdReg2").style.display='inline';
            
            }
        
        }
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
     function ActionAfterSave()
    {
        
        parent.AfterSave();
    }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <table class="TableMain100" id="showData" runat="Server">
            <tr>
                <td style="border: 1px;" valign="top" id="ShowCallInformation" runat="Server" visible="False"
                    colspan="2">
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
                <td colspan="2">
                    <table class="TableMain100">
                        <tr>
                            <td class="lt">
                                <asp:Button ID="btnEdit" runat="Server" Text="Update Visit" CssClass="btnUpdate"
                                    OnClick="btnEdit_Click" Height="18px" Width="82px" Visible="false" />
                                <asp:Button ID="btnPhoneFollowup" runat="Server" Text="Update Phone FollowUp" CssClass="btnUpdate"
                                    OnClick="btnPhoneFollowup_Click" Height="18px" Width="141px" Visible="false" />
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
                <td colspan="1" style="border: solid 1px black; height: 167px;">
                    <table>
                        <tr>
                            <td valign="top" class="mylabel1" style="width: 3%">
                                Name:</td>
                            <td valign="Top" style="width: 12%;">
                                <asp:Label ID="txtName" runat="Server" Font-Bold="true"></asp:Label></td>
                        </tr>
                        <tr>
                            <td class="mylabel1">
                                Branch:</td>
                            <td style="width: 12%;">
                                <asp:Label ID="lblBranch" runat="Server" Font-Bold="True"></asp:Label></td>
                        </tr>
                        <tr>
                            <td class="mylabel1">
                                Legal Status:</td>
                            <td style="width: 12%">
                                <asp:Label ID="lblLegalStatus" runat="Server" Font-Bold="True"></asp:Label></td>
                        </tr>
                        <tr>
                            <td class="mylabel1" style="height: 16px">
                                Introducer:</td>
                            <td style="width: 12%; height: 16px;">
                                <asp:Label ID="lblIntroducer" runat="Server" Font-Bold="True"></asp:Label></td>
                        </tr>
                        <tr>
                            <td id="lblVisitDate" runat="server" class="mylabel1">
                                Visit Date:</td>
                            <td style="width: 12%;">
                                <dxe:ASPxDateEdit ID="ASPxvisitDate" runat="server" EditFormat="Custom" EditFormatString="dd MMMM yyyy hh:mm tt"
                                    UseMaskBehavior="True" Width="200px">
                                    <ButtonStyle Width="13px">
                                    </ButtonStyle>
                                </dxe:ASPxDateEdit>
                            </td>
                        </tr>
                        <tr>
                            <td id="TdPlace" runat="server" class="mylabel1">
                                Visit Place:</td>
                            <td id="TdPlace1" runat="server">
                                <asp:DropDownList ID="DrpVisitPlace" runat="server" Width="200px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 4%;">
                                <asp:Label ID="lblVisitPurpose" CssClass="mylabel1" runat="Server" Text="Visit Purpose:"></asp:Label></td>
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
                <td colspan="1" style="border: solid 1px black; padding-right: 10px; height: 167px;">
                    <table>
                        <tr>
                            <td class="mylabel1">
                                Sales Stage:</td>
                            <td>
                                <asp:DropDownList ID="drpSalesStage" runat="server" Width="200px">
                                </asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td class="mylabel1">
                                Next Activity Type:</td>
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
                                <asp:Label ID="lblNextVisitDate" CssClass="mylabel1" runat="Server" Text="Next Visit Date:"></asp:Label></td>
                            <td id="NVisit1">
                                <dxe:ASPxDateEdit ID="ASPxNextVisitDate" ClientInstanceName="NDate" runat="server"
                                    EditFormat="Custom" EditFormatString="dd MMMM yyyy hh:mm tt" UseMaskBehavior="True"
                                    Width="200px">
                                    <ClientSideEvents DateChanged="function(s,e){CheckDate();}" />
                                    <ButtonStyle Width="13px">
                                    </ButtonStyle>
                                </dxe:ASPxDateEdit>
                            </td>
                        </tr>
                        <tr>
                            <td id="TdVisitPlace" class="mylabel1" style="height: 24px">
                                Next Visit Place :</td>
                            <td id="TdVisitPlace1" style="height: 24px">
                                <asp:DropDownList ID="DrpNextVisitPlace" runat="server" Width="200px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblNextVisitPurpose" CssClass="mylabel1" runat="Server" Text="Next Visit Purpose:"></asp:Label></td>
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
                    Remarks/Note:</td>
                <td valign="top">
                    <asp:TextBox ID="txtNote" runat="Server" TextMode="MultiLine" Width="563px"></asp:TextBox></td>
            </tr>
            <tr>
                <td colspan="2" style="height: 188px">
                    <table style="border: solid 1px black; width: 100%">
                        <tr>
                            <td colspan="3">
                                <table>
                                    <tr>
                                        <td class="mylabel1">
                                            <asp:CheckBox ID="chReg" runat="server" Checked="true"></asp:CheckBox>
                                            <asp:Label ID="Label8" runat="server" Text="Registered"></asp:Label>
                                        </td>
                                        <td class="mylabel1" id="tdReg1" style="display: inline">
                                            <asp:Label ID="Label9" runat="server" Text="Name Of Existing Broker:"></asp:Label>
                                        </td>
                                        <td id="tdReg2" style="display: inline">
                                            <asp:TextBox ID="txtNameEBrok" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label10" runat="server" Text="Associated:" CssClass="mylabel1"></asp:Label></td>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:DropDownList ID="drpmonth" runat="server">
                                                <asp:ListItem Value="1" Text="Jan"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="Feb"></asp:ListItem>
                                                <asp:ListItem Value="3" Text="Mar"></asp:ListItem>
                                                <asp:ListItem Value="4" Text="Apr"></asp:ListItem>
                                                <asp:ListItem Value="5" Text="May"></asp:ListItem>
                                                <asp:ListItem Value="6" Text="Jun"></asp:ListItem>
                                                <asp:ListItem Value="7" Text="Jul"></asp:ListItem>
                                                <asp:ListItem Value="8" Text="Aug"></asp:ListItem>
                                                <asp:ListItem Value="9" Text="Sep"></asp:ListItem>
                                                <asp:ListItem Value="10" Text="Oct"></asp:ListItem>
                                                <asp:ListItem Value="11" Text="Nov"></asp:ListItem>
                                                <asp:ListItem Value="12" Text="Dec"></asp:ListItem>
                                            </asp:DropDownList></td>
                                        <td>
                                            <asp:TextBox ID="txtAsso" runat="server" Width="83px" MaxLength="4" onkeypress="return noNumbers(event)"></asp:TextBox></td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <asp:Label ID="Label11" runat="server" Text="Total Experience:" CssClass="mylabel1"></asp:Label></td>
                            <td>
                                <asp:TextBox ID="txtTotEx" runat="server" Width="60px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 7px">
                                <asp:Label ID="Label12" runat="server" Text="No Of Client:" CssClass="mylabel1"></asp:Label></td>
                            <td style="height: 7px">
                                <asp:TextBox ID="txtclient" runat="server" onkeypress="return noNumbers(event)" Width="87px"></asp:TextBox>
                            </td>
                            <td style="height: 7px">
                                <asp:Label ID="Label13" runat="server" Text="Turnover Per Day:" CssClass="mylabel1"></asp:Label></td>
                            <td style="height: 7px">
                                <asp:TextBox ID="txTurnOver" runat="server" onkeypress="return noNumbers(event)"
                                    Width="89px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label14" runat="server" Text="Sharing Ratio(%):" CssClass="mylabel1"></asp:Label></td>
                            <td>
                                <asp:TextBox ID="txtsRatio" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="Label15" runat="server" Text="Demat Sharing(%):" CssClass="mylabel1"></asp:Label></td>
                            <td>
                                <asp:TextBox ID="txtDSharing" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label16" runat="server" Text="Payment Terms:" CssClass="mylabel1"></asp:Label></td>
                            <td>
                                <asp:TextBox ID="txtPterms" runat="server" Height="63px" Width="284px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="Label17" runat="server" Text="Terms offered by Us:" CssClass="mylabel1"></asp:Label></td>
                            <td>
                                <asp:TextBox ID="txtTerms" runat="server" Height="64px" Width="206px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label18" runat="server" Text="Terms Demanded By clients:" CssClass="mylabel1"></asp:Label></td>
                            <td>
                                <asp:TextBox ID="txtclientterms" runat="server" Height="47px" Width="287px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="Label19" runat="server" Text="Feedback:" CssClass="mylabel1"></asp:Label></td>
                            <td>
                                <asp:TextBox ID="txtfeedback" runat="server" Height="43px" Width="210px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btnUpdate" OnClick="btnSave_Click" />
                </td>
            </tr>
        </table>
   </asp:Content>