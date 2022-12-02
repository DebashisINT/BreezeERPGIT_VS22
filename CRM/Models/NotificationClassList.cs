using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Models
{
    public class NotificationListClass
    {
        public List<NotificationGridProperty> notificationList { get; set; }
    }
    public class notificationData
    {
        public string Notidication_date { get; set; }
        public List<int> Selecteduser { get; set; }
        public string Action { get; set; }
        public string every { get; set; }
        public string NOTIFICATION_ID { get; set; }
    }



    public class NotificationGridProperty
    {
        public string NOTIFICATION_ID { get; set; }
        public string NAME { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public DateTime MODIFIED_DATE { get; set; }

        public string DESCRIPTION { get; set; }


        public DateTime START_DATETIME { get; set; }
        public TimeSpan START_TIME { get; set; }

        public TimeSpan END_TIME { get; set; }


        public string INTERVAL { get; set; }

        public string ISACTIVE { get; set; }
    }


}