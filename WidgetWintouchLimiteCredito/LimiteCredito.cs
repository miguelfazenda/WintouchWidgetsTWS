using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WidgetWintouchLimiteCredito.CustomWidget
{
    class LimiteCredito
    {
        public static decimal GetTotalTipoTipoDocEstadoN(string terceiroCodigo, string tipoTipoDoc)
        {
            Wintouch.Common.BusinessTier.TiposDocumentos.ResetFiltro();
            Wintouch.Common.BusinessTier.TiposDocumentos.Filtro.AddFilterRow("Tipo", tipoTipoDoc);
            var tiposDoc = Wintouch.Common.BusinessTier.TiposDocumentos.GetList().wgctiposdocumentos.ToList().Select(t => t.Codigo);

            Wintouch.Comercial.BusinessTier.GcDocCab.Filtro.Reset();

            //Manha que descobri porque não encontro documentação de como fazer or "A or B or C"
            var tiposDocFilter = string.Join("' or tipodoc='", tiposDoc);
            Wintouch.Comercial.BusinessTier.GcDocCab.Filtro.AddFilterRow("tipodoc", tiposDocFilter);

            Wintouch.Comercial.BusinessTier.GcDocCab.Filtro.AddFilterRow("cliente", terceiroCodigo);
            var docs = Wintouch.Comercial.BusinessTier.GcDocCab.GetList().wGCDocCab;

            decimal total = 0;

            foreach (var doc in docs)
            {
                //AddFilterRow("estado", "N"); não funcionou, por isso teve ser assim >:(
                if (doc.Estado != "N")
                    continue;

                total += doc.base1 + doc.base2 + doc.base3 + doc.base4 + doc.IVA1 + doc.iva2 + doc.iva3 + doc.iva4;
            }

            return total;
        }

        public static decimal GetSaldoContaCorrente(string terceiroCodigo)
        {
            Wintouch.Comercial.BusinessTier.Pendentes.ResetFiltro();
            Wintouch.Comercial.BusinessTier.Pendentes.Filtro.AddFilterRow("entidade", terceiroCodigo);
            Wintouch.Comercial.BusinessTier.Pendentes.Filtro.AddFilterRow("estado", "PND");
            var list = Wintouch.Comercial.BusinessTier.Pendentes.GetList();

            decimal total = 0;

            foreach (var doc in list.wGCPendentes)
            {
                total -= doc.ValorPendente;
            }
            return total;
        }

    }
}
