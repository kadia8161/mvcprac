
using System.Collections.Generic;
using System.Linq;
using MVCPrac.Helper;
using MVCPrac.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
namespace MVCPrac.Controllers
{
    public class StudentController : Controller
    {
        private readonly ILogger<StudentController> _logger;

        public StudentController(ILogger<StudentController> logger)
        {
            _logger = logger;
        }

        //public async Task<IActionResult> Index(string sortOrder)
        public IActionResult Index(string sortByProp, string sortDir, int PageNo = 1)
        {
            using (StudentHelper helper = new StudentHelper())
            {
                var list = helper.GetList(PageNo, sortByProp, sortDir);
                ViewBag.PageNo = PageNo;
                ViewBag.SortDir = ((string.IsNullOrEmpty(sortDir) || sortDir == "DESC") ? "ASC" : "DESC");
                ViewBag.sortByProp = sortByProp;
                int MaxPageNo = 1;
                if (list.TotalRecords > 0)
                {
                    if (list.TotalRecords % Startup.PageSize == 0)
                        MaxPageNo = list.TotalRecords / Startup.PageSize;
                    else
                        MaxPageNo = (list.TotalRecords / Startup.PageSize) + 1;
                }
                ViewData["MaxPageNo"] = MaxPageNo;
                ViewData["TotalRecords"] = list.TotalRecords;
                return View(list.lstStudentModel);
            }
        }

        public IActionResult Details(long? Id)
        {
            using (StudentHelper helper = new StudentHelper())
            {
                StudentCourseModel model = new StudentCourseModel();
                model.SelectedCourses = new List<CourseModel>();
                if (Id.HasValue)
                    model = helper.GetData(Id.Value);

                using (CourseHelper chelper = new CourseHelper())
                {
                    model.Courses = chelper.GetList();
                }
                if (!Id.HasValue || Id.Value == 0)
                {
                    model.DateOfBirth = System.DateTime.Now;
                    model.BirthDate = System.DateTime.Now.ToString("dd-MMM-yyyy");
                }
                return View(model);
            }
        }

        public IActionResult Delete(long Id)
        {
            using (StudentHelper helper = new StudentHelper())
            {
                string Msg = helper.DeleteStudent(Id);
                if (Msg == "")
                    return new JsonResult(new { valid = true, msg = "" });
                else
                    return new JsonResult(new { valid = false, msg = Msg });
            }
        }

        public IActionResult Save(StudentCourseModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.Where(p => p.Errors.Count > 0)
                .Select(p => string.Join(",", p.Errors.Select(q => q.ErrorMessage).ToArray()));
                return new JsonResult(new { valid = false, msg = string.Join(",", errors.ToArray()) });
            }
            using (StudentHelper helper = new StudentHelper())
            {
                string Msg = helper.SaveStudentCourse(model);
                if (Msg == "")
                    return new JsonResult(new { valid = true, msg = "" });
                else
                    return new JsonResult(new { valid = false, msg = Msg });
            }
        }
    }
}