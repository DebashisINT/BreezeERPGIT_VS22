<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.management_City" CodeBehind="City.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">   

    <script type="text/javascript" src="../CentralData/JSScript/GenericJScript.js"></script>

    <script language="javascript" type="text/javascript">
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
    </script>

    <script type="text/javascript">
        //function is called on changing country
        //        function OnCountryChanged(cmbCountry) 
        //        {
        //            grid.GetEditor("cou_country").PerformCallback(cmbCountry.GetValue().toString());
        //        }
        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }
    </script>

    <script type="text/javascript">
        function fn_PopOpen() {
            document.getElementById('<%=hiddenedit.ClientID%>').value = '';
            //            alert('HidenEdit-'+GetObjectID('<%=hiddenedit.ClientID%>').value);
            ctxtcityName.SetText('');
            cCmbCountryName.SetValue("1");
            cCmbState.SetSelectedIndex(0);
            ctxtNseCode.SetText('');
            ctxtBseCode.SetText('');
            ctxtMcxCode.SetText('');
            ctxtMcsxCode.SetText('');
            ctxtNcdexCode.SetText('');
            ctxtCdslCode.SetText('');
            ctxtNsdlCode.SetText('');
            ctxtNdmlCode.SetText('');
            ctxtDotexCode.SetText('');
            ctxtCvlCode.SetText('');
            cPopup_Empcitys.Show();
        }
        function btnSave_citys() {
            if (ctxtcityName.GetText() == '') {
                alert('Please Enter City Name');
                ctxtcityName.Focus();
            }
            else {
                if (document.getElementById('<%=hiddenedit.ClientID%>').value == '')
                    grid.PerformCallback('savecity~');
                else
                    grid.PerformCallback('updatecity~' + GetObjectID('<%=hiddenedit.ClientID%>').value);
            }
        }
        function fn_btnCancel() {
            cPopup_Empcitys.Hide();
        }
        function fn_Editcity(keyValue) {
            grid.PerformCallback('Edit~' + keyValue);
        }
        function fn_Deletecity(keyValue) {
            grid.PerformCallback('Delete~' + keyValue);
        }
        function grid_EndCallBack() {
            if (grid.cpinsert != null) {
                if (grid.cpinsert == 'Success') {
                    alert('Inserted Successfully');
                    cPopup_Empcitys.Hide();
                }
                else {
                    alert("Error On Insertion \n 'Please Try Again!!'");
                    cPopup_Empcitys.Hide();
                }
            }
            if (grid.cpEdit != null) {
                ctxtcityName.SetText(grid.cpEdit.split('~')[0]);
                cCmbState.SetValue(grid.cpEdit.split('~')[1]);
                cCmbCountryName.SetValue(grid.cpEdit.split('~')[2]);
                ctxtNseCode.SetText(grid.cpEdit.split('~')[3]);
                ctxtBseCode.SetText(grid.cpEdit.split('~')[4]);
                ctxtMcxCode.SetText(grid.cpEdit.split('~')[5]);
                ctxtMcsxCode.SetText(grid.cpEdit.split('~')[6]);
                ctxtNcdexCode.SetText(grid.cpEdit.split('~')[7]);
                ctxtCdslCode.SetText(grid.cpEdit.split('~')[8]);
                ctxtNsdlCode.SetText(grid.cpEdit.split('~')[9]);
                ctxtNdmlCode.SetText(grid.cpEdit.split('~')[10]);
                ctxtCvlCode.SetText(grid.cpEdit.split('~')[11]);
                ctxtDotexCode.SetText(grid.cpEdit.split('~')[12]);
                GetObjectID('<%=hiddenedit.ClientID%>').value = grid.cpEdit.split('~')[13];
                cPopup_Empcitys.Show();
            }
            if (grid.cpUpdate != null) {
                if (grid.cpUpdate == 'Success') {
                    alert('Update Successfully');
                    cPopup_Empcitys.Hide();
                }
                else {
                    alert("Error on Updation\n'Please Try again!!'")
                    cPopup_Empcitys.Hide();
                }
            }
            if (grid.cpUpdateValid != null) {
                if (grid.cpUpdateValid == "StateInvalid") {
                    alert("Please Select State");
                    //cPopup_Empcitys.Show();
                    //cCmbState.Focus();
                    //alert(GetObjectID('<%=hiddenedit.ClientID%>').value);
                    //grid.PerformCallback('Edit~'+GetObjectID('<%=hiddenedit.ClientID%>').value);
                    //grid.cpUpdateValid=null;
                }
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
                    cPopup_Empcitys.Hide();
                }
                else {
                    alert("Error on operation \n 'Please Try again!!'")
                    cPopup_Empcitys.Hide();
                }
            }
        }
        function OnCmbCountryName_ValueChange() {
            cCmbState.PerformCallback("BindState~" + cCmbCountryName.GetValue());
        }
        function CmbState_EndCallback() {
            cCmbState.SetSelectedIndex(0);
            cCmbState.Focus();
        }
    </script>

    <style type="text/css">
        .cityDiv {
            height: 25px;
            width: 130px;
            float: left;
            margin-left: 70px;
        }

        .cityTextbox {
            height: 25px;
            width: 50px;
        }

        .Top {
            height: 90px;
            width: 400px;
            background-color: Silver;
            padding-top: 5px;
            valign: top;
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

        .pad {
            padding: 10px;
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
                <strong><span style="color: #000099">City List</span></strong>
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
            <dxe:ASPxGridView ID="cityGrid" runat="server" AutoGenerateColumns="False" ClientInstanceName="grid"
                KeyFieldName="city_id" Width="100%" OnHtmlRowCreated="cityGrid_HtmlRowCreated"
                OnHtmlEditFormCreated="cityGrid_HtmlEditFormCreated" OnCustomCallback="cityGrid_CustomCallback">
                <Columns>
                    <dxe:GridViewDataTextColumn Caption="CityID" FieldName="city_id" ReadOnly="True"
                        Visible="False" FixedStyle="Left" VisibleIndex="0">
                        <EditFormSettings Visible="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="StateID" FieldName="state_id" ReadOnly="True"
                        Visible="False" FixedStyle="Left" VisibleIndex="1">
                        <EditFormSettings Visible="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="CountryID" FieldName="cou_id" ReadOnly="True"
                        Visible="False" FixedStyle="Left" VisibleIndex="2">
                        <EditFormSettings Visible="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="City" FieldName="city_name" Width="12%" FixedStyle="Left"
                        Visible="True" VisibleIndex="3">
                        <EditFormSettings Visible="True" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="State" FieldName="state" Width="12%" FixedStyle="Left"
                        Visible="True" VisibleIndex="4">
                        <EditFormSettings Visible="True" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Country" FieldName="cou_country" Visible="True"
                        Width="7%" VisibleIndex="5">
                        <CellStyle CssClass="gridcellleft" Wrap="False">
                        </CellStyle>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="NSECode" FieldName="City_NSECode" Visible="True"
                        Width="6%" VisibleIndex="6">
                        <CellStyle CssClass="gridcellleft" Wrap="False">
                        </CellStyle>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="BSECode" FieldName="City_BSECode" Visible="True"
                        Width="6%" VisibleIndex="7">
                        <CellStyle CssClass="gridcellleft" Wrap="False">
                        </CellStyle>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="MCXCode" FieldName="City_MCXCode" Visible="True"
                        Width="6%" VisibleIndex="8">
                        <CellStyle CssClass="gridcellleft" Wrap="False">
                        </CellStyle>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="MCXSXCode" FieldName="City_MCXSXCode" Visible="True"
                        Width="6%" VisibleIndex="9">
                        <CellStyle CssClass="gridcellleft" Wrap="False">
                        </CellStyle>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="NCDEXCode" FieldName="City_NCDEXCode" Visible="True"
                        Width="6%" VisibleIndex="10">
                        <CellStyle CssClass="gridcellleft" Wrap="False">
                        </CellStyle>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="CDSLCode" FieldName="City_CDSLCode" Visible="True"
                        Width="6%" VisibleIndex="11">
                        <CellStyle CssClass="gridcellleft" Wrap="False">
                        </CellStyle>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="NSDLCode" FieldName="City_NSDLCode" Visible="True"
                        Width="6%" VisibleIndex="12">
                        <CellStyle CssClass="gridcellleft" Wrap="False">
                        </CellStyle>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="NDMLCode" FieldName="City_NDMLCode" Visible="True"
                        Width="6%" VisibleIndex="13">
                        <CellStyle CssClass="gridcellleft" Wrap="False">
                        </CellStyle>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="CVLCode" FieldName="City_CVLCode" Visible="True"
                        Width="6%" VisibleIndex="14">
                        <CellStyle CssClass="gridcellleft" Wrap="False">
                        </CellStyle>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="DotExCode" FieldName="City_DotExCode" Visible="True"
                        Width="6%" VisibleIndex="15">
                        <CellStyle CssClass="gridcellleft" Wrap="False">
                        </CellStyle>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn ReadOnly="True">
                        <HeaderTemplate>
                            <a href="javascript:void(0);" onclick="fn_PopOpen()"><span style="color: #000099; text-decoration: underline">Add New</span> </a>
                        </HeaderTemplate>
                        <DataItemTemplate>
                            <a href="javascript:void(0);" onclick="fn_Editcity('<%# Container.KeyValue %>')">Edit</a>
                            <a href="javascript:void(0);" onclick="fn_Deletecity('<%# Container.KeyValue %>')">Delete</a>
                        </DataItemTemplate>
                    </dxe:GridViewDataTextColumn>
                </Columns>
                <ClientSideEvents EndCallback="function (s, e) {grid_EndCallBack();}" />
            </dxe:ASPxGridView>
        </div>
        <div class="PopUpArea">
            <dxe:ASPxPopupControl ID="Popup_Empcitys" runat="server" ClientInstanceName="cPopup_Empcitys"
                Width="900px" HeaderText="Add City Details" PopupHorizontalAlign="Center" Height="440px" Top="120" Left="300" 
                BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
                Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
                ContentStyle-CssClass="pad">
                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">
                        <%--<div style="Width:400px;background-color:#FFFFFF;margin:0px;border:1px solid red;">--%>
                        <div class="Top">
                            <div style="margin-bottom: 5px;">
                                <div class="cityDiv">
                                    Countryqqq
                                </div>
                                <div class="Left_Content">
                                    <dxe:ASPxComboBox ID="CmbCountryName" ClientInstanceName="cCmbCountryName" runat="server"
                                        ValueType="System.String" Width="180px" EnableSynchronization="True">
                                        <ClientSideEvents ValueChanged="function(s,e){OnCmbCountryName_ValueChange()}"></ClientSideEvents>
                                    </dxe:ASPxComboBox>
                                </div>
                            </div>
                            <div style="margin-bottom: 5px;">
                                <div class="cityDiv">
                                    State
                                </div>
                                <div class="Left_Content">
                                    <dxe:ASPxComboBox ID="CmbState" ClientInstanceName="cCmbState" runat="server" ValueType="System.String"
                                        Width="180px" EnableSynchronization="True" OnCallback="CmbState_Callback">
                                        <ClientSideEvents EndCallback="CmbState_EndCallback"></ClientSideEvents>
                                    </dxe:ASPxComboBox>
                                </div>
                            </div>
                            <div style="margin-bottom: 5px;">
                                <div class="cityDiv">
                                    City
                                </div>
                                <div class="Left_Content">
                                    <dxe:ASPxTextBox ID="txtcityName" ClientInstanceName="ctxtcityName" runat="server"
                                        Width="180px">
                                    </dxe:ASPxTextBox>
                                </div>
                            </div>
                        </div>
                        <div class="ContentDiv">
                            <div style="height: 20px; width: 280px; background-color: Gray; padding-left: 120px;">
                                <h5>Static Code</h5>
                            </div>
                            <div style="height: 20px; width: 130px; padding-left: 70px; background-color: Gray; float: left;">
                                Exchange
                            </div>
                            <div style="height: 20px; width: 200px; background-color: Gray; text-align: left;">
                                Value
                            </div>
                            <div class="ScrollDiv">
                                <div class="cityDiv" style="padding-top: 5px;">
                                    NSE Code
                                </div>
                                <div style="padding-top: 5px;">
                                    <dxe:ASPxTextBox ID="txtNseCode" ClientInstanceName="ctxtNseCode" runat="server"
                                        CssClass="cityTextbox">
                                    </dxe:ASPxTextBox>
                                </div>
                                <br style="clear: both;" />
                                <div class="cityDiv">
                                    BSE Code
                                </div>
                                <div>
                                    <dxe:ASPxTextBox ID="txtBseCode" ClientInstanceName="ctxtBseCode" runat="server"
                                        CssClass="cityTextbox">
                                    </dxe:ASPxTextBox>
                                </div>
                                <br style="clear: both;" />
                                <div class="cityDiv">
                                    MCX Code
                                </div>
                                <div>
                                    <dxe:ASPxTextBox ID="txtMcxCode" ClientInstanceName="ctxtMcxCode" runat="server"
                                        CssClass="cityTextbox">
                                    </dxe:ASPxTextBox>
                                </div>
                                <br style="clear: both;" />
                                <div class="cityDiv">
                                    MCXSX Code
                                </div>
                                <div>
                                    <dxe:ASPxTextBox ID="txtMcsxCode" ClientInstanceName="ctxtMcsxCode" runat="server"
                                        CssClass="cityTextbox">
                                    </dxe:ASPxTextBox>
                                </div>
                                <br style="clear: both;" />
                                <div class="cityDiv">
                                    NCDEX Code
                                </div>
                                <div>
                                    <dxe:ASPxTextBox ID="txtNcdexCode" ClientInstanceName="ctxtNcdexCode" runat="server"
                                        CssClass="cityTextbox">
                                    </dxe:ASPxTextBox>
                                </div>
                                <br style="clear: both;" />
                                <div class="cityDiv">
                                    CDSL Code
                                </div>
                                <div>
                                    <dxe:ASPxTextBox ID="txtCdslCode" ClientInstanceName="ctxtCdslCode" CssClass="cityTextbox"
                                        runat="server">
                                    </dxe:ASPxTextBox>
                                </div>
                                <br style="clear: both;" />
                                <div class="cityDiv">
                                    NSDL Code
                                </div>
                                <div>
                                    <dxe:ASPxTextBox ID="txtNsdlCode" ClientInstanceName="ctxtNsdlCode" CssClass="cityTextbox"
                                        runat="server">
                                    </dxe:ASPxTextBox>
                                </div>
                                <br style="clear: both;" />
                                <div class="cityDiv">
                                    NDML Code
                                </div>
                                <div>
                                    <dxe:ASPxTextBox ID="txtNdmlCode" ClientInstanceName="ctxtNdmlCode" runat="server"
                                        CssClass="cityTextbox">
                                    </dxe:ASPxTextBox>
                                </div>
                                <br style="clear: both;" />
                                <div class="cityDiv">
                                    CVL Code
                                </div>
                                <div>
                                    <dxe:ASPxTextBox ID="txtCvlCode" ClientInstanceName="ctxtCvlCode" runat="server"
                                        CssClass="cityTextbox">
                                    </dxe:ASPxTextBox>
                                </div>
                                <br style="clear: both;" />
                                <div class="cityDiv">
                                    DOTEX Code
                                </div>
                                <div>
                                    <dxe:ASPxTextBox ID="txtDotexCode" ClientInstanceName="ctxtDotexCode" runat="server"
                                        CssClass="cityTextbox">
                                    </dxe:ASPxTextBox>
                                </div>
                            </div>
                            <br style="clear: both;" />
                            <div class="Footer">
                                <div style="margin-left: 130px; width: 70px; float: left;">
                                    <dxe:ASPxButton ID="btnSave_citys" ClientInstanceName="cbtnSave_citys" runat="server"
                                        AutoPostBack="False" Text="Save">
                                        <ClientSideEvents Click="function (s, e) {btnSave_citys();}" />
                                    </dxe:ASPxButton>
                                </div>
                                <div style="">
                                    <dxe:ASPxButton ID="btnCancel_citys" runat="server" AutoPostBack="False" Text="Cancel">
                                        <ClientSideEvents Click="function (s, e) {fn_btnCancel();}" />
                                    </dxe:ASPxButton>
                                </div>
                                <br style="clear: both;" />
                            </div>
                            <br style="clear: both;" />
                        </div>
                        <%-- </div>--%>
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
