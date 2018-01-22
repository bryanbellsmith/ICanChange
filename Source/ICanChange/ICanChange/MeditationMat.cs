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
            
        }

        public void TryUnassignPawn(Pawn pawn)
        {
            throw new NotImplementedException();
        }
    }
}
