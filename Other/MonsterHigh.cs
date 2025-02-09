using Databaser_Slutprojekt.Menus;
using Microsoft.Data.SqlClient;

namespace Databaser_Slutprojekt.Other;

public class MonsterHigh
{
    public void Run()
    {
        bool loginSucceed = Login();

        if (loginSucceed)
        {
            var menu = new Menu(); 
            menu.Run();
        }
        else
        {
            Console.WriteLine("Login failed more than three times and your user account is therefore locked." +
                              "Contact system admin for unlocking it");
        }
    }
    
    /// <summary>
    /// A login method that I most likely will not have time to do.
    /// </summary>
    /// <returns>true om login succeeds</returns>
    public bool Login()
    {
        Console.Clear();
        Console.WriteLine("This is the log-in view. If you read this text, I did not have time to finalise it. Good for " +
                          "you - that means that your login attempt will succeed and you will have access to EVERYTHNG!" +
                          " Congrats! :D \n" +
                          "\n" +
                          "Press enter to continue");
        while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
        
        return true; 
    }
}