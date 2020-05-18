using System;
using Actio.Common.RabbitMQ;

namespace Actio.ApiGateway.Messages.Commands.Activities
{
    [Message("activities")]
    public class CreateActivity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}