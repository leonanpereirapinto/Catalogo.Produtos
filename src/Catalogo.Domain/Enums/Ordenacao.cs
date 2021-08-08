using System.Runtime.Serialization;

namespace Catalogo.Domain.Enums
{
    public enum Ordenacao
    {
        [EnumMember(Value = "asc")]
        Asc,

        [EnumMember(Value = "desc")]
        Desc
    }
}