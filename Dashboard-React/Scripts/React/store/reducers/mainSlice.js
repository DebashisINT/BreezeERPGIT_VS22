import { createSlice } from '@reduxjs/toolkit'
import axios from 'axios';
const initialState = {
    showComponent: 'home', 
    error:false
}

const mainSlice = createSlice({
    name: 'main',
    initialState,
    reducers: {
        defaultSer: state => {
            state.showComponent = 'service'
        },
        defaultSTB: state => {
            state.showComponent = 'STB'
        },
        defaultHome: state => {
            state.showComponent = 'home'
        },
        Changecomponent: (state, { payload }) => {
            state.showComponent = payload
        },
        error: state =>{
            state.error = true
        }
    },
})

export default postsSlice.reducer
export const { } = postsSlice.actions


export async function swithToService (data) {
    return async dispatch => {
        const res = await  axios({ method: 'post', url: '../ajax/ServiceManagement/ServiceMng.aspx/SrvSession', data: { comment : data }})
        if(res.data.status == 200) {
            dispatch(defaultSer())
        }else{
            dispatch(error())
        }
        
    }  
}
export async function swithToErp (data) {
    return async dispatch => {
        const res = await  axios({ method: 'post', 
        url: '../ajax/ServiceManagement/ServiceMng.aspx/SrvSession', 
        data: { comment : data }})
        if(res.data.status == 200) {
            dispatch(defaultHome())
        }else{
            dispatch(error())
        }
        
    }  
}
  



