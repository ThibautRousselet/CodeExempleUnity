using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

//Cette classe gère la batterie de la lampe torche du joueur
//La lampe peut etre rechargee en secouant le controleur et a 4 etats possibles :
// - Recharge : le joueur secoue la lampe et la batterie augmente, la lumiere devient bleue pendant 2 secondes quand la batterie est pleine
// - Surcharge : Le joueur appuie sur un bouton pour activer ce mode qui rend la lumiere rouge et mortelle pour les ennemis mais consomme plus de batterie
// - Decharge : La batterie est vide donc la lumiere est eteinte
// - Normal : Eclaire avec une lumiere blanche dans les autres cas

public class BatterieLampe : MonoBehaviour
{
    public float Batterie;
    public float MaxBatterie;

    public Light lumiere;
    public SteamVR_Action_Boolean IsSurcharge = null;

    private float timerBatteriePleine;
    private Vector3 posActuel;
    private Vector3 posPrecedente;


    // Start is called before the first frame update
    void Start()
    {
        lumiere = GetComponentInChildren<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        posActuel = this.transform.position;

        //Si on appuie sur le bouton de surcharge, on vide la batterie plus vite et la lumiere devient rouge
        if (IsSurcharge.state && Batterie>0)
        {
            lumiere.color = Color.red;
            lumiere.intensity = 3;
            Batterie-=0.5f;
        }
        //On teste si la lampe est secouée
        else if (Vector3.Distance(posActuel, posPrecedente)>0.2)
        {
            //on recharge la lampe si c est le cas
            if (Batterie < MaxBatterie)
            {
                Batterie+=10;
            } 
            //Si la batterie est pleine, la lumiere devient bleue
            else if (timerBatteriePleine <= 0)
            {
                timerBatteriePleine = 2;
                lumiere.color = Color.cyan;
                lumiere.intensity = 1;
            }
        } 
        else
        //Sinon la lumiere est blanche
        {
            lumiere.color = Color.white;
            lumiere.intensity = 1;
        }

        //On vide la batterie a chaque update
        Batterie -= Time.deltaTime;
        //On vide aleatoirement pour faire varier la duree de vie de la batterie
        if (Random.value > 0.995)
        {
            Batterie--;
        }

        //Plus de lumiere si la batterie est vide
        if (Batterie < 0)
        {
            Batterie = 0;
            lumiere.enabled = false;
            lumiere.intensity = 1;

        } else
        {
            lumiere.enabled = true;
        }

        //On met a jour les valeurs pour le prochain appel
        posPrecedente = posActuel;
        timerBatteriePleine -= Time.deltaTime;
    }
}
