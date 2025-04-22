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
using static livraria.BibliotecaDBDataSet;

namespace livraria
{
    public partial class emprestimo : Form
    {
        public emprestimo()
        {
            InitializeComponent();
            RequisicoesTableAdapter requisicoes = new RequisicoesTableAdapter();
            var dados = from linha in requisicoes.GetData()
                        select linha;
            foreach ( var dado in dados)
            {
                string texto = dado.RequisicaoID.ToString();
                LivrosTableAdapter livros = new LivrosTableAdapter();
                LivrosRow livro = (from linha in livros.GetData()
                                   where linha.LivroID == dado.LivroID
                                   select linha).FirstOrDefault();
                if (livro == null) break;

                UsuariosTableAdapter usuarios = new UsuariosTableAdapter();
                UsuariosRow usuario = (from linha in usuarios.GetData()
                                        where linha.UsuarioID == dado.UsuarioID
                                        select linha).FirstOrDefault();
                if(usuario == null) break;

                FuncionariosTableAdapter funcionarios = new FuncionariosTableAdapter();
                FuncionariosRow funcionario = (from linha in funcionarios.GetData()
                                               where linha.FuncionarioID == dado.FuncionarioID
                                               select linha).FirstOrDefault();
                if (funcionario == null) break;

                texto += "-" + livro.Titulo;
                lboEmprestimos.Items.Add(texto);
            }

            

        }

        private void lboEmprestimos_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (lboEmprestimos == null) return;
            string[] dados = lboEmprestimos.SelectedItem.ToString().Split('-');
            int ID = int.Parse(dados[0]);
            VisualizandoRelacao relacao = new VisualizandoRelacao(ID);
            relacao.Closed += fechandoFormulario;
            relacao.Show();
            this.Hide();
        }
        private void fechandoFormulario (object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void emprestimo_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            telinha2 emprestando = new telinha2();
            emprestando.Closed += fechandoFormulario;
            emprestando.ShowDialog();
        }
    }
}
