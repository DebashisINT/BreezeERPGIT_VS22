using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PMS.Models.ViewModel
{
    public class ProjectViewModel
    {
        public Int64 Proj_Id { get; set; }
        public string Proj_Name { get; set; }
        public string Action { get; set; }
        public string Proj_Code { get; set; }
        public string Proj_Description { get; set; }
        public string Cnt_InternalId { get; set; }
        public string Customer { get; set; }
        public string branch_description { get; set;}
        public string RESOURCE_NAME { get; set; }
        public string BranchName { get; set; }
        public Int64 Proj_Calender { get; set; }
        public Int64 Proj_Bracnchid { get; set; }
        public Int64 Proj_Managerid { get; set; }
        public string Proj_Statuscolor { get; set; }
        public DateTime Proj_EstimateStartdate { get; set; }
        public DateTime Proj_EstimateEnddate { get; set; }
         [RegularExpression(@"^[0-9]+(\.[0-9]{1,2})$", ErrorMessage = "Valid Decimal number with maximum 2 decimal places.")]
        public decimal Proj_Estimatehours { get; set; }
        public decimal Proj_EstimatelabourCost { get; set; }
        public decimal Proj_EstimateExpenseCost { get; set; }
        public decimal proj_EstimateTotCost { get; set; }
        public DateTime Proj_ActualStartdate { get; set; }
        public DateTime Proj_ActualEndDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int ModifiedBy { get; set; }
        public int Approved_by { get; set; }
        public Int64 Proj_estimateHH { get; set; }
        public Int64 Proj_estimateMM { get; set; }
        public DateTime Approved_On { get; set; }
        public Int64 NumberSchemaId { get; set; }
        public string Remarks { get; set; }
        public string ProjectStatus { get; set; }
        public string CancelRemarks { get; set; }
        public string projStage_Desc { get; set; }
        public Boolean iscancel { get; set; }
        public string Doc_No { get; set; }

        public Int64 Proj_Hierarchy { get; set; }

        public string Order_Id { get; set; }
        public string BranchMap_Id { get; set; }
       //chinmoy for termsConditions save start  

        public DateTime SaveTerms_DefectLibilityPeriodDate { get; set; }
        public DateTime SaveTerms_DefectLibilityPeriodToDate { get; set; }
        public string SaveTerms_DefectLibilityPeriodRemarks { get; set; }
        public string SaveTerms_LiqDamage { get; set; }
        public DateTime SaveTerms_LiqDamageAppDate { get; set; }
        public string SaveTerms_Payment { get; set; }
        public string SaveTerms_OrderType { get; set; }
        public string SaveTerms_NatureWork { get; set; }


        public string SaveEditTerms_DefectLibilityPeriodDate { get; set; }
        public string SaveEditTerms_DefectLibilityPeriodToDate { get; set; }
        public string SaveEditTerms_LiqDamageAppDate { get; set; }
        public string hiddenTermsCoditions { get; set; }
        //End

        // Mantis Issue 25051
        public string ShowAddlDetlInProjMast { get; set; }
        // End of Mantis Issue 25051

        public List<Units> BranchList { get; set; }
        public List<ProjectmanList> ProjectManagerList { get; set; }
        public List<Statues> StatusList { get; set; }
        public List<UserList> UserList { get; set; }
        public List<ContractsDetails> ContractDetails { get; set; }
        public List<ddlClass> ddlClass { get; set; }
        public List<HierarchyList> Hierarchy_List { get; set; }
        public List<MultiBranchList> MultiBranchList { get; set; }
        public List<BankGuaranteeList> BankGuaranteeList { get; set; }
        public List<TermsConditionList> TermsConditionList { get; set; }


    }
    public class MultiBranchList
    {
        public Int64 PROJECT_ID { get; set; }
        public Int64 BRANCH_ID { get; set; }
    }
    public class  ProjectmanList
    {
        public string Promanager_id { get; set; }
        public string Promanager_Name { get; set; }
    }

    public class HierarchyList
    {
        public string Hierarchy_id { get; set; }
        public string Hierarchy_Name { get; set; }
    }

    public class ddlClass
    {
        public string Id { get; set; }
        public string Name { get; set; }

    }
    public class ProJectList
    {
        public Int64 Proj_Id { get; set; } 
        public string Proj_Code { get; set; }
        public string Proj_Name { get; set; }
        public string Customer { get; set; }
        public string CreatedName { get; set; }
        public string ModifiedName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ProjectStatus { get; set; }
        public DateTime? Proj_EstimateStartdate { get; set; }
        public DateTime? Proj_EstimateEnddate { get; set; }
        public DateTime? Proj_ActualStartdate { get; set; }
        public DateTime? Proj_ActualEndDate { get; set; }
        public string Cnt_InternalId { get; set; }
        public string BranchName { get; set; }
        public string projStage_Desc { get; set; }

        public String Hierarchy_Name { get; set; }
        public string MultiBranch { get; set; }
    }

    public class BankGuaranteeList
    {

        public Int64 BG_BankGuaranteId { get; set; }
        public Int64 Project_TermsId { get; set; }
        public string BG_BGGroup { get; set; }
        public string BG_BGType { get; set; }
        public decimal BG_Percentage { get; set; }
        public decimal BG_BGValue { get; set; }
        public string BG_BGStatus { get; set; }
        public DateTime? BG_BGValidfrom { get; set; }
        public DateTime? BG_BGValidUpTo { get; set; }
        public string Terms_BankGuaranteeSL { get; set; }
       

    }

    public class  TermsConditionList
    {
        public Int64 Project_TermsId { get; set; }
        public Int64 Doc_id { get; set; }
        public string Doc_Type { get; set; }
        public DateTime? Terms_DefectLibilityPeriodDate { get; set; }
        public DateTime? Terms_DefectLibilityPeriodToDate { get; set; }
        public string Terms_DefectLibilityPeriodRemarks { get; set; }
        public string Terms_LiqDamage { get; set; }
        public DateTime? Terms_LiqDamageAppDate { get; set; }
        public string Terms_Payment { get; set; }
        public string Terms_OrderType { get; set; }
        public string Terms_NatureWork { get; set; }
    
    }

    public class UserList
    {
        public decimal user_id { get; set; }
        public string user_name { get; set; }
    }
    public class ContractsDetails
    {
        public Int64 Details_ID { get; set; }
        public string Contract_No { get; set; }

    }

    public class BranchMapList
    {
        public Int64 Br_id { get; set; }
        public String Br_description { get; set; }
      
    }
    public class SalesOrderList
    {
        public Int64 OrderId { get; set; }
        public String SalesDocumentNumber { get; set; }
        public string Branch { get; set; }
        public DateTime? Order_Date { get; set; }
        public String CustomerName { get; set; }
        public String Order_RevisionNo { get; set; }
        public DateTime? Order_RevisionDate { get; set; }
    }
}