'use strict';
var globalPath = $('base').attr('href');
var Fondeador = function () {
    var datatable;
    var datatableFondeo;
    var datatableCavaliFactoring;
    var datatableCuentaBancaria;
    var datatableContacto;
    var datatableRepresentanteLegal;
    var datatableUbicacion;
    var datatableDocumento;
    var datatableFondeoPrestamo;
    var handleFilterTable = function () {
        var searchButton = document.getElementById('kt_search_button');
        if (!searchButton) {
            return;
        }
        searchButton.addEventListener('click', function (e) {
            e.preventDefault();
            $(searchClear).hide();
            searchButton.setAttribute('data-kt-indicator', 'on');
            searchButton.disabled = true;
            setTimeout(function () {
                searchButton.removeAttribute('data-kt-indicator');
                searchButton.disabled = false;
                $(searchClear).show();
                initDatatable();
            }, 2000);
        });
        var searchClear = document.getElementById('kt_search_clear');
        if (!searchClear) {
            return;
        }
        searchClear.addEventListener('click', function (e) {
            e.preventDefault();
            $(this).closest('form').find('input[type=text], textarea').val('');
            $(this).closest('form').find('select').val('').trigger('change');
            $('#IdEstado').val(1);
            $('#IdEstado').select2();
            initDatatable();
        });

    }
    var initDatatable = function () {
        var table = document.getElementById('kt_fondeador_table');
        if (!table) {
            return;
        }
        $(table).DataTable({ ordering: false }).clear().destroy();
        $.fn.dataTable.ext.errMode = 'none';
        datatable = $(table).DataTable({
            searchDelay: 500,
            processing: true,
            serverSide: true,
            stateSave: false,
            ordering: false,
            ajax: {
                url: globalPath + 'Fondeador/ListadoRegistros',
                type: 'POST',
                datatype: 'json',
                data: $('#kt_search_form').serializeObject()
            },
            columns: [
                { data: 'nIdFondeador', name: 'nIdFondeador', 'autoWidth': true, class: 'text-center' },
                { data: 'cNroDocumento', 'autoWidth': true, class: 'text-center' },
                { data: 'cNombreFondeador', 'autoWidth': true, class: 'text-center' },
                { data: 'dFecRegistro', 'autoWidth': true, class: 'text-left' },
                { data: 'nombreEstado', 'autoWidth': true, class: 'text-left' },
                { data: null, 'autoWidth': true, class: 'text-center', responsivePriority: -1 }
            ],
            columnDefs: [
                {
                    targets: 0,
                    data: null,
                    orderable: false,
                    className: 'text-center',
                    render: function (data, type, row) {
                        var checkbox = `<div class="form-check form-check-sm form-check-custom form-check-solid"><input class="form-check-input checkbox-main" type="checkbox" value="${row.iIdFondeador}" /></div>`;
                        return (row.iEstadoFile == 1) ? `` : checkbox;
                    }
                },
                {
                    //targets: -1,
                    //data: null,
                    //orderable: false,
                    //className: 'text-center',
                    //render: function (data, type, row) {
                    //    var button = `<button type="button" class="btn btn-sm btn-icon btn-light btn-active-light-primary edit-row"><i class="las la-pen fs-2"></i></button>`;
                    //    return (data.iEstadoFile == 1) ? `` : button;
                    //}
                    targets: -1,
                    data: null,
                    orderable: false,
                    className: 'text-end',
                    render: function (data, type, row) {
                        return `<a href="javascript:;" class="btn btn-outline btn-outline-solid btn-outline-primary btn-active-light-primary btn-icon-primary btn-sm" data-kt-menu-trigger="click" data-kt-menu-placement="bottom-end" data-kt-menu-flip="top-end">
                            Acciones
                            <span class="svg-icon svg-icon-5 m-0">
                                <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1">
                                    <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">
                                        <polygon points="0 0 24 0 24 24 0 24"></polygon>
                                        <path d="M6.70710678,15.7071068 C6.31658249,16.0976311 5.68341751,16.0976311 5.29289322,15.7071068 C4.90236893,15.3165825 4.90236893,14.6834175 5.29289322,14.2928932 L11.2928932,8.29289322 C11.6714722,7.91431428 12.2810586,7.90106866 12.6757246,8.26284586 L18.6757246,13.7628459 C19.0828436,14.1360383 19.1103465,14.7686056 18.7371541,15.1757246 C18.3639617,15.5828436 17.7313944,15.6103465 17.3242754,15.2371541 L12.0300757,10.3841378 L6.70710678,15.7071068 Z" fill="#000000" fill-rule="nonzero" transform="translate(12.000003, 11.999999) rotate(-180.000000) translate(-12.000003, -11.999999)"></path>
                                    </g>
                                 </svg>
                            </span>
                        </a>
                        <div class="menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-gray-600 menu-state-bg-light-primary fw-bold fs-7 w-125px py-4" data-kt-menu="true">
                            <div class="menu-item px-3 oculto-acci p-act">
                                <a href="` + globalPath + `Fondeador/Registro?fondeadorId=` + data.nIdFondeador + `" class="menu-link px-3">Editar</a>
                            </div>
                         
                        </div>`;
                    }
                }
            ]
        });
        table = datatable.$;
        datatable.on('draw', function () {
            var searchButton = document.getElementById('kt_search_button');
            var searchClear = document.getElementById('kt_search_clear');
            searchButton.removeAttribute('data-kt-indicator');
            searchButton.disabled = false;
            $(searchClear).show();
            KTMenu.createInstances();
            //initToggleToolbar();
            //toggleToolbars();
            //handleModalControlDocumentario();
            Common.init();
        });
    }
    var handleRegistroFondeador = function () {
        var formRegistroFondeador = document.getElementById('kt_form_registro');
        if (!formRegistroFondeador) {
            return;
        }
        var saveButton = document.getElementById('kt_save_button_registro');
        var validator;
        validator = FormValidation.formValidation(
            formRegistroFondeador,
            {
                fields: {
                    'IdTipoDocumento': {
                        validators: {
                            notEmpty: {
                                message: 'Tipo Documento es obligatorio'
                            }
                        }
                    },
                    'DOI': {
                        validators: {
                            notEmpty: {
                                message: 'DOI o RUC es obligatorio'
                            }
                        }
                    },
                    'RazonSocial': {
                        validators: {
                            notEmpty: {
                                message: 'Razón Social es obligatorio'
                            }
                        }
                    },
                },
                plugins: {
                    trigger: new FormValidation.plugins.Trigger(),
                    bootstrap: new FormValidation.plugins.Bootstrap5({
                        rowSelector: '.fv-row',
                        eleValidClass: '',
                        eleInvalidClass: '',
                    })
                }
            }
        );
        saveButton.addEventListener('click', function (e) {
            e.preventDefault();
            validator.validate().then(function (status) {
                if (status == 'Valid') {
                    saveButton.setAttribute('data-kt-indicator', 'on');
                    saveButton.disabled = true;
                    var idFondeador = document.getElementById('IdFondeador');
                    var urlAction = $(formRegistroFondeador).attr('action');
                    if (idFondeador) {
                        urlAction += '?fondeadorId=' + $(idFondeador).val();
                    }

                    $.ajax({
                        type: 'POST',
                        dataType: 'json',
                        //url: $(formRegistroFondeador).attr('action'),
                        url: urlAction,

                        xhrFields: {
                            withCredentials: true
                        },
                        data: $(formRegistroFondeador).serializeObject(),
                        success: function (data) {
                            saveButton.removeAttribute('data-kt-indicator');
                            saveButton.disabled = false;
                            if (data.succeeded) {
                                Swal.fire({
                                    text: data.message,
                                    icon: 'success',
                                    buttonsStyling: false,
                                    confirmButtonText: 'Listo',
                                    customClass: {
                                        confirmButton: 'btn fw-bold btn-primary',
                                    }
                                }).then(function () {
                                    $(window).attr('location', globalPath + 'Fondeador/Registro?fondeadorId=' + data.data);
                                });
                            } else {
                                //messageError('Ocurrio un error en el proceso.');
                                saveButton.removeAttribute('data-kt-indicator');
                                saveButton.disabled = false;
                                messageError(data.message);
                            }
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            saveButton.removeAttribute('data-kt-indicator');
                            saveButton.disabled = false;
                            messageError(errorThrown);
                        }
                    });

                }
            });
        });
    }

    var handleAddFondeForm = function () {
        var formAddFondeo = document.getElementById('kt_form_add_fondeo');
        if (!formAddFondeo) {
            return;
        }
        var addButton = document.getElementById('kt_add_fondeo');
        var validator;
        $('#IdFondeadorCabecera').val($('#IdFondeador').val());

        validator = FormValidation.formValidation(
            formAddFondeo,
            {
                fields: {
                    'IdTipoNegocio': {
                        validators: {
                            notEmpty: {
                                message: 'Tipo de Negocio es obligatorio'
                            }
                        }
                    },
                    'IdProducto': {
                        validators: {
                            notEmpty: {
                                message: 'Producto es obligatorio'
                            }
                        }
                    },
                    'IdMonedaFondeo': {
                        validators: {
                            notEmpty: {
                                message: 'Moneda es obligatorio'
                            }
                        }
                    },
                    'TasaFondeoAnual': {
                        validators: {
                            notEmpty: {
                                message: 'Tasa Fondeo Anual es obligatorio'
                            }
                        }
                    },
                    'TasaMoratoriaAnual': {
                        validators: {
                            notEmpty: {
                                message: 'Tasa Moratorio Anual es obligatorio'
                            }
                        }
                    },
                    'IdMetodoCalculo': {
                        validators: {
                            notEmpty: {
                                message: 'Metodo de Calculo es obligatorio'
                            }
                        }
                    },
                    'IdModalidad': {
                        validators: {
                            notEmpty: {
                                message: 'Modalidad es obligatorio'
                            }
                        }
                    },
                    'CapitalFinanciado': {
                        validators: {
                            notEmpty: {
                                message: 'Capital Financiado es obligatorio'
                            }
                        }
                    },
                    'DiasPago': {
                        validators: {
                            notEmpty: {
                                message: 'Dias Pago es obligatorio'
                            },
                            digits: {
                                message: 'Ingresar sólo números'
                            }
                        }
                    },
                    'IdRetencionInicial': {
                        validators: {
                            notEmpty: {
                                message: 'Retencion Inicial es obligatorio'
                            }
                        }
                    },
                    'IdCalculoInteres': {
                        validators: {
                            notEmpty: {
                                message: 'Calculo Interes es obligatorio'
                            }
                        }
                    }
                },

                plugins: {
                    //declarative: new FormValidation.plugins.Declarative({
                    //    html5Input: true,
                    //}),
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
            validator.validate().then(function (status) {
                if (status == 'Valid') {
                    addButton.setAttribute('data-kt-indicator', 'on');
                    addButton.disabled = true;
                    setTimeout(function () {
                        $.ajax({
                            type: 'POST',
                            dataType: 'json',
                            url: $(formAddFondeo).attr('action'),
                            data: $(formAddFondeo).serializeObject(),
                            xhrFields: {
                                withCredentials: true
                            },
                            success: function (data) {
                                if (data.succeeded) {
                                    Swal.fire({
                                        text: data.message,
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
                                            initDataTableFondeo();
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
                }
                //else {
                //    messageError('Lo sentimos, parece que se han detectado algunos errores. Vuelve a intentarlo.');
                //}
            });
        });
    }

    var handleDeleteFondeoForm = function () {
        var tableFondeadorFondeo = document.querySelector('#kt_fondeador_fondeo_table');
        if (!tableFondeadorFondeo) {
            return;
        }
        var deleteFondeoButton = tableFondeadorFondeo.querySelectorAll('[data-kt-fondeo-table-filter="delete_row"]');
        deleteFondeoButton.forEach(d => {
            d.addEventListener('click', function (e) {
                e.preventDefault();
                var idFondeador = $(this).data('parent');
                var idFondeo = $(this).data('id');
                var parent = e.target.closest('tr');
                var name = parent.querySelectorAll('td')[0].innerText + ' ' + parent.querySelectorAll('td')[1].innerText;
                Swal.fire({
                    text: '¿Estás seguro de que quieres eliminar a ' + name + '?',
                    icon: 'warning',
                    showCancelButton: true,
                    buttonsStyling: false,
                    confirmButtonText: 'Sí, eliminar!',
                    cancelButtonText: 'Cancelar',
                    customClass: {
                        confirmButton: 'btn fw-bold btn-danger',
                        cancelButton: 'btn fw-bold btn-active-light-primary'
                    }
                }).then(function (result) {
                    if (result.value) {
                        var token = $('input[name="__RequestVerificationToken"]').val();
                        $.ajax({
                            type: 'POST',
                            dataType: 'json',
                            url: globalPath + 'Fondeador/EliminarFondeo',
                            data: {
                                fondeadorFondeoId: idFondeo
                            },
                            headers: {
                                'RequestVerificationToken': token
                            },
                            success: function (data) {
                                if (data) {
                                    Swal.fire({
                                        text: 'Eliminaste correctamente a ' + name + '.',
                                        icon: 'success',
                                        buttonsStyling: false,
                                        confirmButtonText: 'Listo',
                                        customClass: {
                                            confirmButton: 'btn fw-bold btn-primary',
                                        }
                                    }).then(function () {
                                        datatableFondeo.row($(parent)).remove().draw();
                                    });
                                } else {
                                    messageError(name + ' no fue eliminado.');
                                }
                            },
                            error: function (jqXHR, textStatus, errorThrown) {
                                messageError(errorThrown);
                            }
                        });
                    } else if (result.dismiss === 'cancel') {
                        messageError(name + ' no fue eliminado.');
                    }
                });
            });
        });
    }


    var initDataTableFondeo = function () {

        var idFondeador = document.getElementById('IdFondeador');
        if (!idFondeador) {
            return;
        }
        var tableFondeo = document.getElementById('kt_fondeador_fondeo_table');
        if (!tableFondeo) {
            return;
        }
        var tableFondeoAction = $('#Action').val();
        $(tableFondeo).DataTable({ ordering: false }).clear().destroy();
        $.fn.dataTable.ext.errMode = 'none';
        datatableFondeo = $(tableFondeo).DataTable({
            ordering: false,
            ajax: {
                type: 'GET',
                dataType: 'json',
                url: globalPath + 'Fondeador/GetAllFondeo?fondeadorId=' + $(idFondeador).val(),
                dataSrc: function (data) {

                    return data.data;
                }
            },
            columns: [
                { data: 'tipodeNegocio', 'autoWidth': true, class: 'text-center' },
                { data: 'nombreProducto', 'autoWidth': true, class: 'text-center' },
                { data: 'moneda', 'autoWidth': true, class: 'text-center' },
                { data: 'nTasaFondeo', 'autoWidth': true, class: 'text-center' },
                { data: 'nTasaMora', 'autoWidth': true, class: 'text-center' },
                { data: 'nombreMetodoCalculo', 'autoWidth': true, class: 'text-center' },
                { data: 'modalidad', 'autoWidth': true, class: 'text-center' },
                { data: 'nCapitalFinanciado', 'autoWidth': true, class: 'text-center' },
                { data: 'iDiaPago', 'autoWidth': true, class: 'text-center' },
                { data: 'retencionInicialFondeador', 'autoWidth': true, class: 'text-center' },
                { data: 'calculoInteres', 'autoWidth': true, class: 'text-center' },
                { data: null, 'autoWidth': true, class: 'text-center', responsivePriority: -1 }
            ],
            columnDefs: [
                {
                    targets: -1,
                    data: null,
                    visible: (tableFondeoAction == 'Detalle') ? false : true,
                    orderable: false,
                    className: 'text-end',
                    render: function (data, type, row) {
                        return `<a href="javascript:;" class="btn btn-icon btn-light-dark btn-sm" data-kt-fondeo-table-filter="delete_row" data-parent="` + $(idFondeador).val() + `" data-id="` + row.iIdFondeadorDatos + `"><i class="las la-trash fs-2"></i></a>`;
                    }
                }
            ]
        });
        datatableFondeo.on('draw', function () {
            handleDeleteFondeoForm();
        });
    }

    var handleAddCavaliFactoringForm = function () {
        var formAdd = document.getElementById('kt_form_add_cavali_factoring');
        if (!formAdd) {
            return;
        }
        var addButton = document.getElementById('kt_add_cavali_factoring');
        var validator;
        $('#IdFondeadorCabeceraCF').val($('#IdFondeador').val());
        validator = FormValidation.formValidation(
            //formAdd,
            //{
            //    fields: {
            //        'CodigoParticipante': {
            //            validators: {
            //                notEmpty: {
            //                    message: 'Codigo Participante es obligatorio'
            //                }
            //            }
            //        },
            //        'CodigoRUT': {
            //            validators: {
            //                notEmpty: {
            //                    message: 'Codigo RUT es obligatorio'
            //                }
            //            }
            //        },

            //    },
            //    plugins: {
            //        trigger: new FormValidation.plugins.Trigger(),
            //        bootstrap: new FormValidation.plugins.Bootstrap5({
            //            rowSelector: '.fv-row',
            //            eleValidClass: '',
            //            eleInvalidClass: '',
            //        })
            //    }
            //}
        );
        addButton.addEventListener('click', function (e) {
            e.preventDefault();
            validator.validate().then(function (status) {
                if (status == 'Valid') {
                    addButton.setAttribute('data-kt-indicator', 'on');
                    addButton.disabled = true;
                    setTimeout(function () {
                        $.ajax({
                            type: 'POST',
                            dataType: 'json',
                            url: $(formAdd).attr('action'),
                            data: $(formAdd).serializeObject(),
                            xhrFields: {
                                withCredentials: true
                            },
                            success: function (data) {
                                if (data.succeeded) {
                                    Swal.fire({
                                        text: data.message,
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
                                            initDataTableCavaliFactoring();
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
                }
                //else {
                //    messageError('Lo sentimos, parece que se han detectado algunos errores. Vuelve a intentarlo.');
                //}
            });
        });
    }
    var handleDeleteCavaliFactoringForm = function () {
        var tableFondeadorCavaliFactoring = document.querySelector('#kt_fondeador_cavali_factoring_table');
        if (!tableFondeadorCavaliFactoring) {
            return;
        }
        var deleteFondeoButton = tableFondeadorCavaliFactoring.querySelectorAll('[data-kt-cavali-factoring-table-filter="delete_row"]');
        deleteFondeoButton.forEach(d => {
            d.addEventListener('click', function (e) {
                e.preventDefault();
                var idFondeador = $(this).data('parent');
                var idid = $(this).data('id');
                var parent = e.target.closest('tr');
                var name = parent.querySelectorAll('td')[0].innerText + ' ' + parent.querySelectorAll('td')[1].innerText;
                Swal.fire({
                    text: '¿Estás seguro de que quieres eliminar a ' + name + '?',
                    icon: 'warning',
                    showCancelButton: true,
                    buttonsStyling: false,
                    confirmButtonText: 'Sí, eliminar!',
                    cancelButtonText: 'Cancelar',
                    customClass: {
                        confirmButton: 'btn fw-bold btn-danger',
                        cancelButton: 'btn fw-bold btn-active-light-primary'
                    }
                }).then(function (result) {
                    if (result.value) {
                        var token = $('input[name="__RequestVerificationToken"]').val();
                        $.ajax({
                            type: 'POST',
                            dataType: 'json',
                            url: globalPath + 'Fondeador/EliminarCavaliFactoring',
                            data: {
                                fondeadorCavaliFactoringId: idid
                            },
                            headers: {
                                'RequestVerificationToken': token
                            },
                            success: function (data) {
                                if (data) {
                                    Swal.fire({
                                        text: 'Eliminaste correctamente a ' + name + '.',
                                        icon: 'success',
                                        buttonsStyling: false,
                                        confirmButtonText: 'Listo',
                                        customClass: {
                                            confirmButton: 'btn fw-bold btn-primary',
                                        }
                                    }).then(function () {
                                        datatableCavaliFactoring.row($(parent)).remove().draw();
                                    });
                                } else {
                                    messageError(name + ' no fue eliminado.');
                                }
                            },
                            error: function (jqXHR, textStatus, errorThrown) {
                                messageError(errorThrown);
                            }
                        });
                    } else if (result.dismiss === 'cancel') {
                        messageError(name + ' no fue eliminado.');
                    }
                });
            });
        });
    }
    var initDataTableCavaliFactoring = function () {

        var idFondeador = document.getElementById('IdFondeador');
        if (!idFondeador) {
            return;
        }

        var tableCavaliFactoring = document.getElementById('kt_fondeador_cavali_factoring_table');
        if (!tableCavaliFactoring) {
            return;
        }
        var tableCavaliFactoringAction = $('#Action').val();
        $(tableCavaliFactoring).DataTable({ ordering: false }).clear().destroy();
        $.fn.dataTable.ext.errMode = 'none';
        datatableCavaliFactoring = $(tableCavaliFactoring).DataTable({
            ordering: false,
            ajax: {
                type: 'GET',
                dataType: 'json',
                url: globalPath + 'Fondeador/GetAllCavaliFactoring?fondeadorId=' + $(idFondeador).val(),
                dataSrc: function (data) {

                    return data.data;
                }
            },
            columns: [
                { data: 'nCodParticipante', 'autoWidth': true, class: 'text-center' },
                { data: 'nCodRUT', 'autoWidth': true, class: 'text-center' },

                { data: null, 'autoWidth': true, class: 'text-center', responsivePriority: -1 }
            ],
            columnDefs: [
                {
                    targets: -1,
                    data: null,
                    visible: (tableCavaliFactoringAction == 'Detalle') ? false : true,
                    orderable: false,
                    className: 'text-end',
                    render: function (data, type, row) {
                        return `<a href="javascript:;" class="btn btn-icon btn-light-dark btn-sm oculto-acci p-eli" data-kt-cavali-factoring-table-filter="delete_row" data-parent="` + $(idFondeador).val() + `" data-id="` + row.nIdFondeadorCavali + `"><i class="las la-trash fs-2"></i></a>`;
                    }
                }
            ]
        });
        datatableCavaliFactoring.on('draw', function () {
            handleDeleteCavaliFactoringForm();
            Common.init();
        });
    }

    var handleAddCuentaBancariaForm = function () {
        var formAddCuentaBancaria = document.getElementById('kt_form_add_cuenta_bancaria');
        if (!formAddCuentaBancaria) {
            return;
        }
        var addButton = document.getElementById('kt_add_cuenta_bancaria');
        var validator;
        $('#IdFondeadorCabeceraCB').val($('#IdFondeador').val());
        $('#DOICB').val($('#hDOI').val());

        validator = FormValidation.formValidation(
            formAddCuentaBancaria,
            {
                fields: {
                    'CuentaBancaria': {
                        validators: {
                            notEmpty: {
                                message: 'Cuenta Bancaria es obligatorio'
                            },
                            digits: {
                                message: 'Ingresar sólo números'
                            },
                        }
                    },
                    'CCI': {
                        validators: {
                            notEmpty: {
                                message: 'CCI es obligatorio'
                            },
                            digits: {
                                message: 'Ingresar sólo números'
                            },
                        }
                    },
                    'IdBanco': {
                        validators: {
                            notEmpty: {
                                message: 'Banco es obligatorio'
                            }
                            //,
                            //stringLength: {
                            //    min: 3,
                            //    max: 100,
                            //    message: 'Tasa Fondeo Anual debe tener entre 3 y 100 caracteres'
                            //}
                        }
                    },
                    'IdMoneda': {
                        validators: {
                            notEmpty: {
                                message: 'Moneda es obligatorio'
                            }
                        }
                    },
                    'DocConst': {
                        validators: {
                            notEmpty: {
                                message: 'Documento es obligatorio'
                            }
                        }
                    }

                },
                plugins: {
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
            validator.validate().then(function (status) {
                if (status == 'Valid') {
                    addButton.setAttribute('data-kt-indicator', 'on');
                    addButton.disabled = true;
                    setTimeout(function () {

                        var tokenVerification = $('input[name="__RequestVerificationToken"]').val();
                        var fileDocConst = $('#DocConst')[0];
                        var formData = new FormData();
                        formData.append('DOICB', $('#DOICB').val());
                        formData.append('IdFondeadorCabeceraCB', $('#IdFondeadorCabeceraCB').val());
                        formData.append('CuentaBancaria', $('#CuentaBancaria').val());
                        formData.append('CCI', $('#CCI').val());
                        formData.append('IdBanco', $('#IdBanco').val());
                        formData.append('IdMoneda', $('#IdMoneda').val());
                        formData.append('DocConst', fileDocConst.files[0]);

                        $.ajax({
                            type: 'POST',
                            dataType: 'json',
                            url: $(formAddCuentaBancaria).attr('action'),
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
                                        text: data.message,
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
                                            initDataTableCuentaBancaria();
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
                }
                //else {
                //    messageError('Lo sentimos, parece que se han detectado algunos errores. Vuelve a intentarlo.');
                //}
            });
        });
    }
    var handleDeleteCuentaBancariaForm = function () {

        var tableFondeadorCuentaBancaria = document.querySelector('#kt_fondeador_cuenta_bancaria_table');
        if (!tableFondeadorCuentaBancaria) {
            return;
        }
        var deleteFondeoButton = tableFondeadorCuentaBancaria.querySelectorAll('[data-kt-cuenta-bancaria-table-filter="delete_row"]');
        deleteFondeoButton.forEach(d => {
            d.addEventListener('click', function (e) {
                e.preventDefault();
                var idFondeador = $(this).data('parent');
                var idid = $(this).data('id');
                var parent = e.target.closest('tr');
                var name = parent.querySelectorAll('td')[0].innerText + ' ' + parent.querySelectorAll('td')[1].innerText;
                Swal.fire({
                    text: '¿Estás seguro de que quieres eliminar a ' + name + '?',
                    icon: 'warning',
                    showCancelButton: true,
                    buttonsStyling: false,
                    confirmButtonText: 'Sí, eliminar!',
                    cancelButtonText: 'Cancelar',
                    customClass: {
                        confirmButton: 'btn fw-bold btn-danger',
                        cancelButton: 'btn fw-bold btn-active-light-primary'
                    }
                }).then(function (result) {
                    if (result.value) {
                        var token = $('input[name="__RequestVerificationToken"]').val();
                        $.ajax({
                            type: 'POST',
                            dataType: 'json',
                            url: globalPath + 'Fondeador/EliminarCuentaBancaria',
                            data: {
                                fondeadorCuentaBancariaId: idid
                            },
                            headers: {
                                'RequestVerificationToken': token
                            },
                            success: function (data) {
                                if (data) {
                                    Swal.fire({
                                        text: 'Eliminaste correctamente a ' + name + '.',
                                        icon: 'success',
                                        buttonsStyling: false,
                                        confirmButtonText: 'Listo',
                                        customClass: {
                                            confirmButton: 'btn fw-bold btn-primary',
                                        }
                                    }).then(function () {
                                        datatableCuentaBancaria.row($(parent)).remove().draw();
                                    });
                                } else {
                                    messageError(name + ' no fue eliminado.');
                                }
                            },
                            error: function (jqXHR, textStatus, errorThrown) {
                                messageError(errorThrown);
                            }
                        });
                    } else if (result.dismiss === 'cancel') {
                        messageError(name + ' no fue eliminado.');
                    }
                });
            });
        });
    }
    var initDataTableCuentaBancaria = function () {

        var idFondeador = document.getElementById('IdFondeador');
        if (!idFondeador) {
            return;
        }
        var tableCuentaBancaria = document.getElementById('kt_fondeador_cuenta_bancaria_table');
        if (!tableCuentaBancaria) {
            return;
        }
        var tableCuentaBancariaAction = $('#Action').val();
        $(tableCuentaBancaria).DataTable({ ordering: false }).clear().destroy();
        $.fn.dataTable.ext.errMode = 'none';
        datatableCuentaBancaria = $(tableCuentaBancaria).DataTable({
            ordering: false,
            ajax: {
                type: 'GET',
                dataType: 'json',
                url: globalPath + 'Fondeador/GetAllCuentaBancaria?fondeadorId=' + $(idFondeador).val(),
                dataSrc: function (data) {

                    return data.data;
                }
            },
            columns: [
                { data: 'nombreBanco', 'autoWidth': true, class: 'text-center' },
                { data: 'nombreMoneda', 'autoWidth': true, class: 'text-center' },
                { data: 'cCuentaBancaria', 'autoWidth': true, class: 'text-center' },
                { data: 'cCCI', 'autoWidth': true, class: 'text-center' },
                { data: 'cNombreDocumento', 'autoWidth': true, class: 'text-center' },
                { data: null, 'autoWidth': true, class: 'text-center', responsivePriority: -1 }
            ],
            columnDefs: [
                {
                    targets: -1,
                    data: null,
                    visible: (tableCuentaBancariaAction == 'Detalle') ? false : true,
                    orderable: false,
                    className: 'text-end',
                    render: function (data, type, row) {
                        var buttonDownload = ((data.cNombreDocumento == null || data.cNombreDocumento == '') ? `` : `<a href="javascript:;" class="btn btn-icon btn-sm btn-outline btn-outline-solid btn-outline-default me-2" data-kt-cuenta-bancaria-table-filter="download_file" onclick="Fondeador.Download(this)" data-path="` + data.cRutaDocConst + `" data-filename="` + data.cNombreDocumento + `" title="` + data.cNombreDocumento + `"><i class="las la-download fs-2"></i></a>`);
                        var buttonDelete = ((tableCuentaBancariaAction == 'Detalle') ? `` : `<a href="javascript:;" class="btn btn-icon btn-light-dark btn-sm" data-kt-cuenta-bancaria-table-filter="delete_row" data-parent="` + $(idFondeador).val() + `" data-id="` + row.iIdFondeadorCtasBancarias + `"><i class="las la-trash fs-2"></i></a>`);
                        return buttonDownload + buttonDelete;
                        //return `<a href="javascript:;" class="btn btn-icon btn-light-dark btn-sm" data-kt-cuenta-bancaria-table-filter="delete_row" data-parent="` + $(idFondeador).val() + `" data-id="` + row.iIdFondeadorCtasBancarias + `"><i class="las la-trash fs-2"></i></a>`;
                    }
                }
            ]
        });
        datatableCuentaBancaria.on('draw', function () {
            handleDeleteCuentaBancariaForm();
        });
    }


    var handleAddContactoForm = function () {
        var formAdd = document.getElementById('kt_form_add_contacto');
        if (!formAdd) {
            return;
        }
        var addButton = document.getElementById('kt_add_contacto');
        var validator;
        $('#IdFondeadorCabeceraContacto').val($('#IdFondeador').val());
        validator = FormValidation.formValidation(
            formAdd,
            {
                fields: {
                    'Nombres': {
                        validators: {
                            notEmpty: {
                                message: 'Nombres es obligatorio'
                            }
                        }
                    },
                    'ApellidoPaterno': {
                        validators: {
                            notEmpty: {
                                message: 'Apellido Paterno es obligatorio'
                            }
                        }
                    },
                    'ApellidoMaterno': {
                        validators: {
                            notEmpty: {
                                message: 'Apellido Paterno es obligatorio'
                            }
                            //,
                            //stringLength: {
                            //    min: 3,
                            //    max: 100,
                            //    message: 'Tasa Fondeo Anual debe tener entre 3 y 100 caracteres'
                            //}
                        }
                    },
                    'IdTipoContacto': {
                        validators: {
                            notEmpty: {
                                message: 'Tipo Contacto es obligatorio'
                            }
                        }
                    },
                    'Celular': {
                        validators: {
                            notEmpty: {
                                message: 'Celular es obligatorio'
                            },
                            digits: {
                                message: 'Ingresar sólo números'
                            },
                            stringLength: {
                                min: 9,
                                max: 9,
                                message: 'Celular debe tener entre 9 caracteres'
                            }
                        }
                    }
                    ,
                    'Correo': {
                        validators: {
                            notEmpty: {
                                message: 'Correo es obligatorio'
                            },
                            emailAddress: {
                                message: 'Ingresar un email válido',
                            }
                        }
                    }

                },
                plugins: {
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
            validator.validate().then(function (status) {
                if (status == 'Valid') {
                    addButton.setAttribute('data-kt-indicator', 'on');
                    addButton.disabled = true;
                    setTimeout(function () {
                        $.ajax({
                            type: 'POST',
                            dataType: 'json',
                            url: $(formAdd).attr('action'),
                            data: $(formAdd).serializeObject(),
                            xhrFields: {
                                withCredentials: true
                            },
                            success: function (data) {
                                if (data.succeeded) {
                                    Swal.fire({
                                        text: data.message,
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
                                            initDataTableContacto();
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
                }
                //else {
                //    messageError('Lo sentimos, parece que se han detectado algunos errores. Vuelve a intentarlo.');
                //}
            });
        });
    }
    var handleDeleteContactoForm = function () {

        var tableFondeadorContacto = document.querySelector('#kt_fondeador_contacto_table');
        if (!tableFondeadorContacto) {
            return;
        }
        var deleteFondeoButton = tableFondeadorContacto.querySelectorAll('[data-kt-contacto-table-filter="delete_row"]');
        deleteFondeoButton.forEach(d => {
            d.addEventListener('click', function (e) {
                e.preventDefault();
                var idFondeador = $(this).data('parent');
                var idid = $(this).data('id');
                var parent = e.target.closest('tr');
                var name = parent.querySelectorAll('td')[0].innerText + ' ' + parent.querySelectorAll('td')[1].innerText;
                Swal.fire({
                    text: '¿Estás seguro de que quieres eliminar a ' + name + '?',
                    icon: 'warning',
                    showCancelButton: true,
                    buttonsStyling: false,
                    confirmButtonText: 'Sí, eliminar!',
                    cancelButtonText: 'Cancelar',
                    customClass: {
                        confirmButton: 'btn fw-bold btn-danger',
                        cancelButton: 'btn fw-bold btn-active-light-primary'
                    }
                }).then(function (result) {
                    if (result.value) {
                        var token = $('input[name="__RequestVerificationToken"]').val();
                        $.ajax({
                            type: 'POST',
                            dataType: 'json',
                            url: globalPath + 'Fondeador/EliminarContacto',
                            data: {
                                fondeadorContactoId: idid
                            },
                            headers: {
                                'RequestVerificationToken': token
                            },
                            success: function (data) {
                                if (data) {
                                    Swal.fire({
                                        text: 'Eliminaste correctamente a ' + name + '.',
                                        icon: 'success',
                                        buttonsStyling: false,
                                        confirmButtonText: 'Listo',
                                        customClass: {
                                            confirmButton: 'btn fw-bold btn-primary',
                                        }
                                    }).then(function () {
                                        datatableContacto.row($(parent)).remove().draw();
                                    });
                                } else {
                                    messageError(name + ' no fue eliminado.');
                                }
                            },
                            error: function (jqXHR, textStatus, errorThrown) {
                                messageError(errorThrown);
                            }
                        });
                    } else if (result.dismiss === 'cancel') {
                        messageError(name + ' no fue eliminado.');
                    }
                });
            });
        });
    }
    var initDataTableContacto = function () {

        var idFondeador = document.getElementById('IdFondeador');
        if (!idFondeador) {
            return;
        }
        var tableContacto = document.getElementById('kt_fondeador_contacto_table');
        if (!tableContacto) {
            return;
        }
        var tableContactoAction = $('#Action').val();
        $(tableContacto).DataTable({ ordering: false }).clear().destroy();
        $.fn.dataTable.ext.errMode = 'none';
        datatableContacto = $(tableContacto).DataTable({
            ordering: false,
            ajax: {
                type: 'GET',
                dataType: 'json',
                url: globalPath + 'Fondeador/GetAllContacto?fondeadorId=' + $(idFondeador).val(),
                dataSrc: function (data) {

                    return data.data;
                }
            },
            columns: [
                { data: 'cNombre', 'autoWidth': true, class: 'text-center' },
                { data: 'cApellidoPaterno', 'autoWidth': true, class: 'text-center' },
                { data: 'cApellidoMaterno', 'autoWidth': true, class: 'text-center' },
                { data: 'nombreTipoContacto', 'autoWidth': true, class: 'text-center' },
                { data: 'cCelular', 'autoWidth': true, class: 'text-center' },
                { data: 'cCorreo', 'autoWidth': true, class: 'text-center' },
                { data: null, 'autoWidth': true, class: 'text-center', responsivePriority: -1 }
            ],
            columnDefs: [
                {
                    targets: -1,
                    data: null,
                    visible: (tableContactoAction == 'Detalle') ? false : true,
                    orderable: false,
                    className: 'text-end',
                    render: function (data, type, row) {
                        return `<a href="javascript:;" class="btn btn-icon btn-light-dark btn-sm" data-kt-contacto-table-filter="delete_row" data-parent="` + $(idFondeador).val() + `" data-id="` + row.iIdFondeadorContacto + `"><i class="las la-trash fs-2"></i></a>`;
                    }
                }
            ]
        });
        datatableContacto.on('draw', function () {
            handleDeleteContactoForm();
        });
    }

    var handleAddRepresentanteLegalForm = function () {
        var formAdd = document.getElementById('kt_form_add_representante_legal');
        if (!formAdd) {
            return;
        }
        var addButton = document.getElementById('kt_add_representante_legal');
        var validator;
        $('#IdFondeadorCabeceraRL').val($('#IdFondeador').val());
        $('#DOIRL').val($('#hDOI').val());
        validator = FormValidation.formValidation(
            formAdd,
            {
                fields: {
                    'NombresRL': {
                        validators: {
                            notEmpty: {
                                message: 'Nombres es obligatorio'
                            }
                        }
                    },
                    'ApellidoPaternoRL': {
                        validators: {
                            notEmpty: {
                                message: 'Apellido Paterno es obligatorio'
                            }
                        }
                    },
                    'ApellidoMaternoRL': {
                        validators: {
                            notEmpty: {
                                message: 'Apellido Paterno es obligatorio'
                            }
                        }
                    },
                    'CelularRL': {
                        validators: {
                            notEmpty: {
                                message: 'Celular es obligatorio'
                            }
                        }
                    }
                    , 'TelefonoRL': {
                        validators: {
                            notEmpty: {
                                message: 'Telefono es obligatorio'
                            }
                        }
                    },
                    'CorreoRL': {
                        validators: {
                            notEmpty: {
                                message: 'Correo es obligatorio'
                            },
                            emailAddress: {
                                message: 'Ingresar un email válido',
                            }
                        }
                    },

                    'IdTipoRL': {
                        validators: {
                            notEmpty: {
                                message: 'Tipo es obligatorio'
                            }
                        }
                    },

                    'PoderLegal': {
                        validators: {
                            notEmpty: {
                                message: 'Poder Legal es obligatorio'
                            }
                        }
                    }

                },
                plugins: {
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
            validator.validate().then(function (status) {
                if (status == 'Valid') {
                    addButton.setAttribute('data-kt-indicator', 'on');
                    addButton.disabled = true;
                    setTimeout(function () {
                        var tokenVerification = $('input[name="__RequestVerificationToken"]').val();
                        var fileDocConst = $('#PoderLegal')[0];
                        var formData = new FormData();
                        formData.append('IdFondeadorCabeceraRL', $('#IdFondeadorCabeceraRL').val());
                        formData.append('Nombres', $('#NombresRL').val());
                        formData.append('ApellidoPaterno', $('#ApellidoPaternoRL').val());
                        formData.append('ApellidoMaterno', $('#ApellidoMaternoRL').val());
                        formData.append('Celular', $('#CelularRL').val());
                        formData.append('Telefono', $('#TelefonoRL').val());
                        formData.append('Correo', $('#CorreoRL').val());
                        formData.append('IdTipo', $('#IdTipoRL').val());
                        formData.append('DOIRL', $('#DOIRL').val());
                        formData.append('PoderLegal', fileDocConst.files[0]);

                        $.ajax({
                            type: 'POST',
                            dataType: 'json',
                            url: $(formAdd).attr('action'),
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
                                        text: data.message,
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
                                            initDataTableRepresentanteLegal();
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
                }
                //else {
                //    messageError('Lo sentimos, parece que se han detectado algunos errores. Vuelve a intentarlo.');
                //}
            });
        });
    }
    var handleDeleteRepresentanteLegalForm = function () {

        var tableFondeadorRepresentanteLegal = document.querySelector('#kt_fondeador_representante_legal_table');
        if (!tableFondeadorRepresentanteLegal) {
            return;
        }
        var deleteFondeoButton = tableFondeadorRepresentanteLegal.querySelectorAll('[data-kt-representante-legal-table-filter="delete_row"]');
        deleteFondeoButton.forEach(d => {
            d.addEventListener('click', function (e) {
                e.preventDefault();
                var idFondeador = $(this).data('parent');
                var idid = $(this).data('id');
                var parent = e.target.closest('tr');
                var name = parent.querySelectorAll('td')[0].innerText + ' ' + parent.querySelectorAll('td')[1].innerText;
                Swal.fire({
                    text: '¿Estás seguro de que quieres eliminar a ' + name + '?',
                    icon: 'warning',
                    showCancelButton: true,
                    buttonsStyling: false,
                    confirmButtonText: 'Sí, eliminar!',
                    cancelButtonText: 'Cancelar',
                    customClass: {
                        confirmButton: 'btn fw-bold btn-danger',
                        cancelButton: 'btn fw-bold btn-active-light-primary'
                    }
                }).then(function (result) {
                    if (result.value) {
                        var token = $('input[name="__RequestVerificationToken"]').val();
                        $.ajax({
                            type: 'POST',
                            dataType: 'json',
                            url: globalPath + 'Fondeador/EliminarRepresentanteLegal',
                            data: {
                                fondeadorRepresentanteLegalId: idid
                            },
                            headers: {
                                'RequestVerificationToken': token
                            },
                            success: function (data) {
                                if (data) {
                                    Swal.fire({
                                        text: 'Eliminaste correctamente a ' + name + '.',
                                        icon: 'success',
                                        buttonsStyling: false,
                                        confirmButtonText: 'Listo',
                                        customClass: {
                                            confirmButton: 'btn fw-bold btn-primary',
                                        }
                                    }).then(function () {
                                        datatableRepresentanteLegal.row($(parent)).remove().draw();
                                    });
                                } else {
                                    messageError(name + ' no fue eliminado.');
                                }
                            },
                            error: function (jqXHR, textStatus, errorThrown) {
                                messageError(errorThrown);
                            }
                        });
                    } else if (result.dismiss === 'cancel') {
                        messageError(name + ' no fue eliminado.');
                    }
                });
            });
        });
    }
    var initDataTableRepresentanteLegal = function () {

        var idFondeador = document.getElementById('IdFondeador');
        if (!idFondeador) {
            return;
        }
        var tableRepresentanteLegal = document.getElementById('kt_fondeador_representante_legal_table');
        if (!tableRepresentanteLegal) {
            return;
        }
        var tableRepresentanteLegalAction = $('#Action').val();
        $(tableRepresentanteLegal).DataTable({ ordering: false }).clear().destroy();
        $.fn.dataTable.ext.errMode = 'none';
        datatableRepresentanteLegal = $(tableRepresentanteLegal).DataTable({
            ordering: false,
            ajax: {
                type: 'GET',
                dataType: 'json',
                url: globalPath + 'Fondeador/GetAllRepresentanteLegal?fondeadorId=' + $(idFondeador).val(),
                dataSrc: function (data) {

                    return data.data;
                }
            },
            columns: [
                { data: 'cNombre', 'autoWidth': true, class: 'text-center' },
                { data: 'cApellidoPaterno', 'autoWidth': true, class: 'text-center' },
                { data: 'cApellidoMaterno', 'autoWidth': true, class: 'text-center' },
                { data: 'cCelular', 'autoWidth': true, class: 'text-center' },
                { data: 'cCelular', 'autoWidth': true, class: 'text-center' },
                { data: 'cCorreo', 'autoWidth': true, class: 'text-center' },
                { data: 'nombreTipoRegistro', 'autoWidth': true, class: 'text-center' },
                { data: 'cNombreDocumento', 'autoWidth': true, class: 'text-center' },
                { data: null, 'autoWidth': true, class: 'text-center', responsivePriority: -1 }
            ],
            columnDefs: [
                {
                    targets: -1,
                    data: null,
                    visible: (tableRepresentanteLegalAction == 'Detalle') ? false : true,
                    orderable: false,
                    className: 'text-end',
                    render: function (data, type, row) {

                        var buttonDownload = ((data.cNombreDocumento == null || data.cNombreDocumento == '') ? `` : `<a href="javascript:;" class="btn btn-icon btn-sm btn-outline btn-outline-solid btn-outline-default me-2" data-kt-representante-legal-table-filter="download_file" onclick="Fondeador.Download(this)" data-path="` + data.cRutaPoderLegal + `" data-filename="` + data.cNombreDocumento + `" title="` + data.cNombreDocumento + `"><i class="las la-download fs-2"></i></a>`);
                        var buttonDelete = ((tableRepresentanteLegalAction == 'Detalle') ? `` : `<a href="javascript:;" class="btn btn-icon btn-light-dark btn-sm" data-kt-representante-legal-table-filter="delete_row" data-parent="` + $(idFondeador).val() + `" data-id="` + row.iIdFondeadorRepLegal + `"><i class="las la-trash fs-2"></i></a>`);
                        return buttonDownload + buttonDelete;
                        //return `<a href="javascript:;" class="btn btn-icon btn-light-dark btn-sm" data-kt-representante-legal-table-filter="delete_row" data-parent="` + $(idFondeador).val() + `" data-id="` + row.iIdFondeadorContacto + `"><i class="las la-trash fs-2"></i></a>`;
                    }
                }
            ]
        });
        datatableRepresentanteLegal.on('draw', function () {
            handleDeleteRepresentanteLegalForm();
        });
    }

    var handleUbigeo = function (pais, tipo, codigo) {
        var _tipo = tipo + 1;
        var _codigo = $(codigo).val();
        for (var elem = _tipo; elem <= countUbigeo; elem++) {
            $('#' + elem).empty().append('<option value="">Seleccionar</option>');
        }
        $.getJSON(globalPath + 'Fondeador/ListarUbigeos', { pais: pais, tipo: _tipo, codigo: _codigo }).done(function (data) {
            if (data.length > 0) {
                $.each(data, function (key, value) {
                    $('#' + _tipo).append($('<option value="' + value.cCodigo + '">' + value.cDescripcion + '</option>'));
                });
            }
        });
    }

    var handleAddUbicacionForm = function () {
        var formAddUbicacion = document.getElementById('kt_form_add_ubicacion');
        if (!formAddUbicacion) {
            return;
        }
        var addButton = document.getElementById('kt_add_ubicacion');
        var validator;
        $('#IdFondeadorCabeceraUbicacion').val($('#IdFondeador').val());
        validator = FormValidation.formValidation(
            formAddUbicacion,
            {
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
            validator.validate().then(function (status) {
                if (status == 'Valid') {
                    addButton.setAttribute('data-kt-indicator', 'on');
                    addButton.disabled = true;
                    setTimeout(function () {
                        var _ubigeo = {};
                        $('select.ubigeo-list').each(function () {
                            _ubigeo[$(this).attr('name')] = $(this).val();
                        });
                        var json_ubigeo = JSON.stringify(_ubigeo);
                        var ubigeoData = { Ubigeo: json_ubigeo };
                        var formData = $(formAddUbicacion).serializeObject();
                        $.extend(formData, ubigeoData);
                        $.ajax({
                            type: 'POST',
                            dataType: 'json',
                            url: $(formAddUbicacion).attr('action'),
                            data: formData,
                            xhrFields: {
                                withCredentials: true
                            },
                            success: function (data) {
                                if (data.succeeded) {
                                    Swal.fire({
                                        text: data.message,
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
                                            initDataTableUbicacion();
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
                }
                //else {
                //    messageError('Lo sentimos, parece que se han detectado algunos errores. Vuelve a intentarlo.');
                //}
            });
        });
    }
    var handleDeleteUbicacionForm = function () {
        var tableUbicaciones = document.querySelector('#kt_ubicaciones_table');
        if (!tableUbicaciones) {
            return;
        }
        var deleteUbicacionButton = tableUbicaciones.querySelectorAll('[data-kt-ubicacion-table-filter="delete_row"]');
        deleteUbicacionButton.forEach(d => {
            d.addEventListener('click', function (e) {
                e.preventDefault();
                var idid = $(this).data('id');
                var parent = e.target.closest('tr');
                Swal.fire({
                    text: '¿Estás seguro de que quieres eliminar la ubicación?',
                    icon: 'warning',
                    showCancelButton: true,
                    buttonsStyling: false,
                    confirmButtonText: 'Sí, eliminar!',
                    cancelButtonText: 'Cancelar',
                    customClass: {
                        confirmButton: 'btn fw-bold btn-danger',
                        cancelButton: 'btn fw-bold btn-active-light-primary'
                    }
                }).then(function (result) {
                    if (result.value) {
                        var token = $('input[name="__RequestVerificationToken"]').val();
                        $.ajax({
                            type: 'POST',
                            dataType: 'json',
                            url: globalPath + 'Fondeador/EliminarUbicacion',
                            data: {
                                fondeadorUbicacionId: idid
                            },
                            headers: {
                                'RequestVerificationToken': token
                            },
                            success: function (data) {
                                if (data) {
                                    Swal.fire({
                                        text: 'Eliminaste correctamente la ubicación.',
                                        icon: 'success',
                                        buttonsStyling: false,
                                        confirmButtonText: 'Listo',
                                        customClass: {
                                            confirmButton: 'btn fw-bold btn-primary',
                                        }
                                    }).then(function () {
                                        datatableUbicacion.row($(parent)).remove().draw();
                                    });
                                } else {
                                    messageError('La ubicación no fue eliminada.');
                                }
                            },
                            error: function (jqXHR, textStatus, errorThrown) {
                                messageError(errorThrown);
                            }
                        });
                    } else if (result.dismiss === 'cancel') {
                        messageError('La ubicación no fue eliminada.');
                    }
                });
            });
        });
    }
    var initDataTableUbicacion = function () {
        var idFondeador = document.getElementById('IdFondeador');
        if (!idFondeador) {
            return;
        }
        var tableUbicaciones = document.getElementById('kt_ubicaciones_table');
        if (!tableUbicaciones) {
            return;
        }
        var tableUbicacionAction = $('#Action').val();
        $(tableUbicaciones).DataTable({ ordering: false }).clear().destroy();
        $.fn.dataTable.ext.errMode = 'none';
        datatableUbicacion = $(tableUbicaciones).DataTable({
            ordering: false,
            ajax: {
                type: 'GET',
                dataType: 'json',
                url: globalPath + 'Fondeador/GetAllUbicacion?fondeadorId=' + $(idFondeador).val(),
                dataSrc: function (data) {
                    return data.data;
                }
            },
            columns: [
                { data: 'cFormatoUbigeo', 'autoWidth': true, class: 'text-left' },
                { data: 'cDireccion', 'autoWidth': true, class: 'text-left' },
                { data: null, 'autoWidth': true, class: 'text-center', responsivePriority: -1 }
            ],
            columnDefs: [
                {
                    targets: 0,
                    render: function (data) {
                        var ubigeo = '';
                        $.each($.parseJSON(data), function (key, value) {
                            ubigeo += `<strong>` + key + `:</strong> ` + value + `<br>`;
                        });
                        return ubigeo;
                    }
                },
                {
                    targets: -1,
                    data: null,
                    visible: (tableUbicacionAction == 'Detalle') ? false : true,
                    orderable: false,
                    className: 'text-end',
                    render: function (data, type, row) {
                        return `<a href="javascript:;" class="btn btn-icon btn-light-dark btn-sm" data-kt-ubicacion-table-filter="delete_row" data-parent="` + $(idFondeador).val() + `" data-id="` + data.iIdFondeadorUbicacion + `"><i class="las la-trash fs-2"></i></a>`;
                    }
                }
            ]
        });
        datatableUbicacion.on('draw', function () {
            handleDeleteUbicacionForm();
        });
    }


    var handleAddDocumentoForm = function () {
        var formAddDocumento = document.getElementById('kt_form_add_documento');
        if (!formAddDocumento) {
            return;
        }
        var addButton = document.getElementById('kt_add_documento');
        var validator;
        $('#IdFondeadorCabeceraDocumento').val($('#IdFondeador').val());
        $('#DOIDOCUMENTO').val($('#hDOI').val());
        validator = FormValidation.formValidation(
            formAddDocumento,
            {
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
            validator.validate().then(function (status) {
                if (status == 'Valid') {
                    addButton.setAttribute('data-kt-indicator', 'on');
                    addButton.disabled = true;
                    setTimeout(function () {
                        var _ubigeo = {};
                        $('select.ubigeo-list').each(function () {
                            _ubigeo[$(this).attr('name')] = $(this).val();
                        });
                        var json_ubigeo = JSON.stringify(_ubigeo);
                        var ubigeoData = { Ubigeo: json_ubigeo };
                        //var formData = $(formAddDocumento).serializeObject();
                        var dataTipoDocumento = $('#TipoDocumento').select2('data');

                        var tokenVerification = $('input[name="__RequestVerificationToken"]').val();
                        var fileDocConst = $('#fileDocumento')[0];
                        var formData = new FormData();
                        formData.append('IdFondeadorCabeceraDocumento', $('#IdFondeadorCabeceraDocumento').val());
                        formData.append('TipoDocumento', $('#TipoDocumento').val());
                        formData.append('TipoDocumentoDesc', dataTipoDocumento[0].text);
                        formData.append('DOIDOCUMENTO', $('#DOIDOCUMENTO').val());
                        formData.append('fileDocumento', fileDocConst.files[0]);


                        $.extend(formData, ubigeoData);
                        $.ajax({
                            type: 'POST',
                            dataType: 'json',
                            url: $(formAddDocumento).attr('action'),
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
                                        text: data.message,
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
                                            initDataTableDocumento();
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
                    messageError('Lo sentimos, parece que se han detectado algunos errores. Vuelve a intentarlo.');
                }
            });
        });
    }
    var handleDeleteDocumentoForm = function () {
        var tableDocumento = document.querySelector('#kt_documento_table');
        if (!tableDocumento) {
            return;
        }
        var deleteUbicacionButton = tableDocumento.querySelectorAll('[data-kt-documento-table-filter="delete_row"]');
        deleteUbicacionButton.forEach(d => {
            d.addEventListener('click', function (e) {
                e.preventDefault();
                var idid = $(this).data('id');
                var parent = e.target.closest('tr');
                Swal.fire({
                    text: '¿Estás seguro de que quieres eliminar el documento?',
                    icon: 'warning',
                    showCancelButton: true,
                    buttonsStyling: false,
                    confirmButtonText: 'Sí, eliminar!',
                    cancelButtonText: 'Cancelar',
                    customClass: {
                        confirmButton: 'btn fw-bold btn-danger',
                        cancelButton: 'btn fw-bold btn-active-light-primary'
                    }
                }).then(function (result) {
                    if (result.value) {
                        var token = $('input[name="__RequestVerificationToken"]').val();
                        $.ajax({
                            type: 'POST',
                            dataType: 'json',
                            url: globalPath + 'Fondeador/EliminarDocumento',
                            data: {
                                fondeadorDocumentoId: idid
                            },
                            headers: {
                                'RequestVerificationToken': token
                            },
                            success: function (data) {
                                if (data) {
                                    Swal.fire({
                                        text: 'Eliminaste correctamente el documento.',
                                        icon: 'success',
                                        buttonsStyling: false,
                                        confirmButtonText: 'Listo',
                                        customClass: {
                                            confirmButton: 'btn fw-bold btn-primary',
                                        }
                                    }).then(function () {
                                        datatableDocumento.row($(parent)).remove().draw();
                                    });
                                } else {
                                    messageError('El documento no fue eliminada.');
                                }
                            },
                            error: function (jqXHR, textStatus, errorThrown) {
                                messageError(errorThrown);
                            }
                        });
                    } else if (result.dismiss === 'cancel') {
                        messageError('El documento no fue eliminada.');
                    }
                });
            });
        });
    }
    var initDataTableDocumento = function () {
        var idFondeador = document.getElementById('IdFondeador');
        if (!idFondeador) {
            return;
        }
        var tableDocumento = document.getElementById('kt_documento_table');
        if (!tableDocumento) {
            return;
        }
        var tableDocumentoAction = $('#Action').val();
        $(tableDocumento).DataTable({ ordering: false }).clear().destroy();
        $.fn.dataTable.ext.errMode = 'none';
        datatableDocumento = $(tableDocumento).DataTable({
            ordering: false,
            ajax: {
                type: 'GET',
                dataType: 'json',
                url: globalPath + 'Fondeador/GetAllDocumento?fondeadorId=' + $(idFondeador).val(),
                dataSrc: function (data) {
                    return data.data;
                }
            },
            columns: [
                { data: 'nombreTipoDocumento', 'autoWidth': true, class: 'text-left' },
                { data: 'cNombreDocumento', 'autoWidth': true, class: 'text-left' },
                //{ data: 'dFechaCreacion', 'autoWidth': true, class: 'text-left' },
                {
                    data: 'dFechaCreacion', 'autoWidth': true, class: 'text-center', render: function (value) {
                        return moment(value).format('DD/MM/YYYY');
                    }
                },
                { data: null, 'autoWidth': true, class: 'text-center', responsivePriority: -1 }
            ],
            columnDefs: [
                {
                    targets: -1,
                    data: null,
                    visible: (tableDocumentoAction == 'Detalle') ? false : true,
                    orderable: false,
                    className: 'text-end',
                    render: function (data, type, row) {
                        var buttonDownload = ((data.cNombreDocumento == null || data.cNombreDocumento == '') ? `` : `<a href="javascript:;" class="btn btn-icon btn-sm btn-outline btn-outline-solid btn-outline-default me-2 oculto-acci p-des" data-kt-documento-table-filter="download_file" onclick="Fondeador.Download(this)" data-path="` + data.cRutaDocumento + `"data-filename="` + data.cNombreDocumento + `" title="` + data.cNombreDocumento + `"><i class="las la-download fs-2"></i></a>`);
                        var buttonDelete = ((tableDocumentoAction == 'Detalle') ? `` : `<a href="javascript:;" class="btn btn-icon btn-light-dark btn-sm oculto-acci p-eli" data-kt-documento-table-filter="delete_row" data-parent="` + $(idFondeador).val() + `" data-id="` + data.nIdFondeadorDocumento + `"><i class="las la-trash fs-2"></i></a>`);
                        return buttonDownload + buttonDelete;
                    }
                }
            ]
        });
        datatableDocumento.on('draw', function () {
            handleDeleteDocumentoForm();
            Common.init();
        });
    }

    var handleDownloadFile = function (fileName, ruta) {
        window.open(globalPath + 'Fondeador/DownloadFile?filename=' + encodeURIComponent(fileName) + '&ruta=' + ruta, '_blank');
    }

    var handleDisabledField = function () {
        var jqcboproducto = $('#IdProducto');


        jqcboproducto.on("change", function (e) {
            var producto = parseInt($('#IdProducto').val());
            
            console.log(producto)
            if (producto == 2) { //Cobranza Libre
                $("#divDF").show();
            } else {
                $("#divDF").hide();
            }

            //var CapitalFinanciado = document.getElementById('CapitalFinanciado');
            //var MetodoCalculo = document.getElementById('IdMetodoCalculo');
            //var searchDiasPagoButton = document.getElementById('DiasPago');
            //var RetencionInicial = document.getElementById('IdRetencionInicial');
            //var CalculoInteres = document.getElementById('IdCalculoInteres');

            //var valor = producto != 3


            //CapitalFinanciado.disabled = valor;
            //MetodoCalculo.disabled = valor;
            //searchDiasPagoButton.disabled = valor;
            //RetencionInicial.disabled = valor;
            //CalculoInteres.disabled = valor;
        });



    }



    var handleAddPrestamoForm = function () {
        var formAddPrestamo = document.getElementById('kt_form_add_prestamo');
        if (!formAddPrestamo) {
            return;
        }
        var addButton = document.getElementById('kt_add_prestamo');
        var validator;
        $('#IdFondeadorCabeceraPrestamo').val($('#IdFondeador').val());

        validator = FormValidation.formValidation(
            formAddPrestamo,
            {
                fields: {
                    'IdProductoPrestamo': {
                        validators: {
                            notEmpty: {
                                message: 'Producto es obligatorio'
                            }
                        }
                    },
                    'IdMonedaPrestamo': {
                        validators: {
                            notEmpty: {
                                message: 'Moneda es obligatorio'
                            }
                        }
                    },
                    'IdModalidadPrestamo': {
                        validators: {
                            notEmpty: {
                                message: 'Modalidad es obligatorio'
                            }
                        }
                    },
                    'nCapitalPrestamo': {
                        validators: {
                            notEmpty: {
                                message: 'Capital es obligatorio'
                            }
                        }
                    },
                    'nInteresPrestamo': {
                        validators: {
                            notEmpty: {
                                message: 'Interes es obligatorio'
                            }
                        }
                    }
                },
                plugins: {
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
            validator.validate().then(function (status) {
                if (status == 'Valid') {
                    addButton.setAttribute('data-kt-prestamo', 'on');
                    addButton.disabled = true;
                    setTimeout(function () {
                        $.ajax({
                            type: 'POST',
                            dataType: 'json',
                            url: $(formAddPrestamo).attr('action'),
                            data: $(formAddPrestamo).serializeObject(),
                            xhrFields: {
                                withCredentials: true
                            },
                            success: function (data) {
                                if (data.succeeded) {
                                    Swal.fire({
                                        text: data.message,
                                        icon: 'success',
                                        buttonsStyling: false,
                                        confirmButtonText: 'Listo',
                                        customClass: {
                                            confirmButton: 'btn btn-primary'
                                        }
                                    }).then(function (result) {
                                        if (result.isConfirmed) {
                                            $(addButton).closest('form').find('input[type=text],input[type=checkbox],textarea').val('');
                                            $(addButton).closest('form').find('select').val('').trigger('change');
                                            $(addButton).closest('form').find('input[type="checkbox"]').prop('checked', false);
                                            addButton.removeAttribute('data-kt-prestamo');
                                            //$(this).closest('form').find('input[type=text], textarea').val('');
                                            //$(this).closest('form').find('select').val('').trigger('change');
                                            addButton.disabled = false;
                                            initDataTablePrestamo();
                                        }
                                    });
                                } else {
                                    addButton.removeAttribute('data-kt-prestamo');
                                    addButton.disabled = false;
                                    messageError(data.message);
                                }
                            },
                            error: function (jqXHR, textStatus, errorThrown) {
                                addButton.removeAttribute('data-kt-prestamo');
                                addButton.disabled = false;
                                messageError(errorThrown);
                            }
                        });
                    }, 2000);
                }
            });
        });
    }

    var initDataTablePrestamo = function () {
        var idFondeador = document.getElementById('IdFondeador');
        if (!idFondeador) {
            return;
        }
        var tableFondeoPrestamo = document.getElementById('kt_fondeador_prestamo_table');
        if (!tableFondeoPrestamo) {
            return;
        }
        var tableFondeoPrestamoAction = $('#Action').val();
        $(tableFondeoPrestamo).DataTable({ ordering: false }).clear().destroy();
        $.fn.dataTable.ext.errMode = 'none';
        datatableFondeoPrestamo = $(tableFondeoPrestamo).DataTable({
            ordering: false,
            ajax: {
                type: 'GET',
                dataType: 'json',
                url: globalPath + 'Fondeador/GetAllFondeoPrestamo?fondeadorId=' + $(idFondeador).val(),
                dataSrc: function (data) {

                    return data.data;
                }
            },
            columns: [
                { data: 'cProducto', 'autoWidth': true, class: 'text-center' },
                { data: 'cMoneda', 'autoWidth': true, class: 'text-center' },
                { data: 'cModalidad', 'autoWidth': true, class: 'text-center' },
                { data: 'nPorcentajeCapital', 'autoWidth': true, class: 'text-center' },
                { data: 'nPorcentajeInteres', 'autoWidth': true, class: 'text-center' },
                { data: 'nPorcentajeInteresPG', 'autoWidth': true, class: 'text-center' },
                { data: null, 'autoWidth': true, class: 'text-center', responsivePriority: -1 }
            ],
            columnDefs: [
                {
                    targets: -1,
                    data: null,
                    visible: (tableFondeoPrestamoAction == 'Detalle') ? false : true,
                    orderable: false,
                    className: 'text-end',
                    render: function (data, type, row) {
                        return `<a href="javascript:;" class="btn btn-icon btn-light-dark btn-sm" data-kt-prestamo-table-filter="delete_row" data-parent="` + $(idFondeador).val() + `" data-id="` + row.iIdFondeadorPrestamo + `"><i class="las la-trash fs-2"></i></a>`;
                    }
                }
            ]
        });
        datatableFondeoPrestamo.on('draw', function () {
            handleDeletePrestamoForm();
        });
    }



    var handleDeletePrestamoForm = function () {
        var tableFondeadorFondeoPrestamo = document.querySelector('#kt_fondeador_prestamo_table');
        if (!tableFondeadorFondeoPrestamo) {
            return;
        }
        var deletePrestamoButton = tableFondeadorFondeoPrestamo.querySelectorAll('[data-kt-prestamo-table-filter="delete_row"]');
        deletePrestamoButton.forEach(d => {
            d.addEventListener('click', function (e) {
                e.preventDefault();
                var idFondeador = $(this).data('parent');
                var idFondeo = $(this).data('id');
                var parent = e.target.closest('tr');
                var name = parent.querySelectorAll('td')[0].innerText + ' ' + parent.querySelectorAll('td')[1].innerText;
                Swal.fire({
                    text: '¿Estás seguro de que quieres eliminar a ' + name + '?',
                    icon: 'warning',
                    showCancelButton: true,
                    buttonsStyling: false,
                    confirmButtonText: 'Sí, eliminar!',
                    cancelButtonText: 'Cancelar',
                    customClass: {
                        confirmButton: 'btn fw-bold btn-danger',
                        cancelButton: 'btn fw-bold btn-active-light-primary'
                    }
                }).then(function (result) {
                    if (result.value) {
                        var token = $('input[name="__RequestVerificationToken"]').val();
                        $.ajax({
                            type: 'POST',
                            dataType: 'json',
                            url: globalPath + 'Fondeador/EliminarPrestamo',
                            data: {
                                FondeoPrestamoId: idFondeo
                            },
                            headers: {
                                'RequestVerificationToken': token
                            },
                            success: function (data) {
                                if (data) {
                                    Swal.fire({
                                        text: 'Eliminaste correctamente a ' + name + '.',
                                        icon: 'success',
                                        buttonsStyling: false,
                                        confirmButtonText: 'Listo',
                                        customClass: {
                                            confirmButton: 'btn fw-bold btn-primary',
                                        }
                                    }).then(function () {
                                        datatableFondeoPrestamo.row($(parent)).remove().draw();
                                    });
                                } else {
                                    messageError(name + ' no fue eliminado.');
                                }
                            },
                            error: function (jqXHR, textStatus, errorThrown) {
                                messageError(errorThrown);
                            }
                        });
                    } else if (result.dismiss === 'cancel') {
                        messageError(name + ' no fue eliminado.');
                    }
                });
            });
        });
    }


    $('#bAplicanCapitalPrestamo').on('change', function () {
        if ($(this).is(':checked')) {
            console.log("Checkbox " + $(this).prop("id") + " (" + $(this).val() + ") => Seleccionado");
            $('#nValorSeleccionadoCapital').val(1);
            /*$('#nCapitalPrestamo').removeAttr('disabled');*/
        } else {
            console.log("Checkbox " + $(this).prop("id") + " (" + $(this).val() + ") => Deseleccionado");
            $('#nValorSeleccionadoCapital').val(0);
            //$('#nCapitalPrestamo').attr('disabled', 'disabled');
            //$('#nCapitalPrestamo').val('');
        }
    });

    $('#bAplicanInteresPrestamo').on('change', function () {
        if ($(this).is(':checked')) {
            console.log("Checkbox " + $(this).prop("id") + " (" + $(this).val() + ") => Seleccionado");
            $('#nValorSeleccionadoInteres').val(1);
            /*  $('#nInteresPrestamo').removeAttr('disabled');*/
        } else {
            console.log("Checkbox " + $(this).prop("id") + " (" + $(this).val() + ") => Deseleccionado");
            $('#nValorSeleccionadoInteres').val(0);
            //$('#nInteresPrestamo').attr('disabled', 'disabled');
            //$('#nInteresPrestamo').val('');
        }
    });

    $('#bAplicaInteresPeriodoGraciaPrestamo').on('change', function () {
        if ($(this).is(':checked')) {
            console.log("Checkbox " + $(this).prop("id") + " (" + $(this).val() + ") => Seleccionado");
            $('#nValorSeleccionadoPeriodoGracia').val(1);
            /* $('#nInteresPeriodoGraciaPrestamo').removeAttr('disabled');*/
        } else {
            console.log("Checkbox " + $(this).prop("id") + " (" + $(this).val() + ") => Deseleccionado");
            $('#nValorSeleccionadoPeriodoGracia').val(0);
            //$('#nInteresPeriodoGraciaPrestamo').attr('disabled', 'disabled');
            //$('#nInteresPeriodoGraciaPrestamo').val('');
        }
    });

    return {
        init: function () {

            handleFilterTable();
            initDatatable();
            handleRegistroFondeador();

            //handleUbigeo();

            handleAddFondeForm();
            handleDeleteFondeoForm();
            initDataTableFondeo();

            handleAddCavaliFactoringForm();
            handleDeleteCavaliFactoringForm();
            initDataTableCavaliFactoring();

            handleAddCuentaBancariaForm();
            handleDeleteCuentaBancariaForm();
            initDataTableCuentaBancaria();

            handleAddContactoForm();
            handleDeleteContactoForm();
            initDataTableContacto();

            handleAddRepresentanteLegalForm();
            handleDeleteRepresentanteLegalForm();
            initDataTableRepresentanteLegal();

            handleAddUbicacionForm();
            handleDeleteUbicacionForm();
            initDataTableUbicacion();

            handleAddDocumentoForm();
            handleDeleteDocumentoForm();
            initDataTableDocumento();

            handleDisabledField();

            handleAddPrestamoForm();
            initDataTablePrestamo();
            handleDeletePrestamoForm();

            var producto = parseInt($('#IdProducto').val());

            //console.log(producto)
            if (producto == 2) { //Cobranza Libre
                $("#divDF").show();
            } else {
                $("#divDF").hide();

            }
        },
        getUbigeo: function (pais, tipo, codigo) {
            //handleUbigeo(pais, tipo, codigo);
        },
        getDownloadFile: function (fileName, ruta) {
            handleDownloadFile(fileName, ruta);
        },
        Download: function (e) {
            var filename = $(e)[0].dataset.filename;
            var path = $(e)[0].dataset.path;
            Fondeador.getDownloadFile(filename, path);

        }
    }
}();

KTUtil.onDOMContentLoaded(function () {
    Fondeador.init();
});