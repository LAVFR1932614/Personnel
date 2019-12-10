using System;
using System.Threading;

namespace Laboratoire_3_3
{
    enum TypeJoueur { Utilisateur = 1, Ordinateur = 2 }
    class Program
    {
        static Random generateur = new Random();
        public struct Joueur
        {
            public TypeJoueur type;
            public string nom;

            public Joueur(int _nombreDeVies, TypeJoueur _type, string _nom) : this()
            {
                type = _type;
                nom = _nom;
            }
        }

        public struct Cases
        {
            public int caractere;
            public bool use;

            public Cases(int _caractere, bool _use) : this()
            {
                caractere = _caractere;
                use = _use;
            }
        }
        static void Main(string[] args)
        {
            Cases[,] tabJeu = new Cases[3, 3];
            Joueur[] tabJoueurs = new Joueur[2];
            bool quitter = false;

            InitialiserJoueurs(ref tabJoueurs);

            while (quitter == false)
            {
                InitialiserTableauJeu(ref tabJeu);
                Jouer(ref tabJeu, ref tabJoueurs,ref quitter);
            }
            Console.WriteLine("Merci d'avoir joué!");
            Console.ReadKey();
        }

        /*Générateur de nombre aléatoire différent*/
        static int GenererNombre(int min, int max)
        {
            int nombreAleatoire = 0;
            nombreAleatoire = generateur.Next(min, max);
            return nombreAleatoire;
        }

