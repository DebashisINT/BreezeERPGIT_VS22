using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BreezeChat.controls
{
    public class CommonEL
    {
        public int? userid { get; set; }

        public int? usergroupid { get; set; }

        public string url { get; set; }
    }

    public class UserRightsForPage
    {
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
        public bool CanAdd { get; set; }
        public bool CanView { get; set; }
        public bool CanIndustry { get; set; }
        public bool CanCreateActivity { get; set; }
        public bool CanContactPerson { get; set; }
        public bool CanHistory { get; set; }
        public bool CanAddUpdateDocuments { get; set; }
        public bool CanMembers { get; set; }
        public bool CanOpeningAddUpdate { get; set; }
        public bool CanAssetDetails { get; set; }
        public bool CanExport { get; set; }
        public bool CanPrint { get; set; }
        public bool CanBudget { get; set; }
        public bool CanAssignbranch { get; set; }
        public bool Cancancelassignmnt { get; set; }
        public bool CanReassignSupervisor { get; set; }
        public bool CanClose { get; set; }
        public bool CanSpecialEdit { get; set; }
        public bool CanCancel { get; set; }
        public bool CanCreateOrder { get; set; }
        public bool Imagaeupload { get; set; }
        public bool RePrintBarcode { get; set; }
        public bool DocumentCollection { get; set; }
        public bool ClosedSales { get; set; }
        public bool FutureSales { get; set; }
        public bool ClarificationRequired { get; set; }
        public bool CanReassignSalesman { get; set; }
        public bool CanViewAdjustment { get; set; }
        public bool SupervisorFeedback { get; set; }
        public bool SalesmanFeedback { get; set; }
        public bool Verified { get; set; }
        public bool Influencer { get; set; }
        public bool CanRestore { get; set; }
        public bool CanAssignTo { get; set; }
        public bool CanConvertTo { get; set; }
        public bool CanSalesActivity { get; set; }
        public bool CanApproved { get; set; }


        // New rghts for CRM
        public bool CanQualify { get; set; }
        public bool CanCancelLost { get; set; }
        public bool CanSharing { get; set; }
        public bool CanProducts { get; set; }
        public bool CanLiterature { get; set; }
        // End New rghts for CRM
    }
    public class RightEL
    {
        public RightEL()
        {
            //this.Map_UserGroup_Rights = new HashSet<Map_UserGroup_RightsEL>();
        }

        public int Id { get; set; }
        public string Rights { get; set; }
        public Nullable<bool> IsActive { get; set; }

    }
}