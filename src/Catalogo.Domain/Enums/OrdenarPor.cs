using System.Runtime.Serialization;

namespace Catalogo.Domain.Enums
{
    public enum OrdenarPor
    {
        [EnumMember(Value = "nome")]
        Nome,

        [EnumMember(Value = "estoque")]
        Estoque,

        [EnumMember(Value = "valor")]
        Valor
    }
}