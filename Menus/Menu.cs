using System.Diagnostics;

namespace Databaser_Slutprojekt.Menus;

public class Menu
{
    
    public void Run()
    {
        bool runMenu = true;

        while (true)
        {
            string[] menuItems = { "Departments and staff", "Classes and Students", "Economy", "Admin", "End program" };

            var menu = new Menubuilder(menuItems, "What would you like to access?");

            int choice = menu.Run();

            switch (choice)
            {
                case 0:
                    StaffInfoMenu();
                    break;
                case 1:
                    Console.WriteLine("Classes and Students");

                    break;
                case 2:
                    Console.WriteLine("Economy");

                    break;
                case 3:
                    Console.WriteLine("Admin");
                    var adminMenu = new AdminMenu();
                    adminMenu.Run();
                    break;
                case 4:
                    Console.WriteLine("End program");
                    runMenu = false;
                    Console.ReadKey();
                    Console.WriteLine("Have a nice day!");
                    break;
            }

            Console.WriteLine("\nPress enter to continue");
            while (Console.ReadKey(true).Key != ConsoleKey.Enter){}
        }
    }
    
    public void StaffInfoMenu()
    {
        bool runmenu = true;

        while (runmenu)
        {
            string[] menuItems = { "Departments", "Staff", "Return to start menu" };
            var menu = new Menubuilder(menuItems, "What would you like to see?");

            int choice = menu.Run();
            
            Console.Clear();
            
            switch(choice)
            {
                case 0:
                    Console.WriteLine("See departments and klick on department to se all staff in department");
                    break; 
                case 1:
                    Console.WriteLine("See all staff, sort on first, last name or role, asc or desc");
                    break;
                case 2:
                    Console.WriteLine("See all staff, sort on first, last name or role, asc or desc");
                    break;
            }
            Console.WriteLine("\nPress enter to continue");
            while (Console.ReadKey(true).Key != ConsoleKey.Enter){}
        }
    }

    public void ClassesStudents()
    {
        bool runmenu = true;

        while (runmenu)
        {
            string[] menuItems = { "Departments", "Staff", "Return to start menu" };
            var menu = new Menubuilder(menuItems, "What would you like to see?");

            int choice = menu.Run();
            
            Console.Clear();
            
            switch(choice)
            {
                case 0:
                    Console.WriteLine("See departments and klick on department to se all staff in department");
                    break; 
                case 1:
                    Console.WriteLine("See all staff, sort on first, last name or role, asc or desc");
                    break;
                case 2:
                    Console.WriteLine("See all staff, sort on first, last name or role, asc or desc");
                    break;
            }
            Console.WriteLine("\nPress enter to continue");
            while (Console.ReadKey(true).Key != ConsoleKey.Enter){}
        }
    }

    public void Economy()
    {
        bool runmenu = true;

        while (runmenu)
        {
            string[] menuItems = { "Departments", "Staff", "Return to start menu" };
            var menu = new Menubuilder(menuItems, "What would you like to see?");

            int choice = menu.Run();
            
            Console.Clear();
            
            switch(choice)
            {
                case 0:
                    Console.WriteLine("See departments and klick on department to se all staff in department");
                    break; 
                case 1:
                    Console.WriteLine("See all staff, sort on first, last name or role, asc or desc");
                    break;
                case 2:
                    Console.WriteLine("See all staff, sort on first, last name or role, asc or desc");
                    break;
            }
            Console.WriteLine("\nPress enter to continue");
            while (Console.ReadKey(true).Key != ConsoleKey.Enter){}
        }
    }

}