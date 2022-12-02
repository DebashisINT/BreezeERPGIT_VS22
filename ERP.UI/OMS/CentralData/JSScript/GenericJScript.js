 //JScript File
 
//On Loading
///////////////////////////////////////Page Loading Image
////document.write("<div id='prepage' style='position:absolute; font-family:arial; font-size:30; left:250px; top:15px;= background-color:white; layer-background-color:white; height:80; width:150;'>")
////document.write("<table width='100' height='35' border='1' cellpadding='0' cellspacing='0' bordercolor='#C0D6E4'>")
////document.write("  <tr><td><table><tr>")
////document.write("    <td height='25' align='center' bgcolor='#FFFFFF'>")
////document.write("      <img src='/assests/images/progress.gif' width='18' height='18'></td>")
//////document.write("  </tr>")
//////document.write("  <tr>")
////document.write("    <td height='10' width='100%' align='center' bgcolor='#FFFFFF'><font size='2' face='Tahoma'>")
////document.write("	<strong align='center'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Loading...</strong></font></td>")
////document.write(" </tr></table></td> </tr>")
////document.write("</table>")
////document.write("</div>")
////<!-- Begin PRELOADER ************************************************** *****
////function HideLoading() 
////{ //DOM
////    if (document.getElementById)
////    {
////        document.getElementById('prepage').style.visibility='hidden';
////    }
////    else
////    {
////        if (document.layers)
////        { //NS4
////            document.prepage.visibility = 'hidden';
////        }
////        else 
////        { //IE4
////            document.all.prepage.style.visibility = 'hidden';
////        }
////    }
////}
////function ShowLoading() 
////{ //DOM
////    
////    document.getElementById('prepage').style.visibility='visible';
////}
////// End PRELAODER **********************************************-->

 //ProtoType
    String.prototype.trim = function() {
        return this.replace(/^\s+|\s+$/g,"");
    }
    String.prototype.ltrim = function() {
        return this.replace(/^\s+/,"");
    }
    String.prototype.rtrim = function() {
        return this.replace(/\s+$/,"");
    }
    //
    //===Replace All By Replace Character From a String===================================
    String.prototype.replaceAll = function(search, replace)
    {        
        return this.replace(new RegExp('[' + search + ']', 'g'), replace);
    }

function CallMe()
{
    alert("Did You Call Me Hmmmmmmmmm...!!!!!");
}


