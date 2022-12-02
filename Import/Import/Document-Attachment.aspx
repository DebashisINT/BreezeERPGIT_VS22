<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/Masterpage/ERP.Master" AutoEventWireup="true" CodeBehind="Document-Attachment.aspx.cs"
    Inherits="Import.Import.Document_Attachment" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script>


        $(document).ready(function () {

            document.onkeydown = function (e) {

                if (event.altKey == true) {

                    switch (event.keyCode) {

                        case 85:

                            $("#UploadButton").trigger("click");

                            break;
                        case 117:

                            $("#UploadButton").trigger("click");

                            break;


                    }

                }
            }
        });




        $(function () {

            $('body').on('change', '#drp_templatetype', function () {

           //     cDocumentPanel.PerformCallback('BindDocumentNumber' + '~' + $("#drp_templatetype").val());

                $("#lookup_Document_I").attr('readonly', true);
            });

            $("#lookup_Document_I").attr('readonly', true);

        });


        function Productimagepopulate(value) {
            // alert(value);
            //gridDocumentAttachment.PerformCallback();
            if (value == 'Attachment Saved Successfully.') {
                jAlert('Attachment Uploaded Successfully', 'Alert', function () {

                    window.location.href = "../Import/Document-Attachmentlist.aspx";
                });

            }

            else {
                jAlert(value);

            }
        }

        function DownloadImage(keyValue) {


            //$.ajax({
            //    type: "POST",
            //    url: "Document-Attachment.aspx/DownloadAttachment",
            //    data: JSON.stringify({ Id: keyValue }),
            //    contentType: "application/json; charset=utf-8",
            //    dataType: "json",
            //    success: function (data) {
            //    }
            //});


            window.open("../Import/DownloadAttachment.aspx?D=" + keyValue);

        }


        function OnEndCallbackDocument() {
            $("#lookup_Document_I").attr('readonly', true);
        }

        function taggingListButnClick(s, e) {

            ctaggingGrid.PerformCallback('BindComponentGrid' + '~' + $("#drp_templatetype").val());
            cpopup_taggingGrid.Show();


        }

        function taggingListKeyDown(s, e) {

            if (e.htmlEvent.key == "Enter") {
                s.OnButtonClick(0);
            }

        }

        function QuotationNumberChanged() {
            var OrderData = ctaggingGrid.GetSelectedKeysOnPage();
            cpopup_taggingGrid.Hide();
        }



        function PerformCallToGridBind() {

            //   var quote_Id = ctaggingGrid.GetSelectedKeysOnPage();

            ctaggingGrid.PerformCallback('CloseDocumentGrid' + '~' + $("#drp_templatetype").val());

            //ctaggingGrid.GetRowValues(ctaggingGrid.GetFocusedRowIndex(), "Document", function (values) {

            //    alert(values);
            //    cpopup_taggingGrid.Hide();

            //});

        }



        function OnGetRowValuesCallback(values) {
            alert(values);
            ctaggingList.SetValue(values);
            //cPLQADate.SetValue(ComponentDate);
            //cPLQuoteDate.SetEnabled(false);
            cpopup_taggingGrid.Hide();
        }
        function GridDocumentEndCallback() {

          
            if (ctaggingGrid.cpDocment != null) {
                ctaggingList.SetValue(ctaggingGrid.cpDocment);
                cpopup_taggingGrid.Hide();
            }

        }

    </script>

    <style>
        .piTabl > tbody > tr > td {
            padding-right: 15px;
            padding-bottom: 10px;
        }
    </style>



