<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="ServiceCenter.aspx.cs" Inherits="ERP.OMS.Management.Master.ServiceCenter" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <title></title>
     <script src="/assests/pluggins/choosen/choosen.min.js"></script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            var MaxLength = 150;
            $('#txtRemarks').keypress(function (e) {
                if ($(this).val().length >= MaxLength) {
                    e.preventDefault();
                }
            });
        });
        //Code for UDF Control 
        function OpenUdf() {
            if (document.getElementById('IsUdfpresent').value == '0') {
                jAlert("UDF not define.");
            }
            else {
                var url = 'frm_BranchUdfPopUp.aspx?Type=de';
                popup.SetContentUrl(url);
                popup.Show();
            }
            return true;
        }
        // End Udf Code

        function popUpRedirect(obj) {
            jAlert('Saved successfully');
            window.location.href = obj;


        }


        var pinCodeWithAreaId = [];
      

        function ClientSaveClick() {
            Page_ClientValidate();
            if (document.getElementById('txtCode').value.trim() == '') {
                return false;
            }
            if (document.getElementById('txtdrivername').value.trim() == '') {
                return false;
            }
            return true;
        }



        function ListBind() {

            var config = {
                '.chsn': {},
                '.chsn-deselect': { allow_single_deselect: true },
                '.chsn-no-single': { disable_search_threshold: 10 },
                '.chsn-no-results': { no_results_text: 'Oops, nothing found!' },
                '.chsn-width': { width: "100%" }
            }
            for (var selector in config) {
                $(selector).chosen(config[selector]);
            }

        }
        function lstCountry() {
            $('#lstCountry').fadeIn();
        }
        
        function disp_prompt(name) {

           
            if (name == "tab1") {
               
               // document.location.href = "Driver_Correspondence.aspx";
            }
            else if (name == "tab2") {
               
               // document.location.href = "Driver_document.aspx";
            }


        }

        function Close() {
            editwin.close();
        }

       



        function DeleteRow(keyValue) {
            doIt = confirm('Confirm delete?');
            if (doIt) {
                grid.PerformCallback('Delete~' + keyValue);
                
            }
            else {

            }
        }


        function ShowHideFilter1(obj) {

            gridTerminal.PerformCallback(obj);
        }
       
        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }
        function setvaluetovariable(obj1) {
            combo1.PerformCallback(obj1);
        }
        function setvaluetovariable1(obj1) {
            combo2.PerformCallback(obj1);
        }
        function CallList(obj1, obj2, obj3) {
           
            if (obj1.value == "") {
                obj1.value = "%";
            }
            var obj5 = '';
            ajax_showOptionsTEST(obj1, obj2, obj3, obj5);
            if (obj1.value == "%") {
                obj1.value = "";
            }
        }
        function hide_show(obj) {
            if (obj == 'All') {
                document.getElementById("client_pro").style.display = "none";
            }

        }
        function GetClick() {
            btnC.PerformCallback();
        }
        function Page_Load() {
            document.getElementById("TdCombo").style.display = "none";
        }
        
        function CheckLengthNote1() {
            var textbox = document.getElementById('txtRemarks').value;
            if (textbox.trim().length >= 150) {
                return false;
            }
            else {
                return true;
            }
        }
        function CheckingTD(obj) {

            var gridstat = gridTerminal.cpCompCombo;
            if (gridstat == 'anew')
                combo.SetFocus();

           
        }
        FieldName = "cmbExport_DDDWS";
        function LastCall(obj) {
            //height();
        }
        // Code Added By Priti on 21122016 to check Unique Short Name
        function fn_ctxtPro_Name_TextChanged() {
            var finCd = 0;
            if (GetObjectID('hiddenedit').value != '') {
                finCd = GetObjectID('hiddenedit').value;
            }

            var ShortName = document.getElementById("txtCode").value.trim();
            var qString = window.location.href.split("=")[1];
            $.ajax({
                type: "POST",
                url: "DriverAddEdit.aspx/CheckUniqueName",
                data: JSON.stringify({ ShortName: ShortName, divId: finCd }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var data = msg.d;

                    if (data == true) {
                        jAlert("Please Enter Unique Vehicle No");
                        document.getElementById("txtCode").value = '';
                        document.getElementById("txtCode").focus();
                        return false;
                    }
                }
            });
        }
        </script>
          <style type="text/css">
       
        .abs {
            position:absolute;
            right: -19px;
            top: 4px;
        }
          .abs1 {
            position:absolute;
            right: -19px;
            top: 4px;
        }
     
         .chosen-container.chosen-container-single {
            width:100% !important;
        }
        .chosen-choices {
            width:100% !important;
        }
        #lstCountry,#lstState,#lstCity,#lstArea,#lstPin,#lstBranchHead {
            width:100%;
        }
        #lstCountry,#lstState,#lstCity,#lstArea,#lstPin,#lstBranchHead {
            display:none !important;
            
        }
        #lstCountry_chosen,#lstState_chosen,#lstCity_chosen,#lstArea_chosen,#lstPin_chosen,#lstBranchHead_chosen{
            width:100% !important;
        }
        #PageControl1_CC {
        overflow:visible !important;
        }
        #lstState_chosen, #lstCountry_chosen, #lstCity_chosen,#lstPin_chosen,#lstBranchHead_chosen {
            margin-bottom:5px;
        }
              .divControlClass>span.controlClass {
                  margin-top:8px;
              }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
        
            <h3><asp:Label ID="lblAddEdit" runat="server" Text=""></asp:Label></h3>
            <div class="crossBtn"><a href="ServiceCenterList.aspx"><i class="fa fa-times"></i></a></div>
        </div>
    </div>

     <%--debjyoti 22-12-2016--%>
                                        <dxe:ASPxPopupControl ID="ASPXPopupControl" runat="server" 
                                            CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"  ClientInstanceName="popup" Height="630px"
                                            Width="600px" HeaderText="Add/Modify UDF" Modal="true" AllowResize="true" ResizingMode="Postponed" >
                                            <ContentCollection>
                                                <dxe:PopupControlContentControl runat="server">
                                                </dxe:PopupControlContentControl>
                                            </ContentCollection>
                                        </dxe:ASPxPopupControl>

    <asp:HiddenField runat="server" ID="IsUdfpresent" />
     <asp:HiddenField runat="server" ID="hiddenedit" />
                                        <%--End debjyoti 22-12-2016--%>
    <div class="form_main">
        <table style="width: 100%">
            <tr>
                <td style="width: 100%">
                    <dxe:ASPxPageControl ID="PageControl1" runat="server" Width="100%" ActiveTabIndex="0"
                        ClientInstanceName="page">
                        <TabPages>
                            <dxe:TabPage Text="General">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                        <div class="totalWrap">
                                          
                                            <div class="col-md-3">
                                                <label>Center Code <span style="color:red">*</span></label>
                                                <div class="relative">
                                                    <asp:TextBox ID="txtCode" runat="server"  ClientIDMode="Static" Width="100%" 
                                                        MaxLength="80" onblur="fn_ctxtPro_Name_TextChanged()">
                                                                   
                                                    </asp:TextBox>

                                                    <asp:RequiredFieldValidator Display="Dynamic" runat="server" ID="RequiredFieldValidator2" ControlToValidate="txtCode"
                                                        SetFocusOnError="true" ErrorMessage="" class="pullrightClass fa fa-exclamation-circle abs iconRed" ToolTip="Mandatory" ValidationGroup="drivegrp" >                                                        
                                                    </asp:RequiredFieldValidator>
                                                 </div>
                                            </div>
                                              <div class="col-md-3">
                                                <label>Service Center Name <span style="color:red">*</span></label>
                                                <div class="relative">
                                                    <asp:TextBox ID="txtCentername" runat="server"  ClientIDMode="Static" Width="100%" 
                                                        MaxLength="150" >
                                                                   
                                                    </asp:TextBox>

                                                      <asp:RequiredFieldValidator Display="Dynamic" runat="server" ID="RequiredFieldValidator1" ControlToValidate="txtCentername"
                                                        SetFocusOnError="true" ErrorMessage="" class="pullrightClass fa fa-exclamation-circle abs iconRed" ToolTip="Mandatory" ValidationGroup="drivegrp" >                                                        
                                                    </asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>Branch </label>
                                                <div class="relative">
                                                    <asp:DropDownList ID="cmbParentBranch" runat="server" Width="100%"
                                                        >
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                     
                                            
                                            
                                              <div class="col-md-3">
                <label>Is Active?</label>
                <div>
                    <dxe:ASPxCheckBox ID="chkIsActive" runat="server" Text="">
                    </dxe:ASPxCheckBox>
                </div>
            </div><div class="clear"></div>
                                            <div class="col-md-6">
                                                <label>Remarks </label>
                                                <div class="relative">
                                                    <asp:TextBox ID="txtRemarks" runat="server" Width="100%"  TextMode="MultiLine" MaxLength="150"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="clear"></div>
                                            <div class="col-md-12" style="padding-top: 10px;">
                                                <%-- <% if (rights.CanAdd || rights.CanEdit)
                                                    { %>--%>
                                                <asp:Button ID="btnSave" runat="server" Text="Save"  ValidationGroup="drivegrp" CssClass="btn btn-primary dxbButton" OnClick="btnSave_Click" OnClientClick="return ClientSaveClick()"
                                                    />
                                                <asp:Button ID="Button2" runat="server" Text="Cancel" CssClass="btn btn-danger dxbButton" OnClick="Button2_Click"
                                               />
                                              <%--  <input type="button" id="btnUdf" value="UDF" class="btn btn-primary dxbButton" onclick="OpenUdf()" />--%>

                                                <asp:Button ID="btnUdf" runat="server" Text="UDF"  CssClass="btn btn-primary dxbButton" style="display:none;" OnClientClick="if(OpenUdf()){ return false;}"
                                                     />

                                                <%--  <% } %>--%>
                                            </div>
                                        </div>

                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            
                             <dxe:TabPage Name="Correspondence" Text="Correspondence" Visible="false">

                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Documents" Text="Documents" Visible="false">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>

                             <dxe:TabPage Name="UDF" Text="UDF" Visible="false">
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
	                                                if(activeTab == Tab0)
	                                                {
	                                                    disp_prompt('tab0');
	                                                }
	                                               else  if(activeTab == Tab1)
	                                                {
	                                                    disp_prompt('tab1');
	                                                }
	                                                else if(activeTab == Tab2)
	                                                {
	                                                    disp_prompt('tab2');
	                                                }

                                             
                                             
	                                            
	                                            }"></ClientSideEvents>
                    </dxe:ASPxPageControl>

                   <%-- <asp:SqlDataSource ID="sqlCompany" runat="server" 
                        SelectCommand="select cmp_internalId,cmp_name from tbl_master_company"></asp:SqlDataSource>--%>

                  <%--  <asp:SqlDataSource ID="SqlExchange" runat="server" 
                        SelectCommand="select exch_internalId,(select exh_shortName from tbl_master_exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+ exch_segmentId as Exchange from tbl_master_companyExchange where exch_compId=@CompID">
                        <SelectParameters>
                            <asp:SessionParameter Name="CompID" SessionField="ID" Type="string" />
                        </SelectParameters>
                    </asp:SqlDataSource>--%>

                   <%-- <asp:SqlDataSource ID="SqlParentTerminal" runat="server" 
                        SelectCommand="select distinct TradingTerminal_TerminalID from Master_TradingTerminal"></asp:SqlDataSource>--%>

                    <%--<asp:SqlDataSource ID="SqlVendor" runat="server" 
                        SelectCommand="select CTCLVendor_ID,CTCLVendor_Name+' ['+CTCLVendor_ProductType+']' as CTCLVendor_Name from Master_CTCLVendor where CTCLVendor_ExchangeSegment=@CompID1">
                        <SelectParameters>
                            <asp:SessionParameter Name="CompID1" SessionField="ID1" Type="string" />
                        </SelectParameters>
                    </asp:SqlDataSource>--%>

                    <%--<asp:SqlDataSource ID="TrdTerminal" runat="server"
                        SelectCommand="select td.TradingTerminal_ID,e.exh_shortName+'-'+ce.exch_segmentId as Exchange,td.TradingTerminal_TerminalID,td.TradingTerminal_ParentTerminalID,td.TradingTerminal_ProTradeID,td.TradingTerminal_brokerid ,(select isnull(cnt_firstName,'') +' '+isnull(cnt_middleName,'')+' '+isnull(cnt_lastName,'')+ ' [' + isnull(ltrim(rtrim(cnt_UCC)),'') + '] ' from tbl_master_contact where cnt_internalId=td.TradingTerminal_BrokerID) as brokid,(Select  ISNULL(ltrim(rtrim(cnt_firstName)), '') + ' ' + ISNULL(ltrim(rtrim(cnt_middleName)), '')   + ' ' + ISNULL(ltrim(rtrim(cnt_lastName)), '') + ' [' + isnull(ltrim(rtrim(cnt_UCC)),'') + '] ' AS cnt_firstName from tbl_master_contact WHERE  cnt_internalid =td.TradingTerminal_ProTradeID) as ProTradeID,(Select  ISNULL(ltrim(rtrim(cnt_firstName)), '') + ' ' + ISNULL(ltrim(rtrim(cnt_middleName)), '')   + ' ' + ISNULL(ltrim(rtrim(cnt_lastName)), '') + ' [' + isnull(ltrim(rtrim(cnt_UCC)),'') + '] ' AS cnt_firstName from tbl_master_contact WHERE  cnt_internalid =td.TradingTerminal_CliTradeID) as CliTradeID,(Select  ISNULL(ltrim(rtrim(cnt_firstName)), '') + ' ' + ISNULL(ltrim(rtrim(cnt_middleName)), '')   + ' ' + ISNULL(ltrim(rtrim(cnt_lastName)), '') + ' [' + isnull(ltrim(rtrim(cnt_UCC)),'') + '] ' AS cnt_firstName from tbl_master_contact WHERE  cnt_internalid =td.TradingTerminal_AllTradeID) as AllTradeID from  Master_TradingTerminal td, tbl_master_exchange e, tbl_master_companyExchange ce where td.TradingTerminal_CompanyID=ce.exch_compId  and td.TradingTerminal_ExchangeSegmentID=ce.exch_InternalId and  e.exh_cntId=ce.exch_exchId and td.TradingTerminal_BranchID=@BranchID order by TradingTerminal_ID desc"
                        DeleteCommand="delete from Master_TradingTerminal where TradingTerminal_ID=@TradingTerminal_ID">
                        <SelectParameters>
                            <asp:SessionParameter Name="BranchID" SessionField="KeyVal_InternalID" Type="string" />
                        </SelectParameters>
                        <DeleteParameters>
                            <asp:Parameter Name="TradingTerminal_ID" Type="Int32" />
                        </DeleteParameters>
                    </asp:SqlDataSource>--%>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
