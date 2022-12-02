<%@ Page Title="" Language="C#" MaintainScrollPositionOnPostback="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="import_consumption.aspx.cs" Inherits="ERP.OMS.Management.Activities.import_consumption" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .ptTble {
            min-width: 600px;
        }

            .ptTble > thead > tr > th {
                padding: 3px 5px;
                background: #efefef;
            }

            .ptTble > tbody > tr > td {
                padding: 3px 5px;
            }
    </style>




</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <script>
        function UploadComplete() {
            $.ajax({
                type: "POST",
                url: "import_consumption.aspx/GetList",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {
                    var result = msg.d;

                    //console.log(result);
                    var output = "";
                    if (result) {

                        var html = '';
                        html += "<thead>";
                        html += "<th class='hide'>ID</th>";
                        html += "<th>Feild Name</th>";
                        html += "<th>Description</th>";
                        html += "<th>Mapping</th>";
                        html += "</thead>";

                        html += "<tbody>";
                        for (var i = 0; i < result.length; i++) {
                            html += "<tr>";

                            html += "<td class='hide'>" + result[i].ID + "</td>";


                            if (result[i].Mandatory == "True")
                                html += "<td>" + result[i].Caption + "<i class='fa fa-asterisk' style='font-size:10px;color:red'></i></td>";

                            else
                                html += "<td>" + result[i].Caption + "</td>";
                            html += "<td>" + result[i].Desc + "</td>";
                            html += "<td><select id='ddl" + result[i].ID + "'>";



                            for (var j = 0; j < result[i].Excel_Table.length; j++) {
                                if (result[i].Excel_Table[j].ValueFeild == result[i].Excel_Map)
                                    html += "<option selected value='" + result[i].Excel_Table[j].ValueFeild + "'>" + result[i].Excel_Table[j].TextFeild + "</option>";
                                else
                                    html += "<option value='" + result[i].Excel_Table[j].ValueFeild + "'>" + result[i].Excel_Table[j].TextFeild + "</option>";

                                if (result[i].Caption = 'Document Number') {
                                    if ((result[i].Excel_Table[j].ValueFeild == result[i].Excel_Map && result[i].Excel_Table[j].ValueFeild != "") || result[i].Excel_Map != "") {
                                        output = "Yes";
                                    }
                                }

                            }

                            html += "</select>";
                            html += "</td>";
                            html += "</tr>";


                        }
                        html += "</tbody>";

                        $(".ptTble").html(html);

                        if (output == "Yes") {
                            $('#tabs > li').removeClass('active');
                            $('#tab2').addClass('active');
                            $('.tab-content >.tab-pane').removeClass('active in');
                            $('#Section2').addClass('active in');
                        }
                        else {
                            $('#tabs > li').removeClass('active');
                            $('#tab1').addClass('active');
                            $('.tab-content >.tab-pane').removeClass('active in');
                            $('#Section1').addClass('active in');
                        }

                    }

                }
            });
        }

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function (evt, args) {
            UploadComplete();
        });


        function SaveMap() {
            var Mapobj = [];

            $(".ptTble tr").each(function () {
                var tableData = $(this).find('td');
                if (tableData.length > 0) {

                    var id = $(tableData[0]).html().trim();
                    var Myobj = {
                        ID: id,
                        Map_ID: $("#ddl" + id).val()
                    };
                    Mapobj.push(Myobj);
                }
            });

            var obj = {};
            obj.model = Mapobj;

            $.ajax({
                type: "POST",
                url: "import_consumption.aspx/SaveList",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(obj),
                dataType: "json",
                async: false,
                success: function (msg) {

                    //console.log(msg);

                    var result = msg.d;

                    if (result.val = "1") {
                        jAlert(result.text);
                    }
                    else {
                        jAlert(result.text);

                    }

                }
            });

        }

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    $("#dvAccordian").accordion();
                    $("#tabs").tabs();
                }
            });
        };


        $(document).ready(function () {

            //$('#sec2').hide();

            //$('.gonext').click(function () {
            //    $('#sec1').hide();
            //    $('#sec2').show();
            //});
            //$('.goprev').click(function () {
            //    $('#sec1').show();
            //    $('#sec2').hide();
            //})
            //UploadComplete();
        });

        var cancallback = true;
        function AllControlInitilize(s, e) {
            if (cancallback) {
                $('#sec2').hide();

                $('.gonext').click(function () {
                    $('#sec1').hide();
                    $('#sec2').show();
                });
                $('.goprev').click(function () {
                    $('#sec1').show();
                    $('#sec2').hide();
                })
                UploadComplete();
                cancallback = false;
            }
        }


    </script>
    <style>
        /* width */
        #scrollIt::-webkit-scrollbar {
            width: 10px;
        }

        /* Track */
        #scrollIt::-webkit-scrollbar-track {
            background: #f1f1f1;
        }

        /* Handle */
        #scrollIt::-webkit-scrollbar-thumb {
            background: #888;
        }

            /* Handle on hover */
            #scrollIt::-webkit-scrollbar-thumb:hover {
                background: #555;
            }

        .tabStyle .nav-tabs li > a {
            border-radius: 2px 2px 0 0;
            padding: 5px 15px;
            border-color: #e0e0e0;
        }

        .tabStyle .nav-tabs li.active a, .nav-tabs > li.active > a:hover, .nav-tabs > li.active > a:focus, .nav-tabs > li > a:hover {
            background: #0e8bcd;
            color: #fff;
            border-color: #0e8bcd;
        }

        .tabStyle .tab-content {
            margin: 0;
            border: 1px solid #dadada;
            border-top: none;
            padding: 5px 15px;
        }
    </style>

    <div class="panel-heading clearfix">
        <div class="panel-title clearfix">
            <div style="padding-right: 5px;">
                <h3 class="pull-left">Import - Warehouse Stock OUT
                </h3>
            </div>
        </div>
    </div>
    <div class="form_main">
        <asp:UpdatePanel ID="updpnlRefresh" runat="server" UpdateMode="Conditional">
            <ContentTemplate>

                <div class="">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="tab tabStyle" role="tabpanel">
                                <!-- Nav tabs -->
                                <ul class="nav nav-tabs" role="tablist" id="tabs">
                                    <li role="presentation" class="active" id="tab1"><a href="#Section1" aria-controls="home" role="tab" data-toggle="tab">Mapping</a></li>
                                    <li role="presentation" id="tab2"><a href="#Section2" aria-controls="profile" role="tab" data-toggle="tab">Import</a></li>

                                </ul>
                                <!-- Tab panes -->
                                <div class="tab-content tabs">
                                    <div role="tabpanel" class="tab-pane fade in active" id="Section1">
                                        <div class="row">
                                            <div class="col-md-3">
                                                <label><b>Choose File</b> </label>
                                                <div>
                                                    <asp:FileUpload ID="FileImport" CssClass="custom-file-input" runat="server" Width="100%" />
                                                </div>
                                            </div>
                                            <div class="col-md-3" style="padding-top: 28px; padding-left: 0;">
                                                <asp:Button ID="Button1" OnClick="Button1_Click" UseSubmitBehavior="false" CssClass="btn btn-success" runat="server" Text="Edit Mapping" />
                                            </div>
                                        </div>

                                        <div class="row" style="border: 1px solid #efefef; margin: 10px 0; padding: 0; display: inline-block;">
                                            <div class="">

                                                <div style="height: 215px; overflow-y: auto;" id="scrollIt">
                                                    <table class="ptTble">
                                                    </table>
                                                </div>
                                            </div>
                                        </div>
                                        <div>
                                            <button type="button" class="btn btn-success" onclick="SaveMap();">Save Mapping</button>
                                            <%-- <button type="button" class="btn btn-primary gonext" >Next  <i class="fa fa-arrow-right"></i></button>--%>
                                        </div>
                                    </div>
                                    <div role="tabpanel" class="tab-pane fade" id="Section2">
                                        <div class="">

                                            <div class="row">
                                                <div class="col-md-3">
                                                    <label><b>Choose File</b> </label>
                                                    <div>
                                                        <asp:FileUpload ID="FileUploadMain" CssClass="custom-file-input" runat="server" Width="100%" />
                                                    </div>
                                                </div>
                                                <div class="col-md-3" style="padding-top: 28px; padding-left: 0;">
                                                    <asp:Button ID="Button2" OnClick="Button2_Click" UseSubmitBehavior="false" CssClass="btn btn-success" runat="server" Text="Import" />

                                                </div>

                                            </div>

                                        </div>
                                        <div class="">
                                            <dxe:ASPxGridView ID="gridFinancer" ClientInstanceName="grid" Width="100%" AutoGenerateColumns="true"
                                                KeyFieldName="SL" runat="server" OnDataBinding="gridFinancer_DataBinding"
                                                SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" Settings-VerticalScrollableHeight="250">
                                                <%--DataSourceID="gridFinancerDataSource" --%>
                                                <SettingsSearchPanel Visible="True" Delay="5000" />
                                                <ClientSideEvents EndCallback="function(s, e) {
	                                                      EndCall(s.cpEND);
                                                    }" />
                                                <%--<Columns>
                                                        <dxe:GridViewDataTextColumn Visible="True" FieldName="TechnicianId" Caption="Technician ID">
                                                            <CellStyle CssClass="gridcellleft">
                                                            </CellStyle>
                                                            <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                                            <EditFormSettings Visible="False"></EditFormSettings>
                                                        </dxe:GridViewDataTextColumn>

                                                        <dxe:GridViewDataTextColumn Visible="True" FieldName="Technician_Name" Caption="Technician Name">
                                                            <CellStyle CssClass="gridcellleft">
                                                            </CellStyle>
                                                            <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                                            <EditFormSettings Visible="False"></EditFormSettings>
                                                        </dxe:GridViewDataTextColumn>


                                                        <dxe:GridViewDataTextColumn Visible="True" FieldName="BRANCH" Caption="Branch">
                                                            <CellStyle CssClass="gridcellleft">
                                                            </CellStyle>
                                                            <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                                            <EditFormSettings Visible="False"></EditFormSettings>
                                                        </dxe:GridViewDataTextColumn>

                                                        <dxe:GridViewDataTextColumn Visible="True" FieldName="EMPLOYEE_NAME" Caption="Employee Name">
                                                            <CellStyle CssClass="gridcellleft">
                                                            </CellStyle>
                                                            <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                                            <EditFormSettings Visible="False"></EditFormSettings>
                                                        </dxe:GridViewDataTextColumn>

                                                        <dxe:GridViewDataTextColumn Visible="True" FieldName="cnt_ContactNo" Caption="Contact No.">
                                                            <CellStyle CssClass="gridcellleft">
                                                            </CellStyle>
                                                            <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                                            <EditFormSettings Visible="False"></EditFormSettings>
                                                        </dxe:GridViewDataTextColumn>


                                                        <dxe:GridViewDataTextColumn Visible="True" FieldName="Statuss" Caption="Status">
                                                            <CellStyle CssClass="gridcellleft">
                                                            </CellStyle>
                                                            <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                                            <EditFormSettings Visible="False"></EditFormSettings>
                                                        </dxe:GridViewDataTextColumn>

                                                        <dxe:GridViewDataTextColumn Visible="True" FieldName="CREATE_ON" SortOrder="Descending" Caption="Created On" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy">
                                                            <CellStyle CssClass="gridcellleft"></CellStyle>
                                                            <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                                            <EditFormSettings Visible="False"></EditFormSettings>
                                                        </dxe:GridViewDataTextColumn>

                                                        <dxe:GridViewDataTextColumn Visible="True" FieldName="CREATE_BY" Caption="Created By">
                                                            <CellStyle CssClass="gridcellleft">
                                                            </CellStyle>
                                                            <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                                            <EditFormSettings Visible="False"></EditFormSettings>
                                                        </dxe:GridViewDataTextColumn>

                                                        <dxe:GridViewDataTextColumn Visible="True" FieldName="MODIFY_ON" Caption="Updated On" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy">
                                                            <CellStyle CssClass="gridcellleft">
                                                            </CellStyle>
                                                            <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                                            <EditFormSettings Visible="False"></EditFormSettings>
                                                        </dxe:GridViewDataTextColumn>

                                                        <dxe:GridViewDataTextColumn Visible="True" FieldName="MODIFY_BY" Caption="Updated By">
                                                            <CellStyle CssClass="gridcellleft">
                                                            </CellStyle>
                                                            <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                                            <EditFormSettings Visible="False"></EditFormSettings>
                                                        </dxe:GridViewDataTextColumn>

                                                        <dxe:GridViewDataTextColumn VisibleIndex="15" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" Width="0">


                                                            <DataItemTemplate>
                                                                <div class='floatedBtnArea'>
                                                                </div>
                                                            </DataItemTemplate>
                                                            <HeaderTemplate>Actions</HeaderTemplate>
                                                            <EditFormSettings Visible="False"></EditFormSettings>
                                                            <Settings AllowAutoFilterTextInputTimer="False" />
                                                        </dxe:GridViewDataTextColumn>

                                                    </Columns>--%>
                                                <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                                                <StylesEditors>
                                                    <ProgressBar Height="25px">
                                                    </ProgressBar>
                                                </StylesEditors>
                                                <SettingsSearchPanel Visible="True" />
                                                <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowFilterRow="true" ShowFilterRowMenu="True" />
                                                <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True" />
                                            </dxe:ASPxGridView>

                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>
                </div>


                <div id="sec1">
                </div>

                <div class="row" id="sec2">
                </div>

            </ContentTemplate>
            <Triggers>
                <%--<asp:PostBackTrigger ControlID="Button1" />--%>
                <asp:PostBackTrigger ControlID="Button1" />
                <asp:PostBackTrigger ControlID="Button2" />
            </Triggers>

        </asp:UpdatePanel>
    </div>
    <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
        <ClientSideEvents ControlsInitialized="AllControlInitilize" />
    </dxe:ASPxGlobalEvents>
</asp:Content>
