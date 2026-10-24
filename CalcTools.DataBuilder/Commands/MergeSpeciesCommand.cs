using System.CommandLine;
using System.Text.Json;
using CalcTools.DataBuilder.Js;
using CalcTools.DataBuilder.Models.KingerCalc;

namespace CalcTools.DataBuilder.Commands;

public class MergeSpeciesCommand : Command
{
    public MergeSpeciesCommand() : base("merge-species", "Merge species data from Kingler Champ Calculator")
    {
        var inputOption = new Option<string>("--input", "-i")
        {
            Description = "Path to input file with species data from Kingler Champ Calculator.",
            Required = true
        };

        var outputOption = new Option<string>("--output", "-o")
        {
            Description = "Path to output json file",
            Required = true
        };

        var variablesOption = new Option<string[]>("--variable", "-v")
        {
            Description = "Species patch data variables to merge. Order sensitive",
            Required = true
        };

        Options.Add(inputOption);
        Options.Add(outputOption);
        Options.Add(variablesOption);

        SetAction(async parseResult =>
        {
            var inputFilePath = parseResult.GetRequiredValue(inputOption);
            var outputFilePath = parseResult.GetRequiredValue(outputOption);
            var variableNames = parseResult.GetRequiredValue(variablesOption);

            Console.WriteLine($"Reading input file: {inputFilePath}");
            var inputFileContent = await File.ReadAllTextAsync(inputFilePath);

            Console.WriteLine($"Reading variables: {string.Join(", ", variableNames)}");
            var variables = JsParser.GetJsVariables(inputFileContent)
                .Where(v => variableNames.Contains(v.Name, StringComparer.InvariantCultureIgnoreCase))
                .ToList();

            Console.WriteLine($"Found {variables.Count} variables with name {string.Join(", ", variableNames)}");
            if (variables.Count != variableNames.Length)
            {
                Console.WriteLine("There are different number of variables with the same name. Exiting...");
                return;
            }

            Console.WriteLine("Parsing species data");
            var speciesData = variables.Select(v => JsParser.ToJson(v.Value))
                .Select(json => JsonSerializer.Deserialize<Dictionary<string, KinglerCalcSpecies>>(json))
                .Cast<Dictionary<string, KinglerCalcSpecies>>()
                .ToList();

            Console.WriteLine("Merging species data");
            var basedata = speciesData.First();
            var mergedData = speciesData.Skip(1)
                .Aggregate(basedata, ApplySpeciesPatch)
                .ToDictionary(kv => kv.Key, kv => CheckSpecialStats(kv.Value));

            Console.WriteLine($"Saving results to ouput file: {outputFilePath}");
            var json = JsonSerializer.Serialize(mergedData, options: new() { WriteIndented = true });
            await File.WriteAllTextAsync(outputFilePath, json);

            Console.WriteLine("Done!");
        });
    }

    private static Dictionary<string, KinglerCalcSpecies> ApplySpeciesPatch(
        Dictionary<string, KinglerCalcSpecies> data, Dictionary<string, KinglerCalcSpecies> patch)
    {
        var allKeys = data.Keys.Concat(patch.Keys).Distinct();
        return allKeys.ToDictionary(k => k, ApplyPatch);

        KinglerCalcSpecies ApplyPatch(string key)
        {
            var dataSpecies = data.GetValueOrDefault(key);
            var patchSpecies = patch.GetValueOrDefault(key);

            return (dataSpecies, patchSpecies) switch
            {
                (null, not null) => patchSpecies,
                (not null, null) => dataSpecies,
                _ => ApplySpeciesPatch(dataSpecies!, patchSpecies!)
            };
        }
    }

    private static KinglerCalcSpecies ApplySpeciesPatch(KinglerCalcSpecies data, KinglerCalcSpecies patch) => new()
    {
        Types = patch.Types ?? data.Types,
        BaseStats = ApplyCalcPatch(data.BaseStats, patch.BaseStats),
        WeightKg = patch.WeightKg ?? data.WeightKg,
        NotFullyEvolved = patch.NotFullyEvolved ?? data.NotFullyEvolved,
        Gender = patch.Gender ?? data.Gender,
        Abilities = patch.Abilities ?? data.Abilities,
        OtherFormes = patch.OtherFormes ?? data.OtherFormes,
        BaseSpecies = patch.BaseSpecies ?? data.BaseSpecies
    };

    private static KinglerCalcStats? ApplyCalcPatch(KinglerCalcStats? data, KinglerCalcStats? patch)
    {
        if (patch is null)
            return data;
        if (data is null)
            return patch;

        return new KinglerCalcStats
        {
            Hp = patch.Hp ?? data.Hp,
            Attack = patch.Attack ?? data.Attack,
            Defence = patch.Defence ?? data.Defence,
            Speed = patch.Speed ?? data.Speed,
            Special = patch.Special ?? data.Special,
            SpecialAttack = patch.SpecialAttack ?? data.SpecialAttack,
            SpecialDefence = patch.SpecialDefence ?? data.SpecialDefence,
        };
    }

    private static KinglerCalcSpecies CheckSpecialStats(KinglerCalcSpecies species) =>
        species.BaseStats is { SpecialAttack: not null, SpecialDefence: not null, Special: not null }
            ? species with { BaseStats = species.BaseStats with { Special = null } }
            : species;
}