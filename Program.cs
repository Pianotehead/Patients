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
            Menu mainMenu = new Menu(new string[]
            { "Add patient", "Search patient", 
               "Create a journal for a patient",
               "Insert data into the journal of a patient", "Exit"
            });
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
                    ConsoleKey.D3, ConsoleKey.NumPad3,
                    ConsoleKey.D4, ConsoleKey.NumPad4,
                    ConsoleKey.D5, ConsoleKey.NumPad5
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
                        int patientId = AskForPatientId();
                        DataAccess.CreateJournalForPatient(patientId);
                        break;

                    // No need to create neither a Journal nor a Patient object
                    // Read the journal directly into the database.
                    // Only need to create objects when reading from the DB
                    // Might ask Robert about this

                    case ConsoleKey.D4:
                    case ConsoleKey.NumPad4:
                        patientId = AskForPatientId();
                        int journalId = DataAccess.FetchJournalId(patientId);
                        CreateJournalEntry(journalId);
                        break;

                    case ConsoleKey.D5:
                    case ConsoleKey.NumPad5:
                        applicationRunning = false;
                        break;
                }
            } while (applicationRunning);

            Clear();
            WriteLine("\n  Thank you for using this patient registration application");
            Thread.Sleep(1500);
            CursorVisible = true;

        }

        private static void CreateJournalEntry(int journalId)
        {
            Menu getData = new Menu(new string[]
            {
                "Entry by", "Entry"
            });
            string[] journalData = getData.AskForInputs();
            DateTime entryDate = DateTime.Now;
            JournalEntry journalEntry = new JournalEntry(journalId, journalData[0], entryDate, journalData[1]);
            DataAccess.InsertJournalEntry(journalEntry);
        }

        private static int AskForPatientId()
        {
            var allPatients = DataAccess.ListOfPatients();
            var patientColumns = Patient.TurnToStringArrays(allPatients);
            TableMaker patientsTable = new TableMaker(patientColumns);
            patientsTable.CreateTable();
            Write("\n\n  Type ID of patient to create a journal for: ");
            return int.Parse(ReadLine());
        }
    }
}
