<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MainDasBoard.aspx.cs" Inherits="DashBoard.DashBoard.MainDasBoard" %>

<%@ Register assembly="DevExpress.Web.ASPxScheduler.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxScheduler" tagprefix="dxwschs" %>
<%@ Register assembly="DevExpress.XtraScheduler.v15.1.Core, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraScheduler" tagprefix="cc1" %>

<%@ Register assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Dash Board</title>
     
    <script src="Js/jquery-1.3.2.min.js"></script>
    <script src="Js/SchedulerDashBoard.js?v=0.1"></script>


     <style>
         .pendingTaskBtn {
            width: 154px;
            height: 31px;
            margin-bottom: 2px;
            color: #ffffff;
            background: #5c6068ad;
            background-color: #686c74; 
            font-weight: bold; 
            cursor: zoom-in;
         }
         .HeaderStyle-CssClass="GridColumnHeader" {
         font-weight: bold; 
         }
         #editStatus {
         color:red}
     </style>

</head>
    
<body>
    <form id="form1" runat="server">
    <div>
    
<dx:ASPxPageControl ID="ASPxPageControl1" runat="server">
    <TabPages>
            <dx:TabPage Name="Scheduler" Text="Scheduler">
                <ContentCollection>
                    <dx:ContentControl runat="server">

                        <input id="Button1" type="button" value="Task Status" class="pendingTaskBtn" onclick="MyTaskClick()"/> <label style="font-weight:bold; color:maroon"> <- Click to Update Pending/Completed Status.</label>

           <dxwschs:ASPxScheduler ID="ASPxScheduler1" runat="server"   AppointmentDataSourceID="SqlDataSource1" ClientIDMode="AutoID" ResourceDataSourceID="SqlDataSource2" Start="2018-04-27" Theme="Default" OnPopupMenuShowing="ASPxScheduler1_PopupMenuShowing">
            <Storage>
                <Appointments AutoRetrieveId="True">
                    <Mappings AllDay="AllDay" AppointmentId="UniqueID" Description="Description" End="EndDate" Label="Label" Location="Location" RecurrenceInfo="RecurrenceInfo" ReminderInfo="ReminderInfo" ResourceId="ResourceID" Start="StartDate" Status="Status" Subject="Subject" TimeZoneId="ResourceIDs" Type="Type" />
                    <CustomFieldMappings>
                        <dxwschs:ASPxAppointmentCustomFieldMapping Member="CustomField1" Name="CustomField1" />
                    </CustomFieldMappings>
                </Appointments>
                <Resources>
                    <Mappings Caption="ResourceName" Color="Color" Image="Image" ResourceId="ResourceID" />
                </Resources>
            </Storage>
            <views>
<DayView ><TimeRulers>
<cc1:TimeRuler ></cc1:TimeRuler>
</TimeRulers>
</DayView>

<WorkWeekView><TimeRulers>
<cc1:TimeRuler></cc1:TimeRuler>
</TimeRulers>
</WorkWeekView>

                <fullweekview enabled="true">
                    <TimeRulers>
