namespace RPS_DJDIII.Assets.Scripts.Interfaces
{
    public interface ISoundPlayer<T> where T : ISoundHolder
    {
        T audioHandler { get; set; }


    }
}