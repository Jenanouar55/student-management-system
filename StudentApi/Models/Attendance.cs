namespace StudentApi.Models
{
    /// <summary>
    /// Represents an attendance record for a student on a specific date.
    /// </summary>
    public class Attendance
    {
        /// <summary>Primary key, auto-incremented by the database.</summary>
        public int Id { get; set; }

        /// <summary>Foreign key — links this record to a student.</summary>
        public int StudentId { get; set; }

        /// <summary>Navigation property — lets EF Core load the linked Student.</summary>
        public Student? Student { get; set; }

        /// <summary>The date of the class or session.</summary>
        public DateTime Date { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Attendance status: "Present", "Absent", or "Late".
        /// Stored as a plain string so no enum migration is needed.
        /// </summary>
        public string Status { get; set; } = "Present";

        /// <summary>Optional reason or remark (e.g. "sick", "family event").</summary>
        public string Remark { get; set; } = "";
    }
}