<cc1:TimeRuler></cc1:TimeRuler>
</TimeRulers>
                </fullweekview>
            </views>
        </dxwschs:ASPxScheduler>


                                          <asp:SqlDataSource ID="SqlDataSource2" runat="server" SelectCommand="SELECT * FROM [Resources]"></asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" DeleteCommand="DELETE FROM [Appointments] WHERE [UniqueID] = @UniqueID" InsertCommand="INSERT INTO [Appointments] ([Type], [StartDate], [EndDate], [AllDay], [Subject], [Location], [Description], [Status], [Label], [ResourceID], [ResourceIDs], [ReminderInfo], [RecurrenceInfo], [CustomField1], [UserId]) VALUES (@Type, @StartDate, @EndDate, @AllDay, @Subject, @Location, @Description, @Status, @Label, @ResourceID, @ResourceIDs, @ReminderInfo, @RecurrenceInfo, @CustomField1, @UserId)" 
            SelectCommand="SELECT UniqueID,Type,StartDate,EndDate,AllDay,Subject + case isComplete when 1 then ' (Completed)' else ' (Pending)' end Subject,Location,Description,Status,Label,ResourceID,ResourceIDs,ReminderInfo,RecurrenceInfo,CustomField1,UserId FROM [Appointments] WHERE ([UserId] = @UserId)" 
            UpdateCommand="UPDATE [Appointments] SET [Type] = @Type, [StartDate] = @StartDate, [EndDate] = @EndDate, [AllDay] = @AllDay, [Subject] = @Subject, [Location] = @Location, [Description] = @Description, [Status] = @Status, [Label] = @Label, [ResourceID] = @ResourceID, [ResourceIDs] = @ResourceIDs, [ReminderInfo] = @ReminderInfo, [RecurrenceInfo] = @RecurrenceInfo, [CustomField1] = @CustomField1, [UserId] = @UserId WHERE [UniqueID] = @UniqueID">
            <DeleteParameters>
                <asp:Parameter Name="UniqueID" Type="Int32" />
            </DeleteParameters>
            <InsertParameters>
                <asp:Parameter Name="Type" Type="Int32" />
                <asp:Parameter Name="StartDate" Type="DateTime" />
                <asp:Parameter Name="EndDate" Type="DateTime" />
                <asp:Parameter Name="AllDay" Type="Boolean" />
                <asp:Parameter Name="Subject" Type="String" />
                <asp:Parameter Name="Location" Type="String" />
                <asp:Parameter Name="Description" Type="String" />
                <asp:Parameter Name="Status" Type="Int32" />
                <asp:Parameter Name="Label" Type="Int32" />
                <asp:Parameter Name="ResourceID" Type="Int32" />
                <asp:Parameter Name="ResourceIDs" Type="String" />
                <asp:Parameter Name="ReminderInfo" Type="String" />
                <asp:Parameter Name="RecurrenceInfo" Type="String" />
                <asp:Parameter Name="CustomField1" Type="String" />
                <asp:SessionParameter Name="UserId" SessionField="userid" Type="Int64" DefaultValue="0" />
                <%--<asp:Parameter Name="UserId" Type="Int64" />--%>
            </InsertParameters>
            <SelectParameters>
                <asp:SessionParameter DefaultValue="0" Name="UserId" SessionField="userid" Type="Int64" />
            </SelectParameters>
            <UpdateParameters>
                <asp:Parameter Name="Type" Type="Int32" />
                <asp:Parameter Name="StartDate" Type="DateTime" />
                <asp:Parameter Name="EndDate" Type="DateTime" />
                <asp:Parameter Name="AllDay" Type="Boolean" />
                <asp:Parameter Name="Subject" Type="String" />
                <asp:Parameter Name="Location" Type="String" />
                <asp:Parameter Name="Description" Type="String" />
                <asp:Parameter Name="Status" Type="Int32" />
                <asp:Parameter Name="Label" Type="Int32" />
                <asp:Parameter Name="ResourceID" Type="Int32" />
                <asp:Parameter Name="ResourceIDs" Type="String" />
                <asp:Parameter Name="ReminderInfo" Type="String" />
                <asp:Parameter Name="RecurrenceInfo" Type="String" />
                <asp:Parameter Name="CustomField1" Type="String" />
                <asp:Parameter Name="UniqueID" Type="Int32" />
                 <asp:SessionParameter Name="UserId" SessionField="userid" Type="Int64" DefaultValue="0" />
                <%--<asp:Parameter Name="UserId" Type="Int64" />--%>
                
            </UpdateParameters>
        </asp:SqlDataSource>


                        <dx:ASPxPopupControl ID="ASPxPopupControl1" runat="server" CloseAction="CloseButton" Height="400px" Width="900px" ClientInstanceName="cMyTaskPopup" CloseOnEscape="True" Modal="True"
                            PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" HeaderText="Task Status">
                            <ContentCollection>
                                <dx:PopupControlContentControl runat="server">
                                    <dx:ASPxGridView ID="ASPxGridView1" runat="server" AutoGenerateColumns="False" DataSourceID="MytaskDataSource" KeyFieldName="UniqueID" Width="900px" 
                                        ClientInstanceName="ctaskgrid">
                                        <Settings ShowFilterRow="True" />
                                        <SettingsDataSecurity AllowDelete="False" AllowInsert="False" AllowEdit="false" />
                                        <SettingsSearchPanel Visible="True" />
                                        <Columns>
                                            <dx:GridViewDataTextColumn FieldName="UniqueID" ReadOnly="True" ShowInCustomizationForm="True" Visible="False" VisibleIndex="0">
                                                <EditFormSettings Visible="False" />
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="Subject" HeaderStyle-CssClass="GridColumnHeader" ShowInCustomizationForm="True" VisibleIndex="1">
<HeaderStyle CssClass="GridColumnHeader"></HeaderStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="Description" HeaderStyle-CssClass="GridColumnHeader" ShowInCustomizationForm="True" VisibleIndex="2">
<HeaderStyle CssClass="GridColumnHeader"></HeaderStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="TaskStatus" HeaderStyle-CssClass="GridColumnHeader" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="3" >
<HeaderStyle CssClass="GridColumnHeader"></HeaderStyle>
                                            </dx:GridViewDataTextColumn>


                                            <dx:GridViewDataTextColumn ReadOnly="True" Width="12%" CellStyle-HorizontalAlign="Center">
                                                <HeaderStyle HorizontalAlign="Center" />

                                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                                <HeaderTemplate>
                                                Actions
                                                </HeaderTemplate>
                                                <DataItemTemplate> 
                                                <a href="javascript:void(0);"  id="editStatus" onclick="PopupOpen(<%#Eval("UniqueID")%>,<%#Eval("isComplete")%>)" title="Action" class="pad" style="text-decoration: none;"
                                                    >
                                              <%#Eval("ActionCol")%>
                                                  
                                                </a>
                                
                                                </DataItemTemplate>
                                            <Settings AllowAutoFilterTextInputTimer="False" />
                                            </dx:GridViewDataTextColumn>




                                        </Columns>
                                    </dx:ASPxGridView>
                                    <asp:SqlDataSource ID="MytaskDataSource" runat="server" SelectCommand="select UniqueID,Subject ,Description,case isComplete when 1 then 'Completed' else 'Pending' end TaskStatus,cast(isComplete as varchar(2)) isComplete,case isComplete when 1 then '' else 'Mark as Completed' end ActionCol  from Appointments where UserId=@UserId">
                                        <SelectParameters>
                                            <asp:SessionParameter DefaultValue="0" Name="UserId" SessionField="userid" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </dx:PopupControlContentControl>
                            </ContentCollection>
                        </dx:ASPxPopupControl>


                    </dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>



    </TabPages>
</dx:ASPxPageControl>
    
        
     
        
        
         
    
    </div>
    </form>
</body>
</html>
