<%@ Page Title="Outgoing RFAs" Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
     Inherits="ERP.OMS.Management.ToolsUtilities.management_utilities_frmrfa" Codebehind="frmrfa.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
       
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="javascript" type="text/javascript">
            function OnGridFocusedRowChanged() {
                // Query the server for the Row ID "rfa_id" fields from the focused row 
                // The values will be returned to the OnGetRowValues() function 
                grid.GetRowValues(grid.GetFocusedRowIndex(), 'rtd_reqnumber', OnGetRowValues);
            }
            //Value array contains Row ID "rfa_id" field values returned from the server 
            function OnGetRowValues(values) {
                RowID = values;
            }
            function btnHistory_Click()
            {
                document.getElementById("TdGrid").style.display = 'inline';   
                document.getElementById("TrCategory").style.display = 'table-cell';   
                document.getElementById("tdGMain").style.display = 'inline';   
                document.getElementById("tdGSub").style.display = 'inline';   
                document.getElementById("TdReq").style.display = 'none';   
                grid1.PerformCallback(RowID);
            }
            function Rfa_Template()
            {
                frmOpenNewWindow_custom('frmshowtemplate_rfa.aspx?tem_id='+ window.document.aspnetForm.lst_templates.options[window.document.aspnetForm.lst_templates.selectedIndex].value,'250','1000','200','0');
            }
            function frmOpenNewWindow_custom(location,v_height,v_weight,top,left)
            {   
                window.open(location,"Search_Conformation_Box","height="+ v_height +",width="+ v_weight +",top="+ top +",left="+ left +",location=no,directories=no,menubar=no,toolbar=no,status=no,scrollbars=yes,resizable=no,dependent=no'");       
            } 
            function GridBind()
            {
                var cmb = document.getElementById("lst_requestcategory");
                grid.PerformCallback(cmb.value);
            }
            function btnRequest_Click()
            {
                document.getElementById("TdGrid").style.display = 'none';   
                document.getElementById("TdReq").style.display = 'inline';   
            }
            function btnStatus_Click()
            {
                document.getElementById("TdGrid").style.display = 'inline';   
                document.getElementById("TrCategory").style.display = 'table-cell';
                document.getElementById("tdGMain").style.display = 'inline';   
                document.getElementById("tdGSub").style.display = 'none';   
                document.getElementById("TdReq").style.display = 'none';   
            }
            function btnCancel_Click()
            {
                document.getElementById("TdGrid").style.display = 'inline';   
                document.getElementById("TrCategory").style.display = 'table-cell';
                document.getElementById("tdGMain").style.display = 'inline';   
                document.getElementById("tdGSub").style.display = 'none';   
                document.getElementById("TdReq").style.display = 'none';   
            }
        </script>
        <script type="text/ecmascript">
            function btnSave_Click()
            {
                var data = 'Save';
                var cmb = document.getElementById("txt_content");
                data+='~'+cmb.value;
                cmb = document.getElementById("lst_templates");
                data+='~'+cmb.value;
                cmb = document.getElementById("HREC");
                data+='~'+cmb.value;
                CallServer(data, "");
                grid.PerformCallback();
                document.getElementById("TdGrid").style.display = 'inline';   
                document.getElementById("TrCategory").style.display = 'table-cell';
                document.getElementById("tdGMain").style.display = 'inline';   
                document.getElementById("tdGSub").style.display = 'none';   
                document.getElementById("TdReq").style.display = 'none';   
            }
            function ReceiveServerData(rValue)
            {
                var DATA=rValue.split('~');
                if(DATA[0]=="Save")
                {    
                    if(DATA[1]="Y")
                    alert('Update Successfully!');
                }
            }
        </script>
     <div class="panel-heading">
        <div class="panel-title">
            <h3>Outgoing RFAs</h3>
        </div>

    </div> 
     <div class="form_main">
         <table width="100%">
             <tr>
                 <td>
                     <input id="btnRequest" type="button" value="Request" class="btnUpdate btn btn-warning" onclick="btnRequest_Click()"  />
                     <input id="btnStatus" type="button" value="Status" class="btnUpdate btn btn-primary" onclick="btnStatus_Click()"  />
                     <input id="btnHistory" type="button" value="History" class="btnUpdate btn btn-primary" onclick="btnHistory_Click()"  />
                 </td>
                 <td id="TrCategory" align="right">
                     <table class="pull-right" style="width:275px;">
                            <tr>
                                <td style="text-align: right;">
                                    <span class="Ecoheadtxt" >Select Request Category  :</span>
                                </td>
                                <td class="gridcellleft">
                                    <asp:DropDownList ID="lst_requestcategory" runat="server" Width="116px">
                                        <asp:ListItem Value="0">Unread</asp:ListItem>
                                        <asp:ListItem Value="1">Pendding</asp:ListItem>
                                        <asp:ListItem Value="2">Approved</asp:ListItem>
                                        <asp:ListItem Value="3">Rejected</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                    </table>
                </td>
             </tr>
         </table>
         
    <table  class="TableMain100">
       
        <tr>
            <%--<td style="text-align:left; vertical-align:top" width="10%">
                <table>
                    <tr>
                        <td>
                            <input id="btnRequest" type="button" value="Request" class="btnUpdate" onclick="btnRequest_Click()" style="width: 71px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <input id="btnStatus" type="button" value="Status" class="btnUpdate" onclick="btnStatus_Click()" style="width: 71px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <input id="btnHistory" type="button" value="History" class="btnUpdate" onclick="btnHistory_Click()" style="width: 71px" />
                        </td>
                    </tr>
                </table>
            </td>--%>
            <td>
                <table class="TableMain100">
                    <tr>
                        <td id="TdGrid">
                            <table class="TableMain100">
                                
                                <tr>
                                    <td colspan="2" id="tdGMain">
                                        <dxe:ASPxGridView ID="grd_request" ClientInstanceName="grid" KeyFieldName="rtd_reqnumber" runat="server" Width="100%" OnCustomCallback="grd_request_CustomCallback">
                                            <Styles>
                                                <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                </Header>
                                                <LoadingPanel ImageSpacing="10px">
                                                </LoadingPanel>
                                            </Styles>
                                               <Columns>
                                                <dxe:GridViewDataTextColumn FieldName="rtd_reqnumber" Visible="False" VisibleIndex="0">
                                                     <CellStyle CssClass="gridcellleft">
                                                     </CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="Request Id" Caption="Short Name" VisibleIndex="0">
                                                     <CellStyle CssClass="gridcellleft">
                                                     </CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="current Approver" Caption="Current Approver" VisibleIndex="1">
                                                     <CellStyle CssClass="gridcellleft">
                                                     </CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="current Status" Caption="Current Status" VisibleIndex="2">
                                                     <CellStyle CssClass="gridcellleft">
                                                     </CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="current Note" Caption="Current Note" VisibleIndex="3">
                                                     <CellStyle CssClass="gridcellleft">
                                                     </CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="Approval No" Caption="Approval No" VisibleIndex="4">
                                                     <CellStyle CssClass="gridcellleft">
                                                     </CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="mesage" Caption="Message" VisibleIndex="5">
                                                     <CellStyle CssClass="gridcellleft">
                                                     </CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                            </Columns>
                                            <StylesEditors>
                                                <ProgressBar Height="25px">
                                                </ProgressBar>
                                            </StylesEditors>
                                            <SettingsBehavior AllowFocusedRow="True" AllowSort="False" />
                                            <ClientSideEvents FocusedRowChanged="function(s, e) { OnGridFocusedRowChanged(); }"/>
                                            <SettingsPager ShowSeparators="True">
                                                <FirstPageButton Visible="True">
                                                </FirstPageButton>
                                                <LastPageButton Visible="True">
                                                </LastPageButton>
                                            </SettingsPager>
                                        </dxe:ASPxGridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" id="tdGSub">
                                        <dxe:ASPxGridView ID="grd_reqhistory" runat="server" Width="100%" ClientInstanceName="grid1" OnCustomCallback="grd_reqhistory_CustomCallback">
                                            <Styles>
                                                <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                </Header>
                                                <LoadingPanel ImageSpacing="10px">
                                                </LoadingPanel>
                                            </Styles>
                                            <StylesEditors>
                                                <ProgressBar Height="25px">
                                                </ProgressBar>
                                            </StylesEditors>
                                            <Columns>
                                                <dxe:GridViewDataTextColumn FieldName="ID" Caption="ID" VisibleIndex="0">
                                                     <CellStyle CssClass="gridcellleft">
                                                     </CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="Approver Name" Caption="Approver Name" VisibleIndex="1">
                                                     <CellStyle CssClass="gridcellleft">
                                                     </CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="Read Date" Caption="Read Date" VisibleIndex="2">
                                                     <CellStyle CssClass="gridcellleft">
                                                     </CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="Previous Status" Caption="Status" VisibleIndex="3">
                                                     <CellStyle CssClass="gridcellleft">
                                                     </CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="Note" Caption="Note" VisibleIndex="4">
                                                     <CellStyle CssClass="gridcellleft">
                                                     </CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="Next Approver" Caption="Next Approver" VisibleIndex="5">
                                                     <CellStyle CssClass="gridcellleft">
                                                     </CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                            </Columns>
                                            <SettingsPager ShowSeparators="True">
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
                    <tr>
                        <td id="TdReq">
                            <table class="TableMain100">
                                <tr>
                                    <td style="width: 197px;">
                                        <span class="Ecoheadtxt" >Select Template For Request :</span>
                                    </td>
                                    <td class="gridcellleft">
                                        <asp:DropDownList ID="lst_templates" runat="server" Width="271px"  ClientIDMode="Static">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 197px;">
                                        <span class="Ecoheadtxt" >Content :</span>
                                    </td>
                                    <td class="gridcellleft">
                                        <asp:TextBox ID="txt_content" runat="server" TextMode="MultiLine" Height="54px" Width="265px"  ClientIDMode="Static"></asp:TextBox>
                                        <asp:HiddenField ID="HREC" runat="server" ClientIDMode="Static" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td style="text-align:left">
                                        <input id="btnSave" type="button" value="Save" class="btnUpdate btn btn-primary" onclick="btnSave_Click()" />
                                        <input id="btnCancel" type="button" value="Cancel" class="btnUpdate btn btn-primary" onclick="btnCancel_Click()"  />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
          </div> 
</asp:Content>
