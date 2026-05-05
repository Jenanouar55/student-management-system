namespace StudentApi.Models
{
    /// <summary>
    /// Represents a grade (note) for a student in a given subject.
    /// </summary>
    public class Grade
    {
        /// <summary>Primary key, auto-incremented by the database.</summary>
        public int Id { get; set; }

        /// <summary>Foreign key — links this grade to a student.</summary>
        public int StudentId { get; set; }

        /// <summary>Navigation property — lets EF Core load the linked Student.</summary>
        public Student? Student { get; set; }

        /// <summary>Name of the subject (e.g. "Math", "Physics").</summary>
        public string Subject { get; set; } = "";

        /// <summary>Numeric grade value (e.g. 0–20 or 0–100).</summary>
        public double Score { get; set; }

        /// <summary>Optional note or comment about this grade.</summary>
        public string Note { get; set; } = "";

        /// <summary>When this grade was recorded.</summary>
        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
