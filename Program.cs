using NLog;
using BlogsConsole.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Configuration;
using System.Text;

namespace BlogsConsole
{
    // Version 0.3b - Need to figure out how to add Post. Asking instructor for assistance.
    class MainClass
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static int _index = 0;
        public static void Main(string[] args)
        {
            new MainClass();
        }
        MainClass()
        {
            logger.Info("Program started");

            try
            {
                DisplayMainMenu();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
            logger.Info("Program ended");
        }
        private void DisplayMainMenu()
        {
            bool userContinue = true;

            while (userContinue)
            {
                DisplayHeader();
                int menuMin = 1;
                int menuMax = PrintMainMenu();

                Console.Write("[=+> # <+=] Choose ({0}-{1})", menuMin, menuMax);           // menu prompt
                Console.SetCursorPosition(5, Console.CursorTop); // Move the cursor to the '#' in the prompt
                StringBuilder sb = new StringBuilder(); // Stringbuilder object that will hold the user's menu choice
                ConsoleKeyInfo cki;                     // ConsoleKeyInfo object
                cki = Console.ReadKey();                // Get the input from the user
                sb.Append(cki.KeyChar);                 // Throw that input into the StringBuilder object so it can be parsed.

                int userChoice = 0;
                try
                {
                    userChoice = Int32.Parse(sb.ToString());
                }
                catch
                {
                    userChoice = 0;
                }

                switch (userChoice)
                {
                    case 1:
                        DisplayAllBlogs();
                        break;
                    case 2:
                        CreateBlog();
                        break;
                    case 3:
                        CreatePost();
                        break;
                    case 4:
                        ExitGracefully();
                        break;
                    default:
                        InvalidMenuChoice();
                        break;
                }
            }
        }

        private void DisplayAllBlogs()
        {
            DisplayHeader();
            // Display all Blogs from the database
            var db = new BloggingContext();
            var query = db.Blogs.OrderBy(b => b.Name);

            Console.WriteLine("All blogs in the database:");
            foreach (var item in query)
            {
                Console.WriteLine(item.Name);
            }
            PressEnterToContinue();
        }

        private void CreateBlog()
        {
            DisplayHeader();
            // Create and save a new Blog
            Console.Write("Enter a name for a new Blog: ");
            var name = Console.ReadLine();

            var blog = new Blog { Name = name };

            var db = new BloggingContext();
            db.AddBlog(blog);
            logger.Info("Blog added - {name}", name);
            PressEnterToContinue();
        }

        private void CreatePost()
        {
            DisplayHeader();
            // Create a new post inside a blog
            // First we need to select which blog to which the post must be added
            var db = new BloggingContext();
            var query = db.Blogs.OrderBy(b => b.Name);

            // Our special menu needs a List<string> to enumerate through for menu items.
            // This routine creates that List<string> based on the blogs that exist at this moment.
            List<string> listOfBlogs = new List<string>();
            foreach (var item in query)
            {
                listOfBlogs.Add(item.Name);
            }

            // This routine sends the List<string> to MenuItemSelection() and the chosenBlog is in that var.
            var chosenBlog = MenuItemSelection(listOfBlogs);
            
            DisplayHeader();
            Console.WriteLine("You have selected the {0} Blog. You will begin writing your post for this blog.",chosenBlog);
            PressEnterToContinue();

        }


        private string MenuItemSelection(List<string> menuItems)
        {
            _index = 0;
            string str = "";

            Console.CursorVisible = false;
            while (str.Length == 0)
            {
                str = drawMenu(menuItems, Console.CursorLeft, Console.CursorTop);
            }
            Console.CursorVisible = true;
            return str;
        }

        private string drawMenu(List<string> items, int cursorLeft, int cursorTop)
        {
            Console.SetCursorPosition(cursorLeft, cursorTop);
            for (int g = 0; g < items.Count; g++)
            {
                if (g == _index)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine(items.ElementAt(g));
                }
                else
                {
                    Console.WriteLine(items.ElementAt(g));
                }
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
            }

            ConsoleKeyInfo ckey = Console.ReadKey();

            if (ckey.Key == ConsoleKey.DownArrow)
            {
                if (_index == items.Count - 1)
                {
                    Console.SetCursorPosition(cursorLeft, cursorTop);
                    _index = 0;
                }
                else
                {
                    Console.SetCursorPosition(cursorLeft, cursorTop);
                    _index++;
                }
            }
            else if (ckey.Key == ConsoleKey.UpArrow)
            {

                if (_index <= 0)
                {
                    Console.SetCursorPosition(cursorLeft, cursorTop);
                    _index = items.Count - 1;
                }
                else
                {
                    Console.SetCursorPosition(cursorLeft, cursorTop);
                    _index--;
                }
            }
            else if (ckey.Key == ConsoleKey.Enter)
            {
                return items[_index];
            }
            else
            {
                return "";
            }
            return "";
        }
        private void ExitGracefully()
        {
            DisplayHeader();
            Console.WriteLine("Now exiting this application...");
            PressEnterToContinue();
            System.Environment.Exit(0);
        }
        private void InvalidMenuChoice()
        {
            DisplayHeader();
            string invalidChoice = "You have made an invalid selection and therefore must try again.";
            Console.WriteLine("{0,15}", invalidChoice);
            PressEnterToContinue();
        }
        private void PressEnterToContinue()
        {
            Console.Write("Press Enter To Continue: ");
            Console.ReadKey(false);
            Console.WriteLine();
        }

        private string GetStringValue(String prompt)
        {
            var str = "";
            while (true)
            {
                Console.Write((prompt != null) ? prompt : "Please enter your response");
                Console.Write(": ");
                str = Console.ReadLine();

                if (str.Equals("-99"))
                {
                    ExitGracefully();
                }
                else if (str.Equals(""))
                {
                    Console.WriteLine("Invalid entry. Please try again.");
                }
                else
                {
                    return str;
                }
            }
        }
        private int PrintMainMenu()
        {
            string menuName = "Main";
            string[] menuChoices = new string[]
            {
                "Display All Blogs",
                "Add Blog",
                "Create Post (inside Blog)",
                "Exit Program"
            };

            Console.WriteLine(menuName + " Menu");
            for (int i = 0; i < menuChoices.Length; i++)
            {
                Console.WriteLine((i + 1) + ": " + menuChoices[i]);
            }

            return menuChoices.Count();
        }
        private void DisplayHeader()
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            for (int i = 0; i < Console.WindowWidth; i++)
            {
                Console.Write("*");
            }

            int winWidth = Console.WindowWidth - 1;
            Console.SetCursorPosition(winWidth, 1);
            Console.Write("*");

            Console.SetCursorPosition(0, 2);
            for (int i = 0; i < Console.WindowWidth; i++)
            {
                Console.Write("*");
            }
            string menuText = "Welcome to the Gregg Sperling Blogs and Posts System!";
            Console.SetCursorPosition(0, 1);
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (menuText.Length / 2)) + "}", menuText));
            Console.SetCursorPosition(0, 1);
            Console.Write("*");
            Console.SetCursorPosition(0, 5);
        }
    }
}
