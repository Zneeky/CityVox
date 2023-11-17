import { useLocation, Navigate, Outlet } from "react-router-dom";
import { useSelector } from "react-redux/es/hooks/useSelector";

const RequireAuth = ({allowedRoles}) => {
    const user = useSelector((state) => state.user)
    const location = useLocation();

    return(
        user?.role && allowedRoles?.includes(user?.role)
            ? <Outlet />
            : user?.fName
                    ?<Navigate to = "/unautorized" state={{from: location}} replace />
                    :<Navigate to = "/auth/login" state={{from: location}} replace />
    );
}

export default RequireAuth;
