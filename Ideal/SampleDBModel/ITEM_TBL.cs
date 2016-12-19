namespace Ideal.SampleDBModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ITEM_TBL
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ITEM_TBL()
        {
            ACCOUNT_TBL = new HashSet<ACCOUNT_TBL>();
        }

        [Key]
        [Column(TypeName = "numeric")]
        public decimal ITEM_ID { get; set; }

        [Required]
        [StringLength(50)]
        public string PROV_ID { get; set; }

        [Required]
        [StringLength(200)]
        public string ITEM_NAME { get; set; }

        [Column(TypeName = "numeric")]
        public decimal ITEM_BUY_PRICE { get; set; }

        [Column(TypeName = "date")]
        public DateTime ITEM_BUY_DATE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ACCOUNT_TBL> ACCOUNT_TBL { get; set; }

        public virtual PROVIDER_TBL PROVIDER_TBL { get; set; }
    }
}
