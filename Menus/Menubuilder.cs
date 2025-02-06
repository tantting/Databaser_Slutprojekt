namespace Databaser_Slutprojekt.Menus;

public class Menubuilder
{
    private List<string> _menuItems;
    private string _topStatement; 

    public Menubuilder(List<string> menuItems, string topStatement)
    {
        _menuItems = menuItems;
        _topStatement = topStatement;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>En int som motsvara position av vald meny-item</returns>
    public string Run()
    {
  
        int currentSelection = 0; // Initialt valt alternativ

        while (true)
        {
            Console.Clear();

            Console.WriteLine($"{_topStatement}\n" +
                              $"-----------------------------");

            for (int i = 0; i < _menuItems.Count; i++)
            {
                if (i == currentSelection)
                {
                    // Markera det valda alternativet med en pil
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("-> " + _menuItems[i]);
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine("   " + _menuItems[i]);
                }
            }

            // Läs användarens input
            var key = Console.ReadKey(true);

            if (key.Key == ConsoleKey.UpArrow)
            {
                // Flytta upp markören
                currentSelection = (currentSelection == 0) ? _menuItems.Count - 1 : currentSelection - 1;
            }
            else if (key.Key == ConsoleKey.DownArrow)
            {
                // Flytta ner markören
                currentSelection = (currentSelection == _menuItems.Count - 1) ? 0 : currentSelection + 1;
            }
            else if (key.Key == ConsoleKey.Enter)
            {
                return _menuItems[currentSelection];
            }
        }
    }
}