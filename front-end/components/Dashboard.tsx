/*
Main dashboard for the application, when signed in.
*/

export const Dashboard = () => {
    return (
        <div>
            <h2 className="text-xl">Dashboard</h2>
            <p>
                <a href="/decision-engine">TODO Decision Engine</a> |
                <a href="/devices">Device Management</a>
            </p>
            <p>TODO traffic statistics vis</p>
            <p>TODO Message type statistics vis</p>
            <p>TODO Device status</p>
        </div>
    );
};