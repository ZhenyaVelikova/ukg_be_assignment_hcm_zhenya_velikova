import axios from 'axios';

const BASE = process.env.REACT_APP_API_BASE_URL;
const authHeader = () => {
  const token = localStorage.getItem('accessToken');
  return { Authorization: `Bearer ${token}` };
};

// read‑only lookups for your dropdowns:
export async function getDepartments() {
    const response = await axios.get(
      `${BASE}/api/departments/all`,
      { headers: authHeader() }
    );
    return response.data;
  }