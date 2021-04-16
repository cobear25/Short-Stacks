using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;

public class Stack: MonoBehaviour
{
    GameObject boxPrefab;
    int stackSize = 0;
    public Color[] colors;
    public List<Box> boxes = new List<Box>();
    StackRequirement[] requirements;
    StackRequirementType[] rules;
    GameController gameController;

    Dictionary<StackRequirementType, int> requirementsMet = new Dictionary<StackRequirementType, int>() {
        [StackRequirementType.tall] = 0,
        [StackRequirementType.wide] = 0,
        [StackRequirementType.square] = 0,
        [StackRequirementType.darkBrown] = 0,
        [StackRequirementType.lightBrown] = 0,
        [StackRequirementType.yellow] = 0,
        [StackRequirementType.purple] = 0,
        // [StackRequirementType.pink] = 0
    };

    public Stack(GameObject boxPrefab, int size, Color[] colors, GameController gameController) {
        this.boxPrefab = boxPrefab;
        stackSize = size;
        this.colors = colors;
        this.gameController = gameController;
    }

    public void Create(StackRequirement[] requirements, StackRequirementType[] rules) {
        this.requirements = requirements;
        this.rules = rules;
        List<BoxData> tempBoxList = new List<BoxData>();
        // first create boxes that meet the requirements
        foreach (var req in requirements)
        {
            for (int i = 0; i < req.count; i++)
            {
                tempBoxList.Add(CreateBoxFromRequirementType(req.type));
            }
        }

        // then add more random boxes
        int remainingCount = stackSize - tempBoxList.Count;
        for (int i = 0; i < remainingCount; i++) {
            var box = new BoxData();
            box.color = (BoxColor)Random.Range(0, colors.Length);
            int randShape = Random.Range(0, 3);
            box.shape = (BoxShape)randShape;
            tempBoxList.Add(box);
        }

        // shuffle the box list
        for (int i = 0; i < stackSize; i++)
        {
            BoxData current = tempBoxList[i];
            int randomIndex = Random.Range(i, stackSize);
            tempBoxList[i] = tempBoxList[randomIndex];
            tempBoxList[randomIndex] = current;
        }

        // instantiate the boxes
        for (int i = 0; i < stackSize; i++)
        {
            Box box = Instantiate(boxPrefab, new Vector2(-8, i * 2 + 1), Quaternion.identity).GetComponent<Box>(); 
            box.stack = this;
            box.color = tempBoxList[i].color;
            box.shape = tempBoxList[i].shape;
            boxes.Add(box);
        }
        PopulateRequirementsMet();
        MeetsRequirements(requirements);
        tempBoxList.Clear();
        gameController.boxList = boxes;
    }

    public void RemoveBox(Box box) {
        boxes.Remove(box);
        switch (box.shape)
        {
            case BoxShape.square:
                requirementsMet[StackRequirementType.square] -= 1;
                break;
            case BoxShape.wide:
                requirementsMet[StackRequirementType.wide] -= 1;
                break;
            case BoxShape.tall:
                requirementsMet[StackRequirementType.tall] -= 1;
                break;
        }
        switch (box.color)
        {
            case BoxColor.darkBrown:
                requirementsMet[StackRequirementType.darkBrown] -= 1;
                break;
            case BoxColor.lightBrown:
                requirementsMet[StackRequirementType.lightBrown] -= 1;
                break;
            case BoxColor.yellow:
                requirementsMet[StackRequirementType.yellow] -= 1;
                break;
            case BoxColor.purple:
                requirementsMet[StackRequirementType.purple] -= 1;
                break;
            // case BoxColor.pink:
            //     requirementsMet[StackRequirementType.pink] -= 1;
            //     break;
        }
        MeetsRequirements(requirements);
        gameController.BoxRemoved();
    }

