function notify(message, status = null, timeout = 5000,  pos = 'bottom-right') {
    UIkit.notification(message, { status: status, timeout: timeout, pos: pos });
}