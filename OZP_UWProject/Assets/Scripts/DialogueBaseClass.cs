using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DialogueSystem
{
    public class DialogueBaseClass : MonoBehaviour
    {
        public bool finished { get; protected set; }
        protected IEnumerator WriteText(string input, Text textHolder, Color textColor, Font textFont, float delay, AudioClip sound)
        {
            textHolder.color = textColor;
            textHolder.font = textFont;
            for (int i = 0; i < input.Length; i++)
            {
                textHolder.text += input[i];
                if(i % 5 == 0)
                AudioManager.instance.PlaySound(sound);
                yield return new WaitForSeconds(delay);

            }

            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

            finished = true;
        }
    }
}
