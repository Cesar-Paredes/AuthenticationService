namespace AuthenticationService.Models
{
    public class ExceptionModel
    {
        public string? Type { get; set; }
        public string? Title { get; set; }
        public int Status { get; set; }
        public string? Description { get; set; }
    }
}