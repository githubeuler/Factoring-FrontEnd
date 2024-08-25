namespace Factoring.Model.ViewModels
{
    public class FondeoViewModel
    {
        public int IdFondeadorFactura { get; set; }
        public int IdOperacion { get; set; }
        public int IdFondeadorVal { get; set; }
        public string NroOperacion { get; set; }
        public string Fondeador { get; set; }
        public string Girador { get; set; }
        public string FechaRegistro { get; set; }
        public string EstadoFondeo { get; set; }
        public int IdEstado { get; set; }
        public int IdTipoProducto { get; set; }
        public string FechaDesembolso { get; set; }
        public string FechaCobranza { get; set; }
        public int PorTasaMensual { get; set; }
        public int PorComisionFactura { get; set; }
        public int PorSpread { get; set; }

        public int PorCapitalFinanciado { get; set; }
        public int PorTasaAnualFondeo { get; set; }
        public int PorTasaMoraFondeo { get; set; }
        public int IdEstadoFondeo{ get; set; }

    }
}
