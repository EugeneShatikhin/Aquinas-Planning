namespace AquinasCore.Models.ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Payment_History
    {
        public long id { get; set; }

        public long group_id { get; set; }

        [Column(TypeName = "money")]
        public decimal payment_amount { get; set; }

        public DateTime payment_date { get; set; }

        public virtual Groups Groups { get; set; }
    }
}
