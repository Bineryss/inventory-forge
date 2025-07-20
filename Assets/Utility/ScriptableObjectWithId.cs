using UnityEngine;

namespace Utility
{
    public abstract class ScriptableObjectWithId : ScriptableObject
    {
        [SerializeField] protected string id = System.Guid.NewGuid().ToString();

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(id))
            {
                id = System.Guid.NewGuid().ToString();
                UnityEditor.EditorUtility.SetDirty(this);
            }
        }
    }
}