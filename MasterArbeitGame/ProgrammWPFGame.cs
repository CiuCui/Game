using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterArbeitGame
{
    public class ProgrammWPFGame
    {
          
        public const int _anzahlx = 40;
        public const int _anzahly = _anzahlx;

        private const int _gesamtAnzahlKnoten = _anzahlx * _anzahly;

        public int GetAnzahlx()
        {
            return _anzahlx;
        }


        

        
        public const int _übertragungswahrscheinlichkeit = 50;

        
        static Random KanteIstOffen = new Random();
        static Random KnopZufallsZahlen = new Random();


        // Werte nochmal überdenken, so liefert Knop aber überwiegend positive Werte
        readonly double _übertragungsrateAlpha = 1;
        readonly double _standardabweichungÜbertragungsrate = 1.0;
        readonly double _geschlossen = -1.0;


        private Knoten NegativKnoten = new Knoten(-1, -1, -1,-1.0);

        private Knoten[] Knotenmenge = new Knoten[_gesamtAnzahlKnoten];

        public Kante[,] Kantenmenge = new Kante[_gesamtAnzahlKnoten, 4];



        public Knoten[] KrankheitsAusbreitung()
        {
            
           
            Knotenmenge = Knotenkonstruieren(_anzahlx,_anzahly);
            Kantenmenge = GetKanten(_anzahly, _anzahly);
            for (int i = 0; i < _gesamtAnzahlKnoten; i++)
            {
                Knotenmenge[i].ZustandVerändert += KnotenZustandVerändert;
            }
            
            Knotenmenge[GitterMittelPunkt()].Infiziere();
            


            return Knotenmenge;
        }

 
       


        // GitterMittelPunkt bestimmen

        public int GitterMittelPunkt()
        {
            int GitterMittelpunkt;
            GitterMittelpunkt = (_anzahlx * (_anzahly / 2)) + _anzahlx / 2;
            

            return GitterMittelpunkt;
        }



        // Knotenkonstruktion


         private Knoten[] Knotenkonstruieren(int anzahlKnotenxAchse, int anzahlKnotenyAchse)
        {

            for (int zeile = 0; zeile < anzahlKnotenyAchse; zeile++)
            {

                for (int knotennummer = (zeile) * anzahlKnotenxAchse; knotennummer < (zeile + 1) * anzahlKnotenxAchse; knotennummer++)
                {
                    int xKoordinate = knotennummer - zeile * anzahlKnotenxAchse;
                    int yKoordinate = zeile;
                    double infektionszeit =  KnopVerteilungsZahlenGeneratorInfektionszeit(_übertragungsrateAlpha, _standardabweichungÜbertragungsrate);

                    Knotenmenge[knotennummer] = new Knoten(knotennummer, xKoordinate, yKoordinate, infektionszeit);
                }
            }
            return Knotenmenge;
        }



        // Nachbarn ermitteln


        public Knoten[] GetNachbarn(Knoten knotenSuchtNachbarn, int anzahlZellenxAchse, int anzahlZellenyAchse)
        {

            Knoten[] nachbarn = new Knoten[4];
            // 0 ist rechts, 1 ist oben, 2 ist links, 3 ist unten

            int rechterNachbar = knotenSuchtNachbarn.GetKnotennummer() + 1;
            int obererNachbar = knotenSuchtNachbarn.GetKnotennummer() - anzahlZellenxAchse;
            int linkerNachbar = knotenSuchtNachbarn.GetKnotennummer() - 1;
            int untererNachbar = knotenSuchtNachbarn.GetKnotennummer() + anzahlZellenxAchse;



            if (knotenSuchtNachbarn.GetKnotennummer() % anzahlZellenxAchse != (anzahlZellenxAchse - 1))
            {
                nachbarn[0] = Knotenmenge[rechterNachbar];
            }
            else
            {
                nachbarn[0] = NegativKnoten;
            }
            if (knotenSuchtNachbarn.GetKnotennummer() >= anzahlZellenxAchse)
            {
                nachbarn[1] = Knotenmenge[obererNachbar];
            }
            else
            {
                nachbarn[1] = NegativKnoten;
            }

            if (knotenSuchtNachbarn.GetKnotennummer() % anzahlZellenxAchse != 0)
            {
                nachbarn[2] = Knotenmenge[linkerNachbar];
            }
            else
            {
                nachbarn[2] = NegativKnoten;
            }
            if (knotenSuchtNachbarn.GetKnotennummer() < anzahlZellenxAchse * (anzahlZellenyAchse - 1))
            {
                nachbarn[3] = Knotenmenge[untererNachbar];
            }
            else
            {
                nachbarn[3] = NegativKnoten;
            }
            return nachbarn; 
        }

        
        
        // Kantenkonstruktion
                


        public Kante[,] GetKanten(int anzahlZellenxAchseKante, int anzahlZellenyAchseKante)
        {


            for (int knoten = 0; knoten < anzahlZellenxAchseKante*anzahlZellenyAchseKante; knoten++)
            {
                Knoten[] nachbarn = GetNachbarn(Knotenmenge[knoten], anzahlZellenxAchseKante, anzahlZellenyAchseKante);




                 

                for (int i = 0; i <= 3; i++)
                {
                    Kante[] nachbarkanten = new Kante[4];
                    if (KanteIstOffen.Next(0, 100) < _übertragungswahrscheinlichkeit && nachbarn[i] != NegativKnoten)
                    {
                        nachbarkanten[i] = new Kante(Knotenmenge[knoten], nachbarn[i], ExponentialVerteilung(_übertragungsrateAlpha));
                    }
                    else
                    {
                        nachbarkanten[i] = new Kante(Knotenmenge[knoten], nachbarn[i], _geschlossen);
                    }

                    Kantenmenge[knoten, i] = nachbarkanten[i];
                }

            }
            return Kantenmenge;

        }



        // ZustandsänderungFeststellen

          private void KnotenZustandVerändert(object source, EventArgs e)
        {
            
            InfiziereNachbarn((Knoten)source);
            
        }

       

        // Infiziere Nachbarn


        public void InfiziereNachbarn(Knoten knoten)
        {
            int count = 0;

            for (int i = 0; i < 4; i++)
            {
                
                if (Kantenmenge[knoten.GetKnotennummer(), i].GetÜbertragungsDauer() != _geschlossen && Kantenmenge[knoten.GetKnotennummer(), i].GetEndknoten().GetZustand() == 0)
                {
                    
                    Kantenmenge[knoten.GetKnotennummer(), i].GetEndknoten().SetbisherigeZeit(knoten.GetBisherigeZeit() + Kantenmenge[knoten.GetKnotennummer(), i].GetÜbertragungsDauer());

                    //Kantenmenge[knoten.GetKnotennummer(), i].GetEndknoten().ZustandVerändert() += KnotenZustandVerändert;
                    Kantenmenge[knoten.GetKnotennummer(), i].GetEndknoten().Infiziere();
                    


                }
                else
                {
                    Kantenmenge[knoten.GetKnotennummer(), i].SetÜbertragungsDauer(_geschlossen);
                    count++;
                }
            }
            if (count == 4)
            {
                knoten.SetIstEndknoten(true);
            }


        }





      
















                             // Infektionszeiten und Dauern Generatoren


        // kann eigentlich nicht kleiner 0
        // aber könnte auch exponential verteil sein
        private static double KnopVerteilungsZahlenGeneratorInfektionszeit(double übertragugnsrateAlpha, double standardAbweichungÜbertragungsrate)
        {
  

            double ersteInfektionszeit;
            double zweiteInfektionszeit;

            double x1;
            double x2;
            double s;



            do
            {
                x1 = 2.0 * KnopZufallsZahlen.NextDouble() - 1.0;
                x2 = 2.0 * KnopZufallsZahlen.NextDouble() - 1.0;
                s = x1 * x1 + x2 * x2;
            } while (s >= 1.0 || s == 0);

            s = Math.Pow(((-2.0 * Math.Log(s)) / s), 0.5);
            ersteInfektionszeit = x1 * s;
            zweiteInfektionszeit = x2 * s;

            // Erwartungswert und Varianz anpassen -  nochmal drüber nachdenken
            // ist eigentlich die Verteilung wie lange ein Knoten krank bleibt
             ersteInfektionszeit = 4.0 / übertragugnsrateAlpha + standardAbweichungÜbertragungsrate * ersteInfektionszeit;


            return ersteInfektionszeit;

        }


        // ist exponential verteilt, daher anderes Verfahren nötig.
        private static double ExponentialVerteilung(double übertragugnsrateAlpha)
        {
            double gleichverteilteZahl = KnopZufallsZahlen.NextDouble();

            gleichverteilteZahl = (-1 / übertragugnsrateAlpha) * Math.Log(1 - gleichverteilteZahl);

            return gleichverteilteZahl;
        }
        /*
        private static double KnopVerteilungsZahlenGeneratorÜbertragung(double übertragungsrateAlpha, double standardabweichungÜbertragungsrate)
        {
  

            double ersteÜbertragungsdauer;
            double zweiteÜbertragungsdauer;

            double x1;
            double x2;
            double s;



            do
            {
                x1 = 2.0 * KnopZufallsZahlen.NextDouble() - 1.0;
                x2 = 2.0 * KnopZufallsZahlen.NextDouble() - 1.0;
                s = x1 * x1 + x2 * x2;
            } while (s >= 1.0 || s == 0);

            s = Math.Pow(((-2.0 * Math.Log(s)) / s), 0.5);
            ersteÜbertragungsdauer = x1 * s;
            zweiteÜbertragungsdauer = x2 * s;

            // Erwartungswert und Varianz anpassen -  nochmal drüber nachdenken
            // ist eigentlich die Verteilung wie lange ein Knoten krank bleibt
            ersteÜbertragungsdauer = 4.0 / übertragungsrateAlpha + standardabweichungÜbertragungsrate * ersteÜbertragungsdauer;


            return ersteÜbertragungsdauer;

        }

    */





    }





}


// NICHT VERWENDETE CODESCHNIPPSEL

/*
        public void InfektionsAusbreitung()
        {
            for (int knoten = 0; knoten < gesamtAnzahlKnoten; knoten++)
            {


                if (Knotenmenge[knoten].GetZustand() == 1)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (Kantenmenge[knoten, i].GetÜbertragungsDauer() != geschlossen && Kantenmenge[knoten, i].GetEndknoten().GetZustand() == 0)
                        {
                            Kantenmenge[knoten, i].GetEndknoten().Infiziere();
                        }
                        else
                        {
                            Kantenmenge[knoten, i].SetÜbertragungsDauer(geschlossen);
                        }
                    }
                }
            }
        }

    */

