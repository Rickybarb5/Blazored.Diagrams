// noinspection JSUnusedGlobalSymbols

window.BlazorFlows = {
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

        if (BlazorFlows.resizeObservers[elementId]) {
            BlazorFlows.resizeObservers[elementId].disconnect();
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
        BlazorFlows.resizeObservers[elementId] = observer;
    },
    removeResizeObserver: (elementId) => {
        if (BlazorFlows.resizeObservers[elementId]) {
            BlazorFlows.resizeObservers[elementId].disconnect();
            delete BlazorFlows.resizeObservers[elementId];
        }
    },

    removeAllResizeObservers: () => {
        for (let elementId in BlazorFlows.resizeObservers) {
            BlazorFlows.resizeObservers[elementId].disconnect();
        }
        BlazorFlows.resizeObservers = {};
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
};