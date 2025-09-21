using static System.Formats.Asn1.AsnWriter;

namespace Gdzie_jest_dydelf
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private int x = 3;
        private int y = 3;
        private int dydelfs = 1;
        private int szopy = 3;
        private int krokodyl = 0;
        private int czas=10;

        private void button1_Click(object sender, EventArgs e)
        {
            Form3 gra = new Form3(x, y, dydelfs, szopy, krokodyl, czas);
            gra.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (Form2 form2 = new Form2())
            {
                if (form2.ShowDialog() == DialogResult.OK)
                {
                    x = form2.ParamX;
                    y = form2.ParamY;
                    dydelfs = form2.ParamDydelfs;
                    szopy = form2.ParamSzopy;
                    krokodyl = form2.ParamKrokodyl;
                    czas = form2.ParamCzas;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
