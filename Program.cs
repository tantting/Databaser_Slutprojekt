using Databaser_Slutprojekt.Menus;
using Databaser_Slutprojekt.Other;

namespace Databaser_Slutprojekt;

class Program
{
    static void Main(string[] args)
    {
        //ConnectionString: Server=localhost,1433;Database=MonsterHighDB; User = SA; Password = MyStrongPass123; Trust Server Certificate = true
        var monsterHigh = new MonsterHigh(); 
        monsterHigh.Run();
    }
}