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

        public Journal(int id)
        {
            Id = id;
        }

        public int Id { get; }

        public List<JournalEntry> Entries { get; set; } = new List<JournalEntry>();
    }
}