<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.SettingsOptions.management_SettingsOptions_frm_UOMConversion" Codebehind="frm_UOMConversion.aspx.cs" %>




<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    

    

    <style type="text/css">
	
	/* Big box with list of options */
	#ajax_listOfOptions{
		position:absolute;	/* Never change this one */
		width:50px;	/* Width of box */
		height:auto;	/* Height of box */
		overflow:auto;	/* Scrolling features */
		border:1px solid Blue;	/* Blue border */
		background-color:#FFF;	/* White background color */
		text-align:left;
		font-size:0.9em;
		z-index:32767;
	}
	#ajax_listOfOptions div{	/* General rule for both .optionDiv and .optionDivSelected */
		margin:1px;		
		padding:1px;
		cursor:pointer;
		font-size:0.9em;
	}
	#ajax_listOfOptions .optionDiv{	/* Div for each item in list */
		
	}
	#ajax_listOfOptions .optionDivSelected{ /* Selected item in the list */
		background-color:#DDECFE;
		color:Blue;
	}
	#ajax_listOfOptions_iframe{
		background-color:#F00;
		position:absolute;
		z-index:3000;
	}
	
	form{
		display:inline;
	}
	
	</style>
    
 

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script language="javascript" type="text/javascript">

        function PopulateGrid(obj) {

            grid.PerformCallback(obj);
        }
        function Changestatus(obj) {
            var URL = "changeuomsetting.aspx?id=" + obj;
            editwin = dhtmlmodal.open("Editbox", "iframe", URL, "Add/Edit Conversion Multiplier", "width=995px,center=0,resize=1,top=-1", "recal");
            editwin.onclose = function () {
                grid.PerformCallback();
            }
        }

    function ChangestatusNew(obj) {
        debugger;
        var url = 'changeuomsetting.aspx?id=' + obj;
        popup.SetContentUrl(url);
        popup.Show();
    }
    function ShowHideFilter(obj) {
        grid.PerformCallback(obj);
    }
    function callback() {
        grid.PerformCallback();
    }
    function callheight(obj) {
        height();
        // parent.CallMessage();
    }
    </script>

     <div class="panel-heading">
        <div class="panel-title">
            <h3>UOM CONVERSION RATES</h3>
        </div>

    </div>
        <div class="form_main">

              <dxe:ASPxPopupControl ID="ASPXPopupControl" runat="server" ContentUrl="changeuomsetting.aspx"
                                            CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"    ClientInstanceName="popup" Height="300px"
                                            Width="500px" HeaderText="Add/Modify UOM" Modal="true" AllowResize="true" ResizingMode="Postponed" >
                                            <ContentCollection>
                                                <dxe:PopupControlContentControl runat="server">
                                                </dxe:PopupControlContentControl>
                                            </ContentCollection>
                                        </dxe:ASPxPopupControl>
            <table class="TableMain100">
               <%-- <tr>
                    <td class="EHEADER" style="text-align: center;">
                        <strong><span style="color: #000099">UOM CONVERSION RATES </span></strong>
                    </td>
                </tr>--%>
                <tr>
                    <td style="text-align: left; vertical-align: top">
                        <table>
                            <tr>
                                <td id="ShowFilter">
                                   <%--<a href="javascript:ShowHideFilter('s');" class="btn btn-success"><span>Shailter</span></a>--%>
                                <a href="javascript:void(0);" onclick="javascript:ChangestatusNew('Add');" class="btn btn-primary">Add New</a>
                                </td>
                                <td id="Td1">
                                     <%--<a href="javascript:ShowHideFilter('All');" class="btn btn-primary"><span>All Records</span></a>--%>
                                <asp:DropDownList ID="cmbExport" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true"  >
                                    <asp:ListItem Value="0">Export to</asp:ListItem>
                                    <asp:ListItem Value="1">PDF</asp:ListItem>
                                     <asp:ListItem Value="2">XLS</asp:ListItem>
                                     <asp:ListItem Value="3">RTF</asp:ListItem>
                                     <asp:ListItem Value="4">CSV</asp:ListItem>
                        
                                </asp:DropDownList>
                                </td>
                               
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                    <dxe:ASPxGridView ID="grdDocuments" runat="server" AutoGenerateColumns="False"
                        KeyFieldName="tmpconversion_id" Width="100%" OnRowDeleting="grdDocuments_RowDeleting" ClientInstanceName="grid"
                        OnCustomCallback="grdDocuments_CustomCallback" OnHtmlRowCreated="grdDocuments_HtmlRowCreated">
                        <templates>
                                <TitlePanel>
                                    <table style="width: 100%">
                                        <tr>
                                            <td align="right">
                                                <table width="200px">
                                                    <tr>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </TitlePanel>
                                <EditForm>
                                </EditForm>
                            </templates>
                        <settings showgrouppanel="True" />
                        <settingsbehavior confirmdelete="True" allowfocusedrow="True" />
                        <columns>
                                <dxe:GridViewDataTextColumn FieldName="tmpconversion_id" ReadOnly="True" VisibleIndex="0"
                                    Visible="false">
                                    <CellStyle HorizontalAlign="Left">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="left" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="tmpconversion_fromuomname" Caption="From Conversion Name" Width="25%" ReadOnly="True"
                                    VisibleIndex="0">
                                    <CellStyle HorizontalAlign="left">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="left" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="tmpconversion_touomname" Caption="To Conversion Name" Width="25%"
                                    VisibleIndex="1" Visible="true">
                                    <CellStyle HorizontalAlign="left">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="left" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="tmpconversion_multiplier" Caption="Converted Multipier" Width="25%"
                                    VisibleIndex="2">
                                    <CellStyle HorizontalAlign="left">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="left" />
                                </dxe:GridViewDataTextColumn>
                               <%-- <dxe:GridViewDataTextColumn FieldName="tmp_convuomname" Caption="Converted To"
                                    Width="22%" VisibleIndex="3">
                                    <CellStyle HorizontalAlign="left">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="left" />
                                </dxe:GridViewDataTextColumn>--%>
                               <dxe:GridViewDataTextColumn FieldName="change" Caption="Action" Width="15%" VisibleIndex="3">
                                    <CellStyle HorizontalAlign="Center">
                                    </CellStyle>
                                   
                                    <HeaderStyle HorizontalAlign="Center" />
                                     <Settings AllowAutoFilter="False" AllowGroup="False" />
                                      <DataItemTemplate >
                                         <img src="../../../assests/images/changeIcon.png" />
                                      </DataItemTemplate>
                                </dxe:GridViewDataTextColumn>
                                <%--<dxe:GridViewCommandColumn VisibleIndex="4" Width="10%" >--%>
                               <%-- <EditButton Visible="false">
                                </EditButton>
                                <DeleteButton Visible="True">
                                </DeleteButton>--%>
                                <%--<HeaderStyle HorizontalAlign="Center" />
                                <HeaderTemplate>
                                   
                                    <a href="javascript:void(0);" onclick="Changestatus('Add')"><span style="color: #000099;
                                        text-decoration: underline">Add New</span> </a>
                                    
                                </HeaderTemplate>

                            </dxe:GridViewCommandColumn>--%>
                           
                            </columns>

                        <settingssearchpanel visible="True" />
                        <settings showgrouppanel="True" showstatusbar="Hidden" showfilterrow="true" showfilterrowmenu="True" />

                        <SettingsCommandButton>
                <ClearFilterButton Text="Clear">
                </ClearFilterButton>

               
                <DeleteButton Image-Url="/assests/images/Delete.png" ButtonType="Image" Image-AlternateText="Delete">
                </DeleteButton>

               


            </SettingsCommandButton>
                        <%--<SettingsBehavior AllowFocusedRow="True" ColumnResizeMode="NextColumn" />--%>
                        <settings showgrouppanel="True" showstatusbar="Visible" />
                        <styles>
                            <Header SortingImageSpacing="5px" ImageSpacing="5px" CssClass="gridheader"></Header>
                                <LoadingPanel ImageSpacing="10px">
                                </LoadingPanel>
                                <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                </Header>
                                <FocusedGroupRow CssClass="gridselectrow">
                                </FocusedGroupRow>
                                <FocusedRow CssClass="gridselectrow">
                                </FocusedRow>
                            </styles>
                        <settingspager numericbuttoncount="20" pagesize="20" showseparators="True" alwaysshowpager="True">
                                <FirstPageButton Visible="True">
                                </FirstPageButton>
                                <LastPageButton Visible="True">
                                </LastPageButton>
                            </settingspager>
                        <settingsediting mode="PopupEditForm" popupeditformheight="200px" popupeditformhorizontalalign="Center"
                            popupeditformmodal="True" popupeditformverticalalign="WindowCenter" popupeditformwidth="600px"
                            editformcolumncount="1" />
                        <settingstext popupeditformcaption="Add/Modify " ConfirmDelete="Confirm delete?" />

                    </dxe:ASPxGridView>
                    <%--   <asp:SqlDataSource ID="grddoc" runat="server"
                            SelectCommand="" DeleteCommand="DELETE FROM [tbl_master_forms] WHERE [frm_id] = @formID">
                            <DeleteParameters>
                                <asp:Parameter Name="formID" Type="int32" />
                            </DeleteParameters>
                            <SelectParameters>
                                <asp:SessionParameter Name="userlist" SessionField="userchildHierarchy" Type="string" />
                            </SelectParameters>
                        </asp:SqlDataSource>--%>
                </td>
                </tr>
                <tr>
                    <td style="display: none;">
                        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox></td>
                </tr>
            </table>
            <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
            </dxe:ASPxGridViewExporter>
        </div>
</asp:Content>