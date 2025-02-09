using System.Diagnostics;
using System.Globalization;
using System.Linq.Expressions;
using Databaser_Slutprojekt.Menus;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Databaser_Slutprojekt.Services;

public class ADOServices
{
    
    
    private readonly string _connectionString =
        @"Server=localhost,1433;Database=MonsterHighDB; User = SA; Password = MyStrongPass123; Trust Server Certificate = true";
    
    
    private void ExecuteShowQueries(string query, int padding, int? parameter)
    {
        Console.Clear();
        
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                //if there is a parameter to handle in the method call
                if (parameter.HasValue)
                {
                    // Add the parameter with entered value
                    command.Parameters.AddWithValue("@parameter", parameter);
                }

                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            // lines counts all words and spacing when the heading is printed, in order to print a line
                            // under with a good nice looking length
                            int lines = 0;
                            
                            // a for-loop for the column headings/names
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                Console.Write(reader.GetName(i).PadRight(padding));
                                lines += reader.GetName(i).PadRight(padding).Length;
                            }
                            Console.WriteLine();
                            
                            // adding a line under the column headings
                            for (int i = 0; i < lines; i++)
                            {
                                Console.Write("-");
                            }

                            Console.WriteLine();
                            
                            // a while loop running as long as there are rows left to read in the reader
                            while (reader.Read())
                            {
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    Console.Write(reader[i].ToString().PadRight(padding));
                                }
                                Console.WriteLine();
                            }
                        }
                        else
                        {
                            Console.WriteLine("There are no data matching he request");
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }
    }
    
    /// <summary>
    /// Method for printing out an overview of all staff to the console.
    /// </summary>
    /// <returns></returns>
    public bool ShowStaff()
    {
        Console.Clear();
        
        // int padding = 20; 
        
        Console.WriteLine("All Staff");
        Console.WriteLine("---------------------------------------------------------------------------");
        
        string query = $"SELECT "+
                       "s.LastName AS 'Last name', "+
                       "s.FirstName AS 'First name', "+
                       "r.StaffRoleName AS 'Position/Role', "+
                       "DATEDIFF(YEAR, DateHired, "+
                       "GETDATE()) AS 'YearsEmployed' "+
                       "FROM Staff s "+
                       "JOIN StaffRoles r ON s.StaffRoleID = r.ID "+
                       "ORDER BY 'Last name'";
        
        ExecuteShowQueries(query, 20, null);

        return true;
    }
    
    /// <summary>
    /// A method that uses a stored procedure to print out data of a specifc student to the console.
    /// </summary>
    /// <returns>true</returns>
    public bool ShowSpecificStudent()
    {
        Console.WriteLine("Search for student!");
        Console.WriteLine("-------------------");

        int studentID = 0;
        
        studentID = FetchStudentID();

        if (studentID != -1)
        {
            Console.Clear();
            string query = @"EXEC ShowStudent @parameter";
            
            ExecuteShowQueries(query, 20, studentID);
        }
        return true;
    } 
   
    /// <summary>
    /// A method that shows all the grades of a student
    /// </summary>
    /// <returns>true</returns>
    public bool ShowGradesPerStudent()
    {
        Console.Clear();
        Console.WriteLine("Grades per student:");
        Console.WriteLine("--------------------");
        
        int studentID = 0;
        
        studentID = FetchStudentID();

        Console.WriteLine(studentID);

        if (studentID != -1)
        {
            string query = @"SELECT "+
                           "c.CourseName as 'Course',"+
                           "g2.GradingScale as 'Grade',"+
                           "CONVERT(varchar, g1.DateSetGrade, 23) as 'Date when set',"+
                           "g1.FinalGrades as 'Final?', "+
                           "s1.FirstName + ' ' + s1.LastName as 'Teacher'"+
                           "FROM Grades g1 "+
                           "JOIN Courses c ON g1.CourseID = c.ID "+
                           "JOIN CourseTeacher ct on ct.CourseID = c.ID "+
                           "JOIN Staff s1 ON ct.TeacherID = s1.ID "+
                           "JOIN GradingScales g2 ON g1.GradingScale_ID = g2.ID "+
                           "JOIN Students s2 ON s2.ID = g1.StudentID "+
                           "WHERE s2.ID = @parameter";
            
            ExecuteShowQueries(query, 20, studentID);
        }
        return true;
    }
    
    public bool ShowMonthlySalaryCostDepartment()
    {
        Console.Clear();
        Console.WriteLine("MONTHLY SALARY COST PER DEPARTMENT\n");

        string query = @"SELECT d.DepartmentName AS 'Department name', 
                        SUM(s.SalaryMonthly) AS 'Total monthly salary cost' "+
                       "FROM Departments d "+
                       "JOIN DepartmentsStaff ds ON d.ID = ds.DepartmentID "+
                       "JOIN Staff s ON s.ID = ds.StaffID GROUP BY d.DepartmentName";
        
        ExecuteShowQueries(query, 40, null);
        
        return true;
    }
    
    public bool ShowAvgSalaryPerDepartment()
    {
        Console.Clear();
        Console.WriteLine("AVERAGE SALARY PER DEPARTMENT\n");
        
        string query = @"SELECT d.DepartmentName AS 'Department name', 
                        AVG(s.SalaryMonthly) AS 'Average monthly salary' "+
                       "FROM Departments d "+
                       "JOIN DepartmentsStaff ds ON d.ID = ds.DepartmentID "+
                       "JOIN Staff s ON s.ID = ds.StaffID GROUP BY d.DepartmentName";
        
        ExecuteShowQueries(query, 40, null);

        return true;
    }
    
    /// <summary>
    /// A method for setting grades. Uses a stored procedure, including transaction to make sure everything is rolled
    /// back if not followed through
    /// </summary>
    /// <returns></returns>
    public bool SetCourseGrades()
    {
        Console.Clear();
        Console.WriteLine("SET COURSE GRADES\n");
    
        //Start by fetching the ID of the teacher setting the grades
        string queryTeacherID = @"SELECT ID AS 'ID', FirstName + ' ' + LastName AS Name FROM Staff";
        string headingTeacherID = "Which teacher is setting the grade?\n";
        int gradeSetTeacherID = FetchColumnID(queryTeacherID, headingTeacherID);
    
        //Fetch student ID
        int studentID = FetchStudentID();
    
        //Fetch course ID
        string queryCourseID = @"SELECT ID AS 'ID',CourseName AS Course FROM Courses";
        string headingCourseID = "Choose relevant course: \n";
        int courseID = FetchColumnID(queryCourseID, headingCourseID);
    
        //Fetch gradingScaleID
        string queryGradeScaleID =  @"SELECT ID AS 'ID',GradingScale AS Grade FROM GradingScales";
        string headingGradeScaleID = "Choose relevant grade: \n";
        int gradingScaleID = FetchColumnID(queryGradeScaleID, headingGradeScaleID);
    
        var menuItems = new List<string>() {"Yes", "No"};
        var menu = new Menubuilder(menuItems, "Is this a fina/n Answer Yes is final, No if intermediate");
        
        string choice = menu.Run();
    
        //finalGRades = treu is equal to 1 on bit
        bool finalGrades = true;
    
        if (choice == "Yes")
        {
            finalGrades = true; 
        }
        else
        {
            finalGrades = false; 
        }

        Console.WriteLine($"Student = {studentID} Teacher {gradeSetTeacherID} Course= {courseID} grade= {gradingScaleID} final= {finalGrades}");
        Console.ReadLine();
    
        string mainQuery = $@"EXEC SetGrades @StudentID, @GradeSetTeacherID, "+
                           $"@CourseID, @GradingScaleID, @FinalGrades";
        try
        {
            Console.Clear();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
    
                using (SqlCommand command = new SqlCommand(mainQuery, connection))
                {
                    command.Parameters.AddWithValue("@StudentID", studentID);
                    command.Parameters.AddWithValue("@GradeSetTeacherID", gradeSetTeacherID);
                    command.Parameters.AddWithValue("@CourseID", courseID);
                    command.Parameters.AddWithValue("@GradingScaleID", gradingScaleID);
                    command.Parameters.AddWithValue("@FinalGrades", finalGrades); 

                    var rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine($"{rowsAffected} grade is now changed");
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        finally
        {
            Console.WriteLine("Operation completed.");
        }
        return true;
    }
    
    /// <summary>
    /// A method for adding new staff. Used by the admin
    /// </summary>
    /// <returns></returns>
    public bool AddNewStaff()
    {
        Console.Clear();
        Console.WriteLine("Add new staff");
        Console.WriteLine("--------------------------------");

        Console.WriteLine("\nPlease enter the following data:\n");

        Console.Write("First name: ");
        string firstName = Console.ReadLine();

        Console.Write("Last name: ");
        string lastName = Console.ReadLine();

        string getIDquery = @"SELECT * FROM StaffRoles";
        string topStatementMenu = $"What position will {firstName} {lastName} have?";
            
        int staffRoleId = FetchColumnID(getIDquery, topStatementMenu);

        DateTime startDate = GetStartDate(firstName, lastName);

        decimal monthlySalary = GetMonthlySalary(firstName, lastName); 
        
        string query = @"INSERT INTO Staff (FirstName, LastName, StaffRoleID, DateHired, SalaryMonthly) "+
                       "VALUES "+
                       "(@FirstName, @LastName, @StaffRoleID, @DateHired, @Salary)";
        
        try
        {
            Console.Clear();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", firstName);
                    command.Parameters.AddWithValue("@LastName", lastName); 
                    command.Parameters.AddWithValue("@StaffRoleID", staffRoleId);
                    command.Parameters.AddWithValue("@DateHired", startDate);
                    command.Parameters.AddWithValue("@Salary", monthlySalary);
                
                    var rowsAffected = command.ExecuteNonQuery();
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        finally
        {
            Console.WriteLine("Operation completed.");
        }
        return true;
    }

    public List<List<string>> GetMenuDataFromSQL(string query)
    {
        var staffRoles = new List<List<string>> ();
        
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            SqlCommand command = new SqlCommand(query, connection);
            
            try
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var staffRoleItem = new List<string>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                staffRoleItem.Add(reader[i].ToString());
                            }

                            staffRoles.Add(staffRoleItem);
                        }
                    }
                    else
                    {
                        Console.WriteLine("There is not data matching the request");
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        return staffRoles;
    }
    
    public List<List<string>> GetFilteredData(string query, string parameter)
    {
        var filteredDataList = new List<List<string>> ();
        
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            SqlCommand command = new SqlCommand(query, connection);
            
            try
            {
                // Add the parameter with entered value
                command.Parameters.AddWithValue("@parameter", parameter);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var staffRoleItem = new List<string>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                staffRoleItem.Add(reader[i].ToString());
                            }

                            filteredDataList.Add(staffRoleItem);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No data found for the given parameter");
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        return filteredDataList;
    }

    public int FetchColumnID(string query, string topStatement)
    {
        //Get a List of string-arrays containing ID and a column of choice
        var listIDAndInfo = GetMenuDataFromSQL(query);
        
        //Initiate a list to store menu-items to use when running interactive menu
        List<string> menuItems = new List<string>(); 
        
        //Pick the role-names from the list of arrays to use for a menu.
        foreach (var item in listIDAndInfo)
        {
            menuItems.Add(item[1]);
        }
        var menu = new Menubuilder(menuItems, topStatement);
        
        string choice = menu.Run();
        
        //A varialbe for storing the ID of the chosen item
        int iD = 0;
        
        // matches the chosen item to list containing also IDs
        foreach (var item in listIDAndInfo)
        {
            if (choice == item[1])
            {
                int.TryParse(item[0], out iD); 
            }
        }

        return iD; 
    }
    
    public int FetchStudentID()
    {
        bool runLoop = true;
        int studentId = -1; 

        while (runLoop)
        {
            string topStatementSearch = $"By what would you like to search for the students?";
            var menuItemsSearch = new List<string>() { "First name", "Last name", "Exit" };
            var menuSearch = new Menubuilder(menuItemsSearch, topStatementSearch);

            string query = "";
            string searchTerm = menuSearch.Run();
            string parameter = "";

            if (searchTerm != "Exit")
            {
                Console.Clear();
                Console.WriteLine($"Enter a {searchTerm}: ");
                parameter = Console.ReadLine();
            }

            bool exit = false; 

            switch (searchTerm)
            {
                case "First name":
                    query = @"SELECT ID, LastName, FirstNAme, PersonalNumber FROM Students WHERE FirstName = @parameter";
                    break;
                case "Last name":
                    query = @"SELECT ID, LastName, FirstNAme, PersonalNumber FROM Students WHERE LastName = @parameter";
                    break;
                case "Exit":
                    exit = true;
                    runLoop = false;
                    break;
            }

            if (exit != true)
            {
                //Get a List of string-arrays containing staff-roles and their ID
                var filteredStudents = GetFilteredData(query, parameter);

                List<string> menuItems = new List<string>();

                //Pick the role-names from the list of arrays to use for a menu.
                for (int i = 0; i < filteredStudents.Count; i++)
                {
                    string studentItem = "";
                    for (int j = 0; j < filteredStudents[i].Count; j++)
                    {
                        studentItem = studentItem + "  " + filteredStudents[i][j];
                    }

                    menuItems.Add(studentItem);
                }

                if (menuItems.Count != 0)
                {
                    var menu = new Menubuilder(menuItems, $"Which student are you searching for?");

                    string selectedStudent = menu.Run();

                    if (selectedStudent != "Exit")
                    {
                        for (int i = 0; i < menuItems.Count; i++)
                        {
                            if (selectedStudent == menuItems[i])
                            {
                                bool parseString = Int32.TryParse(filteredStudents[i][0], out studentId);
                                runLoop = false;
                            }
                        }
                    }
                    else
                    {
                        runLoop = false;
                    }
                }
                else
                {
                    Console.WriteLine("Therer are no students matching the search term");
                }
            }
        }
        return studentId; 
    }

    public DateTime GetStartDate(string firstName, string lastName)
    {
        DateTime dateResult = DateTime.Now;
        
        bool runLoop = true;

        while (runLoop)
        {
            bool correctDateString = false;

            while (!correctDateString || dateResult < DateTime.Now)
            {
                Console.Clear();
                Console.Write($"When will {firstName} {lastName} start the position (in format YYYYMMDD): ");
                string dateString = Console.ReadLine();

                // Define the expected date format
                string format = "yyyyMMdd";

                // Try to parse the input string to a DateTime object. DateTime.MinValue the smallest possible value
                // for a DateTime object
                correctDateString = DateTime.TryParseExact(dateString, format, CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out dateResult) && dateResult != DateTime.MinValue;

                if (dateResult < DateTime.Now || !correctDateString)
                {
                    Console.WriteLine("\nMake sure you enter a correct date that is in the future");
                    Console.WriteLine("\nPress enter to continue");
                    while (Console.ReadKey(true).Key != ConsoleKey.Enter) {}
                }
            }

            string dateResultForPrint = $"{dateResult.Year}-{dateResult.Month}-{dateResult.Day}";
            string topStatementDate = $"{firstName} {lastName} starts his/her position at {dateResultForPrint}" +
                                      $". Is that correct?";
            var menuItemsDate = new List<string>() { "Yes", "No" }; 
            var menuDate = new Menubuilder(menuItemsDate, topStatementDate);

            if (menuDate.Run() == "Yes")
            {
                runLoop = false;
            }
        }
        return dateResult; 
    }

    public decimal GetMonthlySalary(string firstName, string lastName)
    {
        string answerSalary = "";
        decimal monthlySalary = 0;
        bool runLoop = true; 

        while (!Decimal.TryParse(answerSalary, out monthlySalary) || runLoop)
        {
            Console.Clear();
            Console.Write($"What is the contracted monthly salary of {firstName} {lastName}: ");
            answerSalary = Console.ReadLine();
            
            string topStatementSalary = $"Entered monthly salary {answerSalary} SEK. Is that correct?";
            var menuItemsSalary = new List<string>() { "Yes", "No" }; 
            var menuSalary = new Menubuilder(menuItemsSalary, topStatementSalary);
            
            if(menuSalary.Run() == "Yes")
            {
                runLoop = false;
            }
        }
        return monthlySalary;
    }
}