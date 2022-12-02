<%@ Page Title="Incoming RFAs" Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.ToolsUtilities.management_utilities_frmrfa_transaction" Codebehind="frmrfa_transaction.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     
   <script language="javascript" type="text/javascript">
        function OnGridFocusedRowChanged() {
                // Query the server for the Row ID "req_Id" fields from the focused row 
                // The values will be returned to the OnGetRowValues() function 
                grid.GetRowValues(grid.GetFocusedRowIndex(), 'req_id', OnGetRowValues);
            }
            //Value array contains Row ID "req_Id" field values returned from the server 
            function OnGetRowValues(values) {
                RowID = values;
            }
        function GridBind()
        {
            var cmb = document.getElementById("lst_requesttype");
            grid.PerformCallback(cmb.value);
        }
        function Page_Load()
        {
            document.getElementById("Tdgrid").style.display = 'inline';
            document.getElementById("TdAll").style.display = 'none';
            document.getElementById("TdHistory").style.display = 'none';
        }
    </script>
    <script type="text/ecmascript">
        function btnSave_Click()
        {
           
            var data='Save';
            var cmb = document.getElementById("txt_topic");           
           
            data+='~'+cmb.value;
            cmb = document.getElementById("txt_content");
            data+='~'+cmb.value;
            cmb = document.getElementById("lst_status");
            data+='~'+cmb.value;
            cmb = document.getElementById("txt_note");
            data+='~'+cmb.value;
            cmb = document.getElementById("hdID");        
            data += '~' + cmb.value;           
            CallServer(data, "");
            grid.PerformCallback('Save');
            document.getElementById("Tdgrid").style.display = 'inline';
            document.getElementById("TdAll").style.display = 'none';
            document.getElementById("TdHistory").style.display = 'none';
        }
        function btnRead_Click()
        {
            var data='Read'
            data+='~'+RowID
            CallServer(data, "");
            document.getElementById("Tdgrid").style.display = 'none';
            document.getElementById("TdAll").style.display = 'inline';
            document.getElementById("btnRecord").style.display = 'none';
            document.getElementById("btnSave").style.display = 'none';
            document.getElementById("TdHistory").style.display = 'none';
        }
        function btnModify_Click()
        {
            var data='Modify'
            data+='~'+RowID;
            CallServer(data, "");
            document.getElementById("Tdgrid").style.display = 'none';
            document.getElementById("TdAll").style.display = 'inline';
            document.getElementById("btnRecord").style.display = 'inline';
            document.getElementById("btnSave").style.display = 'inline';
            document.getElementById("TdHistory").style.display = 'none';
        }
        function btnCancel_Click()
        {
            var data='Cancel'
            CallServer(data, "");
            document.getElementById("Tdgrid").style.display = 'inline';
            document.getElementById("TdAll").style.display = 'none';
            document.getElementById("TdHistory").style.display = 'none';
        }
        function btnHistory_Click()
        {
            grid1.PerformCallback(RowID);
            document.getElementById("Tdgrid").style.display = 'inline';
            document.getElementById("TdAll").style.display = 'none';
            document.getElementById("TdHistory").style.display = 'inline';
        }
        function btnRecord_Click()
        {
            var data='Record'
            var cmb = document.getElementById("txt_topic");
            data+='~'+cmb.value;
            cmb = document.getElementById("txt_content");
            data+='~'+cmb.value;
            cmb = document.getElementById("lst_status");
            data+='~'+cmb.value;
            cmb = document.getElementById("txt_note");
            data+='~'+cmb.value;
            cmb = document.getElementById("hdID");
            data+='~'+cmb.value;
            CallServer(data, "");
            document.getElementById("Tdgrid").style.display = 'inline';
            document.getElementById("TdAll").style.display = 'none';
            document.getElementById("TdHistory").style.display = 'none';
        }
        function ReceiveServerData(rValue)
            {
                var DATA=rValue.split('~');
                if(DATA[0]=="Save")
                {    
                    if(DATA[1]="Y")
                    alert('Update Successfully!');
                }
                if(DATA[0]=="Record")
                {    
                    if(DATA[1]="Y")
                    alert('Recorded Successfully!');
                }
                if(DATA[0]=="Read")
                {    
                    var cmb = document.getElementById("txt_topic");
                    cmb.value=DATA[1];
                    cmb = document.getElementById("txt_content");
                    cmb.value=DATA[2];
                    cmb = document.getElementById("txt_note");
                    cmb.value=DATA[3];
                    cmb = document.getElementById("lst_status");
                    cmb.SelectedIndex=DATA[4];
                    cmb = document.getElementById("hdID");
                    cmb.value=DATA[7];
                    var currentlevel=DATA[5];
                    var totallevel=DATA[6];
                    if(currentlevel==totallevel)
                    {
                        document.getElementById("lst_status").style.display = 'none';
                        document.getElementById("btnCancel").style.display = 'none';
                        document.getElementById("btnRecord").style.display = 'inline';
                        document.getElementById("btnSave").style.display = 'none';
                    }
                    else
                    {
                        document.getElementById("lst_status").style.display = 'inline';
                        document.getElementById("btnCancel").style.display = 'inline';
                        document.getElementById("btnRecord").style.display = 'none';
                        document.getElementById("btnSave").style.display = 'inline';
                    }
                }
                if(DATA[0]=="Modify")
                {    
                    var cmb = document.getElementById("txt_topic");
                    cmb.value=DATA[1];
                    cmb = document.getElementById("txt_content");
                    cmb.value=DATA[2];
                    cmb = document.getElementById("txt_note");
                    cmb.value=DATA[3];
                    cmb = document.getElementById("lst_status");
                    cmb.SelectedIndex=DATA[4];
                    cmb = document.getElementById("hdID");
                    cmb.value=DATA[7];
                    var currentlevel=DATA[5];
                    var totallevel=DATA[6];
                    if(currentlevel==totallevel)
                    {
                        document.getElementById("lst_status").style.display = 'inline';
                        document.getElementById("btnCancel").style.display = 'none';
                        document.getElementById("btnRecord").style.display = 'inline';
                        document.getElementById("btnSave").style.display = 'inline';
                    }
                    else
                    {
                        document.getElementById("lst_status").style.display = 'inline';
                        document.getElementById("btnCancel").style.display = 'inline';
                        document.getElementById("btnRecord").style.display = 'none';
                        document.getElementById("btnSave").style.display = 'inline';
                    }
                }
            }
    </script>
