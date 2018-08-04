﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.BookingApi
{
    public class MaîtreDEffects : IMaîtreD
    {
        public MaîtreDEffects(int capacity)
        {
            Capacity = capacity;
        }

        public int Capacity { get; }

        public async ReservationsProgram<int?> TryAccept(Reservation reservation)
        {
            if (!await ReservationsRepository.IsReservationInFuture(reservation))
                return null;

            var reservedSeats = (await ReservationsRepository.ReadReservations(reservation.Date))
                                .Sum(r => r.Quantity);
            if (Capacity < reservedSeats + reservation.Quantity)
                return null;

            reservation.IsAccepted = true;
            return await ReservationsRepository.Create(reservation);
        }

        public MaîtreDEffects WithCapacity(int newCapacity)
        {
            return new MaîtreDEffects(newCapacity);
        }
    }
}
