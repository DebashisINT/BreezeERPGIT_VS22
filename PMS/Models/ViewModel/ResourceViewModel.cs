using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMS.Models.ViewModel
{
    public class ResourceViewModel
    {
        public String resource_id { get; set; }
        public String resourceType { get; set; }
        public String Contact { get; set; }
        public String resourceName { get; set; }
        public String Branch { get; set; }
        public List<Units> BranchList { get; set; }
        public List<ResourecType> resourceTypeList { get; set; }
        public List<Contacts> ContactList { get; set; }

    }

    public class Contacts
    {
        public String id { get; set; }
        public String Name { get; set; }
    }
    public class ResourecType
    {
        public String ID { get; set; }
        public String RESOURCE_NAME { get; set; }
    }

    public class ResourecView
    {
        public long RESOURCE_ID { get; set; }
        public long RESOURCE_TypeID { get; set; }
        public String RESTYPE_NAME { get; set; }
        public String CONTACT { get; set; }
        public String RESOURCE_NAME { get; set; }
        public long BRANCH { get; set; }
        public long CREATE_BY { get; set; }
        public DateTime? CREATE_DATE { get; set; }
        public long UPDATE_BY { get; set; }
        public DateTime? UPDATE_DATE { get; set; }
        public String CREATE_NAME { get; set; }
        public String UPDATE_NAME { get; set; }
        public String RESOURCE_TypeName { get; set; }
        public String BRANCH_NAME { get; set; }
    }

    public class pmsPosProductModel
    {
        public string id { get; set; }
        public string Na { get; set; }
        public string Des { get; set; }
        public string HSN { get; set; }
    }

    public class CustomerModel
    {
        public string id { get; set; }
        public string Na { get; set; }
        public string UId { get; set; }
        public string add { get; set; }
    }
}