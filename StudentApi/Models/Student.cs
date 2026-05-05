namespace StudentApi.Models
{
    /// <summary>
    /// Represents a student record stored in the database.
    /// </summary>
    public class Student
    {
        /// <summary>Primary key, auto-incremented by the database.</summary>
        public int Id { get; set; }

        /// <summary>The student's full name.</summary>
        public string FullName { get; set; } = "";

        /// <summary>The student's email address. Used as a contact identifier.</summary>
        public string Email { get; set; } = "";

        /// <summary>
        /// The date the student was enrolled.
        /// Defaults to the current UTC time when a new record is created.
        /// </summary>
        public DateTime EnrollmentDate { get; set; } = DateTime.UtcNow;
    }
}
