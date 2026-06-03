using System.Collections;
using BTD_Mod_Helper.Api.Testing;

namespace MegaKnowledge;

public class MegaKnowledgeTest : ModTest
{

    public override IEnumerator Test()
    {
        yield return EnsureOnMainMenuWithNoPopUps();

        MegaKnowledge.overrideEnabled = true;

        yield return LoadIntoGame();

        yield return null;

        yield return EnsureOnMainMenuWithNoPopUps();

        MegaKnowledge.overrideEnabled = null;
    }
}