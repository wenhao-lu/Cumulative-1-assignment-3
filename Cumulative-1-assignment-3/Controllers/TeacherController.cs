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
            // debug section
            Debug.WriteLine("debugging");
            Debug.WriteLine(TeacherId);
            Debug.WriteLine(TeacherFname);
            Debug.WriteLine(TeacherLname);
            Debug.WriteLine(EmployeeNumber);
            Debug.WriteLine(HireDate);
            Debug.WriteLine(TeacherSalary);

            // input information to create a new teacher record
            Teacher NewTeacher = new Teacher();
            NewTeacher.TeacherId = TeacherId;
            NewTeacher.TeacherFname = TeacherFname;
            NewTeacher.TeacherLname = TeacherLname;
            NewTeacher.EmployeeNumber = EmployeeNumber;
            NewTeacher.HireDate = HireDate;
            NewTeacher.TeacherSalary = TeacherSalary;

            TeacherDataController controller = new TeacherDataController();

            controller.AddTeacher(NewTeacher);
            // return to the 'Teacher/List' page
            return RedirectToAction("List");
        }

        
        //GET : /Teacher/DeleteConfirm/{id}
        public ActionResult DeleteConfirm(int id)
        {
            // in the 'Teacher/DeleteConfirm/id' page, show the selected teacher's info and ask for delete confirmation
            TeacherDataController controller = new TeacherDataController();
            Teacher NewTeacher = controller.FindTeacher(id);
            return View(NewTeacher);
        }

        //POST : /Teacher/Delete/{id}
        [HttpPost]
        public ActionResult Delete(int id)
        {
            // delete the teacher, redirect to the 'Teacher/List' page
            TeacherDataController controller = new TeacherDataController();
            controller.DeleteTeacher(id);
            return RedirectToAction("List");
        }


        /// <summary>
        /// create a link to the selected teacher update web page ('Update.cshtml')
        /// </summary>
        /// <param name="id">Teacheris ID</param>
        /// <returns>
        /// show current information of the teacher, ask user input new info to update this teacher
        /// </returns>
        /// <example>
        /// GET : /Teacher/Update/99
        /// </example>
        public ActionResult Update(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            Teacher SelectedTeacher = controller.FindTeacher(id);
            return View(SelectedTeacher);
        }


        /// <summary>
        /// Post the update request to the database, with teacher's new infomation that would be updated   
        /// after update, redirect to the 'Show' page with teacher's new info 
        /// </summary>
        /// <param name="id">Id of the Teacher to update</param>
        /// <param name="TeacherFname">Teacher's updated First Name</param>
        /// <param name="TeacherLname">Teacher's updated Last Name</param>
        /// <param name="EmployeeNumber">Teacher's updated Employee Number</param>
        /// <param name="HireDate">Teacher's updated Hire Date</param>
        /// <param name="TeacherSalary">Teacher's updated Salary</param>
        /// <returns>A page shows the teacher's current information</returns>
        /// <example>
        /// POST : /Teacher/Update/99
        /// FORM DATA / POST DATA / REQUEST BODY 
        /// {
        ///	"TeacherFname":"Albus",
        ///	"TeacherLname":"Dumbledore",
        ///	"EmployeeNumber":"9999",
        ///	"HireDate":"1990-12-25",
        ///	"TeacherSalary":"1000"
        /// }
        /// </example>
        [HttpPost]
        public ActionResult Update(int id, string TeacherFname, string TeacherLname, string EmployeeNumber, DateTime HireDate, double TeacherSalary)
        {
            // input teacher's new information
            Teacher TeacherInfo = new Teacher();
            TeacherInfo.TeacherFname = TeacherFname;
            TeacherInfo.TeacherLname = TeacherLname;
            TeacherInfo.EmployeeNumber = EmployeeNumber;
            TeacherInfo.HireDate = HireDate;
            TeacherInfo.TeacherSalary = TeacherSalary;

            TeacherDataController controller = new TeacherDataController();
            controller.UpdateTeacher(id, TeacherInfo);
            // redirect to 'Teacher/Show/id' page  
            return RedirectToAction("Show/" + id);
        }



    }
}

