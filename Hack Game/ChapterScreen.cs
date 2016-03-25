using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hack_Game
{
    class ChapterScreen
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public void ShowScreen()
        {
            Console.Clear();
            Console.CursorVisible = false;
            int cWidth = Console.WindowWidth;
            int cHeight = Console.WindowHeight;
            Console.SetCursorPosition((cWidth - Title.Length) / 2, 2);
            Console.ForegroundColor = ConsoleColor.White;
            WriteInSteps(Title, 100);
            Console.SetCursorPosition(0, 5);
            Console.ForegroundColor = ConsoleColor.Gray;
            WriteInSteps(Description, 50);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\n\n");
            Console.SetCursorPosition((cWidth - 15) / 2, cHeight-3);
            Console.WriteLine("Next >> (ENTER)");
            Console.ForegroundColor = ConsoleColor.Black;
            while (Console.ReadKey().Key != ConsoleKey.Enter) { }
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();
            Console.CursorVisible = true;
        }

        void WriteInSteps(string text, int milSeconds)
        {
            string[] words = text.Split(' ');
            foreach (string word in words)
            {
                if (Console.WindowWidth - Console.CursorLeft < word.Length)
                    Console.Write("\n");
                
                foreach (char c in word)
                {
                    Console.Write(c);
                    Thread.Sleep(milSeconds);
                }
                Console.Write(" ");
            }
        }
    }
}
