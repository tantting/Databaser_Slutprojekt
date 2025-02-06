using System.Diagnostics;

namespace Databaser_Slutprojekt.Menus;

public class Menu
{
    private delegate bool _menuAction();
    
    public void Run()
    {
        var menuItems= new Dictionary<string, _menuAction>
        {
            { "Departments and staff", StaffInfoMenu },
            { "Classes and Students", ClassesStudents },
            { "Economy", Economy },
            { "Admin", RunAdmin },
            { "End program", EndProgram }
        };
        
        bool runMenu = true;

        while (runMenu)
        {
            var menuKeys = new List<string>(menuItems.Keys);
            var menu = new Menubuilder(menuKeys, "What would you like to access?");
            
            //The EndProgram-method returns false, all other true. d
            runMenu = menuItems[menu.Run()](); 

            // int choice = menu.Run();
            //
            // switch (choice)
            // {
            //     case 0:
            //         StaffInfoMenu();
            //         break;
            //     case 1:
            //         Console.WriteLine("Classes and Students");
            //
            //         break;
            //     case 2:
            //         Console.WriteLine("Economy");
            //
            //         break;
            //     case 3:
            //         Console.WriteLine("Admin");
            //         var adminMenu = new AdminMenu();
            //         adminMenu.Run();
            //         break;
            //     case 4:
            //         Console.WriteLine("End program");
            //         runMenu = false;
            //         Console.ReadKey();
            //         Console.WriteLine("Have a nice day!");
            //         break;
            // }

            Console.WriteLine("\nPress enter to continue");
            while (Console.ReadKey(true).Key != ConsoleKey.Enter){}
        }
    }
    
    public bool StaffInfoMenu()
    {
        Console.WriteLine("StaffInfoMenu");
        // bool runmenu = true;
        //
        // while (runmenu)
        // {
            // string[] menuItems = { "Departments", "Staff", "Return to start menu" };
            // var menu = new Menubuilder(menuItems, "What would you like to see?");
            //
            // int choice = menu.Run();
            //
            // Console.Clear();
            //
            // switch(choice)
            // {
            //     case 0:
            //         Console.WriteLine("See departments and klick on department to se all staff in department");
            //         break; 
            //     case 1:
            //         Console.WriteLine("See all staff, sort on first, last name or role, asc or desc");
            //         break;
            //     case 2:
            //         Console.WriteLine("See all staff, sort on first, last name or role, asc or desc");
            //         break;
            // }
            // Console.WriteLine("\nPress enter to continue");
            // while (Console.ReadKey(true).Key != ConsoleKey.Enter){}
        // }
        return true;
    }

    public bool ClassesStudents()
    {
        Console.WriteLine("StaffInfoMenu");
        // bool runmenu = true;
        //
        // while (runmenu)
        // {
            // string[] menuItems = { "Departments", "Staff", "Return to start menu" };
            // var menu = new Menubuilder(menuItems, "What would you like to see?");
            //
            // int choice = menu.Run();
            //
            // Console.Clear();
            //
            // switch(choice)
            // {
            //     case 0:
            //         Console.WriteLine("See departments and klick on department to se all staff in department");
            //         break; 
            //     case 1:
            //         Console.WriteLine("See all staff, sort on first, last name or role, asc or desc");
            //         break;
            //     case 2:
            //         Console.WriteLine("See all staff, sort on first, last name or role, asc or desc");
            //         break;
            // }
        //     Console.WriteLine("\nPress enter to continue");
        //     while (Console.ReadKey(true).Key != ConsoleKey.Enter){}
        // }
        return true;
    }

    public bool Economy()
    {
        Console.WriteLine("Economy");
        // bool runmenu = true;
        //
        // while (runmenu)
        // {
            // string[] menuItems = { "Departments", "Staff", "Return to start menu" };
            // var menu = new Menubuilder(menuItems, "What would you like to see?");
            //
            // int choice = menu.Run();
            //
            // Console.Clear();
            //
            // switch(choice)
            // {
            //     case 0:
            //         Console.WriteLine("See departments and klick on department to se all staff in department");
            //         break; 
            //     case 1:
            //         Console.WriteLine("See all staff, sort on first, last name or role, asc or desc");
            //         break;
            //     case 2:
            //         Console.WriteLine("See all staff, sort on first, last name or role, asc or desc");
            //         break;
            // }
        //     Console.WriteLine("\nPress enter to continue");
        //     while (Console.ReadKey(true).Key != ConsoleKey.Enter){}
        // }
        return true;
    }

    public bool RunAdmin()
    {
        var adminMenu = new AdminMenu(); 
        adminMenu.Run();
        return true;
    }

    public bool EndProgram()
    {
        Console.Clear();
        Console.WriteLine("You have chosen to exit the program!\n" +
                          "\n" +
                          "Have a nice day!\n");

        return false;
    }

}