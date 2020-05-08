using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using HogeHoge.App;
using System;
using System.Threading.Tasks;

namespace HogeHoge.Rpc
{
    public class CommanderService : Commander.CommanderBase
    {
        public override Task<CommanderResult> Activate(Empty request, ServerCallContext context)
        {
            Console.WriteLine("Requested: Activate");
            // ... some actual tasks
            return Task.FromResult(new CommanderResult() { Success = true, Message = "Hoge!" });
        }

        public override async Task<CommanderResult> StartConnect(StartConnectRequest request, ServerCallContext context)
        {
            var message = $"Requested: StartConnect({request.ConnectionType}, {request.ConnectionId})";
            Console.WriteLine(message);
            var asyncResult = await Task.Run(() =>
            {
                // ... some actual tasks
                return message;
            });
            return new CommanderResult { Success = request.ConnectionId >= 0, Message = asyncResult };
        }
    }
}
