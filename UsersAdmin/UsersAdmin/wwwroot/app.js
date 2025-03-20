
window.getBrowserInfo = function () {
    let userAgent = navigator.userAgent;
    let browserName = "Unknown";

    if (userAgent.includes("Firefox")) {
        browserName = "Firefox";
    } else if (userAgent.includes("OPR") || userAgent.includes("Opera")) {
        browserName = "Opera";
    } else if (userAgent.includes("Edg")) {
        browserName = "Edge";
    } else if (userAgent.includes("Chrome") && !userAgent.includes("Edg") && !userAgent.includes("OPR")) {
        browserName = "Chrome";
    } else if (userAgent.includes("Safari") && !userAgent.includes("Chrome")) {
        browserName = "Safari";
    } else if (userAgent.includes("MSIE") || userAgent.includes("Trident")) {
        browserName = "Internet Explorer";
    }

    return browserName;
    return browserName;
};
window.showAlert = (message) => {
    alert(message);
};