<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="RepxSetDefaultDesign.aspx.cs" Inherits="Reports.Reports.REPXReports.RepxSetDefaultDesign" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <script type="text/javascript">

         function ddModuleName_SelectedIndexChanged(s, e) {
             var ModuleName = s.GetValue();
             cddReportName.PerformCallback(s.GetValue());
         }

</script>
    <style>
        .btn-def {
            padding:3px;
            background: #076fa9;
            border: 1px solid #065683;
            color: #fff;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="panel-heading">
    <div class="panel-title">
        <h3>Set Default Design</h3>
    </div>
</div>
<div class="form_main clearfix">
        
    <tr>
        <td>
            <asp:Panel ID="Panel2" CssClass="row" runat="server" Width="100%">
                <div class="col-md-8" style="padding-left:0;">
                                                                       
                    <div class="col-md-3" style="padding-left:0">
                         <label>Select Module</label>
                        <dxe:aspxcombobox ID="ddModuleName" runat="server" SelectedIndex="0" ValueType="System.String" ClientInstanceName="cddModuleName" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" AutoPostBack="false" >
                             <ClientSideEvents SelectedIndexChanged="ddModuleName_SelectedIndexChanged"  />
                            <%--AutoPostBack="True" OnSelectedIndexChanged ="ddModuleName_SelectedIndexChanged" >--%>                            
                        
                        </dxe:aspxcombobox> 

                        <label>Select Design</label>
                        <dxe:aspxcombobox ID="ddReportName" runat="server" SelectedIndex="0" ValueType="System.String" ClientInstanceName="cddReportName" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" OnCallback="ddReportName_CustomCallback" AutoPostBack="false" >
                         <%--<ClientSideEvents SelectedIndexChanged="ddReportName_SelectedIndexChanged"  />--%>
                        </dxe:aspxcombobox> 
                    </div>
                    <div class="clear"></div>
                    <div class="col-md-12" style="padding-top:15px">
                        <dxe:aspxbutton ID="btnSave" runat="server" ToolTip="Click on Save the design" AutoPostBack="false" Text="Save" OnClick="btnSave_Click" CssClass="btn btn-primary"  /></dxe:ASPxButton>
                    </div>
                </div>
            </asp:Panel>
        </td>
    </tr>    
    
</div>
</asp:Content>
