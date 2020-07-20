var hub = $.connection.mainHub;
var raiseHandBtns = document.getElementById("raiseHandButtonGroup");
var downHandBtn = document.getElementById("downHandButton");
var raiseYourHandHtml = "<h1>Raise your hand now!</h1>";
var name;

$.connection.hub.start()
	.done(started)
	.fail(disconnected);
$.connection.hub.disconnected(disconnected);

function started() {
	if (!wasReady) {
		if (!room) {
			room = ask("Which room do you want to join?", true);
		}
		name = ask("What's your name?");
	}
	hub.server.participantConnected(room, name);
}
function comment() {
	hub.server.raiseHandComment();
}
function newtopic() {
	hub.server.raiseHandNewT();
}
function toomuch() {
	hub.server.raiseHandTooM();
}
function tech() {
	hub.server.raiseHandTech();
}
function bullsh() {
	hub.server.raiseHandBullsh();
}
function downHand() {
	hub.server.downHand();
}
hub.client.updateNumber = function (number, handType) {
	replaceNumber("<h2 style=\"display: inline\">Your current number: </h2><h1 style=\"display: inline\">" + number + ". </h1>(" + handType + ")");
	setReady(true);
	disableRaiseHandBtns(true);
	downHandBtn.disabled = false;
};
hub.client.downHand = function () {
	replaceNumber(raiseYourHandHtml);
	setReady(true);
	disableRaiseHandBtns(false);
	downHandBtn.disabled = true;
};
function replaceNumber(number) {
	document.getElementById("number").innerHTML = number;
}
function disableRaiseHandBtns(value) {
	var btns = raiseHandBtns.querySelectorAll("input");
	var i;
	for (i = 0; i < btns.length; ++i) {
		btns[i].disabled = value;
	}
}
function insideSetReady() {
	document.getElementById("inputs").hidden = !ready;
}
function sendReadyMsg() {
	var msg;
	if (ready) {
		msg = "Hello ";
		msg += name;
		msg += ", welcome to the ";
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