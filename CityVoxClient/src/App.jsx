import { BrowserRouter, Navigate, Routes, Route } from 'react-router-dom';
import { useMemo } from "react";
import { useSelector } from "react-redux";
import { CssBaseline, ThemeProvider } from "@mui/material";
import { createTheme } from "@mui/material/styles";
// import { themeSettings } from './theme';
import { createThemeSettings } from './theme/theme';
import RequireAuth from './components/RequireAuth';
import Register from './pages/auth/Register';

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
            <Route path="/auth/register"></Route>
            <Route path="/auth/login"></Route>
            <Route path="/unauthorized"></Route>
            {/*Protected routes*/}
            <Route element={<RequireAuth></RequireAuth>}>

            </Route>

            {/*catch all */}
            <Route path="*" ></Route>
          </Routes>
        </ThemeProvider>
      </BrowserRouter>
    </div>
  )
}

export default App
