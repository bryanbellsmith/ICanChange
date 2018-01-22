using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using Verse.AI;
using UnityEngine;

namespace ICanChange
{
    public static class MeditateUtility
    {
        private static int meditationMatSlotsCount = 1; // load from XML

        /*
        private static List<ThingDef> matDefsBestToWorst;

        public static List<ThingDef> AllMatDefBestToWorst
        {
            get
            {
                return MeditateUtility.matDefsBestToWorst;
            }
        }

        public static void Reset()
        {
            MeditateUtility.matDefsBestToWorst = (from d in DefDatabase<ThingDef>.AllDefs
                                                  where d.isMeditationMat
                                                  orderby)
        }*/

        public static bool IsValidMatFor(Thing matThing, Pawn meditator, Pawn traveller, bool meditaterWillBePrisoner, bool checkSocialProperness, bool ignoreOtherReserverations = false)
        {
            MeditationMat meditationMat = matThing as MeditationMat;
            if (meditationMat == null)
            {
                return false;
            }
            LocalTargetInfo target = meditationMat;
            PathEndMode peMode = PathEndMode.OnCell;
            Danger maxDanger = Danger.Some;
            int mediateSlotsCount = 1;
            if (!traveller.CanReserveAndReach(target, peMode, maxDanger, mediateSlotsCount, -1, null, ignoreOtherReserverations))
            {
                return false;
            }
            if (!MeditateUtility.CanUseMatEver(meditator, meditationMat.def))
            {
                return false;
            }
            if (!meditationMat.AnyUnoccupiedSlot && (!meditator.OnMat() || meditator.CurrentMat() != meditationMat))
        }

        public static bool CanUseMatEver(Pawn p, ThingDef matDef)
        {
            return p.BodySize <= ((MeditationMatProperties)matDef.building).mat_maxBodySize && p.RaceProps.Humanlike == ((MeditationMatProperties)matDef.building).mat_humanlike;
        }

        public static bool OnMat(this Pawn p)
        {
            return p.CurrentMat() != null;
        }

        public static MeditationMat CurrentMat(this Pawn p)
        {
            if (!p.Spawned || p.CurJob == null)
            {
                return null;

            }
            MeditationMat meditationMat = null;
            List<Thing> thingList = p.Position.GetThingList(p.Map);
            for (int i = 0; i < thingList.Count; i++)
            {
                meditationMat = (thingList[i] as MeditationMat);
                if (meditationMat != null)
                {
                    break;
                }
            }
            if (meditationMat == null)
            {
                return null;
            }
            for (int j = 0; j < meditationMat.MeditationSlotsCount; j++)
            {
                if (meditationMat.GetCurOccupant(j) == p)
                {
                    return meditationMat;
                }
            }
        }

        public static IntVec3 GetMeditatingSlotPos (int index, IntVec3 matCenter, Rot4 matRot, IntVec2 matSize)
        {

            if (index < 0 || index >= meditationMatSlotsCount)
            {
                Log.Error(string.Concat(new object[]
                {
                    "Tried to get meditation mat slot pos with index ",
                    index,
                    ", but there are only ",
                    meditationMatSlotsCount,
                    " meditating slots available."
                }));
                return matCenter;
            }
            return new IntVec3(matCenter.x, matCenter.y, matCenter.z);
        }
    }
}
