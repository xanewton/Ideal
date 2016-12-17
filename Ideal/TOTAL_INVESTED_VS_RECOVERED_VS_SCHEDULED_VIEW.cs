namespace Ideal
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TOTAL_INVESTED_VS_RECOVERED_VS_SCHEDULED_VIEW
    {
        [Key]
        [StringLength(34)]
        public string DESCRIPTION { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? VALUE { get; set; }
    }
}
