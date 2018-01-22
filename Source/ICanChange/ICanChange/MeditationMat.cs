using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;

namespace ICanChange
{
    public class MeditationMat : Building, IAssignableBuilding
    {
        public List<Pawn> owners = new List<Pawn>();
        public int MeditationSlotsCount = 1; // move to XML
        public MeditationMatProperties matProperties;
        internal bool AnyUnoccupiedSlot;

        public IEnumerable<Pawn> AssigningCandidates
        {
            get
            {
                if (!base.Spawned)
                {
                    return Enumerable.Empty<Pawn>();
                }
                return base.Map.mapPawns.FreeColonists;
            }
        }

        public IEnumerable<Pawn> AssignedPawns
        {
            get
            {
                return this.owners;
            }
        }

        public int MaxAssignedPawnsCount
        {
            get
            {
                return 1; // only one owner allowed per mat. Look into a better place to store this.
            }
        }

        public void TryAssignPawn(Pawn owner)
        {
            ((Pawn_Mat_Ownership)owner.ownership).ClaimMat(this);
        }

        public void TryUnassignPawn(Pawn pawn)
        {
            if (this.owners.Contains(pawn))
            {
                ((Pawn_Mat_Ownership)pawn.ownership).UnclaimMat();
            }
        }

        public override void DeSpawn()
        {
            this.RemoveAllOwners();
            base.DeSpawn();
        }

        private void RemoveAllOwners()
        {
            for (int i = this.owners.Count - 1; i >= 0; i--)
            {
                ((Pawn_Mat_Ownership)this.owners[i].ownership).UnclaimMat();
            }
        }

        public override string GetInspectString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(base.GetInspectString());
            stringBuilder.AppendLine();

            if (this.owners.Count == 0)
            {
                stringBuilder.AppendLine("Owner: nobody");
            }
            else if (this.owners.Count == 1)
            {
                stringBuilder.AppendLine("Owner: " + this.owners[0].Label);
            }
            else
            {
                stringBuilder.Append("Owners: ");
                bool flag = false;
                for (int i = 0; i < this.owners.Count; i++)
                {
                    if (flag)
                    {
                        stringBuilder.Append(", ");
                    }
                    flag = true;
                    stringBuilder.Append(this.owners[i].LabelShort);
                }
                stringBuilder.AppendLine();
            }
            return stringBuilder.ToString().TrimEndNewlines();
        }

        public override IEnumerable<FloatMenuOption> GetFloatMenuOption(Pawn myPawn)
        {
            if (myPawn.RaceProps.Humanlike && !myPawn.Drafted && base.Faction == Faction.OfPlayer && MeditateUtility.CanUseMatEver(myPawn, this.def))
            {

            }
        }

        public Pawn getCurOccupant(int slotIndex)
        {
            if (!base.Spawned)
            {
                return null;
            }
            IntVec3 meditatingSlotPos = this.GetMeditatingSlotPos(slotIndex);
            List<Thing> list = base.Map.thingGrid.ThingsListAt(meditatingSlotPos);
            for (int i = 0; i < list.Count; i++)
            {
                Pawn pawn = list[i] as Pawn;
                if (pawn != null)
                {
                    if (pawn.CurJob != null)
                    {
                        // somehow need to check if the pawn is meditating?
                        return pawn;
                    }
                }
            }
            return null;
        }

        public IntVec3 GetMeditatingSlotPos(int index)
        {
            return MeditateUtility.GetMeditatingSlotPos(index, base.Position, base.Rotation, this.def.size);
        }
    }
}
