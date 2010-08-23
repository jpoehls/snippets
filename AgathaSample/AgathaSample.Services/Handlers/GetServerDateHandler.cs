using Agatha.Common;
using Agatha.ServiceLayer;
using AgathaSample.Common.RequestsAndResponses;
using System;

namespace AgathaSample.Services.Handlers
{
    public class GetServerDateHandler : RequestHandler<GetServerDateRequest, GetServerDateResponse>
    {
        public override Response Handle(GetServerDateRequest request)
        {
            var response = CreateTypedResponse();
            response.Date = DateTime.Now;
            return response;
        }
    }
}
