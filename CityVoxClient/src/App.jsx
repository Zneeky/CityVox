import { BrowserRouter, Navigate, Routes, Route } from "react-router-dom";
import { useMemo } from "react";
import { useSelector } from "react-redux";
import { CssBaseline, ThemeProvider } from "@mui/material";
import { createTheme } from "@mui/material/styles";
// import { themeSettings } from './theme';
import { createThemeSettings } from "./theme/theme";
import RequireAuth from "./components/RequireAuth";
import Register from "./pages/auth/Register";
import Login from "./pages/auth/Login";
import Dashboard from "./pages/admin/Dashboard";

function App() {
  const mode = useSelector((state) => state.mode);
  const theme = useMemo(() => createTheme(themeSettings(mode)), [mode]);
  const isAuth = Boolean(useSelector((state) => state.token));

  return (
    <div className="app">
      <BrowserRouter>
        <ThemeProvider theme={theme}>
          <CssBaseline />
          <Routes>
            {/*Public routes*/}
            <Route path="/auth/register" element={<Register />}></Route>
            <Route path="/auth/login" element={<Login />}></Route>
            <Route path="/unauthorized"></Route>
            {/*Protected routes for all types of users*/}
            <Route
              element={
                <RequireAuth
                  allowedRoles={["user", "representative", "admin"]}
                />
              }
            >
              <Route path="/"></Route>
              <Route path="/home"></Route>
              <Route path="/reports"></Route>
              <Route path="/emergencies"></Route>
              <Route path="/events"></Route>
              <Route path="/infrastructureIssues"></Route>
            </Route>

            {/*Protected routes for security tier users*/}
            <Route element={<RequireAuth allowedRoles={["admin"]} />}>
              <Route path="/admin/dashboard" element={<Dashboard />}></Route>
              <Route path="/admin/requests"></Route>
            </Route>
            {/*catch all */}
            <Route path="*"></Route>
          </Routes>
        </ThemeProvider>
      </BrowserRouter>
    </div>
  );
}

export default App;
