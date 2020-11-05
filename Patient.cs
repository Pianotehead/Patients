﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
using static System.Console;

namespace Patients
{
    class Patient
    {
        public string FirstName { get; }
        public string LastName { get; }
        public string SocialSecurityNumber { get; }

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

            // FilterSSN #2: Is the birthdate part of the SSN in the future?

            DateTime birthDate = DateTime.ParseExact(socialSecurityNumber.Substring(0, 8), "yyyyMMdd", CultureInfo.CurrentCulture);
            
            if (birthDate > DateTime.Now)
            {
                throw new ArgumentException("This social security number is based on a future date", "socialSecurityNumber");  
            }

            // If the SSN passed filters 1 and 2, then it is a legal social security number

            SocialSecurityNumber = socialSecurityNumber;
        }
    }
}
