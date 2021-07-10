using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Project1.Models.Models
{
    public class Item : _BaseEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }

        public int StepId { get; set; }
        [ForeignKey("StepId ")]
        public virtual Step Step { get; set; }

    }
}
