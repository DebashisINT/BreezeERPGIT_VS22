<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true"
     CodeBehind="RepxReportMain.aspx.cs" Inherits="Reports.Reports.REPXReports.RepxReportMain" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <script type="text/javascript">
         var ReportModuleName = '';
         
         var loadFirstTime = true;

         function cbtnPreviewClick(s, e) {
             //LoadingPanel.Show();
             var ret = true;
             var Bankval = $("#hdnBankValue").val();
             var ModuleName = document.getElementById('HDReportModuleName').value;
             //if (Bankval == "" && ModuleName == "BRSSTATEMENT") {
             if (Bankval == "" && (ModuleName == "BRSSTATEMENT" || ModuleName == "BRSCONSSTATEMENT")) {
                 jAlert('Please select atleast one Bank for generate the report.');
                 ret = false;
             }


             else {
                 LoadingPanel.Show();
             }
             e.processOnServer = ret;
         }


         

         function AllControlInitilize() {
             if (loadFirstTime) {
                 ReportModuleName = document.getElementById('HDReportModuleName').value;
                 if (document.getElementById('btnLoadDesign'))
                     ddReportName_SelectedIndexChanged(cddReportName);
                 loadFirstTime = false;
             }
         }

         function CopyNew() {
             cpopUp_newReport.Show();
             $('#txtFileName').focus();
         }
         function hidePopup() {
             cpopUp_newReport.Hide();
         }
         function ddReportName_SelectedIndexChanged(s, e) {
             var reportname = s.GetValue();
             var SelectedReport = reportname.split('~')[0];
             if (reportname.split('~')[1] == 'D') {
                 if (document.getElementById('btnLoadDesign'))
                     cbtnLoadDesign.SetEnabled(false);
             }
             else {
                 if (document.getElementById('btnLoadDesign'))
                     cbtnLoadDesign.SetEnabled(true);
             }
             if (ReportModuleName == 'CashBook' && SelectedReport == 'Default-DayBook') {
                 cTxtStartDate.SetEnabled(false);
             }
             else {
                 cTxtStartDate.SetEnabled(true);
             }
         }
         function ShowDocumentPopUp(s, e) {
             cDocumentsPopup.Show();
             cgriddocuments.PerformCallback('BindDocumentsDetails');
         }

         function PerformCallToDocumentGridBind() {
             cgriddocuments.PerformCallback('BindDocumentsGridOnSelection');
             cDocumentsPopup.Hide();
             return false;
         }
         function ChangeStateforDocument(value) {
             cgriddocuments.PerformCallback('SelectAndDeSelectDocuments' + '~' + value);
         }

         function ShowBranchPopUp(s, e) {
             cBranchPopup.Show();
             cgridbranch.PerformCallback('BindDocumentsDetails');
         }

         function PerformCallToBranchGridBind() {
             cgridbranch.PerformCallback('BindDocumentsGridOnSelection');
             cBranchPopup.Hide();
             return false;
         }
         function ChangeStateforBranch(value) {
             cgridbranch.PerformCallback('SelectAndDeSelectDocuments' + '~' + value);
         }

         function ShowUserPopUp(s, e) {
             cUserPopup.Show();
             cgriduser.PerformCallback('BindDocumentsDetails');
         }

         function PerformCallToUserGridBind() {
             cgriduser.PerformCallback('BindDocumentsGridOnSelection');
             cUserPopup.Hide();
             return false;
         }
         function ChangeStateforUser(value) {
             cgriduser.PerformCallback('SelectAndDeSelectDocuments' + '~' + value);
         }

         function ShowBankPopUp(s, e) {
             cBankPopup.Show();
             cgridbank.PerformCallback('BindDocumentsDetails');
         }
         function PerformCallToBankGridBind() {
             //cgridbank.PerformCallback('BindDocumentsGridOnSelection');
             var selectedBank = cgridbank.GetSelectedKeysOnPage("ID");
             $("#hdnBankValue").val(selectedBank);
             cBankPopup.Hide();
             return false;
         }
         function ChangeStateforBank(value) {
             cgridbank.PerformCallback('SelectAndDeSelectDocuments' + '~' + value);
         }

         function ShowCashPopUp(s, e) {
             cCashPopup.Show();
             cgridcash.PerformCallback('BindDocumentsDetails');
         }
         function PerformCallToCashGridBind() {
             cgridcash.PerformCallback('BindDocumentsGridOnSelection');
             cCashPopup.Hide();
             return false;
         }
         function ChangeStateforCash(value) {
             cgridcash.PerformCallback('SelectAndDeSelectDocuments' + '~' + value);
         }

         function ShowDocDescPopUp(s, e) {
             cDocDescPopup.Show();
             //cgriddocdesc.PerformCallback('BindDocumentsDetails');
             if (s == 'Party') {
                 cDocDescPopup.SetHeaderText('Select Party');
             }
             else if (s == 'Ledger') {
                 cDocDescPopup.SetHeaderText('Select Ledger');
             }
             else if (s == 'Employee') {
                 //cDocDescPopup.SetHeaderText('Select Employee');
                 cDocDescPopup.SetHeaderText('Select SubLedger');
             }
             else if (s == 'Product') {
                 cDocDescPopup.SetHeaderText('Select Product');
             }
             else {
                     cDocDescPopup.SetHeaderText('Select Warehouse');
             }
             cgriddocdesc.PerformCallback('BindDocumentsDetails' + '~' + s);
         }
         function PerformCallToDocDescGridBind() {
             cgriddocdesc.PerformCallback('BindDocumentsGridOnSelection');
             cDocDescPopup.Hide();
             return false;
         }
         function ChangeStateforDocDesc(value) {
             cgriddocdesc.PerformCallback('SelectAndDeSelectDocuments' + '~' + value);
         }

         function DateChangeForFrom() {

             var sessionValFrom = "<%=Session["FinYearStart"]%>";
             var sessionValTo = "<%=Session["FinYearEnd"]%>";
             var sessionVal = "<%=Session["LastFinYear"]%>";
             var objsession = sessionVal.split('-');
             var MonthDate = cTxtStartDate.GetDate().getMonth() + 1;
             var DayDate = cTxtStartDate.GetDate().getDate();
             var YearDate = cTxtStartDate.GetDate().getYear();
             var Cdate = MonthDate + "/" + DayDate + "/" + YearDate;
             var Sto = new Date(sessionValTo).getMonth() + 1;
             var SFrom = new Date(sessionValFrom).getMonth() + 1;
             var SDto = new Date(sessionValTo).getDate();
             var SDFrom = new Date(sessionValFrom).getDate();

             if (YearDate >= objsession[0]) {
                 if (MonthDate < SFrom && YearDate == objsession[0]) {
                     alert('Enter Date Is Outside Of Financial Year !!');
                     var datePost = (SFrom + '-' + SDFrom + '-' + objsession[0]);
                     cTxtStartDate.SetDate(new Date(datePost));
                 }
                 else if (MonthDate > Sto && YearDate == objsession[1]) {
                     alert('Enter Date Is Outside Of Financial Year !!');
                     var datePost = (SFrom + '-' + SDFrom + '-' + objsession[0]);
                     cTxtStartDate.SetDate(new Date(datePost));
                 }
                 else if (YearDate != objsession[0] && YearDate != objsession[1]) {
                     alert('Enter Date Is Outside Of Financial Year !!');
                     var datePost = (SFrom + '-' + SDFrom + '-' + objsession[0]);
                     cTxtStartDate.SetDate(new Date(datePost));
                 }
             }
             else {
                 alert('Enter Date Is Outside Of Financial Year !!');
                 var datePost = (SFrom + '-' + SDFrom + '-' + objsession[0]);
                 cTxtStartDate.SetDate(new Date(datePost));
             }
         }
         function DateChangeForTo() {
             var sessionValFrom = "<%=Session["FinYearStart"]%>";
             var sessionValTo = "<%=Session["FinYearEnd"]%>";
             var sessionVal = "<%=Session["LastFinYear"]%>";
             var objsession = sessionVal.split('-');
             var MonthDate = cTxtEndDate.GetDate().getMonth() + 1;
             var DayDate = cTxtEndDate.GetDate().getDate();
             var YearDate = cTxtEndDate.GetDate().getYear();
             var Cdate = MonthDate + "/" + DayDate + "/" + YearDate;
             var Sto = new Date(sessionValTo).getMonth() + 1;
             var SFrom = new Date(sessionValFrom).getMonth() + 1;
             var SDto = new Date(sessionValTo).getDate();
             var SDFrom = new Date(sessionValFrom).getDate();

             if (YearDate <= objsession[1]) {
                 if (MonthDate < SFrom && YearDate == objsession[0]) {
                     alert('Enter Date Is Outside Of Financial Year !!');
                     var datePost = (Sto + '-' + SDto + '-' + objsession[1]);
                     cTxtEndDate.SetDate(new Date(datePost));
                 }
                 else if (MonthDate > Sto && YearDate == objsession[1]) {
                     alert('Enter Date Is Outside Of Financial Year !!');
                     var datePost = (Sto + '-' + SDto + '-' + objsession[1]);
                     cTxtEndDate.SetDate(new Date(datePost));
                 }
                 else if (YearDate != objsession[0] && YearDate != objsession[1]) {
                     alert('Enter Date Is Outside Of Financial Year !!');
                     var datePost = (Sto + '-' + SDto + '-' + objsession[1]);
                     cTxtEndDate.SetDate(new Date(datePost));
                 }
             }
             else {
                 alert('Enter Date Is Outside Of Financial Year !!');
                 var datePost = (Sto + '-' + SDto + '-' + objsession[1]);
                 cTxtEndDate.SetDate(new Date(datePost));
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
        .padTable>tbody>tr>td {
            padding-right:15px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="panel-heading">
    <div class="panel-title">
        <%--<h3>Report Parameter</h3>--%>
        <asp:Label ID="lblReportTitle" runat="server" Text=""></asp:Label>
    </div>
</div>
<div class="form_main clearfix">
    <dxe:aspxpopupcontrol ID="popUp_newReport" runat="server" ClientInstanceName="cpopUp_newReport"
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
                                         <asp:TextBox ID="txtFileName" runat="server" MaxLength="50" autocomplete="off" ></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                       <asp:Button ID="btnNewFileSave" runat="server" Text="Ok" OnClick="btnNewFileSave_Click"/>
                                         <asp:Button ID="btnNewFileCancel" runat="server" Text="Cancel" OnClientClick="hidePopup(); return false;"/>
                                    </td>                                   
                            </table>                           
                        </div>

                    </dxe:PopupControlContentControl>
                </contentcollection>
        <headerstyle backcolor="LightGray" forecolor="Black" />
    </dxe:aspxpopupcontrol>
    <%--<asp:DropDownList ID="ddReportName" runat="server" Height="16px" Width="155px">
    </asp:DropDownList>--%>        
    <tr>
        <td>
            <asp:Panel ID="Panel2" CssClass="" runat="server" Width="100%">
                <div class="col-md-12" style="padding-left:0;">
                    <table class="padTable">
                        <tr>
                            <td style="width:250px">
                                <label>Start Date:</label>
                                <dxe:ASPxDateEdit ID="TxtStartDate" Width="100%" runat="server" UseMaskBehavior="True" EditFormat="Custom" EditFormatString="dd MMMM yyyy" ClientInstanceName="cTxtStartDate">
                                    <ButtonStyle Width="13px">
                                    </ButtonStyle>                                    
                                    <%-- <ClientSideEvents DateChanged="function(s,e){DateChangeForFrom();}" />--%>
                                    <DropDownButton Text="From Date">
                                    </DropDownButton>
                                </dxe:ASPxDateEdit>
                                        
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TxtStartDate"
                                    Display="Dynamic" EnableTheming="True" ErrorMessage="Date required!"></asp:RequiredFieldValidator>
                            </td>
                            <td style="width:250px">
                                 <label>End Date:</label>
                                <dxe:ASPxDateEdit ID="TxtEndDate" runat="server" Width="100%" UseMaskBehavior="True" EditFormat="Custom"  EditFormatString="dd MMMM yyyy" ClientInstanceName="cTxtEndDate">
                                        <ButtonStyle Width="13px">
                                        </ButtonStyle>
                                         <%--<ClientSideEvents DateChanged="function(s,e){DateChangeForTo();}" />--%>
                                        <DropDownButton Text="To Date">
                                        </DropDownButton>
                                    </dxe:ASPxDateEdit>                                        
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="TxtEndDate"
                                        Display="Dynamic" EnableTheming="True" ErrorMessage="Date required!"></asp:RequiredFieldValidator>
                            </td>
                            <td style="padding-top:20px;">
                                <dxe:ASPxButton ID="btnDocNo" runat="server" ToolTip="Click on Select the Document(s)" Text="Select Document" AutoPostBack="False" CssClass=" btn btn-primary" ClientInstanceName="cbtnDocNo" ClientVisible="False">
                                    <ClientSideEvents Click="function(s, e){ 
                                                                    ShowDocumentPopUp();
                                                                e.processOnServer=false;
                                                                }"></ClientSideEvents>
                                </dxe:ASPxButton>
                                <dxe:ASPxButton ID="btnBranch" runat="server" ToolTip="Click on Select the Branch(s)" Text="Select Branch" AutoPostBack="False" CssClass=" btn btn-primary" ClientInstanceName="cbtnBranch" ClientVisible="False">
                                    <ClientSideEvents Click="function(s, e){ 
                                                                    ShowBranchPopUp();
                                                                e.processOnServer=false;
                                                                }"></ClientSideEvents>
                                </dxe:ASPxButton>
                                <dxe:ASPxButton ID="btnUser" runat="server" ToolTip="Click on Select the User(s)" Text="Select User" AutoPostBack="False" CssClass="btn btn-primary" ClientInstanceName="cbtnUser" ClientVisible="False">
                                    <ClientSideEvents Click="function(s, e){ 
                                                                    ShowUserPopUp();
                                                                e.processOnServer=false;
                                                                }"></ClientSideEvents>
                                </dxe:ASPxButton>
                                <dxe:ASPxButton ID="btnBank" runat="server" ToolTip="Click on Select the Bank(s)" Text="Select Bank" AutoPostBack="False" CssClass=" btn btn-primary" ClientInstanceName="cbtnBank" ClientVisible="False">
                                    <ClientSideEvents Click="function(s, e){ 
                                                                    ShowBankPopUp();
                                                                e.processOnServer=false;
                                                                }"></ClientSideEvents>
                                </dxe:ASPxButton>
                                    <dxe:ASPxButton ID="btnCash" runat="server" ToolTip="Click on Select the Cash(s)" Text="Select Cash" AutoPostBack="False" CssClass=" btn btn-primary" ClientInstanceName="cbtnCash" ClientVisible="False">
                                    <ClientSideEvents Click="function(s, e){ 
                                                                    ShowCashPopUp();
                                                                e.processOnServer=false;
                                                                }"></ClientSideEvents>
                                </dxe:ASPxButton>
                                <dxe:ASPxButton ID="btnParty" runat="server" ToolTip="Click on Select the Party(s)" Text="Select Party" AutoPostBack="False" CssClass=" btn btn-primary" ClientInstanceName="cbtnParty" ClientVisible="False">
                                    <ClientSideEvents Click="function(s, e){ 
                                                                    ShowDocDescPopUp('Party');
                                                                e.processOnServer=false;
                                                                }"></ClientSideEvents>
                                </dxe:ASPxButton>
                                <dxe:ASPxButton ID="btnLedger" runat="server" ToolTip="Click on Select the Ledger(s)" Text="Select Ledger" AutoPostBack="False" CssClass=" btn btn-primary" ClientInstanceName="cbtnLedger" ClientVisible="False">
                                    <ClientSideEvents Click="function(s, e){ 
                                                                    ShowDocDescPopUp('Ledger');
                                                                e.processOnServer=false;
                                                                }"></ClientSideEvents>
                                </dxe:ASPxButton>
                                <dxe:ASPxButton ID="btnEmployee" runat="server" ToolTip="Click on Select the SubLedger(s)" Text="Select SubLedger" AutoPostBack="False" CssClass=" btn btn-primary" ClientInstanceName="cbtnEmployee" ClientVisible="False">
                                    <ClientSideEvents Click="function(s, e){ 
                                                                    ShowDocDescPopUp('Employee');
                                                                e.processOnServer=false;
                                                                }"></ClientSideEvents>
                                </dxe:ASPxButton>
                                <dxe:ASPxButton ID="btnProduct" runat="server" ToolTip="Click on Select the Product(s)" Text="Select Product" AutoPostBack="False" CssClass=" btn btn-primary" ClientInstanceName="cbtnProduct" ClientVisible="False">
                                    <ClientSideEvents Click="function(s, e){ 
                                                                    ShowDocDescPopUp('Product');
                                                                e.processOnServer=false;
                                                                }"></ClientSideEvents>
                                </dxe:ASPxButton>
                                <div runat="server" id="dvPaystructure" style="display:none">
                                 <label>Pay Structure</label>
                                    <dxe:ASPxComboBox ID="CmbStructure" EnableIncrementalFiltering="True" ClientInstanceName="cmbStructure"
                                                DataSourceID="SqlStructure" SelectedIndex="0" EnableCallbackMode="true"
                                                TextField="StructureName" ValueField="StructureID"
                                                runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True">
                                                
                                            </dxe:ASPxComboBox>
                                </div>
                                <dxe:ASPxButton ID="btnWarehouse" runat="server" ToolTip="Click on Select the Warehouse(s)" Text="Select Warehouse" AutoPostBack="False" CssClass=" btn btn-primary" ClientInstanceName="cbtnWarehouse" ClientVisible="False">
                                    <ClientSideEvents Click="function(s, e){ 
                                                                    ShowDocDescPopUp('Warehouse');
                                                                e.processOnServer=false;
                                                                }"></ClientSideEvents>
                                </dxe:ASPxButton>
                                <dxe:ASPxCheckBox ID="chkZeroBal" runat="server" Text="Suppress Zero Balance" CssClass=" chk chk-primary"  ClientInstanceName="cchkZeroBal" ClientVisible="False"/>
                                </dxe:ASPxCheckBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>Select Report</label>
                                <dxe:aspxcombobox ID="ddReportName" runat="server" SelectedIndex="0" ValueType="System.String" ClientInstanceName="cddReportName" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True">
                                 <ClientSideEvents SelectedIndexChanged="ddReportName_SelectedIndexChanged"  />
                                </dxe:aspxcombobox>
                            </td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td colspan="3" style="padding-top:15px">
                                <dxe:aspxbutton ID="btnNewDesign" ClientInstanceName="cbtnNewDesign" runat="server" ToolTip="Click on make a New design" AutoPostBack="false" Text="New" CssClass="btn btn-success">
                                 <ClientSideEvents Click="CopyNew"></ClientSideEvents>
                                </dxe:aspxbutton>     
                                <dxe:aspxbutton ID="btnLoadDesign" runat="server" ToolTip="Click on Open the design" AutoPostBack="false" Text="Open" OnClick="btnLoadDesign_Click" ClientInstanceName="cbtnLoadDesign" CssClass="btn btn-primary"  /></dxe:ASPxButton>
                                <dxe:aspxbutton ID="btnPreview" runat="server" ToolTip="Click on Preview the report" AutoPostBack="true" Text="Preview" OnClick="btnPreview_Click" CssClass="btn btn-primary" >
                                   <ClientSideEvents Click=" cbtnPreviewClick"></ClientSideEvents>
                                </dxe:ASPxButton>
                            </td>
                        </tr>
                    </table>
                    
                    <div class="clear" >
                        
                    </div>
                                                      
                </div>
            </asp:Panel>
        </td>
    </tr>    
    <%--<br />--%>
    <%--<br />--%>        
    <div class="PopUpArea">
        <dxe:aspxpopupcontrol ID="ASPxDocumentsPopup" runat="server" ClientInstanceName="cDocumentsPopup"
            Width="500px" HeaderText="Select Documents" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    
                    <div style="padding: 7px 0;">
                        <div class="hide">
                        <input type="button" value="Select All Documents" onclick="ChangeStateforDocument('SelectAll')" class="btn btn-primary"></input>                        
                        <input type="button" value="Revert" onclick="ChangeStateforDocument('Revart')" class="btn btn-primary"></input>
                        </div>
                        <input type="button" value="De-select All Documents" onclick="ChangeStateforDocument('UnSelectAll')" class="btn btn-primary"></input>
                    </div>
                    <dxe:ASPxGridView runat="server" KeyFieldName="ID" ClientInstanceName="cgriddocuments" ID="grid_Documents" OnDataBinding="grid_Documents_DataBinding"
                        Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" SettingsPager-Mode="ShowAllRecords" OnCustomCallback="cgridDocuments_CustomCallback"
                        Settings-ShowFooter="false" AutoGenerateColumns="False"                 
                        Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Visible">
                                                      
                        <SettingsBehavior AllowDragDrop="False" AllowSort="False"></SettingsBehavior>
                        <SettingsPager Visible="false"></SettingsPager>
                        <Columns>
                            <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="2" Caption=" " />
                            <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="SrlNo" Width="0" ReadOnly="true" Caption="Sl. No.">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="ID" Width="5" ReadOnly="true" Caption="Doc. ID">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="Doc_Number" Width="15" ReadOnly="true" Caption="Document No">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="Doc_Date" Width="15" ReadOnly="true" Caption="Document Date">
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
                        <SettingsSearchPanel Visible="True" />
                        <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />                                
                    </dxe:ASPxGridView>                    
                    <div class="text-center">
                        <dxe:ASPxButton ID="btnOKDocument" ClientInstanceName="cbtnOKDocument" runat="server"  AutoPostBack="False" Text="OK" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior ="False">
                            <ClientSideEvents Click="function(s, e) {return PerformCallToDocumentGridBind();}" />
                        </dxe:ASPxButton>
                    </div>
                </dxe:PopupControlContentControl>                
            </ContentCollection>
            </dxe:aspxpopupcontrol>
    </div>
    <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
           <ClientSideEvents ControlsInitialized="AllControlInitilize" />
        </dxe:ASPxGlobalEvents>
    <div class="PopUpArea">
        <dxe:aspxpopupcontrol ID="AspxBranchPopup" runat="server" ClientInstanceName="cBranchPopup"
            Width="500px" HeaderText="Select Branch(s)" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <div style="padding: 7px 0;">
                        <div class="hide">
                        <input type="button" value="Select All" onclick="ChangeStateforBranch('SelectAll')" class="btn btn-primary"></input>                        
                        </div>
                        <input type="button" value="De-select All" onclick="ChangeStateforBranch('UnSelectAll')" class="btn btn-primary"></input>
                        <%--<input type="button" value="Revert" onclick="ChangeStateforBranch('Revart')" class="btn btn-primary"></input>--%>
                    </div>
                    <dxe:ASPxGridView runat="server" KeyFieldName="ID" ClientInstanceName="cgridbranch" ID="grid_Branch" OnDataBinding="grid_Branch_DataBinding"
                        Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" OnCustomCallback="cgridBranch_CustomCallback"
                        Settings-ShowFooter="false" AutoGenerateColumns="False"                  
                        Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Visible">
                        <SettingsBehavior AllowDragDrop="False" AllowSort="False"></SettingsBehavior>
                        <%--<SettingsPager Visible="false"></SettingsPager>--%>
                        <Columns>
                            <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="2" Caption=" " />
                            <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="SrlNo" Width="0" ReadOnly="true" Caption="Sl. No.">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="ID" Width="0" ReadOnly="true" Caption="Branch ID">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="Doc_Code" Width="15" ReadOnly="true" Caption="Branch Code">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="Description" Width="15" ReadOnly="true" Caption="Description">
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                        <SettingsPager PageSize="10">
                            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                        </SettingsPager>
                        <SettingsSearchPanel Visible="True" />
                        <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                    </dxe:ASPxGridView>
                    <div class="text-center">
                        <dxe:ASPxButton ID="btnOKBranch" ClientInstanceName="cbtnOKBranch" runat="server" AutoPostBack="False" Text="OK" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior ="False">
                            <ClientSideEvents Click="function(s, e) {return PerformCallToBranchGridBind();}" />
                        </dxe:ASPxButton>
                    </div>
                </dxe:PopupControlContentControl>
            </ContentCollection>

        </dxe:aspxpopupcontrol>
    </div>
    <div class="PopUpArea">
        <dxe:aspxpopupcontrol ID="AspxUserpopup" runat="server" ClientInstanceName="cUserPopup"
            Width="500px" HeaderText="Select User(s)" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <div style="padding: 7px 0;">
                        <div class="hide">
                        <input type="button" value="Select All" onclick="ChangeStateforUser('SelectAll')" class="btn btn-primary"></input>                        
                        <%--<input type="button" value="Revert" onclick="ChangeStateforUser('Revart')" class="btn btn-primary"></input>--%>
                        </div>
                        <input type="button" value="De-select All" onclick="ChangeStateforUser('UnSelectAll')" class="btn btn-primary"></input>
                    </div>
                    <dxe:ASPxGridView runat="server" KeyFieldName="ID" ClientInstanceName="cgriduser" ID="grid_User" OnDataBinding="grid_User_DataBinding"
                        Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" OnCustomCallback="cgridUser_CustomCallback"
                        Settings-ShowFooter="false" AutoGenerateColumns="False"                  
                        Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Visible">
                        <SettingsBehavior AllowDragDrop="False" AllowSort="False"></SettingsBehavior>
                        <%--<SettingsPager Visible="false"></SettingsPager>--%>
                        <Columns>
                            <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="2" Caption=" " />
                            <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="SrlNo" Width="0" ReadOnly="true" Caption="Sl. No.">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="ID" Width="0" ReadOnly="true" Caption="User ID">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="User" Width="10" ReadOnly="true" Caption="Entered by">
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                        <SettingsPager PageSize="10">
                            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                        </SettingsPager>
                        <SettingsSearchPanel Visible="True" />
                        <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                    </dxe:ASPxGridView>
                    <div class="text-center">
                        <dxe:ASPxButton ID="btnOKUser" ClientInstanceName="cbtnOKUser" runat="server" AutoPostBack="False" Text="OK" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior ="False">
                            <ClientSideEvents Click="function(s, e) {return PerformCallToUserGridBind();}" />
                        </dxe:ASPxButton>
                    </div>
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:aspxpopupcontrol>
    </div>
    <div class="PopUpArea">
        <dxe:aspxpopupcontrol ID="AspxBankpopup" runat="server" ClientInstanceName="cBankPopup"
            Width="500px" HeaderText="Select Bank(s)" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <div style="padding: 7px 0;">
                        <div class="hide">
                            <input type="button" value="Select All" onclick="ChangeStateforBank('SelectAll')" class="btn btn-primary"></input>
                            <%--<input type="button" value="Revert" onclick="ChangeStateforBank('Revart')" class="btn btn-primary"></input>--%>
                            <input type="button" value="De-select All" onclick="ChangeStateforBank('UnSelectAll')" class="btn btn-primary"></input>
                        </div>
                    </div>
                    <dxe:ASPxGridView runat="server" KeyFieldName="ID" ClientInstanceName="cgridbank" ID="grid_Bank" OnDataBinding="grid_Bank_DataBinding"
                        Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" OnCustomCallback="cgridBank_CustomCallback"
                        Settings-ShowFooter="false" AutoGenerateColumns="False" SelectionMode="Single"
                        Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Visible">
                        <SettingsBehavior AllowDragDrop="False" AllowSort="False" AllowSelectSingleRowOnly="True"></SettingsBehavior>
                        <%--<SettingsPager Visible="false"></SettingsPager>--%>
                        <Columns>
                            <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="2" Caption=" " />
                            <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="SrlNo" Width="0" ReadOnly="true" Caption="Sl. No.">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="ID" Width="0" ReadOnly="true" Caption="User ID">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="Bank Name" Width="20" ReadOnly="true" Caption="Bank Name">
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                        <SettingsPager PageSize="10">
                            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                        </SettingsPager>
                        <SettingsSearchPanel Visible="True" />
                        <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                        <%--<clientsideevents endcallback="BankCallback_EndCallback" />--%>
                    </dxe:ASPxGridView>
                    <div class="text-center">
                        <dxe:ASPxButton ID="btnOKBank" ClientInstanceName="cbtnOKBank" runat="server" AutoPostBack="False" Text="OK" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior ="False">
                            <ClientSideEvents Click="function(s, e) {return PerformCallToBankGridBind();}" />
                        </dxe:ASPxButton>
                    </div>
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:aspxpopupcontrol>
    </div>
    <div class="PopUpArea">
        <dxe:aspxpopupcontrol ID="AspxCashpopup" runat="server" ClientInstanceName="cCashPopup"
            Width="500px" HeaderText="Select Cash(s)" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <div style="padding: 7px 0;">
                        <div class="hide">
                        <input type="button" value="Select All" onclick="ChangeStateforCash('SelectAll')" class="btn btn-primary"></input>                        
                        <%--<input type="button" value="Revert" onclick="ChangeStateforCash('Revart')" class="btn btn-primary"></input>--%>
                        </div>
                        <input type="button" value="De-select All" onclick="ChangeStateforCash('UnSelectAll')" class="btn btn-primary"></input>
                    </div>
                    <dxe:ASPxGridView runat="server" KeyFieldName="ID" ClientInstanceName="cgridcash" ID="grid_Cash" OnDataBinding="grid_Cash_DataBinding"
                        Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" OnCustomCallback="cgridCash_CustomCallback"
                        Settings-ShowFooter="false" AutoGenerateColumns="False"
                        Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Visible">
                        <SettingsBehavior AllowDragDrop="False" AllowSort="False"></SettingsBehavior>
                        <%--<SettingsPager Visible="false"></SettingsPager>--%>
                        <Columns>
                            <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="2" Caption=" " />
                            <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="SrlNo" Width="0" ReadOnly="true" Caption="Sl. No.">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="ID" Width="0" ReadOnly="true" Caption="User ID">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="Cash" Width="20" ReadOnly="true" Caption="Cash">
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                        <SettingsPager PageSize="10">
                            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                        </SettingsPager>
                        <SettingsSearchPanel Visible="True" />
                        <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                    </dxe:ASPxGridView>
                    <div class="text-center">
                        <dxe:ASPxButton ID="btnOKCash" ClientInstanceName="cbtnOKCash" runat="server" AutoPostBack="False" Text="OK" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior ="False">
                            <ClientSideEvents Click="function(s, e) {return PerformCallToCashGridBind();}" />
                        </dxe:ASPxButton>
                    </div>
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:aspxpopupcontrol>
    </div>
    <div class="PopUpArea">
        <dxe:aspxpopupcontrol ID="AspxDocDescpopup" runat="server" ClientInstanceName="cDocDescPopup"
            Width="600px" HeaderText="" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <div style="padding: 7px 0;">
                        <div class="hide">
                        <input type="button" value="Select All" onclick="ChangeStateforDocDesc('SelectAll')" class="btn btn-primary"></input>                        
                        <%--<input type="button" value="Revert" onclick="ChangeStateforDocDesc('Revart')" class="btn btn-primary"></input>--%>
                        </div>
                        <input type="button" value="De-select All" onclick="ChangeStateforDocDesc('UnSelectAll')" class="btn btn-primary"></input>
                    </div>
                    <dxe:ASPxGridView runat="server" KeyFieldName="ID" ClientInstanceName="cgriddocdesc" ID="grid_DocDesc" OnDataBinding="grid_DocDesc_DataBinding" OnDataBound="gridDocDesc_DataBound"
                        Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" OnCustomCallback="cgridDocDesc_CustomCallback" AutoGenerateColumns="True"
                        Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Visible">
                        <SettingsBehavior AllowDragDrop="False" AllowSort="False"></SettingsBehavior>
                      
                        <Columns>
                            <dxe:GridViewCommandColumn VisibleIndex="1" ShowSelectCheckbox="True" Width="10" Caption=" " />
                            <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="SrlNo" Width="0" ReadOnly="true" Caption="Sl. No.">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="ID" Width="0" ReadOnly="true" Caption="ID">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="Doc Code" Width="50" ReadOnly="true" Caption="">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="Description" Width="100" ReadOnly="true" Caption="">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="Type" Width="50" ReadOnly="true" Caption="">
                            </dxe:GridViewDataTextColumn>
                        </Columns>                        

                        <SettingsPager PageSize="10">
                            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                        </SettingsPager>

                        <SettingsSearchPanel Visible="True" />
                        <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                    </dxe:ASPxGridView>
                    <div class="text-center">
                        <dxe:ASPxButton ID="btnOKDocDesc" ClientInstanceName="cbtnOKDocDesc" runat="server" AutoPostBack="False" Text="OK" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior ="False">
                            <ClientSideEvents Click="function(s, e) {return PerformCallToDocDescGridBind();}" />
                        </dxe:ASPxButton>
                    </div>
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:aspxpopupcontrol>
    </div>
    <%--<br />
    <%--<br />--%>
       
    <%--<asp:LinkButton ID="lnkBtnCopy" runat="server"   OnClientClick="CopyNew();return false;">Copy</asp:LinkButton>--%>
    <br />
    <br />
    
    <asp:HiddenField ID ="RptName" runat="server" />
    <asp:HiddenField ID ="ReturnPage" runat="server" />
    <asp:HiddenField ID ="HDReportModuleName" runat="server" />  
    <asp:HiddenField ID ="hdnBankValue" runat="server" />
     <dxe:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" ContainerElementID="divSubmitButton" 
        Modal="True" Text="Please Wait..">     
    </dxe:ASPxLoadingPanel>

                <asp:SqlDataSource ID="SqlStructure" runat="server" 
                SelectCommand="select StructureName,StructureID from proll_PayStructureMaster where IsDeleted='N'"></asp:SqlDataSource>
    


</div>
</asp:Content>
