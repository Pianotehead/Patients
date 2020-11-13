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
            int sumWidths = 0;

            for (int i = 0; i < rightMargins.Length; i++)
            {
                sumWidths += rightMargins[i];
            }
            if (sumWidths > LargestWindowWidth)
            {
                throw new ArgumentOutOfRangeException("The table is too big for the screen");
            }

            string borderHorizontal = " " + new string('-', sumWidths);
            string nextLine = "  ";

            for (int rows = 0; rows < RowsAndColumns.GetLength(0); rows++)
            {
                if (includeId)
                {
                    nextLine += RowsAndColumns[rows, 0].PadRight(rightMargins[0] + 2);
                    borderHorizontal += "-----";
                }
                nextLine += RowsAndColumns[rows, 1].PadRight(rightMargins[1] + 2);
                nextLine += RowsAndColumns[rows, 2].PadRight(rightMargins[2]);
                WriteLine(nextLine);
                if (rows == 0)
                {
                    WriteLine($"{borderHorizontal}");
                }
                nextLine = "  "; // prepare for next line
            }
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

