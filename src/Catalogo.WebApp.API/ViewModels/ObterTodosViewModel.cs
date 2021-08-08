using System.ComponentModel.DataAnnotations;

namespace WebApp.API.ViewModels
{
    public class ObterTodosViewModel
    {
        [RegularExpression("(nome|valor|estoque):(asc|desc)")]
        public string OrdenarPor { get; set; }

        public string Nome { get; set; }

        public string Id { get; set; }
    }
}