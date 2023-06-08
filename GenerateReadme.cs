#if DEBUG
using System.IO;
using System.Linq;
using BTD_Mod_Helper;
using BTD_Mod_Helper.Api;
using HarmonyLib;
using Il2CppAssets.Scripts.Models.TowerSets;

namespace MegaKnowledge;

internal static class GenerateReadme
{
    private const string ReadMe = "README.md";

    private static string FilePath =>
        Path.Combine(ModHelper.ModSourcesDirectory, nameof(MegaKnowledge), "MegaKnowledges");

    private static readonly TowerSet[] TowerSets = {TowerSet.Primary, TowerSet.Military, TowerSet.Magic, TowerSet.Support};

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
        $@"
<h2>{category}</h2>

<table>{ModContent.GetContent<MegaKnowledge>().Where(knowledge => knowledge.TowerSet == category).OrderBy(knowledge => knowledge.Offset).Select(GenerateEntry).Join(delimiter: "")}
</table>
        ";


    private static string GenerateEntry(MegaKnowledge knowledge) =>
        $@"
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
    </tr>";
}

#endif