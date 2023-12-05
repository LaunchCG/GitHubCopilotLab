using System;
using SALearning.ApiModel;
using SALearning.DBModel;

namespace SALearning.Services
{
    public class AutoMapping : AutoMapper.Profile
    {
        public AutoMapping()
        {
            CreateMap<DateTime, DateOnly>().ConvertUsing(dt => DateOnly.FromDateTime(dt));
            CreateMap<DateOnly, DateTime>().ConvertUsing(dateOnly => dateOnly.ToDateTime(new TimeOnly(0,0)));

            CreateMap<UserProfile, DBProfile>().ReverseMap();
            CreateMap<Account, DBAccount>().ReverseMap();
            CreateMap<Holding, DBHolding>().ReverseMap();
            CreateMap<Operation, DBOperation>().ReverseMap();
        }
    }
}
