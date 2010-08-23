using Agatha.Common;
using Agatha.ServiceLayer;
using AgathaSample.Common.RequestsAndResponses;

namespace AgathaSample.Services.Handlers
{
	public class HelloWorldHandler : RequestHandler<HelloWorldRequest, HelloWorldResponse>
	{
		public override Response Handle(HelloWorldRequest request)
		{
			var response = CreateTypedResponse();
			response.Message = "Hello World!";
			return response;
		}
	}
}