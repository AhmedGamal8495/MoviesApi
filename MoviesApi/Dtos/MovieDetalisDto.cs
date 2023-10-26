using MoviesApi.Models;

namespace MoviesApi.Dtos
{
    public class MovieDetalisDto
    {

        public int Id { get; set; }

        public string Title { get; set; }

        public string StoryLine { get; set; }

        public int Year { get; set; }

        public double Rate { get; set; }

        public byte[] Poster { get; set; }

        public byte GenraId { get; set; }

        public string GenraName { get; set; }
    }
}
