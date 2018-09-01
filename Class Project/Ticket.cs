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
            setTicketId(ticketId);
            setSummary(summary);
            setStatus(status);
            setPriority(priority);
            setSubmitter(submitter);
            setAssigned(assigned);
            setWatching(watching);
        }

        public string getTicketId()
        {
            return ticketId;
        }

        private void setTicketId(string ticketId)
        {
            this.ticketId = ticketId;
        }

        public string getSummary()
        {
            return summary;
        }

        private void setSummary(string summary)
        {
            this.summary = summary;
        }

        public Status getStatus()
        {
            return status;
        }

        public void setStatus(Status status)
        {
            this.status = status;
        }

        public Priority getPriority()
        {
            return priority;
        }

        public void setPriority(Priority priority)
        {
            this.priority = priority;
        }

        public string getSubmitter()
        {
            return submitter;
        }

        private void setSubmitter(string submitter)
        {
            this.submitter = submitter;
        }

        public string getAssigned()
        {
            return assigned;
        }

        public void setAssigned(string assigned)
        {
            this.assigned = assigned;
        }

        public ArrayList getWatching()
        {
            return watching;
        }

        private void setWatching(ArrayList watching)
        {
            this.watching = watching;
        }

        public void appendSummary(string newSummary)
        {
            summary += "\n" + newSummary;
        }

        public void addWatching(string watcher)
        {
            watching.Add(watcher);
        }
    }

}
