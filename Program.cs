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
                                            // Must get the new JournalId.
                                            // Double work, but see no any other way
                                        }
                                        JournalEntry journalEntry = GetJournalData(journalId);
                                        DataAccess.CreateJournalEntry(journalEntry);
                                    }
                                    else if (newPatient == null)
                                    {
                                        Write("\n\n  Patient not found. Please try again");
                                    }

                                    WriteLine("\n\n  Your data has been saved to the journal registration system");
                                    Thread.Sleep(1000);
                                    WriteLine("\n\n  Redirecting you to the previous menu...");
                                    Thread.Sleep(2000);
                                    break;

                                case ConsoleKey.D2:
                                case ConsoleKey.NumPad2:
                                    ViewPatientsJournal();
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
                }
            } while (applicationRunning);

            Clear();
            WriteLine("\n  Thank you for using this patient registration application");
            Thread.Sleep(1500);
            CursorVisible = true;

        }

        private static void ViewPatientsJournal()
        {
            Clear();
            List<Patient> allPatients = DataAccess.ListOfPatients();
            int patientId = AskForPatientId(allPatients);
            Patient findThisPatient = allPatients.Find(fakePatient => fakePatient.Id == patientId);
            bool patientExists = findThisPatient != null;
            bool patientHasJournal = false;
            List<string> journalData = new List<string>();

            if (patientExists)
            {
                patientExists = true;
                journalData = DataAccess.LoadJournal(findThisPatient.Id);
                patientHasJournal = journalData.Count > 0 ? true : false;
            }
            if (patientExists && patientHasJournal)
            {
                string[,] journalItems = new string[journalData.Count + 1, 5];
                string[] headers = new string[] { "Name", "Social Sec. No.", "Entry By","Date of entry","Entry text" };
                string[] cells = new string[headers.Length];

                for (int i = 0; i < headers.Length; i++)
                {
                    journalItems[0, i] = headers[i];
                }

                for (int row = 0; row < journalData.Count; row++)
                {
                    cells = journalData[row].Split(';');
                    for (int cell = 0; cell < cells.Length; cell++)
                    {
                        journalItems[row + 1, cell] = cells[cell];
                    }
                }
                TableMaker journalDataTable = new TableMaker(journalItems);
                journalDataTable.CreateTable();
            }
            else
            {
                WriteLine("\n\n  There is no journal for this patient or (s)he not registered.");
            }

            Write("\n\n  Press any key to return to the previous menu");
            ReadKey(true);
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
            Write("\n\n  Type ID of patient to create or view a journal for: ");
            return int.Parse(ReadLine());
        }
    }
}
