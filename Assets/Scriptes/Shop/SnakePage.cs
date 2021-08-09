using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnakePage : MonoBehaviour
{
    [SerializeField] private SnakeDataBase _dataBase;
    [SerializeField] private SnakeShopPresenterV2 _template;

    public IEnumerable<SnakeShopPresenterV2> Render(IEnumerable<SnakeData> _snakes)
    {
        var presenters = new List<SnakeShopPresenterV2>();
        
        SnakeInventory inventory = new SnakeInventory(_dataBase);
        inventory.Load(new JsonSaveLoad());

        foreach (var snake in _snakes)
        {
            var inst = Instantiate(_template, transform);

            if (inventory.Contains(snake))
            {
                if (inventory.SelectedSnake.GUID == snake.GUID)
                    inst.RenderSelected(snake);
                else
                    inst.RenderUnselected(snake);
            }
            else
            {
                inst.RenderLocked(snake);
            }

            presenters.Add(inst);
        }

        return presenters;
    }
}
