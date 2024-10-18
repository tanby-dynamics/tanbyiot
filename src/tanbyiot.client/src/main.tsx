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
import { CssBaseline } from '@mui/material';
import {Root} from "./layout/Root.tsx";
import {Overview} from "./pages/Overview.tsx";
import {ApplicationStates} from "./pages/ApplicationStates.tsx";

const router = createBrowserRouter([
    {
        path: "/",
        element: <Root/>,
        //errorElement,
        children: [
            {
                path: "",
                element: <Overview/>
            },
            {
                path: "devices",
                element: <Devices/>
            },
            {
                path: "devices/:id",
                element: <DeviceDetails/>
            },
            {
                path: "rules",
                element: <Rules/>
            },
            {
                path: "rules/:id",
                element: <RuleDetails/>
            },
            {
                path: "application-states",
                element: <ApplicationStates/>
            }
        ]
    }
]);

const queryClient = new QueryClient();

ReactDOM.createRoot(document.getElementById('root')!).render(
    <React.StrictMode>
        <QueryClientProvider client={queryClient}>
            <ToastContainer autoClose={2000}/>
            <CssBaseline/>
            <RouterProvider router={router}/>
        </QueryClientProvider>
    </React.StrictMode>,
)
