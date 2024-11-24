using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TwoAnchorRope : MonoBehaviour
{
    [SerializeField]
    GameObject nodePrefab, parentObject, anchor1, anchor2;

    [SerializeField]
    [Range(0.01f, 1f)]
    float tension = 0.1f; // 1 = no slack, 0 = infinite slack

    [SerializeField]
    float distance = 2f; // The distance between nodes

    [SerializeField]
    bool generateRope = false;

    void Update()
    {
        if (generateRope) {
            GenerateRope();
            generateRope = false;
        }
    }

    public void GenerateRope() {
        float length = Vector3.Distance(anchor1.transform.position, anchor2.transform.position); // The distance between the two anchors
        int count = (int) (length / distance); // The amount of nodes necessary to reach the desired length

        for (int i = 0; i < count; i++) {
            GameObject temp;
            temp = Instantiate(nodePrefab, Vector3.Lerp(anchor1.transform.position, anchor2.transform.position, (float) i / count), Quaternion.identity, parentObject.transform);
            temp.name = "Node" + i;

            if (i == 0) {
                temp.GetComponent<CharacterJoint>().connectedBody = anchor1.GetComponent<Rigidbody>();
            } else if (i == count - 1) {
                temp.GetComponent<CharacterJoint>().connectedBody = parentObject.transform.Find("Node" + (i - 1)).GetComponent<Rigidbody>();
                anchor2.GetComponent<CharacterJoint>().connectedBody = temp.GetComponent<Rigidbody>();
            } else {
                // Link nodes via character joint
                temp.GetComponent<CharacterJoint>().connectedBody = parentObject.transform.Find("Node" + (i - 1)).GetComponent<Rigidbody>();
            }
        }

        Destroy(anchor1.GetComponent<CharacterJoint>());
        anchor1.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        anchor2.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }
}
