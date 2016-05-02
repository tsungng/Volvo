using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.IO;

namespace Volvo2
{
    public partial class frmVolvoProgram : Form
    {
        paint paint1 = new paint();
        tradeIn trade1 = new tradeIn();
        order order1 = new order();
        package package1 = new package();
        bool paymentOptionSelected = false;
        bool paymentOptionProcessed = false;
        readonly int locX = 0, locY = 0, locPX, locPY, slTimer = 500, panelX = 244, panelY = 33 , formX = 983, formY= 550;
        string []stringArray = { "..\\..\\..\\pictures\\s60\\Black Stone\\s60blackstone",
                                  "..\\..\\..\\pictures\\s80\\Black Stone\\s80blackstone",
                                  "..\\..\\..\\pictures\\s90\\Black Stone\\s90blackstone",
                                  "..\\..\\..\\pictures\\xc60\\Black Stone\\xc60blackstone",
                                  "..\\..\\..\\pictures\\s60\\Ice White\\s60icewhite",
                                  "..\\..\\..\\pictures\\s80\\Ice White\\s80icewhite",
                                  "..\\..\\..\\pictures\\s90\\Ice White\\s90icewhite",
                                  "..\\..\\..\\pictures\\xc60\\Ice White\\xc60icewhite",
                                  "..\\..\\..\\pictures\\s60\\Flamenco Red Metallic\\s60flamencoredmetallic",
                                  "..\\..\\..\\pictures\\s80\\Ember Black Metallic\\s80emberblackmetallic",
                                  "..\\..\\..\\pictures\\s90\\Luminous Sand Metallic\\s90luminoussandmetallic",
                                  "..\\..\\..\\pictures\\xc60\\Rich Java Metallic\\xc60richjavametallic",
                                  "..\\..\\..\\pictures\\s60\\Seashell Metallic\\s60seashellmetallic",
                                  "..\\..\\..\\pictures\\s80\\Magic Blue Metallic\\s80magicbluemetallic",               
                                  "..\\..\\..\\pictures\\s90\\Mussel Blue Metallic\\s90musselbluemetallic", 
                                  "..\\..\\..\\pictures\\xc60\\Magic Blue Metallic\\xc60magicbluemetallic" };
        string videoAD = "VolvoAd.mp4";
        Image[,] imageArray = new Image[16,5];
        Panel[] panelList = new Panel[6];

        public frmVolvoProgram()
        {
            Thread t1 = new Thread(new ThreadStart(splashScreen));
            t1.Start();
            Thread.Sleep(5000);
            t1.Abort();
            InitializeComponent();
            generateRandomLabel();
            imageLoad();
            locPX = picSelected.Width;
            locPY = picSelected.Height;
            this.Size = new Size(formX, formY);
            panelList[0] = panel1;
            panelList[1] = panel2;
            panelList[2] = panel3;
            panelList[3] = panel4;
            panelList[4] = panel5;
            panelList[5] = pnlDataRetrieval;
            panelLoc();
            wmpVolvoAd.Visible = false;
            picSelected.Image = null;
            picSummary.Image = null;
        }

