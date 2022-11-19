using Cumulative_1_assignment_3.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Cumulative_1_assignment_3.Controllers
{
    public class TeacherDataController : ApiController
    {
        private SchoolDbContext School = new SchoolDbContext();

        /// access the teachers table of the school database
        ///<summary>
        /// returns a list of teachers in the system
        /// a search key is help to select the specific results
        /// </summary>
        /// <param name="SearchKey">Search key to filter</param>
        /// <example>
        /// GET api/teacherdata/listteachers 
        /// </example>
        /// <returns>
        /// TeacherId, TeacherFname, TeacherLname, EmployeeNumber, HireDate, TeacherSalary
        /// </returns>

        [HttpGet]
        [Route("api/teacherdata/listteachers/{SearchKey?}")]

        public IEnumerable<Teacher> ListTeachers(string SearchKey = null)
        {
            //Create an instance of connection to the school database
            MySqlConnection Conn = School.AccessDatabase();

            //Opent the connection between the web server and database
            Conn.Open();

            // sanitize the code to stop hackers
            // string query = "Select * from teachers where lower(teacherfname) like lower('%@sanitize%');
            // cmd.Parameters.AddWithValue("@sanitize", "%" + SearchKey + "%");
            // cmd.Prepare();

            string query = "Select * from teachers where lower(teacherfname) like lower('%" + SearchKey + "%')";

            MySqlCommand cmd = Conn.CreateCommand();
            cmd.CommandText = query;


            Debug.WriteLine("The query is " + query);


            MySqlDataReader ResultSet = cmd.ExecuteReader();


            // create an empty list 
            List<Teacher> Teachers = new List<Teacher>();

            //Loop to get all the rows from the database
            while (ResultSet.Read())
            {
                //Access Columns
                int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                string TeacherFname = ResultSet["teacherfname"].ToString();
                string TeacherLname = ResultSet["teacherlname"].ToString();
                string EmployeeNumber = ResultSet["employeenumber"].ToString();
                DateTime HireDate = Convert.ToDateTime(ResultSet["hiredate"]);
                double TeacherSalary = Convert.ToDouble(ResultSet["salary"]);

                // add every rows of information to the new list
                Teacher NewTeacher = new Teacher();

                NewTeacher.TeacherId = TeacherId;
                NewTeacher.TeacherFname = TeacherFname;
                NewTeacher.TeacherLname = TeacherLname;
                NewTeacher.EmployeeNumber = EmployeeNumber;
                NewTeacher.HireDate = HireDate;
                NewTeacher.TeacherSalary = TeacherSalary;

                Teachers.Add(NewTeacher);

            }

            //Close the connection
            Conn.Close();

            //Return the final list
            return Teachers;
        }

        /// <summary>
        /// similiar to the example from the class 
        /// get a teacher's full information 
        /// </summary>
        /// <param name="TeacherId">the primary key of the teachers table in the database</param>
        [HttpGet]
        [Route("api/TeacherData/FindTeacher/{TeacherId}")]
        public Teacher FindTeacher(int TeacherId)
        {

            MySqlConnection Conn = School.AccessDatabase();

            Conn.Open();

            string query = "Select * from teachers where teacherid = " + TeacherId.ToString();


            MySqlCommand cmd = Conn.CreateCommand();
            cmd.CommandText = query;


            MySqlDataReader ResultSet = cmd.ExecuteReader();
            Teacher SelectedTeacher = new Teacher();
            while (ResultSet.Read())
            {
                SelectedTeacher.TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                SelectedTeacher.TeacherFname = ResultSet["teacherfname"].ToString();
                SelectedTeacher.TeacherLname = ResultSet["teacherlname"].ToString();
                SelectedTeacher.EmployeeNumber = ResultSet["employeenumber"].ToString();
                SelectedTeacher.HireDate = Convert.ToDateTime(ResultSet["hiredate"]);
                SelectedTeacher.TeacherSalary = Convert.ToDouble(ResultSet["salary"]);

            }
            Conn.Close();
            return SelectedTeacher;

        }

    }
}
