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
namespace Ideal
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
