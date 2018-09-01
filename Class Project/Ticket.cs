using System;
using System.Collections;

namespace Class_Project
{
    public class Ticket
    {
        string ticketId;
        string summary;
        Status status;
        Priority priority;
        string submitter;
        string assigned;
        ArrayList watching;

        public Ticket(string ticketId, string summary, Status status, Priority priority, string submitter, string assigned, ArrayList watching)
        {
            SetTicketId(ticketId);
            SetSummary(summary);
            SetStatus(status);
            SetPriority(priority);
            SetSubmitter(submitter);
            SetAssigned(assigned);
            SetWatching(watching);
        }

        public string GetTicketId()
        {
            return ticketId;
        }

        private void SetTicketId(string ticketId)
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

        public ArrayList GetWatching()
        {
            return watching;
        }

        private void SetWatching(ArrayList watching)
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
