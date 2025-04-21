// src/pages/Profile.jsx
import React, { useEffect, useState } from 'react';
import { Box, Typography, Button, CircularProgress, Alert } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import { jwtDecode } from 'jwt-decode';
import { getUser } from '../api/userService';

export default function Profile() {
    const [user, setUser] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState();
    const navigate = useNavigate();

    useEffect(() => {
        const token = localStorage.getItem('accessToken');
        if (!token) {
            setError('Not authenticated');
            setLoading(false);
            return;
        }
        const payload = jwtDecode(token);
        const id = payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier']
            || payload.nameid
            || payload.sub;
        if (!id) {
            setError('Invalid token');
            setLoading(false);
            return;
        }
        getUser(id)
            .then(setUser)
            .catch(() => setError('Failed to load profile'))
            .finally(() => setLoading(false));
    }, []);

    if (loading) return <CircularProgress />;
    if (error) return <Alert severity="error">{error}</Alert>;

    return (
        <Box sx={{ p: 4 }}>
            <Typography variant="h4">My Profile</Typography>
            <Typography><strong>Username:</strong> {user.userName}</Typography>
            <Typography><strong>Email:</strong> {user.person?.email}</Typography>
            <Typography><strong>Role:</strong> {user.roleName}</Typography>
            <Button
                variant="outlined"
                sx={{ mt: 2 }}
                onClick={() => navigate('/profile/change-password')}
            >
                Change Password
            </Button>
        </Box>
    );
}
