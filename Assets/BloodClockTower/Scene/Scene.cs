    using System;
    using UnityEngine.SceneManagement;
    using UObject = UnityEngine.Object;

    public class Scene<T> where T : UObject
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
                    throw new ArgumentNullException(nameof(context), "Impossible to find scene context");
                return context;
            });
        }

        public void Load()
        {
            SceneManager.LoadScene(_name);
        }
    }
