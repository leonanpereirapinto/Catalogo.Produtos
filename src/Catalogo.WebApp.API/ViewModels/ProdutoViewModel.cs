using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace WebApp.API.ViewModels
{
    public class ProdutoViewModel
    {
        [Key]
        [NotNull]
        public Guid? Id { get; set; }

        [NotNull]
        public string Nome { get; set; }

        public int Estoque { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "A Valor do produto não pode ser negativo")]
        public decimal Valor { get; set; }
    }
}