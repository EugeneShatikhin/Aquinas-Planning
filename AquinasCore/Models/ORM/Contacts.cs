namespace AquinasCore.Models.ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Contacts
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long inner_id { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(64)]
        public string c_name { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(64)]
        public string c_value { get; set; }

        public virtual Users Users { get; set; }
    }
}
