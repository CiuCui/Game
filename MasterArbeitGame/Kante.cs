using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterArbeitGame
{
    public class Kante
    {
        private Knoten _startKnoten;
        private Knoten _endKnoten;

        private double _übertragungsdauer = 0;


        public Kante(Knoten startknotenEintragen, Knoten endknotenEintragen, double übertragungsdauerEintragen)
        {
            this.SetStartKnoten(startknotenEintragen);
            this.SetEndKnoten(endknotenEintragen);

            this.SetÜbertragungsDauer(übertragungsdauerEintragen);
        }

        public Knoten GetEndknoten()
        {
            return _endKnoten;
        }
        public void SetEndKnoten(Knoten endknotenEintragen)
        {
            this._endKnoten = endknotenEintragen;

        }

        public void SetStartKnoten(Knoten startknotenEintragen)
        {
            this._startKnoten = startknotenEintragen;
        }
        public Knoten GetStartknoten()
        {
            return _startKnoten;
        }

        public void SetÜbertragungsDauer(double übertragungsdauerEintragen)
        {
            this._übertragungsdauer = übertragungsdauerEintragen;
        }
        public double GetÜbertragungsDauer()
        {
            return _übertragungsdauer;
        }



    }
}
