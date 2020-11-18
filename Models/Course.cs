using System;
using System.Collections.Generic;

namespace MVCPrac.Models
{
    public partial class Course
    {
        public Course()
        {
            StudentCourse = new HashSet<StudentCourse>();
        }

        public int CourseId { get; set; }
        public string Course1 { get; set; }

        public virtual ICollection<StudentCourse> StudentCourse { get; set; }
    }
}
