using Databaser_Slutprojekt.Context;
using Databaser_Slutprojekt.Entities;
using Databaser_Slutprojekt.Menus;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace Databaser_Slutprojekt.Services;

public class EFServices
{
    //Lista alla avdelningar inkl namn, chef och antalet medarbetare på avdelning. Om tid, möjloggör att klicka sig vidare
    //för att se vilka medarbetare som jobbar var.
    public bool ShowDepartments()
    {
        Console.Clear();
        Console.WriteLine("All Departments:");
        Console.WriteLine("------------------\n");
        
        var menuItems = new List<string> {"Ascending", "Descending"};
        var menu = new Menubuilder(menuItems, "How would you like to order the Departments? By...");
        string orderBy = menu.Run();
        
        using (var context = new MyDbContext())
        {
            var departments = context.Departments
                .Include(d => d.Staff)
                .Include(d => d.Manager)
                .Select(d => new
                {
                    DepartmentName = d.DepartmentName,
                    Manager = d.Manager.FirstName + " " + d.Manager.LastName,
                    NumberOfCoWorkers = d.Staff.Count
                });

            IQueryable<dynamic> sortedQuery = context.Departments; 
            
            sortedQuery = (orderBy == "Ascending") ? departments.OrderBy(d => d.DepartmentName):
                departments.OrderByDescending(d => d.DepartmentName);

            var sortedList = sortedQuery.ToList();

            if (sortedList.Count() != 0)
            {
                const string format = "{0,-40} {1,-20} {2, -10}";
                Console.Clear();
                Console.WriteLine("All Departments:" +
                                  "\n");
                Console.WriteLine(format, $"Department Name", $"Manager", $"Coworkers(#)");
                Console.WriteLine("----------------------------------------------------------------------------");
        
                foreach (var department in sortedList)
                {
                    Console.WriteLine(format, $"{department.DepartmentName}", $"{department.Manager}", 
                        $"{department.NumberOfCoWorkers}");
                }
            }
        
        }
        return true;
    }
    
    public bool ShowClasses()
    {
        Console.Clear();
        Console.WriteLine("See all Classes");
        
        using (var context = new MyDbContext())
        {
            var classes = context.Classes
                .Include(c => c.Mentor)
                .Select(c => new
                {
                    Grade = c.Grade,
                    YearBorn = c.YearBorn,
                    Mentor = c.Mentor.FirstName + " " + c.Mentor.LastName,
                    NumberOfStudents = c.Students.Count
                }).OrderByDescending(c => c.YearBorn).ToList();
        
            if (classes.Count != 0)
            {
                const string format = "{0,-15} {1,-15} {2,-20} {3, -10}";
                Console.Clear();
                Console.WriteLine("All Classes:" +
                                  "\n");
                Console.WriteLine(format, $"Grade", $"Year Born", $"Mentor", $"Students(#)");
                Console.WriteLine("----------------------------------------------------------------------------");
        
                foreach (var item in classes)
                {
                    Console.WriteLine(format, $"{item.Grade}", $"{item.YearBorn}", $"{item.Mentor}", 
                        $"{item.NumberOfStudents}");
                }
            }
        }

        return true;
    }
    //Visa information om alla elever (t.ex namn, klass och annat som är intressant/relevant i din databas) (EF)
    public bool ShowAllStudents()
    {
        Console.Clear();
        Console.WriteLine("All Students:");
        Console.WriteLine("------------------\n");

        var menuItems1 = new List<string> { "First name", "Last Name", "Class" };
        var menu1 = new Menubuilder(menuItems1, "By what would you like to order the students");
        string firstorderBy = menu1.Run();

        var menuItems2 = new List<string> { "Ascending", "Descending" };
        var menu2 = new Menubuilder(menuItems2, "Should it be Ascending och Descending?");
        string thenorderBy = menu2.Run();

        using (var context = new MyDbContext())
        {
            var students = context.Students
                .Include(s => s.Class)
                .ThenInclude(s => s.Mentor)
                .Select(s => new
                {
                    PersonalNum = s.PersonalNumber,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Grade = s.Class.Grade,
                    Mentor = s.Class.Mentor.FirstName + " " + s.Class.Mentor.LastName
                });
            
            // Jag kan göra en foreach här men inte efter min sortering...

            IQueryable<dynamic> sortedStudents = context.Students;

            switch (firstorderBy)
            {
                case "First name":
                    sortedStudents = (thenorderBy == "Ascending")
                        ? students.OrderBy(s => s.FirstName)
                        : students.OrderByDescending(s => s.FirstName);
                    break;
                case "Last Name":
                    sortedStudents = (thenorderBy == "Ascending")
                        ? students.OrderBy(s => s.LastName)
                        : students.OrderByDescending(s => s.LastName);
                    break;
                case "Class":
                    sortedStudents= (thenorderBy == "Ascending")
                        ? students.OrderBy(s => s.Grade)
                        : students.OrderByDescending(s => s.Grade);
                    break;
            }
            
            var sortedStudentList = sortedStudents.ToList();

            if (sortedStudentList.Count != 0)
            {
                const string format = "{0,-15} {1,-15} {2,-15} {3,-15} {4, -20}";
                Console.Clear();
                Console.WriteLine("All Students:" +
                                  "\n");
                Console.WriteLine(format, $"Personal num", $"Last Name", $"First Name", $"Grade", $"Mentor");
                Console.WriteLine("----------------------------------------------------------------------------");

                foreach (var item in sortedStudentList)
                {
                    Console.WriteLine(format, $"{item.PersonalNum}", $"{item.LastName}", $"{item.FirstName}",
                        $"{item.Grade}", $"{item.Mentor}");
                }
            }
        }
        return true;
    }

    //Visa en lista på alla aktiva kurser (EF)
        public bool ShowAllActiveCourses()
        {
            Console.Clear();
            Console.WriteLine("All active courses");
            Console.WriteLine("------------------\n");
        
            var menuItems = new List<string> {"Ascending", "Descending"};
            var menu = new Menubuilder(menuItems, "How would you like to order the Courses");
            string orderBy = menu.Run();

            using (var context = new MyDbContext())
            {
                var courses = context.Courses
                    .Where(c => c.ActiveCourse == true)
                    .Select(c =>  new
                    {
                        CourseName = c.CourseName,
                    });

                var sortedQuery = (orderBy == "Ascending")
                    ? courses.OrderBy(c => c.CourseName).OrderBy(c => c.CourseName)
                    : courses.OrderBy(c => c.CourseName).OrderByDescending(c => c.CourseName);

                Console.WriteLine("All Active Courses:\n" +
                                  "-----------------------------");
                foreach (var item in sortedQuery)
                {
                    Console.WriteLine($"{item.CourseName}");
                }
            }

            return true;
        }

    }