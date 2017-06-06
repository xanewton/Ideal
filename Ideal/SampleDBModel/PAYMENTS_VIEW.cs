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
