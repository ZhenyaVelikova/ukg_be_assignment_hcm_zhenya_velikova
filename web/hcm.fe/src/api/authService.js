import axios from 'axios';

const BASE = process.env.REACT_APP_API_BASE_URL;

// Sign-in request model shape: { username, password }
export async function signIn({ username, password }) {
  const response = await axios.post(
    `${BASE}/api/SignIn`,
    { username, password }
  );
  return response.data; // expecting TokenResponseModel
}

// Refresh token: { token, refreshToken }
export async function refreshToken(model) {
  const response = await axios.post(
    `${BASE}/api/RefreshToken`,
    model
  );
  return response.data;
}
