import { BrowserRouter, Navigate, Routes, Route } from "react-router-dom";
import { useMemo } from "react";
import { useSelector } from "react-redux";
import { CssBaseline, ThemeProvider } from "@mui/material";
import { createTheme } from "@mui/material/styles";
import { createThemeSettings } from "./theme/theme";
import RequireAuth from "./components/require-auth";
import Register from "./pages/auth/register-page";
import Login from "./pages/auth/login-page";
import Dashboard from "./pages/admin/dashboard-page";
import Home from "./pages/Home";
import CreateReport from "./pages/create-report";
import CreateEmergency from "./pages/create-emergency";
import CreateInfIssue from "./pages/create-inf-issue";
import "leaflet/dist/leaflet.css";
import Requests from "./pages/admin/requests-page";
import PostPage from "./pages/post/post-page";

function App() {
  const mode = useSelector((state) => state.mode);
  const theme = useMemo(() => createTheme(createThemeSettings(mode)), [mode]);
  const isAuthenticated = useSelector((state) => !!state.user?.username);

  return (
    <div className="app">
      <BrowserRouter>
        <ThemeProvider theme={theme}>
          <CssBaseline />
          <Routes>
            {/* If user is authenticated, redirect to /home */}
            {isAuthenticated ? (
              <>
                <Route path="/" element={<Navigate to="/home" />} />
                <Route path="/auth/login" element={<Navigate to="/home" />} />
                <Route
                  path="/auth/register"
                  element={<Navigate to="/home" />}
                />
              </>
            ) : (
              <>
                {/*Public routes for non-authenticated users*/}
                {/* <Route path="/" element={<Root />} /> */}
                <Route path="/auth/register" element={<Register />} />
                <Route path="/auth/login" element={<Login />} />
              </>
            )}

            {/*Protected routes for all types of users*/}
            <Route
              element={
                <RequireAuth
                  allowedRoles={["User", "Representative", "Admin"]}
                />
              }
            >
              <Route path="/"></Route>
              <Route path="/home" element={<Home />}></Route>
              <Route path="/report" element={<CreateReport />}></Route>
              <Route path="/emergency" element={<CreateEmergency />}></Route>
              <Route path="/events"></Route>
              <Route
                path="/infrastructure_issue"
                element={<CreateInfIssue />}
              ></Route>
              <Route path="/posts" element={<PostPage />}></Route>
            </Route>

            {/*Protected routes for security tier users*/}
            <Route element={<RequireAuth allowedRoles={["Admin"]} />}>
              <Route path="/admin/dashboard" element={<Dashboard />}></Route>
              <Route path="/admin/requests" element={<Requests />}></Route>
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
