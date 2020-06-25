using System.Collections.Generic;


namespace RPS_DJDIII.Assets.Scripts.ScoreKeeping
{
    public class EntryComparer : IComparer<HallofFameEntry>
    {
        public int Compare(HallofFameEntry entry1, HallofFameEntry entry2)
        {
            return entry2.KDRatio.CompareTo(entry1.KDRatio);
        }
    }

}

