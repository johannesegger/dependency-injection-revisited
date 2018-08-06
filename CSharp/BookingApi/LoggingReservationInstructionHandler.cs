using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ploeh.Samples.BookingApi
{
    public class LoggingReservationInstructionHandler : IReservationInstructionHandler
    {
        public List<IReservationInstruction> Instructions { get; } = new List<IReservationInstruction>();

        public Task Handle(IsReservationInFuture instruction)
        {
            Instructions.Add(instruction);
            return Task.CompletedTask;
        }
        public Task Handle(ReadReservations instruction)
        {
            Instructions.Add(instruction);
            return Task.CompletedTask;
        }
        public Task Handle(CreateReservation instruction)
        {
            Instructions.Add(instruction);
            return Task.CompletedTask;
        }
    }
}
