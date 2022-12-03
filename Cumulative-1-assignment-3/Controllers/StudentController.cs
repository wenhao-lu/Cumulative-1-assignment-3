using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cumulative_1_assignment_3.Models;


namespace Cumulative_1_assignment_3.Controllers
{
    public class StudentController : Controller
    {
        // GET: /Teacher/List?SearchKey={value}
        public ActionResult List(string SearchKey)
        {

            // get the user input from the searchbar
            Debug.WriteLine("SearchKey is " + SearchKey);

            // getting the data from the TeacherDataController
            TeacherDataController MyController = new TeacherDataController();
            IEnumerable<Teacher> dataTeachers = MyController.ListTeachers(SearchKey);

            // for debug
            Debug.WriteLine("I got " + dataTeachers.Count());

            return View(dataTeachers);
        }

        // GET: /Teacher/Show/{TeacherId}
        public ActionResult Show(int id)
        {
            TeacherDataController MyController = new TeacherDataController();
            Teacher SelectedTeacher = MyController.FindTeacher(id);
            return View(SelectedTeacher);
        }
    }
}

