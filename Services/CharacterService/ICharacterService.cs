using System.Collections.Generic;
using System.Threading.Tasks;
using dotnet_rpg.DTOs.CharacterDTOs;
using dotnet_rpg.Models;

namespace dotnet_rpg.Services.CharacterService
{
    public interface ICharacterService
    {
        // returneaza o lista de caractere
         Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters();
         // returneaza un singur caracter in functie de ID din parametru;
         Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id);
         // creaza un caracter in baza de date si returneaza o lista de caractere;
         Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter);
        // metoda care face update la un caracter si returneaza  un caracter;
         Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter);
         // metoda care sterge un caracter in functie de parametrul primit si returneaza o lista de caractere;
         Task<ServiceResponse<List<GetCharacterDto>>>DeleteCharacter(int id);
         Task<ServiceResponse<GetCharacterDto>> AddCharacterSkill(AddCharacterSkillDto newCharacterSkill);

    }
}