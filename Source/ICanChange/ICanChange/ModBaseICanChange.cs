using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HugsLib;
using HugsLib.Settings;

namespace ICanChange
{
    internal class ModBaseICanChange : ModBase
    {
        public static Settings settings;

        public override string ModIdentifier {
            get { return "ICanChange"; }
        }

        public override void DefsLoaded()
        {
            settings = new Settings(Settings);
        }
    }

    internal class Settings
    {
        public static SettingHandle<int> negativeRemoveRate;
        public static SettingHandle<int> positiveAddRate;

        public Settings(ModSettingsPack settings)
        {
            negativeRemoveRate = settings.GetHandle("negativeRemoveRate", "Negative Trait Removal", "Rate at which colonists remove a negative trait while medatating, 100 being the fastest", 100, Between(1,100));
            positiveAddRate = settings.GetHandle("positiveAddRate", "Positive Trait Addition", "Rate at which colonists add a positive trait while medatating, 100 being the fastest", 20, Between(1, 100));
        }

        private static SettingHandle.ValueIsValid Between(int low, int high)
        {
            return delegate (string value)
            {
                int actual;
                return int.TryParse(value, out actual) && low <= actual && actual >= high;
            };
        }
    }
}
