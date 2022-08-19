namespace Underworld
{
    public interface ITileMode
    {
        public bool Activate(FireState state = FireState.Start, TileInfo timeActive = null);
        public void Deactivate();
    }
}