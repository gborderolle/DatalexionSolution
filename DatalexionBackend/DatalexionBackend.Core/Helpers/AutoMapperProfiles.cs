using AutoMapper;
using DatalexionBackend.Core.DTO;
using DatalexionBackend.Core.Domain.Entities;

namespace DatalexionBackend.Core.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            #region DatalexionUser

            CreateMap<DatalexionUser, DatalexionUserDTO>()
                .ReverseMap();

            #endregion DatalexionUser

            #region Candidate

            CreateMap<Candidate, CandidateDTO>()
                .ForMember(destinationMember: dest => dest.PhotoURL, opt => opt.MapFrom(src => src.Photo.URL))
                .ReverseMap();

            CreateMap<Candidate, CandidateCreateDTO>()
                .ForMember(destinationMember: dest => dest.Photo, opt => opt.MapFrom(src => src.Photo.URL))
                .ReverseMap();

            #endregion Candidate

            #region Circuit

            // N..N
            CreateMap<Circuit, CircuitDTO>()
              .ForMember(dest => dest.ListPhotosURL, opt => opt.MapFrom(src => src.ListPhotos.Select(photo => photo.URL).ToList()))
                .ForMember(dest => dest.ListCircuitDelegados, opt => opt.MapFrom(src => src.ListCircuitDelegados))
                .ForMember(dest => dest.ListCircuitSlates, opt => opt.MapFrom(src => src.ListCircuitSlates))
                .ForMember(dest => dest.ListCircuitParties, opt => opt.MapFrom(src => src.ListCircuitParties))
                .ReverseMap();

            // N..N
            CreateMap<CircuitCreateDTO, Circuit>()
                .ForMember(dest => dest.ListCircuitDelegados, opt => opt.MapFrom(src =>
                    src.ListCircuitDelegados != null
                        ? src.ListCircuitDelegados.Select(dto => new CircuitDelegado
                        {
                            DelegadoId = dto.DelegadoId,
                        }).ToList()
                        : new List<CircuitDelegado>()))
                .ReverseMap();

            // N..N
            CreateMap<CircuitCreateDTO, Circuit>()
              .ForMember(dest => dest.ListPhotos, opt => opt.Ignore()) // Ignorar porque lo agrego a mano en el Controller
                .ForMember(dest => dest.ListCircuitSlates, opt => opt.MapFrom(src =>
                    src.ListCircuitSlates != null
                        ? src.ListCircuitSlates.Select(dto => new CircuitSlate
                        {
                            SlateId = dto.SlateId,
                        }).ToList()
                        : new List<CircuitSlate>()))
                .ForMember(dest => dest.ListCircuitParties, opt => opt.MapFrom(src =>
                    src.ListCircuitParties != null
                        ? src.ListCircuitParties.Select(dto => new CircuitParty
                        {
                            PartyId = dto.PartyId,
                        }).ToList()
                        : new List<CircuitParty>()))
                .ReverseMap();

            // N..N
            CreateMap<CircuitSlate, CircuitSlateDTO>()
                .ReverseMap();
            // N..N
            CreateMap<CircuitParty, CircuitPartyDTO>()
                .ReverseMap();

            // N..N
            CreateMap<CircuitDelegado, CircuitDelegadoDTO>()
                .ReverseMap();

            CreateMap<CircuitPartyCreateDTO, CircuitParty>()
                .ForMember(dest => dest.PartyId, opt => opt.MapFrom(src => src.PartyId))
                .ForMember(dest => dest.Votes, opt => opt.MapFrom(src => src.Votes))
                .ForMember(dest => dest.CircuitId, opt => opt.MapFrom(src => src.CircuitId))
                .ReverseMap();

            #endregion Circuit

            #region Client

            CreateMap<Client, ClientDTO>()
                .ForMember(dest => dest.ListUsers, opt => opt.MapFrom(src => src.ListUsers))
                .ForMember(dest => dest.ListDelegados, opt => opt.MapFrom(src => src.ListDelegados))
            .ReverseMap();

            CreateMap<Client, ClientCreateDTO>()
                .ForMember(dest => dest.ListUsers, opt => opt.MapFrom(src => src.ListUsers))
                .ForMember(dest => dest.ListDelegados, opt => opt.MapFrom(src => src.ListDelegados))
            .ReverseMap();

            #endregion Client

            #region Delegado

            CreateMap<Delegado, DelegadoDTO>()
                .ForMember(dest => dest.ListCircuitDelegados, opt => opt.MapFrom(src => src.ListCircuitDelegados))
                .ForMember(dest => dest.ListMunicipalities, opt => opt.MapFrom(src => src.ListMunicipalities))
                .ReverseMap();

            CreateMap<Delegado, DelegadoCreateDTO>()
                .ForMember(dest => dest.ListCircuitDelegados, opt => opt.MapFrom(src => src.ListCircuitDelegados))
                .ForMember(dest => dest.ListMunicipalities, opt => opt.MapFrom(src => src.ListMunicipalities))
                .ReverseMap();

            #endregion Delegado

            #region Municipality

            CreateMap<Municipality, MunicipalityDTO>()
                .ForMember(dest => dest.ListCircuits, opt => opt.MapFrom(src => src.ListCircuits))
                .ReverseMap();

            CreateMap<Municipality, MunicipalityCreateDTO>()
           .ForMember(dest => dest.ListCircuits, opt => opt.MapFrom(src => src.ListCircuits))
           .ReverseMap();

            #endregion Municipality

            #region Participant

            CreateMap<Participant, ParticipantDTO>()
                .ReverseMap();

            CreateMap<Participant, ParticipantCreateDTO>()
                .ReverseMap();

            #endregion Participant

            #region Party

            CreateMap<Party, PartyDTO>()
                .ForMember(destinationMember: dest => dest.PhotoLongURL, opt => opt.MapFrom(src => src.PhotoLong.URL))
                .ForMember(destinationMember: dest => dest.PhotoShortURL, opt => opt.MapFrom(src => src.PhotoShort.URL))
                .ForMember(dest => dest.ListWings, opt => opt.MapFrom(src => src.ListWings))
                .ForMember(dest => dest.ListCircuitParties, opt => opt.MapFrom(src => src.ListCircuitParties))
                .ReverseMap();

            CreateMap<Party, PartyCreateDTO>()
                .ForMember(destinationMember: dest => dest.PhotoLong, opt => opt.MapFrom(src => src.PhotoLong.URL))
                .ForMember(destinationMember: dest => dest.PhotoShort, opt => opt.MapFrom(src => src.PhotoShort.URL))
                .ForMember(dest => dest.ListWings, opt => opt.MapFrom(src => src.ListWings))
                .ForMember(dest => dest.ListCircuitParties, opt => opt.MapFrom(src => src.ListCircuitParties))
                .ReverseMap();

            #endregion Party

            #region Province

            CreateMap<Province, ProvinceDTO>()
                .ForMember(dest => dest.ListSlates, opt => opt.MapFrom(src => src.ListSlates))
                .ForMember(dest => dest.ListMunicipalities, opt => opt.MapFrom(src => src.ListMunicipalities))
                .ReverseMap();

            CreateMap<Province, ProvinceCreateDTO>()
                .ForMember(dest => dest.ListSlates, opt => opt.MapFrom(src => src.ListSlates))
                .ForMember(dest => dest.ListMunicipalities, opt => opt.MapFrom(src => src.ListMunicipalities))
                .ReverseMap();

            #endregion Province

            #region Slate

            CreateMap<Slate, SlateDTO>()
                .ForMember(dest => dest.Candidate, opt => opt.MapFrom(src => src.Candidate))
                // Use ForPath instead of ForMember for nested properties
                .ForPath(dest => dest.Candidate.Photo, opt => opt.MapFrom(src => src.Candidate.Photo.URL))
                .ForMember(dest => dest.Wing, opt => opt.MapFrom(src => src.Wing))
                .ForMember(dest => dest.Province, opt => opt.MapFrom(src => src.Province))
                .ForMember(dest => dest.ListParticipants, opt => opt.MapFrom(src => src.ListParticipants))
                .ForMember(dest => dest.ListCircuitSlates, opt => opt.MapFrom(src => src.ListCircuitSlates))
                // Assuming there's a direct PhotoURL property in Slate to map
                .ForMember(dest => dest.PhotoURL, opt => opt.MapFrom(src => src.Photo.URL))
                .ReverseMap();

            CreateMap<Slate, SlateCreateDTO>()
                .ForMember(dest => dest.Candidate, opt => opt.MapFrom(src => src.Candidate))
                // Use ForPath instead of ForMember for nested properties
                .ForPath(dest => dest.Candidate.Photo, opt => opt.MapFrom(src => src.Candidate.Photo.URL))
                .ForMember(dest => dest.Wing, opt => opt.MapFrom(src => src.Wing))
                .ForMember(dest => dest.Province, opt => opt.MapFrom(src => src.Province))
                .ForMember(dest => dest.ListParticipants, opt => opt.MapFrom(src => src.ListParticipants))
                .ForMember(dest => dest.ListCircuitSlates, opt => opt.MapFrom(src => src.ListCircuitSlates))
                // Assuming there's a direct PhotoURL property in Slate to map
                .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => src.Photo.URL))
                .ReverseMap();

            #endregion Slate

            #region Wing

            CreateMap<Wing, WingDTO>()
                .ForMember(dest => dest.ListSlates, opt => opt.MapFrom(src => src.ListSlates))
                .ForMember(dest => dest.PhotoURL, opt => opt.MapFrom(src => src.Photo))
                .ForMember(destinationMember: dest => dest.PhotoURL, opt => opt.MapFrom(src => src.Photo.URL))
                .ReverseMap();

            CreateMap<Wing, WingCreateDTO>()
                .ForMember(dest => dest.ListSlates, opt => opt.MapFrom(src => src.ListSlates))
                .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => src.Photo))
                .ForMember(destinationMember: dest => dest.Photo, opt => opt.MapFrom(src => src.Photo.URL))
                .ReverseMap();

            #endregion Wing

        }

    }
}