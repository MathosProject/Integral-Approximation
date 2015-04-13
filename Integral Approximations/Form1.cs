using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Collections.ObjectModel;
using Mathos.Calculus;
using Mathos.Parser;

using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Integral_Approximations
{
    public partial class Form1 : Form
    {
        MathParser parser = new MathParser();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            integrate();
        }

        private void integrate()
        {
            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();

            var expression = parser.GetTokens(textBox1.Text);

            try
            {
                string[] results = new string[6];
                Task[] tasks = new Task[3];
                
                tasks[0] = Task.Factory.StartNew(delegate() {
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    results[0] = IntegralCalculus.Integrate(x => Eval(expression, x), Convert.ToDouble(textBox2.Text), Convert.ToDouble(textBox3.Text), IntegralCalculus.IntegrationAlgorithm.RectangleMethod, Convert.ToDouble(textBox4.Text)).ToString();
                    sw.Stop();
                    results[3] = sw.ElapsedMilliseconds.ToString();
                });

                tasks[1] = Task.Factory.StartNew(delegate()
                {
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    results[1] = IntegralCalculus.Integrate(x => Eval(expression, x), Convert.ToDouble(textBox2.Text), Convert.ToDouble(textBox3.Text), IntegralCalculus.IntegrationAlgorithm.TrapezoidalRule, Convert.ToDouble(textBox4.Text)).ToString();
                    sw.Stop();
                    results[2] = sw.ElapsedMilliseconds.ToString();
                });

                tasks[2] = Task.Factory.StartNew(delegate()
                {
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    results[4] = IntegralCalculus.Integrate(x => Eval(expression, x), Convert.ToDouble(textBox2.Text), Convert.ToDouble(textBox3.Text), IntegralCalculus.IntegrationAlgorithm.SimpsonsRule, Convert.ToDouble(textBox4.Text)).ToString();
                    sw.Stop();

                    results[5] = sw.ElapsedMilliseconds.ToString();
                });

                Task.WaitAll(tasks);

                label7.Text = results[0];
                label10.Text = results[1];
                label15.Text = results[2];
                label16.Text = results[3];
                label12.Text = results[4];
                label14.Text = results[5];

            }
            catch (Exception e)
            {
                MessageBox.Show("Something went wrong. Please check that:" + Environment.NewLine + Environment.NewLine  + " - lower/upper bounds contain only integers." + Environment.NewLine + " - function is in form of '3x', 'x^2', etc." + Environment.NewLine + " - no. of intervals is an integer." + Environment.NewLine + " - make sure all fields are filled with numbers.");
            }
        }

        public double Eval(ReadOnlyCollection<string> function, double x, string varName = "x")
        {
            parser.LocalVariables[varName] = (decimal)x;

            return Convert.ToDouble(parser.Parse(function));
        }


        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Copyright (C) 2013 - 2014 Artem Los. All rights reserved." + Environment.NewLine + "Based on: " + Environment.NewLine + " - Mathos Core Library Calculus module" + Environment.NewLine + " - Mathos Parser (v. 1.0.7.2)" + Environment.NewLine + Environment.NewLine + "More info at: http://mathosproject.com/");
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string toCopy = label4.Text  + "\t" + label7.Text + "\t" + label16.Text + Environment.NewLine
                           + label9.Text + "\t" + label10.Text + "\t" + label15.Text + Environment.NewLine
                           + label11.Text + "\t" + label12.Text + "\t" + label14.Text + Environment.NewLine;

            Clipboard.SetText(toCopy);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("http://mathosproject.com/wiki/mathos-core-library/calculus/integration/");
            }
            catch
            {
                MessageBox.Show("Unable to acess http://mathosproject.com/wiki/mathos-core-library/calculus/integration/");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox2.Focus();
            textBox2.Select();
            linkLabel1.TabStop = false;
        }

        private void onlineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("http://labs.mathosproject.com/Module/IntegralApproximation.aspx");
            }
            catch
            {
                MessageBox.Show("Unable to access http://labs.mathosproject.com/Module/IntegralApproximation.aspx");
            }
        }
    }

}
