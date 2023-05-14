import { Inter } from 'next/font/google'
import { useState, useEffect } from 'react';
import { Dashboard, SiteHeader } from '../components';

const inter = Inter({ subsets: ['latin'] })

export default function Home() {
  const [ user, setUser ] = useState<User | null>();

  // TODO user api
  useEffect(() => {
    setUser({ username: 'becdetat' });
  });

  return (
    <main
      className={`flex min-h-screen flex-col items-center justify-between p-24 ${inter.className}`}
    >
        <div>
          <SiteHeader/>
          <br/>
          { user && <Dashboard/> }
        </div>
    </main>
  )
}
