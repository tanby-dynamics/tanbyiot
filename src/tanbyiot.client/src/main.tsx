import React from 'react'
import ReactDOM from 'react-dom/client'
import {RouterProvider, createBrowserRouter } from 'react-router-dom'
import '@fontsource/roboto/300.css';
import '@fontsource/roboto/400.css';
import '@fontsource/roboto/500.css';
import '@fontsource/roboto/700.css';
import {Devices} from "./pages/Devices.tsx";
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import {DeviceDetails} from "./pages/DeviceDetails.tsx";
import {Rules} from "./pages/Rules.tsx";
import {RuleDetails} from "./pages/RuleDetails.tsx";
import 'react-toastify/dist/ReactToastify.css';
import { ToastContainer } from 'react-toastify';
import { Auth0Provider, withAuthenticationRequired } from '@auth0/auth0-react';
import { CssBaseline, LinearProgress } from '@mui/material';
import {Root} from "./layout/Root.tsx";
import {Tenants} from "./pages/Tenants.tsx";
import {OnboardingSelectSubscriptionLevel} from "./pages/onboarding/OnboardingSelectSubscriptionLevel.tsx";
import {OnboardingTenantDetails} from "./pages/onboarding/OnboardingTenantDetails.tsx";
import {OnboardingPayment} from "./pages/onboarding/OnboardingPayment.tsx";
import {AdminUsers} from "./pages/admin/AdminUsers.tsx";
import {AdminOverview} from "./pages/admin/AdminOverview.tsx";
import {AdminTenants} from "./pages/admin/AdminTenants.tsx";
import {Overview} from "./pages/Overview.tsx";

// @ts-ignore
function AuthGuard({ component }) {
    const Component = withAuthenticationRequired(component, {
        onRedirecting: () => (
            <LinearProgress/>
        )
    });

    return <Component/>;
}

const router = createBrowserRouter([
    {
        path: "/",
        element: <Root/>,
        //errorElement,
        children: [
            {
                path: "",
                element: <AuthGuard component={Overview}/>
            },
            {
                path: "devices",
                element: <AuthGuard component={Devices}/>
            },
            {
                path: "devices/:id",
                element: <AuthGuard component={DeviceDetails}/>
            },
            {
                path: "rules",
                element: <AuthGuard component={Rules}/>
            },
            {
                path: "rules/:id",
                element: <AuthGuard component={RuleDetails}/>
            },
            {
                path: "admin/overview",
                element: <AuthGuard component={AdminOverview}/>
            },
            {
                path: "admin/tenants",
                element: <AuthGuard component={AdminTenants}/>
            },
            {
                path: "admin/users",
                element: <AuthGuard component={AdminUsers}/>
            },
            {
                path: "tenants",
                element: <AuthGuard component={Tenants}/>
            },
            {
                path: "onboarding/select-subscription-level",
                element: <OnboardingSelectSubscriptionLevel/>
            },
            {
                path: "onboarding/tenant-details",
                element: <OnboardingTenantDetails/>
            },
            {
                path: "onboarding/payment",
                element: <OnboardingPayment/>
            }
        ]
    }
]);

const queryClient = new QueryClient();

ReactDOM.createRoot(document.getElementById('root')!).render(
    <React.StrictMode>
        <Auth0Provider domain={import.meta.env.VITE_AUTH0_DOMAIN}
                       clientId={import.meta.env.VITE_AUTH0_CLIENT_ID}
                       authorizationParams={{
                           redirect_uri: window.location.origin,
                           audience: import.meta.env.VITE_AUTH0_AUDIENCE
                       }}>
            <QueryClientProvider client={queryClient}>
                <ToastContainer autoClose={2000}/>
                <CssBaseline/>
                <RouterProvider router={router}/>
            </QueryClientProvider>
        </Auth0Provider>
    </React.StrictMode>,
)
