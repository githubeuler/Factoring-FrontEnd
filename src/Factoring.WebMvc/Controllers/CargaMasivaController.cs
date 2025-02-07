using Factoring.Service.Proxies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Xml.XPath;
using System.Xml;
using Factoring.Model.Models.ReporteGiradorOperaciones;
using Factoring.Model;
using Factoring.Model.Models.OperacionesFactura;
using Factoring.Model.Models.Girador;
using Factoring.Model.Models.Adquiriente;
using Microsoft.AspNetCore.Hosting;
using Factoring.Model.Models.Operaciones;
using System.Reflection;
using Factoring.Model.Models.CategoriaGirador;
using Newtonsoft.Json;
using System;
using System.Configuration;
using Factoring.Model.Models.AdquirienteUbicacion;
using Factoring.Model.Models.GiradorUbicacion;

namespace Factoring.WebMvc.Controllers
{
    public class CargaMasivaController : Controller
    {
        private static Random random = new Random();
        public const string Operaciones = "Operaciones";
        public const string Facturas = "Facturas";

        private readonly IWebHostEnvironment _env;
        private readonly ICatalogoProxy _catalogoProxy;
        private readonly IFacturaOperacionesProxy _facturaOperacionesProxy;
        private readonly IGiradorProxy _giradorProxy;
        private readonly IAdquirienteProxy _adquirienteProxy;
        private readonly ICategoriaGiradorProxy _categoriaProxy;
        private readonly IOperacionProxy _operacionProxy;
        private readonly IConfiguration _configuration;
        private readonly IFilesProxy _filesProxy;
        private readonly IGiradorUbicacionProxy _giradorUbicacionProxy;
        private readonly IAdquirienteUbicacionProxy _adquirienteUbicacionProxy;

