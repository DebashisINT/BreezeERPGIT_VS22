using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.CommonELS
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

        // New rghts for Aprove Sales order
        public bool CanReadyToInvoice { get; set; }
        public bool CanMakeInvoice { get; set; }
        // End New rghts for Aprove Sales order

        // New rghts for Update Transporter
        public bool CanUpdateTransporter { get; set; }
        // End New rghts for Update Transporter

        // New rghts Tanmoy 03-02-2021
        public bool CanIRN { get; set; }
        public bool CanEWayBill { get; set; }
        public bool CanMRCancellation { get; set; }
        public bool CanWRCancellation { get; set; }
        public bool CanSTBRequisition { get; set; }
        public bool CanHolds { get; set; }
        public bool CanDirectorApproval { get; set; }
        public bool CanInventoryCancellation { get; set; }
        // End New rghts Tanmoy 03-02-2021

        public bool CanReturn { get; set; }//New rghts Priti 12-02-2021

        // New rghts Tanmoy 23-03-2021
        public bool CanPendingDispatch { get; set; }
        public bool CanDispatchAcknowledgment { get; set; }
        // End New rghts Tanmoy 23-03-2021
        // Mantis Issue 24211
        public bool CanCreateOpportunities { get; set; }
        public bool CanAutoCloseOpportunities { get; set; }
        public bool CanCloseOpportunities { get; set; }
        public bool CanReopenOpportunities { get; set; }

        // End of Mantis Issue 24211
        // Mantis Issue 24893
        public bool CanTotalAssigned { get; set; }
        public bool CanRepairingPending { get; set; }
        public bool CanServiceEntered { get; set; }
        public bool CanServicePending { get; set; }

        // End of Mantis Issue 24893

        public bool CanQuotationStatus { get; set; }
        public bool CanReOpen { get; set; }
        //Mantis Issue 25087
        public bool SendSMS { get; set; }
        //End of Mantis Issue 25087
        //Mantis Issue 0024702
        public bool UpdatePartyInvNoDT { get; set; }
        //End of Mantis Issue 0024702
    }
    public  class RightEL
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
