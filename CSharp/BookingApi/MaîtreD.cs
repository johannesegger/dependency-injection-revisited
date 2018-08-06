using System.Linq;

namespace Ploeh.Samples.BookingApi
{
    public class MaîtreD : IMaîtreD
    {
        public MaîtreD(int capacity)
        {
            Capacity = capacity;
        }

        public int Capacity { get; }

        public async ReservationInstructionProgram<int?> TryAccept(Reservation reservation)
        {
            if (!await ReservationInstruction.IsReservationInFuture(reservation))
                return null;

            var reservedSeats = (await ReservationInstruction.ReadReservations(reservation.Date))
                                .Sum(r => r.Quantity);
            if (Capacity < reservedSeats + reservation.Quantity)
                return null;

            reservation.IsAccepted = true;
            return await ReservationInstruction.CreateReservation(reservation);
        }

        public MaîtreD WithCapacity(int newCapacity)
        {
            return new MaîtreD(newCapacity);
        }
    }
}
