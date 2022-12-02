<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Reports_frmReport_Attendance" Codebehind="frmReport_Attendance.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">




    <script type="text/javascript" src="/assests/js/ajax-dynamic-list_rootfile.js"></script>

  

    <script language="javascript" type="text/javascript">
    
    function validation()
    {
       ASPx_EmployeeAtdd.PerformCallback();
    }
    function PageLoad()
    {
        FieldName='btnShow';
        document.getElementById('txtName_hidden').style.display="none";
        ShowEmployeeFilterForm('A');
        height();
    }
    function ShowEmployeeFilterForm(obj)
    {//alert(obj);
        document.getElementById('txtName_hidden').value="";
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
        height();
    }
    function NoOfRows(obj)
    {
        //alert(obj);
        Noofrows=obj;
        document.getElementById('txtName_hidden').style.display="none";
    }
    function show(obj1)
    {
        //alert(obj1);
         document.getElementById(obj1).style.display='table-cell';
    }
    function hide(obj1)
    {
        //alert(obj1);
         document.getElementById(obj1).style.display='none';
    }
     FieldName='btnShow'
      function CallAjax(obj1,obj2,obj3)
    {
        var cmbcompany=document.getElementById('cmbCompany_VI');
        var cmbbranch=document.getElementById('cmbBranch_VI');
        var obj4=cmbcompany.value+'~'+cmbbranch.value
        //alert(obj4);
        ajax_showOptions(obj1,obj2,obj3,obj4);
    }
    function openLegendPage()
    {
        window.open('frmLegendReport_popup.aspx','50','resizable=1,height=250px,width=100px');
    }
