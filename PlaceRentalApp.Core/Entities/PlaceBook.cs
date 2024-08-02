namespace PlaceRentalApp.Core.Entities
{
    public class PlaceBook : BaseEntity
    {
        protected PlaceBook()
        {
        }

        public PlaceBook(int idUser, int idPlace, DateTime startDate, DateTime endDate, string comments)
        {
            IdUser = idUser;
            IdPlace = idPlace;
            StartDate = startDate;
            EndDate = endDate;
            Comments = comments;
        }

        public int IdUser { get; set; }
        public User User { get; private set; }
        public int IdPlace { get; set; }
        public Place Place { get; private set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Comments { get; set; }
    }
}
