using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
using static System.Console;

namespace Patients.Domain
{
    class Patient
    {
        public int Id { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string SocialSecurityNumber { get; }
        public string FullName => $"{FirstName} {LastName}";

        public Journal Journal { get; set; }

        public Patient(string firstName, string lastName, string socialSecurityNumber)
        {
            if (string.IsNullOrWhiteSpace(firstName))
            {
                throw new ArgumentNullException("You must provide a first name", "firstName");
            }
            FirstName = firstName;

            if (string.IsNullOrWhiteSpace(lastName))
            {
                throw new ArgumentNullException("You must provide a first name", "lastName");
            }
            LastName = lastName;

            // Social security number error filters

            // FilterSSN #1: Is it in the format YYYYMMDD-XXXX?

            string expr = @"^(19|20)?\d{2}(0[1-9]|1[0-2])((0[1-9])|((1|2)[0-9])|30|31)-\d{4}$";
            Regex legalSSN = new Regex(expr);

            if (!legalSSN.IsMatch(socialSecurityNumber))
            {
                throw new ArgumentException("Social security number was not in the correct format", "socialSecurityNumber");
            }

            DateTime birthDate = DateTime.ParseExact(socialSecurityNumber.Substring(0, 8), "yyyyMMdd", CultureInfo.CurrentCulture);

            if (birthDate > DateTime.Now)
            {
                throw new ArgumentException("This social security number is based on a future date", "socialSecurityNumber");  
            }

            SocialSecurityNumber = socialSecurityNumber;
        }

        public Patient(int id, string firstName, string lastName, string socialSecurityNumber)
            : this(firstName, lastName, socialSecurityNumber)
        {
            Id = id;
        }

        public static string[,] TurnToStringArrays(List<Patient> patients)
        {
            string[,] patientColumns = new string[patients.Count + 1, 3];
            string[] headers = new string[] { "Id", "Name", "Social Security Number" };

            for (int i = 0; i < headers.Length; i++)
            {
                patientColumns[0, i] = headers[i];
            }
            for (int i = 1; i < patientColumns.GetLength(0); i++)
            {
                patientColumns[i, 0] = patients[i - 1].Id.ToString();
                patientColumns[i, 1] = patients[i - 1].FullName;
                patientColumns[i, 2] = patients[i - 1].SocialSecurityNumber;
            }

            return patientColumns;
        }
    }
}
