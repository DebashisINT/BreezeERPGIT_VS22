<%@ Page Title="Document" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="Product-Multipleimage.aspx.cs"
    Inherits="ERP.OMS.Management.Master.Product_Multipleimage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="../../../Scripts/Lightbox/css/lightbox.css" rel="stylesheet" />
    <script src="../../../Scripts/Lightbox/js/lightbox.js"></script>

    <style>
        .inImage {
            margin: 0;
            padding: 0;
        }

            .inImage li {
                list-style-type: none;
                display: inline-block;
                width: 32%;
                margin: 0 5px 5px 0;
            }

        #mask {
            position: fixed;
            left: 0;
            top: 0;
            bottom: 0;
            right: 0;
            margin: auto;
            visibility: hidden;
            z-index: -2;
            background: #000;
            background: rgba(0,0,0,0.8);
            overflow: hidden;
            opacity: 0;
            transition: all .5s ease-in-out;
        }

            #mask.showing {
                opacity: 1;
                z-index: 9000;
                visibility: visible;
                overflow: auto;
                transition: all .5s ease-in-out;
            }

        #boxes {
            display: table;
            width: 100%;
            height: 100%;
        }

        .window {
            max-width: 780px;
            z-index: 9999;
            padding: 20px;
            border-radius: 15px;
            text-align: center;
            margin: auto;
            background-color: #ffffff;
            font-family: 'Segoe UI Light', sans-serif;
            font-size: 15pt;
        }

            .window img {
                width: 100%;
                height: auto;
            }

        .inner {
            display: table-cell;
            vertical-align: middle;
        }

        #popupfoot {
            font-size: 16pt;
        }

        .showImage {
            margin: 0 0 3em;
            display: table;
            text-align: center;
        }

            .showImage img {
                display: block;
            }


        #maskmap {
            position: fixed;
            left: 0;
            top: 0;
            bottom: 0;
            right: 0;
            margin: auto;
            visibility: hidden;
            z-index: -2;
            background: #000;
            background: rgba(0,0,0,0.8);
            overflow: hidden;
            opacity: 0;
            transition: all .5s ease-in-out;
        }

            #maskmap.showing {
                opacity: 1;
                z-index: 9000;
                visibility: visible;
                overflow: auto;
                transition: all .5s ease-in-out;
            }

        #boxesmap {
            display: table;
            width: 100%;
            height: 100%;
        }

        #popupfootmap {
            font-size: 16pt;
        }
    </style>

    <script>
        $(function () {

            var prodcode = $("#hdnprodID").val();

            //$.ajax({
            //    type: "POST",
            //    url: "Product-Multipleimage.aspx/Imagepopulate",
            //    data: "{'prodId':'" + prodcode + "'}",
            //    contentType: "application/json; charset=utf-8",
            //    dataType: "json",
            //    success: function (msg) {

            //        var mimages = msg.d;
            //        var imagestr = '<ul class="inImage">';

            //        $.each(mimages, function () {

            //            //  alert(this.prod_image);
            //            imagestr = imagestr + '<li> <a href="' + this.prod_image + '" data-lightbox="image-1" data-title="My caption"><img alt="" src="' + this.prod_image + '" style="width: 100%; max-height: 150px" /></a><span><a onclick="DeleteProduct(' + this.product_id + ');"></a></span></li>';

            //        });

            //        imagestr = imagestr + '</ul>';
            //        //  alert(imagestr);
            //        /// $("#divimage").html(imagestr);
            //    }

            //});

            //   alert(prodcode);

            gridProductimage.PerformCallback('ProductImagePopulate~' + prodcode);


            $('.window .close').click(function (e) {
                //Cancel the link behavior
                e.preventDefault();
                $('#mask').removeClass('showing');

            });

        });



        function Productimagepopulate(prodcode) {

            //$.ajax({
            //    type: "POST",
            //    url: "Product-Multipleimage.aspx/Imagepopulate",
            //    data: "{'prodId':'" + prodcode + "'}",
            //    contentType: "application/json; charset=utf-8",
            //    dataType: "json",
            //    success: function (msg) {

            //        var mimages = msg.d;
            //        var imagestr = '<ul class="inImage">';

            //        $.each(mimages, function () {

            //            //  alert(this.prod_image);
            //            imagestr = imagestr + '<li> <a href="' + this.prod_image + '" data-lightbox="image-1" data-title="My caption"><img alt="" src="' + this.prod_image + '" style="width: 100%; max-height: 150px" /></a><span><a onclick="DeleteProduct(' + this.product_id + ')"></a></span></li>';

            //        });

            //        imagestr = imagestr + '</ul>';
            //        // alert(imagestr);
            //        $("#divimage").html(imagestr);


            //    }

            //});

            gridProductimage.PerformCallback('ProductImagePopulate~' + prodcode);

        }



        function fun_ProductMultipageimage() {

            var res = "";

            //for (var i = 0; i < $("#file_product").get(0).files.length; i++) {

            //    res += $("#file_product").get(0).files[i].name + "<br />";

            //}

            var fileUpload = $("#file_product").get(0);
            var files = fileUpload.files;

            //if (files[0].size <= 2048000) {
            // Create FormData object

            var fileData = new FormData();

            // Looping over all files and add it to FormData object
            for (var i = 0; i < files.length; i++) {
                fileData.append(files[i].name, files[i]);
                //  alert(files[i].name);
            }

            var prodcode = $("#hdnprodID").val();

            $.ajax({
                type: "POST",
                url: "Product-Multipleimage.aspx/ImageUpload",
                data: "{'prodId':'" + prodcode + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {

                    var mimages = msg.d;
                    var imagestr = '<ul class="inImage">';

                    gridProductimage.PerformCallback('ProductImagePopulate~' + prodcode);

                }

            });
        }


        function DeleteProduct(imageID) {

            // alert(prodid);
            var prodcode = $("#hdnprodID").val();

            $.ajax({
                type: "POST",
                url: "Product-Multipleimage.aspx/DeleteImage",
                data: "{'ImageID':'" + imageID + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {

                    gridProductimage.PerformCallback('ProductImagePopulate~' + prodcode);

                }

            })

        }



        function SetMainImge(imageID) {

            var prodcode = $("#hdnprodID").val();

            $.ajax({
                type: "POST",
                url: "Product-Multipleimage.aspx/Mainstatus",
                data: "{'ImageID':'" + imageID + "','prodId':'" + prodcode + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",

                success: function (msg) {

                    gridProductimage.PerformCallback('ProductImagePopulate~' + prodcode);
                }

            })

        }


        function ChangeActivestatus(imageID) {

            var prodcode = $("#hdnprodID").val();
            $.ajax({
                type: "POST",
                url: "Product-Multipleimage.aspx/ChangeActivity",
                data: "{ImageID:'" + imageID + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {

                    var mimages = msg.d;
                    gridProductimage.PerformCallback('ProductImagePopulate~' + prodcode);
                }

            })

        }




        function ShowImage(imgfile) {

            $('.image').attr({
                'src': imgfile
            });
            $('#mask').addClass('showing');
        }




    </script>
    <style>

        .piTabl>tbody>tr>td {
            vertical-align:middle;
            padding-right:15px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <div class="panel-heading">

        <div class="panel-title">
            <label id="lblproductname" runat="server"></label>
            <input type="hidden" id="hdnprodID" runat="server" />
            <div class="crossBtn"><a href="/OMS/management/store/Master/sProducts.aspx"><i class="fa fa-times"></i></a></div>
        </div>
    </div>


    <div class="form_main">
        <table class="piTabl">
            <tr>
                <td><label>Product  Image</label></td>
                <td>
                    
                    <asp:FileUpload ID="file_product" runat="server" AllowMultiple="true" />
                </td>
                <td>
                    <asp:CheckBox ID="chkmainimage" runat="server" />
                    <label>Select Main Image</label>
                </td>
                <td>
                    <asp:CheckBox ID="chkActivestatus" runat="server" />
                    <label>Active Status Set</label>
                </td>
                <td>
                    <asp:Button ID="UploadButton" runat="server" CssClass="btn btn-primary" OnClick="UploadButton_Click" Text="Upload File" />
                </td>
            </tr>
        </table>
        <div>    
            <asp:Label ID="FileUploadedList" runat="server" Visible="false" />


        </div>


        <div id="divimage">
        </div>


    </div>

    <br />

    <div>


        <dxe:ASPxGridView ID="gridProductimage" ClientInstanceName="gridProductimage" runat="server" AutoGenerateColumns="False"
            KeyFieldName="ID" Width="100%" OnCustomCallback="gridProductImage_CustomCallback" OnDataBinding="gridProductImage_DataBinding" SettingsBehavior-AllowFocusedRow="true">


            <Columns>


                <%--  <dxe:GridViewDataTextColumn FieldName="Imagename" VisibleIndex="1" Caption="Image Name" SortOrder="Descending">
                </dxe:GridViewDataTextColumn>--%>



                <dxe:GridViewDataTextColumn VisibleIndex="1" Caption="Image" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center">

                    <DataItemTemplate>
                 <%--       <dxe:ASPxImage ID="image1" runat="server" OnInit="image1_Init" ImageUrl="~/Images/blue.png"></dxe:ASPxImage>--%>
                        <dxe:ASPxImage ID="image2" runat="server" OnInit="image2_Init" Width="60px" Height="50px"></dxe:ASPxImage>
                    </DataItemTemplate>

                </dxe:GridViewDataTextColumn>




                <dxe:GridViewDataTextColumn FieldName="Mainimage" VisibleIndex="2" Caption="Main Image" SortOrder="Descending">
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn FieldName="Activestatus" VisibleIndex="3" Caption="Active Status" SortOrder="Descending">
                </dxe:GridViewDataTextColumn>


                <dxe:GridViewDataTextColumn VisibleIndex="4" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center">
                    <DataItemTemplate>
                        <a href="javascript:void(0);" onclick="ShowImage('<%#Eval("prod_image") %>')" title="Reset Online Status" class="pad">
                            <img src="/assests/images/eye.png" /></a>
                    </DataItemTemplate>
                    <HeaderTemplate>View Image</HeaderTemplate>
                    <EditFormSettings Visible="False" />
                </dxe:GridViewDataTextColumn>



                <dxe:GridViewDataTextColumn VisibleIndex="5" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center">
                    <DataItemTemplate>
                        <a href="javascript:void(0);" onclick="SetMainImge('<%#Eval("ID") %>')" title="Set Main Imge" class="pad">
                            <img src="/assests/images/picture.png" /></a>
                    </DataItemTemplate>
                    <HeaderTemplate>Set as Main Image</HeaderTemplate>
                    <EditFormSettings Visible="False" />
                </dxe:GridViewDataTextColumn>



                <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="Activestatus" Caption="Change Active Status">
                    <CellStyle HorizontalAlign="Left">
                    </CellStyle>
                    <HeaderStyle HorizontalAlign="Center" />
                    <DataItemTemplate>
                        <a href="javascript:void(0)" onclick="ChangeActivestatus('<%#Eval("ID") %>')" class="pad">
                            <%#Eval("Activestatus")%>
                        </a>
                    </DataItemTemplate>
                    <EditFormSettings Visible="False" />
                </dxe:GridViewDataTextColumn>


                <dxe:GridViewDataTextColumn VisibleIndex="7" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center">
                    <DataItemTemplate>
                        <a href="javascript:void(0);" onclick="DeleteProduct('<%#Eval("ID") %>')" title="delete Image" class="pad">
                            <img src="/assests/images/delete.png" /></a>
                    </DataItemTemplate>
                    <HeaderTemplate>Delete Image</HeaderTemplate>
                    <EditFormSettings Visible="False" />
                </dxe:GridViewDataTextColumn>




            </Columns>


            <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" />
            <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
            <SettingsEditing Mode="EditForm" />
            <SettingsContextMenu Enabled="true" />
            <SettingsBehavior AutoExpandAllGroups="true" ColumnResizeMode="Control" />
            <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" />

            <SettingsPager PageSize="10">
                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
            </SettingsPager>
            <Settings ShowFilterRow="True" ShowStatusBar="Visible" UseFixedTableLayout="true" />




        </dxe:ASPxGridView>



    </div>

    <div id="mask">
        <div id="boxes">
            <div class="inner">
                <div id="dialog" class="window">
                    <a href="#" class="close">CLOSE</a>
                    <div id="popupfoot">


                        <img src="#" class="image" alt="Loading..."></img>


                    </div>
                </div>
            </div>
        </div>
    </div>


</asp:Content>
