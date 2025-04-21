// src/App.jsx
import React from 'react';
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { ThemeProvider, createTheme, CssBaseline } from '@mui/material';
import LoginPage from './components/LoginPage';
import PeopleList from './components/PeopleList';
import Profile from './components/Profile';
import ChangePassword from './components/ChangePassword';
import Layout from './components/Layout';

const theme = createTheme({
  palette: {
    primary: { main: '#1976d2' },
    secondary: { main: '#dc004e' },
  },
});

function PrivateRoute({ children }) {
  return localStorage.getItem('accessToken')
    ? children
    : <Navigate to="/" replace />;
}

export default function App() {
  return (
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <BrowserRouter>
        <Routes>

          {/* public */}
          <Route path="/" element={<LoginPage />} />

          {/* protected layout */}
          <Route
            element={
              <PrivateRoute>
                <Layout />
              </PrivateRoute>
            }
          >
            <Route path="profile" element={<Profile />} />
            <Route path="profile/change-password" element={<ChangePassword />} />
            <Route path="people" element={<PeopleList />} />
          </Route>

          {/* fallback */}
          <Route path="*" element={<Navigate to="/" replace />} />

        </Routes>
      </BrowserRouter>
    </ThemeProvider>
  );
}
