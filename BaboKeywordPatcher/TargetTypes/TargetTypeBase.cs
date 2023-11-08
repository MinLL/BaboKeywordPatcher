using Mutagen.Bethesda;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Synthesis;

namespace BaboKeywordPatcher.TargetTypes
{
    public class TargetTypeData
    {
        public IPatcherState<ISkyrimMod, ISkyrimModGetter>? State;
        public FormKey HighHeelSoundFormKey;
    }

    public abstract class TargetTypeBase
    {
        protected TargetTypeData? Data;
        bool IsValid = false;

        public bool SetIsValid(IPatcherState<ISkyrimMod, ISkyrimModGetter> state) { return IsValid = IsValidType(state); }

        protected virtual bool IsValidType(IPatcherState<ISkyrimMod, ISkyrimModGetter> state) { return true; }

        public void SetInputData(TargetTypeData data) { Data = data; }

        // These are just globals with extra steps.
        // They are only valid for the duration of a call to IsFound.
        // OO was mistake, lol.
        protected IArmorAddonGetter? ArmorAddon;
        protected IArmorGetter? Armor;

        protected abstract string Name { get; }
        public bool IsFound(IArmorGetter? armor, IArmorAddonGetter? arma)
        {
            if (!IsValid) return false;

            if (armor == null) return false;
            Armor = armor;

            if (!IsValidArmor() 
                || Armor.Armature == null 
                || (Armor.TemplateArmor!=null && !Armor.TemplateArmor.IsNull)) // all templated have same armor addon
                return false;

            ArmorAddon = arma;

            if (ArmorAddon == null || !IsValidArmorAddon()) return false;

            return true;
        }

        protected abstract bool IsValidArmor();
        protected abstract bool IsValidArmorAddon(); 
    }
}
