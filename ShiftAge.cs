using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShiftAge : MonoBehaviour
{
    private LinkedList<GameObject> agesList = new LinkedList<GameObject>();
    // Start is called before the first frame update
    private bool moving = false;
    void Start()
    {
        foreach (Transform age in transform)
        {
            agesList.AddLast(age.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (moving) {
            return;
        }
        if (Input.GetKey("p")) {
            StartCoroutine(ShiftForward());
        }
        if (Input.GetKey("n")) {
            StartCoroutine(ShiftBackward());
        }
    }

    public IEnumerator ShiftForward() {
        moving = true;
        GameObject age = agesList.First.Value;
        agesList.RemoveFirst();
        agesList.AddLast(age);
        ResetAges();
        yield return new WaitForSeconds(0.5f);
        moving = false;
        yield return null;
    }

    public IEnumerator ShiftBackward() {
        moving = true;
        GameObject age = agesList.Last.Value;
        agesList.RemoveLast();
        agesList.AddFirst(age);
        ResetAges();
        yield return new WaitForSeconds(0.5f);
        moving = false;
        yield return null;
    }

    void ResetAges() {
        foreach (GameObject age in agesList) {
            age.SetActive(false);
        }
        agesList.First.Value.SetActive(true);
    }

}
