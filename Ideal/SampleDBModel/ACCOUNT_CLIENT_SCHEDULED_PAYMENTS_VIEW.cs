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

    public partial class ACCOUNT_CLIENT_SCHEDULED_PAYMENTS_VIEW
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(10)]
        public string ACC_ID { get; set; }

        [StringLength(201)]
        public string NAME { get; set; }

        [StringLength(200)]
        public string ITEM_NAME { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string PAYMD_ID { get; set; }

        [Key]
        [Column(Order = 2, TypeName = "date")]
        public DateTime ACC_DATE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? ACC_DISTANCE { get; set; }

        public double? CALCULATED_X_TIMES_ITEM_BUY_PRICE { get; set; }

        [Key]
        [Column(Order = 3, TypeName = "numeric")]
        public decimal ACC_PRICE { get; set; }

        [Key]
        [Column(Order = 4, TypeName = "numeric")]
        public decimal ACC_HITCH { get; set; }

        [Key]
        [Column(Order = 5, TypeName = "numeric")]
        public decimal ACC_INITIAL_BALANCE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? CALCULATED_CURRENT_BALANCE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? CALCULATED_GAIN { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? ITEM_BUY_PRICE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PAYMENTS { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? SUM_SCHEDULED_PAYMENTS { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? DIFF_SCHEDULED { get; set; }
    }
}
