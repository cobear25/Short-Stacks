using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stack : MonoBehaviour
{
    GameObject boxPrefab;
    int stackSize = 0;
    Color[] colors;
    List<Box> boxes = new List<Box>();

    public Stack(GameObject boxPrefab, int size, Color[] colors) {
        this.boxPrefab = boxPrefab;
        stackSize = size;
        this.colors = colors;
    }

    public void Create() {
        for (int i = 0; i < stackSize; i++)
        {
            Box box = Instantiate(boxPrefab, new Vector2(1, i * 2 + 1), Quaternion.identity).GetComponent<Box>(); 
            box.color = colors[Random.Range(0, colors.Length)];
            // box.transform.localScale = new Vector3(Random.Range(0.75f, 1.75f), Random.Range(0.75f, 1.25f), 1);
            int randShape = Random.Range(0, 3);
            box.shape = (BoxShape)randShape;
            box.stack = this;
            boxes.Add(box);
        }
    }

    public void RemoveBox(Box box) {
        boxes.Remove(box);
        Debug.Log(boxes.Count);
    }
}

public enum RequirementType {
    wide, tall, square, darkBrown, lightBrown, yellow, purple, pink
}

public struct Requirement {
    public RequirementType type;
    public int count;

    public Requirement(RequirementType _type, int _count) {
        type = _type;
        count = _count;
    }
}