using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;

namespace ICanChange
{
    internal class Pawn_Mat_Ownership : IExposable
    {
        private Pawn pawn;

        private MeditationMat intOwnedMat;

        public Pawn_Mat_Ownership(Pawn pawn)
        {
            this.pawn = pawn;
        }

        public MeditationMat OwnedMat
        {
            get
            {
                return this.intOwnedMat;
            }
            private set
            {
                if (this.intOwnedMat != value)
                {
                    this.intOwnedMat = value;
                }
            }
        }

        public void ExposeData()
        {
            Scribe_References.Look<MeditationMat>(ref this.intOwnedMat, "ownedMat", false);
            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                if (this.OwnedMat != null)
                {
                    this.OwnedMat.owners.Add(this.pawn);
                   // this.OwnedMat.SortOwners(); // what does this do? do i need it? copied from Verse.PawnOwnership
                }
            }
        }

        public void ClaimMat(MeditationMat newMat)
        {
            if (newMat.owners.Contains(this.pawn))
            {
                return;
            }
            this.UnclaimMat();
            newMat.owners.Add(this.pawn);
            // removed a owner count check that was in Verse.PawnOwnership. mats should only have one owner
            // newMat.SortOwners(); // what does this do? do i need it? copied from Verse.PawnOwnership
            this.OwnedMat = newMat;
        }

        public void UnclaimMat()
        {
            if (this.OwnedMat != null)
            {
                this.OwnedMat.owners.Remove(this.pawn);
                this.OwnedMat = null;
            }
        }

    }

}
