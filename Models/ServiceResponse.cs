namespace dotnet_rpg.Models
{


 // o clasa generica folosita ca return type la Servicii
    public class ServiceResponse<T>
    {
        public T Data { get; set; } 
        public bool Succes { get; set; } = true;
        public string Message { get; set; } = null;

    }
}