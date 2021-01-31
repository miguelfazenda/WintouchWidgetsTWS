using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WidgetWintouchLimiteCredito.CustomWidget
{
    [Wintouch.Core.UI.Widgets.WidgetAttributes("Limite de Crédito")]
    public partial class WidgetLimiteCredito : Wintouch.Core.UI.Widgets.WidgetBase
    {
        DataGridViewCellStyle styleLinhaLimite0, styleLinhaLimite1;
        DataGridViewCellStyle styleLinhaTotalCreditoUtilizado0, styleLinhaTotalCreditoUtilizado1;
        DataGridViewCellStyle styleLinhaSaldo0_Pos, styleLinhaSaldo1_Pos;
        DataGridViewCellStyle styleLinhaSaldo0_Neg, styleLinhaSaldo1_Neg;
        public WidgetLimiteCredito()
        {
            InitializeComponent();

            //Estilos para as células
            styleLinhaLimite0 = new DataGridViewCellStyle(dataGridView1.Columns[0].DefaultCellStyle);
            styleLinhaLimite1 = new DataGridViewCellStyle(dataGridView1.Columns[1].DefaultCellStyle);
            styleLinhaLimite0.BackColor = Color.LightBlue;
            styleLinhaLimite1.BackColor = Color.LightBlue;

            styleLinhaTotalCreditoUtilizado0 = new DataGridViewCellStyle(dataGridView1.Columns[0].DefaultCellStyle);
            styleLinhaTotalCreditoUtilizado1 = new DataGridViewCellStyle(dataGridView1.Columns[1].DefaultCellStyle);
            styleLinhaTotalCreditoUtilizado0.BackColor = Color.LightGray;
            styleLinhaTotalCreditoUtilizado1.BackColor = Color.LightGray;

            styleLinhaTotalCreditoUtilizado0 = new DataGridViewCellStyle(dataGridView1.Columns[0].DefaultCellStyle);
            styleLinhaTotalCreditoUtilizado1 = new DataGridViewCellStyle(dataGridView1.Columns[1].DefaultCellStyle);
            styleLinhaTotalCreditoUtilizado0.BackColor = Color.LightGray;
            styleLinhaTotalCreditoUtilizado1.BackColor = Color.LightGray;

            styleLinhaSaldo0_Neg = new DataGridViewCellStyle(dataGridView1.Columns[0].DefaultCellStyle);
            styleLinhaSaldo1_Neg = new DataGridViewCellStyle(dataGridView1.Columns[1].DefaultCellStyle);
            styleLinhaSaldo0_Pos = new DataGridViewCellStyle(dataGridView1.Columns[0].DefaultCellStyle);
            styleLinhaSaldo1_Pos = new DataGridViewCellStyle(dataGridView1.Columns[1].DefaultCellStyle);
            styleLinhaSaldo0_Neg.BackColor = Color.FromArgb(250, 122, 122);
            styleLinhaSaldo1_Neg.BackColor = styleLinhaSaldo0_Neg.BackColor;
            styleLinhaSaldo0_Pos.BackColor = Color.Green;
            styleLinhaSaldo1_Pos.BackColor = Color.Green;

            //Quando se altera o conteudo da combobox, vai atualizar os dados
            comboBoxFornecedor.SelectedIndexChanged += (sender, e) => { OnRefreshData(); };
            comboBoxFornecedor.TextChanged += (sender, e) => { OnRefreshData(); };

            //Obtem os fornecedores
            Wintouch.Common.BusinessTier.Terceiros.Filtro.Reset();
            Wintouch.Common.BusinessTier.Terceiros.FiltrarFornecedores();
            var lista = Wintouch.Common.BusinessTier.Terceiros.GetList();
            comboBoxFornecedor.Items.AddRange(lista.wgcterceiros.ToList().Select(x => x.Codigo).ToArray());
        }

        Wintouch.Common.Datatier.DsTerceiros.wgcterceirosRow terceiro;

        public override void OnRefreshData()
        {
            dataGridView1.Rows.Clear();

            lblNomeFornecedor.Text = "Forncedor não encontrado";

            //Procura o fornecedor caso tenha mudado
            if(terceiro == null || terceiro.Codigo != comboBoxFornecedor.Text)
                terceiro = Wintouch.Common.BusinessTier.Terceiros.GetItem(comboBoxFornecedor.Text);

            if(terceiro != null)
            {
                lblNomeFornecedor.Text = terceiro.Nome;

                decimal totalEncomendas = LimiteCredito.GetTotalTipoTipoDocEstadoN(terceiro.Codigo, "E");
                decimal totalGuias = LimiteCredito.GetTotalTipoTipoDocEstadoN(terceiro.Codigo, "H");

                decimal saldoContaCorrente = LimiteCredito.GetSaldoContaCorrente(terceiro.Codigo);
                decimal limiteTotal = Wintouch.Comercial.BusinessTier.Terceiros.GetLimiteControloCreditoGlobalFornecedoresOutrosCredores(terceiro);

                decimal totalCreditoUtilizado = totalEncomendas + totalGuias + saldoContaCorrente;
                decimal saldo = limiteTotal - totalCreditoUtilizado;

                dataGridView1.Rows.Add(new object[] { "Limite de crédito", limiteTotal });
                dataGridView1.Rows.Add(new object[] { "Encomendas", totalEncomendas });
                dataGridView1.Rows.Add(new object[] { "Guias", totalGuias });
                dataGridView1.Rows.Add(new object[] { "Conta corrente", saldoContaCorrente });
                dataGridView1.Rows.Add(new object[] { "Total de crédito utilizado", totalCreditoUtilizado });
                dataGridView1.Rows.Add(new object[] { "Saldo de crédito", saldo });

                dataGridView1.Rows[0].Cells[0].Style = styleLinhaLimite0;
                dataGridView1.Rows[0].Cells[1].Style = styleLinhaLimite1;

                dataGridView1.Rows[4].Cells[0].Style = styleLinhaTotalCreditoUtilizado0;
                dataGridView1.Rows[4].Cells[1].Style = styleLinhaTotalCreditoUtilizado1;

                if(saldo > 0)
                {
                    dataGridView1.Rows[5].Cells[0].Style = styleLinhaSaldo0_Pos;
                    dataGridView1.Rows[5].Cells[1].Style = styleLinhaSaldo1_Pos;
                }
                else
                {
                    dataGridView1.Rows[5].Cells[0].Style = styleLinhaSaldo0_Neg;
                    dataGridView1.Rows[5].Cells[1].Style = styleLinhaSaldo1_Neg;
                }
            }

            base.OnRefreshData();
        }
    }
}
