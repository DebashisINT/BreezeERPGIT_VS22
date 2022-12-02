<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.Master.management_master_Member_usergroup" Codebehind="Member_usergroup.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     
   <script type="text/javascript" src="/assests/js/loaddata1.js">
   
   </script>
    <link rel="stylesheet" href="../../windowfiles/dhtmlwindow.css" type="text/css" />
    <script type="text/javascript" src="../../windowfiles/dhtmlwindow.js"></script>

    <link rel="stylesheet" href="../../modalfiles/modal.css" type="text/css" />
    
    <script type="text/javascript" src="../../modalfiles/modal.js"></script>
    <script language="javascript" type="text/javascript">
    function SignOff()
    {
        window.parent.SignOff();
    }
    function height()
    {
        if(document.body.scrollHeight>=500)
            window.frameElement.height = document.body.scrollHeight;
        else
            window.frameElement.height = '500px';
        window.frameElement.Width = document.body.scrollWidth;
    }

    function AddUserDetails() 
    {
       // document.location.href="RootUserDetails.aspx?id=Add"
       var middle=document.getElementById('HiddenField1').value;
       
        var url='Addmember_usergroup.aspx?id='+middle;
        //OnMoreInfoClick1(url,"Add Member",540,350,'Y')
        frmOpenNewWindow1(url,540,350);
       // alert (middle);
        //OnMoreInfoClick(url,"Add Member",'440px','300px',"Y");
       // editwin=dhtmlmodal.open("Editbox", "iframe", url,"Add Member","width=540px,height=350px,center=1,resize=1,top=500", "recal");
       
    }
     function frmOpenNewWindow1(location,v_height,v_weight)
    {
        var y=(screen.availHeight-v_height)/2;
        var x=(screen.availWidth-v_weight)/2;
        editwin=dhtmlmodal.open("Editbox", "iframe", location, "Memberdetail", "width=800px,height=400px,center=1,resize=1,top=0", "recal"); 
        
         editwin.onclose=function()
             {
                 grid.PerformCallback();
             }
    }
    
    function OnMoreInfoClick1(url,HeaderText,Width,Height,anyKey) //AnyKey will help to call back event to child page, if u have to fire more that one function 
        {
             editwin=dhtmlmodal.open("Editbox", "iframe", url,HeaderText , "width="+Width+",height="+Height+",center=1,resize=1,scrolling=2,top=500", "recal")
             editwin.onclose=function()
             {
                 if(anyKey=='Y')
                 {
                    document.getElementById('IFRAME_ForAllPages').contentWindow.callback();
                 }
             }
        }
    
    
      
    
    
    FieldName='Headermain1_cmbSegment';
    function ShowHideFilter(obj)
    {
       grid.PerformCallback(obj);
    }
    function callback()
    {
       grid.PerformCallback('All');
      // grid.PerformCallback();
    } 
    function EndCall(obj)
    {
        height();
    }
   </script>
 <script type="text/javascript" src="/assests/js/init.js"></script>

    <script type="text/javascript" src="/assests/js/ajaxList.js"></script>

    <!--___________________________________________________________________________-->
    <style type="text/css">
	
	/* Big box with list of options */
	#ajax_listOfOptions{
		position:absolute;	    /* Never change this one */
		width:50px;	        /* Width of box */
		height:auto;	        /* Height of box */
		overflow:auto;	        /* Scrolling features */
		border:1px solid Blue;	/* Blue border */
		background-color:#FFF;	/* White background color */
		text-align:left;
		font-size:0.9em;
		z-index:100;
	}
	#ajax_listOfOptions div{	/* General rule for both .optionDiv and .optionDivSelected */
		margin:1px;		
		padding:1px;
		cursor:pointer;
		font-size:0.9em;
	}
	#ajax_listOfOptions .optionDiv{	/* Div for each item in list */
		
	}
	#ajax_listOfOptions .optionDivSelected{ /* Selected item in the list */
		background-color:#DDECFE;
		color:Blue;
		z-index:900;
	}
	#ajax_listOfOptions_iframe{
		background-color:#F00;
		position:absolute;
		z-index:10;
	}
	
	form{
		display:inline;
	}
	
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <table class="TableMain100">
            <tr>
                <td class="EHEADER" style="text-align: center;">
                    <strong><span style="color: #000099">User List</span></strong>
                </td>
            </tr>
            <tr>
                <td>
                    <table width="100%">
                        <tr>
                        <asp:HiddenField ID="HiddenField1" runat="server" />
                            <%--<td style="text-align: left; vertical-align: top">
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
                            </td>--%>
                            <td>
                            </td>
                            <%--<td class="gridcellright">
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
                                </dxe:ASPxComboBox>
                            </td>--%>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <dxe:ASPxGridView ID="userGrid" ClientInstanceName="grid" runat="server" AutoGenerateColumns="False"
                         KeyFieldName="USER_ID" Width="100%" OnCustomCallback="userGrid_CustomCallback" OnCustomJSProperties="userGrid_CustomJSProperties"
                          OnRowCommand="userGrid_RowCommand">
                        <Columns>
                            <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="0" FieldName="USER_ID"
                                Visible="False">
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="0" FieldName="USER_NAME"
                                Caption="Name" Width="40%">
                                <PropertiesTextEdit>
                                    <ValidationSettings ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom" SetFocusOnError="True">
                                        <RequiredField ErrorText="Please Enter user Name" IsRequired="True" />
                                    </ValidationSettings>
                                </PropertiesTextEdit>
                                <EditFormSettings Caption="User Name:" Visible="True" />
                            </dxe:GridViewDataTextColumn>
                            <%--<dxe:GridViewDataTextColumn VisibleIndex="1" Caption="Designation" FieldName="designation">
                            </dxe:GridViewDataTextColumn>--%>
                            <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="2" FieldName="user_loginId"
                                Caption="Login ID">
                                <PropertiesTextEdit>
                                    <ValidationSettings ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom" SetFocusOnError="True">
                                        <RequiredField ErrorText="Please Enter Login Id" IsRequired="True" />
                                    </ValidationSettings>
                                </PropertiesTextEdit>
                                <EditFormSettings Caption="Login Id:" Visible="True" />
                            </dxe:GridViewDataTextColumn>
                            
                            
                                <%-- <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="3" FieldName="Status"
                                Caption="Status">
                                <PropertiesTextEdit>
                                    <ValidationSettings ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom" SetFocusOnError="True">
                                        <RequiredField ErrorText="Please Enter Login Id" IsRequired="True" />
                                    </ValidationSettings>
                                </PropertiesTextEdit>
                                <EditFormSettings Caption="Login Id:" Visible="True" />
                            </dxe:GridViewDataTextColumn>--%>
                            
                          <%-- <dxe:GridViewDataTextColumn Caption="Company Add/Edit" VisibleIndex="4" Width="5%">
                                <DataItemTemplate>
                                    <a href="javascript:void(0);" onclick="AddCompany('<%# Container.KeyValue %>')">Add Company</a>
                                </DataItemTemplate>
                                <EditFormSettings Visible="False" />
                                <HeaderTemplate>
                                 <span style="color: #000099;
                                        ">Company Add/Edit</span>
                                </HeaderTemplate>
                            </dxe:GridViewDataTextColumn>--%>
                            
                            
                            
                            <%--<dxe:GridViewDataTextColumn Caption="Edit" VisibleIndex="4" Width="5%">
                                <DataItemTemplate>
                                    <a href="javascript:void(0);" onclick="EditUserDetails('<%# Container.KeyValue %>')">Edit</a>
                                </DataItemTemplate>
                                <EditFormSettings Visible="False" />
                                <HeaderTemplate>
                                    <%if (Session["PageAccess"].ToString().Trim() == "All" || Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "DelAdd")
                                       { %>
                                    <a href="javascript:void(0);" onclick="AddUserDetails()"><span style="color: #000099;
                                        text-decoration: underline">Add New</span> </a>
                                    <%} %>
                                </HeaderTemplate>
                            </dxe:GridViewDataTextColumn>--%>
                            <%--<dxe:GridViewCommandColumn VisibleIndex="3">
                                                        <DeleteButton Visible="True">
                                                        </DeleteButton>
                                                        
                                                         </dxe:GridViewCommandColumn>--%>
                                                         <dxe:GridViewDataTextColumn VisibleIndex="4">
                                                        <HeaderTemplate>
                                    
                                    <a href="javascript:void(0);" onclick="AddUserDetails()"><span style="color: #000099;
                                        text-decoration: underline">Add New</span> </a>
                                   
                                </HeaderTemplate>
                                                         <DataItemTemplate>
                                                         <asp:LinkButton ID="btn_show" runat="server" Text="Remove" CommandArgument='<%# Container.KeyValue %>' CommandName="res"> </asp:LinkButton>
                                                         </DataItemTemplate>
                                                         </dxe:GridViewDataTextColumn>
                                                         
                        </Columns>
                        <Styles>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                            </Header>
                            <Cell CssClass="gridcellleft">
                            </Cell>
                        </Styles>
                        <Settings ShowStatusBar="Visible" />
                        <SettingsPager NumericButtonCount="20" PageSize="10" AlwaysShowPager="True" ShowSeparators="True">
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </SettingsPager>
                        <SettingsBehavior ConfirmDelete="True" />
                        <ClientSideEvents EndCallback="function(s, e) {
	EndCall(s.cpHeight);
}" />
                    </dxe:ASPxGridView>
                </td>
            </tr>
        </table>
       <%-- <asp:SqlDataSource ID="RootUserDataSource" runat="server" ConflictDetection="CompareAllValues"
            DeleteCommand=""
            OldValuesParameterFormatString="original_{0}" SelectCommand="">
            <DeleteParameters>
                <asp:Parameter Name="original_user_id" Type="Decimal" />
            </DeleteParameters>
            <SelectParameters>
                <asp:SessionParameter Name="branch" SessionField="userbranchHierarchy" Type="string" />
            </SelectParameters>
        </asp:SqlDataSource>--%>
        <dxe:ASPxGridViewExporter ID="exporter" runat="server">
        </dxe:ASPxGridViewExporter>
</asp:Content>
