<%@ Page Language="C#" AutoEventWireup="True" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.management_Branch" Codebehind="Branch.aspx.cs" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Branch</title>
    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />
      <script language="javascript" type="text/javascript">
    
      function OnEditButtonClick(keyValue)
    {
      var url='BranchAddEdit.aspx?id='+ keyValue;
      parent.OnMoreInfoClick(url,"Add New Accout",'920px','500px',"Y");
    }
    
      function OnAddButtonClick() 
    {
         var url='BranchAddEdit.aspx?id=ADD';
         parent.OnMoreInfoClick(url,"Add New Accout",'920px','500px',"Y");
    }
    
    
     function DeleteRow(keyValue)
    {
             doIt=confirm('Confirm delete?');
            if(doIt)
                {
                   grid.PerformCallback('Delete~'+ keyValue);
                   height();
                }
            else{
                  
                }

   
    }
    
    
    function ShowHideFilter1(obj)
    {
    
       gridTerminal.PerformCallback(obj);
    }
    function SignOff()
    {
        window.parent.SignOff()
    }
    function height()
    {//alert(document.body.scrollHeight);
        if(document.body.scrollHeight<=500)
            window.frameElement.height = '500px';
        else
            window.frameElement.height = document.body.scrollHeight;
        window.frameElement.width = document.body.scrollWidth;
    }
     function ShowHideFilter(obj)
    {
       grid.PerformCallback(obj);
    }
       function Page_Load()
   {
        document.getElementById("TdCombo").style.display="none";
   }
     function callback()
    {
       grid.PerformCallback();
        height();
    } 
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table class="TableMain100" style="width:100%">
            <tr>
                <td class="EHEADER" colspan="2" style="text-align: center;">
                    <strong><span style="color: #000099">Branch Details</span></strong>
                </td>
            </tr>
            <tr>
                <td style="text-align: left; vertical-align: top">
                    <table>
                        <tr>
                            <td id="ShowFilter">
                                <a href="javascript:ShowHideFilter('s');"><span style="color: #000099; text-decoration: underline">
                                    Show Filter</span></a>
                            </td>
                            <td id="Td1">
                                <a href="javascript:ShowHideFilter('All');"><span style="color: #000099; text-decoration: underline">
                                    All Records</span></a>
                            </td>
                        </tr>
                    </table>
                </td>
                <td class="gridcellright">
                <div style="float:right">
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
                    </dxe:ASPxComboBox></div>
                </td>
            </tr>
            <tr>
             <dxe:ASPxGridView ID="gridStatus" ClientInstanceName="grid"  Width="100%"
                            KeyFieldName="branch_id" DataSourceID="gridStatusDataSource" runat="server"
                            AutoGenerateColumns="False" OnCustomCallback="gridStatus_CustomCallback">
                            <SettingsBehavior  ConfirmDelete="True" />
                            <Styles>
                               <%-- <FocusedRow CssClass="gridselectrow" BackColor="#FCA977">
                                </FocusedRow>
                                <FocusedGroupRow CssClass="gridselectrow" BackColor="#FCA977">
                                </FocusedGroupRow>--%>
                            </Styles>
                            <SettingsPager NumericButtonCount="20" PageSize="20" ShowSeparators="True" AlwaysShowPager="True">
                                <FirstPageButton Visible="True">
                                </FirstPageButton>
                                <LastPageButton Visible="True">
                                </LastPageButton>
                            </SettingsPager>
                            <Columns>
                                <dxe:GridViewDataTextColumn Visible="False" FieldName="branch_id" Caption="ID" >
                                    <CellStyle CssClass="gridcellleft">
                                    </CellStyle>
                                    <EditFormSettings Visible="False"></EditFormSettings>
                                </dxe:GridViewDataTextColumn>
                            
                                <dxe:GridViewDataTextColumn VisibleIndex="0" FieldName="branch_code" 
                                    Caption="Code">
                                    <CellStyle CssClass="gridcellleft" Wrap="True">
                                    </CellStyle>
                                    <EditFormSettings Visible="False"></EditFormSettings>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="branch_description" 
                                    Caption="Branch Name">
                                    <CellStyle CssClass="gridcellleft" Wrap="True">
                                    </CellStyle>
                                    <EditFormSettings Visible="False"></EditFormSettings>
                                </dxe:GridViewDataTextColumn>
                                
                                <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="ParentBranch"
                                    Caption="Parent Branch">
                                    <CellStyle CssClass="gridcellleft" Wrap="True">
                                    </CellStyle>
                                    <EditFormSettings Visible="False"></EditFormSettings>
                                </dxe:GridViewDataTextColumn>
                                
                                    <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="branch_cpPhone"
                                    Caption="Branch Phone" Visible="True">
                                    <CellStyle CssClass="gridcellleft" >
                                    </CellStyle>
                                    <EditFormSettings Visible="False"></EditFormSettings>
                                </dxe:GridViewDataTextColumn>
                                
                                 
                                    <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="Country"
                                    Caption="Country" Visible="true">
                                    <CellStyle CssClass="gridcellleft">
                                    </CellStyle>
                                    <EditFormSettings Visible="False"></EditFormSettings>
                                </dxe:GridViewDataTextColumn>
                                  
                                    <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="State"
                                    Caption="State" Visible="True">
                                    <CellStyle CssClass="gridcellleft">
                                    </CellStyle>
                                    <EditFormSettings Visible="False"></EditFormSettings>
                                </dxe:GridViewDataTextColumn>
                                 
                                    <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="City"
                                    Caption="City" Visible="True">
                                    <CellStyle CssClass="gridcellleft">
                                    </CellStyle>
                                    <EditFormSettings Visible="False"></EditFormSettings>
                                </dxe:GridViewDataTextColumn>
                                
                                         
                             <dxe:GridViewDataTextColumn VisibleIndex="10"  Width="60px">
                                      <CellStyle CssClass="gridcellleft">
                                    </CellStyle>
                                     <DataItemTemplate>
                                                                                        
                                                <a href="javascript:void(0);" onclick="DeleteRow('<%# Container.KeyValue %>')">
                                                Delete</a>&nbsp;| &nbsp;<a href="javascript:void(0);" onclick="OnEditButtonClick('<%# Container.KeyValue %>')">More Info</a>
                                             
                                    </DataItemTemplate>
                                       <HeaderTemplate>
                                        <a href="javascript:void(0);" onclick="javascript:OnAddButtonClick();"><span style="color: #000099;
                                            text-decoration: underline">Add New</span> </a>
                                    </HeaderTemplate>
                                    <EditFormSettings Visible="False"></EditFormSettings>
                                </dxe:GridViewDataTextColumn>
                       
                            </Columns>
                            <SettingsText ConfirmDelete="Confirm delete?" />
                            <StylesEditors>
                                <ProgressBar Height="25px">
                                </ProgressBar>
                            </StylesEditors>
                            <Settings ShowHorizontalScrollBar="True" />
                        </dxe:ASPxGridView>
                                <dxe:ASPxGridViewExporter ID="exporter" runat="server">
        </dxe:ASPxGridViewExporter>
                                    <asp:SqlDataSource ID="gridStatusDataSource" runat="server" 
                SelectCommand="">
                <SelectParameters>
                    <asp:SessionParameter Name="userlist" SessionField="userchildHierarchy" Type="string" />
                </SelectParameters>
            </asp:SqlDataSource>
            </tr>
            </table>
    </div>
    </form>
</body>
</html>
