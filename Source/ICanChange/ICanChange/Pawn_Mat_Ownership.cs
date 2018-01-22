using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using ICanChange;

namespace ICanChange
{
    public class Pawn_Mat_Ownership : Pawn_Ownership
    {
        private Pawn pawn;

        private MeditationMat intOwnedMat;

        public Pawn_Mat_Ownership (Pawn pawn) : base (pawn)
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
                    // ThoughtUtility.RemovePositiveBedroomThoughts(this.pawn); // from original Verse.Pawn_Ownership, i don't think it's required
                }
            }
        }

        public new void ExposeData()
        {
            base.ExposeData();

            Scribe_References.Look<MeditationMat>(ref this.intOwnedMat, "ownedMat", false);
            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                if (this.OwnedMat != null)
                {
                    this.OwnedMat.owners.Add(this.pawn);
                    // this.OwnedMat.SortOwners(); // from original Verse.Pawn_Ownership, there can only be one owner of a mat
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
            /*if (newMat.owners.Count == newMat.MeditationSlotsCount)
            {
                newMat.owners[newMat.owners.Count - 1].ownership.UnclaimMat();
            }*/ // similar, i don't think this is required
            newMat.owners.Add(this.pawn);
            // newMat.SortOwners(); // i don't think this is required
                        
        }

        public void UnclaimMat()
        {
            if (this.OwnedMat != null)
            {
                this.OwnedMat.owners.Remove(this.pawn);
                this.OwnedMat = null;
            }
        }

        public new void UnclaimAll()
        {
            base.UnclaimAll();
            this.UnclaimMat();
        }
    }
}
