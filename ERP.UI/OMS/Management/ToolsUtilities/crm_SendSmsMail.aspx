<%--================================================== Revision History =============================================
1.0   Pallab    V2.0.38      25-05-2023          0026232: SMS Notification module design modification & check in small device
====================================================== Revision History =============================================--%>

<%@ Page Title="SMS Notification" Language="C#" EnableEventValidation="false" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="crm_SendSmsMail.aspx.cs" Inherits="ERP.OMS.Management.ToolsUtilities.crm_SendSmsMail" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style>
        #EmployeeGrid_DXPagerBottom {
            min-width: 100% !important;
        }

        #EmployeeGrid {
            width: 100 % !important;
        }

        .myAssignTarget {
            margin-bottom: 0;
        }

        #cmbPriority {
            border-radius: 3px;
        }

        .myAssignTarget > li {
            list-style-type: none;
            display: inline-block;
            font-size: 11px;
            text-align: center;
        }

            .myAssignTarget > li:not(:last-child) {
                margin-right: 15px;
            }

            .myAssignTarget > li.mainCircle {
                border: 1px solid #a2d3d8;
                border-radius: 8px;
                overflow: hidden;
            }

            .myAssignTarget > li .heading {
                padding: 2px 12px;
                background: #6d82c5;
                color: #fff;
            }

            .myAssignTarget > li .Num {
                font-size: 14px;
            }

            .myAssignTarget > li.mainHeadCenter {
                font-size: 12px;
                transform: translateY(-16px);
            }

        #myAssignTargetpopup {
            padding: 0;
        }

            #myAssignTargetpopup > li .heading {
                padding: 6px 12px;
                background: #7f96dc;
                font-weight: 600;
                color: #fff;
            }

            #myAssignTargetpopup li .Num {
                font-size: 14px;
                padding: 5px;
            }

        .modal-footer .btn {
            margin-top: 0;
            margin-bottom: 0;
        }

        .mleft15 {
            margin-left: 15px;
        }

        #SalesActivityPopup_PW-1, #popupShowHistory_PW-1 {
            border-radius: 15px;
        }

            #SalesActivityPopup_PW-1 .dxpc-header, #popupShowHistory_PW-1 .dxpc-header {
                background: #3ca1e8;
                background-image: none !important;
                padding: 11px 20px;
                border: none;
                border-radius: 15px 15px 0 0;
            }

            #SalesActivityPopup_PW-1 .dxpc-contentWrapper, #popupShowHistory_PW-1 .dxpc-contentWrapper {
                background: #fff;
                border-radius: 0 0 15px 15px;
            }

            #SalesActivityPopup_PW-1 .dxpc-mainDiv, #popupShowHistory_PW-1 .dxpc-mainDiv {
                background-color: transparent !important;
            }

            #SalesActivityPopup_PW-1 .modal-footer, #popupShowHistory_PW-1 .modal-footer {
                text-align: left;
            }

            #SalesActivityPopup_PW-1 .dxpc-shadow, #popupShowHistory_PW-1 .dxpc-shadow {
                box-shadow: none;
            }
    </style>
    <link href="../../../ckeditor/contents.css" rel="stylesheet" />

    <%--  <script src="~/Scripts/jquery.validate.min.js"></script>--%>
    <script src="../../../ckeditor/ckeditor.js"></script>
    <script>
        $(function () {
            $("#ddlType").change(function () {
                if ($("#ddlType").val() == "Cus") {
                    $("#divCustom").removeClass("hidden");
                    ctxtCustomeSMS.Focus();
                    ctxtCustomeSMS.SetValue('');
                    grid.Refresh();
                }
                else {
                    grid.Refresh();
                    $("#divCustom").addClass("hidden");
                }
            })

            $("#txtEmpenRevw").on("keypress keyup blur", function (event) {
                //this.value = this.value.replace(/[^0-9\.]/g,'');
                $(this).val($(this).val().replace(/[^0-9\.]/g, ''));
                if ((event.which != 46 || $(this).val().indexOf('.') != -1) && (event.which < 48 || event.which > 57)) {
                    event.preventDefault();
                }
            });

            $("#txtcampDemd").on("keypress keyup blur", function (event) {
                //this.value = this.value.replace(/[^0-9\.]/g,'');
                $(this).val($(this).val().replace(/[^0-9\.]/g, ''));
                if ((event.which != 46 || $(this).val().indexOf('.') != -1) && (event.which < 48 || event.which > 57)) {
                    event.preventDefault();
                }
            });
        });

        //var editor1 = CKEDITOR.instances['text_id'];
        $(document).ready(function () {
            // CKEDITOR.replace('editor1');
            var editor = CKEDITOR.instances['text_id'];
            if (editor) { editor.destroy(true); }
            CKEDITOR.replace('text_id');
            $("#ddlType").val('Select');
        });

        function OnEndCallback(s, e) {
        }

        function SendMessage() {

            var message = $("#txtSms").val();

            var seletedUser = "";
            $("#multiselect_to option").each(function () {
                seletedUser = seletedUser + $(this).val() + ",";
            });


            var mobile = seletedUser;
            if ($("#ddlType").val() == "Cus") {
                mobile = $("#txtCustomeSMS").val();
            }

            mobile = mobile.replace(/,\s*$/, "");

            //  var url = '@Url.Action("","")';  ..Management/ToolsUtilities/
            debugger;
            $.ajax({
             
                //type: "POST",
                //url: "crm_SendSmsMail.aspx/SendSMS",
                //data: JSON.stringify({ Mobiles: mobile, messagetext: message }),
                //contentType: "application/json; charset=utf-8",
                //dataType: "json",
                type: "POST",
                url: "crm_SendSmsMail.aspx/SendSMS",
                data: JSON.stringify({ Mobiles: mobile, messagetext: message }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (response) {
                    debugger;
                    if (response == "200") {
                        jAlert('Message sent successfully.');
                    }
                    else {
                        //jAlert('Please try again later.');
                        return false;
                    }
                }
            });



            // window.location.href = url;
        }

        function ProductButnClick(s, e) {
            //if (e.buttonIndex == 0) {
            setTimeout(function () { $("#txtProdSearch").focus(); }, 500);

            $('#txtProdSearch').val('');
            $('#ProductModel').modal('show');

            //if (cproductLookUp.Clear()) {
            //cProductpopUp.Show();
            //cproductLookUp.Focus();                     
            //}
            //  }
        }

        function prodkeydown(e) {
            //Both-->B;Inventory Item-->Y;Capital Goods-->C
            var inventoryType = "";
            var OtherDetails = {}
            if ($.trim($("#txtProdSearch").val()) == "" || $.trim($("#txtProdSearch").val()) == null) {
                return false;
            }

            var SearchKey = $("#txtProdSearch").val();
            var InventoryType = inventoryType;

            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var HeaderCaption = [];
                HeaderCaption.push("Product Code");
                HeaderCaption.push("Product Name");
                HeaderCaption.push("Inventory");
                HeaderCaption.push("HSN/SAC");
                HeaderCaption.push("Class");
                HeaderCaption.push("Brand");
                if ($("#txtProdSearch").val() != '') {
                    callonServer("Services/Master.asmx/GetProductDetailsForSI", SearchKey, InventoryType);
                }
            }
            else if (e.code == "ArrowDown") {
                if ($("input[ProdIndex=0]"))
                    $("input[ProdIndex=0]").focus();
            }
        }

        function openActivity() {
            //cSalesActivityPopup.show();
            //$("#ActivityModel").show();
        }

        function btnSmsClick(s, e) {
            grid.PerformCallback();
        }
        function gridEndcallback(s, e) {
            if (grid.cpResult == "True") {
                //grid.Refresh();
                //grid.Refresh();
              //  jAlert('SMS Sent Successfully.');
            }
            else {
              //  jAlert('SMS not Sent please try again later.');
                //grid.Refresh();
                //grid.Refresh();
            }
        }


        function ValidateDigitsAndDot(s,e) {
            if (e.htmlEvent.key == "&") {
                ASPxClientUtils.PreventEvent(e.htmlEvent);
            }
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

    <%--Rev 1.0--%>
    <link href="/assests/css/custom/newcustomstyle.css" rel="stylesheet" />
    
    <style>
        .chosen-container-single .chosen-single div::after
        {
            font-size: 17px;
        }
        /*.simple-select::after
        {
            top: 7px;
            right: -2px;
        }*/
        .checkbox label
        {
            line-height: 14px;
        }
        .checkbox-group label:before
        {
            top: 1px;
        }
    </style>
    <%--Rev end 1.0--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%-- <div class="panel-heading">
        <div class="panel-title">
            <h3>Marketing Campaign</h3>
        </div>
    </div>--%>
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main clearfix">
        <div class="panel-title clearfix">
        <h3 class="pull-left">
            <asp:Label ID="lblHeadTitle" Text="" runat="server">SMS Notification</asp:Label>
            <%--<label>Add Proforma Invoice/ Quotation</label>--%>
        </h3>

     <%--   <div id="ApprovalCross" runat="server" class="crossBtn"><a href=""><i class="fa fa-times"></i></a></div>
        <div id="divcross" runat="server" class="crossBtn"><a href="MarketingCampaignList.aspx"><i class="fa fa-times"></i></a></div>--%>

    </div>
        <div class="form_main" style="border: 1px solid #ccc; padding: 10px 15px;">
        <%-- <div class="row">
            <div class="col-md-12">
                <div class="col-md-3">
                    <label>Campaign Name</label>
                    <asp:TextBox runat="server" placeholder="Campaign Name" ID="txtName" CssClass="form-control" />
                </div>
                <div class="col-md-3">
                    <label>Source of Campaign</label>
                    <asp:TextBox runat="server" placeholder="Source of Campaign" ID="txtSourceCamp" CssClass="form-control" />
                </div>

                <div class="col-md-3">
                    <label>Campaign Budget</label>
                    <asp:TextBox runat="server" placeholder="0.00" ID="txtcampDemd" CssClass="form-control" />
                </div>
                <div class="col-md-3">
                    <label>Expected Revenue</label>
                    <asp:TextBox runat="server" placeholder="0.00" ID="txtEmpenRevw" CssClass="form-control" />
                </div>
            </div>
        </div>--%>
        <%--   <div class="row">
            <div class="col-md-12">
                <div class="col-md-3">
                    <label>Description </label>
                    <asp:TextBox TextMode="MultiLine" placeholder="Description..." Style="max-width: 304px; margin: 0px 4px 5px 0px; width: 396px; height: 49px;" runat="server" ID="TextBox1" CssClass="form-control" />
                </div>
                <div class="col-md-3">
                    <label>Products/Services</label>
                    <%--     <dxe:ASPxLabel ID="ASPxLabel3" runat="server" Text="Salesman/Agents">
                    </dxe:ASPxLabel>

                    <dxe:ASPxButtonEdit ID="txtProducts" runat="server" ReadOnly="true" ClientInstanceName="ctxtProducts" TabIndex="8" Width="100%">
                        <Buttons>
                            <dxe:EditButton>
                            </dxe:EditButton>
                        </Buttons>
                        <ClientSideEvents ButtonClick="function(s,e){ProductButnClick();}" KeyDown="ProductButnClick" />
                    </dxe:ASPxButtonEdit>
                </div>
            </div>
        </div>--%>
        <div class="row">
            <%--Rev 1.0: "simple-select" class add --%>
            <div class="col-md-3 simple-select">
                <label>Entity Type</label>
                <asp:DropDownList runat="server" CssClass="form-control" ID="ddlType">
                    <asp:ListItem Value="Select" Text="Select" />
                    <asp:ListItem Value="CL" Text="Customer" />
                    <asp:ListItem Value="LD" Text="Lead" />
                    <asp:ListItem Value="Cus" Text="Custom" />
                </asp:DropDownList>
            </div>
            <div class="col-md-3 hidden lblmTop8" id="divCustom">
                <label>Phone Number</label>
                    <dxe:ASPxTextBox ID="txtCustomeSMS" runat="server" ClientInstanceName="ctxtCustomeSMS" HorizontalAlign="Left" Font-Size="12px" Width="100%" Height="15px">
                                        <MaskSettings Mask="9999999999" IncludeLiterals="None" />
                                        <ValidationSettings Display="None"></ValidationSettings>
                                    </dxe:ASPxTextBox>
                <%--<asp:TextBox runat="server" placeholder="Phone Number" MaxLength="10" ID="txtCustomeSMS" CssClass="form-control"></asp:TextBox>--%>
                <%--<input type="text" class="form-control tagsInput" id="txtCustomeSMS" value="" data-role="tagsinput" />--%>
            </div>
           
        </div>
        <div class="clear"></div>
        <br />
        <div class="row">
            <div class="col-md-12">
                
                    <dxe:ASPxGridView runat="server" ClientInstanceName="grid" ID="grid"
                        Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" Settings-ShowFooter="false"
                        SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollBarMode="auto"
                        OnCellEditorInitialize="grid_CellEditorInitialize" OnCustomCallback="grid_CustomCallback"
                        OnDataBinding="grid_DataBinding"
                        KeyFieldName="cnt_id"
                        DataSourceID="EntityServerModeDataSource"
                        Settings-VerticalScrollableHeight="150" CommandButtonInitialize="false" Settings-ShowStatusBar="Hidden">
                        <SettingsPager Visible="false"></SettingsPager>
                        <Styles>
                            <StatusBar CssClass="statusBar">
                            </StatusBar>
                        </Styles>
                        <Columns>

                            <dxe:GridViewDataTextColumn FieldName="cnt_internalid" Caption="cntID" ReadOnly="true" VisibleIndex="1" Width="5%" Visible="false">
                                <PropertiesTextEdit>
                                </PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewCommandColumn Caption="" SelectAllCheckboxMode="AllPages" ShowSelectCheckbox="true" Width="8%" VisibleIndex="2">
                            </dxe:GridViewCommandColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="3" Caption="Entity Name" ReadOnly="false" FieldName="Name">
                                <PropertiesTextEdit Style-HorizontalAlign="Left">
                                    <ValidationSettings RequiredField-IsRequired="false" Display="None"></ValidationSettings>
                                </PropertiesTextEdit>
                                <CellStyle Wrap="False" HorizontalAlign="Left" CssClass="gridcellleft"></CellStyle>
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>
                             <dxe:GridViewDataTextColumn VisibleIndex="4" Caption="Phone Number" ReadOnly="false" FieldName="phf_phoneNumber">
                                <PropertiesTextEdit Style-HorizontalAlign="Left">
                                    <ValidationSettings RequiredField-IsRequired="false" Display="None"></ValidationSettings>
                                </PropertiesTextEdit>
                                <CellStyle Wrap="False" HorizontalAlign="Left" CssClass="gridcellleft"></CellStyle>
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                        <ClientSideEvents EndCallback="gridEndcallback" />
                        <%--   <ClientSideEvents EndCallback="OnEndCallback" CustomButtonClick="OnCustomButtonClick" RowClick="GetVisibleIndex" BatchEditStartEditing="GetVisibleIndex" />--%>
                        <SettingsDataSecurity AllowEdit="true" />
                        <SettingsPager Mode="ShowPager" Visible="true" AlwaysShowPager="true"></SettingsPager>
                        <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                    </dxe:ASPxGridView>
                    <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
                        ContextTypeName="ERPDataClassesDataContext" TableName="v_EntityList" />
               
            </div>
        </div>
        <%--    <div class="clear"></div>--%>
        <br />
        <div class="row">
            <div class="col-md-12">
                    <dxe:ASPxMemo runat="server" ID="txtSms" Height="100" MaxLength="160" NullText="Please Enter Your Message.." 
                        CssClass="form-control" style="width: 100%;border: 2px solid #e4e4e4;padding:8px">
                        <ClientSideEvents KeyPress="ValidateDigitsAndDot" />
                    </dxe:ASPxMemo>
                    <%--<textarea maxlength="360" id="txtSms" placeholder="Please Enter Your Message.." class="form-control" style="margin: 0px -338.25px 5px 0px; width: 1322px; height: 85px;"></textarea>--%>
            </div>
        </div>
        <br />
        <div class="row">
            <div class="col-md-12">
                
                    <%--<button id="btnSms" class="btn btn-success" onclick="SendMessage();">Send SMS</button>--%>
                    <dxe:ASPxButton Text="Send SMS" ID="btnSms" CssClass="btn btn-info" UseSubmitBehavior="false" ClientSideEvents-Click="btnSmsClick" runat="server" />
                    <%--<button class="btn btn-success" data-toggle="modal" id="btnEmail" data-target="#MailPopup" onclick="return false;" data-backdrop="static" data-keyboard="true">Send Email</button>--%>
                    <%--    <asp:Button Text="Send Email" ID="btnEmail" CssClass="btn btn-success" data-toggle="modal" data-target="#skillPopup" data-backdrop="static" data-keyboard="true" runat="server" />--%>
                    <%--<asp:Button Text="Convert To Lead" runat="server" CssClass="btn btn-success" ID="btnConvertLead" />--%>
                    <%-- <asp:Button Text="Craete Activity" ID="btnActivity" CssClass="btn btn-success" runat="server" OnClientClick="openActivity()return false; " />--%>
                    <%--<button type="button" value="Craete Activity" id="btnActivity" class="btn btn-success" onclick="return cSalesActivityPopup.Show();">Create Activity</button>--%>
                    <asp:Button Text="Cancel" ID="btnCancel" CssClass="btn btn-danger" runat="server" />
            </div>
        </div>
    </div>
    </div>
    <div class="modal fade pmsModal w40" id="MailPopup" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel">Enter Email Body</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close" onclick="openModal()">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="pmsForm">
                        <div class="form-group row">
                            <div class="col-md-12">
                                <%-- <div class="col-md-1">
                                    <label>To</label></div>--%>
                                <%-- <div class="col-md-11">--%>
                                <input type="text" placeholder="To" class="form-control tagsInput" id="txtTo" /><%--</div>--%>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-md-12">
                                <%-- <div class="col-md-1">
                                    <label>CC</label></div>--%>
                                <%-- <div class="col-md-11">--%>
                                <input type="text" placeholder="CC" class="form-control" id="txtCC" /><%--</div>--%>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-md-12">
                                <%-- <div class="col-md-1">
                                    <label>BCC</label></div>--%>
                                <%--<div class="col-md-11">--%>
                                <input type="text" placeholder="BCC" class="form-control" id="txtBCC" /><%--</div>--%>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-md-12">
                                <%-- <div class="col-md-1">
                                    <label>Subject </label>
                                </div>
                                <div class="col-md-11">--%>
                                <input type="text" placeholder="Subject" class="form-control" id="txtSub" /><%--</div>--%>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-md-12" id="TrMessage">
                                <%-- <label class="Ecoheadtxt">Body :</label>--%>
                                <div>
                                    <%--  <textarea name="editor1" id="editor1" rows="10"  class="form_control">
                                        This is my textarea to be replaced with CKEditor.
                                    </textarea>--%>
                                    <asp:TextBox ID="text_id" runat="server"></asp:TextBox>
                                </div>
                                <div style="text-align: right; display: none;">
                                    <a href="#" onclick="frmOpenNewWindow()">
                                        <span style="color: #000099; text-decoration: underline">Use Reserved Word</span></a>
                                </div>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-md-12">
                                <label>Attach File</label>
                                <input type="file" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer" style="padding: 6px 11px 8px;">
                    <button type="button" class="btn btn-danger btn-radius" data-dismiss="modal" onclick="openModal()">Close</button>
                    <button type="button" class="btn btn-success btn-radius" id="divSaveinf" onclick="SaveSkill()">Send</button>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="ProductModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Product Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="prodkeydown(event)" id="txtProdSearch" autofocus width="100%" placeholder="Search By Product Code or Name" />

                    <div id="ProductTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Product Code</th>
                                <th>Product Name</th>
                                <th>Inventory</th>
                                <th>HSN/SAC</th>
                                <th>Class</th>
                                <th>Brand</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>




    <dxe:ASPxPopupControl ID="SalesActivityPopup" runat="server" ClientInstanceName="cSalesActivityPopup"
        Width="650px" HeaderText="Campaign Activity" PopupHorizontalAlign="WindowCenter" Height="500px"
        PopupVerticalAlign="WindowCenter" CssClass="DevPopTypeNew" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
        <ContentStyle VerticalAlign="Top"></ContentStyle>
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanelLeadActivity" ClientInstanceName="cCallbackPanelLeadActivity">
                    <PanelCollection>
                        <dxe:PanelContent runat="server">

                            <div class="col-md-12">
                                <ul class="myAssignTarget" id="myAssignTargetpopup">

                                    <li class="mainCircle">
                                        <div class="heading">
                                            <dxe:ASPxLabel ID="ASPxLabel1" runat="server" Text="Campaign Name ">
                                            </dxe:ASPxLabel>
                                        </div>
                                        <div id="lblsource" class="Num">
                                            &nbsp;<dxe:ASPxLabel ID="lblshowLeadName" runat="server" Text=""></dxe:ASPxLabel>
                                        </div>
                                    </li>
                                    <li class="mainCircle">
                                        <div class="heading">
                                            <dxe:ASPxLabel ID="ASPxLabel2" runat="server" Text="Due Date "></dxe:ASPxLabel>
                                        </div>
                                        <div id="lblIndustry" class="Num">
                                            &nbsp;<dxe:ASPxLabel ID="lblshowDueDate" runat="server" Text=""></dxe:ASPxLabel>
                                        </div>
                                    </li>
                                    <li class="mainCircle">
                                        <div class="heading">
                                            <dxe:ASPxLabel ID="ASPxLabel4" runat="server" Text="Priority ">
                                            </dxe:ASPxLabel>

                                        </div>
                                        <div id="lblMiscComments" class="Num">
                                            &nbsp;
                                                    <dxe:ASPxLabel ID="lblshowPriority" runat="server" Text="">
                                                    </dxe:ASPxLabel>
                                        </div>
                                    </li>

                                </ul>

                            </div>
                            <div class="clear"></div>
                            <div class="clearfix">


                                <div class="col-md-6">
                                    <div class="visF">
                                        <div id="ltd_ActivityDate" class="labelt">
                                            <div class="visF">
                                                <dxe:ASPxLabel ID="ASPxLabel5" runat="server" Text="Activity Date">
                                                </dxe:ASPxLabel>
                                                <span style="color: red;">*</span>
                                            </div>
                                        </div>
                                        <div id="td_ActivityDate">
                                            <div class="visF">
                                                <dxe:ASPxDateEdit ID="dtActivityDate" TabIndex="9" runat="server" Date="" Width="100%" ClientInstanceName="cdtActivityDate">
                                                    <TimeSectionProperties>
                                                        <TimeEditProperties EditFormatString="hh:mm tt" />
                                                    </TimeSectionProperties>
                                                </dxe:ASPxDateEdit>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <label>
                                        <dxe:ASPxLabel ID="ASPxLabel6" runat="server" Text="" CssClass="pdl8"></dxe:ASPxLabel>
                                    </label>
                                    <button type="button" class="btn btn-primary btn-block" onclick="Products('POE')">Product</button>
                                </div>
                                <div class="clear"></div>
                                <div class="col-md-6">
                                    <div class="visF">
                                        <div id="td_Activity" class="labelt">
                                            <div class="visF">
                                                <dxe:ASPxLabel ID="lblActivity" runat="server" Text="Activity">
                                                </dxe:ASPxLabel>
                                                <span style="color: red;">*</span>
                                            </div>
                                        </div>
                                        <div id="td_Type">
                                            <div class="visF">
                                                <asp:DropDownList ID="cmbActivity" runat="server" TabIndex="2" Width="100%">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="visF">
                                        <div class="labelt">
                                            <label>
                                                <dxe:ASPxLabel ID="blnActivityType" runat="server" Text="Type" CssClass="pdl8"></dxe:ASPxLabel>
                                                <span style="color: red;">*</span>

                                            </label>
                                            <div id="td_Type">
                                                <div class="">
                                                    <asp:DropDownList ID="cmbType" runat="server" TabIndex="3" Width="100%">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="clear"></div>
                                <div class="col-md-12">
                                    <div class="">
                                        <label>
                                            <dxe:ASPxLabel ID="lblSubject" runat="server" Text="Subject" CssClass="pdl8"></dxe:ASPxLabel>
                                            <span style="color: red;">*</span>

                                        </label>
                                        <div id="td_Type">
                                            <div class="">
                                                <asp:TextBox ID="txtSubject" runat="server" CssClass="form-control" TabIndex="4" MaxLength="500" Width="100%">
                                                </asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="clear"></div>
                                <div class="col-md-12">
                                    <div class="">
                                        <label>
                                            <dxe:ASPxLabel ID="lblDetails" runat="server" Text="Details" CssClass="pdl8"></dxe:ASPxLabel>
                                            <span style="color: red;">*</span>

                                        </label>
                                        <div id="td_Details">
                                            <div class="">
                                                <asp:TextBox ID="txtDetails" runat="server" TextMode="MultiLine" Columns="20" Rows="4" CssClass="form-control" TabIndex="5" MaxLength="500" Width="100%">
                                                </asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="clear"></div>
                                <div class="col-md-12">
                                    <div class="">
                                        <div id="td_lAssignto" class="labelt">
                                            <dxe:ASPxLabel ID="lblSalesActivityAssignTo" runat="server" Text="Assign To">
                                            </dxe:ASPxLabel>
                                            <span style="color: red;">*</span>
                                        </div>
                                        <div id="td_dAssignto">
                                            <asp:DropDownList ID="cmbSalesActivityAssignTo" runat="server" TabIndex="6" Width="100%">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="clear"></div>
                                <div class="col-md-6">
                                    <div class="">
                                        <div id="td_lDuration" class="labelt">
                                            <dxe:ASPxLabel ID="lblDuration" runat="server" Text="Duration">
                                            </dxe:ASPxLabel>
                                            <span style="color: red;">*</span>
                                        </div>
                                        <div id="td_dDuration">
                                            <asp:DropDownList ID="cmbDuration" runat="server" TabIndex="7" Width="100%">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="">
                                        <div id="td_lPriority" class="labelt">
                                            <dxe:ASPxLabel ID="lblPriority" runat="server" Text="Priority">
                                            </dxe:ASPxLabel>
                                            <span style="color: red;">*</span>
                                        </div>
                                        <div id="td_dPriority">
                                            <asp:DropDownList ID="cmbPriority" runat="server" TabIndex="8" Width="100%">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="">
                                        <div id="td_lDue" class="labelt">
                                            <dxe:ASPxLabel ID="lblDue" runat="server" Text="Due" CssClass="pdl8"></dxe:ASPxLabel>
                                        </div>
                                        <div>
                                            <%-- <dxe:ASPxDateEdit ID="DtxtDue" runat="server" EditFormatString="dd-MM-yyyy hh:mm:ss"  ClientInstanceName="cDtxtDue" EditFormat="Custom" UseMaskBehavior="True" TabIndex="20" Width="100%">
                                                    <ButtonStyle Width="13px">
                                                    </ButtonStyle>
                                                    <TimeSectionProperties>
                                                        <TimeEditProperties EditFormatString="hh:mm tt" />
                                                    </TimeSectionProperties>
                                                </dxe:ASPxDateEdit>--%>

                                            <dxe:ASPxDateEdit ID="DtxtDue" TabIndex="9" runat="server" Date="" Width="100%" ClientInstanceName="cDtxtDue">
                                                <TimeSectionProperties>
                                                    <TimeEditProperties EditFormatString="hh:mm tt" />
                                                </TimeSectionProperties>
                                                <%-- <ClientSideEvents DateChanged="Enddate" />--%>
                                            </dxe:ASPxDateEdit>
                                        </div>
                                    </div>
                                </div>
                            </div>


                            <div class="clearfix">








                                <asp:HiddenField ID="hdnEntityID" runat="server" />
                            </div>

                            <div class="modal-footer">
                                <button type="button" class="btnOkformultiselection btn btn-success">Save</button><%--onclick="SaveSalesActivity()"--%>
                                <button type="button" class="btnOkformultiselection btn btn-danger">Cancel</button>
                                <%--onclick="CancelSalesActivity()"--%>
                                <button type="button" class="btnOkformultiselection btn btn-info">Show History</button>
                                <%--onclick="ShowHistory()"--%>
                            </div>

                        </dxe:PanelContent>
                    </PanelCollection>
                    <%--  <ClientSideEvents EndCallback="CallbackPanelLeadActivityEndCall" />--%>
                </dxe:ASPxCallbackPanel>
            </dxe:PopupControlContentControl>
        </ContentCollection>

    </dxe:ASPxPopupControl>
</asp:Content>
