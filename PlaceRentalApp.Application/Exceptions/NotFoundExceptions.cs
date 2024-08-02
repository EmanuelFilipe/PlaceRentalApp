namespace PlaceRentalApp.Application.Exceptions
{
    public class NotFoundExceptions : Exception
    {
        public NotFoundExceptions(string message = "Not Found") : base(message)
        {
            
        }
    }
}
