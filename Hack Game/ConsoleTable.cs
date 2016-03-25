using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hack_Game
{
    [Serializable()]
    class ConsoleTable
    {
        private List<string> columns = new List<string>();

        public List<string> Columns
        {
            get { return columns; }
            set { columns = value; }
        }

        private List<string[]> rows = new List<string[]>();

        public List<string[]> Rows
        {
            get { return rows; }
            set { rows = value; }
        }

        /// <summary>
        /// Draw the table to console window
        /// </summary>
        /// <param name="caption">Caption shown in the header</param>
        /// <param name="drawHeader">Draws the column names if true</param>
        public void DrawToConsole(string caption, bool drawHeader)
        {
            int cWidth = Console.WindowWidth;
            int cHeight = Console.WindowHeight;

            //Draw headline
            for (int i = 1; i < (cWidth - caption.Length) / 2; i++)
            {
                Console.Write("=");
            }
            Console.Write(" " + caption + " ");
            for (int i = ((cWidth - caption.Length) / 2) + caption.Length; i < cWidth-1; i++)
            {
                Console.Write("=");
            }
            Console.Write("|");
            Console.SetCursorPosition(cWidth - 1, Console.CursorTop);
            Console.Write("|");
            for (int i = 0; i < cWidth; i++)
            {
                Console.Write("=");
            }

            //Draw columns
            int columnWidth = (cWidth / 2) - 2;

            if (drawHeader)
            {
                columnWidth = (cWidth / columns.Count) - 2;

                for (int i = 0; i < columns.Count; i++)
                {
                    Console.Write("|");
                    for (int n = 0; n < (columnWidth - columns[i].Length) / 2; n++)
                    {
                        Console.Write(" ");
                    }
                    Console.Write(columns[i]);
                    for (int n = ((columnWidth - columns[i].Length) / 2) + columns[i].Length; n < columnWidth; n++)
                    {
                        Console.Write(" ");
                    }
                    if (Console.CursorLeft == cWidth - 2)
                        Console.Write(" ");
                    Console.Write("|");
                }
                for (int i = 0; i < cWidth; i++)
                {
                    Console.Write("-");
                }
            }

            //Draw rows
            for (int i = 0; i < rows.Count; i++)
            {
                for (int n = 0; n < rows[i].Length; n++)
                {
                    Console.Write("|");
                    for (int x = 0; x < (columnWidth - rows[i][n].Length) / 2; x++)
                    {
                        Console.Write(" ");
                    }
                    if (rows[i][n].Length < columnWidth)
                        Console.Write(rows[i][n]);
                    else
                    {
                        string cut = " " + rows[i][n].Remove(columnWidth - 5) + "... ";
                        Console.Write(cut);
                    }
                    for (int x = ((columnWidth - rows[i][n].Length) / 2) + rows[i][n].Length; x < columnWidth; x++)
                    {
                        Console.Write(" ");
                    }
                    if (Console.CursorLeft == cWidth - 2)
                        Console.Write(" ");
                    Console.Write("|");
                }
            }
            for (int i = 0; i < cWidth; i++)
            {
                Console.Write("-");
            }
            Console.WriteLine();
        }
    }
}
