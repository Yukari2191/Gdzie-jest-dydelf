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
    public partial class Form3 : Form
    {
        private int rows;
        private int cols;
        private int totalDydelfs;
        private int czas;
        private Dictionary<(int, int), string> pola = new Dictionary<(int, int), string>();
        private HashSet<(int, int)> znalezioneDydelfy = new HashSet<(int, int)>();
        private System.Windows.Forms.Timer gameTimer;
        private Label lblStatus;
        private DataGridView dataGridView1;

        public Form3(int x, int y, int dydelfs, int szopy, int krokodyl, int czas)
        {
            InitializeComponent();

            this.rows = x;
            this.cols = y;
            this.totalDydelfs = dydelfs;
            this.czas = czas;
            this.Size = new Size(60 * cols + 50, 60 * rows + 100);

            dataGridView1 = new DataGridView
            {
                RowHeadersVisible = false,
                ColumnHeadersVisible = false,
                AllowUserToAddRows = false,
                ReadOnly = true,
                Width = 60 * cols,
                Height = 60 * rows,
                ScrollBars = ScrollBars.None
            };

            // Dodaj kolumny
            for (int i = 0; i < cols; i++)
            {
                var col = new DataGridViewTextBoxColumn();
                col.Width = 60;
                dataGridView1.Columns.Add(col);
            }

            // Dodaj wiersze
            for (int i = 0; i < rows; i++)
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[i].Height = 60;
            }

            // Ustaw domyślny kolor tła komórek na biały
            foreach (DataGridViewRow row in dataGridView1.Rows)
                foreach (DataGridViewCell cell in row.Cells)
                    cell.Style.BackColor = Color.White;

            // Dodaj zdarzenie kliknięcia
            dataGridView1.CellClick += dataGridView1_CellContentClick_1;

            // Dodaj kontrolkę do formularza
            Controls.Add(dataGridView1);


            foreach (DataGridViewColumn col in dataGridView1.Columns)
                col.Width = 60;

            foreach (DataGridViewRow row in dataGridView1.Rows)
                row.Height = 60;

            Controls.Add(dataGridView1);
            dataGridView1.CellClick += dataGridView1_CellContentClick_1;

            lblStatus = new Label { Text = $"Pozostało: {czas}s", Dock = DockStyle.Bottom, Height = 30, TextAlign = ContentAlignment.MiddleCenter };
            lblStatus.Location = new Point(10, dataGridView1.Bottom + 10);
            lblStatus.Size = new Size(dataGridView1.Width, 30);
            Controls.Add(lblStatus);

            RozmiescElementy(dydelfs, szopy, krokodyl);
            StartTimer();
        }

        private void RozmiescElementy(int dydelfs, int szopy, int krokodyl)
        {
            var rnd = new Random();
            var wszystkiePola = Enumerable.Range(0, rows * cols).OrderBy(_ => rnd.Next()).ToList();

            foreach (var idx in wszystkiePola.Take(dydelfs))
                pola[(idx / cols, idx % cols)] = "D";

            foreach (var idx in wszystkiePola.Skip(dydelfs).Take(szopy))
                pola[(idx / cols, idx % cols)] = "S";

            if (krokodyl > 0)
                pola[(wszystkiePola.Skip(dydelfs + szopy).First() / cols, wszystkiePola.Skip(dydelfs + szopy).First() % cols)] = "K";
        }

        private void StartTimer()
        {
            gameTimer = new System.Windows.Forms.Timer();
            gameTimer.Interval = 1000;
            int remaining = czas;
            gameTimer.Tick += (s, e) =>
            {
                remaining--;
                lblStatus.Text = $"Pozostało: {remaining}s";
                if (remaining <= 0)
                {
                    gameTimer.Stop();
                    ZakonczGre("Czas minął!");
                }
            };
            gameTimer.Start();
        }

        private void ZakonczGre(string message)
        {
            gameTimer?.Stop();
            MessageBox.Show(message);
            Close();
        }

        private bool krokodylKlikniety = false;
        private (int, int)? pozycjaKrokodyla = null;
        private System.Windows.Forms.Timer timerKrok = null;

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dataGridView1.RowCount || e.ColumnIndex < 0 || e.ColumnIndex >= dataGridView1.ColumnCount || krokodylKlikniety)
                return;

            var pos = (e.RowIndex, e.ColumnIndex);
            if (pola.GetValueOrDefault(pos) != "K" && dataGridView1[e.ColumnIndex, e.RowIndex].Style.BackColor != Color.White && dataGridView1[e.ColumnIndex, e.RowIndex].Style.BackColor != Color.Green)
                return;

            if (!pola.ContainsKey(pos))
            {
                dataGridView1[e.ColumnIndex, e.RowIndex].Value = "X";
                dataGridView1[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.DarkGray;
                return;
            }

            string co = pola[pos];
            switch (co)
            {
                case "D":
                    if (!znalezioneDydelfy.Contains(pos))
                    {
                        znalezioneDydelfy.Add(pos);
                        dataGridView1[e.ColumnIndex, e.RowIndex].Value = "Dydelf";
                        dataGridView1[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.LightGray;
                        if (znalezioneDydelfy.Count == totalDydelfs)
                            ZakonczGre("Wygrałeś!");
                    }
                    break;

                case "S":
                    dataGridView1[e.ColumnIndex, e.RowIndex].Value = "Szop";
                    dataGridView1[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.Khaki;
                    var timerSzop = new System.Timers.Timer(2000);
                    timerSzop.Elapsed += (s, ev) =>
                    {
                        timerSzop.Stop();
                        Invoke(() =>
                        {
                            for (int dx = -1; dx <= 1; dx++)
                            {
                                for (int dy = -1; dy <= 1; dy++)
                                {
                                    int newRow = e.RowIndex + dx;
                                    int newCol = e.ColumnIndex + dy;
                                    if (newRow >= 0 && newRow < rows && newCol >= 0 && newCol < cols)
                                    {
                                        dataGridView1[newCol, newRow].Value = "";
                                        dataGridView1[newCol, newRow].Style.BackColor = Color.White;
                                    }
                                }
                            }
                        });
                    };
                    timerSzop.Start();
                    break;

                case "K":
                    if (!krokodylKlikniety)
                    {
                        krokodylKlikniety = true;
                        pozycjaKrokodyla = pos;

                        dataGridView1[e.ColumnIndex, e.RowIndex].Value = "Krokodyl";
                        dataGridView1[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.Green;

                        timerKrok = new System.Windows.Forms.Timer();
                        timerKrok.Interval = 2000;
                        timerKrok.Tick += (s2, ev2) =>
                        {
                            timerKrok.Stop();
                            ZakonczGre("Przegrałeś! Trafiłeś na krokodyla.");
                        };
                        timerKrok.Start();
                    }
                    else if (pozycjaKrokodyla == pos && timerKrok != null && timerKrok.Enabled)
                    {
                        timerKrok.Stop();
                        krokodylKlikniety = false;
                        pozycjaKrokodyla = null;
                        dataGridView1[e.ColumnIndex, e.RowIndex].Value = "";
                        dataGridView1[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.White;
                    }
                    break;
            }
        }

    }
}
