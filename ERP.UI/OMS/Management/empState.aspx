<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="empState.aspx.cs" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.empState" %>

<%--<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>

<%@ Register Assembly="DevExpress.Web.v15.1" Namespace="DevExpress.Web"
    TagPrefix="dxe000001" %>
<%@ Register Assembly="DevExpress.Web.v15.1" Namespace="DevExpress.Web"
    TagPrefix="dxpc" %>
<%@ Register Assembly="DevExpress.Web.v15.1" Namespace="DevExpress.Web" TagPrefix="dx" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />
    <link href="../CentralData/CSS/GenericCss.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="/assests/js/loaddata1.js"></script>

    <script type="text/javascript" src="../CentralData/JSScript/GenericJScript.js"></script>

    <script language="javascript" type="text/javascript">
        function SignOff() {
            window.parent.SignOff();
        }
        function height() {
            if (document.body.scrollHeight >= 500)
                window.frameElement.height = document.body.scrollHeight;
            else
                window.frameElement.height = '500px';
            window.frameElement.Width = document.body.scrollWidth;
        }
    </script>

    <script type="text/javascript">
        //function is called on changing country
        function OnCountryChanged(cmbCountry) {
            grid.GetEditor("cou_country").PerformCallback(cmbCountry.GetValue().toString());
        }
        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }
    </script>

    <%-- added by krishnendu--%>

    <script type="text/javascript">
        function fn_PopOpen() {
            document.getElementById('<%=hiddenedit.ClientID%>').value = '';
            //            alert('HidenEdit-'+GetObjectID('<%=hiddenedit.ClientID%>').value);
            ctxtStateName.SetText('');
            ctxtNseCode.SetText('');
            ctxtBseCode.SetText('');
            ctxtMcxCode.SetText('');
            ctxtMcsxCode.SetText('');
            ctxtNcdexCode.SetText('');
            ctxtCdslCode.SetText('');
            ctxtNsdlCode.SetText('');
            ctxtNdmlCode.SetText('');
            ctxtDotexidCode.SetText('');
            ctxtCvlidCode.SetText('');
            cPopup_EmpStates.Show();
        }
        function btnSave_States() {
            if (ctxtStateName.GetText() == '') {
                alert('Please Enter State Name');
                ctxtStateName.Focus();
            }
            else {
                if (document.getElementById('<%=hiddenedit.ClientID%>').value == '')
                    grid.PerformCallback('savestate~' + ctxtStateName.GetText() + '~' + cCmbCountryName.GetText() + '~' + ctxtNseCode.GetText() + '~' + ctxtBseCode.GetText() + '~' + ctxtMcxCode.GetText() + '~' + ctxtMcsxCode.GetText() + '~' + ctxtNcdexCode.GetText() + '~' + ctxtCdslCode.GetText() + '~' + ctxtNsdlCode.GetText() + '~' + ctxtNdmlCode.GetText() + '~' + ctxtDotexidCode.GetText() + '~' + ctxtCvlidCode.GetText());
                else
                    grid.PerformCallback('updatestate~' + GetObjectID('<%=hiddenedit.ClientID%>').value);
                //                 grid.PerformCallback('updatestate~'+ctxtStateName.GetText()+'~'+ cCmbCountryName.GetText()+'~'+GetObjectID('hiddenedit').value);
            }
        }
        function fn_btnCancel() {
            cPopup_EmpStates.Hide();
        }
        function fn_EditState(keyValue) {
            grid.PerformCallback('Edit~' + keyValue);
        }
        function fn_DeleteState(keyValue) {
            grid.PerformCallback('Delete~' + keyValue);
        }
        function grid_EndCallBack() {
            if (grid.cpinsert != null) {
                if (grid.cpinsert == 'Success') {
                    alert('Inserted Successfully');
                    cPopup_EmpStates.Hide();
                }
                else {
                    alert("Error On Insertion\n'Please Try Again!!'");
                }
            }

            if (grid.cpEdit != null) {
                ctxtStateName.SetText(grid.cpEdit.split('~')[0]);
                cCmbCountryName.SetValue(grid.cpEdit.split('~')[1]);
                ctxtNseCode.SetValue(grid.cpEdit.split('~')[2]);
                ctxtBseCode.SetValue(grid.cpEdit.split('~')[3]);
                ctxtMcxCode.SetValue(grid.cpEdit.split('~')[4]);
                ctxtMcsxCode.SetValue(grid.cpEdit.split('~')[5]);
                ctxtNcdexCode.SetValue(grid.cpEdit.split('~')[6]);
                ctxtCdslCode.SetValue(grid.cpEdit.split('~')[7]);
                ctxtNsdlCode.SetValue(grid.cpEdit.split('~')[8]);
                ctxtNdmlCode.SetValue(grid.cpEdit.split('~')[9]);
                ctxtDotexidCode.SetValue(grid.cpEdit.split('~')[10]);
                ctxtCvlidCode.SetValue(grid.cpEdit.split('~')[11]);
                GetObjectID('<%=hiddenedit.ClientID%>').value = grid.cpEdit.split('~')[12];
                 cPopup_EmpStates.Show();
             }

             if (grid.cpUpdate != null) {
                 if (grid.cpUpdate == 'Success') {
                     alert('Update Successfully');
                     cPopup_EmpStates.Hide();
                 }
                 else
                     alert("Error on Updation\n'Please Try again!!'")
             }

             if (grid.cpDelete != null) {
                 if (grid.cpDelete == 'Success')
                     alert('Deleted Successfully');
                 else
                     alert("Error on deletion\n'Please Try again!!'")
             }

             if (grid.cpExists != null) {
                 if (grid.cpExists == "Exists") {
                     alert('Record already Exists');
                     cPopup_EmpStates.Hide();
                 }
                 else
                     alert("Error on operatio\n'Please Try again!!'")
             }

         }
    </script>

    <style type="text/css">
        .stateDiv {
            height: 25px;
            width: 130px;
            float: left;
            margin-left: 70px;
        }

        .StateTextbox {
            height: 25px;
            width: 50px;
        }

        .Top {
            height: 80px;
            width: 400px;
            background-color: Silver;
            padding-top: 5px;
        }

        .Footer {
            height: 30px;
            width: 400px;
            background-color: Silver;
            padding-top: 10px;
        }

        .ScrollDiv {
            height: 250px;
            width: 400px;
            background-color: Silver;
            overflow-x: hidden;
            overflow-y: scroll;
        }

        .ContentDiv {
            width: 400px;
            height: 300px;
            border: 2px;
            background-color: Silver;
        }

        .TitleArea {
            height: 20px;
            padding-left: 10px;
            padding-right: 3px;
            background-image: url( '../images/EHeaderBack.gif' );
            background-repeat: repeat-x;
            background-position: bottom;
            text-align: center;
        }

        .FilterSide {
            float: left;
            padding-left: 15px;
            width: 50%;
        }

        .SearchArea {
            width: 100%;
            height: 30px;
            padding-top: 5px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <div class="Main">
            <div class="TitleArea">
                <strong><span style="color: #000099">State List</span></strong>
            </div>
            <div class="SearchArea">
                <div class="FilterSide">
                    <div style="float: left; padding-right: 5px;">
                        <a href="javascript:ShowHideFilter('s');"><span style="color: #000099; text-decoration: underline">Show Filter</span></a>
                    </div>
                    <div>
                        <a href="javascript:ShowHideFilter('All');"><span style="color: #000099; text-decoration: underline">All Records</span></a>
                    </div>
                </div>
                <div class="ExportSide">
                    <div style="margin-left: 90%;">
                        <dxe:ASPxComboBox ID="cmbExport" runat="server" AutoPostBack="true" BackColor="Navy"
                            Font-Bold="False" ForeColor="White" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged"
                            ValueType="System.Int32" Width="130px">
                            <Items>
                                <dxe:ListEditItem Text="Select" Value="0" />
                                <dxe:ListEditItem Text="PDF" Value="1" />
                                <dxe:ListEditItem Text="XLS" Value="2" />
                                <dxe:ListEditItem Text="RTF" Value="3" />
                                <dxe:ListEditItem Text="CSV" Value="4" />
                            </Items>
                            <ButtonStyle BackColor="#C0C0FF" ForeColor="Black">
                            </ButtonStyle>
                            <ItemStyle BackColor="Navy" ForeColor="White">
                                <HoverStyle BackColor="#8080FF" ForeColor="White">
                                </HoverStyle>
                            </ItemStyle>
                            <Border BorderColor="White" />
                            <DropDownButton Text="Export">
                            </DropDownButton>
                        </dxe:ASPxComboBox>
                    </div>
                </div>
            </div>
        </div>

        <div class="GridViewArea">
            <dxe:ASPxGridView ID="StateGrid" runat="server" AutoGenerateColumns="False" ClientInstanceName="grid"
                KeyFieldName="id" Width="100%" OnHtmlRowCreated="StateGrid_HtmlRowCreated" OnHtmlEditFormCreated="StateGrid_HtmlEditFormCreated"
                OnCustomCallback="StateGrid_CustomCallback">
                <Columns>
                    <dxe:GridViewDataTextColumn Caption="ID" FieldName="id" ReadOnly="True" Visible="False"
                        FixedStyle="Left" VisibleIndex="0">
                        <EditFormSettings Visible="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="State" FieldName="state" Width="8%" FixedStyle="Left"
                        Visible="True" VisibleIndex="1">
                        <EditFormSettings Visible="True" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Country Name" FieldName="countryId" Visible="False"
                        Width="8%" VisibleIndex="2">
                        <CellStyle CssClass="gridcellleft" Wrap="False">
                        </CellStyle>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="NSECode" FieldName="State_NSECode" Visible="True"
                        Width="8%" VisibleIndex="3">
                        <CellStyle CssClass="gridcellleft" Wrap="False">
                        </CellStyle>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="BSECode" FieldName="State_BSECode" Visible="True"
                        Width="8%" VisibleIndex="4">
                        <CellStyle CssClass="gridcellleft" Wrap="False">
                        </CellStyle>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="MCXCode" FieldName="State_MCXCode" Visible="True"
                        Width="8%" VisibleIndex="5">
                        <CellStyle CssClass="gridcellleft" Wrap="False">
                        </CellStyle>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="MCXSXCode" FieldName="State_MCXSXCode" Visible="True"
                        Width="8%" VisibleIndex="6">
                        <CellStyle CssClass="gridcellleft" Wrap="False">
                        </CellStyle>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="NCDEX" FieldName="State_NCDEXCode" Visible="True"
                        Width="8%" VisibleIndex="7">
                        <CellStyle CssClass="gridcellleft" Wrap="False">
                        </CellStyle>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="CdslID" FieldName="State_CdslID" Visible="True"
                        Width="8%" VisibleIndex="8">
                        <CellStyle CssClass="gridcellleft" Wrap="False">
                        </CellStyle>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="NsdlID" FieldName="State_NsdlID" Visible="True"
                        Width="8%" VisibleIndex="9">
                        <CellStyle CssClass="gridcellleft" Wrap="False">
                        </CellStyle>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="NDMLId" FieldName="State_NDMLId" Visible="True"
                        Width="8%" VisibleIndex="10">
                        <CellStyle CssClass="gridcellleft" Wrap="False">
                        </CellStyle>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="DotExID" FieldName="State_DotExID" Visible="True"
                        Width="8%" VisibleIndex="11">
                        <CellStyle CssClass="gridcellleft" Wrap="False">
                        </CellStyle>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="CVLID" FieldName="State_CVLID" Visible="True"
                        Width="8%" VisibleIndex="12">
                        <CellStyle CssClass="gridcellleft" Wrap="False">
                        </CellStyle>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn ReadOnly="True">
                        <HeaderTemplate>
                            <a href="javascript:void(0);" onclick="fn_PopOpen()"><span style="color: #000099; text-decoration: underline">Add New</span> </a>
                        </HeaderTemplate>
                        <DataItemTemplate>
                            <a href="javascript:void(0);" onclick="fn_EditState('<%# Container.KeyValue %>')">Edit</a>
                            <a href="javascript:void(0);" onclick="fn_DeleteState('<%# Container.KeyValue %>')">Delete</a>
                        </DataItemTemplate>
                    </dxe:GridViewDataTextColumn>
                </Columns>
                <ClientSideEvents EndCallback="function (s, e) {grid_EndCallBack();}" />
            </dxe:ASPxGridView>
        </div>

        <%--added by krishnendu--%>
        <div class="PopUpArea">
            <dxe:ASPxPopupControl ID="Popup_EmpStates" runat="server" ClientInstanceName="cPopup_EmpStates"
                Width="400px" HeaderText="Add States Details" PopupHorizontalAlign="WindowCenter"
                Height="475px" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton" Modal="True">
                <ContentCollection>
                    <dxe:PopupControlContentControl ID="SRPopupControlContentControl" runat="server">
                        <div class="Top">
                            <div>
                                <div class="stateDiv">
                                    State &nbsp;
                                </div>
                                <div>
                                    <dxe:ASPxTextBox ID="txtStateName" ClientInstanceName="ctxtStateName" ClientEnabled="true"
                                        runat="server" Height="25px" Width="180px">
                                    </dxe:ASPxTextBox>
                                </div>
                            </div>
                            <br style="clear: both;" />
                            <div>
                                <div class="stateDiv">
                                    Country Name &nbsp;
                                </div>
                                <div>
                                    <dxe:ASPxComboBox ID="CmbCountryName" ClientInstanceName="cCmbCountryName" runat="server"
                                        Width="180px" Height="25px" ValueType="System.String" AutoPostBack="false" EnableSynchronization="False"
                                        SelectedIndex="0">
                                    </dxe:ASPxComboBox>
                                </div>
                            </div>
                        </div>
                        <div class="ContentDiv">
                            <div style="height: 20px; width: 280px; background-color: Gray; padding-left: 120px;">
                                <h5>Static Code</h5>
                            </div>
                            <div style="background-color: Gray; overflow: hidden">
                                <div style="height: 20px; width: 130px; float: left; margin-left: 70px;">Exchange</div>
                                <div style="height: 20px; width: 200px; text-align: left; margin-left: 50px;">
                                    Value
                                </div>
                            </div>
                            <div class="ScrollDiv">
                                <div class="stateDiv" style="padding-top: 5px;">
                                    NSE Code
                                </div>
                                <div style="padding-top: 5px;">
                                    <dxe:ASPxTextBox ID="txtNseCode" ClientInstanceName="ctxtNseCode" ClientEnabled="true"
                                        runat="server" CssClass="StateTextbox">
                                    </dxe:ASPxTextBox>
                                </div>
                                <br style="clear: both;" />
                                <div class="stateDiv">
                                    BSE Code
                                </div>
                                <div>
                                    <dxe:ASPxTextBox ID="txtBseCode" ClientInstanceName="ctxtBseCode" ClientEnabled="true"
                                        runat="server" CssClass="StateTextbox">
                                    </dxe:ASPxTextBox>
                                </div>
                                <br style="clear: both;" />
                                <div class="stateDiv">
                                    MCX Code
                                </div>
                                <div>
                                    <dxe:ASPxTextBox ID="txtMcxCode" ClientInstanceName="ctxtMcxCode" ClientEnabled="true"
                                        runat="server" CssClass="StateTextbox">
                                    </dxe:ASPxTextBox>
                                </div>
                                <br style="clear: both;" />
                                <div class="stateDiv">
                                    MCXSX Code
                                </div>
                                <div>
                                    <dxe:ASPxTextBox ID="txtMcsxCode" ClientInstanceName="ctxtMcsxCode" ClientEnabled="true"
                                        runat="server" CssClass="StateTextbox">
                                    </dxe:ASPxTextBox>
                                </div>
                                <br style="clear: both;" />
                                <div class="stateDiv">
                                    NCDEX Code
                                </div>
                                <div>
                                    <dxe:ASPxTextBox ID="txtNcdexCode" ClientInstanceName="ctxtNcdexCode" ClientEnabled="true"
                                        runat="server" CssClass="StateTextbox">
                                    </dxe:ASPxTextBox>
                                </div>
                                <br style="clear: both;" />
                                <div class="stateDiv">
                                    CDSL Code
                                </div>
                                <div>
                                    <dxe:ASPxTextBox ID="txtCdslCode" ClientInstanceName="ctxtCdslCode" ClientEnabled="true"
                                        CssClass="StateTextbox" runat="server">
                                    </dxe:ASPxTextBox>
                                </div>
                                <br style="clear: both;" />
                                <div class="stateDiv">
                                    NSDL Code
                                </div>
                                <div>
                                    <dxe:ASPxTextBox ID="txtNsdlCode" ClientInstanceName="ctxtNsdlCode" ClientEnabled="true"
                                        CssClass="StateTextbox" runat="server">
                                    </dxe:ASPxTextBox>
                                </div>
                                <br style="clear: both;" />
                                <div class="stateDiv">
                                    NDML Code
                                </div>
                                <div>
                                    <dxe:ASPxTextBox ID="txtNdmlCode" ClientInstanceName="ctxtNdmlCode" ClientEnabled="true"
                                        runat="server" CssClass="StateTextbox">
                                    </dxe:ASPxTextBox>
                                </div>
                                <br style="clear: both;" />
                                <div class="stateDiv">
                                    DOTEXID Code
                                </div>
                                <div>
                                    <dxe:ASPxTextBox ID="txtDotexidCode" ClientInstanceName="ctxtDotexidCode" ClientEnabled="true"
                                        runat="server" CssClass="StateTextbox">
                                    </dxe:ASPxTextBox>
                                </div>
                                <br style="clear: both;" />
                                <div class="stateDiv">
                                    CVLID Code
                                </div>
                                <div>
                                    <dxe:ASPxTextBox ID="txtCvlidCode" ClientInstanceName="ctxtCvlidCode" ClientEnabled="true"
                                        runat="server" CssClass="StateTextbox">
                                    </dxe:ASPxTextBox>
                                </div>
                            </div>
                            <br style="clear: both;" />
                            <div class="Footer">
                                <div style="margin-left: 130px; width: 70px; float: left;">
                                    <dxe:ASPxButton ID="btnSave_States" ClientInstanceName="cbtnSave_States" runat="server"
                                        AutoPostBack="False" Text="Save">
                                        <ClientSideEvents Click="function (s, e) {btnSave_States();}" />
                                    </dxe:ASPxButton>
                                </div>
                                <div style="">
                                    <dxe:ASPxButton ID="btnCancel_States" runat="server" AutoPostBack="False" Text="Cancel">
                                        <ClientSideEvents Click="function (s, e) {fn_btnCancel();}" />
                                    </dxe:ASPxButton>
                                </div>
                                <br style="clear: both;" />
                            </div>
                            <br style="clear: both;" />
                        </div>
                    </dxe:PopupControlContentControl>
                </ContentCollection>
                <HeaderStyle BackColor="LightGray" ForeColor="Black" />
            </dxe:ASPxPopupControl>
            <dxe:ASPxGridViewExporter ID="exporter" runat="server">
            </dxe:ASPxGridViewExporter>
        </div>
        <div class="HiddenFieldArea" style="display: none;">
            <asp:HiddenField runat="server" ID="hiddenedit" />
        </div>
    </div>
</asp:Content>
