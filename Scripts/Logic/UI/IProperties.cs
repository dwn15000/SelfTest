using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Property
{
    public string name;
    public GameObject value;
}

public class IProperties : MonoBehaviour
{
    public Property[] _properties;

    private Dictionary<string, Property> _propMap;

    public GameObject GetProperty(string name)
    {
        if (_properties == null)
        {
            return null;
        }

        if (_propMap == null)
        {
            _propMap = new Dictionary<string, Property>();
        }

        Property p = null;

        if (_propMap.TryGetValue(name, out p))
        {
            
            return p.value;
        }

        for (int i = 0; i < _properties.Length; i++)
        {
            p = _properties[i];
            if (p.name.Equals(name))
            {
                _propMap.Add(name, p);
                return p.value;
            }
        }
        return null;
    }

}
