using System.Collections.Generic;
using dotnet_rpg.DTOs.Skills;
using dotnet_rpg.DTOs.WeaponDto;
using dotnet_rpg.Models;

namespace dotnet_rpg.DTOs.CharacterDTOs
{

    // data transfer object - contine proprietatile care urmeaza a fi returnate pe request;
    public class GetCharacterDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "Frodo";
        public int HitPoints { get; set; } = 100;
        public int Strength { get; set; } = 10;
        public int Defence { get; set; } = 10;
        public int Intelligence { get; set; } = 10;
        public RpgClass Class { get; set; } = RpgClass.Knight;
        public GetWeaponDto Weapon { get; set; }
        public List<GetSkillDto> Skills { get; set; }
        public int Fights { get; set; }
        public int Victories { get; set; }
        public int Defeats { get; set; }

    }
}