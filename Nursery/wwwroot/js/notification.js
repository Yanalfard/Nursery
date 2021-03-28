/**
 * Reverted UIKIT notification 
 * @param {string} message
 * @param {string} status primary, success, warning, danger
 * @param {number} timeout
 * @param {string} pos top-left, top-center, top-right, bottom-left, bottom-center, bottom-right
 */
function notify(message, status = null, timeout = 5000, pos = 'bottom-right') {
    UIkit.notification(message, { status: status, timeout: timeout, pos: pos });
}

