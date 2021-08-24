﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FontAwesome.Sharp;
using Smallib.ChildForms;
using System.Data.SqlClient;

namespace Smallib
{
    public partial class Principal : Form
    {
        //string de conexão com o banco de dados
        SqlConnection conectar = new SqlConnection(@"Data Source=localhost\SQLEXPRESS;Initial Catalog = BIBLIOTECA; Integrated Security = True"); //Variável para conexão com o banco
        SqlDataAdapter dados; //uma das classes que auxilia na recuperação de dados
        SqlCommandBuilder cmd; //mostra os códigos SQL
        DataTable datb; //DataTable é quem vai receber os dados do adapter

        //Fields
        private IconButton currentBtn;
        private Panel leftBorderBtn;
        public Form currentChildForm;
        public string userLogin;

        public Principal(Cadastro_Usuario usuario)
        {
            InitializeComponent();

            userLogin = usuario.login_usuario.ToString();
            labelNameUser.Text = "Bem vindo, " + userLogin + "!";

            leftBorderBtn = new Panel();
            leftBorderBtn.Size = new Size(7, 60);
            panelMenu.Controls.Add(leftBorderBtn);

            //abrindo a conexão com o banco de dados
            conectar.Open();

            //verifica se a senha já foi alterada alguma vez e retorna true ou false
            SqlCommand verifyUserPasswordAlreadyChanged = new SqlCommand("SELECT senha_alterada FROM Usuario WHERE login_usuario = '"+ userLogin +"' AND senha_alterada = 0", conectar);
            bool result = verifyUserPasswordAlreadyChanged.ExecuteReader().HasRows;

            //fechando a conexão com o banco de dados
            conectar.Close();

            if (result)
            {
                PrimeiraEntrada principal = new PrimeiraEntrada(userLogin);
                principal.ShowDialog();
                this.ShowInTaskbar = false;
                Close();
            }

            OpenChildForm(new Home(this));
            ActivateButton(btnHome, RGBColors.azulEscuro);
        }

        
        //Structs
        public struct RGBColors
        {
            public static Color azulEscuro = Color.FromArgb(46, 81, 116);
            public static Color azulClaro = Color.FromArgb(122, 201, 245);
            public static Color azul = Color.FromArgb(44, 136, 217);
            public static Color verde = Color.FromArgb(26, 174, 159);
            public static Color laranja = Color.FromArgb(232, 131, 58);
            public static Color vermelho = Color.FromArgb(249, 88, 155);
            public static Color cinzaEscuro = Color.FromArgb(249, 88, 155);
            public static Color cinza = Color.FromArgb(197, 197, 197);
            public static Color branco = Color.FromArgb(253, 253, 253);
        }

        //Methods
        private void ActivateButton(object senderBtn, Color color)
        {
            if (senderBtn != null)
            {
                
                DisableButton();

                //Button
                currentBtn = (IconButton)senderBtn;

                currentBtn.ForeColor = color;
                //currentBtn.TextAlign = ContentAlignment.MiddleCenter;
                currentBtn.IconColor = color;
                //currentBtn.TextImageRelation = TextImageRelation.TextBeforeImage;
                //currentBtn.ImageAlign = ContentAlignment.MiddleRight;
                //Left border button
                leftBorderBtn.BackColor = color;
                leftBorderBtn.Location = new Point(0, currentBtn.Location.Y);
                leftBorderBtn.Visible = true;
                leftBorderBtn.BringToFront();
            }
        }

        private void DisableButton()
        {
            if (currentBtn != null)
            {
                currentBtn.ForeColor = RGBColors.cinza;
                //currentBtn.TextAlign = ContentAlignment.MiddleLeft;
                currentBtn.IconColor = RGBColors.azulClaro;
                //currentBtn.TextImageRelation = TextImageRelation.ImageBeforeText;
                //currentBtn.ImageAlign = ContentAlignment.MiddleLeft;
            }
        }

        public void OpenChildForm(Form childForm)
        {
            if (currentChildForm != null)
            {
                currentChildForm.Close();
            }

            currentChildForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panelPrincipal.Controls.Add(childForm);
            panelPrincipal.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        private void btnInicio_Click(object sender, EventArgs e)
        {
            ActivateButton(btnHome, RGBColors.azulEscuro);
            OpenChildForm(new Home(this));
        }

        private void btnEmprestimo_Click(object sender, EventArgs e)
        {
            ActivateButton(btnEmprestimo, RGBColors.azulEscuro);
            OpenChildForm(new Emprestimo(this));
        }

        private void btnDevolucao_Click(object sender, EventArgs e)
        {
            ActivateButton(btnDevolucao, RGBColors.azulEscuro);
            OpenChildForm(new Devolucao(this));
        }

        private void btnCadastros_Click(object sender, EventArgs e)
        {
            ActivateButton(btnCadastros, RGBColors.azulEscuro);
            OpenChildForm(new CadastrosRoot(this));
        }

        private void btnRelatorios_Click(object sender, EventArgs e)
        {
            ActivateButton(btnRelatorios, RGBColors.azulEscuro);
            OpenChildForm(new RelatoriosRoot(this));
        }

        private void btnConfiguracoes_Click(object sender, EventArgs e)
        {
            ActivateButton(btnConfiguracoes, RGBColors.azulEscuro);
            OpenChildForm(new ConfiguracoesRoot(this, userLogin));
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Deseja mesmo fazer logout?", "Logout", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                Login login = new Login();
                this.Hide();
                login.ShowDialog();
                this.Close();
            }
        }
    }
}