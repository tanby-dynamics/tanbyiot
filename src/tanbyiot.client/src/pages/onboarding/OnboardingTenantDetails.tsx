import { useLocation, useNavigate } from "react-router-dom";
import { Button } from "@mui/material";
import { useState } from "react";

export function OnboardingTenantDetails() {
    const location = useLocation();
    const navigate = useNavigate();
    const [ name, setName ] = useState("");
    
    function goBack() {
        navigate("/onboarding/select-subscription-level", {
            state: {
                ...location.state
            }
        });
    }
    
    function goNext() {
        navigate("/onboarding/payment", {
            state: {
                ...location.state,
                name
            }
        })
    }
    
    return (
        <>
            <p>Give it a name</p>
            <label>Tenant name</label>
            <input type={"text"} onChange={(e) => setName(e.target.value)}/>
            <Button onClick={goBack}>Back</Button>
            <Button onClick={goNext} disabled={!name}>Next</Button>
        </>
    );

}