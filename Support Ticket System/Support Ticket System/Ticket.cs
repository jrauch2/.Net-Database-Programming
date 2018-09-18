using System.Collections.Generic;
using Class_Project;

namespace Support_Ticket_System
{
    public class Ticket
    {
        private int _ticketId;
        private string _summary;
        private Status _status;
        private Priority _priority;
        private string _submitter;
        private string _assigned;
        private List<string> _watching;

        public Ticket(int ticketId, string summary, Status status, Priority priority, string submitter, string assigned, List<string> watching)
        {
            SetTicketId(ticketId);
            SetSummary(summary);
            SetStatus(status);
            SetPriority(priority);
            SetSubmitter(submitter);
            SetAssigned(assigned);
            SetWatching(watching);
        }

        public int GetTicketId()
        {
            return _ticketId;
        }

        private void SetTicketId(int ticketId)
        {
            _ticketId = ticketId;
        }

        public string GetSummary()
        {
            return _summary;
        }

        private void SetSummary(string summary)
        {
            _summary = summary;
        }

        public Status GetStatus()
        {
            return _status;
        }

        public void SetStatus(Status status)
        {
            _status = status;
        }

        public Priority GetPriority()
        {
            return _priority;
        }

        public void SetPriority(Priority priority)
        {
            _priority = priority;
        }

        public string GetSubmitter()
        {
            return _submitter;
        }

        private void SetSubmitter(string submitter)
        {
            _submitter = submitter;
        }

        public string GetAssigned()
        {
            return _assigned;
        }

        public void SetAssigned(string assigned)
        {
            _assigned = assigned;
        }

        public List<string> GetWatching()
        {
            return _watching;
        }

        private void SetWatching(List<string> watching)
        {
            _watching = watching;
        }

        public void AppendSummary(string newSummary)
        {
            _summary += "\n" + newSummary;
        }

        public void AddWatching(string watcher)
        {
            _watching.Add(watcher);
        }

        public string GetWatchingString()
        {
            var watchers = "";

            for (var i = 0; i < _watching.Count; i++)
            {
                if (i == _watching.Count - 1)
                {
                    watchers += _watching[i];
                }
                else
                {
                    watchers += _watching[i] + "|";
                }
            }

            return watchers;
        }

        public override string ToString()
        {
            return $"{_ticketId},\"{_summary}\",{_status},{_priority},{_submitter},{_assigned},{GetWatchingString()}";
        }
    }
}