        public void calculateOrderPrice()
        {
            order1.fullP = 0;
            order1.Subtotal = 0;
            order1.Total = 0;
            order1.fullP += order1.priceCar;
            order1.fullP += order1.getTitlePrice();
            order1.fullP += paint1.paintP;
            order1.fullP += package1.Price;
            order1.Subtotal = order1.fullP;
            order1.fullP *= (1 + order1.getTaxRate());
            order1.Total = order1.fullP;
        }
        public void calculatefinanceP()
        {
            //calculateOrderPrice();
            double temp = (1 + (order1.getFinRate() / 12));
            if (!chkTradeIn.Checked)
                order1.MonthlyPrice = ((order1.fullP) * (order1.getFinRate() / 12)) / (1 - Math.Pow(temp, (order1.monthFin * -1)));
            else
                order1.MonthlyPrice = ((order1.fullP - trade1.TradeValue) * (order1.getFinRate() / 12)) / (1 - Math.Pow(temp, (order1.monthFin * -1)));
            //MessageBox.Show(order1.afterTrade.ToString());

        }
        public void calculateCashP()
        {
            calculateOrderPrice();
            if (trade1.TradeValue != 0 && chkTradeIn.Checked)
                order1.cashP = order1.fullP - trade1.TradeValue - order1.getCashRate();
            else
                order1.cashP = order1.fullP - order1.getCashRate();
            lblCash.Text = order1.cashP.ToString("C");
        }
        public void displaySummary()
        {
            string carSum = "", tradeSum = "", financeSum = "", cashSum = "";
            carSum += "Name: " + order1.Name;
            carSum += "\nAddress: " + order1.Address;
            carSum += "\nZip: " + order1.zipCoder;
            carSum += "\nPhone Number: " + order1.PhoneNumber;
            carSum += "\nState: " + order1.stateName;
            carSum += "\nModel: " + order1.Model;
            carSum += "\nMSRP: " + order1.priceCar.ToString("C");
            carSum += "\nColor: " + paint1.Color;
            if (paint1.Metallic)
                carSum += "\nMetallic: Yes";
            else
                carSum += "\nMetallic: No";
            carSum += "\nPackage: " + package1.Name;
            carSum += "\nTitle and Tags: " + order1.getTitlePrice().ToString("C");
            carSum += "\nTax: " + order1.getTaxRate().ToString("P1");
            carSum += "\nTotal Price: " + order1.fullP.ToString("C");
            if (chkTradeIn.Checked)
            {
                tradeSum += "Make: " + trade1.Make;
                tradeSum += "\nModel: " + trade1.Model;
                tradeSum += "\nMileage: " + trade1.Mileage;
                tradeSum += "\nYear: " + trade1.Year;
                tradeSum += "\nCountry: " + trade1.carDest;
                tradeSum += "\nCondition: " + trade1.carCon;
                tradeSum += "\nMSRP: " + trade1.Msrp.ToString("C");
                tradeSum += "\nTrade-In Value: " + trade1.TradeValue.ToString("C");
                //tradeSum += "\nAfter Trade-In: " + order1.afterTrade.ToString("C");
                lblSummaryTrade.Visible = true;
                lblSummaryTrade.Text = tradeSum;
            }
            else
                lblSummaryTrade.Visible = false;
            if (radoFinance.Checked)
            {
                financeSum += "Months: " + order1.monthFin;
                financeSum += "\nAPR: " + order1.getFinRate().ToString("P1"); ;
                financeSum += "\nPrice per month: " + order1.MonthlyPrice.ToString("C");
                lblSummaryFin.Visible = true;
                lblSummaryFin.Text = financeSum;
            }
            else
                lblSummaryFin.Visible = false;
            if (radoCash.Checked)
            {
                cashSum += "Cash Discount: " + order1.getCashRate().ToString("C");
                cashSum += "\nCash Price: " + order1.cashP.ToString("C");
                lblSummaryCash.Visible = true;
                lblSummaryCash.Text = cashSum;
            }
            else
                lblSummaryCash.Visible = false;
            lblSummaryCar.Text = carSum;
        }

        public void splashScreen()
        {
            Application.Run(new SplashScreen());
        }

