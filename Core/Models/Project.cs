using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Project
    {
        //ctrl d for copy line above
        public int ProjectId { get; set; }
        [Required]
        [StringLength(50)]//Validation using data annotations
        public string Name { get; set; }
        public List<Ticket> Tickets { get; set; }
    }
}
