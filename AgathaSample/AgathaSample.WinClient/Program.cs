using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Agatha.Common;
using Agatha.Ninject;
using AgathaSample.Common.RequestsAndResponses;
using AgathaSample.WinClient.Security;
using Ninject;

namespace AgathaSample.WinClient
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            InitializeAgatha();
            Console.WriteLine(@"Press a key to end this program (hopefully, you saw ""Hello World!"" twice)");
            CallTheServiceSynchronously();
            CallTheServiceAsynchronously();
            Console.ReadKey();
        }

        private static void CallTheServiceSynchronously()
        {
            Console.WriteLine("Calling the service synchronously...");
            using (var requestDispatcher = KernelContainer.Kernel.Get<IRequestDispatcher>())
            {
                Console.WriteLine("requesting a hello world... (synchronously)");
                requestDispatcher.Add(new HelloWorldRequest());
                Console.WriteLine("asking the server to reverse this string... (synchronously)");
                requestDispatcher.Add(new ReverseStringRequest
                                          {StringToReverse = "asking the server to reverse this string"});

                Console.WriteLine(requestDispatcher.Get<HelloWorldResponse>().Message);
                Console.WriteLine(requestDispatcher.Get<ReverseStringResponse>().ReversedString);

                Console.WriteLine("Sending HelloWorldCommand");
                requestDispatcher.Send(new HelloWorldCommand());
            }
        }

        private static void CallTheServiceAsynchronously()
        {
            Console.WriteLine("Calling the service asynchronously...");
            var requestDispatcher = KernelContainer.Kernel.Get<IAsyncRequestDispatcher>();

            Console.WriteLine("requesting a hello world... (asynchronously)");
            requestDispatcher.Add(new HelloWorldRequest());
            Console.WriteLine("asking the server to reverse this string... (asynchronously)");
            requestDispatcher.Add(new ReverseStringRequest
                                      {StringToReverse = "asking the server to reverse this string"});
            requestDispatcher.ProcessRequests(ResponsesReceived, e => Console.WriteLine(e.ToString()));
            // NOTE: this request dispatcher will be disposed once the responses have been received...

            Console.WriteLine("Sending HelloWorldCommand");
            requestDispatcher = KernelContainer.Kernel.Get<IAsyncRequestDispatcher>();
            requestDispatcher.Add(new HelloWorldCommand());
            requestDispatcher.ProcessOneWayRequests();
            // NOTE: and this other request dispatcher instance will also be disposed once the call has been _sent_ (since there is no response)
        }

        private static void ResponsesReceived(ReceivedResponses receivedResponses)
        {
            Console.WriteLine(receivedResponses.Get<HelloWorldResponse>().Message);
            Console.WriteLine(receivedResponses.Get<ReverseStringResponse>().ReversedString);
        }

        private static void InitializeAgatha()
        {
            var c = new ClientConfiguration(Assembly.Load("AgathaSample.Common"),
                                            new Container(KernelContainer.Kernel))
                        {
                            //  this wires up some custom types for implementing
                            //  the AuthenticatedRequest handling
                            //  this isn't necessary if you don't need such.
                            RequestProcessorImplementation = typeof (CustomRequestProcessorProxy),
                            RequestDispatcherImplementation = typeof (AuthenticatedRequestDispatcher),
                            AsyncRequestDispatcherImplementation = typeof (AuthenticatedAsyncRequestDispatcher),
                            AsyncRequestProcessorImplementation = typeof (CustomAsyncRequestProcessorProxy)
                        };
            c.Initialize();
        }
    }
}