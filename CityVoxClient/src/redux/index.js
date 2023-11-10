import { createSlice } from "@reduxjs/toolkit";

const initialState = {
  mode: "light",
  user: null,
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
    },
    setLogout: (state) => {
      state.user = null;
    },
    
  },
});

export const { setMode, setLogin, setLogout} =
  authSlice.actions;
export default authSlice.reducer;