</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="panel-heading">
        <div class="panel-title">
            <h3>Incoming RFAs</h3>
        </div>

    </div>
      <div class="form_main">
          <div class="row">
              <div class="col-md-12">
                  <input id="btnRead" type="button" value="Read" onclick="btnRead_Click()" class=" btn btn-primary " />
                  <input id="btnModify" type="button" value="Modify" onclick="btnModify_Click()" class=" btn btn-success " />
                  <input id="btnHistory" type="button" value="History" onclick="btnHistory_Click()" class=" btn btn-info" />
                  <div class="pull-right">
                      <span class="Ecoheadtxt" >Select RFA Category :</span>
                      <asp:DropDownList ID="lst_requesttype" runat="server" Width="107px" >
                            <asp:ListItem Value="1">New</asp:ListItem>
                            <asp:ListItem Value="2">Pending</asp:ListItem>
                            <asp:ListItem Value="3">Forward</asp:ListItem>
                            <asp:ListItem Value="4">Closed</asp:ListItem>
                        </asp:DropDownList>
                  </div>
                            
              </div>
          </div>       
        <table class="TableMain100">
                    <tr>
                        <td id="Tdgrid">
                            <table class="TableMain100">
                                
                                <tr>
                                    <td colspan="3">
                                        <dxe:ASPxGridView ID="grd_incomingrfa" runat="server" Width="100%" ClientInstanceName="grid" KeyFieldName="req_id" OnCustomCallback="grd_incomingrfa_CustomCallback">
                                            <Styles>
                                                <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                </Header>
                                                <LoadingPanel ImageSpacing="10px">
                                                </LoadingPanel>
                                            </Styles>
                                        
                                            <Columns>
                                                <dxe:GridViewDataTextColumn FieldName="req_id" Visible="False" VisibleIndex="0">
                                                     <CellStyle CssClass="gridcellleft">
                                                     </CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="shortname" Caption="Topic" VisibleIndex="0">
                                                     <CellStyle CssClass="gridcellleft">
                                                     </CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="createuser" Caption="Request User" VisibleIndex="1">
                                                     <CellStyle CssClass="gridcellleft">
                                                     </CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="createdate" Caption="Request Date" VisibleIndex="2">
                                                     <CellStyle CssClass="gridcellleft">
                                                     </CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="closedate" Caption="Request EndDate" VisibleIndex="3">
                                                     <CellStyle CssClass="gridcellleft">
                                                     </CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="approver" Caption="Previous Approver" VisibleIndex="4">
                                                     <CellStyle CssClass="gridcellleft">
                                                     </CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="status" Caption="Previous Status" VisibleIndex="5">
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
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td id="TdAll">
                            <table class="TableMain100">
                                <tr>
                                    <td class="headText" colspan="2" ><span style="text-align:center;background: #eeeeee;
    padding: 5px;width:100%;display:block;margin-bottom:10px;">RFA Details</span></td> 
                                </tr>
                                <tr>
                                    <td style="">
                                        <span class="Ecoheadtxt" >Topic :</span>
                                    </td>
                                    <td class="gridcellleft">
                                        <asp:TextBox ID="txt_topic" runat="server" Width="511px" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="">
                                        <span class="Ecoheadtxt" >Content :</span>
                                    </td>
                                    <td class="gridcellleft">
                                        <asp:TextBox ID="txt_content" runat="server" TextMode="MultiLine" Height="55px" Width="511px" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="">
                                        <span class="Ecoheadtxt" >Status :</span>
                                    </td>
                                    <td class="gridcellleft">
                                        <asp:DropDownList ID="lst_status" runat="server" Width="511px" ClientIDMode="Static">
                                            <asp:ListItem Value="0">Not Viewed </asp:ListItem>
                                            <asp:ListItem Value="1">Approv</asp:ListItem>
                                            <asp:ListItem Value="2">Rejected</asp:ListItem>
                                            <asp:ListItem Value="3">Approved with Modifications</asp:ListItem>
                                            <asp:ListItem Value="4">Pending Approval By Higher Authority</asp:ListItem>
                                            <asp:ListItem Value="5">Pending Approval</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style=">
                                        <span class="Ecoheadtxt" >Notes :</span>
                                    </td>
                                    <td class="gridcellleft">
                                        <asp:TextBox ID="txt_note" runat="server" TextMode="MultiLine" Width="511px" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td style="text-align:left">
                                        <input id="btnSave" type="button" value="Save" class="btn btn-primary" onclick="btnSave_Click()" /> 
                                        <input id="btnRecord" type="button" value="Record" class="btn btn-info" onclick="btnRecord_Click()" />
                                        <input id="btnCancel" type="button" value="Cancel" class="btn btn-danger" onclick="btnCancel_Click()" />
                                        <input type="hidden" id="hdID" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td id="TdHistory">
                            <dxe:ASPxGridView ID="grd_reqhistory" runat="server" Width="100%" ClientInstanceName="grid1" OnCustomCallback="grd_reqhistory_CustomCallback">
                                <Styles>
                                    <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                    </Header>
                                    <LoadingPanel ImageSpacing="10px">
                                    </LoadingPanel>
                                </Styles>
                            
                                <Columns>
                                    <dxe:GridViewDataTextColumn FieldName="ID" Caption="Id" Visible="true" VisibleIndex="0">
                                          <CellStyle CssClass="gridcellleft">
                                          </CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn FieldName="Approver Name" Caption="Approver Name" VisibleIndex="0">
                                          <CellStyle CssClass="gridcellleft">
                                          </CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn FieldName="Read Date" Caption="Read Date" VisibleIndex="1">
                                          <CellStyle CssClass="gridcellleft">
                                          </CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn FieldName="Previous Status" Caption="Previous Status" VisibleIndex="2">
                                          <CellStyle CssClass="gridcellleft">
                                          </CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn FieldName="Note" Caption="Note" VisibleIndex="3">
                                           <CellStyle CssClass="gridcellleft">
                                           </CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn FieldName="Next Approver" Caption="Next Approver" VisibleIndex="4">
                                           <CellStyle CssClass="gridcellleft">
                                           </CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                </Columns>
                                <StylesEditors>
                                    <ProgressBar Height="25px">
                                    </ProgressBar>
                                </StylesEditors>
                            </dxe:ASPxGridView>
                        </td>
                    </tr>
                </table>          
    </div>
</asp:Content>
