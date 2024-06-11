import React from 'react'
import ReactDOM from 'react-dom/client'
import {RouterProvider, createBrowserRouter } from 'react-router-dom'
import '@fontsource/roboto/300.css';
import '@fontsource/roboto/400.css';
import '@fontsource/roboto/500.css';
import '@fontsource/roboto/700.css';
import {AppTemplate} from "./AppTemplate.tsx";
import {Dashboard} from "./Dashboard.tsx";
import {Devices} from "./Devices.tsx";
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import {DeviceDetails} from "./DeviceDetails.tsx";
import {Rules} from "./Rules.tsx";
import {RuleDetails} from "./RuleDetails.tsx";
import 'react-toastify/dist/ReactToastify.css';
import { ToastContainer } from 'react-toastify';
import { Auth0Provider, withAuthenticationRequired } from '@auth0/auth0-react';
import { CssBaseline } from '@mui/material';

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
        element: <AppTemplate><Dashboard/></AppTemplate>
    },
    {
        path: "/devices",
        element: <AppTemplate><AuthGuard component={Devices}/></AppTemplate>
    },
    {
        path: "/devices/:id",
        element: <AppTemplate><AuthGuard component={DeviceDetails}/></AppTemplate>
    },
    {
        path: "/rules",
        element: <AppTemplate><AuthGuard component={Rules}/></AppTemplate>
    },
    {
        path: "/rules/:id",
        element: <AppTemplate><AuthGuard component={RuleDetails}/></AppTemplate>
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
