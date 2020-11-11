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
            var sqlInsert = $@"INSERT INTO Patients (FirstName, LastName, SocialSecurityNumber)
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
            var sql = $@"SELECT * FROM Patients WHERE SocialSecurityNumber = @SocialSecurityNumber";
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
    }
}