///////////////////////////////////#region : Date Relavant Function
function DevE_CheckForLockDate(DateObject,LockDate)
{
    var monthtext=['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sept','Oct','Nov','Dec'];
    
    var SelectedDate = new Date(DateObject.GetDate());
    var monthnumber = SelectedDate.getMonth();
    var monthday    = SelectedDate.getDate();
    var year        = SelectedDate.getYear();
    
    
    var SelectedDateValue=new Date(year, monthnumber, monthday);
    ///Checking of Transaction Date For MaxLockDate
    var MaxLockDate=new Date(LockDate);
    monthnumber = MaxLockDate.getMonth();
    monthday    = MaxLockDate.getDate();
    year        = MaxLockDate.getYear();
    
    var MaxLockDateNumeric=new Date(year, monthnumber, monthday).getTime();
//                alert('TransactionDate='+TransactionDate+'\n'+'MaxLockDate= '+MaxLockDate);
    //alert(ValueDate+'~'+ValueDateNumeric+'~'+VisibleIndexE);
    if(SelectedDateValue<=MaxLockDateNumeric)
    {
        alert('Any Transaction Below ['+monthday+'-'+monthtext[monthnumber]+'-'+year+'] Has Been Locked!!!');
        MaxLockDate.setDate(MaxLockDate.getDate() + 1);
        DateObject.SetDate(MaxLockDate);
        return;
    }
    ///End Checking of Date For MaxLockDate
}
function DevE_CheckForFinYear(DateObject,FinYearStartDate,FinYearEndDate,LastFinYearDate)    
{    
    var SelectedDate = new Date(DateObject.GetDate());
    var monthnumber = SelectedDate.getMonth();
    var monthday    = SelectedDate.getDate();
    var year        = SelectedDate.getYear();
    
    var SelectedDateValue=new Date(year, monthnumber, monthday);
    ///Date Should Between Current Fin Year StartDate and EndDate
    var FYS =FinYearStartDate;
    var FYE =FinYearEndDate;
    var LFY =LastFinYearDate;
    var FinYearStartDate = new Date(FYS);
    var FinYearEndDate = new Date(FYE);
    var LastFinYearDate=new Date(LFY);
    
    monthnumber = FinYearStartDate.getMonth();
    monthday    = FinYearStartDate.getDate();
    year        = FinYearStartDate.getYear();
    var FinYearStartDateValue=new Date(year, monthnumber, monthday);
    
  
    monthnumber = FinYearEndDate.getMonth();
    monthday    = FinYearEndDate.getDate();
    year        = FinYearEndDate.getYear();
    var FinYearEndDateValue=new Date(year, monthnumber, monthday);
    
//                alert('SelectedDateValue :'+SelectedDateValue.getTime()+
//                '\nFinYearStartDateValue :'+FinYearStartDateValue.getTime()+
//                '\nFinYearEndDateValue :'+FinYearEndDateValue.getTime());
    
    var SelectedDateNumericValue=SelectedDateValue.getTime();
    var FinYearStartDateNumericValue=FinYearStartDateValue.getTime();
    var FinYearEndDatNumbericValue=FinYearEndDateValue.getTime();
    if(SelectedDateNumericValue>=FinYearStartDateNumericValue && SelectedDateNumericValue<=FinYearEndDatNumbericValue)
    {
//                   alert('Between');
    }
    else
    {
       alert('Enter Date Is Outside Of Financial Year !!');
       if(SelectedDateNumericValue<FinYearStartDateNumericValue)
       {
            DateObject.SetDate(new Date(FinYearStartDate));
       }
       if(SelectedDateNumericValue>FinYearEndDatNumbericValue)
       {
            DateObject.SetDate(new Date(FinYearEndDate));
       }
       DateObject.Focus();
    }
    ///End OF Date Should Between Current Fin Year StartDate and EndDate
}

