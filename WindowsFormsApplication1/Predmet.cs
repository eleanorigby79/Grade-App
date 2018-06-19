using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication1
{
    public class Predmet
    {
        public int ID { get; set; }
        public String imePredmeta { get; set; }
        public int ECTS { get; set; }
        public int ocjena { get; set; }
        public DateTime datum { get; set; }

        public Predmet(int ID, string imePredmeta, int ECTS, int ocjena, DateTime datum)
        {
            this.ID = ID;
            this.imePredmeta = imePredmeta;
            this.ECTS = ECTS;
            this.ocjena = ocjena;
            this.datum = datum;
        }
    }
}
