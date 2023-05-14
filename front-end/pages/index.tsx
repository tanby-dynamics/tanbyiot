import { Inter } from 'next/font/google'
import { useState } from 'react';
import { Dashboard } from '../components';

const inter = Inter({ subsets: ['latin'] })

type User = {
  username: string;
};

export default function Home() {
  const [ user, setUser ] = useState<User | null>();

  const signIn = () => setUser({ username: 'becdetat' });
  const signOut = () => setUser(null);

  return (
    <main
      className={`flex min-h-screen flex-col items-center justify-between p-24 ${inter.className}`}
    >
      <div>
        <h1 className="text-2xl">Pretty Swish</h1>
        <p>Expressive IoT Platform by <a href="https://tanbydynamics.co">Tanby Dynamics</a></p>
        { !user && <p><a href="/sign-in">Sign in</a> or <a href="/sign-up">Create an account</a></p> }
        { !user && <p><button onClick={signIn}>Sign in as becdetat</button></p> }
        { user && (
          <>
            <p>Signed in as {user.username}</p>
            <p><button onClick={signOut}>Sign out</button></p>
          </>
        ) }
      </div>
      { user && <Dashboard/> }
    </main>
  )
}
