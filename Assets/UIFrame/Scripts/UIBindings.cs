using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;

public class UIBindings : MonoBehaviour
{
    [Serializable]
    public class Binding
    {
        public string Id;
        public GameObject GameObject;
        public Component Component;
    }

    public Binding[] bindings;
    private Dictionary<string, GameObject> gameObjects = new Dictionary<string, GameObject>();
    private Dictionary<string, Component> components = new Dictionary<string, Component>();

    private void Awake()
    {
        foreach (var binding in bindings)
        {
            if (binding.GameObject != null)
                gameObjects[binding.Id] = binding.GameObject;
            if (binding.Component != null)
                components[binding.Id] = binding.Component;
        }
    }

    public T Get<T>(string id) where T : Component
    {
        if (components.TryGetValue(id, out Component component))
        {
            return component as T;
        }
        return null;
    }
} 