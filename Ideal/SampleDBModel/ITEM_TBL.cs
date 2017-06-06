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
