namespace MoviesApi.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [MaxLength(250)]
        public string Title { get; set; }  

        public string StoryLine { get; set; }

        public int Year  { get; set;}

        public double Rate { get; set; }

        public byte[] Poster { get; set; }  

        public byte GenraId { get; set; }

        public Genra Genra { get; set; }

    }
}
