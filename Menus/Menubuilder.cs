namespace Databaser_Slutprojekt.Menus;

/// <summary>
/// A class with the blueprint for all kind of menues or when the user gets to choose between two options. 
/// </summary>
public class Menubuilder
{
    //_menuItems holds the options presented to the user. 
    private List<string> _menuItems;
    //_topStatement is the heading/message before the menu or option. 
    private string _topStatement; 
    
    public Menubuilder(List<string> menuItems, string topStatement)
    {
        _menuItems = menuItems;
        _topStatement = topStatement;
    }

    /// <summary>
    /// A method for building and running a menu. 
    /// </summary>
    /// <returns>En int som motsvara position av vald meny-item</returns>
    public string Run()
    {
        //Starting position of the pointer/arrow.
        int currentSelection = 0; 

        while (true)
        {
            Console.Clear();

            Console.WriteLine($"{_topStatement}");
            
            for (int i = 0; i < _topStatement.Length; i++)
            {
                Console.Write("-");
            }
            Console.WriteLine();

            for (int i = 0; i < _menuItems.Count; i++)
            {
                if (i == currentSelection)
                {
                    //the option chosen is marked with color and ->
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("-> " + _menuItems[i]);
                    //Reset the color to white-ish for the other options
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine("   " + _menuItems[i]);
                }
            }

            // key read the user input
            var key = Console.ReadKey(true);

            if (key.Key == ConsoleKey.UpArrow)
            {
                // moves the marker up. 
                currentSelection = (currentSelection == 0) ? _menuItems.Count - 1 : currentSelection - 1;
            }
            else if (key.Key == ConsoleKey.DownArrow)
            {
                // moves the marker down
                currentSelection = (currentSelection == _menuItems.Count - 1) ? 0 : currentSelection + 1;
            }
            else if (key.Key == ConsoleKey.Enter)
            {
                return _menuItems[currentSelection];
            }
        }
    }
}