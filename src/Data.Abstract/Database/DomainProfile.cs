using AutoMapper;
using ru.emlsoft.WMS.Data.Abstract.Doc;
using ru.emlsoft.WMS.Data.Abstract.Storage;
using ru.emlsoft.WMS.Data.Dto.Doc;
using ru.emlsoft.WMS.Data.Dto.Storage;
using System;
using System.Linq;

namespace ru.emlsoft.WMS.Data.Abstract.Database
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            CreateMap<Storage.Storage, StorageDto>()
                .ForMember(x => x.StorageName, x => x.MapFrom(y => y.Name))
                .ForMember(x => x.Rows, x => x.MapFrom(y => y.Rows.Count()));


            CreateMap<StoreOrd, StoreOrdDto>()
                .ForMember(x => x.Pallet, x => x.Ignore())
                .ForMember(x => x.Cell, x => x.Ignore())
                .ForMember(x => x.Good, x => x.Ignore())
                .AfterMap((src, dst) => StoreOrdToDto(src, dst));

            CreateMap<Remains, RemainsDto>()
                .ForMember(x => x.Pallet, x => x.Ignore())
                .ForMember(x => x.Cell, x => x.Ignore())
                .ForMember(x => x.Good, x => x.Ignore())
                .AfterMap((src, dst) => RemainsToDto(src, dst));


            CreateMap<Good, GoodDto>()
                .ForMember(x => x.Code, x => x.Ignore())
                .ForMember(x => x.GoodName, x => x.MapFrom(y => y.Name))
                .AfterMap((src, dst) => dst.Code = src.Code?.Code);

            CreateMap<GoodDto, Good>()
                .ForMember(x => x.Name, x => x.MapFrom(y => y.GoodName))
                .ForMember(x => x.Code, x => x.Ignore())
                .ForMember(x => x.CodeId, x => x.Ignore());

            CreateMap<Partner, PartnerDto>()
                .ForMember(x => x.PartnerName, x => x.MapFrom(y => y.Name));
            CreateMap<PartnerDto, Partner>()
                .ForMember(x => x.Name, x => x.MapFrom(y => y.PartnerName));

            CreateMap<InputDto, Doc.Doc>().As<Input>();
            CreateMap<InputDto, Input>()
                .ForMember(x => x.InputCell, x => x.Ignore())
                .AfterMap((src, dst) => dst.Input = dst);

            CreateMap<Doc.Doc, DocDto>();
            CreateMap<Input, InputDto>()
                .ForMember(x => x.InputCell, x => x.Ignore());
        }

        public IWMSDataProvider? DataProvider { get; set; }

        public static StoreOrdDto StoreOrdToDto(IMapper mapper, IWMSDataProvider db, StoreOrd src)
        {
            if (src == null)
                throw new ArgumentNullException(nameof(src));

            var ret = mapper.Map<StoreOrdDto>(src);

            if (src.GoodId != 0)
            {
                if (src.Good == null)
                    ret.Good = db.GetGoodName(src.GoodId);
            }

            if (src.CellId != 0)
            {
                if (src.Cell?.Code?.Code == null)
                    ret.Cell = db.GetCellCode(src.CellId);
            }
            return ret;
        }

        private void StoreOrdToDto(StoreOrd src, StoreOrdDto dst)
        {
            if (src == null)
                return;

            if (src.GoodId != 0)
            {
                if (src.Good == null)
                {
                    if (DataProvider == null)
                        dst.Good = $"The good Id={src.GoodId} was not resoved";
                }
                else
                {
                    dst.Good = src.Good.Name;
                }
            }

            if (src.CellId != 0)
            {
                if (src.Cell?.Code?.Code == null)
                {
                    if (DataProvider == null)
                        dst.Cell = $"The cell Id={src.CellId} was not resoved";

                }
                else
                {
                    dst.Cell = src.Cell.Code.Code;
                }
            }

            if (src?.Pallet?.Code?.Code != null)
                dst.Pallet = src.Pallet.Code.Code;
        }
        public static RemainsDto RemainToDto(IMapper mapper, IWMSDataProvider db, Remains src)
        {
            var ret = mapper.Map<RemainsDto>(src);

            if (src.GoodId != 0)
            {
                if (src.Good == null)
                    ret.Good = db.GetGoodName(src.GoodId);
            }

            if (src.CellId != 0)
            {
                if (src.Cell?.Code?.Code == null)
                    ret.Cell = db.GetCellCode(src.CellId);
            }
            return ret;
        }

        private void RemainsToDto(Remains src, RemainsDto dst)
        {
            if (src == null)
                return;

            if (src.GoodId != 0)
            {
                if (src.Good == null)
                {
                    if (DataProvider == null)
                        dst.Good = $"The good Id={src.GoodId} was not resoved";
                }
                else
                {
                    dst.Good = src.Good.Name;
                }
            }

            if (src.CellId != 0)
            {
                if (src.Cell?.Code?.Code == null)
                {
                    if (DataProvider == null)
                        dst.Cell = $"The cell Id={src.CellId} was not resoved";

                }
                else
                {
                    dst.Cell = src.Cell.Code.Code;
                }
            }

            if (src?.Pallet?.Code?.Code != null)
                dst.Pallet = src.Pallet.Code.Code;
        }
    }
}
