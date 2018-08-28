using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterArbeitGame
{


    public class Knoten
    {
        private int _xKoordinate;
        private int _yKoordinate;
        public int nichts = 0;
        // 0 gesund, 1 infiziert, 2 immun
        private int _zustand;
        private double _infektionszeit;
        private double _bisherigeZeit;



        private int _knotennummer;


        // Zustandsveränderung
        public static event EventHandler ZustandVerändert; // korrekturversuch

      



        protected virtual void OnZustandVerändert()
        {
            if (ZustandVerändert != null)
            {
                ZustandVerändert(this, EventArgs.Empty);
            }
        }

        public void Infiziere()
        {
            this.SetZustand(1);
            OnZustandVerändert();
        }


        //Konstruktoren


        public Knoten(int knotennummer, int xKoordinate, int yKoordinate, double infektionszeit)
        {
            SetXKoordinate(xKoordinate);
            SetYKoordinate(yKoordinate);
            SetKnotennummer(knotennummer);
            SetZustand(0);
            SetInfektionszeit(infektionszeit);
            SetbisherigeZeit(0.0);
           
        }


        //Koordinaten Getter und Setter

        public int GetXKoordinate()
        {
            return _xKoordinate;
        }
        public int GetYKoordinate()
        {
            return _yKoordinate;
        }
        public void SetXKoordinate(int xKoordinateEintragen)
        {
            this._xKoordinate = xKoordinateEintragen;
        }
        public void SetYKoordinate(int yKoordinateEintragen)
        {
            this._yKoordinate = yKoordinateEintragen;
        }

        // Nummer festlegen

        public void SetKnotennummer(int knotennummerEintragen)
        {
            this._knotennummer = knotennummerEintragen;
        }
        public int GetKnotennummer()
        {
            return _knotennummer;
        }

        // Zustand festlegen

        public void SetZustand(int zustandEintragen)
        {
            this._zustand = zustandEintragen;
        }

        public int GetZustand()
        {
            return _zustand;
        }



        // Infektionszeit festlegen
        public void SetInfektionszeit(double übertragungszeitEintragen)
        {
            this._infektionszeit = übertragungszeitEintragen;
        }
        public double GetInfektionszeit()
        {
            return _infektionszeit;
        }




        // bisherigeZeit festlegen

        public void SetbisherigeZeit(double bisherigeZeitEintragen)
        {
            this._bisherigeZeit = bisherigeZeitEintragen;
        }
        public double GetBisherigeZeit()
        {
            return _bisherigeZeit;
        }

       




    }
}
