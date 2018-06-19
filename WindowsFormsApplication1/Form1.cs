using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{

    public partial class Form1 : Form
    {
        static class Constants
        {
            public const int ocjena = 5;
            public const int noOfSemester = 6;
        }

        string text1;
        int txtToECTS, txtToOcjena, txtToAkGodina,txtToSemestar;
        List<String> predmeti = new List<String>();
        List<Predmet> upisani = new List<Predmet>();
        List<String> semestri = new List<string>();
        decimal prosjek=0;
        decimal tezinskiProsjek=0;
        DateTime date;
        DataGridView dataGV;
        public Form1()
        {
            InitializeComponent();
            DBCommunation.refreshComboBoxSubject(ref comboBox3);
            List<String> combo = DBCommunation.refreshComboBox();
            comboBox4.Items.Clear();
            foreach (String element in combo)
                comboBox4.Items.Add(element);

            for (int i = 1; i <= Constants.noOfSemester; i++)
                 comboBox2.Items.Add(Convert.ToString(i) + ". semestar");

            for (int i = 1; i <= Constants.ocjena; i++)
                comboBox5.Items.Add(Convert.ToString(i));

            DBCommunation.refreshComboBoxSemester(ref semestri);
            foreach (string sem in semestri)
                    comboBox1.Items.Add(sem + ". semestar");
        }
        //dodaj
        private void button1_Click(object sender, EventArgs e)
        {
            string temp = (string)comboBox2.SelectedItem;
            temp = temp.Remove(1);
            if (DBCommunation.checkExistence((string)comboBox3.SelectedItem) == 1)
                DBCommunation.insertData((string)comboBox3.SelectedItem,Convert.ToInt16(comboBox5.SelectedItem), date,Convert.ToInt16(temp));
            else
                MessageBox.Show("Predmet je već upisan");
            
        }

     

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
        //izracunaj tezinski prosjek
        private void button2_Click_1(object sender, EventArgs e)
        {
            tezinskiProsjek = Math.Round(tezinskiProsjek, 2);
            tezinskiProsjek = DBCommunation.calculateHeavy();
            textBox6.Text = String.Format("{0:0.0000}", tezinskiProsjek);
        }
        //izracunaj prosjek
        private void button3_Click_1(object sender, EventArgs e)
        {
            prosjek = Math.Round(prosjek, 2);
            prosjek = DBCommunation.calculate();
            textBox7.Text = String.Format("{0:0.0000}", prosjek);
        }
        //ispis podataka
        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            int tempECTS = 0, tempOcjena = 0;
            DateTime datum = new DateTime();
            DBCommunation.printProperty((string)comboBox4.SelectedItem,ref tempECTS,ref tempOcjena,ref datum);
            textBox8.Text = (string)comboBox4.SelectedItem;
            textBox9.Text = Convert.ToString(tempECTS);
            textBox10.Text = Convert.ToString(tempOcjena);
            textBox11.Text = Convert.ToString(date);

        }
        //osvjezavanje padajuce liste
        private void button4_Click(object sender, EventArgs e)
        {
            
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        //padajuca lista semestara-osvjezi
        private void button7_Click(object sender, EventArgs e)
        {
            
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        //padajuca lista semestara
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = DBCommunation.getContent((string)comboBox1.SelectedItem);
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.Refresh();

        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox13_TextChanged(object sender, EventArgs e)
        {

        }
        //semestri
        private void comboBox2_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            
        }
        //predmeti
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            
            
        }

        //datum kad je predmet polozen
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
                date = dateTimePicker1.Value.Date;
        }

        //ocjena
        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

    }
}