function DevE_CompareDateForMin(DateObjectFrm,DateObjectTo,Msg)    
{
    var SelectedDate = new Date(DateObjectFrm.GetDate());
    var monthnumber = SelectedDate.getMonth();
    var monthday    = SelectedDate.getDate();
    var year        = SelectedDate.getYear();
    var SelectedDateValueFrm=new Date(year, monthnumber, monthday);
    
    var SelectedDate = new Date(DateObjectTo.GetDate());
    monthnumber = SelectedDate.getMonth();
    monthday    = SelectedDate.getDate();
    year        = SelectedDate.getYear();
    var SelectedDateValueTo=new Date(year, monthnumber, monthday);
    var SelectedDateNumericValueFrm=SelectedDateValueFrm.getTime();
    var SelectedDateNumericValueTo=SelectedDateValueTo.getTime();
    if(SelectedDateNumericValueFrm>SelectedDateNumericValueTo)
    {
        alert(Msg);
        DateObjectTo.SetDate(new Date(DateObjectFrm.GetDate()));
    }
}
function DevE_CompareDateForMin_AddDay(DateObjectFrm,DateObjectTo,NoOfDayToAdd,Msg)    
{
    var SelectedDate = new Date(DateObjectFrm.GetDate());
    var monthnumber = SelectedDate.getMonth();
    var monthday    = SelectedDate.getDate();
    var year        = SelectedDate.getYear();
    var SelectedDateValueFrm=new Date(year, monthnumber, monthday);
    
    var SelectedDate = new Date(DateObjectTo.GetDate());
    monthnumber = SelectedDate.getMonth();
    monthday    = SelectedDate.getDate();
    year        = SelectedDate.getYear();
    var SelectedDateValueTo=new Date(year, monthnumber, monthday);
    var SelectedDateNumericValueFrm=SelectedDateValueFrm.getTime();
    var SelectedDateNumericValueTo=SelectedDateValueTo.getTime();
    if(SelectedDateNumericValueFrm>SelectedDateNumericValueTo)
    {
        if(Msg!='n') alert(Msg);
        var AddDayDate=new Date(DateObjectFrm.GetDate());
        AddDayDate.setDate(AddDayDate.getDate() + NoOfDayToAdd);
        DateObjectTo.SetDate(AddDayDate);
    }
}
function DevE_CompareDateAndAddDay(DateObject1,DateObject2,NoOfDay)
{
    var AddDayDate=new Date(DateObject1.GetDate());
    AddDayDate.setDate(AddDayDate.getDate() + NoOfDay);
    DateObject2.SetDate(AddDayDate);
}
function DevE_CompareDateToControl(DateObjectFrm,StaticDate,Msg)    
{
    //alert(StaticDate);
    var SelectedDate = new Date(DateObjectFrm.GetDate());
    var monthnumber = SelectedDate.getMonth();
    var monthday    = SelectedDate.getDate();
    var year        = SelectedDate.getYear();
    var SelectedDateValueFrm=new Date(year, monthnumber, monthday);
    
    var SelectedDate = new Date(StaticDate);
    monthnumber = SelectedDate.getMonth();
    monthday    = SelectedDate.getDate();
    year        = SelectedDate.getYear();
    var SelectedDateValueTo=new Date(year, monthnumber, monthday);
    
    var SelectedDateNumericValueFrm=SelectedDateValueFrm.getTime();
    var SelectedDateNumericValueTo=SelectedDateValueTo.getTime();
    if(SelectedDateNumericValueFrm<SelectedDateNumericValueTo)
    {
       alert(Msg);
       DateObjectFrm.SetDate(SelectedDate);
    }
}
//Set Date
function DevE_SetDateTime(DateObjectFrm,StaticDate)    
{
    var SelectedDate = new Date(StaticDate);
    DateObjectFrm.SetDate(SelectedDate);
}
//Set To date Maximum as Financial Year End
function DevE_CompareDateToMax(DateObjectTo,StaticDate,Msg)    
{
    var SelectedDate = new Date(DateObjectTo.GetDate());
    var monthnumber = SelectedDate.getMonth();
    var monthday    = SelectedDate.getDate();
    var year        = SelectedDate.getYear();
    var SelectedDateValueTo=new Date(year, monthnumber, monthday);
    
    var SelectedDate = new Date(StaticDate);
    monthnumber = SelectedDate.getMonth();
    monthday    = SelectedDate.getDate();
    year        = SelectedDate.getYear();
    var StaticDateValue=new Date(year, monthnumber, monthday);
    
    var SelectedDateNumericValueTo=SelectedDateValueTo.getTime();
    var StaticNumericDateValue=StaticDateValue.getTime();
    if(SelectedDateNumericValueTo>StaticNumericDateValue)
    {
       alert(Msg);
       DateObjectTo.SetDate(SelectedDate);
    }
}
//Compare To Date and Set From Date equal as To Date 
function DevE_CompareDateFromMax(DateObjectFrm,DateObjectTo,Msg)    
{
    var SelectedDate = new Date(DateObjectFrm.GetDate());
    var monthnumber = SelectedDate.getMonth();
    var monthday    = SelectedDate.getDate();
    var year        = SelectedDate.getYear();
    var SelectedDateValueFrm=new Date(year, monthnumber, monthday);
    
    var SelectedDate = new Date(DateObjectTo.GetDate());
    monthnumber = SelectedDate.getMonth();
    monthday    = SelectedDate.getDate();
    year        = SelectedDate.getYear();
    var SelectedDateValueTo=new Date(year, monthnumber, monthday);
    var SelectedDateNumericValueFrm=SelectedDateValueFrm.getTime();
    var SelectedDateNumericValueTo=SelectedDateValueTo.getTime();
    if(SelectedDateNumericValueFrm>SelectedDateNumericValueTo)
    {
        alert(Msg);
        DateObjectFrm.SetDate(new Date(DateObjectTo.GetDate()));
    }
}

//compare date without alert message

function DevE_CompareDateToMax(DateObjectTo, StaticDate) {


    var SelectedDate = new Date(DateObjectTo.GetDate());
    var monthnumber = SelectedDate.getMonth();
    var monthday = SelectedDate.getDate();
    var year = SelectedDate.getYear();
    var SelectedDateValueTo = new Date(year, monthnumber, monthday);

    var SelectedDate = new Date(StaticDate);
    monthnumber = SelectedDate.getMonth();
    monthday = SelectedDate.getDate();
    year = SelectedDate.getYear();
    var StaticDateValue = new Date(year, monthnumber, monthday);

    var SelectedDateNumericValueTo = SelectedDateValueTo.getTime();
    var StaticNumericDateValue = StaticDateValue.getTime();
    if (SelectedDateNumericValueTo > StaticNumericDateValue) {
        //       alert(Msg);
        DateObjectTo.SetDate(SelectedDate);
    }
}

