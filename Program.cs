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
            { "Add patient", "Journals", 
              "Exit"
            });
            Menu journalMenu = new Menu(new string[]
            {  "Write to a journal",
               "Show journal", "Back to the main menu"
            });
            ConsoleKey menuChoice;
            bool applicationRunning = true;
            do
            {
                mainMenu.CreateNumberedMenu();
                CursorVisible = false;
                menuChoice = Menu.ActOnOnlyTheseKeys
                (
                    ConsoleKey.D1, ConsoleKey.NumPad1,
                    ConsoleKey.D2, ConsoleKey.NumPad2,
                    ConsoleKey.D3, ConsoleKey.NumPad3
                );
                Clear();
                switch (menuChoice)
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
                        bool insideSubmenu = true;
                        do
                        {
                            Clear();
                            journalMenu.CreateNumberedMenu();
                            menuChoice = Menu.ActOnOnlyTheseKeys
                            (
                                ConsoleKey.D1, ConsoleKey.NumPad1,
                                ConsoleKey.D2, ConsoleKey.NumPad2,
                                ConsoleKey.D3, ConsoleKey.NumPad3
                            );
                            switch (menuChoice)
                            {
                                case ConsoleKey.D1:
                                case ConsoleKey.NumPad1:
                                    Clear();
                                    WriteLine("Wait a sec...");
                                    Thread.Sleep(5000);
                                    break;

                                case ConsoleKey.D2:
                                case ConsoleKey.NumPad2:
                                    Clear();
                                    Write("\n\n  Please type the Social Security Number of the patient\n");
                                    Write("  for which you want to view the journal (yyyymmdd-nnnn): ");
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
                                    WriteLine("Wait a sec...");
                                    Thread.Sleep(5000);
                                    break;

                                case ConsoleKey.D3:
                                case ConsoleKey.NumPad3:
                                    insideSubmenu = false;
                                    break;
                            }


                        } while (insideSubmenu);
                        break;

                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        applicationRunning = false;
                        //int patientId = AskForPatientId();
                        //DataAccess.CreateJournalForPatient(patientId);
                        break;

                    //case ConsoleKey.D4:
                    //case ConsoleKey.NumPad4:
                    //    patientId = AskForPatientId();
                    //    int journalId = DataAccess.FetchJournalId(patientId);
                    //    CreateJournalEntry(journalId);
                    //    break;

                    //case ConsoleKey.D5:
                    //case ConsoleKey.NumPad5:
                    //    // Get journal data
                    //    patientId = AskForPatientId();
                    //    journalId = DataAccess.FetchJournalId(patientId);
                    //    var journalEntries = DataAccess.LoadJournal(journalId);
                    //    //BUGFIX201121-1 Prevent app from crashing on empty input
                    //    PrintTheJournal(journalEntries);
                    //    WriteLine("\n\n  Press any key to return to the main menu");
                    //    ReadKey(true);
                    //    break;

                    //case ConsoleKey.D6:
                    //case ConsoleKey.NumPad6:
                    //    applicationRunning = false;
                    //    break;
                }
            } while (applicationRunning);

            Clear();
            WriteLine("\n  Thank you for using this patient registration application");
            Thread.Sleep(1500);
            CursorVisible = true;

        }

        private static void PrintTheJournal(List<JournalEntry> journalEntries)
        {
            Clear();
            WriteLine("\n\n");
            if (journalEntries.Count == 0)
            {
                WriteLine("  Patient is not registered or doesn't have a journal");
            }
            else
            {
                foreach (var jEntry in journalEntries)
                {
                    Write($"  {jEntry.JournalId}  ");
                    Write($"{jEntry.EntryBy}  ");
                    Write($"{jEntry.EntryDate}  ");
                    Write($"{jEntry.Entry}  \n");
                }
            }
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
