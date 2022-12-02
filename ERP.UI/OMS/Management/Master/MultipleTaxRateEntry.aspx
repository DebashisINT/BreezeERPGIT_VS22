<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="MultipleTaxRateEntry.aspx.cs" Inherits="ERP.OMS.Management.Master.MultipleTaxRateEntry" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Activities/CSS/SearchPopup.css" rel="stylesheet" />
    <script src="../Activities/JS/SearchPopup.js"></script>
    <script type="text/javascript">
        function HSNButnClick(s, e) {
            $('#HSNSACModel').modal('show');
        }
        function CloseHSNSACModel() {
            $('#HSNSACModel').modal('hide');
        }


        function HSNKeyDown(e) {

            var OtherDetails = {}
            OtherDetails.SearchKey = $("#txtHsnSearch").val();

            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var HeaderCaption = [];
                HeaderCaption.push("HSN/SAC Code");
                HeaderCaption.push("Description");
                HeaderCaption.push("Type");
                if ($("#txtHsnSearch").val() != '') {
                    callonServer("../Master/MultipleTaxRateEntry.aspx/GetHsnSac", OtherDetails, "HsnSacTable", HeaderCaption, "HsnIndex", "SetHsnSac");
                }
            }
            else if (e.code == "ArrowDown") {
                if ($("input[customerindex=0]"))
                    $("input[customerindex=0]").focus();
            }

        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div>
       <div class="cnt" id="dvAll">
                                    <table class="padding">
                                        <tr>
                                            <td>
                                                <div>Hsn Code</div>
                                            </td>
                                            <td>
                                                <div>
                                                    <dxe:ASPxButtonEdit ID="txtHSNSAC" runat="server" ReadOnly="true" ClientInstanceName="ctxtHSNSAC">
                                                             
                                                             <Buttons>
                                                                 <dxe:EditButton>
                                                                 </dxe:EditButton>
                                                                 
                                                             </Buttons>
                                                             <ClientSideEvents ButtonClick="function(s,e){HSNButnClick();}" KeyDown="function(s,e){HSNKeyDown(s,e);}"/>
                                                         </dxe:ASPxButtonEdit>
                                                </div>
                                            </td>
                                        </tr>


                                        <%--                                    <tr>
                                        <td><div>Parent Id</div></td>
                                        <td>
                                            <div>
                                                <dxe:ASPxComboBox ID="cmbParentId" ClientInstanceName="ccmbParentId" runat="server" Width="100%"
                                                    OnCallback="cmbParentId_Callback">
                                                    <ValidationSettings RequiredField-IsRequired="true" Display="None"></ValidationSettings>
                                                </dxe:ASPxComboBox>
                                            </div>
                                        </td>
                                    </tr>--%>


                                    </table>
                                </div>


    </div>



    <div class="modal fade" id="HSNSACModel" role="dialog" data-backdrop="static"
        data-keyboard="false">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" onclick="CloseHSNSACModel();">&times;</button>
                    <h4 class="modal-title">HSN/SAC Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="HSNKeyDown(event)" id="txtHsnSearch" autofocus width="100%" placeholder="Search By Sub Account Name or Code" />

                    <div id="HsnSacTable">
                        <table border='1' width="100%">
                            <tr class="HeaderStyle">
                                <th>HSN/SAC Code</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" onclick="CloseHSNSACModel();">Close</button>
                </div>
            </div>

        </div>
    </div>

</asp:Content>
