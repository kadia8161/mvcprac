using System;
using System.Collections.Generic;

namespace MVCPrac.Models
{
    public partial class Student
    {
        public Student()
        {
            StudentCourse = new HashSet<StudentCourse>();
        }

        public long StudentId { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public byte Gender { get; set; }

        public virtual ICollection<StudentCourse> StudentCourse { get; set; }
    }
}