        public void playVideo()
        {
            try{
                wmpVolvoAd.Show();
                wmpVolvoAd.URL = videoAD;
                wmpVolvoAd.Dock = DockStyle.Fill;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(),"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        public void panelLoc()
        {
            for(int i =0; i<panelList.Length; i++)
            {
                panelList[i].Location= new Point(panelX, panelY);
                panelList[i].Visible = false;
            }
            panelList[0].Visible = true;
        }

        public void imageLoad()
        {
            try
            {
                for(int i =0; i< 16; i++)
                {
                    for(int k = 0; k < 5; k++)
                    {
                        imageArray[i,k] = Image.FromFile(stringArray[i] + (k + 1) + ".jpg");
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Did not load images. Please check the file path(s)!\n" + ex.ToString(),"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        public void calctradein()
        {
            double tradeinvalue, value, depreciation = 0, condition = 1, year;
            trade1.Mileage = Convert.ToDouble(txtMileage.Text);
            trade1.Year = Convert.ToDouble(cbotradeyear.SelectedItem);
            year = 2016 - trade1.Year;
            value = Convert.ToDouble(txttradeinmsrp.Text);
            trade1.Msrp = value;
            trade1.Make = txtMake.Text;
            trade1.Model = txtModel.Text;
            if (rdotradeindomestic.Checked)
            {
                depreciation = .85;
                trade1.Depre = .85;
                trade1.carDest = rdotradeindomestic.Text;
            }
            if (rdotradeinforeign.Checked)
            {
                depreciation = .9;
                trade1.Depre = .9;
                trade1.carDest = rdotradeinforeign.Text;
            }
            if (rdotradeinconditionpoor.Checked)
            {
                condition = .7;
                trade1.carCon = rdotradeinconditionpoor.Text;
            }
            if (rdotradeinconditiongood.Checked)
            {
                condition = 1;
                trade1.carCon = rdotradeinconditiongood.Text;
            }
            if (rdotradeinconditionexcellent.Checked)
            {
                trade1.carCon = rdotradeinconditionexcellent.Text;
                condition = 1;
            }
            for (int i = 0; i < year; i++)
            {
                value = value * depreciation;

            }
            if (trade1.Mileage > (10000 * year))
                value = value * .8;
            tradeinvalue = value * condition;
            trade1.TradeValue = tradeinvalue;
            lblTradeIn.Text = trade1.TradeValue.ToString("C");
        }

        private void Form1_Load(object sender, System.EventArgs e)
        {
            for (int i = 2016; i > 1900; i--)
            {
                cbotradeyear.Items.Add(i);
            }
        }
        private void btnNext1_Click(object sender, System.EventArgs e)
        {
            if (validateForm() == true)
            {
                panelList[1].Visible = true;
                lblNameSummary2.Text = txtName.Text;
                order1.Name = txtName.Text;
                order1.PhoneNumber = txtPhoneNumber.Text;
                order1.stateName = cmboState.Text;
                order1.zipCoder = txtZip.Text;
                order1.AccountNumber = Convert.ToInt32(lblRandomSummary2.Text);
                order1.Address = txtAddress.Text;
            }
        }

        private void btnClear_Click(object sender, System.EventArgs e)
        {
            txtName.Text = "";
            txtAddress.Text = "";
            txtZip.Text = "";
            txtPhoneNumber.Text = "";
            cmboState.Text = "";
            lblNameSummary2.Text = "";
        }

        private void btnBack1_Click(object sender, System.EventArgs e)
        {
            panelList[1].Visible = false;
            panelList[0].Visible = true;
        }

        private void btnNext2_Click(object sender, System.EventArgs e)
        {
            if (radoS40.Checked || radoS60.Checked || radoS70.Checked || radoS80.Checked)
            {
                lblModelError.Visible = false;
                panelList[2].Visible = true;
                panelList[1].Visible = false;
                //panel3.Visible = true;
                picSelected.Image = null;
                radColorBlue.Text = "Blue";
                if (radoS60.Checked)
                    radColorRed.Text = "Ember Black";
                else if (radoS70.Checked)
                    radColorRed.Text = "Luminous Sand";
                else if (radoS40.Checked)
                {
                    radColorRed.Text = "Red";
                    radColorBlue.Text = "Seashell";
                }
                else if (radoS80.Checked)
                    radColorRed.Text = "Rich Java";              
                if (radoS40.Checked)
                {
                    picSummary.Image = picS40.Image;
                    lblModelSummary2.Text = "S40";
                    picBlack.Image = imageArray[0, 0];
                    picWhite.Image = imageArray[4, 0];
                    picRed.Image = imageArray[8, 0];
                    picBlue.Image = imageArray[12, 0];
                    order1.priceCar = 34150;
                    radPackB.Visible = false;
                    picPackB.Visible = false;
                }
                else if (radoS60.Checked)
                {
                    picSummary.Image = picS60.Image;
                    lblModelSummary2.Text = "S60";
                    picBlack.Image = imageArray[1, 0];
                    picWhite.Image = imageArray[5, 0];
                    picRed.Image = imageArray[9, 0];
                    picBlue.Image = imageArray[13, 0];
                    order1.priceCar = 43450;
                    radPackB.Visible = false;
                    picPackB.Visible = false;
                }
                else if (radoS70.Checked)
                {
                    picSummary.Image = picS70.Image;
                    lblModelSummary2.Text = "S70";
                    picBlack.Image = imageArray[2, 0];
                    picWhite.Image = imageArray[6, 0];
                    picRed.Image = imageArray[10, 0];
                    picBlue.Image = imageArray[14, 0];
                    order1.priceCar = 46950;
                    radPackB.Visible = true;
                    picPackB.Visible = true;
                }
                else if (radoS80.Checked)
                {
                    picSummary.Image = picS80.Image;
                    lblModelSummary2.Text = "S80";
                    picBlack.Image = imageArray[3, 0];
                    picWhite.Image = imageArray[7, 0];
                    picRed.Image = imageArray[11, 0];
                    picBlue.Image = imageArray[15, 0];
                    order1.priceCar = 68100;
                    radPackB.Visible = true;
                    picPackB.Visible = true;
                }
                order1.Model = lblModelSummary2.Text;
                lblMSRPSummary2.Text = order1.priceCar.ToString("C");
            }
            else
                lblModelError.Visible = true;                
        }

        private void btnNext3_Click(object sender, EventArgs e)
        {
            if ((radColorBlack.Checked || radColorBlue.Checked || radColorRed.Checked || radColorWhite.Checked)
                && (radPackA.Checked || radStandardPack.Checked || radPackB.Checked))
            {
                lblColorPackageError.Visible = false;
                panelList[3].Visible = true;
                panelList[2].Visible = false;
                //panel4.Visible = true;
                lblPackageSummary2.Visible = true;
                lblPaintSummary2.Visible = true;
                if (radPackA.Checked)
                {
                    lblPackageSummary2.Text = "A (+$2200)";
                    package1.Price = 2200;
                    package1.Name = radPackA.Text;
                   
                }
                else if (radPackB.Checked)
                {
                    lblPackageSummary2.Text = "B (+$3250)";
                    package1.Price = 3250;
                    package1.Name = radPackB.Text;
                }
                else if (radStandardPack.Checked)
                {
                    lblPackageSummary2.Text = "Standard";
                    package1.Price = 0;
                    package1.Name = radStandardPack.Text;
                }
                if (chkMetallic.Checked)
                {
                    lblPaintSummary2.Text = "Metallic (+$650)";
                    paint1.Metallic = true;
                    paint1.paintP = 650;
                }
                else
                {
                    lblPaintSummary2.Text = "Gloss";
                    paint1.Metallic = false;
                    paint1.paintP = 0;
                }
            }
            else
                lblColorPackageError.Visible = true;              
        }

        private void generateRandomLabel()
        {
            Random rng = new Random();
            int acc = rng.Next(12345, 97959);
            lblRandom.Text = acc.ToString();
            lblRandomSummary2.Text = acc.ToString();
        }

        /*
            VALIDATION BELOW THIS LINE
        */

        public bool validateForm()
        {
            bool valid = true;
            Regex regName = new Regex(@"^.{1,30}$");
            Regex regZip = new Regex(@"^\d{5}(-\d{4})?$");
            Regex regNumber = new Regex(@"^\(?\d{3}\)?-?\d{3}-?\d{4}$");

            if (!regName.IsMatch(txtName.Text))
            {
                lblNameError.Visible = true;
                valid = false;
            }
            else
            {
                lblNameError.Visible = false;
            }
            if (!regName.IsMatch(txtAddress.Text))
            {
                lblAddressError.Visible = true;
                valid = false;
            }
            else
            {
                lblAddressError.Visible = false;
            }
            if (!regZip.IsMatch(txtZip.Text))
            {
                lblZipError.Visible = true;
                valid = false;
            }
            else
            {
                lblZipError.Visible = false;
            }
            if (!regNumber.IsMatch(txtPhoneNumber.Text))
            {
                lblPhoneError.Visible = true;
                valid = false;
            }
            else
            {
                lblPhoneError.Visible = false;
            }
            if (cmboState.SelectedIndex == -1)
            {
                lblStateError.Visible = true;
                valid = false;
            }
            else
            {
                lblStateError.Visible = false;
            }
            return valid;

        }

        private void cmboMonth_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (cmboMonth.SelectedIndex == -1)
                MessageBox.Show("Please select months from the finance tab!");
        }
        /*
            VALIDATION ENDS HERE
        */
        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult res = new DialogResult();
            res = MessageBox.Show("Are you sure you want to exit?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
                this.Close();
        }

        private void btnBack2_Click(object sender, EventArgs e)
        {
            panelList[2].Visible = false;
            panelList[1].Visible = true;
        }

        private void btnBack3_Click(object sender, EventArgs e)
        {
            panelList[3].Visible = false;
            panelList[2].Visible = true;
        }

        private void radColorBlack_CheckedChanged(object sender, EventArgs e)
        {
            if (radColorBlack.Checked)
            {
                for (int i = 0; i < 5; i++)
                {
                        picSelected.Image = null;
                        if (radoS40.Checked)
                            picSelected.CreateGraphics().DrawImage(imageArray[0, i], locX, locY, locPX, locPY);
                        else if (radoS60.Checked)
                            picSelected.CreateGraphics().DrawImage(imageArray[1, i], locX, locY, locPX, locPY);
                        else if (radoS70.Checked)
                            picSelected.CreateGraphics().DrawImage(imageArray[2, i], locX, locY, locPX, locPY);
                        else if (radoS80.Checked)
                            picSelected.CreateGraphics().DrawImage(imageArray[3, i], locX, locY, locPX, locPY);
                        Thread.Sleep(slTimer);
                }
            }
            paint1.Color = radColorBlack.Text;
            picSelected.Image = picSummary.Image = picBlack.Image;
        }

        private void radColorWhite_CheckedChanged(object sender, EventArgs e)
        {
            if (radColorWhite.Checked)
            {
                for (int i = 0; i < 5; i++)
                {
                    picSelected.Image = null;
                    if (radoS40.Checked)
                        picSelected.CreateGraphics().DrawImage(imageArray[4, i], locX, locY, locPX, locPY);
                    else if (radoS60.Checked)
                        picSelected.CreateGraphics().DrawImage(imageArray[5, i], locX, locY, locPX, locPY);
                    else if (radoS70.Checked)
                        picSelected.CreateGraphics().DrawImage(imageArray[6, i], locX, locY, locPX, locPY);
                    else if (radoS80.Checked)
                        picSelected.CreateGraphics().DrawImage(imageArray[7, i], locX, locY, locPX, locPY);
                    Thread.Sleep(slTimer);
                }
            }
            paint1.Color = radColorWhite.Text;
            picSelected.Image = picSummary.Image = picWhite.Image;
        }

        private void radColorRed_CheckedChanged(object sender, EventArgs e)
        {
            if (radColorRed.Checked)
            {
                for (int i = 0; i < 5; i++)
                {
                    picSelected.Image = null;
                    if (radoS40.Checked)
                        picSelected.CreateGraphics().DrawImage(imageArray[8, i], locX, locY, locPX, locPY);
                    else if (radoS60.Checked)
                        picSelected.CreateGraphics().DrawImage(imageArray[9, i], locX, locY, locPX, locPY);
                    else if (radoS70.Checked)
                        picSelected.CreateGraphics().DrawImage(imageArray[10, i], locX, locY, locPX, locPY);
                    else if (radoS80.Checked)
                        picSelected.CreateGraphics().DrawImage(imageArray[11, i], locX, locY, locPX, locPY);
                    Thread.Sleep(slTimer);

                }
            }
            paint1.Color = radColorRed.Text;
            picSelected.Image = picSummary.Image = picRed.Image;
        }

        private void radColorBlue_CheckedChanged(object sender, EventArgs e)
        {

            if (radColorBlue.Checked)
            {
                for (int i = 0; i < 5; i++)
                {
                    picSelected.Image = null;
                    if (radoS40.Checked)
                        picSelected.CreateGraphics().DrawImage(imageArray[12, i], locX, locY, locPX, locPY);
                    else if (radoS60.Checked)
                        picSelected.CreateGraphics().DrawImage(imageArray[13, i], locX, locY, locPX, locPY);
                    else if (radoS70.Checked)
                        picSelected.CreateGraphics().DrawImage(imageArray[14, i], locX, locY, locPX, locPY);
                    else if (radoS80.Checked)
                        picSelected.CreateGraphics().DrawImage(imageArray[15, i], locX, locY, locPX, locPY);
                    Thread.Sleep(slTimer);
                }
            }
            paint1.Color = radColorBlue.Text;
            picSelected.Image = picSummary.Image = picBlue.Image;
        }

        private void btnTradeIn_Click(object sender, EventArgs e)
        {
            calculateOrderPrice();
            order1.afterTrade = order1.fullP - trade1.TradeValue;
        }

        private void radoFinance_CheckedChanged(object sender, EventArgs e)
        {
            if (radoFinance.Checked)
            {
                grpFinance.Visible = true;
                grpCash.Visible = false;
                paymentOptionSelected = true;
            }
            else
            {
                grpFinance.Visible = false;
                paymentOptionSelected = false;
            }
        }

        private void radoCash_CheckedChanged(object sender, EventArgs e)
        {
            if (radoCash.Checked)
            {
                grpCash.Visible = true;
                grpFinance.Visible = false;
                paymentOptionSelected = true;
            }
            else
            {
                grpCash.Visible = false;
                paymentOptionSelected = false;
            }
        }
        private void chkTradeIn_Click(object sender, EventArgs e)
        {
            if (chkTradeIn.Checked)
            {
                grpTradeIn.Visible = true;
                lblTradeInSelect.Visible = true;
            }
            else {
                grpTradeIn.Visible = false;
                lblTradeInSelect.Visible = false;
            }
        }

        private void playToolStripMenuItem_Click(object sender, EventArgs e)
        {
            playVideo();
        }

        private void closeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            wmpVolvoAd.close();
            wmpVolvoAd.Hide();
        }
        //Navigator on Panel 5--Data Retrieval Panel
        private void bindingNavigator1_RefreshItems(object sender, EventArgs e)
        {

        }
        //Button to Add Customer Info on Panel 5--Data Retrieval Panel
        private void btnCustAdd_Click(object sender, EventArgs e)
        {
            List<string> customers = new List<string>();
            using (StreamReader r = new StreamReader("..\\customer.txt"))
            {
                string line;
                while ((line = r.ReadLine()) != null)
                {
                    customers.Add(line);
                }
            }
            lstCustInfo.DataSource = customers;
        }

        private void SaveCustomer_Click(object sender, EventArgs e)
        {
            string name = txtName.Text;
            string address = txtAddress.Text;
            string zip = txtZip.Text;
            string phone = txtPhoneNumber.Text;
            string state = cmboState.SelectedItem.ToString();
            string order = lblRandomSummary2.Text;

            using (StreamWriter sw = new StreamWriter("..\\customer.txt", true))
            {
                sw.WriteLine(string.Join(",", new object[] { name, address, zip, phone, state, order }));
            }
        }

        private void btnExistingCustomer_Click(object sender, EventArgs e)
        {
            panelList[5].Visible = true;
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            panelList[5].Visible = false;
            txtName.Text = txtCustomerName.Text;
            txtAddress.Text = txtCustAddress.Text;
            txtZip.Text = txtCustZip.Text;
            txtPhoneNumber.Text = txtCustPhone.Text;
            cmboState.SelectedItem = txtCustState.Text;
            lblRandomSummary2.Text = txtCustOrder.Text;
            lblRandom.Text = txtCustOrder.Text;
            //int index = cmboState.Items.IndexOf(txtCustState.Text);


        }

        private void lstCustInfo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string tempstring = lstCustInfo.SelectedItem.ToString() ;
            string[] info = tempstring.Split(',');
            txtCustomerName.Text = info[0];
            txtCustAddress.Text = info[1];
            txtCustState.Text = info[4];
            txtCustZip.Text = info[2];
            txtCustPhone.Text = info[3];
            txtCustOrder.Text = info[5];
        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            DialogResult res = new DialogResult();
            res = MessageBox.Show("Submit all information?", "Submit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                MessageBox.Show("Information submitted!");
                Close();
            }
        }

        private void btnBackSummary_Click(object sender, EventArgs e)
        {
            panelList[4].Visible = false;
            panelList[3].Visible = true;
        }

        private void chkTradeIn_CheckedChanged(object sender, EventArgs e)
        {
            grpTradeIn.Visible = true;
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult res = new DialogResult();
            res = MessageBox.Show("Are you sure you want to exit without saving?","Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            if (res == DialogResult.Yes)
                Close();
        }

        private void btnNext4_Click(object sender, EventArgs e)
        {
            bool valid = true;

            if (paymentOptionSelected == false)
            {
                lblPaymentError.Visible = true;
                valid = false;
            }
            else
            {
                lblPaymentError.Visible = false;
            }
            if (paymentOptionProcessed == false)
            {
                lblPaymentProcessError.Visible = true;
                valid = false;
            }
            else
            {
                lblPaymentProcessError.Visible = false;
            }
            if (valid == true)
            {
                if (radoFinance.Checked)
                {
                    lblModePaymentSum2.Text = "Finance " + cmboMonth.Text + " Mon.";
                }
                if (radoCash.Checked)
                {
                    lblModePaymentSum2.Text = "Cash";
                }
                lblTotalSum2.Text = order1.Total.ToString("C");
                lblsubtotalsum2.Text = order1.Subtotal.ToString("C");
                lblModePaymentSum2.Visible = true;
                lblsubtotalsum2.Visible = true;
                lblTotalSum2.Visible = true;
                displaySummary();
                panelList[4].Visible = true;
                panelList[3].Visible = false;
               // panel5.Visible = true;
            }
        }

        private void btntradeincalculate_Click_1(object sender, EventArgs e)
        {
            bool valid = true;
            Regex regMake = new Regex(@"^[A-z]{1,20}$");
            Regex regMod = new Regex(@"^.{1,20}$");
            Regex regMile = new Regex(@"^\d{1,6}$");
            Regex regMsrp = new Regex(@"^\d{1,6}$");

            if (!regMake.IsMatch(txtMake.Text))
            {
                valid = false;
                lblMakeError.Visible = true;
            }
            else
            {
                lblMakeError.Visible = false;
            }

            if (!regMod.IsMatch(txtModel.Text))
            {
                valid = false;
                lblModelError2.Visible = true;
            }
            else
            {
                lblModelError2.Visible = false;
            }
            if (!regMile.IsMatch(txtMileage.Text))
            {
                valid = false;
                lblMileageError.Visible = true;
            }
            else
            {
                lblMileageError.Visible = false;
            }
            if (!regMsrp.IsMatch(txttradeinmsrp.Text))
            {
                valid = false;
                lblMSRPError.Visible = true;
            }
            else
            {
                lblMSRPError.Visible = false;
            }
            if (rdotradeindomestic.Checked == false && rdotradeinforeign.Checked == false)
            {
                valid = false;
                lblDomesticError.Visible = true;
            }
            else
            {
                lblDomesticError.Visible = false;
            }
            if (rdotradeinconditionexcellent.Checked == false && rdotradeinconditiongood.Checked == false && rdotradeinconditionpoor.Checked == false)
            {
                valid = false;
                lblConditionError.Visible = true;
            }
            else
            {
                lblConditionError.Visible = false;
            }

            if (valid == true)
            {
                calctradein();
            }
        }

        private void btnCash_Click(object sender, EventArgs e)
        {
            paymentOptionProcessed = true;
            calculateCashP();
        }

        private void btnFinance_Click(object sender, EventArgs e)
        {

            if (cmboMonth.Text != "")
            {
                paymentOptionProcessed = true;
                lblMonthError.Visible = false;
                double months = 0;
                months = Convert.ToDouble(cmboMonth.Text);
                order1.monthFin = (int)months;
                calculateOrderPrice();
                calculatefinanceP();
                lblFinanceP.Text = order1.MonthlyPrice.ToString("C");
            }
            else
            {
                lblMonthError.Visible = true;
            }
        }
    }
}

