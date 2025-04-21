// src/pages/ChangePassword.jsx
import React, { useState } from 'react';
import { Box, TextField, Button, Alert, Typography } from '@mui/material';
import { changePassword } from '../api/userService';
import { useNavigate } from 'react-router-dom';

function parseJwt(token) {
    const base64 = token.split('.')[1].replace(/-/g, '+').replace(/_/g, '/');
    const json = decodeURIComponent(
        atob(base64)
            .split('')
            .map(c => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
            .join('')
    );
    return JSON.parse(json);
}

export default function ChangePassword() {
    const [current, setCurrent] = useState('');
    const [newPwd, setNewPwd] = useState('');
    const [confirm, setConfirm] = useState('');
    const [error, setError] = useState('');
    const [success, setSuccess] = useState(false);
    const navigate = useNavigate();

    const handleSubmit = async () => {
        setError('');
        if (!current || !newPwd) {
            setError('All fields are required');
            return;
        }
        if (newPwd !== confirm) {
            setError('Passwords do not match');
            return;
        }
        try {
            const token = localStorage.getItem('accessToken');
            if (!token) throw new Error('Not authenticated');

            const payload = parseJwt(token);
            // adjust the key if your JWT uses a different claim name
            const me = payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier']
                || payload.nameid
                || payload.sub;
            if (!me) throw new Error('Could not determine your user ID');

            await changePassword(me, {
                currentPassword: current,
                newPassword: newPwd
            });

            setSuccess(true);
            setTimeout(() => navigate('/profile'), 1500);
        } catch (e) {
            setError(e.response?.data || e.message);
        }
    };

    return (
        <Box sx={{ maxWidth: 400, mx: 'auto', mt: 4 }}>
            <Typography variant="h6">Change Password</Typography>
            {error && <Alert severity="error">{error}</Alert>}
            {success && <Alert severity="success">Password changed!</Alert>}
            <TextField
                label="Current Password"
                type="password"
                fullWidth
                margin="normal"
                value={current}
                onChange={e => setCurrent(e.target.value)}
            />
            <TextField
                label="New Password"
                type="password"
                fullWidth
                margin="normal"
                value={newPwd}
                onChange={e => setNewPwd(e.target.value)}
            />
            <TextField
                label="Confirm New Password"
                type="password"
                fullWidth
                margin="normal"
                value={confirm}
                onChange={e => setConfirm(e.target.value)}
            />
            <Button variant="contained" fullWidth sx={{ mt: 2 }} onClick={handleSubmit}>
                Change Password
            </Button>
        </Box>
    );
}
