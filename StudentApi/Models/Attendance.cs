namespace StudentApi.Models
{
    public class Attendance
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public Student? Student { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "Present";
        public string Remark { get; set; } = "";
    }
}
