using AutoMapper;
using FriscoDev.UI.Models;

namespace FriscoDev.UI.App_Start
{
    public class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            Mapper.Initialize(cfg => { cfg.AddProfile<ModelProfile>(); });
        }

    }
}