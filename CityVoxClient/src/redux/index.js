import { createSlice } from "@reduxjs/toolkit";

const initialState = {
  mode: "light",
  user: null,
  token: null,
};

export const authSlice = createSlice({
  name: "auth",
  initialState,
  reducers: {
    setMode: (state) => {
      state.mode = state.mode === "light" ? "dark" : "light";
    },
    setLogin: (state, action) => {
      state.user = action.payload.user;
      state.profile= action.payload.profile;
      state.token = action.payload.token;
    },
    setLogout: (state) => {
      state.user = null;
      state.profile= null;
      state.token = null;
    },
    
  },
});

export const { setMode, setLogin, setLogout} =
  authSlice.actions;
export default authSlice.reducer;