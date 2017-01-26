﻿namespace RedBlueGames.Tools.TextTyper
{
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;
    using RedBlueGames.Tools.TextTyper;
    using UnityEngine.UI;

    /// <summary>
    /// Class that tests TextTyper and shows how to interface with it.
    /// </summary>
    public class Test_UGUI : MonoBehaviour
    {
        public AudioClip printSoundEffect;
        public Text text;
        private Queue<string> scripts = new Queue<string>();

        [SerializeField]
        [Tooltip("The text typer element to test typing with")]
        private TextTyper testTextTyper;

        public void Start()
        {
            this.testTextTyper.PrintCompleted.AddListener(this.HandlePrintCompleted); 
            this.testTextTyper.CharacterPrinted.AddListener(this.HandleCharacterPrinted);

            scripts.Enqueue("When printing extremely long words, notice how they no longer wrap as they are printed.");
            scripts.Enqueue("Hello! My name is<delay=0.5>... </delay>NPC. Got it, bub?");
            scripts.Enqueue("You can <b>use</b> <i>uGUI</i> <size=40>text</size> <size=20>tag</size> and <color=#ff0000ff>color</color> tag <color=#00ff00ff>like this</color>.");
            scripts.Enqueue("bold <b>text</b> test <b>bold</b> text <b>test</b>");
            scripts.Enqueue("You can <size=40>size 40</size> and <size=20>size 20</size>");
            scripts.Enqueue("You can <color=#ff0000ff>color</color> tag <color=#00ff00ff>like this</color>.");
            ShowScript();
        }

        public void OnClickWindow()
        {
            if (this.testTextTyper.IsSkippable())
            {
                this.testTextTyper.Skip();
            }
            else
            {
                ShowScript();
            }
        }


        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
            
                var tag = RichTextTag.ParseNext("blah<color=red>boo</color");
                LogTag(tag);
                tag = RichTextTag.ParseNext("<color=blue>blue</color");
                LogTag(tag);
                tag = RichTextTag.ParseNext("No tag in here");
                LogTag(tag);
                tag = RichTextTag.ParseNext("No <color=blueblue</color tag here either");
                LogTag(tag);
                tag = RichTextTag.ParseNext("This tag is a closing tag </bold>");
                LogTag(tag);
            }
        }

        private void ShowScript()
        {
            if (scripts.Count <= 0)
            {
                return;
            }

            this.testTextTyper.TypeText(scripts.Dequeue());
        }

        private void LogTag(RichTextTag tag)
        {
            if (tag != null)
            {
                Debug.Log("Tag: " + tag.ToString());
            }
        }

        private void HandleCharacterPrinted(string printedCharacter)
        {
            // Do not play a sound for whitespace
            if (printedCharacter == " " || printedCharacter == "\n")
            {
                return;
            }

            var audioSource = this.GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = this.gameObject.AddComponent<AudioSource>();
            }

            audioSource.clip = this.printSoundEffect;
            audioSource.Play();
        }

        private void HandlePrintCompleted()
        {
            Debug.Log("TypeText Complete");
        }
    }
}