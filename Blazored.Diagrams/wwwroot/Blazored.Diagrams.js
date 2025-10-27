// noinspection JSUnusedGlobalSymbols

window.BlazoredDiagrams = {
    canvases: {},
    tracked: {},
    resizeObservers: {},  // New object to store resize observers

    getBoundingClientRect: el => {
        return el.getBoundingClientRect();
    },

    getElementId: (element) => {
        if (!element.id) {
            element.id = 'blazor-generated-' + Math.random().toString(36).substr(2, 9);
        }
        return element.id;
    },

    initializeResizeObserver: (dotNetReference, elementId) => {
        const element = document.getElementById(elementId);
        if (!element) return;

        if (BlazoredDiagrams.resizeObservers[elementId]) {
            BlazoredDiagrams.resizeObservers[elementId].disconnect();
        }

        const observer = new ResizeObserver(entries => {
            for (let entry of entries) {
                dotNetReference.invokeMethodAsync('OnResizeAsync', elementId, {
                    width: entry.contentRect.width,
                    height: entry.contentRect.height
                });
            }
        });

        observer.observe(element);
        BlazoredDiagrams.resizeObservers[elementId] = observer;
    },
    removeResizeObserver: (elementId) => {
        if (BlazoredDiagrams.resizeObservers[elementId]) {
            BlazoredDiagrams.resizeObservers[elementId].disconnect();
            delete BlazoredDiagrams.resizeObservers[elementId];
        }
    },

    removeAllResizeObservers: () => {
        for (let elementId in BlazoredDiagrams.resizeObservers) {
            BlazoredDiagrams.resizeObservers[elementId].disconnect();
        }
        BlazoredDiagrams.resizeObservers = {};
    },
    isClickOnPath: function (pathId, x, y) {
        const path = document.getElementById(pathId);
        if (!path) return false;

        const svgElement = path.ownerSVGElement;
        const point = svgElement.createSVGPoint();
        point.x = x;
        point.y = y;

        // Transform the point from screen coordinates to SVG coordinates
        const transformedPoint = point.matrixTransform(svgElement.getScreenCTM().inverse());

        // Check if the point is inside the path
        return path.isPointInStroke(transformedPoint) || path.isPointInFill(transformedPoint);
    },
    handleZoom: function (elementId, dotNetHelper) {
        const element = document.getElementById(elementId);
        if (element) {
            element.addEventListener('wheel', (e) => {


                // Extract relevant properties from the wheel event
                const eventArgs = {
                    deltaX: e.deltaX,
                    deltaY: e.deltaY,
                    clientX: e.clientX,
                    clientY: e.clientY,
                    ctrlKey: e.ctrlKey,
                    shiftKey: e.shiftKey,
                    altKey: e.altKey
                };
                // Prevent scrolling the page
                dotNetHelper.invokeMethodAsync('OnZoom', eventArgs);
                e.preventDefault();
            }, {passive: false});
        }
    },
    /**
     * Capture the element matched by selector and return a data URL (PNG).
     * options: { scale, backgroundColor, scrollX, scrollY, useCORS }
     */
    captureComponentAsDataUrl: async function(selector, options) {
        const el = document.querySelector(selector);
        if (!el) throw `No element found for selector: ${selector}`;

        // Default sensible options:
        const defaultOptions = {
            scale: window.devicePixelRatio || 1,
            backgroundColor: null,
            useCORS: true,
            logging: false
        };
        const opts = Object.assign({}, defaultOptions, options || {});

        // Ensure the element is visible / layout settled
        await new Promise(r => requestAnimationFrame(r));

        const canvas = await html2canvas(el, opts);
        // toDataURL might be heavy for large canvases; you can use canvas.toBlob if needed
        const dataUrl = canvas.toDataURL("image/png");
        return dataUrl;
    },

    /**
     * Trigger download of a dataUrl (data:image/png;base64,...)
     */
    downloadDataUrl: function(dataUrl, filename) {
        const a = document.createElement('a');
        a.href = dataUrl;
        a.download = filename || 'capture.png';
        document.body.appendChild(a);
        a.click();
        a.remove();
    },

    /**
     * Capture and immediately download
     */
    captureAndDownload: async function(selector, filename, options) {
        const dataUrl = await this.captureComponentAsDataUrl(selector, options);
        this.downloadDataUrl(dataUrl, filename);
    }
};
