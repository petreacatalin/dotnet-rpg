using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using dotnet_rpg.Data;
using dotnet_rpg.DTOs.CharacterDTOs;
using dotnet_rpg.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        // AutoMapper - mapeaza de pe DTO pe entitate; mapeaza de pe o clasa pe alta ( proprietati asemanatoare )
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        // verifica daca user care vrea sa faca request ; verifici daca e autorizat sa faca request
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CharacterService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccesor)
        {
            _httpContextAccessor = httpContextAccesor;
            _context = context;
            _mapper = mapper;

        }

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            var serviceReponse = new ServiceResponse<List<GetCharacterDto>>();
            Character character = _mapper.Map<Character>(newCharacter);
            character.User= await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
            _context.Add(character);
            await _context.SaveChangesAsync();
            // lui serviceresponse data i se atribuie o Lista de caractere din baza de datE , de pe tabela characters 
            // unde selecteaza (ca un foreach) automapper 
            // pt fiecare caracter (cum lambda) din baza de date ,creez un DTO , ToListAsync - da toata lista
            serviceReponse.Data = await _context.Characters.Where(c => c.User.Id==GetUserId()).Select(c => _mapper.Map<GetCharacterDto>(c)).ToListAsync();
            return serviceReponse;
        }

        // returneaza o lista de serviceresponse de getcharacter
        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            var serviceReponse = new ServiceResponse<List<GetCharacterDto>>();

            try
            {
                //   luam prima valoare care indeplineste conditia ca id din database sa fie = cu id din request;
                Character character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id && c.User.Id==GetUserId());
                if(character != null)
                {

                
                _context.Characters.Remove(character);
                await _context.SaveChangesAsync();

                serviceReponse.Data = _context.Characters.Where(c => c.User.Id== GetUserId()).Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
                }
                else
                {
                    serviceReponse.Succes = false;  
                    serviceReponse.Message= "Character not found.";
                }
            }
            catch (Exception ex)
            {
                serviceReponse.Succes = false;
                serviceReponse.Message = ex.Message;
            }
            return serviceReponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            var serviceReponse = new ServiceResponse<List<GetCharacterDto>>();
            // da-mi toate caracterele user-ului care face requestul
            var dbCharacters = await _context.Characters
            .Include(c => c.Weapon)
            .Include(c => c.Skills)
            .Where(c => c.User.Id == GetUserId()).ToListAsync();
            //pt fiecare caracter din database mapam un DTO si returnez o Lista de dto;
            serviceReponse.Data = dbCharacters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            return serviceReponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var serviceReponse = new ServiceResponse<GetCharacterDto>();
            // returneaza prima valoare sau default(null) , unde id char este egal cu id de pe request ( parametru)
            var dbCharacter = await _context.Characters
            .Include(c => c.Weapon)
            .Include(c => c.Skills)
            .FirstOrDefaultAsync(c => c.Id == id && c.User.Id==GetUserId());
            // serviceresponse.data o sa aiba valoare unui nou caracter dbCharacter de pe baza date;
            
            serviceReponse.Data= _mapper.Map<GetCharacterDto>(dbCharacter);            
            // automapper explicat:
            // serviceReponse.Data = new GetCharacterDto
            // {
            //     Id = dbCharacter.Id,
            //     Name = dbCharacter.Name,
            //     HitPoints = dbCharacter.HitPoints,
            //     Strength = dbCharacter.Strength,
            //     Intelligence = dbCharacter.Intelligence,
            //     Defence = dbCharacter.Defence,
            //     Class = dbCharacter.Class
            // };
            return serviceReponse;
        }
            // returneaza un serviceresponde de getchardto , vine obiect din request;
        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
                // instantiem un serviceresponde de getchardto
            var serviceReponse = new ServiceResponse<GetCharacterDto>();
            try
            {

                Character character = await _context.Characters
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == updatedCharacter.Id);
                if(character.User.Id == GetUserId())
                {
                character.Name = updatedCharacter.Name;
                character.HitPoints = updatedCharacter.HitPoints;
                character.Strength = updatedCharacter.Strength;
                character.Defence = updatedCharacter.Defence;
                character.Intelligence = updatedCharacter.Intelligence;
                character.Class = updatedCharacter.Class;             
                // explicatie de ce face SaveChanges
                _context.Update(character);
                await _context.SaveChangesAsync();
                // avem serviceresp.data si mapam characterul updatat.
                serviceReponse.Data = _mapper.Map<GetCharacterDto>(character); 
                }
                else
                {
                    serviceReponse.Succes= false;
                    serviceReponse.Message="character not found.";
                }

            }
            catch (Exception ex)
            {
                serviceReponse.Succes = false;
                serviceReponse.Message = ex.Message;
            }
            return serviceReponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> AddCharacterSkill(AddCharacterSkillDto newCharacterSkill)
        {
            var response = new ServiceResponse<GetCharacterDto>();
            try
            {
                var character = await _context.Characters
                .Include(c => c.Weapon)
                .Include(c => c.Skills)
                .FirstOrDefaultAsync(c => c.Id == newCharacterSkill.CharacterId && c.User.Id == GetUserId());
                if(character == null)
                {
                    response.Succes=false;
                    response.Message="Character not found.";
                    return response;
                }
                var skill= await _context.Skills.FirstOrDefaultAsync(s => s.Id == newCharacterSkill.SkillId);
                if(skill == null)
                {
                    response.Succes= false;
                    response.Message="Skill not found.";
                    return response;
                }
                character.Skills.Add(skill);
                await _context.SaveChangesAsync();
                response.Data= _mapper.Map<GetCharacterDto>(character);
            }
            catch(Exception ex)
            {
                response.Succes=false;
                response.Message= ex.Message;
            }
            return response;
        }
    }
}