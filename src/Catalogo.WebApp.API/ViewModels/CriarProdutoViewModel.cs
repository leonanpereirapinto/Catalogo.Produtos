using System.ComponentModel.DataAnnotations;

namespace WebApp.API.ViewModels
{
    public class CriarProdutoViewModel
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Nome { get; set; }

        public int Estoque { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Range(0, double.MaxValue, ErrorMessage = "A Valor do produto não pode ser negativo")]
        public decimal Valor { get; set; }
    }
}