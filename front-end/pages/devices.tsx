import { SiteHeader } from '@/components';
import { Inter } from 'next/font/google'
import { useState } from 'react';

const inter = Inter({ subsets: ['latin'] })

export default function Devices() {
    return (
        <main className={`flex min-h-screen flex-col items-center justify-between p-24 ${inter.className}`}>
            <div>
                <SiteHeader/>
                <br/>
                <h2 className="text-xl">
                    <a href="/">Dashboard</a> |
                    Device Management
                </h2>
                <br/>
                <p><a href="/new-device">Set up a new device</a></p>
                <p>TODO list of devices</p>
                
            </div>
        </main>
    );
};