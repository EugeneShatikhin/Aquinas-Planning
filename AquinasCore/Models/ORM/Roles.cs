using Newtonsoft.Json;
namespace AquinasCore.Models.ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Roles
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Roles()
        {
            Tasks = new HashSet<Tasks>();
            Users = new HashSet<Users>();
        }
        [Key]
        public int id { get; set; }

        [Required]
        [StringLength(255)]
        public string name { get; set; }
        [Required]
        public int weight { get; set; }
        [Required]
        public bool isAdmin { get; set; }
        [Required]
        public bool InviteUsers { get; set; }
        [Required]
        public bool MakeTask { get; set; }
        [Required]
        public bool CloseTask { get; set; }
        [Required]
        public bool SelfAssign { get; set; }
        [Required]
        public bool AssignOthers { get; set; }
        [Required]
        public bool CreateTag { get; set; }
        [Required]
        public bool AssignTag { get; set; }
        [Required]
        public bool Comment { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [JsonIgnore]
        public virtual ICollection<Tasks> Tasks { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [JsonIgnore]
        public virtual ICollection<Users> Users { get; set; }
    }
}
