<%@ Page Title="Vendors/Service Providers" Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.Master.management_master_HRrecruitmentagent" CodeBehind="HRrecruitmentagent.aspx.cs" EnableEventValidation="false" %>
<%@ Register Src="~/OMS/Management/Master/UserControls/GSTINSettings.ascx" TagPrefix="GSTIN" TagName="gstinSettings" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/assests/pluggins/choosen/choosen.min.js"></script>
    <style>
        #EmployeeGrid_DXPagerBottom {
            min-width: 100% !important;
        }
           #lstReferedBy {
            width: 200px;
        }

        #lstReferedBy {
            display: none !important;
        }

        #lstReferedBy_chosen {
            width: 100% !important;
        }
         #lstTaxRates_MainAccount_chosen {
            width: 100% !important;
        }
        
    
        .floatedBtnArea {
            top:3px;
        }
    </style>
    <script src="../../../assests/pluggins/bootstrap-multiselect/bootstrap-multiselect.min.js"></script>
    
    <script src="Js/HRrecruitmentagent.js?v1=1.0"></script>
    <script src="../../../assests/pluggins/choosen/choosen.min.js"></script>
     <script src="Js/vendorPopup.js?var=2.0"></script>
   
    <script>
        function hideotherstatus() {
            //rev srijeeta  mantis issue 0024515[timeout changed from 400 to 10000000]
            setTimeout(function () { CASPxDirectCustomerCopyToVendorPopup.Show() }, 10000000);
            sessionStorage.setItem("popupcopyvendor", "");
            //document.getElementById("Trprofessionother").style.display = 'none';
        }
        function OnCopyToVendorclickClick(keyValue) {
            debugger;
            var url = 'HRrecruitmentagent.aspx?id=' + keyValue;
            //OnMoreInfoClick(url, 'Modify Agent Details', '940px', '450px', 'Y');
            window.location.href = url;

            sessionStorage.setItem("popupcopyvendor", "vendorcopy");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Vendors/Service Providers</h3>
        </div>

    </div>
    <div class="form_main">

        <table class="TableMain100">

            <tr>
                <td>
                    <table width="100%">
                        <tr>
                            <td style="text-align: left; vertical-align: top">
                                <table>
                                    <tr>
                                        <td id="ShowFilter">
                                            <% if (rights.CanAdd)
                                               { %>
                                            <a href="javascript:void(0);" onclick="OnAddButtonClick()" class="btn btn-success btn-radius"><span class="btn-icon"><i class="fa fa-plus" ></i></span> Add New </a>
                                            <%} %>
                                            <%--<a href="javascript:ShowHideFilter('s');" class="btn btn-primary"><span>Show Filter</span></a>--%>
                                            <% if (rights.CanExport)
                                               { %>
                                            <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-radius" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true">
                                                <asp:ListItem Value="0">Export to</asp:ListItem>
                                                <asp:ListItem Value="1">PDF</asp:ListItem>
                                                <asp:ListItem Value="2">XLS</asp:ListItem>
                                                <asp:ListItem Value="3">RTF</asp:ListItem>
                                                <asp:ListItem Value="4">CSV</asp:ListItem>

                                            </asp:DropDownList>
                                            <%} %>
                                        </td>
                                        <td id="Td1">
                                            <%--<a href="javascript:ShowHideFilter('All');" class="btn btn-primary"><span>All Records</span></a>--%>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td></td>
                            <td class="gridcellright pull-right">
                                <%--<dxe:ASPxComboBox ID="cmbExport" runat="server" AutoPostBack="true"
                                    Font-Bold="False" ForeColor="black" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged"
                                    ValueType="System.Int32" Width="130px">
                                    <Items>
                                        <dxe:ListEditItem Text="Select" Value="0" />
                                        <dxe:ListEditItem Text="PDF" Value="1" />
                                        <dxe:ListEditItem Text="XLS" Value="2" />
                                        <dxe:ListEditItem Text="RTF" Value="3" />
                                        <dxe:ListEditItem Text="CSV" Value="4" />
                                    </Items>
                                    <ButtonStyle>
                                    </ButtonStyle>
                                    <ItemStyle>
                                        <HoverStyle>
                                        </HoverStyle>
                                    </ItemStyle>
                                    <Border BorderColor="black" />
                                    <DropDownButton Text="Export">
                                    </DropDownButton>
                                </dxe:ASPxComboBox>--%>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="gridcellcenter relative">
                    
                </td>
            </tr>
        </table>
        <div class="clearfix relative">
            <dxe:ASPxGridView ID="EmployeeGrid" runat="server" KeyFieldName="cnt_Id" AutoGenerateColumns="False"
                        DataSourceID="EntityServerModeDataSource" Width="100%" ClientInstanceName="grid" OnCustomCallback="EmployeeGrid_CustomCallback"

                        SettingsDataSecurity-AllowDelete="false" Settings-HorizontalScrollBarMode="Auto" Settings-VerticalScrollableHeight="280" Settings-VerticalScrollBarMode="Auto" >
                        <ClientSideEvents EndCallback="function(s,e) { ShowError(s.cpInsertError); }" />
                        <SettingsSearchPanel Visible="True" Delay="5000" />
                        <%--<Settings ShowTitlePanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />--%>
                        <%--    <settingspager   pagesize="10" showseparators="True" alwaysshowpager="True">
                            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200"/>
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </settingspager>--%>
                        <SettingsEditing Mode="PopupEditForm" PopupEditFormHorizontalAlign="Center" PopupEditFormModal="True"
                            PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="900px" EditFormColumnCount="3" />

                        <Settings ShowGroupPanel="True" ShowFilterRow="true" ShowFilterRowMenu="true" />
                        <SettingsBehavior AllowFocusedRow="True" ConfirmDelete="True" ColumnResizeMode="NextColumn" />
                        <SettingsText PopupEditFormCaption="Add/ Modify Employee" />
                        <Columns>
                            <dxe:GridViewDataTextColumn FieldName="Name" ReadOnly="True" VisibleIndex="1" Width="280">
                                <CellStyle CssClass="gridcellleft" Wrap="True">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditCellStyle Wrap="True">
                                </EditCellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Unique_ID" Caption="Unique ID" VisibleIndex="0" Width="290">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Phone" FieldName="phone" VisibleIndex="2" Width="150">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Status" FieldName="Status" VisibleIndex="3" Width="60">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="GSTIN" FieldName="gstin" VisibleIndex="4" Width="160">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormCaptionStyle HorizontalAlign="left">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False" />
                            </dxe:GridViewDataTextColumn>
                            <%-- Rev Rajdip --%>
                            <dxe:GridViewDataTextColumn Caption="PAN" FieldName="Pan_NO" VisibleIndex="4" Width="160">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormCaptionStyle HorizontalAlign="left">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False" />
                            </dxe:GridViewDataTextColumn>
                               <dxe:GridViewDataTextColumn Caption="Tax Entity Type" FieldName="TaxEntityType" VisibleIndex="4" Width="180px">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormCaptionStyle HorizontalAlign="left">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False" />
                            </dxe:GridViewDataTextColumn>
                            <%-- End Rev Rajdip --%>
                            <dxe:GridViewDataTextColumn Caption="Account Group" FieldName="AccountGroup_Name" VisibleIndex="5" Width="180">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormCaptionStyle HorizontalAlign="left">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False" />
                            </dxe:GridViewDataTextColumn>


                            <dxe:GridViewDataTextColumn Caption="Name To Print In Cheque" FieldName="cnt_PrintNameToCheque" VisibleIndex="6" Width="190">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False" />
                            </dxe:GridViewDataTextColumn>


                            <dxe:GridViewDataTextColumn Caption="Created By" FieldName="CreatedBy" Width="200"
                                VisibleIndex="7">
                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>

                              <dxe:GridViewDataDateColumn Caption="Created On" SortOrder="Descending" FieldName="CreateDate" PropertiesDateEdit-DisplayFormatString="dd-MM-yyyy" Width="200"
                                VisibleIndex="8">
                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataDateColumn>
                             <dxe:GridViewDataTextColumn Caption="Last Modified By" FieldName="LastUpdatedBy" Width="200"
                                VisibleIndex="9">
                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="10" FieldName="LastUpdateOn" Settings-AllowAutoFilter="False"
                                Caption="Last Modified On" Width="200">
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                           

                            <%--<dxe:GridViewDataTextColumn Caption="Details" VisibleIndex="3" Width="5%">
                                <DataItemTemplate>
                                    <a href="javascript:void(0);" onclick="ClickOnMoreInfo('<%# Container.KeyValue %>')">More Info...</a>
                                </DataItemTemplate>
                                <CellStyle HorizontalAlign="Left" Wrap="False">
                                </CellStyle>
                                <EditFormSettings Visible="False" />
                                <HeaderStyle HorizontalAlign="Center" />

                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Cont.Person" VisibleIndex="4" Width="5%">
                                <DataItemTemplate>
                                    <a href="javascript:void(0);" onclick="OnContactInfoClick('<%#Eval("Id") %>','<%#Eval("Name") %>')">Show</a>
                                </DataItemTemplate>
                                <CellStyle HorizontalAlign="Left" Wrap="False">
                                </CellStyle>
                                <EditFormSettings Visible="False" />
                            </dxe:GridViewDataTextColumn>--%>
                            <dxe:GridViewDataTextColumn VisibleIndex="10" Width="0">
                                <DataItemTemplate>
                                    <div class='floatedBtnArea'>
                                    <% if (rights.CanEdit)
                                       { %>
                                    <a href="javascript:void(0);" onclick="ClickOnMoreInfo('<%# Container.KeyValue %>')" title="" class="" style="text-decoration: none;">
                                        <span class='ico ColorSix'><i class='fa fa-pencil'></i></span><span class='hidden-xs'>Edit</span>
                                    </a>
                                    <% 
                                       }


                                       if (rights.CanView)
                                       {
                                    %>
                                    <a href="javascript:void(0);" onclick="ClickVIewInfo('<%# Eval("Id") %>')" title="" class="" style="text-decoration: none;">
                                        <span class='ico ColorFive'><i class='fa fa-eye'></i></span><span class='hidden-xs'>View</span>
                                    </a>
                                    <%
                                           }
                                    %>
                                    <a href="javascript:void(0);" onclick="OnContactInfoClick('<%#Eval("Id") %>','<%#Eval("Name") %>')" title="" style="text-decoration: none; display: none">
                                        <span class='ico editColor'><i class='fa fa-eye' aria-hidden='true'></i></span><span class='hidden-xs'>Show</span>
                                    </a>
                                    <% if (rights.CanDelete)
                                       { %>
                                    <a href="javascript:void(0);" onclick="OnDelete('<%# Eval("Id") %>')" title="" class="">
                                        <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>Delete</span></a>
                                    <% } %>
                                              <% if (rights.CanAdd)
                                       { %>
                                    <a href="javascript:void(0);" onclick="OnCopyToVendorclickClick('<%# Container.KeyValue %>')" title="" class="" style="text-decoration: none;">
                                        <span class='ico editColor'><i class='fa fa-files-o' aria-hidden='true'></i></span><span class='hidden-xs'>Copy</span></a>
                                    </a>
                                    <% 
                                       }%>
                                     </div>
                                </DataItemTemplate>
                                <HeaderTemplate></HeaderTemplate>
                                <CellStyle HorizontalAlign="Center" Wrap="False">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <HeaderStyle HorizontalAlign="Center" />
                                <EditFormSettings Visible="False" />
                            </dxe:GridViewDataTextColumn>
                            <%-- <dxe:GridViewDataTextColumn Caption="Created User" FieldName="user_name" Visible="False"
                                VisibleIndex="7">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False" />
                            </dxe:GridViewDataTextColumn>--%>
                        </Columns>
                    <SettingsCookies Enabled="true" StorePaging="true" Version="1.0" />
                        <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                        <SettingsPager PageSize="10">
                            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                        </SettingsPager>
                        <ClientSideEvents RowClick="gridRowclick" />
                        <Styles>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                            </Header>
                        </Styles>
                    </dxe:ASPxGridView>
        </div>

        <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
            ContextTypeName="ERPDataClassesDataContext" TableName="v_VendorMasterList"/>

        <asp:SqlDataSource ID="EmployeeDataSource" runat="server" SelectCommand=""></asp:SqlDataSource>
        <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
        <br />



        <dxe:ASPxPopupControl ID="DirectAddCustPopup" runat="server"
            CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="AspxDirectAddCustPopup" Height="650px"
            Width="1020px" HeaderText="Add New Vendor" Modal="true" AllowResize="true" ResizingMode="Postponed">

            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>

        <asp:HiddenField ID="hidIsLigherContactPage" runat="server" />
    </div>

    <dxe:ASPxPopupControl ID="AspxDirectCustomerViewPopup" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="CAspxDirectCustomerViewPopup" Height="650px"
        Width="1020px" HeaderText="Vendor View" Modal="true" AllowResize="False">

        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>
                                <%--rev rajdip--%>
       <dxe:ASPxPopupControl ID="ASPxDirectCustomerCopyToVendorPopup" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="CASPxDirectCustomerCopyToVendorPopup" Height="370px"
        Width="1020px" HeaderText="Vendor Copy" Modal="true" AllowResize="false">
         
        <ContentCollection>

                <dxe:PopupControlContentControl runat="server">

                 <asp:Label ID="lblHeader" runat="server" Font-Bold="True" Font-Size="15px" ForeColor="Navy"
                        Width="819px" Height="18px"></asp:Label>

   <div class="row">
                                            <div class="col-md-3 hide">
                                                <label>
                                                    <dxe:ASPxLabel ID="ASPxLabel1" runat="server" Text="Salutation">
                                                    </dxe:ASPxLabel>
                                                </label>
                                                <div>
                                                    <asp:DropDownList ID="CmbSalutation" runat="server" Width="100%" TabIndex="1">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>


                                               <%--Chinmoy added 02-04-2020 start--%> 	
                                                     <div class="col-md-2 lblmTop8" id="ddl_Num" runat="server" style="display: none">	
                                                            <label>	
                                                                <dxe:ASPxLabel ID="lbl_NumberingScheme" Width="120px" runat="server" Text="Numbering Scheme">	
                                                                </dxe:ASPxLabel>	
                                                            </label>	
                                                            <asp:DropDownList ID="ddl_numberingScheme" runat="server" Width="100%">	
                                                              	
                                                            </asp:DropDownList>	
                                                        </div>	
                                          	
                                            	
                                             	
                                                        	
                                                        <div class="col-md-2 lblmTop8" runat="server" id="dvCustDocNo" style="display: none">	
                                                            <label>	
                                                                <dxe:ASPxLabel ID="lbl_CustDocNo" runat="server" Text="Unique ID" Width="">	
                                                                </dxe:ASPxLabel>	
                                                                <span style="color: red">*</span>	
                                                            </label>	
                                                            <dxe:ASPxTextBox ID="txt_CustDocNo" runat="server" ClientInstanceName="ctxt_CustDocNo" Width="100%" MaxLength="80">	
                                                                <ClientSideEvents TextChanged="function(s, e) {UniqueCodeCheck();}" />	
                                                            </dxe:ASPxTextBox>	
                                                            	
                                                        </div>	
                                                    <%-- End--%>	
                                                       


                                            <div class="col-md-3">
                                                <label>
                                                    <dxe:ASPxLabel ID="ASPxLabel5" runat="server" Text="Name">
                                                    </dxe:ASPxLabel>
                                                    <span style="color:red"> *</span>
                                                </label>
                                                <div style="position:relative">
                                                    <dxe:ASPxTextBox ID="txtFirstName" runat="server" Width="100%" TabIndex="2"  MaxLength="150" clientInstanceName="ctxtFirstName" CssClass="upper">
                                                       <%-- <ValidationSettings ValidationGroup="a">
                                                        </ValidationSettings>--%>
                                                    </dxe:ASPxTextBox> 
                                                   
                                                    <span id="MandatoryName" class="pullleftClass fa fa-exclamation-circle iconRed " style="color:red;display:none;position:absolute;    right: -20px;
    top: 5px;" title="Mandatory"></span>
                                                       
                                                    <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtFirstName"
                                                        Display="Dynamic" ErrorMessage="Mandatory." SetFocusOnError="True" ValidationGroup="a"
                                                         ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                                </div>
                                            </div>
                                           <div class="col-md-3" runat="server" id="dvUniqueId">
                                                <label><dxe:ASPxLabel ID="ASPxLabel4" runat="server" Text="Unique ID" >
                                                </dxe:ASPxLabel>
                                                    <span style="color:red"></span>
                                                </label>
                                                <div style="position:relative">
                                                    <dxe:ASPxTextBox ID="txtCode" runat="server" Width="100%" TabIndex="3" MaxLength="80" clientInstanceName="ctxtCode" CssClass="upper">
                                                        <clientsideevents lostfocus="function(s, e) {
	                                                            UniqueCheck();
                                                            }" />
                                                     </dxe:ASPxTextBox>
                                                    <span id="MandatoryShortName" class="pullleftClass fa fa-exclamation-circle iconRed " style="color:red;display:none;position:absolute;    right: -20px;
    top: 5px;" title="Mandatory"></span>

                                                    <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtCode"
                                                        Display="Dynamic" ErrorMessage="Mandatory."  ForeColor="Red" ValidationGroup="a"></asp:RequiredFieldValidator>--%>
                                                </div>
                                            </div>
                                            <div class="col-md-3" style="display:none;">
                                                 <label>&nbsp;<dxe:ASPxLabel ID="ASPxLabel10" runat="server" Text="Branch" Width="59px">
                                                    </dxe:ASPxLabel>
                                                 </label>
                                                <div>
                                                    <asp:DropDownList ID="cmbBranch" runat="server" Width="100%" TabIndex="4">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-md-3 hide">
                                                <label><dxe:ASPxLabel ID="ASPxLabel13" runat="server" Text="Source">
                                                </dxe:ASPxLabel>
                                                </label>
                                                <div>
                                                    <asp:DropDownList ID="cmbSource" runat="server" Width="100%" TabIndex="5" onchange="ChangeSource()">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label><dxe:ASPxLabel ID="ASPxLabel17" runat="server" Text="Referred By" Width="73px">
                                                </dxe:ASPxLabel>
                                                </label>
                                                <div>
                                                    <asp:TextBox ID="txtReferedBy" runat="server"  Width="100%" MaxLength="100" TabIndex="6"></asp:TextBox>
                                                  <%--  <asp:TextBox ID="txtReferedBy_hidden" runat="server" Visible="False"></asp:TextBox>--%>
                                                    <asp:ListBox ID="lstReferedBy" CssClass="chsn"   runat="server" Font-Size="12px" Width="100%" TabIndex="6"  data-placeholder="Select..."></asp:ListBox>
                                                      
                                                </div>
                                            </div>
                                                        <%-- rev srijeeta  mantis issue 0024515--%>
                                                         <div class="col-md-2 lblmTop8" runat="server" id="Div1" >	
                                                            <label>	
                                                                <dxe:ASPxLabel ID="ASPxLabel7" runat="server" Text="Alternative Code" Width="" maxLength="100">	
                                                                </dxe:ASPxLabel>	
                                                               
                                                            </label>	
                                                            <dxe:ASPxTextBox ID="alttext" runat="server" ClientInstanceName="ctxt_CustDocNoaltcode" Width="100%" MaxLength="100">	
                                                                	
                                                            </dxe:ASPxTextBox>	
                                                            	
                                                        </div>	
                                                         <%-- end of rev srijeeta  mantis issue 0024515--%>
                                            <div class="clear" ></div>
                                            <div class="col-md-4">
                                                <label><dxe:ASPxLabel ID="ASPxLabel19" runat="server" Text="Status" Width="95px">
                                                </dxe:ASPxLabel>
                                                </label>
                                                <div>
                                                    <asp:DropDownList ID="cmbContactStatus" runat="server" Width="100%" TabIndex="7">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                              

                                            <div class="col-md-4">
                                                <label>
                                                    <dxe:ASPxLabel ID="ASPxLabel20" runat="server" Text="Legal Status" Width="70px">
                                                    </dxe:ASPxLabel>
                                                </label>
                                                <div>
                                                    <asp:DropDownList ID="cmbLegalStatus" runat="server" Width="100%" TabIndex="8">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-md-4 lblmTop8">
                                                <label style="margin-top:0">
                                                    <dxe:ASPxLabel ID="ASPxLabel2" runat="server" Text="Date Of Incorporation">
                                                    </dxe:ASPxLabel>
                                                </label>
                                                <div>
                                                   <%-- <dxe:ASPxDateEdit ID="DateOfIncoorporation" runat="server" DisplayFormatString="yyyy-MM-dd" EditFormatString="yyyy-MM-dd"
                                                        TabIndex="9">
                                                    </dxe:ASPxDateEdit>  --%>   
                                                    <dxe:ASPxDateEdit ID="DateOfIncoorporation" runat="server" DisplayFormatString="dd-MM-yyyy"  EditFormatString="dd-MM-yyyy"
                                                        TabIndex="9"  clientInstanceName="cDateOfIncoorporation" Width="100%">                                       
                                                    </dxe:ASPxDateEdit>                                             
                                                </div>
                                            </div>
                                            <div class="clear"></div>
                                              <div class="col-md-3 lblmTop8">
                                                <div class="padBot5 lblmTop8" style="display: block;">
                                                    <span>Main Account</span> 
                                                </div>
                                                <div class="Left_Content"> 
                                                      <asp:ListBox ID="lstTaxRates_MainAccount" CssClass="chsn" runat="server" Font-Size="200px" Width="100%" data-placeholder="Select..." TabIndex="10"  onchange="changeFunc();"></asp:ListBox>
                                                    <asp:HiddenField ID="hndTaxRates_MainAccount_hidden" runat="server" />
                                                </div>
                                              </div>


                                          <div class="col-md-6" id="divBranch" style="display:block;">
                                            <label>
                                                Branch :<%--<span style="color: Red;">*</span>  --%>                                            
                                            </label>
                                            <div>
                                                 <div><asp:Label ID="lblSelectedBranch" runat="server"></asp:Label></div>  

                                                <dxe:ASPxComboBox ID="cmbMultiBranches" ClientInstanceName="CmbBranch" runat="server"   Visible="false"
                                                    ValueType="System.String" DataSourceID="branchdtl" ValueField="branch_id"
                                                    TextField="branch_description" EnableIncrementalFiltering="true"
                                                    Width="90%" AutoPostBack="false">
                                                    <ClientSideEvents SelectedIndexChanged="CmbBranchChanged" Init="CmbBranchChanged" />
                                                </dxe:ASPxComboBox>
                                                <input type="button" onclick="MultiBranchClick()" class="btn btn-small btn-primary" value="Select Branch(s)" id="MultiBranchButton" ></input>
                                            </div>                                          
                                        </div>
         <asp:SqlDataSource ID="branchdtl" runat="server" 
            SelectCommand="select '0' as branch_id ,  'Select' as branch_description union all   select branch_id,branch_description from tbl_master_branch order by branch_description"></asp:SqlDataSource>




                                               <div class="col-md-3 hide" id="divtxtTaxRates_SubAccount"  >
                                                    <div class="padBot5" style="display: block; height: auto;">
                                                        Sub Account
                                                    </div>
                                                    <div class="Left_Content"> 
                                                        <asp:ListBox ID="lstTaxRates_SubAccount" CssClass="chsn" runat="server" Font-Size="12px" Width="100%" data-placeholder="Select..." onchange="changeSubFunc();"></asp:ListBox>
                                                        <asp:HiddenField ID="hndTaxRates_SubAccount_hidden" runat="server" />
                                                    </div>
                                                </div>

                                              <div class="clear"></div>
                                            <%--Debjyoti GSTIN in Vendor--%>
                                             <div id="td_registered" class="labelt col-md-3" runat="server">
                                                 <label>Registered?</label>
                                                    <div class="visF">

                                                                
                                                        <asp:RadioButtonList runat="server" ID="radioregistercheck" RepeatDirection="Horizontal" Width="130px">
                                                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="No" Value="0"  Selected="True"></asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </div>
                                                </div>
                                                <div class="col-md-6 lblmTop8">
                                                <label >GSTIN   </label>
                                                <div class="relative"> 
                                                      <ul class="nestedinput">
                                                        <li>
                                                            <dxe:ASPxTextBox ID="txtGSTIN1" ClientInstanceName="ctxtGSTIN111"  MaxLength="2"  TabIndex="10"  runat="server" Width="50px">
                                                              <ClientSideEvents KeyPress="function(s,e){ fn_AllowonlyNumeric(s,e);}" />
                                                            </dxe:ASPxTextBox>
                                                        </li>
                                                        <li class="dash"> - </li>
                                                        <li>
                                                            <dxe:ASPxTextBox ID="txtGSTIN2" ClientInstanceName="ctxtGSTIN222"  MaxLength="10"  TabIndex="11"  runat="server" Width="150px"> 
                                                          <ClientSideEvents KeyUp="Gstin2TextChanged" />
                                                                   </dxe:ASPxTextBox>
                                                        </li>
                                                        <li class="dash"> - </li>
                                                        <li>
                                                            <dxe:ASPxTextBox ID="txtGSTIN3" ClientInstanceName="ctxtGSTIN333"  MaxLength="3"  TabIndex="12"  runat="server" Width="50px"> 
                                                            </dxe:ASPxTextBox>
                                                            <span id="invalidGst" class="pullleftClass fa fa-exclamation-circle iconRed " style="color:red;display:none;padding-left: 9px;left:304px" title="Invalid GSTIN"></span>
                                                        </li>
                                                    </ul>  
                                                      <a href="#" onclick="validateGSTIN();"   style="padding-left:10px" >Validate GST</a>  
                                                    <input  class="hide" id="myInput" />
                                                 </div>
                                                 </div>
                                          <div class="col-md-3">
                                                 <label style="margin-top: 7px;">PAN
                                                    <a href="#"  style="left: -12px; top: 20px;"> 
                                                <i id="I1" runat="server" class="fa fa-question" aria-hidden="true"  onclick="AboutPanClick()"></i>                                              
                                                             </a>
                                                        </label>
                                                <div style="position:relative">
                                                    <dxe:ASPxTextBox ID="txtNumber" runat="server" Width="100%" TabIndex="9"  MaxLength="150" clientInstanceName="ctxtNumber" CssClass="upper">
                                                       <%-- <ValidationSettings ValidationGroup="a">
                                                        </ValidationSettings>--%>
                                                    </dxe:ASPxTextBox> 
                                                   
                                                    <span id="Mandatorypan" class="pullleftClass fa fa-exclamation-circle iconRed " style="color:red;display:none;position:absolute;    right: -20px;
                                                     top: 5px;" title="Mandatory"></span>
                                                       
                                                    <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtFirstName"
                                                        Display="Dynamic" ErrorMessage="Mandatory." SetFocusOnError="True" ValidationGroup="a"
                                                         ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                                </div>
                                                  <dxe:ASPxLabel ID="labelformat" Style="color: #03A9F4;width: 100px;width: 100%;font-size: 11px;font-weight: 600;display: inline-block;" Width="100px" runat="server" ForeColor="#0099FF" CssClass="formatcss" ClientInstanceName="lbformat" Text="Sample : AAAAA9999A"></dxe:ASPxLabel>
                                         
                                            </div>
                                                <div class="clear"></div>
                                                <div class="col-md-3 visF" id="td_Applicablefrom" > 
                                                        <label class="labelt">
                                                            <dxe:ASPxLabel ID="lbl_Applicablefrom" runat="server" Text="Applicable From">
                                                            </dxe:ASPxLabel>
                                                        </label>
                                                        <div class="visF">
                                                            <dxe:ASPxDateEdit ID="dt_ApplicableFrom" TabIndex="13" runat="server" Width="100%" EditFormat="Custom"  ClientInstanceName="cApplicableFrom"
                                                                EditFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                                                                <ButtonStyle Width="13px">
                                                                </ButtonStyle>
                                                            </dxe:ASPxDateEdit>

                                                        </div>
                                                </div>
                                            <%--Debjyoti GSTIN in Vendor--%>
                                            <div class="col-md-3">
                                                <label><dxe:ASPxLabel ID="ASPxLabel3" runat="server" Text="Name To Print in Cheque" Width="273px">
                                                </dxe:ASPxLabel>
                                                </label>
                                                <div>
                                                    <asp:TextBox ID="txtNameInCheque" runat="server"  Width="100%" MaxLength="200"  TabIndex="14"></asp:TextBox>
                                                 
                                                      
                                                </div>
                                            </div>
                                            
                                             <div class="col-md-3 " >
                                                <label>
                                                    <dxe:ASPxLabel ID="ASPxLabel6" runat="server" Text="Accounts Group">
                                                    </dxe:ASPxLabel>
                                                </label>
                                                <div>
                                                    <asp:DropDownList ID="ddlAssetLiability" runat="server" Width="100%" TabIndex="1">
                                                    </asp:DropDownList>
                                            </div>
                                        </div>
                                             <div  class="labelt col-md-3" >
                                                            <div id="DivVendorType" class="visF hide">

                                                                <label>Vendor Type</label>
                                                                <asp:RadioButtonList runat="server" ID="rdl_VendorType" RepeatDirection="Horizontal" Width="210px">
                                                                    <asp:ListItem Text="Regular" Value="R" ></asp:ListItem>
                                                                    <asp:ListItem Text="Composition" Value="C"  ></asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </div>
                                                </div>
                                               <div  class="labelt col-md-3" >
                                                       <div style="padding-top: 30px;"> <%--<input type="checkbox" id="chkcopy" />--%>
                                                           <asp:CheckBox id="chkcopy" runat="server" />
                                                           <label id="lblCopyAll">Copy All To Vendor</label>
                                                       </div>
                                               </div>
                                            <div class="clear"></div>
                                            <div class="col-md-12" style="padding-top:15px">
                                                <dxe:ASPxButton ID="btnSave" runat="server" Text="Save" ValidationGroup="a" TabIndex="13"
                                                    OnClick="btnSave_Click"  CssClass="btn btn-primary">
                                                    <clientsideevents Click="function(s, e) {
                                                      
                                                                if(document.getElementById('lstReferedBy').value){
                                                                document.getElementById('RefferedByValue').value= document.getElementById('lstReferedBy').value;
                                                                }
                                                        
	                                                            var valid= validate();
                                                                e.processOnServer = valid;
                                                       
                                                                ValidatePanno();
                                                        }" />
                                                    <%--  hidepopup(); --%>
                                                  <%--  <ClientSideEvents EndCallback="branchGridEndCallBack" />--%>
                                                </dxe:ASPxButton>
                                                     <asp:Button ID="btnUdf" runat="server" Text="UDF"  CssClass="btn btn-primary dxbButton"  OnClientClick="if(OpenUdf()){ return false;}"/>

                                               <%-- <asp:Button ID="GstinSettingsButton" runat="server" Text="GSTIN Settings Branchwise"  CssClass="btn btn-primary dxbButton"  OnClientClick="openGstin();return false;"/>--%>
                                                <GSTIN:gstinSettings runat="server" ID="GstinSettingsButton" />
                                            </div>
                                            
                                        </div>

                  <asp:HiddenField ID="hddnGSTIN2Val" runat="server" />
                    <asp:HiddenField ID="HdId" runat="server" />
                     <asp:HiddenField ID="refferByDD" runat="server" />
        <asp:HiddenField ID="hddnGSTINFlag" runat="server" />
                    <asp:HiddenField ID="RefferedByValue" runat="server" />
                    <asp:HiddenField ID="hddnApplicationMode" runat="server" />
                    <asp:HiddenField ID="hdnflag" runat="server" />
                            <asp:HiddenField ID="hdnBranchAllSelected" runat="server" />
                    <asp:HiddenField ID="hdnoldid" runat="server" />
                     <asp:HiddenField ID="hdnflg" runat="server" />
                    <asp:HiddenField ID="hdKeyVal_InternalID" runat="server" />
                    <asp:HiddenField runat="server" ID="Keyval_internalId" />
                    <asp:HiddenField ID="hdIsMainAccountInUse" runat="server" />
                     <asp:HiddenField ID="hdnAutoNumStg" runat="server" />	
        <asp:HiddenField ID="hdnTransactionType" runat="server" />	
                      <asp:HiddenField ID="hddnDocNo" runat="server" />	
                      <asp:HiddenField ID="hdnNumberingId" runat="server" />
                    </dxe:PopupControlContentControl>

        </ContentCollection>
    </dxe:ASPxPopupControl>
     <dxe:ASPxPopupControl ID="BranchSelectPopup" runat="server" Width="700"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="cBranchSelectPopup"
        HeaderText="Select Branch" AllowResize="false" ResizingMode="Postponed" Modal="true">
        <contentcollection>
            <dxe:PopupControlContentControl runat="server">

                 <div style="margin-bottom:10px;margin-top:10px;"> Apply for All Branch &nbsp; 
                     <asp:CheckBox ID="chkAllBranch" runat="server" OnClick="SelectAllBranches(this);" />
                     <asp:Label ID="lblBranch"  runat="server" Text="All Branch Selected, No need to select individual Branch"  CssClass="vehiclecls"></asp:Label>
                 </div>



                <dxe:ASPxGridView ID="branchGrid" runat="server" KeyFieldName="branch_id" AutoGenerateColumns="False" DataSourceID="BranchdataSource"
                    Width="100%" ClientInstanceName="cbranchGrid" OnCustomCallback="branchGrid_CustomCallback"
                    SelectionMode="Multiple" SettingsBehavior-AllowFocusedRow="true">
                    <Columns>

                        <dxe:GridViewCommandColumn ShowSelectCheckbox="true" VisibleIndex="0" Width="60" Caption="Select" />


                        <dxe:GridViewDataTextColumn Caption="Branch Code" FieldName="branch_code"
                            VisibleIndex="1" FixedStyle="Left">
                            <CellStyle CssClass="gridcellleft" Wrap="true">
                            </CellStyle>
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Branch Description" FieldName="branch_description"
                            VisibleIndex="1" FixedStyle="Left">
                            <CellStyle CssClass="gridcellleft" Wrap="true">
                            </CellStyle>
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>
                    </Columns>
                    
                    <SettingsPager NumericButtonCount="20" PageSize="10" ShowSeparators="True" Mode="ShowPager">
                        <FirstPageButton Visible="True">
                        </FirstPageButton>
                        <LastPageButton Visible="True">
                        </LastPageButton>
                    </SettingsPager>
                    <SettingsSearchPanel Visible="True" />
                    <Settings ShowGroupPanel="False" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                    <SettingsLoadingPanel Text="Please Wait..." />
                    <ClientSideEvents EndCallback="branchGridEndCallBack" />
                </dxe:ASPxGridView>

                        <asp:SqlDataSource ID="BranchdataSource" runat="server"  SelectCommand="select branch_id,branch_code,branch_description from tbl_master_branch"></asp:SqlDataSource>




                <br />
                <input type="button" value="Ok" class="btn btn-primary" onclick="SaveSelectedBranch()" />
                <div style="float:right;">
                <input type="button" runat="server"  value="Select All" onclick="selectAll()" />
                 <input type="button" runat="server"    value="Deselect All" onclick="unselectAll()" />
                </div>
            </dxe:PopupControlContentControl>
        </contentcollection>
    </dxe:ASPxPopupControl>
     <%-- Pan No Details --%>
 


       <dxe:ASPxPopupControl ID="popuppan" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="cpopuppan" Height="150px"
        Width="980px" HeaderText="PAN Format" Modal="true" AllowResize="false">
         
        <ContentCollection>


                <dxe:PopupControlContentControl runat="server">


                               <div>
              <div>
                  <font color="red">
                               <h5>1.First three characters are alphabetic series running from AAA to ZZZ</h5><br />
                               <h5>2.Fourth character of PAN represents the status of the PAN holder.C  Company P  Person H  HUF(Hindu Undivided Family) </h5><br />
                              <h5> 3.F  Firm A  Association of Persons (AOP) T  AOP (Trust) B  Body of Individuals (BOI) L  Local Authority J  Artificial Juridical Person G  Government</h5><br />
                               <h5>4.Fifth character represents the first character of the PAN holders last name/surname.</h5><br />
                               <h5>5.Next four characters are sequential number running from 0001 to 9999.</h5><br />
                               <h5>6.Last character in the PAN is an alphabetic check digit.</h5><br />
                    </font>
                    </div>
                </div> 



                    </dxe:PopupControlContentControl>
       

     


                    </ContentCollection>
           </dxe:ASPxPopupControl>

    <%-- End Pan No Details --%>
    <%--end rev rajdip--%>
</asp:Content>
