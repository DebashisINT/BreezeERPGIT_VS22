<%@ Page Title="CTC Import" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.DailyTask.management_DailyTask_frm_ImportEmployeeCTC" CodeBehind="frm_ImportEmployeeCTC.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  

   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>CTC Import</h3>
        </div>
    </div>
    <div class="form_main">
        <asp:Panel ID="Panelmain" runat="server" Visible="true" >
                <table id="tbl_main" class="login" cellspacing="0" cellpadding="0">
                    <tbody>

                        <tr>
                            <td class="lt">
                                <table class="width100per" cellspacing="12" cellpadding="0">
                                    <tbody>
                                        <tr>
                                            <td colspan="2">This Routine Imports Employee CTC with Details.Upload only CSV File.
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" style="width: 278px" colspan="2">
                                                <asp:FileUpload ID="NCDEXSelectFile" runat="server" Width="452px"  />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" align="left">
                                                                <asp:Button ID="BtnSave" runat="server" Text="Import File" CssClass="btn btn-primary"
                                                                     OnClick="BtnSave_Click" />
                                                            </td>
                                            <td style="width: 200px">
                                                <asp:Label ID="lblMsgAccCode" Width="120px" ForeColor="Red" runat="server"></asp:Label></td>
                                            
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </asp:Panel>
    </div>
</asp:Content>
