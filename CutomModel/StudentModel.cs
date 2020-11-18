using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace MVCPrac.Models
{
    public class StudentModel
    {

        [Required]
        public long StudentId { get; set; }
        [Required]
        [MaxLength(300)]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Required]

        public DateTime DateOfBirth { get; set; }
        [Required]
        public int Gender { get; set; }
        public string Gender_Str { get; set; }
        public int Age { get; set; }
        [Display(Name = "Birth Date")]
        public string BirthDate { get; set; }
        public List<CourseModel> SelectedCourses { get; set; }
    }

    public class StudentListModel
    {
        public List<StudentModel> lstStudentModel { get; set; }
        public int TotalRecords { get; set; }
        public int PageNo { set; get; }
    }
}