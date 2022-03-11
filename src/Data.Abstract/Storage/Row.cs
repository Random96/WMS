﻿using ru.emlsoft.WMS.Data.Abstract.Access;
using ru.EmlSoft.WMS.Localization.Resources;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ru.emlsoft.WMS.Data.Abstract.Storage
{
    [Display(Name = "Хранение", Description = "Ряд стелажа", ResourceType = typeof(SharedResource))]
    public class Row : Entity
    {
        public Row() : base (EntityType.Row)
        {
            Code = String.Empty;
            Tiers = new List<Tier>();
        }

        public string Code { get; set; }

        public int StorageId { get; set; }
        public virtual Storage? Storage { get; set; }
        public virtual ICollection<Tier> Tiers { get; set; } = new List<Tier>();
    }
}