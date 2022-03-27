using AutoMapper;
using ru.emlsoft.WMS.Data.Abstract.Doc;
using ru.emlsoft.WMS.Data.Abstract.Storage;
using ru.emlsoft.WMS.Data.Dto.Doc;
using ru.emlsoft.WMS.Data.Dto.Storage;
using System;
using System.Linq;

namespace ru.emlsoft.WMS.Data.EF
{
    internal class DomainProfile : Profile
    {
		public DomainProfile()
		{
			CreateMap<Storage, StorageDto>()
				.ForMember(x=>x.StorageName, x=>x.MapFrom(y=>y.Name))
				.ForMember(x => x.Rows, x => x.MapFrom(y => y.Rows.Count()));

			CreateMap<Good, GoodDto>()
				.ForMember(x => x.Code, x => x.Ignore())
				.ForMember(x => x.GoodName, x => x.MapFrom(y => y.Name))
				.AfterMap((src, dst)=>dst.Code = src.Code?.Code);

			CreateMap<GoodDto, Good>()
				.ForMember(x=>x.Name, x=>x.MapFrom(y=>y.GoodName))
				.ForMember(x => x.Code, x => x.Ignore())
				.ForMember(x => x.CodeId, x => x.Ignore());

			CreateMap<Partner, PartnerDto>()
				.ForMember(x=>x.PartnerName, x=>x.MapFrom(y=>y.Name));
			CreateMap<PartnerDto, Partner>()
				.ForMember(x=>x.Name, x=>x.MapFrom(y=>y.PartnerName));

			CreateMap<InputDto, Doc>().As<Input>();
			CreateMap<InputDto, Input>()
				.ForMember(x=>x.InputCell, x=>x.Ignore())
				.AfterMap((src, dst) => dst.Input = dst);

			CreateMap<Doc, DocDto>();
			CreateMap<Input, InputDto>()
				.ForMember(x=>x.InputCell, x=>x.Ignore());
		}
	}
}
