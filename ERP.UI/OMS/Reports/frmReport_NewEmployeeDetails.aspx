<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Reports.Reports_frmReport_NewEmployeeDetails" Codebehind="frmReport_NewEmployeeDetails.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script  type="text/ecmascript">
        function PageLoad() {
            FieldName = 'ctl00_ContentPlaceHolder3_bnSubmit';
            fromDt.Focus();
            // hide('tdgrid');
        }
        //    function show(obj1)
        //    {
        //         document.getElementById(obj1).style.display='inline';
        //    }
        //    function hide(obj1)
        //    {
        //         document.getElementById(obj1).style.display='none';
        //    }
        function showgrid() {
            grid.PerformCallback('BindGrid~');
            //        if(fromDt.GetValue() != null)
            //        {
            //            show('tdgrid');   
            //        }
            //        else
            //        {
            //            hide('tdgrid');
            //        }
        }


        function aspxEmpGrid_EndCallBack() {
            height();
        }
</script>
      <div class="panel-heading">
       <div class="panel-title">
           <h3>New Employee's Details Report</h3>
       </div>

   </div> 
<div class="form_main">
    <table class="TableMain100">
     
        
        <tr>
            <td >
                <table class="" >
                    <tr>
                        <td class="gridcellleft" valign="top" style="height: 40px;padding-right:15px">
                            <dxe:ASPxDateEdit ID="fromDate" ClientInstanceName="fromDt" runat="server" 
                                     EditFormatString="dd-MM-yyyy" DropDownButton-Width="100%"  Font-Size="12px" Width="145px" EditFormat="Custom" UseMaskBehavior="True">
                                    <ButtonStyle Width="13px"></ButtonStyle>
                                <ValidationSettings Display="Dynamic"  ErrorTextPosition="Bottom">
<RequiredField IsRequired="True" ErrorText="Invalid Date"></RequiredField>
</ValidationSettings>
                                    <DropDownButton Text="FromDate" Width="50px"></DropDownButton>
                            </dxe:ASPxDateEdit>
                            
                        </td>
                        
                        
                        <td class="gridcellleft" valign="top" style="height: 40px;padding-right:15px">
                            <dxe:ASPxDateEdit ID="toDate" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                     EditFormatString="dd-MM-yyyy" DropDownButton-Width="100%"  Font-Size="12px" Width="145px">
                                    <ButtonStyle Width="13px"></ButtonStyle>
                                <ValidationSettings Display="Dynamic" ErrorTextPosition="Bottom">
<RequiredField IsRequired="True" ErrorText="Invalid Date"></RequiredField>
</ValidationSettings>
                                    <DropDownButton Text="ToDate" Width="50px"></DropDownButton>
                            </dxe:ASPxDateEdit>
                        </td>
                        <td class="gridcellleft" valign="top" style=" height: 40px;padding-right:15px">
                            <dxe:aspxcombobox id="cmbCompany" runat="server" 
                                 
                                valuetype="System.String" DropDownButton-Text="Company" Font-Size="12px" Width="148px" EnableIncrementalFiltering="True">
                        <ButtonStyle Width="13px"></ButtonStyle>
                                <DropDownButton Text="Company" Width="50px"></DropDownButton>
                        </dxe:aspxcombobox>
                        </td>
                        <td class="gridcellleft" valign="top" style="  height: 40px;padding-right:15px">                
                            <dxe:aspxcombobox id="cmbBranch" runat="server" 
                                 
                                valuetype="System.String" DropDownButton-Text="Branch" Font-Size="12px" Width="148px" EnableIncrementalFiltering="True">
                            <ButtonStyle Width="13px"></ButtonStyle>
                                <DropDownButton Text="Branch" Width="50px"></DropDownButton>
                            </dxe:aspxcombobox>
                        </td>
                        <td class="gridcellleft" valign="top" style=" height: 40px;padding-right:15px">                
                            <dxe:aspxcombobox id="cmbDepartment" runat="server" 
                                 
                                valuetype="System.String" DropDownButton-Text="Department" Font-Size="12px" Width="148px" EnableIncrementalFiltering="True">
                            <ButtonStyle Width="13px"></ButtonStyle>
                                <DropDownButton Text="Department" Width="50px"></DropDownButton>
                            </dxe:aspxcombobox>
                        </td>
                        <td class="gridcellleft" valign="top" style="height: 40px;padding-right:15px">               
                            <dxe:ASPxButton ID="bnSubmit" runat="server" 
                                 Text="GO"  CssClass="btn btn-primary btn-xs" AutoPostBack="False">
                                <ClientSideEvents Click="function(s, e) {
	showgrid();
}" />
                            </dxe:ASPxButton>
                        </td>
                        <td class="gridcellleft" valign="top" style="  height: 40px;padding-right:15px">                
                <dxe:ASPxComboBox ID="cmbExport" runat="server" AutoPostBack="true"  Font-Bold="False" ForeColor="black" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged"  ValueType="System.Int32" Width="112px" >
                        <Items>
