using UnityEngine;
using UnityEngine.Events;

public class SnakeInitializer : MonoBehaviour
{
    [SerializeField] private SnakeDataBase _snakeDataBase;
    [SerializeField] private Track _track;
    [SerializeField] private FinishPath _finish;
    [SerializeField] private GameObject _tapToPlay;
    [SerializeField] private PathLineDrawer _lineDrawer;

    public event UnityAction<Snake> Initialized;

    private void Start()
    {
        SnakeInventory snakeInventory = new SnakeInventory(_snakeDataBase);
        snakeInventory.Load(new JsonSaveLoad());

        var selectedSnake = snakeInventory.SelectedSnake.Prefab;

        var inst = Instantiate(selectedSnake, transform.position, transform.rotation);
        inst.Init(_track, _finish, _tapToPlay);
        //inst.Moving += _lineDrawer.DrawLine;

        Initialized?.Invoke(inst);
    }
}
