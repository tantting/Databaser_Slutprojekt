using System.Globalization;
using Databaser_Slutprojekt.Menus;
using Microsoft.Data.SqlClient;

namespace Databaser_Slutprojekt.Services;

public class ADOServices
{
    
    
    private readonly string _connectionString =
        @"Server=localhost,1433;Database=MonsterHighDB; User = SA; Password = MyStrongPass123; Trust Server Certificate = true";
    
   
    //const string format = "{0,-15} {1,-15} {2,-15} {3,-15} {4, -20}";
    private void ExcecuteQueries(string query, int padding, List<string> headings)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    for (int i = 0; i < headings.Count; i++)
                    {
                        Console.Write(headings[i].PadRight(padding));
                    }
                    Console.WriteLine();
                    Console.WriteLine("---------------------------------------------------------------------------");
                    
                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            Console.Write(reader[i].ToString().PadRight(padding));
                        }
                        Console.WriteLine();
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
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
            Console.WriteLine("\nPress enter to continue");
            while (Console.ReadKey(true).Key != ConsoleKey.Enter) {}
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
        
        var headings = new List<string>() { "Last name", "First name", "Position", "Years at School" };
        
        ExcecuteQueries(query, 20, headings);

        return true;
    }
    
    //Skapa en Stored Procedure som tar emot ett Id och returnerar viktig information om den elev som är registrerad med aktuellt Id. (SQL via ADO.Net)
    //1. Visa information om en elev, vilken klass hen tillhör och vilken/vilka lärare hen har samt vilka betyg hen har fått i en specifik kurs. (SQL via ADO.Net)
    public bool ShowSpecificStudent()
    {
        Console.Clear();
        Console.WriteLine("Search for student / stored procedure");

        Console.WriteLine("Wish to Update info? Admin only");

        return true;
    } 
   
    public bool ShowGradesPerStudent()
    {
        //Vi vill kunna ta fram alla betyg för en elev i varje kurs/ämne de läst och vi vill kunna se vilken
        //lärare som satt betygen, vi vill också se vilka datum betygen satts. (SQL via ADO.Net)
        Console.Clear();
        Console.WriteLine("Grades per student");

        return true;
    }
    
    
    public bool ShowSalaryTotalYear()
    {
        Console.Clear();
        Console.WriteLine("Total salary cost ");

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

        Console.WriteLine("Please enter the following data:");

        Console.Write("First name: ");
        string firstName = Console.ReadLine();

        Console.Write("Last name: ");
        string lastName = Console.ReadLine();
        

        int staffRoleId = FetchRoleID(firstName, lastName);

        DateTime startDate = GetStartDate(firstName, lastName);

        decimal monthlySalary = GetMonthlySalary(firstName, lastName); 
        
        //Console.Clear(); 
        //Console.WriteLine($"{firstName} {lastName} with ID {iD} will start on {startDate.ToString()} with the salary {monthlySalary.ToString()} SEK");
        string query = @"INSERT INTO Staff (FirstName, LastName, StaffRoleID, DateHired, SalaryMonthly) VALUES (@FirstName, @LastName, @StaffRoleID, @DateHired, @Salary)";
        
        ExceCuteAlterTableQueries(query, firstName, lastName, staffRoleId, startDate, monthlySalary);
        
        return true;
    }

    public List<string[]> GetMenuData()
    {
        var staffRoles = new List<string[]> ();
        
        string query = @"SELECT * FROM StaffRoles";
        
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
                        var staffRoleItem = new string[2]; 
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            staffRoleItem[i] = reader[i].ToString(); 
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

    public int FetchID(string firstName, string lastName)
    {
        //Get a List of string-arrays containing staff-roles and their ID
        var staffRoles = GetMenuData();

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

    public decimal GetStudentID()
    {
        
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