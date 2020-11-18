using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace MVCPrac.Models
{
    public class StudentCourseModel : StudentModel
    {         
        public List<CourseModel> Courses { get; set; }         
    }
}