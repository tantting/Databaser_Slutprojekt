namespace Databaser_Slutprojekt.Services;

public class ADOServices
{
    //Skolan vill kunna ta fram en översikt över all personal där det framgår namn och vilka befattningar de har samt
    //hur många år de har arbetat på skolan. Administratören vill också ha möjlighet att spara ner ny personal. (SQL via ADO.Net)
    public bool ShowStaff()
    {
        Console.Clear();
        Console.WriteLine("See all Staff");

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
        Console.WriteLine("(Admin) Add new staff");

        return true;
    }
}