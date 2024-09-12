using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MailboxGive : MonoBehaviour
{
    GameObject player;
    KaiMovement movement;
    public bool mailboxHasMailbag = false;
    public MailboxGive nextMailbox;
    // Start is called before the first frame update
    void Start()
    {
        //GameObject[] MailboxesList;
        player = GameObject.Find("Player");
        movement = player.GetComponent<KaiMovement>();
        //MailboxesList = (GameObject.FindGameObjectsWithTag("mailbox"));
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < 2f && Input.GetKeyDown(KeyCode.F) && !mailboxHasMailbag && !nextMailbox.mailboxHasMailbag)
        {
            movement.ToggleBagOff();
            nextMailbox.mailboxHasMailbag = true;
        }
        else if (Vector3.Distance(transform.position, player.transform.position) < 2f && Input.GetKeyDown(KeyCode.F) && (mailboxHasMailbag))
        {
            movement.ToggleBagOn();
            mailboxHasMailbag = false;
        }
    }
}
