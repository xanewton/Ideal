namespace Ideal
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ACCOUNT_TBL
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ACCOUNT_TBL()
        {
            PAYMENT_TBL = new HashSet<PAYMENT_TBL>();
            SCHEDULED_PAYMENT_TBL = new HashSet<SCHEDULED_PAYMENT_TBL>();
        }

        [Key]
        [StringLength(10)]
        public string ACC_ID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal CLI_ID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal ITEM_ID { get; set; }

        [Required]
        [StringLength(50)]
        public string PAYMD_ID { get; set; }

        [Column(TypeName = "date")]
        public DateTime ACC_DATE { get; set; }

        [StringLength(50)]
        public string ACC_STATUS { get; set; }

        [Column(TypeName = "numeric")]
        public decimal ACC_PRICE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal ACC_HITCH { get; set; }

        [Column(TypeName = "numeric")]
        public decimal ACC_INITIAL_BALANCE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? ACC_CURRENT_BALANCE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? ACC_DISTANCE { get; set; }

        [Required]
        [StringLength(10)]
        public string ACC_SELLER_ID { get; set; }

        [StringLength(200)]
        public string ACC_NOTES { get; set; }

        public virtual EMPLOYEE_TBL EMPLOYEE_TBL { get; set; }

        public virtual CLIENT_TBL CLIENT_TBL { get; set; }

        public virtual ITEM_TBL ITEM_TBL { get; set; }

        public virtual PAYMENT_MODE_TBL PAYMENT_MODE_TBL { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PAYMENT_TBL> PAYMENT_TBL { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SCHEDULED_PAYMENT_TBL> SCHEDULED_PAYMENT_TBL { get; set; }
    }
}
