// src/components/LoginPage.jsx
import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  Box,
  Paper,
  TextField,
  Button,
  Typography,
  Alert,
  InputAdornment,
  IconButton
} from '@mui/material';
import { Visibility, VisibilityOff } from '@mui/icons-material';
import { signIn } from '../api/authService';

export default function LoginPage() {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [showPassword, setShow] = useState(false);
  const [error, setError] = useState(null);
  const navigate = useNavigate();

  const togglePassword = () => setShow(show => !show);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError(null);
    try {
      const tokenData = await signIn({ username, password });
      localStorage.setItem('accessToken', tokenData.accessToken);
      localStorage.setItem('refreshToken', tokenData.refreshToken);
      navigate('/people');
    } catch {
      setError('Invalid credentials');
    }
  };

  return (
    <Box
      sx={{
        display: 'flex',
        justifyContent: 'center',
        alignItems: 'center',
        minHeight: '100vh',
        px: 2,           // small horizontal padding on mobile
        bgcolor: 'background.default'
      }}
    >
      <Paper
        elevation={3}
        sx={{
          p: 4,
          width: '100%',
          maxWidth: 360   // ~360px wide card
        }}
      >
        <Box
          component="form"
          onSubmit={handleSubmit}
          sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}
        >
          <Typography variant="h5" align="center">
            Sign In
          </Typography>

          {error && <Alert severity="error">{error}</Alert>}

          <TextField
            label="Username"
            variant="outlined"
            value={username}
            onChange={e => setUsername(e.target.value)}
            required
            fullWidth
          />

          <TextField
            label="Password"
            variant="outlined"
            type={showPassword ? 'text' : 'password'}
            value={password}
            onChange={e => setPassword(e.target.value)}
            required
            fullWidth
            InputProps={{
              endAdornment: (
                <InputAdornment position="end">
                  <IconButton
                    onClick={togglePassword}
                    edge="end"
                    aria-label="toggle password visibility"
                  >
                    {showPassword ? <VisibilityOff /> : <Visibility />}
                  </IconButton>
                </InputAdornment>
              )
            }}
          />

          <Button
            type="submit"
            variant="contained"
            size="large"
            fullWidth
          >
            Log In
          </Button>
        </Box>
      </Paper>
    </Box>
  );
}
