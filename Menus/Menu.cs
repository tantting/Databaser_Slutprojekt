using System.Diagnostics;
using Databaser_Slutprojekt.Services;

namespace Databaser_Slutprojekt.Menus;

public class Menu
{
    private delegate bool _menuAction();

    private EFServices _efServices = new EFServices();
    private ADOServices _adoServices = new ADOServices();
    
    public void Run()
    {
        var menuItems= new Dictionary<string, _menuAction>
        {
            { "Departments and staff", StaffInfoMenu },
            { "Classes and Students", ClassAndStudentMenu },
            { "Courses", CourseMenu },
            { "Grades", GradesMenu },
            { "Economy", EconomyMenu },
            { "End program", EndMenuLoop }
        };
        
        bool runMenu = true;

        while (runMenu)
        {
            var menuKeys = new List<string>(menuItems.Keys);
            var menu = new Menubuilder(menuKeys, "What would you like to access?");
            
            //The EndProgram-method returns false, all other true. d
            runMenu = menuItems[menu.Run()](); 
        }
        Console.Clear();
        Console.WriteLine("You have chosen to exit the program!\n" +
                          "\n" +
                          "Have a nice day!\n");
        
        Console.WriteLine("\nPress enter to continue");
        while (Console.ReadKey(true).Key != ConsoleKey.Enter){}
    }
    
    public bool StaffInfoMenu()
         {
             var menuItems= new Dictionary<string, _menuAction>
             {
                 { "All departments", _efServices.ShowDepartments },
                 { "All Staff", _adoServices.ShowStaff },
                 { "Add Staff (Admin)", _adoServices.AddNewStaff },
                 { "Return to start menu", EndMenuLoop },
             };
             
             bool runMenu = true;
     
             while (runMenu)
             {
                 var menuKeys = new List<string>(menuItems.Keys);
                 var menu = new Menubuilder(menuKeys, "What would you like to see?");
                 
                 //The EndProgram-method returns false, all other true. d
                 runMenu = menuItems[menu.Run()]();

                 if (runMenu == true)
                 {
                     Console.WriteLine("\nPress enter to continue");
                     while (Console.ReadKey(true).Key != ConsoleKey.Enter) {}
                 }
             }

             return true;
         }

    public bool ClassAndStudentMenu()
    {
            var menuItems= new Dictionary<string, _menuAction>
            {
                { "Class info", _efServices.ShowClasses },
                { "All Students", _efServices.ShowAllStudents },
                { "Search for specific student", _adoServices.ShowSpecificStudent},
                { "Return to start menu", EndMenuLoop },
            };
             
            bool runMenu = true;
     
            while (runMenu)
            {
                var menuKeys = new List<string>(menuItems.Keys);
                var menu = new Menubuilder(menuKeys, "What would you like to see?");
                 
                //The EndProgram-method returns false, all other true. d
                runMenu = menuItems[menu.Run()](); 
                
                if (runMenu == true)
                {
                    Console.WriteLine("\nPress enter to continue");
                    while (Console.ReadKey(true).Key != ConsoleKey.Enter) {}
                }
            }
            return true;
    }
    
    public bool CourseMenu()
    {
        var menuItems= new Dictionary<string, _menuAction>
        {
            { "All our active course", _efServices.ShowAllActiveCourses },
            { "All teachers and their courses", _adoServices.ShowAllTeachersAndTheirCourses },
            { "Return to start menu", EndMenuLoop },
        };
             
        bool runMenu = true;
     
        while (runMenu)
        {
            var menuKeys = new List<string>(menuItems.Keys);
            var menu = new Menubuilder(menuKeys, "What would you like to see?");
                 
            //The EndProgram-method returns false, all other true. d
            runMenu = menuItems[menu.Run()](); 
            
            if (runMenu == true)
            {
                Console.WriteLine("\nPress enter to continue");
                while (Console.ReadKey(true).Key != ConsoleKey.Enter) {}
            }
        }
        return true;
    }

    public bool GradesMenu()
    {
        var menuItems= new Dictionary<string, _menuAction>
        {
            { "See grades of specific Student", _efServices.ShowAllActiveCourses },
            { "Set course grades", _adoServices.SetCourseGrades},
            { "Return to start menu", EndMenuLoop },
        };
             
        bool runMenu = true;
     
        while (runMenu)
        {
            var menuKeys = new List<string>(menuItems.Keys);
            var menu = new Menubuilder(menuKeys, "What would you like to see?");
                 
            //The EndProgram-method returns false, all other true. d
            runMenu = menuItems[menu.Run()](); 
            
            if (runMenu == true)
            {
                Console.WriteLine("\nPress enter to continue");
                while (Console.ReadKey(true).Key != ConsoleKey.Enter) {}
            }
        }
        return true;
    }
    public bool EconomyMenu()
    {
        var menuItems= new Dictionary<string, _menuAction>
        {
            { "Yearly salary expenditures", _adoServices.ShowSalaryTotalYear },
            { "Average salary for all staff", _adoServices.ShowAvgSalaryTotal },
            { "Average salary per department", _adoServices.ShowAvgSalaryPerDepartment },
            { "Return to start menu", EndMenuLoop },
        };
             
        bool runMenu = true;
     
        while (runMenu)
        {
            var menuKeys = new List<string>(menuItems.Keys);
            var menu = new Menubuilder(menuKeys, "What would you like to see?");
            
            runMenu = menuItems[menu.Run()](); 
            
            if (runMenu == true)
            {
                Console.WriteLine("\nPress enter to continue");
                while (Console.ReadKey(true).Key != ConsoleKey.Enter) {}
            }
        }
        return true;
    }

    public bool EndMenuLoop()
    {
        return false;
    }

}