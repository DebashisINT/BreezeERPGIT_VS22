import { createSlice } from '@reduxjs/toolkit'
import axios from 'axios';

const initialState ={
  loading: false,
  hasErrors: false,
  rightsData:[],
  // pulled states
  approvalCount: 0,
  mngCount: 0,
  skeletenLoad: false,
  homePermissions: false,
  notifyCalled: false,
  announcement: [],
  showTickerA: false,
  permissions: {
      AccountsBtn: false,
      Attbtn: false,
      CRMButton: false,
      CUSTButton: false,
      CustRMButton: false,
      FinancialButton: false,
      PMSButton: false,
      PurchaseDbButton: false,
      SalesDbButton: false,
      dhnDashBoardSession: "",
      divPopHead: "",
      dvApprovalWaiting: false,
      dvInveDashboard: false,
      dvKPISummary: false,
      dvManagementNotification: false,
      followupBtn: false,
      hdnDefaultDashboardService: "",
      tasklistbtn: false,  
  },
  STB: false,
  CustomerProfile: true,
  NewsTickerPage: true
}
 
const postsSlice = createSlice({
  name: 'posts',
  initialState,
  reducers: {
    getRights: state => {
      state.loading = true
    },
    getLoading: state => {
      state.skeletenLoad = true
    },
    getRightsSuccess: (state, { payload }) => {
      state.rightsData = payload
      state.loading = false
      state.hasErrors = false
    },
    getFailure: state => {
      state.loading = false
      state.hasErrors = true
    },
    getAnnSuccess: (state, { payload }) =>{
      state.announcement= payload,
      state.showTickerA = true,
      state.loading= false
    },
    getpermissionSuccess: (state, { payload }) =>{
      state.permissions = payload,
      state.skeletenLoad= false
    },
    setpermissionFromLocal: (state, { payload }) =>{
      state.skeletenLoad= false,
      state.permissions = payload
    },
    setNotifyCounter: (state, { payload }) =>{
      console.log(payload)
      state.approvalCount= payload.n,
      state.mngCount= payload.o
    },
  },
})

export default postsSlice.reducer
export const { getRights, getRightsSuccess, getFailure, getAnnSuccess, getpermissionSuccess, getLoading, setpermissionFromLocal, setNotifyCounter } = postsSlice.actions

export const rightsSelector = state => state.rightsData

// Asynchronous thunk action
// notification counts
export function GetAllNotificationsAcction() {
  return async dispatch => {
      
      const ApprovalWaitingData = await axios({
        method: 'post',
        url: '../ajax/approvalWaiting/approvalWaiting.aspx/getAllApprovalWaitingData',
        data: {
            action: "ALLCount"
        }
      })
      const NotificationData = await axios({
        method: 'post',
        url: '../ajax/mngNotification/mngNotification.aspx/GetAllNotificationData',
        data: {
            action: "ALL"
        }
      })
      console.log('ApprovalWaitingData', ApprovalWaitingData)
      console.log('NotificationData', NotificationData)
      if(ApprovalWaitingData.status == 200){
        const ApprovalCount = getCounter(ApprovalWaitingData.data.d[0])
        const NotificationCount = getCounter(NotificationData.data.d[0])
        
        dispatch(setNotifyCounter({n: ApprovalCount, o: NotificationCount}))
      }else{
        dispatch(getFailure())
      }
  }
}
function getCounter(data){
  let obj = data;
  let total = 0;
  for (var el in obj) {
      if (obj.hasOwnProperty(el)) {
          total += parseFloat(obj[el]);
      }
  }
  return total;
}


//permissions
export function getDashboardRedirect() {
  return async dispatch => {
      dispatch(getLoading())
      const response = await axios({method: 'post', url: '../ajax/permissions/permissions.aspx/getDashboardRedirect',data: {}})
      if(response.status == 200){
        dispatch(getLoading())
        dispatch(getpermissionSuccess(response.data.d))
      }else{
        dispatch(getFailure())
      }
  }
}


// get announcement
export function GetAnnouncement() {
  return async dispatch => {
    //dispatch(getRights())
   
      const response = await axios({method: 'post', url: '../ajax/announcement/announcement.aspx/GetMyAnnouncement',data: {}})
      console.log('AnnouncementData', response.data.d)
      //const data = response.data.d
      if(response.status == 200){
        dispatch(getAnnSuccess(response.data.d))
      }else{
        dispatch(getFailure())
      }
      
  }
}
