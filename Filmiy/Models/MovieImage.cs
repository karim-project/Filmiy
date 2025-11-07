namespace Filmiy.Models
{
    public class MovieImage
    {
        public int Id { get; set; }

        public string FileName { get; set; } = null!;

        public int MovieId { get; set; }
        public Movie? Movie { get; set; }
        public string Img { get; set; } = string.Empty;
    }
}