//Compare Two Dates
//WhichCompare EQ,GT,LT,GE,LE
//Msg Your Own Msg
//ObjToReset Object Name To Reset
//DateToBeSet Date that Reset ObjToReset
function CompareDate(Date1,Date2,WhichCompare,Msg,ObjToReset,DateToBeSet)
{
    var DateFirst = new Date(Date1);
    var DateSecond = new Date(Date2);
    var oDateToBeSet=new Date(DateToBeSet);
    
    monthnumber = DateFirst.getMonth();
    monthday    = DateFirst.getDate();
    year        = DateFirst.getYear();
    var DateFirstValue=new Date(year, monthnumber, monthday);
    
  
    monthnumber = DateSecond.getMonth();
    monthday    = DateSecond.getDate();
    year        = DateSecond.getYear();
    var DateSecondeValue=new Date(year, monthnumber, monthday);
    
    var DateFirstNumeric=DateFirstValue.getTime();
    var DateSecondeNumeric=DateSecondeValue.getTime();
    
    if(WhichCompare=='EQ')
    {
        if(DateFirstNumeric=DateSecondeNumeric)
            return;
        else
        {
            alert(Msg);
            ObjToReset.SetDate(oDateToBeSet);
        }
    }
    if(WhichCompare=='GT')
    {
        if(DateFirstNumeric>DateSecondeNumeric)
            return;
        else
        {
            alert(Msg);
            ObjToReset.SetDate(oDateToBeSet);
        }
    }
    if(WhichCompare=='LT')
    {
        if(DateFirstNumeric<DateSecondeNumeric)
            return;
        else
        {
            alert(Msg);
            ObjToReset.SetDate(oDateToBeSet);
        }
    }
    if(WhichCompare=='GE')
    {
        if(DateFirstNumeric>=DateSecondeNumeric)
            return;
       else
        {
            alert(Msg);
            ObjToReset.SetDate(oDateToBeSet);
        }
    }
    if(WhichCompare=='LE')
    {
        if(DateFirstNumeric<=DateSecondeNumeric)
            return;
        else
        {
            alert(Msg);
            ObjToReset.SetDate(oDateToBeSet);
        }
    }
        
}
//This Method will Return true Or False After Date Compare
function CompareDate_TrueFalse(Date1,Date2,WhichCompare)
{
    var DateFirst = new Date(Date1);
    var DateSecond = new Date(Date2);
    
    monthnumber = DateFirst.getMonth();
    monthday    = DateFirst.getDate();
    year        = DateFirst.getYear();
    var DateFirstValue=new Date(year, monthnumber, monthday);
    
  
    monthnumber = DateSecond.getMonth();
    monthday    = DateSecond.getDate();
    year        = DateSecond.getYear();
    var DateSecondeValue=new Date(year, monthnumber, monthday);
    
    var DateFirstNumeric=DateFirstValue.getTime();
    var DateSecondeNumeric=DateSecondeValue.getTime();
    
    if(WhichCompare=='EQ')
    {
        if(DateFirstNumeric=DateSecondeNumeric)
            return false;
        else
        {
            return true;
        }
    }
    if(WhichCompare=='GT')
    {
        if(DateFirstNumeric>DateSecondeNumeric)
            return false;
        else
        {
            return true;
        }
    }
    if(WhichCompare=='LT')
    {
        if(DateFirstNumeric<DateSecondeNumeric)
            return false;
        else
        {
            return true;
        }
    }
    if(WhichCompare=='GE')
    {
        if(DateFirstNumeric>=DateSecondeNumeric)
            return false;
       else
        {
            return true;
        }
    }
    if(WhichCompare=='LE')
    {
        if(DateFirstNumeric<=DateSecondeNumeric)
            return false;
        else
        {
            return true;
        }
    }

}
function DevE_CheckForFinYearWithoutAlert(DateObject, FinYearStartDate, FinYearEndDate, LastFinYearDate) {
    var SelectedDate = new Date(DateObject.GetDate());
    var monthnumber = SelectedDate.getMonth();
    var monthday = SelectedDate.getDate();
    var year = SelectedDate.getYear();

    var SelectedDateValue = new Date(year, monthnumber, monthday);
    ///Date Should Between Current Fin Year StartDate and EndDate
    var FYS = FinYearStartDate;
    var FYE = FinYearEndDate;
    var LFY = LastFinYearDate;
    var FinYearStartDate = new Date(FYS);
    var FinYearEndDate = new Date(FYE);
    var LastFinYearDate = new Date(LFY);

    monthnumber = FinYearStartDate.getMonth();
    monthday = FinYearStartDate.getDate();
    year = FinYearStartDate.getYear();
    var FinYearStartDateValue = new Date(year, monthnumber, monthday);


    monthnumber = FinYearEndDate.getMonth();
    monthday = FinYearEndDate.getDate();
    year = FinYearEndDate.getYear();
    var FinYearEndDateValue = new Date(year, monthnumber, monthday);

    //                alert('SelectedDateValue :'+SelectedDateValue.getTime()+
    //                '\nFinYearStartDateValue :'+FinYearStartDateValue.getTime()+
    //                '\nFinYearEndDateValue :'+FinYearEndDateValue.getTime());

    var SelectedDateNumericValue = SelectedDateValue.getTime();
    var FinYearStartDateNumericValue = FinYearStartDateValue.getTime();
    var FinYearEndDatNumbericValue = FinYearEndDateValue.getTime();
    if (SelectedDateNumericValue >= FinYearStartDateNumericValue && SelectedDateNumericValue <= FinYearEndDatNumbericValue) {
        //                   alert('Between');
    }
    else {

        if (SelectedDateNumericValue < FinYearStartDateNumericValue) {
            DateObject.SetDate(new Date(FinYearStartDate));
        }
        if (SelectedDateNumericValue > FinYearEndDatNumbericValue) {
            DateObject.SetDate(new Date(FinYearEndDate));
        }
        DateObject.Focus();
    }
    ///End OF Date Should Between Current Fin Year StartDate and EndDate
}


