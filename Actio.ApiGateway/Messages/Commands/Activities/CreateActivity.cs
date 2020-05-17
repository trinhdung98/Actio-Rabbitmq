using System;

namespace Actio.ApiGateway.Messages.Commands.Activities
{
    public class CreateActivity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}