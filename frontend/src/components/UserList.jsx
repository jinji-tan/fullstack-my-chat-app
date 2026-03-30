import { useState, useEffect } from "react";
import { getUserApi } from "../services/api";

const UserList = ({ setReceiverName, currentReceiverName }) => {
    const [users, setUsers] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        console.log("TOKEN:", localStorage.getItem("token"));
        console.log("MY NAME:", localStorage.getItem("userName"));
        const fetchUsers = async () => {
            try {
                const data = await getUserApi();

                const token = localStorage.getItem("token");

                const formattedUsers = data.map((u) => {
                    return {
                        id: u.id,
                        name: `${u.firstName} ${u.lastName}`,
                        initial: u.firstName.charAt(0).toUpperCase(),
                        // Use the status coming FROM the database/API, not your local variable
                        status: "offline"
                    };
                });

                setUsers(formattedUsers);

            } catch (error) {
                console.error("Error fetching users:", error);
            }
            finally {
                setLoading(false);
            }
        };

        fetchUsers();
    }, [])

    if (loading) {
        return (
            <div className="user-list-card loading-state">
                <div className="user-list-header">
                    <h3>Chat</h3>
                </div>
                <div className="loader-container">
                    <div className="spinner"></div>
                    <p>Loading friends...</p>
                </div>
            </div>
        );
    }

    return (
        <div className="user-list-card">
            <div className="user-list-header">
                <h3>Chat</h3>
                <span className="user-count">{users.length} Users</span>
            </div>
            <hr />

            <div className="user-list-content">
                {users.map((user) => (
                    <div key={user.id} className={`user-item ${currentReceiverName === user.name ? `active` : ``}`}
                        onClick={() => setReceiverName(user.name)}>
                        <div className="user-avatar">
                            {user.initial}
                            <span className={`status-dot ${user.status}`}></span>
                        </div>
                        <div className="user-info">
                            <p className="user-name">{user.name}</p>
                            <p className="user-status-text">{user.status}</p>
                        </div>
                    </div>
                ))}
            </div>
            <hr />
        </div>
    );
};

export default UserList;