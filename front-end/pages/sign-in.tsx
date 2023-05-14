import { Inter } from 'next/font/google'

const inter = Inter({ subsets: ['latin'] })

export default function SignIn() {
    return (
        <main
            className={`flex min-h-screen flex-col items-center justify-between p-24 ${inter.className}`}
        >
            <form method="POST">
                <label htmlFor="username">Username</label>
                <input type="text" name="username"/>
                <br/>
                <button type="submit">Sign in</button>
            </form>
        </main>
    );
};