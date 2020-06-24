using rockpaper_djd;
using System.Collections.Generic;

public class EntryComparer : IComparer<HallofFameEntry>
{
    public int Compare(HallofFameEntry entry1, HallofFameEntry entry2)
    {
        return entry2.KDRatio.CompareTo(entry1.KDRatio);
    }
}
