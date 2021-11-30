using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuggyForegroundSpriteAnimation : MonoBehaviour
{
    [SerializeField] int buggyLevel = 1;
    [SerializeField] GameObject invertColorPrefab;

    private List<GameObject> invertBlocks;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) {
            Instantiate(invertColorPrefab);
        }
    }

    void addInvertBlock()
    {
        Instantiate(invertColorPrefab);
    }
}
