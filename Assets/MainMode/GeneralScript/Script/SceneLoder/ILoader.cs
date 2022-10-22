using System.Threading.Tasks;

namespace MainMode.LoadScene
{
    public interface ILoader
    {
        public Task LoadAsync();
        public void Unload();
    }
}