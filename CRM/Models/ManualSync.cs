using CRM.Models.DataContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CRM.Models
{
    public class TaskCreationClass
    {
        public List<UserTaskCreationList> UserList { get; set; }

        public List<UserTaskCreationList> SelectedUser { get; set; }


        public List<ActionList> ActionList { get; set; }
        public bool IsActive { get; set; }
        public string ddlAction { get; set; }
        public string every { get; set; }
        public string TaskCreation_ID { get; set; }

        public string TASK_SUBJECT { get; set; }

        public string TASK_DESCRIPTION { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public DateTime? start_date { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public DateTime? due_date { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ddlpriority { get; set; }

        public string flag { get; set; }

        public string ddlday { get; set; }
        public string start_day { get; set; }



    }

    public class TaskCreationData
    {

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public DateTime? start_date { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public DateTime? due_date { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Action { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string every { get; set; }
        public List<int> Selecteduser { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string TaskCreation_ID { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string IsActive { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]

        public string TASK_SUBJECT { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]

        public string TASK_DESCRIPTION { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ddlpriority { get; set; }

        public string ddlday { get; set; }
        public string start_day { get; set; }



    }

    public class ActionList
    {
        public string ActionID { get; set; }
        public string actionname { get; set; }
    }

    public class UserTaskCreationList
    {
        public string UserID { get; set; }
        public string username { get; set; }
    }

}