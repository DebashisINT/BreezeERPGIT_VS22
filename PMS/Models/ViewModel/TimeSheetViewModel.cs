using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMS.Models.ViewModel
{
    public class TimeSheetViewModel
    {
        public string TimeSheetID { get; set; }
        public string Action_type { get; set; }
        public DateTime StartDate { get; set; }  
        public string txtExternalComments { get; set; }
        public string txtDescription { get; set; }
        public string Time_Type { get; set; }
        public string Time_ProjectTask { get; set; }
        public string Duration { get; set; }
        public string Time_Project { get; set; }
        public string Time_Roll { get; set; }
        public string BranchID { get; set; }

        //public string DurationName { get; set; }
        //public string Time_TypeName { get; set; }
        //public string Time_ProjectTaskName { get; set; }
        //public string Time_ProjectName { get; set; }
        //public string Time_RollName { get; set; }

        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int is_pageload { get; set; }
        public string response_code { get; set; }
        public string response_msg { get; set; }
        public List<Projects> ProjectsList { get; set; }
        public List<Rolls> RollsList { get; set; }
        public List<Durations> DurationsList { get; set; }
        public List<ProTypes> TypesList { get; set; }
        public List<ProjectTasks> ProjectTasksList { get; set; }
        public List<Units> BranchList { get; set; }
        public string ListBranch { get; set; }
    }

    public class ProjectTasks
    {
        public string ProjectTask_ID { get; set; }
        public string ProjectTask_Name { get; set; }
    }

    public class ProTypes
    {
        public string Type_ID { get; set; }
        public string Type_Name { get; set; }
    }

    public class Durations
    {
        public string Duration_ID { get; set; }
        public string Time_Duration { get; set; }
    }


    public class Projects
    {
        public string Proj_Id { get; set; }
        public string Proj_Name { get; set; }
    }

    public class ProjectsTask
    {
        public string ProjectTask_ID { get; set; }
        public string ProjectTask_Name { get; set; }
    }

    public class Rolls
    {        
         public string ROLE_ID { get; set; }
         public string ROLE_NAME { get; set; }
    }

}