    void PopulateRequirementsMet() {
        foreach (var box in boxes)
        {
            switch (box.shape)
            {
                case BoxShape.square:
                    requirementsMet[StackRequirementType.square] += 1;
                    break;
                case BoxShape.wide:
                    requirementsMet[StackRequirementType.wide] += 1;
                    break;
                case BoxShape.tall:
                    requirementsMet[StackRequirementType.tall] += 1;
                    break;
            } 
            switch (box.color) {
                case BoxColor.darkBrown:
                    requirementsMet[StackRequirementType.darkBrown] += 1;
                    break;
                case BoxColor.lightBrown:
                    requirementsMet[StackRequirementType.lightBrown] += 1;
                    break;
                case BoxColor.yellow:
                    requirementsMet[StackRequirementType.yellow] += 1;
                    break;
                case BoxColor.purple:
                    requirementsMet[StackRequirementType.purple] += 1;
                    break;
                // case BoxColor.pink:
                //     requirementsMet[StackRequirementType.pink] += 1;
                //     break;
            }
        }
    }

    void MeetsRequirements(StackRequirement[] requirements)
    {
        for (int i = 1; i <= requirements.Length; i++)
        {
            var req = requirements[i - 1]; 
            // check for how many meet requirements
            int totalMet = 0;
            foreach (var box in boxes)
            {
                if (box.HasRequirementType(req.type)) {
                    totalMet++;
                }
            }
            var met = Mathf.Min(totalMet, req.count);
            var extras = Mathf.Max(totalMet - req.count, 0);
            gameController.RequirementsMet(i, met, req.count, extras);
        }
    }

    public void CheckRules() {
        gameController.ClearBrokenRules();
        // go up to the second to last box because the last one has nothing to check after it
        var rulesBroken = new int[] {0, 0, 0, 0};
        for (int i = 0; i < boxes.Count - 1; i++)
        {
            Box box = boxes[i];
            int brokenCount = 0;
            for (int j = 0; j < rules.Length; j++)
            {
                var rule = rules[j]; 
                if (box.HasRequirementType(rule) && boxes[i + 1].HasRequirementType(rule)) {
                    gameController.BrokenRuleAt(box.boxTop);
                    rulesBroken[j] = 1;
                    brokenCount++;
                    break;
                }  
            }
        }
        gameController.WhichRulesBroken(rulesBroken);
    }

    BoxData CreateBoxFromRequirementType(StackRequirementType reqType) {
        var box = new BoxData();
        switch (reqType) {
            case StackRequirementType.square:
                box.shape = BoxShape.square;
                box.color = box.color = (BoxColor)Random.Range(0, colors.Length);
                break;
            case StackRequirementType.wide:
                box.shape = BoxShape.wide;
                box.color = box.color = (BoxColor)Random.Range(0, colors.Length);
                break;
            case StackRequirementType.tall:
                box.shape = BoxShape.tall;
                box.color = box.color = (BoxColor)Random.Range(0, colors.Length);
                break;
            case StackRequirementType.darkBrown:
                box.color = BoxColor.darkBrown;
                box.shape = (BoxShape)Random.Range(0, 3);
                break;
            case StackRequirementType.lightBrown:
                box.color = BoxColor.lightBrown;
                box.shape = (BoxShape)Random.Range(0, 3);
                break;
            case StackRequirementType.yellow:
                box.color = BoxColor.yellow;
                box.shape = (BoxShape)Random.Range(0, 3);
                break;
            case StackRequirementType.purple:
                box.color = BoxColor.purple;
                box.shape = (BoxShape)Random.Range(0, 3);
                break;
            case StackRequirementType.pink:
                box.color = BoxColor.pink;
                box.shape = (BoxShape)Random.Range(0, 3);
                break;
        }
        return box;
    }
}

public enum StackRequirementType {
    wide, tall, square, darkBrown, lightBrown, yellow, purple, pink
}

public struct StackRequirement {
    public StackRequirementType type;
    public int count;

    public StackRequirement(StackRequirementType _type, int _count) {
        type = _type;
        count = _count;
    }
}