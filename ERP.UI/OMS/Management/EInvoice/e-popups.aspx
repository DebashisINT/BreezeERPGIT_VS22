<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="e-popups.aspx.cs" Inherits="ERP.OMS.Management.EInvoice.e_popups" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
<style>
       body{
          background-color: #f5f5f5;
        }
        .fileuploader {
            position: relative;
            width: 60%;
            margin: auto;
            height: 400px;
            border: 4px dashed #ddd;
            background: #f6f6f6;
            margin-top: 85px;
        }
        .fileuploader #upload-label{
          background: rgba(231, 97, 92, 0);
          color: #fff;
          position: absolute;
          height: 115px;
          top: 20%;
          left: 0;
          right: 0;
          margin-right: auto;
          margin-left: auto;
          min-width: 20%;
          text-align: center;
          cursor: pointer;
        }
        .fileuploader.active{
          background: #fff;
        }
        .fileuploader.active #upload-label{
          background: #fff;
          color: #e7615c;
        }

        .fileuploader #upload-label i:hover {
            color: #444;
            font-size: 9.4rem;
            -webkit-transition: width 2s;
        }

        .fileuploader #upload-label span.title{
          font-size: 1em;
          font-weight: bold;
          display: block;
        }

        span.tittle {
            position: relative;
            top: 222px;
            color: #bdbdbd;
        }

        .fileuploader #upload-label i{
          text-align: center;
          display: block;
          color: #e7615c;
          height: 115px;
          font-size: 9.5rem;
          position: absolute;
          top: -12px;
          left: 0;
          right: 0;
          margin-right: auto;
          margin-left: auto;
        }
        /** Preview of collections of uploaded documents **/
        .preview-container{
          position: relative;
          bottom: 0px;
          width: 35%;
          margin: auto;
          top: 25px;
          visibility: hidden;
        }
        .preview-container #previews{
          max-height: 400px;
          overflow: auto; 
        }
        .preview-container #previews .zdrop-info{
          width: 88%;
          margin-right: 2%;
        }
        .preview-container #previews.collection{
          margin: 0;
          box-shadow: none;
        }

        .preview-container #previews.collection .collection-item {
            background-color: #e0e0e0;
        }

        .preview-container #previews.collection .actions a{
          width: 1.5em;
          height: 1.5em;
          line-height: 1;
        }
        .preview-container #previews.collection .actions a i{
          font-size: 1em;
          line-height: 1.6;
        }
        .preview-container #previews.collection .dz-error-message{
          font-size: 0.8em;
          margin-top: -12px;
          color: #F44336;
        }



        /*media querie*/

        @media only screen and (max-width: 601px){
          .fileuploader {
            width: 100%;
          }

         .preview-container {
            width: 100%;
          }
        }
   </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    
    <link href="../../../assests/pluggins/fileuploads/css/dropify.min.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css?family=Poppins:400,500,700&display=swap" rel="stylesheet">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.16.1/umd/popper.min.js" ></script>
    <script src="../../../assests/pluggins/fileuploads/js/dropify.min.js"></script>
    
    <style>
        .cap {
            font-size:34px;
            color:red;
        }
        .dropify-wrapper{
            border: 2px dashed #E5E5E5;
        }
        .ppTabl {
            margin:0 auto;
            
        }
        .ppTabl>tbody>tr>td:first-child{
            text-align:right;
            padding-right:15px;
        }
        .ppTabl>tbody>tr>td {
            padding:8px 0;
            font-size:15px;
            text-align:left;
        }
        .empht {
            font-size: 18px;
            color: #d68f0d;
            margin: 15px;
        }
        .poppins {
            font-family: 'Poppins', sans-serif;
        }
        .bcShad {
            position: fixed;
            width: 100%;
            background: rgba(0,0,0,0.75);
            height: 100%;
            left: 0;
            z-index: 10;
            top: 0;
        }
        .popupSuc {
            position: absolute;
            z-index: 12;
            background: #fff;
            padding: 20px;
            min-width: 650px;
            left: 50%;
            top: 50%;
            transform: translate(-50%, -50%);
        }
        .bInfoIt{
            text-align: center;
            border-top: 1px solid #ccc;
            border-bottom: 1px solid #ccc;
            padding: 12px;
        }
        .bInfoIt p {
            margin:0;
        }
    </style>
    <!-- Button trigger modal -->
<button type="button" class="btn btn-primary" data-toggle="modal" data-target="#exampleModal" data-backdrop="static" data-keyboard="false">
  Upload GSTN
</button>
    <p class="cap">fhdfh dfh shdfh dh</p>
    
    <script type="text/javascript">
        $(document).ready(function () {
            $('.dropify').dropify({
                messages: {
                    'default': 'Upload to GSTN Server (Drag and drop a file here or click)',
                    'replace': 'Drag and drop or click to replace',
                    'remove': 'Remove',
                    'error': 'Ooops, something wrong appended.'
                },
                error: {
                    'fileSize': 'The file size is too big (1M max).'
                }
            });
        })
        </script>               
 <div class="bcShad"></div> 
 <div class="popupSuc">

     <div class="bInfoIt">
         <p>Document has been uploaded successfully to GSTN server</p>
         <p>IRN : 845216dffdgh4551dfh15544sdg5574dsg446sdg6464464ddf8dgsssdgdgg5665</p>
     </div>
     <table class="ppTabl ">
        <tr>
            <td>Invoice Number :</td>
            <td><b>Snhhi / hjkj / 25456</b></td>
        </tr>
        <tr>
            <td>Date : </td>
            <td><b>02 - Mar - 2020</b> </td>
        </tr>
        <tr>
            <td>Customer : </td>
            <td><b>C001 - Albudh hdggh hjdhj</b></td>
        </tr>
        <tr>
            <td>Amount : </td>
            <td><b>INR 34568.00</b></td>
        </tr>
    </table>
 </div>              

<!-- Modal -->
<div class="modal fade pmsModal w40" id="exampleModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
  <div class="modal-dialog" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="exampleModalLabel">Upload GSTIN</h5>
        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true">&times;</span>
        </button>
      </div>
      <div class="modal-body poppins">
        <div class="text-center">
            <img src="../../../assests/images/invoiceII.png" style="width: 70px;margin-bottom: 15px;" />
        </div>
        <div>
            <input type="file" class="dropify" data-height="100" />
        </div>
          <div class="text-center pTop10">
              <table class="ppTabl ">
                  <tr>
                      <td>Invoice Number :</td>
                      <td><b>Snhhi / hjkj / 25456</b></td>
                  </tr>
                  <tr>
                      <td>Date : </td>
                      <td><b>02 - Mar - 2020</b> </td>
                  </tr>
                  <tr>
                      <td>Customer : </td>
                      <td><b>C001 - Albudh hdggh hjdhj</b></td>
                  </tr>
                  <tr>
                      <td>Amount : </td>
                      <td><b>INR 34568.00</b></td>
                  </tr>
              </table>
              <div class="empht">Do you want to procced with upload ?</div>
              <div>
                  
              </div>
          </div>
      </div>
      <div class="modal-footer">
          <button class="btn btn-info" type="button">Upload</button>
          <button class="btn btn-danger" data-dismiss="modal">Cancel</button>
      </div>
    </div>
  </div>
</div>
</asp:Content>
