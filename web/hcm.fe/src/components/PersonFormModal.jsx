// src/components/PersonFormModal.jsx
import React, { useState, useEffect } from 'react';
import {
    Dialog,
    DialogTitle,
    DialogContent,
    DialogActions,
    Button,
    TextField,
    FormControl,
    InputLabel,
    Select,
    MenuItem,
    FormHelperText,
    Grid
} from '@mui/material';
import { getUsers } from '../api/userService';
import { getDepartments } from '../api/departmentService';
import { getPositions } from '../api/positionService';
import { getRoles } from '../api/roleService';

const USERNAME_MAX = 50;
const FIRSTNAME_MAX = 50;
const LASTNAME_MAX = 50;
const EMAIL_MAX = 100;

export default function PersonFormModal({ open, onClose, onSave, initialData }) {
    const isEdit = Boolean(initialData);
    const [form, setForm] = useState({
        userName: '',
        password: '',
        isActive: true,
        roleId: '',
        person: {
            firstName: '',
            lastName: '',
            email: '',
            departmentId: '',
            positionId: '',
            startDate: '',
            terminationDate: '',
            personType: 0,
            reportsToId: ''
        }
    });
    const [errors, setErrors] = useState({});
    const [depts, setDepts] = useState([]);
    const [pos, setPos] = useState([]);
    const [users, setUsers] = useState([]);
    const [roles, setRoles] = useState([]);

    const PersonType = { Internal: 1, External: 2 };

    useEffect(() => {
        Promise.all([getDepartments(), getPositions(), getUsers(), getRoles()])
            .then(([d, p, u, r]) => {
                setDepts(d);
                setPos(p);
                setUsers(u);
                setRoles(r);
            });
    }, []);

    useEffect(() => {
        if (initialData) {
            setForm({
                userName: initialData.userName,
                password: '',
                isActive: initialData.isActive,
                roleId: initialData.roleId,
                person: {
                    firstName: initialData.person.firstName,
                    lastName: initialData.person.lastName,
                    email: initialData.person.email,
                    departmentId: initialData.person.departmentId,
                    positionId: initialData.person.positionId,
                    startDate: initialData.person.startDate.split('T')[0],
                    terminationDate: initialData.person.terminationDate
                        ? initialData.person.terminationDate.split('T')[0]
                        : '',
                    personType: initialData.person.personType,
                    reportsToId: initialData.person.reportsToId || ''
                }
            });
            setErrors({});
        } else {
            setForm({
                userName: '',
                password: '',
                isActive: true,
                roleId: '',
                person: {
                    firstName: '',
                    lastName: '',
                    email: '',
                    departmentId: '',
                    positionId: '',
                    startDate: '',
                    terminationDate: '',
                    personType: 0,
                    reportsToId: ''
                }
            });
            setErrors({});
        }
    }, [initialData, open]);

    const handleChange = (field, value) => {
        setForm(f => ({ ...f, [field]: value }));
        setErrors(e => ({ ...e, [field]: null }));
    };

    const handlePersonChange = (field, value) => {
        setForm(f => {
            const updated = { ...f.person, [field]: value };
            let next = { ...f, person: updated };
            if (field === 'email' && !isEdit) {
                next.userName = value;
                setErrors(e => ({ ...e, userName: null }));
            }
            return next;
        });
        setErrors(e => ({ ...e, [field]: null }));
    };

    const handleSubmit = () => {
        const e = {};
        if (!form.userName.trim()) e.userName = 'Username is required';
        else if (form.userName.length > USERNAME_MAX) e.userName = `Max ${USERNAME_MAX} chars`;
        if (!isEdit && !form.password) e.password = 'Password is required';
        if (!form.roleId) e.roleId = 'Role is required';

        const p = form.person;
        if (!p.firstName.trim()) e.firstName = 'First name is required';
        else if (p.firstName.length > FIRSTNAME_MAX) e.firstName = `Max ${FIRSTNAME_MAX} chars`;
        if (!p.lastName.trim()) e.lastName = 'Last name is required';
        else if (p.lastName.length > LASTNAME_MAX) e.lastName = `Max ${LASTNAME_MAX} chars`;
        if (!p.email.trim()) e.email = 'Email is required';
        else if (p.email.length > EMAIL_MAX) e.email = `Max ${EMAIL_MAX} chars`;
        else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(p.email)) e.email = 'Invalid email format';
        if (!p.departmentId) e.departmentId = 'Department is required';
        if (!p.positionId) e.positionId = 'Position is required';
        if (!p.startDate) e.startDate = 'Start date is required';
        if (!Object.values(PersonType).includes(p.personType)) e.personType = 'Select a valid type';

        if (Object.keys(e).length > 0) {
            setErrors(e);
            return;
        }

        const payload = {
            userName: form.userName,
            password: form.password,
            isActive: form.isActive,
            roleId: form.roleId,
            person: {
                firstName: p.firstName,
                lastName: p.lastName,
                email: p.email,
                departmentId: p.departmentId,
                positionId: p.positionId,
                startDate: p.startDate,
                terminationDate: p.terminationDate || null,
                personType: p.personType,
                reportsToId: p.reportsToId || null
            }
        };
        onSave(payload);
    };

    return (
        <Dialog open={open} onClose={onClose} fullWidth maxWidth="sm">
            <DialogTitle>{isEdit ? 'Edit Person' : 'Add Person'}</DialogTitle>
            <DialogContent dividers>
                <Grid container spacing={2}>
                    {!isEdit && (
                        <Grid item xs={6}>
                            <TextField
                                label="Password"
                                type="password"
                                fullWidth
                                required
                                value={form.password}
                                onChange={e => handleChange('password', e.target.value)}
                                error={!!errors.password}
                                helperText={errors.password}
                            />
                        </Grid>
                    )}
                    <Grid item xs={6}>
                        <FormControl fullWidth required error={!!errors.roleId}>
                            <InputLabel id="role-label">Role</InputLabel>
                            <Select
                                labelId="role-label"
                                id="role"
                                value={form.roleId}
                                label="Role"
                                onChange={e => handleChange('roleId', e.target.value)}
                            >
                                <MenuItem value="" disabled>Select role</MenuItem>
                                {roles.map(r => (
                                    <MenuItem key={r.id} value={r.id}>{r.name}</MenuItem>
                                ))}
                            </Select>
                            <FormHelperText>{errors.roleId}</FormHelperText>
                        </FormControl>
                    </Grid>
                    <Grid item xs={6}>
                        <TextField
                            label="First Name"
                            fullWidth
                            required
                            value={form.person.firstName}
                            onChange={e => handlePersonChange('firstName', e.target.value)}
                            error={!!errors.firstName}
                            helperText={errors.firstName}
                        />
                    </Grid>
                    <Grid item xs={6}>
                        <TextField
                            label="Last Name"
                            fullWidth
                            required
                            value={form.person.lastName}
                            onChange={e => handlePersonChange('lastName', e.target.value)}
                            error={!!errors.lastName}
                            helperText={errors.lastName}
                        />
                    </Grid>
                    <Grid item xs={12}>
                        <TextField
                            label="Email"
                            type="email"
                            fullWidth
                            required
                            value={form.person.email}
                            onChange={e => handlePersonChange('email', e.target.value)}
                            error={!!errors.email}
                            helperText={errors.email}
                        />
                    </Grid>
                    <Grid item xs={6}>
                        <FormControl fullWidth required error={!!errors.departmentId}>
                            <InputLabel id="department-label">Department</InputLabel>
                            <Select
                                labelId="department-label"
                                id="department"
                                value={form.person.departmentId}
                                label="Department"
                                onChange={e => handlePersonChange('departmentId', e.target.value)}
                            >
                                <MenuItem value="" disabled>Select department</MenuItem>
                                {depts.map(d => (
                                    <MenuItem key={d.id} value={d.id}>{d.name}</MenuItem>
                                ))}
                            </Select>
                            <FormHelperText>{errors.departmentId}</FormHelperText>
                        </FormControl>
                    </Grid>
                    <Grid item xs={6}>
                        <FormControl fullWidth required error={!!errors.positionId}>
                            <InputLabel id="position-label">Position</InputLabel>
                            <Select
                                labelId="position-label"
                                id="position"
                                value={form.person.positionId}
                                label="Position"
                                onChange={e => handlePersonChange('positionId', e.target.value)}
                            >
                                <MenuItem value="" disabled>Select position</MenuItem>
                                {pos.map(p => (
                                    <MenuItem key={p.id} value={p.id}>{p.name}</MenuItem>
                                ))}
                            </Select>
                            <FormHelperText>{errors.positionId}</FormHelperText>
                        </FormControl>
                    </Grid>
                    <Grid item xs={6}>
                        <FormControl fullWidth required error={!!errors.personType}>
                            <InputLabel id="person-type-label">Person Type</InputLabel>
                            <Select
                                labelId="person-type-label"
                                id="person-type"
                                value={form.person.personType}
                                label="Person Type"
                                onChange={e => handlePersonChange('personType', e.target.value)}
                            >
                                <MenuItem value="" disabled>Select type</MenuItem>
                                {Object.entries(PersonType).map(([label, val]) => (
                                    <MenuItem key={val} value={val}>{label}</MenuItem>
                                ))}
                            </Select>
                            <FormHelperText>{errors.personType}</FormHelperText>
                        </FormControl>
                    </Grid>
                    <Grid item xs={6}>
                        <FormControl fullWidth>
                            <InputLabel id="reports-to-label">Reports To</InputLabel>
                            <Select
                                labelId="reports-to-label"
                                id="reports-to"
                                value={form.person.reportsToId}
                                label="Reports To"
                                onChange={e => handlePersonChange('reportsToId', e.target.value)}
                            >
                                <MenuItem value=""><em>None</em></MenuItem>
                                {users.map(u => (
                                    <MenuItem key={u.id} value={u.id}>{u.userName}</MenuItem>
                                ))}
                            </Select>
                        </FormControl>
                    </Grid>
                    <Grid item xs={6}>
                        <TextField
                            label="Start Date"
                            type="date"
                            fullWidth
                            required
                            InputLabelProps={{ shrink: true }}
                            value={form.person.startDate}
                            onChange={e => handlePersonChange('startDate', e.target.value)}
                            error={!!errors.startDate}
                            helperText={errors.startDate}
                        />
                    </Grid>
                    <Grid item xs={6}>
                        <TextField
                            label="Termination Date"
                            type="date"
                            fullWidth
                            InputLabelProps={{ shrink: true }}
                            value={form.person.terminationDate}
                            onChange={e => handlePersonChange('terminationDate', e.target.value)}
                        />
                    </Grid>
                </Grid>
            </DialogContent>
            <DialogActions>
                <Button onClick={onClose}>Cancel</Button>
                <Button variant="contained" onClick={handleSubmit}>
                    {isEdit ? 'Save' : 'Add'}
                </Button>
            </DialogActions>
        </Dialog>
    );
}
