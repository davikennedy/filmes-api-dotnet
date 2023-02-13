using AutoMapper;
using FilmesAPI.Data.Dtos;
using FilmesAPI.Models;

namespace EnderecosAPI.Profiles
{
    public class EnderecoProfile : Profile
    {
        public EnderecoProfile() 
        {
            CreateMap<CreateEnderecoDto, Endereco>();
            CreateMap<Endereco, ReadEnderecoDto>();
            CreateMap<UpdateEnderecoDto, Endereco>();
        }
    }
}
