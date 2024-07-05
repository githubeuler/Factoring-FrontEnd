'use strict';
var globalPath = $('base').attr('href');
var RegistroGirador = function () {
    var datatable;
    var datatableContactoGirador;
    var datatableRepresentanteLegal;
    var datatableCuentaBancaria;
    var datatableUbicaciones;
    var datatableDocumentos;
    var datatableLinea;
    var datatableCategoriaGirador;

    //if ($('#bPredeterminado').attr('checked')) {
    //    $('#nValorSeleccionado').val(1);
    //}
    //else {
    //    $('#nValorSeleccionado').val(1);
    //}


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
        var table = document.getElementById('kt_giradores_table');
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
                url: globalPath + 'Girador/GetGiradorAllList',
                type: 'POST',
                datatype: 'json',
                data: $('#kt_search_form').serializeObject()
            },
            columns: [
                { data: 'nIdGirador', name: 'nIdGirador', 'autoWidth': true, class: 'text-center' },
                { data: 'cRegUnicoEmpresa', 'autoWidth': true, class: 'text-left' },
                { data: 'cRazonSocial', 'autoWidth': true, class: 'text-left' },
              /*  { data: 'cNombreSector', 'autoWidth': true, class: 'text-left' },*/
                {
                    data: 'dFechaCreacion', 'autoWidth': true, class: 'text-center', render: function (value) {
                        return moment(value).format('DD/MM/YYYY');
                    }
                },
               /* { data: 'cNombrePais', 'autoWidth': true, class: 'text-left' },*/
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
                    //render: function (data, type, row) {
                    //    return `<a href="javascript:;" class="btn btn-outline btn-outline-solid btn-outline-primary btn-active-light-primary btn-icon-primary btn-sm" data-kt-menu-trigger="click" data-kt-menu-placement="bottom-end" data-kt-menu-flip="top-end">
                    //        Acciones
                    //        <span class="svg-icon svg-icon-5 m-0">
                    //            <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1">
                    //                <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">
                    //                    <polygon points="0 0 24 0 24 24 0 24"></polygon>
                    //                    <path d="M6.70710678,15.7071068 C6.31658249,16.0976311 5.68341751,16.0976311 5.29289322,15.7071068 C4.90236893,15.3165825 4.90236893,14.6834175 5.29289322,14.2928932 L11.2928932,8.29289322 C11.6714722,7.91431428 12.2810586,7.90106866 12.6757246,8.26284586 L18.6757246,13.7628459 C19.0828436,14.1360383 19.1103465,14.7686056 18.7371541,15.1757246 C18.3639617,15.5828436 17.7313944,15.6103465 17.3242754,15.2371541 L12.0300757,10.3841378 L6.70710678,15.7071068 Z" fill="#000000" fill-rule="nonzero" transform="translate(12.000003, 11.999999) rotate(-180.000000) translate(-12.000003, -11.999999)"></path>
                    //                </g>
                    //             </svg>
                    //        </span>
                    //    </a>
                    //    <div class="menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-gray-600 menu-state-bg-light-primary fw-bold fs-7 w-125px py-4" data-kt-menu="true">
                    //        <div class="menu-item px-3 p-act">
                    //            <a href="` + globalPath + `Girador/Registro?giradorId=` + data.nIdGirador + `" class="menu-link px-3">Editar</a>
                    //        </div>
                    //        <div class="menu-item px-3 p-con">
                    //            <a href="` + globalPath + `Girador/Detalle?giradorId=` + data.nIdGirador + `" class="menu-link px-3">Detalle</a>
                    //        </div>
                    //    </div>`;
                    //}

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
                            <div class="menu-item px-3">
                                <a href="` + globalPath + `Girador/Registro?giradorId=` + data.nIdGirador + `" class="menu-link px-3">Editar</a>
                            </div>
                            <div class="menu-item px-3">
                                <a href="` + globalPath + `Girador/Detalle?giradorId=` + data.nIdGirador + `" class="menu-link px-3">Detalle</a>
                            </div>
                        </div>`;
                    }
                }
            ]
        });
        table = datatable.$;
        datatable.on('draw', function () {
            initToggleToolbar();
            toggleToolbars();
            KTMenu.createInstances();
            Common.init();
        });
    }
    var initToggleToolbar = function () {
        var container = document.querySelector('#kt_giradores_table');
        var checkboxes = container.querySelectorAll('[type="checkbox"]');
        var deleteSelected = document.querySelector('[data-kt-girador-table-select="delete_selected"]');
        checkboxes.forEach(c => {
            c.addEventListener('click', function () {
                setTimeout(function () {
                    toggleToolbars();
                }, 50);
            });
        });
        deleteSelected.addEventListener('click', function () {
            Swal.fire({
                text: '¿Está seguro de que desea eliminar los registros seleccionados?',
                icon: 'warning',
                showCancelButton: true,
                buttonsStyling: false,
                showLoaderOnConfirm: true,
                confirmButtonText: 'Eliminar',
                cancelButtonText: 'Cancelar',
                customClass: {
                    confirmButton: 'btn fw-bold btn-danger',
                    cancelButton: 'btn fw-bold btn-active-light-primary'
                },
            }).then(function (result) {
                if (result.value) {
                    var arrCheckBox = $.map($('input.checkbox-main:checkbox:checked'), function (e, i) {
                        return +e.value;
                    });
                    $.ajax({
                        type: 'POST',
                        dataType: 'json',
                        url: globalPath + 'Girador/DeleteGirador',
                        data: {
                            selectedGirador: arrCheckBox
                        },
                        beforeSend: function () {
                            var timerInterval;
                            Swal.fire({
                                title: 'Espere un momento',
                                html: 'Se estan eliminando los registros en <b></b> milisegundos...',
                                timer: 2000,
                                timerProgressBar: true,
                                didOpen: () => {
                                    Swal.showLoading()
                                    var b = Swal.getHtmlContainer().querySelector('b')
                                    timerInterval = setInterval(() => {
                                        b.textContent = Swal.getTimerLeft()
                                    }, 100)
                                },
                                willClose: () => {
                                    clearInterval(timerInterval)
                                }
                            })
                        },
                        success: function (data) {
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
                                    datatable.draw();
                                });
                                var headerCheckbox = container.querySelectorAll('[type="checkbox"]')[0];
                                headerCheckbox.checked = false;
                            }
                        }
                    });
                } else if (result.dismiss === 'cancel') {
                    Swal.fire({
                        text: 'Los registros seleccionados no fueron eliminados.',
                        icon: 'error',
                        buttonsStyling: false,
                        confirmButtonText: 'Ok',
                        customClass: {
                            confirmButton: 'btn fw-bold btn-primary',
                        }
                    });
                }
            });
        });
    }
    var toggleToolbars = function () {
        var container = document.querySelector('#kt_giradores_table');
        var toolbarBase = document.querySelector('[data-kt-girador-table-toolbar="base"]');
        var toolbarSelected = document.querySelector('[data-kt-girador-table-toolbar="selected"]');
        var selectedCount = document.querySelector('[data-kt-girador-table-select="selected_count"]');
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
        //RegistroGirador.getRevalidateFormElement(form, 'Estado', validator);
        //RegistroGirador.getRevalidateFormElement(form, 'Pais', validator);
        //RegistroGirador.getRevalidateFormElement(form, 'Sector', validator);
        //RegistroGirador.getRevalidateFormElement(form, 'GrupoEconomico', validator);
        if (saveButton) {
            saveButton.addEventListener('click', function (e) {
                e.preventDefault();
                validator.validate().then(function (status) {
                    if (status == 'Valid') {
                        saveButton.setAttribute('data-kt-indicator', 'on');
                        saveButton.disabled = true;
                        var idGirador = document.getElementById('IdGirador');
                        var urlAction = $(form).attr('action');
                        if (idGirador) {
                            urlAction += '?giradorId=' + $(idGirador).val();
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
                                                $(window).attr('location', globalPath + 'Girador/Registro?giradorId=' + data.data);
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
        var idGirador = document.getElementById('IdGirador');
        if (!idGirador) {
            return;
        }
        var tableContacto = document.getElementById('kt_giradores_contacto_table');
        if (!tableContacto) {
            return;
        }
        var tableContactoAction = $('#Action').val();
        $(tableContacto).DataTable({ ordering: false }).clear().destroy();
        $.fn.dataTable.ext.errMode = 'none';
        datatableContactoGirador = $(tableContacto).DataTable({
            ordering: false,
            ajax: {
                type: 'GET',
                dataType: 'json',
                url: globalPath + 'Girador/GetAllContactos?giradorId=' + $(idGirador).val(),
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
                        return `<a href="javascript:;" class="btn btn-icon btn-light-dark btn-sm p-eli" data-kt-contact-table-filter="delete_row" data-parent="` + $(idGirador).val() + `" data-id="` + data.nIdGiradorContacto + `"><i class="las la-trash fs-2"></i></a>`;
                    }
                }
            ]
        });
        datatableContactoGirador.on('draw', function () {
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
        $('#IdGiradorCabecera').val($('#IdGirador').val());
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
        var tableGiradorContacto = document.querySelector('#kt_giradores_contacto_table');
        if (!tableGiradorContacto) {
            return;
        }
        var deleteContactButton = tableGiradorContacto.querySelectorAll('[data-kt-contact-table-filter="delete_row"]');
        deleteContactButton.forEach(d => {
            d.addEventListener('click', function (e) {
                e.preventDefault();
                var idGirador = $(this).data('parent');
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
                            url: globalPath + 'Girador/EliminarContacto',
                            data: {
                                giradorContactoId: idContact
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
                                        datatableContactoGirador.row($(parent)).remove().draw();
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
    var initDataTableRepresentanteLegal = function () {
        var idGirador = document.getElementById('IdGirador');
        if (!idGirador) {
            return;
        }
        var tableRepresentanteLegal = document.getElementById('kt_representante_legal_table');
        if (!tableRepresentanteLegal) {
            return;
        }
        var tableRepresentanteAction = $('#Action').val();
        $(tableRepresentanteLegal).DataTable({ ordering: false }).clear().destroy();
        $.fn.dataTable.ext.errMode = 'none';
        datatableRepresentanteLegal = $(tableRepresentanteLegal).DataTable({
            ordering: false,
            ajax: {
                type: 'GET',
                dataType: 'json',
                url: globalPath + 'Girador/GetAllRepresentanteLegal?giradorId=' + $(idGirador).val(),
                dataSrc: function (data) {
                    return data.data;
                }
            },
            columns: [
                { data: 'cNombre', 'autoWidth': true, class: 'text-center' },
                { data: 'cApellidoPaterno', 'autoWidth': true, class: 'text-center' },
                { data: 'cCelular', 'autoWidth': true, class: 'text-center' },
                { data: 'cEmail', 'autoWidth': true, class: 'text-center' },
                { data: 'cNombreDocumento', 'autoWidth': true, class: 'text-center' },
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
                        console.log('GetAllRepresentanteLegal')
                        console.log(data)
                        var buttonDownload = ((data.cNombreDocumento == null || data.cNombreDocumento == '') ? `` : `<a href="javascript:;" class="btn btn-icon btn-sm btn-outline btn-outline-solid btn-outline-default me-2 p-des" data-kt-representante-table-filter="download_file" data-filename="` + data.cNombreDocumento + `" onclick="RegistroGirador.fnDownloadLegal(` + data.nIdGiradorRepresentanteLegal + `)" title="` + data.cNombreDocumento + `" data-id="` + data.nIdGiradorRepresentanteLegal + `"><i class="las la-download fs-2"></i></a>`);
                        var buttonDelete = ((tableRepresentanteAction == 'Detalle') ? `` : `<a href="javascript:;" class="btn btn-icon btn-light-dark btn-sm p-eli" data-kt-representante-table-filter="delete_row" data-parent="` + $(idGirador).val() + `" data-path="` + data.cRuta + `" data-id="` + data.nIdGiradorRepresentanteLegal + `"><i class="las la-trash fs-2"></i></a>`);
                        return buttonDownload + buttonDelete;
                    }
                }
            ]
        });
        datatableRepresentanteLegal.on('draw', function () {
            handleDeleteRepresentanteLegalForm();
            Common.init();
        });
    }
    var handleAddRepresentanteLegalForm = function () {
        var formAddRepresentanteLegal = document.getElementById('kt_form_add_representante');
        if (!formAddRepresentanteLegal) {
            return;
        }
        var addButton = document.getElementById('kt_add_representante');
        var validator;
        $('#IdGiradorCabeceraRepresentante').val($('#IdGirador').val());
        validator = FormValidation.formValidation(
            formAddRepresentanteLegal,
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
                    'PoderLegal': {
                        validators: {
                            notEmpty: {
                                message: 'Poder Legal es obligatorio'
                            },
                            file: {
                                extension: 'pdf',
                                type: 'application/pdf',
                                message: 'Debe adjuntar un archivo PDF',
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
                        var filePoderLegal = $('#PoderLegal')[0];
                        var formData = new FormData();
                        formData.append('IdGiradorCabeceraRepresentante', $('#IdGiradorCabeceraRepresentante').val());
                        formData.append('Nombre', $('#Nombre').val());
                        formData.append('ApellidoPaterno', $('#ApellidoPaterno').val());
                        formData.append('ApellidoMaterno', $('#ApellidoMaterno').val());
                        formData.append('Celular', $('#Celular').val());
                        formData.append('Email', $('#Email').val());
                        formData.append('poderLegal', filePoderLegal.files[0]);
                        $.ajax({
                            type: 'POST',
                            dataType: 'json',
                            url: $(formAddRepresentanteLegal).attr('action'),
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
                                            $(window).attr('location', globalPath + 'Girador/Registro?giradorId=' + data.data);
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
    var handleDownloadRepresentanteLegal = function () {
        var tableGiradorRepresentante = document.querySelector('#kt_representante_legal_table');
        if (!tableGiradorRepresentante) {
            return;
        }
        var downloadRepresentanteButton = tableGiradorRepresentante.querySelectorAll('[data-kt-representante-table-filter="download_file"]');
        downloadRepresentanteButton.forEach(d => {
            d.addEventListener('click', function (e) {
                e.preventDefault();
                //var filename = $(this).data('filename');
                //var filename = '\\pafactoring\\Girador\\PoderLegal\\' + filename;
                var idDocumento = $(this).data('id');
                RegistroGirador.getDownloadFile(idDocumento, 1);
            });
        });
    }
    var handleDeleteRepresentanteLegalForm = function () {
        var tableGiradorRepresentante = document.querySelector('#kt_representante_legal_table');
        if (!tableGiradorRepresentante) {
            return;
        }
        var deleteRepresentanteButton = tableGiradorRepresentante.querySelectorAll('[data-kt-representante-table-filter="delete_row"]');
        deleteRepresentanteButton.forEach(d => {
            d.addEventListener('click', function (e) {
                e.preventDefault();
                var idGirador = $(this).data('parent');
                var idRepresentante = $(this).data('id');
                var ruta = $(this).data('path');
                var parent = e.target.closest('tr');
                var nameRepresentante = parent.querySelectorAll('td')[0].innerText + ' ' + parent.querySelectorAll('td')[1].innerText;
                Swal.fire({
                    text: '¿Estás seguro de que quieres eliminar a ' + nameRepresentante + '?',
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
                            url: globalPath + 'Girador/EliminarRepresentante',
                            data: {
                                giradorRepresentanteId: idRepresentante,
                                filePath : ruta
                            },
                            headers: {
                                'RequestVerificationToken': token
                            },
                            success: function (data) {
                                if (data) {
                                    Swal.fire({
                                        text: 'Eliminaste correctamente a ' + nameRepresentante + '.',
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
                                    messageError(nameRepresentante + ' no fue eliminado.');
                                }
                            },
                            error: function (jqXHR, textStatus, errorThrown) {
                                messageError(errorThrown);
                            }
                        });
                    } else if (result.dismiss === 'cancel') {
                        messageError(nameRepresentante + ' no fue eliminado.');
                    }
                });
            });
        });
    }
    var initDataTableCuentaBancaria = function () {
        var idGirador = document.getElementById('IdGirador');
        if (!idGirador) {
            return;
        }
        var tableCuentaBancaria = document.getElementById('kt_cuenta_bancaria_table');
        if (!tableCuentaBancaria) {
            return;
        }
        var tableCuentaAction = $('#Action').val();
        $(tableCuentaBancaria).DataTable({ ordering: false }).clear().destroy();
        $.fn.dataTable.ext.errMode = 'none';
        datatableCuentaBancaria = $(tableCuentaBancaria).DataTable({
            ordering: false,
            ajax: {
                type: 'GET',
                dataType: 'json',
                url: globalPath + 'Girador/GetAllCuentasBancarias?giradorId=' + $(idGirador).val(),
                dataSrc: function (data) {
                    console.log(data.data)
                    return data.data;
                }
            },
            columns: [
                { data: 'cCuenta', 'autoWidth': true, class: 'text-center' },
                { data: 'cCCI', 'autoWidth': true, class: 'text-center' },
                { data: 'nombreBanco', 'autoWidth': true, class: 'text-center' },
                { data: 'nombreMoneda', 'autoWidth': true, class: 'text-center' },
                { data: 'cNombreDocumento', 'autoWidth': true, class: 'text-center' },
                { data: null, 'autoWidth': true, class: 'text-center' },
                //{
                //    data: 'cDescripcionPredeterminada', 'autoWidth': true, class: 'text-center', render: function (value) {
                //        return '<input type="radio" name="prede" data-id="'+ value +'">';
                //    }
                //},
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
                        var buttonDownload = ((data.cNombreDocumento == null || data.cNombreDocumento == '') ? `` : `<a href="javascript:;" class="btn btn-icon btn-sm btn-outline btn-outline-solid btn-outline-default me-2 p-des" data-kt-cuenta-table-filter="download_file" data-filename="` + data.cNombreDocumento + `" onclick="RegistroGirador.fnDownloadBancaria(` + data.nIdGiradorCuentaBancaria + `)" title="` + data.cNombreDocumento + `" data-id="` + data.nIdGiradorCuentaBancaria + `"><i class="las la-download fs-2"></i></a>`);
                        var buttonDelete = ((tableCuentaAction == 'Detalle') ? `` : `<a href="javascript:;" class="btn btn-icon btn-light-dark btn-sm p-eli" data-kt-cuenta-table-filter="delete_row" data-parent="` + $(idGirador).val() + `" data-path="` + data.cRuta + `" data-id="` + data.nIdGiradorCuentaBancaria + `"><i class="las la-trash fs-2"></i></a>`);
                        return buttonDownload + buttonDelete;
                    }
                },
                {
                targets: 5,
                data: null,
                visible: true,
                orderable: false,
                className: 'text-end',
                render: function (data, type, row) {
                    console.log(data)
                    var radio = `<input type="radio" ` + (data.cDescripcionPredeterminada == 'SI' ? `checked` : ``) + ` class="form-check-input radio-predeterminado" name="prede" value="` + data.cDescripcionPredeterminada + `" data-id="` + data.nIdGiradorCuentaBancaria + `" data-idGirador="` + data.nIdGiradorCuentaBancaria +`">`;
                    //var buttonDownload = ((data.cNombreDocumento == null || data.cNombreDocumento == '') ? `` : `<a href="javascript:;" class="btn btn-icon btn-sm btn-outline btn-outline-solid btn-outline-default me-2 p-des" data-kt-cuenta-table-filter="download_file" data-filename="` + data.cNombreDocumento + `" onclick="RegistroGirador.fnDownloadBancaria(` + data.nIdGiradorCuentaBancaria + `)" title="` + data.cNombreDocumento + `" data-id="` + data.nIdGiradorCuentaBancaria + `"><i class="las la-download fs-2"></i></a>`);
                    //var buttonDelete = ((tableCuentaAction == 'Detalle') ? `` : `<a href="javascript:;" class="btn btn-icon btn-light-dark btn-sm p-eli" data-kt-cuenta-table-filter="delete_row" data-parent="` + $(idGirador).val() + `" data-id="` + data.nIdGiradorCuentaBancaria + `"><i class="las la-trash fs-2"></i></a>`);
                    return radio;
                }
                }
            ]
        });
        datatableCuentaBancaria.on('draw', function () {
            handleDeleteCuentaBancariaForm();
            handleEditCuentaBancariaForm();
            Common.init();
        });
    }
    var handleEditCuentaBancariaForm = function () {

        $('.radio-predeterminado').on('change', function (e) {
            //console.log(e.target.value)
            //console.log(e.target.dataset.id)
 
            $(".radio-predeterminado").prop("disabled", true);
            var tokenVerification = $('input[name="__RequestVerificationToken"]').val();
            var formData = new FormData();
            formData.append('IdGiradorCabeceraCuenta', e.target.dataset.id);
            formData.append('IdGirador', $('#IdGirador').val());
            formData.append('bPredeterminado', true);
            console.log(formData)
            setTimeout(function () {
                $.ajax({
                    type: 'POST',
                    dataType: 'json',
                    url: 'Girador/EditarCuentaBancaria',
                    cache: false,
                    contentType: false,
                    processData: false,
                    headers: {
                        'RequestVerificationToken': tokenVerification
                    },
                    data: formData,
                    success: function (data) {
                        console.log(data)
                        if (data.succeeded) {
                            Swal.fire({
                                text: 'Cuenta bancaria predeterminada.',
                                icon: 'success',
                                buttonsStyling: false,
                                confirmButtonText: 'Listo',
                                customClass: {
                                    confirmButton: 'btn btn-primary'
                                }
                            }).then(function (result) {
                                if (result.isConfirmed) {
                                    $(".radio-predeterminado").prop("disabled", false);
                                    //$(window).attr('location', globalPath + 'Operacion/Registro?operacionId=' + data.data);
                                }
                            });
                           
                        } else {
                            $(".radio-predeterminado").prop("disabled", false);
                            messageError(data.message);
                            
                        }
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        $(".radio-predeterminado").prop("disabled", false);
                        messageError(errorThrown);
                    }
                });
            }, 2000);
        });
    }
    var handleAddCuentaBancariaForm = function () {
        var formAddCuentaBancaria = document.getElementById('kt_form_add_cuenta');
        if (!formAddCuentaBancaria) {
            return;
        }
        var addButton = document.getElementById('kt_add_cuenta');
        var validator;
        $('#IdGiradorCabeceraCuenta').val($('#IdGirador').val());
        validator = FormValidation.formValidation(
            formAddCuentaBancaria,
            {
                fields: {
                    'NroCuentaBancaria': {
                        validators: {
                            notEmpty: {
                                message: 'Cuenta Bancaria es obligatorio'
                            },
                            stringLength: {
                                min: 5,
                                max: 100,
                                message: 'Cuenta Bancaria debe tener entre 5 y 100 caracteres'
                            }
                        }
                    },
                    'CCI': {
                        validators: {
                            notEmpty: {
                                message: 'CCI es obligatorio'
                            },
                            stringLength: {
                                min: 5,
                                max: 100,
                                message: 'CCI debe tener entre 5 y 100 caracteres'
                            }
                        }
                    },
                    'Banco': {
                        validators: {
                            notEmpty: {
                                message: 'Banco es obligatorio'
                            }
                        }
                    },
                    'Moneda': {
                        validators: {
                            notEmpty: {
                                message: 'Moneda es obligatorio'
                            }
                        }
                    },
                    'DocConst': {
                        validators: {
                            notEmpty: {
                                message: 'Doc. Const. es obligatorio'
                            },
                            file: {
                                extension: 'pdf',
                                type: 'application/pdf',
                                message: 'Debe adjuntar un archivo PDF',
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
                        formData.append('IdGiradorCabeceraCuenta', $('#IdGiradorCabeceraCuenta').val());
                        formData.append('NroCuentaBancaria', $('#NroCuentaBancaria').val());
                        formData.append('CCI', $('#CCI').val());
                        formData.append('Banco', $('#Banco').val());
                        formData.append('Moneda', $('#Moneda').val());
                        formData.append('docConst', fileDocConst.files[0]);

                        formData.append('nValorSeleccionado', $('#nValorSeleccionado').val());
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
                                            $("input[type=checkbox]").prop("checked", false);
                                            initDataTableCuentaBancaria();
                                        }
                                    });
                                } else {
                                    addButton.removeAttribute('data-kt-indicator');
                                    addButton.disabled = false;
                                    $("input[type=checkbox]").prop("checked", false);
                                    messageError(data.message);
                                }
                            },
                            error: function (jqXHR, textStatus, errorThrown) {
                                addButton.removeAttribute('data-kt-indicator');
                                $("input[type=checkbox]").prop("checked", false);
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
    var handleDownloadCuentaBancaria = function () {
     
        var tableGiradorCuenta = document.querySelector('#kt_cuenta_bancaria_table');
        if (!tableGiradorCuenta) {
            return;
        }
        var downloadCuentaButton = tableGiradorCuenta.querySelectorAll('[data-kt-cuenta-table-filter="download_file"]');
        downloadCuentaButton.forEach(d => {
            d.addEventListener('click', function (e) {
                e.preventDefault();
             
                //var filename = $(this).data('filename');
                //var filename = '\\pafactoring\\Girador\\CuentasBancarias\\' + filename;
                var idDocumento = $(this).data('id');
                RegistroGirador.getDownloadFile(idDocumento, 2);
            });
        });
    }
    var handleDeleteCuentaBancariaForm = function () {
        var tableGiradorCuenta = document.querySelector('#kt_cuenta_bancaria_table');
        if (!tableGiradorCuenta) {
            return;
        }
        var deleteCuentaButton = tableGiradorCuenta.querySelectorAll('[data-kt-cuenta-table-filter="delete_row"]');
        deleteCuentaButton.forEach(d => {
            d.addEventListener('click', function (e) {
                e.preventDefault();
                var idGirador = $(this).data('parent');
                var idCuenta = $(this).data('id');
                var ruta = $(this).data('path');
                var parent = e.target.closest('tr');
                var nameCuenta = parent.querySelectorAll('td')[0].innerText + ' del Banco ' + parent.querySelectorAll('td')[2].innerText;
                Swal.fire({
                    text: '¿Estás seguro de que quieres eliminar la cuenta ' + nameCuenta + '?',
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
                            url: globalPath + 'Girador/EliminarCuentaBancaria',
                            data: {
                                giradorCuentaId: idCuenta,
                                filePath: ruta
                            },
                            headers: {
                                'RequestVerificationToken': token
                            },
                            success: function (data) {
                                if (data) {
                                    Swal.fire({
                                        text: 'Eliminaste correctamente la cuenta ' + nameCuenta + '.',
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
                                    messageError('La cuenta ' + nameCuenta + ' no fue eliminado.');
                                }
                            },
                            error: function (jqXHR, textStatus, errorThrown) {
                                messageError(errorThrown);
                            }
                        });
                    } else if (result.dismiss === 'cancel') {
                        messageError('La cuenta ' + nameCuenta + ' no fue eliminado.');
                    }
                });
            });
        });
    }
    var initDataTableUbicaciones = function () {
        var idGirador = document.getElementById('IdGirador');
        if (!idGirador) {
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
                url: globalPath + 'Girador/GetAllUbicaciones?giradorId=' + $(idGirador).val(),
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
                        return `<a href="javascript:;" class="btn btn-icon btn-light-dark btn-sm p-eli" data-kt-ubicacion-table-filter="delete_row" data-parent="` + $(idGirador).val() + `" data-id="` + data.nIdGiradorDireccion + `"><i class="las la-trash fs-2"></i></a>`;
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
        $('#IdGiradorCabeceraUbicacion').val($('#IdGirador').val());
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
                            url: globalPath + 'Girador/EliminarUbicacion',
                            data: {
                                giradorUbicacionId: idUbicacion
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
        $.getJSON(globalPath + 'Girador/ListarUbigeos', {pais: pais, tipo: _tipo, codigo: _codigo}).done(function (data) {
            if (data.length > 0) {
                $.each(data, function (key, value) {
                    $('#' + _tipo).append($('<option value="' + value.cCodigo + '">' + value.cDescripcion + '</option>'));
                });
            }
        });
    }
    var initDataTableDocumentos = function () {
        var idGirador = document.getElementById('IdGirador');
        if (!idGirador) {
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
                url: globalPath + 'Girador/GetAllDocumentos?giradorId=' + $(idGirador).val(),
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
                        var buttonDownload = ((data.cNombreDocumento == null || data.cNombreDocumento == '') ? `` : `<a href="javascript:;" class="btn btn-icon btn-sm btn-outline btn-outline-solid btn-outline-default me-2 p-des" data-kt-documento-table-filter="download_file" data-filename="` + data.cNombreDocumento + `" onclick="RegistroGirador.fnDownloadDocumentos(` + data.nIdGiradorDocumento + `)" title="` + data.cNombreDocumento + `" data-id="` + data.nIdGiradorDocumento + `"><i class="las la-download fs-2"></i></a>`);
                        var buttonDelete = ((tableDocumentosAction == 'Detalle') ? `` : `<a href="javascript:;" class="btn btn-icon btn-light-dark btn-sm p-eli" data-kt-documento-table-filter="delete_row" data-parent="` + $(idGirador).val() + `" data-path="` + data.cRuta + `" data-id="` + data.nIdGiradorDocumento + `"><i class="las la-trash fs-2"></i></a>`);
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
        $('#IdGiradorCabeceraDocumentos').val($('#IdGirador').val());
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
                        formData.append('IdGiradorCabeceraDocumentos', $('#IdGiradorCabeceraDocumentos').val());
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
    var handleDownloadDocumentos = function () {
        var tableGiradorDocumentos = document.querySelector('#kt_documentos_table');
        if (!tableGiradorDocumentos) {
            return;
        }
        var downloadDocumentoButton = tableGiradorDocumentos.querySelectorAll('[data-kt-documento-table-filter="download_file"]');
        downloadDocumentoButton.forEach(d => {
            d.addEventListener('click', function (e) {
                e.preventDefault();
                //var filename = $(this).data('filename');
                //var filename = filename;
                var idDocumento = $(this).data('id');
                RegistroGirador.getDownloadFile(idDocumento,3);
            });
        });
    }
    var handleDeleteDocumentosForm = function () {
        var tableGiradorDocumentos = document.querySelector('#kt_documentos_table');
        if (!tableGiradorDocumentos) {
            return;
        }
        var deleteDocumentosButton = tableGiradorDocumentos.querySelectorAll('[data-kt-documento-table-filter="delete_row"]');
        deleteDocumentosButton.forEach(d => {
            d.addEventListener('click', function (e) {
                e.preventDefault();
                var idGirador = $(this).data('parent');
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
                            url: globalPath + 'Girador/EliminarDocumentos',
                            data: {
                                giradorDocumentoId: idDocumento,
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
    var initDataTableLinea = function () {
        var idGirador = document.getElementById('IdGirador');
        if (!idGirador) {
            return;
        }
        var tableLinea = document.getElementById('kt_linea_table');
        if (!tableLinea) {
            return;
        }
        var tableLineaAction = $('#Action').val();
        $(tableLinea).DataTable({ ordering: false }).clear().destroy();
        $.fn.dataTable.ext.errMode = 'none';
        datatableLinea = $(tableLinea).DataTable({
            ordering: false,
            ajax: {
                type: 'GET',
                dataType: 'json',
                url: globalPath + 'Girador/GetAllLinea?giradorId=' + $(idGirador).val(),
                dataSrc: function (data) {
                    return data.data;
                }
            },
            columns: [
                { data: 'inversionista', 'autoWidth': true, class: 'text-left' },
                { data: 'tipoMoneda', 'autoWidth': true, class: 'text-left' },
                { data: 'lineaMeta', 'autoWidth': true, class: 'text-end' },
                { data: 'lineaDisponible', 'autoWidth': true, class: 'text-end' },
                { data: null, 'autoWidth': true, class: 'text-center', responsivePriority: -1 }
            ],
            columnDefs: [

                {
                    targets: -1,
                    data: null,
                    visible: (tableLineaAction == 'Detalle') ? false : true,
                    orderable: false,
                    className: 'text-end',
                    render: function (data, type, row) {
                        var buttonDelete = ((tableLineaAction == 'Detalle') ? `` : `<a href="javascript:;" class="btn btn-icon btn-light-dark btn-sm p-eli" data-kt-linea-table-filter="delete_row" data-parent="` + $(idGirador).val() + `" data-id="` + data.nIdGiradorLinea + `"><i class="las la-trash fs-2"></i></a>`);
                        return buttonDelete;
                    }
                }
            ]
        });
        datatableLinea.on('draw', function () {
            handleDeleteLineaForm();
            Common.init();
        });
    }
    var handleAddLineaForm = function () {
        var formAddLinea = document.getElementById('kt_form_add_linea');
        if (!formAddLinea) {
            return;
        }
        var addButton = document.getElementById('kt_add_linea');
        var validator;
        $('#IdGiradorCabeceraLinea').val($('#IdGirador').val());
        validator = FormValidation.formValidation(
            formAddLinea,
            {
                fields: {
                    'Fondeador': {
                        validators: {
                            notEmpty: {
                                message: 'Fondeador es obligatorio'
                            }
                        }
                    },
                    'LineaMeta': {
                        validators: {
                            notEmpty: {
                                message: 'Línea Meta es obligatorio'
                            }
                        }
                    },
                    'Moneda': {
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
        addButton.addEventListener('click', function (e) {
            e.preventDefault();
            validator.validate().then(function (status) {
                if (status == 'Valid') {
                    addButton.setAttribute('data-kt-indicator', 'on');
                    addButton.disabled = true;
                    setTimeout(function () {
                        var tokenVerification = $('input[name="__RequestVerificationToken"]').val();
                        var formData = new FormData();
                        formData.append('IdGirador', $('#IdGiradorCabeceraLinea').val());
                        formData.append('IdInversionista', $('#Fondeador').val());
                        formData.append('IdTipoMoneda', $('#MonedaLinea').val());
                        formData.append('LineaMeta', $('#LineaMeta').val());
                        $.ajax({
                            type: 'POST',
                            dataType: 'json',
                            url: $(formAddLinea).attr('action'),
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
                                            initDataTableLinea();
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
    var handleDeleteLineaForm = function () {
        var tableGiradorLinea = document.querySelector('#kt_linea_table');
        if (!tableGiradorLinea) {
            return;
        }
        var deleteLineaButton = tableGiradorLinea.querySelectorAll('[data-kt-linea-table-filter="delete_row"]');
        deleteLineaButton.forEach(d => {
            d.addEventListener('click', function (e) {
                e.preventDefault();
                var idGirador = $(this).data('parent');
                var idLinea = $(this).data('id');
                var parent = e.target.closest('tr');
                Swal.fire({
                    text: '¿Estás seguro de que quieres eliminar el registro?',
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
                            url: globalPath + 'Girador/EliminarLinea',
                            data: {
                                giradorLineaId: idLinea
                            },
                            headers: {
                                'RequestVerificationToken': token
                            },
                            success: function (data) {
                                if (data) {
                                    Swal.fire({
                                        text: 'Eliminaste correctamente el registro',
                                        icon: 'success',
                                        buttonsStyling: false,
                                        confirmButtonText: 'Listo',
                                        customClass: {
                                            confirmButton: 'btn fw-bold btn-primary',
                                        }
                                    }).then(function () {
                                        datatableLinea.row($(parent)).remove().draw();
                                    });
                                } else {
                                    messageError('El registro no fue eliminado.');
                                }
                            },
                            error: function (jqXHR, textStatus, errorThrown) {
                                messageError(errorThrown);
                            }
                        });
                    } else if (result.dismiss === 'cancel') {
                        messageError('El registro no fue eliminado.');
                    }
                });
            });
        });
    }
    var handleDownloadFile = function (idDocumento, tipoDocumento) {
        window.open(globalPath + 'Girador/DownloadFile?idDocumento=' + encodeURIComponent(idDocumento) + '&tipoDocumento=' + encodeURIComponent(tipoDocumento), '_blank');
    }
    var handleLevantarObservacion = function () {
        $('.comment').click(function (e) {
            e.preventDefault;
            var stateFlag = $(this).data('flag');
            var stateSelected = $(this).data('state');
            var giradorId = $('#IdGirador').val();
            var htmlContent = '';
            $('#kt_modal_levantar_observacion').find('.modal-body').html('');
            if (stateFlag == '1') {
                var form = $('#kt_form_registro');
                var token = $('input[name="__RequestVerificationToken"]', form).val();
                Swal.fire({
                    text: '¿Está seguro de que desea enviar a Evaluación Legal el Registro?',
                    icon: 'warning',
                    showCancelButton: true,
                    buttonsStyling: false,
                    showLoaderOnConfirm: true,
                    confirmButtonText: 'Enviar',
                    cancelButtonText: 'Cancelar',
                    customClass: {
                        confirmButton: 'btn fw-bold btn-danger',
                        cancelButton: 'btn fw-bold btn-active-light-primary'
                    },
                }).then(function (result) {
                    if (result.value) {
                        $.ajax({
                            type: 'POST',
                            dataType: 'json',
                            url: globalPath + 'Girador/EvaluacionLegal',
                            xhrFields: {
                                withCredentials: true
                            },
                            data: {
                                __RequestVerificationToken: token,
                                giradorId: giradorId,
                                estadoGirador: stateSelected
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
                                            $(window).attr('location', globalPath + 'Girador');
                                        }
                                    });
                                } else {
                                    messageError(data.message);
                                }
                            },
                            error: function (jqXHR, textStatus, errorThrown) {
                                messageError(errorThrown);
                            }
                        });
                    }
                });
            } else if (stateFlag == '4') {
                htmlContent = `
                <div class="flex-row-fluid">
                    <div class="w-100">
                        <div class="fv-row">
                            <input type="hidden" id="IdGirador" name="IdGirador" value="` + giradorId + `" />
                            <input type="hidden" id="EstadoGirador" name="EstadoGirador" value="` + stateSelected + `" />
                            <textarea id="ComentarioGirador" name="ComentarioGirador" required="required" class="form-control form-control-solid" rows="5" maxlength="3000" data-fv-not-empty___message="The username is required" placeholder="Ingresar Comentario..."></textarea>
                        </div>
                    </div>
                </div>`;
                $('#kt_modal_levantar_observacion').find('.modal-body').append(htmlContent);
                var modal = bootstrap.Modal.getOrCreateInstance('#kt_modal_levantar_observacion');
                modal.show();
            }
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
            if ($('#ComentarioGirador').val() == '') {
                messageError('Comentario Girador es Obligatorio');
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
                                    $(window).attr('location', globalPath + 'Girador');
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
    var handleComentarios = function () {
        var idGirador = $('#IdGirador').val();
        if (typeof idGirador != 'undefined' || idGirador != null) {
            var commentHtml = ``;
            $('#kt_help_scroll').html('');
            $.get(globalPath + 'Girador/GetAllComentariosGirador', { giradorId: idGirador }).done(function (data) {
                if (data.succeeded) {
                    if (data.data.length > 0) {
                        $.each(data.data, function (key, value) {
                            commentHtml += `<div class="justify-content-start mb-5">
                                <div class="d-flex flex-column align-items-start">
                                    <div class="d-flex align-items-center mb-2">
                                        <div class="ms-3">
                                            <a href="javascript:;" class="fs-5 fw-bolder text-gray-900 text-hover-primary me-1">` + value.cUsuarioCreador + `</a>
                                            <span class="text-muted fs-7 mb-1">(` + value.dFechaCreacion + `)</span>
                                        </div>
                                    </div>
                                    <div class="p-5 rounded bg-light-info text-dark fw-bold text-start w-100" data-kt-element="message-text">
                                        <strong>Nro. Operación: </strong> ` + value.nNroOperacion  + ` <br>
                                        ` + value.cComentario + `
                                    </div>
                                </div>
                            </div>`;
                        });
                    } else {
                        commentHtml += `<div class="d-flex align-items-center rounded py-5 px-5 bg-light-warning">
                            <i class="fas fa-info-circle fs-2x text-primary me-2"></i>
                            <div class="text-gray-700 fw-bold fs-6">No se encontraron comentarios para el girador</div>
                        </div>`;
                    }
                    $('#kt_help_scroll').html(commentHtml);
                }
            });
        }
    }

    /*Categoria*/
    var initDataTableCategoria = function () {
        var idGirador = document.getElementById('IdGirador');
        if (!idGirador) {
            return;
        }
        var table = document.getElementById('kt_giradores_categoria_table');
        if (!table) {
            return;
        }

        var tableAction = $('#Action').val();
        $(table).DataTable({ ordering: false }).clear().destroy();
        $.fn.dataTable.ext.errMode = 'none';
        datatableCategoriaGirador = $(table).DataTable({
            ordering: false,
            ajax: {
                type: 'GET',
                dataType: 'json',
                url: globalPath + 'Girador/GetAllCategorias?giradorId=' + $(idGirador).val(),
                dataSrc: function (data) {
                    return data.data;
                }
            },
            columns: [
                { data: 'categoria', 'autoWidth': true, class: 'text-center' },
                { data: 'aceptante', 'autoWidth': true, class: 'text-center' },
                { data: 'dFechaCreacion', 'autoWidth': true, class: 'text-center', render: function (value) { return moment(value).format('DD/MM/YYYY'); } },
                { data: 'usuario', 'autoWidth': true, class: 'text-center' },
                { data: null, 'autoWidth': true, class: 'text-center', responsivePriority: -1 }
            ],
            columnDefs: [
                {
                    targets: -1,
                    data: null,
                    visible: (tableAction == 'Detalle') ? false : true,
                    orderable: false,
                    className: 'text-end',
                    render: function (data, type, row) {
                        return `<a href="javascript:;" class="btn btn-icon btn-light-dark btn-sm p-eli" data-kt-categoria-table-filter="delete_row" data-parent="` + $(idGirador).val() + `" data-id="` + data.nIdGiradorConfirming + `"><i class="las la-trash fs-2"></i></a>`;
                    }
                }
            ]
        });
        datatableCategoriaGirador.on('draw', function () {
            handleDeleteCategoriaForm();
            Common.init();
        });
    }

    var handleDeleteCategoriaForm = function () {
        var table = document.querySelector('#kt_giradores_categoria_table');
        if (!table) {
            return;
        }
        var deleteButton = table.querySelectorAll('[data-kt-categoria-table-filter="delete_row"]');
        deleteButton.forEach(d => {
            d.addEventListener('click', function (e) {
                e.preventDefault();
                var idGirador = $(this).data('parent');
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
                            url: globalPath + 'Girador/EliminarContacto',
                            data: {
                                giradorContactoId: idContact
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
                                        datatableContactoGirador.row($(parent)).remove().draw();
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

    var handleAddCategoriaForm = function () {
        var formAdd = document.getElementById('kt_form_add_categoria');
        if (!formAdd) {
            return;
        }
        var addButton = document.getElementById('kt_add_categoria');
        var validator;
        $('#IdGiradorCabeceraCategoria').val($('#IdGirador').val());


        $('#Categoria').on('change', function (e) {

            (parseInt($('#Categoria').val()) == 3 || parseInt($('#Categoria').val()) == 4) ? $('#divAceptante').show() : $('#divAceptante').hide(); $("#Aceptante").val('').trigger('change');
            /*
            var _categoria = $('#Categoria').val();

            if (parseInt(_categoria) == 3 || parseInt(_categoria) == 4) { //CONFIRMING ORIGINACION : 3, CONFIRMING TRADICIONAL : 4
                $('#divAceptante').show()
            }
            */

            //var idGirador = this.options[this.selectedIndex].value;
            //$('#IdGiradorDireccion').empty().append('<option value="">Seleccionar</option>');
            //$.getJSON(globalPath + 'Operacion/GetListDireccionByGirador', { idGirador: idGirador }).done(function (data) {
            //    if (data.length > 0) {
            //        $.each(data, function (key, value) {
            //            $('#IdGiradorDireccion').append($('<option value="' + value.nIdGiradorDireccion + '">' + value.cDireccion + '</option>'));
            //        });
            //    }
            //});
        });


        validator = FormValidation.formValidation(
            formAdd,
            {
                fields: {
                    'Categoria': {
                        validators: {
                            notEmpty: {
                                message: 'Categoria es obligatorio'
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

                    if (parseInt($('#Categoria').val()) == 3 || parseInt($('#Categoria').val()) == 4) {
                        if ($('#Aceptante').val() == '') {
                            messageError('Aceptante es obligatorio');
                            return;
                        }
                    }

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
                                            initDataTableCategoria();
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

    var handleDeleteCategoriaForm = function () {
        var table= document.querySelector('#kt_giradores_categoria_table');
        if (!table) {
            return;
        }
        var deleteContactButton = table.querySelectorAll('[data-kt-categoria-table-filter="delete_row"]');
        deleteContactButton.forEach(d => {
            d.addEventListener('click', function (e) {
                e.preventDefault();
                var idGirador = $(this).data('parent');
                var id = $(this).data('id');
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
                            url: globalPath + 'Girador/EliminarCategoria',
                            data: {
                                giradorCategoriaId: id
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
                                        datatableCategoriaGirador.row($(parent)).remove().draw();
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


    /*Categoria*/

    return {
        init: function () {
            handleFilterTable();
            initDatatable();
            handleRegisterForm();
            initDataTableContacto();
            handleAddContactoForm();
            handleDeleteContactoForm();
            //initDataTableRepresentanteLegal();
            //handleAddRepresentanteLegalForm();
            //handleDeleteRepresentanteLegalForm();
            //initDataTableCuentaBancaria();
            //handleEditCuentaBancariaForm();
            //handleAddCuentaBancariaForm();
            //handleDeleteCuentaBancariaForm();
            //initDataTableUbicaciones();
            //handleAddUbicacionForm();
            //handleDeleteUbicacionForm();
            initDataTableDocumentos();
            handleAddDocumentosForm();
            handleDeleteDocumentosForm();
            //initDataTableLinea();
            //handleAddLineaForm();
            //handleLevantarObservacion();
            //handleComentarios();

            //initDataTableCategoria();
            //handleAddCategoriaForm();
            //handleDeleteCategoriaForm();
        },
        getRevalidateFormElement: function (form, elem, val) {
            handleRevalidateFormElement(form, elem, val);
        },
        //getUbigeo: function (pais, tipo, codigo) {
        //    handleUbigeo(pais, tipo, codigo);
        //},
        //getDownloadFile: function (idDocumento, tipoDocumento) {
        //    handleDownloadFile(idDocumento, tipoDocumento);
        //},
        //fnDownloadBancaria: function (idDocumento) {
        //    handleDownloadFile(idDocumento, 2);
        //},
        //fnDownloadLegal: function (idDocumento) {
        //    handleDownloadFile(idDocumento, 1);
        //},
        fnDownloadDocumentos: function (idDocumento) {
            handleDownloadFile(idDocumento, 3);
        },
        
    }
}();
KTUtil.onDOMContentLoaded(function () {
    RegistroGirador.init();
});