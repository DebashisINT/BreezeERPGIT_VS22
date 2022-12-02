<%@ Page Title="Account Head" Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/Erp.Master"
    Inherits="ERP.OMS.Management.Master.management_master_frm_OpeningBalance" CodeBehind="frm_OpeningBalance.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        //function aaa(obj, evt, uid) { 
        //    evt = (evt) ? evt : event;
        //    var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode : ((evt.which) ? evt.which : 0)); 
        //    if ((charCode != 9) && (charCode != 8)) { 
        //        var CurrentTextBox_id = uid.id;
        //        var CurrentTextBox_Value = uid.id.value;  
        //        var txtF_Char = CurrentTextBox_id.charAt(0); 
        //        if (txtF_Char == 'L') {
        //            var RespectiveTextBox_id = CurrentTextBox_id.replace(/\L/, "R"); 
        //        } 
        //        else { 
        //            var RespectiveTextBox_id = CurrentTextBox_id.replace(/\R/, "L"); 
        //            var dd = document.getElementById(RespectiveTextBox_id); 
        //        } 
        //        var respValue = document.getElementById(RespectiveTextBox_id + '_Raw').value;
        //        var blankvalue = 'Rs. 0.20';
        //        if (respValue != 'Rs. 0.00') { 
        //            at1.SetValue('000000.00'); 
        //            Dtxt2.SetValue('11.00');
        //            var rr = 'D' + 'txt2'; 
        //        }
        //    }
        //}

        function fn_SetDROpeningAmt(s, e) {
            var cramt = cCRopening.GetText();
            var dramt = cDRopening.GetText(); 
            if (dramt != 'Rs. 0.00' && dramt != 'Rs.0.00')
            {
                if (cramt != 'Rs. 0.00' && cramt != 'Rs.0.00')
                { 
                    cCRopening.SetText('');
                }
            } 
        }

        function fn_SetCROpeningAmt(s, e) {
            var cramt = cCRopening.GetText();
            var dramt = cDRopening.GetText(); 
            if (cramt != 'Rs. 0.00' && cramt!='Rs.0.00') {
                if (dramt != 'Rs. 0.00' && dramt != 'Rs.0.00') { 
                    cDRopening.SetText('');
                }
            }
        }

        

        function fn_GetBranchOpeningAmt(s, e) {
            var branchid = s.GetValue();
             
            $.ajax({
                type: "POST",
                url: 'frm_OpeningBalance.aspx/GetBranchOpeningAmt',
                data: "{BranchId:\"" + branchid + "\"}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                     
                    var list = msg.d;
                    if (list.length > 0) {
                        
                        //for (var i = 0; i < list.length; i++) {

                            var dramt = '';
                            var cramt = '';
                            dramt = list[0].split('|')[0];
                            cramt = list[0].split('|')[1];
                           
                            cDRopening.SetText(dramt);
                            cCRopening.SetText(cramt);
                           
                            //lstMainAccountItems.push('<option value="' +
                            //id + '">' + name
                            //+ '</option>');
                        //}

                        //$(lBox).append(lstMainAccountItems.join(''));
                        //ListMainAccountBind();
                        //$('#lstMainAccount').trigger("chosen:updated");
                        //$('#lstMainAccount').prop('disabled', false).trigger("chosen:updated");

                    }
                    else {
                        cDRopening.SetText('');
                        cCRopening.SetText('');

                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    jAlert(textStatus);
                }
            });

        }

    </script>

   

    <script type="text/javascript">
        function OnCloseButtonClick(s, e) {
            window.location.href = 'MainAccountHead.aspx'
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Opening Balance <asp:Label runat="server" ID="lblAccountName" Text=""></asp:Label> </h3>
        </div>
        <div class="crossBtn"><a href="MainAccountHead.aspx"><i class="fa fa-times"></i></a></div>
    </div>
    <div class="form_main" style="border:1px solid #ccc;padding:5px  15px;">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div>
                    <asp:HiddenField ID="hdnStatus" runat="server" />
                    <table cellpadding="1" cellspacing="1">
                        <tr>
                            <td>
                                <table cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" width="100px;">Company :
                                        </td>
                                        <td>
                                            <asp:Label ID="lblCompanyName" runat="server" Width="350px"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table cellpadding="1" cellspacing="1">
                                    <tr>
                                        <%--<td class="gridcellleft" width="100px;">Segement :
                                        </td>--%>
                                        <td>
                                            <asp:Label ID="lblSegmentName" runat="server" Width="350px" Visible="false"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" width="100px;">Branch :
                                        </td>
                                        <td>
                                            <dxe:ASPxComboBox ID="cmbBranch" ClientInstanceName="cmbBranch" runat="server" DataSourceID="dsBranch"  Width="250px"
                                                ValueType="System.String" AutoPostBack="false" ValueField="BANKBRANCH_ID" TextField="BANKBRANCH_NAME"
                                                EnableIncrementalFiltering="true" EnableSynchronization="False" OnSelectedIndexChanged="cmbBranch_SelectedIndexChanged">
                                                <ClientSideEvents SelectedIndexChanged="function(s,e){fn_GetBranchOpeningAmt(s,e)}" />
                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right">
                                                    <RequiredField IsRequired="true" ErrorText="Mandatory" />
                                                </ValidationSettings>
                                            </dxe:ASPxComboBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" width="100px;">Opening DR :
                                        </td>
                                        <td>
                                            <dxe:ASPxTextBox ID="DRopening" runat="server"   ClientInstanceName="cDRopening" Width="250px"> 
                                                <MaskSettings Mask="<Rs.|*Rs. ><0..99999999999g>.<00..99>" IncludeLiterals="DecimalSymbol"
                                                    ErrorText="None" />
                                                <ClientSideEvents  LostFocus="function(s,e){ fn_SetDROpeningAmt(s,e);}" />
                                            </dxe:ASPxTextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" width="100px;">Opening CR :
                                        </td>
                                        <td>
                                            <dxe:ASPxTextBox ID="CRopening" runat="server" Width="250px" ClientInstanceName="cCRopening">
                                                <%--<MaskSettings Mask="<0..999999999999g>.<0..99g>" />--%>
                                                <MaskSettings Mask="<Rs.|*Rs. ><0..99999999999g>.<00..99>" IncludeLiterals="DecimalSymbol"
                                                    ErrorText="None" />
                                                <ClientSideEvents  LostFocus="function(s,e){ fn_SetCROpeningAmt(s,e);}" /> 
                                            </dxe:ASPxTextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 156px;">
                                <table>
                                    <tr>
                                        <td style="padding-left:100px;">
                                            <dxe:ASPxButton ID="Button1" runat="server" AutoPostBack="false" CssClass="btn btn-primary" Text="Save" OnClick="Button1_Click"
                                                VerticalAlign="Bottom">
                                            </dxe:ASPxButton>
                                        </td>
                                        <td>
                                            <dxe:ASPxButton ID="ASPxButton1" runat="server" CssClass="btn btn-danger" Text="Cancel" VerticalAlign="Bottom">
                                                <ClientSideEvents Click="OnCloseButtonClick" />
                                            </dxe:ASPxButton>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <asp:SqlDataSource ID="dsBranch" runat="server" ConflictDetection="CompareAllValues"
                        SelectCommand=""></asp:SqlDataSource>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

