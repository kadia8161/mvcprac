using MVCPrac.BAL;
using MVCPrac.Models;
using System.Collections.Generic;
using System;
namespace MVCPrac.Helper
{
    public class StudentHelper : IData, IDisposable
    {
        StudentBAL StudentBAL;
        public StudentHelper()
        {
            StudentBAL = new StudentBAL();
        }
        public StudentListModel GetList(int PageNo, string sortByProp, string sortDir)
        {
            return StudentBAL.GetStudentList(PageNo, sortByProp, sortDir);
        }
        public StudentCourseModel GetData(long Id)
        {
            return StudentBAL.GetStudentData(Id);
        }
        public string SaveStudentCourse(StudentCourseModel model)
        {
            bool IsSaved = false;
            IsSaved = StudentBAL.SaveStudentCourse(model);
            if (IsSaved)
                return "";
            else
                return string.Join(",", StudentBAL.ErrorMsg.ToArray());

        }
        public string DeleteStudent(long Id)
        {
            bool IsSaved = false;
            IsSaved = StudentBAL.DeleteStudent(Id);
            if (IsSaved)
                return "";
            else
                return string.Join(",", StudentBAL.ErrorMsg.ToArray());
        }
        public void Dispose()
        {
            if (StudentBAL != null)
                StudentBAL.Dispose();
        }
    }
}