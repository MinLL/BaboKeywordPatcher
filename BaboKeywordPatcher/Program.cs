using System;
using System.Collections.Generic;
using System.Linq;
using Mutagen.Bethesda;
using Mutagen.Bethesda.Synthesis;
using Mutagen.Bethesda.Skyrim;
using System.Threading.Tasks;

namespace BaboKeywordPatcher
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            return await SynthesisPipeline.Instance
                .AddPatch<ISkyrimMod, ISkyrimModGetter>(RunPatch)
                .SetTypicalOpen(GameRelease.SkyrimSE, "BaboKeywords.esp")
                .Run(args);
        }

        public static IKeywordGetter LoadKeyword(IPatcherState<ISkyrimMod, ISkyrimModGetter> state, String kwd)
        {
            state.LinkCache.TryResolve<IKeywordGetter>(kwd, out var ReturnKwd);
            if (ReturnKwd == null)
            {
                throw new Exception("Failed to load keyword " + kwd);
            }
            return ReturnKwd;
        }

        public static bool StrMatch(String name, String comparator)
        {
            return (name.IndexOf(comparator, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        public static bool IsDeviousRenderedItem(String name)
        {
            return (StrMatch(name, "scriptinstance") || StrMatch(name, "rendered"));
        }

        public static IKeywordGetter? EroticArmor;
        public static IKeywordGetter? SLA_ArmorHarness;
        public static IKeywordGetter? SLA_ArmorSpendex;
        public static IKeywordGetter? SLA_ArmorTransparent;
        public static IKeywordGetter? SLA_BootsHeels;
        public static IKeywordGetter? SLA_VaginalDildo;
        public static IKeywordGetter? SLA_AnalPlug;
        public static IKeywordGetter? SLA_PiercingClit;
        public static IKeywordGetter? SLA_PiercingNipple;
        public static IKeywordGetter? SLA_ArmorPretty;

        public static void LoadKeywords(IPatcherState<ISkyrimMod, ISkyrimModGetter> state)
        {
            SLA_ArmorHarness = LoadKeyword(state, "SLA_ArmorHarness");
            try // SLAX vs SLA Babo spell this keyword differently. Check for both.
            {
                SLA_ArmorSpendex = LoadKeyword(state, "SLA_ArmorSpendex");
            }
            catch
            {
                SLA_ArmorSpendex = LoadKeyword(state, "SLA_ArmorSpandex");
            }
            SLA_ArmorTransparent = LoadKeyword(state, "SLA_ArmorTransparent");
            SLA_BootsHeels = LoadKeyword(state, "SLA_BootsHeels");
            SLA_VaginalDildo = LoadKeyword(state, "SLA_VaginalDildo");
            SLA_AnalPlug = LoadKeyword(state, "SLA_AnalPlug");
            SLA_PiercingClit = LoadKeyword(state, "SLA_PiercingClit");
            SLA_PiercingNipple = LoadKeyword(state, "SLA_PiercingNipple");
            SLA_ArmorPretty = LoadKeyword(state, "SLA_ArmorPretty");
            EroticArmor = LoadKeyword(state, "EroticArmor");
        }

        private static void AddTag(Armor AEO, IKeywordGetter tag)
        {
            System.Console.WriteLine("Added keyword " + tag.ToString() + " to armor " + AEO.Name);
            if (AEO.Keywords == null)
            {
                System.Console.WriteLine("AOE.Keywords == null: " + AEO);
                // AEO.Keywords!.Add(tag);
            }
            else
            {
                if (!AEO.Keywords.Contains(tag))
                {
                    AEO.Keywords!.Add(tag);
                }
            }
        }

        // Keywords are static / nullabe, but are initialized on runtime. Ignore warning.
        #pragma warning disable CS8604 // Possible null reference argument.
        public static void ParseName(IPatcherState<ISkyrimMod, ISkyrimModGetter> state, IArmorGetter armor, String name)
        {
            bool matched = false;
            var armorEditObj = state.PatchMod.Armors.GetOrAddAsOverride(armor);
            if (armorEditObj == null )
            {
                System.Console.WriteLine("Armor is null for " + name);
                return;
            }
            // EroticArmor
            if (StrMatch(name, "harness") || StrMatch(name, "corset") || StrMatch(name, "StraitJacket") || StrMatch(name, "suit") ||
                StrMatch(name, "HobbleDress") || StrMatch(name, "tentacles") || StrMatch(name, "dress") || StrMatch(name, "latex") ||
                StrMatch(name, "slave") || StrMatch(name, "chastity") || StrMatch(name, "cuff") || StrMatch(name, "binder")
                )
            {
                matched = true;
                AddTag(armorEditObj, EroticArmor);
            }
            //SLA_ArmorHarness
            if (StrMatch(name, "harness"))
            {
                matched = true;
                AddTag(armorEditObj, SLA_ArmorHarness);
            }
            // SLA_ArmorSpendex
            if (StrMatch(name, "suit") || StrMatch(name, "spandex") || StrMatch(name, "spendex"))
            {
                matched = true;
                AddTag(armorEditObj, SLA_ArmorSpendex);
            }
            // SLA_ArmorTransparent
            if (StrMatch(name, "transparent"))
            {
                matched = true;
                AddTag(armorEditObj, SLA_ArmorTransparent);
            }
            // SLA_BootsHeels
            if ((IsDeviousRenderedItem(name) && StrMatch(name, "boots")) || StrMatch(name, "heels"))
            {
                matched = true;
                AddTag(armorEditObj, SLA_BootsHeels);
            }
            //SLA_VaginalDildo
            if ((StrMatch(name, "plug") && StrMatch(name, "vag")) || StrMatch(name, "vaginal") || StrMatch(name, "vibrator"))
            {
                matched = true;
                AddTag(armorEditObj, SLA_VaginalDildo);
            }
            // SLA_AnalPlug
            if (StrMatch(name, "anal") || StrMatch(name, "buttplug") || StrMatch(name, "vibrator"))
            {
                matched = true;
                AddTag(armorEditObj, SLA_AnalPlug);
            }
            // SLA_PiercingClit
            if (StrMatch(name, "piercingv") || StrMatch(name, "vpiercing"))
            {
                matched = true;
                AddTag(armorEditObj, SLA_PiercingClit);
            }
            // SLA_PiercingNipple
            if (StrMatch(name, "piercingn") || StrMatch(name, "npiercing"))
            {
                matched = true;
                AddTag(armorEditObj, SLA_PiercingNipple);
            }
            //SLA_ArmorPretty
            if (!matched && (StrMatch(name, "armor") || StrMatch(name, "cuiras") || StrMatch(name, "robes")))
            { // I use a skimpy armor replacer (But not to the level of bikini). Having ArmorPretty on all armors is appropriate.
                matched = true;
                AddTag(armorEditObj, SLA_ArmorPretty);
            }
            if (matched)
            {
                // System.Console.WriteLine("Matched: " + name);
            }
        }
        #pragma warning restore CS8604 // Possible null reference argument.

        public static void RunPatch(IPatcherState<ISkyrimMod, ISkyrimModGetter> state)
        {
            //Your code here!
            LoadKeywords(state);
            foreach (var armorGetter in state.LoadOrder.PriorityOrder.WinningOverrides<IArmorGetter>())
            {
                try 
                {
                    if (armorGetter.Name != null)
                    {
                        string? v = armorGetter.Name.ToString();
                        if (v != null)
                        {
                            ParseName(state, armorGetter, v);
                        }
                    }
                    else
                    {
                        if (armorGetter.EditorID != null)
                        {
                            ParseName(state, armorGetter, armorGetter.EditorID);
                        }
                    }
                }
                // MoreNastyCritters breaks the patching process. Ignore it.
                catch (Exception e)
                {
                    System.Console.WriteLine("Caught exception: " + e);
                }
            }
            System.Console.WriteLine("Done.");
        }
    }
}
