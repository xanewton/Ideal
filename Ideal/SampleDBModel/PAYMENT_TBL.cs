namespace Ideal.SampleDBModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PAYMENT_TBL
    {
        [Key]
        [Column(TypeName = "numeric")]
        public decimal PAY_ID { get; set; }

        [Required]
        [StringLength(10)]
        public string ACC_ID { get; set; }

        public DateTime PAY_DATE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal PAY_AMOUNT { get; set; }

        public int? PAY_VISITS { get; set; }

        [Required]
        [StringLength(10)]
        public string PAY_COLLECTOR { get; set; }

        [Required]
        [StringLength(10)]
        public string PAY_INSPECTOR { get; set; }

        public virtual ACCOUNT_TBL ACCOUNT_TBL { get; set; }

        public virtual EMPLOYEE_TBL EMPLOYEE_TBL { get; set; }

        public virtual EMPLOYEE_TBL EMPLOYEE_TBL1 { get; set; }
    }
}
