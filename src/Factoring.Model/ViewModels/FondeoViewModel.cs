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
        public decimal PorTasaMensual { get; set; }
        public decimal PorComisionFactura { get; set; }
        public decimal PorSpread { get; set; }

        public decimal PorCapitalFinanciado { get; set; }
        public decimal PorTasaAnualFondeo { get; set; }
        public decimal PorTasaMoraFondeo { get; set; }
        public int IdEstadoFondeo{ get; set; }

    }
}
