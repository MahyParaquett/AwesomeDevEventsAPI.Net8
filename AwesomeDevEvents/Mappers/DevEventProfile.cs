using AutoMapper;
using AwesomeDevEventsAPI.Etities;
using AwesomeDevEventsAPI.Models;

namespace AwesomeDevEventsAPI.Mappers
{
    public class DevEventProfile : Profile
    {
        public DevEventProfile() 
        {
            //Mapeamento do dominio para a view (para situações de get)
            CreateMap<DevEvent,DevEventViewModel>();
            CreateMap<DevEventSpeaker, DevEventSpeakerViewModel>();

            //Mapeio do input para dentro do domínio(para situações de cadastro/alteração)
            CreateMap<DevEventInputModel, DevEvent>();
            CreateMap<DevEventSpeakerInputModel, DevEventSpeaker>();

        }
    }
}
