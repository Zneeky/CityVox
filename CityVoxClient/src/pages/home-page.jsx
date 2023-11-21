import { Button, Box } from "@mui/material"
import { LoginRefresh } from "../utils/api"
const HomePage = () => {

    
return(
    <Box>
            <Button variant="contained" color="primary" onClick={LoginRefresh}>
                Refresh Login
            </Button>
        </Box>
)
}

export default HomePage;