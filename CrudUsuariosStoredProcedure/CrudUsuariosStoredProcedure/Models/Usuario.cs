using System.ComponentModel.DataAnnotations;

namespace CrudUsuariosStoredProcedure.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Digite o nome!")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Digite o sobrenome !")]
        public string Sobrenome { get; set; }

        [Required(ErrorMessage = "Digite o email"),
            EmailAddress(ErrorMessage = "Email incorreto!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Digite o cargo!")]
        public string Cargo { get; set; }


    }
}
