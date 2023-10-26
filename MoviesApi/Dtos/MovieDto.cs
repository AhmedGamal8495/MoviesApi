using MoviesApi.Models;

namespace MoviesApi.Dtos
{
    public class MovieDto
    {
        [MaxLength(250)]
        public string Title { get; set; }

        public string StoryLine { get; set; }

        public int Year { get; set; }

        public double Rate { get; set; }

        public IFormFile? Poster { get; set; }

        public byte GenraId { get; set; }


    }
}
