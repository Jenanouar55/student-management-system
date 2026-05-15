namespace StudentApi.Models
{
    public class Grade
    {
        public int Id { get; set; }

        public int StudentId { get; set; }
        public Student? Student { get; set; }

        public int CourseId { get; set; }
        public Course? Course { get; set; }

        public double Score { get; set; }
        public string Note { get; set; } = "";
        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
