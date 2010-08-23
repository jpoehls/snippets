using System.Diagnostics;
using Agatha.ServiceLayer;
using AgathaSample.Common.RequestsAndResponses;

namespace AgathaSample.Services.Handlers
{
    public class HelloWorldCommandHandler : OneWayRequestHandler<HelloWorldCommand>
    {
        public override void Handle(HelloWorldCommand request)
        {
            Debug.WriteLine("HelloWorldCommand recieved!");
        }
    }
}