</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <div class="panel-heading">
        <div class="panel-title clearfix">

            <h3>
                <asp:Label ID="lblHeading" runat="server" Text="Document Attachment"></asp:Label>
                

            </h3>

            <div id="ApprovalCross" runat="server" class="crossBtn"><a href=""><i class="fa fa-times"></i></a></div>
            <div id="divcross" runat="server" class="crossBtn"><a href="Document-AttachmentList.aspx"><i class="fa fa-times"></i></a></div>

        </div>
    </div>



    <div class="form_main" style="border: 1px solid #ccc; padding: 10px 15px;">
        <div class="row">
            <div class="col-md-3">
                <label>Document Type  <span style="color: red;">*</span> </label>
                <div>

                    <asp:DropDownList ID="drp_templatetype" runat="server" Width="100%">

                        <asp:ListItem Value="0">Select Type</asp:ListItem>
                        <asp:ListItem Value="IPO">Import Purchase Order</asp:ListItem>
                        <asp:ListItem Value="IPI">Import Order Acceptance(Indent)</asp:ListItem>
                        <asp:ListItem Value="IPBL">Bill of Lading(B/L)</asp:ListItem>
                        <asp:ListItem Value="IPINV">Import Purchase Invoice</asp:ListItem>
                         <asp:ListItem Value="IPBETR">Bill of Entry(B/E)</asp:ListItem>
                        <asp:ListItem Value="IPBETR">Import Stock Receipt</asp:ListItem>
                         <asp:ListItem Value="IPLCO">L/C Entry</asp:ListItem>
                    </asp:DropDownList>

                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-3">
                    <label><dxe:ASPxLabel ID="lbl_IndentRequisition" runat="server" Text="Document Number">
                    </dxe:ASPxLabel></label>
                    <div>
                    <dxe:ASPxButtonEdit ID="taggingList" ClientInstanceName="ctaggingList" runat="server" ReadOnly="true" Width="100%">
                        <buttons>
                                                <dxe:EditButton>
                                                </dxe:EditButton>
                                            </buttons>
                        <clientsideevents buttonclick="taggingListButnClick" keydown="taggingListKeyDown" />
                    </dxe:ASPxButtonEdit>


                    <dxe:ASPxPopupControl ID="popup_taggingGrid" runat="server" ClientInstanceName="cpopup_taggingGrid"
                        HeaderText="Select Document Number" PopupHorizontalAlign="WindowCenter"
                        BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton" Height="400px" Width="850px"
                        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
                        ContentStyle-CssClass="pad">

                        <contentstyle verticalalign="Top" cssclass="pad">
                        </contentstyle>

                        <contentcollection>

                                                <dxe:PopupControlContentControl runat="server">

                                                    <div>

                                                        <dxe:ASPxGridView ID="taggingGrid" ClientInstanceName="ctaggingGrid" runat="server" KeyFieldName="ID"
                                                            Width="100%" SettingsBehavior-AllowDragDrop="false" SettingsPager-Mode="ShowAllRecords"
                                                            Settings-ShowFooter="false" AutoGenerateColumns="False" Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Visible"
                                                            OnCustomCallback="taggingGrid_CustomCallback" OnDataBinding="taggingGrid_DataBinding" SettingsBehavior-AllowSelectSingleRowOnly="true">
                                                            <SettingsBehavior AllowDragDrop="False"></SettingsBehavior>

                                                            <SettingsPager Visible="false"></SettingsPager>


                                                            <Columns>



                                                                <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="40" Caption=" " VisibleIndex="0" />

                                                                <dxe:GridViewDataTextColumn FieldName="Document" Caption="Document Number" Width="150" VisibleIndex="1">
                                                                </dxe:GridViewDataTextColumn>

                                                                <dxe:GridViewDataTextColumn FieldName="PurchaseOrder_Date" Caption="Date" Width="100" VisibleIndex="2" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy">
                                                                </dxe:GridViewDataTextColumn>

                                                                <dxe:GridViewDataTextColumn FieldName="branch_description" Caption="Unit" Width="150" VisibleIndex="3">
                                                                </dxe:GridViewDataTextColumn>

                                                                <dxe:GridViewDataTextColumn FieldName="Vendor" Caption="Vendor" Width="150" VisibleIndex="4">
                                                                </dxe:GridViewDataTextColumn>


                                                            </Columns>


                                                            <SettingsDataSecurity AllowEdit="true" />
                                                            <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                                                          <ClientSideEvents EndCallback="GridDocumentEndCallback"/>
                                                        </dxe:ASPxGridView>


                                                    </div>
                                                    <br />
                                                    <div class="text-center">

                                                        <dxe:ASPxButton ID="Button2" ClientInstanceName="cButton2" runat="server" AutoPostBack="False" Text="OK" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                            <ClientSideEvents Click="function(s, e) {return PerformCallToGridBind();}" />
                                                        </dxe:ASPxButton>

                                                    </div>


                                                </dxe:PopupControlContentControl>
                                            </contentcollection>
                        <headerstyle backcolor="LightGray" forecolor="Black" />
                    </dxe:ASPxPopupControl>
                    </div>


                </div>
        </div>
        <div class="row">
            <div class="col-md-3">
                
                <label>Document(s) <span style="color: red;">*</span> </label>
                   
                <div id="Div_FileUpload" runat="server">
                    <asp:FileUpload ID="file_product" runat="server" AllowMultiple="true" Width="100%" />
                    <span style="color: red;">Maximum 5 Mb allowed(pdf,word,excel)</span>
                    
                </div>
                <div>
                    <asp:Label ID="lblDocument" runat="server"></asp:Label>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-4">
                <asp:Button ID="UploadButton" runat="server" CssClass="btn btn-primary" OnClick="UploadButton_Click" Text="U&#818;pload File" />
            </div>
        </div>
        
        <table class="piTabl">


            <tr>
                

            </tr>
            <tr>

                <%--  <td>Document Number  <span style="color: red;">*</span> </td>

                <td>

                    <dxe:ASPxCallbackPanel runat="server" ID="ASPxCallbackPanel1" ClientInstanceName="cDocumentPanel" OnCallback="ComponentDocument_Callback" >
                        <panelcollection>

                            <dxe:PanelContent runat="server">

                                <dxe:ASPxGridLookup ID="lookup_Document" SelectionMode="Single" runat="server" ClientInstanceName="gridDocumentLookup"
                                    OnDataBinding="lookup_Document_DataBinding" 
                                    KeyFieldName="ID" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False">


                                    <Columns>

                                        <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="50px" Caption=" " />

                                        <dxe:GridViewDataColumn FieldName="Document" Visible="true" VisibleIndex="1" width="200px" Caption="Document Number" Settings-AutoFilterCondition="Contains">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataColumn>


                                        <dxe:GridViewDataDateColumn FieldName="PurchaseOrder_Date" Visible="true"  VisibleIndex="2"  width="200px" Caption="Date" Settings-AutoFilterCondition="Contains" >
                                            <Settings AutoFilterCondition="Contains" />
                                             <PropertiesDateEdit  DisplayFormatString="dd-MM-yyyy"></PropertiesDateEdit>
                                        </dxe:GridViewDataDateColumn>


                                        <dxe:GridViewDataColumn FieldName="branch_description" Visible="true" VisibleIndex="3" width="200px" Caption="Branch Name" Settings-AutoFilterCondition="Contains">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataColumn>


           

                                    </Columns>

                                    <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                        <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" />
                                        <SettingsPager Mode="ShowPager">
                                        </SettingsPager>

                                        <SettingsPager PageSize="20">
                                            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,20,50,100,150,200" />
                                        </SettingsPager>

                                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                      
                                    </GridViewProperties>

                                   

                                </dxe:ASPxGridLookup>
                            </dxe:PanelContent>
                        </panelcollection>

                    </dxe:ASPxCallbackPanel>


                </td>--%>

                


            </tr>
            <tr>
                

            </tr>
            <tr>
                <td></td>
                <td>
                    
                </td>

            </tr>

        </table>

        <br />

        <div>
            <asp:Label ID="FileUploadedList" runat="server" Visible="false" />
        </div>



    </div>


</asp:Content>
