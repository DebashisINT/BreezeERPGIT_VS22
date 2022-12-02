import { configureStore } from '@reduxjs/toolkit'

import postsReducer  from './reducers/homeSlice'

export const store = configureStore({
  reducer: {
    posts: postsReducer
  }
})