namespace AquinasCore.Models.ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Comments
    {
        public long id { get; set; }

        public long task_id { get; set; }

        public long author_id { get; set; }

        [Required]
        [StringLength(512)]
        public string text { get; set; }

        public DateTime date { get; set; }

        public virtual Tasks Tasks { get; set; }
    }
}