        /*Fonction qui permet d'afficher la grille*/
        static void AfficherGrille(ref Cases[,] tabJeu)
        {
            string grille = " _________________\n" +
                            "|     |     |     |\n" +
                            "|     |     |     |\n" +
                            "|_____|_____|_____|\n" +
                            "|     |     |     |\n" +
                            "|     |     |     |\n" +
                            "|_____|_____|_____|\n" +
                            "|     |     |     |\n" +
                            "|     |     |     |\n" +
                            "|_____|_____|_____|\n";
            /*Console.SetCursorPosition(15, 20);        centrer la grille plus tard en creant un tableau*/
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(grille);
            Console.ForegroundColor = ConsoleColor.White;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Console.SetCursorPosition(3 + (6 * j), 2 + (3 * i));
                    if (tabJeu[i, j].caractere == 79 || tabJeu[i, j].caractere == 88)
                        Console.Write((char)tabJeu[i, j].caractere);
                    else
                        Console.Write(tabJeu[i, j].caractere);
                }
            }
        }
        /*Fonction qui analyse le choix*/
        static void AnalyserChoix(ref Joueur joueur, ref Cases[,] tabJeu, char caractere)
        {
            int choix = 0;

            Console.WriteLine("\n\nQuel case voulez vous modifier " + joueur.nom + " ?");
            choix = Convert.ToInt32(Console.ReadLine());

            switch (choix)
            {
                case 1: ModifierCase(ref tabJeu[0, 0], caractere); break;
                case 2: ModifierCase(ref tabJeu[0, 1], caractere); break;
                case 3: ModifierCase(ref tabJeu[0, 2], caractere); break;
                case 4: ModifierCase(ref tabJeu[1, 0], caractere); break;
                case 5: ModifierCase(ref tabJeu[1, 1], caractere); break;
                case 6: ModifierCase(ref tabJeu[1, 2], caractere); break;
                case 7: ModifierCase(ref tabJeu[2, 0], caractere); break;
                case 8: ModifierCase(ref tabJeu[2, 1], caractere); break;
                case 9: ModifierCase(ref tabJeu[2, 2], caractere); break;
            }
        }

        /*Fonction qui vérifie si on peut modifier la case*/
        static Cases ModifierCase(ref Cases caseATester, char caractere)
        {

            if (caseATester.use == false)
            {
                caseATester.caractere = caractere;
                caseATester.use = true;
            }
            else;

            return caseATester;
        }

        /*Fonction qui initialise les joueurs*/
        static void InitialiserJoueurs(ref Joueur[] tabJoueurs)
        {
            int choix = 0;

            Console.WriteLine("Voulez vous jouer contre un humain ou un ordinateur?\n1-Humain\n2-Ordinateur");
            choix = Convert.ToInt32(Console.ReadLine());
            if (choix == 2)
                tabJoueurs[1].type = TypeJoueur.Ordinateur;
            else
                tabJoueurs[1].type = TypeJoueur.Utilisateur;

            tabJoueurs[0].type = TypeJoueur.Utilisateur;

            Console.WriteLine("Quel est le nom du premier joueur?");
            tabJoueurs[0].nom = Console.ReadLine();
            Console.WriteLine("Quel est le nom du deuxième joueur?");
            tabJoueurs[1].nom = Console.ReadLine();
        }

        /*Fonction qui initialise les valeurs du tableau*/
        static void InitialiserTableauJeu(ref Cases[,] tabJeu)
        {
            int valeur = 1;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    tabJeu[i, j].caractere = valeur;
                    tabJeu[i, j].use = false;
                    valeur++;
                }
            }
        }

        /*Fonction de jeux*/
        static void Jouer(ref Cases[,] tabJeu, ref Joueur[] tabJoueurs,ref bool quitter)
        {
            bool gagne = false;
            int nbCaseModifiees = 0;
            int tourJoueur = GenererNombre(0, 3);//permet de decider aléatoirement du premier joueur a jouer
            int indiceDernierJoueurJoue = 0;

            while (gagne == false && nbCaseModifiees < 9)
            {
                nbCaseModifiees++;
                tourJoueur++;
                AfficherGrille(ref tabJeu);
                if (tourJoueur % 2 == 0)
                {
                    if (tabJoueurs[1].type == TypeJoueur.Utilisateur)
                    {
                        AnalyserChoix(ref tabJoueurs[1], ref tabJeu, 'O');
                        indiceDernierJoueurJoue = 1;
                    }
                    else
                    {
                        ChoixOrdinateur(ref tabJeu, 'O');
                        indiceDernierJoueurJoue = 1;
                        AnimationDeJeux(tabJoueurs[1].nom);
                    }
                }
                else
                {
                    AnalyserChoix(ref tabJoueurs[0], ref tabJeu, 'X');
                    indiceDernierJoueurJoue = 0;
                }

                AfficherGrille(ref tabJeu);
                VerifierSiOnGagne(ref tabJeu, ref gagne,tabJoueurs[indiceDernierJoueurJoue].nom,ref quitter);
            }

            if (gagne == false)
            {
                Console.WriteLine("\n\n\nMatch nul!");
                Rejouer(ref quitter);
            }
                
        }

        /*Fonction qui vérifie si on gagne*/
        static void VerifierSiOnGagne(ref Cases[,] tabJeu, ref bool gagne,string nomGagnant,ref bool quitter)
        {
            if (tabJeu[0, 0].caractere == tabJeu[0, 1].caractere && tabJeu[0, 1].caractere == tabJeu[0, 2].caractere)
                gagne = true;
            else if (tabJeu[1, 0].caractere == tabJeu[1, 1].caractere && tabJeu[1, 1].caractere == tabJeu[1, 2].caractere)
                gagne = true;
            else if (tabJeu[2, 0].caractere == tabJeu[2, 1].caractere && tabJeu[2, 1].caractere == tabJeu[2, 2].caractere)
                gagne = true;
            else if (tabJeu[0, 0].caractere == tabJeu[1, 0].caractere && tabJeu[1, 0].caractere == tabJeu[2, 0].caractere)
                gagne = true;
            else if (tabJeu[0, 1].caractere == tabJeu[1, 1].caractere && tabJeu[1, 1].caractere == tabJeu[2, 1].caractere)
                gagne = true;
            else if (tabJeu[0, 2].caractere == tabJeu[1, 2].caractere && tabJeu[1, 2].caractere == tabJeu[2, 2].caractere)
                gagne = true;
            else if (tabJeu[0, 0].caractere == tabJeu[1, 1].caractere && tabJeu[1, 1].caractere == tabJeu[2, 2].caractere)
                gagne = true;
            else if (tabJeu[0, 2].caractere == tabJeu[1, 1].caractere && tabJeu[1, 1].caractere == tabJeu[2, 0].caractere)
                gagne = true;

            if (gagne == true)
            {
                Console.WriteLine("\n\n\nFelicitation, "+nomGagnant + ", vous avez gagné!");
                Rejouer(ref quitter);
            }
            // [vertical,horizontal]
        }

        /*Fonction pour rejouer*/
        static void Rejouer(ref bool quitter)
        {
            Console.WriteLine("Voulez vous rejouer?\n1-Oui\n2-Non");
            int choix = Convert.ToInt32(Console.ReadLine());
            if (choix == 2)
                quitter = true;
        }

        /*Fonction de mini intelligence artificielle*/
        static void ChoixOrdinateur(ref Cases[,] tabJeu, char caractere)
        {
            bool dejaModifie = false;
            if (dejaModifie == false)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (tabJeu[i, 0].caractere == tabJeu[i, 1].caractere && tabJeu[i, 2].use == false)              //Horizontal si il manque le dernier
                    {
                        ModifierCase(ref tabJeu[i, 2], caractere);
                        dejaModifie = true;
                    }
                }
            }
            if (dejaModifie == false)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (tabJeu[i, 2].caractere == tabJeu[i, 1].caractere && tabJeu[i, 0].use == false)              //Horizontal si il manque le premier
                    {
                        ModifierCase(ref tabJeu[i, 0], caractere);
                        dejaModifie = true;
                    }
                }
            }
            if (dejaModifie == false)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (tabJeu[i, 0].caractere == tabJeu[i, 2].caractere && tabJeu[i, 1].use == false)              //Horizontal si il manque le milieu
                    {
                        ModifierCase(ref tabJeu[i, 1], caractere);
                        dejaModifie = true;
                    }
                }
            }
            if (dejaModifie == false)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (tabJeu[0, j].caractere == tabJeu[1, j].caractere && tabJeu[2, j].use == false)              //Vertical si il manque lui du bas
                    {
                        ModifierCase(ref tabJeu[2, j], caractere);
                        dejaModifie = true;
                    }
                }
            }
            if (dejaModifie == false)
            {

                for (int j = 0; j < 3; j++)
                {
                    if (tabJeu[1, j].caractere == tabJeu[2, j].caractere && tabJeu[0, j].use == false)              //Vertical si il manque lui du haut
                    {
                        ModifierCase(ref tabJeu[0, j], caractere);
                        dejaModifie = true;
                    }
                }
            }
            if (dejaModifie == false)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (tabJeu[0, j].caractere == tabJeu[2, j].caractere && tabJeu[1, j].use == false)              //Vertical si il manque lui du milieu
                    {
                        ModifierCase(ref tabJeu[1, j], caractere);
                        dejaModifie = true;
                    }
                }
            }
            if (dejaModifie == false)
            {
                if (tabJeu[0, 0].caractere == tabJeu[1, 1].caractere && tabJeu[2, 2].use == false)
                {
                    ModifierCase(ref tabJeu[2, 2], caractere);
                    dejaModifie = true;
                }
            }
            if (dejaModifie == false)
            {
                if (tabJeu[2, 2].caractere == tabJeu[1, 1].caractere && tabJeu[0, 0].use == false)
                {
                    ModifierCase(ref tabJeu[0, 0], caractere);
                    dejaModifie = true;
                }
            }
            if (dejaModifie == false)
            {
                if (tabJeu[0, 0].caractere == tabJeu[2, 2].caractere && tabJeu[1, 1].use == false)
                {
                    ModifierCase(ref tabJeu[1, 1], caractere);
                    dejaModifie = true;
                }
            }
            if (dejaModifie == false)
            {
                if (tabJeu[2, 0].caractere == tabJeu[1, 1].caractere && tabJeu[0, 2].use == false)
                {
                    ModifierCase(ref tabJeu[0, 2], caractere);
                    dejaModifie = true;
                }
            }
            if (dejaModifie == false)
            {
                if (tabJeu[2, 0].caractere == tabJeu[0, 2].caractere && tabJeu[1, 1].use == false)
                {
                    ModifierCase(ref tabJeu[1, 1], caractere);
                    dejaModifie = true;
                }
            }
            if (dejaModifie == false)
            {
                if (tabJeu[1, 1].caractere == tabJeu[0, 2].caractere && tabJeu[2, 0].use == false)
                {
                    ModifierCase(ref tabJeu[2, 0], caractere);
                    dejaModifie = true;
                }
            }
            if (dejaModifie == false)
            {
                bool quitter = false;

                while (quitter == false)
                {
                    int i = GenererNombre(0, 3);
                    int j = GenererNombre(0, 3);

                    if (tabJeu[i, j].use == false)
                    {
                        quitter = true;
                        ModifierCase(ref tabJeu[i, j], caractere);
                    }
                }
            }
        }

        /*Fonction pour l'animation de jeu de l'ordinateur*/

        public static void AnimationDeJeux(string nom)
        {
            Console.Clear();
            Console.WriteLine(nom + " est entrain de jouer", 15);
            for (int i = 0; i < 3; i++)
            {
                Console.Write(" .");
                Thread.Sleep(500);
            }
        }
    }
}

