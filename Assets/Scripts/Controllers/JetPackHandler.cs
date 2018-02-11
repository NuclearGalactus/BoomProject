using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class JetPackHandler : NetworkBehaviour {
    public float amt;
    public float max = 5f;
    public JetPackFuel_Hud HUD;
    float cooldown = 2f;
    float cooldown_c;
    public ParticleSystem systemL;
    public ParticleSystem systemR;
    // Use this for initialization
    void Start()
    {
        amt = max;
        if (isLocalPlayer)
        {
            HUD = GameObject.FindGameObjectWithTag("JetPackFuel").GetComponent<JetPackFuel_Hud>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        if (Input.GetButton("Jump") && amt > 0)
        {
            if (!(GetComponent<PlayerMovement>().verticalVelocity > 20f))
            {
                if(GetComponent<PlayerMovement>().verticalVelocity < 0)
                {
                    GetComponent<PlayerMovement>().verticalVelocity += 22f * Time.deltaTime;
                }
                else
                {
                    GetComponent<PlayerMovement>().verticalVelocity += 11f * Time.deltaTime;
                }
            }

            amt -= Time.deltaTime;
            cooldown_c = cooldown;
        }
        else
        {
            cooldown_c -= Time.deltaTime;
            if(cooldown_c < 0 && amt < max)
            {
                amt += Time.deltaTime * 2;
                if(amt > max)
                {
                    amt = max;
                }
            }
        }
        if (Input.GetButtonDown("Jump") && amt > 0)
        {
            CmdSetParticleState(true);
        }
        if(Input.GetButtonUp("Jump") || amt < 0)
        {
            CmdSetParticleState(false);
        }
        HUD.setFuel(amt / max);
    }
    [Command]
    public void CmdSetParticleState(bool playing)
    {
        RpcSetParticleState(playing);
    }
    [ClientRpc]
    public void RpcSetParticleState(bool playing)
    {
        Debug.Log(GetComponent<Player>().localName + " Jetpack is: " + playing);
        if (playing)
        {
            systemL.Play();
            systemR.Play();
        }
        else
        {
            systemL.Stop();
            systemR.Stop();
        }
    }
}
