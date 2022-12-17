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

        /// <summary>
        /// Add a Teacher to the Database
        /// </summary>
        /// <param name="NewTeacher">
        /// An input from user to the create a new teacher's record to the column of the teachers' table
        /// </param>
        /// <example>
        /// POST api/teacherdata/addteacher 
        /// {
        ///	"TeacherId":"99",
        ///	"TeacherFname":"Albus",
        ///	"TeacherLname":"Dumbledore",
        ///	"EmployeeNumber":"9999",
        ///	"HireDate":"1990-12-25"
        ///	"TeacherSalary":"1000"
        /// }
        /// </example>
        [HttpPost]
        public void AddTeacher (Teacher NewTeacher)
        {
            // establish the link to the database
            MySqlConnection Conn = School.AccessDatabase();

            Conn.Open();

            // SQL query command
            string query = "Insert into teachers (teacherid, teacherfname, teacherlname, employeenumber,hiredate,salary) values(@TeacherId,@TeacherFname,@TeacherLname,@EmployeeNumber,@HireDate,@TeacherSalary)";
            MySqlCommand cmd = Conn.CreateCommand();
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@TeacherId", NewTeacher.TeacherId);
            cmd.Parameters.AddWithValue("@TeacherFname", NewTeacher.TeacherFname);
            cmd.Parameters.AddWithValue("@TeacherLname", NewTeacher.TeacherLname);
            cmd.Parameters.AddWithValue("@EmployeeNumber", NewTeacher.EmployeeNumber);
            cmd.Parameters.AddWithValue("@HireDate", NewTeacher.HireDate);
            cmd.Parameters.AddWithValue("@TeacherSalary", NewTeacher.TeacherSalary);

            cmd.Prepare();

            MySqlDataReader ResultSet = cmd.ExecuteReader();
            Teacher SelectedTeacher = new Teacher();
        }

        /// <summary>
        /// Deletes the selected Teacher (with provided ID) from the database 
        /// </summary>
        /// <param name="id">Teacher's ID</param>
        /// <example>
        /// POST /api/teacherdata/deleteteacher/321421
        /// </example>
        [HttpPost]
        public void DeleteTeacher(int id)
        {
            // establish the link to the database
            MySqlConnection Conn = School.AccessDatabase();

            Conn.Open();

            // SQL query command
            MySqlCommand cmd = Conn.CreateCommand();
            cmd.CommandText = "Delete from teachers where teacherid=@id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            // close the link
            Conn.Close();
        }

        /// <summary>
        /// Updates an existing Teachers' infomation on the database
        /// </summary>
        /// <param name="TeacherInfo">
        /// New information will be updated to the selected teacher 
        /// </param>
        /// <example>
        /// POST api/TeacherData/UpdateTeacher/321421
        /// {
        ///	"TeacherId":"321421",
        ///	"TeacherFname":"Harry",
        ///	"TeacherLname":"Potter",
        ///	"EmployeeNumber":"8888",
        ///	"HireDate":"0001-01-01"
        ///	"TeacherSalary":"33"       
        /// }
        /// </example>
        [HttpPost]
        public void UpdateTeacher(int id, Teacher TeacherInfo)
        {
            // establish the link to the database
            MySqlConnection Conn = School.AccessDatabase();

            Conn.Open();

            // SQL query command
            MySqlCommand cmd = Conn.CreateCommand();
            // ask user to input new infomation to update the selected teacher
            cmd.CommandText = "update teachers set teacherfname=@TeacherFname, teacherlname=@TeacherLname, employeenumber=@EmployeeNumber, hiredate=@HireDate  where salary=@TeacherSalary";
            cmd.Parameters.AddWithValue("@TeacherId", id);
            cmd.Parameters.AddWithValue("@TeacherFname", TeacherInfo.TeacherFname);
            cmd.Parameters.AddWithValue("@TeacherLname", TeacherInfo.TeacherLname);
            cmd.Parameters.AddWithValue("@EmployeeNumber", TeacherInfo.EmployeeNumber);
            cmd.Parameters.AddWithValue("@HireDate", TeacherInfo.HireDate);
            cmd.Parameters.AddWithValue("@TeacherSalary", TeacherInfo.TeacherSalary);

            cmd.Prepare();
            cmd.ExecuteNonQuery();

            // close the link
            Conn.Close();
        }
    }
}
