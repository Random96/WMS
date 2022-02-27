using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ru.EmlSoft.WMS.Data.Dto
{
    public class MenuDto
    {
        public MenuDto(string name, IEnumerable<MenuItemDto> items)
        {
            Name = name;

            Items = items;
        }

        public string Name { get; set; }

        public IEnumerable<MenuItemDto> Items { get; set; }
    }
}
