// src/components/Layout.jsx
import React from 'react';
import { Box, Toolbar } from '@mui/material';
import Sidebar from './Sidebar';
import { Outlet } from 'react-router-dom';

export default function Layout() {
    return (
        <Box sx={{ display: 'flex' }}>
            <Sidebar />
            <Box component="main" sx={{ flexGrow: 1, p: 3 }}>
                <Toolbar />
                <Outlet />
            </Box>
        </Box>
    );
}
