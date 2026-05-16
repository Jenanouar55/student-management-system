using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace StudentApi.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public int Credits { get; set; }
        public string Teacher { get; set; } = "";

        [ValidateNever]
        public ICollection<Grade> Grades { get; set; } = new List<Grade>();
    }
}
