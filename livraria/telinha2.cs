using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using livraria.BibliotecaDBDataSetTableAdapters;

namespace livraria
{
    public partial class telinha2 : Form
    {
        public telinha2()
        {
            InitializeComponent();

                LivrosTableAdapter livros = new LivrosTableAdapter();
            var dados = from linha in livros.GetData()
                        select linha;
            foreach (var dado in dados)
            {
                string texto = dado.LivroID.ToString();
                texto += ";" + dado.Titulo;
                cboLivro.Items.Add(texto);
            }

            cboLivro.SelectedIndex = 0;
            FuncionariosTableAdapter funcionarios = new FuncionariosTableAdapter();
            var dados2 = from linha in funcionarios.GetData() 
                         select linha;
            foreach( var dado2 in dados2)
            {
                string texto = dado2.ToString();
                texto += ";" + dado2.NomeCompleto;
                cboFuncionario.Items.Add(texto);
            }

            cboFuncionario.SelectedIndex = 0;
            UsuariosTableAdapter usuarios = new UsuariosTableAdapter();
            var dados3 = from linha in usuarios.GetData()
                        select linha;
            foreach (var dado3 in dados3)
            {
                string texto = dado3.ToString();
                texto += ";" + dado3.Nome;
                cboUsuario.Items.Add(texto);
            }

            cboUsuario.SelectedIndex = 0;
            cboStatus.Items.AddRange(new string[] { "Aprovada" + "Devolvido" });
            cboStatus.SelectedIndex = 0;
        }

        private void btnEmprestar_Click(object sender, EventArgs e)
        {
            string[] dados = cboLivro.SelectedItem.ToString().Split(';');
            int livroID = int.Parse(dados[0]);
            dados = cboUsuario.SelectedItem.ToString().Split(';');
            int UsuarioID = int.Parse(dados[0]);
            dados = dados[0].ToString().Split(';');
            int FuncionarioID = int.Parse(dados[0]);
            String Status = cboStatus.SelectedItem.ToString();
            DateTime dataRequisicao = DateTime.Now;
            DateTime dataDevolucao = DataDevolução.Value;
            RequisicoesTableAdapter requisicoes = new RequisicoesTableAdapter();
            requisicoes.Insert(UsuarioID, livroID, FuncionarioID, dataRequisicao, dataDevolucao, Status);
            this.Close();
        }
    }
}
