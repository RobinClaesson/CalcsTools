using System.Text.Json.Serialization;
using CalcTools.Core.BaseData.Json;

namespace CalcTools.Core.BaseData;

[JsonConverter(typeof(JsonPokemonTypeConverter))]
public enum PokemonType
{
    Normal,
    Fighting,
    Flying,
    Poison,
    Ground,
    Rock,
    Bug,
    Ghost,
    Steel,
    Fire,
    Water,
    Grass,
    Electric,
    Psychic,
    Ice,
    Dragon,
    Dark,
    Fairy,
    Stellar,
    Unknown
}