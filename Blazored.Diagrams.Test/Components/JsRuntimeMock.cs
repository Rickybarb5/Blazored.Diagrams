using Microsoft.JSInterop.Infrastructure;

namespace Blazored.Diagrams.Test.Components;

using Moq;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Extensions;

/// <summary>
///     Helper class for creating and working with Moq mocks of <see cref="IJSRuntime"/>.
/// </summary>
public class JSRuntimeMock
{
    private readonly Dictionary<string, object> _jsInvocations = new();
    
    public Mock<IJSRuntime> Mock { get; }

    public JSRuntimeMock()
    {
        Mock = new Mock<IJSRuntime>();
        SetupMock();
    }

    private void SetupMock()
    {
        // Setup GetBoundingClientRect
        Mock.Setup(x => x.InvokeAsync<Rect>(
                JsFunctionConstants.BoundingClientRectFunctionName,
                It.IsAny<object[]>()))
            .ReturnsAsync(new Rect { Width = 100, Height = 50, Top = 0, Left = 0  });

        // Setup IsClickOnPath
        Mock.Setup(x => x.InvokeAsync<bool>(
                JsFunctionConstants.IsClickOnPathFunctionName,
                It.IsAny<object[]>()))
            .ReturnsAsync(false);

        // Setup HandleWheelEvent (InvokeVoidAsync)
        Mock.Setup(x => x.InvokeAsync<IJSVoidResult>(
                "BlazoredDiagrams.handleZoom",
                It.IsAny<object[]>()))
            .ReturnsAsync(Moq.Mock.Of<IJSVoidResult>());

        // Setup InitializeResizeObserver
        Mock.Setup(x => x.InvokeAsync<IJSVoidResult>(
                JsFunctionConstants.InitializeResizeObserver,
                It.IsAny<object[]>()))
            .ReturnsAsync(Moq.Mock.Of<IJSVoidResult>());

        // Setup GetElementId
        Mock.Setup(x => x.InvokeAsync<string>(
                JsFunctionConstants.GetElementId,
                It.IsAny<object[]>()))
            .ReturnsAsync((string functionName, object[] args) => 
                args.Length > 0 && args[0] is ElementReference elementRef 
                    ? $"element-{elementRef.Id}" 
                    : "default-element-id");

        // Setup UnregisterObserver
        Mock.Setup(x => x.InvokeAsync<IJSVoidResult>(
                JsFunctionConstants.UnregisterObserver,
                It.IsAny<object[]>()))
            .ReturnsAsync(Moq.Mock.Of<IJSVoidResult>());

        // Setup RemoveAllObservers
        Mock.Setup(x => x.InvokeAsync<IJSVoidResult>(
                JsFunctionConstants.RemoveAllObservers,
                It.IsAny<object[]>()))
            .ReturnsAsync(Moq.Mock.Of<IJSVoidResult>());
    }

    /// <summary>
    ///     Sets the return value for GetBoundingClientRect calls.
    /// </summary>
    public JSRuntimeMock WithBoundingClientRect(double width, double height, double x = 0, double y = 0)
    {
        Mock.Setup(x => x.InvokeAsync<Rect>(
                JsFunctionConstants.BoundingClientRectFunctionName,
                It.IsAny<object[]>()))
            .ReturnsAsync(new Rect { Width = width, Height = height, Top = x, Left = y });
        
        return this;
    }

    /// <summary>
    ///     Sets the return value for IsClickOnPath calls.
    /// </summary>
    public JSRuntimeMock WithIsClickOnPath(bool result)
    {
        Mock.Setup(x => x.InvokeAsync<bool>(
                JsFunctionConstants.IsClickOnPathFunctionName,
                It.IsAny<object[]>()))
            .ReturnsAsync(result);
        
        return this;
    }

    /// <summary>
    ///     Sets the return value for IsClickOnPath based on a predicate.
    /// </summary>
    public JSRuntimeMock WithIsClickOnPath(Func<string, double, double, bool> predicate)
    {
        Mock.Setup(x => x.InvokeAsync<bool>(
                JsFunctionConstants.IsClickOnPathFunctionName,
                It.IsAny<object[]>()))
            .ReturnsAsync((string functionName, object[] args) =>
            {
                if (args.Length >= 3 && 
                    args[0] is string pathId && 
                    args[1] is double clientX && 
                    args[2] is double clientY)
                {
                    return predicate(pathId, clientX, clientY);
                }
                return false;
            });
        
        return this;
    }

    /// <summary>
    ///     Sets the return value for GetElementId calls.
    /// </summary>
    public JSRuntimeMock WithGetElementId(string elementId)
    {
        Mock.Setup(x => x.InvokeAsync<string>(
                JsFunctionConstants.GetElementId,
                It.IsAny<object[]>()))
            .ReturnsAsync(elementId);
        
        return this;
    }

    /// <summary>
    ///     Sets the return value for GetElementId based on ElementReference.
    /// </summary>
    public JSRuntimeMock WithGetElementId(Func<ElementReference, string> idGenerator)
    {
        Mock.Setup(x => x.InvokeAsync<string>(
                JsFunctionConstants.GetElementId,
                It.IsAny<object[]>()))
            .ReturnsAsync((string functionName, object[] args) =>
            {
                if (args.Length > 0 && args[0] is ElementReference elementRef)
                {
                    return idGenerator(elementRef);
                }
                return "default-element-id";
            });
        
        return this;
    }

    /// <summary>
    ///     Verifies that GetBoundingClientRect was called.
    /// </summary>
    public void VerifyGetBoundingClientRectCalled(Times times)
    {
        Mock.Verify(x => x.InvokeAsync<Rect>(
            JsFunctionConstants.BoundingClientRectFunctionName,
            It.IsAny<object[]>()), times);
    }

    /// <summary>
    ///     Verifies that IsClickOnPath was called with specific parameters.
    /// </summary>
    public void VerifyIsClickOnPathCalled(string pathId, double clientX, double clientY, Times times)
    {
        Mock.Verify(x => x.InvokeAsync<bool>(
            JsFunctionConstants.IsClickOnPathFunctionName,
            It.Is<object[]>(args => 
                args.Length >= 3 && 
                args[0].Equals(pathId) && 
                args[1].Equals(clientX) && 
                args[2].Equals(clientY))), times);
    }

    /// <summary>
    ///     Verifies that HandleWheelEvent was called.
    /// </summary>
    public void VerifyHandleWheelEventCalled(Times times)
    {
        Mock.Verify(x => x.InvokeAsync<IJSVoidResult>(
            "BlazoredDiagrams.handleZoom",
            It.IsAny<object[]>()), times);
    }

    /// <summary>
    ///     Verifies that InitializeResizeObserver was called.
    /// </summary>
    public void VerifyInitializeResizeObserverCalled(Times times)
    {
        Mock.Verify(x => x.InvokeAsync<IJSVoidResult>(
            JsFunctionConstants.InitializeResizeObserver,
            It.IsAny<object[]>()), times);
    }

    /// <summary>
    ///     Verifies that UnregisterObserver was called with specific element ID.
    /// </summary>
    public void VerifyUnregisterObserverCalled(string elementId, Times times)
    {
        Mock.Verify(x => x.InvokeAsync<IJSVoidResult>(
            JsFunctionConstants.UnregisterObserver,
            It.Is<object[]>(args => args.Length > 0 && args[0].Equals(elementId))), times);
    }
}