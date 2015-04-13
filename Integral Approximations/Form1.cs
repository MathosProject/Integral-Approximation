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
                watch.Start();
                label7.Text = IntegralCalculus.Integrate(x => Eval(expression, x), Convert.ToDouble(textBox2.Text), Convert.ToDouble(textBox3.Text), IntegralCalculus.IntegrationAlgorithm.RectangleMethod, Convert.ToDouble(textBox4.Text)).ToString();
                watch.Stop();
                label16.Text = watch.ElapsedMilliseconds.ToString();
                watch.Reset();

                watch.Start();
                label10.Text = IntegralCalculus.Integrate(x => Eval(expression, x) , Convert.ToDouble(textBox2.Text), Convert.ToDouble(textBox3.Text), IntegralCalculus.IntegrationAlgorithm.TrapezoidalRule, Convert.ToDouble(textBox4.Text)).ToString();
                watch.Stop();
                label15.Text = watch.ElapsedMilliseconds.ToString();
                watch.Reset();

                watch.Start();
                label12.Text = IntegralCalculus.Integrate(x => Eval(expression, x), Convert.ToDouble(textBox2.Text), Convert.ToDouble(textBox3.Text), IntegralCalculus.IntegrationAlgorithm.SimpsonsRule, Convert.ToDouble(textBox4.Text)).ToString();
                watch.Stop();

                label14.Text = watch.ElapsedMilliseconds.ToString();
                watch.Reset();
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
