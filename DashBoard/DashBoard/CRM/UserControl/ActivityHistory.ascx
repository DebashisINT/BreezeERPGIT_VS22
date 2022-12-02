<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ActivityHistory.ascx.cs" Inherits="DashBoard.DashBoard.CRM.UserControl.ActivityHistory" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>


<script>
    function ActHisCountGenerate(e) {
        //cgridActHis.PerformCallback();
        if (cempButtonEdit.GetValue() == null) {
            jAlert('Please select Salesman for generate the report.');
        }
        else {
            cgridActHis.PerformCallback();
        }
        e.preventDefault();
    }

    function EmployeeSelect() {
        $('#EmployeeModel').modal('show');
    }

    function EmployeeKeyDown(s, e) {
        if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
            EmployeeSelect();
        }
    }

    function Employeekeydown(e) {

        var OtherDetails = {}
        OtherDetails.SerarchKey = $("#txtEmpSearch").val();

        if (e.code == "Enter" || e.code == "NumpadEnter") {
            var HeaderCaption = [];
            HeaderCaption.push("Salesmen");
            HeaderCaption.push("ID");
            if ($("#txtEmpSearch").val() != '') {
                callonServer("../Service/general.asmx/GetEmployee", OtherDetails, "EmployeeTable", HeaderCaption, "EmployeeIndex", "SetEmployee");

                e.preventDefault();
                return false;
            }
        }
        else if (e.code == "ArrowDown") {
            if ($("input[EmployeeIndex=0]"))
                $("input[EmployeeIndex=0]").focus();
        }

    }

    function SetEmployee(id, name) {
        $('#EmployeeModel').modal('hide');
        cempButtonEdit.SetText(name);
        $('#EmpIdActHis').val(id);
    }


    function ValueSelected(e, indexName) {
        if (e.code == "Enter" || e.code == "NumpadEnter") {
            var Id = e.target.parentElement.parentElement.cells[0].innerText;
            var name = e.target.parentElement.parentElement.cells[1].children[0].value;
            if (Id) {
                SetEmployee(Id, name);
            }

        }

        else if (e.code == "ArrowDown") {
            thisindex = parseFloat(e.target.getAttribute(indexName));
            thisindex++;
            if (thisindex < 10)
                $("input[" + indexName + "=" + thisindex + "]").focus();
        }
        else if (e.code == "ArrowUp") {
            thisindex = parseFloat(e.target.getAttribute(indexName));
            thisindex--;
            if (thisindex > -1)
                $("input[" + indexName + "=" + thisindex + "]").focus();
            else {

                $('#txtEmpSearch').focus();
            }
        }

    }
</script>

