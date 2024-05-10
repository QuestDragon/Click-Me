using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CLICK_ME_For_C_Sharp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            comboBox1.Items.Add("未設定");
            comboBox1.Items.Add("10秒");
            comboBox1.Items.Add("30秒");
            comboBox1.Items.Add("60秒");
            comboBox1.SelectedIndex = 0;
        }

        int A = 0;

        private void button1_Click(object sender, EventArgs e)
        {
            A = A + 1;
            label1.Text = A.ToString();
        }

        private void End_Click(object sender, EventArgs e)
        {
            DialogResult exit = MessageBox.Show("終了しますか？",  "Click Me!! For C Sharp",MessageBoxButtons.YesNo , MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
            if ( exit == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            A = 0;
            label1.Text = A.ToString();
            button1.Enabled = true;
            button1.Focus();
            if (comboBox1.SelectedIndex == 0)
            {
                A = 0;
                label2.Text = "0s";
                button1.Enabled = true;
                button1.Focus();
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                button2.Enabled = false;
                button1.Focus();
                count3();
            }
            else if (comboBox1.SelectedIndex == 2)
            {
                button2.Enabled = false;
                button1.Focus();
                count2();
            }
            else if (comboBox1.SelectedIndex == 3)
            {
                button2.Enabled = false;
                button1.Focus();
                count();
            }
        }

        private int counter;
        private string s = "s";

        private void count()
        {
            counter = 60;
            timer1 = new Timer();
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = 1000; // 1 second
            timer1.Start();
            label2.Text = counter.ToString() + s;
        }

        private void count2()
        {
            counter = 30;
            timer1 = new Timer();
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = 1000; // 1 second
            timer1.Start();
            label2.Text = counter.ToString() + s;

        }

        private void count3()
        {
            counter = 10;
            timer1 = new Timer();
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = 1000; // 1 second
            timer1.Start();
            label2.Text = counter.ToString() + s;

        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            counter = counter - 1;
            label2.Text = counter.ToString() + s;
            if (counter == 0)
            {
                timer1.Stop();
                label2.Text = counter.ToString() + s;
                DialogResult timeup = MessageBox.Show("時間切れ！！", "Click Me!! For C Sharp", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (timeup == DialogResult.OK)
                {
                    button1.Enabled = false;
                    button2.Enabled = true;
                    label2.Text = "--";
                }
            }
        }
    }
}
