using AutoMapper;
using dotnet_rpg.DTOs.CharacterDTOs;
using dotnet_rpg.DTOs.Skills;
using dotnet_rpg.DTOs.WeaponDto;
using dotnet_rpg.Models;

namespace dotnet_rpg
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Character, GetCharacterDto>();
            CreateMap<AddCharacterDto, Character>();
            CreateMap<Weapon, GetWeaponDto>();
            CreateMap<Skill, GetSkillDto>();
        }
    }
}