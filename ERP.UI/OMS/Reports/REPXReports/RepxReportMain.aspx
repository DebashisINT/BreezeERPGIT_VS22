<%@ Page Title="Report Designer" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="RepxReportMain.aspx.cs" Inherits="ERP.OMS.Reports.REPXReports.RepxReportMain" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <script type="text/javascript">
         function CopyNew() {
             cpopUp_newReport.Show();
         }
         function hidePopup() {
             cpopUp_newReport.Hide();
         }
         function ddReportName_SelectedIndexChanged(s,e)
         {
             var reportname = s.GetValue();
             if (reportname.split('~')[1] == 'D') {
                 cbtnLoadDesign.SetEnabled(false);
             }
             else {
                 cbtnLoadDesign.SetEnabled(true);
             }
         }
         function ShowPopUp(s, e){ 
             cDocumentsPopup.Show();
             cgriddocuments.PerformCallback('BindDocumentsDetails');
         }

         function PerformCallToGridBind() {
             //cgriddocuments.PerformCallback('BindDocumentsDetails');
             //cbtnOK.PerformCallback('BindQuotationGridOnSelection');
             //$('#hdnPageStatus').val('Quoteupdate');
             //cDocumentsPopup.Hide();
             cgriddocuments.PerformCallback('BindDocumentsGridOnSelection');
             cDocumentsPopup.Hide();
             return false;
         }
         function ChangeState(value) {
             cgriddocuments.PerformCallback('SelectAndDeSelectDocuments' + '~' + value);
         }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <dxe:ASPxPopupControl ID="popUp_newReport" runat="server" ClientInstanceName="cpopUp_newReport"
        Width="400px" HeaderText="New Report" PopupHorizontalAlign="WindowCenter"
        BackColor="white" Height="100px" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
        <contentcollection>
                    <dxe:PopupControlContentControl runat="server">
                        <div class="Top clearfix">
                            <table>
                                <tr>
                                    <td>
                                        <span style="padding-right:5px">File Name</span>
                                    </td>
                                    <td>
                                         <asp:TextBox ID="txtFileName" runat="server" MaxLength="50"  ></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                       <asp:Button ID="btnNewFileSave" runat="server" Text="Save" OnClick="btnNewFileSave_Click"/>
                                         <asp:Button ID="btnNewFileCancel" runat="server" Text="Cancel" OnClientClick="hidePopup(); return false;"/>
                                    </td>                                   
                            </table>                           
                        </div>

                    </dxe:PopupControlContentControl>
                </contentcollection>
        <headerstyle backcolor="LightGray" forecolor="Black" />
    </dxe:ASPxPopupControl>
    <%--<asp:DropDownList ID="ddReportName" runat="server" Height="16px" Width="155px">
    </asp:DropDownList>--%>        
    <tr>
        <td>
            <asp:Panel ID="Panel2" runat="server" Width="100%">
                <div class="col-md-6" style="padding-left:0;">
                    <table>
                        <tr>
                            <td style="padding-right:15px">
                                    <label>Start Date:</label>
                                <dxe:ASPxDateEdit ID="TxtStartDate" runat="server" UseMaskBehavior="True" EditFormat="Custom" EditFormatString="dd MMMM yyyy">
                                    <ButtonStyle Width="13px">
                                    </ButtonStyle>
                                    <DropDownButton Text="From Date">
                                    </DropDownButton>
                                </dxe:ASPxDateEdit>
                                        
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TxtStartDate"
                        Display="Dynamic" EnableTheming="True" ErrorMessage="Date required!"></asp:RequiredFieldValidator></td>
                            <td style="padding-right:15px">
                                <label>End Date:</label>
                                <dxe:ASPxDateEdit ID="TxtEndDate" runat="server" UseMaskBehavior="True" EditFormat="Custom"  EditFormatString="dd MMMM yyyy">
                                        <ButtonStyle Width="13px">
                                        </ButtonStyle>
                                        <DropDownButton Text="To Date">
                                        </DropDownButton>
                                    </dxe:ASPxDateEdit>                                        
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="TxtEndDate"
                            Display="Dynamic" EnableTheming="True" ErrorMessage="Date required!"></asp:RequiredFieldValidator></td>                            
                            <td style="padding-top:15px;">
                                <dxe:ASPxButton ID="btnDocNo" runat="server" Text="Select"  CssClass="btn btn-success">
                                    <ClientSideEvents Click="function(s, e){ 
                                                                   ShowPopUp();
                                                                }"></ClientSideEvents>
                                </dxe:ASPxButton>
                            </td>                            
                        </tr>
                    </table>                                    
                    <div>                                        
                    </div>                                     
                </div>
            </asp:Panel>
        </td>
    </tr>    
    <%--<br />--%>
    <%--<br />--%>        
    <div class="PopUpArea">
        <dxe:ASPxPopupControl ID="ASPxDocumentsPopup" runat="server" ClientInstanceName="cDocumentsPopup"
            Width="900px" HeaderText="Select Documents" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <div style="padding: 7px 0;">
                        <input type="button" value="Select All Documents" onclick="ChangeState('SelectAll')" class="btn btn-primary"></input>
                        <input type="button" value="De-select All Documents" onclick="ChangeState('UnSelectAll')" class="btn btn-primary"></input>
                        <input type="button" value="Revert" onclick="ChangeState('Revart')" class="btn btn-primary"></input>
                    </div>
                    <dxe:ASPxGridView runat="server" KeyFieldName="Quote_Id" ClientInstanceName="cgriddocuments" ID="grid_Documents"
                        Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" SettingsPager-Mode="ShowAllRecords" OnCustomCallback="cgridDocuments_CustomCallback"
                        Settings-ShowFooter="false" AutoGenerateColumns="False"                 
                        Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Visible">
                                                      
                        <SettingsBehavior AllowDragDrop="False" AllowSort="False"></SettingsBehavior>
                        <SettingsPager Visible="false"></SettingsPager>
                        <Columns>
                            <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="10" Caption=" " />
                            <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="SrlNo" Width="10" ReadOnly="true" Caption="Sl. No.">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="Quote_Id" Width="20" ReadOnly="true" Caption="Document No">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="Quote_Date" Width="15" ReadOnly="true" Caption="Document Date">
                            </dxe:GridViewDataTextColumn>
                            <%--<dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="gvColDocumentNo" ReadOnly="true" Caption="Document No" Width="0">--%>
                            <%--</dxe:GridViewDataTextColumn>--%>
                            <%--<dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="Product_Shortname" ReadOnly="true" Width="100" Caption="Product Name">--%>
                            <%--</dxe:GridViewDataTextColumn>--%>
                            <%--<dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="gvColDiscription" Width="200" ReadOnly="true" Caption="Product Description">--%>
                            <%--</dxe:GridViewDataTextColumn>--%>
                            <%--<dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="Quotation_No" ReadOnly="true" Caption="Quotation Id" Width="0">--%>
                            <%--</dxe:GridViewDataTextColumn>--%>
                            <%--<dxe:GridViewDataTextColumn Caption="Bal Quantity" FieldName="gvColQuantity" Width="70" VisibleIndex="6">--%>
                                <%--<PropertiesTextEdit>--%>                                                               
                                    <%--<MaskSettings Mask="<0..999999999999>.<0..99>" AllowMouseWheel="false" />--%>
                                <%--</PropertiesTextEdit>--%>
                            <%--</dxe:GridViewDataTextColumn>--%>

                        </Columns>
                                                     
                                                       
                    </dxe:ASPxGridView>
                    <div class="text-center">
                        <dxe:ASPxButton ID="btnOK" ClientInstanceName="cbtnOK" runat="server"  AutoPostBack="False" Text="OK" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior ="False">
                            <ClientSideEvents Click="function(s, e) {return PerformCallToGridBind();}" />
                        </dxe:ASPxButton>
                    </div>
                </dxe:PopupControlContentControl>
            </ContentCollection>
            </dxe:ASPxPopupControl>
    </div>
    <%--<br />--%>
    <%--<br />--%>
    <dxe:ASPxComboBox ID="ddReportName" runat="server" SelectedIndex="0" ValueType="System.String" Width="30%" EnableSynchronization="True" EnableIncrementalFiltering="True">
     <ClientSideEvents SelectedIndexChanged="ddReportName_SelectedIndexChanged"  />
    </dxe:ASPxComboBox>    
    <%--<asp:LinkButton ID="lnkBtnCopy" runat="server"   OnClientClick="CopyNew();return false;">Copy</asp:LinkButton>--%>
    <br />
    <br />
    <dxe:ASPxButton ID="btnNewDesign" ClientInstanceName="cbtnNewDesign" runat="server" AutoPostBack="false" Text="NewDesign">
     <ClientSideEvents Click="CopyNew"></ClientSideEvents>
    </dxe:ASPxButton>     
    <dxe:ASPxButton ID="btnLoadDesign" runat="server" AutoPostBack="false" Text="LoadDesign" OnClick="btnLoadDesign_Click" ClientInstanceName="cbtnLoadDesign"  /></dxe:ASPxButton>
    <dxe:ASPxButton ID="btnPreview" runat="server" AutoPostBack="false" Text="Preview" OnClick="btnPreview_Click"  /></dxe:ASPxButton>
    <asp:HiddenField ID ="RptName" runat="server" />
</asp:Content>