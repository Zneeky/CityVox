import {
    Avatar,
    Box,
    Card,
    Checkbox,
    Stack,
    Table,
    TableBody,
    TableCell,
    TableHead,
    TablePagination,
    TableRow,
    Typography
} from '@mui/material';
import { useState } from 'react';
import UserRoleChangeDialog from './user-role-change-dialogue';

const UsersTable = ({ users, page, count, onPageChange }) => {

    const [openDialog, setOpenDialog] = useState(false);
    const [selectedUserName, setSelectedUserName] = useState(null);

    const handleRoleClick = (username) => {
        setSelectedUserName(username); // Set the selected user ID
        setOpenDialog(true); // Open the dialog
    };

    const handlePageChange = (event, newPage) => {
        onPageChange(newPage);
    };

    return (
        <>
            <Card>
                <Box sx={{ minWidth: 800 }}>
                    <Table>
                        <TableHead>
                            <TableRow>
                                <TableCell>
                                    Username
                                </TableCell>
                                <TableCell>
                                    Email
                                </TableCell>
                                <TableCell>
                                    Role
                                </TableCell>
                                <TableCell>
                                    Signed Up
                                </TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {users && users.map((user) => {
                                return (
                                    <TableRow key={user.$id}>
                                        <TableCell>
                                            <Stack
                                                alignItems="center"
                                                direction="row"
                                                spacing={2}
                                            >
                                                <Avatar src={user.ProfilePicture} />
                                                <Typography variant="subtitle2">
                                                    {user.Username}
                                                </Typography>
                                            </Stack>
                                        </TableCell>
                                        <TableCell>
                                            {user.Email}
                                        </TableCell>
                                        <TableCell
                                            onClick={() => handleRoleClick(user.Username)}
                                            style={{ cursor: 'pointer' }} // Change cursor to indicate clickable
                                        >
                                            {user.Role}
                                        </TableCell>
                                        <TableCell>
                                            {user.SignedUp}
                                        </TableCell>
                                    </TableRow>
                                )
                            })}
                        </TableBody>
                    </Table>
                    <TablePagination
                        component="div"
                        count={count}
                        onPageChange={handlePageChange}
                        page={page}
                        rowsPerPage={5}
                        onRowsPerPageChange={(event) => onPageChange(0)} // reset to first page if rows per page changes
                        rowsPerPageOptions={[5, 10, 25, 50, 100]}
                    />
                </Box>
            </Card>
            {/* User Role Change Dialog */}
            <UserRoleChangeDialog
                open={openDialog}
                onClose={() => setOpenDialog(false)}
                username={selectedUserName}
            />
        </>
    )
}

export default UsersTable;