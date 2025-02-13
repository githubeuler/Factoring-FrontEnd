'use strict';
var globalPath = $('base').attr('href');

var RegistroUsuario = function () {
    var datatable;

    var initDatatable = function () {
        var table = document.getElementById('kt_usuarios_table');
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
                url: globalPath + 'Usuario/GetUsuarioAllList',
                type: 'POST',
                datatype: 'json',
                data: $('#kt_search_form').serializeObject()
            },
            columns: [
                { data: 'nIdUsuario', name: 'nIdUsuario', 'autoWidth': true, class: 'text-center' },
                { data: 'cCodigoUsuario', 'autoWidth': true, class: 'text-left' },
                { data: 'cNombreUsuario', 'autoWidth': true, class: 'text-left' },
                { data: 'cFechaRegistro', 'autoWidth': true, class: 'text-center' },
                //{
                //    data: 'dFechaCreacion', 'autoWidth': true, class: 'text-center', render: function (value) {
                //        return moment(value).format('DD/MM/YYYY');
                //    }
                //},
                { data: 'cFechaCese', 'autoWidth': true, class: 'text-center' },
                { data: 'cCorreo', 'autoWidth': true, class: 'text-center' },
                { data: 'cNombrePais', 'autoWidth': true, class: 'text-center' },
                { data: 'cNombreRol', 'autoWidth': true, class: 'text-center' },
                { data: 'cActivo', 'autoWidth': true, class: 'text-center' },

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
                        //if (data.nEstado == '0') {
                        //    buttonAction += ``;
                        //} // else if (data.nEditar == '10' || data.nEstado == '5')
                        //else if (data.nEditar > 0) {
                        //    buttonAction += `<a href="javascript:;" class="btn btn-icon btn-light-dark btn-sm open-modal p-eva" data-bs-toggle="modal" data-bs-target="#kt_modal_evaluacion_operacion" data-n-operacion=${data.nIdOperaciones} title="Evaluar"><i class="las la-check-square fs-2"></i></a>

                        //        <a href="${globalPath}Operacion/Detalle?operacionId=${data.nIdOperaciones}" class="btn btn-sm btn-icon btn-light btn-active-light-primary detail-row p-con"><i class="las la-search fs-2"></i></a>
                        //        <button data-delete-table="delete_row" data-row= ${data.nIdOperaciones}  class="btn btn-sm btn-icon btn-light btn-active-light-primary edit-row me-2 p-eli"><i class="las la-ban fs-2"></i></button> `;
                        //} else {

                        //    /*var _button = `<a href="${globalPath}VentaCartera/Editar?prestamoId=${data.iIdPrestamoVentaCartera}" class="btn btn-sm btn-icon btn-light btn-active-light-primary edit-row me-2"><i class="las la-pen fs-2"></i></a> <a href="javascript:;" class="btn btn-icon btn-light-dark btn-sm p-eva open-modal" data-bs-toggle="modal" data-bs-target="#kt_modal_pago" data-n-pago="1" title="Evaluar"><i class="las la-check-square fs-2"></i></a>`*/
                        //    buttonAction += `<a href="${globalPath}Operacion/Registro?operacionId=${data.nIdOperaciones}" class="btn btn-sm btn-icon btn-light btn-active-light-primary edit-row me-2 p-act" title="Editar"><i class="las la-pen fs-2"></i></a>

                        //        <a href="javascript:;" class="btn btn-icon btn-light-dark btn-sm open-modal p-eva" data-bs-toggle="modal" data-bs-target="#kt_modal_evaluacion_operacion" data-n-operacion=${data.nIdOperaciones} title="Evaluar"><i class="las la-check-square fs-2"></i></a>

                        //        <a href="${globalPath}Operacion/Detalle?operacionId=${data.nIdOperaciones}" class="btn btn-sm btn-icon btn-light btn-active-light-primary detail-row p-con"><i class="las la-search fs-2"></i></a>
                        //        <button data-delete-table="delete_row" data-row= ${data.nIdOperaciones}  class="btn btn-sm btn-icon btn-light btn-active-light-primary edit-row me-2 p-eli"><i class="las la-ban fs-2"></i></button> `;
                        //}
                        if (data.cActivo == 'ACTIVO') {
                            buttonAction += `
                         <div style="display:inline-flex">
                         <a href="${globalPath}Usuario/Registro?usuarioId=${data.nIdUsuario}" title="Editar" class="btn btn-sm btn-icon btn-light btn-active-light-primary edit-row me-2 oculto-acci p-act"><i style="color:blue" class="las la-pen fs-2"></i></a>
                         <button  data-delete-table="delete_row" data-id="` + row.nIdUsuario + `" data-row= ${data.nIdUsuario} title="Inactivar"  class="btn btn-sm btn-icon btn-light btn-active-light-primary edit-row me-2 oculto-acci p-anu"><i style="color:red" class="las la-ban fs-2"></i></button> 
                         <button data-reset-password-table="resete-password_row" data-id="` + row.nIdUsuario + `" data-codigo="` + row.cCodigoUsuario + `" data-row= ${data.nIdUsuario} title="Resetear contraseña"  class="btn btn-sm btn-icon btn-light btn-active-light-primary edit-row me-2 oculto-acci p-rese p-reset-pwd"><i style="color:green" class="las la la-refresh fs-2"></i></button>
                         </div>
                         `
                        } else {
                            buttonAction += `<a href="${globalPath}Usuario/Registro?usuarioId=${data.nIdUsuario}" title="Editar" class="btn btn-sm btn-icon btn-light btn-active-light-primary edit-row me-2 oculto-acci p-act"><i style="color:blue" class="las la-pen fs-2"></i></a>
                         
                         `
                        }
                       
                        //<button data-reset-password-table="resete-password_row" data-id="` + row.nIdUsuario + `" data-codigo="` + row.cCodigoUsuario + `" data-row= ${data.nIdUsuario} title="Resetear contraseña"  class="btn btn-sm btn-icon btn-light btn-active-light-primary edit-row me-2 p-reset-pwd"><i class="las la la-refresh fs-2"></i></button>

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
            //initToggleToolbar();
            //toggleToolbars();
            //handleDeleteOperacionForm();
            var searchButton = document.getElementById('kt_search_button');
            var searchClear = document.getElementById('kt_search_clear');
            searchButton.removeAttribute('data-kt-indicator');
            //handleModalControlEvaluacion();
            searchButton.disabled = false;

            /*   handleAnularEvaluacion2();*/
            handleDeleteUsuarioForm();
            handleResetPwdUsuarioForm();
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
    //var initToggleToolbar = function () {

    //}

    var handleDeleteUsuarioForm = function () {
        var tableUsuario = document.querySelector('#kt_usuarios_table');
        if (!tableUsuario) {
            return;
        }
        var deleteUsuarioButton = tableUsuario.querySelectorAll('[data-delete-table="delete_row"]');
        deleteUsuarioButton.forEach(d => {
            d.addEventListener('click', function (e) {
                e.preventDefault();
                debugger;
                //var idFondeador = $(this).data('parent');
                var idUsuario = $(this).data('id');
                var parent = e.target.closest('tr');
                var name = parent.querySelectorAll('td')[0].innerText + ' ' + parent.querySelectorAll('td')[2].innerText;
                Swal.fire({
                    text: '¿Estás seguro de que quieres inactivar a ' + name + '?',
                    icon: 'warning',
                    showCancelButton: true,
                    buttonsStyling: false,
                    confirmButtonText: 'Sí, inactivar!',
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
                            url: globalPath + 'Usuario/EliminarUsuario',
                            data: {
                                usuarioId: idUsuario
                            },
                            headers: {
                                'RequestVerificationToken': token
                            },
                            success: function (data) {
                                if (data) {
                                    Swal.fire({
                                        text: 'Inactivaste correctamente a ' + name + '.',
                                        icon: 'success',
                                        buttonsStyling: false,
                                        confirmButtonText: 'Listo',
                                        customClass: {
                                            confirmButton: 'btn fw-bold btn-primary',
                                        }
                                    }).then(function () {
                                        //datatableFondeo.row($(parent)).remove().draw();
                                        initDatatable();
                                    });
                                } else {
                                    //messageError(name + ' no fue inactivado.');
                                }
                            },
                            error: function (jqXHR, textStatus, errorThrown) {
                                messageError(errorThrown);
                            }
                        });
                    } else if (result.dismiss === 'cancel') {
                        //messageError(name + ' no fue inactivado.');
                    }
                });
            });
        });
    }

    var handleResetPwdUsuarioForm = function () {
        var tableUsuario = document.querySelector('#kt_usuarios_table');
        if (!tableUsuario) {
            return;
        }
        var resetPwdUsuarioButton = tableUsuario.querySelectorAll('[data-reset-password-table="resete-password_row"]');
        resetPwdUsuarioButton.forEach(d => {
            d.addEventListener('click', function (e) {
                e.preventDefault();
                debugger;
                //var idFondeador = $(this).data('parent');
                var idUsuario = $(this).data('id');
                var codigoUsuario = $(this).data('codigo');
                var parent = e.target.closest('tr');
                var name = parent.querySelectorAll('td')[0].innerText + ' ' + parent.querySelectorAll('td')[2].innerText;
                Swal.fire({
                    text: '¿Estás seguro de resetear la contraseña de ' + name + '?',
                    icon: 'warning',
                    showCancelButton: true,
                    buttonsStyling: false,
                    confirmButtonText: 'Sí, resetear!',
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
                            url: globalPath + 'Account/ResetPassword',
                            data: {
                                IdUsuario: idUsuario,
                                CodigoUsuario: codigoUsuario
                            },
                            headers: {
                                'RequestVerificationToken': token
                            },
                            success: function (data) {
                                if (data) {
                                    Swal.fire({
                                        html: data.message,
                                        icon: 'success',
                                        buttonsStyling: false,
                                        confirmButtonText: 'Listo',
                                        customClass: {
                                            confirmButton: 'btn fw-bold btn-primary',
                                        }
                                    }).then(function () {
                                        //datatableFondeo.row($(parent)).remove().draw();
                                        //initDatatable();
                                    });
                                } else {
                                    //messageError(name + ' no fue reseteado.');
                                }
                            },
                            error: function (jqXHR, textStatus, errorThrown) {
                                messageError(errorThrown);
                            }
                        });
                    } else if (result.dismiss === 'cancel') {
                        //messageError(name + ' no fue reseteado.');
                    }
                });
            });
        });
    }

    var handleFilterTable = function () {
        var searchButton = document.getElementById('kt_search_button');
        if (!searchButton) {
            return;
        }


        //$('#FechaCreacion').flatpickr({
        //    dateFormat: 'd/m/Y',
        //    defaultDate: 'today'
        //});

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
            $("#IdEstado").val(-1).trigger('change');
            $("#IdPais").val(0).trigger('change');
            initDatatable();
        });
    }

    var handleAddForm = function () {
       
        var formAdd = document.getElementById('kt_form_add');
        if (!formAdd) {
            return;
        }
        var saveButton = document.getElementById('kt_save_button');
        var validator;
        //$('#IdFondeadorCabecera').val($('#IdFondeador').val());

        validator = FormValidation.formValidation(
            formAdd,
            {
                fields: {

                    'IdPais': {
                        validators: {
                            notEmpty: {
                                message: 'Pais es obligatorio'
                            },
                            //stringLength: {
                            //    min: 11,
                            //    max: 11,
                            //    message: 'RUC debe tener 11 dígitos'
                            //},
                            //digits: {
                            //    message: 'Ingresar sólo números'
                            //}
                        }
                    },
                    'IdRol': {
                        validators: {
                            notEmpty: {
                                message: 'Rol es obligatorio'
                            },
                            //stringLength: {
                            //    min: 11,
                            //    max: 11,
                            //    message: 'RUC debe tener 11 dígitos'
                            //},
                            //digits: {
                            //    message: 'Ingresar sólo números'
                            //}
                        }
                    },
                    'Ruc': {
                        validators: {
                            notEmpty: {
                                message: 'Ruc es obligatorio'
                            },
                            stringLength: {
                                min: 11,
                                max: 11,
                                message: 'RUC debe tener 11 dígitos'
                            },
                            digits: {
                                message: 'Ingresar sólo números'
                            }
                        }
                    },

                    'RazonSocial': {
                        validators: {
                            notEmpty: {
                                message: 'Razon Social es obligatorio'
                            },
                            //stringLength: {
                            //    min: 11,
                            //    max: 11,
                            //    message: 'RUC debe tener 11 dígitos'
                            //},
                            //digits: {
                            //    message: 'Ingresar sólo números'
                            //}
                        }
                    },

                    'IdTipoDocumento': {
                        validators: {
                            notEmpty: {
                                message: 'Tipo de Documento es obligatorio'
                            },
                            //stringLength: {
                            //    min: 11,
                            //    max: 11,
                            //    message: 'RUC debe tener 11 dígitos'
                            //},
                            //digits: {
                            //    message: 'Ingresar sólo números'
                            //}
                        }
                    },

                    'NumeroDocumento': {
                        validators: {
                            notEmpty: {
                                message: 'N° de Documento es obligatorio'
                            },
                            //stringLength: {
                            //    min: 11,
                            //    max: 11,
                            //    message: 'RUC debe tener 11 dígitos'
                            //},
                            //digits: {
                            //    message: 'Ingresar sólo números'
                            //}
                        }
                    },

                    'Cargo': {
                        validators: {
                            notEmpty: {
                                message: 'Cargo es obligatorio'
                            },
                            //stringLength: {
                            //    min: 11,
                            //    max: 11,
                            //    message: 'RUC debe tener 11 dígitos'
                            //},
                            //digits: {
                            //    message: 'Ingresar sólo números'
                            //}
                        }
                    },

                    'Telefono': {
                        validators: {
                            //notEmpty: {
                            //    message: 'Telefono es obligatorio'
                            //},
                            //stringLength: {
                            //    min: 11,
                            //    max: 11,
                            //    message: 'RUC debe tener 11 dígitos'
                            //},
                            digits: {
                                message: 'Ingresar sólo números'
                            }
                        }
                    },

                    'Celular': {
                        validators: {
                            notEmpty: {
                                message: 'Celular es obligatorio'
                            },
                            //stringLength: {
                            //    min: 11,
                            //    max: 11,
                            //    message: 'RUC debe tener 11 dígitos'
                            //},
                            digits: {
                                message: 'Ingresar sólo números'
                            }
                        }
                    },
                    'CodigoUsuario': {
                        validators: {
                            notEmpty: {
                                message: 'Codigo es obligatorio'
                            },
                            //stringLength: {
                            //    min: 5,
                            //    max: 200,
                            //    message: 'Razón Social debe tener entre 5 y 200 caracteres'
                            //}
                        }
                    },
                    'NombreUsuario': {
                        validators: {
                            notEmpty: {
                                message: 'Nombre es obligatorio'
                            },
                            //stringLength: {
                            //    min: 5,
                            //    max: 200,
                            //    message: 'Razón Social debe tener entre 5 y 200 caracteres'
                            //}
                        }
                    },
                    'Correo': {
                        validators: {
                            notEmpty: {
                                message: 'Correo es obligatorio'
                            },
                            emailAddress: {
                                message: 'Correo electrónico inválido.',
                            },
                            //stringLength: {
                            //    min: 5,
                            //    max: 200,
                            //    message: 'Razón Social debe tener entre 5 y 200 caracteres'
                            //}
                        }
                    },
                    'Contrasena': {
                        validators: {
                            notEmpty: {
                                message: 'Contraseña es obligatorio'
                            },
                            //stringLength: {
                            //    min: 5,
                            //    max: 200,
                            //    message: 'Razón Social debe tener entre 5 y 200 caracteres'
                            //}
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
                    var IdUsuario = document.getElementById('IdUsuario');
                    var urlAction = $(formAdd).attr('action');
                    if (IdUsuario) {
                        urlAction += '?usuarioId=' + $(IdUsuario).val();
                    }

                    $.ajax({
                        type: 'POST',
                        dataType: 'json',
                        //url: $(formRegistroFondeador).attr('action'),
                        url: urlAction,

                        xhrFields: {
                            withCredentials: true
                        },
                        data: $(formAdd).serializeObject(),
                        success: function (data) {
                            saveButton.removeAttribute('data-kt-indicator');
                            saveButton.disabled = false;
                            if (data.succeeded) {
                                console.log(data)
                                Swal.fire({
                                    html: data.message,
                                    icon: 'success',
                                    buttonsStyling: false,
                                    confirmButtonText: 'Listo',
                                    customClass: {
                                        confirmButton: 'btn fw-bold btn-primary',
                                    }
                                }).then(function () {
                                    $(window).attr('location', globalPath + 'Usuario/Registro?usuarioId=' + data.data);
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

        //saveButton.addEventListener('click', function (e) {
           
        //    e.preventDefault();
        //    validator.validate().then(function (status) {
        //        if (status == 'Valid') {
        //            saveButton.setAttribute('data-kt-indicator', 'on');
        //            saveButton.disabled = true;
        //            setTimeout(function () {
        //                $.ajax({
        //                    type: 'POST',
        //                    dataType: 'json',
        //                    url: $(formAdd).attr('action'),
        //                    data: $(formAdd).serializeObject(),
        //                    xhrFields: {
        //                        withCredentials: true
        //                    },
        //                    success: function (data) {
        //                        if (data.succeeded) {
        //                            Swal.fire({
        //                                text: 'Usuario creado correctamente.' ,//data.message,
        //                                icon: 'success',
        //                                buttonsStyling: false,
        //                                confirmButtonText: 'Listo',
        //                                customClass: {
        //                                    confirmButton: 'btn btn-primary'
        //                                }
        //                            }).then(function (result) {
        //                                if (result.isConfirmed) {
        //                                    //$(saveButton).closest('form').find('input[type=text], textarea').val('');
        //                                    //saveButton.removeAttribute('data-kt-indicator');
        //                                    //saveButton.disabled = false;
        //                                    //initDatatable();
        //                                    $(window).attr('location', globalPath + 'Usuario/Registro?UsuarioId=' + data.data);
        //                                }
        //                            });
        //                        } else {
        //                            saveButton.removeAttribute('data-kt-indicator');
        //                            saveButton.disabled = false;
        //                            messageError(data.message);
        //                        }
        //                    },
        //                    error: function (jqXHR, textStatus, errorThrown) {
        //                        saveButton.removeAttribute('data-kt-indicator');
        //                        saveButton.disabled = false;
        //                        messageError(errorThrown);
        //                    }
        //                });
        //            }, 2000);
        //        }
        //        //else {
        //        //    messageError('Lo sentimos, parece que se han detectado algunos errores. Vuelve a intentarlo.');
        //        //}
        //    });
        //});
    }

    return {
        init: function () {
            initDatatable();
            handleFilterTable();
            handleAddForm();
            handleDeleteUsuarioForm();
            handleResetPwdUsuarioForm()
        }
    }
}();
KTUtil.onDOMContentLoaded(function () {
    RegistroUsuario.init();
});