namespace LibraryApi.DTO
{
    public class RentalResponse
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public long StartDate { get; set; }
        public long? EndDate { get; set; }
        public BookResponse Book { get; set; }
        public RenteeResponse Rentee { get; set; }
    }
}