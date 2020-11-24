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
                        List<Patient> allPatients = new List<Patient>();
                        bool insideSubmenu = true;
                        do
                        {
                            Clear();
                            journalMenu.CreateNumberedMenu();
                            ConsoleKey subMenuChoice = Menu.ActOnOnlyTheseKeys
                            (
                                ConsoleKey.D1, ConsoleKey.NumPad1,
                                ConsoleKey.D2, ConsoleKey.NumPad2,
                                ConsoleKey.D3, ConsoleKey.NumPad3
                            );
                            switch (subMenuChoice)
                            {
                                case ConsoleKey.D1:
                                case ConsoleKey.NumPad1:

                                    Clear();
                                    allPatients = DataAccess.ListOfPatients();
                                    int reportForPatient = AskForPatientId(allPatients);
                                    Patient newPatient = allPatients.Find(fakePatient => fakePatient.Id == reportForPatient);
                                    int journalId = DataAccess.FetchJournalId(reportForPatient);

                                    if (newPatient != null)
                                    {
                                        if (journalId == 0)
                                        {
                                            DataAccess.CreateJournal(newPatient);
                                            journalId = DataAccess.FetchJournalId(reportForPatient);
                                            // Must get the new JournalId. Double work, don't see another way
                                        }
                                        JournalEntry journalEntry = GetJournalData(journalId);
                                        DataAccess.CreateJournalEntry(journalEntry);

                                    }
                                    else if (newPatient == null)
                                    {
                                        Write("\n\n  Patient not found. Please try again");
                                    }


                                    // Next 2020-11-23:
                                    // 2) Write the new JournalId to the database (if it didn't exist)
                                    // 3) Write the bew journal entry to the database
                                    // (2-3 can probably be combined into one)

                                    // PROBLEM 2020-11-24:
                                    // THE APP CREATES A NEW JOURNAL, EVEN THOUGH IT EXISTED ALREADY

                                    //CONCLUSION
                                    //Very difficult to create both classes and write to the database at the same time
                                    //Because the data can't be read into classes until it has been written to the
                                    //database and read from it. That is a new database operation.
                                    //If patient has no journal, it has no journalId either. The journal ID is
                                    //created when read into the database. Same goes for IDs for journal entries.
                                    //There is no way of knowing these ID numbers beforehand.

                                    WriteLine("\n\n  Your data has been saved to the journal registration system");
                                    Thread.Sleep(1000);
                                    WriteLine("\n\n  Redirecting you to the previous menu...");
                                    Thread.Sleep(2000);

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
                    Write($"  {jEntry.JournalId}  {jEntry.EntryBy}");
                    Write($"{jEntry.EntryDate}  {jEntry.Entry}  \n");                    
                }
            }
        }

        private static JournalEntry GetJournalData(int journalId)
        {
            Menu getData = new Menu(new string[]
            {
                "Entry by", "Entry"
            });
            string[] journalData = getData.AskForInputs();
            DateTime entryDate = DateTime.Now;
            //Should have created the entryDate in the class, will not bother now
            JournalEntry journalEntry = new JournalEntry(journalId, journalData[0], entryDate, journalData[1]);
            return journalEntry;
        }

        private static int AskForPatientId(List<Patient> patients)
        {
            var patientColumns = Patient.TurnToStringArrays(patients);
            TableMaker patientsTable = new TableMaker(patientColumns);
            patientsTable.CreateTable();
            Write("\n\n  Type ID of patient to create a journal for: ");
            return int.Parse(ReadLine());
        }
    }
}
