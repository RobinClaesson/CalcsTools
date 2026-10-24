namespace CalcTools.Core;

public static class GamesInfo
{
    public static IReadOnlyDictionary<int, string> GetTmMoves(int generation) => generation switch
    {
        3 => Gen3TmMoves,
        _ => new Dictionary<int, string>()
    };

    public static IReadOnlyDictionary<int, string> GetHmMoves(int generation) => generation switch
    {
        3 => Gen3HmMoves,
        _ => new Dictionary<int, string>()
    };
    
    private static readonly Dictionary<int, string> Gen3TmMoves = new()
    {
        { 1, "Focus Punch" }, { 2, "Dragon Claw" }, { 3, "Water Pulse" }, { 4, "Calm Mind" }, { 5, "Roar" },
        { 6, "Toxic" }, { 7, "Hail" }, { 8, "Bulk Up" }, { 9, "Bullet Seed" }, { 10, "Hidden Power" },
        { 11, "Sunny Day" }, { 12, "Taunt" }, { 13, "Ice Beam" }, { 14, "Blizzard" }, { 15, "Hyper Beam" },
        { 16, "Light Screen" }, { 17, "Protect" }, { 18, "Rain Dance" }, { 19, "Giga Drain" }, { 20, "Safeguard" },
        { 21, "Frustration" }, { 22, "Solar Beam" }, { 23, "Iron Tail" }, { 24, "Thunderbolt" }, { 25, "Thunder" },
        { 26, "Earthquake" }, { 27, "Return" }, { 28, "Dig" }, { 29, "Psychic" }, { 30, "Shadow Ball" },
        { 31, "Brick Break" }, { 32, "Double Team" }, { 33, "Reflect" }, { 34, "Shock Wave" }, { 35, "Flamethrower" },
        { 36, "Sludge Bomb" }, { 37, "Sandstorm" }, { 38, "Fire Blast" }, { 39, "Rock Tomb" }, { 40, "Aerial Ace" },
        { 41, "Torment" }, { 42, "Facade" }, { 43, "Secret Power" }, { 44, "Rest" }, { 45, "Attract" }, { 46, "Thief" },
        { 47, "Steel Wing" }, { 48, "Skill Swap" }, { 49, "Snatch" }, { 50, "Overheat" }
    };

    private static readonly Dictionary<int, string> Gen3HmMoves = new()
    {
        { 1, "Cut" }, { 2, "Fly" }, { 3, "Surf" }, { 4, "Strength" },
        { 5, "Flash" }, { 6, "Rock Smash" }, { 7, "Waterfall" }, { 8, "Dive" }
    };
}
