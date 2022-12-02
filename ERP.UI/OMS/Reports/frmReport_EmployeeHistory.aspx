<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Reports_frmReport_EmployeeHistory" Codebehind="frmReport_EmployeeHistory.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   <%-- <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />

    <script type="text/javascript" src="/assests/js/loaddata1.js"></script>--%>

    <script language="javascript" type="text/javascript">
        function CallAjax(obj1, obj2, obj3) {
            var cmbcompany = document.getElementById('cmbCompany_VI');
            var cmbbranch = document.getElementById('cmbBranch_VI');
            var obj4 = cmbcompany.value + '~' + cmbbranch.value
            ajax_showOptions(obj1, obj2, obj3, obj4);
        }
        //function height() {
        //    if (document.body.scrollHeight > 300)
        //        window.frameElement.height = document.body.scrollHeight;
        //    else
        //        window.frameElement.height = '300px';
        //    window.frameElement.widht = document.body.scrollWidht;
        //}

        function PageLoad() {
            FieldName = 'btnShow';
            hide('txtName_hidden');
            if (document.getElementById('txtName').value == "") {
                hide('trTree');
                hide('tdgrid');
            }
            else {
                show('trTree');
                show('tdgrid');
            }
            height();
        }


        function show(obj1) {
            //alert(obj1);
            document.getElementById(obj1).style.display = 'inline';
        }
        function hide(obj1) {
            //alert(obj1);
            document.getElementById(obj1).style.display = 'none';
        }

    </script>
     <script type="text/javascript" src="/assests/js/init.js"></script>
	<script type="text/javascript" src="/assests/js/ajax-dynamic-list_rootfile.js" ></script>
  

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <div class="panel-heading">
       <div class="panel-title">
           <h3>Employee History Report</h3>
       </div>

   </div> 
      <div class="form_main">
        <table class="TableMain100" cellpadding="opx" cellspacing="0px">
           
            <tr>
                <td>
                    <table class="TableMain100">
                        <tr>
                            <td style="text-align: center">
                                <table>
                                    <tr>
                                        <td align="left" style="padding-right:15px">
                                            <dxe:ASPxComboBox ID="cmbCompany" runat="server" ClientInstanceName="cmbCompany"
                                                Font-Bold="False" Font-Size="12px" ValueType="System.String" EnableIncrementalFiltering="True">
                                                <ButtonStyle Width="13px">
                                                </ButtonStyle>
                                                <DropDownButton Text="Company">
                                                </DropDownButton>
                                            </dxe:ASPxComboBox>
                                        </td>
                                        <td class="gridcellleft">
                                            <dxe:ASPxComboBox ID="cmbBranch" runat="server" ClientInstanceName="cmbBranch" EnableIncrementalFiltering="True"
                                                EnableTheming="true" Font-Overline="False" Font-Size="12px" ValueType="System.String"
                                                Width="130px">
                                                <ButtonStyle Width="13px">
                                                </ButtonStyle>
                                                <DropDownButton Text="Branch">
                                                </DropDownButton>
                                            </dxe:ASPxComboBox>
                                        </td>
                                        <td class="gridcellright">
                                            <span class="Ecoheadtxt"><strong>Name:</strong></span>
                                        </td>
                                        <td class="gridcellleft">
                                            <asp:TextBox ID="txtName" runat="server" Font-Size="11px" Width="252px"></asp:TextBox><asp:TextBox
                                                ID="txtName_hidden" runat="server" Width="14px"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtName"
                                                ErrorMessage=""></asp:RequiredFieldValidator>
                                        </td>
                                        <td id="tdtxtname" class="gridcellright">
                                            <dxe:ASPxButton ID="btnSubmit" runat="server" AutoPostBack="False" OnClick="btnSubmit_Click"
                                                Text="Show" CssClass="btn btn-primary btn-xs">
                                            </dxe:ASPxButton>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <table style="border-right: white thin solid; border-top: white thin solid; border-left: white thin solid;
                                    border-bottom: white thin solid;" id="trTree" width="100%">
                                    <tr>
                                        <td class="gridcellright">
                                            <span class="Ecoheadtxt" style="color: blue"><strong>Employee Name:</strong></span></td>
                                        <td class="gridcellleft" colspan="2">
                                            <asp:Label ID="lblEmpName" runat="server" Font-Size="12px" Width="100%"></asp:Label>
                                        </td>
                                        <td class="gridcellright">
                                            <span class="Ecoheadtxt" style="color: blue"><strong>Code:</strong></span></td>
                                        <td class="gridcellleft" colspan="2">
                                            <asp:Label ID="lblCode" runat="server" Font-Size="12px" Width="100%"></asp:Label></td>
                                        <td class="gridcellright">
                                            <span class="Ecoheadtxt" style="color: blue"><strong>Date Of Joining:</strong></span></td>
                                        <td class="gridcellleft">
                                            <asp:Label ID="lblDOJ" runat="server" Font-Size="12px" Width="100%"></asp:Label></td>
                                        <td class="gridcellright" style="font-weight: bold; color: #0000ff">
                                            <span class="Ecoheadtxt" style="color: blue"><strong>Date Of Leaving:</strong></span></td>
                                        <td class="gridcellleft">
                                            <asp:Label ID="lblDOL" runat="server" Font-Size="12px" Width="100%"></asp:Label></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td id="tdgrid">
                                <dxe:ASPxGridView ID="gridHistory" ClientInstanceName="grid" KeyFieldName="emp_cntId"
                                    runat="server" AutoGenerateColumns="False" Width="100%">
                                    <Styles>
                                        <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                        </Header>
                                        <LoadingPanel ImageSpacing="10px">
                                        </LoadingPanel>
                                        <Cell CssClass="gridcellleft">
                                        </Cell>
                                    </Styles>
                                    <StylesEditors>
                                        <ProgressBar Height="25px">
                                        </ProgressBar>
                                    </StylesEditors>
                                    <Columns>
                                        <dxe:GridViewDataTextColumn Caption="Company" VisibleIndex="0" FieldName="cmp"
                                            Width="250px">
                                            <CellStyle Wrap="False">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Branch" VisibleIndex="1" FieldName="branch"
                                            Width="100px">
                                            <CellStyle Wrap="False">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Designation" VisibleIndex="2" FieldName="deg"
                                            Width="100px">
                                            <CellStyle Wrap="False">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="From" VisibleIndex="3" FieldName="joindate"
                                            Width="50px">
                                            <CellStyle Wrap="False">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="To" VisibleIndex="4" FieldName="enddate" Width="50px">
                                            <CellStyle Wrap="False">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="CTC" VisibleIndex="5" FieldName="ctc" Width="20px">
                                            <CellStyle Wrap="False">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Reporting Head" VisibleIndex="6" Width="150px"
                                            FieldName="reportHead">
                                            <CellStyle Wrap="False">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Working Schedule" VisibleIndex="7" FieldName="workingHr"
                                            Width="20px">
                                            <CellStyle Wrap="False">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                    </Columns>
                                    <SettingsBehavior AllowFocusedRow="True" />
                                    <SettingsPager AlwaysShowPager="True" NumericButtonCount="20" ShowSeparators="True"
                                        PageSize="20">
                                        <FirstPageButton Visible="True">
                                        </FirstPageButton>
                                        <LastPageButton Visible="True">
                                        </LastPageButton>
                                    </SettingsPager>
                                </dxe:ASPxGridView>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
       </div>
</asp:Content>