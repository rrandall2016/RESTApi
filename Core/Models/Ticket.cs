using Core.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Ticket
    {

        public int? TicketId { get; set; }
        [Required]
        public int? ProjectId { get; set; }
        [Required]
        [StringLength(100)]
        public string Title { get; set; }
        public string Description { get; set; }
        [StringLength(50)]
        public string Owner { get; set; }

        //Data annotations validation attributes
        //We use the action filter for validation because we want it to be used
        //only when requesting the version 2 POST method without breaking the first

        [Ticket_EnsureReportDatePresent]
        public DateTime? ReportDate { get; set; }
        [Ticket_EnsureDueDatePresent]
        [Ticket_EnsureFutureDueDateOnCreationAttribute]
        [Ticket_EnsureDueDateAfterReportDate]
        public DateTime? DueDate { get; set; }

        public Project Project { get; set; }

        //Validation methods

        //When creating a ticket, if due date is entered, it has to be in the future 
        public bool ValidateFutureDueDate() 
        {
            if (TicketId.HasValue) return true;

            if (!DueDate.HasValue) return true;

            return (DueDate.Value > DateTime.Now);
        }        
        //When owner is assigned, the report date has to be present
        public bool ValidateReportDatePresence() 
        {
            if (string.IsNullOrWhiteSpace(Owner)) return true;

            return ReportDate.HasValue;
        }
        public bool ValidateDueDatePresence() 
        {
            if (string.IsNullOrWhiteSpace(Owner)) return true;

            return DueDate.HasValue;
        }
        //When duedate and report date are present, due date has to be later or equal 
        public bool ValidateDueDateAfterReportDate()
        {
            if (!DueDate.HasValue || !ReportDate.HasValue) return true;

            return DueDate.Value.Date >= ReportDate.Value.Date;
        }

    }
}
