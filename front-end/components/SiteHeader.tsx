/* */
import { useState } from 'react';

export const SiteHeader = () => {
    const [ user, setUser ] = useState<User | null>();

    const signIn = () => setUser({ username: 'becdetat' });
    const signOut = () => setUser(null);
  
    return (
        <div>
            <h1 className="text-2xl">Edge IoT</h1>
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
    )
  };