function DevE_SetFirstLastDay(DateObjSource, DateObjFirstDayTarget, DateObjLastDayTarget) {
    var datefor = new Date(DateObjSource.GetDate());
    var monthnumber = datefor.getMonth();
    var monthday = datefor.getDate() - datefor.getDate() + 1;
    var year = datefor.getYear();

    var ExpireDateValue = new Date(year, monthnumber, monthday);
    var lastday = new Date(year, monthnumber + 1, 0);
    DateObjFirstDayTarget.SetDate(ExpireDateValue);
    DateObjLastDayTarget.SetDate(lastday);

}

function DevE_SetFirstDayOfMonth(DateObjSource, DateObjFirstDayTarget) {
    var datefor = new Date(DateObjSource.GetDate());
    var monthnumber = datefor.getMonth();
    var monthday = datefor.getDate() - datefor.getDate() + 1;
    var year = datefor.getYear();
    var ExpireDateValue = new Date(year, monthnumber, monthday);
    DateObjFirstDayTarget.SetDate(ExpireDateValue);

}
function DevE_SetLastDayOfMonth(DateObjSource, DateObjLastDayTarget) {
    var datefor = new Date(DateObjSource.GetDate());
    var monthnumber = datefor.getMonth();
    var monthday = datefor.getDate() - datefor.getDate() + 1;
    var year = datefor.getYear();
    var lastday = new Date(year, monthnumber + 1, 0);
    DateObjLastDayTarget.SetDate(lastday);

}



///////////////////////////////////#endregion : Date Relavant Function

///////////////////////////////////#region : Validation
//Call Like onkeypress="return onlyNumbers();"
function onlyNumbers(evt)
{
	var e = event || evt; // for trans-browser compatibility
	var charCode = e.which || e.keyCode;

	if (charCode > 31 && (charCode < 48 || charCode > 57))
		return false;

	return true;

}

