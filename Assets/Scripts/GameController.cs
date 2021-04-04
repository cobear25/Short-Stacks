using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject boxPrefab;
    public Color[] boxColors;
    // Start is called before the first frame update
    void Start()
    {
        Stack stack = new Stack(boxPrefab, 15, boxColors);
        stack.Create();
        GenerateRequirements();
        GenerateRequirements();
        GenerateRequirements();
        GenerateRequirements();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateRequirements() {
        int type = Random.Range(0, 8);
        int count = Random.Range(1, 4);
        var requirement = new Requirement((RequirementType)type, count);
        Debug.Log(TextForRequirement(requirement));
    }

    string TextForRequirement(Requirement requirement) {
        var typeString = "";
        switch (requirement.type) {
            case RequirementType.darkBrown:
                typeString = "Dark Brown";
                break;
            case RequirementType.lightBrown:
                typeString = "Light Brown";
                break;
            case RequirementType.yellow:
                typeString = "Yellow";
                break;
            case RequirementType.pink:
                typeString = "Pink";
                break;
            case RequirementType.purple:
                typeString = "Purple";
                break;
            case RequirementType.square:
                typeString = "Square";
                break;
            case RequirementType.tall:
                typeString = "Tall";
                break;
            case RequirementType.wide:
                typeString = "Wide";
                break;
        }
        if (requirement.count == 1) {
            return $"1 {typeString} box";
        } else {
            return $"{requirement.count} {typeString} boxes";
        }
    }
}
