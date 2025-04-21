// src/api/userService.js
import axios from 'axios';

const BASE = process.env.REACT_APP_API_BASE_URL;
const authHeader = () => {
  const token = localStorage.getItem('accessToken');
  return { Authorization: `Bearer ${token}` };
};

export async function getPeople(page = 1, pageSize = 20) {
  const response = await axios.get(
    `${BASE}/api/users/people?page=${page}&pageSize=${pageSize}`,
    { headers: authHeader() }
  );
  return response.data; // IEnumerable<PeopleListResponseModel>
}

export async function getUser(id) {
  const response = await axios.get(
    `${BASE}/api/users/${id}`,
    { headers: authHeader() }
  );
  return response.data; // UserResponseModel
}

export async function createUser(body) {
  await axios.post(
    `${BASE}/api/users`,
    body,
    { headers: { ...authHeader(), 'Content-Type': 'application/json' } }
  );
}

export async function updateUser(id, body) {
  await axios.put(
    `${BASE}/api/users/${id}`,
    body,
    { headers: { ...authHeader(), 'Content-Type': 'application/json' } }
  );
}

export async function deleteUser(id) {
  await axios.delete(
    `${BASE}/api/users/${id}`,
    { headers: authHeader() }
  );
}

export async function changePassword(id, payload) {
  const token = localStorage.getItem('accessToken');
  await axios.post(
    `${BASE}/api/users/${id}/change-password`,
    payload,
    { headers: { Authorization: `Bearer ${token}` } }
  );
}

export async function getUsers() {

  const response = await axios.get(
    `${BASE}/api/users`,
    { headers: authHeader() }
  );
  return response.data; 
}
