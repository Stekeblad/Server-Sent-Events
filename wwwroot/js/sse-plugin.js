
/** expecting: string, object, function(object)
 * Remarks: based on https://medium.com/@david.richards.tech/sse-server-sent-events-using-a-post-request-without-eventsource-1c0bd6f14425
 */
async function postAndListenForEvents(url, formData, onEventReceived) {
    const response = await fetch(url, {
        method: 'POST',
        headers: {
            'Accept': 'text/event-stream'
            // Depending on submitted type of form you may need to add a header for Content-Type
        },
        body: formData
    });

    const reader = response.body.pipeThrough(new TextDecoderStream()).getReader();

    while (true) {
        const { value, done } = await reader.read();
        if (done) break;

        // Extract data object from the event, convert to JSON and send to listener
        for(const line of value.split('\n'))
        {
            if (line.startsWith("data:")) {
                var data = line.substring(6);
                const json = JSON.parse(data);
                onEventReceived(json);
            }
        }
    }
}

function onEventRecieved(eventData) {
    const eventList = document.getElementById("sse-events");
    var elem;
    // Only show one line in log or add new lines for each message?
    if (form.getAttribute("data-sseAppendLog") === "true") {
        elem = document.createElement("li");
        eventList.appendChild(elem);
    } else {
        elem = eventList.firstChild;
        if (elem === null) {
            elem = document.createElement("li");
            eventList.appendChild(elem);
        }
    }
    elem.textContent = eventData.message;
}

const form = document.getElementById("sse-form");
form.addEventListener("submit", (event) => {
    event.preventDefault();

    const formData = new FormData(form);
    postAndListenForEvents(form.getAttribute("action"), formData, onEventRecieved);
});

