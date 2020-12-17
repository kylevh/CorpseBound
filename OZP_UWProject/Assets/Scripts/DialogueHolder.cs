using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    public class DialogueHolder : MonoBehaviour
    {
        private IEnumerator dialogueSeq;
        private bool dialogueFinished;

        private void Update()
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                Deactivate();
                gameObject.SetActive(false);
                //GameManager.gm.ResumeGame();
            }
        }

        private void OnEnable()
        {

            dialogueSeq = dialogueSequence();
            StartCoroutine(dialogueSeq);
        }
        private IEnumerator dialogueSequence()
        {
            //GameManager.gm.PauseGame();
            if (!dialogueFinished)
            {
                for (int i = 0; i < transform.childCount - 1; i++)
                {
                    Deactivate();
                    transform.GetChild(i).gameObject.SetActive(true);
                    yield return new WaitUntil(() => transform.GetChild(i).GetComponent<DialogueLine>().finished);
                }
            }

            else
            {
                int index = transform.childCount - 1;
                Deactivate();
                transform.GetChild(index).gameObject.SetActive(true);
                yield return new WaitUntil(() => transform.GetChild(index).GetComponent<DialogueLine>().finished);
            }

            dialogueFinished = true;
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.F));
            gameObject.SetActive(false);
            //GameManager.gm.ResumeGame();
        }

        private void Deactivate()
        {
            for(int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
