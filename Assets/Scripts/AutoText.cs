using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

public class AutoText : MonoBehaviour
{
    TextMeshPro textMesh;
    private string text = "";
    private TypingDone doneFun;
    public AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        textMesh = GetComponent<TextMeshPro>();
    }

    public void TypeText(string text, TypingDone fun) {
        this.text = text;
        StopCoroutine("StartTyping");
        Type(fun);
    }

    public delegate void TypingDone();

    void Type(TypingDone fun) {
        doneFun = fun;
        textMesh.text = "";
        StartCoroutine("StartTyping");
    }

    IEnumerator StartTyping()
    {
        // string[] words = Regex.Split(text, @"\W|_");
        string[] words = Regex.Split(text, @" ");
	    foreach (string word in words)
	    {
		    textMesh.text += word;
            textMesh.text += " ";
            audio.Play();
		    yield return new WaitForSeconds (0.1f);
	    }
	    doneFun();
    }
}
