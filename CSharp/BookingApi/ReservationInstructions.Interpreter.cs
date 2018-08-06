using System;
using System.Collections.Generic;

namespace Ploeh.Samples.BookingApi
{
    public static class ReservationInstruction
    {
        public static ReservationInstruction<bool> IsReservationInFuture(Reservation reservation)
            => new IsReservationInFuture(reservation);
        public static ReservationInstruction<IReadOnlyCollection<Reservation>> ReadReservations(DateTimeOffset date)
            => new ReadReservations(date);
        public static ReservationInstruction<int> CreateReservation(Reservation reservation)
            => new CreateReservation(reservation);
    }
}
