<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="ERP.OMS.Management.Master.management_frm_BranchUdfPopUp" MasterPageFile="~/OMS/MasterPage/PopUp.Master" CodeBehind="frm_BranchUdfPopUp.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .pad {
            
            Contact_Document .aspx;
        }
    </style>
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

    
    <%-- <script src="/assests/pluggins/choosen/choosen.min.js"></script>--%>
    <script language="javascript" type="text/javascript">

        ////for chosen

        //$(document).ready(function () {
        //    ListBind(); 
        //});
        //function ListBind() { 
        //    var config = {
        //        '.chsn': {},
        //        '.chsn-deselect': { allow_single_deselect: true },
        //        '.chsn-no-single': { disable_search_threshold: 10 },
        //        '.chsn-no-results': { no_results_text: 'Oops, nothing found!' },
        //        '.chsn-width': { width: "100%" }
        //    }
        //    for (var selector in config) {
        //        $(selector).chosen(config[selector]);
        //    }


        //}

        ////end of chosen

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
            ctabControl.PerformCallback('SAVETABDATA');
        }

        function HidetabPopUp() {
            
            clearAllMandatory();
            clearAllControl();
            clearControl(BeforeEdit);
            if (parent.cUDFpopup){
                parent.cUDFpopup.Hide();
            } else {
                 parent.popup.Hide();
            } 
             
        }

        function DeleteRow(keyValue) {
            jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    grid.PerformCallback('Delete~' + keyValue);
                }
            });
        }


        function LastCall(obj) {
            if (ctabControl.cpMandatory != null) {
                if (ctabControl.cpMandatory != "") {
                    for (var i = 0 ; i < ctabControl.cpMandatory.split('~').length; i++) {
                        console.log('#Mandatory' + ctabControl.cpMandatory.split('~')[i]);
                        $('#Mandatory' + ctabControl.cpMandatory.split('~')[i]).css({ 'display': 'block' });
                    }
                }
            }

            if (ctabControl.cpErrorMsg != null) {
                if (ctabControl.cpErrorMsg.trim() != "") {
                    jAlert(ctabControl.cpErrorMsg);
                    ctabControl.cpErrorMsg = null;
                    return;
                }
            }

            if (ctabControl.cpReturnMsg != null) {
                if (ctabControl.cpReturnMsg.trim() != "") {
                    jAlert(ctabControl.cpReturnMsg);
                    ctabControl.cpReturnMsg = null;
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
            if (ctabControl.cpEditJson != null) {
                if (ctabControl.cpEditJson.trim() != "") {
                    console.log(ctabControl.cpEditJson);
                    BeforeEdit = JSON.parse(ctabControl.cpEditJson);
                    var jsonData = JSON.parse(ctabControl.cpEditJson);
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
                    ctabControl.cpEditJson = null;
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
                console.log(jsonData.cat_max_date.split('/')[2].substring(0, 4));
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
            alert('1');
            clearControl(BeforeEdit);
            alert('2');
            this.Hide();
            alert('3');
            //cPopup_Empcitys.Hide();
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
            $('.controlClass, .divControlClass, .dxeMemo_PlasticBlue, .dxeButtonEdit_PlasticBlue ').removeClass("hide");
            $('#btn_tab_save').removeClass("hide");
            $('#btn_tab_update').addClass("hide");
            clearAllControl();
            clearAllMandatory();
            //cPopup_Empcitys.Show();
            cAspxTabPopUpControl.Show();
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
                document.location.href = "BranchAddEdit.aspx?id="+ <%=Convert.ToString( Session["con_branch"] )%>+'';
            }
            if (name == "tab1") {
                document.location.href = "Contact_Document.aspx";
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
        .col-md-6 {
            width:95% !important;
            float:left;
        }
        .divControlClass>span.controlClass {
                  margin-top:8px;
                      display: block;
              }
        .chsn + .chosen-container-multi {
            width:100% !important;
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <%--<div class="panel-title">
       
        <h3>
            <asp:Label ID="lblHeadTitle" runat="server"></asp:Label>
        </h3>

        <div class="crossBtn"><a href="Branch.aspx"><i class="fa fa-times"></i></a></div>
    </div>--%>
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
                                    <input id="btnCancel" class="btn btn-danger" onclick="MakeInVisible()" type="button" value="Close" />
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
                                    <input id="btndateCancel" class="btn btn-danger" onclick="MakeDatePopupInVisible()" type="button" value="Close" />
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
    
        <div>
            <table width="100%">
                 
                <tr>
                    <div class="Top clearfix">
                           
                            <table width="100%">
                                     <tr>
                                         <td colspan="3">
                                            
                                           <dxe:ASPxPageControl ID="tabControl" runat="server" ClientInstanceName="ctabControl" OnCallback="tabControl_Callback">
                                               <clientsideevents endcallback="function(s, e) {
	LastCall();
}" />

                                           </dxe:ASPxPageControl>
                                                
                                         </td>

                                     </tr>
                                <tr>
                                    <td colspan="3" style="padding-left:15px;padding-top:15px;"> 
                                        <input type="button" value="Save" onclick="SaveTabData()" id="btn_tab_save"  Class="btn btn-primary" >
                                        <%--<input type="button" value="Save" onclick="SaveEdit()" id="btn_tab_update"  Class="btn btn-primary" >--%>
                                    <input id="btntabCancel" class="btn btn-danger" onclick="HidetabPopUp()" type="button" value="Close" /> 
                                        </td>
                                        
                                    </tr>
                                </table>


                        </div>

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
    
</asp:Content>