        public CargaMasivaController(
            IWebHostEnvironment webHostEnvironment,
            ICatalogoProxy catalogoProxy,
            IFacturaOperacionesProxy facturaOperacionesProxy,
            IGiradorProxy giradorProxy,
            IAdquirienteProxy adquirienteProxy,
            ICategoriaGiradorProxy categoriaProxy,
            IOperacionProxy operacionProxy,
            IConfiguration configuration,
            IFilesProxy filesProxy,
            IGiradorUbicacionProxy giradorUbicacionProxy,
            IAdquirienteUbicacionProxy adquirienteUbicacionProxy) {

            _env = webHostEnvironment;
            _catalogoProxy = catalogoProxy;
            _facturaOperacionesProxy = facturaOperacionesProxy;
            _giradorProxy = giradorProxy;
            _adquirienteProxy = adquirienteProxy;
            _categoriaProxy = categoriaProxy;
            _operacionProxy = operacionProxy;
            _configuration = configuration;
            _filesProxy = filesProxy;
            _giradorUbicacionProxy = giradorUbicacionProxy;
            _adquirienteUbicacionProxy = adquirienteUbicacionProxy;
        }
        public async Task<IActionResult> Index()
        {
            var TipoDocumentoSunat = await _catalogoProxy.GetCatalogoList(new Model.Models.Catalogo.CatalogoListDto { Codigo = 100, Tipo = 1,Valor = "1" });
            ViewBag.TipoDocumentoSunat = TipoDocumentoSunat.Data;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CargaMasivaXML()
        {
            List<ReportesGiradorOperacionesResponse> lstAuxiliar = new List<ReportesGiradorOperacionesResponse>();
            string userName = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            int nIdGirador = 0;

            string SinUbigeoGirador = "";
            string SinUbigeoAceptante = "";

            try
            {
                var countFilesXML = Request.Form.Files.Count();
                if (countFilesXML == 0)
                    throw new Exception("Debe adjuntar los archivos XML de las facturas.");

                foreach (var fileXml in Request.Form.Files)
                {
                    byte[] data;
                    using (var br = new BinaryReader(fileXml.OpenReadStream()))
                        data = br.ReadBytes((int)fileXml.OpenReadStream().Length);

                    string path = $"{_env.WebRootPath}\\assets\\upload";
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                    string sFileName = $"{path}\\{fileXml.FileName}";
                    System.IO.File.WriteAllBytes(sFileName, data);

                    //**************************************************************************************
                    //  VARIABLES
                    //**************************************************************************************
                    string cFactura = string.Empty;
                    string cGirador = string.Empty;
                    string cGiradorRUT = string.Empty;
                    string cGiradorDireccion = string.Empty;
                    string cGiradorUbigeo = string.Empty;

                    string cAdquiriente = string.Empty;
                    string cAdquirienteRUT = string.Empty;
                    string cAdquirienteDireccion = string.Empty;
                    string cAdquirienteUbigeo = string.Empty;


                    string cMoneda = string.Empty;
                    string cFieldName = string.Empty;
                    DateTime dFechaEmision = DateTime.Today;
                    DateTime dFechaVencimiento = DateTime.Today;
                    decimal nMonto = 0;

                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(sFileName);

                    #region Ensobrado

                    XPathNavigator navNs = xmlDocument.CreateNavigator();
                    navNs.MoveToFollowing(XPathNodeType.Element);
                    IDictionary<string, string> nsDictList = navNs.GetNamespacesInScope(XmlNamespaceScope.All);

                    XmlNamespaceManager ns = new XmlNamespaceManager(xmlDocument.NameTable);

                    foreach (var nsItem in nsDictList)
                    {
                        if (string.IsNullOrEmpty(nsItem.Key))
                            ns.AddNamespace("sig", nsItem.Value);
                        else
                            ns.AddNamespace(nsItem.Key, nsItem.Value);
                    }
                    ns.AddNamespace("xs", "http://www.w3.org/2001/XMLSchema");

                    var nav = xmlDocument.CreateNavigator();

                    #endregion

                    string fecha = nav.SelectSingleNode("/descendant::*[local-name() = 'IssueDate'][1]", ns)?.Value ?? "1900-01-01";
                    dFechaEmision = Convert.ToDateTime(fecha);
                    fecha = nav.SelectSingleNode("/descendant::*[local-name() = 'PaymentDueDate'][1]", ns)?.Value ?? "1900-01-01";
                    dFechaVencimiento = Convert.ToDateTime(fecha);

                    cFactura = nav.SelectSingleNode("//*[local-name() = 'Invoice']/*[local-name() = 'ID']", ns)?.Value ?? "0000000";
                    cGiradorRUT = nav.SelectSingleNode("/*[local-name() = 'Invoice']/*[local-name() = 'AccountingSupplierParty']/*[local-name() = 'Party']/*[local-name() = 'PartyIdentification']/*[local-name() = 'ID']", ns)?.Value ?? "";
                    cAdquirienteRUT = nav.SelectSingleNode("/*[local-name() = 'Invoice']/*[local-name() = 'AccountingCustomerParty']/*[local-name() = 'Party']/*[local-name() = 'PartyIdentification']/*[local-name() = 'ID']", ns)?.Value ?? "";
                    cGirador = nav.SelectSingleNode("/*[local-name() = 'Invoice']/*[local-name() = 'AccountingSupplierParty']/*[local-name() = 'Party']/*[local-name() = 'PartyLegalEntity']/*[local-name() = 'RegistrationName']", ns)?.Value ?? "";
                    cGiradorDireccion = nav.SelectSingleNode("/*[local-name() = 'Invoice']/*[local-name() = 'AccountingSupplierParty']/*[local-name() = 'Party']/*[local-name() = 'PartyLegalEntity']/*[local-name() = 'RegistrationAddress']/*[local-name() = 'AddressLine']/*[local-name() = 'Line']", ns)?.Value ?? "";
                    cGiradorUbigeo = nav.SelectSingleNode("/*[local-name() = 'Invoice']/*[local-name() = 'AccountingSupplierParty']/*[local-name() = 'Party']/*[local-name() = 'PartyLegalEntity']/*[local-name() = 'RegistrationAddress']/*[local-name() = 'ID']", ns)?.Value ??
                        nav.SelectSingleNode("/*[local-name() = 'Invoice']/*[local-name() = 'AccountingSupplierParty']/*[local-name() = 'Party']/*[local-name() = 'PartyLegalEntity']/*[local-name() = 'RegistrationAddress']/*[local-name() = 'CountrySubentityCode']", ns)?.Value ?? "";
                    cAdquiriente = nav.SelectSingleNode("/*[local-name() = 'Invoice']/*[local-name() = 'AccountingCustomerParty']/*[local-name() = 'Party']/*[local-name() = 'PartyLegalEntity']/*[local-name() = 'RegistrationName']", ns)?.Value ?? "";
                    cAdquirienteDireccion = nav.SelectSingleNode("/*[local-name() = 'Invoice']/*[local-name() = 'AccountingCustomerParty']/*[local-name() = 'Party']/*[local-name() = 'PartyLegalEntity']/*[local-name() = 'RegistrationAddress']/*[local-name() = 'AddressLine']/*[local-name() = 'Line']", ns)?.Value ?? "";
                    cAdquirienteUbigeo = nav.SelectSingleNode("/*[local-name() = 'Invoice']/*[local-name() = 'AccountingCustomerParty']/*[local-name() = 'Party']/*[local-name() = 'PartyLegalEntity']/*[local-name() = 'RegistrationAddress']/*[local-name() = 'ID']", ns)?.Value ??
                         nav.SelectSingleNode("/*[local-name() = 'Invoice']/*[local-name() = 'AccountingCustomerParty']/*[local-name() = 'Party']/*[local-name() = 'PartyLegalEntity']/*[local-name() = 'RegistrationAddress']/*[local-name() = 'CountrySubentityCode']", ns)?.Value ?? "";
                    cMoneda = nav.SelectSingleNode("//*[local-name() = 'DocumentCurrencyCode']", ns)?.Value ?? "";
                    //nMonto = Convert.ToDecimal(nav.SelectSingleNode("/*[local-name() = 'Invoice']/*[local-name() = 'LegalMonetaryTotal']/*[local-name() = 'PayableAmount']", ns)?.Value ?? "0");

                    foreach (XPathNavigator node in nav.Select("//cac:PaymentTerms", ns))
                    {
                        bool bValidNode = false;
                        foreach (XPathNavigator child in node.SelectChildren(XPathNodeType.All))
                        {
                            if (child.Value.ToUpper() == "CREDITO")
                                bValidNode = true;
                            if (child.Name == "cbc:Amount" && bValidNode)
                                nMonto = Convert.ToDecimal(child.Value);
                        }
                    }

                  

                    var oGirador = GetGirador(cGiradorRUT).Result;
                    if (oGirador == null)
                    {
                        var result = await _giradorProxy.Create(new GiradorCreateDto
                        {
                            IdPais = 1,//PERU
                            RegUnicoEmpresa = cGiradorRUT,
                            RazonSocial = cGirador.ToUpper(),
                            IdSector = 1,
                            IdGrupoEconomico = 1,
                            Venta = 100,
                            Compra = 100,
                            UsuarioCreador = userName
                        });

                        oGirador = GetGirador(cGiradorRUT).Result;
                        if (!string.IsNullOrEmpty(cGiradorUbigeo))
                        {
                            var resultGU = await _giradorUbicacionProxy.Create(new UbicacionGiradorInsertDto
                            {
                                IdGirador = oGirador.nIdGirador,
                                FormatoUbigeo = $"{{\"Departamento\":\"{cGiradorUbigeo.Substring(0, 2)}\",\"Provincia\":\"{cGiradorUbigeo.Substring(0, 4)}\",\"Distrito\":\"{cGiradorUbigeo}\"}}",
                                Direccion = cGiradorDireccion,
                                IdTipoDireccion = 1
                            });
                        }
                        else
                        {
                            SinUbigeoGirador = "<br/> El Girador no tiene direccion.";
                        }

                    }

                        //throw new Exception($"Girador {cGirador} con RUC {cGiradorRUT} NO REGISTRADO.");
                    nIdGirador = oGirador.nIdGirador;

                    var oAdquiriente = GetAdquiriente(cAdquirienteRUT).Result;
                    if (oAdquiriente == null)
                    {
                        var result = await _adquirienteProxy.Create(new AdquirienteInsertDto
                        {
                            IdPais = 1,//PERU
                            RegUnicoEmpresa = cAdquirienteRUT,
                            RazonSocial = cAdquiriente.ToUpper(),
                            IdSector = 1,
                            IdGrupoEconomico = 1,
                            UsuarioCreador = userName
                        });

                        oAdquiriente = GetAdquiriente(cAdquirienteRUT).Result;
                        if (!string.IsNullOrEmpty(cAdquirienteUbigeo))
                        {
                            var resultAU = await _adquirienteUbicacionProxy.Create(new UbicacionAdquirienteInsertDto
                            {
                                IdAdquiriente = oAdquiriente.nIdAdquiriente,
                                FormatoUbigeo = $"{{\"Departamento\":\"{cAdquirienteUbigeo.Substring(0, 2)}\",\"Provincia\":\"{cAdquirienteUbigeo.Substring(0, 4)}\",\"Distrito\":\"{cAdquirienteUbigeo}\"}}",
                                Direccion = cAdquirienteDireccion,
                                IdTipoDireccion = 1
                            });
                        }
                        else
                        {
                            SinUbigeoGirador = "<br/> El Aceptante no tiene direccion.";
                        }
                        
                    }
                        //throw new Exception($"Adquiriente {cAdquiriente} con RUC {cAdquirienteRUT} NO REGISTRADO.");

                    string[] docnum = cFactura.Split('-');
                    var NroDocumento = new { Serie = docnum[0].ToString(), Numero = docnum[1].ToString() };
                    var jsonFactura = System.Text.Json.JsonSerializer.Serialize(NroDocumento).ToString();
                    var oFactura = GetFactura(oGirador.nIdGirador,oAdquiriente.nIdAdquiriente, jsonFactura).Result;
                    if (oFactura.Succeeded)
                        if (oFactura.Data.nEstado == 1)     //  1 = ACTIVA      0 = ANULADA
                            //ESTADO DE LA FACTURA    :   0 = ANULADO     22 = RETIRADO
                            //if (oFactura.Data.NombreEstado != "0" && oFactura.Data.NombreEstado != "22")
                            throw new Exception($"Factura Nro. {cFactura}  ya se encuentra registrada.");

                    var oRecord = new ReportesGiradorOperacionesResponse
                    {
                        nNroOperacion = 0,
                        IdGirador = oGirador.nIdGirador,
                        IdAdquiriente = oAdquiriente.nIdAdquiriente,
                        IdTipoMoneda = (cMoneda == "PEN" ? 1 : 2),
                        Girador = oGirador.cRazonSocial,
                        Aceptante = oAdquiriente.cRazonSocial,
                        NroFactura = cFactura,
                        cMoneda = cMoneda,
                        //nMontoOperacion = nMonto,
                        ImporteNetoFactura = nMonto,
                        dFechaEmision = dFechaEmision,
                        dFechaVencimiento = dFechaVencimiento,
                        dFechaPagoNegociado = dFechaVencimiento,
                        NombreDocumentoXML = fileXml.FileName,
                        Estado = "OK"
                    };
                    lstAuxiliar.Add(oRecord);
                }

                //***********************************************************************
                //  VALIDA LA INFORMACION
                //***********************************************************************
                var dupGirador = lstAuxiliar.GroupBy(x => x.IdGirador);
                if (dupGirador.Count() > 1)
                    throw new Exception("No se permiten facturas de diferentes Giradores.");

                var dupAdquiriente = lstAuxiliar.GroupBy(x => x.IdAdquiriente);
                if (dupAdquiriente.Count() > 1)
                    throw new Exception("No se permiten facturas de diferentes Adquirientes.");

                var dupMoneda = lstAuxiliar.GroupBy(x => x.IdTipoMoneda);
                if (dupMoneda.Count() > 1)
                    throw new Exception("No se permiten facturas de diferentes Monedas.");
            }
            catch (Exception ex)
            {
                return Json(new { succeeded = false, message = ex.Message });
            }

            //var _Categorias = await _categoriaProxy.GetAllCategoriasListGirador(nIdGirador);
            //List<CategoriaGiradorDto> lst = new List<CategoriaGiradorDto>();
  

            //lst.Add(new CategoriaGiradorDto() { Categoria = "VENTA DE CARTERA",nCategoria = 1 });
            //lst.Add(new CategoriaGiradorDto() { Categoria = "FONDOS PROPIOS", nCategoria = 2 });
            var lstCategoria = await _catalogoProxy.GetCatalogoList(new Model.Models.Catalogo.CatalogoListDto { Codigo = 102, Tipo = 1, Valor = "1" }); //_Categorias.Data.ToList();
            return Json(new { succeeded = true, message = $"Se {(lstAuxiliar.Count > 1 ? "cargaron" : "cargo")} {lstAuxiliar.Count} {(lstAuxiliar.Count > 1 ? "archivos" : "archivo")} XML.{SinUbigeoGirador} {SinUbigeoAceptante}", data = lstAuxiliar, comboCategoria = lstCategoria.Data.OrderByDescending(x=> x.nId) });
        }

        private async Task<ResponseData<OperacionesFacturaListDto>> GetFactura(int IdGirador, int IdAdquiriente, string NroFactura)
        {
            ResponseData<OperacionesFacturaListDto> oRecord;
            try
            {
                oRecord = await _facturaOperacionesProxy.GetInvoiceByNumber(IdGirador,IdAdquiriente, NroFactura);
            }
            catch (Exception)
            {
                throw;
            }

            return oRecord;
        }

        private async Task<GiradorResponseDatatableDto> GetGirador(string sRUC)
        {
            string userName = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            GiradorResponseDatatableDto oRecord = null;
            try
            {
                var requestData = new GiradorRequestDatatableDto();
                requestData.Pageno = 0;
                requestData.PageSize = 5;
                requestData.Sorting = "nIdGirador";
                requestData.SortOrder = "asc";
                requestData.FilterRuc = sRUC;
                requestData.Usuario = userName;
                var data = await _giradorProxy.GetAllListGirador(requestData);

                if (data.Data.Count > 0)
                    oRecord = data.Data[0];
            }
            catch (Exception)
            {
                throw;
            }

            return oRecord;
        }

        private async Task<AdquirienteResponseDatatableDto> GetAdquiriente(string sRUC)
        {
            string userName = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            AdquirienteResponseDatatableDto oRecord = null;
            try
            {
                var requestData = new AdquirienteRequestDatatableDto();
                requestData.Pageno = 0;
                requestData.PageSize = 5;
                requestData.Sorting = "nIdAdquiriente";
                requestData.SortOrder = "asc";
                requestData.FilterRuc = sRUC;
                requestData.Usuario = userName;
                var data = await _adquirienteProxy.GetAllListAdquiriente(requestData);
                if (data.Data.Count > 0)
                    oRecord = data.Data[0];
            }
            catch (Exception)
            {
                throw;
            }

            return oRecord;
        }

        public async Task<JsonResult> GetOperacion(string sNumOperacion)
        {
            try
            {
                var requestData = new OperacionesRequestDataTableDto();
                requestData.Pageno = 0;
                requestData.PageSize = 5;
                requestData.Sorting = "nIdOperaciones";
                requestData.SortOrder = "asc";
                requestData.FilterNroOperacion = sNumOperacion;

                var data = await _operacionProxy.GetAllListOperaciones(requestData);
                var recordsTotal = data.Data.Count;
                if (recordsTotal == 0)
                {
                    string sMessage = "Número de Operación <span style='font-weight:bold; color:#fff; background-color:#f44336;'>&nbsp; NO REGISTRADO. &nbsp;</span>";
                    throw new Exception(sMessage);
                }

                var nIDOperacion = data.Data[0].nIdOperaciones;
                var dataOperacion = await _operacionProxy.GetOperaciones(nIDOperacion);
                OperacionSingleResponseDto oRecord = dataOperacion.Data;
                //if (oRecord.nEstado != 13)  //  13 = APROBADO TOTAL
                if (oRecord.nEstado != 1 && oRecord.nEstado != 14 &&
                                            oRecord.nEstado != 16 && oRecord.nEstado != 17 &&
                                            oRecord.nEstado != 18 && oRecord.nEstado != 19 &&
                                            oRecord.nEstado != 21 && oRecord.nEstado != 41 &&
                                            oRecord.nEstado != 42)
                {
                    //string sMessage = "Girador : <b>" + oRecord.cRazonSocialGirador + "</b> <br>" +
                    //                "Adquiriente : <b>" + oRecord.cRazonSocialAdquiriente + "</b> <br>" +
                    //                "Estado : <b>" + oRecord.NombreEstado + "</b> <br><br>" +
                    //                "Sólo se permite <span style='font-weight:bold; color:#fff; background-color:green;'>&nbsp; APROBADO TOTAL &nbsp;</span>";
                    string sMessage = "Girador : <b>" + oRecord.cRazonSocialGirador + "</b> <br>" +
                                      "Adquiriente : <b>" + oRecord.cRazonSocialAdquiriente + "</b> <br>" +
                                      "Estado : <b>" + oRecord.nEstado + " - " + oRecord.NombreEstado + "</b> <br><br>" +
                                      "<span style='font-weight:bold; color:#fff; background-color:red;'>&nbsp; NO PERMITIDO. &nbsp;</span>";
                    throw new Exception(sMessage);
                }

                return Json(new { succeeded = true, data = oRecord });
            }
            catch (Exception ex)
            {
                return Json(new { succeeded = false, message = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessBatch(int operacionId)
        {
            var ItemsExcel = new List<OperacionesFacturaInsertMasivotDto>();
            string userName = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            Microsoft.Extensions.Primitives.StringValues responseData;
            Request.Form.TryGetValue("lstFacturas", out responseData);
            var lstFacturas = JsonConvert.DeserializeObject<List<ReportesGiradorOperacionesResponse>>(responseData);

            try
            {
                //*************************************************************************************
                //  REGISTRO DE LA OPERACION
                //*************************************************************************************
                bool bNewOpe = (operacionId == 0);
                ResponseData<int> resultOpe = null;
                var oRecord = lstFacturas[0];
                if (operacionId == 0)
                {
                    resultOpe = await _operacionProxy.Create(new OperacionesInsertDto
                    {
                        IdGirador = oRecord.IdGirador,
                        IdAdquiriente = oRecord.IdAdquiriente,
                        //IdInversionista = 1,
                        IdGiradorDireccion = 0,
                        IdAdquirienteDireccion = 0,
                        TEM = Convert.ToDecimal(oRecord.nTEM),
                        PorcentajeFinanciamiento = Convert.ToDecimal(oRecord.nPorcentajeFinanciamiento),
                        MontoOperacion = Convert.ToDecimal(oRecord.nMontoOperacion),
                        DescContrato = 0,
                        DescFactura = 0,
                        DescCobranza = Convert.ToDecimal(oRecord.ComisionCobranza),
                        IdTipoMoneda = oRecord.IdTipoMoneda,
                        PorcentajeRetencion = oRecord.retencion,
                        UsuarioCreador = userName,
                        InteresMoratorio = Convert.ToDecimal(oRecord.interesMoratorio),
                        IdCategoria = oRecord.IdCategoria,
                        MotivoTransaccion = "...",
                        SustentoComercial = "...",
                        Plazo = 0,
                        CantidadFactura = lstFacturas.Count
                    });
                    if (!resultOpe.Succeeded)
                        throw new Exception(resultOpe.Message);

                    operacionId = resultOpe.Data;

                    //**********************************************************************
                    //  CREA LA EVALUACION RESPECTIVA E INSERTA EL COMENTARIO
                    //**********************************************************************
                    //if (resultOpe.Succeeded)
                    //{
                    //    var resultado = await _evaluacionOperacionesProxy.Create(new EvaluacionOperacionesInsertDto
                    //    {
                    //        IdOperaciones = operacionId,
                    //        IdCatalogoEstado = 1,
                    //        UsuarioCreador = userName
                    //    });

                    //    if (resultado.Succeeded)
                    //    {
                    //        await _evaluacionOperacionesComentariosProxy.Create(new EvaluacionOperacionesComentariosInsertDto
                    //        {
                    //            IdEvaluacionOperaciones = resultado.Data,
                    //            IdCatalogoTipoComentario = 1,
                    //            Comentario = "CARGA MASIVA",
                    //            UsuarioCreador = userName
                    //        });
                    //    }
                    //}
                }
               

                //  OBTIENE EL NUMERO DE OPERACION
                int nEstadoOperacion = 0;
                string nroOperacion = string.Empty;
                var operacionDetalle = await _operacionProxy.GetOperaciones((int)operacionId);
                if (operacionDetalle != null)
                {
                    nroOperacion = operacionDetalle.Data.nNroOperacion;
                    nEstadoOperacion = operacionDetalle.Data.nEstado;
                }

                List<IFormFile> filesXml = (List<IFormFile>)Request.Form.Files;
                string randon = RandomString(10);
                var path = _configuration[$"PathDocumentos:{Operaciones}"].ToString() + "\\" + nroOperacion + _configuration[$"PathDocumentos:{Facturas}"].ToString();
                foreach (var oFactura in lstFacturas)
                {
                    string[] docnum = oFactura.NroFactura.Split('-');
                    var NroDocumento = new { Serie = docnum[0].ToString(), Numero = docnum[1].ToString() };
                    var oXML = filesXml.Where(xFile => xFile.FileName == oFactura.NombreDocumentoXML);
                    var archivoXML = await _filesProxy.UploadFile(oXML.First(), randon + "_" + oFactura.NombreDocumentoXML, path);
                    if (archivoXML.Succeeded)
                    {
                        var result = await _facturaOperacionesProxy.Create(new OperacionesFacturaInsertDto
                        {
                            IdOperaciones = operacionId,
                            NroDocumento = System.Text.Json.JsonSerializer.Serialize(NroDocumento).ToString(),
                            Monto = oFactura.ImporteNetoFactura,
                            FechaEmision = (DateTime)oFactura.dFechaEmision,
                            FechaVencimiento = (DateTime)oFactura.dFechaVencimiento,
                            FechaPagoNegociado = (DateTime)oFactura.dFechaPagoNegociado,
                            NombreDocumentoXML = oFactura.NombreDocumentoXML,
                            RutaDocumentoXML = Path.Combine(path, randon + "_" + oFactura.NombreDocumentoXML),
                            NombreDocumentoPDF = "",
                            RutaDocumentoPDF = "",
                            UsuarioCreador = userName
                        });
                        if (!result.Succeeded)
                            throw new Exception(archivoXML.Message);

                        //****************************************************************************
                        //  ACTUALIZA EL ESTADO DE LAS FACTURAS
                        //****************************************************************************
                        if (!bNewOpe)
                            result = await _facturaOperacionesProxy.Editar(new OperacionesFacturaEditDto()
                            {
                                FechaPagoNegociado = (DateTime)oFactura.dFechaPagoNegociado,
                                UsuarioActualizacion = userName,
                                IdOperacionesFacturas = operacionId,
                                Estado = nEstadoOperacion
                            });
                    }
                    else
                        throw new Exception(archivoXML.Message);
                }

                //****************************************************************************
                //  ELIMINA LOS ARCHIVOS
                //****************************************************************************
                path = $"{_env.WebRootPath}\\assets\\upload";
                System.IO.DirectoryInfo di = new DirectoryInfo(path);
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }

                return Json(new { succeeded = resultOpe.Succeeded, message = resultOpe.Message, operationID = operacionId });
            }
            catch (Exception ex)
            {
                return Json(new { succeeded = false, message = ex.Message });
            }
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}
