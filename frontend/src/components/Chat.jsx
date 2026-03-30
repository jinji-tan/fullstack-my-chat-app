import UserList from "./UserList";

export const Chat = ({ receiverName }) => {

    const messages = [
        { id: 1, text: "Hey! How is the project going?", sender: "receiver" },
        { id: 2, text: "It's going great! Just finishing the chat UI.", sender: "sender" },
        { id: 3, text: "Awesome, can't wait to see it.", sender: "receiver" },
    ];
  
    return (
        <div className="chat-window">
            <div className="chat-header">
                <h3>{receiverName}</h3>
            </div>

            <div className="chat-messages">
                {messages.map((msg) => (
                    <div key={msg.id} className={`message-wrapper ${msg.sender}`}>
                        <div className="message-bubble">
                            {msg.text}
                        </div>
                    </div>
                ))}
            </div>

            <form className="chat-input-area" onSubmit={(e) => e.preventDefault()}>
                <input type="text" placeholder="Type a message..." />
                <button type="submit">Send</button>
            </form>
        </div>
    );
};



