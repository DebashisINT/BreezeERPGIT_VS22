<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.ToolsUtilities.management_utilities_frm_TemplateMaster" CodeBehind="frm_TemplateMaster.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <script type="text/javascript">

        //function SignOff() {
        //    window.parent.SignOff();
        //}
        //function height() {
        //    if (document.body.scrollHeight >= 500)
        //        window.frameElement.height = document.body.scrollHeight;
        //    else
        //        window.frameElement.height = '500px';
        //    window.frameElement.Width = document.body.scrollWidth;
        //}
        function DeleteRow(keyValue) {
            doIt = confirm('Confirm delete?');
            if (doIt) {
                grid.PerformCallback('Delete~' + keyValue);
                //height();
            }
            else {

            }


        }

        function ShowHideFilter(obj) {

            grid.PerformCallback(obj);
        }


        function OnEditButtonClick(keyValue) {
            var url = '../EmailSetupAddEdit.aspx?id=' + keyValue;
            //OnMoreInfoClick(url, "Add New Accout", '820px', '400px', "Y");
            window.location.href = url;
        }

        function OnAddButtonClick() {
            var url = 'frm_TemplateMasterAddEdit.aspx?id=ADD';
            //OnMoreInfoClick(url, "Add New Template", '950px', '500px', "Y");
            window.location.href = url;
        }

        function OnShowButtonClick(keyValue) {
            var url = 'frm_TemplateTypeAdd.aspx?id=Show&TypeID=' + keyValue;;
            //OnMoreInfoClick(url, "Add New Accout", '820px', '400px', "Y");
            window.location.href = url;
        }



        function OnMoreAccessCick(keyValue) {
            grid.PerformCallback('Access~' + keyValue);
            //height();


        }



        function OnAddEditClick(e, obj) {
            var url = 'frm_TemplateMasterAddEdit.aspx?id=' + obj;
            window.location.href = url;
            //OnMoreInfoClick(url, "Add New Template", '950px', '500px', "Y");
            //        FieldName='ASPxPopupControl1_ASPxCallbackPanel1_drpBranch';
            //        Filter='N';
            //        RowID='';
            //        var data=obj.split('~');
            //        if(data.length>1)
            //            RowID=data[1];
            //        popup.Show();
            //        popPanel.PerformCallback(obj);
        }

        function callback() {
            //               var applicant=document.getElementById("ASPxPopupControl1_ASPxCallbackPanel1_txtApplicant_hidden").value;
            //               AppBank.PerformCallback(applicant);
            //               cmbSubSequentBank.PerformCallback(applicant);
            grid.PerformCallback();
        }


        function OnDeleteClick(e, obj) {
            if (confirm('Are You Sure you want to Delete This Transaction?') == true) {
                grid.PerformCallback(obj);
            }
        }
        function btnSave_Click() {

            Filter = 'Y';
            if (RowID == '') {
                var TemplateName = document.getElementById("ASPxPopupControl1_ASPxCallbackPanel1_txtShortName").value;
                var Content = document.getElementById("ASPxPopupControl1_ASPxCallbackPanel1_txtContent").value;

                if (TemplateName == '') {
                    alert('Template Name is Required.')
                    return false;
                }
                else if (Content == '') {
                    alert('Content can not be blank.')
                    return false;
                }
                else {
                    var obj = 'SaveNew';
                    popPanel.PerformCallback(obj);

                }

            }
            else {
                var obj = 'SaveOld~' + RowID;
                popPanel.PerformCallback(obj);
            }

        }
        function EndCallBack(obj) {
            if (obj != '') {
                var data = obj.split('~');
                if (data[0] == 'Edit') {

                }
            }
            if (Filter == 'Y') {
                popup.Hide();
                grid.PerformCallback();
            }
        }

        function calldispose() {
            var type = document.getElementById("ASPxPopupControl1_ASPxCallbackPanel1_cmbType").value;
            var y = (screen.availHeight - 450) / 2;
            var x = (screen.availWidth - 900) / 2;
            var str = 'frm_TemplateReservedWord.aspx?Type=' + type;
            window.open(str, "Search_Conformation_Box", "height=350,width=700,top=" + y + ",left=" + x + ",location=no,directories=no,menubar=no,toolbar=no,status=yes,scrollbars=no,resizable=no,dependent=no");


        }


        function btnCancel_Click() {
            popup.Hide();
        }

        function OnAddButtonClick() {
            var url = 'frm_TemplateMasterAddEdit.aspx?id=ADD';
            //OnMoreInfoClick(url, "Add New Template", '950px', '500px', "Y");
            window.location.href = url;
        }
    </script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Template Master</h3>
        </div>
    </div>
    <div class="form_main" style="height: 800px;">
        <table class="TableMain100">

            <tr>
                <td>
                    <table>
                        <tr>
                            <td id="ShowFilter">
                                <%--<% if (rights.CanAdd)
                                               { %>--%>
                                <a href="javascript:void(0);" onclick="javascript:OnAddButtonClick();" class="btn btn-primary"><span>Add New</span> </a>
                                <%--<%} %>--%>
                                <%--<a href="javascript:ShowHideFilter('s');" class="btn btn-success"><span>Show Filter</span></a>--%>
                            </td>
                            <td id="Td1">
                                <%--<a href="javascript:ShowHideFilter('s');" class="btn btn-success"><span>Show Filter</span></a>--%>
                                <%--  <a href="javascript:ShowHideFilter('All');" class="btn btn-primary"><span>All Records</span></a>--%>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <dxe:ASPxGridView ID="gridStatus" ClientInstanceName="grid" Width="100%" KeyFieldName="Tmplt_ID"
                        DataSourceID="gridStatusDataSource" runat="server" AutoGenerateColumns="False"
                        OnCustomCallback="gridStatus_CustomCallback">
                        <SettingsBehavior AllowFocusedRow="false" ConfirmDelete="True" />
                        <Styles>
                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                            </Header>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                            <%--  <FocusedRow BackColor="#FEC6AB">
                            </FocusedRow>--%>
                        </Styles>
                        <Columns>
                            <dxe:GridViewDataTextColumn Visible="False" FieldName="Tmplt_ID" Caption="ID">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="0" FieldName="Tmplt_UsedFor" Caption="Template Used For">
                                <CellStyle Wrap="True" CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="Tmplt_ShortName" Caption="Template Description">
                                <CellStyle Wrap="True" CssClass="gridcellleft">
                                </CellStyle>
                                <%--  <EditFormSettings Visible="False"></EditFormSettings>--%>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="12" Width="6%">
                                <DataItemTemplate>
                                    <a href="javascript:void(0);" onclick="OnAddEditClick(this,'<%# Container.KeyValue %>')" class="pad">
                                        <img src="/assests/images/Edit.png" /></a>
                                    <a href="javascript:void(0);" onclick="OnDeleteClick(this,'Delete~'+'<%# Container.KeyValue %>')">
                                        <img src="/assests/images/Delete.png" />
                                    </a>
                                    <%--<a href="javascript:void(0);" onclick="btnDetail_Click('<%# Container.KeyValue %>')">
                                                            <u>Detail</u> </a>--%>
                                </DataItemTemplate>
                                <%--  <EditFormSettings Visible="False" />--%>
                                <CellStyle Wrap="False" HorizontalAlign="Center">
                                </CellStyle>
                                <HeaderTemplate>
                                    <%--<a href="javascript:void(0);" onclick="OnAddEditClick(this,'AddNew')"><u>Add New</u>
                                        </a>--%>
                                    <%--<a href="javascript:void(0);" onclick="javascript:OnAddButtonClick();"><span style="text-decoration: underline">Add New</span> </a>--%>
                                    Actions
                                </HeaderTemplate>
                                <HeaderStyle Wrap="False" HorizontalAlign="Center" />
                            </dxe:GridViewDataTextColumn>








                            <%-- <dxe:GridViewDataTextColumn VisibleIndex="13">
                                                <DataItemTemplate>
                                                    <a href="javascript:void(0);" onclick="OnDeleteClick(this,'Delete~'+'<%# Container.KeyValue %>')">
                                                        <u>Delete</u> </a>
                                                </DataItemTemplate>
                                                <EditFormSettings Visible="False" />
                                                <CellStyle CssClass="gridcellleft">
                                                </CellStyle>
                                            </dxe:GridViewDataTextColumn>--%>
                            <%--<dxe:GridViewDataTextColumn VisibleIndex="2" ><DataItemTemplate>
                                
                     
                                                <a href="javascript:void(0);" onclick="DeleteRow('<%# Container.KeyValue %>')">
                                                Delete</a>&nbsp;                                  
                                    
