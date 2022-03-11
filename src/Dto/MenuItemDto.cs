using System;
using System.Linq;

namespace ru.emlsoft.WMS.Data.Dto
{
    public class MenuItemDto
    {
        public MenuItemDto(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public string Name { get; set; }
        public string Description { get; set; }
    }
}
