import MagnifyingGlassIcon from '@heroicons/react/24/solid/MagnifyingGlassIcon';
import { Card, InputAdornment, OutlinedInput, SvgIcon } from '@mui/material';

const UsersSearch = () => {
    return (
        <Card sx={{ p: 2 }}>
            <OutlinedInput
                defaultValue=""
                fullWidth
                placeholder="Search by username"
                startAdornment={(
                    <InputAdornment position="start">
                        <SvgIcon
                            color="action"
                            fontSize="small"
                        >
                            <MagnifyingGlassIcon />
                        </SvgIcon>
                    </InputAdornment>
                )}
                sx={{ maxWidth: 500 }}
            />
        </Card>
    )
}

export default UsersSearch;