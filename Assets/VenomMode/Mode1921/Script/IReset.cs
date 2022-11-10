using MainMode.LoadScene;

namespace MainMode
{
    public interface IReset
    {
        public void Restart(PlayerConfig config = null);
    }
}