namespace Ideal.SampleDBModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class EMPLOYEE_TBL
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EMPLOYEE_TBL()
        {
            ACCOUNT_TBL = new HashSet<ACCOUNT_TBL>();
            PAYMENT_TBL = new HashSet<PAYMENT_TBL>();
            PAYMENT_TBL1 = new HashSet<PAYMENT_TBL>();
        }

        [Key]
        [StringLength(10)]
        public string EMP_ID { get; set; }

        [Required]
        [StringLength(50)]
        public string EMP_TYPE { get; set; }

        [Required]
        [StringLength(100)]
        public string EMP_NAMES { get; set; }

        [StringLength(100)]
        public string EMP_FAMILY_NAME { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ACCOUNT_TBL> ACCOUNT_TBL { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PAYMENT_TBL> PAYMENT_TBL { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PAYMENT_TBL> PAYMENT_TBL1 { get; set; }
    }
}
