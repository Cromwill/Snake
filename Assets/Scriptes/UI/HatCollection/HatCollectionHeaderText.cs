using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

[RequireComponent(typeof(TMP_Text))]
public class HatCollectionHeaderText : MonoBehaviour
{
    [SerializeField] private HatDataBase _dataBase;

    private TMP_Text _header;

    private void Awake()
    {
        _header = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        var allHatCount = _dataBase.Data.Count();

        HatCollection collection = new HatCollection(_dataBase);
        collection.Load(new JsonSaveLoad());

        var collectedHats = collection.Data.Count();

        _header.text = $"Snake hat\n{collectedHats}/{allHatCount}";
    }
}