<div>
    <aside class="colWraper">
        <div class="diverh">
            <asp:HiddenField runat="server" ID="EmpIdActHis" />
            <table>
                <tr>
                    <td class="firstCell" style="width: 103px"><i class="fa fa-history" style="font-size: 25px; color: #f7ffb7;"></i><span class="trSpan">(Activity History)</span></td>
                    <td class="pad5">


                        <dx:ASPxDateEdit ID="FromDateActHis" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" AllowNull="false"
                            ClientInstanceName="cFormDateActHis" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True" data-toggle="tooltip" title="From Date"
                            Theme="PlasticBlue">
                            <ButtonStyle Width="13px">
                            </ButtonStyle>
                        </dx:ASPxDateEdit>
                    </td>


                    <td class="pad5">
                        <dx:ASPxDateEdit ID="TodateActHis" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" AllowNull="false"
                            ClientInstanceName="cTodateActHis" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True" data-toggle="tooltip" title="To Date"
                            Theme="PlasticBlue">
                            <ButtonStyle Width="13px">
                            </ButtonStyle>
                        </dx:ASPxDateEdit>
                    </td>

                    <td class="pad5">
                        <asp:DropDownList ID="dpActivitylist" runat="server" Width="100%" Style="margin-top: 5px;" data-toggle="tooltip" title="Activity Status">
                            <asp:ListItem Text="Future Sale" Value="3"></asp:ListItem>
                            <asp:ListItem Text="Clarification Required" Value="5"></asp:ListItem>
                            <asp:ListItem Text="Document Collection" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Closed" Value="2"></asp:ListItem>
                        </asp:DropDownList>
                    </td>


                    <td class="pad5">
                        <dx:ASPxTextBox ID="txtlastDays" ClientInstanceName="ctxtlastDays" runat="server" Width="80%" ValidationSettings-Display="Dynamic" data-toggle="tooltip" title="No. of History.">
                            <MaskSettings Mask="&lt;1..5&gt;" AllowMouseWheel="false" />
                        </dx:ASPxTextBox>
                    </td>

                    <td class="pad5 relative">
                        <span style="color: red;position: absolute;left: -8px;top: 18px;">*</span>
                        <dx:ASPxButtonEdit ID="empButtonEdit" ReadOnly="true" runat="server" ClientInstanceName="cempButtonEdit" data-toggle="tooltip" title="Salesmen">
                            <Buttons>
                                <dx:EditButton></dx:EditButton>
                            </Buttons>
                            <ClientSideEvents ButtonClick="function(s,e){EmployeeSelect();}" KeyDown="function(s,e){EmployeeKeyDown(s,e);}" />
                        </dx:ASPxButtonEdit>
                    </td>


                    <td class="pad5">
                        <%--<input type="button" value="Show" class="btn btn-success" onclick="CallCountGenerate()" />--%>

                        <a href="#" data-toggle="tooltip" title="Generate" class="white">
                            <i class="fa fa-play-circle" onclick="ActHisCountGenerate(event)"></i>
                        </a>
                    </td>
                    <td class="pad5">
                        <asp:LinkButton ID="LinkButton1ActHis" class="white" runat="server" OnClick="LinkButton1_Click" data-toggle="tooltip" title="Export to Excel">
                          <%--  <i class="fa fa-file"></i>--%>
                             <img src="../../Dashboard/images/excel.png" class="excelIco" />
                        </asp:LinkButton>

                    </td>

                    <td class="pad5">
                        <span data-toggle="tooltip" title="Help">
                            <span data-toggle="popover" data-placement="left" data-html="true" data-content="This report shows the Activity History based on each Activities entered by the user in the given period for their assigned task."><i class="fa fa-question-circle"></i></span>
                        </span>
                    </td>

                </tr>
            </table>


        </div>






        <dx:ASPxGridViewExporter ID="exporterActHis" runat="server" Landscape="false" PaperKind="A4" GridViewID="gridActHis"
            PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true" FileName="Call Count">
        </dx:ASPxGridViewExporter>

        <dx:ASPxGridView ID="gridActHis" runat="server" ClientInstanceName="cgridActHis" KeyFieldName="SMenId"
            Width="100%" Settings-HorizontalScrollBarMode="Auto"
            SettingsBehavior-ColumnResizeMode="Control" DataSourceID="EntityServerModeActHis"
            Settings-VerticalScrollableHeight="237" SettingsBehavior-AllowSelectByRowClick="true"
            Settings-VerticalScrollBarMode="Auto" Theme="PlasticBlue" OnCustomCallback="gridActHis_CustomCallback"
            Settings-ShowFilterRow="true" Settings-ShowFilterRowMenu="true">
            <SettingsDataSecurity AllowDelete="false" AllowEdit="false" AllowInsert="false" />
            <SettingsBehavior EnableCustomizationWindow="true" />
            <Settings ShowFooter="true" />
            <SettingsContextMenu Enabled="true" />
            <Columns>


                 <dx:GridViewBandColumn HeaderStyle-CssClass="gridHeader" Caption="Activity Details" Name="ActDet" HeaderStyle-HorizontalAlign="Center">
                      <columns>

                <dx:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Salesman" FieldName="SmenName" Width="250">
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Customer" FieldName="CustName" Width="250">
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Product/Class" FieldName="ProdClass" Width="250">
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Budget" FieldName="Budget" Width="100">
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Equals" />
                </dx:GridViewDataTextColumn>
                 </columns>
                </dx:GridViewBandColumn>



                <dx:GridViewBandColumn HeaderStyle-CssClass="gridHeader" Caption="Outcome 1" Name="Outcomeband1" HeaderStyle-HorizontalAlign="Center">
                      <columns>

                <dx:GridViewDataDateColumn Caption="Date" Width="95" FieldName="OutcomeDate1"
                    PropertiesDateEdit-DisplayFormatString="dd-MM-yyyy" PropertiesDateEdit-EditFormatString="dd-MM-yyyy">
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Equals" />
                </dx:GridViewDataDateColumn>


                <dx:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Outcome" FieldName="OutCome1" Width="200">
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Remarks" FieldName="Remarks1" Width="200">
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dx:GridViewDataTextColumn>
                </columns>
                </dx:GridViewBandColumn>


                   <dx:GridViewBandColumn HeaderStyle-CssClass="gridHeader" Caption="Outcome 2" Name="Outcomeband2" HeaderStyle-HorizontalAlign="Center">
                      <columns>
                <dx:GridViewDataDateColumn Caption="Date" Width="95" FieldName="OutcomeDate2"
                    PropertiesDateEdit-DisplayFormatString="dd-MM-yyyy" PropertiesDateEdit-EditFormatString="dd-MM-yyyy">
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Equals" />
                </dx:GridViewDataDateColumn>

                <dx:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Outcome" FieldName="OutCome2" Width="200">
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Remarks" FieldName="Remarks2" Width="200">
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dx:GridViewDataTextColumn>
                </columns>
                </dx:GridViewBandColumn>



                <dx:GridViewBandColumn HeaderStyle-CssClass="gridHeader" Caption="Outcome 3" Name="Outcomeband3" HeaderStyle-HorizontalAlign="Center">
                      <columns>
                <dx:GridViewDataDateColumn Caption="Date" Width="95" FieldName="OutcomeDate3"
                    PropertiesDateEdit-DisplayFormatString="dd-MM-yyyy" PropertiesDateEdit-EditFormatString="dd-MM-yyyy">
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Equals" />
                </dx:GridViewDataDateColumn>


                    <dx:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Outcome" FieldName="OutCome3" Width="200">
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Remarks" FieldName="Remarks3" Width="200">
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dx:GridViewDataTextColumn>
                  </columns>
                </dx:GridViewBandColumn>


                 <dx:GridViewBandColumn HeaderStyle-CssClass="gridHeader" Caption="Outcome 4" Name="Outcomeband4" HeaderStyle-HorizontalAlign="Center">
                      <columns>
                <dx:GridViewDataDateColumn Caption="Date" Width="95" FieldName="OutcomeDate4"
                    PropertiesDateEdit-DisplayFormatString="dd-MM-yyyy" PropertiesDateEdit-EditFormatString="dd-MM-yyyy">
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Equals" />
                </dx:GridViewDataDateColumn>


                <dx:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Outcome" FieldName="OutCome4" Width="200">
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Remarks" FieldName="Remarks4" Width="200">
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dx:GridViewDataTextColumn>
                  </columns>
                </dx:GridViewBandColumn>




                     <dx:GridViewBandColumn HeaderStyle-CssClass="gridHeader" Caption="Outcome 5" Name="Outcomeband5" HeaderStyle-HorizontalAlign="Center">
                      <columns>
                <dx:GridViewDataDateColumn Caption="Date" Width="95" FieldName="OutcomeDate5"
                    PropertiesDateEdit-DisplayFormatString="dd-MM-yyyy" PropertiesDateEdit-EditFormatString="dd-MM-yyyy">
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Equals" />
                </dx:GridViewDataDateColumn>


                <dx:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Outcome" FieldName="OutCome5" Width="200">
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Remarks" FieldName="Remarks5" Width="200">
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dx:GridViewDataTextColumn>

                           </columns>
                </dx:GridViewBandColumn>



                <dx:GridViewBandColumn HeaderStyle-CssClass="gridHeader" Caption="Feedback 1" Name="FeedBackband1" HeaderStyle-HorizontalAlign="Center">
                    <columns>
                        <dx:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="FeedBack" FieldName="FeedBack1" Width="200">
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>  
                        <dx:GridViewDataDateColumn Caption="Date" Width="95" FieldName="FeedBackDate1"
                        PropertiesDateEdit-DisplayFormatString="dd-MM-yyyy" PropertiesDateEdit-EditFormatString="dd-MM-yyyy">
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Equals" />
                        </dx:GridViewDataDateColumn>
                               </columns>
                </dx:GridViewBandColumn>





            <dx:GridViewBandColumn HeaderStyle-CssClass="gridHeader" Caption="Feedback 2" Name="FeedBackband2" HeaderStyle-HorizontalAlign="Center">
            <columns>
                <dx:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="FeedBack" FieldName="FeedBack2" Width="200">
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataDateColumn Caption="Date" Width="95" FieldName="FeedBackDate2"
                    PropertiesDateEdit-DisplayFormatString="dd-MM-yyyy" PropertiesDateEdit-EditFormatString="dd-MM-yyyy">
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Equals" />
                </dx:GridViewDataDateColumn>
                        </columns>
                </dx:GridViewBandColumn>



                <dx:GridViewBandColumn HeaderStyle-CssClass="gridHeader" Caption="Feedback 3" Name="FeedBackband3" HeaderStyle-HorizontalAlign="Center">
            <columns>
                <dx:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="FeedBack" FieldName="FeedBack3" Width="200">
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataDateColumn Caption="Date" Width="95" FieldName="FeedBackDate3"
                    PropertiesDateEdit-DisplayFormatString="dd-MM-yyyy" PropertiesDateEdit-EditFormatString="dd-MM-yyyy">
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Equals" />
                </dx:GridViewDataDateColumn>
                   </columns>
                </dx:GridViewBandColumn>


                   <dx:GridViewBandColumn HeaderStyle-CssClass="gridHeader" Caption="Feedback 4" Name="FeedBackband4" HeaderStyle-HorizontalAlign="Center">
            <columns>
                <dx:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="FeedBack" FieldName="FeedBack4" Width="200">
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataDateColumn Caption="Date" Width="95" FieldName="FeedBackDate4"
                    PropertiesDateEdit-DisplayFormatString="dd-MM-yyyy" PropertiesDateEdit-EditFormatString="dd-MM-yyyy">
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Equals" />
                </dx:GridViewDataDateColumn>
                 </columns>
                </dx:GridViewBandColumn>



                   <dx:GridViewBandColumn HeaderStyle-CssClass="gridHeader" Caption="Feedback 5" Name="FeedBackband5" HeaderStyle-HorizontalAlign="Center">
            <columns>
                <dx:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="FeedBack" FieldName="FeedBack5" Width="200">
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataDateColumn Caption="Date" Width="95" FieldName="FeedBackDate5"
                    PropertiesDateEdit-DisplayFormatString="dd-MM-yyyy" PropertiesDateEdit-EditFormatString="dd-MM-yyyy">
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Equals" />
                </dx:GridViewDataDateColumn>
                </columns>
                </dx:GridViewBandColumn>

            </Columns>

            <SettingsPager PageSize="10" NumericButtonCount="4">
                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
            </SettingsPager>
        </dx:ASPxGridView>

        <dx:LinqServerModeDataSource ID="EntityServerModeActHis" runat="server" OnSelecting="EntityServerModeActHis_Selecting" />


        <!--Employee Modal -->
        <div class="modal fade" id="EmployeeModel" role="dialog">
            <div class="modal-dialog">

                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">Search</h4>
                    </div>
                    <div class="modal-body">
                        <input type="text" onkeydown="Employeekeydown(event)" id="txtEmpSearch" autofocus width="100%" autocomplete="off" placeholder="Search By Employee Name or Unique Id" />

                        <div id="EmployeeTable">
                            <table border='1' width="100%" class="dynamicPopupTbl">
                                <tr class="HeaderStyle">
                                    <th class="hide">id</th>
                                    <th>Salesmen</th>
                                    <th>ID</th>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    </div>
                </div>

            </div>
        </div>


    </aside>

</div>

