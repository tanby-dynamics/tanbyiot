/* This really should be a modal */
import { Inter } from 'next/font/google';
import { SiteHeader } from '../components';
import { useState } from 'react';

const inter = Inter({ subsets: ['latin'] })

export default function NewDevice() {
    const [ connected, setConnected ] = useState(false);

    return (
        <main className={`flex min-h-screen flex-col items-center justify-between p-24 ${inter.className}`}>
            <div>
                <SiteHeader/>
                <br/>
                <h2 className="text-xl">
                    <a href="/">Dashboard</a> | <a href="/devices">Device Management</a> | New device
                </h2>
                <br/>
                <ol>
                <li>Here's an API key for the device: <code>NJKYV87VG78GEA8</code></li>
                <li>TODO instructions on how to configure the device</li>
                </ol>
                {connected && <p>Connected!</p>}
                {!connected && <p>Not connected</p>}
            </div>
        </main>            
    );
}

