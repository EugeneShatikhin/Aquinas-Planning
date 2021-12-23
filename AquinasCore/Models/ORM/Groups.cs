using Newtonsoft.Json;
namespace AquinasCore.Models.ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Groups
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Groups()
        {
            Payment_History = new HashSet<Payment_History>();
            Users = new HashSet<Users>();
        }

        [Key]
        public long group_id { get; set; }

        [Required]
        [StringLength(255)]
        public string group_name { get; set; }

        [Required]
        [StringLength(255)]
        public string group_description { get; set; }

        [JsonIgnore]
        public long owner_id { get; set; }

        [Required]
        [StringLength(255)]
        [JsonIgnore]
        public string credentials { get; set; }

        [Column(TypeName = "money")]
        [JsonIgnore]
        public decimal current_price { get; set; }

        [JsonIgnore]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Payment_History> Payment_History { get; set; }

        [JsonIgnore]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Users> Users { get; set; }
    }
}