function letternumber(e)
{
    var key;
    var keychar;

    if (window.event)
       key = window.event.keyCode;
    else if (e)
       key = e.which;
    else
       return true;
    keychar = String.fromCharCode(key);
    keychar = keychar.toLowerCase();

    // control keys
    if ((key==null) || (key==0) || (key==8) || 
        (key==9) || (key==13) || (key==27) )
       return true;

    // alphas and numbers
    else if ((("abcdefghijklmnopqrstuvwxyz0123456789").indexOf(keychar) > -1))
       return true;
    else
       return false;
}
function isValidDate(s) {
    // format D(D)/M(M)/(YY)YY
    var dateFormat = /^\d{1,4}[\.|\/|-]\d{1,2}[\.|\/|-]\d{1,4}$/;

    if (dateFormat.test(s)) {
        // remove any leading zeros from date values
        s = s.replace(/0*(\d*)/gi,"$1");
        var dateArray = s.split(/[\.|\/|-]/);
      
        // correct month value
        dateArray[1] = dateArray[1]-1;

        // correct year value
        if (dateArray[2].length<4) {
            // correct year value
            dateArray[2] = (parseInt(dateArray[2]) < 50) ? 2000 + parseInt(dateArray[2]) : 1900 + parseInt(dateArray[2]);
        }

        var testDate = new Date(dateArray[2], dateArray[1], dateArray[0]);
        if (testDate.getDate()!=dateArray[0] || testDate.getMonth()!=dateArray[1] || testDate.getFullYear()!=dateArray[2]) {
            return false;
        } else {
            return true;
        }
    } else {
        return false;
    }
}
function Requiredfield(obj,ErrMsg)
{
    if(document.getElementById(obj).value.toUpperCase().trim()=="NO RECORD FOUND" ||
        document.getElementById(obj).value.toUpperCase().trim()=="")
    {
        alert(ErrMsg);
        document.getElementById(obj).value = "";
        document.getElementById(obj).focus();
        return false;
    }
    return true;
}
function Control_Empty(obj,ErrMsg)
{
   var control = document.getElementById(obj); 
   if(control.value.trim()=='')
   {
        alert(ErrMsg);
        document.getElementById(obj).focus(); 
        return false;
    }
    else 
    {
        return true;
    } 
}
function Control_CompareText(obj,CompareText,ErrMsg)
{
   var control = document.getElementById(obj); 
   var TextToCompare='';
   var WhatReturn='true';
   for(var i=0;i<CompareText.split(',').length;i++)
   {
       TextToCompare=CompareText.split(',')[i];
       if(control.value.trim() == TextToCompare)
       {
            alert(ErrMsg);
            document.getElementById(obj).focus(); 
            WhatReturn='false';
        }
    }
    if(WhatReturn=='false')
        return false;
    else
        return true; 
}
//These Method When Compare Any Control and Focus On Other
function Control_Empty_FocusOtherCntrl(obj,FcsObj,ErrMsg)
{
   var control = document.getElementById(obj); 
   if(control.value.trim()=='')
   {
        alert(ErrMsg);
        document.getElementById(FcsObj).focus(); 
        return false;
    }
    else 
    {
        return true;
    } 
}
function Control_CompareText_FocusOtherCntrl(obj,CompareText,FcsObj,ErrMsg)
{
   var control = document.getElementById(obj); 
   var TextToCompare='';
   var WhatReturn='true';
   for(var i=0;i<CompareText.split(',').length;i++)
   {
       TextToCompare=CompareText.split(',')[i];
       if(control.value.trim() == TextToCompare)
       {
            alert(ErrMsg);
            document.getElementById(FcsObj).focus();
//            document.getElementById(FcsObj).select();  
            WhatReturn='false';
        }
    }
    if(WhatReturn=='false')
        return false;
    else
        return true; 
}


///////////////////////////////////#endregion : Validation


//////////////////////////////////#region : Other Method
  //Check for Expiry Date Where Its Expire Or Not if Expire Return True
