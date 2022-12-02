<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="ERP.OMS.Management.Master.management_master_Contact_Remarks" MasterPageFile="~/OMS/MasterPage/ERP.Master" CodeBehind="Contact_Remarks.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <style>
        dxflHARSys > table, .dxflHARSys > div {
            text-align: left !important;
            margin-left: 0px !important;
            padding-left: 55px !important;
        }

        .dxpc-headerContent {
            color: #fff !important;
        }

        .dxgvPopupEditForm {
            background-color: Silver;
            margin: 8px 0px 8px 8px;
            width: 584px;
        }

        .dxgvCommandColumn a {
            color: #000000;
            font-weight: normal;
            font-size: 9pt;
            font-family: Tahoma;
            vertical-align: middle;
            border: solid 1px #7F7F7F;
            background: #E0DFDF url('../../WebResource.axd?d=_spZ71SnQgyimh4Ik74-9HFcObMTcdvPomzoDw07BJ7yszl8Y6ulZ8IMPG2bwgTIvk0NlZXVp5bqz54XCmU0ch7CFaYzQi1FcfXaGG94usLzNQNHBJHBtzt52SMe4MPHZ5kNiogkXYXSknkreo_-rWyjQdokHUVEUqZw7VR1WskJUAwD0&t=635755229276311331') top;
            background-repeat: repeat-x;
            padding: 3px;
            cursor: pointer;
            display: inline-block;
        }

        .dxgvCommandColumn {
            text-align: center;
        }

        #tabControl {
            width: 100%;
        }

        #tabControl_CC {
            overflow: visible !important;
        }
    </style>


    <script language="javascript" type="text/javascript">

        //debjyoti 13-12-2016
        var BeforeEdit;

        function clearAllMandatory() {
            var fieldData = document.getElementById('AllControslId').value;
            for (var i = 0 ; i < fieldData.split('~').length ; i++) {
                var type = fieldData.split('~')[i].split('/')[1];
                var fldId = fieldData.split('~')[i].split('/')[0];

                if (document.getElementById('Mandatory' + fldId)) {
                    $('#Mandatory' + fldId).css({ 'display': 'none' });
                }
            }
        }
        function clearAllControl() {
            var fieldData = document.getElementById('AllControslId').value;
            for (var i = 0 ; i < fieldData.split('~').length ; i++) {
                var type = fieldData.split('~')[i].split('/')[1];
                var fldId = fieldData.split('~')[i].split('/')[0];




                if (type == 1 || type == 4 || type == 5) {
                    document.getElementById('txt' + fldId).value = '';
                }

                if (type == 2) {
                    document.getElementById('memo' + fldId).value = '';
                }
                if (type == 6) {
                    document.getElementById('dd' + fldId).value = '';
                }

                if (type == 7) {
                    document.getElementById('chk' + fldId).checked = false;
                }

                if (type == 8) {
                    $("#rd" + fldId + " input[type=radio]:checked").prop("checked", false);
                }
            }


        }
        function SaveTabData() {
            clearAllMandatory();
            grid.PerformCallback('SAVETABDATA');
        }

        function HidetabPopUp() {
            clearAllMandatory();
            clearAllControl();
            cAspxTabPopUpControl.Hide();
        }

        function DeleteRow(keyValue) {
            jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    grid.PerformCallback('Delete~' + keyValue);
                }
            });
        }


        function LastCall(obj) {
            if (grid.cpMandatory != null) {
                if (grid.cpMandatory != "") {
                    for (var i = 0 ; i < grid.cpMandatory.split('~').length; i++)
                        $('#Mandatory' + grid.cpMandatory.split('~')[i]).css({ 'display': 'block' });
                }
            }

            if (grid.cpErrorMsg != null) {
                if (grid.cpErrorMsg.trim() != "") {
                    jAlert(grid.cpErrorMsg);
                    grid.cpErrorMsg = null;
                    return;
                }
            }

            if (grid.cpReturnMsg != null) {
                if (grid.cpReturnMsg.trim() != "") {
                    jAlert(grid.cpReturnMsg);
                    grid.cpReturnMsg = null;
                    console.log(BeforeEdit);

                    clearControl(BeforeEdit);

                    if (BeforeEdit && BeforeEdit.cat_field_type == 3) {
                        cAspxDatePopUp.Hide();
                    } else {
                        cAspxTabPopUpControl.Hide();
                    }

                    return;
                }
            }
            if (grid.cpEditJson != null) {
                if (grid.cpEditJson.trim() != "") {
                    console.log(grid.cpEditJson);
                    BeforeEdit = JSON.parse(grid.cpEditJson);
                    var jsonData = JSON.parse(grid.cpEditJson);
                    jsonData.rea_Remarks = jsonData.rea_Remarks.replace(/~/g, '\n');
                    seWidth(jsonData);
                    if (jsonData.cat_field_type == 3) {
                        document.getElementById('lblDate').value = jsonData.cat_description;
                        cAspxDatePopUp.Show();
                    } else {
                        //   cPopup_Empcitys.Show();
                        cAspxTabPopUpControl.Show();
                    }
                    showControl(jsonData);
                    grid.cpEditJson = null;
                }
            }
        }

        function seWidth(obj) {
            if (obj.cat_field_type == 8 || obj.cat_field_type == 7) {
                cPopup_Empcitys.SetWidth(320);
            }
        }
        function showControl(jsonData) {
            $("#lbl" + jsonData.cat_id).removeClass("hide");
            var actTab = ctabControl.GetTabByName(jsonData.cat_group_id);
            ctabControl.SetActiveTabIndex(actTab.index)

            if (jsonData.cat_field_type == 1 || jsonData.cat_field_type == 4 || jsonData.cat_field_type == 5) {
                $("#txt" + jsonData.cat_id).removeClass("hide");
                $("#txt" + jsonData.cat_id).closest("div.divControlClass").removeClass("hide");

                document.getElementById('txt' + jsonData.cat_id).value = jsonData.rea_Remarks;
                //$("#txt" + jsonData.cat_id).text(jsonData.rea_Remarks);
            }

            if (jsonData.cat_field_type == 2) {
                $("#memo" + jsonData.cat_id).removeClass("hide");
                $("#memo" + jsonData.cat_id).closest("div.divControlClass").removeClass("hide");
                $("#memo" + jsonData.cat_id).text(jsonData.rea_Remarks);
            }

            if (jsonData.cat_field_type == 3) {
                $("#dtEditDate").removeClass("hide");
                var EditDate = new Date(jsonData.rea_Remarks.split('/')[2].substring(0, 4), jsonData.rea_Remarks.split('/')[0] - 1, jsonData.rea_Remarks.split('/')[1], 0, 0, 0, 0);
                var MaxDate = new Date(jsonData.cat_max_date.split('/')[2].substring(0, 4), jsonData.cat_max_date.split('/')[0] - 1, jsonData.cat_max_date.split('/')[1], 0, 0, 0, 0);
                console.log("date:", EditDate);
                cdtEditDate.SetDate(EditDate);

                if (jsonData.cat_max_date.split('/')[2].substring(0, 4) != '1900')
                     cdtEditDate.SetMaxDate(MaxDate);

            }



            if (jsonData.cat_field_type == 6) {
                $("#dd" + jsonData.cat_id).removeClass("hide");
                $("#dd" + jsonData.cat_id).closest("div.divControlClass").removeClass("hide");
                $("#dd" + jsonData.cat_id + " ").val(jsonData.rea_Remarks);

            }

            if (jsonData.cat_field_type == 7) {
                $("#chk" + jsonData.cat_id).closest('span').removeClass("hide");
                $("#chk" + jsonData.cat_id).closest("div.divControlClass").removeClass("hide");
                $("#chk" + jsonData.cat_id).text(jsonData.rea_Remarks);
                if (jsonData.rea_Remarks == 1)
                    $("#chk" + jsonData.cat_id).attr('checked', true);
                else
                    $("#chk" + jsonData.cat_id).attr('checked', false);
            }

            if (jsonData.cat_field_type == 8) {
                $("#rd" + jsonData.cat_id).removeClass("hide");
                $("#rd" + jsonData.cat_id).closest("div.divControlClass").removeClass("hide");
                $("#rd" + jsonData.cat_id).select(jsonData.rea_Remarks);
                //$("." + jsonData.rea_Remarks).attr('checked', 'checked');
                $("input[type=radio][value='" + jsonData.rea_Remarks + "']").prop("checked", true)
            }
        }

        function GetUpdatedData(jsonData) {
            if (jsonData.cat_field_type == 1 || jsonData.cat_field_type == 4 || jsonData.cat_field_type == 5) {
                jsonData.rea_Remarks = document.getElementById('txt' + jsonData.cat_id).value;
            }

            if (jsonData.cat_field_type == 2) {
                jsonData.rea_Remarks = document.getElementById('memo' + jsonData.cat_id).value;
            }
            if (jsonData.cat_field_type == 6) {
                jsonData.rea_Remarks = document.getElementById('dd' + jsonData.cat_id).value;
            }

            if (jsonData.cat_field_type == 7) {

                if (document.getElementById('chk' + jsonData.cat_id).checked)
                    jsonData.rea_Remarks = 1;
                else
                    jsonData.rea_Remarks = 0;
            }

            if (jsonData.cat_field_type == 8) {
                if ($("#rd" + jsonData.cat_id + " input[type=radio]:checked").val())
                    jsonData.rea_Remarks = $("#rd" + jsonData.cat_id + " input[type=radio]:checked").val();
                else
                    jsonData.rea_Remarks = '';

            }

            return jsonData;

        }

        function clearControl(jsonData) {
            if (jsonData) {


                if (jsonData.cat_field_type == 1 || jsonData.cat_field_type == 4 || jsonData.cat_field_type == 5) {
                    document.getElementById('txt' + jsonData.cat_id).value = '';
                }

                if (jsonData.cat_field_type == 2) {
                    document.getElementById('memo' + jsonData.cat_id).value = '';
                }
                if (jsonData.cat_field_type == 6) {
                    document.getElementById('dd' + jsonData.cat_id).value = '';
                }

                if (jsonData.cat_field_type == 7) {
                    document.getElementById('chk' + jsonData.cat_id).checked = false;
                }

                if (jsonData.cat_field_type == 8) {
                    $("#rd" + jsonData.cat_id + " input[type=radio]:checked").prop("checked", false);
                }

                return jsonData;
            }
        }

        function SaveEdit() {
            var jsonString = JSON.stringify(GetUpdatedData(BeforeEdit));
            grid.PerformCallback('EDIT~' + jsonString);
        }

        function UpdateDate() {
            var jsonString = JSON.stringify(GetUpdatedData(BeforeEdit));
            grid.PerformCallback('UPDATE_DATE~' + jsonString);
        }

        function OnEdit(obj) {
            Action = 'edit';
            cPopup_Empcitys.SetWidth(cPopup_Empcitys.GetWidth() / 2);
            // cPopup_Empcitys.SetSize(300,200);
            $('.controlClass, .divControlClass, .dxeMemo_PlasticBlue, .dxeButtonEdit_PlasticBlue,.controlClass span.required').addClass("hide");
            $('#btn_tab_save').addClass("hide");
            $('#btn_tab_update').removeClass("hide");
            //document.getElementById("SaveRow").style.display = 'inline';
            Status = obj;
            grid.PerformCallback('BEFORE_' + obj);

        }
        //debjyoti 13-12-2016

        function MakeInVisible() {
            console.log("test");
            clearControl(BeforeEdit);
            cPopup_Empcitys.Hide();
        }
        function MakeDatePopupInVisible() {
            cAspxDatePopUp.Hide();
        }
        function HidePopUp() {
            var p = window.parent;
            var popup = p.window["cPopup_Empcitys"];
            popup.Hide();
        }
        function OnNewAdd() {
            // grid.AddNewRow();
            //cPopup_Empcitys.SetWidth(600);
            console.log("controlId", document.getElementById('AllControslId').value);
            if (document.getElementById('AllControslId').value.trim() != '') {
                $('.controlClass, .divControlClass, .dxeMemo_PlasticBlue, .dxeButtonEdit_PlasticBlue ').removeClass("hide");
                $('#btn_tab_save').removeClass("hide");
                $('#btn_tab_update').addClass("hide");
                clearAllControl();
                clearAllMandatory();
                //cPopup_Empcitys.Show();
                cAspxTabPopUpControl.Show();
            } else {
                jAlert('UDF not define.');
            }
        }

        function SignOff() {
            // window.parent.SignOff()
        }
        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }
        function callback() {
            grid.PerformCallback();
        }
        function disp_prompt(name) {
            if (name == "tab0") {
                //alert(name);
                document.location.href = "Contact_general.aspx";
            }
            if (name == "tab1") {
                //alert(name);
                document.location.href = "Contact_Correspondence.aspx";
            }
            else if (name == "tab2") {
                //alert(name);
                document.location.href = "Contact_BankDetails.aspx";
            }
            else if (name == "tab3") {
                //alert(name);
                document.location.href = "Contact_DPDetails.aspx";
            }
            else if (name == "tab4") {
                //alert(name);
                document.location.href = "Contact_Document.aspx";
            }
            else if (name == "tab12") {
                //alert(name);
                document.location.href = "Contact_FamilyMembers.aspx";
            }
            else if (name == "tab5") {
                //alert(name);
                document.location.href = "Contact_Registration.aspx";
            }
            else if (name == "tab7") {
                //alert(name);
                document.location.href = "Contact_GroupMember.aspx";
            }
            else if (name == "tab8") {
                //alert(name);
                document.location.href = "Contact_Deposit.aspx";
            }
            else if (name == "tab9") {
                //alert(name);
                //document.location.href="Contact_Remarks.aspx"; 
            }
            else if (name == "tab10") {
                //alert(name);
                document.location.href = "Contact_Education.aspx";
            }
            else if (name == "tab11") {
                //alert(name);
                document.location.href = "contact_brokerage.aspx";
            }
            else if (name == "tab6") {
                //alert(name);
                document.location.href = "contact_other.aspx";
            }
            else if (name == "tab13") {
                document.location.href = "contact_Subscription.aspx";
            }

        }
        FieldName = 'ASPxPageControl1_ASPxLabel3';
    </script>
    <style>
        /*#DynamicControlPanel {
            height: 400px;
            overflow-y: auto;
        }*/

        #DynamicControlPanel span {
            display: block;
        }
        #dtTabControl {
        width:100%;
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="panel-title">
        <%-- <h3>Contact Remarks List</h3>--%>
        <h3>
            <asp:Label ID="lblHeadTitle" runat="server"></asp:Label>
        </h3>

        <div class="crossBtn"><a href="frmContactMain.aspx?requesttype=<%= Session["Contactrequesttype"] %>"><i class="fa fa-times"></i></a></div>
    </div>
    <asp:HiddenField ID="AllControslId" runat="server" />
    <div class="PopUpArea">
        <dxe:ASPxPopupControl ID="Popup_Empcitys" runat="server" ClientInstanceName="cPopup_Empcitys"
            Width="600px" HeaderText="Add/Modify UDF " PopupHorizontalAlign="WindowCenter"
            BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <contentcollection>
                    <dxe:PopupControlContentControl runat="server">
                        <%--<div style="Width:400px;background-color:#FFFFFF;margin:0px;border:1px solid red;">--%>
                        <div class="Top clearfix">
                           
                            <table width="100%">
                                     <tr>
                                         <td colspan="3">
                                             <asp:Panel ID="DynamicControlPanel" runat="server" width="100%" CssClass="insdhide"> </asp:Panel>
                                           
                                                
                                         </td>

                                     </tr>
                                <tr>
                                    <td colspan="3" style="padding-left:15px">
                                       <%-- <asp:Button ID="btn_save" runat="server" Text="Save" OnClick="Button1_Click" CssClass="btn btn-primary" ></asp:Button>--%>
                                        <input type="button" value="Save" onclick="SaveEdit()" id="btn_update"  Class="btn btn-primary" >
                                        <%-- <input id="btnSave" class="btn btn-primary" onclick="Call_save(status)" type="button" value="Save" />--%>
                                    <input id="btnCancel" class="btn btn-danger" onclick="MakeInVisible()" type="button" value="Cancel" />
                                        </td>
                                        
                                    </tr>
                                </table>


                        </div>
                         
                    </dxe:PopupControlContentControl>
                </contentcollection>
            <headerstyle backcolor="LightGray" forecolor="Black" />
        </dxe:ASPxPopupControl>

    </div>

    <%--Tab Area--%>
    <div class="PopUpArea">
        <dxe:ASPxPopupControl ID="AspxTabPopUpControl" runat="server" ClientInstanceName="cAspxTabPopUpControl"
            Width="620px" HeaderText="Add/Modify UDF " PopupHorizontalAlign="WindowCenter"
            BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <contentcollection>
                    <dxe:PopupControlContentControl runat="server">
                        <%--<div style="Width:400px;background-color:#FFFFFF;margin:0px;border:1px solid red;">--%>
                        <div class="Top clearfix">
                           
                            <table width="100%">
                                     <tr>
                                         <td colspan="3">
                                            
                                           <dxe:ASPxPageControl ID="tabControl" runat="server" ClientInstanceName="ctabControl">

                                           </dxe:ASPxPageControl>
                                                
                                         </td>

                                     </tr>
                                <tr>
                                    <td colspan="3" style="padding-left:15px"> 
                                        <input type="button" value="Save" onclick="SaveTabData()" id="btn_tab_save"  Class="btn btn-primary" >
                                        <input type="button" value="Save" onclick="SaveEdit()" id="btn_tab_update"  Class="btn btn-primary" >
                                    <input id="btntabCancel" class="btn btn-danger" onclick="HidetabPopUp()" type="button" value="Cancel" />
                                        </td>
                                        
                                    </tr>
                                </table>


                        </div>
                         
                    </dxe:PopupControlContentControl>
                </contentcollection>
            <headerstyle backcolor="LightGray" forecolor="Black" />
        </dxe:ASPxPopupControl>

    </div>
    <%--End Of Tab Area--%>

    <div class="PopUpArea">
        <dxe:ASPxPopupControl ID="AspxDatePopUp" runat="server" ClientInstanceName="cAspxDatePopUp"
           Width="620px" HeaderText="Add/Modify UDF " PopupHorizontalAlign="WindowCenter"
            BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <contentcollection>
                    <dxe:PopupControlContentControl runat="server">
                      <contentcollection>
                    <dxe:PopupControlContentControl runat="server">
                        <%--<div style="Width:400px;background-color:#FFFFFF;margin:0px;border:1px solid red;">--%>
                        <div class="Top clearfix">
                           
                            <table width="100%"> 
                                 <tr  >
                                        <td>
                                       
                                             <dxe:ASPxPageControl ID="dtTabControl" runat="server" ClientInstanceName="cdtTabControl">
                                             <tabpages>
                                <dxe:TabPage Name="General" Text="Date">

                                    <ContentCollection>
                                        <dxe:ContentControl runat="server">
                                               <asp:Label runat="server" ID ="lblDate" Text="Date" ></asp:Label>
                                             
                                           <dxe:ASPxDateEdit ID="dtEditDate" width="270px" height="28px" runat="server" DisplayFormatString="dd-MM-yyyy"  NullText="dd-MM-yyyy"  ClientInstanceName="cdtEditDate">
                                             </dxe:ASPxDateEdit>

                                        </dxe:ContentControl>
                                    </ContentCollection>
                                </dxe:TabPage>
                                                 </tabpages>
                                        </dxe:ASPxPageControl>

                                          <%--  <asp:Label runat="server" ID ="lblDate" Text="Date" ></asp:Label>
                                             
                                           <dxe:ASPxDateEdit ID="dtEditDate" width="270px" height="28px" runat="server" DisplayFormatString="dd-MM-yyyy"  NullText="dd-MM-yyyy"  ClientInstanceName="cdtEditDate">
                                             </dxe:ASPxDateEdit>--%>

                                        </td>
                                         
                                   </tr>

                                <tr><td colspan="3" >
                                            <input id="btnSave" class="btn btn-primary" onclick="UpdateDate()" type="button" value="Save" />
                                    <input id="btndateCancel" class="btn btn-danger" onclick="MakeDatePopupInVisible()" type="button" value="Cancel" />
                                        </td>
                                        
                                    </tr>
                                </table>


                        </div>
                         
                    </dxe:PopupControlContentControl>
                </contentcollection>
            <contentstyle verticalalign="Top"></contentstyle>

            <headerstyle backcolor="LightGray" forecolor="Black" />
                    </dxe:PopupControlContentControl>
                </contentcollection>
            <headerstyle backcolor="LightGray" forecolor="Black" />
        </dxe:ASPxPopupControl>

    </div>
    <div class="form_main">
        <div>
            <table width="100%">
                <tr>
                    <td class="EHEADER" style="text-align: center">
                        <asp:Label ID="lblName" runat="server" Font-Bold="True"></asp:Label>


                    </td>
                </tr>
            </table>
            <table class="TableMain100">
                <tr>
                    <td>
                        <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="9" ClientInstanceName="page"
                            Font-Size="12px">
                            <tabpages>
                                <dxe:TabPage Name="General" Text="General">

                                    <ContentCollection>
                                        <dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
                                    </ContentCollection>
                                </dxe:TabPage>

                                <dxe:TabPage Name="CorresPondence" Text="Correspondence">

                                    <ContentCollection>
                                        <dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
                                    </ContentCollection>
                                </dxe:TabPage>
                                <dxe:TabPage Name="BankDetails" Text="Bank">

                                    <ContentCollection>
                                        <dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
                                    </ContentCollection>
                                </dxe:TabPage>
                                <dxe:TabPage Name="DPDetails" Visible="false" Text="DP">

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
                                <dxe:TabPage Name="Registration" Text="Registration">

                                    <ContentCollection>
                                        <dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
                                    </ContentCollection>
                                </dxe:TabPage>
                                <dxe:TabPage Name="Other" Visible="false" Text="Other">

                                    <ContentCollection>
                                        <dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
                                    </ContentCollection>
                                </dxe:TabPage>
                                <dxe:TabPage Name="GroupMember" Text="Group">
                                    <ContentCollection>
                                        <dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
                                    </ContentCollection>
                                </dxe:TabPage>
                                <dxe:TabPage Name="Deposit" Visible="false" Text="Deposit">
                                    <ContentCollection>
                                        <dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
                                    </ContentCollection>
                                </dxe:TabPage>
                                <dxe:TabPage Name="Remarks" Text="UDF">
                                    <ContentCollection>
                                        <dxe:ContentControl runat="server">
                                            <div style="float:left;">
                                                <% if (rights.CanAdd)
                                                   { %>
                                                  <%--<a href="javascript:void(0);" class="btn btn-primary" onclick="grid.AddNewRow();"><span>Add New</span> </a>--%>
                                                <a href="javascript:void(0);" class="btn btn-primary" onclick="OnNewAdd() "><span>Add New</span> </a>
                                                <% } %>
                                            </div>
                                            <dxe:ASPxGridView ID="GridRemarks" ClientInstanceName="grid" runat="server" AutoGenerateColumns="False" OnStartRowEditing="GridRemarks_StartRowEditing"
                                                DataSourceID="SqlRemarks" KeyFieldName="id" Width="100%" OnCellEditorInitialize="GridRemarks_CellEditorInitialize" OnInitNewRow="GridRemarks_InitNewRow"
                                                OnCustomCallback="GridRemarks_CustomCallback" OnCommandButtonInitialize="GridRemarks_CommandButtonInitialize">
                                               <%-- <SettingsSearchPanel Visible="True" />--%>
                                                <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true"  />
                                                <Columns>
                                                    <dxe:GridViewDataTextColumn FieldName="id" ReadOnly="True" Visible="False" VisibleIndex="0">
                                                        <EditFormSettings Visible="False" />
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="rea_internalId" VisibleIndex="1" Visible="False">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="description" Caption="UDF Name"
                                                        Width="40%" VisibleIndex="0">
                                                        <EditFormSettings Visible="False" />
                                                        <PropertiesTextEdit Width="350px"></PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataComboBoxColumn FieldName="cat_id" Visible="False" VisibleIndex="1">
                                                        <PropertiesComboBox Width="300px" DataSourceID="sqlCategory" ValueField="id" TextField="cat_description"
                                                            ValueType="System.String">
                                                        </PropertiesComboBox>
                                                        <EditFormSettings Caption="Category" Visible="True" VisibleIndex="0" />
                                                        <EditCellStyle Wrap="False">
                                                        </EditCellStyle>
                                                    </dxe:GridViewDataComboBoxColumn>
                                                    <dxe:GridViewDataMemoColumn Caption="UDF Value" FieldName="rea_Remarks" VisibleIndex="1"
                                                        Width="40%">
                                                        <EditCellStyle HorizontalAlign="Left">
                                                        </EditCellStyle>
                                                        <EditFormCaptionStyle HorizontalAlign="Right">
                                                        </EditFormCaptionStyle>
                                                        <CellStyle CssClass="gridcellleft">
                                                        </CellStyle>
                                                        <PropertiesMemoEdit Height="100px" Width="350px">
                                                        </PropertiesMemoEdit>
                                                        <EditFormSettings Caption="Remarks" VisibleIndex="1" />
                                                    </dxe:GridViewDataMemoColumn>
                                                    

                                                    <%--debjyoti 13-12-2016--%>
                                                     <dxe:GridViewDataTextColumn Caption="" VisibleIndex="3" Width="6%">
                                <CellStyle HorizontalAlign="Center">
                                </CellStyle>
                                <HeaderStyle HorizontalAlign="Center" />
                                <HeaderTemplate>
                                    Actions
                                    <%--<asp:HyperLink ID="HyperLink2" runat="server"
                                        NavigateUrl="javascript:void(0)" onclick="javascript:MakeRowVisible()">Add New</asp:HyperLink>--%>
                                </HeaderTemplate>
                                <DataItemTemplate>
                                    <% if (rights.CanEdit)
                                       { %>
                                    <a href="javascript:void(0);" onclick="OnEdit('EDIT~'+'<%# Container.KeyValue %>')">
                                        <img src="../../../assests/images/Edit.png" alt="Edit"></a>
                                    <% } %>
                                    <% if (rights.CanDelete)
                                       { %>
                                     <a href="javascript:void(0);" onclick="DeleteRow('<%# Container.KeyValue %>')" alt="Delete">
                                        <img src="../../../assests/images/Delete.png" /></a>
                                     <% } %>
                                </DataItemTemplate>
                            </dxe:GridViewDataTextColumn>

                                                    <%--debjyoti 13-12-2016--%>

                                                </Columns>
                                                <SettingsCommandButton>
                                                    <EditButton Image-Url="../../../assests/images/Edit.png" ButtonType="Image" Image-AlternateText="Edit" Styles-Style-CssClass="pad">
                                                    </EditButton>
                                                    <DeleteButton Image-Url="../../../assests/images/Delete.png" ButtonType="Image" Image-AlternateText="Delete">
                                                    </DeleteButton>
                                                    <UpdateButton Text="Save" ButtonType="Button" Styles-Style-CssClass="btn btn-primary"></UpdateButton>
                                                    <CancelButton Text="Cancel" ButtonType="Button" Styles-Style-CssClass="btn btn-danger"></CancelButton>
                                                </SettingsCommandButton>

                                                <Styles>
                                                    <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                    </Header>
                                                    <LoadingPanel ImageSpacing="10px">
                                                    </LoadingPanel>
                                                </Styles>
                                                <Settings ShowGroupPanel="True" ShowTitlePanel="false" />
                                                <SettingsText PopupEditFormCaption="Add Remarks" ConfirmDelete="Confirm delete?" />
                                                <SettingsPager NumericButtonCount="20" PageSize="20" ShowSeparators="True">
                                                    <FirstPageButton Visible="True">
                                                    </FirstPageButton>
                                                    <LastPageButton Visible="True">
                                                    </LastPageButton>
                                                </SettingsPager>
                                                <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True" />
                                                <SettingsEditing Mode="PopupEditForm"  PopupEditFormHorizontalAlign="Center"
                                                    PopupEditFormModal="True" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="500px"
                                                    EditFormColumnCount="1" />
                                                <Templates>
                                                    <TitlePanel>
                                                        <div style="display: none">
                                                            <table style="width: 100%">
                                                                <tr>
                                                                    <td style="text-align: left; vertical-align: top">
                                                                        <table>
                                                                            <tr>
                                                                                <td id="ShowFilter">
                                                                                    <%-- <a href="javascript:ShowHideFilter('s');"><span style="color: #000099; text-decoration: underline">
                                                                                    Show Filter</span></a>--%>
                                                                                </td>
                                                                                <td id="Td1">
                                                                                    <%--  <a href="javascript:ShowHideFilter('All');"><span style="color: #000099; text-decoration: underline">
                                                                                    All Records</span></a>--%>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <%--  <table style="width:100%">
                                                <tr>
                                                     <td align="right">
                                                        <table width="200">
                                                            <tr>
                                                                
                                                                <td>
                                                                    <dxe:ASPxButton ID="ASPxButton2" runat="server" Text="Search" ToolTip="Search" OnClick="btnSearch"  Height="18px" Width="88px">
                                                                    </dxe:ASPxButton>
                                                                </td>
                                                                <td>
                                                                    <dxe:ASPxButton ID="ASPxButton1" runat="server" Text="ADD" ToolTip="Add New Data"   Height="18px" Width="88px" AutoPostBack="False">
                                                                        <clientsideevents click="function(s, e) {grid.AddNewRow();}" />
                                                                    </dxe:ASPxButton>
                                                                </td>
                                                                                                    
                                                                 
                                                              </tr>
                                                          </table>
                                                      </td>   
                                                 </tr>
                                            </table>--%>
                                                        </div>
                                                    </TitlePanel>
                                                </Templates>
                                                <%--debjyoti 13-12-2016--%>
                                                  <clientsideevents endcallback="function(s, e) {
	LastCall(s.cpHeight);
}" />
                                                <%--debjyoti 13-12-2016 end--%>
                                            </dxe:ASPxGridView>
                                        </dxe:ContentControl>
                                    </ContentCollection>
                                </dxe:TabPage>
                                <dxe:TabPage Name="Education" Visible="false" Text="Education">
                                    <ContentCollection>
                                        <dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
                                    </ContentCollection>
                                </dxe:TabPage>
                                <dxe:TabPage Name="Trad. Prof." Visible="false" Text="Trad.Prof">
                                    <%--<TabTemplate ><span style="font-size:x-small">Trad.Prof</span>&nbsp;<span style="color:Red;">*</span> </TabTemplate>--%>
                                    <ContentCollection>
                                        <dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
                                    </ContentCollection>
                                </dxe:TabPage>
                                <dxe:TabPage Name="FamilyMembers" Visible="false" Text="Family">
                                    <ContentCollection>
                                        <dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
                                    </ContentCollection>
                                </dxe:TabPage>
                                <dxe:TabPage Name="Subscription" Visible="false" Text="Subscription">
                                    <ContentCollection>
                                        <dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
                                    </ContentCollection>
                                </dxe:TabPage>
                            </tabpages>
                            <clientsideevents activetabchanged="function(s, e) {
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
	                                            var Tab13=page.GetTab(13);
	                                             
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
	                                            else if(activeTab == Tab13)
	                                            {
	                                               disp_prompt('tab13');
	                                            }
	                                            
	                                            }"></clientsideevents>
                            <contentstyle>
                                <Border BorderColor="#002D96" BorderStyle="Solid" BorderWidth="1px" />
                            </contentstyle>
                            <loadingpanelstyle imagespacing="6px">
                            </loadingpanelstyle>
                        </dxe:ASPxPageControl>
                    </td>
                </tr>
                <tr>
                    <td style="height: 8px">
                        <table style="width: 100%;">
                            <tr>
                                <td align="right" style="width: 843px"></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <asp:SqlDataSource ID="SqlRemarks" runat="server"
                DeleteCommand="DELETE FROM [tbl_master_contactRemarks] WHERE [id] = @id"
                InsertCommand="INSERT INTO [tbl_master_contactRemarks] ([rea_internalId],[cat_id], [rea_Remarks], [CreateDate], [CreateUser]) VALUES (@rea_internalId,@cat_id, @rea_Remarks, getdate(), @CreateUser)"
                SelectCommand="SELECT id,rea_internalId,cat_id,case (select cat_field_type from tbl_master_remarksCategory h where h.id=[tbl_master_contactRemarks].cat_id ) when 3 then case rtrim(rea_Remarks) when '' then '' else CONVERT(VARCHAR(20),cast (rea_Remarks as Datetime),106) end when 7 then case rea_Remarks when 1 then 'Checked' else 'Not Checked' end  else rea_Remarks end as 'rea_Remarks',CreateDate,CreateUser,LastModifyDate,LastModifyUser,isnull((select cat_description from tbl_master_remarksCategory where id=tbl_master_contactRemarks.cat_id),'None') as description FROM [tbl_master_contactRemarks] where rea_internalId=@rea_internalId"
                UpdateCommand="UPDATE [tbl_master_contactRemarks] SET [rea_internalId] = @rea_internalId,cat_id=@cat_id, [rea_Remarks] = @rea_Remarks,  [LastModifyDate] = getdate(), [LastModifyUser] = @LastModifyUser WHERE [id] = @id">
                <DeleteParameters>
                    <asp:Parameter Name="id" Type="Int32" />
                </DeleteParameters>
                <InsertParameters>
                    <asp:SessionParameter Name="rea_internalId" SessionField="KeyVal_InternalID" Type="string" />
                    <asp:Parameter Name="cat_id" Type="int32" />
                    <asp:Parameter Name="rea_Remarks" Type="String" />
                    <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="string" />
                </InsertParameters>
                <UpdateParameters>
                    <asp:SessionParameter Name="rea_internalId" SessionField="KeyVal_InternalID" Type="string" />
                    <asp:Parameter Name="cat_id" Type="int32" />
                    <asp:Parameter Name="rea_Remarks" Type="String" />
                    <asp:SessionParameter Name="LastModifyUser" SessionField="userid" Type="string" />
                    <asp:Parameter Name="id" Type="Int32" />
                </UpdateParameters>
                <SelectParameters>
                    <asp:SessionParameter Name="rea_internalId" SessionField="KeyVal_InternalID" Type="string" />
                </SelectParameters>
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="sqlCategory" runat="server" ConflictDetection="CompareAllValues"
                SelectCommand="SELECT * FROM [tbl_master_remarksCategory] where  cat_applicablefor=@KeyVal1">
                <SelectParameters>


                    <%-- <asp:SessionParameter Name="Type" SessionField="KeyVal1" Type="string" />--%>
                    <asp:SessionParameter Name="KeyVal1" SessionField="KeyVal1" Type="string" />
                </SelectParameters>
            </asp:SqlDataSource>
        </div>
    </div>
</asp:Content>
