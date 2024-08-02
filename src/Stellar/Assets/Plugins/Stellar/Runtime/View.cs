using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Plugins.Stellar.Runtime
{
    public abstract class View : MonoBehaviour
    {
        [SerializeField] protected Canvas Canvas;
        [SerializeField] protected CanvasScaler CanvasScaler;
        
        private void OnValidate()
        {
            Canvas = GetComponent<Canvas>();
            CanvasScaler = GetComponent<CanvasScaler>();

            Type type = GetType();
            FieldInfo[] fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            var fieldDictionary = fields
                .Where(fieldInfo => fieldInfo.IsPrivate && fieldInfo.GetValue(this) == null)
                .ToDictionary(fieldInfo =>
            {
                var fieldName = fieldInfo.Name;
                if (fieldInfo.IsPrivate)
                {
                    fieldName = fieldName.TrimStart('_');
                }
                return fieldName.ToLower();
            });

            if (fieldDictionary.Count > 0)
            {
                var childTransforms = transform.GetComponentsInChildren<Transform>();

                bool fieldsUpdated = false;
            
                foreach (var child in childTransforms)
                {
                    var childNameLower = child.name.ToLower();
                    if (fieldDictionary.TryGetValue(childNameLower, out var fieldInfo))
                    {
                        if (child.TryGetComponent(fieldInfo.FieldType, out var component))
                        {
                            fieldInfo.SetValue(this, component);
                            fieldsUpdated = true;
                        }
                    }
                }

                if (fieldsUpdated)
                {
                    EditorUtility.SetDirty(this);
                }
            }
        }

        public virtual async UniTask OpenAsync<TViewModel>(TViewModel viewModel)
        {
            await UniTask.Delay(1000);
            Canvas.enabled = true;
        }

        public virtual async UniTask CloseAsync()
        {
            await UniTask.Delay(1000);
            Canvas.enabled = false;
        }
    }
}