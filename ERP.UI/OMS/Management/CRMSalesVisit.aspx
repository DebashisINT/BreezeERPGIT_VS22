<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.management_CRMSalesVisit" CodeBehind="CRMSalesVisit.aspx.cs" %>

<%--<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe000001" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register assembly="DevExpress.Web.v15.1" namespace="DevExpress.Web" tagprefix="dx" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script language="javascript" type="text/javascript">
        function SignOff() {
            window.parent.SignOff()
        }
        function height() {
            window.frameElement.height = document.body.scrollHeight;
            window.frameElement.widht = document.body.scrollWidht;
        }
        function OnMoreInfoClick(KeyValue, Id, SalesVisitId) {
            document.location.href = "CRMSalesVisit.aspx?id=" + KeyValue + "&id1=" + Id + "&id2=" + SalesVisitId;

        }

        function OnMoreInfoClick1(Id) {
            frmOpenNewWindow1("ShowHistory_Phonecall.aspx?id1=" + Id, 300, 800);
        }
        function frmOpenNewWindow1(location, v_height, v_weight) {
            var x = (screen.availHeight - v_height) / 2;
            var y = (screen.availWidth - v_weight) / 2
            window.open(location, "Search_Conformation_Box", "height=" + v_height + ",width=" + v_weight + ",top=" + x + ",left=" + y + ",location=no,directories=no,menubar=no,toolbar=no,status=yes,scrollbars=no,resizable=no,dependent=no");
        }
        var chkobj;
        var objchk = null;
        function showPanel(obj, msg) {
            alert(obj + msg)
        }
        function chkclicked(obj, msg12) {
            var txt = document.getElementById("hiddenleadid")
            if (objchk == null) {
                objchk = obj;
                objchk.checked = true;
            }
            else {
                objchk.checked = false;
                objchk = obj;
                objchk.checked = true;
            }
            txt.value = msg12;
        }
        function adddatetime(obj1, adddays, addtime) {
            var t
            var t1
            var old_date
            var lst_id
            var new_date
            var txt_date
            txt_date = adddays + " " + addtime;
        }

        function Changedata123(txtdate, txttime, nowdate, nowtime, cdate, ctime) {
            var a = document.getElementById("ctl00_ContentPlaceHolder3_txtStartDate")
            a.value = nowdate + " " + nowtime
            var b = document.getElementById("ctl00_ContentPlaceHolder3_txtEndTime")
            b.value = cdate + " " + ctime
            var drp = document.getElementById("ctl00_ContentPlaceHolder3_TxtOut")
            var drpVal = drp.value;
            var drpVal1 = drpVal.split("!");
            if ((drpVal1[1] == 1) || (drpVal1[1] == 2) || (drpVal1[1] == 3) || (drpVal1[1] == 4) || (drpVal1[1] == 5)) {
                document.getElementById("ctl00_ContentPlaceHolder3_lblNextVisitDate").style.display = "block"
                document.getElementById("ctl00_ContentPlaceHolder3_lblNextVisitPlace").style.display = "block"
                //document.getElementById("ctl00_ContentPlaceHolder3_drpNextVisitPlace").style.display = "block"
            }
            else {
                if ((drpVal1[1] == 9) || (drpVal1[1] == 10)) {
                    document.getElementById("ctl00_ContentPlaceHolder3_txtStartDate").disabled = false
                    //                document.getElementById("ctl00_ContentPlaceHolder3_txtNextVisitDate1").disabled = false
                    //                document.getElementById("ctl00_ContentPlaceHolder3_drpNextVisitPlace").disabled = true
                    //                document.getElementById("ctl00_ContentPlaceHolder3_drpVisitPlace").disabled = true
                }
                else {
                    if ((drpVal1[1] == 11) || (drpVal1[1] == 12)) {
                        document.getElementById("ctl00_ContentPlaceHolder3_txtStartDate").disabled = false
                        //                    document.getElementById("ctl00_ContentPlaceHolder3_txtNextVisitDate1").disabled = false
                        //                    document.getElementById("ctl00_ContentPlaceHolder3_drpNextVisitPlace").disabled = false
                        //                    document.getElementById("ctl00_ContentPlaceHolder3_drpVisitPlace").disabled = false
                    }
                    else {
                        if (drpVal1[1] == 13) {
                            document.getElementById("ctl00_ContentPlaceHolder3_txtStartDate").disabled = false
                            //document.getElementById("ctl00_ContentPlaceHolder3_txtNextVisitDate1").disabled = true
                            //document.getElementById("ctl00_ContentPlaceHolder3_drpNextVisitPlace").disabled = true
                            //document.getElementById("ctl00_ContentPlaceHolder3_drpVisitPlace").disabled = true 

                        }
                        else {
                            document.getElementById("ctl00_ContentPlaceHolder3_lblNextVisitDate").style.display = "none"
                            document.getElementById("ctl00_ContentPlaceHolder3_lblNextVisitPlace").style.display = "none"
                            //document.getElementById("ctl00_ContentPlaceHolder3_drpNextVisitPlace").style.display = "none"
                        }
                    }
                }
            }
        }
        function FillValues(id) {
            //        var sel = document.getElementById('txtProductCount');
            //        sel.value=id;
            noofproduct = id;
        }
        function calldispose(Obj, val) {
            //        if(val=="salesvisit")
            //        {
            //            var str = "SalesVisitOutCome.aspx?call="+val+"&obj="+Obj
            //        }
            //        else
            //        {
            //            var str = "SalesOutCome1.aspx?call="+val+"&obj="+Obj
            //        }
            var str = "frmSalesVisitOutCome.aspx?call=" + val + "&obj=" + Obj
            frmOpenNewWindow1(str, 400, 900)
        }
        function funChangeNext(obj) {
            var o = document.getElementById("ctl00_ContentPlaceHolder3_lblNextVisitDate")
            if (obj.id == 'ctl00_ContentPlaceHolder3_rdrCall') {
                o.innerText = "Next Call Date"
                document.getElementById("tdnextvisit").style.display = 'none';
                document.getElementById("tdnextvisit1").style.display = 'none';
            }
            else {
                o.innerText = "Next Visit Date"
                document.getElementById("tdnextvisit").style.display = 'inline';
                document.getElementById("tdnextvisit1").style.display = 'inline';
            }
        }
        function chkOnSaveClick123() {
            var drp = document.getElementById("ctl00_ContentPlaceHolder3_TxtOut");
            var st = drp.value.split("!");
            if ((st[1] == 4) || (st[1] == 12)) {
                //var sel = document.getElementById('txtProductCount');
                if (noofproduct == 0) {
                    alert("For confirm sale choose atleast one product");
                    return false;
                }
            }
            if ((st[1] == 1) || (st[1] == 2) || (st[1] == 3) || (st[1] == 4) || (st[1] == 8)) {
                var sel = document.getElementById("ctl00_ContentPlaceHolder3_drpVisitPlace");
                if (sel.value == 0) //|| (sel.options.length == 0)
                {
                    alert("Please select the visit address");
                    return false;
                }
            }
            if ((st[1] == 1) || (st[1] == 2) || (st[1] == 3) || (st[1] == 4) || (st[1] == 8) || (st[1] == 11) || (st[1] == 12)) {
                var sel = document.getElementById("ctl00_ContentPlaceHolder3_drpNextVisitPlace");
                var obrdr = document.getElementById("ctl00_ContentPlaceHolder3_rdrVisit");
                if (obrdr.checked == true) {
                    if (sel.value == 0) //|| (sel.options.length == 0)
                    {
                        alert("Please select the next visit address");
                        return false;
                    }
                }
            }
            return true;
        }
        function All_CheckedChanged() {
            grid.PerformCallback()
        }
        function Specific_CheckedChanged() {
            grid.PerformCallback()
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table class="TableMain">
            <tr>
                <td style="text-align: left; width: 10px">
                    <table style="width: 8px">
                        <tr id="btntbl" runat="server">
                            <td>
                                <asp:Button ID="BtnPending" runat="server" Text="Pending/New Visit" CssClass="btnUpdate btn btn-warning"
                                    OnClick="BtnPending_Click" />
                            </td>
                            <td>
                                <asp:Button ID="BtnOpen" runat="server" Text="Open" CssClass="btnUpdate btn btn-success" OnClick="BtnOpen_Click"
                                     />
                            </td>
                            <td>
                                <asp:Button ID="BtnClosed" runat="server" Text="Closed Non Usable" CssClass="btnUpdate btn btn-default"
                                    OnClick="BtnClosed_Click"  />
                            </td>
                            <td>
                                <asp:Button ID="BtnConfirm" runat="server" Text="Confirm Sale" CssClass="btnUpdate btn btn-primary"
                                    OnClick="BtnConfirm_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table class="TableMain100">
                        <tr style="height: auto" id="activityRow" runat="server">
                            <td class="Ecoheadtxt" style="width: 7%">
                                <asp:Label ID="LBLActivity" runat="server" Text="Activities Of :"></asp:Label></td>
                            <td class="gridcellleft" style="text-align: left;">
                                <asp:DropDownList ID="DDLActivity" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DDLActivity_SelectedIndexChanged"
                                    Width="165px" CssClass="EcoheadCon">
                                </asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td colspan="2" style="text-align: left">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="Lrd" runat="server" GroupName="a" Checked="True" />
                                        </td>
                                        <td>
                                            <asp:Label ID="Label4" runat="server" Text="From Lead Data" Font-Size="X-Small" ForeColor="Blue"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="Erd" runat="server" GroupName="a" />
                                        </td>
                                        <td>
                                            <asp:Label ID="Label5" runat="server" Text="From Existing Customer Data" Font-Size="X-Small"
                                                ForeColor="Blue"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="grdtbl" runat="server" style="height: auto">
                            <td colspan="2">
                                <dxe:ASPxGridView ID="AspxActivity" runat="server" ClientInstanceName="grid" KeyFieldName="ActId"
                                    AutoGenerateColumns="False" Width="100%" OnCustomCallback="AspxActivity_CustomCallback">
                                    <Columns>
                                        <dxe:GridViewDataTextColumn FieldName="Id" VisibleIndex="0">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn FieldName="Name" ReadOnly="True" VisibleIndex="1">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn FieldName="ActId" Visible="False" VisibleIndex="2">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn FieldName="SalesVisitId" Visible="False" VisibleIndex="2">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn FieldName="AssignBy" Visible="False" VisibleIndex="2">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn FieldName="NextVisitDate" VisibleIndex="2">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn FieldName="Address1" ReadOnly="True" VisibleIndex="3">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn FieldName="LastOutcome" VisibleIndex="4">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Details" VisibleIndex="5">
                                            <DataItemTemplate>
                                                <a href="#" onclick="OnMoreInfoClick('<%# Container.KeyValue %>','<%#Eval("Id") %>','<%#Eval("SalesVisitId") %>')">Show</a>
                                            </DataItemTemplate>
                                            <EditFormSettings Visible="False" />
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="History" VisibleIndex="6">
                                            <DataItemTemplate>
                                                <a href="#" onclick="OnMoreInfoClick1('<%#Eval("Id") %>')">History</a>
                                            </DataItemTemplate>
                                            <EditFormSettings Visible="False" />
                                        </dxe:GridViewDataTextColumn>
                                    </Columns>
                                    <Styles>
                                        <LoadingPanel ImageSpacing="10px">
                                        </LoadingPanel>
                                        <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                        </Header>
                                    </Styles>
                                </dxe:ASPxGridView>
                            </td>
                        </tr>
                        <tr id="showdetailstbl" runat="server">
                            <td colspan="2">
                                <asp:Panel ID="PnlShowDetails" runat="server" Width="100%" Visible="False">
                                    <table border="2" class="TableMain100">
                                        <tr>
                                            <td style="width: 100%">
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td class="Ecoheadtxt" style="width: 9%; height: 11px">
                                                            <span><strong>Alloated By :</strong> </span>
                                                        </td>
                                                        <td class="gridcellleft" style="width: 14%; height: 11px">
                                                            <asp:Label ID="txtAlloatedBy" runat="server" CssClass="EcoheadCon"></asp:Label></td>
                                                        <td class="Ecoheadtxt" style="width: 8%; height: 11px">
                                                            <strong><span>Allotted On : </span></strong>
                                                        </td>
                                                        <td class="gridcellleft" style="width: 24%; height: 11px">
                                                            <asp:Label ID="txtDateOfAllottment" runat="server" CssClass="EcoheadCon" Width="92px"></asp:Label>
                                                            <asp:Label ID="txtTotalNumberofCalls" runat="server" Visible="False" CssClass="EcoheadCon"
                                                                Width="130px"></asp:Label></td>
                                                        <td class="Ecoheadtxt" style="width: 7%; height: 11px">
                                                            <strong><span>Priority :</span></strong></td>
                                                        <td class="gridcellleft" style="width: 15%; height: 11px">
                                                            <asp:Label ID="txtPriority" runat="server" CssClass="EcoheadCon"></asp:Label></td>
                                                        <td class="Ecoheadtxt" style="width: 5%; height: 11px">
                                                            <strong><span>Start By :</span></strong></td>
                                                        <td class="gridcellleft" style="width: 17%; height: 11px">
                                                            <asp:Label ID="txtSeheduleStart" runat="server" CssClass="EcoheadCon"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td class="Ecoheadtxt" style="width: 9%; height: 11px">
                                                            <strong><span>End By :</span></strong></td>
                                                        <td class="gridcellleft" style="width: 14%; height: 11px">
                                                            <asp:Label ID="txtSeheduleEnd" runat="server" CssClass="EcoheadCon"></asp:Label></td>
                                                        <td class="Ecoheadtxt" style="width: 8%; height: 11px">
                                                            <span><strong>Started On:</strong> </span>
                                                        </td>
                                                        <td class="gridcellleft" style="width: 24%; height: 11px">
                                                            <asp:Label ID="txtAcutalStart" runat="server" CssClass="EcoheadCon"></asp:Label></td>
                                                        <td class="Ecoheadtxt" style="width: 7%; height: 11px">
                                                            <span><strong>Instruction :</strong> </span>
                                                        </td>
                                                        <td class="gridcellleft" style="width: 17%; height: 11px">
                                                            <asp:Label ID="lblShortNote" runat="server" CssClass="EcoheadCon"></asp:Label></td>
                                                        <td style="width: 5%; height: 11px"></td>
                                                        <td style="width: 25%; height: 11px"></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100%">
                                                <table class="TableMain100">
                                                    <tr>
                                                        <td style="text-align: left;">
                                                            <asp:Button ID="btnUpdateVisit" Text="Update Visit" CssClass="btnUpdate" runat="server"
                                                                OnClick="btnUpdateVisit_Click" Height="21px" />
                                                            <asp:Button ID="btnPhoneFollowUP" Text="Phone Follow Up" CssClass="btnUpdate" runat="server"
                                                                Width="110px" OnClick="btnPhoneFollowUP_Click" Height="21px" />
                                                            <input type="button" value="Modify Phone/Address" id="btn_UpdateAddress" class="btnUpdate"
                                                                style="width: 139px; height: 21px;" onclick="frmOpenNewWindow1('../management/Contact_Correspondence.aspx?type=modify&requesttype=lead&formtype=lead', 550, 800)" />
                                                            <input type="button" value="Modify Contact Details" id="btn_UpdateDetails" class="btnUpdate"
                                                                style="width: 139px; height: 21px;" onclick="frmOpenNewWindow1('../management/Lead_general.aspx?type=modify&requesttype=lead&formtype=lead', 400, 900)" />
                                                            <input type="button" id="Button1" name="btnHistory" value="History" class="btnUpdate"
                                                                onclick="frmOpenNewWindow1('../management/ShowHistory_Phonecall.aspx', 300, 800)"
                                                                style="height: 21px" />
                                                        </td>
                                                        <td style="width: 457px; height: 11px">
                                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                                <tr>
                                                                    <td class="Ecoheadtxt" style="width: 5%; text-align: left; height: 11px">
                                                                        <asp:Label ID="Label1" runat="server" Text="Last Call :" Font-Bold="True" Width="51px"></asp:Label>
                                                                    </td>
                                                                    <td class="gridcellleft" style="width: 14%; height: 11px">
                                                                        <asp:Label ID="lblLastVisit" runat="server" CssClass="EcoheadCon"></asp:Label>
                                                                    </td>
                                                                    <td class="Ecoheadtxt" style="width: 6%; height: 11px">
                                                                        <asp:Label ID="Label3" runat="server" Text="Next Call :" Font-Bold="True"></asp:Label>
                                                                    </td>
                                                                    <td class="gridcellleft" style="width: 16%; height: 11px">
                                                                        <asp:Label ID="lblNextVisit" runat="server" CssClass="EcoheadCon"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="Ecoheadtxt" style="width: 5%; text-align: left; height: 11px">
                                                                        <asp:Label ID="Label2" runat="server" Text="OutCome :" Font-Bold="True"></asp:Label>
                                                                    </td>
                                                                    <td class="gridcellleft" colspan="3" style="width: 16%; height: 11px">
                                                                        <asp:Label ID="lblLastOutcome" runat="server" CssClass="EcoheadCon"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                            </table>
                                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; vertical-align: top;">
                                                                <tr>
                                                                    <td valign="top" class="Ecoheadtxt" style="width: 100%">
                                                                        <iframe id="iFrmInformation" style="vertical-align: top" src="../management/CallUserInformation.aspx?id=SalesVisit"
                                                                            width="100%" height="218" frameborder="0" scrolling="no"></iframe>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr id="pnldatatbl" runat="server">
                            <td colspan="2">
                                <asp:Panel ID="pnlData" Visible="False" Enabled="false" runat="server" Width="100%">
                                    <table width="100%" cellpadding="2" cellspacing="3">
                                        <tr>
                                            <td class="Ecoheadtxt" style="width: 14%; height: 12px">
                                                <span>OutCome :</span></td>
                                            <td colspan="5" class="Ecoheadtxt" style="text-align: left; height: 12px">
                                                <input type="hidden" id="txtOutCome_id" name="txtOutCome_id" />
                                                <asp:TextBox ID="txtOutCome" runat="server" Width="50%"></asp:TextBox>
                                                <asp:TextBox ID="TxtOut" runat="server" BackColor="Transparent" BorderColor="Transparent"
                                                    BorderStyle="None" ForeColor="#DDECFE"></asp:TextBox>&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="Ecoheadtxt" style="width: 14%; height: 12px">
                                                <asp:Label ID="lblVisitDateTime" runat="server" Text="Visit DateTime" Font-Bold="False"></asp:Label>
                                            </td>
                                            <td class="Ecoheadtxt" style="width: 23%; text-align: left; height: 12px">
                                                <dxe:ASPxDateEdit ID="ASPxDateEdit" runat="server" EditFormat="Custom" EditFormatString="dd MMMM yyyy hh:mm tt"
                                                    UseMaskBehavior="True">
                                                    <ButtonStyle Width="13px">
                                                    </ButtonStyle>
                                                </dxe:ASPxDateEdit>
                                                <%--<asp:TextBox ID="ASPxDateEdit" runat="server"></asp:TextBox>
                                            <asp:Image ID="Image2" runat="server" ImageUrl="~/images/calendar.jpg" />--%>
                                            </td>
                                            <td class="Ecoheadtxt" id="tdVisitPlace" runat="server" style="width: 8%; height: 12px">
                                                <span>
                                                    <asp:Label ID="lblVisitPlace" runat="server" Text="Visit Place :"></asp:Label>
                                                </span>
                                            </td>
                                            <td class="Ecoheadtxt" id="tdVisitPlace1" runat="server" style="width: 13%; text-align: left; height: 12px">
                                                <asp:DropDownList ID="drpVisitPlace" runat="server" Width="120px">
                                                </asp:DropDownList>
                                            </td>
                                            <td id="Td1" runat="server" class="Ecoheadtxt" style="width: 8%; height: 12px">
                                                <a href="sales_conveyence.aspx" onclick="window.open(this.href,'popupwindow','left=120,top=170,height=450,width=900,scrollbars=no,toolbar=no,location=center,menubar=no'); return false;">Expenses</a>
                                            </td>
                                            <td id="Td2" runat="server" class="Ecoheadtxt" style="text-align: left; height: 12px"></td>
                                        </tr>
                                        <tr>
                                            <td class="Ecoheadtxt" style="width: 14%; height: 12px">
                                                <span>Note/Remarks :</span>
                                            </td>
                                            <td colspan="5" class="Ecoheadtxt" style="text-align: left; height: 12px">
                                                <asp:TextBox ID="txtNotes" TextMode="MultiLine" runat="server" Height="45px" Width="75%"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="Ecoheadtxt" style="width: 14%; height: 12px">
                                                <span>Next ActivityType</span></td>
                                            <td class="Ecoheadtxt" colspan="5" style="text-align: left; height: 12px">
                                                <asp:RadioButton ID="rdrCall" runat="server" GroupName="rdr" Text="Phone FollowUp"
                                                    Width="76px" />
                                                <asp:RadioButton ID="rdrVisit" runat="server" GroupName="rdr" Text="Visit" Checked="True"
                                                    Width="18px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="Ecoheadtxt" style="width: 14%; height: 12px">
                                                <asp:Label ID="lblNextVisitDate" Text="Next Visit Date : " runat="server" Font-Bold="False"
                                                    Width="130px"></asp:Label></td>
                                            <td class="Ecoheadtxt" style="width: 23%; text-align: left; height: 12px">
                                                <dxe:ASPxDateEdit ID="ASPxNextVisit" runat="server" EditFormat="Custom" EditFormatString="dd MMMM yyyy hh:mm tt">
                                                    <ButtonStyle Width="13px">
                                                    </ButtonStyle>
                                                </dxe:ASPxDateEdit>
                                                <%-- <asp:TextBox ID="ASPxNextVisit" runat="server"></asp:TextBox>
                                            <asp:Image ID="Image1" runat="server" ImageUrl="~/images/calendar.jpg" />--%>
                                            </td>
                                            <td class="Ecoheadtxt" id="tdnextvisit" style="width: 8%; height: 12px">
                                                <asp:Label ID="lblNextVisitPlace" Text="Next Visit Place : " runat="server" Font-Bold="False"
                                                    Width="85px"></asp:Label></td>
                                            <td class="Ecoheadtxt" id="tdnextvisit1" colspan="3" style="text-align: left; height: 12px">
                                                <asp:DropDownList ID="drpNextVisitPlace" runat="server" Width="120px">
                                                </asp:DropDownList>
                                                <asp:TextBox ID="txtNextVisitPlace" runat="server" Visible="False"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="6" align="center">
                                                <asp:Button ID="btnSavePhoneCallDetails" runat="server" Text="Save" CssClass="btnUpdate"
                                                    OnClick="btnSave1_Click" Height="21px" />
                                                <asp:Button ID="btnSetReminder" runat="server" Text="Reminder" Visible="False" CssClass="btnUpdate"
                                                    OnClick="btnSetReminder_Click" Height="21px" />
                                                <asp:Button ID="BtnSCancel" runat="server" Text="Cancel" CssClass="btnUpdate" OnClick="BtnSCancel_Click"
                                                    Height="21px" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:TextBox ID="txtStartDate" runat="server" BackColor="Transparent" BorderColor="Transparent"
                                    BorderStyle="None" ForeColor="#DDECFE"></asp:TextBox>
                                <asp:TextBox ID="txtEndTime" runat="server" BackColor="Transparent" BorderColor="Transparent"
                                    BorderStyle="None" ForeColor="#DDECFE"></asp:TextBox>
                                <asp:TextBox ID="txtExp" Text="0" runat="server" Visible="False" BackColor="Transparent"
                                    BorderColor="Transparent" BorderStyle="None" ForeColor="#DDECFE"></asp:TextBox>
                                <asp:Label ID="lblVisitExp" runat="server" Text="Visit Expenses:" BackColor="Transparent"
                                    BorderColor="Transparent" BorderStyle="None" ForeColor="#DDECFE" Visible="false"
                                    Width="65px"></asp:Label>
                                <input type="hidden" id="txtProductCount" name="txtProductCount" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
