using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using livraria.BibliotecaDBDataSetTableAdapters;
using static livraria.BibliotecaDBDataSet;

namespace livraria
{
    public partial class VisualizandoRelacao : Form
    {
        private RequisicoesTableAdapter requisicoes = new RequisicoesTableAdapter();
        private RequisicoesRow requisicao;

        private LivrosTableAdapter livros = new LivrosTableAdapter();
        private LivrosRow livro;

        private UsuariosTableAdapter usuarios = new UsuariosTableAdapter();
        private UsuariosRow usuario;

        private FuncionariosTableAdapter funcionarios = new FuncionariosTableAdapter();
        private FuncionariosRow funcionario;

        public VisualizandoRelacao(int ID)
        {
            

        InitializeComponent();
            
            RequisicoesRow requisicao = (from linha in requisicoes.GetData()
                                         where linha.RequisicaoID == ID
                                         select linha).FirstOrDefault();
            if (requisicao == null) return;
            livro = (from linha in livros.GetData()
                                         where linha.LivroID == requisicao.LivroID
                                         select linha).FirstOrDefault();

            if (livro == null) return;
            txtQuantidadeLivro.Text = livro.QuantidadeDisponivel.ToString();
            txtTituloLivro.Text = livro.Titulo;
            usuario = (from linha in usuarios.GetData()
                                   where linha.UsuarioID == requisicao.UsuarioID
                                   select linha).FirstOrDefault();

            if (livro == null) return;
            txtNomeUsuario.Text = usuario.Nome;
            funcionario = (from linha in funcionarios.GetData()
                                   where linha.FuncionarioID == requisicao.FuncionarioID
                                   select linha).FirstOrDefault();

            if (funcionario == null) return;    
            txtNomeFuncionario.Text = funcionario.NomeCompleto;
            txtCargoFuncionario.Text = funcionario.Cargo;

            try
            {
                DataLogin.Value = funcionario.UltimoLogin;
            }
            catch
            {
                cboStatus.Items.Add("Pendente");
                cboStatus.Items.Add("Aprovado");
                cboStatus.Items.Add("Devolvido");
                cboStatus.Text  = requisicao.Status;
            }
            try
            {
                DataDevolucao.Value = requisicao.DataDevolucao;
            }
            catch
            {
                DataDevolucao.Value = DateTime.Now;
            }
            try
            {
                DataRequisicao.Value = requisicao.DataDevolucao;
            }
            catch
            {
                DataRequisicao.Value = DateTime.Now;
            }
        }
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Voltar();
        }

        private void Voltar()
        {
            emprestimo emprestimo = new emprestimo();
            emprestimo.Closed += fecharFormulario;
            emprestimo.Show();
            this.Hide();
        }

        private void fecharFormulario (object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            if(cboStatus.SelectedItem == "Devolvido")
            {
                requisicao.Status = "Devolvido";
                requisicao.DataDevolucao = DataDevolucao.Value;
                livro.QuantidadeDisponivel++;
                funcionario.UltimoLogin = DateTime.Now;
                requisicoes.Update(livro);
                funcionarios.Update(livro);
                Voltar();
            }
            else if(cboStatus.SelectedItem == "Aprovado")
            {
                Voltar();
            }
            else
            {
                if( requisicao.Status == "Pendente")
                {
                    Voltar();
                    return;
                }
                requisicao.Status = "Pendente";
                requisicoes.Update(requisicao);
                Voltar();
            }
        }

        private void VisualizandoRelacao_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
            Application.Exit();
        }
    }
}
