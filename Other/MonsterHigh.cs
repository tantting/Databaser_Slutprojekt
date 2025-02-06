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
    /// A login method that I most likely will not have time to do)
    /// </summary>
    /// <returns>true om login succeeds</returns>
    public bool Login()
    {
        Console.Clear();
        Console.WriteLine("Det är nu som du loggar in men jag hann inte dit om du ser detta. Så inloggningen" +
                          "lyckades mao och du har access till ALLT! Grattis :)\n" +
                          "\n" +
                          "Tryck enter för att fortsätta");
        while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
        
        return true; 
    }
}