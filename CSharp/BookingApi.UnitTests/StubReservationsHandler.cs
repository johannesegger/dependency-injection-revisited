using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ploeh.Samples.BookingApi.UnitTests
{
    public class StubReservationsHandler : IReservationInstructionHandler
    {

        private readonly bool isInFuture;
        private readonly IReadOnlyCollection<Reservation> reservations;
        private readonly int id;

        public StubReservationsHandler(
            bool isInFuture,
            IReadOnlyCollection<Reservation> reservations,
            int id)
        {
            this.isInFuture = isInFuture;
            this.reservations = reservations;
            this.id = id;
        }

        public Task Handle(IsReservationInFuture instr)
        {
            instr.SetResult(isInFuture);
            return Task.CompletedTask;
        }

        public Task Handle(ReadReservations instr)
        {
            instr.SetResult(reservations);
            return Task.CompletedTask;
        }

        public Task Handle(CreateReservation instr)
        {
            instr.SetResult(id);
            return Task.CompletedTask;
        }
    }
}
