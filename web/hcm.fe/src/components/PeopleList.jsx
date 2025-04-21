import React, { useEffect, useState } from 'react';
import {
  Box,
  Typography,
  TableContainer,
  Table,
  TableHead,
  TableBody,
  TableRow,
  TableCell,
  Paper,
  CircularProgress,
  Alert,
  Button
} from '@mui/material';
import { useNavigate } from 'react-router-dom';
import {
  getPeople,
  getUser,
  createUser,
  updateUser,
  deleteUser
} from '../api/userService';

import PersonFormModal from './PersonFormModal';

export default function PeopleList() {
  const [people, setPeople] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState();
  const [openModal, setOpenModal] = useState(false);
  const [editingId, setEditingId] = useState(null);
  const [initialData, setInitialData] = useState(null);
  const navigate = useNavigate();

  const fetchPeople = async () => {
    setLoading(true);
    try {
      const data = await getPeople();
      setPeople(data);
    } catch (err) {
      console.error(err);
      setError('Failed to load people.');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchPeople();
  }, []);

  const handleAdd = () => {
    setInitialData(null);
    setEditingId(null);
    setOpenModal(true);
  };

  const handleEdit = async (personId) => {
    setLoading(true);
    try {
      const user = await getUser(personId);
      setInitialData(user);
      setEditingId(personId);
      setOpenModal(true);
    } catch (err) {
      console.error(err);
      setError('Failed to load person details.');
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (personId) => {
    if (!window.confirm('Are you sure you want to delete this person?')) return;
    try {
      await deleteUser(personId);
      fetchPeople();
    } catch (err) {
      console.error(err);
      setError('Failed to delete person.');
    }
  };

  const handleSave = async (formData) => {
    try {
      if (editingId) {
        await updateUser(editingId, formData);
      } else {
        await createUser(formData);
      }
      setOpenModal(false);
      fetchPeople();
    } catch (err) {
      console.error(err);
      setError('Failed to save person.');
    }
  };

  if (loading) return (
    <Box sx={{ display: 'flex', justifyContent: 'center', mt: 4 }}>
      <CircularProgress />
    </Box>
  );

  if (error) return <Alert severity="error">{error}</Alert>;

  return (
    <Box sx={{ p: 2 }}>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 2 }}>
        <Typography variant="h5">People</Typography>
        <Box>
          <Button variant="contained" color="primary" onClick={handleAdd} sx={{ mr: 1 }}>Add Person</Button>
          <Button variant="outlined" onClick={() => {
            localStorage.removeItem('accessToken');
            localStorage.removeItem('refreshToken');
            navigate('/');
          }}>Logout</Button>
        </Box>
      </Box>

      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Full Name</TableCell>
              <TableCell>Email</TableCell>
              <TableCell>Start Date</TableCell>
              <TableCell>Termination Date</TableCell>
              <TableCell>Position</TableCell>
              <TableCell>Department</TableCell>
              <TableCell>Actions</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {people.map(p => (
              <TableRow key={p.personId}>
                <TableCell>{p.fullName}</TableCell>
                <TableCell>{p.email}</TableCell>
                <TableCell>{new Date(p.startDate).toLocaleDateString()}</TableCell>
                <TableCell>{p.terminationDate ?? '—'}</TableCell>
                <TableCell>{p.positionName}</TableCell>
                <TableCell>{p.departmentName}</TableCell>
                <TableCell>
                  <Button size="small" onClick={() => handleEdit(p.personId)}>Edit</Button>
                  <Button size="small" color="error" onClick={() => handleDelete(p.personId)}>Delete</Button>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>

      <PersonFormModal
        open={openModal}
        onClose={() => setOpenModal(false)}
        onSave={handleSave}
        initialData={initialData}
      />
    </Box>
  );
}