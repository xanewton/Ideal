#region Copyright
/*
 * Copyright (C) 2017 Angel Garcia
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
#endregion
namespace Ideal.SampleDBModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CLIENT_TBL
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CLIENT_TBL()
        {
            ACCOUNT_TBL = new HashSet<ACCOUNT_TBL>();
        }

        [Key]
        [Column(TypeName = "numeric")]
        public decimal CLI_ID { get; set; }

        [Required]
        [StringLength(100)]
        public string CLI_NAMES { get; set; }

        [StringLength(100)]
        public string CLI_FAMILY_NAME { get; set; }

        [StringLength(20)]
        public string CLI_HOME_PHONE { get; set; }

        [StringLength(20)]
        public string CLI_WORK_PHONE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? CLI_HOME_ADDR_ID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? CLI_WORK_ADDR_ID { get; set; }

        [StringLength(100)]
        public string CLI_OCUPATION { get; set; }

        [StringLength(200)]
        public string CLI_NOTES { get; set; }

        [StringLength(100)]
        public string CLI_PARTNER_NAMES { get; set; }

        [StringLength(100)]
        public string CLI_PARTNER_FAMILY_NAME { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? CLI_REFERENCE_ID1 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? CLI_REFERENCE_ID2 { get; set; }

        [StringLength(50)]
        public string CLI_SCORE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ACCOUNT_TBL> ACCOUNT_TBL { get; set; }

        public virtual ADDRESS_TBL ADDRESS_TBL { get; set; }

        public virtual ADDRESS_TBL ADDRESS_TBL1 { get; set; }

        public virtual REFERENCE_TBL REFERENCE_TBL { get; set; }

        public virtual REFERENCE_TBL REFERENCE_TBL1 { get; set; }
    }
}
