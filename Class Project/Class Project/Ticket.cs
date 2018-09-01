using System.Collections.Generic;

namespace Class_Project
{
    public class Ticket
    {
        private int ticketId;
        private string summary;
        private Status status;
        private Priority priority;
        private string submitter;
        private string assigned;
        private List<string> watching;

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
            return ticketId;
        }

        private void SetTicketId(int ticketId)
        {
            this.ticketId = ticketId;
        }

        public string GetSummary()
        {
            return summary;
        }

        private void SetSummary(string summary)
        {
            this.summary = summary;
        }

        public Status GetStatus()
        {
            return status;
        }

        public void SetStatus(Status status)
        {
            this.status = status;
        }

        public Priority GetPriority()
        {
            return priority;
        }

        public void SetPriority(Priority priority)
        {
            this.priority = priority;
        }

        public string GetSubmitter()
        {
            return submitter;
        }

        private void SetSubmitter(string submitter)
        {
            this.submitter = submitter;
        }

        public string GetAssigned()
        {
            return assigned;
        }

        public void SetAssigned(string assigned)
        {
            this.assigned = assigned;
        }

        public List<string> GetWatching()
        {
            return watching;
        }

        private void SetWatching(List<string> watching)
        {
            this.watching = watching;
        }

        public void AppendSummary(string newSummary)
        {
            summary += "\n" + newSummary;
        }

        public void AddWatching(string watcher)
        {
            watching.Add(watcher);
        }

        public string GetWatchingString()
        {
            string watchers = "";

            for (int i = 0; i < watching.Count; i++)
            {
                if (i == watching.Count - 1)
                {
                    watchers += watching[i];
                }
                else
                {
                    watchers += watching[i] + "|";
                }
            }

            return watchers;
        }       

        public override string ToString()
        {
            return ticketId + ",\"" + summary + "\"," + status + "," + priority + "," + submitter + "," + assigned + "," + GetWatchingString();
        }
    }
}
