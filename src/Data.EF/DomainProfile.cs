using AutoMapper;
using ru.emlsoft.WMS.Data.Abstract.Storage;
using ru.emlsoft.WMS.Data.Dto.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ru.emlsoft.WMS.Data.EF
{
    internal class DomainProfile : Profile
    {
		public DomainProfile()
		{
			CreateMap<Storage, StorageDto>()
				.ForMember(x=>x.StorageName, x=>x.MapFrom(y=>y.Name))
				.ForMember(x => x.Rows, x => x.MapFrom(y => y.Rows.Count()));
		}
	}
}
