using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MVCPrac.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MVCPrac.Common;

namespace MVCPrac.BAL
{
    public class StudentBAL : IDisposable
    {
        private Interview06092020_akashContext ctx;
        public List<string> ErrorMsg;
        public StudentBAL()
        {
            ctx = new Interview06092020_akashContext();
            ErrorMsg = new List<string>();
        }

        public StudentCourseModel GetStudentData(long Id)
        {
            // ctx.Student.Where(p => p.StudentId == Id).FirstOrDefault();
            //return ctx.Student.Where(p => p.StudentId == Id).Select(p=> new StudentCourseModel()
            var model = ctx.Student.Where(p => p.StudentId == Id).Select(p => new StudentCourseModel()
            {
                StudentId = p.StudentId,
                Name = p.Name,
                DateOfBirth = p.DateOfBirth,
                Gender = p.Gender,
                SelectedCourses = p.StudentCourse.Select(
                   q => new CourseModel
                   {
                       CourseId = q.CourseId,
                       CourseName = q.Course.Course1
                   }).ToList()
            }).FirstOrDefault();
            model.Gender_Str = model.Gender == 0 ? "Male" : "Famale";
            model.BirthDate = model.DateOfBirth.ToString("dd-MMM-yyyy");
            return model;
            // return new StudentCourseModel()
            // {
            //     StudentId = model.StudentId,
            //     Name = model.Name,
            //     DateOfBirth = model.DateOfBirth,
            //     Gender = model.Gender,
            //     Gender_Str = model.Gender == 1 ? "Male" : "Famale",
            //     BirthDate = model.DateOfBirth.ToString("dd-MMM-yyyy"),
            //     SelectedCourses = model.StudentCourse.Select(
            //        p => new CourseModel
            //        {
            //            CourseId = p.CourseId,
            //            CourseName = p.Course.Course1
            //        }).ToList()
            // };
        }

        public StudentListModel GetStudentList(int PageNo, string sortBy = "Name", string orderByDir = "ASC")
        {
            StudentListModel lstmodel = new StudentListModel();
            var CountRec = new SqlParameter("@CountRec", 0);
            CountRec.Direction = ParameterDirection.InputOutput;
            if (ctx.Student.Any())
                lstmodel.TotalRecords = ctx.Student.Where(p => p.StudentId > 0).Count();
            else
                lstmodel.TotalRecords = 0;
            if (string.IsNullOrEmpty(sortBy))
                sortBy = "Name";

            int SkipRecords = 0;
            if (PageNo > 1)
                SkipRecords = Startup.PageSize * (PageNo - 1);

            //List<Student> model = ctx.Student.ToList();
            List<Student> model = null;
            if (sortBy != "Age")
            {
                if (orderByDir == "DESC")
                    model = ctx.Student.OrderByDescending(sortBy).Skip(SkipRecords).Take(Startup.PageSize).ToList();
                else
                    model = ctx.Student.OrderBy(sortBy).Skip(SkipRecords).Take(Startup.PageSize).ToList();
            }
            else
                model = ctx.Student.ToList();
            var lstStudentmodel = model.Select(
                  p => new StudentModel()
                  {
                      StudentId = p.StudentId,
                      Name = p.Name,
                      DateOfBirth = p.DateOfBirth,
                      Gender = p.Gender,
                      Gender_Str = p.Gender == 0 ? "Male" : "Famale",
                      BirthDate = p.DateOfBirth.ToString("dd-MMM-yyyy"),
                      Age = int.Parse(((DateTime.Now - p.DateOfBirth).Days / 365).ToString())
                  }).ToList();
            if (sortBy == "Age")
            {
                if (orderByDir == "DESC")
                    lstmodel.lstStudentModel = lstStudentmodel.OrderByDescending(p => p.Age).Skip(SkipRecords).Take(Startup.PageSize).ToList();
                else
                    lstmodel.lstStudentModel = lstStudentmodel.OrderBy(p => p.Age).Skip(SkipRecords).Take(Startup.PageSize).ToList();
            }
            else
                lstmodel.lstStudentModel = lstStudentmodel;
            return lstmodel;
        }

        public bool SaveStudentCourse(StudentCourseModel savemodel)
        {
            try
            {
                var StudentId = new SqlParameter("@StudentId", savemodel.StudentId);
                StudentId.Direction = ParameterDirection.InputOutput;
                var Name = new SqlParameter("@Name", savemodel.Name);
                var DateOfBirth = new SqlParameter("@DateOfBirth", savemodel.DateOfBirth);
                var Gender = new SqlParameter("@Gender", savemodel.Gender);
                var CourseIds = new SqlParameter("@CourseIds", string.Join(",", savemodel.SelectedCourses.Select(p => p.CourseId).ToArray()));
                int retval = ctx.Database.ExecuteSqlRaw(@"[dbo].[prcStudentCourseInsert] @StudentId OUTPUT
                                            ,@Name
                                            ,@DateOfBirth
                                            ,@Gender
                                            ,@CourseIds", new object[] { StudentId, Name, DateOfBirth, Gender, CourseIds });
                if (retval > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                ErrorMsg.Add(ex.Message);
                return false;
            }
        }

        public bool DeleteStudent(long Id)
        {
            try
            {
                var StudentId = new SqlParameter("@StudentId", Id);
                int retval = ctx.Database.ExecuteSqlRaw("[dbo].[prcDeleteStudent] @StudentId ", new object[] { StudentId });
                if (retval > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                ErrorMsg.Add(ex.Message);
                return false;
            }
        }
        public void Dispose()
        {
            if (ctx != null)
                ctx.Dispose();
        }
    }
}