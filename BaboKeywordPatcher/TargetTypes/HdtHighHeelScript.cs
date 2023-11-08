using Mutagen.Bethesda;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Synthesis;

namespace BaboKeywordPatcher.TargetTypes
{
    internal class HdtHighHeelScript : TargetTypeBase
    {
        protected override string Name => "HdtHighHeel reader";

        protected override bool IsValidType(IPatcherState<ISkyrimMod, ISkyrimModGetter> state)
        {
            if (state.LoadOrder.ContainsKey("hdtHighHeel.esm")) return true;

            Console.WriteLine("'hdtHighHeel.esm' doeasnt exist! Exit..");
            return false;            
        }

        protected override bool IsValidArmor()
        {
            // search if script added and valid
            if (Armor!.VirtualMachineAdapter == null) return false;
            if (Armor.VirtualMachineAdapter.Scripts == null) return false;
            if (Armor.VirtualMachineAdapter.Scripts.Count == 0) return false;
            var hhScriptEntrieGetter = Armor.VirtualMachineAdapter.Scripts.FirstOrDefault(sc => string.Equals(sc.Name, "hdthighheelshoes", StringComparison.InvariantCultureIgnoreCase));
            if (hhScriptEntrieGetter == default) return false;
            if (Program.Settings.Value.MinOffsetValue <= 0) return true; // dont need to check effect offset when it 0

            // check effect offset
            if (hhScriptEntrieGetter.Properties.Count != 1) return false; // must be 1 hh spell propety

            var scriptProperty = hhScriptEntrieGetter.Properties[0];
            if (scriptProperty is not ScriptObjectProperty objectProperty) return false;

            var spellFormLink = objectProperty.Object;
            if (!objectProperty.Object.TryResolve<ISpellGetter>(Program.LinkCache!, out var spellGetter)) return false;
            if (spellGetter.Effects.Count != 1) return false; // must be one effect, maybe will change later

            var offsetEffect = spellGetter.Effects[0];
            if (offsetEffect.Data == null) return false; // effect data not set

            if(offsetEffect.Data.Magnitude < Program.Settings.Value.MinOffsetValue) return false;

            return true;
        }

        protected override bool IsValidArmorAddon() { return true; }
    }
}
