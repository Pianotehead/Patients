using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;

namespace Patients.Domain
{
    class TableMaker
    {
        public string[,] RowsAndColumns { get; }

        public TableMaker(string[,] rowsAndColumns)
        {
            RowsAndColumns = rowsAndColumns;
        }

        public void CreateTable(bool includeId = true)
        {
            Clear();
            int[] rightMargins = FindLongest();
            string borderVertical = " | ";
            string footer = "";

            int sumWidths = 0;
            for (int i = 0; i < rightMargins.Length; i++)
            {
                sumWidths += rightMargins[i];
            }

            if (includeId)
            {
                footer = "\n\n  ID: ";
                sumWidths += borderVertical.Length * 5;
            }
            else
            {
                footer = "\n\n  [C] Completed | [D] Delete | [Any other key] Back to main menu";
                sumWidths += (borderVertical.Length * 4 - rightMargins[0]);
            }

            if (sumWidths > LargestWindowWidth)
            {
                throw new ArgumentOutOfRangeException("The table is too big for the screen");
            }

            string borderHorizontal = " " + new string('-', sumWidths - 2);
            string nextLine = "";
            WriteLine($"\n{borderHorizontal}");

            for (int rows = 0; rows < RowsAndColumns.GetLength(0); rows++)
            {
                nextLine += borderVertical;

                if (includeId)
                {
                    nextLine += RowsAndColumns[rows, 0].PadRight(rightMargins[0]) + borderVertical;
                }
                nextLine += RowsAndColumns[rows, 1].PadRight(rightMargins[1]) + borderVertical;
                nextLine += RowsAndColumns[rows, 2].PadRight(rightMargins[2]) + borderVertical;
                nextLine += RowsAndColumns[rows, 3].PadRight(rightMargins[3]) + borderVertical;

                WriteLine(nextLine);
                WriteLine(borderHorizontal);

                nextLine = ""; // prepare for next line
            }

            Write(footer);
        }

        private int[] FindLongest()
        {
            int[] longest = new int[RowsAndColumns.GetLength(1)];
            
            for (int columns = 0; columns < longest.Length; columns++)
            {
                for (int rows = 0; rows < RowsAndColumns.GetLength(0); rows++)
                {
                    if (RowsAndColumns[rows,columns].Length > longest[columns])
                    {
                        longest[columns] = RowsAndColumns[rows, columns].Length;
                    }
                }
            }

            return longest;
        }
    }
}

