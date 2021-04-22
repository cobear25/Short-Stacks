using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    public GameObject boxPrefab;
    public GameObject brokenRuleTextPrefab;
    public Color[] boxColors;
    public Color[] boxColors2;

    public TextMeshProUGUI totalMoneyText;
    public TextMeshProUGUI plusMoneyText;
    public TextMeshProUGUI minusMoneyText;

    public GameObject skipTutorialButton;
    public TextMeshPro tutorialText;
    public GameObject doneButton;
    public GameObject undoButton;
    public GameObject stackText;
    public TextMeshPro req1Text;
    public TextMeshPro req2Text;
    public TextMeshPro req3Text;
    public TextMeshPro req4Text;
    public TextMeshPro req1OverflowText;
    public TextMeshPro req2OverflowText;
    public TextMeshPro req3OverflowText;
    public TextMeshPro req4OverflowText;
    public TextMeshPro rule1Text;
    public TextMeshPro rule2Text;
    public TextMeshPro rule3Text;
    public TextMeshPro rule4Text;
    public TextMeshPro rule1Dash;
    public TextMeshPro rule2Dash;
    public TextMeshPro rule3Dash;
    public TextMeshPro rule4Dash;

    public TextMeshProUGUI doneButtonText;

    public GameObject slash1;
    public GameObject slash2;
    public GameObject slash3;
    public GameObject slash4;
    public GameObject check1;
    public GameObject check2;
    public GameObject check3;
    public GameObject check4;
    public GameObject ruleCheck1;
    public GameObject ruleCheck2;
    public GameObject ruleCheck3;
    public GameObject ruleCheck4;

    public GameObject gameOverText;

    List<GameObject> brokenRuleTextList = new List<GameObject>();
    List<Transform> brokenRuleBoxTops = new List<Transform>();

    int totalMoney = 0;
    int plusMoney = 0;
    int minusMoney = 0;
    int req1PlusMoney = 0;
    int req2PlusMoney = 0;
    int req3PlusMoney = 0;
    int req4PlusMoney = 0;
    int req1MinusMoney = 0;
    int req2MinusMoney = 0;
    int req3MinusMoney = 0;
    int req4MinusMoney = 0;
    int rulesBroken = 0;

    int tutorialStep = 1;
    int boxesRemoved = 0;

    public int requirementCount = 2;
    public int ruleCount = 2;
    public int stackSize = 10;
    public int stackNumber = 0;

    AutoText tutorialAutoText;

    Stack currentStack;
    public List<Box> boxList;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        currentStack = new Stack(boxPrefab, 1, boxColors, this);
        tutorialAutoText = tutorialText.GetComponent<AutoText>();
        Invoke("StartTutorial", 1);
    }

    void StartTutorial() {
        tutorialAutoText.TypeText("HEY YOU. Yeah, you with the face. Wanna make some dough? I've got some stacks of boxes here that I need taken care of. They're too tall and I'll pay you to sort them out a bit.", NoOp);
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

    void NoOp() {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateMoneyText() {
        plusMoney = req1PlusMoney + req2PlusMoney + req3PlusMoney + req4PlusMoney;
        minusMoney = (rulesBroken * 5) + req1MinusMoney + req2MinusMoney + req3MinusMoney + req4MinusMoney;
        totalMoneyText.text = $"${totalMoney}";
        plusMoneyText.text = $"+${plusMoney}";
        minusMoneyText.text = $"-${minusMoney}";
        if (stackNumber > 5 || stackNumber < 1) {
            plusMoneyText.text = "";
            minusMoneyText.text = "";
        }
        if (stackNumber < 1) {
            totalMoneyText.text = "";
        }
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
        for (int i = 0; i < brokenRules.Length; i++)
        {
            if (i < ruleCount) {
                RuleBroken(i, brokenRules[i] == 1);
            }
        }
    }

    void RuleBroken(int ruleNumber, bool isBroken) {
        switch (ruleNumber) {
            case 0:
                if (isBroken) {
                    rule1Text.color = Color.red;
                    ruleCheck1.SetActive(false);
                } else {
                    rule1Text.color = Color.black;
                    ruleCheck1.SetActive(true);
                }
                break;
            case 1:
                if (isBroken) {
                    rule2Text.color = Color.red;
                    ruleCheck2.SetActive(false);
                } else {
                    rule2Text.color = Color.black;
                    ruleCheck2.SetActive(true);
                }
                break;
            case 2:
                if (isBroken) {
                    rule3Text.color = Color.red;
                    ruleCheck3.SetActive(false);
                } else {
                    rule3Text.color = Color.black;
                    ruleCheck3.SetActive(true);
                }
                break;
            case 3:
                if (isBroken) {
                    rule4Text.color = Color.red;
                    ruleCheck4.SetActive(false);
                } else {
                    rule4Text.color = Color.black;
                    ruleCheck4.SetActive(true);
                }
                break;
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
                req1MinusMoney = extras * 2;
                if (met < outOf) {
                    slash1.SetActive(true);
                    req1MinusMoney = 5;
                    req1PlusMoney = 0;
                } else {
                    req1PlusMoney = met * 5;
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
                req2MinusMoney = extras * 2;
                if (met < outOf) {
                    slash2.SetActive(true);
                    req2MinusMoney = 5;
                    req2PlusMoney = 0;
                } else {
                    req2PlusMoney = met * 5;
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
                req3MinusMoney = extras * 2;
                if (met < outOf) {
                    slash3.SetActive(true);
                    req3MinusMoney = 5;
                    req3PlusMoney = 0;
                } else {
                    req3PlusMoney = met * 5;
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
            case 4:
                req4MinusMoney = extras * 2;
                if (met < outOf) {
                    slash4.SetActive(true);
                    req4MinusMoney = 5;
                    req4PlusMoney = 0;
                } else {
                    req4PlusMoney = met * 5;
                    slash4.SetActive(false);
                }
                if (met == outOf && extras <= 0) {
                    check4.SetActive(true);
                } else {
                    check4.SetActive(false);
                }
                if (extras > 0 || met < outOf) {
                    req4OverflowText.text = $"{met + extras}/";
                } else {
                    req4OverflowText.text = "";
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
        currentStack = new Stack(boxPrefab, stackSize, boxColors, this);
        var requirementList = new List<StackRequirement>();
        while (requirementList.Count < requirementCount) {
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
        while (ruleList.Count < ruleCount) {
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
        for (int i = 0; i < requirementList.Count; i++)
        {
            switch (i) {
                case 0:
                    req1Text.text = TextForRequirement(requirementList[i]);
                    break;
                case 1:
                    req2Text.text = TextForRequirement(requirementList[i]);
                    break;
                case 2:
                    req3Text.text = TextForRequirement(requirementList[i]);
                    break;
                case 3:
                    req4Text.text = TextForRequirement(requirementList[i]);
                    break;
            }
        }
        for (int i = 0; i < ruleList.Count; i++)
        {
            switch (i) {
                case 0:
                    rule1Text.text = TextForRule(ruleList[i]);
                    rule1Dash.text = "-";
                    break;
                case 1:
                    rule2Text.text = TextForRule(ruleList[i]);
                    rule2Dash.text = "-";
                    break;
                case 2:
                    rule3Text.text = TextForRule(ruleList[i]);
                    rule3Dash.text = "-";
                    break;
                case 3:
                    rule4Text.text = TextForRule(ruleList[i]);
                    rule4Dash.text = "-";
                    break;
            }
        }
        undoButton.SetActive(false);
    }

    public void NextTutorialStep() {
        if (tutorialStep == 1) {
            tutorialAutoText.TypeText("It's simple enough, just click on a box to get rid of it. Go on, click away.", NoOp);
            currentStack = new Stack(boxPrefab, stackSize, boxColors, this);
            currentStack.Create(new StackRequirement[]{}, new StackRequirementType[]{});
            StartCoroutine(PlaceBoxes(currentStack.boxes));
            doneButton.SetActive(false);
        } else if (tutorialStep == 2) {
        } else if (tutorialStep == 3) {
            tutorialAutoText.TypeText("I'm going to give you a set of Requirements to meet and a set of Rules to follow. You'll earn $5 for each requirement met, but I'll dock you $2 for each one over the requirement, and dock you $5 if you go too low. I'll also dock you $5 for each rule broken.", NoOp);
            doneButtonText.text = "Go";
        } else if (tutorialStep == 4) {
            doneButtonText.text = "Done";
            stackNumber = 1;
            stackText.SetActive(true);
            tutorialText.gameObject.SetActive(false);
            skipTutorialButton.SetActive(false);
            UpdateMoneyText();
            NextStack();
        }

        tutorialStep++;
    }
    
    public void SubmitStack() {
        switch (stackNumber) {
            case 0:
                NextTutorialStep();
                break;
            case 1:
                requirementCount = 2;
                ruleCount = 3;
                stackNumber++;
                stackSize = 12;
                NextStack();
                break;
            case 2:
                requirementCount = 3;
                ruleCount = 3;
                stackNumber++;
                stackSize = 14;
                NextStack();
                break;
            case 3:
                requirementCount = 3;
                ruleCount = 4;
                stackNumber++;
                stackSize = 16;
                NextStack();
                break;
            case 4:
                requirementCount = 4;
                ruleCount = 4;
                stackNumber++;
                stackSize = 18;
                NextStack();
                break;
            case 5:
                // done
                requirementCount = 0;
                ruleCount = 0;
                stackSize = 0;
                doneButtonText.text = "New Game";
                stackText.SetActive(false);
                plusMoneyText.text = "";
                minusMoneyText.text = "";
                stackNumber++;
                gameOverText.SetActive(true);
                NextStack();
                break;
            case 6:
                // new game
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                break;
        }
    }

    public void SkipTutorial() {
        doneButtonText.text = "Done";
        stackNumber = 1;
        stackText.SetActive(true);
        tutorialText.gameObject.SetActive(false);
        skipTutorialButton.SetActive(false);
        UpdateMoneyText();
        NextStack();
    }

    public void BoxRemoved() {
        boxesRemoved++;
        if (boxesRemoved == 3 && tutorialStep == 2) {
            tutorialAutoText.TypeText("Ok, THAT'S ENOUGH. You're costing me money. I've got some special requirements for these stacks and I want you to follow what I say exactly.", NoOp);
            doneButton.SetActive(true);
            tutorialStep++;
        }
        if (stackNumber > 0) {
            undoButton.SetActive(true);
        }
    }

    public void UndoButtonTapped() {
        currentStack.Undo();
        Invoke("FixGravity", 0.5f);
        undoButton.SetActive(false);
    }

    void FixGravity() {
        foreach (var box in currentStack.boxes)
        {
            box.rigidbody2d.gravityScale = 1; 
        }
    }
}
