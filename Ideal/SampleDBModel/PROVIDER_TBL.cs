namespace Ideal.SampleDBModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PROVIDER_TBL
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PROVIDER_TBL()
        {
            ITEM_TBL = new HashSet<ITEM_TBL>();
        }

        [Key]
        [StringLength(50)]
        public string PROV_ID { get; set; }

        [Required]
        [StringLength(100)]
        public string PROV_NAME { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PROV_ADDR_ID { get; set; }

        [StringLength(20)]
        public string PROV_PHONE { get; set; }

        [StringLength(200)]
        public string PROV_WEBSITE { get; set; }

        [StringLength(100)]
        public string PROV_NOTES { get; set; }

        public virtual ADDRESS_TBL ADDRESS_TBL { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ITEM_TBL> ITEM_TBL { get; set; }
    }
}
