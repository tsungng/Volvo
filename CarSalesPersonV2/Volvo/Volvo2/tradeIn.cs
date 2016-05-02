using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volvo2
{
    class tradeIn
    {
        private string make;
        private string model;
        private double msrp;
        private double year;
        private double mileage;
        private double tradeValue;
        private double depre;
        private string con;
        private string carDe;

        public tradeIn()
        {
            msrp = 0;
            year = 0;
            mileage = 0;
            tradeValue = 0;
            depre = 0;
            con = "";
        }

        public string Make
        {
            get
            {
                return make;
            }

            set
            {
                make = value;
            }
        }

        public string Model
        {
            get
            {
                return model;
            }

            set
            {
                model = value;
            }
        }

        public double Msrp
        {
            get
            {
                return msrp;
            }

            set
            {
                msrp = value;
            }
        }

        public double Year
        {
            get
            {
                return year;
            }

            set
            {
                year = value;
            }
        }

        public double Mileage
        {
            get
            {
                return mileage;
            }

            set
            {
                mileage = value;
            }
        }

        public double Depre
        {
            get
            {
                return depre;
            }

            set
            {
                depre = value;
            }
        }

        public double TradeValue
        {
            get
            {
                return tradeValue;
            }

            set
            {
                tradeValue = value;
            }
        }
        //car condition
        public string carCon { get { return con; } set { con = value; } }
        //car domestic/foreign
        public string carDest { get { return carDe; } set { carDe = value; } }
    }
}
