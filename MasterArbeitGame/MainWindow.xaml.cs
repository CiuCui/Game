using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Threading;


namespace MasterArbeitGame
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
            


            zeichenfläche.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            zeichenfläche.Arrange(new Rect(0.0, 0.0, zeichenfläche.DesiredSize.Width, zeichenfläche.DesiredSize.Height));
            
            //int anzahlZellenBreit = Programm.GetAnzahlx();
            
            
        }

        public ProgrammWPFGame Programm = new ProgrammWPFGame();


        public int anzahlZellenBreit =  ProgrammWPFGame._anzahlx;
        public int anzahlZellenHoch = ProgrammWPFGame._anzahly;
        
        public byte ByteKonstruktion(double zahl, double maximumBisherigeZeit)
        {
            double maximum = maximumBisherigeZeit;
            zahl = zahl / maximum;
            zahl = zahl * 255;

           
            return (byte)zahl;
        }

        public double MaximumBisherigeZeit(Knoten[] Knotenmenge)
        {
            double maximum = 0;
            for (int i = 0; i < anzahlZellenBreit*anzahlZellenHoch; i++)
            {
                if (maximum < Knotenmenge[i].GetBisherigeZeit())
                {
                    maximum = Knotenmenge[i].GetBisherigeZeit();
                }
            }
            return maximum;
        }
       


        

        private void buttonKonstruktion_Click(object sender, RoutedEventArgs e)
        {
           // anzahlZellenBreit = (int)sliderxAchse.Value;
           // anzahlZellenHoch = (int)slideryAchse.Value;
            int gesamtAnzahlKnoten = anzahlZellenBreit * anzahlZellenHoch;


            // Grundarrays Initialisieren
            Rectangle[,] felder = new Rectangle[ProgrammWPFGame._anzahly, ProgrammWPFGame._anzahlx];
            Knoten NegativKnoten = new Knoten(-1, -1, -1, -1.0);
            NegativKnoten.SetInfektionszeit(-1.0);
            Knoten[] Knotenmenge = new Knoten[gesamtAnzahlKnoten];
            Kante[,] Kantenmenge = new Kante[gesamtAnzahlKnoten, 4];



            // Krankheitsausbreitung
            Knotenmenge = Programm.KrankheitsAusbreitung();


            
            

            // Konstruktion der visuellen Ausgabe
            for (int knoten = 0; knoten < gesamtAnzahlKnoten; knoten++)
            {

                SolidColorBrush myBrush = new SolidColorBrush();
                var r = new Rectangle();
                r.Width = zeichenfläche.ActualWidth / anzahlZellenBreit - 0.1;
                r.Height = zeichenfläche.ActualHeight / anzahlZellenHoch - 0.1;
                //r.Fill = (Knotenmenge[knoten].GetZustand() == 1) ? Brushes.Black : Brushes.Red;
                if (Knotenmenge[knoten].GetBisherigeZeit() != 0)
                {
                    myBrush.Color = Color.FromRgb(190, ByteKonstruktion(Knotenmenge[knoten].GetBisherigeZeit(), MaximumBisherigeZeit(Knotenmenge)),0);
                    r.Fill = myBrush;
                }
                else
                {
                    myBrush.Color = Color.FromRgb(20, 17, 29);
                    r.Fill = myBrush;
                }
                zeichenfläche.Children.Add(r);
                Canvas.SetLeft(r, Knotenmenge[knoten].GetXKoordinate() * zeichenfläche.ActualWidth / anzahlZellenBreit);
                Canvas.SetTop(r, Knotenmenge[knoten].GetYKoordinate() * zeichenfläche.ActualHeight / anzahlZellenHoch);
                felder[Knotenmenge[knoten].GetYKoordinate(), Knotenmenge[knoten].GetXKoordinate()] = r;
            }

       

    
            felder[anzahlZellenBreit/2, anzahlZellenHoch/2].Fill = Brushes.White;
           
           
            
        }

        
    }
}
