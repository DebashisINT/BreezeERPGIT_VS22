<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.management_frmSalesMutualFund1" CodeBehind="frmSalesMutualFund1.aspx.cs" %>

<%--<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe000001" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../CSS/style.css" rel="stylesheet" type="text/css" />

    <script language="javascript" type="text/javascript">
        function callfunction(chkObj, txtObj) {
            var c = document.getElementById(chkObj);
            var a = document.getElementById(txtObj);
            alert(c.checked)
            if (c.checked == true) {
                a.disabled = true;
            }
            else {
                a.disabled = false;
            }
        }
        function frmOpenNewWindow1(location, v_height, v_weight) {
            var y = (screen.availHeight - v_height) / 2;
            var x = (screen.availWidth - v_weight) / 2;
            window.open(location, "Search_Conformation_Box", "height=" + v_height + ",width=" + v_weight + ",top=" + y + ",left=" + x + ",location=no,directories=no,menubar=no,toolbar=no,status=yes,scrollbars=no,resizable=no,dependent=no");
        }        

        function CallForms(val) {

            document.getElementById("drpBanks").style.display = 'none';
            document.getElementById("drpSubsequent").style.display = 'none';
            document.getElementById("drpMedicalReports").style.display = 'none';
            document.getElementById("drpPOAAgreement").style.display = 'none';
            document.getElementById("drpTripartiteAgreement").style.display = 'none';
            document.getElementById("drpKYCDocument").style.display = 'none';
            document.getElementById("drpSignatureProof").style.display = 'none';
            document.getElementById("drpAgeProof").style.display = 'none';
            document.getElementById("drpAddressProof").style.display = 'none';
            document.getElementById("drpPhotoIdProof").style.display = 'none';
            document.getElementById("drpProductApplicationForm").style.display = 'none';
            document.getElementById("drpNextVisitPurpose").style.display = 'none';
            document.getElementById("DrpNextVisitPlace").style.display = 'none';
            document.getElementById("drpSalesStage").style.display = 'none';
            document.getElementById("drpInvestmentType").style.display = 'none';

            //       document.getElementById("DrpVisitPlace").style.display ='none';
            //       document.getElementById("drpVisitPurpose").style.display ='none';  

            parent.CallForms(val);


        }
        function ClosingDHTML() {

            document.getElementById("drpBanks").style.display = 'inline';
            document.getElementById("drpSubsequent").style.display = 'inline';
            document.getElementById("drpMedicalReports").style.display = 'inline';
            document.getElementById("drpPOAAgreement").style.display = 'inline';
            document.getElementById("drpTripartiteAgreement").style.display = 'inline';
            document.getElementById("drpKYCDocument").style.display = 'inline';
            document.getElementById("drpSignatureProof").style.display = 'inline';
            document.getElementById("drpAgeProof").style.display = 'inline';
            document.getElementById("drpAddressProof").style.display = 'inline';
            document.getElementById("drpPhotoIdProof").style.display = 'inline';
            document.getElementById("drpProductApplicationForm").style.display = 'inline';
            document.getElementById("drpNextVisitPurpose").style.display = 'inline';
            document.getElementById("DrpNextVisitPlace").style.display = 'inline';
            document.getElementById("drpSalesStage").style.display = 'inline';
            document.getElementById("drpInvestmentType").style.display = 'inline';
            //        document.getElementById("DrpVisitPlace").style.display ='inline';
            //        document.getElementById("drpVisitPurpose").style.display ='inline';  
        }

        function ActionAfterSave() {

            parent.AfterSave();
            //height();
        }
        function funChangeNext(obj) {

            var o = document.getElementById("lblNextVisitDate")
            var s = document.getElementById("lblNextVisitPurpose")
            if (obj.id == "rdrCall") {
                o.innerText = "Next Call Date"
                s.innerText = "Next Call Purpose"
                document.getElementById("TdVisitPlace").style.display = 'none';
                document.getElementById("TdVisitPlace1").style.display = 'none';
            }
            else {
                o.innerText = "Next Visit Date"
                s.innerText = "Next Visit Purpose"
                document.getElementById("TdVisitPlace").style.display = 'inline';
                document.getElementById("TdVisitPlace1").style.display = 'inline';
            }
        }
        function setvisibleforcall() {
            document.getElementById("TdVisitPlace").style.display = 'none';
            document.getElementById("TdVisitPlace1").style.display = 'none';
        }
        function setvisibleforvisit() {
            document.getElementById("TdVisitPlace").style.display = 'inline';
            document.getElementById("TdVisitPlace1").style.display = 'inline';
        }

        function functionSalesStage(obj1) {
            var obj = document.getElementById(obj1)
            var s = document.getElementById('lbllogindate')
            if (obj.value == 2) {
                s.innerText = 'Login Date';
                document.getElementById("Login").style.display = 'inline';
                document.getElementById("Login1").style.display = 'inline';
                document.getElementById("rdrCall").disabled = true;
                document.getElementById("rdrVisit").disabled = true;
                document.getElementById("NVisit").style.display = 'none';
                document.getElementById("NVisit1").style.display = 'none';
                document.getElementById("drpNextVisitPurpose").disabled = true;
                document.getElementById("DrpNextVisitPlace").disabled = true;
            }
            else {
                if (obj.value == 3) {
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
                else {
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
        function CheckDate() {

            var bdate = NDate.GetDate();
            var tdate = new Date();

            var date1 = tdate.getMonth() + 1 + '/' + tdate.getDate() + '/' + tdate.getFullYear();
            var date2 = bdate.getMonth() + 1 + '/' + bdate.getDate() + '/' + bdate.getFullYear();

            var new_date1 = new Date(date1);
            var new_date2 = new Date(date2);
            if (new_date1 > new_date2) {
                alert('Date can not be less than Current Date');
            }

        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="TableMain100" id="showData" runat="Server">
        <tr>
            <td style="border: 2px; width: 100%" valign="top" id="ShowCallInformation" runat="Server"
                visible="False" colspan="2">
                <table class="TableMaun100">
                    <tr>
                        <td style="width: 2%">
                            <asp:Label ID="Label1" runat="server" Text="Alloated By :" Width="65px" CssClass="mylabel"></asp:Label>
                        </td>
                        <td style="width: 20%">
                            <asp:Label ID="txtAlloatedBy" runat="Server"></asp:Label>
                        </td>
                        <td style="width: 2%">
                            <asp:Label ID="Label3" runat="server" CssClass="mylabel" Text="Allotted On :" Width="65px"></asp:Label></td>
                        <td style="width: 10%">
                            <asp:Label ID="txtDateOfAllottment" runat="Server" Width="108px"></asp:Label>
                        </td>
                        <td style="width: 10%">
                            <asp:Label ID="Label5" runat="server" CssClass="mylabel" Text="Priority :" Width="65px"></asp:Label></td>
                        <td style="width: 10%">
                            <asp:Label ID="txtPriority" runat="Server" Width="73px"></asp:Label></td>
                        <td style="width: 2%">
                            <asp:Label ID="Label2" runat="server" CssClass="mylabel" Text="Start By :" Width="65px"></asp:Label></td>
                        <td>
                            <asp:Label ID="txtSeheduleStart" runat="server"></asp:Label>
                            <asp:Label ID="txtTotalNumberofCalls" runat="Server" Visible="False" Width="81px"></asp:Label></td>
                    </tr>
                    <tr>
                        <td style="width: 2%;">
                            <asp:Label ID="Label4" runat="server" CssClass="mylabel" Text="End By :" Width="65px"></asp:Label></td>
                        <td>
                            <asp:Label ID="txtSeheduleEnd" runat="server" Width="130px"></asp:Label></td>
                        <td style="width: 2%;">
                            <asp:Label ID="Label6" runat="server" CssClass="mylabel" Text="Started On: " Width="65px"></asp:Label></td>
                        <td style="width: 10%">
                            <asp:Label ID="txtAcutalStart" runat="Server"></asp:Label></td>
                        <td>
                            <asp:Label ID="Label7" runat="server" CssClass="mylabel" Text="Instruction :" Width="64px"></asp:Label></td>
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
                        <td align="left">
                            <asp:Button ID="btnEdit" runat="Server" Text="Update Visit" CssClass="btnUpdate"
                                OnClick="btnEdit_Click" Height="18px" Width="82px" />
                            <asp:Button ID="btnPhoneFollowup" runat="Server" Text="Update Phone FollowUp" CssClass="btnUpdate"
                                OnClick="btnPhoneFollowup_Click" Height="18px" Width="141px" />
                        </td>
                        <td class="lt">
                            <asp:HyperLink ID="hpaddphone" runat="server" CssClass="decoration" NavigateUrl="javascript:void(0)"
                                onclick="CallForms('Address')" Width="63px">Address/Phone</asp:HyperLink>
                            &nbsp;
                                <asp:HyperLink ID="hpContact" runat="server" CssClass="decoration" NavigateUrl="javascript:void(0)"
                                    onclick="CallForms('Contact')" Width="41px">Contact</asp:HyperLink>
                            &nbsp;
                                <asp:HyperLink ID="hpFinance" runat="server" CssClass="decoration" NavigateUrl="javascript:void(0)"
                                    onclick="CallForms('Bank')" Width="63px">Bank/Finance</asp:HyperLink>
                            &nbsp;
                                <asp:HyperLink ID="hpReg" runat="server" CssClass="decoration" NavigateUrl="javascript:void(0)"
                                    onclick="CallForms('Registration')" Width="60px">Registration</asp:HyperLink>
                            &nbsp;&nbsp;<asp:HyperLink ID="hpDocument" runat="server" CssClass="decoration" NavigateUrl="javascript:void(0)"
                                onclick="CallForms('Document')" Width="50px">Document</asp:HyperLink>&nbsp;
                                <asp:HyperLink ID="hpHistory" runat="server" CssClass="decoration" NavigateUrl="javascript:void(0)"
                                    onclick="CallForms('History')" Width="45px">History</asp:HyperLink>&nbsp;
                                <asp:HyperLink ID="HyperLink1" runat="server" CssClass="decoration" NavigateUrl="javascript:void(0)"
                                    onclick="CallForms('Expences')" Width="58px">Expences</asp:HyperLink>&nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table class="TableMain99" style="border: solid 1px black">
                    <asp:Panel ID="pnl" runat="Server" Enabled="False">
                        <tr>
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label8" runat="server" CssClass="mylabel1" Text="Name" Width="65px"></asp:Label></td>
                                        <td style="width: 5%">
                                            <asp:Label ID="txtName" runat="Server" Font-Bold="true"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td valign="top" rowspan="2">
                                            <asp:Label ID="Label9" runat="server" CssClass="mylabel1" Text="Remarks/Note" Width="65px"></asp:Label></td>
                                        <td valign="top" rowspan="2">
                                            <asp:TextBox ID="txtNote" runat="Server" TextMode="MultiLine" Height="75px" Width="443px"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                    </tr>
                                    <tr>
                                        <td id="lblVisitDate" runat="server" class="mylabel1">
                                            <asp:Label ID="Label10" runat="server" CssClass="mylabel1" Text="Visit Date" Width="65px"></asp:Label>
                                        </td>
                                        <td>
                                            <dxe:ASPxDateEdit ID="txtvisitdate" runat="server" EditFormat="Custom" EditFormatString="dd MMMM yyyy hh:mm tt"
                                                UseMaskBehavior="True" Width="148px">
                                                <ButtonStyle Width="13px">
                                                </ButtonStyle>
                                            </dxe:ASPxDateEdit>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td id="TdPlace" runat="server">&nbsp;<asp:Label ID="Label11" runat="server" CssClass="mylabel1" Text="Visit Place"
                                            Width="65px"></asp:Label></td>
                                        <td id="TdPlace1" runat="server">
                                            <asp:DropDownList ID="DrpVisitPlace" runat="server" Width="148px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblVisitPurpose" runat="Server" Text="Visit Purpose" CssClass="mylabel1"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="drpVisitPurpose" runat="Server">
                                                <asp:ListItem Value="Product Presentation" Text="Product Presentation"></asp:ListItem>
                                                <asp:ListItem Value="Document Collection" Text="Document Collection"></asp:ListItem>
                                                <asp:ListItem Value="Cheque Collection" Text="Cheque Collection"></asp:ListItem>
                                                <asp:ListItem Value="Medical Reports" Text="Medical Reports"></asp:ListItem>
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label14" runat="server" CssClass="mylabel1" Text="AMC" Width="65px"></asp:Label></td>
                                        <td>
                                            <asp:Label ID="txtAMC" runat="Server" Font-Bold="true"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label13" runat="server" CssClass="mylabel1" Text="MF Scheme" Width="65px"></asp:Label></td>
                                        <td>
                                            <asp:Label ID="txtMFScheme" runat="Server" Font-Bold="true"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label15" runat="server" CssClass="mylabel1" Text="Investment Type"
                                                Width="85px"></asp:Label></td>
                                        <td>
                                            <asp:DropDownList ID="drpInvestmentType" runat="Server" Width="154px">
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label16" runat="server" CssClass="mylabel1" Text="Application No"
                                                Width="80px"></asp:Label></td>
                                        <td>
                                            <asp:TextBox ID="txtApplicationNo" runat="Server"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label17" runat="server" CssClass="mylabel1" Text="Folio No" Width="65px"></asp:Label></td>
                                        <td>
                                            <asp:TextBox ID="txtFolioNo" runat="Server"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label18" runat="server" CssClass="mylabel1" Text="Amount" Width="65px"></asp:Label></td>
                                        <td>
                                            <asp:TextBox ID="txtAmount" runat="Server"></asp:TextBox>[Rs.]</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label19" runat="server" CssClass="mylabel1" Text="Cheque Number" Width="80px"></asp:Label></td>
                                        <td align="left">
                                            <asp:TextBox ID="txtChequeNumber" runat="Server"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label20" runat="server" CssClass="mylabel1" Text="Cheque Date" Width="65px"></asp:Label></td>
                                        <td align="left">
                                            <dxe:ASPxDateEdit ID="txtchqdate" runat="server" EditFormat="Custom" EditFormatString="dd MMMM yyyy hh:mm tt"
                                                UseMaskBehavior="True" Width="159px">
                                                <ButtonStyle Width="13px">
                                                </ButtonStyle>
                                            </dxe:ASPxDateEdit>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label21" runat="server" CssClass="mylabel1" Text="Bank" Width="65px"></asp:Label></td>
                                        <td align="left">
                                            <asp:DropDownList ID="drpBanks" runat="Server" AppendDataBoundItems="True" Width="159px">
                                                <asp:ListItem Value="0">None</asp:ListItem>
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 1%">
                                            <asp:Label ID="Label22" runat="server" CssClass="mylabel1" Text="SubSequent Payment Method"
                                                Width="156px"></asp:Label></td>
                                        <td align="left">
                                            <asp:DropDownList ID="drpSubsequent" runat="Server" Width="159px">
                                                <asp:ListItem>Cheque</asp:ListItem>
                                                <asp:ListItem>ECS</asp:ListItem>
                                            </asp:DropDownList></td>
                                    </tr>
                                </table>
                            </td>
                            <td colspan="2" style="border-left: solid 1px black" valign="top">
                                <table>
                                    <tr>
                                        <td style="width: 3%;">
                                            <asp:Label ID="Label38" runat="server" CssClass="mylabel1" Text="Sales Stage" Width="65px"></asp:Label></td>
                                        <td>
                                            <asp:DropDownList ID="drpSalesStage" runat="server" Width="185px">
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label36" runat="server" CssClass="mylabel1" Text="Next Activity Type"
                                                Width="114px"></asp:Label></td>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="rdrCall" runat="Server" GroupName="rdr" Text="Phone FollowUp"
                                                            onclick="javascript:funChangeNext(this);" />
                                                    </td>
                                                    <td class="lt" style="padding-left: 12px">
                                                        <asp:RadioButton ID="rdrVisit" runat="Server" GroupName="rdr" Text="Visit" Checked="true"
                                                            Width="96px" onclick="javascript:funChangeNext(this);" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td id="Login">
                                            <asp:Label ID="lbllogindate" runat="Server" Text="Login Date" CssClass="mylabel1"></asp:Label>
                                        </td>
                                        <td id="Login1">
                                            <dxe:ASPxDateEdit ID="txtlogindate" runat="server" EditFormat="Custom" EditFormatString="dd MMMM yyyy hh:mm tt"
                                                UseMaskBehavior="True" Width="184px">
                                                <ButtonStyle Width="13px">
                                                </ButtonStyle>
                                            </dxe:ASPxDateEdit>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td id="NVisit" valign="top">
                                            <asp:Label ID="lblNextVisitDate" runat="Server" Text="Next Visit Date" CssClass="mylabel1"></asp:Label>
                                        </td>
                                        <td id="NVisit1">
                                            <dxe:ASPxDateEdit ID="txtnextvisitdate" runat="server" ClientInstanceName="NDate"
                                                EditFormat="Custom" UseMaskBehavior="True" Width="184px">
                                                <ClientSideEvents DateChanged="function(s,e){CheckDate();}" />
                                                <ButtonStyle Width="13px">
                                                </ButtonStyle>
                                            </dxe:ASPxDateEdit>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td id="TdVisitPlace">
                                            <asp:Label ID="Label34" runat="server" CssClass="mylabel1" Text="Next Visit Place"
                                                Width="93px"></asp:Label></td>
                                        <td id="TdVisitPlace1">
                                            <asp:DropDownList ID="DrpNextVisitPlace" runat="server" Width="183px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblNextVisitPurpose" runat="Server" Text="Next Visit Purpose" CssClass="mylabel1"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="drpNextVisitPurpose" runat="Server" Width="181px">
                                                <asp:ListItem Value="Product Presentation" Text="Product Presentation"></asp:ListItem>
                                                <asp:ListItem Value="Document Collection" Text="Document Collection"></asp:ListItem>
                                                <asp:ListItem Value="Cheque Collection" Text="Cheque Collection"></asp:ListItem>
                                                <asp:ListItem Value="Medical Reports" Text="Medical Reports"></asp:ListItem>
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <table style="border: solid 1px white">
                                                <tr>
                                                    <td colspan="2" align="center">
                                                        <strong>Checklist</strong></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label31" runat="server" CssClass="mylabel1" Text="Product Application form"
                                                            Width="133px"></asp:Label></td>
                                                    <td>
                                                        <asp:DropDownList ID="drpProductApplicationForm" runat="Server" Width="155px">
                                                            <asp:ListItem>Pending</asp:ListItem>
                                                            <asp:ListItem>Recevied</asp:ListItem>
                                                            <asp:ListItem>Not Applicable</asp:ListItem>
                                                        </asp:DropDownList></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label30" runat="server" CssClass="mylabel1" Text="Photo ID Proof"
                                                            Width="132px"></asp:Label></td>
                                                    <td>
                                                        <asp:DropDownList ID="drpPhotoIdProof" runat="Server" Width="155px">
                                                            <asp:ListItem>Pending</asp:ListItem>
                                                            <asp:ListItem>Recevied</asp:ListItem>
                                                            <asp:ListItem>Not Applicable</asp:ListItem>
                                                        </asp:DropDownList></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label29" runat="server" CssClass="mylabel1" Text="Address Proof" Width="129px"></asp:Label></td>
                                                    <td>
                                                        <asp:DropDownList ID="drpAddressProof" runat="Server" Width="155px">
                                                            <asp:ListItem>Pending</asp:ListItem>
                                                            <asp:ListItem>Recevied</asp:ListItem>
                                                            <asp:ListItem>Not Applicable</asp:ListItem>
                                                        </asp:DropDownList></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label28" runat="server" CssClass="mylabel1" Text="Age Proof" Width="65px"></asp:Label></td>
                                                    <td>
                                                        <asp:DropDownList ID="drpAgeProof" runat="Server" Width="155px">
                                                            <asp:ListItem>Pending</asp:ListItem>
                                                            <asp:ListItem>Recevied</asp:ListItem>
                                                            <asp:ListItem>Not Applicable</asp:ListItem>
                                                        </asp:DropDownList></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label27" runat="server" CssClass="mylabel1" Text="Signature Proof"
                                                            Width="132px"></asp:Label></td>
                                                    <td>
                                                        <asp:DropDownList ID="drpSignatureProof" runat="Server" Width="155px">
                                                            <asp:ListItem>Pending</asp:ListItem>
                                                            <asp:ListItem>Recevied</asp:ListItem>
                                                            <asp:ListItem>Not Applicable</asp:ListItem>
                                                        </asp:DropDownList></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label26" runat="server" CssClass="mylabel1" Text="KYC Document" Width="131px"></asp:Label></td>
                                                    <td>
                                                        <asp:DropDownList ID="drpKYCDocument" runat="Server" Width="155px">
                                                            <asp:ListItem>Pending</asp:ListItem>
                                                            <asp:ListItem>Recevied</asp:ListItem>
                                                            <asp:ListItem>Not Applicable</asp:ListItem>
                                                        </asp:DropDownList></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label25" runat="server" CssClass="mylabel1" Text="Tripartite Agreement"
                                                            Width="128px"></asp:Label></td>
                                                    <td>
                                                        <asp:DropDownList ID="drpTripartiteAgreement" runat="Server" Width="155px">
                                                            <asp:ListItem>Pending</asp:ListItem>
                                                            <asp:ListItem>Recevied</asp:ListItem>
                                                            <asp:ListItem>Not Applicable</asp:ListItem>
                                                        </asp:DropDownList></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label24" runat="server" CssClass="mylabel1" Text="POA Agreement" Width="129px"></asp:Label></td>
                                                    <td>
                                                        <asp:DropDownList ID="drpPOAAgreement" runat="Server" Width="155px">
                                                            <asp:ListItem>Pending</asp:ListItem>
                                                            <asp:ListItem>Recevied</asp:ListItem>
                                                            <asp:ListItem>Not Applicable</asp:ListItem>
                                                        </asp:DropDownList></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label23" runat="server" CssClass="mylabel1" Text="Medical Reports"
                                                            Width="128px"></asp:Label></td>
                                                    <td>
                                                        <asp:DropDownList ID="drpMedicalReports" runat="Server" Width="155px">
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
            <td align="center">
                <asp:Button ID="btnSave" runat="server" Enabled="false" Text="Save" OnClick="btnSave_Click"
                    CssClass="btnUpdate" />
                <asp:Button ID="btnCancel" runat="Server" Enabled="False" Text="Discard" OnClick="btnCancel_Click"
                    CssClass="btnUpdate" />
            </td>
        </tr>
    </table>
</asp:Content>
