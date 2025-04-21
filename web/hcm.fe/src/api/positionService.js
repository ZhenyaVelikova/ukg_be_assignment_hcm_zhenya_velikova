import axios from 'axios';

const BASE = process.env.REACT_APP_API_BASE_URL;
const authHeader = () => {
  const token = localStorage.getItem('accessToken');
  return { Authorization: `Bearer ${token}` };
};

export async function getPositions() {
    const response = await axios.get(
      `${BASE}/api/positions/all`,
      { headers: authHeader() }
    );
    return response.data; 
  }
  