function IsProductExpired(CompareToDate,SessionExpireDate)
{
    ///Date Should Between Current Fin Year StartDate and EndDate
    var ExpireDate = new Date(SessionExpireDate);
    var CompareDate = new Date(CompareToDate);
    
    
    monthnumber = ExpireDate.getMonth();
    monthday    = ExpireDate.getDate();
    year        = ExpireDate.getYear();
    var ExpireDateValue=new Date(year, monthnumber, monthday);
    
  
    monthnumber = CompareDate.getMonth();
    monthday    = CompareDate.getDate();
    year        = CompareDate.getYear();
    var CompareDateValue=new Date(year, monthnumber, monthday);
    
    // alert('ExpireDateValue :'+ExpireDateValue.getTime()+
    // '\nCompareDateValue :'+CompareDateValue.getTime());
    
    var ExpireDateNumeric=ExpireDateValue.getTime();
    var CompareDateNumeric=CompareDateValue.getTime();

    if(ExpireDateNumeric<CompareDateNumeric)
    {
        alert('T~Licency Expired.Please Renew Your Licence for No Further Interruption!!!.Sorry For InConvenience.');
        return;
    }
    else
        return 'F~';
        
}
function HideShow(obj,HS)
{
    if(HS=="H")
        document.getElementById(obj).style.display="None";
    else
        document.getElementById(obj).style.display="inline";
}
function EnableDisableControl(obj,ED)
{
    if(ED=="E")
    {
        document.getElementById(obj).disabled=false;
    }
    else
    {
        document.getElementById(obj).disabled=true;
    }
}
function VisibleInVisible(obj,HS)
{
    if(HS=="H")
        document.getElementById(obj).style.visibility="hidden";
    else
        document.getElementById(obj).style.visibility="visible";
}
function SetValue(obj,Value)
{
    document.getElementById(obj).value=Value;
}
function GetValue(obj)
{
    return document.getElementById(obj).value;
}
function GetObjectID(obj)
{
    return document.getElementById(obj);
} 

function WaterMark_Focus(objname, waterMarkText) {
    obj = document.getElementById(objname);
    if (obj.value.trim() == waterMarkText || obj.value.trim() == "No Record Found") {
        obj.value = "";
        obj.className = "NormalTextBox TextBox";
    }
}
function WaterMark_Blur(objname,waterMarkText) {
    obj = document.getElementById(objname);
    //alert(obj.value);
    if (obj.value.trim() == "" || obj.value.trim() =="No Record Found") {
        obj.value = waterMarkText;
        obj.className = "WaterMarkedTextBox TextBox";
    }
    else {
        obj.className = "TextBox";
    }
}
function SetStyle(obj,StyleName,StyleValue)
{
    if(StyleName=="BGImage")
        document.getElementById(obj).style.backgroundImage=StyleValue;
    if(StyleName=="BGColor")
        document.getElementById(obj).style.backgroundColor=StyleValue;
    if(StyleName=="BGRepeat")
        document.getElementById(obj).style.backgroundRepeat=StyleValue;
    if(StyleName=="Color")
        document.getElementById(obj).style.color=StyleValue;
    if(StyleName=="Display")
        document.getElementById(obj).style.display=StyleValue;
    if(StyleName=="Border")
        document.getElementById(obj).style.border=StyleValue; 
    if(StyleName=="Scroll")
        document.getElementById(obj).style.overflow=StyleValue;
        
}
function SetinnerHTML(obj,Value)
{
    document.getElementById(obj).innerHTML=Value;
}
function GetinnerHTML(obj,Value)
{
    return document.getElementById(obj).innerHTML;
}

/// Set Body Tag onload Event Like onload="setInterval('blinkIt()',500)"
/// And Use This Tag <blink></blink> To Blink a html Object Like <div>,<a>,<b> etc..
//To Use Below Method Properly
function blinkIt() 
{
    if (!document.all) return;
    else 
    {
      for(i=0;i<document.all.tags('blink').length;i++)
      {
        s=document.all.tags('blink')[i];
        s.style.visibility=(s.style.visibility=='visible') ?'hidden':'visible';
      }
    }
}
//////////////////////////////////#endregion : Other Method




///////////////////////////////////#region : Ajax
function CallAjax(obj1,obj2,obj3,Query)
{
    var CombinedQuery=new String(Query);
    ajax_showOptions(obj1,obj2,obj3,replaceChars(CombinedQuery),'Main');
}
function replaceChars(entry)
{
    out = "+"; // replace this
    add = "--"; // with this
    temp = "" + entry; // temporary holder
    while (temp.indexOf(out)>-1) 
    {
        pos= temp.indexOf(out);
        temp = "" + (temp.substring(0, pos) + add + 
        temp.substring((pos + out.length), temp.length));
     }
     return temp;
} 
///////////////////////////////////#endregion : Validation
