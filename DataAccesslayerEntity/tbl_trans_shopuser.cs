//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataAccesslayerEntity
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_trans_shopuser
    {
        public Nullable<long> User_Id { get; set; }
        public Nullable<long> Shop_Id { get; set; }
        public string Lat_visit { get; set; }
        public string Long_visit { get; set; }
        public long VisitId { get; set; }
        public string location_name { get; set; }
        public string distance_covered { get; set; }
        public Nullable<System.DateTime> SDate { get; set; }
        public string Stime { get; set; }
        public string shops_covered { get; set; }
    }
}