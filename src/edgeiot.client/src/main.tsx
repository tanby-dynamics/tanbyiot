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

const router = createBrowserRouter([
    {
        path: "/",
        element: <AppTemplate><Dashboard/></AppTemplate>
    },
    {
        path: "/devices",
        element: <AppTemplate><Devices/></AppTemplate>
    },
    {
        path: "/devices/:id",
        element: <AppTemplate><DeviceDetails/></AppTemplate>
    },
    {
        path: "/rules",
        element: <AppTemplate><Rules/></AppTemplate>
    },
    {
        path: "/rules/:id",
        element: <AppTemplate><RuleDetails/></AppTemplate>
    }
]);

const queryClient = new QueryClient();

ReactDOM.createRoot(document.getElementById('root')!).render(
    <React.StrictMode>      
        <QueryClientProvider client={queryClient}>
            <RouterProvider router={router}/>
        </QueryClientProvider>
    </React.StrictMode>,
)
