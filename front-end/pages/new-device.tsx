/* This really should be a modal */
import { Inter } from 'next/font/google';
import { SiteHeader } from '../components';
import { useState, useEffect } from 'react';
import addDeviceService from '@/services/add-device-service';
import toastr from 'toastr';
import { Axios, AxiosError } from 'axios';

const inter = Inter({ subsets: ['latin'] })

export default function NewDevice() {
    const [ connected, setConnected ] = useState(false);
    const [ apiKey, setApiKey ] = useState('');
    const [ newApiKeyLoading, setNewApiKeyLoading ] = useState(true);

    useEffect(() => {
        const inner = async () => {
            try {
                var response = await addDeviceService.getNewApiKey();
                setApiKey(response.data);
            } catch (e: any) {
                toastr.error(e.response?.data, 'Error getting new API key');
                setApiKey('ERROR');
            }
            setNewApiKeyLoading(false);
        };
        inner();
    }, []);

    return (
        <main>
            <div>
                <SiteHeader/>
                <br/>
                <h2>
                    <a href="/">Dashboard</a> | <a href="/devices">Device Management</a> | New device
                </h2>
                <br/>
                {newApiKeyLoading && <p>Loading API key...</p>}
                {!newApiKeyLoading && apiKey != 'ERROR' && (
                    <>
                        <p>
                            Here is the new API key for the device:<br/>
                            <code>{apiKey}</code>
                        </p>
                        <p>TODO instructions on how to configure the device</p>
                        {connected && <p>Connected!</p>}
                        {!connected && <p>Waiting on device to connect...</p>}
                    </>
                )}
                {apiKey === 'ERROR' && <p>Error getting new API key</p>}
            </div>
        </main>            
    );
}

