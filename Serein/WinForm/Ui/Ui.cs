﻿using System.Windows.Forms;

namespace Serein.Ui
{
    public partial class Ui : Form
    {
        public Ui()
        {
            MultiOpenCheck();
            InitializeComponent();
            Initialize();
            UpdateVersion();
        }
    }
}