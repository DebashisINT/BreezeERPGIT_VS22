import { createSlice } from '@reduxjs/toolkit'

const initialState ={
  loading: false,
  hasErrors: false,
  rightsData: [
    {id:'5ddd', title:'some stuff'},
    {id:'5ddd', title:'some stuff'}
  ],
}
 
const postsSlice = createSlice({
  name: 'home',
  initialState,
  reducers: {
    getRights: state => {
      state.loading = true
    },
    getRightsSuccess: (state, { payload }) => {
      state.rightsData = payload
      state.loading = false
      state.hasErrors = false
    },
    getRightsFailure: state => {
      state.loading = false
      state.hasErrors = true
    },
  },
})

export default postsSlice.reducer

export const rightsSelector = state => state.recipes

// Asynchronous thunk action
export function fetchRights() {
  return async dispatch => {
    dispatch(getRights())

    try {
      const response = await fetch('https://www.themealdb.com/api/json/v1/1/search.php?s=')
      const data = await response.json()

      dispatch(getRightsSuccess(data))
    } catch (error) {
      dispatch(getRightsFailure())
    }
  }
}