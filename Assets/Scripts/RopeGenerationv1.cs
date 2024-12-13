using System;
using UnityEngine;

public class RopeGenerationv1 : MonoBehaviour
{
    [SerializeField]
    GameObject nodePrefab, parentObject;

    [SerializeField]
    [Range(1, 1000)]
    int length = 1;

    [SerializeField]
    float distance = 2f;

    [SerializeField]
    bool generateRope, snapFirst, snapLast = false;

    void Update()
    {
        if (generateRope) {
            GenerateRope();
            generateRope = false;
        }
    }

    public void GenerateRope()
    {
        int count = (int) (length / distance); // The amount of nodes necessary to reach the desired length
        for (int i = 0; i < count; i++) {
            GameObject temp;
            temp = Instantiate(nodePrefab, new Vector3(parentObject.transform.position.x, parentObject.transform.position.y - (i * distance), parentObject.transform.position.z), Quaternion.identity, parentObject.transform);
            temp.transform.eulerAngles = new Vector3(180, 0, 0);
            temp.name = "Node" + i;

            if (i == 0) {
                Destroy(temp.GetComponent<CharacterJoint>());
            } else {
                // Link nodes via character joint
                temp.GetComponent<CharacterJoint>().connectedBody = parentObject.transform.Find("Node" + (i - 1)).GetComponent<Rigidbody>();
            }
        }

        // Snapping removes physics from the first and last nodes (you can place them manually)
        if (snapFirst) {
            parentObject.transform.Find("Node0").GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }

        if (snapLast) {
            parentObject.transform.Find("Node" + (count - 1)).GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
    }
}
