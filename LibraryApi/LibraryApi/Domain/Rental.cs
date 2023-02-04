using System;
using LibraryApi.Enums;

namespace LibraryApi.Domain
{
    public class Rental
    {
        private Rental()
        {
        }

        public Rental(int userId, int bookId)
        {
            UserId = userId;
            BookId = bookId;
            Status = RentalStatus.Pending;
            StartDate = DateTime.Now;
        }

        public int Id { get; }
        public int UserId { get; }
        public int BookId { get; }
        public RentalStatus Status { get; private set; }
        public DateTime StartDate { get; }
        public DateTime? EndDate { get; private set; }

        public User User { get; }
        public Book Book { get; }

        public void CloseRental()
        {
            EndDate = DateTime.Now;
            switch (Status)
            {
                case RentalStatus.Active:
                    Status = RentalStatus.Completed;
                    break;
                case RentalStatus.Pending:
                    Status = RentalStatus.Canceled;
                    break;
            }
        }

        public void ActivateRental()
        {
            Status = RentalStatus.Active;
        }

        public bool IsPending()
        {
            return Status is RentalStatus.Pending;
        }

        public bool IsCanceled()
        {
            return Status is RentalStatus.Canceled;
        }

        public bool IsCompleted()
        {
            return Status is RentalStatus.Completed;
        }
    }
}