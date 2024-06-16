import { Button } from "@mui/material";
import { useLocation, useNavigate } from "react-router-dom";
import {useTenantsApi} from "../../api/TenantsApi.ts";
import {SubscriptionLevel} from "../../api/enums.ts";
import { toast } from "react-toastify";
import { useQueryClient } from "@tanstack/react-query";
import {QueryKeys} from "../../api/constants.ts";

export function OnboardingPayment() {
    const location = useLocation();
    const navigate = useNavigate();
    const tenantsApi = useTenantsApi();
    const queryClient = useQueryClient();
    
    function goBack() {
        navigate("/onboarding/tenant-details", {
            state: {
                ...location.state
            }
        })
    }
    
    async function createTenant() {
        try {
            await tenantsApi.addTenant({
                name: location.state.name as string,
                subscriptionLevel: location.state.level as SubscriptionLevel
            });
            toast.success("Created tenant");
            await queryClient.refetchQueries({
                queryKey: [QueryKeys.User]    
            });
            navigate("/");
        } catch (error) {
            console.error("Error creating tenant", error);
            toast.error("Error creating tenant");
        }
    }
    
    return (
        <>
            <p>Set up payment</p>
            <Button onClick={() => createTenant()}>Next</Button>
            <Button onClick={goBack}>Back</Button>
        </>
    );
}