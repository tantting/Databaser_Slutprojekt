using Databaser_Slutprojekt.Menus;
using Databaser_Slutprojekt.Other;

namespace Databaser_Slutprojekt;

class Program
{
    static void Main(string[] args)
    {
        // An instance of MonsterHigh is initiated in order to run the program
        var monsterHigh = new MonsterHigh(); 
        monsterHigh.Run();
    }
}