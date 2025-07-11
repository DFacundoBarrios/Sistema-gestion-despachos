using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace CrudMVCApp.Models
{
    public class Direccion
    {
        public int Id { get; set; } 

        [Required(ErrorMessage = "El campo Calle es obligatorio.")]
        [StringLength(150, ErrorMessage = "El Calle no puede tener más de 150 caracteres.")] 
        public string Calle { get; set; }
        [Required(ErrorMessage = "El campo Ciudad es obligatorio.")]
        [MaxLength(20, ErrorMessage = "El Ciudad no puede tener más de 20 caracteres.")]
        [MinLength(3, ErrorMessage = "El Ciudad no puede tener menos 3 caracteres.")]
        public string Ciudad { get; set; }
        public string CodigoPostal { get; set; }
        
        public int PersonaId { get; set; }

        
        [ValidateNever]
        public Persona? Persona { get; set; }

        public Direccion() { }
    }
}
