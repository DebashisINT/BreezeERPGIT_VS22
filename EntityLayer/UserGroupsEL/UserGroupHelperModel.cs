using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.UserGroupsEL
{
    public class UserGroupHelperModel
    {
        public string mode { get; set; }
    }

    public class GetUserGroupModel
    {
        public int grp_id { get; set; }

        public string grp_name { get; set; }
    }

    public class TranAccessByGroupModel
    {
        public int MenuId { get; set; }
        public bool CanAdd { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
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
        public bool CreateOrder { get; set; }
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
        public bool TotalAssigned { get; set; }

        public bool RepairingPending { get; set; }
        public bool ServiceEntered { get; set; }
        public bool ServicePending { get; set; }
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

    public class UserGroupSaveModel
    {
        public int grp_id { get; set; }

        public int grp_segmentId { get; set; }

        public string grp_name { get; set; }

        public int? CreateUser { get; set; }

        public int? LastModifyUser { get; set; }

        public string UserGroupRights { get; set; }

        public string mode { get; set; }
    }

    public class GroupUserListModel
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public int UserGroupId { get; set; }
    }

    public class GetContactPersListModel
    {

        public string ContactId { get; set; }

        public string name { get; set; }
    }
}
