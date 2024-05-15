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

const router = createBrowserRouter([
    {
        path: "/",
        element: <AppTemplate><Dashboard/></AppTemplate>
    },
    {
        path: "/devices",
        element: <AppTemplate><Devices/></AppTemplate>
    }
]);

ReactDOM.createRoot(document.getElementById('root')!).render(
    <React.StrictMode>      
        <RouterProvider router={router}/>
    </React.StrictMode>,
)
