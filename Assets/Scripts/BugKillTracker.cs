using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugKillTracker : MonoBehaviour
{
    [SerializeField] private int numberOfBugsKilledBeforeReversingTheirAttack = 5;

    private int numberOfBugsKilled;

    // Start is called before the first frame update
    void Start()
    {
        numberOfBugsKilled = 0;        
    }

    // Update is called once per frame
    void Update()
    {
        if (numberOfBugsKilled >= numberOfBugsKilledBeforeReversingTheirAttack)
        {
            GameObject[] bugs = GameObject.FindGameObjectsWithTag("Bug");
            foreach (var bug in bugs)
            {
                bug.GetComponent<BugBehavior>().SetAttackingBoss(true);
            }

            numberOfBugsKilled = 0;
        }
    }

    public void KillBug()
    {
        numberOfBugsKilled++;
        Debug.Log("# of Bugs Killed: " + numberOfBugsKilled);
    }
}
