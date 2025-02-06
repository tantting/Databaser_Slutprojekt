namespace Databaser_Slutprojekt.Services;

public class EFServices
{
    //Lista alla avdelningar inkl namn, chef och antalet medarbetare på avdelning. Om tid, möjloggör att klicka sig vidare
    //för att se vilka medarbetare som jobbar var.
    public bool ShowDepartments()
    {
        Console.Clear();
        Console.WriteLine("See all Departments");
        
        return true;
    }
    public bool ShowClasses()
    {
        Console.Clear();
        Console.WriteLine("See all Classes");

        return true;
    }
    //Visa information om alla elever (t.ex namn, klass och annat som är intressant/relevant i din databas) (EF)
    public bool ShowAllStudents()
    {
        Console.Clear();
        Console.WriteLine("See all Students");

        return true;
    }

    //Visa en lista på alla aktiva kurser (EF)
    public bool ShowAllActiveCourses()
    {
        Console.Clear();
        Console.WriteLine("All active courses");

        return true;
    }

}

