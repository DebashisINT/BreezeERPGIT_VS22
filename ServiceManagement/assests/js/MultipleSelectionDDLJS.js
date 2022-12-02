/*
09042009 Bryian Tan.
*/

/*detect the browser version and name*/
var Browser = {
  Version: function() {
    var version = 999; // we assume a sane browser
    if (navigator.appVersion.indexOf("MSIE") != -1)
      // bah, IE again, lets downgrade version number
      version = parseFloat(navigator.appVersion.split("MSIE")[1]);
    return version;
  }
}

function showIE6Tooltip(e){
    //we only want this to execute if ie6
    if (navigator.appName=='Microsoft Internet Explorer' && Browser.Version() == 6) {
        if(!e){var e = window.event;}
        var obj = e.srcElement;
        
        tempX = event.clientX + (document.documentElement.scrollLeft || document.body.scrollLeft);
        tempY = event.clientY + (document.documentElement.scrollTop || document.body.scrollTop);
        
        var tooltip = document.getElementById('ie6SelectTooltip');
        tooltip.innerHTML = obj.options.title; //set the title to the div
        //display the tooltip based on the mouse location
        tooltip.style.left = tempX;
         tooltip.style.top = tempY+10;
        tooltip.style.width = '100%';
        tooltip.style.display = 'block';
      }
    }
    function hideIE6Tooltip(e){
     //we only want this to execute if ie6
       if (navigator.appName=='Microsoft Internet Explorer' && Browser.Version() == 6) {
        var tooltip = document.getElementById('ie6SelectTooltip');
        tooltip.innerHTML = '';
        tooltip.style.display = 'none';
       }
    }

function getCheckBoxListItemsChecked(elementId) {
    
    //var
    var elementRef = document.getElementById(elementId);
    var checkBoxArray = elementRef.getElementsByTagName('input');
    var checkedValues = '';
    var checkedText = '';
    var checkedSelIndex = '';
    var myCheckBox = new Array();

    for (var i = 0; i < checkBoxArray.length; i++) {
        var checkBoxRef = checkBoxArray[i];

        if (checkBoxRef.checked == true) {
        
        //selected index
            if (checkedSelIndex.length > 0)
                checkedSelIndex += ', ';
            checkedSelIndex +=i;     
        
        //value
            if (checkedValues.length > 0)
                checkedValues += ', ';

            checkedValues += checkBoxRef.value;

        //text
            var labelArray = checkBoxRef.parentNode.getElementsByTagName('label');

            if (labelArray.length > 0) {
                if (checkedText.length > 0)
                    checkedText += ', ';
                checkedText += labelArray[0].innerHTML;
            }

        }
    }

    myCheckBox[0] = checkedText;
    myCheckBox[1] = checkedValues;
    myCheckBox[2] = checkedSelIndex;

    return myCheckBox;
}

function readCheckBoxList(chkBox, ddlList, hiddenFieldText, hiddenFieldValue, hiddenFieldSelIndex) {
    var checkedItems = getCheckBoxListItemsChecked(chkBox);
    alert(checkedItems);
    $get(ddlList).options[0].innerHTML = checkedItems[1]; //set the dropdownlist value
    $get(ddlList).title = checkedItems[0]; //set the title for the dropdownlist
    //set hiddenfield value
    $get(hiddenFieldValue).value = checkedItems[1];
    $get(hiddenFieldText).value = checkedItems[0];
    $get(hiddenFieldSelIndex).value = checkedItems[2];
}
