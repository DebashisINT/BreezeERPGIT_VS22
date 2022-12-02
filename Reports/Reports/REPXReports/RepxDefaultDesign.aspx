<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true"
     CodeBehind="RepxReportMain.aspx.cs" Inherits="Reports.Reports.REPXReports.RepxReportMain" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <script type="text/javascript">

         
         function ddReportName_SelectedIndexChanged(s, e) {
             debugger;
             var reportname = s.GetValue();
             if (reportname.split('~')[1] == 'D') {
                 if (document.getElementById('btnLoadDesign'))
                    cbtnLoadDesign.SetEnabled(false);
             }            
             else {
                 if (document.getElementById('btnLoadDesign'))
                    cbtnLoadDesign.SetEnabled(true);
             }
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
        <h3>Report Parameter</h3>
    </div>
</div>
<div class="form_main clearfix">
 
    <tr>
        <td>
            <asp:Panel ID="Panel2" CssClass="row" runat="server" Width="100%">
                <div class="col-md-8" style="padding-left:0;">
                    <div class="col-md-3" style="padding-left:0">
                        <label>Select Design</label>
                        <dxe:aspxcombobox ID="ddReportName" runat="server" SelectedIndex="0" ValueType="System.String" ClientInstanceName="cddReportName" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True">
                         <ClientSideEvents SelectedIndexChanged="ddReportName_SelectedIndexChanged"  />
                        </dxe:aspxcombobox> 
                    </div>
                    <div class="clear"></div>
                    <div class="col-md-12" style="padding-top:15px">
                        <dxe:aspxbutton ID="btnSave" ClientInstanceName="cbtnSave" runat="server" ToolTip="Click on save the design" AutoPostBack="false" Text="Save" CssClass="btn btn-success">
                        </dxe:aspxbutton>     
                    </div>
                                                      
                </div>
            </asp:Panel>
        </td>
    </tr>    
   
    <br />
    <br />
    
</div>
</asp:Content>
