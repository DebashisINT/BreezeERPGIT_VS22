<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="blankPage.aspx.cs" Inherits="ERP.OMS.Management.Activities.blankPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .bod-table {
            width:100%;
            border-radius:5px;
            overflow:hidden;
        }
        .bod-table td {
            padding:5px;
            background: #c2d8e6;
            font-weight: 500;
        }
        .bod-table.none td {
            background: none;
        }
        .bac {
            background: #7c9cb1;
 
            margin: 10px 0;
            padding: 10px 15px;
            border-radius: 5px;
        }
        .greyd {
            background: #ececec;
            margin: 10px 0;
            padding: 0px 15px;
            border-radius: 5px;
        }
        .newLbl .lblHolder table tr:first-child td {
            background: #2bb1bf;
        }
        table.pad td {
            padding:4px 10px;
        }
        section.rds {
            margin-top: 25px;
            border: 1px solid #ccc;
            padding:3px 15px;
        }
        span.fieldsettype {
            background: #1671b7;
            padding: 8px 10px;
            color: #fff;
            position: relative;
            top: -10px;
            z-index:5;
        }
        span.fieldsettype::before{
            content: "";
            border-left: 9px solid transparent;
            border-right: 9px solid transparent;
            border-bottom: 13px solid #184d75;
            position: absolute;
            right: -9px;
            z-index
                : -1;
        }
        .table-duplicate {
            width:100%;
        }
        .table-duplicate td>label {
            background:#215b61;
            color:#fff;
            
            padding:2px 5px;
        }
        .horizontallblHolder {
            height:auto;
            border:1px solid #12a79b;
            border-radius:3px;
            overflow:hidden;
        }
        .horizontallblHolder>table>tbody>tr>td {
            padding:8px 10px;
            background: #ffffff;
            background: -moz-linear-gradient(top, #ffffff 0%, #f3f3f3 50%, #ededed 51%, #ffffff 100%);
            background: -webkit-linear-gradient(top, #ffffff 0%,#f3f3f3 50%,#ededed 51%,#ffffff 100%);
            background: linear-gradient(to bottom, #ffffff 0%,#f3f3f3 50%,#ededed 51%,#ffffff 100%);
            filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#ffffff', endColorstr='#ffffff',GradientType=0 );
        }
        .horizontallblHolder>table>tbody>tr>td:first-child {
                background: #12a79b;
                color: #fff;   
        }
         .horizontallblHolder>table>tbody>tr>td:last-child{
            font-weight: 500;
            text-transform: uppercase;
            color: #121212;
         }
    </style>
 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-title clearfix">
        <h3 class="pull-left">
            Add Sale Invoice ( Finance )
            <%--<label>Add Proforma Invoice/ Quotation</label>--%>
        </h3>
        <div class="pull-right reverse wrapHolder content horizontal-images" style="display: block;">
            <div class="Top clearfix">
                <ul>
                    <li>
                        <div class="lblHolder" id="">
                            <table>
                                <tr>
                                    <td>Selected Branch</td>
                                </tr>
                                <tr>
                                    <td>
                                        Main branch
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </li>
                    <li>
                        <div class="lblHolder" id="">
                            <table>
                                <tr>
                                    <td>Selected Product</td>
                                </tr>
                                <tr>
                                    <td>
                                        LG 32" TV 
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </li>
                    <li>
                        <div class="lblHolder" id="divPacking" style="display: none;">
                            <table>
                                <tr>
                                    <td>Available Stock</td>
                                </tr>
                                <tr>
                                    <td>
                                        0.00
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </li>
                </ul>
                
            </div>
        </div>
        <div id="divcross" class="crossBtn" visible="false"><a href="PosSalesInvoiceList.aspx"><i class="fa fa-times"></i></a></div>

    </div>
    <div class="form_main" >
         <div class="row">
             <div class="col-md-12">
                 <table class="bod-table">
                     <tr>
                         <td class="hd">Numbering Scheme</td>
                         <td><select class="form-control"><option>Select</option></select></td>
                         <td class="hd">Invoice No</td>
                         <td><input type="text" /></td>
                         <td class="hd">Date</td>
                         <td><input type="text" /></td>
                         <td class="hd">Delivery Type</td>
                         <td><input type="text" /></td>
                         <td class="hd">Customer</td>
                         <td><input type="text" /></td>
                     </tr>
                     <tr>
                         <td class="hd">Reference</td>
                         <td colspan=""><input type="text" /></td>
                         <td class="hd">Challan No</td>
                         <td><input type="text" /></td>
                         <td class="hd">Challan Date</td>
                         <td><input type="text" /></td>
                         <td class="hd">Select GST</td>
                         <td><input type="text" /></td>
                         <td>
                             <select class="form-control">
                                 <option>Select Barcode</option>
                                 <option>Select Model</option>
                                 <option>Select Serial</option>
                             </select>
                         </td>
                         <td>
                             <div class="input-group">
                                 <input type="text" class="form-control" placeholder="Username" aria-describedby="basic-addon1" >
                                 <span class="input-group-addon btn-primary" style="padding: 5px;"><i class="fa fa-plus-circle" aria-hidden="true"></i></span>
                            </div>
                         </td>
                     </tr>
                     
                 </table>
   <div class="bac ">
                     <table class="bod-table none">
                     <tr>
                         <td class="hd">Financer</td>
                         <td ><select class="form-control"><option>Select</option></select></td>
                         <td class="hd">Exec .Name</td>
                         <td ><input type="text" /></td>
                         <td class="hd" >EMI Details</td>
                         <td><input type="text" /></td>
                         <td class="hd">Finance Amt</td>
                         <td colspan=""><input type="text" /></td>
                     </tr>
                     <tr>
                         
                         <td class="hd">DBD</td>
                         <td><input type="text" /></td>
                         <td class="hd">DBD %</td>
                         <td><input type="text" /></td>
                         <td class="hd">Proc .Fee</td>
                         <td><input type="text" /></td>
                         <td class="hd">SF Code</td>
                         <td><input type="text" /></td>
                     </tr>
                     
                 </table>
                 </div>
                 <div style="margin-top:8px"><img src="../../../assests/images/JournalEntry.jpeg" /></div>
              
                 <div class="greyd">
                     <table class="bod-table none">
                     <tr>
                         <td class="hd">Old unit</td>
                         <td ><select class="form-control"><option>Select</option></select></td>
                         <td class="hd">Unit Value</td>
                         <td ><input type="text" /></td>
                         <td class="hd" >Remarks</td>
                         <td><input type="text" /></td>
                         <td class="hd">Advance Receipt No</td>
                         <td colspan=""><input type="text" /></td>
                     </tr>
                 </table>
                 </div>
                 <div class="content reverse horizontal-images clearfix" style="width:100%;margin-right: 0;padding: 8px;height:auto;border-top: 1px solid #ccc;border-bottom: 1px solid #ccc;border-radius: 0;">
                        <ul>
                            <li>
                                <div class="horizontallblHolder" id="">
                                    <table>
                                        <tbody><tr>
                                            <td>Taxable Amt</td>
                                            <td>
                                                14450.00
                                            </td>
                                        </tr>
                                       
                                    </tbody></table>
                                </div>
                            </li>
                            <li>
                                <div class="horizontallblHolder" id="">
                                    <table>
                                        <tbody><tr>
                                            <td>Tax Amt</td>
                                            <td>
                                                152200.00
                                            </td>
                                        </tr>
                                       
                                    </tbody></table>
                                </div>
                            </li>
                            <li>
                                <div class="horizontallblHolder" id="" >
                                    <table>
                                        <tbody><tr>
                                            <td>Amount with Tax</td>
                                            <td>
                                               152200.00
                                            </td>
                                        </tr>
                                        
                                    </tbody></table>
                                </div>
                            </li>
                            <li>
                                <div class="horizontallblHolder" id="" >
                                    <table>
                                        <tbody><tr>
                                            <td>Less old unit value</td>
                                             <td>
                                               152200.00
                                            </td>
                                        </tr>
                                        
                                    </tbody></table>
                                </div>
                            </li>
                            <li>
                                <div class="horizontallblHolder" id="" >
                                    <table>
                                        <tbody><tr>
                                            <td>Less Advance</td>
                                            <td>
                                                152200.00
                                            </td>
                                        </tr>
                                        
                                    </tbody></table>
                                </div>
                            </li>
                            <li>
                                <div class="horizontallblHolder" id="" >
                                    <table>
                                        <tbody><tr>
                                            <td>Invoice Value</td>
                                            <td>
                                                152200.00
                                            </td>
                                        </tr>
                                       
                                    </tbody></table>
                                </div>
                            </li>
                        </ul>
                
                    </div>
                 <section class="rds">
                     <div class="clearfix">
                         <span class="fieldsettype">Cash Invoice Payment Details</span>
                         <table class="pull-right pad">
                             <tr>
                                 <td><strong>Select cash Amt</strong></td>
                                 <td ><select class="form-control"><option>Select</option></select></td>
                                 <td class="hd"><strong>Enter cash Amt</strong></td>
                                 <td ><input type="text" /></td>
                             </tr>
                         </table>
                     </div>
                     <table class="table-duplicate">
                         <tr>
                             <td>
                                 <label>Payment Type</label>
                                 <select class="form-control"><option>Select</option></select>
                             </td>
                             <td>
                                 <label>Select Card Type</label>
                                 <select class="form-control"><option>Select</option></select>
                             </td>
                             <td>
                                 <label>Enter Card No</label>
                                 <select class="form-control"><option>Select</option></select>
                             </td>
                             <td>
                                 <label>Authorization No</label>
                                 <select class="form-control"><option>Select</option></select>
                             </td>
                             <td>
                                 <label>Remarks ( if any )</label>
                                 <select class="form-control"><option>Select</option></select>
                             </td>
                             <td>
                                 <label>Enter Amount</label>
                                 <select class="form-control"><option>Select</option></select>
                             </td>
                             <td class="action">
                                 <span style="margin-top: 18px;margin-left: 10px;display:inline-block;margin-right:5px;"><img src="/assests/images/add.png" /></span>
                                 <span><img src="/assests/images/crs.png" /></span>
                             </td>
                         </tr>
                         <%--<tr>
                             <td>
                                 <label>Payment Type</label>
                                 <select class="form-control"><option>Select</option></select>
                             </td>
                             <td>
                                 <label>Enter cheque No </label>
                                 <select class="form-control"><option>Select</option></select>
                             </td>
                             <td>
                                 <label>Enter Date</label>
                                 <select class="form-control"><option>Select</option></select>
                             </td>
                             <td>
                                 <label>Drawee Bank</label>
                                 <select class="form-control"><option>Select</option></select>
                             </td>
                             <td>
                                 <label>Remarks ( if any )</label>
                                 <select class="form-control"><option>Select</option></select>
                             </td>
                             <td>
                                 <label>Enter Amount</label>
                                 <select class="form-control"><option>Select</option></select>
                             </td>
                             <td class="action">
                                 <span style="margin-top: 18px;margin-left: 10px;display:inline-block;margin-right:5px;"><img src="/assests/images/add.png" /></span>
                                 <span><img src="/assests/images/crs.png" /></span>
                             </td>
                         </tr>
                         <tr>
                             <td>
                                 <label>Payment Type</label>
                                 <select class="form-control"><option>Select</option></select>
                             </td>
                             <td colspan="3">
                                 <label>Enter Details of the Coupon</label>
                                 <select class="form-control"><option>Select</option></select>
                             </td>
                             
                             <td>
                                 <label>Remarks ( if any )</label>
                                 <select class="form-control"><option>Select</option></select>
                             </td>
                             <td>
                                 <label>Enter Amount</label>
                                 <select class="form-control"><option>Select</option></select>
                             </td>
                             <td class="action">
                                 <span style="margin-top: 18px;margin-left: 10px;display:inline-block;margin-right:5px;"><img src="/assests/images/add.png" /></span>
                                 <span><img src="/assests/images/crs.png" /></span>
                             </td>
                         </tr>--%>
                     </table>
                 </section>
                 <div>
                     <button class="btn btn-primary">Save</button>
                 </div>
             </div>
         </div>  
    </div>
</asp:Content>
