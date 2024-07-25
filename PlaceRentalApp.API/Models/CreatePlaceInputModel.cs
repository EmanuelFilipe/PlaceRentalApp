namespace PlaceRentalApp.API.Models
{
    public class CreatePlaceInputModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal DailyPrice { get; set; }
        public AddressInputModel Address { get; set; }
        public int AllowedNumberPerson { get; set; }
        public bool AllowPets{ get; set; }
        public int CreatedBy { get; set; }
    }
}
