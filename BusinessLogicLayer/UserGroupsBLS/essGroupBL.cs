using DataAccessLayer;
using EntityLayer.CommonELS;
using EntityLayer.UserGroupsEL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.UserGroupsBLS
{
    public class essGroupHelperProcedures
    {
        public const string PROC_essGroups_Helper = "[dbo].[PROC_essGroups_Helper]";
        public const string PROC_USP_essGroups = "[dbo].[PROC_USP_essGroups]";
        public const string PROC_LST_ContactPerson = "[dbo].[PROC_LST_ContactPerson]";
    }

    public class essGroupBL
    {
        #region Public methods
        public List<GetUserGroupModel> FetchAllGroups()
        {
            ProcedureExecute Proc = new ProcedureExecute(essGroupHelperProcedures.PROC_essGroups_Helper);
            Proc.AddPara("@mode", PROC_essGroups_Helper_Modes.FetchAllGroups.ToString());
            DataTable dt = Proc.GetTable();
            return DbHelpers.ToModelList<GetUserGroupModel>(dt);
        }

        public DataTable FetchAllGroupsDataTable()
        {
            ProcedureExecute Proc = new ProcedureExecute(essGroupHelperProcedures.PROC_essGroups_Helper);
            Proc.AddPara("@mode", PROC_essGroups_Helper_Modes.FetchAllGroups.ToString());

            return Proc.GetTable();
        }

        public List<TranAccessByGroupModel> GetTranAccessByGroup(int GroupId)
        {
            ProcedureExecute Proc = new ProcedureExecute(essGroupHelperProcedures.PROC_essGroups_Helper);
            Proc.AddPara("@grp_id", GroupId);
            Proc.AddPara("@mode", PROC_essGroups_Helper_Modes.GetTranAccessByGroup.ToString());
            DataTable dt = Proc.GetTable();
            return DbHelpers.ToModelList<TranAccessByGroupModel>(dt);
        }

        public List<GroupUserListModel> GetUsersByGroupIdKeyValue(int GroupId)
        {
            ProcedureExecute Proc = new ProcedureExecute(essGroupHelperProcedures.PROC_essGroups_Helper);
            Proc.AddPara("@grp_id", GroupId);
            Proc.AddPara("@mode", PROC_essGroups_Helper_Modes.GetGroupTaggedUsers.ToString());
            DataTable dt = Proc.GetTable();
            return DbHelpers.ToModelList<GroupUserListModel>(dt);
        }

        public List<GetContactPersListModel> GetContactlistByIdKeyValue(string agentInternalId)
        {
            ProcedureExecute Proc = new ProcedureExecute(essGroupHelperProcedures.PROC_LST_ContactPerson);
            Proc.AddPara("@cp_agentInternalId", agentInternalId);
            DataTable dt = Proc.GetTable();
            return DbHelpers.ToModelList<GetContactPersListModel>(dt);
        }

        public CommonResult SaveUserGroupData(UserGroupSaveModel model)
        {
            ProcedureExecute Proc = new ProcedureExecute(essGroupHelperProcedures.PROC_USP_essGroups);

            Proc.AddPara("@CreateUser", model.CreateUser);
            Proc.AddPara("@grp_name", model.grp_name);
            Proc.AddPara("@grp_segmentId", model.grp_segmentId);
            Proc.AddPara("@LastModifyUser", model.LastModifyUser);
            Proc.AddPara("@UserGroupRights", model.UserGroupRights);
            Proc.AddPara("@mode", model.mode);
            Proc.AddPara("@ResultStat", SqlDbType.Bit, ParameterDirection.Output);
            Proc.AddPara("@ResultText", SqlDbType.VarChar, 100, ParameterDirection.Output);
            Proc.AddPara("@grp_id", SqlDbType.Int, ParameterDirection.InputOutput, model.grp_id);

            int res = Proc.RunActionQuery();

            object ResultStat = Proc.GetParaValue("@ResultStat");
            object grp_id = Proc.GetParaValue("@grp_id");
            object ResultText = Proc.GetParaValue("@ResultText");

            return new CommonResult()
            {
                IsSuccess = (ResultStat != null) ? Convert.ToBoolean(ResultStat) : false,
                AddonData = (grp_id != null) ? Convert.ToInt32(grp_id) : 0,
                Message = (ResultText != null) ? Convert.ToString(ResultText) : ""
            };
        }

        #endregion
    }
    public enum PROC_essGroups_Helper_Modes
    {
        FetchAllGroups = 1,
        GetTranAccessByGroup = 2,
        GetGroupTaggedUsers = 3
    }

    public enum PROC_USP_essGroups_Modes
    {
        INSERT = 1,
        UPDATE = 2,
        DELETE = 3
    }
}
