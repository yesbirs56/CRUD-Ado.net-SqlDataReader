using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace ConnectSQLToConsole
{
    internal class ConvertCurrency
    {
        private readonly static string _conString = ConfigurationManager.ConnectionStrings["PLAYDB"].ConnectionString;

        private static void Main(string[] args)
        {
            Console.WriteLine("Enter 0 To Delete Table :");
            Console.WriteLine("Enter 1 To Create Table :");
            Console.WriteLine("Enter 2 To Delete a row in Table :");
            Console.WriteLine("Enter 3 To Insert Into Table :");
            Console.WriteLine("Enter 4 To Read Data From Table :");


            String ch = "y";
            
            while (ch != "n")
            {
                Console.WriteLine("--------------------------------------------------------");

                Console.Write("Enter Option: ");
                int c = Convert.ToInt32(Console.ReadLine());
                int n = 0;

                switch (c)
                {
                    case 0:
                        Console.WriteLine("Delete Student Table");

                        DeleteTable();
                        break;

                    case 1:
                        Console.WriteLine("Create Student Table");

                        CreateTable();
                        break;

                    case 2:
                        Console.WriteLine("Delete Row Table");
                        Console.Write("Enter student Id :");
                        int index = Convert.ToInt32(Console.ReadLine());
                        n = DeleteStudent(index);
                        Console.WriteLine($"{n} student deleted");
                        break;

                    case 3:
                        Console.WriteLine("Insert Into Table");
                        Student student = TakeStudentInfo();
                        n = AddStudent(student);
                        Console.WriteLine($"{n} student added");

                        break;

                    case 4:
                        Console.WriteLine("Get the data of All Students");
                        List<Student> students = GetAllStudents();
                        ShowStudents(students);
                        break;

                    default:
                        Console.WriteLine("Wrong Input");
                        break;
                }
                
                Console.Write("\nMore Query? (y/n): ");
                ch = Console.ReadLine().ToLower();
                Console.WriteLine("--------------------------------------------------------");
            }
        }

        public static void ShowStudents(List<Student> students)

        {
            Console.WriteLine($"|------------------------------------------|");
            Console.WriteLine($"|{"ID",5}  |  {"NAME",-20}  |  {"AGE",5}  |");
            Console.WriteLine($"|------------------------------------------|");
            foreach (var student in students)
            {
                
                Console.WriteLine($"|{student.Id,5}  |  {student.Name,-20}  |   {student.Age,5} |");
            }
            Console.WriteLine($"|------------------------------------------|");
        }

        public static Student TakeStudentInfo()
        {
            Console.Write("Enter student Name :");
            string name = Console.ReadLine();
            Console.Write("Enter student Age :");
            int age = Convert.ToInt32(Console.ReadLine());
            return new(name, age);
        }

        public static List<Student> GetAllStudents()
        {
            List<Student> students = new List<Student>();
            using (SqlConnection con = new SqlConnection(_conString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {

                    cmd.CommandText = $@"Select * From Student";
                    cmd.Connection = con;
                    con.Open();

                    try
                    {
                        using(SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int id = reader.GetInt32(0);
                                string name = reader.GetString(1);
                                int age = reader.GetInt32(2);
                                students.Add(new Student(name, age)
                                {
                                    Id = id
                                });

                            }
                        }
                    }
                    catch (SqlException exc)
                    {
                        Console.WriteLine($"{exc.Message}");
                    }
                    finally
                    {
                        con.Close();
                    }
                }
            }
            return students;
        }

        public static void DeleteTable()
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = $@"DROP TABLE STUDENT";
                    cmd.Connection = con;
                    con.Open();

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException exc)
                    {
                        Console.WriteLine($"{exc.Message}");
                    }
                    finally
                    {
                        con.Close();
                    }
                }
            }
        }

        public static int DeleteStudent(int id)
        {
            int n = 0;
            using (SqlConnection con = new SqlConnection(_conString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = $@"DELETE FROM Student WHERE ID = {id} ";
                    cmd.Connection = con;
                    con.Open();

                    try
                    {
                        n = cmd.ExecuteNonQuery();
                    }
                    catch (SqlException exc)
                    {
                        Console.WriteLine($"{exc.Message}");
                    }
                    finally
                    {
                        con.Close();
                    }
                }
            }
            return n;

        }

        private static int AddStudent(Student student)
        {
            
            int n = 0;

            using (SqlConnection con = new SqlConnection(_conString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = $@"Insert Into Student(Name, Age) Values ( '{student.Name}', {student.Age} )";
                    cmd.Connection = con;
                    con.Open();

                    try
                    {
                        n = cmd.ExecuteNonQuery();
                    }
                    catch (SqlException exc)
                    {
                        Console.WriteLine($"{exc.Message}");
                    }
                    finally
                    {
                        con.Close();
                    }
                }
            }

            return n;
        }

        private static void CreateTable()
        {
            
            using (SqlConnection con = new SqlConnection(_conString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = $@"create Table Student" +
                        $@"(" +
                        $@"Id int not null primary key identity(1,1) ," +
                        $@"Name nvarchar(20) not null ," +
                        $@"Age int not null ," +
                        $@")";
                    cmd.Connection = con;
                    con.Open();

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException exc)
                    {
                        Console.WriteLine($"{exc.Message}");
                    }
                    finally
                    {
                        con.Close();
                    }
                }
            }
        }
    }
}