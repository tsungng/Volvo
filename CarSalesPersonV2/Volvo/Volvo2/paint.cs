﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volvo2
{
    class paint
    {
        private string color;
        private double price, paintPrice;
        private bool metallic;


        public paint()
        {
            price = 0;
            metallic = false;
        }
        public double paintP { get { return paintPrice; } set { paintPrice = value; } }
        public string Color
        {
            get
            {
                return color;
            }

            set
            {
                color = value;
            }
        }

        public double Price
        {
            get
            {
                return price;
            }

            set
            {
                price = value;
            }
        }

        public bool Metallic
        {
            get
            {
                return metallic;
            }

            set
            {
                metallic = value;
            }
        }

    }
}
