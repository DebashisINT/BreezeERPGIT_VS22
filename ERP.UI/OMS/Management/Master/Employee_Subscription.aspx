<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true"
    Inherits="ERP.OMS.Management.Master.management_master_Employee_Subscription" Codebehind="Employee_Subscription.aspx.cs" %>

<%--<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe000001" %>

<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxTabControl"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxe" %>--%>
<%--<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxTabControl" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxClasses" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe000001" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
    <script language="javascript" type="text/javascript">
    function disp_prompt(name)
    {
//        alert(name);
        if ( name == "tab0")
        {
        //alert(name);
        document.location.href="Employee_general.aspx"; 
        }
        if ( name == "tab1")
        {
        //alert(name);
        document.location.href="Employee_Correspondence.aspx"; 
        }
        else if ( name == "tab2")
        {
        //alert(name);
        document.location.href="Employee_BankDetails.aspx"; 
        }
        else if ( name == "tab3")
        {
        //alert(name);
        document.location.href="Employee_DPDetails.aspx"; 
        }
        else if ( name == "tab4")
        {
        //alert(name);
        document.location.href="Employee_Document.aspx"; 
        }
        else if ( name == "tab5")
        {
        //alert(name);
        document.location.href="Employee_FamilyMembers.aspx"; 
        }
        else if ( name == "tab6")
        {
        //alert(name);
        document.location.href="Employee_Registration.aspx"; 
        }
        else if ( name == "tab7")
        {
        //alert(name);
        document.location.href="Employee_GroupMember.aspx"; 
        }
        else if ( name == "tab8")
        {
        //alert(name);
        document.location.href="Employee_Employee.aspx"; 
        }
        else if ( name == "tab9")
        {
        //alert(name);
        document.location.href="Employee_EmployeeCTC.aspx"; 
        }
        else if ( name == "tab10")
        {
        //alert(name);
        document.location.href="Employee_Remarks.aspx"; 
        }
        else if ( name == "tab11")
        {
        //alert(name);
        document.location.href="Employee_Education.aspx";
        }
          else if ( name == "tab12")
        {
        //alert(name);
        document.location.href="Employee_Subscription.aspx";
        }
    }
    function BindCombo()
    {
        
       dpModeClient.PerformCallback();
       dpFrequencyClient.PerformCallback();
       dpNotificationClient.PerformCallback();
       dpPhoneNoClient.PerformCallback();
       dpEmailClient.PerformCallback();
    }

 function BindComboForPhoneEmail(obj)
 {
        dpPhoneNoClient.PerformCallback();
        dpEmailClient.PerformCallback();
        ModeSms=obj;
        VisibleMailPhone(ModeSms)
 }
 function VisibleMailPhone(obj)
 {
    if(obj=='E')
    {    
        document.getElementById("TdPhone").style.display='none';
        document.getElementById("TdPhoneLabel").style.display='none';
        document.getElementById("TdEmail").style.display='inline';
        document.getElementById("TdEmailLabel").style.display='inline';
        
    }
    else if(obj=='S')
    {
        document.getElementById("TdEmail").style.display='none';
        document.getElementById("TdEmailLabel").style.display='none';
        document.getElementById("Tdphone").style.display='inline';
        document.getElementById("TdPhoneLabel").style.display='inline';
    }
    else
    {
        document.getElementById("TdEmail").style.display='inline';
        document.getElementById("TdEmailLabel").style.display='inline';
        document.getElementById("Tdphone").style.display='inline';
        document.getElementById("TdPhoneLabel").style.display='inline';
    }
        
}
function DisableDescription(obj)
{
  
    if(obj!="")
    {
       var a=obj.split('~');
       if(a[0,0]=='UPDATE')
       {      
             
             dpDescription.SetEnabled(false);
             
             if(a[0,1]=='S')
             {
                document.getElementById("TdEmail").style.display='none';
                document.getElementById("TdEmailLabel").style.display='none';
                document.getElementById("Tdphone").style.display='inline';
                document.getElementById("TdPhoneLabel").style.display='inline';
             }
             else if(a[0,1]=='E')
             {
                document.getElementById("TdPhone").style.display='none';
                document.getElementById("TdPhoneLabel").style.display='none';
                document.getElementById("TdEmail").style.display='inline';
                document.getElementById("TdEmailLabel").style.display='inline';
             }
             
             VisibilyOfFreeMessage(a[0,2]);
       }

    } 
    if(obj=='visible')
    {
        VisibilyOfFreeMessage('N');
    }
 }
 function VisibilyOfFreeMessage(obj)
 {
   
    if(obj=='N')
    {
        document.getElementById("tdFreeMessage").style.display='none';
        document.getElementById("tdFreeMessageLabel").style.display='none';
        
    }
    else if(obj=='Y')
    {
       
       
       document.getElementById("tdFreeMessage").style.display='inline';
       document.getElementById("tdFreeMessageLabel").style.display='inline';
    }
    }
 function BindTextBoxWithId(obj)
 {
//    alert(obj);
   AccIDClient.SetValue(obj);

 }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
       <div class="panel-title">
            <h3>Employee Subscription List</h3>
            <div class="crossBtn">
            <a href="Employee_general.aspx" id="goBackCrossBtn"><i class="fa fa-times"></i></a>
         
        </div>
        </div>
        <div class="form_main">
        <table class="TableMain100">
            <tr>
                <td style="text-align: center">
                    <asp:Label ID="lblHeader" runat="server" Font-Bold="True" Font-Size="15px" ForeColor="Navy"
                        Width="819px" Height="18px"></asp:Label></td>
            </tr>
            <tr>
                <td>
                    <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="12" ClientInstanceName="page">
                        <TabPages>
                            <dxe:TabPage Text="General" Name="General">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Text="CorresPondence" Name="CorresPondence">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Bank Details" Text="Bank">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="DP Details" Text="DP">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Documents" Text="Documents">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Family Members" Text="Family">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Registration" Text="Registration">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Group Member" Text="Group">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Employee" Text="Employee">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Employee CTC" Text="CTC">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Remarks" Text="Remarks">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Education" Text="Education">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Subscription" Text="Subscription">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                        <dxe:ASPxGridView runat="server" Width="100%" ID="gridSubscription" DataSourceID="SqlSubscriptions"
                                            KeyFieldName="topicsubscriptions_id" AutoGenerateColumns="False" ClientInstanceName="gridSubscription"
                                            OnRowValidating="gridSubscription_RowValidating" OnRowDeleting="gridSubscription_RowDeleting"
                                            OnCustomJSProperties="gridSubscription_CustomJSProperties1" OnStartRowEditing="gridSubscription_StartRowEditing"
                                            OnRowUpdating="gridSubscription_RowUpdating" OnRowInserting="gridSubscription_RowInserting"
                                            OnInit="gridSubscription_Init" OnInitNewRow="gridSubscription_InitNewRow" OnHtmlEditFormCreated="gridSubscription_HtmlEditFormCreated" OnCancelRowEditing="gridSubscription_CancelRowEditing">
                                            <Templates>
                                                <EditForm>
                                                    <table class="TableMain100">
                                                        <tr>
                                                            <td style="border: solid 1px blue; width: 80%">
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="Label1" runat="server" Text="Topic:"></asp:Label>
                                                                        </td>
                                                                        <td colspan="3">
                                                                            <dxe:ASPxComboBox ID="dpDescription" runat="server" DataSourceID="sqlDescription"
                                                                                EnableIncrementalFiltering="true" ValueField="topics_code" TextField="topics_description"
                                                                                ClientInstanceName="dpDescription" Width="100%">
                                                                                <ClientSideEvents SelectedIndexChanged="function(s,e){BindCombo()}" />
                                                                            </dxe:ASPxComboBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="Label11" runat="server" Text="Access Code:"></asp:Label></td>
                                                                        <td colspan="3" >
                                                                            <dxe:ASPxTextBox ID="txtAccID" runat="server" ReadOnly="true" ClientInstanceName="AccIDClient"
                                                                                Width="100%">
                                                                            </dxe:ASPxTextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="Label4" runat="server" Text="Notification Method:"></asp:Label>
                                                                        </td>
                                                                        <td colspan="3">
                                                                            <dxe:ASPxComboBox ID="dpNotification" runat="server" DataSourceID="sqlNotification"
                                                                                ValueField="col" TextField="Notification" ClientInstanceName="dpNotificationClient"
                                                                                Width="100%" OnCallback="dpNotification_Callback">
                                                                            </dxe:ASPxComboBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="Label2" runat="server" Text="Mode:"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <dxe:ASPxComboBox ID="dpMode" runat="server" DataSourceID="sqlMode" ValueField="col"
                                                                                TextField="Descriptions" OnCallback="dpMode_Callback" ClientInstanceName="dpModeClient">
                                                                                <ClientSideEvents SelectedIndexChanged="function(s,e){BindComboForPhoneEmail(s.GetValue())}"
                                                                                    EndCallback="function(s,e){BindComboForPhoneEmail(s.GetValue())}" />
                                                                            </dxe:ASPxComboBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="Label3" runat="server" Text="Frequency:"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <dxe:ASPxComboBox ID="dpFrequency" runat="server" DataSourceID="sqlFrequency" ValueField="col"
                                                                                TextField="Frequency" ClientInstanceName="dpFrequencyClient" OnCallback="dpFrequency_Callback" OnCustomJSProperties="dpFrequency_CustomJSProperties">
                                                                                 <ClientSideEvents EndCallback="function(s,e){BindTextBoxWithId(s.cpValueCombo1)}" />
                                                                                 
                                                                            </dxe:ASPxComboBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td id="TdPhoneLabel">
                                                                            <asp:Label ID="Label7" runat="server" Text="Phone Number:"></asp:Label>
                                                                        </td>
                                                                        <td id="TdPhone">
                                                                            <dxe:ASPxComboBox ID="dpPhoneNo" runat="server" DataSourceID="sqlPhone" ValueField="col"
                                                                                TextField="Phone" ClientInstanceName="dpPhoneNoClient" OnCallback="dpPhoneNo_Callback">
                                                                            </dxe:ASPxComboBox>
                                                                        </td>
                                                                        <td id="TdEmailLabel">
                                                                            <asp:Label ID="Label8" runat="server" Text="Email Id:"></asp:Label>
                                                                        </td>
                                                                        <td id="TdEmail">
                                                                            <dxe:ASPxComboBox ID="dpEmail" runat="server" DataSourceID="sqlEmail" ValueField="col"
                                                                                TextField="email" ClientInstanceName="dpEmailClient" OnCallback="dpEmail_Callback">
                                                                            </dxe:ASPxComboBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="Label9" runat="server" Text="Chargeable:"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <dxe:ASPxComboBox ID="dpchargeable" runat="server" ValueField="col" TextField="Charge"
                                                                                SelectedIndex="1" ClientInstanceName="dpChargeClient" OnCustomJSProperties="dpchargeable_CustomJSProperties">
                                                                                <Items>
                                                                                    <dxe:ListEditItem Text="Yes" Value="Y" />
                                                                                    <dxe:ListEditItem Text="No" Value="N" />
                                                                                </Items>
                                                                                <ClientSideEvents SelectedIndexChanged="function(s,e){VisibilyOfFreeMessage(s.GetValue());}"
                                                                                    EndCallback="function(s,e){VisibilyOfFreeMessage(s.cpa);}" />
                                                                            </dxe:ASPxComboBox>
                                                                        </td>
                                                                        <td id="tdFreeMessageLabel">
                                                                            <asp:Label ID="Label10" runat="server" Text="Free Messages:"></asp:Label>
                                                                        </td>
                                                                        <td id="tdFreeMessage">
                                                                            <dxe:ASPxTextBox ID="txtFreeMessage" runat="server" ClientInstanceName="txtFreeMessageClient">
                                                                            </dxe:ASPxTextBox>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="Label5" runat="server" Text="Start Date:"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <dxe:ASPxDateEdit ID="ASPxStartDate" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                                                Width="171px" EditFormatString="dd-MM-yyyy">
                                                                                <ButtonStyle Width="13px">
                                                                                </ButtonStyle>
                                                                            </dxe:ASPxDateEdit>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="Label6" runat="server" Text="End Date:"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <dxe:ASPxDateEdit ID="ASPxDateTo" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                                                EditFormatString="dd-MM-yyyy" Width="171px">
                                                                                <ButtonStyle Width="13px">
                                                                                </ButtonStyle>
                                                                            </dxe:ASPxDateEdit>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                <div style="text-align: right; padding: 2px 2px 2px 2px">
                                                                    <dxe:ASPxGridViewTemplateReplacement ID="UpdateButton" ReplacementType="EditFormUpdateButton"
                                                                        runat="server"  Styles-Style-CssClass="btn btn-primary">
                                                                    </dxe:ASPxGridViewTemplateReplacement>
                                                                    <dxe:ASPxGridViewTemplateReplacement ID="CancelButton" ReplacementType="EditFormCancelButton"
                                                                        runat="server" Styles-Style-CssClass="btn btn-danger">
                                                                    </dxe:ASPxGridViewTemplateReplacement>
                                                                </div>
                                                                <asp:SqlDataSource ID="sqlDescription" runat="server" 
                                                                    SelectCommand="select topics_code,topics_description from [Master_Topics] where (topics_applicableentity='M' or topics_applicableentity='X') and topics_subscriptionrequired='Y' and topics_status='E' order by topics_description">
                                                                </asp:SqlDataSource>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </EditForm>
                                            </Templates>
                                            <ClientSideEvents EndCallback="function(s,e){DisableDescription(s.cpValue);}"></ClientSideEvents>
                                            <Styles>
                                                <Header SortingImageSpacing="5px" ImageSpacing="5px">
                                                </Header>
                                                <LoadingPanel ImageSpacing="10px">
                                                </LoadingPanel>
                                            </Styles>
                                            <Settings ShowGroupPanel="True" ShowTitlePanel="True" ShowFooter="True" ShowStatusBar="Visible">
                                            </Settings>
                                            <SettingsBehavior ConfirmDelete="True" AllowFocusedRow="True"></SettingsBehavior>
                                            <SettingsPager AlwaysShowPager="True" PageSize="20" NumericButtonCount="20" ShowSeparators="True">
                                                <FirstPageButton Visible="True">
                                                </FirstPageButton>
                                                <LastPageButton Visible="True">
                                                </LastPageButton>
                                            </SettingsPager>
                                            <Columns>
                                                <dxe:GridViewDataTextColumn Visible="False" ReadOnly="True" VisibleIndex="0" FieldName="topicsubscriptions_id">
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn Visible="False" ReadOnly="True" VisibleIndex="0" FieldName="TopicSubscriptions_TopicCode"
                                                    Caption="Topic Code">
                                                    <EditFormCaptionStyle Wrap="False" HorizontalAlign="Left">
                                                    </EditFormCaptionStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataComboBoxColumn FieldName="topics_description" Caption="Description"
                                                    VisibleIndex="0">
                                                    <PropertiesComboBox ValueType="System.String">
                                                    </PropertiesComboBox>
                                                    <CellStyle CssClass="gridcellleft">
                                                    </CellStyle>
                                                    <EditFormCaptionStyle Wrap="False" HorizontalAlign="Left">
                                                    </EditFormCaptionStyle>
                                                    <EditFormSettings Visible="True"></EditFormSettings>
                                                </dxe:GridViewDataComboBoxColumn>
                                                <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="TopicSubscriptions_NotificationMethod"
                                                    Caption="Notification Method">
                                                    <CellStyle CssClass="gridcellleft">
                                                    </CellStyle>
                                                    <EditFormCaptionStyle Wrap="False" HorizontalAlign="Left">
                                                    </EditFormCaptionStyle>
                                                    <EditFormSettings Visible="True"></EditFormSettings>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="TopicSubscriptions_DeliveryMode"
                                                    Caption="Delivery Mode">
                                                    <CellStyle CssClass="gridcellleft">
                                                    </CellStyle>
                                                    <EditFormCaptionStyle Wrap="False" HorizontalAlign="Left">
                                                    </EditFormCaptionStyle>
                                                    <EditFormSettings Visible="True"></EditFormSettings>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="TopicSubscriptions_DeliveryFrequency"
                                                    Caption="Delivery Frequency">
                                                    <CellStyle CssClass="gridcellleft">
                                                    </CellStyle>
                                                    <EditFormCaptionStyle Wrap="False" HorizontalAlign="Left">
                                                    </EditFormCaptionStyle>
                                                    <EditFormSettings Visible="True"></EditFormSettings>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="TopicSubscriptions_StartDate"
                                                    Caption="Start Date">
                                                    <EditFormCaptionStyle Wrap="False" HorizontalAlign="Left">
                                                    </EditFormCaptionStyle>
                                                    <EditFormSettings Visible="True"></EditFormSettings>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="TopicSubscriptions_EndDate"
                                                    Caption="End Date">
                                                    <EditFormCaptionStyle Wrap="False" HorizontalAlign="Left">
                                                    </EditFormCaptionStyle>
                                                    <EditFormSettings Visible="True"></EditFormSettings>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewCommandColumn VisibleIndex="6" ShowDeleteButton="true"  ShowEditButton="true" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                   <%-- <DeleteButton Visible="True">
                                                    </DeleteButton>--%>
                                                    <HeaderTemplate>
                                                        <a href="javascript:void(0);" onclick="gridSubscription.AddNewRow();"><span >Add New</span> </a>
                                                    </HeaderTemplate>
                                                   
                                                </dxe:GridViewCommandColumn>
                                            </Columns>
                                             <SettingsCommandButton>
                                                                      <DeleteButton  Text="Delete">
                                                                        </DeleteButton>
                                                                       <EditButton  Text="Edit">
                                                                        </EditButton>
                                                                </SettingsCommandButton>
                                        </dxe:ASPxGridView>
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                        </TabPages>
                        <ClientSideEvents ActiveTabChanged="function(s, e) {
	                                            var activeTab   = page.GetActiveTab();
	                                            var Tab0 = page.GetTab(0);
	                                            var Tab1 = page.GetTab(1);
	                                            var Tab2 = page.GetTab(2);
	                                            var Tab3 = page.GetTab(3);
	                                            var Tab4 = page.GetTab(4);
	                                            var Tab5 = page.GetTab(5);
	                                            var Tab6 = page.GetTab(6);
	                                            var Tab7 = page.GetTab(7);
	                                            var Tab8 = page.GetTab(8);
	                                            var Tab9 = page.GetTab(9);
	                                            var Tab10 = page.GetTab(10);
	                                            var Tab11 = page.GetTab(11);
	                                            var Tab12 = page.GetTab(12);
	                                            if(activeTab == Tab0)
	                                            {
	                                                disp_prompt('tab0');
	                                            }
	                                            if(activeTab == Tab1)
	                                            {
	                                                disp_prompt('tab1');
	                                            }
	                                            else if(activeTab == Tab2)
	                                            {
	                                                disp_prompt('tab2');
	                                            }
	                                            else if(activeTab == Tab3)
	                                            {
	                                                disp_prompt('tab3');
	                                            }
	                                            else if(activeTab == Tab4)
	                                            {
	                                                disp_prompt('tab4');
	                                            }
	                                            else if(activeTab == Tab5)
	                                            {
	                                                disp_prompt('tab5');
	                                            }
	                                            else if(activeTab == Tab6)
	                                            {
	                                                disp_prompt('tab6');
	                                            }
	                                            else if(activeTab == Tab7)
	                                            {
	                                                disp_prompt('tab7');
	                                            }
	                                            else if(activeTab == Tab8)
	                                            {
	                                                disp_prompt('tab8');
	                                            }
	                                            else if(activeTab == Tab9)
	                                            {
	                                                disp_prompt('tab9');
	                                            }
	                                            else if(activeTab == Tab10)
	                                            {
	                                                disp_prompt('tab10');
	                                            }
	                                            else if(activeTab == Tab11)
	                                            {
	                                                disp_prompt('tab11');
	                                            }
	                                             else if(activeTab == Tab12)
	                                            {
	                                                disp_prompt('tab12');
	                                            }
	                                            }"></ClientSideEvents>
                        <ContentStyle>
                            <Border BorderColor="#002D96" BorderStyle="Solid" BorderWidth="1px" />
                        </ContentStyle>
                        <LoadingPanelStyle ImageSpacing="6px">
                        </LoadingPanelStyle>
                        <TabStyle Font-Size="12px">
                        </TabStyle>
                    </dxe:ASPxPageControl>
                </td>
            </tr>
        </table>
             </div>
     
        <asp:SqlDataSource ID="SqlSubscriptions" runat="server" 
            SelectCommand="select a.topicsubscriptions_id,a.TopicSubscriptions_TopicCode,b.topics_description,case a.TopicSubscriptions_DeliveryMode when 'E' then 'Email' when 'M' then 'Message' when 'B' then 'Email & SMS' when 'S' then 'SMS' else 'Reminder' end as TopicSubscriptions_DeliveryMode,case a.TopicSubscriptions_NotificationMethod when 'A' then 'Auto' when 'B' then 'Both' else 'On Request' end as TopicSubscriptions_NotificationMethod,case a.TopicSubscriptions_DeliveryFrequency when 'E' then 'Upon Event' when 'D' then 'Daily' when 'F' then 'Fort Nightly' when 'M' then 'Monthly'  when 'Q' then 'Quaterly' when 'W' then 'Weekly' else 'Yearly' end as TopicSubscriptions_DeliveryFrequency,a.TopicSubscriptions_PhoneID,a.TopicSubscriptions_EmailID,convert(varchar(2),a.TopicSubscriptions_StartDate,103)+'/'+convert(varchar(3),TopicSubscriptions_StartDate,107) +'/'+ convert(varchar(4),a.TopicSubscriptions_StartDate,120) as TopicSubscriptions_StartDate,case TopicSubscriptions_EndDate when '1900-01-01 00:00:00.000' then '' else convert(varchar(2),a.TopicSubscriptions_EndDate,103)+'/'+convert(varchar(3),a.TopicSubscriptions_EndDate,107) +'/'+ convert(varchar(4),a.TopicSubscriptions_EndDate,120)  end as TopicSubscriptions_EndDate from Trans_TopicSubscriptions as a inner join Master_Topics as b on b.topics_code=a.TopicSubscriptions_TopicCode where a.TopicSubscriptions_ContactID= @internalID "
            InsertCommand="insert into tblTemp (FirstName) values(1)" 
            UpdateCommand="update tblTemp set FirstName=1">
            <SelectParameters>
                <asp:SessionParameter Name="internalID" SessionField="KeyVal_InternalID" Type="String" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="sqlMode" runat="server" ConflictDetection="CompareAllValues"
            SelectCommand="">
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="sqlFrequency" runat="server" ConflictDetection="CompareAllValues"
            SelectCommand="">
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="sqlNotification" runat="server" ConflictDetection="CompareAllValues"
           SelectCommand="">
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="sqlPhone" runat="server" ConflictDetection="CompareAllValues"
            SelectCommand="">
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="sqlEmail" runat="server" ConflictDetection="CompareAllValues"
           SelectCommand="">
        </asp:SqlDataSource>
   </asp:Content>

