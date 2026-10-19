using System.Text.Json.Serialization;
using CalcTools.Core.HzlaCalc.Json;

namespace CalcTools.Core.HzlaCalc;

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
    Unkown
}