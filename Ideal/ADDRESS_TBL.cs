namespace Ideal
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ADDRESS_TBL
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ADDRESS_TBL()
        {
            CLIENT_TBL = new HashSet<CLIENT_TBL>();
            CLIENT_TBL1 = new HashSet<CLIENT_TBL>();
            PROVIDER_TBL = new HashSet<PROVIDER_TBL>();
            REFERENCE_TBL = new HashSet<REFERENCE_TBL>();
        }

        [Key]
        [Column(TypeName = "numeric")]
        public decimal ADDR_ID { get; set; }

        [Required]
        [StringLength(200)]
        public string ADDR_STREET_NUMBER { get; set; }

        [Required]
        [StringLength(100)]
        public string ADDR_NEIGHBORHOOD { get; set; }

        [Required]
        [StringLength(100)]
        public string ADDR_CITY { get; set; }

        [Required]
        [StringLength(50)]
        public string ADDR_STATE { get; set; }

        [Required]
        [StringLength(50)]
        public string ADDR_COUNTRY { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CLIENT_TBL> CLIENT_TBL { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CLIENT_TBL> CLIENT_TBL1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PROVIDER_TBL> PROVIDER_TBL { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<REFERENCE_TBL> REFERENCE_TBL { get; set; }
    }
}
