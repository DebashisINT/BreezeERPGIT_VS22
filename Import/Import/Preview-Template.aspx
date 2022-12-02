<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Preview-Template.aspx.cs" Inherits="Import.Import.Preview_Template" %>

<!DOCTYPE html>

<style>
@page { size: auto;  margin: 0mm; }
</style>


<script language="javascript">

    function printdiv(printpage) {
        //var headstr = "<html><head><title></title></head><body>";
        //var footstr = "</body>";
        //var newstr = document.all.item(printpage).innerHTML;
        //var oldstr = document.body.innerHTML;
        //document.body.innerHTML = headstr + newstr + footstr;
        //window.print();
        //document.body.innerHTML = oldstr;
        //return false;


        var printContents = document.getElementById(printpage).innerHTML;
        var originalContents = document.body.innerHTML;
        document.getElementById('lblheading').style.display = 'none';
        //document.getElementById('footer').style.display = 'none';
        document.body.innerHTML = printContents;

        window.print();


        document.body.innerHTML = originalContents;


    }




</script>


        <input name="b_print" type="button" style="float:right;"   onClick="printdiv('div_print');" value=" Print ">
             <br />
        <div id="div_print">

            <div id="lblheading" runat="server"></div>

            <div id="lblbody" runat="server"></div>

        </div>



