using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static AttributsFiole;

//Le but du jeu et de ramener des fioles de la couleur demandee
//Cette classe permet de detecter le placement d une dans le recipient dediee 
// A chaque fois que le joueur amene la bonne fiole, le score augmente de 1, la carte s' assombrit et des zombies apparaissent pour compliquer le jeu
public class DetectionFiole : MonoBehaviour
{
    private Couleur couleurCible;
    private int score = 0; //nb de fioles ramenées

    public ZombieSpawn ZombieSpawn;

    private Collider autreObjet;//Collider de l objet place dans le recipient

    public GameObject HUD = null;
    private UnityEngine.UI.Text hudTexte = null;

    public GameObject lumiereCouloirs; //Collection de GroupeLampes de chaque couloir
    private List<GroupeLampe> groupeLampeCouloirs = new List<GroupeLampe>();
    public GroupeLampe lobby;
    public GroupeLampe salle1;
    public GroupeLampeSecours secoursLobby;
    public GroupeLampeSecours secoursSalle1;

    void Start()
    {
        hudTexte = HUD.GetComponentInChildren<Text>();
        SetCibleAleatoire();
        ZombieSpawn = this.GetComponent<ZombieSpawn>();

        //Place les groupes de lumieres dans une liste
        foreach (GroupeLampe groupeL in lumiereCouloirs.GetComponentsInChildren<GroupeLampe>())
        {
            groupeLampeCouloirs.Add(groupeL);
        }
    }

    //Appelee quand un objet est placee dans le recipient
    private void OnTriggerEnter(Collider collider)
    {
        autreObjet = collider;
        Couleur CouleurAutre = autreObjet.gameObject.GetComponent<AttributsFiole>().CouleurFiole;

        //Compare la couleur cible a celle de l'objet place
        if (couleurCible == CouleurAutre)
        {
            OnDetection();
        }

        //On detruit l'objet dans tous les cas
        Destroy(autreObjet.gameObject);
    }

    void Update()
    {
        //On peut passer a la phase suivante en appuyant sur p pour tester
        if (Input.GetKeyDown("p"))
        {
            OnDetection();
        }
    }

    //On passe a la phase suivante quand la bonne fiole est ammenee
    private void OnDetection()
    {
        score++;
        switch (score)
        {
            //Chaque phase applique des modifications sur la map
            case 1:
                Phase1();
                break;
            case 2:
                Phase2();
                break;
            case 3:
                Phase3();
                break;
            case 4:
                Phase4();
                break;

        }
    }

    //A chaque phase, on eteind plus de lumieres et on fait apparaitre des zombies
    private void Phase1()
    {
        //Fait clignoter 3 couloirs
        groupeLampeCouloirs[2].setClignotement(true);
        groupeLampeCouloirs[3].setClignotement(true);
        groupeLampeCouloirs[4].setClignotement(true);

        //spawn 10 zombies
        ZombieSpawn.zombieSpawn(10);
    }

    private void Phase2()
    {
        //On eteint la salle 1
        secoursSalle1.SetIntensite(1);
        salle1.SetIntensite(0);

        ZombieSpawn.zombieSpawn(15);

        groupeLampeCouloirs[0].SetIntensite(0);
        groupeLampeCouloirs[3].SetIntensite(0);

        groupeLampeCouloirs[5].setClignotement(true);
        groupeLampeCouloirs[6].setClignotement(true);
    }

    private void Phase3()
    {
        groupeLampeCouloirs[2].SetIntensite(0);
        groupeLampeCouloirs[3].SetIntensite(0);
        groupeLampeCouloirs[4].SetIntensite(0);
        groupeLampeCouloirs[5].SetIntensite(0);
        groupeLampeCouloirs[6].SetIntensite(0);
        groupeLampeCouloirs[7].SetIntensite(0);

        lobby.SetIntensite(0);
        secoursLobby.SetIntensite(1);

        ZombieSpawn.zombieSpawn(20);

    }

    //Apres avoir ramene 3 fioles le joueur gagne
    private void Phase4()
    {
        hudTexte.text = "VOUS AVEZ GAGNE";
    }

    private void SetCible(Couleur couleur)
    {
        couleurCible = couleur;
        hudTexte.text = "Objectif : \n Ramener une fiole " + couleurCible.ToString();
    }

    //Choisi aleatoirement une couleur cible et l affiche
    private void SetCibleAleatoire()
    {
        couleurCible = GenereCouleurAlea();
        hudTexte.text = "Objectif : \n Ramener une fiole " + couleurCible.ToString();
    }

    //Permet de generer une couleur aléatoire
    private Couleur GenereCouleurAlea()
    {
        Couleur coul;
        float rand = Random.Range(0.0f, 5.0f);
        if (rand < 1.0f)
            coul = Couleur.Blanc;
        else if (rand < 2.0f)
            coul = Couleur.Bleu;
        else if (rand < 3.0f)
            coul = Couleur.Jaune;
        else if (rand < 4.0f)
            coul = Couleur.Rouge;
        else
            coul = Couleur.Vert;
        return coul;
    }
}