<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.Reports_frmReport_LeaveBalance_FromMyPage" Codebehind="frmReport_LeaveBalance_FromMyPage.aspx.cs" %>


<%--<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe000001" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>

<%@ Register assembly="DevExpress.Web.v15.1" namespace="DevExpress.Web" tagprefix="dx" %>--%>
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
    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />

    <script type="text/javascript" src="/assests/js/loaddata1.js"></script>

    <script type="text/javascript" src="/assests/js/init.js"></script>

    <script type="text/javascript" src="/assests/js/ajax-dynamic-list_wofolder.js"></script>

    <script language="javascript" type="text/javascript">
         function SignOff()
            {
            window.parent.SignOff()
            }
        function height()
        {
           
           if(document.body.scrollHeight>=650)
                {
                    window.frameElement.height = document.body.scrollHeight;
                }
           else
                {
                    window.frameElement.height = '650px';
                }
                window.frameElement.widht = document.body.scrollWidht;
           
        }
    </script>

    <script language="javascript" type="text/javascript">
//function CallAjax(obj1,obj2,obj3)
//    {
//        var cmbcompany=document.getElementById('cmbCompany_VI');
//        var cmbbranch=document.getElementById('cmbBranch_VI');
//        var obj4=cmbcompany.value+'~'+cmbbranch.value
//        ajax_showOptions(obj1,obj2,obj3,obj4);
//    }
    </script>

    <script language="ecmascript" type="text/ecmascript">
