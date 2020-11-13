using System;
using System.Collections.Generic;
using System.Text;

namespace Patients.Domain
{
    class Journal
    {
        // NOTE: No attempts made to validate data. Maybe do it some other time?
        // Maybe it is not needed?
        public Journal()
        {

        }

        public Journal(int patientId)
        {
            if (patientId < 1)
            {
                throw new ArgumentException("Not a legal value for PatientID", "patientId");
            }
            PatientId = patientId;
        }

        public Journal(int id, int patientId)
            : this(patientId)
        {
            if (id < 1)
            {
                throw new ArgumentException("Not a legal value for PatientID", "patientId");
            }
            Id = id;
        }

        public int Id { get; }
        public int PatientId { get; }

        public List<JournalEntry> Entries { get; set; } = new List<JournalEntry>();
    }
}