using Blazored.Diagrams.Behaviours;
using Blazored.Diagrams.Extensions;
using Blazored.Diagrams.Sandbox.Pages.Examples.CalculatorExample.OperatorNode;
using Blazored.Diagrams.Sandbox.Pages.Examples.CalculatorExample.ResultNode;
using Blazored.Diagrams.Services.Diagrams;
using Blazored.Diagrams.Services.Events;

namespace Blazored.Diagrams.Sandbox.Pages.Examples.CalculatorExample;

public class CalculatorBehaviour : BaseBehaviour
{
    private readonly IDiagramService _service;
    private readonly List<IDisposable> _subscriptions;

    public CalculatorBehaviour(IDiagramService service)
    {
        _service = service;
        _subscriptions =
        [
            _service.Events.SubscribeWhere<IncomingLinkAddedEvent>(p => p.AddedLink.TargetPort is OperatorInputPort,
                HandleOperatorNode),
            _service.Events.SubscribeWhere<IncomingLinkRemovedEvent>(p => p.RemovedLink.TargetPort is OperatorInputPort,
                HandleOperatorNode),
            _service.Events.SubscribeWhere<IncomingLinkAddedEvent>(p => p.AddedLink.TargetPort is ResultInputPort,
                HandleResultNode),
            _service.Events.SubscribeWhere<IncomingLinkRemovedEvent>(p => p.RemovedLink.TargetPort is ResultInputPort,
                HandleResultNode),
        ];
    }

    // private IEnumerable<decimal?> GetNumbers()
    // {
    //     // return IncomingLinks.Select(x => x.SourcePort.Parent).Cast<INumberOutput>().Select(x => x.NumberOutput);
    // }

    private void HandleResultNode(IncomingLinkAddedEvent obj)
    {
        if (obj.AddedLink.TargetPort?.Parent is ResultNode.ResultNode rn)
        {
            rn.NumberOutput = (obj.AddedLink.SourcePort.Parent as INumberOutput)!.NumberOutput;
            // rn.OnNumberChanged += 
        }
    }

    private void HandleResultNode(IncomingLinkRemovedEvent obj)
    {
        if (obj.RemovedLink.TargetPort?.Parent is ResultNode.ResultNode rn)
        {
            rn.NumberOutput = null;
        }
    }

    private void HandleOperatorNode(IncomingLinkRemovedEvent obj)
    {
        throw new NotImplementedException();
    }

    private void HandleOperatorNode(IncomingLinkAddedEvent obj)
    {
        if (obj.AddedLink.SourcePort.Parent is INumberOutput no)
        {
            // node.Cal
        }
    }

    // private IEnumerable<decimal?> GetNumbers()
    // {
    //     // return IncomingLinks.Select(x => x.SourcePort.Parent).Cast<INumberOutput>().Select(x => x.NumberOutput);
    // }

    public void Calculate()
    {
        // var numbers = OutputPort.GetNumbers().ToList();
        // if (numbers.All(x => x.HasValue))
        // {
        //     Parent.NumberOutput = Parent.Operator switch
        //     {
        //         Operator.Add => numbers.Sum(),
        //         Operator.Subtract => numbers.Aggregate((x, y) => x - y),
        //         Operator.Multiply => numbers.Aggregate((x, y) => x * y),
        //         Operator.Divide => numbers.Aggregate((x, y) => x / y),
        //         _ => throw new ArgumentOutOfRangeException()
        //     };
        // }
    }

    public bool IsEnabled { get; set; }
}