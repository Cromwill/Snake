using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class HatSpawner : MonoBehaviour
{
    [SerializeField] private HatDataBase _dataBase;
    [Tooltip("Если true - спавнится новая шляпка (которой нет в коллекции")]
    [SerializeField] private bool _isNewHat;

    public event UnityAction<SnakeHat, HatData> Spawned;

    private void Start()
    {
        HatCollection collection = new HatCollection(_dataBase);
        collection.Load(new JsonSaveLoad());

        var selectedHat = collection.SelectedHat;
        if (_isNewHat)
            selectedHat = GetNextHat(selectedHat);

        var inst = Instantiate(selectedHat.Prefab, transform.position, Quaternion.identity);
        Spawned?.Invoke(inst, selectedHat);
    }

    private HatData GetNextHat(HatData data)
    {
        var dataBase = _dataBase.Data.ToList();
        var dataIndex = dataBase.IndexOf(data);

        if (dataIndex < _dataBase.Data.Count() - 1)
            dataIndex++;

        return dataBase[dataIndex];
    }
}
