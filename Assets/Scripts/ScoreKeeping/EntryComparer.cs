using System.Collections.Generic;


namespace RPS_DJDIII.Assets.Scripts.ScoreKeeping
{
    public class EntryComparer : IComparer<HallOfFameEntry>
    {
        public int Compare(HallOfFameEntry entry1, HallOfFameEntry entry2)
        {
            return entry2.KDRatio.CompareTo(entry1.KDRatio);
        }
    }

}

