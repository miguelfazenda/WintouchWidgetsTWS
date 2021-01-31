using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WidgetWintouchLimiteCredito.CustomWidget
{
    [Wintouch.Core.UI.Widgets.WidgetAttributes("Estado dos PDAs")]
    public partial class WidgetEstadoPDAs : Wintouch.Core.UI.Widgets.WidgetBase
    {
        DataGridViewCellStyle styleVerde;

        public WidgetEstadoPDAs()
        {
            InitializeComponent();

            //Estilo verde
            styleVerde = new DataGridViewCellStyle(dataGridView1.RowsDefaultCellStyle);
            styleVerde.BackColor = Color.LightGreen;

            //Esconde a possibilidade de selecionar 
            dataGridView1.SelectionChanged += DataGridView1_SelectionChanged;
        }

        //Impede que se selecione
        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            ((DataGridView)sender).ClearSelection();
        }

        public override void OnRefreshData()
        {
            Wintouch.Comercial.BusinessTier.AutoVenda.CtrlPDAs.ResetFiltro();
            var lista = Wintouch.Comercial.BusinessTier.AutoVenda.CtrlPDAs.GetList();

            dataGridView1.Rows.Clear();

            foreach(var item in lista.wGCCtrlPDAs)
            {
                var row = new DataGridViewRow();
                dataGridView1.Rows.Add(row);
                row.Cells[0].Value = item.IDPosto;
                row.Cells[1].Value = item.Estado;

                //Se o estado for 2 a linha fica verde
                if(item.Estado == 2)
                {
                    row.DefaultCellStyle = styleVerde;
                }
            }

            base.OnRefreshData();
        }

        private void btnImportar_Click(object sender, EventArgs e)
        {
            var form = new Wintouch.Comercial.UI.Forms.Tools.frmGCExportImportPDA();
            form.StartDialog();
        }

        private void btnServidores_Click(object sender, EventArgs e)
        {
            var form = new Wintouch.Manager.UI.Forms.frmMngServers();
            form.Start();
        }
    }
}
