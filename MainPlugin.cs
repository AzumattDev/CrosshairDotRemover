using BepInEx;
using HarmonyLib;

namespace CrosshairDotRemover
{
    [BepInPlugin(ModGUID, ModName, ModVersion)]
    public class CrosshairDotRemoverPlugin : BaseUnityPlugin

    {
        internal const string ModName = "CrosshairDotRemover";
        internal const string ModVersion = "1.0.0";
        internal const string Author = "Azumatt";
        private const string ModGUID = Author + "." + ModName;
        private readonly Harmony _harmony = new(ModGUID);

        private void Awake()
        {
            _harmony.PatchAll();
        }
    }
}