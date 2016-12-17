namespace Ideal
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PAYMENTS_VIEW
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(10)]
        public string ACC_ID { get; set; }

        [StringLength(201)]
        public string NAME { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(200)]
        public string ITEM_NAME { get; set; }

        [Key]
        [Column(Order = 2, TypeName = "numeric")]
        public decimal PAY_AMOUNT { get; set; }

        [Key]
        [Column(Order = 3)]
        public DateTime PAY_DATE { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(10)]
        public string PAY_COLLECTOR { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(10)]
        public string PAY_INSPECTOR { get; set; }
    }
}
