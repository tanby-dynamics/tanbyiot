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
import { CssBaseline } from '@mui/material';
import {AdminDashboard} from "./pages/AdminDashboard.tsx";
import {Root} from "./layout/Root.tsx";
import {Tenants} from "./pages/Tenants.tsx";

// @ts-ignore
function AuthGuard({ component }) {
    const Component = withAuthenticationRequired(component, {
        onRedirecting: () => (
            <div>
                Redirecting
            </div>
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
                path: "admin",
                element: <AdminDashboard/>
            },
            {
                path: "tenants",
                element: <AuthGuard component={Tenants}/>
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
