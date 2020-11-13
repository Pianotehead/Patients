using System;

namespace Patients.Domain
{
    class JournalEntry
    {
        // NOTE: No attempts made to validate data. Maybe do it some other time?
        // Maybe it is not needed?
        public JournalEntry(string entryBy, DateTime entryDate, string entry)
        {
            EntryBy = entryBy;
            EntryDate = entryDate;
            Entry = entry;
        }

        public JournalEntry(int journalId, string entryBy, DateTime entryDate, string entry)
            : this(entryBy, entryDate, entry)
        {
            JournalId = journalId;
        }

        public JournalEntry(int id, int journalId, string entryBy, DateTime entryDate, string entry)
            : this(journalId, entryBy, entryDate, entry)
        {
            Id = id;
        }

        public int Id { get; }
        public int JournalId { get; }
        public string EntryBy { get; }
        public DateTime EntryDate { get; }
        public string Entry { get; }
    }
}