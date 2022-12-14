﻿using Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class CategoryTag : BaseEntity
    {
        public Category Category { get; set; }
        public int CategoryId { get; set; }

        public Tag Tag { get; set; }
        public int TagId { get; set; }
    }
}
