using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Models
{
    public class NotificationClass
    {
        public List<UserNotificationList> UserList { get; set; }

        public List<UserNotificationList> SelectedUser { get; set; }

        public List<SupervisorList> SupervisorList { get; set; }
        public List<StateList> StateList { get; set; }
        public string ddlAction { get; set; }
        public List<ActionLists> ActionList { get; set; }
        public bool IsActive { get; set; }
    }

    public class NotificationData
    {
        public string Notidication_date { get; set; }
        public string Action { get; set; }
        public string every { get; set; }
        public List<int> Selecteduser { get; set; }
        public string NOTIFICATION_ID { get; set; }
        public string starttime { get; set; }
        public string endtime { get; set; }
        public string IsActive { get; set; }
    }

    public class ActionLists
    {
        public string ActionID { get; set; }
        public string actionname { get; set; }
    }
    public class StateList
    {
        public string ID { get; set; }
        public string Name { get; set; }
    }
    public class SupervisorList
    {
        public string ID { get; set; }
        public string Name { get; set; }
    }
    public class UserNotificationList
    {
        public string UserID { get; set; }
        public string username { get; set; }
    }

}