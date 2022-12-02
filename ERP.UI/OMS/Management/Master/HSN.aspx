<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="HSN.aspx.cs" Inherits="ERP.OMS.Management.Master.HSN" %>


    <asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/assests/pluggins/choosen/choosen.min.js"></script>
    <script type="text/javascript">
        
        function fn_EditProfession(keyValue) {
            //document.getElementById("profreq").style.visibility = "hidden";
            grid.PerformCallback('BeforeTaxMapping~' + keyValue);
            cPopupProfession.SetHeaderText("Modify Tax Mapping");
           
        }


        function AddNewGroup() {         
            cPopUp_groupMaster.SetHeaderText('Add HSN');
            $('#MandatoryCode').css({ 'display': 'none' });
            $('#MandatoryDesc').css({ 'display': 'none' });
            Status = 'SAVE_NEW';
            document.getElementById('txtCode').value = '';
            document.getElementById('txtDescription').value = '';
            cPopUp_groupMaster.Show();
        }
        function Call_save() {
            if (validate()) {
                grid.PerformCallback(Status);
            }
        }
        

        function btnSave_Profession() {

            //if (ctxtProfession.GetText() == '') {
            //    //alert('Please Enter Profession Name');
            //    document.getElementById("profreq").style.visibility = "visible";
            //    ctxtProfession.Focus();

            //}
            //else {
            //    var hfpid = chfProfId.Get('hfProfId');
            //    if (hfpid == '') {
            //        document.getElementById("profreq").style.visibility = "hidden";
            //  alert('Hi');
            var PurchaseCGST = CcmbCGST.GetValue();
            var PurchaseSGST = CcmbSGST.GetValue();
            var PurchaseIGST = CcmbIGST.GetValue();
            var SaleCGST = CcmbSaleCGST.GetValue();
            var SaleSGST = CcmbSaleSGST.GetValue();
            var SaleIGST = CcmbSaleIGST.GetValue();

            var result;

            $.ajax({
                type: "POST",
                url: "HSN.aspx/ChecktaxRateForHSN",
                data: JSON.stringify({ PurchaseCGST: PurchaseCGST, PurchaseSGST: PurchaseSGST, PurchaseIGST: PurchaseIGST, SaleCGST: SaleCGST, SaleSGST: SaleSGST, SaleIGST: SaleIGST }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async:false,
                success: function (msg) {
                     result = msg.d;
                   
                }
            });

            var hsn = document.getElementById('txtHsnCode1').value;
            if (result == "FALSE") {


                jConfirm('CGST and SGST tax rate are mismatch with IGST tax rate. Proceed?', 'Confirmation Dialog', function (r) {

                    if(r==true)
                    {
                        
                        grid.PerformCallback('SaveTaxMap~' + hsn);
                    }

                });
               
            }
            else
            {
               
                 grid.PerformCallback('SaveTaxMap~' + hsn);
            }
            //grid.PerformCallback('SaveTaxMap~' + ctxtProfession.GetText());
            //    }
            //    else {
            //        document.getElementById("profreq").style.visibility = "hidden";
            //        grid.PerformCallback('UpdateProfession~' + chfProfId.Get('hfProfId'));
            //    }
            //}


        }



        function validate() {

            var desc = document.getElementById('txtCode').value;
            var code = document.getElementById('txtDescription').value;
            
            var returnVal = true;
            if (code.trim() == '') {
                $('#MandatoryCode').css({ 'display': 'block' });
                returnVal = false;
            }
            else {
                $('#MandatoryCode').css({ 'display': 'none' });
            }

            if (desc.trim() == '') {
                $('#MandatoryDesc').css({ 'display': 'block' });
                returnVal = false;
            }
            else {
                $('#MandatoryDesc').css({ 'display': 'none' });
            }
           
           



            return returnVal;
        }

        function fn_btnProfession() {
           
            cPopupProfession.Hide();
        }

        function LastCall() {
            if (grid.cpBeforeTaxMappingJson != null) {               
                if (grid.cpBeforeTaxMappingJson != '') {
                    var jsonData = JSON.parse(grid.cpBeforeTaxMappingJson);                   
                    cPopupProfession.Show();
                    document.getElementById('txtHsnCode1').value = jsonData.HsnCode;
                  
                    CcmbCGST.SetValue(jsonData.hsnPurCGST);                    
                    CcmbSGST.SetValue(jsonData.hsnPurSGST);
                    CcmbIGST.SetValue(jsonData.hsnPurIGST);
                    CcmbUTGST.SetValue(jsonData.hsnPurUTGST);
                    CcmbSaleCGST.SetValue(jsonData.hsnSaleCGST);                    
                    CcmbSaleSGST.SetValue(jsonData.hsnSaleSGST);
                    CcmbSaleIGST.SetValue(jsonData.hsnSaleIGST);
                    CcmbSaleUTGST.SetValue(jsonData.hsnSaleUTGST);
                    CcmbSacType.SetValue(jsonData.hsnType);

                    grid.cpBeforeTaxMappingJson = '';
                }
                
            }



            if (grid.cpMsg != null) {
                if (grid.cpMsg != '') {
                    jAlert(grid.cpMsg);
                    grid.cpMsg = null;
                }
            }

            if (grid.cpHide != null) {
                if (grid.cpHide == 'Y') {
                    grid.cpHide = null;
                    cPopupProfession.Hide();
                    cPopUp_groupMaster.Hide();
                }                   
            }

          
            if (grid.cpEditJson != null) {
              //  alert('test1');
                if (grid.cpEditJson.trim() != '') {
                    //alert(grid.cpEditJson.replace(/\n/g, ''));

                 
                    var jsonData = JSON.parse(grid.cpEditJson);
                   //alert(jsonData.Code);
                    document.getElementById('txtCode').value = jsonData.Code;
                    document.getElementById('txtDescription').value = jsonData.Description;
                    grid.cpEditJson = '';
                   
                }
            }

        }

        function OnEdit(obj) {
           
            cPopUp_groupMaster.SetHeaderText('Modify HSN');
            $('#MandatoryCode').css({ 'display': 'none' });
            $('#MandatoryDesc').css({ 'display': 'none' });
            $("#txtCode").attr("disabled", "disabled");
            Status = obj;
            grid.PerformCallback('BEFORE_' + obj);
            cPopUp_groupMaster.Show();
        }

        function MakeRowInVisible() {
            cPopUp_groupMaster.Hide();
        }

        function DeleteRow(keyValue) {
            jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    grid.PerformCallback('Delete~' + keyValue);
                }
            });
        } 

        function gridRowclick(s, e) {
            $('#gridudfGroup').find('tr').removeClass('rowActive');
            $('.floatedBtnArea').removeClass('insideGrid');
            //$('.floatedBtnArea a .ico').css({ 'opacity': '0' });
            $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea').addClass('insideGrid');
            $(s.GetRow(e.visibleIndex)).addClass('rowActive');
            setTimeout(function () {
                //alert('delay');
                var lists = $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a');
                //$(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a .ico').css({'opacity': '1'});
                //$(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a').each(function (e) {
                //    setTimeout(function () {
                //        $(this).fadeIn();
                //    }, 100);
                //});    
                $.each(lists, function (index, value) {
                    //console.log(index);
                    //console.log(value);
                    setTimeout(function () {
                        $(value).css({ 'opacity': '1' });
                    }, 100);
                });
            }, 200);
        }
    </script>
   <script type="text/javascript">
       $(document).ready(function () {
           if ($('body').hasClass('mini-navbar')) {
               var windowWidth = $(window).width();
               var cntWidth = windowWidth - 90;
               grid.SetWidth(cntWidth);
           } else {
               var windowWidth = $(window).width();
               var cntWidth = windowWidth - 220;
               grid.SetWidth(cntWidth);
           }

           $('.navbar-minimalize').click(function () {
               if ($('body').hasClass('mini-navbar')) {
                   var windowWidth = $(window).width();
                   var cntWidth = windowWidth - 220;
                   grid.SetWidth(cntWidth);
               } else {
                   var windowWidth = $(window).width();
                   var cntWidth = windowWidth - 90;
                   grid.SetWidth(cntWidth);
               }

           });
       });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>HSN Master</h3>
        </div>
    </div>
    <div class="PopUpArea">
        <dxe:ASPxPopupControl ID="PopUp_groupMaster" runat="server" ClientInstanceName="cPopUp_groupMaster"
            Width="400px" HeaderText="Add UDF Group" PopupHorizontalAlign="WindowCenter"
            BackColor="white" Height="100px" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True" >
            <contentcollection>
                    <dxe:PopupControlContentControl runat="server">                       
                        <div class="Top clearfix">
                           
                            <table>
                                    <tr>
                                        <td>HSN <span style="color:red">*</span></td>
                                        <td>
                                            <asp:TextBox ID="txtCode" runat="server" MaxLength="50"></asp:TextBox></td>
                                        <td>
                                            <span id="MandatoryCode" class="pullleftClass fa fa-exclamation-circle iconRed " style="color:red;display:none;padding-left: 9px;" title="Mandatory"></span>
                                        </td>
                                   </tr>
                                 <tr>
                                        <td>Description <span style="color:red">*</span></td>
                                        <td>
                                            <asp:TextBox ID="txtDescription" runat="server" MaxLength="4000" TextMode="MultiLine" Rows="5" ></asp:TextBox></td>
                                        <td>
                                           <span id="MandatoryDesc" class="pullleftClass fa fa-exclamation-circle iconRed " style="color:red;display:none;padding-left: 9px;" title="Mandatory"></span>
                                        </td>
                                   </tr>
                                  
                                <tr>
                                    <td colspan="3" style="padding-left:121px;">
                                            <input id="btnSave" class="btn btn-primary" onclick="Call_save(status)" type="button" value="Save" />
                                    <input id="btnCancel" class="btn btn-danger" onclick="MakeRowInVisible()" type="button" value="Cancel" />
                                        </td>
                                        
                                    </tr>
                                </table>


                        </div>
                         
                    </dxe:PopupControlContentControl>
                </contentcollection>
            <headerstyle backcolor="LightGray" forecolor="Black" />
        </dxe:ASPxPopupControl>

    </div>


    <div class="form_main">
        <table class="TableMain100" >
            <tr>
                <td colspan="4">
                    <table class="TableMain100">
                        <tr>
                            <td colspan="4" style="text-align: left; vertical-align: top">
                                <table>
                                    <tr>
                                        <td id="ShowFilter">
                                            <% if (rights.CanAdd)
                                               { %>
                                            <asp:HyperLink ID="HyperLink2" runat="server"
                                                NavigateUrl="javascript:void(0)" onclick="javascript:AddNewGroup()" class="btn btn-success btn-radius"><span class="btn-icon"><i class="fa fa-plus" ></i></span>Add New</asp:HyperLink>
                                            <%} %>
                                            
                                        </td>
                                        <td id="Td1">
                                           
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="gridcellright"></td>
                        </tr>
                    </table>
                </td>
            </tr>

            <tr>
                <td class="relative">
                    <dxe:ASPxGridView ID="gridudfGroup" runat="server" ClientInstanceName="grid" AutoGenerateColumns="False" 
                        DataSourceID="SqlDataSource1"  Width="100%" OnCustomCallback="gridudfGroup_CustomCallback"  Settings-VerticalScrollableHeight="280" Settings-VerticalScrollBarMode="Auto"
                        OnCustomJSProperties="gridudfGroup_CustomJSProperties" OnHtmlRowCreated="gridudfGroup_HtmlRowCreated" SettingsDataSecurity-AllowDelete="false" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false">

                        <settingspager pagesize="10">
                            <FirstPageButton Visible="True"></FirstPageButton>

                            <LastPageButton Visible="True"></LastPageButton>

                            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200"/>
                              </settingspager>
                        <settings showgrouppanel="True" showstatusbar="Visible" showfilterrow="true" showfilterrowmenu="true" />

                        <clientsideevents endcallback="function(s, e) {
	                                LastCall(s.cpHeight);
                                }" />

                        <settingspager numericbuttoncount="10" pagesize="10" showseparators="True" alwaysshowpager="True">
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>

