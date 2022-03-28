using System.Threading.Tasks;
using dotnet_rpg.DTOs.CharacterDTOs;
using dotnet_rpg.DTOs.WeaponDto;
using dotnet_rpg.Models;

namespace dotnet_rpg.Services.WeaponService
{
    public interface IWeaponService
    {
         Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon);
    }
}