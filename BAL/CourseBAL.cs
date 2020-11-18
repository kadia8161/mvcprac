using System.Linq;
using System.Collections.Generic;
using MVCPrac.Models;
using System;
namespace MVCPrac.BAL
{
    public class CourseBAL : IDisposable
    {
        private Interview06092020_akashContext ctx;
        public List<string> ErrorMsg;
        public CourseBAL()
        {
            ctx = new Interview06092020_akashContext();
            ErrorMsg = new List<string>();
        }

        public List<CourseModel> GetCoursList()
        {
            return ctx.Course.Select(p => new CourseModel()
            {
                CourseId = p.CourseId,
                CourseName = p.Course1
            }).ToList();
        }
        public void Dispose()
        {
            if (ctx != null)
                ctx.Dispose();
        }
    }
}