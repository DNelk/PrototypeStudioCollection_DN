using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogPetter : MonoBehaviour
{
    public Dictionary<int, bool> dogsIhavePet; //A dictionary of dogs we have pet. If we pet their front but not their back, the value is false
    // Start is called before the first frame update
    void Start()
    {
        dogsIhavePet = new Dictionary<int, bool>();
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other);
        if(other.gameObject.CompareTag("DogCollider"))
        {
            string dogPart = other.gameObject.GetComponent<DogCollider>().BodyPosition;
            GameObject superParent = other.gameObject.transform.parent.gameObject.transform.parent.gameObject;//The actual dog gameObject
            int id = superParent.GetInstanceID(); 
            if (dogPart == "front")
            {
                if(!dogsIhavePet.ContainsKey(id))
                    dogsIhavePet.Add(id, false);
            }
            else if (dogPart == "back")
            {
                if (dogsIhavePet.ContainsKey(id))
                {
                    if (!dogsIhavePet[id])
                    {
                        dogsIhavePet[id] = true;
                        RobotGameManager.instance.DogPetComplete(superParent.CompareTag("Raccoon"));
                    }
                }
            }
        }
    }
}
