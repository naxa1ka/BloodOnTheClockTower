using System;
using UnityEngine;
using UGObject = UnityEngine.GameObject;
using UObject = UnityEngine.Object;

namespace Nxlk.GameObject
{
    public static class GameObjectExtensions
    {
        public static void SafetyDestroy(this UGObject gameObject)
        {
            try
            {
                UObject.Destroy(gameObject);
            }
            catch (Exception exception)
            {
                if (exception is not MissingReferenceException)
                    throw;
            }
        }
    }
}
