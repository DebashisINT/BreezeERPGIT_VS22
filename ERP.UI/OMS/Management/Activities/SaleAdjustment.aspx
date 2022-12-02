<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="SaleAdjustment.aspx.cs" Inherits="ERP.OMS.Management.Activities.SaleAdjustment" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="JS/SaleAdjustment.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

     <div class="panel-title clearfix">
         <h3 class="pull-left">
            <label>Adjustment of Customer</label>
        </h3> 
    </div>

     <div class="form_main">
          <div class="col-md-3">  
               <div class="col-md-3" >
                <dxe:ASPxLabel ID="lbl_NumberingScheme" runat="server" Text="Numbering Scheme">
                </dxe:ASPxLabel>
                <asp:DropDownList ID="ddl_numberingScheme" runat="server" Width="100%" TabIndex="2">
                </asp:DropDownList>
              </div>
               
              <div class="col-md-3"> 
                                                        <dxe:ASPxTextBox ID="txt_PLQuoteNo" runat="server" ClientInstanceName="ctxt_PLQuoteNo" TabIndex="2" Width="100%">
                                                            <ClientSideEvents TextChanged="function(s, e) {UniqueCodeCheck();}" />
                                                        </dxe:ASPxTextBox>

                                                        <span id="MandatorysQuoteno" style="display: none" class="validclass">
                                                            <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                                        </span>
                                                       
                                                    </div>
          </div>
      </div>
</asp:Content>