//    function height()
//    {
//        if(document.body.scrollHeight>300)
//            window.frameElement.height = document.body.scrollHeight;
//        else
//            window.frameElement.height = '300px';
////        alert(window.frameElement.width);
////        alert(document.documentElement.clientWidth);
//        window.frameElement.width = document.documentElement.clientWidth;
////        alert(window.frameElement.width);
//    }
    function LastCall(obj)
    {
       // height();
    }
    function btnShowClick()
    {
        if(ReportType.GetValue()=='Screen')
        {
            ASPx_EmployeeAtdd.PerformCallback();
        }
        else
        {
            var user='';
            if(User.GetValue()=='A')
                user='All';
            else
            {
                var a=document.getElementById("txtName_hidden");
                user=a.value;
            }
            var url='frmReport_Attendance_Print.aspx?id='+Year.GetValue()+'~'+Month.GetValue()+'~'+Company.GetValue()+'~'+Branch.GetValue()+'~'+user;
            //alert(url)
            OnMoreInfoClick(url,"Employee Attendance Details",'940px','450px',"N");
        }
    }
    </script>
    <style>
        .padtable>tbody>tr>td {
            padding:8px 0;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="panel-heading">
       <div class="panel-title">
           <h3>Employee's Attendance Report</h3>
       </div>

   </div> 
      <div class="form_main">

        <table class="TableMain100" cellpadding="opx" cellspacing="0px">
            
            <tr>
                <td>
                    <table border="0" class="TableMain" cellpadding="0" cellspacing="0" style="width: 100%;
                        padding-right: 0px; padding-left: 0px; padding-bottom: 0px; padding-top: 0px;">
                        <tr>
                            <td valign="bottom" class="gridcellleft">
                                <table class="padtable">
                                    <tr>
                                        <td style="text-align: left;padding-bottom:8px">
                                            <a href="#" onclick="javascript:openLegendPage();"><span class="Ecoheadtxt">
                                                <strong>Legends</strong></span></a>
                                        </td>
                                        <td class="gridcellleft" style="width: 50px;padding-right:15px" valign="top" >
                                            <dxe:ASPxComboBox ID="cmbYear" Width="120px" runat="server" Font-Size="12px" ValueType="System.String"
                                                Font-Bold="False" ClientInstanceName="Year">
                                                <ButtonStyle Width="13px">
                                                </ButtonStyle>
                                                <DropDownButton Text="Year">
                                                </DropDownButton>
                                            </dxe:ASPxComboBox>
                                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="cmbYear"
                                                runat="server" ErrorMessage="Invalid Year" Width="117px"></asp:RequiredFieldValidator>--%></td>
                                        <td class="gridcellleft" style="width: 50px;padding-right:15px" valign="top">
                                            <dxe:ASPxComboBox ID="cmbMonth" Width="120px" runat="server" Font-Size="12px" ValueType="System.String"
                                                Font-Bold="False" ClientInstanceName="Month">
                                                <Items>
                                                    <dxe:ListEditItem Text="January" Value="1" />
                                                    <dxe:ListEditItem Text="February" Value="2" />
                                                    <dxe:ListEditItem Text="March" Value="3" />
                                                    <dxe:ListEditItem Text="April" Value="4" />
                                                    <dxe:ListEditItem Text="May" Value="5" />
                                                    <dxe:ListEditItem Text="June" Value="6" />
                                                    <dxe:ListEditItem Text="July" Value="7" />
                                                    <dxe:ListEditItem Text="August" Value="8" />
                                                    <dxe:ListEditItem Text="September" Value="9" />
                                                    <dxe:ListEditItem Text="October" Value="10" />
                                                    <dxe:ListEditItem Text="November" Value="11" />
                                                    <dxe:ListEditItem Text="December" Value="12" />
                                                </Items>
                                                <ButtonStyle Width="13px">
                                                </ButtonStyle>
                                                <DropDownButton Text="Month">
                                                </DropDownButton>
                                            </dxe:ASPxComboBox>
                                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="cmbMonth"
                                                runat="server" ErrorMessage="Invalid Month" Width="118px"></asp:RequiredFieldValidator>--%></td>
                                        <td class="gridcellleft" style="width: 50px;padding-right:15px" valign="top">
                                            <dxe:ASPxComboBox ID="cmbCompany" Width="120px" runat="server" Font-Size="12px" ValueType="System.String"
                                                Font-Bold="False" ClientInstanceName="Company">
                                                <ButtonStyle Width="13px">
                                                </ButtonStyle>
                                                <DropDownButton Text="Company">
                                                </DropDownButton>
                                            </dxe:ASPxComboBox>
                                        </td>
                                        <td class="gridcellleft" style="width: 50px;" valign="top">
                                            <dxe:ASPxComboBox ID="cmbBranch" Width="120px" runat="server" ValueType="System.String"
                                                Font-Size="12px" ClientInstanceName="Branch" EnableIncrementalFiltering="True" EnableTheming="true" Font-Overline="False">
                                                <ButtonStyle Width="13px">
                                                </ButtonStyle>
                                                <DropDownButton Text="Branch" Width="40px">
                                                </DropDownButton>
                                            </dxe:ASPxComboBox>
                                        </td>
                                        
                                        <%--<td class="gridcellleft" style="width: 340px" valign="top">
                                        </td>
                                        <td class="gridcellleft pull-right" style="width: 50px" valign="top">
                                         <dxe:ASPxComboBox ID="cmbExport" runat="server" AutoPostBack="true" 
                                        Font-Bold="False" ForeColor="black" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged"
                                        ValueType="System.Int32" Width="130px">
                                        <Items>
                                            <dxe:ListEditItem Text="Select" Value="0" />
                                            <dxe:ListEditItem Text="XLS" Value="2" />
                                        </Items>
                                        <ButtonStyle >
                                        </ButtonStyle>
                                        <ItemStyle >
                                            <HoverStyle >
                                            </HoverStyle>
                                        </ItemStyle>
                                        <Border BorderColor="black" />
                                        <DropDownButton Text="Export">
                                        </DropDownButton>
                                    </dxe:ASPxComboBox>
                                        </td>--%>
                                    </tr>
                                    
                                    <tr>
                                        <td class="gridcellleft" style="padding-bottom:8px" >
                                            <span class="Ecoheadtxt"><strong>User:</strong></span></td>
                                        <td class="gridcellleft" valign="top">
                                            <dxe:ASPxRadioButtonList ID="rbUser" runat="server" SelectedIndex="0" ItemSpacing="10px"
                                                RepeatDirection="Horizontal" TextWrap="False" Font-Size="12px" ClientInstanceName="User">
                                                <Items>
                                                    <dxe:ListEditItem Text="All" Value="A" />
                                                    <dxe:ListEditItem Text="Specific" Value="S" />
                                                </Items>
                                                <ClientSideEvents ValueChanged="function(s, e) {ShowEmployeeFilterForm(s.GetValue());}" />
                                                <DisabledStyle>
                                                    <Border BorderStyle="None" BorderWidth="0px" />
                                                </DisabledStyle>
                                                <Paddings Padding="0px" />
                                                <Border BorderStyle="None" BorderWidth="0px" />
                                            </dxe:ASPxRadioButtonList>
                                        </td>
                                        
                                    </tr>
                                    <tr>
                                        <td class="gridcellleft" id="tdname" style="padding-bottom:8px" >
                                            <span class="Ecoheadtxt" ><strong>Name:</strong></span></td>
                                        <td class="gridcellleft" id="tdtxtname" style="vertical-align:top" colspan="2">
                                            <asp:TextBox ID="txtName" runat="server" Width="252px" Font-Size="11px"></asp:TextBox>
                                            <asp:TextBox ID="txtName_hidden" runat="server" Width="14px"></asp:TextBox></td>
                                        
                                    </tr>
                                    <tr>
                                        <td class="gridcellleft" >
                                            <span class="Ecoheadtxt" ><strong>Report Type:</strong></span></td>
                                        <td class="gridcellleft" valign="top">
                                            <dxe:ASPxRadioButtonList ID="RBReportType" runat="server" RepeatDirection="Horizontal"
                                                SelectedIndex="0" TextSpacing="3px" ItemSpacing="10px" ClientInstanceName="ReportType">
                                                <Items>
                                                    <dxe:ListEditItem Text="Screen" Value="Screen" />
                                                    <dxe:ListEditItem Text="Print" Value="Print" />
                                                </Items>
                                                <ValidationSettings ErrorText="Error has occurred">
                                                    <ErrorImage Width="14px" />
                                                </ValidationSettings>
                                                <Border BorderWidth="0px" />
                                                <Paddings Padding="0px" />
                                            </dxe:ASPxRadioButtonList>
                                        </td>
                                        
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td class="gridcellleft" valign="top">
                                            <dxe:ASPxButton ID="btnShow" runat="server" Text="Show" AutoPostBack="false" CssClass="btn btn-primary">
                                            <ClientSideEvents Click="function(s,e){btnShowClick();}" />
                                            </dxe:ASPxButton>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        
                        <tr>
                            <td style="vertical-align: top; text-align: left;">
                                <table>
                                    
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table class="TableMain100" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="gridcellcenter">
                                <dxe:ASPxGridView ID="ASPx_EmployeeAtdd" ClientInstanceName="ASPx_EmployeeAtdd"
                                    runat="server" AutoGenerateColumns="False" DataSourceID="SDSAttdMain" OnCustomCallback="ASPx_EmployeeAtdd_CustomCallback"
                                    Width="100%" OnHtmlDataCellPrepared="ASPx_EmployeeAtdd_HtmlDataCellPrepared"
                                    Font-Size="12px" OnCustomJSProperties="ASPx_EmployeeAtdd_CustomJSProperties">
                                    <Styles>
                                        <Header ImageSpacing="8px" SortingImageSpacing="8px">
                                        </Header>
                                        <LoadingPanel ImageSpacing="10px">
                                        </LoadingPanel>
                                    </Styles>
                                    <Columns>
                                        <dxe:GridViewDataTextColumn Caption="Employee Name" FieldName="empName" 
                                            VisibleIndex="0" FixedStyle="Left">
                                            <CellStyle Wrap="False" CssClass="gridcellleft">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Emp. Code" FieldName="code"  VisibleIndex="1"
                                            FixedStyle="Left">
                                            <CellStyle Wrap="False" CssClass="gridcellleft">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Company Name" FieldName="cmp_Name" 
                                            VisibleIndex="2">
                                            <CellStyle Wrap="False" CssClass="gridcellleft">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Branch" FieldName="branch_description"
                                            VisibleIndex="3">
                                            <CellStyle Wrap="False" CssClass="gridcellleft">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="1" FieldName="day1" VisibleIndex="4">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="2" FieldName="day2"  VisibleIndex="5">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="3" FieldName="day3"  VisibleIndex="6">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="4" FieldName="day4"  VisibleIndex="7">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="5" FieldName="day5"  VisibleIndex="8">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="6" FieldName="day6"  VisibleIndex="9">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="7" FieldName="day7"  VisibleIndex="10">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="8" FieldName="day8"  VisibleIndex="11">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="9" FieldName="day9"  VisibleIndex="12">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="10" FieldName="day10"  VisibleIndex="13">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="11" FieldName="day11"  VisibleIndex="14">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="12" FieldName="day12" VisibleIndex="15">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="13" FieldName="day13"  VisibleIndex="16">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="14" FieldName="day14"  VisibleIndex="17">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="15" FieldName="day15"  VisibleIndex="18">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="16" FieldName="day16"  VisibleIndex="19">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="17" FieldName="day17"  VisibleIndex="20">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="18" FieldName="day18"  VisibleIndex="21">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="19" FieldName="day19"  VisibleIndex="22">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="20" FieldName="day20"  VisibleIndex="23">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="21" FieldName="day21"  VisibleIndex="24">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="22" FieldName="day22"  VisibleIndex="25">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="23" FieldName="day23"  VisibleIndex="26">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="24" FieldName="day24"  VisibleIndex="27">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="25" FieldName="day25"  VisibleIndex="28">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="26" FieldName="day26"  VisibleIndex="29">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="27" FieldName="day27"  VisibleIndex="30">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="28" FieldName="day28"  VisibleIndex="31">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="29" FieldName="day29" VisibleIndex="32">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="30" FieldName="day30"  VisibleIndex="33">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="31" FieldName="day31" VisibleIndex="34">
                                        </dxe:GridViewDataTextColumn>
                                    </Columns>
                                    <SettingsPager PageSize="20" ShowSeparators="True">
                                        <FirstPageButton Visible="True">
                                        </FirstPageButton>
                                        <LastPageButton Visible="True">
                                        </LastPageButton>
                                    </SettingsPager>
                                    <ClientSideEvents EndCallback="function(s, e) {
	                                        LastCall(s.cpHeight);
                                        }" />
                                    <Settings ShowHorizontalScrollBar="True" />
                                    <SettingsBehavior ColumnResizeMode="NextColumn" />
                                </dxe:ASPxGridView>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="height: 22px">
                                <%--<CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true"
                                    DisplayGroupTree="False" />--%>
                                <%--<CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true"
                                    DisplayGroupTree="False" />
                                <CR:CrystalReportPartsViewer ID="CrystalReportPartsViewer1" runat="server" AutoDataBind="True"
                                    ReportSourceID="CrystalReportSource1" />
                                <CR:CrystalReportSource ID="CrystalReportSource1" runat="server">
                                    <Report FileName="Reports\AttendenceRport.rpt">
                                    </Report>
                                </CR:CrystalReportSource>--%>
                                <asp:SqlDataSource ID="SDSAttdMain" runat="server">
                                </asp:SqlDataSource>
                                            <dxe:ASPxGridViewExporter ID="exporter" runat="server">
                                            </dxe:ASPxGridViewExporter>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
           </div> 
</asp:Content>
