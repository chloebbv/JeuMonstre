using System;

namespace JeuMonstre
{
    class Program
    {
        private static Random random = new Random();
        static void Main(string[] args)
        {
            AfficheMenu();
            ConsoleKeyInfo consoleKeyInfo = Console.ReadKey(true);
            while (consoleKeyInfo.Key != ConsoleKey.D1 && consoleKeyInfo.Key != ConsoleKey.D2 && consoleKeyInfo.Key != ConsoleKey.NumPad1 && consoleKeyInfo.Key != ConsoleKey.NumPad2)
            {
                AfficheMenu();
                consoleKeyInfo = Console.ReadKey(true);
                
            }
            if (consoleKeyInfo.Key == ConsoleKey.NumPad1 || consoleKeyInfo.Key == ConsoleKey.D1)
                Jeu1();
            else
                Jeu2();
            Console.Read();
        }


        private  static void AfficheMenu()
        {
            Console.Clear();
            Console.WriteLine("Veuillez choisir votre mode de jeu :");
            Console.WriteLine("\t1 : Contre les monstres");
            Console.WriteLine("\t2 : Contre le boss de fin");
        }


        private static void Jeu1()
        {
            Joueur chloe = new Joueur(150);
            int cptFacile = 0;
            int cptDifficile = 0;
            
            while (chloe.EstVivant)
            {   
                MonstreFacile monstre = FabriqueDeMonstre();
                while (monstre.EstVivant && chloe.EstVivant)
                {
                    
                    chloe.Attaque(monstre);
                    
                    if (monstre.EstVivant)
                        monstre.Attaque(chloe);
                }

                if (chloe.EstVivant)
                {
                    if (monstre is MonstreDifficile)
                        cptDifficile++;
                    else
                        cptFacile++;
                }

                else
                {
                    Console.WriteLine("Vous êtes mort :(");
                    break;
                }

                
            }
            Console.WriteLine("Bravo! Vous avez tué {0} monstres faciles et {1} monstres difficiles. Vous avez {2} points.", cptFacile, cptDifficile, cptFacile + cptDifficile * 2);

        }

        private static MonstreFacile FabriqueDeMonstre()
        {
            if (random.Next(2) == 0)
                return new MonstreFacile();
            else
                return new MonstreDifficile();
        }

        private static void Jeu2()
        {
            BossDeFin boss = new BossDeFin(250);
            Joueur xavier = new Joueur(150);

            while (boss.EstVivant && xavier.EstVivant)
            {
                xavier.Attaque(boss);
                if (boss.EstVivant)
                    boss.Attaque(xavier);
            }

            if (xavier.EstVivant)
                Console.WriteLine("Bravo!! Vous avez vaincu le boss de fin!");
            else
                Console.WriteLine("Oups ... Vous êtes mort!");
        }


        public abstract class Personnage
        {
            public abstract void Attaque(Personnage personnage);
            public abstract void SubitDegats(int degats);
            public abstract bool EstVivant { get; protected set; }
            public int LanceLeDe()
            {
                Random random = new Random();
                return random.Next(1, 7);
            }
        }


        public class MonstreFacile : Personnage
        {
            private const int degats = 10;
            public override bool EstVivant { get; protected set; }
            public MonstreFacile()
            {
                EstVivant = true;
            }
            public override void Attaque(Personnage personnage)
            {

                int lanceMonstre = LanceLeDe();
                int lanceJoueur = personnage.LanceLeDe();
                if (lanceMonstre > lanceJoueur)
                    personnage.SubitDegats(degats);
            }
            public override void SubitDegats(int degats)
            {
                EstVivant = false;
            }
        }


        public class MonstreDifficile : MonstreFacile
        {
            private const int degatsSort = 5;

            public override void Attaque(Personnage personnage)
            {
                base.Attaque(personnage);
                personnage.SubitDegats(SortMagique());
            }

            private int SortMagique()
            {
                int valeur = LanceLeDe();
                if (valeur == 6)
                    return 0;
                return degatsSort * valeur;
            }
        }

        public abstract class PersonnageAPointsDeVie : Personnage
        {
            public int PtsDeVie { get; set; }
            public override bool EstVivant
            {
                get { return PtsDeVie > 0; }
                protected set { }
            }

            public int LanceLeDe(int valeur)
            {
                Random random = new Random();
                return random.Next(1, valeur);
            }
        }

        public class Joueur : PersonnageAPointsDeVie
        {
            public Joueur(int points)
            {
                PtsDeVie = points;
            }
            public override void Attaque(Personnage personnage)
            {
                if (personnage is PersonnageAPointsDeVie)
                {
                    int nbPts = LanceLeDe(26);
                    personnage.SubitDegats(nbPts);
                }
                else
                {
                    int lanceJoueur = LanceLeDe();
                    int lanceMonstre = personnage.LanceLeDe();
                    if (lanceJoueur >= lanceMonstre)
                        personnage.SubitDegats(0);
                }
            }
            public override void SubitDegats(int degats)
            {
                if (!BouclierFonctionne())
                    PtsDeVie -= degats;
            }
            private bool BouclierFonctionne()
            {
                return LanceLeDe() <= 2;
            }
        }

        public class BossDeFin : PersonnageAPointsDeVie
        {
            public BossDeFin(int points)
            {
                PtsDeVie = points;
            }

            public override void Attaque(Personnage personnage)
            {
                int nbPts = LanceLeDe(26);
                personnage.SubitDegats(nbPts);
            }

            public override void SubitDegats(int degats)
            {
                PtsDeVie -= degats;
            }
        }
    }
}
