using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    public GameObject boxPrefab;
    public GameObject brokenRuleTextPrefab;
    public Color[] boxColors;

    public TextMeshProUGUI totalMoneyText;
    public TextMeshProUGUI plusMoneyText;
    public TextMeshProUGUI minusMoneyText;

    public TextMeshPro req1Text;
    public TextMeshPro req2Text;
    public TextMeshPro req3Text;
    public TextMeshPro req1OverflowText;
    public TextMeshPro req2OverflowText;
    public TextMeshPro req3OverflowText;
    public TextMeshPro rule1Text;
    public TextMeshPro rule2Text;
    public TextMeshPro rule3Text;

    public GameObject slash1;
    public GameObject slash2;
    public GameObject slash3;
    public GameObject check1;
    public GameObject check2;
    public GameObject check3;

    List<GameObject> brokenRuleTextList = new List<GameObject>();
    List<Transform> brokenRuleBoxTops = new List<Transform>();

    int totalMoney = 0;
    int plusMoney = 0;
    int minusMoney = 0;
    int req1PlusMoney = 0;
    int req2PlusMoney = 0;
    int req3PlusMoney = 0;
    int req1MinusMoney = 0;
    int req2MinusMoney = 0;
    int req3MinusMoney = 0;
    int rulesBroken = 0;

    Stack currentStack;
    public List<Box> boxList;
    // Start is called before the first frame update
    void Start()
    {
        UpdateMoneyText();
        currentStack = new Stack(boxPrefab, 1, boxColors, this);
        NextStack();
    }

    IEnumerator PlaceBoxes(List<Box> boxes)
    {   
        float y = 8;
        foreach (var box in boxes)
        {
            yield return new WaitForSeconds(0.1f); 
            box.rigidbody2d.gravityScale = 1;
            box.transform.position = new Vector2(1, y);
            y += 1.5f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateMoneyText() {
        plusMoney = req1PlusMoney + req2PlusMoney + req3PlusMoney;
        minusMoney = (rulesBroken * 5) + req1MinusMoney + req2MinusMoney + req3MinusMoney;
        totalMoneyText.text = $"${totalMoney}";
        plusMoneyText.text = $"+${plusMoney}";
        minusMoneyText.text = $"-${minusMoney}";
    }

    StackRequirement GenerateRequirement() {
        int type = Random.Range(0, 8);
        int count = Random.Range(2, 4);
        var requirement = new StackRequirement((StackRequirementType)type, count);
        return requirement;
    }

    string TextForRequirement(StackRequirement requirement) {
        var typeString = "";
        switch (requirement.type) {
            case StackRequirementType.darkBrown:
                typeString = "Dark Brown";
                break;
            case StackRequirementType.lightBrown:
                typeString = "Light Brown";
                break;
            case StackRequirementType.yellow:
                typeString = "Yellow";
                break;
            case StackRequirementType.pink:
                typeString = "Pink";
                break;
            case StackRequirementType.purple:
                typeString = "Purple";
                break;
            case StackRequirementType.square:
                typeString = "Square";
                break;
            case StackRequirementType.tall:
                typeString = "Tall";
                break;
            case StackRequirementType.wide:
                typeString = "Wide";
                break;
        }
        if (requirement.count == 1) {
            return $"  1 {typeString} box";
        } else {
            return $"  {requirement.count} {typeString} boxes";
        }
    }

    string TextForRule(StackRequirementType rule) {
        var typeString = "";
        switch (rule) {
            case StackRequirementType.darkBrown:
                typeString = "Dark Brown";
                break;
            case StackRequirementType.lightBrown:
                typeString = "Light Brown";
                break;
            case StackRequirementType.yellow:
                typeString = "Yellow";
                break;
            case StackRequirementType.pink:
                typeString = "Pink";
                break;
            case StackRequirementType.purple:
                typeString = "Purple";
                break;
            case StackRequirementType.square:
                typeString = "Square";
                break;
            case StackRequirementType.tall:
                typeString = "Tall";
                break;
            case StackRequirementType.wide:
                typeString = "Wide";
                break;
        }
        return $"NO {typeString.ToUpper()} BOXES TOUCHING";
    }

    public void ClearBrokenRules() {
        foreach (var obj in brokenRuleTextList)
        {
            Destroy(obj); 
        }
        brokenRuleTextList.Clear();
        brokenRuleBoxTops.Clear();
        AddBrokenRuleText();
    }
    
    public void WhichRulesBroken(int[] brokenRules) {
        rule1Text.color = Color.black;
        rule2Text.color = Color.black;
        rule3Text.color = Color.black;
        if (brokenRules[0] == 1) {
            rule1Text.color = Color.red;
        }
        if (brokenRules[1] == 1) {
            rule2Text.color = Color.red;
        }
        if (brokenRules[2] == 1) {
            rule3Text.color = Color.red;
        }
    }

    public void BrokenRuleAt(Transform boxTop) {
        var text = Instantiate(brokenRuleTextPrefab);
        brokenRuleTextList.Add(text);
        brokenRuleBoxTops.Add(boxTop);
        // Invoke("AddBrokenRuleText", 0.1f);
        AddBrokenRuleText();
    }

    void AddBrokenRuleText() {
        rulesBroken = 0;
        for (int i = 0; i < brokenRuleTextList.Count; i++)
        {
            Transform boxTop = brokenRuleBoxTops[i]; 
            GameObject text = brokenRuleTextList[i];
            var newPos = new Vector2(4.5f, transform.TransformPoint(boxTop.position).y - 1.4f);
            text.transform.position = newPos;
            rulesBroken++;
        }
        UpdateMoneyText();
    }

    public void RequirementsMet(int reqNumber, int met, int outOf, int extras) {
        switch (reqNumber) {
            case 1:
                req1PlusMoney = met * 5;
                req1MinusMoney = extras;
                if (met < outOf) {
                    slash1.SetActive(true);
                } else {
                    slash1.SetActive(false);
                }
                if (met == outOf && extras <= 0) {
                    check1.SetActive(true);
                } else {
                    check1.SetActive(false);
                }
                if (extras > 0 || met < outOf) {
                    req1OverflowText.text = $"{met + extras}/";
                } else {
                    req1OverflowText.text = "";
                }
                break;
            case 2:
                req2PlusMoney = met * 5;
                req2MinusMoney = extras;
                if (met < outOf) {
                    slash2.SetActive(true);
                } else {
                    slash2.SetActive(false);
                }
                if (met == outOf && extras <= 0) {
                    check2.SetActive(true);
                } else {
                    check2.SetActive(false);
                }
                if (extras > 0 || met < outOf) {
                    req2OverflowText.text = $"{met + extras}/";
                } else {
                    req2OverflowText.text = "";
                }
                break;
            case 3:
                req3PlusMoney = met * 5;
                req3MinusMoney = extras;
                if (met < outOf) {
                    slash3.SetActive(true);
                } else {
                    slash3.SetActive(false);
                }
                if (met == outOf && extras <= 0) {
                    check3.SetActive(true);
                } else {
                    check3.SetActive(false);
                }
                if (extras > 0 || met < outOf) {
                    req3OverflowText.text = $"{met + extras}/";
                } else {
                    req3OverflowText.text = "";
                }
                break;
        }
        UpdateMoneyText();
    }

    public void NextStack() {
        totalMoney += plusMoney;
        totalMoney -= minusMoney;
        ClearBrokenRules();
        var oldBoxList = new List<Box>(boxList);
        foreach (var box in oldBoxList)
        {
            box.Pop();
        }
        currentStack = new Stack(boxPrefab, 15, boxColors, this);
        var requirementList = new List<StackRequirement>();
        while (requirementList.Count < 3) {
            var req = GenerateRequirement();
            var isUnique = true;
            foreach (var requirement in requirementList)
            {
                if (requirement.type == req.type) {
                    isUnique = false;
                } 
            }
            if (isUnique)
            {
                requirementList.Add(req);
            }
        }
        // each rule is just that a certain requirement type doesn't touch
        var ruleList = new List<StackRequirementType>();
        while (ruleList.Count < 3) {
            var rule = (StackRequirementType)Random.Range(0, 8);
            var isUnique = true;
            foreach (var r in ruleList)
            {
                if (r == rule) {
                    isUnique = false;
                } 
            }
            if (isUnique) {
                ruleList.Add(rule);
            }
        }
        currentStack.Create(requirementList.ToArray(), ruleList.ToArray());
        StartCoroutine(PlaceBoxes(currentStack.boxes));
        req1Text.text = TextForRequirement(requirementList[0]);
        req2Text.text = TextForRequirement(requirementList[1]);
        req3Text.text = TextForRequirement(requirementList[2]);

        rule1Text.text = TextForRule(ruleList[0]);
        rule2Text.text = TextForRule(ruleList[1]);
        rule3Text.text = TextForRule(ruleList[2]);
    }
}
