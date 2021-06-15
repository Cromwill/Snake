using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tests : MonoBehaviour
{
    [SerializeField] private SnakeDataBase _snakeDataBase;

    public void ResetInventory()
    {
        SnakeInventory snakeInventory = new SnakeInventory(_snakeDataBase);
        snakeInventory.Save(new JsonSaveLoad());
    }
}
