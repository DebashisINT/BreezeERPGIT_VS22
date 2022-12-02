using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMS.Models.ViewModel
{
    public class ProficiencyViewModel
    {
        public string ProficiencyID { get; set; }
        public string ProficiencyNAME { get; set; }
        public string Ratable_Entity { get; set; }
        public string Min_Rate { get; set; }
        public string Max_Rate { get; set; }
        public string RatingName { get; set; }
        public string Rating_Value { get; set; }
        public string IsDefault { get; set; }
        public string Create_By { get; set; }
        public string Create_Date { get; set; }
        public string Update_BY { get; set; }
        public string Update_Date { get; set; }
        public string RATING1 { get; set; }
        public string RATING2 { get; set; }
        public string RATING3 { get; set; }
        public string RATING4 { get; set; }
        public string RATING5 { get; set; }
        public string RATING6 { get; set; }
        public string RATING7 { get; set; }
        public string RATING8 { get; set; }
        public string RATING9 { get; set; }
        public string RATING10 { get; set; }
        public int DEFAULTVALU { get; set; }
        public string Characteristic { get; set; }
        public List<MaxRates> Max_RateList { get; set; }
        public List<CharacteristicType> CharacteristicTypeList { get; set; }

    }

    public class ProficiencyList
    {
        public long ProficiencyID { get; set; }
        public string ProficiencyNAME { get; set; }
        public string Ratable_Entity { get; set; }
        public int Min_Rate { get; set; }
        public int Max_Rate { get; set; }
        public string RatingName { get; set; }
        public int Rating_Value { get; set; }
        public bool IsDefault { get; set; }
        public string CREATE_NAME { get; set; }
        public DateTime? Create_Date { get; set; }
        public string UPDATE_NAME { get; set; }
        public DateTime? Update_Date { get; set; }
    }

    public class MaxRates
    {
        public string RATING_ID { get; set; }
        public string RATING { get; set; }
    }

    public class CharacteristicType
    {
        public string Characteristic_ID { get; set; }
        public string Characteristic { get; set; }
    }

}