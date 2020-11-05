using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.SqlClient;

namespace Patients.Domain
{
    class DataAccess
    {
        static string connectionString = "Server=localhost;Database=PatientJournal;Integrated Security=True";

        public static void AddNewPatientToDB(Patient patient)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            var sqlInsert = $@"INSERT INTO Patients (FirstName, LastName, SocialSecurityNumber)
                VALUES (@FirstName, @LastName, @SocialSecurityNumber)";

            SqlCommand insertPatient = new SqlCommand(sqlInsert, connection);

            insertPatient.Parameters.AddWithValue("@FirstName", patient.FirstName);
            insertPatient.Parameters.AddWithValue("@LastName", patient.LastName);
            insertPatient.Parameters.AddWithValue("@SocialSecurityNumber", patient.SocialSecurityNumber);

            // Connection has not yet been opened, but all variables in place

            connection.Open();
            insertPatient.ExecuteNonQuery();
            connection.Close();
        }
    }
}
