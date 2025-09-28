window.myUtils = {
    addDefaultPreventingHandler: function (element, eventName) {
        element.addEventListener(eventName, e => e.preventDefault(), { passive: false });
    }
};