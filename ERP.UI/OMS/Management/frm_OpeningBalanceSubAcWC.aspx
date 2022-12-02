<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/PopUp.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.management_frm_OpeningBalanceSubAcWC" CodeBehind="frm_OpeningBalanceSubAcWC.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe000001" %>--%>
    <script type="text/javascript">
        function OnCloseButtonClick(s, e)
        {
            e.processOnServer = false;
            var parentWindow = window.parent;
            parent.window.popup.Hide();

        }

        function fn_SetDROpeningAmt(s, e) {
            var cramt = cCRopening.GetText();
            var dramt = cDRopening.GetText();
            if (dramt != 'Rs. 0.00' && dramt != 'Rs.0.00') {
                if (cramt != 'Rs. 0.00' && cramt != 'Rs.0.00') {
                    cCRopening.SetText('');
                }
            }
        }

        function fn_SetCROpeningAmt(s, e) {
            var cramt = cCRopening.GetText();
            var dramt = cDRopening.GetText();
            if (cramt != 'Rs. 0.00' && cramt != 'Rs.0.00') {
                if (dramt != 'Rs. 0.00' && dramt != 'Rs.0.00') {
                    cDRopening.SetText('');
                }
            }
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
                    //alert(dd);
                    //alert(dd.);
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
    <div>
        <asp:HiddenField ID="hdnStatus" runat="server" />
        <asp:HiddenField ID="hdnSubAcCode" runat="server" />
        <table class="TableMain100" id="maintransation">
            <tr>
                <td class="Ecoheadtxt" style="width: 118px">Company :
                </td>
                <td style="width: 173px" >
                     <asp:Label ID="lblCompanyName" runat="server"  ></asp:Label>
                    <%--<dxe:ASPxComboBox ID="cmbCompany" runat="server" DataSourceID="dsCompany" ValueType="System.String"
                        ValueField="COMPANYID" TextField="COMPANYNAME" EnableIncrementalFiltering="True">
                        <ClientSideEvents SelectedIndexChanged="function(s, e) { clientinstSegment.PerformCallback(s.GetValue());}" />
                    </dxe:ASPxComboBox>--%>
                </td>
                 
            </tr>
           <%-- .............................Code Commented and Added by Sam on 05122016.to avoid this section. ..................................... --%>


           <%-- <tr>
                <td class="Ecoheadtxt" style="width: 118px">Segemnt :
                </td>
                <td style="width: 173px">
                    <dxe:ASPxComboBox ID="cmbSegment" runat="server" DataSourceID="SelectSegment" ValueType="System.String"
                        ValueField="SEGMENTID" TextField="EXCHANGENAME" EnableIncrementalFiltering="true"
                        ClientInstanceName="clientinstSegment">
                    </dxe:ASPxComboBox>
                </td>
                <td></td>
            </tr>--%>

      <%--.............................Code Above Commented and Added by Sam on 05122016 to avoid this section...................................... --%>

            <tr>
                <td class="Ecoheadtxt" style="width: 118px">Branch :
                </td>
                <td  style="text-align: left">
                    <dxe:ASPxComboBox ID="cmbBranch" runat="server" DataSourceID="dsBranch" ValueType="System.String"
                        ValueField="BANKBRANCH_ID" TextField="BANKBRANCH_NAME" EnableIncrementalFiltering="true">
                    </dxe:ASPxComboBox>
                </td>
            </tr>
            <tr>
                <td class="Ecoheadtxt" style="width: 118px">Opening DR :
                </td>
                <td style="width: 173px">
                    <dxe:ASPxTextBox ID="DRopening" runat="server" Width="170px"  ClientInstanceName="cDRopening">
                        <MaskSettings Mask="<Rs. |*Rs. ><0..999999999g>.<00..99>" IncludeLiterals="DecimalSymbol"
                            ErrorText="None" />
                        <ClientSideEvents  TextChanged="function(s,e){ fn_SetDROpeningAmt(s,e);}" />
                    </dxe:ASPxTextBox>
                </td>
                 
            </tr>
            <tr>
                <td class="Ecoheadtxt" style="width: 118px; height: 26px;">Opening CR :
                </td>
                <td style="width: 173px; height: 26px;">
                    <dxe:ASPxTextBox ID="CRopening" runat="server" Width="170px" ClientInstanceName="cCRopening">
                        <MaskSettings Mask="<Rs. |*Rs. ><0..999999999g>.<00..99>" IncludeLiterals="DecimalSymbol"
                            ErrorText="None" />
                        <ClientSideEvents  TextChanged="function(s,e){ fn_SetCROpeningAmt(s,e);}" /> 
                    </dxe:ASPxTextBox>
                </td>
                
            </tr>

            <tr>
                <td valign="top" style="width: 118px; text-align: right;"></td>
                <td style="width: 173px; text-align: right;">
                    <table>
                        <tr>
                            <td>
                                <dxe:ASPxButton ID="Button1" runat="server" AutoPostBack="false" Text="Save" OnClick="Button1_Click" CssClass="btn btn-primary"
                                    VerticalAlign="Bottom">
                                </dxe:ASPxButton>
                            </td>
                            <td>
                                <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Cancel" CssClass="btn btn-danger"
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
        <asp:SqlDataSource ID="dsBranch" runat="server"  ConflictDetection="CompareAllValues"
            SelectCommand=""></asp:SqlDataSource>
        <%--<asp:SqlDataSource ID="dsCompany" runat="server" 
            SelectCommand="SELECT COMP.CMP_INTERNALID AS COMPANYID , COMP.CMP_NAME AS COMPANYNAME  FROM TBL_MASTER_COMPANY AS COMP"></asp:SqlDataSource>
        <asp:SqlDataSource ID="SelectSegment" runat="server"  ConflictDetection="CompareAllValues"
            SelectCommand="">--%>
            <%--<SelectParameters>
                    <asp:Parameter Name="COMPANYID" Type="string" />
                </SelectParameters>
        </asp:SqlDataSource>--%>
    </div>
</asp:Content>


