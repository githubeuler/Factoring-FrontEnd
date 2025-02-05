'use strict';
var globalPath = $('base').attr('href');
var fondeador = '4';
var montoFacturaTotal = 0;
var RegistroOperacion = function () {
    var datatable;
    var datatableFacturas;
    var datatableCavali;
    var datatableDocumentoSolicitud;
    var handleFilterTable = function () {
        var searchButton = document.getElementById('kt_search_button');
        if (!searchButton) {
            return;
        }


        $('#FechaCreacion').flatpickr({
            dateFormat: 'd/m/Y',
            defaultDate: 'today'
        });

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
            $("#Estado").val('').trigger('change');
            initDatatable();
        });
    }
    var initToggleToolbar = function () {
        //var container = document.querySelector('#kt_pago_table');
        //var container2 = document.querySelector('n-pago');
        //$('#dFechaPago').flatpickr({
        //    dateFormat: 'd/m/Y',
        //    defaultDate: 'today'

        //});
    }
    var initDatatable = function () {
        var table = $('#kt_operaciones_table');
        if (!table) {
            return;
        }
        // Verifica si el DataTable ya está inicializado
        if ($.fn.DataTable.isDataTable(table)) {
            table.DataTable().destroy();
        }

        //$(table).DataTable({ ordering: false }).clear().destroy();
        $.fn.dataTable.ext.errMode = 'none';
        datatable = $(table).DataTable({
            searchDelay: 500,
            processing: true,
            serverSide: true,
            stateSave: false,
            ordering: false,
            ajax: {
                url: globalPath + 'Operacion/GetOperacionAllList',
                type: 'POST',
                datatype: 'json',
                data: $('#kt_search_form').serializeObject()
            },
            columns: [
                { data: 'nIdOperaciones', name: 'nIdOperaciones', 'autoWidth': true, class: 'text-center' },
                { data: 'nNroOperacion', 'autoWidth': true, class: 'text-left' },
                { data: 'cRazonSocialGirador', 'autoWidth': true, class: 'text-left' },
                { data: 'cRazonSocialAdquiriente', 'autoWidth': true, class: 'text-left' },
                {
                    data: 'dFechaCreacion', 'autoWidth': true, class: 'text-center', render: function (value) {
                        return moment(value).format('DD/MM/YYYY');
                    }
                },
                { data: 'nombreEstado', 'autoWidth': true, class: 'text-center' },
                { data: null, 'autoWidth': true, class: 'text-center', responsivePriority: -1 }
            ],
            columnDefs: [
                {
                    targets: 0,
                    render: function (data) {
                        return `<div class="form-check form-check-sm form-check-custom form-check-solid"><input class="form-check-input checkbox-main" type="checkbox" value="${data}" /></div>`;
                    }
                },
                {
                    targets: -1,
                    data: null,
                    orderable: false,
                    className: 'text-end',
                    render: function (data, type, row) {
                        var buttonAction = ``;
                        if (data.nEstado == '0') {
                            buttonAction += ``;
                        } // else if (data.nEditar == '10' || data.nEstado == '5')
                        else if (data.nEditar > 0) {
                            buttonAction += `<a href="javascript:;" class="btn btn-icon btn-light-dark btn-sm open-modal p-eva" data-bs-toggle="modal" data-bs-target="#kt_modal_evaluacion_operacion" data-n-operacion=${data.nIdOperaciones} title="Evaluar"><i class="las la-check-square fs-2"></i></a>
                                
                                <a href="${globalPath}Operacion/Detalle?operacionId=${data.nIdOperaciones}" class="btn btn-sm btn-icon btn-light btn-active-light-primary detail-row p-con"><i class="las la-search fs-2"></i></a> 
                                <button data-delete-table="delete_row" data-row= ${data.nIdOperaciones}  class="btn btn-sm btn-icon btn-light btn-active-light-primary edit-row me-2 p-eli"><i class="las la-ban fs-2"></i></button> 
                                  <a href="javascript:;" class="btn btn-icon btn-light-dark btn-sm open-modal p-eva" data-bs-toggle="modal" data-bs-target="#kt_modal_calcular_operacion" data-n-operacion=${data.nIdOperaciones} data-n-nroperacion=${data.nNroOperacion} title="Calcular"><i class="las la-calculator fs-2"></i></a>
                                `;
                        }

                        else {

                            /*var _button = `<a href="${globalPath}VentaCartera/Editar?prestamoId=${data.iIdPrestamoVentaCartera}" class="btn btn-sm btn-icon btn-light btn-active-light-primary edit-row me-2"><i class="las la-pen fs-2"></i></a> <a href="javascript:;" class="btn btn-icon btn-light-dark btn-sm p-eva open-modal" data-bs-toggle="modal" data-bs-target="#kt_modal_pago" data-n-pago="1" title="Evaluar"><i class="las la-check-square fs-2"></i></a>`*/
                            buttonAction += `<a href="${globalPath}Operacion/Registro?operacionId=${data.nIdOperaciones}" class="btn btn-sm btn-icon btn-light btn-active-light-primary edit-row me-2 p-act" title="Editar"><i class="las la-pen fs-2"></i></a> 
                                
                                <a href="javascript:;" class="btn btn-icon btn-light-dark btn-sm open-modal p-eva" data-bs-toggle="modal" data-bs-target="#kt_modal_evaluacion_operacion" data-n-operacion=${data.nIdOperaciones} title="Evaluar"><i class="las la-check-square fs-2"></i></a>
                                
                                <a href="${globalPath}Operacion/Detalle?operacionId=${data.nIdOperaciones}" class="btn btn-sm btn-icon btn-light btn-active-light-primary detail-row p-con"><i class="las la-search fs-2"></i></a> 
                                <button data-delete-table="delete_row" data-row= ${data.nIdOperaciones}  class="btn btn-sm btn-icon btn-light btn-active-light-primary edit-row me-2 p-eli"><i class="las la-ban fs-2"></i></button>
                                  <a href="javascript:;" class="btn btn-icon btn-light-dark btn-sm open-modal p-eva" data-bs-toggle="modal" data-bs-target="#kt_modal_calcular_operacion" data-n-operacion=${data.nIdOperaciones} data-n-nroperacion=${data.nNroOperacion} title="Calcular"><i class="las la-calculator fs-2"></i></a>
                                `;
                        }

                        return buttonAction;
                        //if (data.nEstado == '1') {
                        //    buttonAction += `<div class="menu-item px-3"><a href="javascript:;" data-bs-toggle="modal" data-bs-target="#kt_facturas_carga_masiva" data-id="` + data.nIdOperaciones + `" class="menu-link px-3 open-masivo-factura p-car">Cargar Facturas</a></div>`;
                        //}
                        //return `<a href="javascript:;" class="btn btn-outline btn-outline-solid btn-outline-primary btn-active-light-primary btn-icon-primary btn-sm" data-kt-menu-trigger="click" data-kt-menu-placement="bottom-end" data-kt-menu-flip="top-end">
                        //    Acciones
                        //    <span class="svg-icon svg-icon-5 m-0">
                        //        <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1">
                        //            <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">
                        //                <polygon points="0 0 24 0 24 24 0 24"></polygon>
                        //                <path d="M6.70710678,15.7071068 C6.31658249,16.0976311 5.68341751,16.0976311 5.29289322,15.7071068 C4.90236893,15.3165825 4.90236893,14.6834175 5.29289322,14.2928932 L11.2928932,8.29289322 C11.6714722,7.91431428 12.2810586,7.90106866 12.6757246,8.26284586 L18.6757246,13.7628459 C19.0828436,14.1360383 19.1103465,14.7686056 18.7371541,15.1757246 C18.3639617,15.5828436 17.7313944,15.6103465 17.3242754,15.2371541 L12.0300757,10.3841378 L6.70710678,15.7071068 Z" fill="#000000" fill-rule="nonzero" transform="translate(12.000003, 11.999999) rotate(-180.000000) translate(-12.000003, -11.999999)"></path>
                        //            </g>
                        //         </svg>
                        //    </span>
                        //</a>
                        //<div class="menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-gray-600 menu-state-bg-light-primary fw-bold fs-7 w-175px py-4" data-kt-menu="true">` +
                        //    buttonAction + `
                        //    <div class="menu-item px-3 p-con p-con">
                        //        <a href="` + globalPath + `Operacion/Detalle?operacionId=` + data.nIdOperaciones + `" class="menu-link px-3">Detalle</a>
                        //    </div>
                        //</div>`;
                    }
                }
            ]
        });
        table = datatable.$;
        datatable.on('draw', function () {
            initToggleToolbar();
            toggleToolbars();
            handleDeleteOperacionForm();
            var searchButton = document.getElementById('kt_search_button');
            var searchClear = document.getElementById('kt_search_clear');
            searchButton.removeAttribute('data-kt-indicator');
            handleModalControlEvaluacion();
            handleModalControlCalculo();
            searchButton.disabled = false;

            /*   handleAnularEvaluacion2();*/

            $(searchClear).show();
            KTMenu.createInstances();
            Common.init();
        });
        $(document).on('click', '.open-masivo-factura', function () {
            var operacionId = $(this).data('id');
            $('#fileExcelFacturasMasivo').val('');
            Dropzone.forElement('#kt_dropzonejs_facturas_xml').removeAllFiles(true);
            $('.modal-body #operacionId').val(operacionId);
        });
    }
    var initToggleToolbar = function () {
        var container = document.querySelector('#kt_operaciones_table');
        var checkboxes = container.querySelectorAll('[type="checkbox"]');
        /*var deleteSelected = document.querySelector('[data-kt-operaciones-table-select="delete_selected"]');*/
        checkboxes.forEach(c => {
            c.addEventListener('click', function () {
                setTimeout(function () {
                    toggleToolbars();
                }, 50);
            });
        });
        var searchButton = document.getElementById('kt_search_button');
        /*  var searchClear = document.getElementById('kt_search_clear');*/
        searchButton.removeAttribute('data-kt-indicator');
        searchButton.disabled = false;
        //deleteSelected.addEventListener('click', function () {
        //    Swal.fire({
        //        text: '¿Está seguro de que desea eliminar los registros seleccionados?',
        //        icon: 'warning',
        //        showCancelButton: true,
        //        buttonsStyling: false,
        //        showLoaderOnConfirm: true,
        //        confirmButtonText: 'Eliminar',
        //        cancelButtonText: 'Cancelar',
        //        customClass: {
        //            confirmButton: 'btn fw-bold btn-danger',
        //            cancelButton: 'btn fw-bold btn-active-light-primary'
        //        },
        //    }).then(function (result) {
        //        if (result.value) {
        //            var arrCheckBox = $.map($('input.checkbox-main:checkbox:checked'), function (e, i) {
        //                return +e.value;
        //            });
        //            $.ajax({
        //                type: 'POST',
        //                dataType: 'json',
        //                url: globalPath + 'Operacion/DeleteOperacion',
        //                data: {
        //                    selectedOperacion: arrCheckBox
        //                },
        //                beforeSend: function () {
        //                    var timerInterval;
        //                    Swal.fire({
        //                        title: 'Espere un momento',
        //                        html: 'Se estan eliminando los registros en <b></b> milisegundos...',
        //                        timer: 2000,
        //                        timerProgressBar: true,
        //                        didOpen: () => {
        //                            Swal.showLoading()
        //                            var b = Swal.getHtmlContainer().querySelector('b')
        //                            timerInterval = setInterval(() => {
        //                                b.textContent = Swal.getTimerLeft()
        //                            }, 100)
        //                        },
        //                        willClose: () => {
        //                            clearInterval(timerInterval)
        //                        }
        //                    })
        //                },
        //                success: function (data) {
        //                    if (data.succeeded) {
        //                        Swal.fire({
        //                            text: data.message,
        //                            icon: 'success',
        //                            buttonsStyling: false,
        //                            confirmButtonText: 'Listo',
        //                            customClass: {
        //                                confirmButton: 'btn fw-bold btn-primary',
        //                            }
        //                        }).then(function () {
        //                            datatable.draw();
        //                        });
        //                        var headerCheckbox = container.querySelectorAll('[type="checkbox"]')[0];
        //                        headerCheckbox.checked = false;
        //                    }
        //                }
        //            });
        //        } else if (result.dismiss === 'cancel') {
        //            Swal.fire({
        //                text: 'Los registros seleccionados no fueron eliminados.',
        //                icon: 'error',
        //                buttonsStyling: false,
        //                confirmButtonText: 'Ok',
        //                customClass: {
        //                    confirmButton: 'btn fw-bold btn-primary',
        //                }
        //            });
        //        }
        //    });
        //});
    }





    var handleDeleteOperacionForm = function () {
        var deleteRow = document.querySelectorAll('[data-delete-table="delete_row"]');
        if (!deleteRow) {
            return;
        }
        deleteRow.forEach(d => {
            d.addEventListener('click', function (e) {
                debugger;
                e.preventDefault();
                var idOperacion = $(this).data('row');
                var parent = e.target.closest('tr');
                Swal.fire({
                    text: '¿Estás seguro de que quieres eliminar la operacón?',
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
                    debugger;
                    if (result.value) {
                        var token = $('input[name="__RequestVerificationToken"]').val();
                        $.ajax({
                            type: 'POST',
                            dataType: 'json',
                            url: globalPath + 'Operacion/DeleteOperacion',
                            data: {
                                nIdOperacion: idOperacion
                            },
                            headers: {
                                'RequestVerificationToken': token
                            },
                            success: function (data) {
                                debugger;
                                if (data.succeeded) {
                                    Swal.fire({
                                        text: 'Eliminaste correctamente la operación.',
                                        icon: 'success',
                                        buttonsStyling: false,
                                        confirmButtonText: 'Listo',
                                        customClass: {
                                            confirmButton: 'btn fw-bold btn-primary',
                                        }
                                    }).then(function () {
                                        datatable.row($(parent)).remove().draw();
                                    });
                                } else {
                                    messageError('La operación no fue eliminada (' + data.message + ')');
                                }
                            },
                            error: function (jqXHR, textStatus, errorThrown) {
                                messageError(errorThrown);
                            }
                        });
                    } else if (result.dismiss === 'cancel') {
                        messageError('La operación no fue eliminada.');
                    }
                });
            });
        });
    }






    var toggleToolbars = function () {
        var container = document.querySelector('#kt_operaciones_table');
        var toolbarBase = document.querySelector('[data-kt-operaciones-table-toolbar="base"]');
        var toolbarSelected = document.querySelector('[data-kt-operaciones-table-toolbar="selected"]');
        var selectedCount = document.querySelector('[data-kt-operaciones-table-select="selected_count"]');
        var allCheckboxes = container.querySelectorAll('tbody [type="checkbox"]');
        var checkedState = false;
        var count = 0;
        allCheckboxes.forEach(c => {
            if (c.checked) {
                checkedState = true;
                count++;
            }
        });
        if (checkedState) {
            selectedCount.innerHTML = count;
            toolbarBase.classList.add('d-none');
            toolbarSelected.classList.remove('d-none');
        } else {
            toolbarBase.classList.remove('d-none');
            /* toolbarSelected.classList.add('d-none');*/
        }
    }
    var handleRegisterForm = function (e) {
        var form = document.getElementById('kt_form_registro');
        if (!form) {
            return;
        }
        $('#IdGirador').on('change', function (e) {
            var idGirador = this.options[this.selectedIndex].value;

            $('#IdGiradorDireccion').empty().append('<option value="">Seleccionar</option>');
            $.getJSON(globalPath + 'Operacion/GetListDireccionByGirador', { idGirador: idGirador }).done(function (data) {
                if (data != null) {
                    if (data.length > 0) {
                        $.each(data, function (key, value) {
                            $('#IdGiradorDireccion').append($('<option value="' + value.nIdGiradorDireccion + '">' + value.cDireccion + '</option>'));
                        });
                    }
                }
            });

            $('#IdCategoria').empty().append('<option value="">Seleccionar</option>');
            $.getJSON(globalPath + 'Operacion/GetListCategoriaByGirador', { idGirador: idGirador }).done(function (data) {
                if (data.length > 0) {
                    $.each(data, function (key, value) {
                        $('#IdCategoria').append($('<option value="' + value.nId + '">' + value.cNombre + '</option>'));
                    });
                }
            });

        });
        $('#IdAdquiriente').on('change', function (e) {
            var idAdquiriente = this.options[this.selectedIndex].value;
            $('#IdAdquirienteDireccion').empty().append('<option value="">Seleccionar</option>');
            $.getJSON(globalPath + 'Operacion/GetListDireccionByAdquiriente', { idAdquiriente: idAdquiriente }).done(function (data) {
                if (data.length > 0) {
                    $.each(data, function (key, value) {
                        $('#IdAdquirienteDireccion').append($('<option value="' + value.nIdAdquirienteDireccion + '">' + value.cDireccion + '</option>'));
                    });
                }
            });
        });


        if ($('#IdCategoria').val() == "1" || $('#IdCategoria').val() == "3") {
            $('#box-retencion').show();
        } else {
            $('#PorcentajeRetencion').val(0);
            $('#box-retencion').hide();
        }

        $('#IdCategoria').on('change', function (e) {
            $('#InteresMoratorio').attr("disabled", "true");
            $('#InteresMoratorio').val('');
            $('#InteresMoratorio').attr('placeholder', '0.00');
            $("#InteresMoratorio").removeAttr("disabled")
        });

        var PICKER_VENCIMIENTO = flatpickr('#fechaVencimiento', {
            dateFormat: 'd/m/Y',
            defaultDate: 'today'
        });

        var PICKER_PAGO = flatpickr('#fechaPagoNegociado', {
            dateFormat: 'd/m/Y',
            defaultDate: 'today'
        });

        $('#fechaEmision').flatpickr({
            dateFormat: 'd/m/Y',
            defaultDate: 'today',
            onChange: function (dateStr, dateObj) {
                PICKER_VENCIMIENTO.set('minDate', dateObj);
                PICKER_VENCIMIENTO.setDate(dateObj);
                PICKER_PAGO.set('minDate', dateObj);
                PICKER_PAGO.setDate(dateObj);
            }
        });
        var saveButton = document.getElementById('kt_save_button');
        var evaluateButton = document.getElementById('kt_evaluate_button');
        var validator;
        validator = FormValidation.formValidation(
            form,
            {
                fields: {
                    'IdGirador': {
                        validators: {
                            notEmpty: {
                                message: 'Girador es obligatorio'
                            }
                        }
                    },
                    'IdAdquiriente': {
                        validators: {
                            notEmpty: {
                                message: 'Adquiriente es obligatorio'
                            }
                        }
                    },
                    'IdCategoria': {
                        validators: {
                            notEmpty: {
                                message: 'categoria es obligatorio'
                            }
                        }
                    },

                    'TEM': {
                        validators: {
                            notEmpty: {
                                message: 'TEM es obligatorio'
                            },
                            numeric: {
                                thousandsSeparator: '',
                                decimalSeparator: '.',
                                message: 'Ingresar sólo números'
                            },
                            between: {
                                min: 0,
                                max: 100,
                                message: 'El valor del TEM debe ser entre 0 y 100',
                            }
                        }
                    },

                    'InteresMoratorio': {
                        validators: {
                            notEmpty: {
                                message: 'Tasa moratoria es obligatorio'
                            },
                            numeric: {
                                thousandsSeparator: '',
                                decimalSeparator: '.',
                                message: 'Ingresar sólo números'
                            },
                            between: {
                                min: 0,
                                max: 100,
                                message: 'El valor de la tasa moratoria debe ser entre 0 y 100',
                            }
                        }
                    },

                    'PorcentajeFinanciamiento': {
                        validators: {
                            notEmpty: {
                                message: 'Porcentaje Financiamiento es obligatorio'
                            },
                            numeric: {
                                thousandsSeparator: '',
                                decimalSeparator: '.',
                                message: 'Ingresar sólo números'
                            },
                            between: {
                                min: 0,
                                max: 100,
                                message: 'El valor del Porcentaje Financiamiento debe ser entre 0 y 100',
                            }
                        }
                    },
                    'MontoOperacion': {
                        validators: {
                            notEmpty: {
                                message: 'Monto Operación es obligatorio'
                            },
                            numeric: {
                                thousandsSeparator: '',
                                decimalSeparator: '.',
                                message: 'Ingresar sólo números'
                            }
                        }
                    },
                    'DescCobranza': {
                        validators: {
                            notEmpty: {
                                message: 'Comisión de Estructuración es obligatorio'
                            },
                            numeric: {
                                thousandsSeparator: '',
                                decimalSeparator: '.',
                                message: 'Ingresar sólo números'
                            },
                            between: {
                                min: 0,
                                max: 100,
                                message: 'Comisión de Estructuración acepta % entre 0.01 y 100',
                            },
                        }
                    },
                    'DescFactura': {
                        validators: {
                            notEmpty: {
                                message: 'Descuento de Factura es obligatorio'
                            },
                            numeric: {
                                thousandsSeparator: '',
                                decimalSeparator: '.',
                                message: 'Ingresar sólo números'
                            },
                            //between: {
                            //    min: 0,
                            //    max: 100,
                            //    message: 'Descuento de Factura acepta % entre 0.01 y 100',
                            //},
                        }
                    },
                    'DescContrato': {
                        validators: {
                            notEmpty: {
                                message: 'Descuento de Contrato es obligatorio'
                            },
                            numeric: {
                                thousandsSeparator: '',
                                decimalSeparator: '.',
                                message: 'Ingresar sólo números'
                            },
                        }
                    },
                    'IdTipoMoneda': {
                        validators: {
                            notEmpty: {
                                message: 'Moneda es obligatorio'
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
        RegistroOperacion.getRevalidateFormElement(form, 'IdGirador', validator);
        RegistroOperacion.getRevalidateFormElement(form, 'IdAdquiriente', validator);
        RegistroOperacion.getRevalidateFormElement(form, 'IdCategoria', validator);
        //RegistroOperacion.getRevalidateFormElement(form, 'IdGiradorDireccion', validator);
        //RegistroOperacion.getRevalidateFormElement(form, 'IdAdquirienteDireccion', validator);
        RegistroOperacion.getRevalidateFormElement(form, 'IdTipoMoneda', validator);


        //RegistroOperacion.getRevalidateFormElement(form, 'IdGirador', validator);
        //RegistroOperacion.getRevalidateFormElement(form, 'IdAdquiriente', validator);
        //RegistroOperacion.getRevalidateFormElement(form, 'IdInversionista', validator);
        //RegistroOperacion.getRevalidateFormElement(form, 'IdGiradorDireccion', validator);
        //RegistroOperacion.getRevalidateFormElement(form, 'IdAdquirienteDireccion', validator);
        /* RegistroOperacion.getRevalidateFormElement(form, 'IdTipoMoneda', validator);*/
        saveButton.addEventListener('click', function (e) {
            e.preventDefault();
            validator.validate().then(function (status) {
                if (status == 'Valid') {
                    saveButton.setAttribute('data-kt-indicator', 'on');
                    saveButton.disabled = true;
                    var idOperacion = document.getElementById('IdOperacion');
                    var urlAction = $(form).attr('action');
                    if (idOperacion) {
                        urlAction += '?operacionId=' + $(idOperacion).val() + '&operacionFlat=I';
                    }
                    setTimeout(function () {
                        $.ajax({
                            type: 'POST',
                            dataType: 'json',
                            url: urlAction,
                            xhrFields: {
                                withCredentials: true
                            },
                            data: $(form).serializeObject(),
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
                                            $(window).attr('location', globalPath + 'Operacion/Registro?operacionId=' + data.data);
                                        }
                                    });
                                } else {
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
                    }, 2000);
                } else {
                    messageError('Lo sentimos, parece que se han detectado algunos errores. Vuelve a intentarlo.');
                }
            });
        });

        //CONDICIONAL SALVAVIDAS
        if (!evaluateButton) {
            return;
        }
        evaluateButton.addEventListener('click', function (e) {
            e.preventDefault();
            validator.validate().then(function (status) {
                if (status == 'Valid') {
                    /* var isEmpty = datatableFacturas.rows().count() === 0;
                     if (isEmpty) {
                         messageError('Antes de enviar la operación a Evaluar, debería ingresar como mínimo 1 factura.');
                     } else {*/
                    evaluateButton.setAttribute('data-kt-indicator', 'on');
                    evaluateButton.disabled = true;
                    var idOperacion = document.getElementById('IdOperacion');
                    var urlAction = $(form).attr('action');
                    if (idOperacion) {
                        urlAction += '?operacionId=' + $(idOperacion).val() + '&operacionFlat=E';
                    }
                    setTimeout(function () {
                        $.ajax({
                            type: 'POST',
                            dataType: 'json',
                            url: urlAction,
                            xhrFields: {
                                withCredentials: true
                            },
                            data: $(form).serializeObject(),
                            success: function (data) {
                                if (data.succeeded) {
                                    Swal.fire({
                                        text: 'Operación enviada a evaluación correctamente.',
                                        icon: 'success',
                                        buttonsStyling: false,
                                        confirmButtonText: 'Listo',
                                        customClass: {
                                            confirmButton: 'btn btn-primary'
                                        }
                                    }).then(function (result) {
                                        if (result.isConfirmed) {
                                            $(window).attr('location', globalPath + 'Operacion');
                                        }
                                    });
                                } else {
                                    evaluateButton.removeAttribute('data-kt-indicator');
                                    evaluateButton.disabled = false;
                                    messageError(data.message);
                                }
                            },
                            error: function (jqXHR, textStatus, errorThrown) {
                                evaluateButton.removeAttribute('data-kt-indicator');
                                evaluateButton.disabled = false;
                                messageError(errorThrown);
                            }
                        });
                    }, 2000);
                    // }
                } else {
                    messageError('Lo sentimos, parece que se han detectado algunos errores. Vuelve a intentarlo.');
                }
            });
        });
    }

    var handleRegisteLevantarrForm = function (e) {
        $('.comment').click(function (e) {
            e.preventDefault;
            var modal = bootstrap.Modal.getOrCreateInstance('#kt_modal_levantar_observacion');
            modal.show();
        });

        var form = document.getElementById('kt_modal_resultado_form');
        if (!form) {
            return;
        }
        var saveButton = document.getElementById('kt_save_button_observado');
        if (!saveButton) {
            return;
        }

        saveButton.addEventListener('click', function (e) {
            e.preventDefault();
            if ($('#ComentarioOperaciones').val() == '') {
                messageError('Comentario levantar operación es Obligatorio');
                return;
            }
            saveButton.setAttribute('data-kt-indicator', 'on');
            saveButton.disabled = true;
            setTimeout(function () {
                $.ajax({
                    type: 'POST',
                    dataType: 'json',
                    url: $(form).attr('action'),
                    xhrFields: {
                        withCredentials: true
                    },
                    data: $(form).serializeObject(),
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
                                    $(window).attr('location', globalPath + 'Operacion');
                                }
                            });
                        } else {
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
            }, 2000);
        });
    }
    var handleAnularEvaluacion = function () {
        var anularButton = document.getElementById('kt_anular_button');
        if (!anularButton) {
            return;
        }
        anularButton.addEventListener('click', function (e) {
            e.preventDefault();
            Swal.fire({
                text: '¿Está seguro de que desea anular la operación?',
                icon: 'warning',
                showCancelButton: true,
                buttonsStyling: false,
                showLoaderOnConfirm: true,
                confirmButtonText: 'Anular',
                cancelButtonText: 'Cancelar',
                customClass: {
                    confirmButton: 'btn fw-bold btn-danger',
                    cancelButton: 'btn fw-bold btn-active-light-primary'
                },
            }).then(function (result) {
                if (result.value) {
                    var idOperacion = $('#IdOperacion').val();
                    anularButton.setAttribute('data-kt-indicator', 'on');
                    anularButton.disabled = true;
                    setTimeout(function () {
                        $.ajax({
                            type: 'POST',
                            dataType: 'json',
                            url: globalPath + 'Operacion/AnularOperacion',
                            xhrFields: {
                                withCredentials: true
                            },
                            data: {
                                operacionId: idOperacion
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
                                            $(window).attr('location', globalPath + 'Operacion');
                                        }
                                    });
                                } else {
                                    anularButton.removeAttribute('data-kt-indicator');
                                    anularButton.disabled = false;
                                    messageError(data.message);
                                }
                            },
                            error: function (jqXHR, textStatus, errorThrown) {
                                anularButton.removeAttribute('data-kt-indicator');
                                anularButton.disabled = false;
                                messageError(errorThrown);
                            }
                        });
                    }, 2000);
                }
            });
        });
    }
    var initDataTableFacturas = function () {
        var idOperacion = document.getElementById('IdOperacion');
        if (!idOperacion) {
            return;
        }
        var tableFacturas = document.getElementById('kt_facturas_table');
        if (!tableFacturas) {
            return;
        }
        var tableFacturaAction = $('#Action').val();
        $(tableFacturas).DataTable({ ordering: false }).clear().destroy();
        $.fn.dataTable.ext.errMode = 'none';
        datatableFacturas = $(tableFacturas).DataTable({
            ordering: false,
            ajax: {
                type: 'GET',
                dataType: 'json',
                url: globalPath + 'Operacion/GetAllFacturas?operacionId=' + $(idOperacion).val(),
                dataSrc: function (data) {
                    return data.data;
                }
            },
            columns: [
                { data: 'cNroDocumento', 'autoWidth': true, class: 'text-left' },
                { data: 'nMonto', 'autoWidth': true, class: 'text-end', render: $.fn.dataTable.render.number(',', '.', 2, '') },
                //{
                //    data: 'nMonto', 'autoWidth': true, class: 'text-center', render: function (value, eee, row) {
                //        //console.log(row)
                //        return  value == 'Detalle'
                //            ? value
                //            : '<div> <input style="text-align: center" kt-t-ff="' + row.nMonto + '" type="text" data-id-fact="' + row.nIdOperacionesFacturas + '" id="monto_' + row.nIdOperacionesFacturas + '" class="form-control form-control monto-input" value="' + value + '"></div>';;
                //    }
                //},


                {
                    data: 'dFechaEmision', 'autoWidth': true, class: 'text-center', render: function (value) {
                        return moment(value).format('DD/MM/YYYY');
                    }
                },
                {
                    data: 'dFechaVencimiento', 'autoWidth': true, class: 'text-center fv', render: function (value) {
                        return moment(value).format('DD/MM/YYYY');
                    }
                },
                {
                    data: 'dFechaPagoNegociado', 'autoWidth': true, class: 'text-center', render: function (value, eee, row) {
                        //console.log(row)
                        return value == '0001-01-01T00:00:00' ? '' : (tableFacturaAction == 'Detalle')
                            ? moment(value).format('DD/MM/YYYY')
                            : (row.cIdEstadoFacturaHistorico.includes('4')
                                ? moment(value).format('DD/MM/YYYY')
                                : '<div> <input style="text-align: center" kt-t-fv="' + row.dFechaVencimiento + '" data-fv="' + row.dFechaVencimiento + '" type="text" data-id="' + row.nIdOperacionesFacturas + '" id="date_' + row.nIdOperacionesFacturas + '" class="form-control form-control flatpickr-input date-fnego" value="' + moment(value).format('DD/MM/YYYY') + '"></input> <span style="display:none" id="span_' + row.nIdOperacionesFacturas + '" class="spinner-border spinner-border-sm align-middle ms-2"></span></div>');
                    }
                },
                { data: 'cNombreDocumentoXML', 'autoWidth': true, class: 'text-center' },
                { data: 'nombreEstado', 'autoWidth': true, class: 'text-center' },
                { data: null, 'autoWidth': true, class: 'text-center', responsivePriority: -1 }

            ],
            columnDefs: [
                {
                    targets: 0,
                    render: function (data) {

                        var serieDocumento = ``;
                        $.each($.parseJSON(data), function (key, value) {
                            serieDocumento += `<strong>` + key + `:</strong> ` + value + `<br>`;
                        });
                        return serieDocumento;
                    }
                },
                {
                    targets: -1,
                    data: null,
                    visible: true,
                    orderable: false,
                    className: 'text-end',
                    render: function (data, type, row) {
                        montoFacturaTotal = data.nMontoTotal;
                        var buttonDownload = ((data.cNombreDocumentoXML == null || data.cNombreDocumentoXML == '') ? `` : `<a href="javascript:;" class="btn btn-icon btn-sm btn-outline btn-outline-solid btn-outline-default me-2 p-des" data-kt-factura-table-filter="download_file" data-filename="` + data.cNombreDocumentoXML + `" onclick="RegistroOperacion.fnDownloadOperaciones(` + data.nIdOperacionesFacturas + `)" title="` + data.cNombreDocumentoXML + `" data-id="` + data.nIdOperacionesFacturas + `"><i class="las la-download fs-2"></i></a>`);
                        var buttonDelete = ((tableFacturaAction == 'Detalle') ? `` : `<a href="javascript:;" class="btn btn-icon btn-light-dark btn-sm p-eli" data-kt-factura-table-filter="delete_row" data-parent="` + $(idOperacion).val() + `" data-id="` + data.nIdOperacionesFacturas + `" data-path="` + data.cRutaDocumentoXML + `" data-Operacion="` + data.nroOperacion + `"><i class="las la-ban fs-2"></i></a>`);
                        //var buttoEdit = `<a href="javascript:;" class="btn btn-icon btn-light-dark btn-sm open-modal p-edit" data-bs-toggle="modal" data-bs-target="#kt_factura_monto_modal" data-n-operacion="' + data.nIdOperaciones + '" title="Editar"><i class="las la-check-square fs-2"></i></a>`;
                        var buttonEdit = '<a href="javascript:;" class="btn btn-icon btn-light-dark btn-sm open-modal p-edit" data-bs-toggle="modal" data-bs-target="#kt_factura_monto_modal" data-n-operacion="' + $(idOperacion).val() + `" data-idfactura="` + data.nIdOperacionesFacturas + `" data-monto="` + data.nMonto + '" title="Editar"><i class="las la-pen fs-2"></i></a>';
                        return buttonDownload + buttonDelete + buttonEdit;
                        //((tableFacturaAction == 'Detalle') ? `` : ` < a href = "javascript:;" class="btn btn-icon btn-light-dark btn-sm open-modal p-eva" data - bs - toggle="modal" data - bs - target="#kt_modal_evaluacion_operacion" data - n - operacion="`+ data.nIdOperaciones + `" data - path="` + data.cRutaDocumentoXML + title="Evaluar"><i class="las la - check - square fs - 2"></i></a>`);
                        // var buttoEdit = ((tableFacturaAction == 'Detalle') ? `` : `<a href="javascript:;" class="btn btn-icon btn-light-dark btn-sm p-edit" data-kt-factura-table-filter="edit_row" data-parent="` + $(idOperacion).val() + `" data-id="` + data.nIdOperacionesFacturas + `" data-path="` + data.cRutaDocumentoXML + `" data-Operacion="` + data.nroOperacion + `" data-monto="` + data.nMonto + `"><i class="las la-trash fs-2"></i></a>`);

                        //`<a href="${globalPath}Operacion/Registro?operacionId=${data.nIdOperaciones}" class="btn btn-sm btn-icon btn-light btn-active-light-primary edit-row me-2 p-act" title="Editar"><i class="las la-pen fs-2"></i></a> 
                        return buttonDownload + buttonDelete + buttoEdit;
                    }
                }
            ]
        });
        datatableFacturas.on('draw', function () {
            handleDeleteFacturaForm();
            handleEditFacturaForm();
            /*handOpenFacturaForm();*/
            Common.init();
        });
    }
    var initDataTableCavali = function () {

        var idOperacion = document.getElementById('IdOperacion');
        if (!idOperacion) {
            return;
        }
        var tableCavali = document.getElementById('kt_cavali_table');
        if (!tableCavali) {
            return;
        }
        var tableCavaliAction = $('#Action').val();
        $(tableCavali).DataTable({ ordering: false }).clear().destroy();
        $.fn.dataTable.ext.errMode = 'none';
        datatableCavali = $(tableCavali).DataTable({
            ordering: false,
            ajax: {
                type: 'GET',
                dataType: 'json',
                url: globalPath + 'Operacion/GetAllCavali?operacionId=' + $(idOperacion).val(),
                dataSrc: function (data) {
                    return data.data;
                }
            },
            columns: [
                { data: 'cFactura', 'autoWidth': true, class: 'text-left' },
                { data: 'cRespuestaAceptante', 'autoWidth': true, class: 'text-left' },
                {
                    data: 'cAcqResponseDate', 'autoWidth': true, class: 'text-center', render: function (value) {
                        return moment(value).format('DD/MM/YYYY');
                    }
                },
                {
                    data: 'dFechaActualizacion', 'autoWidth': true, class: 'text-center', render: function (value) {
                        return moment(value).format('DD/MM/YYYY');
                    }
                }

            ],
            columnDefs: [
                {
                    targets: 0,
                    render: function (data) {

                        var serieDocumento = ``;
                        $.each($.parseJSON(data), function (key, value) {
                            serieDocumento += `<strong>` + key + `:</strong> ` + value + `<br>`;
                        });
                        return serieDocumento;
                    }
                },
            ]
        });
        datatableCavali.on('draw', function () {
        });
    }
    var handleAddFacturaForm = function () {
        var formAddFactura = document.getElementById('kt_form_add_factura');
        if (!formAddFactura) {
            return;
        }
        var addButton = document.getElementById('kt_add_factura');
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
            var montoOperacion = $('#HiddenMontoOperacion').val()
            console.log(parseFloat(montoFacturaTotal), parseFloat($('#Monto').val()), parseFloat(montoOperacion))
            if (parseFloat(montoFacturaTotal) + parseFloat($('#Monto').val()) > parseFloat(montoOperacion)) {
                messageError('Lo sentimos, El monto total de Facturas no debe ser mayor al monto Importe total de planilla ' + montoOperacion + '.');
                return;
            }
            validator.validate().then(function (status) {
                if (status == 'Valid') {
                    addButton.setAttribute('data-kt-indicator', 'on');
                    addButton.disabled = true;
                    setTimeout(function () {
                        var tokenVerification = $('input[name="__RequestVerificationToken"]').val();
                        var _documento = {};
                        $('input.factura-list').each(function () {
                            _documento[$(this).attr('name')] = $(this).val();
                        });
                        var json_documento = JSON.stringify(_documento);
                        var fileInputXml = $('#fileXml')[0];
                        var formData = new FormData();
                        formData.append('IdOperacionCabeceraFacturas', $('#IdOperacionCabeceraFacturas').val());
                        formData.append('nIdGiradorFact', $('#nIdGiradorFact').val());
                        formData.append('nIdAdquirenteFact', $('#nIdAdquirenteFact').val());
                        formData.append('fileXml', fileInputXml.files[0]);
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
                                            //initDataTableFacturas();
                                            $(window).attr('location', globalPath + 'Operacion/Registro?operacionId=' + $('#IdOperacionCabeceraFacturas').val());
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

    var initToggleToolbarModal = function () {
        $('#nIdOperacionEval').val($('#IdOperacion').val());
        //var container = document.querySelector('#kt_pago_table');
        //var container2 = document.querySelector('n-pago');    
    }

    $('#kt_modal_evaluacion_operacion').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget); // Botón que abrió el modal
        var nOperacion = button.data('n-operacion');

        $('#nIdOperacionEval').val(nOperacion);

    });

    $('#kt_modal_calcular_operacion').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget); // Botón que abrió el modal
        var nOperacion = button.data('n-operacion');
        var nroOperacion = button.data('n-nroperacion');
        
        $('#nIdOperacionCal').val(nOperacion);
        $('#nNroOperacionCal').val(nroOperacion);
        $('#cFechaCalculo').flatpickr({
            dateFormat: 'd/m/Y',
            defaultDate: 'today'
        });

    });

    //var buttonEdit = '<a href="javascript:;" class="btn btn-icon btn-light-dark btn-sm open-modal p-edit" data-bs-toggle="modal" data-bs-target="#kt_factura_monto_modal" data-n-operacion="' + data.nIdOperaciones + `" data-idfactura="` + data.nIdOperacionesFacturas + `" data-monto="` + data.nMonto + '" title="Editar"><i class="las la-pen fs-2"></i></a>';
    $('#kt_factura_monto_modal').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget); // Botón que activa el modal
        var nOperacion = button.data('n-operacion'); // Extrae la información de los datos del botón
        var nOperacionFactura = button.data('idfactura');
        var nmonto = button.data('monto');
        /*  var modal = $(this);*/
        // var nOpe = button.data('n-operacion'); // Obtener el valor data-n-pago
        console.log('Valor data-n-nOpe:', nOperacion);
        console.log('Valor data-idfactura:', nOperacionFactura);
        console.log('Valor data-monto: ', nmonto);
        $('#nIdOperaciones').val(nOperacion);
        $('#nIdOperacionesFacturas').val(nOperacionFactura);
        $('#nMonto').val(nmonto);
    });



    var handleDownloadFactura = function () {
        var tableFacturas = document.querySelector('#kt_facturas_table');
        if (!tableFacturas) {
            return;
        }
        var downloadFacturaButton = tableFacturas.querySelectorAll('[data-kt-factura-table-filter="download_file"]');
        downloadFacturaButton.forEach(d => {
            d.addEventListener('click', function (e) {

                e.preventDefault();
                var filename = $(this).data('filename');
                var nIdOperacionFactura = $(this).data('id');
                //filename = nroOperacion + '\\' + filename;
                RegistroOperacion.getDownloadFile(nIdOperacionFactura);
            });
        });
    }

    var handleModalEditarMonto = function () {
        var form = document.getElementById('kt_factura_monto_form');
        if (!form) {
            return;
        }
        var idope = $('#nIdOperaciones').val();
        console.log('idope::.::', idope);

        var saveButton = document.getElementById('kt_save_monto_button');
        var validator;

        validator = FormValidation.formValidation(
            form,
            {
                fields: {
                    'nMonto': {
                        validators: {
                            notEmpty: {
                                message: 'Monto Factura es obligatorio'
                            },
                            numeric: {
                                thousandsSeparator: '',
                                decimalSeparator: '.',
                                message: 'Ingresar sólo números'
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
            });
        var idope = $('#nIdOperaciones').val();
        console.log('idope::.::', idope);
        //RegistroOperacion.getRevalidateFormElement(form, 'nIdEstadoEvaluacion', validator);
        //RegistroOperacion.getRevalidateFormElement(form, 'cComentario', validator);
        saveButton.addEventListener('click', function (e) {
            e.preventDefault();
            validator.validate().then(function (status) {
                if (status == 'Valid') {
                    saveButton.setAttribute('data-kt-indicator', 'on');
                    saveButton.disabled = true;
                    setTimeout(function () {
                        $.ajax({
                            type: 'POST',
                            dataType: 'json',
                            url: $(form).attr('action'),
                            xhrFields: {
                                withCredentials: true
                            },
                            data: $(form).serializeObject(),
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
                                            $(window).attr('location', globalPath + 'Operacion/Registro?operacionId=' + $('#nIdOperaciones').val());
                                        }
                                    });
                                } else {
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
                    }, 2000);
                } else {
                    messageError('Lo sentimos, parece que se han detectado algunos errores. Vuelve a intentarlo.');
                }
            });
        });
    }

    var handleModalControlEvaluacion = function () {
        var table = document.getElementById('kt_operaciones_table');
        if (!table) {
            return;
        }

        var form = document.getElementById('kt_modal_evaluacion_form');
        if (!form) {
            return;
        }

        var saveButton = $('#kt_save_estado_button');
        var validator;

        validator = FormValidation.formValidation(
            form,
            {
                fields: {
                    'nIdEstadoEvaluacion': {
                        validators: {
                            notEmpty: {
                                message: 'Estado es obligatorio'
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
            });
        RegistroOperacion.getRevalidateFormElement(form, 'nIdEstadoEvaluacion', validator);
        RegistroOperacion.getRevalidateFormElement(form, 'cComentario', validator);
        saveButton.off('click');
        saveButton.on('click', function (e) {
            e.preventDefault();
            validator.validate().then(function (status) {
                if (status == 'Valid') {
                    saveButton.attr('data-kt-indicator', 'on');
                    saveButton.prop('disabled', true);
                    setTimeout(function () {
                        $.ajax({
                            type: 'POST',
                            dataType: 'json',
                            url: $(form).attr('action'),
                            xhrFields: {
                                withCredentials: true
                            },
                            data: $(form).serializeObject(),
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
                                            $(window).attr('location', globalPath + 'Operacion');
                                        }
                                    });
                                } else {
                                    saveButton.removeAttr('data-kt-indicator');
                                    saveButton.prop('disabled', false);
                                    messageError(data.message);
                                }
                            },
                            error: function (jqXHR, textStatus, errorThrown) {
                                saveButton.removeAttr('data-kt-indicator');
                                saveButton.prop('disabled', false);
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

    var handleDeleteFacturaForm = function () {
        var tableFacturas = document.querySelector('#kt_facturas_table');
        if (!tableFacturas) {
            return;
        }
        var deleteFacturaButton = tableFacturas.querySelectorAll('[data-kt-factura-table-filter="delete_row"]');
        deleteFacturaButton.forEach(d => {
            d.addEventListener('click', function (e) {
                debugger;
                e.preventDefault();
                var idOperacion = $(this).data('id');
                var filePath = $(this).data('path');

                var parent = e.target.closest('tr');
                Swal.fire({
                    text: '¿Estás seguro de que quieres eliminar la factura?',
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
                    debugger;
                    if (result.value) {
                        var token = $('input[name="__RequestVerificationToken"]').val();
                        $.ajax({
                            type: 'POST',
                            dataType: 'json',
                            url: globalPath + 'Operacion/EliminarFactura',
                            data: {
                                operacionFacturaId: idOperacion,
                                filePath: filePath
                            },
                            headers: {
                                'RequestVerificationToken': token
                            },
                            success: function (data) {
                                debugger;
                                if (data.succeeded) {
                                    Swal.fire({
                                        text: 'Eliminaste correctamente la Factura.',
                                        icon: 'success',
                                        buttonsStyling: false,
                                        confirmButtonText: 'Listo',
                                        customClass: {
                                            confirmButton: 'btn fw-bold btn-primary',
                                        }
                                    }).then(function () {
                                        datatableFacturas.row($(parent)).remove().draw();
                                    });
                                } else {
                                    messageError('La Factura no fue eliminada (' + data.message + ')');
                                }
                            },
                            error: function (jqXHR, textStatus, errorThrown) {
                                messageError(errorThrown);
                            }
                        });
                    } else if (result.dismiss === 'cancel') {
                        messageError('La Factura no fue eliminada.');
                    }
                });
            });
        });
    }





    var handOpenFacturaForm = function () {
        var tableFacturas = document.querySelector('#kt_facturas_table');
        if (!tableFacturas) {
            return;
        }
        var editFacturaButton = tableFacturas.querySelectorAll('[data-kt-factura-table-filter="edit_row"]');
        editFacturaButton.forEach(d => {
            d.addEventListener('click', function (e) {
                debugger;
                e.preventDefault();
                $('#kt_factura_monto_modal').show();
            });
        });


    }







    var handleEditFacturaForm = function () {


        var tableFacturas = document.querySelectorAll('#kt_facturas_table tbody tr');
        if (!tableFacturas) {
            return;
        }

        tableFacturas.forEach(f => {
            var inputsFv = f.querySelectorAll('[data-fv]');
            // console.log(inputsFv)
            inputsFv.forEach(d => {

                //console.log($(d))


                flatpickr('#' + $(d)[0].id, {
                    dateFormat: 'd/m/Y',
                    minDate: Date.parse($(d)[0].dataset.fv)

                });
            });
        });




        //flatpickr('.date-fnego', {
        //    dateFormat: 'd/m/Y',

        //});

        $('.date-fnego').on('change', function (e) {
            //console.log(e.target.value)
            //console.log(e.target.dataset.id)

            var inputFecha = $('#date_' + e.target.dataset.id);
            var spanFecha = $('#span_' + e.target.dataset.id);
            //console.log(inputFecha)
            inputFecha.hide();
            spanFecha.show();


            //return
            var token = $('input[name="__RequestVerificationToken"]').val();
            var formData = new FormData();
            formData.append('nIdOperacionesFacturas', e.target.dataset.id);
            formData.append('dFechaPagoNegociado', e.target.value);

            setTimeout(function () {
                $.ajax({
                    type: 'POST',
                    dataType: 'json',
                    url: 'Operacion/EditarFactura',
                    cache: false,
                    contentType: false,
                    processData: false,
                    headers: {
                        'RequestVerificationToken': token
                    },
                    data: formData,
                    success: function (data) {
                        //console.log(data)
                        if (data) {
                            Swal.fire({
                                text: 'Fecha de pago negociado actualizado.',
                                icon: 'success',
                                buttonsStyling: false,
                                confirmButtonText: 'Listo',
                                customClass: {
                                    confirmButton: 'btn btn-primary'
                                }
                            }).then(function (result) {
                                if (result.isConfirmed) {
                                    inputFecha.show();
                                    spanFecha.hide();
                                    //$(window).attr('location', globalPath + 'Operacion/Registro?operacionId=' + data.data);
                                }
                            });
                        } else {
                            //saveButton.removeAttribute('data-kt-indicator');
                            //saveButton.disabled = false;
                            inputFecha.show();
                            spanFecha.hide();
                            messageError(data.message);
                        }
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        //saveButton.removeAttribute('data-kt-indicator');
                        //saveButton.disabled = false;
                        inputFecha.show();
                        spanFecha.hide();
                        messageError(errorThrown);
                    }
                });
            }, 2000);
        });
    }

    var handleUploadExcel = function () {
        var form = document.getElementById('kt_modal_masivo_form');
        if (!form) {
            return;
        }
        var uploadButton = document.getElementById('kt_upload_button');
        var validator;
        $(document).on('click', '.open-masivo-operacion', function () {
            $('#fileExcelMasivo').val('');
        });
        validator = FormValidation.formValidation(
            form,
            {
                fields: {
                    'fileExcelMasivo': {
                        validators: {
                            notEmpty: {
                                message: 'Archivo Excel de Operaciones es obligatorio'
                            },
                            file: {
                                extension: 'xls,xlsx',
                                type: 'application/vnd.ms-excel,application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
                                message: 'Debe adjuntar un archivo Excel en los formatos .xlsx o .xls',
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
        uploadButton.addEventListener('click', function (e) {
            e.preventDefault();
            validator.validate().then(function (status) {
                if (status == 'Valid') {
                    uploadButton.setAttribute('data-kt-indicator', 'on');
                    uploadButton.disabled = true;
                    setTimeout(function () {
                        var tokenVerification = $('input[name="__RequestVerificationToken"]').val();
                        var fileUpload = $('#fileExcelMasivo')[0];
                        var formData = new FormData();
                        formData.append('fileExcel', fileUpload.files[0]);
                        $.ajax({
                            type: 'POST',
                            dataType: 'json',
                            url: $(form).attr('action'),
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
                                        $(window).attr('location', globalPath + 'Operacion');
                                    });
                                } else {
                                    uploadButton.removeAttribute('data-kt-indicator');
                                    uploadButton.disabled = false;
                                    messageError(data.message);
                                }
                            },
                            error: function (jqXHR, textStatus, errorThrown) {
                                uploadButton.removeAttribute('data-kt-indicator');
                                uploadButton.disabled = false;
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
    var handleUploadFacturas = function () {
        var form = document.getElementById('kt_modal_masivo_facturas_form');
        if (!form) {
            return;
        }
        var uploadFacturasButton = document.getElementById('kt_upload_button_factura');
        var validator;
        var id = '#kt_dropzonejs_facturas_xml';
        var dropzone = document.querySelector(id);
        var previewNode = dropzone.querySelector('.dropzone-item');
        var previewTemplate = previewNode.parentNode.innerHTML;
        previewNode.id = '';
        previewNode.parentNode.removeChild(previewNode);
        var facturasDropzone = new Dropzone(id, {
            url: globalPath + 'Operacion/CargaMasivaFacturas',
            paramName: 'file',
            autoQueue: false,
            uploadMultiple: true,
            parallelUploads: 100,
            maxFiles: 100,
            previewTemplate: previewTemplate,
            maxFilesize: 4,
            previewsContainer: id + ' .dropzone-items',
            clickable: id + ' .dropzone-select',
            acceptedFiles: '.xml',
            accept: function (file, done) {
                if (file.type != 'text/xml') {
                    done('Error: No se aceptan archivos de este tipo.');
                } else {
                    done();
                }
            },
            init: function () {
                this.on('addedfile', function (file) {
                    var dropzoneItems = dropzone.querySelectorAll('.dropzone-item');
                    dropzoneItems.forEach(dropzoneItem => {
                        dropzoneItem.style.display = '';
                    });
                    dropzone.querySelector('.dropzone-remove-all').style.display = 'inline-block';
                });
                this.on('totaluploadprogress', function (progress) {
                    var progressBars = dropzone.querySelectorAll('.progress-bar');
                    progressBars.forEach(progressBar => {
                        progressBar.style.width = progress + '%';
                    });
                });
                this.on('removedfile', function (file) {
                    if (facturasDropzone.files.length < 1) {
                        dropzone.querySelector('.dropzone-remove-all').style.display = 'none';
                    }
                });
                this.on('sending', function (file, response, formData) {
                    var progressBars = dropzone.querySelectorAll('.progress-bar');
                    var fileFacturas = $('#fileExcelFacturasMasivo')[0];
                    progressBars.forEach(progressBar => {
                        progressBar.style.opacity = '1';
                    });
                    formData.append('operacionId', $('#operacionId').val());
                    formData.append('fileExcelFacturas', fileFacturas.files[0]);
                    formData.append('filesXml', file);
                });
                this.on('successmultiple', function (file, response) {
                    if (response.succeeded) {
                        Swal.fire({
                            text: response.message,
                            icon: 'success',
                            buttonsStyling: false,
                            confirmButtonText: 'Listo',
                            customClass: {
                                confirmButton: 'btn btn-primary'
                            }
                        }).then(function (result) {
                            $(window).attr('location', globalPath + 'Operacion');
                        });
                    } else {
                        uploadFacturasButton.removeAttribute('data-kt-indicator');
                        uploadFacturasButton.disabled = false;
                        $('#kt_facturas_carga_masiva').modal('hide');
                        messageError(response.message);
                    }
                });
                this.on('complete', function (progress) {
                    var progressBars = dropzone.querySelectorAll('.dz-complete');
                    setTimeout(function () {
                        progressBars.forEach(progressBar => {
                            progressBar.querySelector('.progress-bar').style.opacity = '0';
                            progressBar.querySelector('.progress').style.opacity = '0';
                        });
                    }, 300);
                });
                this.on('queuecomplete', function (progress) {
                    var uploadIcons = dropzone.querySelectorAll('.dropzone-upload');
                    uploadIcons.forEach(uploadIcon => {
                        uploadIcon.style.display = 'none';
                    });
                });
            }
        });
        dropzone.querySelector('.dropzone-remove-all').addEventListener('click', function () {
            dropzone.querySelector('.dropzone-remove-all').style.display = 'none';
            facturasDropzone.removeAllFiles(true);
        });
        validator = FormValidation.formValidation(
            form,
            {
                fields: {
                    'fileExcelFacturasMasivo': {
                        validators: {
                            notEmpty: {
                                message: 'Archivo Excel de Facturas es Obligatorio'
                            },
                            file: {
                                extension: 'xls,xlsx',
                                type: 'application/vnd.ms-excel,application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
                                message: 'Debe adjuntar un archivo Excel en los formatos .xlsx o .xls',
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
        uploadFacturasButton.addEventListener('click', function (e) {
            e.preventDefault();
            validator.validate().then(function (status) {
                if (status == 'Valid') {
                    uploadFacturasButton.setAttribute('data-kt-indicator', 'on');
                    uploadFacturasButton.disabled = true;
                    facturasDropzone.enqueueFiles(facturasDropzone.getFilesWithStatus(Dropzone.ADDED));
                } else {
                    messageError('Lo sentimos, parece que se han detectado algunos errores. Vuelve a intentarlo.');
                }
            });
        });
    }
    var handleRevalidateFormElement = function (form, element, validator) {
        $(form.querySelector('[name="' + element + '"]')).on('change', function () {
            validator.revalidateField(element);
        });
    }
    var handleDownloadFile = function (nIdOperacionFactura) {
        window.open(globalPath + 'Operacion/DownloadFile?nIdOperacionFactura=' + encodeURIComponent(nIdOperacionFactura), '_blank');
    }

    var handleDescarga = function () {
        var form = document.getElementById('kt_search_form');
        if (!form) {
            return;
        }
        var exportButton = document.getElementById('kt_export_button');


        if (!exportButton) {
            return;
        }
        exportButton.addEventListener('click', function (event) {
            event.preventDefault();

            var operacion = $('#NroOperacion').val();
            var girador = $('#RazonGirador').val();
            var adquiriente = $('#RazonAdquiriente').val();
            var fecha = $('#FechaCreacion').val();
            var estado = $('#Estado').val();

            window.open(globalPath + `Operacion/DescargarRegistroOperacionArchivo?operacion=${operacion}&girador=${girador}&adquiriente=${adquiriente}&fecha=${fecha}&estado=${estado}`, '_blank');

        });
    }

    //****************************INI-21-01-2023****************************//


    var handleDownloadFileSolicitud = function (nIdSolicitudEvaluacion) {
        window.open(globalPath + 'Operacion/DownloadFileSolicitud?nIdSolicitudEvaluacion=' + encodeURIComponent(nIdSolicitudEvaluacion), '_blank');
    }

    var handleAddDocumentoSolicitudForm = function () {
        var formAddDocumento = document.getElementById('kt_form_add_documentos');
        if (!formAddDocumento) {
            return;
        }
        var addButton = document.getElementById('kt_add_documento');
        var validator;
        $('#IdOperacionCabeceraFacturas').val($('#IdOperacion').val());
        validator = FormValidation.formValidation(
            formAddDocumento,
            {
                fields: {
                    'fileDocumento': {
                        validators: {
                            file: {
                                //extension: 'xml',
                                //type: 'text/xml',
                                message: 'Debe adjuntar un documento',
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
            validator.validate().then(function (status) {
                if (status == 'Valid') {
                    addButton.setAttribute('data-kt-indicator', 'on');
                    addButton.disabled = true;
                    setTimeout(function () {

                        var tokenVerification = $('input[name="__RequestVerificationToken"]').val();
                        var fileInputXml = $('#fileDocumento')[0];

                        var formData = new FormData();
                        formData.append('IdOperacionCabeceraFacturas', $('#IdOperacionCabeceraFacturas').val());
                        formData.append('nTipoDocumento', $('#TipoDocumento').val());
                        formData.append('fileDocumentoXml', fileInputXml.files[0]);

                        $.ajax({
                            type: 'POST',
                            url: $(formAddDocumento).attr('action'),
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
                                            initDataTableDocumentoSolicitud();
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


    var initDataTableDocumentoSolicitud = function () {
        var idOperacion = document.getElementById('IdOperacion');
        if (!idOperacion) {
            return;
        }
        var tableDocumentoSolicitud = document.getElementById('kt_documento_table');
        if (!tableDocumentoSolicitud) {
            return;
        }
        var tableDocumentoSolicitudAction = $('#Action').val();
        $(tableDocumentoSolicitud).DataTable({ ordering: false }).clear().destroy();
        $.fn.dataTable.ext.errMode = 'none';
        datatableDocumentoSolicitud = $(tableDocumentoSolicitud).DataTable({
            ordering: false,
            ajax: {
                type: 'GET',
                dataType: 'json',
                url: globalPath + 'Operacion/GetAllDocumentoSolicitud?operacionId=' + $(idOperacion).val(),
                dataSrc: function (data) {
                    console.log('data:', data);

                    return data.data;
                }
            },
            columns: [
                { data: 'nIdDocumentoSolEvalOperacion', 'autoWidth': true, class: 'text-left' },
                { data: 'idSolEvalOperacion', 'autoWidth': true, class: 'text-left' },
                { data: 'desDocumento', 'autoWidth': true, class: 'text-left' },
                { data: 'nombreDocumento', 'autoWidth': true, class: 'text-left' },
                {
                    data: 'fechaCreacion', 'autoWidth': true, class: 'text-center', render: function (value) {
                        return moment(value).format('DD/MM/YYYY');
                    }
                },
                { data: 'cRutaDocumento', 'autoWidth': true, class: 'text-left' },

                { data: null, 'autoWidth': true, class: 'text-center', responsivePriority: -1 }
            ],
            columnDefs: [
                {
                    targets: 0,
                    visible: false,
                    searchable: false
                },
                {
                    targets: 1,
                    visible: false,
                    searchable: false
                },
                {
                    targets: 5,
                    visible: false,
                    searchable: false
                },
                {
                    targets: -1,
                    data: null,
                    visible: true,
                    orderable: false,
                    className: 'text-end',
                    render: function (data, type, row) {
                        /*  montoFacturaTotal = data.nMontoTotal;*/
                        /*var buttonDownload = '';*/
                        if (typeof data != 'undefined') {
                            var buttonDownload = ((data.nombreDocumento == null || data.nombreDocumento == '') ? `` : `<a href="javascript:;" class="btn btn-icon btn-sm btn-outline btn-outline-solid btn-outline-default me-2 p-des" data-kt_documento_table-filter="download_file" data-filename="` + data.nombreDocumento + `" onclick="RegistroOperacion.fnDownloadSolicitudOperaciones(` + data.nIdDocumentoSolEvalOperacion + `)" title="` + data.nombreDocumento + `" data-id="` + data.nIdDocumentoSolEvalOperacion + `"><i class="las la-download fs-2"></i></a>`);
                            var buttonDelete = ((tableDocumentoSolicitudAction == 'Detalle') ? `` : `<a href="javascript:;" class="btn btn-icon btn-light-dark btn-sm p-eli" data-kt_documento_table-filter="delete_row" data-id="` + data.nIdDocumentoSolEvalOperacion + `" data-path="` + data.cRutaDocumento + `" ><i class="las la-ban fs-2"></i></a>`);
                        }
                        return buttonDownload + buttonDelete;
                    }
                }
            ]
        });
        datatableDocumentoSolicitud.on('draw', function () {
            handleDeleteDocumentoSolicitudForm();
            handleEditDocumentSolicitudForm();
            Common.init();
        });
    }


    var handleDeleteDocumentoSolicitudForm = function () {
        var tableDocumentoSolicitud = document.querySelector('#kt_documento_table');
        if (!tableDocumentoSolicitud) {
            return;
        }
        var deleteDocumentoSolicitudButton = tableDocumentoSolicitud.querySelectorAll('[data-kt_documento_table-filter="delete_row"]');
        deleteDocumentoSolicitudButton.forEach(d => {
            d.addEventListener('click', function (e) {
                debugger;
                e.preventDefault();
                var nIdDocumentoSolEvalOperacion = $(this).data('id');
                var filePath = $(this).data('path');

                var parent = e.target.closest('tr');
                Swal.fire({
                    text: '¿Estás seguro de que quieres eliminar el documento ?',
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
                    debugger;
                    if (result.value) {
                        var token = $('input[name="__RequestVerificationToken"]').val();
                        $.ajax({
                            type: 'POST',
                            dataType: 'json',
                            url: globalPath + 'Operacion/EliminarDocumentoSolicitud',
                            data: {
                                nIdDocumentoSolEvalOperacion: nIdDocumentoSolEvalOperacion,
                                filePath: filePath
                            },
                            headers: {
                                'RequestVerificationToken': token
                            },
                            success: function (data) {
                                debugger;
                                if (data.succeeded) {
                                    Swal.fire({
                                        text: 'Eliminaste correctamente el documento.',
                                        icon: 'success',
                                        buttonsStyling: false,
                                        confirmButtonText: 'Listo',
                                        customClass: {
                                            confirmButton: 'btn fw-bold btn-primary',
                                        }
                                    }).then(function () {
                                        datatableDocumentoSolicitud.row($(parent)).remove().draw();
                                    });
                                } else {
                                    messageError('El documento no fue eliminado (' + data.message + ')');
                                }
                            },
                            error: function (jqXHR, textStatus, errorThrown) {
                                messageError(errorThrown);
                            }
                        });
                    } else if (result.dismiss === 'cancel') {
                        messageError('El documento no fue eliminado.');
                    }
                });
            });
        });
    }


    var handleDownloadDocumentoSolicitud = function () {
        var tableFacturas = document.querySelector('#kt_documento_table');
        if (!tableFacturas) {
            return;
        }
        var downloadSolicitudDocumentoButton = tableFacturas.querySelectorAll('[data-kt_documento_table-filter="download_file"]');
        downloadSolicitudDocumentoButton.forEach(d => {
            d.addEventListener('click', function (e) {

                e.preventDefault();
                /*var filename = $(this).data('filename');*/
                var nIdSolicitudEvaluacion = $(this).data('id');
                //filename = nroOperacion + '\\' + filename;
                RegistroOperacion.getDownloadSolicitudOperacionesFile(nIdSolicitudEvaluacion);
            });
        });
    }

    var handleEditDocumentSolicitudForm = function () {
        //flatpickr('.date-fnego', {
        //    dateFormat: 'd/m/Y',
        //});
        //$('.date-fnego').on('change', function (e) {
        //    //console.log(e.target.value)
        //    //console.log(e.target.dataset.id)

        //    var inputFecha = $('#date_' + e.target.dataset.id);
        //    var spanFecha = $('#span_' + e.target.dataset.id);
        //    console.log(inputFecha)
        //    inputFecha.hide();
        //    spanFecha.show();


        //    //return
        //    var token = $('input[name="__RequestVerificationToken"]').val();
        //    var formData = new FormData();
        //    formData.append('nIdOperacionesFacturas', e.target.dataset.id);
        //    formData.append('dFechaPagoNegociado', e.target.value);

        //    setTimeout(function () {
        //        $.ajax({
        //            type: 'POST',
        //            dataType: 'json',
        //            url: 'Operacion/EditarFactura',
        //            cache: false,
        //            contentType: false,
        //            processData: false,
        //            headers: {
        //                'RequestVerificationToken': token
        //            },
        //            data: formData,
        //            success: function (data) {
        //                console.log(data)
        //                if (data) {
        //                    Swal.fire({
        //                        text: 'Fecha de pago negociado actualizado.',
        //                        icon: 'success',
        //                        buttonsStyling: false,
        //                        confirmButtonText: 'Listo',
        //                        customClass: {
        //                            confirmButton: 'btn btn-primary'
        //                        }
        //                    }).then(function (result) {
        //                        if (result.isConfirmed) {
        //                            inputFecha.show();
        //                            spanFecha.hide();
        //                        }
        //                    });
        //                } else {
        //                    inputFecha.show();
        //                    spanFecha.hide();
        //                    messageError(data.message);
        //                }
        //            },
        //            error: function (jqXHR, textStatus, errorThrown) {
        //                inputFecha.show();
        //                spanFecha.hide();
        //                messageError(errorThrown);
        //            }
        //        });
        //    }, 2000);
        //});
    }
    //****************************FIN-21-01-2023****************************//

    var handleModalControlCalculo = function () {
        var table = document.getElementById('kt_operaciones_table');
        if (!table) {
            return;
        }

        var form = document.getElementById('kt_modal_calcular_operacion_form');
        if (!form) {
            return;
        }

        var saveButton = $('#kt_save_calculo_button');
        var validator;

        validator = FormValidation.formValidation(
            form,
            {
                fields: {
                    'nIdOperacionCal': {
                        validators: {
                            notEmpty: {
                                message: 'el codigo es obligatorio'
                            }
                        }
                    },
                    'nNroOperacionCal': {
                        validators: {
                            notEmpty: {
                                message: 'el nro de operació es obligatorio'
                            }
                        }
                    },

                    'cFechaCalculo': {
                        validators: {
                            notEmpty: {
                                message: 'La fecha es obligatorio'
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
            });
        RegistroOperacion.getRevalidateFormElement(form, 'nIdOperacionCal', validator);
        RegistroOperacion.getRevalidateFormElement(form, 'nNroOperacionCal', validator);
        RegistroOperacion.getRevalidateFormElement(form, 'cFechaCalculo', validator);
        saveButton.off('click');
        saveButton.on('click', function (e) {
            e.preventDefault();
            validator.validate().then(function (status) {
                if (status == 'Valid') {
                    saveButton.attr('data-kt-indicator', 'on');
                    saveButton.prop('disabled', true);
                    setTimeout(function () {
                        $.ajax({
                            type: 'POST',
                            dataType: 'json',
                            url: $(form).attr('action'),
                            xhrFields: {
                                withCredentials: true
                            },
                            data: $(form).serializeObject(),
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
                                            $(window).attr('location', globalPath + 'Operacion');
                                        }
                                    });
                                } else {
                                    saveButton.removeAttr('data-kt-indicator');
                                    saveButton.prop('disabled', false);
                                    messageError(data.message);
                                }
                            },
                            error: function (jqXHR, textStatus, errorThrown) {
                                saveButton.removeAttr('data-kt-indicator');
                                saveButton.prop('disabled', false);
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

    return {
        init: function () {
            handleFilterTable();
            initDatatable();
            handleRegisterForm();
            handleRegisteLevantarrForm();
            handleAnularEvaluacion();
            initDataTableFacturas();
            initDataTableCavali()
            handleAddFacturaForm();
            handleDeleteFacturaForm();
            handleDeleteOperacionForm();
            handleUploadExcel();
            handleUploadFacturas();
            initToggleToolbarModal();
            handOpenFacturaForm();
            // handleAnularEvaluacion2();
            /* handleFormEvaluarOperacion();*/
            /*  handleComentarios();*/
            handleDescarga();

            //****************************INI-21-01-2023****************************//
            handleAddDocumentoSolicitudForm();
            handleDeleteDocumentoSolicitudForm();
            initDataTableDocumentoSolicitud();
            handleEditDocumentSolicitudForm();
            handleModalEditarMonto();
            //****************************FIN-21-01-2023****************************//
        },
        getRevalidateFormElement: function (form, elem, val) {
            handleRevalidateFormElement(form, elem, val);
        },
        getDownloadFile: function (nIdOperacionFactura) {
            handleDownloadFile(nIdOperacionFactura);
        },
        fnDownloadOperaciones: function (nIdOperacionFactura) {
            handleDownloadFile(nIdOperacionFactura);
        },

        fnDownloadSolicitudOperaciones: function (nIdSolicitudEvaluacion) {
            handleDownloadFileSolicitud(nIdSolicitudEvaluacion);
        },

        getDownloadSolicitudOperacionesFile: function (nIdSolicitudEvaluacion) {
            handleDownloadFileSolicitud(nIdSolicitudEvaluacion);
        },
    }

}();
KTUtil.onDOMContentLoaded(function () {
    RegistroOperacion.init();
});
