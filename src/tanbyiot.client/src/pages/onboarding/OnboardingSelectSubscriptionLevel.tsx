import { Button } from "@mui/material";
import {SubscriptionLevel} from "../../api/enums.ts";
import {useLocation, useNavigate } from "react-router-dom";

export function OnboardingSelectSubscriptionLevel() {
    const navigate = useNavigate();
    const location = useLocation();
    
    function selectSubscriptionLevel(level: SubscriptionLevel) {
        navigate("/onboarding/tenant-details", {
            state: {
                ...location.state,
                level
            }
        });
    }
    
    return (
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
    );
}