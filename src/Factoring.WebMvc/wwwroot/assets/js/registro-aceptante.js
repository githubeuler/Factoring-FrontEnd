'use strict';
var globalPath = $('base').attr('href');
var RegistroAceptante = function () {
    var datatable;
    var datatableContactoAceptante;
    var datatableUbicaciones;
    var datatableDocumentos;

    $('input[type=checkbox]').on('change', function () {
        if ($(this).is(':checked')) {
            console.log("Checkbox " + $(this).prop("id") + " (" + $(this).val() + ") => Seleccionado");
            $('#nValorSeleccionado').val(1);
        } else {
            console.log("Checkbox " + $(this).prop("id") + " (" + $(this).val() + ") => Deseleccionado");
            $('#nValorSeleccionado').val(0);
        }
    });
    var handleFilterTable = function () {

        $('#FechaInicioActividades').flatpickr({
            dateFormat: 'd/m/Y'
        });

        $('#FechaFirmaContrato').flatpickr({
            dateFormat: 'd/m/Y'
        });

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
            initDatatable();
        });
    }
    var initDatatable = function () {
        var table = document.getElementById('kt_aceptantes_table');
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
                url: globalPath + 'Aceptante/GetAceptanteAllList',
                type: 'POST',
                datatype: 'json',
                data: $('#kt_search_form').serializeObject()
            },
            columns: [
                { data: 'nIdAdquiriente', name: 'nIdAdquiriente', 'autoWidth': true, class: 'text-center' },
                { data: 'cRegUnicoEmpresa', 'autoWidth': true, class: 'text-left' },
                { data: 'cRazonSocial', 'autoWidth': true, class: 'text-left' },
                {
                    data: 'dFechaCreacion', 'autoWidth': true, class: 'text-center', render: function (value) {
                        return moment(value).format('DD/MM/YYYY');
                    }
                },
                { data: 'nombreEstado', 'autoWidth': true, class: 'text-center' },
                { data: null, 'width': '15%', class: 'text-center', responsivePriority: -1 }
            ],
            columnDefs: [
                {
                    targets: 0,
                    render: function (data, type, row) {
                        var checkbox = ((row.nEstado == '1') ? `<div class="form-check form-check-sm form-check-custom form-check-solid"><input class="form-check-input checkbox-main" type="checkbox" value="${data}" /></div>` : ``);
                        return checkbox;
                    }
                },
                {
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
                                <a href="` + globalPath + `Aceptante/Registro?adquirienteId=` + data.nIdAdquiriente + `" class="menu-link px-3">Editar</a>
                            </div>
                            <div class="menu-item px-3 oculto-acci p-con">
                                <a href="` + globalPath + `Aceptante/Detalle?adquirienteId=` + data.nIdAdquiriente + `" class="menu-link px-3">Detalle</a>
                            </div>
                        </div>`;
                    }
                }
            ]
        });
        table = datatable.$;
        datatable.on('draw', function () {
            //initToggleToolbar();
            toggleToolbars();
            KTMenu.createInstances();
            Common.init();
        });
    }
    //var initToggleToolbar = function () {
    //    var container = document.querySelector('#kt_giradores_table');
    //    var checkboxes = container.querySelectorAll('[type="checkbox"]');
    //    var deleteSelected = document.querySelector('[data-kt-girador-table-select="delete_selected"]');
    //    checkboxes.forEach(c => {
    //        c.addEventListener('click', function () {
    //            setTimeout(function () {
    //                toggleToolbars();
    //            }, 50);
    //        });
    //    });
    //    deleteSelected.addEventListener('click', function () {
    //        Swal.fire({
    //            text: '¿Está seguro de que desea eliminar los registros seleccionados?',
    //            icon: 'warning',
    //            showCancelButton: true,
    //            buttonsStyling: false,
    //            showLoaderOnConfirm: true,
    //            confirmButtonText: 'Eliminar',
    //            cancelButtonText: 'Cancelar',
    //            customClass: {
    //                confirmButton: 'btn fw-bold btn-danger',
    //                cancelButton: 'btn fw-bold btn-active-light-primary'
    //            },
    //        }).then(function (result) {
    //            if (result.value) {
    //                var arrCheckBox = $.map($('input.checkbox-main:checkbox:checked'), function (e, i) {
    //                    return +e.value;
    //                });
    //                $.ajax({
    //                    type: 'POST',
    //                    dataType: 'json',
    //                    url: globalPath + 'Aceptante/DeleteGirador',
    //                    data: {
    //                        selectedGirador: arrCheckBox
    //                    },
    //                    beforeSend: function () {
    //                        var timerInterval;
    //                        Swal.fire({
    //                            title: 'Espere un momento',
    //                            html: 'Se estan eliminando los registros en <b></b> milisegundos...',
    //                            timer: 2000,
    //                            timerProgressBar: true,
    //                            didOpen: () => {
    //                                Swal.showLoading()
    //                                var b = Swal.getHtmlContainer().querySelector('b')
    //                                timerInterval = setInterval(() => {
    //                                    b.textContent = Swal.getTimerLeft()
    //                                }, 100)
    //                            },
    //                            willClose: () => {
    //                                clearInterval(timerInterval)
    //                            }
    //                        })
    //                    },
    //                    success: function (data) {
    //                        if (data.succeeded) {
    //                            Swal.fire({
    //                                text: data.message,
    //                                icon: 'success',
    //                                buttonsStyling: false,
    //                                confirmButtonText: 'Listo',
    //                                customClass: {
    //                                    confirmButton: 'btn fw-bold btn-primary',
    //                                }
    //                            }).then(function () {
    //                                datatable.draw();
    //                            });
    //                            var headerCheckbox = container.querySelectorAll('[type="checkbox"]')[0];
    //                            headerCheckbox.checked = false;
    //                        }
    //                    }
    //                });
    //            } else if (result.dismiss === 'cancel') {
    //                Swal.fire({
    //                    text: 'Los registros seleccionados no fueron eliminados.',
    //                    icon: 'error',
    //                    buttonsStyling: false,
    //                    confirmButtonText: 'Ok',
    //                    customClass: {
    //                        confirmButton: 'btn fw-bold btn-primary',
    //                    }
    //                });
    //            }
    //        });
    //    });
    //}
    var toggleToolbars = function () {
        var container = document.querySelector('#kt_aceptantes_table');
        var toolbarBase = document.querySelector('[data-kt-aceptante-table-toolbar="base"]');
        var toolbarSelected = document.querySelector('[data-kt-aceptante-table-toolbar="selected"]');
        var selectedCount = document.querySelector('[data-kt-aceptante-table-select="selected_count"]');
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
            toolbarSelected.classList.add('d-none');
        }
    }
    var handleRegisterForm = function (e) {
        var form = document.getElementById('kt_form_registro');
        if (!form) {
            return;
        }
        var saveButton = document.getElementById('kt_save_button');
        var validator;
        validator = FormValidation.formValidation(
            form,
            {
                fields: {

                    'RegUnicoEmpresa': {
                        validators: {
                            notEmpty: {
                                message: 'RUC es obligatorio'
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
                                message: 'Razón Social es obligatorio'
                            },
                            stringLength: {
                                min: 5,
                                max: 200,
                                message: 'Razón Social debe tener entre 5 y 200 caracteres'
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
        if (saveButton) {
            saveButton.addEventListener('click', function (e) {
                e.preventDefault();
                validator.validate().then(function (status) {
                    if (status == 'Valid') {
                        saveButton.setAttribute('data-kt-indicator', 'on');
                        saveButton.disabled = true;
                        var idAceptante = document.getElementById('IdAdquiriente');
                        var urlAction = $(form).attr('action');
                        if (idAceptante) {
                            urlAction += '?adquirienteId=' + $(idAceptante).val();
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
                                                $(window).attr('location', globalPath + 'Aceptante/Registro?adquirienteId=' + data.data);
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

    }
    var handleRevalidateFormElement = function (form, element, validator) {
        $(form.querySelector('[name="' + element + '"]')).on('change', function () {
            validator.revalidateField(element);
        });
    }
    var initDataTableContacto = function () {
        var idAceptante = document.getElementById('IdAdquiriente');
        if (!idAceptante) {
            return;
        }
        var tableContacto = document.getElementById('kt_aceptantes_contacto_table');
        if (!tableContacto) {
            return;
        }
        var tableContactoAction = $('#Action').val();
        $(tableContacto).DataTable({ ordering: false }).clear().destroy();
        $.fn.dataTable.ext.errMode = 'none';
        datatableContactoAceptante = $(tableContacto).DataTable({
            ordering: false,
            ajax: {
                type: 'GET',
                dataType: 'json',
                url: globalPath + 'Aceptante/GetAllContactos?aceptanteId=' + $(idAceptante).val(),
                dataSrc: function (data) {
                    return data.data;
                }
            },
            columns: [
                { data: 'cNombre', 'autoWidth': true, class: 'text-center' },
                { data: 'cApellidoPaterno', 'autoWidth': true, class: 'text-center' },
                { data: 'cCelular', 'autoWidth': true, class: 'text-center' },
                { data: 'cEmail', 'autoWidth': true, class: 'text-center' },
                { data: 'nombreTipoContacto', 'autoWidth': true, class: 'text-center' },
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
                        return `<a href="javascript:;" class="btn btn-icon btn-light-dark btn-sm oculto-acci p-eli" data-kt-contact-table-filter="delete_row" data-parent="` + $(idAceptante).val() + `" data-id="` + data.nIdAdquirienteContacto + `"><i class="las la-trash fs-2"></i></a>`;
                    }
                }
            ]
        });
        datatableContactoAceptante.on('draw', function () {
            handleDeleteContactoForm();
            Common.init();
        });
    }
    var handleAddContactoForm = function () {
        var formAddContacto = document.getElementById('kt_form_add_contacto');
        if (!formAddContacto) {
            return;
        }
        var addButton = document.getElementById('kt_add_contacto');
        var validator;
        $('#IdAdquirienteCabecera').val($('#IdAdquiriente').val());
        validator = FormValidation.formValidation(
            formAddContacto,
            {
                fields: {
                    'Nombre': {
                        validators: {
                            notEmpty: {
                                message: 'Nombre es obligatorio'
                            },
                            stringLength: {
                                min: 3,
                                max: 100,
                                message: 'Nombre debe tener entre 3 y 100 caracteres'
                            }
                        }
                    },
                    'ApellidoPaterno': {
                        validators: {
                            notEmpty: {
                                message: 'Apellido Paterno es obligatorio'
                            },
                            stringLength: {
                                min: 3,
                                max: 100,
                                message: 'Apellido Paterno debe tener entre 3 y 100 caracteres'
                            }
                        }
                    },
                    'ApellidoMaterno': {
                        validators: {
                            notEmpty: {
                                message: 'Apellido Materno es obligatorio'
                            },
                            stringLength: {
                                min: 3,
                                max: 100,
                                message: 'Apellido Materno debe tener entre 3 y 100 caracteres'
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
                                message: 'Celular debe tener 9 números'
                            }
                        }
                    },
                    'Email': {
                        validators: {
                            notEmpty: {
                                message: 'Email es obligatorio'
                            },
                            emailAddress: {
                                message: 'Ingresar un email válido',
                            }
                        }
                    },
                    'TipoContacto': {
                        validators: {
                            notEmpty: {
                                message: 'Tipo Contacto es obligatorio'
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
                            url: $(formAddContacto).attr('action'),
                            data: $(formAddContacto).serializeObject(),
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
                } else {
                    messageError('Lo sentimos, parece que se han detectado algunos errores. Vuelve a intentarlo.');
                }
            });
        });
    }
    var handleDeleteContactoForm = function () {
        var tableAceptanteContacto = document.querySelector('#kt_aceptantes_contacto_table');
        if (!tableAceptanteContacto) {
            return;
        }
        var deleteContactButton = tableAceptanteContacto.querySelectorAll('[data-kt-contact-table-filter="delete_row"]');
        deleteContactButton.forEach(d => {
            d.addEventListener('click', function (e) {
                e.preventDefault();
                var idContact = $(this).data('id');
                var parent = e.target.closest('tr');
                var nameContact = parent.querySelectorAll('td')[0].innerText + ' ' + parent.querySelectorAll('td')[1].innerText;
                Swal.fire({
                    text: '¿Estás seguro de que quieres eliminar a ' + nameContact + '?',
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
                            url: globalPath + 'Aceptante/EliminarContacto',
                            data: {
                                aceptanteContactoId: idContact
                            },
                            headers: {
                                'RequestVerificationToken': token
                            },
                            success: function (data) {
                                if (data) {
                                    Swal.fire({
                                        text: 'Eliminaste correctamente a ' + nameContact + '.',
                                        icon: 'success',
                                        buttonsStyling: false,
                                        confirmButtonText: 'Listo',
                                        customClass: {
                                            confirmButton: 'btn fw-bold btn-primary',
                                        }
                                    }).then(function () {
                                        datatableContactoAceptante.row($(parent)).remove().draw();
                                    });
                                } else {
                                    messageError(nameContact + ' no fue eliminado.');
                                }
                            },
                            error: function (jqXHR, textStatus, errorThrown) {
                                messageError(errorThrown);
                            }
                        });
                    } else if (result.dismiss === 'cancel') {
                        messageError(nameContact + ' no fue eliminado.');
                    }
                });
            });
        });
    }
    var initDataTableUbicaciones = function () {
        var idAceptante = document.getElementById('IdAdquiriente');
        if (!idAceptante) {
            return;
        }
        var tableUbicaciones = document.getElementById('kt_ubicaciones_table');
        if (!tableUbicaciones) {
            return;
        }
        var tableUbicacionAction = $('#Action').val();
        $(tableUbicaciones).DataTable({ ordering: false }).clear().destroy();
        $.fn.dataTable.ext.errMode = 'none';
        datatableUbicaciones = $(tableUbicaciones).DataTable({
            ordering: false,
            ajax: {
                type: 'GET',
                dataType: 'json',
                url: globalPath + 'Aceptante/GetAllUbicaciones?adquirienteId=' + $(idAceptante).val(),
                dataSrc: function (data) {
                    return data.data;
                }
            },
            columns: [
                { data: 'cFormatoUbigeo', 'autoWidth': true, class: 'text-center' },
                { data: 'cDireccion', 'autoWidth': true, class: 'text-center' },
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
                        return `<a href="javascript:;" class="btn btn-icon btn-light-dark btn-sm oculto-acci p-eli" data-kt-ubicacion-table-filter="delete_row" data-parent="` + $(idAceptante).val() + `" data-id="` + data.nIdAdquirienteDireccion + `"><i class="las la-trash fs-2"></i></a>`;
                    }
                }
            ]
        });
        datatableUbicaciones.on('draw', function () {
            handleDeleteUbicacionForm();
            Common.init();
        });
    }
    var handleAddUbicacionForm = function () {
        var formAddUbicacion = document.getElementById('kt_form_add_ubicacion');
        if (!formAddUbicacion) {
            return;
        }
        var addButton = document.getElementById('kt_add_ubicacion');
        var validator;
        $('#IdAdquirienteCabeceraUbicacion').val($('#IdAdquiriente').val());
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
                                            initDataTableUbicaciones();
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
    var handleDeleteUbicacionForm = function () {
        var tableUbicaciones = document.querySelector('#kt_ubicaciones_table');
        if (!tableUbicaciones) {
            return;
        }
        var deleteUbicacionButton = tableUbicaciones.querySelectorAll('[data-kt-ubicacion-table-filter="delete_row"]');
        deleteUbicacionButton.forEach(d => {
            d.addEventListener('click', function (e) {
                e.preventDefault();
                var idUbicacion = $(this).data('id');
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
                            url: globalPath + 'Aceptante/EliminarUbicacion',
                            data: {
                                adquirienteUbicacionId: idUbicacion
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
                                        datatableUbicaciones.row($(parent)).remove().draw();
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
    var handleUbigeo = function (pais, tipo, codigo) {
        var _tipo = tipo + 1;
        var _codigo = $(codigo).val();
        for (var elem = _tipo; elem <= countUbigeo; elem++) {
            $('#' + elem).empty().append('<option value="">Seleccionar</option>');
        }
        $.getJSON(globalPath + 'Aceptante/ListarUbigeos', { pais: pais, tipo: _tipo, codigo: _codigo }).done(function (data) {
            if (data.length > 0) {
                $.each(data, function (key, value) {
                    $('#' + _tipo).append($('<option value="' + value.cCodigo + '">' + value.cDescripcion + '</option>'));
                });
            }
        });
    }
    var initDataTableDocumentos = function () {
        var idAceptante = document.getElementById('IdAdquiriente');
        if (!idAceptante) {
            return;
        }
        var tableDocumentos = document.getElementById('kt_documentos_table');
        if (!tableDocumentos) {
            return;
        }
        var tableDocumentosAction = $('#Action').val();
        $(tableDocumentos).DataTable({ ordering: false }).clear().destroy();
        $.fn.dataTable.ext.errMode = 'none';
        datatableDocumentos = $(tableDocumentos).DataTable({
            ordering: false,
            ajax: {
                type: 'GET',
                dataType: 'json',
                url: globalPath + 'Aceptante/GetAllDocumentos?aceptanteId=' + $(idAceptante).val(),
                dataSrc: function (data) {
                    return data.data;
                }
            },
            columns: [
                { data: 'nombreTipoDocumento', 'autoWidth': true, class: 'text-center' },
                { data: 'cNombreDocumento', 'autoWidth': true, class: 'text-left' },
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
                    visible: true,
                    orderable: false,
                    className: 'text-end',
                    render: function (data, type, row) {
                        var buttonDownload = ((data.cNombreDocumento == null || data.cNombreDocumento == '') ? `` : `<a href="javascript:;" class="btn btn-icon btn-sm btn-outline btn-outline-solid btn-outline-default me-2 oculto-acci p-des" data-kt-documento-table-filter="download_file" data-filename="` + data.cNombreDocumento + `" onclick="RegistroAceptante.fnDownloadDocumentos(` + data.nIdAceptanteDocumento + `)" title="` + data.cNombreDocumento + `" data-id="` + data.nIdAceptanteDocumento + `"><i class="las la-download fs-2"></i></a>`);
                        var buttonDelete = ((tableDocumentosAction == 'Detalle') ? `` : `<a href="javascript:;" class="btn btn-icon btn-light-dark btn-sm  oculto-acci p-eli" data-kt-documento-table-filter="delete_row" data-parent="` + $(idAceptante).val() + `" data-path="` + data.cRuta + `" data-id="` + data.nIdAceptanteDocumento + `"><i class="las la-trash fs-2"></i></a>`);
                        return buttonDownload + buttonDelete;
                    }
                }
            ]
        });
        datatableDocumentos.on('draw', function () {
            handleDeleteDocumentosForm();
            Common.init();
        });
    }
    var handleAddDocumentosForm = function () {
        var formAddDocumentos = document.getElementById('kt_form_add_documentos');
        if (!formAddDocumentos) {
            return;
        }
        var addButton = document.getElementById('kt_add_documento');
        var validator;
        $('#IdAceptanteCabeceraDocumentos').val($('#IdAdquiriente').val());
        validator = FormValidation.formValidation(
            formAddDocumentos,
            {
                fields: {
                    'TipoDocumento': {
                        validators: {
                            notEmpty: {
                                message: 'Tipo Documento es obligatorio'
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
                        var fileDocumento = $('#fileDocumento')[0];
                        var formData = new FormData();
                        formData.append('IdAceptanteCabeceraDocumentos', $('#IdAceptanteCabeceraDocumentos').val());
                        formData.append('TipoDocumento', $('#TipoDocumento').val());
                        formData.append('fileDocumento', fileDocumento.files[0]);
                        $.ajax({
                            type: 'POST',
                            dataType: 'json',
                            url: $(formAddDocumentos).attr('action'),
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
                                            initDataTableDocumentos();
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
    var handleDeleteDocumentosForm = function () {
        var tableAceptanteDocumentos = document.querySelector('#kt_documentos_table');
        if (!tableAceptanteDocumentos) {
            return;
        }
        var deleteDocumentosButton = tableAceptanteDocumentos.querySelectorAll('[data-kt-documento-table-filter="delete_row"]');
        deleteDocumentosButton.forEach(d => {
            d.addEventListener('click', function (e) {
                e.preventDefault();               
                var idDocumento = $(this).data('id');
                var ruta = $(this).data('path');
                var parent = e.target.closest('tr');
                var nameDocumento = parent.querySelectorAll('td')[1].innerText;
                Swal.fire({
                    text: '¿Estás seguro de que quieres eliminar el documento ' + nameDocumento + '?',
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
                            url: globalPath + 'Aceptante/EliminarDocumentos',
                            data: {
                                aceptanteDocumentoId: idDocumento,
                                filePath: ruta
                            },
                            headers: {
                                'RequestVerificationToken': token
                            },
                            success: function (data) {
                                if (data) {
                                    Swal.fire({
                                        text: 'Eliminaste correctamente el documento ' + nameDocumento + '.',
                                        icon: 'success',
                                        buttonsStyling: false,
                                        confirmButtonText: 'Listo',
                                        customClass: {
                                            confirmButton: 'btn fw-bold btn-primary',
                                        }
                                    }).then(function () {
                                        datatableDocumentos.row($(parent)).remove().draw();
                                    });
                                } else {
                                    messageError('El documento ' + nameDocumento + ' no fue eliminado.');
                                }
                            },
                            error: function (jqXHR, textStatus, errorThrown) {
                                messageError(errorThrown);
                            }
                        });
                    } else if (result.dismiss === 'cancel') {
                        messageError('El documento ' + nameDocumento + ' no fue eliminado.');
                    }
                });
            });
        });
    }
    var handleDownloadFile = function (idDocumento, tipoDocumento) {
        window.open(globalPath + 'Aceptante/DownloadFile?idDocumento=' + encodeURIComponent(idDocumento) + '&tipoDocumento=' + encodeURIComponent(tipoDocumento), '_blank');
    }  
 
    return {
        init: function () {
            handleFilterTable();
            initDatatable();
            handleRegisterForm();
            initDataTableContacto();
            handleAddContactoForm();
            handleDeleteContactoForm();
            initDataTableUbicaciones();
            handleAddUbicacionForm();
            handleDeleteUbicacionForm();
            initDataTableDocumentos();
            handleAddDocumentosForm();
            handleDeleteDocumentosForm();;
        },
        getRevalidateFormElement: function (form, elem, val) {
            handleRevalidateFormElement(form, elem, val);
        },
        getUbigeo: function (pais, tipo, codigo) {
            handleUbigeo(pais, tipo, codigo);
        },
        fnDownloadDocumentos: function (idDocumento) {
            handleDownloadFile(idDocumento, 4);
        },

    }
}();
KTUtil.onDOMContentLoaded(function () {
    RegistroAceptante.init();
});