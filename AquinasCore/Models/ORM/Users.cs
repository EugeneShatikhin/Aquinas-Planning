using Newtonsoft.Json;
namespace AquinasCore.Models.ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Users
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Users()
        {
            Contacts = new HashSet<Contacts>();
            Tasks = new HashSet<Tasks>();
            Groups = new HashSet<Groups>();
            Roles = new HashSet<Roles>();
        }

        [Key]
        public long inner_id { get; set; }

        [Required]
        [StringLength(64)]
        public string username { get; set; }

        [Required]
        [JsonIgnore]
        [StringLength(64)]
        public string password { get; set; }

        [Required]
        [StringLength(64)]
        public string firstname { get; set; }

        [Required]
        [StringLength(64)]
        public string lastname { get; set; }

        [StringLength(255)]
        public string info { get; set; }

        [StringLength(255)]
        public string pfp_url { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Contacts> Contacts { get; set; }

        [JsonIgnore]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tasks> Tasks { get; set; }
        [JsonIgnore]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Groups> Groups { get; set; }
        [JsonIgnore]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Roles> Roles { get; set; }
    }
}
