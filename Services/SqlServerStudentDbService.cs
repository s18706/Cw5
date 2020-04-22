using System;
using System.Data.SqlClient;
using Cw5.DTOs;
using Cw5.DTOs.Requests;
using Cw5.DTOs.Responses;

namespace Cw5.Services
{
    public class SqlServerStudentDbService : IStudentDbService
    {

        public SqlServerStudentDbService(/*.. */ )
        {

        }

        public void EnrollStudent(EnrollStudentRequest request)
        {
            using (var con = new SqlConnection("Server=localhost,32770;Initial Catalog=s18706;User ID=sa;Password=Root1234"))
            using (var com = new SqlCommand())
            {
                com.Connection = con;

                con.Open();
                var tran = con.BeginTransaction();

                try
                {
                    com.CommandText = "select IdStudies from studies where name=@name";
                    com.Parameters.AddWithValue("name", request.Studies);

                    var dr = com.ExecuteReader();
                    if (!dr.Read())
                    {
                        tran.Rollback();
                    }

                    int idstudies = (int) dr["IdStudies"];

                    com.CommandText = "select * from Enrollment where semester=1 AND idstudy=@idstudy";
                    com.Parameters.AddWithValue("idstudy", idstudies);
                    dr = com.ExecuteReader();

                    if (!dr.Read())
                    {
                        com.CommandText =
                            "Insert Into Enrollment VALUES ((SELECT MAX(IdEnrollment) FROM Enrollment) + 1,"
                            + "1,@idstudy,@date";
                        com.Parameters.AddWithValue("idstudy", idstudies);
                        com.Parameters.AddWithValue("date", DateTime.Now);
                        com.ExecuteReader();
                        com.CommandText = "select * from Enrollment where semester=1 AND idstudy=@idstudy";
                        dr = com.ExecuteReader();
                    }

                    var idEnrollment = (int) dr["IdEnrollment"];

                    com.CommandText = "INSERT INTO Student(IndexNumber, FirstName, LastName, Birthdate, IdEnrollment) "
                                      + "VALUES(@Index, @Fname, @Lname,@BDate,@Studies)";
                    com.Parameters.AddWithValue("index", request.IndexNumber);
                    com.Parameters.AddWithValue("fname", request.FirstName);
                    com.Parameters.AddWithValue("lname", request.LastName);
                    com.Parameters.AddWithValue("bdate", request.Birthdate);
                    com.Parameters.AddWithValue("studies", idEnrollment);
                    com.ExecuteNonQuery();

                    tran.Commit();

                }
                catch (SqlException exc)
                {
                    tran.Rollback();
                }
            }
        }

        public void PromoteStudents(PromoteStudentRequest request)
        {
            throw new NotImplementedException();
        }
        
        public bool CheckIndexNumber(string index)
        {
            using (var con = new SqlConnection("Server=localhost,32770;Initial Catalog=s18706;User ID=sa;Password=Root1234"))
            using (var com = new SqlCommand())
            {
                com.Connection = con;

                con.Open();
                
                com.CommandText = "select * from student where IndexNumber=@index";
                com.Parameters.AddWithValue("index", index);
                
                var dr = com.ExecuteReader();
                if (!dr.Read())
                {
                    return false;
                }
            }
            return true;
        }
        
        public bool CheckUserPassword(LoginRequestDto index)
        {
            using (var con = new SqlConnection("Server=localhost,32770;Initial Catalog=s18706;User ID=sa;Password=Root1234"))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                con.Open();
                
                com.CommandText = "select * from student where IndexNumber=@index AND Password=@password";
                com.Parameters.AddWithValue("index", index.Login);
                com.Parameters.AddWithValue("password", index.Haslo);
                
                var dr = com.ExecuteReader();
                if (!dr.Read())
                {
                    return false;
                }
            }
            return true;
        }
    }
}
