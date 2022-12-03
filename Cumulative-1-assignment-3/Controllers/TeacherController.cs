using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cumulative_1_assignment_3.Models;
using System.Diagnostics;


namespace Cumulative_1_assignment_3.Controllers
{
    public class TeacherController : Controller
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

        // GET: /Teacher/New

        public ActionResult New()
        {
            return View();
        }

        
        // GET: /Teacher/Create
        [HttpPost]
        public ActionResult Create(int TeacherId, string TeacherFname, string TeacherLname, string EmployeeNumber, DateTime HireDate, int TeacherSalary)
        {

            Debug.WriteLine("debugging");
            Debug.WriteLine(TeacherId);
            Debug.WriteLine(TeacherFname);
            Debug.WriteLine(TeacherLname);
            Debug.WriteLine(EmployeeNumber);
            Debug.WriteLine(HireDate);
            Debug.WriteLine(TeacherSalary);


            Teacher NewTeacher = new Teacher();
            NewTeacher.TeacherId = TeacherId;
            NewTeacher.TeacherFname = TeacherFname;
            NewTeacher.TeacherLname = TeacherLname;
            NewTeacher.EmployeeNumber = EmployeeNumber;
            NewTeacher.HireDate = HireDate;
            NewTeacher.TeacherSalary = TeacherSalary;



            TeacherDataController controller = new TeacherDataController();
            controller.AddTeacher(NewTeacher);



            return RedirectToAction("List");
        }









    }
}

