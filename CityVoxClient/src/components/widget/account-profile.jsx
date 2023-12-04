import {
    Avatar,
    Box,
    Button,
    Card,
    CardActions,
    CardContent,
    Divider,
    Typography
} from '@mui/material';

import { useSelector } from 'react-redux/es/hooks/useSelector';


const AccountProfile = () => {
    const appUser = useSelector((state) => state.user);
    return (
        <Card>
            <CardContent>
                <Box
                    sx={{
                        alignItems: 'center',
                        display: 'flex',
                        flexDirection: 'column'
                    }}
                >
                    <Avatar
                        src={appUser.pfp}
                        sx={{
                            height: 80,
                            mb: 2,
                            width: 80
                        }}
                    />
                    <Typography
                        gutterBottom
                        variant="h5"
                    >
                        {appUser.username}
                    </Typography>
                    <Typography
                        color="text.secondary"
                        variant="body2"
                    >
                        {appUser.fName} {appUser.lName}
                    </Typography>
                </Box>
            </CardContent>
            <Divider />
            <CardActions>
                {/* <Button
                    fullWidth
                    variant="text"
                >
                    Upload picture
                </Button> */}
            </CardActions>
        </Card>
    );
}

export default AccountProfile;
