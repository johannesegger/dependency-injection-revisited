using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Ploeh.Samples.BookingApi.Sql
{
    public class SqlReservationsProgramHandler : IReservationInstructionHandler
    {
        private readonly string connectionString;

        public SqlReservationsProgramHandler(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public Task<bool> Handle(IsReservationInFuture instruction)
        {
            var isInFuture = DateTimeOffset.Now < instruction.Reservation.Date;
            return Task.FromResult(isInFuture);
        }

        public Task<IReadOnlyCollection<Reservation>> Handle(ReadReservations instruction)
        {
            var reservations = ReadReservations(
                instruction.Date.Date,
                instruction.Date.Date.AddDays(1).AddTicks(-1));
            return Task.FromResult(reservations);
        }

        private IReadOnlyCollection<Reservation> ReadReservations(
            DateTimeOffset min,
            DateTimeOffset max)
        {
            var result = new List<Reservation>();

            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(readByRangeSql, conn))
            {
                cmd.Parameters.Add(new SqlParameter("@MinDate", min));
                cmd.Parameters.Add(new SqlParameter("@MaxDate", max));

                conn.Open();
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                        result.Add(
                            new Reservation
                            {
                                Date = (DateTimeOffset)rdr["Date"],
                                Name = (string)rdr["Name"],
                                Email = (string)rdr["Email"],
                                Quantity = (int)rdr["Quantity"]
                            });
                }
            }

            return result;
        }

        private const string readByRangeSql = @"
            SELECT [Date], [Name], [Email], [Quantity]
            FROM [dbo].[Reservations]
            WHERE YEAR(@MinDate) <= YEAR([Date])
            AND MONTH(@MinDate) <= MONTH([Date])
            AND DAY(@MinDate) <= DAY([Date])
            AND YEAR([Date]) <= YEAR(@MaxDate)
            AND MONTH([Date]) <= MONTH(@MaxDate)
            AND DAY([Date]) <= DAY(@MaxDate)";

        public Task<int> Handle(CreateReservation instruction)
        {
            return Task.FromResult(Create(instruction.Reservation));
        }

        private int Create(Reservation reservation)
        {
            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(createReservationSql, conn))
            {
                cmd.Parameters.Add(
                    new SqlParameter("@Date", reservation.Date));
                cmd.Parameters.Add(
                    new SqlParameter("@Name", reservation.Name));
                cmd.Parameters.Add(
                    new SqlParameter("@Email", reservation.Email));
                cmd.Parameters.Add(
                    new SqlParameter("@Quantity", reservation.Quantity));

                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        private const string createReservationSql = @"
            INSERT INTO [dbo].[Reservations] ([Date], [Name], [Email], [Quantity])
            VALUES (@Date, @Name, @Email, @Quantity)";
    }
}
