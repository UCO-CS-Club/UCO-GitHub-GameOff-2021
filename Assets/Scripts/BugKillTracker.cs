using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugKillTracker : MonoBehaviour
{
    [SerializeField] private int numberOfBugsKilledBeforeReversingTheirAttack = 30;

    private int numberOfBugsKilled;

    // Start is called before the first frame update
    void Start()
    {
        numberOfBugsKilled = 0;        
    }

    // Update is called once per frame
    void Update()
    {
        if (numberOfBugsKilled == numberOfBugsKilledBeforeReversingTheirAttack)
        {
            numberOfBugsKilled = 0;
            GameObject[] bugs = GameObject.FindGameObjectsWithTag("Bug");
            foreach (var bug in bugs)
            {
                bug.GetComponent<BugBehavior>().SetAttackingBoss();
            }
        }
    }

    public void KillBug()
    {
        numberOfBugsKilled++;
    }
}