</DataItemTemplate>

<CellStyle CssClass="gridcellleft"></CellStyle>
<HeaderTemplate>
                                        <a href="javascript:void(0);" onclick="javascript:OnAddButtonClick();"><span style="color: #000099;
                                            text-decoration: underline">Add Template</span> </a>
                                    
</HeaderTemplate>

<EditFormSettings Visible="False"></EditFormSettings>
</dxe:GridViewDataTextColumn>--%>
                        </Columns>
                        <SettingsCommandButton>


                            <EditButton Image-Url="/assests/images/Edit.png" ButtonType="Image" Image-AlternateText="Edit" Styles-Style-CssClass="pad">
                            </EditButton>
                            <DeleteButton Image-Url="/assests/images/Delete.png" ButtonType="Image" Image-AlternateText="Delete">
                            </DeleteButton>
                            <UpdateButton Text="Update" ButtonType="Button" Styles-Style-CssClass="btn btn-primary btn-xs"></UpdateButton>
                            <CancelButton Text="Cancel" ButtonType="Button" Styles-Style-CssClass="btn btn-danger btn-xs"></CancelButton>
                        </SettingsCommandButton>
                        <StylesEditors>
                            <ProgressBar Height="25px">
                            </ProgressBar>
                        </StylesEditors>
                        <SettingsBehavior AllowFocusedRow="false" AllowSort="False" AllowMultiSelection="True" />
                        <%--<SettingsPager AlwaysShowPager="True" NumericButtonCount="20" ShowSeparators="True"
                            PageSize="15">
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </SettingsPager>--%>
                        <SettingsText Title="Template" />
                        <SettingsSearchPanel Visible="True" />
                        <Settings ShowGroupedColumns="True" ShowGroupPanel="True" ShowFilterRow="true" ShowFilterRowMenu="true" />
                    </dxe:ASPxGridView>
                    <asp:SqlDataSource ID="gridStatusDataSource" runat="server" 
                        SelectCommand="">
                        <SelectParameters>
                            <asp:SessionParameter Name="userlist" SessionField="userchildHierarchy" Type="string" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </td>
            </tr>
            <tr>
                <td>
                    <dxe:ASPxPopupControl ID="ASPxPopupControl1" ClientInstanceName="popup" runat="server"
                        AllowDragging="True" PopupHorizontalAlign="OutsideRight" HeaderText="Template Master"
                        EnableHotTrack="False" BackColor="#DDECFE" Width="100%" CloseAction="CloseButton">
                        <ContentCollection>
                            <dxe:PopupControlContentControl runat="server">
                                <dxe:ASPxCallbackPanel ID="ASPxCallbackPanel1" runat="server" Width="100%" ClientInstanceName="popPanel"
                                    OnCallback="ASPxCallbackPanel1_Callback" OnCustomJSProperties="ASPxCallbackPanel1_CustomJSProperties">
                                    <PanelCollection>
                                        <dxe:PanelContent runat="server">
                                            <table class="TableMain100">
                                                <tr>
                                                    <td class="gridcellleft">
                                                        <span class="Ecoheadtxt">Template Used For</span>
                                                    </td>
                                                    <td class="gridcellleft">
                                                        <asp:DropDownList ID="cmbType" runat="server" Width="300px" Font-Size="11px" TabIndex="6">
                                                            <asp:ListItem Value="AL" Text="Both"></asp:ListItem>
                                                            <asp:ListItem Value="EM" Text="Employee"></asp:ListItem>
                                                            <asp:ListItem Value="CL" Text="Client"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td class="gridcellleft">
                                                        <span class="Ecoheadtxt">Short Name:</span>
                                                    </td>
                                                    <td class="gridcellleft">
                                                        <asp:TextBox ID="txtShortName" runat="server" Width="300px" Font-Size="11px" TabIndex="1"></asp:TextBox>
                                                    </td>
                                                </tr>


                                                <tr id="TrUploadFile">
                                                    <td class="gridcellleft">
                                                        <span class="Ecoheadtxt">Content</span>
                                                    </td>
                                                    <td class="gridcellleft">
                                                        <br />

                                                        <%-- <asp:LinkButton ID="btnReserve" runat="server" Text="Use Reserved Word"></asp:LinkButton>              --%>
                                                        <a id="btnReserve" runat="server" style="cursor: pointer;"><span style="color: #000099; font-size: 8pt; text-decoration: underline;">Use Reserved Variable</span></a>
                                                        <asp:TextBox TextMode="MultiLine" ID="txtContent" runat="server" Width="750px" Height="370px" Font-Size="11px"
                                                            TabIndex="2"></asp:TextBox>
                                                    </td>
                                                </tr>



                                                <tr>
                                                    <td></td>
                                                    <td colspan="2" class="gridcellleft">
                                                        <input id="Button1" type="button" value="Save" class="btnUpdate" onclick="btnSave_Click()"
                                                            style="width: 60px" tabindex="41" />
                                                        <input id="Button2" type="button" value="Cancel" class="btnUpdate" onclick="btnCancel_Click()"
                                                            style="width: 60px" tabindex="42" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </dxe:PanelContent>
                                    </PanelCollection>
                                    <ClientSideEvents EndCallback="function(s, e) {
	                                                    EndCallBack(s.cpLast);
                                                    }" />
                                </dxe:ASPxCallbackPanel>
                            </dxe:PopupControlContentControl>
                        </ContentCollection>
                        <HeaderStyle HorizontalAlign="Left">
                            <Paddings PaddingRight="6px" />
                        </HeaderStyle>
                        <SizeGripImage Height="16px" Width="16px" />
                        <CloseButtonImage Height="12px" Width="13px" />
                        <ClientSideEvents CloseButtonClick="function(s, e) {
	 popup.Hide();
}" />
                    </dxe:ASPxPopupControl>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
