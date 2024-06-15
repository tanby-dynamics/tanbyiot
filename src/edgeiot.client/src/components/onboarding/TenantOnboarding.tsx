import {AppBar, Box, Button, Toolbar, Typography} from "@mui/material";
import {SubscriptionLevel} from "../../api/enums.ts";
import {useState} from "react";
import {useTenantsApi} from "../../api/TenantsApi.ts";
import { toast } from "react-toastify";
import { useNavigate } from "react-router-dom";

enum TenantOnboardingStep {
    SelectSubscription,
    GiveItAName,
    SetUpPayment
}

export function TenantOnboarding() {
    const [ step, setStep ] = useState(TenantOnboardingStep.SelectSubscription);
    const [ subscriptionLevel, setSubscriptionLevel ] = useState<SubscriptionLevel>();
    const [ name, setName ] = useState("");
    const tenantsApi = useTenantsApi();
    const navigate = useNavigate();
    
    function selectSubscriptionLevel(level: SubscriptionLevel) {
        setSubscriptionLevel(level);
        setStep(TenantOnboardingStep.GiveItAName);
    }
    
    async function createTenant() {
        try {
            await tenantsApi.addTenant({
                name,
                subscriptionLevel: subscriptionLevel!
            });
            toast.success("Created tenant");
            navigate("/");
        } catch (error) {
            console.error("Error creating tenant", error);
            toast.error("Error creating tenant");
        }
    }
    
    return (
        <Box sx={{ display: "flex" }}>
            <AppBar position={"fixed"}>
                <Toolbar>
                    <Typography variant={"h6"}>
                        edgeiot
                    </Typography>
                </Toolbar>
            </AppBar>
            <Box component={"main"}
                 sx={{ flexGrow: 1, p: 3}}>
                <Toolbar/>
                {step === TenantOnboardingStep.SelectSubscription && (
                    <>
                        <p>
                            show subscription levels, autoselect if it has previously been selected from the brochure
                            site
                        </p>
                        <Button onClick={() => selectSubscriptionLevel(SubscriptionLevel.Developer)}>Developer</Button>
                        <Button onClick={() => selectSubscriptionLevel(SubscriptionLevel.Startup)}>Startup</Button>
                        <Button onClick={() => selectSubscriptionLevel(SubscriptionLevel.Business)}>Business</Button>
                        <Button onClick={() => selectSubscriptionLevel(SubscriptionLevel.Industry)}>Industry</Button>
                    </>
                )}
                {step === TenantOnboardingStep.GiveItAName && (
                    <>
                        <p>Give it a name</p>
                        <label>Tenant name</label>
                        <input type={"text"} onChange={(e) => setName(e.target.value)}/>
                        <Button onClick={() => setStep(TenantOnboardingStep.SelectSubscription)}>Back</Button>
                        <Button onClick={() => setStep(TenantOnboardingStep.SetUpPayment)} disabled={!name}>Next</Button>
                    </>
                )}
                {step === TenantOnboardingStep.SetUpPayment && (
                    <>
                        <p>Set up payment</p>
                        <Button onClick={() => createTenant()}>Next</Button>
                        <Button onClick={() => setStep(TenantOnboardingStep.GiveItAName)}>Back</Button>
                        
                    </>
                )}
            </Box>
        </Box>
    );
}