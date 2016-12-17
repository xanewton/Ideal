namespace Ideal
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PAYMENT_MODE_TBL
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PAYMENT_MODE_TBL()
        {
            ACCOUNT_TBL = new HashSet<ACCOUNT_TBL>();
        }

        [Key]
        [StringLength(50)]
        public string PAYMD_ID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal PAYMD_AMOUNT { get; set; }

        [StringLength(50)]
        public string PAYMD_FREQUENCY { get; set; }

        [StringLength(25)]
        public string PAYMD_DAY { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ACCOUNT_TBL> ACCOUNT_TBL { get; set; }
    }
}
