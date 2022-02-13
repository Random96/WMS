﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ru.EmlSoft.WMS.Data.Abstract.Database
{
    public enum Ordering
    {
        Asc, Dsc
    }

    public class OrderElement
    {
        private readonly Ordering _order;
        private string _name;

        public OrderElement(Ordering order, string name)
        {
            _order = order;
            _name = name;
        }

        public Ordering Order => _order;
        public string Name => _name;

    }
}