<PageSizeItemSettings Items="10,50, 100, 150, 200" Visible="True"></PageSizeItemSettings>
                        </settingspager>

                        <settingsediting mode="PopupEditForm" popupeditformheight="200px" popupeditformhorizontalalign="Center"
                            popupeditformmodal="True" popupeditformverticalalign="WindowCenter" popupeditformwidth="600px"
                            editformcolumncount="1" />
                        <settings showgrouppanel="True" showstatusbar="Visible" showfilterrow="true" showfilterrowmenu="true" />

                        <settingsbehavior confirmdelete="True" columnresizemode="NextColumn" />

                        <settingscommandbutton>
                           
                            <EditButton Image-Url="../../../assests/images/Edit.png" ButtonType="Image" Image-AlternateText="Edit" Styles-Style-CssClass="pad">
                                    <Image AlternateText="Edit" Url="../../../assests/images/Edit.png"></Image>
                             </EditButton>
                            <DeleteButton Image-Url="../../../assests/images/Delete.png" ButtonType="Image" Image-AlternateText="Delete" Styles-Style-CssClass="pad">
                                    <Image AlternateText="Delete" Url="../../../assests/images/Delete.png"></Image>
                            </DeleteButton>
                              <UpdateButton Text="Update" ButtonType="Button" Styles-Style-CssClass="btn btn-primary" Image-Width>
                                    <Styles>
                                    <Style CssClass="btn btn-primary"></Style>
                                    </Styles>
                            </UpdateButton>
                            <CancelButton Text="Cancel" ButtonType="Button"></CancelButton>
                        </settingscommandbutton>
                        <settingssearchpanel visible="True"  Delay="7000"/>
                        <settingstext popupeditformcaption="Add/Modify Category" confirmdelete="Confirm delete?" />
                        <styleseditors>
                            <ProgressBar Height="25px">
                            </ProgressBar>
                        </styleseditors>

                        <columns>
                            <dxe:GridViewDataTextColumn FieldName="HSN_Id" ReadOnly="True" Visible="False" VisibleIndex="0">
                                <EditFormSettings Visible="False" />
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Code" ReadOnly="True" Caption="HSN" Visible="true" VisibleIndex="0" Width="20%">
                                <EditFormSettings Visible="True"  />
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Description" Caption="Description"   Width="70%"
                                VisibleIndex="1" ShowInCustomizationForm="True">
                                <editcellstyle wrap="True">
                                </editcellstyle>
                                <CellStyle CssClass="gridcellleft" wrap="True">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>


                            <dxe:GridViewDataTextColumn Caption="" VisibleIndex="2" Width="0" Visible="false" >
                                <CellStyle HorizontalAlign="Center">
                                </CellStyle>
                                <HeaderStyle HorizontalAlign="Center" />
                                <HeaderTemplate>
                                    
                                </HeaderTemplate>
                                <DataItemTemplate>
                                    <div class='floatedBtnArea'>
                                         <% if (rights.CanEdit)
                                       { %>
                                    <a href="javascript:void(0);" onclick="OnEdit('EDIT~'+'<%#Eval("Code") %>')">
                                       <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Edit</span></a>
                                    <% } %>
                                    <% if (rights.CanDelete)
                                       { %>
                                     <a href="javascript:void(0);" onclick="DeleteRow('<%#Eval("Code") %>')"  >
                                        <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>Delete</span></a>
                                     <% } %>
                                    </div>
                                </DataItemTemplate>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                      

                              <dxe:GridViewDataTextColumn Caption="" VisibleIndex="3" Width="8%" >
                                <CellStyle HorizontalAlign="Center">
                                </CellStyle>
                                <HeaderStyle HorizontalAlign="Center" />
                                <HeaderTemplate>
                                    Tax Map
                               
                                </HeaderTemplate>
                                <DataItemTemplate>                                        
                                     <%--<a href="javascript:void(0);" onclick="DeleteRow('<%#Eval("Code") %>')"   alt="Tax Map">--%>
                                     <a href="javascript:void(0);" onclick="fn_EditProfession('<%#Eval("Code") %>')" class="pad">
                                        <img src="../../../assests/images/Edit.png" /></a>                                    
                                    
                                </DataItemTemplate>
                                  <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>

                        </columns>
                        <ClientSideEvents RowClick="gridRowclick" />
                        <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                        <styles>
                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                            </Header>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                        </styles>
                    </dxe:ASPxGridView>



                      <dxe:ASPxPopupControl ID="PopupProfession" runat="server" ClientInstanceName="cPopupProfession"
                        Width="600px" Height="120px" HeaderText="" PopupHorizontalAlign="Windowcenter"
                        PopupVerticalAlign="WindowCenter" CloseAction="closeButton" Modal="true">
                        <contentcollection>
                            <dxe:PopupControlContentControl ID="countryPopup" runat="server">
                                <div class="Top clearfix">
                                    <div class="col-md-6">
                                         <div class=" relative">
                                            <div class="profDiv">
                                                HSN Code 
                                            </div>
                                            <div style="position:relative">

                                                    <asp:TextBox ID="txtHsnCode1" runat="server" MaxLength="50" Enabled="false"></asp:TextBox>
                                                <%--<dxe:ASPxTextBox ID="txtHsnCode" ClientInstanceName="ctxtHsnCode" ClientEnabled="true"
                                                    runat="server" Height="25px" Width="240px" MaxLength="50">
                                                </dxe:ASPxTextBox>--%>           
                                            </div>
                                        </div>

                                                <div class=" relative">
                                            <label><strong>Type</strong></label>
                                            <div style="position:relative">

                                                 <dxe:ASPxComboBox ID="cmbSacType" runat="server" ClientInstanceName="CcmbSacType"
                                                    ValueType="System.String" width="100%">      
                                                </dxe:ASPxComboBox>
                                                       
                                            </div>
                                        </div>

                                   
                                    
                                        <div class=" relative">
                                           <label><strong>Input GST</strong></label>
                                    
                                         </div>
                                            
                                 
                                         <div class="relative">
                                           <label>CGST</label>
                                            <div>
                                                <dxe:ASPxComboBox ID="cmbCGST" runat="server" ClientInstanceName="CcmbCGST"
                                                    ValueType="System.String" width="100%">      
                                                </dxe:ASPxComboBox>
                                             
                                            </div>
                                       </div>
                                        <div class="relative">
                                           <label>SGST</label>
                                            <div>
                                                <dxe:ASPxComboBox ID="cmbSGST" runat="server" ClientInstanceName="CcmbSGST"
                                                    ValueType="System.String" width="100%">      
                                                </dxe:ASPxComboBox>
                                             
                                            </div>
                                       </div>
                                         <div class="relative">
                                           <label>UTGST</label>
                                            <div>
                                                <dxe:ASPxComboBox ID="cmbUTGST" runat="server" ClientInstanceName="CcmbUTGST"
                                                    ValueType="System.String" width="100%">      
                                                </dxe:ASPxComboBox>
                                             
                                            </div>
                                       </div>
                                        <div class="relative">
                                           <label>IGST</label>
                                            <div>
                                                <dxe:ASPxComboBox ID="cmbIGST" runat="server" ClientInstanceName="CcmbIGST"
                                                    ValueType="System.String" width="100%">      
                                                </dxe:ASPxComboBox>
                                             
                                            </div>
                                       </div>
                                        
                                       

                                  </div>
                                    <div class="col-md-6">
                                        <div class=" relative" style="margin-top: 42px;">
                                           <label><strong>Output GST</strong></label>
                                    
                                         </div>
                                            
                                 
                                         <div class="relative">
                                           <label>CGST</label>
                                            <div>
                                                <dxe:ASPxComboBox ID="cmbsaleCGST" runat="server" ClientInstanceName="CcmbSaleCGST"
                                                    ValueType="System.String" width="100%">      
                                                </dxe:ASPxComboBox>
                                             
                                            </div>
                                       </div>
                                        <div class="relative">
                                           <label>SGST</label>
                                            <div>
                                                <dxe:ASPxComboBox ID="cmbSaleSGST" runat="server" ClientInstanceName="CcmbSaleSGST"
                                                    ValueType="System.String" width="100%">      
                                                </dxe:ASPxComboBox>
                                             
                                            </div>
                                       </div>
                                        <div class="relative">
                                           <label>UTGST</label>
                                            <div>
                                                <dxe:ASPxComboBox ID="cmbSaleUTGST" runat="server" ClientInstanceName="CcmbSaleUTGST"
                                                    ValueType="System.String" width="100%">      
                                                </dxe:ASPxComboBox>
                                             
                                            </div>
                                       </div>
                                        <div class="relative">
                                           <label>IGST</label> 
                                            <div>
                                                <dxe:ASPxComboBox ID="cmbSaleIGST" runat="server" ClientInstanceName="CcmbSaleIGST"
                                                    ValueType="System.String" width="100%">      
                                                </dxe:ASPxComboBox>
                                             
                                            </div>
                                       </div>
                                        
                                        
                                    </div>
                                   
                                   </div>
                                <div class="text-center" style="padding-top:25px;">
                                    <dxe:ASPxButton ID="btnSave_Profession" ClientInstanceName="cbtnSave_Profession" runat="server" CssClass="btn btn-primary"
                                                AutoPostBack="False" Text="Save">
                                                <ClientSideEvents Click="function (s, e) {btnSave_Profession();}" />
                                            </dxe:ASPxButton>
                                            <dxe:ASPxButton ID="btnCancel_Profession" CssClass="btn btn-danger" runat="server" AutoPostBack="False" Text="Cancel">
                                                <ClientSideEvents Click="function (s, e) {fn_btnProfession();}" />
                                            </dxe:ASPxButton>
                                </div>
                                
                            </dxe:PopupControlContentControl>
                        </contentcollection>
                        <headerstyle backcolor="LightGray" forecolor="Black" />
                    </dxe:ASPxPopupControl>


                </td>
            </tr>
        </table>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server"
             SelectCommand="select HSN_Id, Code, [Description] from tbl_HSN_Master">
           
            <FilterParameters>
                <%--<asp:Parameter Name="pin_code" Type="String" />
                <asp:Parameter Name="city_id" Type="String" />--%>
            </FilterParameters>
        </asp:SqlDataSource>
         <%-- <asp:SqlDataSource ID="SqlDataSourceapplicable" runat="server"  
            SelectCommand="SELECT APP_NAME FROM tbl_master_UDFApplicable where IS_ACTIVE=0 order by ORDER_BY" >
        </asp:SqlDataSource>--%>
        <br />
    </div>
</asp:Content>




