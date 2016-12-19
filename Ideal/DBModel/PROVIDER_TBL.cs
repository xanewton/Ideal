#region Copyright
/*
 * Copyright (C) 2016 Angel Garcia
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
namespace Ideal.DBModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PROVIDER_TBL
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PROVIDER_TBL()
        {
            ITEM_TBL = new HashSet<ITEM_TBL>();
        }

        [Key]
        [StringLength(50)]
        public string PROV_ID { get; set; }

        [Required]
        [StringLength(100)]
        public string PROV_NAME { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PROV_ADDR_ID { get; set; }

        [StringLength(20)]
        public string PROV_PHONE { get; set; }

        [StringLength(200)]
        public string PROV_WEBSITE { get; set; }

        [StringLength(100)]
        public string PROV_NOTES { get; set; }

        public virtual ADDRESS_TBL ADDRESS_TBL { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ITEM_TBL> ITEM_TBL { get; set; }
    }
}
