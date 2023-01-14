namespace MoviesApi.Dtos
{
    public class CreateGenrasDto
    {
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
