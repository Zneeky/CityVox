import { Box, Button, Container, Stack, SvgIcon, Typography } from "@mui/material";
import { TopNav } from "../../components/navigation/top-nav";
import { useCallback, useEffect, useState } from "react";
import { useSelector } from "react-redux/es/hooks/useSelector";
import { SideNav } from "../../components/navigation/side-nav";
import { useLocation } from "react-router-dom";
import { LayoutRoot, LayoutContainer, SIDE_NAV_WIDTH } from "../home-page";
import UsersSearch from "../../components/widget/users-search";
import UsersTable from "../../components/widget/users-table";
import { GetUsersCount, GetUsers } from "../../utils/api";


const ManageUsers = () => {
    const appUser = useSelector((state) => state.user)
    const location = useLocation();
    const pathname = location.pathname;
    const [openNav, setOpenNav] = useState(false);

    const [users, setUsers] = useState([]);
    const [page, setPage] = useState(0);
    const [count, setCount] = useState(0);

    const handlePathnameChange = useCallback(() => {
        if (openNav) {
            setOpenNav(false);
        }
    }, [openNav]);

    useEffect(
        () => {
            async function fetchUsersCount() {
                const usersCount = await GetUsersCount(appUser.accessToken);
                setCount(usersCount);
            }
            
            async function fetchUsers() {
                const usersData = await GetUsers(appUser.accessToken, page, 5); // here 5 is the rows per page
                setUsers(usersData);
            }

            fetchUsersCount();
            fetchUsers();
            handlePathnameChange();
        },
        // eslint-disable-next-line react-hooks/exhaustive-deps
        [pathname,page]
    );
    return (
        <>
            <TopNav onNavOpen={() => setOpenNav(true)} />
            <SideNav onClose={() => setOpenNav(false)} open={openNav} />
            <LayoutRoot>
                <LayoutContainer>
                    <Box
                        component="main"
                        sx={{
                            flexGrow: 1,
                            py: 8
                        }}
                    >
                        <Container maxWidth="xl">
                            <Stack spacing={3}>
                                <Stack
                                    direction="row"
                                    justifyContent="space-between"
                                    spacing={4}
                                >
                                    <Stack spacing={1}>
                                        <Typography variant="h4">
                                            CityVox Users
                                        </Typography>
                                    </Stack>
                                </Stack>
                                <UsersSearch />
                                <UsersTable users={users} page={page} count={count} onPageChange={setPage} />
                            </Stack>
                        </Container>
                    </Box>
                </LayoutContainer>
            </LayoutRoot>
        </>
    )
}

export default ManageUsers;