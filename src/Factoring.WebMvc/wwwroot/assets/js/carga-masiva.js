'use strict';
var globalPath = $('base').attr('href');
var CargaMasiva = function () {
    var datatableFacturas;
    var lstFactura;
    var lstCategoria;
    var handleAddFacturaForm = function () {
        var formAddFactura = document.getElementById('kt_form_add_factura');
        if (!formAddFactura) {
            return;
        }

        var formRegister = document.getElementById('kt_register_form');
        if (!formRegister) {
            return;
        }

        

        var addButton = document.getElementById('kt_add_xml');
        var validator;
        $('#IdOperacionCabeceraFacturas').val($('#IdOperacion').val());
        validator = FormValidation.formValidation(
            formAddFactura,
            {
                fields: {
                    'fileXml': {
                        validators: {
                            file: {
                                extension: 'xml',
                                type: 'text/xml',
                                message: 'Debe adjuntar un archivo XML',
                            }
                        }
                    }
                },
                plugins: {
                    declarative: new FormValidation.plugins.Declarative({
                        html5Input: true,
                    }),
                    trigger: new FormValidation.plugins.Trigger(),
                    bootstrap: new FormValidation.plugins.Bootstrap5({
                        rowSelector: '.fv-row',
                        eleValidClass: '',
                        eleInvalidClass: '',
                    })
                }
            }
        );
        addButton.addEventListener('click', function (e) {
            e.preventDefault();

            datatableFacturas.clear().draw();

            validator.validate().then(function (status) {
                if (status == 'Valid') {
                    addButton.setAttribute('data-kt-indicator', 'on');
                    addButton.disabled = true;
                    setTimeout(function () {
                        var tokenVerification = $('input[name="__RequestVerificationToken"]').val();
                        var fileInputXml = $('#fileXml')[0];
                        var formData = new FormData();
                        for (var x = 0; x < fileInputXml.files.length; x++) {
                            formData.append("fileXml", fileInputXml.files[x]);
                        }

                        $.ajax({
                            type: 'POST',
                            url: $(formAddFactura).attr('action'),
                            dataType: 'json',
                            data: formData,
                            cache: false,
                            contentType: false,
                            processData: false,
                            headers: {
                                'RequestVerificationToken': tokenVerification
                            },
                            success: function (data) {
                                if (data.succeeded) {
                                    Swal.fire({
                                        html: data.message,
                                        icon: 'success',
                                        buttonsStyling: false,
                                        confirmButtonText: 'Listo',
                                        customClass: {
                                            confirmButton: 'btn btn-primary'
                                        }
                                    }).then(function (result) {
                                        if (result.isConfirmed) {
                                            $(addButton).closest('form').find('input[type=text], textarea').val('');
                                            addButton.removeAttribute('data-kt-indicator');
                                            addButton.disabled = false;

                                            lstFactura = data.data;
                                            datatableFacturas.rows.add(data.data).draw();

                                            lstCategoria = data.comboCategoria;
                                            setComboCategoria();
                                        }
                                    });
                                } else {
                                    addButton.removeAttribute('data-kt-indicator');
                                    addButton.disabled = false;
                                    messageError(data.message);
                                }
                            },
                            error: function (jqXHR, textStatus, errorThrown) {
                                addButton.removeAttribute('data-kt-indicator');
                                addButton.disabled = false;
                                messageError(errorThrown);
                            }
                        });
                    }, 2000);
                } else {
                    messageError('Favor de ingresar los campos obligatorios y volver a intentarlo.');
                }
            });
        });

        //var searchButton = document.getElementById('kt_search');
        //searchButton.addEventListener('click', function (e) {
        //    e.preventDefault();

        //    var nRowCount = datatableFacturas.data().count();
        //    if (nRowCount == 0) {
        //        messageError("Debe cargar un archivo como mínimo...");
        //        return;
        //    }

        //    $("#kt_operation_form")[0].reset();
        //    $("#kt_operation_modal").modal("show");

        //    setTimeout(function () {
        //        $("#txtNROperacion").focus();
        //    }, 1000);
        //});

        var registerButton = document.getElementById('kt_register');
        registerButton.addEventListener('click', function (e) {
            e.preventDefault();

            var nRowCount = datatableFacturas.data().count();
            if (nRowCount == 0) {
                messageError("Debe cargar un archivo como mínimo...");
                return;
            }

            var oRecord = datatableFacturas.row(0).data();
            var nMontoPlanilla = 0;
            datatableFacturas.rows().every(function (rowIdx, tableLoop, rowLoop) {
                var data = this.data();
                nMontoPlanilla += parseFloat(data.importeNetoFactura);
            });

            $("#kt_register_form")[0].reset();

            $("#txtIDOperacion").val(0);
            $("#txtRazonSocialGirador").val(oRecord.girador);
            $("#txtRazonSocialAdquiriente").val(oRecord.aceptante);
            $("#txtMonto").val(nMontoPlanilla.toFixed(2));
            var oControl = document.getElementById('txtMonto');
            oControl.disabled = true;
            setComboCategoria()
            $("#kt_register_modal").modal("show");
        });

        var processRegisterButton = document.getElementById('kt_register_process');
        var validatorprocessRegisterButton;
        $('#IdOperacionCabeceraFacturas').val($('#IdOperacion').val());
        validatorprocessRegisterButton = FormValidation.formValidation(
            formRegister,
            {
                fields: {
                    'TEM': {
                        validators: {
                            notEmpty: {
                                message: 'Tasa Mensual es obligatorio'
                            }
                        }
                    },
                    'PorcentajeFinanciamiento': {
                        validators: {
                            notEmpty: {
                                message: '% Financiamiento es obligatorio'
                            }
                        }
                    },
                    'DescCobranza': {
                        validators: {
                            notEmpty: {
                                message: '% Comision es obligatorio'
                            }
                        }
                    }

                },
                plugins: {
                    declarative: new FormValidation.plugins.Declarative({
                        html5Input: true,
                    }),
                    trigger: new FormValidation.plugins.Trigger(),
                    bootstrap: new FormValidation.plugins.Bootstrap5({
                        rowSelector: '.fv-row',
                        eleValidClass: '',
                        eleInvalidClass: '',
                    })
                }
            }
        );

        processRegisterButton.addEventListener('click', function (e) {
            e.preventDefault();
            validatorprocessRegisterButton.validate().then(function (status) {
                if (status == 'Valid') {

                    processRegisterButton.setAttribute('data-kt-indicator', 'on');
                    processRegisterButton.disabled = true;

                    var nCategoria = parseInt($("#comboCategoria").val());
                    var nTEM = ($("#txtTEM").val() == "" ? 0 : $("#txtTEM").val());
                    var nDescCobranza = ($("#txtDescCobranza").val() == "" ? 0 : $("#txtDescCobranza").val());
                    var nFinanciamiento = ($("#txtFinanciamiento").val() == "" ? 0 : $("#txtFinanciamiento").val());
                    var nTasa = ($("#txtTasaMoratoria").val() == "" ? 0 : $("#txtTasaMoratoria").val());
                    var nMonto = ($("#txtMonto").val() == "" ? 0 : $("#txtMonto").val());
                    var nRetencion = ($("#txtRetencion").val() == "" ? 0 : $("#txtRetencion").val());
                    var tokenVerification = $('input[name="__RequestVerificationToken"]').val();
                    var lstInvoice = new Array();
                    for (let i = 0; i < lstFactura.length; i++) {
                        var json = JSON.stringify(lstFactura[i]);
                        var oRecord = JSON.parse(json);

                        oRecord.IdCategoria = nCategoria;
                        oRecord.nTEM = nTEM;
                        oRecord.ComisionCobranza = nDescCobranza;
                        oRecord.nPorcentajeFinanciamiento = nFinanciamiento;
                        oRecord.interesMoratorio = nTasa;
                        oRecord.nMontoOperacion = nMonto;
                        oRecord.retencion = nRetencion;

                        lstInvoice.push(oRecord);
                    }

                    var formData = new FormData();
                    var fileInputXml = $('#fileXml')[0];
                    for (var x = 0; x < fileInputXml.files.length; x++) {
                        formData.append("fileXml", fileInputXml.files[x]);
                    }
                    formData.append('operacionId', $('#txtIDOperacion').val());
                    formData.append('lstFacturas', JSON.stringify(lstInvoice));

                    processRegisterButton.setAttribute('data-kt-indicator', 'on');
                    processRegisterButton.disabled = true;

                    $.ajax({
                        type: 'POST',
                        url: globalPath + "CargaMasiva/ProcessBatch",
                        data: formData,
                        cache: false,
                        contentType: false,
                        processData: false,
                        headers: {
                            'RequestVerificationToken': tokenVerification
                        },
                        success: function (data) {
                            setTimeout(function () {
                                processRegisterButton.removeAttribute('data-kt-indicator');
                                processRegisterButton.disabled = false;
                            }, 1000);

                            if (data.succeeded) {
                                Swal.fire({
                                    html: data.message,
                                    icon: 'success',
                                    buttonsStyling: false,
                                    confirmButtonText: 'Listo',
                                    customClass: {
                                        confirmButton: 'btn btn-primary'
                                    }
                                }).then(function (result) {
                                    if (result.isConfirmed)
                                        $(window).attr('location', globalPath + 'CargaMasiva/Index');
                                });

                                $("#kt_operation_modal").modal("hide");

                            } else {
                                messageError(data.message);
                            }
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            processRegisterButton.removeAttribute('data-kt-indicator');
                            processRegisterButton.disabled = false;
                            messageError(errorThrown);
                        }
                    });

                     } else {
                    messageError('Favor de ingresar los campos obligatorios y volver a intentarlo.');
                }
                });

           
        });

        //
        var formOperacion = document.getElementById('kt_operation_form');
        if (!formOperacion) {
            return;
        }
        var searchButtonOperacion = document.getElementById('kt_search_button_operacion');
        searchButtonOperacion.addEventListener('click', function (e) {
            e.preventDefault();

            setTimeout(function () {
                searchButtonOperacion.setAttribute('data-kt-indicator', 'on');
                searchButtonOperacion.disabled = true;
            }, 1000);

            var sNroOperacion = $("#txtNROperacion").val();
            $.ajax({
                type: 'POST',
                url: $(formOperacion).attr('action'),
                data: { sNumOperacion: sNroOperacion },
                success: function (data) {
                    setTimeout(function () {
                        searchButtonOperacion.removeAttribute('data-kt-indicator');
                        searchButtonOperacion.disabled = false;
                    }, 1000);

                    if (data.succeeded) {
                        $("#kt_register_form")[0].reset();

                        var oRecord = data.data;
                        $("#txtIDOperacion").val(oRecord.nIdOperaciones);
                        $("#txtRazonSocialGirador").val(oRecord.cRazonSocialGirador);
                        $("#txtRazonSocialAdquiriente").val(oRecord.cRazonSocialAdquiriente);
                        $("#txtTEM").val(oRecord.nTEM);
                        $("#txtFinanciamiento").val(oRecord.nPorcentajeFinanciamiento);
                        $("#txtDescCobranza").val(oRecord.nDescCobranza);
                        $("#txtTasaMoratoria").val(oRecord.interesMoratorio);
                        $("#txtRetencion").val(oRecord.nPorcentajeRetencion);
                        $("#comboCategoria").val(oRecord.IdCategoria);

                        var nMontoPlanilla = parseFloat(oRecord.nMontoOperacion);
                        datatableFacturas.rows().every(function (rowIdx, tableLoop, rowLoop) {
                            var data = this.data();
                            nMontoPlanilla += parseFloat(data.importeNetoFactura);
                        });
                        $("#txtMonto").val(nMontoPlanilla.toFixed(2));
                        var oControl = document.getElementById('txtMonto');
                        oControl.disabled = false;

                        setTimeout(function () {
                            //$("#comboCategoria").focus();

                            $("#kt_register_modal").modal("show");
                            //$("#kt_register_modal").focus();
                            $("#txtTEM").focus();
                        }, 1000);

                        $("#kt_operation_modal").modal("hide");
                    } else {
                        Swal.fire({
                            title: "<strong>Operación : " + sNroOperacion + "</strong>",
                            icon: 'error',
                            html: data.message
                        })
                        //messageError(data.message);
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    searchButtonOperacion.removeAttribute('data-kt-indicator');
                    searchButtonOperacion.disabled = false;
                    messageError(errorThrown);
                }
            });
        });

        function setComboCategoria() {
            //******************************************************************
            //  CATEGORIA DEL GIRADOR
            //******************************************************************
            //  1	ORIGINACIÓN
            //  2	TRADICIONAL
            //  3	CONFIRMING ORIGINACIÓN
            //  4	CONFIRMING TRADICIONAL
            //******************************************************************
            $("#comboCategoria").empty();
            if (lstCategoria.length > 0) {
                var dropdown = document.getElementById("comboCategoria");
                for (var i = 0; i < lstCategoria.length; i++) {
                    var oRecCat = lstCategoria[i];
                    dropdown.options[i] = new Option(oRecCat.cNombre, oRecCat.nId);
                }
                $("#comboCategoria").val(2).trigger('change');

                //$("#comboCategoria").prepend('<option selected="selected" value="0"> Seleccione Categoría </option>');
            }
           
        }

        $("#comboCategoria")
            .change(function () {
                var nCategoria = parseInt($(this).val());

                if (nCategoria == 1 || nCategoria == 3) {
                    $("#box-tasa").show();
                    $("#box-retencion").show();
                }
                else {
                    $("#txtTasaMoratoria").val(0);
                    $("#txtRetencion").val(0);
                    $("#box-tasa").hide();
                    $("#box-retencion").hide();
                }
            });
    }

    var initDataTableFacturas = function () {
        var tableFacturas = document.getElementById('kt_facturas_table');
        if (!tableFacturas) {
            return;
        }

        var groupColumn = 0;
        $(tableFacturas).DataTable({ ordering: false }).clear().destroy();
        $.fn.dataTable.ext.errMode = 'none';
        datatableFacturas = $(tableFacturas).DataTable({
            ordering: false,
            columns: [
                { data: 'girador', 'autoWidth': true, class: 'text-left' },
                { data: 'aceptante', 'autoWidth': true, class: 'text-left' },
                { data: 'nroFactura', 'autoWidth': true, class: 'text-center' },
                {
                    data: 'dFechaEmision', 'autoWidth': true, class: 'text-center', render: function (value) {
                        return moment(value).format('DD/MM/YYYY');
                    }
                },
                {
                    data: 'dFechaVencimiento', 'autoWidth': true, class: 'text-center', render: function (value) {
                        return moment(value).format('DD/MM/YYYY');
                    }
                },
                {
                    data: 'dFechaPagoNegociado', 'autoWidth': true, class: 'text-center', render: function (value) {
                        return moment(value).format('DD/MM/YYYY');
                    }
                },
                { data: 'cMoneda', 'autoWidth': true, class: 'text-center' },
                { data: 'importeNetoFactura', 'autoWidth': true, class: 'text-end', render: $.fn.dataTable.render.number(',', '.', 2, '') }
            ],
            columnDefs: [{ visible: false, targets: groupColumn }],
            order: [[groupColumn, 'asc']],
            //columnDefs: [{ visible: false, targets: [0,1] }],
            //order: [[0, 'asc'], [1, 'asc']],
            createdRow: function (row, data, index) {
                if (data.estado == "OK")
                    $(row).css("background-color", "white");
                else
                    $(row).css("background-color", "orange");
            },
            drawCallback: function (settings) {
                var api = this.api();
                var rows = api.rows({ page: 'current' }).nodes();
                var last = null;

                api
                    .column(groupColumn, { page: 'current' })
                    .data()
                    .each(function (group, i) {
                        if (last !== group) {
                            $(rows)
                                .eq(i)
                                .before('<tr class="group"><td colspan="8">GIRADOR : ' + group + '</td></tr>');

                            last = group;
                        }
                    });
            }
        });
    }

    return {
        init: function () {
            initDataTableFacturas();
            handleAddFacturaForm();
        }
    }
}();
KTUtil.onDOMContentLoaded(function () {
    CargaMasiva.init();
});