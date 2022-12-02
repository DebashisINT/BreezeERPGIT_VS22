<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.Activities.management_activities_frmbulkemail_attachment" CodeBehind="frmbulkemail_attachment.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script language="javascript" type="text/javascript">

        //function is called on changing Selection

        function OnGridServerSelectAll(obj) {
            OnGridSelectionChanged();
        }
        function OnGridSelectionChanged() {
            gridLocal.GetSelectedFieldValues('filepathServer', OnGridSelectionComplete);
        }
        function OnGridSelectionComplete(values) {
            counter = 'n';
            for (var i = 0; i < values.length; i++) {
                if (counter != 'n')
                    counter += ',' + values[i];
                else
                    counter = values[i];
            }
            //alert(counter);
        }
    </script>
    <script type="text/javascript" language="javascript">
        function AtTheTimePageLoad() {
            counter = 'n';
            FieldName = 'chkAttachment';
            document.getElementById("trSave").style.display = 'none';
            document.getElementById("trGrid").style.display = 'none';
            document.getElementById("trAttachment").style.display = 'none';
            document.getElementById("trAttachmentBody").style.display = 'none';
            document.getElementById("trDocument").style.display = 'none';
            var check = document.getElementById("chkAttachment");
            check.checked = false;
            checkAttachmentclick(false);
        }
        function checkAttachmentclick(obj) {
            if (obj == false) {
                document.getElementById("tdBrowse").style.display = 'none';
                document.getElementById("UploadAttachment").style.display = 'none';
            }
            if (obj == true) {
                document.getElementById("tdBrowse").style.display = '';
                document.getElementById("UploadAttachment").style.display = '';
            }
        }

        function UploadFuction(obj) {
            FieldName = 'chkAttachment';
            var DATA = obj.split(',');
            if (DATA[0] == '0') {
                document.getElementById("trSave").style.display = '';
                document.getElementById("trGrid").style.display = '';
                document.getElementById("trAttachment").style.display = '';
                document.getElementById("tdBrowse").style.display = '';
                document.getElementById("UploadAttachment").style.display = '';
                document.getElementById("trAttachmentBody").style.display = 'none';
                document.getElementById("trDocument").style.display = '';
            }
            if (DATA[0] == '1') {
                document.getElementById("trSave").style.display = '';
                document.getElementById("trGrid").style.display = '';
                document.getElementById("trAttachment").style.display = '';
                document.getElementById("tdBrowse").style.display = 'none';
                document.getElementById("UploadAttachment").style.display = 'none';
                document.getElementById("trAttachmentBody").style.display = '';
                document.getElementById("trDocument").style.display = '';
                if (DATA[1] == 'True') {
                    document.getElementById("tdBrowse").style.display = '';
                    document.getElementById("UploadAttachment").style.display = ''
                }
            }
        }
        function btnRemove_click() {
            if (counter != 'n') {
                var senddata = 'remvloc~' + counter;
                gridLocal.PerformCallback(senddata);
                counter = 'n';
            }

        }

    </script>
    <script type="text/ecmascript">

        function SendingMailOption(obj) {

            if (obj != '') {
                var senddata = 'option~' + obj;
                //alert(senddata);
                CallServer(senddata, "");
            }
            else {
                AtTheTimePageLoad();
                var senddata = 'optionnull';
                CallServer(senddata, "");
            }
        }
        function btnSend_click() {
            var senddata = 'send';
            CallServer(senddata, "");
        }
        function ReceiveServerData(rValue) {
            var DATA = rValue.split('~');
            //alert(rValue); 
            if (DATA[0] == "option") {
                if (DATA[1] != 'n') {
                    if (DATA[1] == '1') {
                        document.getElementById("trSave").style.display = '';
                        document.getElementById("trGrid").style.display = 'none';
                        document.getElementById("trAttachment").style.display = '';
                        document.getElementById("tdBrowse").style.display = 'none';
                        document.getElementById("UploadAttachment").style.display = 'none';
                        document.getElementById("trAttachmentBody").style.display = '';
                        document.getElementById("trDocument").style.display = '';
                    }
                    if (DATA[1] == '0') {
                        document.getElementById("trSave").style.display = '';
                        document.getElementById("trGrid").style.display = 'none';
                        document.getElementById("trAttachment").style.display = '';
                        document.getElementById("tdBrowse").style.display = '';
                        document.getElementById("UploadAttachment").style.display = '';
                        document.getElementById("trAttachmentBody").style.display = 'none';
                        document.getElementById("trDocument").style.display = '';
                    }
                }
                else
                    alert('No Data Found!');
            }
            if (DATA[0] == "send") {
                if (DATA[1] == "Y") {
                    AtTheTimePageLoad();
                    alert('Mail Delivered Successfully!');
                }
                else
                    alert(DATA[1]);
            }
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Bulk Email Template</h3>
        </div>

    </div>
    <div class="form_main inner">
        <table class="TableMain100" >

            <tr>
                <td class="gridcellright" align="left" style="width:200px;">
                    <span style="color: #000099">Bulk Email Template:</span>
                </td>
                <td class="gridcellleft"  ><asp:DropDownList ID="cmbBulkEmailTemplate" runat="server" ClientIDMode="Static" class="form-control" Width="250px">
                </asp:DropDownList>
                </td>

            </tr>
            <tr id="trDocument">
                <td class="gridcellright"  align="left">
                    <span style="color: #000099">Document Name:</span>
                </td>
                <td class="gridcellleft" align="left">&nbsp;<asp:TextBox ID="txtDocumentName" runat="server" Font-Size="11px" ClientIDMode="Static" Width="250px"></asp:TextBox>
                </td>
            </tr>
            <tr id="trAttachmentBody">
                <td class="gridcellright">
                    <span style="color: #000099">Browse File For Body:</span>
                </td>
                <td class="gridcellleft" align="left">
                    <table>
                        <tr>
                            <td align="left">
                                <asp:FileUpload ID="UploadBody" runat="server"  ClientIDMode="Static" />
                                <span style="color: #000099">Has Attachment </span>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkAttachment" runat="server" Height="13px" Width="37px" ClientIDMode="Static" />
                            </td>
                            <td>
                                <asp:Label ID="lblMessage" runat="server" Font-Bold="True" ForeColor="Red" ClientIDMode="Static"></asp:Label>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="UploadBody"
                                    Display="Dynamic" ErrorMessage="upload only [.Doc] or [.txt] File !!" ValidationExpression="^.+\.((txt)|(doc))$"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="trAttachment">
                <td class="gridcellright" >
                    <span id="tdBrowse" style="color: #000099">Browse File For Attachment:</span>
                </td>
                <td class="gridcellleft" align="left">&nbsp;<asp:FileUpload ID="UploadAttachment" runat="server"  ClientIDMode="Static" />&nbsp;&nbsp;
                <input id="btnUpload" runat="server" type="button" value="Upload" class="btnUpdate btn btn-success btn-xs mTop5" onserverclick="btnUpload_ServerClick" />
                <input id="btnRemove" type="button" value="Remove" class="btnUpdate btn btn-danger btn-xs mTop5" onclick="btnRemove_click()" /></td>
            </tr>
            <tr id="trGrid">
                <td colspan="2">
                    <dxe:ASPxGridView ID="GridAttachment" ClientInstanceName="gridLocal" runat="server" Width="100%" KeyFieldName="filepathServer" AutoGenerateColumns="False" OnCustomCallback="GridAttachment_CustomCallback" ClientIDMode="Static">
                        <Styles>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                            </Header>
                        </Styles>
                        <SettingsPager AlwaysShowPager="True" NumericButtonCount="20" ShowSeparators="True" Visible="False">
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </SettingsPager>
                        <SettingsBehavior AllowMultiSelection="True" AllowSort="False" />
                        <ClientSideEvents SelectionChanged="function(s, e) { OnGridSelectionChanged(); }" />

                        <Columns>
                            <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="3%">
                                <HeaderStyle HorizontalAlign="Center">
                                    <Paddings PaddingBottom="1px" PaddingTop="1px" />
                                </HeaderStyle>
                                <HeaderTemplate>
                                    <input type="checkbox" onclick="gridServer.SelectAllRowsOnPage(this.checked); OnGridServerSelectAll(this.checked);" style="vertical-align: middle;" title="Select/Unselect all rows on the page"></input>
                                </HeaderTemplate>
                            </dxe:GridViewCommandColumn>
                            <dxe:GridViewDataTextColumn Caption="Document Name" FieldName="filename" ReadOnly="True"
                                VisibleIndex="1">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormSettings Visible="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Doc" FieldName="filepath"
                                ReadOnly="True" VisibleIndex="2">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormSettings Visible="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Document Physical Location" FieldName="filepathServer" ReadOnly="True" VisibleIndex="4" Width="10%">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormSettings Visible="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataColumn Caption="Details" VisibleIndex="8" Width="35%">
                                <DataItemTemplate>
                                    <a href="viewImage.aspx?id=<%#Container.KeyValue %>" target="_blank">View..</a>
                                </DataItemTemplate>
                            </dxe:GridViewDataColumn>


                        </Columns>
                    </dxe:ASPxGridView>
                </td>
            </tr>
            <tr id="tr1">
                <td></td>
                <td class="gridcellleft">&nbsp;</td>
            </tr>
            <tr id="trSave">
                <td></td>
                <td class="gridcellleft">
                    <input id="btnSend" type="button" value="Send" class="btnUpdate btn btn-primary" onclick="btnSend_click()" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
