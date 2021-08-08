using System;
using System.ComponentModel.DataAnnotations;

namespace WebApp.API.ViewModels
{
    public class ProdutoViewModel
    {
        [Key]
        public Guid? Id { get; set; }

        public string Nome { get; set; }

        public int Estoque { get; set; }

        public decimal Valor { get; set; }
    }
}