using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Resources;
using static System.Console;

namespace Patients.Domain
{
    class DataAccess
    {
        static string connectionString = "Server=localhost;Database=PatientJournal;Integrated Security=True";

        public static void AddNewPatientToDB(Patient patient)
        {
            var sqlInsert = @"INSERT INTO Patients (FirstName, LastName, SocialSecurityNumber)
                VALUES (@FirstName, @LastName, @SocialSecurityNumber)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand insertPatient = new SqlCommand(sqlInsert, connection))
            {
                insertPatient.Parameters.AddWithValue("@FirstName", patient.FirstName);
                insertPatient.Parameters.AddWithValue("@LastName", patient.LastName);
                insertPatient.Parameters.AddWithValue("@SocialSecurityNumber", patient.SocialSecurityNumber);

                connection.Open();
                insertPatient.ExecuteNonQuery();
                connection.Close();

            }
        }

        public static Patient FindPatient(string socialSecNumber)
        {
            var sql = "SELECT * FROM Patients WHERE SocialSecurityNumber = @SocialSecurityNumber";
            Patient patient = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@SocialSecurityNumber", socialSecNumber);
                connection.Open();
                SqlDataReader dataReader = command.ExecuteReader();

                if (dataReader.Read())
                {
                    patient = new Patient(id: (int)dataReader["Id"],
                    firstName: (string)dataReader["FirstName"], lastName: (string)dataReader["LastName"],
                    socialSecurityNumber: (string)dataReader["SocialSecurityNumber"]);
                }
                else
                {
                    Console.WriteLine("\nAn error has occurred. The patient register might be emtpy, the");
                    Console.WriteLine("patient not registered, or connection to the database failed\n");
                }

                connection.Close();
            }

            return patient;
        }

        public static List<Patient> ListOfPatients()
        {
            string sql = "SELECT Id, FirstName, LastName, SocialSecurityNumber FROM Patients";
            List<Patient> allPatients = new List<Patient>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int id = (int)reader["Id"];
                    string firstName = (string)reader["FirstName"];
                    string lastName = (string)reader["LastName"];
                    string socialSecurityNumber = (string)reader["SocialSecurityNumber"];
                    allPatients.Add(new Patient(id, firstName, lastName, socialSecurityNumber));
                }
                connection.Close();
            }
            return allPatients;
        }

        public static void CreateJournalForPatient(int patientId)
        {
            if (!PatientExists(patientId))
            {
                WriteLine("\nThere is no patient with the given ID\n");
                return;
            }

            var sql = @"INSERT INTO Journals (PatientId)
                VALUES (@PatientId)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand newJournal = new SqlCommand(sql, connection))
            {
                newJournal.Parameters.AddWithValue("@PatientId", patientId);
                connection.Open();
                newJournal.ExecuteNonQuery();
                connection.Close();

            }
        }

        public static Boolean PatientExists(int patientId)
        {
            string sql = @"SELECT Id FROM Patients WHERE Id=@Id";
            int idOfFoundPatient = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Id", patientId);
                connection.Open();
                SqlDataReader dataReader = command.ExecuteReader();

                if (dataReader.Read())
                {
                    idOfFoundPatient = (int)dataReader["Id"];
                }
                else
                {
                    Console.WriteLine("\nAn error has occurred. The patient register might be emtpy, the");
                    Console.WriteLine("patient not registered, or connection to the database failed\n");
                }

                connection.Close();
            }
            if (idOfFoundPatient == patientId)
            {
                return true;
            }
            return false;
        }

        public static int FetchJournalId(int patientId)
        {
            var sql = @"SELECT Id FROM Journals
                WHERE PatientId = @PatientId";
            int journalId = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@PatientId", patientId);
                connection.Open();
                SqlDataReader dataReader = command.ExecuteReader();

                if (dataReader.Read())
                {
                    journalId = (int)dataReader["Id"];
                }
                else
                {
                    Console.WriteLine("\nAn error has occurred. The journal does not exist");
                    Console.WriteLine("or connection to the database failed\n");
                }

                connection.Close();

                

            }

            return journalId;
        }

        public static void InsertJournalEntry(JournalEntry journalEntry)
        {
            var sql = @"INSERT INTO JournalEntries (
                        JournalId, EntryBy, EntryDate, Entry)
                        VALUES(@JournalId, @EntryBy, @EntryDate, @Entry)";
            //NOTE! Must send a JournalEntry instance into the method
            //      to get all the necessary data.
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@JournalId", journalEntry.JournalId);
                command.Parameters.AddWithValue("@EntryBy", journalEntry.EntryBy);
                command.Parameters.AddWithValue("@EntryDate", journalEntry.EntryDate);
                command.Parameters.AddWithValue("@Entry", journalEntry.Entry);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}
