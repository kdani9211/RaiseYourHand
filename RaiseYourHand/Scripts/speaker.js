var hub = $.connection.mainHub;
var clientsTable = document.getElementById("clients");

$.connection.hub.start()
	.done(started)
	.fail(disconnected);
$.connection.hub.disconnected(disconnected);

function started() {
	if (!room && !wasReady) {
		room = ask("Which room do you want to join?", true);
	}
	hub.server.speakerConnected(room);
}
hub.client.updateQueue = function (hands) {
	setReady(true);
	generateTable(hands);
};
function generateTable(hands) {
	clientsTable.innerHTML = "";
	var i;
	for (i = 0; i < hands.length; ++i) {
		var row = clientsTable.insertRow(-1);
		var numCell = row.insertCell(-1);
		numCell.innerHTML = i + 1;
		var nameCell = row.insertCell(-1);
		nameCell.innerHTML = hands[i].Participant.Name;
		var handCell = row.insertCell(-1);
		handCell.innerHTML = hands[i].HandText;
		var btnCell = row.insertCell(-1);
		btnCell.appendChild(generateDownHandBtn(hands[i].Participant.Name));
	}
}
function generateDownHandBtn(name) {
	var btn = document.createElement("input");
	btn.setAttribute("type", "button");
	btn.value = "Down hand!";
	btn.classList.add("btn");
	btn.classList.add("btn-danger");
	btn.classList.add("btn-sm");
	btn.onclick = function () { downHand(name); };
	return btn;
}
function downHand(name) {
	hub.server.downHandBySpeaker(name);
}
function insideSetReady() {
	clientsTable.hidden = !ready;
}
function sendReadyMsg() {
	var msg;
	if (ready) {
		msg = "Welcome to the ";
		if (room) {
			msg += room;
			msg += " ";
		}
		msg += "room!";
	}
	else {
		msg = "Your connection was interrupted. Check your connection!";
	}
	alert(msg);
}