using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Windows.Forms;

namespace Volvo2
{
    class order
    {
        private int accountNumber;
        private string name, date;
        private string address;
        private string phoneNumber;
        private string model;
        private double subtotal;
        private double total;
        private double monthlyPrice;
        private string zipCode;
        private string state;
        private double fullPrice, cashPrice, taxPrice, carPrice, afterTradePrice;//financePrice;
        private int monthF;
        private readonly double tax, titleTags, finance, cash;

        public order()
        {
            accountNumber = 0;
            subtotal = 0;
            total = 0;
            monthlyPrice = 0;
            fullPrice = 0;
            monthF = 0;
            titleTags = 325;
            tax = 0.06;
            finance = 0.07;
            // financePrice = 0;
            taxPrice = 0;
            cash = 750;
            cashPrice = 0;
        }

        public int AccountNumber { get { return accountNumber; } set { accountNumber = value; } }
        public string Name { get { return name; } set { name = value; } }
        public string Address { get { return address; } set { address = value; } }
        public string PhoneNumber { get { return phoneNumber; } set { phoneNumber = value; } }
        public string Model { get { return model; } set { model = value; } }
        public double Subtotal { get { return subtotal; } set { subtotal = value; } }
        public double Total { get { return total; } set { total = value; } }
        public double MonthlyPrice { get { return monthlyPrice; } set { monthlyPrice = value; } }
        public string zipCoder { get { return zipCode; } set { zipCode = value; } }
        public string stateName { get { return state; } set { state = value; } }
        public double fullP { get { return fullPrice; } set { fullPrice = value; } }
        public double cashP { get { return cashPrice; } set { cashPrice = value; } }
        public int monthFin { get { return monthF; } set { monthF = value; } }
        public double priceCar { get { return carPrice; } set { carPrice = value; } }
        public double getTitlePrice() { return titleTags; }
        public double getTaxRate() { return tax; }
        public double getFinRate() { return finance; }
        public double taxP { get { return taxPrice; } set { taxPrice = value; } }
        public double afterTrade { get { return afterTradePrice; } set { afterTradePrice = value; } }
        public double getCashRate() { return cash; }
        public string Date { get {return date; } set {date = value; } }
        public void calculateCashP() { cashP -= cash; }
    }
}
