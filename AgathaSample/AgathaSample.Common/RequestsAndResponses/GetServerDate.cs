using Agatha.Common;
using System;

namespace AgathaSample.Common.RequestsAndResponses
{
	public class GetServerDateRequest : Request {}

	public class GetServerDateResponse : Response
	{
		public DateTime Date { get; set; }
	}
}
