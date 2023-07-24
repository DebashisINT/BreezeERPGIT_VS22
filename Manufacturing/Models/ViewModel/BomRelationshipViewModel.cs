using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Manufacturing.Models.ViewModel.BOMEntryModel;

namespace Manufacturing.Models.ViewModel
{
    public class BomRelationshipViewModel
    {
        public String BOMRelation_ID { get; set; }
        public String BomRelationshipNo { get; set; }
        public List<BranchUnit> UnitList { get; set; }
        public String Unit { get; set; }
        public String BomRelationshipName { get; set; }
        public String ParentBOMID { get; set; }
        public String ParentBOMNo { get; set; }
        public String ParentBOMFG { get; set; }
        public String ParentBOMREV { get; set; }
        public String ChildBOMID { get; set; }
        public String ChildBOMFG { get; set; }
        public String ChildBOMREV { get; set; }
        public String ChildBOM_ID { get; set; }
        public String CreatedBy { get; set; }
        public String ModifyBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ModifyDate { get; set; }

        public String ProductId { get; set; }
        
    }
    public class ParentBOMList
    {
        public string Details_ID { get; set; }
        public string BOM_No { get; set; }
        public string ProductsName { get; set; }
        public string REVNo { get; set; }
    
    }

    public class udtChildBOM
    { 
        public Int64 ChildBOM_ID { get; set; }
        public string ChildBOM_FG { get; set; }
        public string ChildBOM_REV { get; set; }

    
    }
}