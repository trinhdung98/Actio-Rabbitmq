using System;
using Actio.Common.Messages;

namespace Actio.Services.Activities.Messages.Commands
{
    public class CreateActivity : ICommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}