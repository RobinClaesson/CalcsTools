using System.CommandLine;
using CalcTools.DataBuilder.Commands;

var rootCommand = new RootCommand()
{
    new MergeSpeciesCommand()
};

await rootCommand.Parse(args).InvokeAsync();