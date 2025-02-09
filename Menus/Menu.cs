using System.Diagnostics;
using Databaser_Slutprojekt.Services;

namespace Databaser_Slutprojekt.Menus;

public class Menu
{
    //introduce a delegate variable that will be used to present menues to user and to connect menu choice with methods.
    private delegate bool _menuAction();

    //need objects of EFServices and ADOServices in order to get to all database-related queries. 
    private EFServices _efServices = new EFServices();
    private ADOServices _adoServices = new ADOServices();
    
    /// <summary>
    /// A method for running the first startmenu.
    /// </summary>
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
            //menuKeys holds the Keys in the menuItems, dictionary. 
            var menuKeys = new List<string>(menuItems.Keys);
            
            //topStatement is the heading of the menu. 
            var menu = new Menubuilder(menuKeys, "What would you like to access?");
            
            //The method EndMenuLoop returns runMenu = false, all other meny-methods
            //returns true. 
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
                
                 //The method EndMenuLoop returns runMenu = false, all other meny-methods
                 // returns true. 
                 runMenu = menuItems[menu.Run()]();

                 //If runMenu is false, the user can return straight back to the start meny. 
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
            { "See grades of specific Student", _adoServices.ShowGradesPerStudent },
            { "Set course grades", _adoServices.SetCourseGrades},
            { "Return to start menu", EndMenuLoop },
        };
             
        bool runMenu = true;
     
        while (runMenu)
        {
            var menuKeys = new List<string>(menuItems.Keys);
            var menu = new Menubuilder(menuKeys, "What would you like to see?");
                 
            //"Return to start" returns false, the rest returns true.
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
            { "Monthly salary cost per department", _adoServices.ShowMonthlySalaryCostDepartment },
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