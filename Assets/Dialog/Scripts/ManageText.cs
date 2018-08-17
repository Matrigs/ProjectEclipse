using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This class will manage all the text tha appears in the bubble text
public class ManageText : MonoBehaviour
{
    public RectTransform BubbleBox;
    public Text BubbleText;

    public float OffSetWidth;
    public float OffSetHeight;

    public float MaxTextWidth;

    [Header("Dialog Documents")]
    public TextAsset[] DialogTexts;

   
    [System.Serializable]
    public struct Characters
    {
        public GameObject character;
        public string codeInDocument;
    }

    [Header("Characters")]
    public Characters[] TalkableCharacters;

    [HideInInspector]
    public int currentText = 0;              
    [HideInInspector]
    public int currentDialogLine = 0;    
    [HideInInspector]
    public bool endedBubble;
    [HideInInspector]
    public bool endedDialog;

    private string newText;
    private int totalTextWidth = 0;
    private int totalTextHeight = 0;

    private DialogControl dialogController;
    
    private List<GameObject> currentSpeakers = new List<GameObject>(); 
    private List<string> currentLines = new List<string>();       

    void Start()
    {
        dialogController = this.GetComponent<DialogControl>();
    }

    private void Update() {

       if (endedBubble)
       {
            if (currentDialogLine < currentLines.Count && endedDialog)
            {
                if (currentLines[currentDialogLine].Length != 0)
                {
                    dialogController.showSpeechBallon(currentSpeakers[currentDialogLine]);
                    presentBubbleText();
                    currentDialogLine += 1;
                    totalTextWidth = 0;
                }

            } else
            {
                endedDialog = false;
                currentDialogLine = 0;
                totalTextWidth = 0;
                currentSpeakers.Clear();
                currentLines.Clear();
            }
       }

    }

    public void startDialog(int number)
    {
        currentText = number - 1;
        FindSpeaker(DialogTexts[currentText]);
        endedBubble = true; 
        endedDialog = true; 
    }


    // adjust de bubble with the text
    private void presentBubbleText()
    {
        endedBubble = false;

        CalculateWidthOfMessage(currentLines[currentDialogLine]);

        BubbleText.text = newText;
        totalTextHeight = (int)(BubbleText.preferredHeight);

        if (totalTextWidth < MaxTextWidth)
        {
            BubbleBox.sizeDelta = new Vector2(totalTextWidth + OffSetWidth, totalTextHeight + OffSetHeight);
        }
        else
        {
            print(MaxTextWidth + OffSetWidth + " Tamanho");
            BubbleBox.sizeDelta = new Vector2(MaxTextWidth + OffSetWidth, totalTextHeight + OffSetHeight);
        }
    }

    // Organize the speakers and their lines in two arrays
    private void FindSpeaker(TextAsset text)
    {
        char[] arr = text.text.ToCharArray();
        List<char> dialogLine = new List<char>();
        bool willSearchdialog = false;
        for (int i=0;i<arr.Length;i++)
        {
            char currentLetter = arr[i];

            if (currentLetter == ' ')
            {
                if (!willSearchdialog)
                {
                    dialogLine.Add(currentLetter);
                }

            } else
            {
                if (willSearchdialog)
                {
                    willSearchdialog = false;
                }
                dialogLine.Add(currentLetter);
            }

            // get the speaker
            if (currentLetter == ':')
            {
                dialogLine.Remove(currentLetter);

                // Check if not the first speaker
                if (dialogLine.Contains('\n'))
                {
                    dialogLine.Remove('\n');
                    dialogLine.RemoveAt(0);
                } 

                foreach (Characters speaker in TalkableCharacters)
                {
                    if (speaker.codeInDocument == new string(dialogLine.ToArray()))
                    {
                        currentSpeakers.Add(speaker.character);
                        break;
                    }
                }

                dialogLine.Clear();
                willSearchdialog = true;
            }

            // get the line
            if (currentLetter == ';')
            { 
                dialogLine.Remove(currentLetter);
                currentLines.Add(new string(dialogLine.ToArray()));

                dialogLine.Clear();
            }

        }


        }

    // Todo -- separar por palavra e não letra, se uma palavra for maior que todo o texto, separar a palavra

    // Calculates the Width of the text
    private void CalculateWidthOfMessage(string message)
    {
        // voltar até o primeiro espaço e jogar a palavra pra baixo

        Font myFont = BubbleText.font;  //chatText is my Text component
        CharacterInfo characterInfo = new CharacterInfo();
        int lineText = 0;

        List<char> lst = new List<char>();

        char[] arr = message.ToCharArray();
        foreach (char c in arr) { lst.Add(c); }

        for (int i = 0; i < lst.Count; i++)
        {
            myFont.GetCharacterInfo(lst[i], out characterInfo, BubbleText.fontSize);
            totalTextWidth += characterInfo.advance;
            lineText += characterInfo.advance;            

            // Don't let the text pass it's maximum
            if (lineText > MaxTextWidth)
            {

               // print("entrei");
                //print(MaxTextWidth);
                //print(lineText);
                int j = i-1;
                if (lst[i-1] != ' ')
                {

                    for (; j >= 0; j--)
                    {
                        if(lst[j] == ' ')
                        {
                            break;
                        }
                    }

                    if(j == 0)
                    {
                        j = i-1;
                        lst.Insert(j, '-');
                    }

                } 

                lineText = 0;
                lst.Insert(j, '\n');
            }
        }

        string txt = new string(lst.ToArray());
        newText = txt;
    }
}
