<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.ToolsUtilities.management_ToolsUtilities_EOD_Process" Codebehind="EOD_Process.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     
<style type="text/css">
    #divLogBackup {margin-bottom:15px;}
    .imgHand{ float:left; background-image: url(../images/rightArrowHand.jpg); width: 33px; height: 12px;}
    </style>
<script language="javascript" type="text/javascript">
     function btnStartLogBackUp_Click()
     {
        cCbpEOD.PerformCallback("EOD_LogBackUp~");
     }
     function btnRecycleExcel_Click()
     {
        cCbpEOD.PerformCallback("EOD_RecycleExcel~");
     }     
     function CbpEOD_EndCallBack()
     {
        if(cCbpEOD.cpLogBackupInfo!=undefined)
        {
            if(cCbpEOD.cpLogBackupInfo=="Success")
            {
              alert('Log EOD Process Successfully Completed');
            }
            else
            {
                alert('There is Some Problem to Complete Process');
            }
        }
        if(cCbpEOD.cpRecycleExcelInfo!=undefined)
        {
            if(cCbpEOD.cpRecycleExcelInfo=="Recycled")
            {
              alert('Recycle Excel EOD Process Successfully Completed');
            }
            else if (cCbpEOD.cpRecycleExcelInfo == "Failure")
            {
                alert('There is Some Problem to Complete Process');
            }
            else if (cCbpEOD.cpRecycleExcelInfo == "Directory")
            {
                alert('Directory does not exist.');
            }
        }
     }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <div class="panel-heading">
       <div class="panel-title">
           <h3>End Of Day Processes</h3>
       </div>

   </div> 
        <div class="form_main">
          <%--  <table border="1" width="98%">
                <tr>
                    <td class="EHEADER" align="center" colspan="6">
                        <strong><span id="SpanHeader" style="color: #000099">End Of Day Processes</span></strong>
                    </td>
                </tr>
            </table>
            <br />--%>
            <dxe:ASPxCallbackPanel ID="CbpEOD" runat="server" ClientInstanceName="cCbpEOD" OnCallback="CbpEOD_Callback">
                <ClientSideEvents EndCallback="function(s, e) {CbpEOD_EndCallBack(); }" />
                <PanelCollection>
                    <dxe:PanelContent runat="server">
                        <div id="divLogBackup">
                            <div class="imgHand" title="This Will Flush The Memory Where Log Save that Cause Low Performance. Take Log BackUp atleast in a Week for Good Performance..">
                            </div>
                            <div>
                                <dxe:ASPxButton ID="btnStartLogBackUp" runat="server" AutoPostBack="False" Text="Start Log BackUp"
                                    Width="200px" UseSubmitBehavior="False" ToolTip="Please Click To Start and Wait Until Process Complete." CssClass="btn btn-primary">
                                    <ClientSideEvents Click="function(s, e) {btnStartLogBackUp_Click();}"></ClientSideEvents>
                                </dxe:ASPxButton>
                            </div>
                        </div>
                        <div id="divExcelRecycle">
                            <div class="imgHand" title="Recycle Excel Contents atleast in a Week for Good Performance..">
                            </div>
                            <div>
                                <dxe:ASPxButton ID="btnRecycleExcel" runat="server" AutoPostBack="False" Text="Recycle Excel"
                                    Width="200px" UseSubmitBehavior="False" ToolTip="Please Click To Start and Wait Until Process Complete." CssClass="btn btn-primary">
                                    <ClientSideEvents Click="function(s, e) {btnRecycleExcel_Click();}"></ClientSideEvents>
                                </dxe:ASPxButton>
                            </div>
                        </div>
                    </dxe:PanelContent>
                </PanelCollection>
            </dxe:ASPxCallbackPanel>
        </div>
</asp:Content>
