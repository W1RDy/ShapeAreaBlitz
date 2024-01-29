using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class ChooserLevel : MonoBehaviour
{
    [SerializeField] int firstLevelSceneIndex;
    public int levelIndex = -1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Level") levelIndex = collision.GetComponent<LevelIcon>().index + firstLevelSceneIndex;       
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag != "Level") levelIndex = -1;
    }
}