<dxe:ListEditItem Text="Select" Value="0"></dxe:ListEditItem>
<dxe:ListEditItem Text="PDF" Value="1"></dxe:ListEditItem>
<dxe:ListEditItem Text="XLS" Value="2"></dxe:ListEditItem>
<dxe:ListEditItem Text="RTF" Value="3"></dxe:ListEditItem>
<dxe:ListEditItem Text="CSV" Value="4"></dxe:ListEditItem>
</Items>
                        <ButtonStyle></ButtonStyle>
                        <Border BorderColor="black" />
                        <ItemStyle >
<HoverStyle ></HoverStyle>
</ItemStyle>
                        <DropDownButton Text="Export"></DropDownButton>
                    </dxe:ASPxComboBox></td>
                    </tr>
                </table>
            </td>
        </tr>
        
        <tr>
            <td id="tdgrid">
                <dxe:ASPxGridView Width="100%" ID="aspxEmpGrid" KeyFieldName="emp_contactId" ClientInstanceName="grid" DataSourceID="SqlEmployeeDetails" runat="server" AutoGenerateColumns="False"
                      OnCustomCallback="aspxEmpGrid_CustomCallback" Settings-ShowHorizontalScrollBar="false" >
                       <ClientSideEvents EndCallback="function (s, e) {aspxEmpGrid_EndCallBack();}" />
<Styles  >
<Header SortingImageSpacing="5px" ImageSpacing="5px"></Header>

<LoadingPanel ImageSpacing="10px"></LoadingPanel>
</Styles>
<Columns>
    <dxe:GridViewDataDateColumn Caption="Joining Date" FieldName="joindate"
        VisibleIndex="0" >
        <CellStyle HorizontalAlign="Left" Wrap="False">
        </CellStyle>
    </dxe:GridViewDataDateColumn>
    <dxe:GridViewDataTextColumn Caption="EmpName[code]" FieldName="Name" VisibleIndex="1"
        Width="50px">
        <CellStyle HorizontalAlign="Left" Wrap="False">
        </CellStyle>
    </dxe:GridViewDataTextColumn>
    <dxe:GridViewDataTextColumn Caption="Designation" FieldName="designation" VisibleIndex="2"
        >
        <CellStyle HorizontalAlign="Left" Wrap="False">
        </CellStyle>
    </dxe:GridViewDataTextColumn>
    <dxe:GridViewDataTextColumn Caption="Company" FieldName="company" VisibleIndex="3"
        >
        <CellStyle HorizontalAlign="Left" Wrap="False">
        </CellStyle>
    </dxe:GridViewDataTextColumn>
    <dxe:GridViewDataTextColumn Caption="Branch" FieldName="branch" VisibleIndex="4"
        >
        <CellStyle HorizontalAlign="Left" Wrap="False">
        </CellStyle>
    </dxe:GridViewDataTextColumn>
    <dxe:GridViewDataTextColumn Caption="Department" FieldName="department" VisibleIndex="5"
        >
        <CellStyle HorizontalAlign="Left" Wrap="False">
        </CellStyle>
    </dxe:GridViewDataTextColumn>
    <dxe:GridViewDataTextColumn Caption="Reporting Head" FieldName="reportTo" VisibleIndex="6"
        >
        <CellStyle HorizontalAlign="Left" Wrap="False">
        </CellStyle>
    </dxe:GridViewDataTextColumn>

</Columns>
<settings  ShowHorizontalScrollBar="false" ></settings>
<StylesEditors>
<ProgressBar Height="25px"></ProgressBar>
</StylesEditors>
<SettingsBehavior AllowFocusedRow="True" />  
                     <SettingsPager AlwaysShowPager="True" NumericButtonCount="20" ShowSeparators="True" PageSize="20">
                                                <FirstPageButton Visible="True">
                                                </FirstPageButton>
                                                <LastPageButton Visible="True">
                                                </LastPageButton>
                                            </SettingsPager>
                    <Settings ShowGroupPanel="True" />
</dxe:ASPxGridView>
                <dxe:ASPxGridViewExporter ID="exporter" Landscape="true" runat="server">
                </dxe:ASPxGridViewExporter>
            </td>
        </tr>
    </table>
    <asp:SqlDataSource ID="SqlEmployeeDetails" runat="server">
    </asp:SqlDataSource>
</div>
</asp:Content>
