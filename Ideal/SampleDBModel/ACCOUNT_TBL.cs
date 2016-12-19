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
namespace Ideal.SampleDBModel
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PAYMENT_TBL> PAYMENT_TBL { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SCHEDULED_PAYMENT_TBL> SCHEDULED_PAYMENT_TBL { get; set; }

        public virtual EMPLOYEE_TBL EMPLOYEE_TBL { get; set; }

        public virtual CLIENT_TBL CLIENT_TBL { get; set; }

        public virtual ITEM_TBL ITEM_TBL { get; set; }

        public virtual PAYMENT_MODE_TBL PAYMENT_MODE_TBL { get; set; }
    }
}
