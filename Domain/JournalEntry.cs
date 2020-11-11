using System;

namespace Patients.Domain
{
    class JournalEntry
    {
        // NOTE: No attempts made to validate data. Maybe do it some other time?
        // Maybe it is not needed?
        public JournalEntry(string entryBy, DateTime entryDate, string entry)
        {
            Entry = entry;
            EntryBy = entryBy;
            EntryDate = entryDate;
        }

        public JournalEntry(int id, string entryBy, DateTime entryDate, string entry)
        {
            Id = id;
            Entry = entry;
            EntryBy = entryBy;
            EntryDate = entryDate;
        }

        public int Id { get; }
        public string EntryBy { get; }
        public DateTime EntryDate { get; }
        public string Entry { get; }
    }
}