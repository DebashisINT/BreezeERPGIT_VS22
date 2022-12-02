<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.Master.management_master_rootComp_exchange" CodeBehind="rootComp_exchange.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script language="javascript" type="text/javascript">
        FieldName = 'btnCancel';

        function disp_prompt(name) {
            // alert(name);
            if (name == "tab0") {
                //alert(name);
                document.location.href = "rootcompany_general.aspx";
            }
            if (name == "tab1") {
                //alert(name);
                document.location.href = "rootComp_Correspondence.aspx";
            }
            if (name == "tab2") {
                //alert(name);
                //document.location.href="rootComp_exchange.aspx";         
            }
            else if (name == "tab3") {
                //alert(name);
                document.location.href = "rootComp_dpMembership.aspx";
            }
            else if (name == "tab4") {
                //alert(name);
                document.location.href = "rootComp_document.aspx";
            }
        }

        function OnAuthSigInfoClick(keyValue) {

            var url = 'AuthoSignatory.aspx?id=' + keyValue;
            //         OnMoreInfoClick(url,"Modify Athorised Signatory Details",'940px','450px',"Y");
            var HeaderText = 'Authorised Signatory';
            //        var width='940px';
            //        var Height='450px'
            editwin1 = dhtmlwindow_inner.open("Editbox", "iframe", url, HeaderText, "width=840px,height=350px,center=1,resize=1,scrolling=2,top=500", "recal")
            editwin1.onclose = function () {

            }

        }

        function CallList(obj1, obj2, obj3) {
            var obj5 = '';
            ajax_showOptions(obj1, obj2, obj3, obj5);
        }



        //    function SignOff()
        //    {
        //        window.parent.SignOff();
        //    }
        //    function height()
        //     {
        ////     alert("123");

        //        if(document.body.scrollHeight>=600)
        //            window.frameElement.height = document.body.scrollHeight;
        //        else
        //            window.frameElement.height = '700px';
        //        window.frameElement.Width = document.body.scrollWidth;
        //    }
        function DeleteRow(keyValue) {
            doIt = confirm('Confirm delete?');
            if (doIt) {
                grid.PerformCallback('Delete~' + keyValue);
                height();
            }
            else {

            }


        }

        function FilterOff(obj) {

            if (obj == 'EXA0000001') {
                document.getElementById("trExchaneName").style.display = "none";
                document.getElementById("trSegment").style.display = "none";
                document.getElementById("trMemType").style.display = "none";
                document.getElementById("TmCode").style.display = "none";
                document.getElementById("CmCode").style.display = "none";
                document.getElementById("trSEBI").style.display = "none";
                document.getElementById("trExp").style.display = "none";
                document.getElementById("TrFMC").style.display = "none";
                document.getElementById("TrFmcEX").style.display = "none";
                document.getElementById("TrCompOf").style.display = "none";
                document.getElementById("Cmbpid").style.display = "none";
                document.getElementById("Trbroker").style.display = "none";
                document.getElementById("Trexchange").style.display = "none";

            }
            else {
                //              document.getElementById("trExchaneName").style.display = "inline";
                //              document.getElementById("trSegment").style.display="inline";
                //              document.getElementById("trMemType").style.display="inline";
                //              document.getElementById("TmCode").style.display="inline";
                //              document.getElementById("CmCode").style.display="inline";
                //              document.getElementById("trSEBI").style.display="inline";
                //              document.getElementById("trExp").style.display="inline";
                //              document.getElementById("TrFMC").style.display="inline";
                //              document.getElementById("TrFmcEX").style.display="inline";
                //              document.getElementById("TrCompOf").style.display="inline";
                //              document.getElementById("Cmbpid").style.display="inline";
                //              document.getElementById("Trbroker").style.display="inline";
                //              document.getElementById("Trexchange").style.display="inline";

            }

        }


        function ShowHideFilter(obj) {

            grid.PerformCallback(obj);
        }


        function OnAddEditClick(e, obj) {

            // FieldName='ASPxPopupControl1_ASPxCallbackPanel1_drpBranch';
            Filter = 'N';
            RowID = '';
            var data = obj.split('~');
            if (data.length > 1)
                RowID = data[1];
            popup.Show();
            popPanel.PerformCallback(obj);
        }

        function callback() {
            var applicant = document.getElementById("ASPxPopupControl1_ASPxCallbackPanel1_txtApplicant_hidden").value;
            AppBank.PerformCallback(applicant);
            cmbSubSequentBank.PerformCallback(applicant);
        }


        function OnDeleteClick(e, obj) {
            if (confirm('Are You Sure you want to Delete This Transaction?') == true) {
                grid.PerformCallback(obj);
            }
        }
        function btnSave_Click() {

            Filter = 'Y';

            if (RowID == '') {
                var obj = 'SaveNew';
                popPanel.PerformCallback(obj);

            }
            else {
                var obj = 'SaveOld~' + RowID;
                popPanel.PerformCallback(obj);
            }

        }
        function EndCallBack(obj, obj1) {


            //Validate();
            if (obj1 == 'Y') {
                alert("Already Exists!..");
                return false;
            }

            if (obj1 == 'S') {
                FilterOff('EXA0000001')
            }


            if (obj != '') {
                var data = obj.split('~');
                if (data[0] == 'Edit') {

                }
            }
            if (Filter == 'Y') {
                popup.Hide();
                grid.PerformCallback();
            }

        }

        //   function calldispose()
        //    {
        //      var  type=document.getElementById("ASPxPopupControl1_ASPxCallbackPanel1_cmbType").value;
        //            var y=(screen.availHeight-450)/2;
        //        var x=(screen.availWidth-900)/2;
        //        var str = 'frm_TemplateReservedWord.aspx?Type='+type;
        //        window.open(str,"Search_Conformation_Box","height=350,width=700,top="+ y +",left="+ x +",location=no,directories=no,menubar=no,toolbar=no,status=yes,scrollbars=no,resizable=no,dependent=no");       
        //      

        //    }


        function btnCancel_Click() {
            popup.Hide();
        }
        function EndCall() {
            if (grid.cpDelmsg != null)
                alert(grid.cpDelmsg);
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <%--  <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="3600">
            </asp:ScriptManager>--%>
        <table class="TableMain100">
            <tr>
                <td style="text-align: center" class="EHEADER">
                    <asp:Literal ID="LitCompName" runat="server"></asp:Literal>
                    <div class="crossBtn"><a href="root_Companies.aspx"><i class="fa fa-times"></i></a></div>
                </td>
            </tr>
            <tr>
                <td>
                    <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="2" ClientInstanceName="page"
                        Width="100%">
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
                            <dxe:TabPage Name="Exchange Segment" Text="Exchange Segment">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                        <table class="TableMain100">
                                            <tr>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td id="Td1" align="left">
                                                                <a href="javascript:ShowHideFilter('s');"><span style="color: #000099; text-decoration: underline">Show Filter</span></a> || <a href="javascript:ShowHideFilter('All');"><span style="color: #000099; text-decoration: underline">All Records</span></a>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <dxe:ASPxGridView ID="gridContract" ClientInstanceName="grid" Width="100%" KeyFieldName="exch_internalId"
                                                        DataSourceID="gridStatusDataSource" runat="server" AutoGenerateColumns="False"
                                                        OnCustomCallback="gridStatus_CustomCallback">
                                                        <SettingsBehavior AllowFocusedRow="True" ConfirmDelete="True" />
                                                        <Styles>
                                                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                            </Header>
                                                            <LoadingPanel ImageSpacing="10px">
                                                            </LoadingPanel>
                                                            <FocusedRow BackColor="#FEC6AB">
                                                            </FocusedRow>
                                                        </Styles>
                                                        <ClientSideEvents EndCallback="function(s,e){EndCall();}" />
                                                        <Columns>
                                                            <dxe:GridViewDataTextColumn Visible="False" FieldName="exch_internalId" Caption="ID">
                                                                <CellStyle CssClass="gridcellleft">
                                                                </CellStyle>
                                                                <EditFormSettings Visible="False"></EditFormSettings>
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="ExchangeName" Caption="Exchange Name">
                                                                <CellStyle Wrap="True" CssClass="gridcellleft">
                                                                </CellStyle>
                                                                <EditFormSettings Visible="False"></EditFormSettings>
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="SegmentName" Caption="Segment Name">
                                                                <CellStyle Wrap="True" CssClass="gridcellleft">
                                                                </CellStyle>
                                                                <EditFormSettings Visible="False"></EditFormSettings>
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="MemberShipType" Caption="Membership Type">
                                                                <CellStyle Wrap="True" CssClass="gridcellleft">
                                                                </CellStyle>
                                                                <EditFormSettings Visible="False"></EditFormSettings>
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="exch_sebiNo" Caption="SEBI No.">
                                                                <CellStyle Wrap="True" CssClass="gridcellleft">
                                                                </CellStyle>
                                                                <EditFormSettings Visible="False"></EditFormSettings>
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataTextColumn VisibleIndex="4">
                                                                <DataItemTemplate>
                                                                    <a href="javascript:void(0);" onclick="OnAuthSigInfoClick('<%# Container.KeyValue %>')">Auth.Signatory</a>
                                                                </DataItemTemplate>
                                                                <EditFormSettings Visible="False" />
                                                                <CellStyle Wrap="False">
                                                                </CellStyle>
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataTextColumn VisibleIndex="5">
                                                                <DataItemTemplate>
                                                                    <a href="javascript:void(0);" onclick="OnAddEditClick(this,'Edit~'+'<%# Container.KeyValue %>')">
                                                                        <u>More Info</u> </a>&nbsp;&nbsp;&nbsp;<a href="javascript:void(0);" onclick="OnDeleteClick(this,'Delete~'+'<%# Container.KeyValue %>')">
                                                                            <u>Delete</u> </a>
                                                                </DataItemTemplate>
                                                                <EditFormSettings Visible="False" />
                                                                <CellStyle Wrap="False">
                                                                </CellStyle>
                                                                <HeaderTemplate>
                                                                    <a href="javascript:void(0);" onclick="OnAddEditClick(this,'AddNew')"><u>Add New</u>
                                                                    </a>
                                                                </HeaderTemplate>
                                                                <HeaderStyle Wrap="False" />
                                                            </dxe:GridViewDataTextColumn>
                                                        </Columns>
                                                        <StylesEditors>
                                                            <ProgressBar Height="25px">
                                                            </ProgressBar>
                                                        </StylesEditors>
                                                        <SettingsBehavior AllowFocusedRow="True" AllowSort="False" AllowMultiSelection="True" />
                                                        <SettingsPager AlwaysShowPager="True" NumericButtonCount="20" ShowSeparators="True"
                                                            PageSize="15">
                                                            <FirstPageButton Visible="True">
                                                            </FirstPageButton>
                                                            <LastPageButton Visible="True">
                                                            </LastPageButton>
                                                        </SettingsPager>
                                                        <SettingsText Title="Template" />
                                                        <Settings ShowGroupedColumns="True" ShowGroupPanel="True" />
                                                    </dxe:ASPxGridView>
                                                    <asp:SqlDataSource ID="gridStatusDataSource" runat="server"
                                                        SelectCommand="">
                                                        <SelectParameters>
                                                            <asp:SessionParameter Name="userlist" SessionField="userchildHierarchy" Type="string" />
                                                        </SelectParameters>
                                                    </asp:SqlDataSource>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>--%>
                                                    <dxe:ASPxPopupControl ID="ASPxPopupControl1" ClientInstanceName="popup" runat="server"
                                                        AllowDragging="True" PopupHorizontalAlign="OutsideRight" HeaderText="ADD/Modify Exchange"
                                                        EnableHotTrack="False" BackColor="#DDECFE" Width="700px" Height="470px" CloseAction="CloseButton">
                                                        <ContentCollection>
                                                            <dxe:PopupControlContentControl runat="server">
                                                                <dxe:ASPxCallbackPanel ID="ASPxCallbackPanel1" runat="server" Width="700px" Height="450px"
                                                                    ClientInstanceName="popPanel" OnCallback="ASPxCallbackPanel1_Callback" OnCustomJSProperties="ASPxCallbackPanel1_CustomJSProperties">
                                                                    <PanelCollection>
                                                                        <dxe:PanelContent runat="server">
                                                                            <table class="TableMain100">
                                                                                <tr id="trExchaneName">
                                                                                    <td class="gridcellleft">Exchange Name :
                                                                                    </td>
                                                                                    <td class="gridcellleft">
                                                                                        <asp:DropDownList ID="cmbExchName" runat="server" Width="300px" TabIndex="0" onchange="FilterOff(this.value);">
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr id="trSegment">
                                                                                    <td class="gridcellleft">Segment Name :
                                                                                    </td>
                                                                                    <td class="gridcellleft">
                                                                                        <asp:DropDownList ID="cmbSegName" runat="server" Width="300px" TabIndex="1">
                                                                                            <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                                                                            <asp:ListItem Text="Capital Market(CM)" Value="CM"></asp:ListItem>
                                                                                            <asp:ListItem Text="WDM" Value="WDM"></asp:ListItem>
                                                                                            <asp:ListItem Text="Futures & Options(FO)" Value="FO"></asp:ListItem>
                                                                                            <asp:ListItem Text="Currency Derivative(CDX)" Value="CDX"></asp:ListItem>
                                                                                            <asp:ListItem Text="Commodity(SPOT)" Value="SPOT"></asp:ListItem>
                                                                                            <asp:ListItem Text="Commodity Derivative(COMM)" Value="COMM"></asp:ListItem>
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr id="trMemType">
                                                                                    <td class="gridcellleft">Membership Type:
                                                                                    </td>
                                                                                    <td class="gridcellleft">
                                                                                        <asp:DropDownList ID="cmbMemberType" runat="server" Width="300px" TabIndex="2">
                                                                                            <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                                                                            <asp:ListItem Text="Trading Member (TM)" Value="TM"></asp:ListItem>
                                                                                            <asp:ListItem Text="Trading Cum Clearing Member (TCM)" Value="TCM"></asp:ListItem>
                                                                                            <asp:ListItem Text="Institutional Trading Cum Clearing Member(ITCM)" Value="ITCM"></asp:ListItem>
                                                                                            <asp:ListItem Text="Professional Clearing Member  (PCM)" Value="PCM"></asp:ListItem>
                                                                                            <asp:ListItem Text="Trading Member Of CSE (CTM)" Value="CTM"></asp:ListItem>

                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr id="TmCode">
                                                                                    <td class="gridcellleft">TM Code :
                                                                                    </td>
                                                                                    <td class="gridcellleft">
                                                                                        <asp:TextBox ID="txtTMCODE" runat="server" Width="297px" TabIndex="3"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr id="CmCode">
                                                                                    <td class="gridcellleft">CM Code :
                                                                                    </td>
                                                                                    <td class="gridcellleft">
                                                                                        <asp:TextBox ID="txtCMCODE" runat="server" Width="297px" TabIndex="4"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr id="Cmbpid">
                                                                                    <td class="gridcellleft">CMBP ID :
                                                                                    </td>
                                                                                    <td class="gridcellleft">
                                                                                        <asp:TextBox ID="txtcmbpid" runat="server" Width="297px" TabIndex="5"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr id="trSEBI">
                                                                                    <td class="gridcellleft">SEBI Regn No. :
                                                                                    </td>
                                                                                    <td class="gridcellleft">
                                                                                        <asp:TextBox ID="txtSEBINO" runat="server" Width="297px" TabIndex="6"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr id="trExp">
                                                                                    <td class="gridcellleft">SEBI Regn. Expiry Date :
                                                                                    </td>
                                                                                    <td class="gridcellleft">
                                                                                        <dxe:ASPxDateEdit ID="dtSEBIEXP" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                                                            TabIndex="7" Width="300px">
                                                                                            <ButtonStyle Width="13px">
                                                                                            </ButtonStyle>
                                                                                        </dxe:ASPxDateEdit>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr id="TrFMC">
                                                                                    <td class="gridcellleft">FMC Regn. No.
                                                                                    </td>
                                                                                    <td class="gridcellleft">
                                                                                        <asp:TextBox ID="txtFMCNO" runat="server" Width="297px" TabIndex="8"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr id="TrFmcEX">
                                                                                    <td class="gridcellleft">FMC Regn. Expiry Date :
                                                                                    </td>
                                                                                    <td class="gridcellleft">
                                                                                        <dxe:ASPxDateEdit ID="dtFMCEXP" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                                                            TabIndex="9" Width="300px">
                                                                                            <ButtonStyle Width="13px">
                                                                                            </ButtonStyle>
                                                                                        </dxe:ASPxDateEdit>
                                                                                    </td>
                                                                                </tr>

                                                                                <tr id="TrCompOf">
                                                                                    <td class="gridcellleft">Compliance Officer
                                                                                    </td>
                                                                                    <td class="gridcellleft">
                                                                                        <asp:TextBox ID="txtCompliance" runat="server" Width="297px" TabIndex="10"></asp:TextBox>
                                                                                        <asp:HiddenField ID="txtCompliance_hidden" runat="server" />
                                                                                    </td>
                                                                                </tr>
                                                                                <tr id="Trbroker">
                                                                                    <td class="gridcellleft">Inv Grievance Email id Of Broker
                                                                                    </td>
                                                                                    <td class="gridcellleft" style="color: Red;">
                                                                                        <asp:TextBox ID="txtbroker" runat="server" Width="297px" TabIndex="10"></asp:TextBox>
                                                                                        It will be used in Contractnote (ECN)
                                                                                    </td>
                                                                                </tr>
                                                                                <tr id="Trexchange">
                                                                                    <td class="gridcellleft">Inv Grievance Email id Of Exchange
                                                                                    </td>
                                                                                    <td class="gridcellleft" style="color: Red;">
                                                                                        <asp:TextBox ID="txtexchange" runat="server" Width="297px" TabIndex="10"></asp:TextBox>
                                                                                        It will be used in Contractnote (ECN)
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="gridcellleft"></td>
                                                                                    <td class="gridcellleft">
                                                                                        <input id="Button1" type="button" value="Save" class="btnUpdate" onclick="btnSave_Click()"
                                                                                            style="width: 60px" tabindex="11" />
                                                                                        <input id="Button2" type="button" value="Cancel" class="btnUpdate" onclick="btnCancel_Click()"
                                                                                            style="width: 60px" tabindex="12" />
                                                                                    </td>
                                                                                </tr>

                                                                            </table>
                                                                        </dxe:PanelContent>
                                                                    </PanelCollection>
                                                                    <ClientSideEvents EndCallback="function(s, e) { EndCallBack(s.cpLast,s.cpfast); }" />
                                                                </dxe:ASPxCallbackPanel>
                                                            </dxe:PopupControlContentControl>
                                                        </ContentCollection>
                                                        <HeaderStyle HorizontalAlign="Left">
                                                            <Paddings PaddingRight="6px" />
                                                        </HeaderStyle>
                                                        <SizeGripImage Height="16px" Width="16px" />
                                                        <CloseButtonImage Height="12px" Width="13px" />
                                                        <ClientSideEvents CloseButtonClick="function(s, e) {popup.Hide();}" />
                                                    </dxe:ASPxPopupControl>
                                                    <%--</ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click"></asp:AsyncPostBackTrigger>
                                                        </Triggers>
                                                    </asp:UpdatePanel>--%>
                                                </td>
                                            </tr>
                                        </table>
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="DP Memberships" Text="DP Memberships">
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
                        </TabPages>
                        <ClientSideEvents ActiveTabChanged="function(s, e) {
	                                            var activeTab   = page.GetActiveTab();
	                                            var Tab0 = page.GetTab(0);
	                                            var Tab1 = page.GetTab(1);
	                                            var Tab2 = page.GetTab(2);
	                                            var Tab3 = page.GetTab(3);
	                                            var Tab4 = page.GetTab(4);
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
            <tr>
                <td>
                    <asp:SqlDataSource ID="Exchange" runat="server"
                        SelectCommand="SELECT [exh_cntId], [exh_name] FROM [tbl_master_exchange]"></asp:SqlDataSource>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
