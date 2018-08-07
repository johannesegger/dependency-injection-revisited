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

        public Task<bool> Handle(IsReservationInFuture instr)
        {
            return Task.FromResult(isInFuture);
        }

        public Task<IReadOnlyCollection<Reservation>> Handle(ReadReservations instr)
        {
            return Task.FromResult(reservations);
        }

        public Task<int> Handle(CreateReservation instr)
        {
            return Task.FromResult(id);
        }
    }
}
