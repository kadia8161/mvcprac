using MVCPrac.BAL;
using MVCPrac.Models;
using System.Collections.Generic;
using System;
namespace MVCPrac.Helper
{
    public class CourseHelper : IData, IDisposable
    {
         CourseBAL CourseBAL;
        public CourseHelper()
        {
            CourseBAL = new CourseBAL();
        }
        public List<CourseModel> GetList()
        {
            return CourseBAL.GetCoursList();
        }
        public void Dispose()
        {
            if (CourseBAL != null)
                CourseBAL.Dispose();
        }
    }
}