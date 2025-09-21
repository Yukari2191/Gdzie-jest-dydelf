using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gdzie_jest_dydelf
{
    public partial class Form2 : Form
    {
        public int ParamX { get; private set; }
        public int ParamY { get; private set; }
        public int ParamDydelfs { get; private set; }
        public int ParamSzopy { get; private set; }
        public int ParamKrokodyl { get; private set; }
        public int ParamCzas { get; private set; }

        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                ParamX = int.Parse(textBox1.Text);
                ParamY = int.Parse(textBox2.Text);
                ParamDydelfs = int.Parse(textBox3.Text);
                ParamSzopy = int.Parse(textBox6.Text);
                ParamKrokodyl = int.Parse(textBox4.Text);
                ParamCzas = int.Parse(textBox5.Text);

                // Sprawdzenie zakresów
                if (ParamX < 3 || ParamX > 10)
                {
                    MessageBox.Show("Liczba wierszy (X) musi być od 3 do 10.");
                    return;
                }
                if (ParamY < 3 || ParamY > 10)
                {
                    MessageBox.Show("Liczba kolumn (Y) musi być od 3 do 10.");
                    return;
                }
                if (ParamCzas < 10 || ParamCzas > 60)
                {
                    MessageBox.Show("Czas musi być od 10 do 60 sekund.");
                    return;
                }
                if (ParamDydelfs < 1 || ParamDydelfs > 6)
                {
                    MessageBox.Show("Liczba dydelfów musi być od 1 do 6.");
                    return;
                }
                if (ParamSzopy < 3 || ParamSzopy > 8)
                {
                    MessageBox.Show("Liczba szopów musi być od 3 do 8.");
                    return;
                }
                if (ParamKrokodyl < 0 || ParamKrokodyl > 1)
                {
                    MessageBox.Show("Liczba krokodyli może być tylko 0 lub 1.");
                    return;
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (FormatException)
            {
                MessageBox.Show("Wprowadź poprawne liczby we wszystkich polach.");
            }
        }
    }
}
