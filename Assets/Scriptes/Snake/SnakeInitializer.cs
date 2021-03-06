using UnityEngine;
using UnityEngine.Events;

public class SnakeInitializer : MonoBehaviour
{
    [SerializeField] private SnakeDataBase _snakeDataBase;
    [SerializeField] private Track _track;
    [SerializeField] private FinishPath _finish;
    [SerializeField] private PathLineDrawer _lineDrawer;

    public event UnityAction<Snake> Initialized;

    private void Start()
    {
        SnakeInventory snakeInventory = new SnakeInventory(_snakeDataBase);
        snakeInventory.Load(new JsonSaveLoad());

        var selectedSnake = snakeInventory.SelectedSnake.Prefab;

        var inst = Instantiate(selectedSnake, transform.position, transform.rotation);
        inst.Init(_track, _finish);

        Initialized?.Invoke(inst);
    }
}
