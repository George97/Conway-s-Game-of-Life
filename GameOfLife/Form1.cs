using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameOfLife
{
    public partial class Form1 : Form
    {
        private FormService _formService;
        private FileService _fileService;
        private LifeProcess _life;
        private bool _timerStarted = false;

        public Form1()
        {
            InitializeComponent();
            _formService = new FormService(drawPanel);
            _fileService = new FileService($"{AppDomain.CurrentDomain.BaseDirectory}/templates.txt");
            ConwayRules rules = new ConwayRules();
            _life = new LifeProcess(_formService, rules);
        }

        private void drawPanel_MouseClick(object sender, MouseEventArgs e)
        {
            int x = e.X - (e.X % _formService.CellSize);
            int y = e.Y - (e.Y % _formService.CellSize);
            Cell cell = new Cell()
            {
                X = x,
                Y = y,
                Neighbours = 0,
                State = State.ALive
            };
            _life.OldGeneration.Add(cell);
            _formService.DrawCell(cell);
        }

        private void nextBtn_Click(object sender, EventArgs e)
        {
            _life.Cycle();
        }

        private void speedTracker_Scroll(object sender, EventArgs e)
        {
            int interval = speedTracker.Value > 0 ? speedTracker.Value : 1;
            timer1.Interval = _formService.Speed / interval;
        }

        private void startBtn_Click(object sender, EventArgs e)
        {
            if(!_timerStarted)
            {
                int interval =  speedTracker.Value > 0 ? speedTracker.Value : 1;
                timer1.Interval = _formService.Speed / interval;
                timer1.Start();
                startBtn.Text = "Stop";
            }
            else
            {
                startBtn.Text = "Start";
                timer1.Stop();
            }
            _timerStarted = !_timerStarted;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            _life.Cycle();
        }

        private void clearBtn_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            _formService.ClearPanel();
            _life.SetInitialState();
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            _fileService.Write(_life.OldGeneration);
        }

        private void uploadBtn_Click(object sender, EventArgs e)
        {
            _life.OldGeneration = _fileService.Read();
            foreach (var cell in _life.OldGeneration)
            {
                _formService.DrawCell(cell);
            }
        }

        private void drawPanel_Paint(object sender, PaintEventArgs e)
        {
            _formService.DrawGrid();
        }
    }
}
