
//  convert numbers to comma separated currency 1,00,000.00
const numberWithCommas = (x) => {
    x = x.toString();
    if (x.toString().indexOf('.') == -1) {
        var lastThree = x.substring(x.length - 3);
        var otherNumbers = x.substring(0, x.length - 3);
        if (otherNumbers != '')
            lastThree = ',' + lastThree;
        var res = otherNumbers.replace(/\B(?=(\d{2})+(?!\d))/g, ",") + lastThree;

    } else {
        var dec = x.substr(x.indexOf('.') + 1, x.length);
        x = x.substr(0, x.indexOf('.'))
        var lastThree = x.substring(x.length - 3);
        var otherNumbers = x.substring(0, x.length - 3);
        if (otherNumbers != '')
            lastThree = ',' + lastThree;
        var res = otherNumbers.replace(/\B(?=(\d{2})+(?!\d))/g, ",") + lastThree + '.' + dec;
    }
    return res;
}
//  convert numbers to lakh, corore, thousand
const numFormatterLocal = (num) => {
    if (num > 999.99 && num < 100000) {
        return (num / 1000).toFixed(0) + 'K'; // convert to K for number from > 1000 < 1 million 
    } else if (num < 0 && num > -100000) {
        return (num / 1000).toFixed(0) + 'K'; // convert to K for number from > 1000 < 1 million 
    } else if (num < -99999.99 && num > -10000000) {
        return (num / 100000).toFixed(0) + 'L'; // convert to K for number from > 1000 < 1 million 
    } else if (num > 99999.99 && num < 10000000) {
        return (num / 100000).toFixed(0) + 'L'; // convert to M for number from > 1 million 
    } else if (num > 9999999.99) {
        return (num / 10000000).toFixed(0) + 'C'; // convert to M for number from > 1 million 
    } else if (num < -9999999.99) {
        return (num / 10000000).toFixed(0) + 'C'; // convert to M for number from > 1 million 
    } else if (num < 900) {
        return (num * 1).toFixed(0); // if value < 1000, nothing to do
    }
}
//  convert numbers to 'K', 'M', "B" etc
function numFormatterGlobal(x) {
    if(isNaN(x)) return x;
    if(x < 9999) {
        return x;
    }
    //if(x < 1000000) {
    //    return Math.round(x/1000) + "K";
    //}
    if( x < 10000000) {
        return (x/1000000).toFixed(2) + "M";
    }
    if(x < 1000000000) {
        return Math.round((x/1000000)) + "M";
    }
    if(x < 1000000000000) {
        return Math.round((x/1000000000)) + "B";
    }
    return "1T+";
}
//  convert todays date to DD-MM-YYYY
function getCurrentDate() {
    var x = new Date().toLocaleDateString().split('/');
    var month = '';
    var day = '';
    var year = '';
    if (x[0].length > 1) {
        month = x[0]
    } else {
        month = '0' +x[0]
    }
    if (x[1].length > 1) {
        day = x[1]
    } else {
        day = '0' + x[1]
    }
    if (x[2].length > 1) {
        year = x[2]
    } else {
        year = '0' + x[2]
    }
    return year + '-' + month + '-' + day
}
// get date object from database and convert to DD-MM-YYYY
function convertToDate (x) {
    let a = Date(x)
    var d = new Date(a),
    month = '' + (d.getMonth() + 1),
    day = '' + d.getDate(),
    year = d.getFullYear();

    if (month.length < 2) 
        month = '0' + month;
    if (day.length < 2) 
        day = '0' + day;
    return [day, month, year].join('-');
    //return new Date(a).toLocaleDateString()
}

// week work WTD
 Date.prototype.getWeek = function () {
    var target = new Date(this.valueOf());
    var dayNr = (this.getDay() + 6) % 7;
    target.setDate(target.getDate() - dayNr + 3);
    var firstThursday = target.valueOf();
    target.setMonth(0, 1);
    if (target.getDay() != 4) {
        target.setMonth(0, 1 + ((4 - target.getDay()) + 7) % 7);
    }
    return 1 + Math.ceil((firstThursday - target) / 604800000);
}
let numOfdaysPastSinceLastMonday;
function getDateRangeOfWeek(weekNo) {
    var d1 = new Date();
    numOfdaysPastSinceLastMonday = eval(d1.getDay() - 1);
    d1.setDate(d1.getDate() - numOfdaysPastSinceLastMonday);
    var weekNoToday = d1.getWeek();
    var weeksInTheFuture = eval(weekNo - weekNoToday);
    d1.setDate(d1.getDate() + eval(7 * weeksInTheFuture));
    var rangeIsFrom = eval(d1.getMonth() + 1) + "/" + d1.getDate() + "/" + d1.getFullYear();
    d1.setDate(d1.getDate() + 6);
    var rangeIsTo = eval(d1.getMonth() + 1) + "/" + d1.getDate() + "/" + d1.getFullYear();
    return rangeIsFrom + " to " + rangeIsTo;
};

export default Date.prototype.whatWeek = function () {
    var onejan = new Date(this.getFullYear(), 0, 1);
    var today = new Date(this.getFullYear(), this.getMonth(), this.getDate());
    var dayOfYear = ((today - onejan + 86400000) / 86400000);
    return Math.ceil(dayOfYear / 7)
};
// QTD
function quarter_of_the_year(date) {
    var month = date.getMonth() + 1;
    return (Math.ceil(month / 3));
}


export {getDateRangeOfWeek,  numFormatterLocal , numberWithCommas, numFormatterGlobal, getCurrentDate, convertToDate, quarter_of_the_year };


// new Date().toISOString().split('T')[0] OR new Date().toISOString().slice(0,10);
// it will return 2020-09-30 