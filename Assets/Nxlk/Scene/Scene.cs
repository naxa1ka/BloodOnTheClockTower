using System;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using UObject = UnityEngine.Object;

namespace Nxlk.Scene
{
    public class Scene<T>
        where T : UObject
    {
        private readonly string _name;
        private readonly Lazy<T> _context;

        public T Context => _context.Value;

        public Scene(string name)
        {
            _name = name;
            _context = new Lazy<T>(() =>
            {
                var context = UObject.FindObjectOfType<T>();
                if (context == null)
                    throw new ArgumentNullException(
                        nameof(context),
                        "Impossible to find scene context"
                    );
                return context;
            });
        }

        public async UniTask Load() => await SceneManager.LoadSceneAsync(_name);
    }
}
