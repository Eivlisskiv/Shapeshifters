using UnityEngine;

namespace Scripts.OOP.EnemyBehaviors.Ability
{
    public abstract class ObjectSpawner<TObject> : IAbilitybehavior
        where TObject : Object
    {
        private readonly string path;

        protected ObjectSpawner(string ressoucePath)
        {
            path = ressoucePath;
        }

        public abstract void Initialize(BaseController self);

        protected virtual bool TryInstantiate(out TObject obj)
        {
            obj = Resources.Load<TObject>(path);
            if (!obj) return false;

            obj = Object.Instantiate(obj);
            if (!obj) return false;

            return true;
        }

    }

    public abstract class GameObjectSpawner<TComponent> : ObjectSpawner<GameObject>
        where TComponent : Component
    {
        protected GameObjectSpawner(string ressoucePath) : base(ressoucePath) { }

        protected virtual bool TryInstantiate(out GameObject obj, out TComponent component)
        {
            if(!base.TryInstantiate(out obj))
            {
                component = null;
                return false;
            }

            return obj.TryGetComponent(out component);
        }
    }
}