function PageLoad()
    {
        FieldName='btnShow';
        document.getElementById('txtName_hidden').style.display="none";
       
        ShowEmployeeFilterForm('A');
       
    }
    
    function ShowEmployeeFilterForm(obj)
    {
        if(obj=='A')
        {
            hide('tdtxtname');
            hide('tdname');
            document.getElementById('txtName_hidden').style.display="none";
        }
        if(obj=='S')
        {
            show('tdtxtname');
            show('tdname');
            document.getElementById('txtName_hidden').style.display="none";
        }
    }
    function show(obj1)
    {
        //alert(obj1);
         document.getElementById(obj1).style.display='inline';
    }
    function hide(obj1)
    {
        //alert(obj1);
         document.getElementById(obj1).style.display='none';
    }
     FieldName='btnShow'
    function aftersave(obj1,obj2)
    {
       ShowEmployeeFilterForm(obj1);
    }
    function ShowTreeList(obj)
    {
        grid.PerformCallback();
        show('tdTree');
    }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <table class="TableMain100">
            <tr>
                <td class="EHEADER" colspan="0" style="text-align: center;">
                    <strong><span style="color: #000099">Employee's Leave Balance Report</span></strong>
                </td>
            </tr>
            <tr>
                <td>
                    <table>
                        <tr>
                            <td style="text-align: left;">
                                <table>
                                    <tr>
                                        <td>
                                            <dxe:ASPxComboBox ID="cmbCompany" ClientInstanceName="cmbCompany" runat="server"
                                                Font-Bold="False" Font-Size="12px" ValueType="System.String" EnableIncrementalFiltering="True">
                                                <ButtonStyle Width="13px">
                                                </ButtonStyle>
                                                <DropDownButton Text="Company">
                                                </DropDownButton>
                                            </dxe:ASPxComboBox>
                                        </td>
                                        <td class="gridcellleft" style="font-weight: bold; width: 50px;">
                                            <dxe:ASPxComboBox ID="cmbBranch" ClientInstanceName="cmbBranch" runat="server" EnableIncrementalFiltering="True"
                                                EnableTheming="False" Font-Overline="False" Font-Size="12px" Height="10px" ValueType="System.String"
                                                Width="180px">
                                                <ButtonStyle Width="13px">
                                                </ButtonStyle>
                                                <DropDownButton Text="Branch" Width="40px">
                                                </DropDownButton>
                                            </dxe:ASPxComboBox>
                                        </td>
                                        <td style="text-align: left;">
                                            <dxe:ASPxDateEdit ID="cmbDate" runat="server" DateOnError="Today" EditFormat="Custom"
                                                EditFormatString="dd MMMM yyyy" Font-Size="12px" Width="130px" UseMaskBehavior="True">
                                                <ButtonStyle Width="13px">
                                                </ButtonStyle>
                                                <DropDownButton ImagePosition="Top" Text="Date" Width="30px">
                                                </DropDownButton>
                                            </dxe:ASPxDateEdit>
                                        </td>
                                       
                                        <td id="tdname" class="gridcellright" style="width: 27px;">
                                            <span class="Ecoheadtxt" style="color: blue"><strong>Name:</strong></span>
                                        </td>
                                        <td id="tdtxtname" class="gridcellleft" style="width: 164px;">
                                            <asp:TextBox ID="txtName" runat="server" Font-Size="11px" Width="197px"></asp:TextBox>
                                        </td>
                                        <td class="gridcellright" style="width: 94px;">
                                            <dxe:ASPxButton ID="btnSubmit" runat="server" AutoPostBack="False" Text="Show">
                                                <ClientSideEvents Click="function(s, e) {
	ShowTreeList(s);
}" />
                                            </dxe:ASPxButton>
                                        </td>
                                         <td style="width: 41px; text-align: right; visibility: hidden">
                                            <span class="Ecoheadtxt" style="color: blue"><strong>User:</strong></span>
                                        </td>
                                        <td style="visibility: hidden">
                                            <dxe:ASPxRadioButtonList ID="rbUser" runat="server" Font-Size="12px" RepeatDirection="Horizontal"
                                                SelectedIndex="1" TextWrap="False">
                                                <Items>
                                                    <dxe:ListEditItem Text="All" Value="A" />
                                                    <dxe:ListEditItem Text="Specific" Value="S" />
                                                </Items>
                                                <ClientSideEvents ValueChanged="function(s, e) {ShowEmployeeFilterForm(s.GetValue());}" />
                                                <Border BorderWidth="0px" />
                                                <Paddings PaddingLeft="0px" />
                                            </dxe:ASPxRadioButtonList>
                                            <asp:TextBox
                                                ID="txtName_hidden" runat="server" Width="14px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellright">
                                <dxe:ASPxComboBox ID="cmbExport" runat="server" AutoPostBack="true" BackColor="Navy"
                                    Font-Bold="False" ForeColor="White" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged"
                                    ValueType="System.Int32" Width="110px">
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
                                <dxe:ASPxGridViewExporter ID="exporter" runat="server">
                                </dxe:ASPxGridViewExporter>
                            </td>
                        </tr>
                        <tr>
                            <td id="tdTree" style="text-align: left;">
                                <dxe:ASPxGridView ID="GridLeaveBalenece" ClientInstanceName="grid" runat="server"
                                    AutoGenerateColumns="False" Width="940px" OnCustomCallback="GridLeaveBalenece_CustomCallback">
                                    <Styles>
                                        <Header ImageSpacing="5px" SortingImageSpacing="5px" Font-Bold="True">
                                        </Header>
                                        <LoadingPanel ImageSpacing="10px">
                                        </LoadingPanel>
                                        <Cell HorizontalAlign="Left" CssClass="gridcellleft">
                                        </Cell>
                                    </Styles>
                                    <Images>
                                        <CollapsedButton Height="12px" Url="~/App_Themes/Office2003 Blue/GridView/gvCollapsedButton.png"
                                            Width="11px" />
                                        <ExpandedButton Height="12px" Url="~/App_Themes/Office2003 Blue/GridView/gvExpandedButton.png"
                                            Width="11px" />
                                        <DetailCollapsedButton Height="12px" Url="~/App_Themes/Office2003 Blue/GridView/gvCollapsedButton.png"
                                            Width="11px" />
                                        <DetailExpandedButton Height="12px" Url="~/App_Themes/Office2003 Blue/GridView/gvExpandedButton.png"
                                            Width="11px" />
                                        <FilterRowButton Height="13px" Width="13px" />
                                    </Images>
                                    <Columns>
                                        <dxe:GridViewDataTextColumn Caption="Name" FixedStyle="Left" FieldName="EmpName"
                                            VisibleIndex="0" Width="150px">
                                            <CellStyle Wrap="False" BackColor="#DDECFE">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Code" FixedStyle="Left" FieldName="code" VisibleIndex="1"
                                            Width="50px">
                                            <CellStyle Wrap="False" BackColor="#DDECFE">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Designation" FixedStyle="Left" VisibleIndex="2"
                                            FieldName="Designation" Width="150px">
                                            <CellStyle Wrap="False" BackColor="#DDECFE">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Company" VisibleIndex="3" FieldName="compName"
                                            Width="300px">
                                            <CellStyle Wrap="False">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Branch" FieldName="branch" VisibleIndex="4"
                                            Width="150px">
                                            <CellStyle Wrap="False">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Total PL" VisibleIndex="5" FieldName="TotalPL"
                                            Width="60px">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Availed PL" VisibleIndex="6" FieldName="PLAvailed"
                                            Width="60px">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Aveliable PL" VisibleIndex="7" FieldName="PLAveliable"
                                            Width="60px">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Total CL" VisibleIndex="8" FieldName="TotalCL"
                                            Width="60px">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Availed CL" VisibleIndex="9" FieldName="CLAvailed"
                                            Width="60px">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Aveliable CL" VisibleIndex="10" FieldName="CLAveliable"
                                            Width="60px">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Total SL" VisibleIndex="11" FieldName="TotalSL"
                                            Width="60px">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Availed SL" VisibleIndex="12" FieldName="SLAvailed"
                                            Width="60px">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Aveliable SL" VisibleIndex="13" FieldName="SLAveliable"
                                            Width="60px">
                                        </dxe:GridViewDataTextColumn>
                                    </Columns>
                                    <StylesEditors>
                                        <ProgressBar Height="25px">
                                        </ProgressBar>
                                    </StylesEditors>
                                    <SettingsPager PageSize="20" ShowSeparators="True" AlwaysShowPager="True">
                                        <FirstPageButton Visible="True">
                                        </FirstPageButton>
                                        <LastPageButton Visible="True">
                                        </LastPageButton>
                                    </SettingsPager>
                                    <Settings ShowHorizontalScrollBar="True" />
                                </dxe:ASPxGridView>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
   </asp:Content>
