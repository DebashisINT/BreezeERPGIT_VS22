<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/PopUp.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.management_frm_OpeningBalanceSubAc" CodeBehind="frm_OpeningBalanceSubAc.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe000001" %>--%>
    <link href="../CSS/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function OnCloseButtonClick(s, e)
        {
            e.processOnServer = false;
            var parentWindow = window.parent;
            parent.window.popup.Hide();
        }
    </script>

    <script type="text/javascript">
        function aaa(obj,evt,uid)
        {


            evt = (evt) ? evt : event;
            var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :((evt.which) ? evt.which : 0));
            if((charCode != 9)&& (charCode != 8))
            {      
                var CurrentTextBox_Value=uid.id.value;
                var CurrentTextBox_id = uid.id;

     
                var txtF_Char=CurrentTextBox_id.charAt(0);
                //            alert(txtF_Char);
  

                //This txtF_Char if start Here....
                if(txtF_Char=='L')
                {
                    var RespectiveTextBox_id=CurrentTextBox_id.replace(/\L/,"R");
                    alert(RespectiveTextBox_id);
                }

                else
                {
        
                    var RespectiveTextBox_id=CurrentTextBox_id.replace(/\R/,"L");
                    alert(RespectiveTextBox_id);
                    var dd=document.getElementById(RespectiveTextBox_id);
                    alert(dd);
                    alert(dd.);
                }
        
                var respValue=document.getElementById(RespectiveTextBox_id + '_Raw').value;
                var blankvalue='Rs. 0.20';
                if(respValue!='Rs. 0.00')
                {
                    alert(at1.GetValue());
                    at1.SetValue('000000.00');
                    alert(Dtxt2.GetValue());
                    Dtxt2.SetValue('11.00');
                    var rr='D'+'txt2';
                    alert(rr);
       
                }    
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <asp:HiddenField ID="hdnStatus" runat="server" />
                <table border="10" cellpadding="1" cellspacing="1">
                    <tr>
                        <td>
                            <table border="10" cellpadding="1" cellspacing="1">
                                <tr>
                                    <td class="gridcellleft" bgcolor="#b7ceec">Company :
                                    </td>
                                    <td>
                                        <asp:Label ID="lblCompanyName" runat="server" Width="300px"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table border="10" cellpadding="1" cellspacing="1">
                                <tr>
                                    <td class="gridcellleft" bgcolor="#b7ceec">Segemnt :
                                    </td>
                                    <td>
                                        <asp:Label ID="lblSegmentName" runat="server" Width="300px"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table border="10" cellpadding="1" cellspacing="1">
                                <tr>
                                    <td class="gridcellleft" bgcolor="#b7ceec">Branch :
                                    </td>
                                    <td>
                                        <dxe:ASPxComboBox ID="cmbBranch" ClientInstanceName="cmbBranch" runat="server" DataSourceID="dsBranch"
                                            ValueType="System.String" AutoPostBack="true" ValueField="BANKBRANCH_ID" TextField="BANKBRANCH_NAME"
                                            EnableIncrementalFiltering="true" EnableSynchronization="False" OnSelectedIndexChanged="cmbBranch_SelectedIndexChanged">
                                        </dxe:ASPxComboBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table border="10" cellpadding="1" cellspacing="1">
                                <tr>
                                    <td class="gridcellleft" bgcolor="#b7ceec">Opening DR :
                                    </td>
                                    <td>
                                        <dxe:ASPxTextBox ID="DRopening" runat="server" Width="170px">
                                            <MaskSettings Mask="<Rs.|*Rs. ><0..999999999g>.<00..99>" IncludeLiterals="DecimalSymbol"
                                                ErrorText="None" />
                                        </dxe:ASPxTextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table border="10" cellpadding="1" cellspacing="1">
                                <tr>
                                    <td class="gridcellleft" bgcolor="#b7ceec">Opening CR :
                                    </td>
                                    <td>
                                        <dxe:ASPxTextBox ID="CRopening" runat="server" Width="170px">
                                            <MaskSettings Mask="<Rs. |*Rs. ><0..999999999g>.<00..99>" IncludeLiterals="DecimalSymbol"
                                                ErrorText="None" />
                                        </dxe:ASPxTextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>

                    <tr>

                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <dxe:ASPxButton ID="Button1" runat="server" AutoPostBack="false" Text="Save" OnClick="Button1_Click"
                                            VerticalAlign="Bottom">
                                        </dxe:ASPxButton>
                                    </td>
                                    <td>
                                        <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Cancel"
                                            VerticalAlign="Bottom">
                                            <ClientSideEvents Click="OnCloseButtonClick" />
                                        </dxe:ASPxButton>
                                    </td>
                                </tr>
                            </table>

                        </td>
                        <td></td>
                    </tr>
                </table>
                <asp:SqlDataSource ID="dsBranch" runat="server" 
                    SelectCommand=""></asp:SqlDataSource>

            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
