using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Payroll.Models
{

    public class P_formula_header
    {
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string table { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string short_nm { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public DateTime? applicbl_frm { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public DateTime ?applicbl_to { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string FormulaHeaderName { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string tableFormulaCode { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public int? TableBreakUpId { get; set; }
    }

    public class P_formula_dtls
    {

        public string low { get; set; }
        public string high { get; set; }
        public string value { get; set; }

        public string TableFormulaDetail_ID { get; set; }

    }

    public class TableFormulaHeadBreakup
    {
        public string TableFormulaCode { get; set; }
        public string TableName { get; set; }
        public string ShortName { get; set; }
        public DateTime ApplicatedFrom { get; set; }
        public DateTime ApplicatedTo { get; set; }
       
    }


    public class FormulaApply
    {

        public P_formula_header header { get; set; }
        public List<P_formula_dtls> dtls { get; set; }
        public List<TableFormulaHeadBreakup> outerdtls { get; set; }
        public string response_code { get; set; }
        public string response_msg { get; set; }

    }

    public class Msg
    {
        public string response_code { get; set; }
        public string response_msg { get; set; }
    }
}
