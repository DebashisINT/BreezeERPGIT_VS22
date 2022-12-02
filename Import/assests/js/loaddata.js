document.write("<div id='prepage' style='position:absolute; font-family:arial; font-size:30; left:250px; top:150px;= background-color:white; layer-background-color:white; height:80; width:150;'>")
document.write("<table width='200' height='35' border='1' cellpadding='0' cellspacing='0' bordercolor='#C0D6E4'>")
document.write("  <tr><td><table><tr>")
document.write("    <td height='25' align='center' bgcolor='#FFFFFF'>")
document.write("      <img src='/assests/images/progress.gif' width='18' height='18'></td>")
//document.write("  </tr>")
//document.write("  <tr>")
document.write("    <td height='10' width='100%' align='center' bgcolor='#FFFFFF'><font size='2' face='Tahoma'>")
document.write("	<strong align='center'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Please Wait..</strong></font></td>")
document.write(" </td></table></tr> </tr>")
document.write("</table>")
document.write("</div>")
//Begin PRELOADER ************************************************** *****

function clearPreloadPage() 
{ //DOM
    if (document.getElementById)
    {
        document.getElementById('prepage').style.visibility='hidden';
    }
    else
    {
        if (document.layers)
        { //NS4
            document.prepage.visibility = 'hidden';
        }
        else 
        { //IE4
            document.all.prepage.style.visibility = 'hidden';
        }
    }
}

function HideLoading() 
{ //DOM
    if (document.getElementById)
    {
        document.getElementById('prepage').style.visibility='hidden';
    }
    else
    {
        if (document.layers)
        { //NS4
            document.prepage.visibility = 'hidden';
        }
        else 
        { //IE4
            document.all.prepage.style.visibility = 'hidden';
        }
    }
}
function ShowLoading() 
{ //DOM
    document.getElementById('prepage').style.visibility='visible';
}
// End PRELAODER **********************************************-->
