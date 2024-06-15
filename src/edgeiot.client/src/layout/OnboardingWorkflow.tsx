import { useAuth0 } from "@auth0/auth0-react";
import {useUsersApi} from "../api/UsersApi.ts";
import { useQuery } from "@tanstack/react-query";
import {TenantOnboarding} from "../components/onboarding/TenantOnboarding.tsx";

export function OnboardingWorkflow(props: { children: any }) {
    const {
        isLoading: isAuth0UserLoading,
        isAuthenticated,
        loginWithRedirect
    } = useAuth0();
    const usersApi = useUsersApi();
    
    const {
        isPending: isUserPending,
        data: user
    } = useQuery({
        queryKey: ["user"],
        queryFn: usersApi.getCurrentUser,
        enabled: isAuthenticated
    });

    if (isAuth0UserLoading) {
        return null;
    }
    
    if (!isAuthenticated) {
        loginWithRedirect();
        return null;
    }    
    
    if (isUserPending || !user) {
        return null;
    }
    
    if (user.tenants.length === 0) {
        return <TenantOnboarding/>;
    }
    
    // Show the application    
    return props.children;
}