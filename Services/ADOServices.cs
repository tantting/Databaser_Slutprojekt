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
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                Console.Write(reader.GetName(i).PadRight(padding));
                            }

                            Console.WriteLine();
                            Console.WriteLine(
                                "---------------------------------------------------------------------------");

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

    public void ExceCuteAlterTableQueries(string query, string firstName, string lastName, int staffRoleId, 
        DateTime dateStartPosition, decimal salary)
    {
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
                    command.Parameters.AddWithValue("@DateHired", dateStartPosition);
                    command.Parameters.AddWithValue("@Salary", salary);
                
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
    }


    //Skolan vill kunna ta fram en översikt över all personal där det framgår namn och vilka befattningar de har samt
    //hur många år de har arbetat på skolan. Administratören vill också ha möjlighet att spara ner ny personal. (SQL via ADO.Net)
    public bool ShowStaff()
    {
        Console.Clear();
        
        // int padding = 20; 
        
        Console.WriteLine("All Staff");
        Console.WriteLine("---------------------------------------------------------------------------");
        
        string query = $"SELECT \ns.LastName AS 'Last name',\ns.FirstName AS 'First name',\nr.StaffRoleName AS " +
                       "'Position/Role',\nDATEDIFF(YEAR, DateHired, GETDATE()) AS 'YearsEmployed'\nFROM Staff s\nJOIN " +
                       "StaffRoles r ON s.StaffRoleID = r.ID\nORDER BY 'Last name'";
        
        //var headings = new List<string>() { "Last name", "First name", "Position", "Years at School" };
        
        ExecuteShowQueries(query, 20, null);

        return true;
    }
    
    //Skapa en Stored Procedure som tar emot ett Id och returnerar viktig information om den elev som är registrerad med aktuellt Id. (SQL via ADO.Net)
    //1. Visa information om en elev, vilken klass hen tillhör och vilken/vilka lärare hen har samt vilka betyg hen har fått i en specifik kurs. (SQL via ADO.Net)
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
            //List<string> headings = new List<string>() { "Name", "Personal number", "Grade", "Mentor" };
            
            ExecuteShowQueries(query, 20, studentID);
        }
        return true;
    } 
   
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
            string query = @"SELECT c.CourseName as 'Course',g2.GradingScale as 'Grade',g1.DateSetGrade as 'Date when set',"+
                           "s1.FirstName + ' ' + s1.LastName as 'Teacher'FROM Grades g1 JOIN Courses c ON g1.CourseID = c.ID "+
                           "JOIN CourseTeacher ct on ct.CourseID = c.ID JOIN Staff s1 ON ct.TeacherID = s1.ID "+
                           "JOIN GradingScales g2 ON g1.GradingScale_ID = g2.ID JOIN Students s2 ON s2.ID = g1.StudentID "+
                           "WHERE s2.ID = @parameter";

            //var headings = new List<string>() { "Course", "Grade", "Date when set", "Teacher" };
            
            ExecuteShowQueries(query, 20, studentID);
        }
        return true;
    }
    
    
    public bool ShowSalaryTotalYear()
    {
        Console.Clear();
        Console.WriteLine("TOTAL SALARY COST PER DEPARTMENT\n");

        string query = @"SELECT d.DepartmentName AS 'Department name', SUM(s.SalaryMonthly) AS 'Total monthly salary cost' "+
                       "FROM Departments d JOIN DepartmentsStaff ds ON d.ID = ds.DepartmentID "+
                       "JOIN Staff s ON s.ID = ds.StaffID GROUP BY d.DepartmentName";
        
        
        return true;
    }
    
    public bool ShowAvgSalaryTotal()
    {
        Console.Clear();
        Console.WriteLine("Avg salar of all staff ");

        return true;
    }
    
    public bool ShowAvgSalaryPerDepartment()
    {
        Console.Clear();
        Console.WriteLine("Avg salar per department");

        return true;
    }
    
    //Sätt betyg på en elev genom att använda Transactions ifall något går fel. (SQL via ADO.Net)
    public bool SetCourseGrades()
    {
        Console.Clear();
        Console.WriteLine("set student grades");

        return true;
    }
    
    //2. Skapa en View som visar alla lärare och vilka utbildningar de ansvarar för. (SQL via ADO.Net)
    public bool ShowAllTeachersAndTheirCourses()
    {
        Console.Clear();
        Console.WriteLine("All teachers and their courses");

        return true;
    }
    
    //Administratören vill också ha möjlighet att spara ner ny personal. (SQL via ADO.Net)
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

        int staffRoleId = FetchStaffRoleID(firstName, lastName);

        DateTime startDate = GetStartDate(firstName, lastName);

        decimal monthlySalary = GetMonthlySalary(firstName, lastName); 
        
        //Console.Clear(); 
        //Console.WriteLine($"{firstName} {lastName} with ID {iD} will start on {startDate.ToString()} with the salary {monthlySalary.ToString()} SEK");
        string query = @"INSERT INTO Staff (FirstName, LastName, StaffRoleID, DateHired, SalaryMonthly) VALUES (@FirstName, @LastName, @StaffRoleID, @DateHired, @Salary)";
        
        ExceCuteAlterTableQueries(query, firstName, lastName, staffRoleId, startDate, monthlySalary);
        
        return true;
    }

    public List<List<string>> GetMenuData(string query)
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

    public int FetchStaffRoleID(string firstName, string lastName)
    {
        string query = @"SELECT * FROM StaffRoles";
        
        //Get a List of string-arrays containing staff-roles and their ID
        var staffRoles = GetMenuData(query);
        
        List<string> menuItems = new List<string>(); 
        
        //Pick the role-names from the list of arrays to use for a menu.
        foreach (var role in staffRoles)
        {
            menuItems.Add(role[1]);
        }
        var menu = new Menubuilder(menuItems, $"What position will {firstName} {lastName} have?");
        
        string roleChosen = menu.Run();
        
        //A varialbe for storing the ID of the chosen role. 
        int iD = 0;
        
        foreach (var role in staffRoles)
        {
            if (roleChosen == role[1])
            {
                int.TryParse(role[0], out iD); 
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

    /// <summary>
    /// NOT in use currently
    /// </summary>
    /// <param name="originalStr"></param>
    /// <returns></returns>
    public string SplitString(string originalStr)
    {
        // Split the string in year, month and day
        string year = originalStr.Substring(0, 4);
        string month = originalStr.Substring(4, 2);
        string day = originalStr.Substring(6, 2);
        
        // Put it all togeher again
        string formatedStr = $"{year}-{month}-{day}";

        return formatedStr;
    }
}