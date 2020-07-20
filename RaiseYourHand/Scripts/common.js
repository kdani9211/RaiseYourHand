var room;
var ready = false;
var wasReady = false;
var reconnectionTimer;

function ask(question, nullenabled = false) {
	var result;
	if (nullenabled) {
		return window.prompt(question, "");
	}
	while (!result) {
		result = window.prompt(question);
	}
	return result;
}
function disconnected() {
	setReady(false);
	reconnectionTimer = setTimeout(function () {
		$.connection.hub.start()
			.done(started)
			.fail(disconnected);
	}, 5000);
}
function setReady(value) {
	if (ready === value)
		return;
	ready = value;
	if (ready)
		wasReady = true;
	insideSetReady();
	sendReadyMsg();
	if (ready && reconnectionTimer) {
		clearTimeout(reconnectionTimer);
		reconnectionTimer = null;
	}
}