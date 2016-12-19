namespace Ideal.SampleDBModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SCHEDULED_PAYMENT_TBL
    {
        [Key]
        [Column(TypeName = "numeric")]
        public decimal SCHPAY_ID { get; set; }

        [Required]
        [StringLength(10)]
        public string ACC_ID { get; set; }

        public DateTime SCHPAY_DATE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal SCHPAY_AMOUNT { get; set; }

        public virtual ACCOUNT_TBL ACCOUNT_TBL { get; set; }
    }
}
