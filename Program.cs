using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
using Patients.Domain;
using static System.Console;

namespace Patients
{
    class Program
    {
        static void Main(string[] args)
        {
            // 5. nov 2020 - AddNewPatientToDB seems to work
            // Next: Create the menu, where I can choose to
            //       fill in the patient info from the console.

            Menu mainMenu = new Menu(new string[] { "Add patient", "Search patient", "Exit" });
            ConsoleKey input;
            bool applicationRunning = true;
            do
            {
                mainMenu.CreateNumberedMenu();
                CursorVisible = false;
                input = Menu.ActOnOnlyTheseKeys
                (
                    ConsoleKey.D1, ConsoleKey.NumPad1,
                    ConsoleKey.D2, ConsoleKey.NumPad2,
                    ConsoleKey.D3, ConsoleKey.NumPad3
                );
                Clear();
                switch (input)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        Menu askForData = new Menu(new string[] { 
                            "First name", "Last name", "Social security number (YYYMMDD-XXXX)" });
                        string[] patientInfo = askForData.AskForInputs();
                        Patient patient = new Patient(patientInfo[0], patientInfo[1], patientInfo[2]);
                        DataAccess.AddNewPatientToDB(patient);
                        break;

                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        // 2. Search patient
                        Clear();
                        Write("Please insert Social Security Number of patient to search for: ");
                        string socialSecNumber = ReadLine();
                        Patient findThisPatient = DataAccess.FindPatient(socialSecNumber);
                        Clear();
                        if (findThisPatient != null)
                        {
                            WriteLine(findThisPatient.FullName);
                        }
                        else
                        {
                            WriteLine("Patient was not found");
                        }
                        Write("\nPress any key to continue...");
                        ReadKey(true);
                        break;

                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        applicationRunning = false;
                        break;
                }
            } while (applicationRunning);

            Clear();
            WriteLine("\n  Thank you for using this patient registration application");
            Thread.Sleep(1500);
            CursorVisible = true;

        }
    }
}
