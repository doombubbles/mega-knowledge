using System;
using System.IO;
using System.Linq;
using BTD_Mod_Helper;
using BTD_Mod_Helper.Api.Commands;
using HarmonyLib;
using Il2CppAssets.Scripts.Models.TowerSets;

namespace MegaKnowledge;

internal class GenerateReadme : ModCommand<GenerateCommand>
{
    public override bool Execute(ref string resultText)
    {
        try
        {
            Generate();
            return true;
        }
        catch (Exception e)
        {
            resultText = e.Message;
            ModHelper.Error<MegaKnowledgeMod>(e);
        }

        return false;
    }

    public override string Command => "megaknowledgemds";

    public override string Help => "Generate Mega Knowledge readmes";

    private const string ReadMe = "README.md";

    private static string FilePath =>
        Path.Combine(ModHelper.ModSourcesDirectory, nameof(MegaKnowledge), "MegaKnowledges");

    private static readonly TowerSet[] TowerSets =
        [TowerSet.Primary, TowerSet.Military, TowerSet.Magic, TowerSet.Support];

    public static void Generate()
    {
        var total = "# All Mega Knowledge Effects";

        foreach (var towerSet in TowerSets)
        {
            var text = GenerateCategory(towerSet);
            SaveMd(text, Path.Combine(towerSet.ToString(), ReadMe));
            total += text;
        }

        SaveMd(total, ReadMe);
    }

    private static void SaveMd(string text, string path) => File.WriteAllText(Path.Combine(FilePath, path), text);

    private static string GenerateCategory(TowerSet category) =>
        $"""
         <h2>{category}</h2>

         <table>
         {GetContent<MegaKnowledge>().Where(knowledge => knowledge.TowerSet == category).OrderBy(knowledge => knowledge.Offset).Select(GenerateEntry).Join(delimiter: "\n")}
         </table>
         """;


    private static string GenerateEntry(MegaKnowledge knowledge) =>
        $"""
             <tr>
                 <td width='15%' align='center'>
                     <img alt='{knowledge.DisplayName}' src='/MegaKnowledges/{knowledge.TowerSet}/{knowledge.Name}.png'>
                 </td>
                 <td align='center'>
                     <h2>{knowledge.DisplayName}</h2>
                 </td>
                 <td>
                     {knowledge.Description}
                 </td>
             </tr>
         """;
}