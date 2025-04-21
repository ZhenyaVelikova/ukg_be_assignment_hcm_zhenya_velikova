import React from 'react';
import { Drawer, List, ListItemButton, ListItemIcon, ListItemText, Toolbar } from '@mui/material';
import PersonIcon from '@mui/icons-material/Person';
import GroupIcon from '@mui/icons-material/Group';
import { useNavigate, useLocation } from 'react-router-dom';

const drawerWidth = 240;

export default function Sidebar() {
    const navigate = useNavigate();
    const { pathname } = useLocation();

    const items = [
        { text: 'My Profile', icon: <PersonIcon />, path: '/profile' },
        { text: 'People List', icon: <GroupIcon />, path: '/people' }
    ];

    return (
        <Drawer
            variant="permanent"
            sx={{
                width: drawerWidth,
                flexShrink: 0,
                [`& .MuiDrawer-paper`]: { width: drawerWidth, boxSizing: 'border-box' },
            }}
        >
            <Toolbar />
            <List>
                {items.map(({ text, icon, path }) => (
                    <ListItemButton
                        key={text}
                        selected={pathname === path}
                        onClick={() => navigate(path)}
                    >
                        <ListItemIcon>{icon}</ListItemIcon>
                        <ListItemText primary={text} />
                    </ListItemButton>
                ))}
            </List>
        </Drawer>
    );
}
