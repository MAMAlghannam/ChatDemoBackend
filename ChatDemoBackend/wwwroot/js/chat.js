var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

function sendMessage(event) {
    event.preventDefault();

    connection
        .invoke("SendMessage", event.target.message.value) // invoke
        .then(rep => {
            //console.log("message sent", rep)
            var liElement = document.createElement("LI")
            var liContent = document.createTextNode("You: " + event.target.message.value)
            liElement.appendChild(liContent);
            document.querySelector(".messages-list").appendChild(liElement)

            event.target.reset();
        })
        .catch(e => { alert("message sent error\n" + e); console.log("message sent error", e) })
}

let shoutldInvokeTyping = true;
function iamTyping() {
    if (shoutldInvokeTyping) {
        connection.invoke("IsTyping")
        shoutldInvokeTyping = false
        setTimeout(() => {
            shoutldInvokeTyping = true
        }, 3000)
    }
}

window.addEventListener('load', () => {
     connection.start()
        .then(rep => {
            console.log("Connection started", rep);
            //console.log("Connection:", connection);
        })
        .catch(e => {
            console.log("Connection error:", e);
            alert("Connection error\n" + e); 
        })

    connection.on("AcceptConnection", () => {
        console.log("AcceptConnection");
        let username = document.querySelector('form input[name="username"]').value;
        let userId = document.querySelector('form input[name="userId"]').value;
        connection
            .invoke("Subscribe", userId, username)
            .catch(e => alert("Subscribe error\n" + JSON.stringify(e))) // invoke
    });

    connection.on("ReceiveConnectedUsers", (list) => {
        var connectedList = document.querySelector(".users-list");
        console.log("ReceiveConnectedUsers", list)

        Object.entries(list).map(item => {
            let userId = document.querySelector('form input[name="userId"]').value;
            var liElement = document.createElement("LI")
            if (item[0] == userId) {
                var liContent = document.createTextNode("You")
            } else {
                var liContent = document.createTextNode(item[1].username)
            }
            liElement.id = item[0]
            liElement.appendChild(liContent);

            connectedList.appendChild(liElement)
        })

    });

    connection.on("SomeoneIsTyping", (username, userId) => {
        var spanElement = document.createElement("SPAN")
        var spanContent = document.createTextNode(username)
        spanElement.id = userId;
        spanElement.appendChild(spanContent)

        document.querySelector('.typing-alert').appendChild(spanElement)

        console.log(username + " typing")

        setTimeout(() => {
            try {
                document.querySelector('.typing-alert span[id="'+userId+'"]').remove();
            } catch (e) {
                console.log('removing is typing element for ' + userId);
            }
        }, 3000)

    });

    connection.on("ReceiveMessage", (user, message) => {
        console.log("ReceiveMessage", user, message);

        var liElement = document.createElement("LI")
        var liContent = document.createTextNode(user + ": " + message)
        liElement.appendChild(liContent);

        document.querySelector(".messages-list").appendChild(liElement)
    });

    connection.on("SomeoneJoined", (info) => {
        var liElement = document.createElement("LI")
        var liContent = document.createTextNode(info.username)
        liElement.id = info.userId
        liElement.appendChild(liContent);

        document.querySelector(".users-list").appendChild(liElement)
    });

    connection.on("SomeoneLeft", (userId) => {
        try {
            document.querySelector(".users-list li[id='"+userId+"']").remove();
        } catch (e) {
            console.log("Error in SomeoneLeft for userId " + userId, e)
        }
    });

})
