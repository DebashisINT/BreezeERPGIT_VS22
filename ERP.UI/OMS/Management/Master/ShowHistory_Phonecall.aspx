<%@ Page Title="Show History" Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/Erp.Master"
    Inherits="ERP.OMS.Management.Master.management_Activities_ShowHistory_Phonecall" CodeBehind="ShowHistory_Phonecall.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function InsertFeedback(sid, dtid, tid, Supervisor, Note, activitystatus, NextCall, CallDate) {

          
          //  alert(slsid+' '+tid)
            $('#MandatoryRemarksFeedback').attr('style', 'display:none;position: absolute; right: -20px; top: 8px;');
            txtFeedback.SetValue();
            $('#chkmailfeedback').prop('checked', false);
            stid = sid;
            refeedbackvar = dtid;
            sSupervisor = Supervisor;
            ActivityNote = Note;
            Outcome = activitystatus;
            sNextCall = NextCall;
            sCalldate = CallDate;
            $("#hdtid").val(tid);
            cPopup_Feedback.Show();
        }


        function CallFeedback_save() {
            cPopup_Feedback.Hide();
            var flag = true;
            var Remarks = txtFeedback.GetValue();
            if (Remarks == "" || Remarks == null) {
                $('#MandatoryRemarksFeedback').attr('style', 'display:block;position: absolute; right: -20px; top: 8px;');
                flag = false;
            }
            else {
                $('#MandatoryRemarksFeedback').attr('style', 'display:none;position: absolute; right: -20px; top: 8px;');

                grid.PerformCallback('Feedback~' + refeedbackvar + '~' + Remarks + '~' + $("#hdtid").val() + '~' + stid + '~' + sSupervisor + '~' + ActivityNote + '~' + Outcome + '~' + sNextCall + '~' + sCalldate);
                grid.Refresh();
            }
            return flag;

        }




        function CancelFeedback_save() {


            txtFeedback.SetValue();
            cPopup_Feedback.Hide();
            $('#chkmailfeedback').prop('checked', false);
        }


        function ShowDetailFeedBack(stid,actid,Typeid) {

        
            cAspxAspxFeedGrid.PerformCallback('Feedbackdetails~' + actid + '~' + Typeid + '~' + stid);
            cPopup_Feddbackdetails.Show();
            // cComponentPanel.PerformCallback(slsid);
          
        }

        <%--$(document).ready(function () {
            var mod = '<%= Session["Contactrequesttype"] %>';
            if (mod == 'customer') {
                document.getElementById("lnkClose").href = 'CustomerMasterList.aspx';
            }
            else {
                document.getElementById("lnkClose").href = 'frmContactMain.aspx?requesttype=<%= Session["Contactrequesttype"] %>';
            }
        })--%>

    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title clearfix">
            <h3 class="pull-left">
                <asp:Label ID="lblHead" runat="server" Text=""></asp:Label>
            </h3>
            <div id="content-6" class="pull-right reverse wrapHolder content horizontal-images" style="margin-right: 0px;">
                                                    <ul>
                                                        <li>
                                                            <div class="lblHolder">
                                                                <table>
                                                                    <tbody>
                                                                        <tr>
                                                                            <td>Assigned By </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="lblassignedBy" runat="server"></asp:Label></td>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>
                                                            </div>

                                                        </li>


                                                        <li>
                                                            <div class="lblHolder">
                                                                <table>
                                                                    <tbody>
                                                                        <tr>
                                                                            <td>Assigned To </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="lblassignedto" runat="server"></asp:Label>
                                                                               

                                                                            </td>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>
                                                            </div>

                                                        </li>


                                                       
                                                    </ul>

                                                </div>
        </div>
        <div class="crossBtn" id="crsbuttn" runat="server">
            
            <%--<a href="Lead.aspx"><i class="fa fa-times"></i></a>--%>
            <asp:HyperLink
                ID="goBackCrossBtn" ClientIDMode="Static"
                NavigateUrl="frmContactMain.aspx?requesttype=Lead"
                runat="server">
                <i class="fa fa-times"></i>
            </asp:HyperLink>
        </div>
    </div>

    <div class="form_main">
        <div class="clearfix" id="divdetails">
            <div class="pull-left">
                <div style="float: left; padding-right: 5px;">

                    <asp:DropDownList ID="drdSalesActivityDetails" runat="server" Height="34px" CssClass="btn btn-sm btn-primary expad" OnSelectedIndexChanged="drdSalesActivityDetails_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Value="0">Export to</asp:ListItem>
                        <asp:ListItem Value="1">PDF</asp:ListItem>
                        <asp:ListItem Value="2">XLS</asp:ListItem>
                        <asp:ListItem Value="3">RTF</asp:ListItem>
                        <asp:ListItem Value="4">CSV</asp:ListItem>
                    </asp:DropDownList>


                </div>
            </div>

                
        </div>

        <table class="TableMain100" width="100%">
            <tr id="headerid" runat="server">
                <td class="Ecoheadtxt">
                    <h3 style="background: #cecbc8; font-size: 18px; padding: 8px;font-weight: 300;">
                        <asp:Label ID="lblName" runat="Server"></asp:Label>
                    </h3>
                </td>
            </tr>


            <tr id="AllOtherHistory" runat="server">

                <td>
                    <dxe:ASPxGridView ID="SalesDetailsGrid" runat="server" AutoGenerateColumns="False" ClientInstanceName="grid"
                        Width="100%"  OnCustomCallback="SalesDetailsGrid_CustomCallback" DataSourceID="SalesDetailsGridDataSource" >
                        <Columns>

                            <%-- <dxe:GridViewDataComboBoxColumn FieldName="AssignTo" Caption="Assigned To" GroupIndex="0" SortOrder="Descending">
                <PropertiesComboBox DataSourceID="AssignedToDataSource"  
                    ValueField="cnt_id" TextField="AssignTo" />
            </dxe:GridViewDataComboBoxColumn>--%>

                            <dxe:GridViewDataComboBoxColumn FieldName="Type" Visible="true" Caption="Activity Type" GroupIndex="0" PropertiesComboBox-EnableSynchronization="True" Settings-AllowDragDrop="True" Settings-AllowGroup="False">
                                <PropertiesComboBox DataSourceID="ActivityTypeDataSource"
                                    ValueField="Type" TextField="Type" />
                            </dxe:GridViewDataComboBoxColumn>

                            <dxe:GridViewDataTextColumn FieldName="Assigned Date" Caption="Assigned On" GroupIndex="1" Width="13%">
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn FieldName="AssignTo" Caption="Assigned To" VisibleIndex="0" Visible="false">
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn FieldName="Type" VisibleIndex="10" Caption="Activity Type" Visible="false">
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn FieldName="AssignedBy" Caption="Assigned By" VisibleIndex="1"
                                Width="18%" Visible="false">
                            </dxe:GridViewDataTextColumn>

                            <%--<dxe:GridViewDataTextColumn FieldName="Assigned Date" Caption="Assigned On" VisibleIndex="2"
                                Width="13%">
                            </dxe:GridViewDataTextColumn>--%>

                            <dxe:GridViewDataTextColumn FieldName="CallDate" VisibleIndex="2" Caption="Call/Visit Date">
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn FieldName="Supervisor" VisibleIndex="3">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Note" VisibleIndex="4" Caption="Activity Note">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="activitystatus" VisibleIndex="5" Caption="Outcome">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Status" VisibleIndex="6" Caption="Status" Width="12%">
                            </dxe:GridViewDataTextColumn>


                            <dxe:GridViewDataTextColumn FieldName="NextCall" VisibleIndex="7" Caption="Next Activity Date">
                            </dxe:GridViewDataTextColumn>
                            <%--<dxe:GridViewDataTextColumn FieldName="CallDate" VisibleIndex="8" Caption="Call/Visit Date">
                            </dxe:GridViewDataTextColumn>--%>
                             <dxe:GridViewDataTextColumn FieldName="feed_remarks" VisibleIndex="8" Caption="Feedback Details">
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="9" Caption="Actions"  Width="80px">
                                <CellStyle HorizontalAlign="Center">
                                </CellStyle>
                                <HeaderStyle HorizontalAlign="Center" />
                                <DataItemTemplate>
                                       <a href="javascript:void(0)" onclick="ShowDetailFeedBack('<%#Eval("sls_id") %>','<%#Eval("detailsid") %>','<%#Eval("Tid") %>')" class="pad">
                                        <img src="/assests/images/history.png" title="Feedback Details" width="20" height="20"></a>
                                  
                                    <a href="javascript:void(0)" onclick="InsertFeedback('<%#Eval("sls_id") %>','<%#Eval("detailsid") %>','<%#Eval("Tid") %>','<%#Eval("Supervisor") %>','<%#Eval("Note") %>','<%#Eval("activitystatus") %>','<%#Eval("NextCall") %>','<%#Eval("CallDate") %>')" class="pad">
                                        <img src="/assests/images/feedback.png" title="Feedback" width="20" height="20"></a>
                                </DataItemTemplate>
                                <EditFormSettings Visible="False" />
                            </dxe:GridViewDataTextColumn>

                        </Columns>
                        <Styles>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                            <Header ImageSpacing="5px" SortingImageSpacing="5px" >
                            </Header>
                            <Cell CssClass="gridcellleft">
                            </Cell>
                        </Styles>
                        <SettingsPager NumericButtonCount="20" ShowSeparators="True">
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </SettingsPager>
                        <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" />
                        <SettingsBehavior AutoExpandAllGroups="true" />

                    </dxe:ASPxGridView>
                </td>

            </tr>

            <tr>
                <td class="Ecoheadtxt">
                    <div class="Ecoheadtxt" runat="Server" id="showActivityPanl">
                    </div>
                </td>
            </tr>

        </table>
    </div>


    <dxe:ASPxPopupControl ID="Popup_Feedback" runat="server" ClientInstanceName="cPopup_Feedback"
            Width="400px" HeaderText="Feedback" PopupHorizontalAlign="WindowCenter"
            BackColor="white" Height="100px" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <%--<div style="Width:400px;background-color:#FFFFFF;margin:0px;border:1px solid red;">--%>
                    <div class="Top clearfix">

                        <table style="width:94%">
                           
                            <tr><td>Feedback<span style="color: red">*</span></td>
                                <td class="relative">
                                     <dxe:ASPxMemo ID="txtInstFeedback" runat="server" Width="100%" Height="50px" ClientInstanceName="txtFeedback"></dxe:ASPxMemo>
                                                                        <span id="MandatoryRemarksFeedback" style="display: none">
                                        <img id="gridHistory_DXPEForm_efnew_DXEFL_DXEditor1234_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                                </td></tr>
                             <tr>    <td >
                                      <asp:CheckBox ID="chkmailfeedback" runat="server"  ClientIDMode="Static"  /> Send Mail
                                     </td> <td colspan="2" style="padding-left: 121px;"></td></tr>
                            <tr>
                                <td colspan="3" style="padding-left: 121px;">
                                    <input id="btnFeedbackSave" class="btn btn-primary" onclick="CallFeedback_save()" type="button" value="Save" />
                                    <input id="btnFeedbackCancel" class="btn btn-danger" onclick="CancelFeedback_save()" type="button" value="Cancel" />
                                </td>

                            </tr>
                        </table>


                    </div>

                </dxe:PopupControlContentControl>
            </ContentCollection>
            <HeaderStyle BackColor="LightGray" ForeColor="Black" />

          
        </dxe:ASPxPopupControl>


    <dxe:ASPxPopupControl ID="Popup_Product" runat="server" ClientInstanceName="cPopup_Feddbackdetails"
            Width="700px" HeaderText="Feedback History List" PopupHorizontalAlign="WindowCenter"
            BackColor="white" Height="100px" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton" 
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">

                      
                                                       <dxe:PanelContent runat="server">

                                    <dxe:ASPxGridView ID="AspxFeedGrid" ClientInstanceName="cAspxAspxFeedGrid" Width="100%"  runat="server"  AutoGenerateColumns="False" OnCustomCallback="AspxPFeedbackdetails_CustomCallback">

                                      <Columns>
                                            <dxe:GridViewDataTextColumn Visible="True" FieldName="Name"   Caption="Entered By">
                                                
                                           
                                            </dxe:GridViewDataTextColumn>
                    

                                              <dxe:GridViewDataTextColumn Visible="True" FieldName="Datefeed"   Caption="Entered Date">
                                                
                                           
                                            </dxe:GridViewDataTextColumn>
                                         

                                            <dxe:GridViewDataTextColumn Visible="True" FieldName="Remarks"   Caption="Details">
                                            
                                           
                                            </dxe:GridViewDataTextColumn>

                                     </Columns>
                                                             
                                                            <SettingsBehavior AllowSort="false" AllowGroup="false" />
                        </dxe:ASPxGridView>
          

                                                              </dxe:PanelContent>
                                              
                        </dxe:PopupControlContentControl>
               
            </ContentCollection>
            <HeaderStyle BackColor="LightGray" ForeColor="Black" />

          
        </dxe:ASPxPopupControl>


    <asp:HiddenField ID="hdtid" runat="server"/>
    <%--   <asp:SqlDataSource ID="AssignedToDataSource" runat="server"></asp:SqlDataSource>--%>
    <asp:SqlDataSource ID="SalesDetailsGridDataSource" runat="server" ></asp:SqlDataSource>
    <asp:SqlDataSource ID="ActivityTypeDataSource" runat="server" 
        SelectCommand="SELECT aty_id ,aty_activityType Type FROM tbl_master_activitytype where (Is_Active=1 or aty_id=9)"></asp:SqlDataSource>
    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="true" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>
</asp:Content>
