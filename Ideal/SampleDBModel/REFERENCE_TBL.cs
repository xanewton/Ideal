namespace Ideal.SampleDBModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class REFERENCE_TBL
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public REFERENCE_TBL()
        {
            CLIENT_TBL = new HashSet<CLIENT_TBL>();
            CLIENT_TBL1 = new HashSet<CLIENT_TBL>();
        }

        [Key]
        [Column(TypeName = "numeric")]
        public decimal REF_ID { get; set; }

        [Required]
        [StringLength(100)]
        public string REF_NAMES { get; set; }

        [StringLength(100)]
        public string REF_FAMILY_NAME { get; set; }

        [StringLength(20)]
        public string REF_PHONE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? REF_ADDR_ID { get; set; }

        [Required]
        [StringLength(50)]
        public string REF_RELATION { get; set; }

        public virtual ADDRESS_TBL ADDRESS_TBL { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CLIENT_TBL> CLIENT_TBL { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CLIENT_TBL> CLIENT_TBL1 { get; set; }
    }
}
