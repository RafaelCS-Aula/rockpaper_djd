using System.Collections.Generic;


namespace RPS_DJDIII.Assets.Scripts.ScoreKeeping
{
    public class EntryComparer : IComparer<HallFameEntry>
    {
        public int Compare(HallFameEntry entry1, HallFameEntry entry2)
        {
            return entry2.KDRatio.CompareTo(entry1.KDRatio);
        }
    }

}

