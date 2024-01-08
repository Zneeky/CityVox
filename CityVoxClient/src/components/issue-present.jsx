import { Box, Card, CardContent, Divider, Typography, useTheme } from "@mui/material"
import { alpha } from "@mui/material";
import { indigo, success, warning, info } from "../theme/colors";
import { Link } from "react-router-dom";
import { useSelector } from "react-redux/es/hooks/useSelector";


const IssuePresent = ({ issue, type }) => {
    const appUser = useSelector((state) => state.user)
    const getStatusColor = () => {
        if (issue.Status === "Reported") {
            return `${warning.main}`; //red
        } else {
            return `${info.main}`;
        }
    };
    return (
        <Card>
            <CardContent>
                <Box sx={{ pb: "5px", display: "flex", alignItems: "center" }}>
                    <Typography
                        sx={{
                            m: "0 auto",
                            p: "2px 10px",
                            borderRadius: "20px",
                        }}
                    >
                        {issue.Municipality}
                    </Typography>
                </Box>
                <Box sx={{ pb: "5px", display: "flex", alignItems: "center" }}>
                    <Typography
                        sx={{
                            m: "0 auto",
                            backgroundColor: alpha(getStatusColor(), 0.3),
                            color: getStatusColor(),
                            p: "2px 10px",
                            borderRadius: "20px",
                        }}
                    >
                        {issue.Status}
                    </Typography>
                </Box>
                <Typography variant="h6">
                    {issue.Title}
                </Typography>
                <Box
                    component="img"
                    src={issue.ImageUrl}
                    sx={{
                        display: "flex",
                        justifyContent: "center",
                        pb: 3,
                        width: "300px",
                        height: "450px",
                        m: "auto",
                    }}
                ></Box>
                <Divider />
                <Typography variant="body2">
                    {issue.Description}
                </Typography>
                {(appUser.role==="admin" || appUser.username===issue.CreatorUsername) && 
                <Typography color="text.secondary" display="inline" variant="body2">
                    <Link
                        to={`/${type}/edit/${issue.Id}`}
                        style={{ textDecoration: "none", color: `${indigo.dark}` }}
                    >
                        Edit
                    </Link>
                </Typography>}
            </CardContent>
        </Card>
    )
}

export default IssuePresent