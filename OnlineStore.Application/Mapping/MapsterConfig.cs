using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Application.Mapping;
public static class MapsterConfig
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<DateTime, DateOnly>.NewConfig()
            .MapWith(src => DateOnly.FromDateTime(src));

        TypeAdapterConfig<DateOnly, DateTime>.NewConfig()
            .MapWith(src => src.ToDateTime(TimeOnly.MinValue));
    }
}
