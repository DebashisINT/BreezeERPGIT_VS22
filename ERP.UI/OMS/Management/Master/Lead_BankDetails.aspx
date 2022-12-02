<%@ Page Title="Bank Details" Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/Erp.Master"
    Inherits="ERP.OMS.Management.Master.management_Master_Lead_BankDetails" CodeBehind="Lead_BankDetails.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript">
        function callAjax(obj, obj1, obj2, obj3) {
            var o = document.getElementById("SearchCombo")
            ajax_showOptions(obj, obj1, obj2, o.value)
        }
        function chkAct(str12, str) {
            var str = document.getElementById(str)
            str.value = str12;
        }

        function disp_prompt(name) {
            //var ID = document.getElementById(txtID);
            if (name == "tab0") {
                //alert(name);
                document.location.href = "Lead_general.aspx";
            }
            if (name == "tab1") {
                //alert(name);
                document.location.href = "Lead_Correspondence.aspx";
            }
            else if (name == "tab2") {
                //alert(name);
                //document.location.href="Lead_BankDetails.aspx"; 
            }
            //else if (name == "tab3") {
            //    //alert(name);
            //    document.location.href = "Lead_DPDetails.aspx";
            //}
            else if (name == "tab4") {
                //alert(name);
                document.location.href = "Lead_Document.aspx";
            }
            else if (name == "tab5") {
                //alert(name);
                document.location.href = "Lead_FamilyMembers.aspx";
            }
            else if (name == "tab6") {
                //alert(name);
                document.location.href = "Lead_Registration.aspx";
            }
            else if (name == "tab7") {
                //alert(name);
                document.location.href = "Lead_GroupMember.aspx";
            }
            else if (name == "tab8") {
                //alert(name);
                document.location.href = "Lead_Remarks.aspx";
            }
        }
        function CallList(obj1, obj2, obj3) {
            var obj4 = '';
            //alert(valuse);
            if (valuse == 0)
                obj4 = 'bnk_bankName';
            if (valuse == 1)
                obj4 = 'bnk_Micrno';
            if (valuse == 2)
                obj4 = 'bnk_branchName';
            //alert(obj4);
            ajax_showOptions(obj1, obj2, obj3, obj4);
        }
        function setvaluetovariable(obj1) {
            valuse = obj1;
        }
        valuse = '0';
        FieldName = 'ASPxPageControl1_txtequity';
    </script>

    <div class="panel-heading">
        <div class="panel-title">
            <h3>Lead Bank Details</h3>
        </div>
        <div class="crossBtn"><a href="Lead.aspx"><i class="fa fa-times"></i></a></div>
    </div>
    <div class="form_main">
        <table class="TableMain100">
            <tr>
                <td>
                    <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="2" ClientInstanceName="page">
                        <TabPages>
                            <dxe:TabPage Name="General" Text="General">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="CorresPondence" Text="CorresPondence">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Bank Details" Text="Bank Details">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                        <a class="btn btn-primary" href="javascript:void(0);" onclick="gridBank.AddNewRow();"><span>Add New</span> </a>
                                        <asp:Label runat="server" Font-Bold="True" ForeColor="Red" ID="lblmessage"></asp:Label>
                                        <dxe:ASPxGridView runat="server" ClientInstanceName="gridBank" KeyFieldName="Id"
                                            AutoGenerateColumns="False" DataSourceID="BankDetails" Width="100%" Font-Size="12px"
                                            ID="BankDetailsGrid" OnRowUpdating="BankDetailsGrid_RowUpdating" OnRowValidating="BankDetailsGrid_RowValidating"
                                            OnRowInserting="BankDetailsGrid_RowInserting" OnHtmlEditFormCreated="BankDetailsGrid_HtmlEditFormCreated">
                                            <Columns>
                                                <dxe:GridViewDataTextColumn FieldName="Id" Caption="Type" Visible="False" VisibleIndex="0">
                                                    <EditFormSettings Visible="False" Caption="ID"></EditFormSettings>
                                                    <CellStyle CssClass="gridcellleft">
                                                    </CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="Category" Width="12%" Caption="Category"
                                                    VisibleIndex="0">
                                                    <EditFormSettings Visible="False" Caption="Category"></EditFormSettings>
                                                    <CellStyle CssClass="gridcellleft">
                                                    </CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="AccountType" Width="12%" Caption="Account Type"
                                                    VisibleIndex="1">
                                                    <EditFormSettings Visible="False" Caption="AccountType"></EditFormSettings>
                                                    <CellStyle CssClass="gridcellleft">
                                                    </CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="BankName" Width="12%" Caption="Bank Name"
                                                    VisibleIndex="2">
                                                    <EditFormSettings Visible="False" Caption="BankName"></EditFormSettings>
                                                    <CellStyle CssClass="gridcellleft">
                                                    </CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="Branch" Width="12%" Caption="Branch" VisibleIndex="3">
                                                    <EditFormSettings Visible="False" Caption="Branch"></EditFormSettings>
                                                    <CellStyle CssClass="gridcellleft">
                                                    </CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="MICR" Width="12%" Caption="MICR" VisibleIndex="4">
                                                    <EditFormSettings Visible="False" Caption="MICR"></EditFormSettings>
                                                    <CellStyle CssClass="gridcellleft">
                                                    </CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataComboBoxColumn FieldName="Category" Caption="Category" Visible="False"
                                                    VisibleIndex="0">
                                                    <PropertiesComboBox ValueType="System.String">
                                                        <Items>
                                                            <dxe:ListEditItem Text="Default" Value="Default"></dxe:ListEditItem>
                                                            <dxe:ListEditItem Text="Secondary" Value="Secondary"></dxe:ListEditItem>
                                                        </Items>
                                                    </PropertiesComboBox>
                                                    <EditFormSettings Visible="True"></EditFormSettings>
                                                    <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                    </EditFormCaptionStyle>
                                                </dxe:GridViewDataComboBoxColumn>
                                                <dxe:GridViewDataComboBoxColumn FieldName="AccountType" Caption="Account Type"
                                                    Visible="False" VisibleIndex="0">
                                                    <PropertiesComboBox ValueType="System.String">
                                                        <Items>
                                                            <dxe:ListEditItem Text="Saving" Value="Saving"></dxe:ListEditItem>
                                                            <dxe:ListEditItem Text="Current" Value="Current"></dxe:ListEditItem>
                                                            <dxe:ListEditItem Text="Joint" Value="Joint"></dxe:ListEditItem>
                                                        </Items>
                                                    </PropertiesComboBox>
                                                    <EditFormSettings Visible="True"></EditFormSettings>
                                                    <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                    </EditFormCaptionStyle>
                                                </dxe:GridViewDataComboBoxColumn>
                                                <dxe:GridViewDataTextColumn FieldName="AccountNumber" Width="12%" Caption="Account Number"
                                                    VisibleIndex="5">
                                                    <EditFormSettings Visible="True" Caption="AccountNumber"></EditFormSettings>
                                                    <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                    </EditFormCaptionStyle>
                                                    <CellStyle CssClass="gridcellleft">
                                                    </CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="AccountName" Width="12%" Caption="Account Name"
                                                    VisibleIndex="6">
                                                    <EditFormSettings Visible="True" Caption="AccountName"></EditFormSettings>
                                                    <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                    </EditFormCaptionStyle>
                                                    <CellStyle CssClass="gridcellleft">
                                                    </CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="BankName" Caption="BankName" Visible="False"
                                                    VisibleIndex="0">
                                                    <EditFormSettings Visible="True" Caption="BankName"></EditFormSettings>
                                                    <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                    </EditFormCaptionStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewCommandColumn VisibleIndex="7" ShowCancelButton="true" ShowEditButton="true" HeaderStyle-HorizontalAlign="Center">

                                                    <HeaderTemplate>
                                                        
                                                    </HeaderTemplate>
                                                </dxe:GridViewCommandColumn>
                                            </Columns>
                                            <SettingsCommandButton>

                                             
                                                                    <EditButton Image-Url="/assests/images/Edit.png" ButtonType="Image" Image-AlternateText="Edit">
                                                                    </EditButton>
                                                  <DeleteButton Image-Url="/assests/images/Delete.png" ButtonType="Image" Image-AlternateText="Delete">
                                                                    </DeleteButton>
                                                                    <UpdateButton Text="Update" ButtonType="Button" Styles-Style-CssClass="btn btn-primary btn-xs"></UpdateButton>
                                                                    <CancelButton Text="Cancel" ButtonType="Button" Styles-Style-CssClass="btn btn-danger btn-xs"></CancelButton>
                                            </SettingsCommandButton>
                                            <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True"></SettingsBehavior>
                                            <SettingsPager PageSize="20" NumericButtonCount="20">
                                                <FirstPageButton Visible="True">
                                                </FirstPageButton>
                                                <LastPageButton Visible="True">
                                                </LastPageButton>
                                            </SettingsPager>
                                            <SettingsEditing PopupEditFormWidth="600px" PopupEditFormHeight="250px" PopupEditFormHorizontalAlign="Center"
                                                PopupEditFormVerticalAlign="WindowCenter" PopupEditFormModal="True" EditFormColumnCount="1">
                                            </SettingsEditing>
                                            <Settings ShowStatusBar="Visible" ShowGroupPanel="true"></Settings>
                                            <SettingsText ConfirmDelete="Are you sure to delete this record?" PopupEditFormCaption="Add/Modify Bank Details"></SettingsText>
                                            <Styles>
                                                <Header SortingImageSpacing="5px" ImageSpacing="5px">
                                                </Header>
                                                <LoadingPanel ImageSpacing="10px">
                                                </LoadingPanel>
                                            </Styles>
                                            <Templates>
                                                <EditForm>
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td style="text-align: center;">
                                                                <table>
                                                                    <tr>
                                                                        <td style="text-align: right;">Category:</td>
                                                                        <td style="text-align: left;" colspan="2">
                                                                            <dxe:ASPxComboBox ID="drpCategory" runat="server" ValueType="System.String" Width="203px"
                                                                                Value='<%#Bind("Category") %>' SelectedIndex="0" EnableIncrementalFiltering="true">
                                                                                <Items>
                                                                                    <dxe:ListEditItem Text="Default" Value="Default" />
                                                                                    <dxe:ListEditItem Text="Secondary" Value="Secondary" />
                                                                                </Items>
                                                                                <ValidationSettings CausesValidation="True" SetFocusOnError="True">
                                                                                    <RequiredField IsRequired="True" ErrorText="Select category" />
                                                                                </ValidationSettings>
                                                                            </dxe:ASPxComboBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="text-align: right;">Account Type:</td>
                                                                        <td style="text-align: left;" colspan="2">
                                                                            <dxe:ASPxComboBox ID="drpAccountType" runat="server" ValueType="System.String" Value='<%#Bind("AccountType") %>'
                                                                                Width="203px" SelectedIndex="0" EnableIncrementalFiltering="true">
                                                                                <Items>
                                                                                    <dxe:ListEditItem Text="Saving" Value="Saving" />
                                                                                    <dxe:ListEditItem Text="Current" Value="Current" />
                                                                                    <dxe:ListEditItem Text="Joint" Value="Joint" />
                                                                                </Items>
                                                                                <ValidationSettings CausesValidation="True" SetFocusOnError="True">
                                                                                    <RequiredField IsRequired="True" ErrorText="Select Account Type" />
                                                                                </ValidationSettings>
                                                                            </dxe:ASPxComboBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="text-align: right;">Bank Name:</td>
                                                                        <td style="text-align: left;">
                                                                            <asp:TextBox ID="txtbankname" runat="server" Width="200px" Text='<%#Bind("BankName1") %>'></asp:TextBox>
                                                                            <asp:TextBox ID="txtbankname_hidden" runat="server" Visible="false"></asp:TextBox>
                                                                        </td>
                                                                        <td style="text-align: left;">Search By:</td>
                                                                        <td>
                                                                            <dxe:ASPxComboBox ID="drpSearchBank" runat="server" ValueType="System.String" SelectedIndex="0"
                                                                                ClientInstanceName="combo" Width="100px" EnableIncrementalFiltering="true">
                                                                                <Items>
                                                                                    <dxe:ListEditItem Text="BankName" Value="bnk_bankName" />
                                                                                    <dxe:ListEditItem Text="MICR No" Value="bnk_Micrno" />
                                                                                    <dxe:ListEditItem Text="Branch Name" Value="bnk_branchName" />
                                                                                </Items>
                                                                                <ClientSideEvents ValueChanged="function(s,e){
                                                                                                    var indexr = s.GetSelectedIndex();
                                                                                                    setvaluetovariable(indexr)
                                                                                                    }" />
                                                                            </dxe:ASPxComboBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="text-align: right;">Account Number:</td>
                                                                        <td style="text-align: left;" colspan="2">
                                                                            <asp:TextBox ID="txtAccountNo" runat="server" Text='<%#Bind("AccountNumber") %>'
                                                                                Width="200px"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="text-align: right;">Account Name:</td>
                                                                        <td style="text-align: left;" colspan="2">
                                                                            <asp:TextBox ID="txtAnccountName" runat="server" Text='<%#Bind("AccountName") %>'
                                                                                Width="200px"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="text-align: right;" colspan="2">
                                                                            <dxe:ASPxButton ID="btnUpdate" runat="server" Text="Update" ToolTip="Update data"
                                                                                Height="18px" Width="88px" AutoPostBack="False">
                                                                                <ClientSideEvents Click="function(s, e) {gridBank.UpdateEdit();}" />
                                                                            </dxe:ASPxButton>
                                                                        </td>
                                                                        <td style="text-align: left;" colspan="2">
                                                                            <dxe:ASPxButton ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel data"
                                                                                Height="18px" Width="88px" AutoPostBack="False">
                                                                                <ClientSideEvents Click="function(s, e) {gridBank.CancelEdit();}" />
                                                                            </dxe:ASPxButton>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </EditForm>
                                            </Templates>
                                        </dxe:ASPxGridView>
                                        <br />
                                        <asp:Panel runat="server" Width="100%" ID="BankDetailsPanel">
                                            <table border="1" width="100%">
                                                <tbody>
                                                    <tr>
                                                        <td style="background-color: #2c4182; color:#fff;height:25px; text-align: center" colspan="2">
                                                            <span><strong>Investment</strong></span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top" width="50%" style="padding-left:10px;" align="left">
                                                            <table>
                                                                <tbody>
                                                                    <tr>
                                                                        <td style="text-align: right" class="Ecoheadtxt">
                                                                            <span style="">Gross Annual Salary </span>
                                                                        </td>
                                                                        <td>:</td>
                                                                        <td style="text-align: left" class="Ecoheadtxt">
                                                                            <span style="">Rs.</span><asp:TextBox runat="server" CssClass="form-control"
                                                                                ID="txtgrossannualsalary"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="text-align: right" class="Ecoheadtxt">
                                                                            <span style="">Annual Trunover </span>
                                                                        </td>
                                                                        <td>:</td>
                                                                        <td class="Ecoheadtxt">
                                                                            <span>Rs.</span><asp:TextBox runat="server"
                                                                                CssClass="form-control" ID="txtannualTrunover"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="text-align: right" class="Ecoheadtxt">
                                                                            <span style="">Gross Profit </span>
                                                                        </td>
                                                                        <td>:</td>
                                                                        <td style="" class="Ecoheadtxt">
                                                                            <span style="">Rs.</span><asp:TextBox runat="server" CssClass="form-control"
                                                                                ID="txtGrossProfit"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="text-align: right" class="Ecoheadtxt">
                                                                            <span style="">Approx. Expenses (PM) </span>
                                                                        </td>
                                                                        <td>:</td>
                                                                        <td class="Ecoheadtxt">
                                                                            <span style="">Rs.</span><asp:TextBox runat="server" CssClass="form-control"
                                                                                ID="txtPMExpenses"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="text-align: right" class="Ecoheadtxt">
                                                                            <span style="">Approx. Saving (PM) </span>
                                                                        </td>
                                                                        <td>:</td>
                                                                        <td style="" class="Ecoheadtxt">
                                                                            <span style="">Rs.</span><asp:TextBox runat="server" CssClass="form-control"
                                                                                ID="txtPMSaving"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </td>
                                                        <td style="width: 60%">
                                                            <table>
                                                                <tbody>
                                                                    <tr>
                                                                        <td style="width: 50%">
                                                                            <table>
                                                                                <tbody>
                                                                                    <tr>
                                                                                        <td style="text-align: right" class="Ecoheadtxt">
                                                                                            <span style="">Equity</span></td>
                                                                                        <td>:</td>
                                                                                        <td style="" class="Ecoheadtxt">
                                                                                            <span style="">Rs.</span><asp:TextBox runat="server" CssClass="form-control"
                                                                                                ID="txtequity"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="text-align: right" class="Ecoheadtxt">
                                                                                            <span style="">Mutal Fund</span></td>
                                                                                        <td>:</td>
                                                                                        <td style="" class="Ecoheadtxt">
                                                                                            <span style="">Rs.</span><asp:TextBox runat="server" CssClass="form-control"
                                                                                                ID="txtMutalFund"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="text-align: right" class="Ecoheadtxt">
                                                                                            <span style="">Bank FD's</span></td>
                                                                                        <td>:</td>
                                                                                        <td style="" class="Ecoheadtxt">
                                                                                            <span style="">Rs.</span><asp:TextBox runat="server" CssClass="form-control"
                                                                                                ID="txtBankFD"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="text-align: right" class="Ecoheadtxt">
                                                                                            <span style="">Debt's Instruments</span></td>
                                                                                        <td>:</td>
                                                                                        <td style="" class="Ecoheadtxt">
                                                                                            <span style="">Rs.</span><asp:TextBox runat="server" CssClass="form-control"
                                                                                                ID="txtDebtsInstruments"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="text-align: right" class="Ecoheadtxt">
                                                                                            <span style="">NSS's</span></td>
                                                                                        <td>:</td>
                                                                                        <td style="" class="Ecoheadtxt">
                                                                                            <span style="">Rs.</span><asp:TextBox runat="server" CssClass="form-control"
                                                                                                ID="txtNSS"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                </tbody>
                                                                            </table>
                                                                        </td>
                                                                        <td style="width: 50%">
                                                                            <table>
                                                                                <tbody>
                                                                                    <tr>
                                                                                        <td style="text-align: right" class="Ecoheadtxt">
                                                                                            <span style="">Life Insurance</span></td>
                                                                                        <td>:</td>
                                                                                        <td style="" class="Ecoheadtxt">
                                                                                            <span style="">Rs.</span><asp:TextBox runat="server" CssClass="form-control"
                                                                                                ID="txtLifeInsurance"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="text-align: right" class="Ecoheadtxt">
                                                                                            <span style="">Health Insurance</span></td>
                                                                                        <td>:</td>
                                                                                        <td style="" class="Ecoheadtxt">
                                                                                            <span style="">Rs.</span><asp:TextBox runat="server" CssClass="form-control"
                                                                                                ID="txtHealthInsurance"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="text-align: right" class="Ecoheadtxt">
                                                                                            <span style="">Real Estate</span></td>
                                                                                        <td>:</td>
                                                                                        <td style="" class="Ecoheadtxt">
                                                                                            <span style="">Rs.</span><asp:TextBox runat="server" CssClass="form-control"
                                                                                                ID="txtRealEstate"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="text-align: right" class="Ecoheadtxt">
                                                                                            <span style="">Precious Metals/Stones</span></td>
                                                                                        <td>:</td>
                                                                                        <td style="" class="Ecoheadtxt">
                                                                                            <span style="">Rs.</span><asp:TextBox runat="server" CssClass="form-control"
                                                                                                ID="txtPreciousMetals"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="text-align: right" class="Ecoheadtxt">
                                                                                            <span style="">Other's</span></td>
                                                                                        <td>:</td>
                                                                                        <td style="" class="Ecoheadtxt">
                                                                                            <span style="">Rs.</span><asp:TextBox runat="server" CssClass="form-control"
                                                                                                ID="txtOthers"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                </tbody>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="padding-left:10px;">
                                                            <table>
                                                                <tbody>
                                                                    <tr>
                                                                        <td style="text-align: right;" class="Ecoheadtxt">
                                                                            <span style="">Has Fund For Investment </span>
                                                                        </td>
                                                                        <td style="width: 11px">:</td>
                                                                        <td style="" class="Ecoheadtxt">
                                                                            <asp:CheckBox runat="server" ID="chkHasFundInvestment"
                                                                                OnCheckedChanged="chkHasFundInvestment_CheckedChanged"></asp:CheckBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="text-align: right" class="Ecoheadtxt">
                                                                            <span style="">If Yes Then Availabe Funds </span>
                                                                        </td>
                                                                        <td style="width: 11px">:</td>
                                                                        <td style="" class="Ecoheadtxt">
                                                                            <asp:TextBox runat="server" CssClass="form-control" ID="txtAvailableFund"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="text-align: right" class="Ecoheadtxt">
                                                                            <span style="">If Yes Then Investment Horizon </span>
                                                                        </td>
                                                                        <td style="width: 11px">:</td>
                                                                        <td style="" class="Ecoheadtxt">
                                                                            <asp:TextBox runat="server" CssClass="form-control" ID="txtInvestmentHorizon"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </td>
                                                        <td style="padding-left:10px;">
                                                            <table>
                                                                <tbody>
                                                                    <tr>
                                                                        <td>
                                                                            <table>
                                                                                <tbody>
                                                                                    <tr>
                                                                                        <td style="text-align: right" class="Ecoheadtxt">
                                                                                            <span style="">Ready to Transfer Existing Portfoilio </span>
                                                                                        </td>
                                                                                        <td>:</td>
                                                                                        <td style="" class="Ecoheadtxt" align="left">
                                                                                            <asp:CheckBox runat="server" ID="chkPortFoilio"
                                                                                                OnCheckedChanged="chkPortFoilio_CheckedChanged"></asp:CheckBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="text-align: right" class="Ecoheadtxt">
                                                                                            <span style="">If Yes Then Amount </span>
                                                                                        </td>
                                                                                        <td>:</td>
                                                                                        <td style="" class="Ecoheadtxt">
                                                                                            <asp:TextBox runat="server" ID="TxtPortFoilioAmount"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                </tbody>
                                                                            </table>
                                                                        </td>
                                                                        <td>
                                                                            <table>
                                                                                <tbody>
                                                                                    <tr>
                                                                                        <td style="text-align: right" class="Ecoheadtxt">
                                                                                            <span style="">&nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; Own House </span>
                                                                                        </td>
                                                                                        <td>:</td>
                                                                                        <td style="" class="Ecoheadtxt">
                                                                                            <asp:CheckBox runat="server" ID="chkhouse"></asp:CheckBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="text-align: right" class="Ecoheadtxt">
                                                                                            <span style="">&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; Own Vehicle
                                                                                            </span>
                                                                                        </td>
                                                                                        <td>:</td>
                                                                                        <td style="" class="Ecoheadtxt">
                                                                                            <asp:CheckBox runat="server" ID="chkVehicle"></asp:CheckBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                </tbody>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: right" align="center" colspan="2">
                                                            <asp:Button runat="server" Text="Save" ID="btn_Finance_Save" OnClick="btn_Finance_Save_Click" CssClass="btn btn-primary"></asp:Button>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </asp:Panel>
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="DP Details" Text="DP Details"  Visible="false">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Documents" Text="Documents">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Family Members" Text="Family Members">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Registration" Text="Registration">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Group Member" Text="Group Member">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Remarks" Text="Remarks">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                        </TabPages>
                        <ClientSideEvents ActiveTabChanged="function(s, e) {
	                                            var activeTab   = page.GetActiveTab();
	                                            var Tab0 = page.GetTab(0);
	                                            var Tab1 = page.GetTab(1);
	                                            var Tab2 = page.GetTab(2);
	                                            var Tab3 = page.GetTab(3);
	                                            var Tab4 = page.GetTab(4);
	                                            var Tab5 = page.GetTab(5);
	                                            var Tab6 = page.GetTab(6);
	                                            var Tab7 = page.GetTab(7);
	                                            var Tab8 = page.GetTab(8);
	                                            if(activeTab == Tab0)
	                                            {
	                                                disp_prompt('tab0');
	                                            }
	                                            if(activeTab == Tab1)
	                                            {
	                                                disp_prompt('tab1');
	                                            }
	                                            else if(activeTab == Tab2)
	                                            {
	                                                disp_prompt('tab2');
	                                            }
	                                            else if(activeTab == Tab3)
	                                            {
	                                                disp_prompt('tab3');
	                                            }
	                                            else if(activeTab == Tab4)
	                                            {
	                                                disp_prompt('tab4');
	                                            }
	                                            else if(activeTab == Tab5)
	                                            {
	                                                disp_prompt('tab5');
	                                            }
	                                            else if(activeTab == Tab6)
	                                            {
	                                                disp_prompt('tab6');
	                                            }
	                                            else if(activeTab == Tab7)
	                                            {
	                                                disp_prompt('tab7');
	                                            }
	                                            else if(activeTab == Tab8)
	                                            {
	                                                disp_prompt('tab8');
	                                            }
	                                            
	                                            }"></ClientSideEvents>
                        <ContentStyle>
                            <Border BorderColor="#002D96" BorderStyle="Solid" BorderWidth="1px" />
                        </ContentStyle>
                        <LoadingPanelStyle ImageSpacing="6px">
                        </LoadingPanelStyle>
                        <TabStyle Font-Size="12px">
                        </TabStyle>
                    </dxe:ASPxPageControl>
                </td>
                <td style="width: 3px"></td>
            </tr>
        </table>
    </div>
    <asp:SqlDataSource ID="BankDetails" runat="server" 
        SelectCommand="BankDetailsSelect" InsertCommand="BankDetailsInsert" InsertCommandType="StoredProcedure"
        SelectCommandType="StoredProcedure" UpdateCommand="BankDetailsUpdate" UpdateCommandType="StoredProcedure"
        DeleteCommand="delete from tbl_trans_contactBankDetails where cbd_id=@Id">
        <SelectParameters>
            <asp:SessionParameter Name="insuId" SessionField="KeyVal_InternalID" Type="String" />
        </SelectParameters>
        <InsertParameters>
            <asp:Parameter Name="Category" Type="String" />
            <asp:SessionParameter Name="insuId" SessionField="KeyVal_InternalID" Type="String" />
            <asp:Parameter Name="BankName1" Type="String" />
            <asp:Parameter Name="AccountNumber" Type="String" />
            <asp:Parameter Name="AccountType" Type="String" />
            <asp:Parameter Name="AccountName" Type="String" />
            <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="String" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="Category" Type="String" />
            <asp:Parameter Name="BankName1" Type="String" />
            <asp:Parameter Name="AccountNumber" Type="String" />
            <asp:Parameter Name="AccountType" Type="String" />
            <asp:Parameter Name="AccountName" Type="String" />
            <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="String" />
            <asp:Parameter Name="Id" Type="String" />
        </UpdateParameters>
        <DeleteParameters>
            <asp:Parameter Name="Id" Type="int32" />
        </DeleteParameters>
    </asp:SqlDataSource>
</asp:Content>
