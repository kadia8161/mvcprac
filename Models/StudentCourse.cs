using System;
using System.Collections.Generic;

namespace MVCPrac.Models
{
    public partial class StudentCourse
    {
        public long StudentCourseId { get; set; }
        public int CourseId { get; set; }
        public long StudentId { get; set; }

        public virtual Course Course { get; set; }
        public virtual Student Student { get; set; }
    }
}
