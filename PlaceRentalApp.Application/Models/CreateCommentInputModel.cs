namespace PlaceRentalApp.Application.Models
{
    public class CreateCommentInputModel
    {
        public int IdPlace { get; set; }
        public int IdUser { get; set; }
        public string Comment { get; set; }
    }
}
